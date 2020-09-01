using System.Collections.Generic;
using rankalonetable;
using UnityEngine;

public partial class UIRankingPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get => false;
    }

    //子页签最大数量(固定50个)
    private const int MaxSubTabCount = 50;

    // private Schedule schedule;

    //当前选中主页签
    private RankingType curTabType = RankingType.Grade;

    //当前主页签是否打开
    private bool isOpenCurTab = true;

    //当前选中子页签
    private RankingSubType curTabSubType = RankingSubType.All;

    List<RankingType> rankingTypes;

    /// <summary>
    /// 排行榜数据
    /// </summary>
    Map<string, RankingData> mapRankingInfo;

    private List<UILabel> lb_titles;
    private Map<RankingType, string[]> mapTitleContent;

    /// <summary>
    /// 当前选中信息框的index
    /// </summary>
    private int selectInfoIndex = -1;

    private Map<RankingType, List<RankingSubType>> mapRankingSubTypes;

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.RoleRankInfo, RefreshData);
        mClientEvent.Reg((uint) CEvent.UnionRankInfo, RefreshData);

        mbtn_strength.onClick = OnClickStrength;
        mbtn_upGrade.onClick = OnClickUpGrade;

        lb_titles = new List<UILabel> {mlb2, mlb3, mlb4};
        mapTitleContent = new Map<RankingType, string[]>
        {
            [RankingType.Grade] = UtilityMainMath.StrToStrArr(CSString.Format(1158)),
            [RankingType.FightingPower] = UtilityMainMath.StrToStrArr(CSString.Format(1159)),
            [RankingType.Wing] = UtilityMainMath.StrToStrArr(CSString.Format(1160)),
            [RankingType.Guild] = UtilityMainMath.StrToStrArr(CSString.Format(1161)),
        };
        
        mapRankingSubTypes = CSRankingInfo.Instance.MapRankingSubTypes;
    }

    public override void Show()
    {
        base.Show();
        if (rankingTypes == null && mapRankingInfo == null)
        {
            rankingTypes = CSRankingInfo.Instance.RankingTypes;
            mapRankingInfo = CSRankingInfo.Instance.MapRankingInfo;
            //默认全服等级排行数据请求
            if (!mapRankingInfo.ContainsKey($"{(int) RankingType.Grade}{(int) RankingSubType.All}"))
            {
                for (int i = 0; i < mapRankingSubTypes[RankingType.Grade].Count; i++)
                {
                    Net.CSRoleRankInfoMessage((int) RankingType.Grade, (int) mapRankingSubTypes[RankingType.Grade][i]);
                }

                RefreshLastReqTime(RankingType.Grade);
                // CSRankingInfo.Instance.LastReqRankingInfoTime = CSServerTime.Instance.TotalSeconds;
            }
            else
            {
                if (CSServerTime.Instance.TotalSeconds - CSRankingInfo.Instance.DicLastReqRankingInfoTime[RankingType.Grade] >= 300)
                {
                    for (int i = 0; i < mapRankingSubTypes[RankingType.Grade].Count; i++)
                    {
                        Net.CSRoleRankInfoMessage((int) RankingType.Grade,
                            (int) mapRankingSubTypes[RankingType.Grade][i]);
                    }
                    RefreshLastReqTime(RankingType.Grade);
                    // CSRankingInfo.Instance.LastReqRankingInfoTime = CSServerTime.Instance.TotalSeconds;
                }
                else
                {
                    ResetSchedule();
                    InitData();
                }
            }
        }
    }


    void RefreshLastReqTime(RankingType type)
    {
        if (CSRankingInfo.Instance.DicLastReqRankingInfoTime.ContainsKey(type))
            CSRankingInfo.Instance.DicLastReqRankingInfoTime[type] = CSServerTime.Instance.TotalSeconds;
        else
            CSRankingInfo.Instance.DicLastReqRankingInfoTime.Add(type, CSServerTime.Instance.TotalSeconds);
    }

    /// <summary>
    /// 网络回包刷新数据
    /// </summary>
    void RefreshData(uint id, object data)
    {
        ResetSchedule();
        InitData();
    }

    /// <summary>
    /// 设置定时器(当前页一直未切换的情况,每五分钟自动刷新一次数据)
    /// </summary>
    void SetSchedule()
    {
        ScriptBinder.InvokeRepeating(300f, 300f, OnSchedule);
        // schedule = Timer.Instance.InvokeRepeating(300f, 300f, OnSchedule); //每隔5分钟刷新
    }

    void OnSchedule()
    {
        //发送当前页刷新信息请求
        for (int i = 0; i < mapRankingSubTypes[curTabType].Count; i++)
        {
            Net.CSRoleRankInfoMessage((int) curTabType, (int)mapRankingSubTypes[curTabType][i]);
        }
        // Net.CSRoleRankInfoMessage((int) curTabType, (int) curTabSubType);
        RefreshLastReqTime(curTabType);
        // CSRankingInfo.Instance.LastReqRankingInfoTime = CSServerTime.Instance.TotalSeconds;
    }

    void ResetSchedule()
    {
        ScriptBinder.StopInvokeRepeating();
        SetSchedule();
    }

    void InitData()
    {
        RefreshGridTab();
        RefreshRightInfo();
    }

    void RefreshGridTab()
    {
        mgrid_tab.UnBind<UIRankingTabBinder>();
        mgrid_tab.MaxCount = rankingTypes.Count;
        GameObject gp;
        for (int i = 0; i < mgrid_tab.MaxCount; i++)
        {
            gp = mgrid_tab.controlList[i];
            var eventHandle = UIEventListener.Get(gp);
            UIRankingTabBinder Binder;
            if(eventHandle.parameter == null)
            {
                Binder = new UIRankingTabBinder();
                Binder.Setup(eventHandle);
            }
            else
            {
                Binder = eventHandle.parameter as UIRankingTabBinder;
            }
            Binder.type = rankingTypes[i];
            Binder.curSubType = curTabSubType;
            Binder.isSelect = curTabType == rankingTypes[i];
            Binder.isOpen = Binder.isSelect ? isOpenCurTab : false;
            Binder.actionTab = OnClickTab;
            Binder.actionItem = OnClickSubTab;
            Binder.Bind(null);
        }

        muitable_tab.repositionNow = true;
        muitable_tab.Reposition();
    }
    
    
    /// <summary>
    /// 点击大页签
    /// </summary>
    /// <param name="index"></param>
    void OnClickTab(RankingType type, bool isOpen)
    {
        if (type == curTabType)
        {
            isOpenCurTab = !isOpen;
            RefreshGridTab();
        }
        else
        {
            curTabType = type;
            curTabSubType = RankingSubType.All;
            isOpenCurTab = true;

            ResetSchedule();
            InitData();
        }
    }

    /// <summary>
    /// 点击子页签
    /// </summary>
    /// <param name="subType"></param>
    void OnClickSubTab(RankingSubType subType)
    {
        if (curTabSubType == subType) return;
        curTabSubType = subType;
        ResetSchedule();
        RefreshRightInfo();
    }


    // private List<UIRankingInfoBinder> listRankingInfoBinder;

    /// <summary>
    /// 刷新右边排行榜信息
    /// </summary>
    void RefreshRightInfo()
    {
        SetTitleText();
        selectInfoIndex = -1;
        if (!mapRankingInfo.ContainsKey($"{(int) curTabType}{(int) curTabSubType}"))
        {
            for (int i = 0; i < mapRankingSubTypes[curTabType].Count; i++)
            {
                Net.CSRoleRankInfoMessage((int) curTabType, (int)mapRankingSubTypes[curTabType][i]);
            }
            RefreshLastReqTime(curTabType);
            // Net.CSRoleRankInfoMessage((int) curTabType, (int) curTabSubType);
        }
        else
        {
            if (CSServerTime.Instance.TotalSeconds - CSRankingInfo.Instance.DicLastReqRankingInfoTime[curTabType] >= 300)
            {
                for (int i = 0; i < mapRankingSubTypes[curTabType].Count; i++)
                {
                    Net.CSRoleRankInfoMessage((int) curTabType, (int)mapRankingSubTypes[curTabType][i]);
                }
                // Net.CSRoleRankInfoMessage((int) curTabType, (int) curTabSubType);
                RefreshLastReqTime(curTabType);
                // CSRankingInfo.Instance.LastReqRankingInfoTime = CSServerTime.Instance.TotalSeconds;
            }
            else
            {
                mgrid_info.MaxCount = 10;
                mwarp_info.minIndex = -49;
                mwarp_info.maxIndex = 0;
                mwarp_info.cullContent = false;
                mwarp_info.SortBasedOnScrollMovement();
                mwarp_info.enabled = true;
                mwarp_info.onInitializeItem = OnUpdateItem;
                mScrollView_Info.ResetPosition();
                //底部信息
                SetBottomData();
            }
        }
    }
    
    void OnUpdateItem(GameObject go, int index, int realIndex)
    {
        int realIndexAbs = Mathf.Abs(realIndex);
        var eventHandle = UIEventListener.Get(go);
        UIRankingInfoBinder Binder;
        if(eventHandle.parameter == null)
        {
            Binder = new UIRankingInfoBinder();
            Binder.Setup(eventHandle);
        }
        else
        {
            Binder = eventHandle.parameter as UIRankingInfoBinder;
        }
                    
        Binder.index = realIndexAbs;
        Binder.type = curTabType;
        Binder.subType = curTabSubType;
        Binder.isSelect = realIndexAbs == selectInfoIndex;
        Binder.actionInfo = OnClickInfo;
        Binder.Bind(mapRankingInfo[$"{(int) curTabType}{(int) curTabSubType}"]);
    }

    void SetBottomData()
    {
        RankingData rankingData = mapRankingInfo[$"{(int) curTabType}{(int) curTabSubType}"];
        //我的排名
        switch (curTabType)
        {
            case RankingType.Grade:
            case RankingType.FightingPower:
            case RankingType.Wing:
                mlb_myrank.text = rankingData.RoleRankInfo.myRank > 0
                    ? rankingData.RoleRankInfo.myRank.ToString()
                    : CSString.Format(1164);
                mlb_myrank.gameObject.SetActive(true);
                if (curTabType == RankingType.Grade||curTabType == RankingType.FightingPower)
                {
                    mlb_myrank.gameObject.SetActive(curTabSubType==0||(int)curTabSubType==CSMainPlayerInfo.Instance.Career);
                }
                break;
            case RankingType.Guild:
                mlb_myrank.gameObject.SetActive(false);
                break;
        }
        //按钮(我要升级/我要变强)
        switch (curTabType)
        {
            case RankingType.Grade:
            case RankingType.FightingPower:
                mbtn_strength.gameObject.SetActive(curTabSubType==0||(int)curTabSubType==CSMainPlayerInfo.Instance.Career);
                mbtn_upGrade.gameObject.SetActive(false);
                break;
            case RankingType.Wing:
                mbtn_strength.gameObject.SetActive(false);
                mbtn_upGrade.gameObject.SetActive(true);
                break;
            case RankingType.Guild:
                mbtn_strength.gameObject.SetActive(false);
                mbtn_upGrade.gameObject.SetActive(false);
                break;
        }
    }

    void OnClickInfo(int index)
    {
        if (selectInfoIndex != index)
            selectInfoIndex = index;
        //有人上榜则弹出人物选中面板
        RankingData rankingData = mapRankingInfo[$"{(int) curTabType}{(int) curTabSubType}"];
        switch (curTabType)
        {
            case RankingType.Grade:
            case RankingType.FightingPower:
            case RankingType.Wing:
                if (rankingData.RoleRankInfo.roleRankInfo.Count - 1 >= selectInfoIndex)
                {
                    RoleRankInfo roleRankInfo = rankingData.RoleRankInfo.roleRankInfo[selectInfoIndex];
                    if (roleRankInfo.roleId == CSMainPlayerInfo.Instance.ID)
                    {
                        UtilityTips.ShowTips(1172, 1.5f, ColorType.Red);
                        return;
                    }

                    MenuInfo info = new MenuInfo();
                    info.sundryId = (int) PanelSelcetType.Ranking;
                    info.roleId = roleRankInfo.roleId;
                    info.roleName = roleRankInfo.name;
                    info.lv = roleRankInfo.level;
                    info.sex = roleRankInfo.sex;
                    info.career = roleRankInfo.career;
                    info.guildId = roleRankInfo.unionId;
                    info.position = roleRankInfo.position;
                    UIManager.Instance.CreatePanel<UIRoleSelectionMenuPanel>((f) =>
                    {
                        (f as UIRoleSelectionMenuPanel).ShowSelectData(info);
                    });
                }

                break;
            case RankingType.Guild:
                if (rankingData.UnionRankInfo.unionRankInfo.Count - 1 >= selectInfoIndex)
                {
                    UnionRankInfo unionRankInfo = rankingData.UnionRankInfo.unionRankInfo[selectInfoIndex];
                    if (unionRankInfo.leaderId == CSMainPlayerInfo.Instance.ID)
                    {
                        UtilityTips.ShowTips(10006, 1.5f, ColorType.Red);
                        return;
                    }

                    MenuInfo info = new MenuInfo();
                    info.sundryId = (int) PanelSelcetType.Ranking;
                    info.roleId = unionRankInfo.leaderId;
                    info.roleName = unionRankInfo.name;
                    info.lv = unionRankInfo.leaderLevel;
                    info.sex = unionRankInfo.sex;
                    info.career = unionRankInfo.career;
                    info.guildId = unionRankInfo.unionId;
                    info.position = (int) GuildPos.President; //会长
                    UIManager.Instance.CreatePanel<UIRoleSelectionMenuPanel>((f) =>
                    {
                        (f as UIRoleSelectionMenuPanel).ShowSelectData(info);
                    });
                }

                break;
        }
    }

    /// <summary>
    /// 设置头文字
    /// </summary>
    /// <param name="type"></param>
    void SetTitleText()
    {
        for (int i = 0; i < lb_titles.Count; i++)
        {
            lb_titles[i].text = mapTitleContent[curTabType][i];
        }
    }

    void OnClickStrength(GameObject go)
    {
        switch (curTabType)
        {
            case RankingType.Grade:
            case RankingType.FightingPower:
                UtilityPanel.JumpToPanel(29101);
                UIManager.Instance.ClosePanel<UIRankingCombinedPanel>();
                break;
        }
    }

    void OnClickUpGrade(GameObject go)
    {
        switch (curTabType)
        {
            case RankingType.Wing:
                UtilityPanel.JumpToPanel(11900);
                UIManager.Instance.ClosePanel<UIRankingCombinedPanel>();
                break;
        }
    }

    public override void OnHide()
    {
        base.OnHide();
        ScriptBinder.StopInvokeRepeating();
        // if (Timer.Instance.IsInvoking(schedule))
        //     Timer.Instance.CancelInvoke(schedule);
    }

    protected override void OnDestroy()
    {
        mgrid_tab.UnBind<UIRankingTabBinder>();
        mgrid_info.UnBind<UIRankingInfoBinder>();
        // listRankingTabBinder = null;
        // listRankingInfoBinder = null;
        base.OnDestroy();
    }
}