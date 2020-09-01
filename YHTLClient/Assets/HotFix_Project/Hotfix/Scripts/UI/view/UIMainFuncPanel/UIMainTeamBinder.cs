using System;
using System.Collections.Generic;
using UnityEngine;

public class UIMainTeamBinder : UIBinder
{
    private GameObject gp;
    GameObject obj_player;
    GameObject obj_invitation;
    GameObject obj_team_btns;
    UISprite sp_tubiao;
    UILabel lb_name;
    UILabel lb_level;
    GameObject obj_flag;
    UISlider slider_hp;
    UISlider slider_magic;
    UISprite fg;
    GameObject sp_select;
    
    private UIGridContainer grid_team_btns;
    private GameObject sp_bg1;
    private GameObject sp_bg2;
    private GameObject btn_close;
    string[] arrBtnsContent;
    
    private Map<int, string> careerIconName;
    public bool isInvitationTeam = false;
    public bool isLeader = false;
    public bool isSelect = false;
    team.TeamMember teamMember;
    public Action<int> actionItem;
    public int selectIndex = 0;

    public override void Init(UIEventListener handle)
    {
        gp = handle.gameObject;
        obj_player = Get<GameObject>("obj_player");
        obj_invitation = Get<GameObject>("obj_invitation");
        obj_team_btns = Get<GameObject>("obj_team_btns");
        sp_tubiao = Get<UISprite>("obj_player/sp_tubiao");
        lb_name = Get<UILabel>("obj_player/lb_name");
        lb_level = Get<UILabel>("obj_player/lb_level");
        obj_flag = Get<GameObject>("obj_player/obj_flag");
        slider_hp = Get<UISlider>("obj_player/slider_hp");
        slider_magic = Get<UISlider>("obj_player/slider_magic");
        fg = Get<UISprite>("fg", slider_hp.transform);
        grid_team_btns = Get<UIGridContainer>("grid_team_btns", obj_team_btns.transform);
        sp_bg1 = Get<GameObject>("sp_bg1", obj_team_btns.transform);
        sp_bg2 = Get<GameObject>("sp_bg2", obj_team_btns.transform);
        sp_select = Get<GameObject>("sp_select", obj_player.transform);
        btn_close = Get<GameObject>("btn_close", obj_team_btns.transform);
        
        UIEventListener.Get(gp).onClick = OnClickItem;
        UIEventListener.Get(btn_close).onClick = OnClickCloseTeamBtns;

        if (null==careerIconName)
            careerIconName = new Map<int, string>();
        careerIconName.Add(1, "mission_zhan");
        careerIconName.Add(2, "mission_fa");
        careerIconName.Add(3, "mission_dao");

        arrBtnsContent = UtilityMainMath.StrToStrArr(CSString.Format(1677));
        obj_team_btns.SetActive(false);
    }

    public override void Bind(object data)
    {
        if (data != null)
            teamMember = (team.TeamMember)data;
        RefreshUI();
    }

    void RefreshUI()
    {
        obj_player.SetActive(!isInvitationTeam);
        obj_invitation.SetActive(isInvitationTeam);
        sp_select.SetActive(isSelect);
        if (selectIndex == -1)
            obj_team_btns.SetActive(false);

        //显示队员信息
        if (!isInvitationTeam && teamMember!=null)
        {
            sp_tubiao.spriteName = careerIconName[teamMember.career];
            sp_tubiao.color = teamMember.hp > 0 ? Color.white : Color.gray;
            lb_name.text = teamMember.name;
            lb_name.color = UtilityColor.GetColor(teamMember.hp > 0 ? ColorType.White : ColorType.WeakText);
            lb_level.text = $"{teamMember.level}{lb_level.FormatStr}";
            lb_level.color = UtilityColor.GetColor(teamMember.hp > 0 ? ColorType.White : ColorType.WeakText);
            slider_hp.value = teamMember.hp*1f / teamMember.maxHp;
            fg.spriteName = slider_hp.value >= 0.3f?"team_hp2":"team_hp";
            slider_magic.value = teamMember.mp*1f / teamMember.maxMp;
            obj_flag.SetActive(isLeader);
        }
    }
    
    void OnClickItem(GameObject go)
    {
        if (isInvitationTeam)
        {
            obj_team_btns.SetActive(false);
            UIManager.Instance.CreatePanel<UITeamInvitationPanel>();
        }
        else
        {
            if (CSTeamInfo.Instance.MyTeamData == null) return;
            if (arrBtnsContent == null) return;
            if (arrBtnsContent.Length != 5) return;
            long roleId = teamMember.roleId;
            if (CSMainPlayerInfo.Instance.ID == CSTeamInfo.Instance.MyTeamData.teamInfo.leaderId) //如果我是队长
            {
                obj_team_btns.SetActive(true);
                if (roleId == CSMainPlayerInfo.Instance.ID) //点的是自己
                {
                    grid_team_btns.MaxCount = 3;
                    sp_bg1.SetActive(false);
                    sp_bg2.SetActive(false);
                    //sp_bg3.SetActive(true);

                    GameObject gp;
                    UILabel lb_btn_item;
                    for (int i = 0; i < grid_team_btns.MaxCount; i++)
                    {
                        gp = grid_team_btns.controlList[i];
                        lb_btn_item = gp.transform.Find("lb_btn_item").gameObject.GetComponent<UILabel>();
                        if (i == 0)
                        {
                            lb_btn_item.text = arrBtnsContent[0];
                            UIEventListener.Get(gp).onClick = OnClickDisposeTeam;
                        }
						else if (i == 1)
                        {
                            lb_btn_item.text = arrBtnsContent[1];
                            UIEventListener.Get(gp).onClick = OnClickLeaveTeam;
                        }
                        else if (i == 2)
                        {
                            lb_btn_item.text = arrBtnsContent[4];
                            UIEventListener.Get(gp).onClick = CSNostalgiaEquipInfo.Instance.OnClickFriend;
                        }
                        
                    }
                }
                else //点的是队员
                {
                    grid_team_btns.MaxCount = 2;
                    sp_bg1.SetActive(false);
                    sp_bg2.SetActive(true);

                    GameObject gp;
                    UILabel lb_btn_item;
                    for (int i = 0; i < grid_team_btns.MaxCount; i++)
                    {
                        gp = grid_team_btns.controlList[i];
                        lb_btn_item = gp.transform.Find("lb_btn_item").gameObject.GetComponent<UILabel>();
                        if (i == 0)
                        {
                            lb_btn_item.text = arrBtnsContent[2];
                            UIEventListener.Get(gp).onClick = OnClickChangeTeamLeader;
                        }
                        else if (i == 1)
                        {
                            lb_btn_item.text = arrBtnsContent[3];
                            UIEventListener.Get(gp).onClick = OnClickLeaveTeam;
                        }
                    }
                }
            }
            else //不是队长(只有点自己才有反应)
            {
                if (roleId == CSMainPlayerInfo.Instance.ID)
                {
                    obj_team_btns.SetActive(true);
                    grid_team_btns.MaxCount = 1;
                    sp_bg1.SetActive(true);
                    sp_bg2.SetActive(false);

                    GameObject gp = grid_team_btns.controlList[0];
                    UILabel lb_btn_item = gp.transform.Find("lb_btn_item").gameObject.GetComponent<UILabel>();

                    lb_btn_item.text = arrBtnsContent[1];
                    UIEventListener.Get(gp).onClick = OnClickLeaveTeam;
                }
                else
                {
                    obj_team_btns.SetActive(false);
                }
            }
        }
        actionItem?.Invoke(index);
    }

    /// <summary>
    /// 转让队长
    /// </summary>
    /// <param name="go"></param>
    void OnClickChangeTeamLeader(GameObject go)
    {
        long roleId = teamMember.roleId;
        Net.ReqChangeTeamLeaderMessage(roleId);
        obj_team_btns.SetActive(false);
    }

    /// <summary>
    /// 退队或者踢人
    /// </summary>
    /// <param name="go"></param>
    void OnClickLeaveTeam(GameObject go)
    {
        long roleId = teamMember.roleId;
        Net.ReqLeaveTeamMessage(roleId);
        obj_team_btns.SetActive(false);
    }

    /// <summary>
    /// 解散队伍
    /// </summary>
    /// <param name="go"></param>
    void OnClickDisposeTeam(GameObject go)
    {
        Net.ReqDisposeTeamsMessage();
        obj_team_btns.SetActive(false);
    }

    /// <summary>
    /// 关闭对队员操作面板
    /// </summary>
    /// <param name="go"></param>
    void OnClickCloseTeamBtns(GameObject go)
    {
        obj_team_btns.SetActive(false);
    }
    
    public override void OnDestroy()
    {
        gp = null;
        obj_player = null;
        obj_invitation = null; 
        obj_team_btns = null;
        sp_tubiao = null;
        lb_name = null;
        lb_level = null;
        obj_flag = null;
        slider_hp = null;
        slider_magic = null;
        fg = null;
        careerIconName = null;
        teamMember = null;
        grid_team_btns= null;
        sp_bg1 = null;
        sp_bg2 = null;
        btn_close = null;
        arrBtnsContent = null;
        sp_select = null;
        actionItem = null;
    }
}
