using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UITeamPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get => false;
    }

    enum teamPanelType
    {
        MyTeam, //我的队伍
        NearbyTeam, //附近队伍
        RequestTeam, //组队请求
    }

    enum myTeamInvitationType
    {
        Manual = 1, //手动组队
        Refuse = 2, //拒绝组队
        Auto = 3, //自动组队
    }

    long myRoleId = CSMainPlayerInfo.Instance.ID;
    teamPanelType myTeamPanelType;

    public override void Init()
    {
        base.Init();

        mClientEvent.Reg((uint) CEvent.GetMyTeamInfo, GetMyTeamInfo);
        mClientEvent.Reg((uint) CEvent.AddTeamForMe, AddTeamForMe);
        mClientEvent.Reg((uint) CEvent.AddTeamOther, AddTeamOther);
        mClientEvent.Reg((uint) CEvent.QuitTeam, QuitTeam);
        mClientEvent.Reg((uint) CEvent.ChangeLeader, ChangeLeader);
        mClientEvent.Reg((uint) CEvent.TeamTabMessage, TeamTabMessage);
        mClientEvent.AddEvent(CEvent.NostalgiaRefreshTime,RefreshNostalgiaTime);
        

        UIEventListener.Get(mbtn_automaticteam.gameObject, myTeamInvitationType.Auto).onClick =
            OnClickMyTeamInvitationType;
        UIEventListener.Get(mbtn_manualteam.gameObject, myTeamInvitationType.Manual).onClick =
            OnClickMyTeamInvitationType;
        UIEventListener.Get(mbtn_refuseteam.gameObject, myTeamInvitationType.Refuse).onClick =
            OnClickMyTeamInvitationType;
        UIEventListener.Get(mbtn_myteam.gameObject, teamPanelType.MyTeam).onClick = OnClickTeamPanelType;
        UIEventListener.Get(mbtn_nearbyteam.gameObject, teamPanelType.NearbyTeam).onClick = OnClickTeamPanelType;
        UIEventListener.Get(mbtn_reqteam.gameObject, teamPanelType.RequestTeam).onClick = OnClickTeamPanelType;
        UIEventListener.Get(mbtn_friends).onClick = CSNostalgiaEquipInfo.Instance.OnClickFriend;

        RefreshNostalgiaTime();
        
        
        mbtn_releaseteam.onClick = OnClickReleaseTeam;
        mbtn_invitationteam.onClick = OnClickInvitationTeam;
        mbtn_quitteam.onClick = OnClickQuitTeam;
        mbtn_close_releaseteam.onClick = OnClickCloseReleaseTeam;
        mbtn_nearby_releaseteam.onClick = OnClickNearbyReleaseTeam;
        mbtn_guild_releaseteam.onClick = OnClickGuildReleaseTeam;
        mbtn_createteam.onClick = OnClickCreateTeam;
        mbtn_refreshteam.onClick = OnClickRefreshTeam;
    }

    private void RefreshNostalgiaTime(uint uievtid = 0, object data = null)
    {
        ScriptBinder.InvokeRepeating(0,1,RefreshTime);
    }

    private void RefreshTime()
    {
        
        var Info = CSNostalgiaEquipInfo.Instance;
        long time = (Info.nextSummonTime - CSServerTime.Instance.TotalMillisecond)/ 1000;
        if (time >= 0)
            mlb_nostalgiatime.text = CSServerTime.Instance.FormatLongToTimeStr(time, 3);
        else
        {
            mlb_nostalgiatime.text = "";
            ScriptBinder.StopInvokeRepeating();
        }
    }


    public override void Show()
    {
        base.Show();
        InitDataTeam();
    }

    public override void SelectChildPanel(int type = 0)
    {
        switch (type)
        {
            case 2:
                CSGame.Sington.StartCoroutine(SetToggle());
                ShowTeamTab(teamPanelType.RequestTeam, CSTeamInfo.Instance.TeamId);
                break;
        }
    }

    IEnumerator SetToggle()
    {
        yield return null;
        mbtn_reqteam.GetComponent<UIToggle>().Set(true);
    }

    void InitDataTeam()
    {
        mbtn_myteam.GetComponent<UIToggle>().Set(true);
        mobj_tab_team.SetActive(true);
        ShowTeamTab(teamPanelType.MyTeam, CSTeamInfo.Instance.TeamId);
    }

    void GetMyTeamInfo(uint id, object data)
    {
        ShowMyTeamInfo();
    }

    /// <summary>
    /// 接收我入队（包括创建队伍）广播
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void AddTeamForMe(uint id, object data)
    {
        if (data == null) return;
        team.TeamInfo msg = (team.TeamInfo) data;

        mobj_nonteam.SetActive(false);
        mobj_btn_nonexistentteam.SetActive(false);
        mScrollViewMyTeam.SetActive(true);
        mobj_btn_existteam.SetActive(true);
        mbtn_reqteam.gameObject.SetActive(myRoleId == msg.leaderId);

        ShowMyTeamInfo();


        if (myTeamPanelType == teamPanelType.NearbyTeam)
        {
            mbtn_myteam.gameObject.GetComponent<UIToggle>().Set(true);
            mobj_btn_refreshteam.SetActive(false);
            mScrollViewNearbyTeam.SetActive(false);
            mScrollViewReqTeam.SetActive(false);
            mobj_releaseteam.SetActive(false);
            mobj_nonteamNearby.SetActive(false);
            mobj_nonteamReq.SetActive(false);
            if (CSTeamInfo.Instance.TeamId != 0)
            {
                mobj_nonteam.SetActive(false);
                mobj_btn_nonexistentteam.SetActive(false);
                mScrollViewMyTeam.SetActive(true);
                mobj_btn_existteam.SetActive(true);
                Net.ReqGetTeamInfoMessage();
                //ShowMyTeamInfo();
            }
            else //没有队伍
            {
                CSEffectPlayMgr.Instance.ShowUITexture(mobj_nonteam, "pattern");
                mobj_nonteam.SetActive(true);
                mobj_btn_nonexistentteam.SetActive(true);
                mScrollViewMyTeam.SetActive(false);
                mobj_btn_existteam.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 接收其他人入队广播
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void AddTeamOther(uint id, object data)
    {
        if (data == null) return;
        if (mobj_btn_existteam.activeSelf && !mobj_btn_nonexistentteam.activeSelf)
        {
            mobj_nonteam.SetActive(false);
            mobj_btn_nonexistentteam.SetActive(false);
            mScrollViewMyTeam.SetActive(true);
            mobj_btn_existteam.SetActive(true);
            ShowMyTeamInfo();
        }
    }

    /// <summary>
    /// 接收退队广播
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void QuitTeam(uint id, object data)
    {
        if (data == null) return;
        team.LeaveTeamResponse msg = (team.LeaveTeamResponse) data;
        //如果我离开队伍
        if (msg.roleId == myRoleId && mobj_btn_existteam.activeSelf &&
            !mobj_btn_nonexistentteam.activeSelf)
        {
            CSEffectPlayMgr.Instance.ShowUITexture(mobj_nonteam, "pattern");
            mobj_nonteam.SetActive(true);
            mobj_btn_nonexistentteam.SetActive(true);
            mScrollViewMyTeam.SetActive(false);
            mobj_btn_existteam.SetActive(false);
            mbtn_reqteam.gameObject.SetActive(false);
        }
        else if (msg.roleId != myRoleId && mobj_btn_existteam.activeSelf &&
                 !mobj_btn_nonexistentteam.activeSelf)
        {
            mobj_nonteam.SetActive(false);
            mobj_btn_nonexistentteam.SetActive(false);
            mScrollViewMyTeam.SetActive(true);
            mobj_btn_existteam.SetActive(true);
            ShowMyTeamInfo();
        }
    }

    /// <summary>
    /// 接收队长变更广播
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void ChangeLeader(uint id, object data)
    {
        if (data == null) return;
        if (mobj_btn_existteam.activeSelf && !mobj_btn_nonexistentteam.activeSelf)
        {
            ShowMyTeamInfo();
        }

        team.TeamLeaderChanged msg = (team.TeamLeaderChanged) data;
        mbtn_reqteam.gameObject.SetActive(myRoleId == msg.newLeaderId);
    }

    /// <summary>
    /// 接收组队tab广播
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void TeamTabMessage(uint id, object data)
    {
        if (data == null) return;
        team.TeamTabInfo msg = (team.TeamTabInfo) data;
        if (myTeamPanelType == teamPanelType.NearbyTeam &&
            (TeamTab) msg.tab == TeamTab.RoundTeams)
        {
            ShowTeamNearbyInfo();
        }
    }

    /// <summary>
    /// 队伍信息切换页签
    /// </summary>
    /// <param name="go"></param>
    void OnClickTeamPanelType(GameObject go)
    {
        teamPanelType temp_type = (teamPanelType) UIEventListener.Get(go).parameter;
        myTeamPanelType = temp_type;
        ShowTeamTab(temp_type, CSTeamInfo.Instance.TeamId);
    }

    /// <summary>
    /// 我的组队模式选择
    /// </summary>
    /// <param name="go"></param>
    void OnClickMyTeamInvitationType(GameObject go)
    {
        myTeamInvitationType temp_type = (myTeamInvitationType) UIEventListener.Get(go).parameter;
        Net.ReqSetTeamModeMessage((int) temp_type);
    }

    /// <summary>
    /// 控制切换标签页后显示隐藏物体，页面赋值
    /// </summary>
    /// <param name="type">点击页签索引</param>
    /// <param name="teamId">我的队伍Id</param>
    void ShowTeamTab(teamPanelType type, long teamId = 0)
    {
        //只有当我有队伍而且我时队长时，才显示组队请求按钮
        mbtn_reqteam.gameObject.SetActive(teamId != 0 && CSTeamInfo.Instance.MyTeamData.teamInfo.leaderId == myRoleId);

        //组队模式显示
        UIEventListener[] arrBtnTeamMode = new UIEventListener[3]
            {mbtn_manualteam, mbtn_refuseteam, mbtn_automaticteam};
        arrBtnTeamMode[CSMainPlayerInfo.Instance.TeamMode - 1].gameObject.GetComponent<UIToggle>().Set(true);

        switch (type)
        {
            case teamPanelType.MyTeam:
                mobj_btn_refreshteam.SetActive(false);
                mScrollViewNearbyTeam.SetActive(false);
                mScrollViewReqTeam.SetActive(false);
                mobj_releaseteam.SetActive(false);
                mobj_nonteamNearby.SetActive(false);
                mobj_nonteamReq.SetActive(false);
                if (teamId != 0)
                {
                    mobj_nonteam.SetActive(false);
                    mobj_btn_nonexistentteam.SetActive(false);
                    mScrollViewMyTeam.SetActive(true);
                    mobj_btn_existteam.SetActive(true);
                    Net.ReqGetTeamInfoMessage();
                    //ShowMyTeamInfo();
                }
                else //没有队伍
                {
                    CSEffectPlayMgr.Instance.ShowUITexture(mobj_nonteam, "pattern");
                    mobj_nonteam.SetActive(true);
                    mobj_btn_nonexistentteam.SetActive(true);
                    mScrollViewMyTeam.SetActive(false);
                    mobj_btn_existteam.SetActive(false);
                }

                break;
            case teamPanelType.NearbyTeam:
                mobj_nonteam.SetActive(false);
                mobj_btn_nonexistentteam.SetActive(false);
                mScrollViewMyTeam.SetActive(false);
                mobj_btn_existteam.SetActive(false);
                mobj_btn_refreshteam.SetActive(true);
                mScrollViewNearbyTeam.SetActive(true);
                mScrollViewReqTeam.SetActive(false);
                //mobj_nonteamNearby.SetActive(false);
                mobj_nonteamReq.SetActive(false);
                RefreshTeamNearby();
                ShowTeamNearbyInfo();
                break;
            case teamPanelType.RequestTeam:
                mobj_nonteam.SetActive(false);
                mobj_btn_nonexistentteam.SetActive(false);
                mScrollViewMyTeam.SetActive(false);
                mobj_btn_existteam.SetActive(false);
                mobj_btn_refreshteam.SetActive(true);
                mScrollViewNearbyTeam.SetActive(false);
                mScrollViewReqTeam.SetActive(true);
                mobj_nonteamNearby.SetActive(false);
                //mobj_nonteamReq.SetActive(false);
                ShowTeamReqInfo();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 当我有队伍时显示队伍的信息
    /// </summary>
    void ShowMyTeamInfo()
    {
        ILBetterList<team.TeamMember> listTeamShowOrder =
            CSTeamInfo.Instance.SortDicTeamShowOrder(CSTeamInfo.Instance.MyTeamData.teamInfo);
        if (listTeamShowOrder!=null)
        {
            //是否满员
            bool isMax = listTeamShowOrder.Count >= CSTeamInfo.NumFullStarffed;
            //非满员需要添加一个"+"号
            mgrid_myteam.MaxCount = isMax ? listTeamShowOrder.Count : listTeamShowOrder.Count + 1;

            GameObject gp;
            for (int i = 0; i < mgrid_myteam.MaxCount; i++)
            {
                gp = mgrid_myteam.controlList[i].gameObject;
                var eventHandle = UIEventListener.Get(gp);
                UIMyTeamBinder Binder;
                if (eventHandle.parameter == null)
                {
                    Binder = new UIMyTeamBinder();
                    Binder.Setup(eventHandle);
                }
                else
                {
                    Binder = eventHandle.parameter as UIMyTeamBinder;
                }
                Binder.isAdd = !isMax && i == mgrid_myteam.MaxCount - 1;
                Binder.index = i;
                Binder.actionItem = OnClickPlayers;
                Binder.Bind(Binder.isAdd ? null : listTeamShowOrder[i]);
            }

            //队伍人数赋值
            mlb_numberteam.text = $"{mlb_numberteam.FormatStr}{listTeamShowOrder.Count}";
        }
    }

    /// <summary>
    /// 点击队员
    /// </summary>
    /// <param name="go"></param>
    void OnClickPlayers(team.TeamMember teamMember, bool isAdd)
    {
        if (isAdd)
        {
            UIManager.Instance.CreatePanel<UITeamInvitationPanel>();
        }
        else
        {
            if (teamMember != null && teamMember.roleId != myRoleId)
            {
                MenuInfo info = new MenuInfo();
                info.sundryId = (int) PanelSelcetType.RoleTeam;
                info.selfTeamLeaderId = CSTeamInfo.Instance.MyTeamData.teamInfo.leaderId;
                info.SetTeamTips(
                    teamMember.roleId,
                    teamMember.name,
                    teamMember.sex,
                    teamMember.level,
                    0,
                    teamMember.career,
                    CSTeamInfo.Instance.TeamId);
                UIManager.Instance.CreatePanel<UIRoleSelectionMenuPanel>((f) =>
                {
                    (f as UIRoleSelectionMenuPanel).ShowSelectData(info);
                });
            }
        }
    }

    /// <summary>
    /// 显示附近队伍信息
    /// </summary>
    void ShowTeamNearbyInfo()
    {
        GameObject gp;
        ScriptBinder gpBinder;
        UISprite sp_role_head;
        UILabel lb_role_name;
        UILabel lb_role_level;
        UILabel lb_team_number;
        GameObject btn_addteam;

        ILBetterList<team.TeamBrief> listTeamNearby =
            CSTeamInfo.Instance.listTeamNearby;
        if (listTeamNearby.Count == 0)
        {
            CSEffectPlayMgr.Instance.ShowUITexture(mobj_nonteamNearby, "pattern");
            mobj_nonteamNearby.SetActive(true);
        }
        else
        {
            mobj_nonteamNearby.SetActive(false);
        }

        mgrid_nearbyteam.MaxCount = listTeamNearby.Count;
        for (int i = 0; i < listTeamNearby.Count; i++)
        {
            gp = mgrid_nearbyteam.controlList[i];
            gpBinder = gp.transform.GetComponent<ScriptBinder>();
            sp_role_head = gpBinder.GetObject("sp_role_head") as UISprite;
            lb_role_name = gpBinder.GetObject("lb_role_name") as UILabel;
            lb_role_level = gpBinder.GetObject("lb_role_level") as UILabel;
            lb_team_number = gpBinder.GetObject("lb_team_number") as UILabel;
            btn_addteam = gpBinder.GetObject("btn_addteam") as GameObject;

            //头像(根据职业和性别确定头像)
            sp_role_head.spriteName =
                Utility.GetPlayerIcon(listTeamNearby[i].leader.sex, listTeamNearby[i].leader.career);
            //名字
            lb_role_name.text = listTeamNearby[i].leader.name;
            //等级
            lb_role_level.text = CSString.Format(570, listTeamNearby[i].leader.level);
            lb_team_number.text = $"{listTeamNearby[i].size}/{CSTeamInfo.NumFullStarffed}";
            UIEventListener.Get(btn_addteam, listTeamNearby[i].id).onClick = OnClickAddTeam;
        }
    }

    /// <summary>
    /// 申请加入附近队伍请求
    /// </summary>
    void OnClickAddTeam(GameObject go)
    {
        long teamId = (long) UIEventListener.Get(go).parameter;
        Net.ReqApplyTeamMessage(teamId);
    }

    /// <summary>
    /// 组队请求页面信息（仅我为队长时才有）
    /// </summary>
    void ShowTeamReqInfo()
    {
        GameObject gp;
        ScriptBinder gpBinder;
        UISprite sp_role_head;
        UILabel lb_role_name;
        UILabel lb_role_level;
        GameObject btn_agree_addteam;
        GameObject btn_refuse_addteam;

        ILBetterList<team.TeamMember> listLeaderApplyMessage =
            CSTeamInfo.Instance.MyTeamData.listLeaderApplyMessage;


        if (listLeaderApplyMessage.Count == 0)
        {
            CSEffectPlayMgr.Instance.ShowUITexture(mobj_nonteamReq, "pattern");
            mobj_nonteamReq.SetActive(true);
        }
        else
        {
            mobj_nonteamReq.SetActive(false);
        }

        mgrid_reqteam.MaxCount = listLeaderApplyMessage.Count;
        for (int i = 0; i < listLeaderApplyMessage.Count; i++)
        {
            gp = mgrid_reqteam.controlList[i];
            gpBinder = gp.transform.GetComponent<ScriptBinder>();
            sp_role_head = gpBinder.GetObject("sp_role_head") as UISprite;
            lb_role_name = gpBinder.GetObject("lb_role_name") as UILabel;
            lb_role_level = gpBinder.GetObject("lb_role_level") as UILabel;
            btn_agree_addteam = gpBinder.GetObject("btn_agree_addteam") as GameObject;
            btn_refuse_addteam = gpBinder.GetObject("btn_refuse_addteam") as GameObject;

            //头像（根据职业和性别确定头像）
            sp_role_head.spriteName =
                Utility.GetPlayerIcon(listLeaderApplyMessage[i].sex, listLeaderApplyMessage[i].career);
            lb_role_name.text = listLeaderApplyMessage[i].name;
            lb_role_level.text = CSString.Format(570, listLeaderApplyMessage[i].level);

            //添加两个按钮回调
            UIEventListener.Get(btn_agree_addteam, listLeaderApplyMessage[i].roleId).onClick = OnClickAgreeAddTeam;
            UIEventListener.Get(btn_refuse_addteam, listLeaderApplyMessage[i].roleId).onClick = OnClickRefuseAddTeam;
        }
    }

    /// <summary>
    /// 队长同意
    /// </summary>
    /// <param name="go"></param>
    void OnClickAgreeAddTeam(GameObject go)
    {
        long roleId = (long) UIEventListener.Get(go).parameter;
        Net.ReqConfirmTeamApplyMessage(roleId, ConfirmTeamApplyType.Accept);
        ILBetterList<team.TeamMember> listLeaderApplyMessage = CSTeamInfo.Instance.MyTeamData.listLeaderApplyMessage;
        for (int i = 0; i < listLeaderApplyMessage.Count; i++)
        {
            if (listLeaderApplyMessage[i].roleId == roleId)
            {
                listLeaderApplyMessage.Remove(listLeaderApplyMessage[i]);
            }
        }

        //如果处理完入队申请
        if (listLeaderApplyMessage.Count == 0)
        {
            HotManager.Instance.EventHandler.SendEvent(CEvent.HandleEnrollmentApplication);
        }

        ShowTeamReqInfo();
    }

    /// <summary>
    ///  队长拒绝
    /// </summary>
    /// <param name="go"></param>
    void OnClickRefuseAddTeam(GameObject go)
    {
        long roleId = (long) UIEventListener.Get(go).parameter;
        Net.ReqConfirmTeamApplyMessage(roleId, ConfirmTeamApplyType.Refuse);
        ILBetterList<team.TeamMember> listLeaderApplyMessage = CSTeamInfo.Instance.MyTeamData.listLeaderApplyMessage;
        for (int i = 0; i < listLeaderApplyMessage.Count; i++)
        {
            if (listLeaderApplyMessage[i].roleId == roleId)
            {
                listLeaderApplyMessage.Remove(listLeaderApplyMessage[i]);
            }
        }

        //如果处理完入队申请
        if (listLeaderApplyMessage.Count == 0)
        {
            HotManager.Instance.EventHandler.SendEvent(CEvent.HandleEnrollmentApplication);
        }

        ShowTeamReqInfo();
    }

    /// <summary>
    /// 刷新附近队伍信息(打开附近队伍界面时，还有点击刷新按钮时)
    /// </summary>
    void RefreshTeamNearby()
    {
        Net.ReqGetTeamTabMessage(TeamTab.RoundTeams, 0);
    }

    void OnClickReleaseTeam(GameObject go)
    {
        mobj_releaseteam.SetActive(true);
    }

    void OnClickInvitationTeam(GameObject go)
    {
        UIManager.Instance.CreatePanel<UITeamInvitationPanel>();
    }

    /// <summary>
    /// 按下退出队伍
    /// </summary>
    /// <param name="go"></param>
    void OnClickQuitTeam(GameObject go)
    {
        Net.ReqLeaveTeamMessage(myRoleId);
    }

    void OnClickCloseReleaseTeam(GameObject go)
    {
        mobj_releaseteam.SetActive(false);
    }

    /// <summary>
    /// 附近发布组队
    /// </summary>
    /// <param name="go"></param>
    void OnClickNearbyReleaseTeam(GameObject go)
    {
        string[] arrText = GetArrStringConfig(536);
        string content =
            $"[url=func:5:team:{CSTeamInfo.Instance.TeamId.ToString()}]{arrText[0]}[u]{arrText[1]}[/u][/url]";
        CSChatManager.Instance.SendChatMsg(input: null, channel: (int) ChatType.CT_NEIGHBOURING, content: content);
        mobj_releaseteam.SetActive(false);
    }

    /// <summary>
    /// 行会发布组队
    /// </summary>
    /// <param name="go"></param>
    void OnClickGuildReleaseTeam(GameObject go)
    {
        string[] arrText = GetArrStringConfig(536);
        string content =
            $"[url=func:5:team:{CSTeamInfo.Instance.TeamId.ToString()}]{arrText[0]}[u]{arrText[1]}[/u][/url]";
        CSChatManager.Instance.SendChatMsg(input: null, channel: (int) ChatType.CT_GUILD, content: content);
        mobj_releaseteam.SetActive(false);
    }

    /// <summary>
    /// 根据冒号分割字符串
    /// </summary>
    /// <param name="id">ClientTips表id</param>
    /// <returns></returns>
    string[] GetArrStringConfig(int id)
    {
        string textConfig = ClientTipsTableManager.Instance.GetClientTipsContext(id);
        string tempText = textConfig;
        for (int i = 0; i < textConfig.Length; i++)
        {
            if (textConfig[i] == '：')
            {
                tempText = textConfig.Replace('：', ':');
                break;
            }
        }

        string[] arrText = tempText.Split(':');
        return arrText;
    }

    /// <summary>
    /// 按下创建队伍
    /// </summary>
    /// <param name="go"></param>
    void OnClickCreateTeam(GameObject go)
    {
        Net.ReqCreateTeamMessage();
    }

    /// <summary>
    /// 点刷新按钮（分附近队伍和组队请求下两种状态）
    /// </summary>
    /// <param name="go"></param>
    void OnClickRefreshTeam(GameObject go)
    {
        switch (myTeamPanelType)
        {
            case teamPanelType.NearbyTeam:
                RefreshTeamNearby();
                break;
            case teamPanelType.RequestTeam:
                ShowTeamReqInfo();
                break;
            default:
                break;
        }
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mobj_nonteam);
        base.OnDestroy();
    }
}