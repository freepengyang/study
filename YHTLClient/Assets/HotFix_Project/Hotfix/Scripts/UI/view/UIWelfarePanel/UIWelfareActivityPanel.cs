using System;
using System.Collections.Generic;
using UnityEngine;


public enum WelfareActivityType //与sundry表中的页签字符串分割后的顺序对应(枚举从1开始与GameModel中的Layer对应),否则页签上的文字将显示不正确
{
    MonthCard = 1, //月卡
    Discount, //优惠礼包
    WelfareDirectPurchase, //直购礼包
    AddUpRecharge, //累充

    //DayCharge ,//每日充值
    DayChargeMap, //每日充值地图
    MonthCardMap, //月卡地图
    VipMap, //会员地图
    DownloadGift, //下载有礼
    // LifeTimeFund,//终生基金
    ActivationCode,//激活码兑换
}

public partial class UIWelfareActivityPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }


    const int TabConfigIdInSundry = 616; //页签信息在dundry表中的id
    string[] tabStrings;


    Map<int, UIWelFareTabBtnItem> panelDic;
    CSBetterLisHot<UIWelFareTabBtnItem> showList;
    Map<uint, List<UIWelFareTabBtnItem>> specialEventsDic;

    UIWelFareTabBtnItem curTab;

    public override void Init()
    {
        base.Init();

        tabStrings = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(TabConfigIdInSundry));
        mbtn_close.onClick = CloseBtnClick;

        //在此处注册页面
        //RegPanel<UIServerActivityRechargePanel>(WelfareActivityType.DayCharge, RedPointType.DayCharge);
        RegPanel<UIServerActivityMonthCardPanel>(WelfareActivityType.MonthCard, FunctionType.funcP_monthCard,
            RedPointType.MonthCard);
        // RegPanel<UIServerActivityGiftBagPanel>(WelfareActivityType.LimitGift);
        RegPanel<UIServerActivityMapPanel>(WelfareActivityType.DayChargeMap, FunctionType.none,
            RedPointType.DayChargeMap);
        // RegPanel<UILifeTimeFundPanel>(WelfareActivityType.LifeTimeFund, FunctionType.none, RedPointType.LifeTimeFund);
        RegPanel<UIServerActivityDownloadPanel>(WelfareActivityType.DownloadGift, FunctionType.none,
            RedPointType.DownloadGift);
        RegPanel<UIAddUpRechargePanel>(WelfareActivityType.AddUpRecharge, FunctionType.none,
            RedPointType.AddUpRecharge);
        RegPanel<UIWelfareDirectPurchasePanel>(WelfareActivityType.WelfareDirectPurchase, FunctionType.none,
            RedPointType.DirectPurchaseGift, RedPointType.DirectPurchaseReceive);
        RegPanel<UIWelfareMonthMapPanel>(WelfareActivityType.MonthCardMap, FunctionType.funcP_monthCard,
            RedPointType.MonthCardMap);
        RegPanel<UIWelfareVIPMapPanel>(WelfareActivityType.VipMap);
        RegPanel<UIWelfareGiftBagPanel>(WelfareActivityType.Discount, FunctionType.none,
            RedPointType.DiscountGiftBag);
        RegPanel<UIWelfareActivationCodePanel>(WelfareActivityType.ActivationCode);


        //此处注册需要特殊处理显示隐藏规则的页面
        //RegSepcialRefreshTab(WelfareActivityType.DayCharge,CSDayChargeInfo.Instance.isReceived, CEvent.GetDayChargeInfo);
        RegSepcialRefreshTab(WelfareActivityType.DownloadGift, CSDownloadGiftInfo.Instance.CanShowDownloadRewardPanel,
            CEvent.PlayerRoleExtraValues);

        RefreshLeftTabs();

        SetMoneyIds(1, 4);

        mClientEvent.AddEvent(CEvent.FastAccessJumpToPanel, CloseEvent);

        mClientEvent.SendEvent(CEvent.HideChatPanel);
    }

    public override void Show()
    {
        base.Show();

        OpenPanel();
    }

    void OpenPanel()
    {
        if (showList != null && showList.Count > 0) TabsClick(showList[0].UIPrefab);
        else
        {
            CloseBtnClick(null);
        }
    }


    protected override void OnDestroy()
    {
        if (panelDic != null)
        {
            for (panelDic.Begin(); panelDic.Next();)
            {
                if (panelDic.Value.panelType != null && panelDic.Value.panel != null)
                {
                    UIManager.Instance.ClosePanel(panelDic.Value.panelType);
                }

                panelDic.Value.Dispose();
            }

            panelDic.Clear();
        }

        panelDic = null;
        showList?.Clear();
        showList = null;
        specialEventsDic?.Clear();
        specialEventsDic = null;
        subType = 1;
        base.OnDestroy();
    }

    public override void SelectChildPanel(int type = 1)
    {
        if (showList == null) return;

        UIWelFareTabBtnItem item = showList.FirstOrNull(x => { return (int)x.welfareType == type; });
        if (item == null)
        {
            FNDebug.LogError("跳转到福利大厅子页面失败");
            return;
        }

        TabsClick(item.UIPrefab);
    }


    public static int subType = 1;

    public override void SelectChildPanel(int type, int _subType)
    {
        if (type == 0 || showList == null) return;

        UIWelFareTabBtnItem item = showList.FirstOrNull(x => { return (int)x.welfareType == type; });
        if (item == null)
        {
            FNDebug.LogError("跳转到福利大厅子页面失败");
            return;
        }

        subType = _subType;
        TabsClick(item.UIPrefab);
    }


    /// <summary>注册页面</summary>
    void RegPanel<T>(WelfareActivityType _type, FunctionType funcOpen = FunctionType.none,
        params RedPointType[] pointTypes) where T : UIBasePanel
    {
        if (panelDic == null) panelDic = new Map<int, UIWelFareTabBtnItem>();
        UIWelFareTabBtnItem tabItem = mPoolHandleManager.GetCustomClass<UIWelFareTabBtnItem>();
        tabItem.BindPanel(_type, typeof(T), funcOpen, pointTypes);
        panelDic.Add((int) _type, tabItem);
    }


    /// <summary>
    /// 注册需要做特殊刷新处理的页签页面
    /// </summary>
    void RegSepcialRefreshTab(WelfareActivityType _type, Func<bool> state, params CEvent[] cEvents)
    {
        if (specialEventsDic == null) specialEventsDic = new Map<uint, List<UIWelFareTabBtnItem>>();
        if (!panelDic.ContainsKey((int) _type)) return; //注册刷新规则前需要先注册界面

        UIWelFareTabBtnItem tab = panelDic[(int) _type];

        for (int i = 0; i < cEvents.Length; i++)
        {
            List<UIWelFareTabBtnItem> regItems;
            if (!specialEventsDic.TryGetValue((uint) cEvents[i], out regItems))
            {
                regItems = new List<UIWelFareTabBtnItem>();
                mClientEvent.Reg((uint) cEvents[i], TabShowOrHideEvent);
                specialEventsDic.Add((uint) cEvents[i], regItems);
            }

            if (regItems.Contains(tab)) continue;
            tab.specialShowRule = state;
            regItems.Add(tab);
        }
    }


    void RefreshLeftTabs()
    {
        if (panelDic == null || panelDic.Count < 1) return;
        if (showList == null) showList = new CSBetterLisHot<UIWelFareTabBtnItem>();
        else showList.Clear();

        for (panelDic.Begin(); panelDic.Next();)
        {
            if (panelDic.Value == null) continue;
            FunctionType funcOpen = panelDic.Value.funcOpenType;
            if ((funcOpen == FunctionType.none || UICheckManager.Instance.DoCheckFunction(funcOpen)) &&
                (panelDic.Value.specialShowRule == null || panelDic.Value.specialShowRule.Invoke()))
            {
                panelDic.Value.showState = true;
                showList.Add(panelDic.Value);
            }
        }

        if (showList.Count < 1) return;

        showList.Sort((a, b) => { return a.welfareType - b.welfareType; });

        mgrid_tabs.MaxCount = showList.Count;
        for (int i = 0; i < mgrid_tabs.MaxCount; i++)
        {
            showList[i].BindObj(mgrid_tabs.controlList[i], GetTabLabel(showList[i].welfareType), mtrans_panelParent);
            UIEventListener.Get(showList[i].UIPrefab, showList[i]).onClick = TabsClick;
        }
    }


    void TabShowOrHideEvent(uint id, object data)
    {
        if (!specialEventsDic.ContainsKey(id) || specialEventsDic[id] == null || specialEventsDic[id].Count < 1) return;
        for (int i = 0; i < specialEventsDic[id].Count; i++)
        {
            if (specialEventsDic[id][i] == null || specialEventsDic[id][i].specialShowRule == null) continue;

            if (specialEventsDic[id][i].showState != specialEventsDic[id][i].specialShowRule.Invoke())
            {
                RefreshLeftTabs();
                OpenPanel();
                break;
            }
        }
    }


    void CloseBtnClick(GameObject go)
    {
        UIManager.Instance.ClosePanel<UIWelfareActivityPanel>();
    }


    void TabsClick(GameObject _go)
    {
        UIWelFareTabBtnItem tabsItem = (UIWelFareTabBtnItem) UIEventListener.Get(_go).parameter;
        if (curTab != null)
        {
            curTab.OnClick(false);
        }

        curTab = tabsItem;
        curTab.OnClick(true);
    }


    string GetTabLabel(WelfareActivityType _type)
    {
        if (tabStrings == null) return "";
        int index = (int) _type - 1;
        //string[] allTab = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(TabConfigIdInSundry));
        if (index < 0 || index >= tabStrings.Length) return "";

        return tabStrings[index];
    }

    void CloseEvent(uint id, object data)
    {
        int panelId = System.Convert.ToInt32(data);
        if (UtilityPanel.CheckGameModelPanelIsThis<UIWelfareActivityPanel>(panelId))
        {
            return;
        }
        Close();
    }
}


public class UIWelFareTabBtnItem : UIBase, IDispose
{
    UILabel _lb_name;

    UILabel lb_name
    {
        get { return _lb_name ?? (_lb_name = Get<UILabel>("Label")); }
    }

    UILabel _lb_nameInCheck;

    UILabel lb_nameInCheck
    {
        get { return _lb_nameInCheck ?? (_lb_nameInCheck = Get<UILabel>("Checkmark/Label")); }
    }

    GameObject _obj_check;

    GameObject obj_check
    {
        get { return _obj_check ?? (_obj_check = Get<GameObject>("Checkmark")); }
    }

    GameObject _redPoint;

    public GameObject redPoint
    {
        get { return _redPoint ?? (_redPoint = Get<GameObject>("red")); }
    }


    public WelfareActivityType welfareType;

    Transform panelParent;

    GameObject panelObj;
    public Type panelType;
    public UIBasePanel panel;

    public RedPointType[] redPointTypes;
    public FunctionType funcOpenType;

    public Func<bool> specialShowRule;

    public bool showState;

    public override void Dispose()
    {
        UnBindObj();

        panelParent = null;
        panelObj = null;
        panelType = null;
        panel = null;
        specialShowRule = null;
        base.Dispose();
    }

    public void UnBindObj()
    {
        if (_redPoint != null) CSRedPointManager.Instance.Recycle(redPoint);
        _lb_name = null;
        _lb_nameInCheck = null;
        _obj_check = null;
        _redPoint = null;

        UIPrefabTrans = null;
        UIPrefab = null;
    }

    public void BindObj(GameObject go, string tabLabel, Transform _panelParent)
    {
        UnBindObj();
        UIPrefab = go;
        panelParent = _panelParent;

        lb_name.text = tabLabel;
        lb_nameInCheck.text = tabLabel;

        obj_check.SetActive(false);
        redPoint.SetActive(false);

        if (redPointTypes != null && redPointTypes.Length > 0)
            CSRedPointManager.Instance.RegisterRedPoint(redPoint, redPointTypes);
    }


    public void BindPanel(WelfareActivityType _type, Type _panelType, FunctionType _funcOpen,
        params RedPointType[] _pointType)
    {
        welfareType = _type;
        panelType = _panelType;
        redPointTypes = _pointType;
        funcOpenType = _funcOpen;

        specialShowRule = null;
        showState = false;
    }


    public void OnClick(bool active)
    {
        obj_check?.SetActive(active);
        if (active)
        {
            if (panel == null)
            {
                UIManager.Instance.CreatePanel(panelType, p =>
                {
                    UIBasePanel t_panel = p as UIBasePanel;
                    if (t_panel != null)
                    {
                        panel = t_panel;
                        panelObj = panel.UIPrefab;
                        panelObj.transform.SetParent(panelParent);
                        panelObj.transform.localPosition = Vector3.zero;
                        switch (welfareType)
                        {
                            case WelfareActivityType.Discount:
                                panel?.SelectChildPanel(UIWelfareActivityPanel.subType);
                                break;
                        }
                    }
                });
            }
            else
            {
                panelObj?.SetActive(true);
                switch (welfareType)
                {
                    case WelfareActivityType.Discount:
                        panel?.SelectChildPanel(UIWelfareActivityPanel.subType);
                        break;
                    default:
                        panel?.Show();
                        panel?.OnShow();
                        break;
                }
                
            }
        }
        else
        {
            if (panel != null)
            {
                panelObj?.SetActive(false);
                panel?.OnHide();
            }
        }
    }
}