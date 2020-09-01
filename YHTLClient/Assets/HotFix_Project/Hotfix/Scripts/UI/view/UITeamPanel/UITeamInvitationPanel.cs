using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UITeamInvitationPanel : UIBasePanel
{
    public override bool ShowGaussianBlur { get => false; }

    enum panelType
    {
        Nearby = 1,     //附近
        Friend = 2,     //好友
        Guild = 3,      //行会
    }
    
    panelType myPanelType = panelType.Nearby;
    
    
    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint)CEvent.TeamTabMessage, TeamTabMessage);
        mClientEvent.Reg((uint)CEvent.AddTeamOther, AddTeamOther);
        UIEventListener.Get(mbtn_nearby.gameObject, panelType.Nearby).onClick = OnClickTeamInvitationType;
        UIEventListener.Get(mbtn_friend.gameObject, panelType.Friend).onClick = OnClickTeamInvitationType;
        UIEventListener.Get(mbtn_guild.gameObject, panelType.Guild).onClick = OnClickTeamInvitationType;

        mbtn_close.onClick = OnClose;
        mbtn_close1.onClick = OnClose;
    }

    public override void Show()
    {
        base.Show();
        InitData();
    }
      
    void InitData()
    {
        RefreshPlayer(panelType.Nearby);
        ILBetterList<team.TeamMember> listPlayers = CSTeamInfo.Instance.listPlayersNearby;
        mlb_tips1.SetActive(listPlayers.Count == 0);
        mlb_tips2.SetActive(false);
        mlb_tips3.SetActive(false);
        ShowPlayers(listPlayers);
    }

    /// <summary>
    /// 接收其他人入队广播
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void AddTeamOther(uint id, object data)
    {
        RefreshPlayer(myPanelType);
    }

    /// <summary>
    /// 接收tab广播
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void TeamTabMessage(uint id, object data)
    {
        if (data == null) return;
        team.TeamTabInfo msg = (team.TeamTabInfo)data;
        ILBetterList<team.TeamMember> listPlayers = new ILBetterList<team.TeamMember>();
        if (myPanelType == panelType.Nearby &&
            (TeamTab)msg.tab == TeamTab.RoundPlayers)
        {
            listPlayers = CSTeamInfo.Instance.listPlayersNearby;
            mlb_tips1.SetActive(listPlayers.Count == 0);
            mlb_tips2.SetActive(false);
            mlb_tips3.SetActive(false);
        }
        else if (myPanelType == panelType.Friend &&
                 (TeamTab)msg.tab == TeamTab.Friends)
        {
            listPlayers = CSTeamInfo.Instance.listPlayersFriend;
            mlb_tips1.SetActive(false);
            mlb_tips2.SetActive(listPlayers.Count == 0);
            mlb_tips3.SetActive(false);
        }
        else if (myPanelType == panelType.Guild &&
                 (TeamTab)msg.tab == TeamTab.Union)
        {
            listPlayers = CSTeamInfo.Instance.listPlayersGuild;
            mlb_tips1.SetActive(false);
            mlb_tips2.SetActive(false);
            mlb_tips3.SetActive(listPlayers.Count == 0);
        }
        
        ShowPlayers(listPlayers);
    }

    void OnClickTeamInvitationType(GameObject go)
    {
        ILBetterList<team.TeamMember> listPlayers = new ILBetterList<team.TeamMember>();
        panelType temp_type = (panelType)UIEventListener.Get(go).parameter;
        myPanelType = temp_type;
        RefreshPlayer(temp_type);
        switch (temp_type)
        {
            case panelType.Nearby:
                listPlayers = CSTeamInfo.Instance.listPlayersNearby;
                mlb_tips1.SetActive(listPlayers.Count == 0);
                mlb_tips2.SetActive(false);
                mlb_tips3.SetActive(false);
                break;
            case panelType.Friend:
                listPlayers = CSTeamInfo.Instance.listPlayersFriend;
                mlb_tips1.SetActive(false);
                mlb_tips2.SetActive(listPlayers.Count == 0);
                mlb_tips3.SetActive(false);
                break;
            case panelType.Guild:
                listPlayers = CSTeamInfo.Instance.listPlayersGuild;
                mlb_tips1.SetActive(false);
                mlb_tips2.SetActive(false);
                mlb_tips3.SetActive(listPlayers.Count == 0);
                break;
            default:
                break;
        }
        ShowPlayers(listPlayers);
    }

    void ShowPlayers(ILBetterList<team.TeamMember> listPlayers)
    {
        GameObject gp;
        ScriptBinder gpBinder;
        UISprite sp_role_head;
        UILabel lb_role_name;
        UILabel lb_role_level;
        GameObject btn_invitationteam;

        mgrid_reqteam.MaxCount = listPlayers.Count;
        for (int i = 0; i < listPlayers.Count; i++)
        {
            gp = mgrid_reqteam.controlList[i];
            gpBinder = gp.transform.GetComponent<ScriptBinder>();
            sp_role_head = gpBinder.GetObject("sp_role_head") as UISprite;
            lb_role_name = gpBinder.GetObject("lb_role_name") as UILabel;
            lb_role_level = gpBinder.GetObject("lb_role_level") as UILabel;
            btn_invitationteam = gpBinder.GetObject("btn_invitationteam") as GameObject;

            //头像
            sp_role_head.spriteName = Utility.GetPlayerIcon(listPlayers[i].sex,listPlayers[i].career);
            lb_role_name.text =listPlayers[i].name;
            CSStringBuilder.Clear();
            lb_role_level.text = CSStringBuilder.Append(lb_role_level.FormatStr, listPlayers[i].level).ToString();
            UIEventListener.Get(btn_invitationteam, listPlayers[i].roleId).onClick = OnClickTeamInvitation;

        }
    }

    void OnClickTeamInvitation(GameObject go)
    {
        long roleId = (long)UIEventListener.Get(go).parameter;
        Net.ReqInviteTeamMessage(roleId, (TeamTab)myPanelType);
    }

    void RefreshPlayer(panelType type)
    {
        Net.ReqGetTeamTabMessage((TeamTab)type, 0);
    }


    void OnClose(GameObject go)
    {
        UIManager.Instance.ClosePanel<UITeamInvitationPanel>();
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}