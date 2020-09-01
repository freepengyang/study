using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class UISelectionMenuPanel : UIBase
{
    public override bool ShowGaussianBlur { get => false; }
    
    public Action<GameObject> closeSelectionPanel;

    /// <summary>
    /// 筛选出来的按钮
    /// </summary>
    List<MenuType> menus = new List<MenuType>();

    private MenuInfo menuInfo = null;

    public override void Init()
    {
        base.Init();
    }
    
    public override void Show()
    {
        base.Show();
    }

    public void ShowBtnsData(MenuInfo info, UISprite spHead=null)
    {
        if (info == null) return;
        menuInfo = info;
        CSSelectionManger.Instance.ScreenButton(info, menus);
        InitData(spHead);
    }

    void InitData(UISprite spHead=null)
    {
        mgrid_btns.MaxCount = menus.Count;
        GameObject gp = null;
        TABLE.BUTTON myButton;
        for (int i = 0; i < menus.Count; i++)
        {
            gp = mgrid_btns.controlList[i];
            myButton = ButtonTableManager.Instance[(int)menus[i]];
            gp.transform.Find("lb_btn_item").GetComponent<UILabel>().text = myButton.buttonName;
            UIEventListener.Get(gp, menus[i]).onClick = OnClickItem;
        }
        UIBtnsMenuCulation.Instance.SetUIBtnsAdaption(msp_bg, mgrid_btns, spHead);
    }

    public void SetUIBtnsAdaption(UISprite spHead = null)
    {
        
    }

    void OnClickItem(GameObject go)
    {
        MenuType type = (MenuType)UIEventListener.Get(go).parameter;
        if (null == menuInfo) return;
        switch (type)
        {
            case MenuType.TYPE_CHECK_INFO:
                OnClickCheckInfo(menuInfo.roleId);
                break;
            case MenuType.TYPE_CHAT:
                OnClickPrivateChat(menuInfo.roleId, menuInfo.roleName,menuInfo.career,menuInfo.lv,menuInfo.sex);
                break;
            case MenuType.TYPE_ADD_FRIEND:
                OnClickAddFriend(menuInfo.roleId);
                break;
            case MenuType.TYPE_DELETE_FRIEND:
                OnClickDeleteFriend(menuInfo.roleId, menuInfo.roleName);
                break;
            case MenuType.TYPE_APPLY_TEAM:
                OnClickApplyTeam(menuInfo.roleId, menuInfo.teamId, TeamTab.QuickJoinTeam);
                break;
            case MenuType.TYPE_INVITE_TEAM:
                ReqInviteTeamMessage(menuInfo.roleId, TeamTab.QuickJoinTeam);
                break;
            case MenuType.TYPE_KICK_TEAM:
                ReqLeaveTeamMessage(menuInfo.roleId);
                break;
            case MenuType.TYPE_ASSIGN_CAPTAIN:
                ReqChangeTeamLeaderMessage(menuInfo.roleId);
                break;
            case MenuType.TYPE_QUIT_TEAM:
                ReqLeaveTeamMessage(menuInfo.roleId);
                break;
            case MenuType.TYPE_DISSOLVE_TEAM:
                break;
            case MenuType.TYPE_PULL_BLACKLIST:
                OnClickPullBlackList(menuInfo.roleId);
                break;
            case MenuType.TYPE_CANCEL_BLACKLIST:
                OnClickCancelBlakList(menuInfo.roleId);
                break;
            case MenuType.TYPE_ADD_ENEMYP:
                OnClickAddEnemy(menuInfo.roleId);
                break;
            case MenuType.TYPE_DELETE_ENEMY:
                OnClickDeleteEnemy(menuInfo.roleId);
                break;
            case MenuType.TYPE_INVITE_GUILD:
                CSGuildInfo.Instance.InvitePlayerJoinGuild(menuInfo.roleId,menuInfo.roleName);
                this.Close();
                break;
            case MenuType.TYPE_KICK_GUILD:
                CSGuildInfo.Instance.KickGuildPlayer(menuInfo.contribute, menuInfo.roleId, menuInfo.roleName);
                this.Close();
                break;
            case MenuType.TYPE_APPLY_GIVESPEAKLIMITS:
                Net.ReqUpdateSpeakLimitsMessage(menuInfo.roleId.ToGoogleList(), true);
                //UtilityTips.ShowGreenTips(1831);
                this.Close();
                break;
            case MenuType.TYPE_CANCEL_GIVESPEAKLIMITS:
                Net.ReqUpdateSpeakLimitsMessage(menuInfo.roleId.ToGoogleList(), false);
                //UtilityTips.ShowGreenTips(1832);
                this.Close();
                break;
            case MenuType.TYPE_JUBAO:
                break;
            case MenuType.TYPE_COPY_NAME:
                break;
            case MenuType.TYPE_ADJUST_GUILD_POS:
                UIManager.Instance.CreatePanel<UIGuildPositionPanel>(f =>
                {
                    (f as UIGuildPositionPanel).Show(menuInfo);
                });
                this.Close();
                break;
            default:
                break;
        }
    }

    void OnClickApplyTeam(long roleId, long teamId, TeamTab teamTab)
    {
        if (teamId<=0)
        {
            //申请建队并邀请
            Net.ReqInviteTeamMessage(roleId, teamTab);
        }
        else
        {
            //申请入队
            Net.ReqApplyTeamMessage(teamId);
        }
        Close();
    }

    void OnClickCheckInfo(long roleId)
    {
        //请求查看信息
        Net.ReqGetOtherPlayerInfoMessage(roleId);
        Close();
    }

    void ReqInviteTeamMessage(long roleId, TeamTab teamTab)
    {
        Net.ReqInviteTeamMessage(roleId, teamTab);
        Close();
    }

    void ReqLeaveTeamMessage(long roleId)
    {
        Net.ReqLeaveTeamMessage(roleId);
        Close();
    }

    void ReqChangeTeamLeaderMessage(long roleId)
    {
        Net.ReqChangeTeamLeaderMessage(roleId);
        Close();
    }

    public void OnClickPrivateChat(long roleId, string roleName, int career, int level,int sex)
    {
        CSFriendInfo.Instance.AddPrivateChat(roleId, roleName, career, level, sex,true,false);
        UIManager.Instance.CreatePanel<UIRelationCombinedPanel>(f =>
        {
            UIRelationCombinedPanel panel = f as UIRelationCombinedPanel;
            panel.OpenChildPanel((int)UIRelationCombinedPanel.ChildPanelType.CPT_FRIEND)?.RefreshData(4, roleId);
        });
        HotManager.Instance.EventHandler.SendEvent(CEvent.HideChatPanel);

        Close();
    }

    public void OnClickAddFriend(long roleId)
    {
        Net.ReqAddRelationMessage(roleId.ToGoogleList(), (int)FriendType.FT_FRIEND);
    }

    public void OnClickDeleteFriend(long roleId, string roleName)
    {
        UtilityTips.ShowPromptWordTips(9,() => {
            DeleteReleation(roleId);
            Close();
        }, roleName);
    }

    public void OnClickDeleteEnemy(long roleId)
    {
        DeleteReleation(roleId);
        Close();
    }

    private void DeleteReleation(long roleId)
    {
        Net.ReqDeleteRelationMessage(roleId.ToGoogleList());
        //if (UIManager.Instance.IsPanel<UIFriendPanel>())
        //{
        //    UIManager.Instance.GetPanel<UIFriendPanel>().Init();
        //}
    }

    protected void OnClickPullBlackList(long roleId)
    {
        var relation = CSFriendInfo.Instance.GetRelation(roleId);
        if (relation == FriendType.FT_FRIEND)
        {
            UtilityTips.ShowPromptWordTips(10, () =>
            {
                Net.ReqAddRelationMessage(roleId.ToGoogleList(), 3);
                Close();
            });
        }
        else
        {
            Net.ReqAddRelationMessage(roleId.ToGoogleList(), 3);
            Close();
        }
    }

    protected void DeleteEnemy(long roleId)
    {
        DeleteReleation(roleId);
        Close();
    }

    protected void OnClickAddEnemy(long roleId)
    {
        Net.ReqAddRelationMessage(roleId.ToGoogleList(), (int)FriendType.FT_ENEMY);
        Close();
    }

    protected void OnClickCancelBlakList(long roleId)
    {
        DeleteReleation(roleId);
        Close();
    }

    protected override void Close()
    {
        closeSelectionPanel?.Invoke(UIPrefab);
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
