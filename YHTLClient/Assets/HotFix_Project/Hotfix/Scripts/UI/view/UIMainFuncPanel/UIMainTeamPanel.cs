using System.Collections;
using System.Collections.Generic;
using team;
using UnityEngine;

/// <summary>
/// 主界面上和任务栏一起的组队栏
/// </summary>
public class UIMainTeamPanel : UIBase
{
    public override bool ShowGaussianBlur => false;
    
    public override UILayerType PanelLayerType => UILayerType.Resident;

    UIGridContainer grid_team_players;
    private int selectIndex = -1;
    
    ILBetterList<team.TeamMember> listTeamMember = new ILBetterList<TeamMember>();

    protected override void _InitScriptBinder()
    {
        grid_team_players = ScriptBinder.GetObject("grid_team_players") as UIGridContainer;
    }

    public override void Init()
    {
        base.Init();
        //mClientEvent.Reg((uint)CEvent.GetMyTeamInfo, GetMyTeamInfo);
        mClientEvent.Reg((uint) CEvent.AddTeamForMe, AddTeamForMe);
        mClientEvent.Reg((uint) CEvent.AddTeamOther, AddTeamOther);
        mClientEvent.Reg((uint) CEvent.QuitTeam, QuitTeam);
        mClientEvent.Reg((uint) CEvent.ChangeLeader, ChangeLeader);
        mClientEvent.Reg((uint) CEvent.UpdatePlayerHpMpInfoMessage, UpdatePlayerHpMpInfoMessage);
        mClientEvent.Reg((uint) CEvent.UpdatePlayerLevelInfoMessage, UpdatePlayerLevelInfoMessage);
        mClientEvent.Reg((uint) CEvent.ResetMainTeamSelect, ResetSelect);
        mClientEvent.Reg((uint) CEvent.SelectMyTeamPlayer, SelectMyTeamPlayer);
        mClientEvent.Reg((uint) CEvent.NoSelectLastMyTeamPlayer, NoSelectLastMyTeamPlayer);
    }

    public override void Show()
    {
        base.Show();
        selectIndex = -1;
        InitData();
    }
    
    /// <summary>
    /// 上一个选中的人现在处于未选中状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void NoSelectLastMyTeamPlayer(uint id, object data)
    {
        if (data == null) return;
        long roleId = (long) data;
        for (int i = 0, max = listTeamMember.Count; i < max; i++)
        {
            team.TeamMember member = listTeamMember[i];
            if (member.roleId == roleId)
            {
                selectIndex = -1;
                break;
            }
        }
        InitData();
    }
    
    /// <summary>
    /// 点击队伍里的人
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void SelectMyTeamPlayer(uint id, object data)
    {
        if (data == null) return;
        selectIndex = (int) data;
        InitData();
    }

    /// <summary>
    /// 更新玩家等级信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void UpdatePlayerLevelInfoMessage(uint id, object data)
    {
        InitData();
    }

    /// <summary>
    /// 更新玩家血量信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void UpdatePlayerHpMpInfoMessage(uint id, object data)
    {
        InitData();
    }

    void ResetSelect(uint id, object data)
    {
        selectIndex = -1;
        InitData();
    }

    void AddTeamForMe(uint id, object data)
    {
        selectIndex = -1;
        InitData();
    }

    void AddTeamOther(uint id, object data)
    {
        InitData();
    }

    void QuitTeam(uint id, object data)
    {
        selectIndex = -1;
        InitData();
    }

    //弃用(走AddTeamForMe)
    void ChangeLeader(uint id, object data)
    {
        selectIndex = -1;
        InitData();
    }

    void InitData()
    {
        GameObject gp;
        if (CSTeamInfo.Instance.TeamId == 0) //没有队伍
        {
            grid_team_players.MaxCount = 1;
            gp = grid_team_players.controlList[0];
            var eventHandle = UIEventListener.Get(gp);
            UIMainTeamBinder Binder;
            if (eventHandle.parameter == null)
            {
                Binder = new UIMainTeamBinder();
                Binder.Setup(eventHandle);
            }
            else
            {
                Binder = eventHandle.parameter as UIMainTeamBinder;
            }

            Binder.isInvitationTeam = true;
            Binder.isLeader = false;
            Binder.isSelect = false;
            Binder.index = 0;
            Binder.selectIndex = selectIndex;
            Binder.actionItem = null;
            Binder.Bind(null);
        }
        else
        {
            if (CSTeamInfo.Instance.MyTeamData == null) return;
            listTeamMember = CSTeamInfo.Instance.SortDicTeamShowOrder(CSTeamInfo.Instance.MyTeamData.teamInfo);
            if (listTeamMember.Count == CSTeamInfo.NumFullStarffed) //队伍满员
            {
                grid_team_players.MaxCount = CSTeamInfo.NumFullStarffed;
                for (int i = 0; i < grid_team_players.MaxCount; i++)
                {
                    gp = grid_team_players.controlList[i];
                    var eventHandle = UIEventListener.Get(gp);
                    UIMainTeamBinder Binder;
                    if (eventHandle.parameter == null)
                    {
                        Binder = new UIMainTeamBinder();
                        Binder.Setup(eventHandle);
                    }
                    else
                    {
                        Binder = eventHandle.parameter as UIMainTeamBinder;
                    }

                    Binder.isInvitationTeam = false;
                    Binder.isLeader = i == 0;
                    Binder.isSelect = i == selectIndex;
                    Binder.index = i;
                    Binder.selectIndex = selectIndex;
                    Binder.actionItem = OnClickItem;
                    Binder.Bind(listTeamMember[i]);
                }
            }
            else
            {
                grid_team_players.MaxCount = listTeamMember.Count + 1;
                for (int i = 0; i < grid_team_players.MaxCount; i++)
                {
                    gp = grid_team_players.controlList[i];
                    //如果是最后一个(+号)
                    if (i == grid_team_players.MaxCount - 1)
                    {
                        var eventHandle = UIEventListener.Get(gp);
                        UIMainTeamBinder Binder;
                        if (eventHandle.parameter == null)
                        {
                            Binder = new UIMainTeamBinder();
                            Binder.Setup(eventHandle);
                        }
                        else
                        {
                            Binder = eventHandle.parameter as UIMainTeamBinder;
                        }

                        Binder.isInvitationTeam = true;
                        Binder.isLeader = false;
                        Binder.isSelect = false;
                        Binder.actionItem = null;
                        Binder.index = i;
                        Binder.selectIndex = selectIndex;
                        Binder.Bind(null);
                    }
                    else
                    {
                        var eventHandle = UIEventListener.Get(gp);
                        UIMainTeamBinder Binder;
                        if (eventHandle.parameter == null)
                        {
                            Binder = new UIMainTeamBinder();
                            Binder.Setup(eventHandle);
                        }
                        else
                        {
                            Binder = eventHandle.parameter as UIMainTeamBinder;
                        }

                        Binder.isInvitationTeam = false;
                        Binder.isLeader = i == 0;
                        Binder.isSelect = i == selectIndex;
                        Binder.index = i;
                        Binder.selectIndex = selectIndex;
                        Binder.actionItem = OnClickItem;
                        Binder.Bind(listTeamMember[i]);
                    }
                }
            }
        }
    }

    void OnClickItem(int index)
    {
        // if (index == selectIndex) return;
        selectIndex = index;
        InitData();
    }

    protected override void OnDestroy()
    {
        grid_team_players.UnBind<UIMainTeamBinder>();
        base.OnDestroy();
    }
}