using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TABLE;
using UnityEngine;

public enum OpenActivityType
{
    None,
    ServerActivityRank = 1,//装备评分
    EquipCollection,//装备收集
    SealCompete,//封印比拼
    BOSSFirstKill,//boss首杀
    EquipRewards,//卧龙装备悬赏
    ServerActivityGift = 6,//开服礼包
    ChiefCarnival,//首领狂欢
    MonsterSlay,//屠魔活动
    Sabac,//沙巴克
    SevenLogin,//七日登录
	PerEquipRewards = 11,//普通装备悬赏
}

public partial class UIServerActivityPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }

    Dictionary<OpenActivityType, AcPanelTabsItem> tabsList = new Dictionary<OpenActivityType, AcPanelTabsItem>();
    Dictionary<OpenActivityType, AcPanelChildren> childrenDic = new Dictionary<OpenActivityType, AcPanelChildren>();
    AcPanelTabsItem curTab;

    public UIServerActivityPanel()
    {
        // childrenDic.Add(OpenActivityType.ServerActivityRank,
        //     new AcPanelChildren(typeof(UIServerActivityRankPanel), new List<int>() { 10101 }, RedPointType.ServerActivityRank,
        //         OpenActivityType.ServerActivityRank));
        childrenDic.Add(OpenActivityType.EquipCollection,
            new AcPanelChildren(typeof(UIServerActivityEquipPanel), new List<int>() { 10102 }, RedPointType.EquipCollect,
                OpenActivityType.EquipCollection));
        childrenDic.Add(OpenActivityType.SealCompete,
            new AcPanelChildren(typeof(UIServerActivitySealPanel), new List<int>() { 10103 }, RedPointType.SealComPetition,
                OpenActivityType.SealCompete));
        childrenDic.Add(OpenActivityType.BOSSFirstKill,
            new AcPanelChildren(typeof(UIServerActivityBossKillPanel), new List<int>() { 10104 }, RedPointType.BossFirstKill,
                OpenActivityType.BOSSFirstKill));
        childrenDic.Add(OpenActivityType.EquipRewards,
            new AcPanelChildren(typeof(UIServerActivityEquipRewardsPanel), new List<int>() { 10105 }, RedPointType.EquipRewards,
                OpenActivityType.EquipRewards));
        childrenDic.Add(OpenActivityType.ServerActivityGift,
            new AcPanelChildren(typeof(UIServerActivityGiftPanel), new List<int>() { 10106, 10107, 10108, 10109, 10110, 10111 }, RedPointType.None,
                OpenActivityType.ServerActivityGift));
        childrenDic.Add(OpenActivityType.ChiefCarnival,
            new AcPanelChildren(typeof(UIServerActivityBossPanel), new List<int>() { 10120 }, RedPointType.BossKuangHuan,
                OpenActivityType.ChiefCarnival));
        childrenDic.Add(OpenActivityType.MonsterSlay,
            new AcPanelChildren(typeof(UIServerActivitySlayPanel), new List<int>() { 10121 }, RedPointType.MonsterSlay,
                OpenActivityType.MonsterSlay));
        childrenDic.Add(OpenActivityType.Sabac,
            new AcPanelChildren(typeof(UIServerActivityGuildPanel), new List<int>() { 10122 }, RedPointType.None,
                OpenActivityType.Sabac));
        childrenDic.Add(OpenActivityType.SevenLogin,
            new AcPanelChildren(typeof(UISevenLoginPanel), new List<int>() { 10123 }, RedPointType.SevenLogin,
                OpenActivityType.SevenLogin));
		childrenDic.Add(OpenActivityType.PerEquipRewards,
			new AcPanelChildren(typeof(UIServerActivityEquipRewardsPanel), new List<int>() { 10124 }, RedPointType.PerEquipRewards,
				OpenActivityType.PerEquipRewards));
	}
    public override void Init()
    {
        base.Init();
        InitTabs();
        UIEventListener.Get(mbtn_close).onClick = CloseClick;
        SetMoneyIds(1, 4);
    }
    void InitTabs()
    {
        tabsList.Clear();
        CSBetterLisHot<OpenActivityType> temp_indList = new CSBetterLisHot<OpenActivityType>();
        var iter = childrenDic.GetEnumerator();
        while (iter.MoveNext())
        {
            if (CSOpenServerACInfo.Instance.GetOpenAcState(iter.Current.Value.funcId[0]))
            {
                temp_indList.Add(iter.Current.Value.acType);
            }
        }
        temp_indList.Sort((a, b) =>
        {
            CSBetterLisHot<SPECIALACTIVITY> tableA = SpecialActivityTableManager.Instance.GetTableDataByEventId((int)a);
            CSBetterLisHot<SPECIALACTIVITY> tableB = SpecialActivityTableManager.Instance.GetTableDataByEventId((int)b);
            if (tableA.Count > 0 && tableB.Count > 0)
            {
                return tableA[0].order - tableB[0].order;
            }
            return 0;
        });
        mgrid_tabs.MaxCount = temp_indList.Count;
        for (int i = 0; i < mgrid_tabs.MaxCount; i++)
        {
            //string name = SpecialActivityTableManager.Instance.GetSpecialActivityEventName((int)temp_indList[i]);
            string name = SpecialActivityTableManager.Instance.GetTabName((int)temp_indList[i]);
            GameObject temp_go = mgrid_tabs.controlList[i];
            temp_go.transform.Find("Label").GetComponent<UILabel>().text = name;
            temp_go.transform.Find("Checkmark/Label").GetComponent<UILabel>().text = name;
            tabsList.Add(temp_indList[i], new AcPanelTabsItem(mgrid_tabs.controlList[i], childrenDic[temp_indList[i]], mtrans_parent.gameObject));
            UIEventListener.Get(tabsList[temp_indList[i]].go, tabsList[temp_indList[i]]).onClick = TabsClick;
            RegisterRed(temp_go.transform.Find("red").gameObject, childrenDic[temp_indList[i]].redType);
        }
    }

    public void OpenPanel(OpenActivityType _type)
    {
        FNDebug.Log(tabsList.Count);
        TabsClick(tabsList[_type].go);
    }
	void TabsClick(GameObject _go)
    {
        AcPanelTabsItem tabsItem = (AcPanelTabsItem)UIEventListener.Get(_go).parameter;
        int funcid = tabsItem.panelDefine.funcId[0];
        int openLevel = SpecialActivityTableManager.Instance.GetSpecialActivityOpenLevel(funcid);
        if (openLevel <= CSMainPlayerInfo.Instance.Level)
        {
            if (curTab != null)
            {
                curTab.SetState(false);
            }

            curTab = tabsItem;
            curTab.SetState(true);
        }
        else
        {
            UtilityTips.ShowRedTips( CSString.Format(2031,openLevel));
        }



    }
    public override void Show()
    {
        base.Show();
        var iter = tabsList.GetEnumerator();
        while (iter.MoveNext())
        {
            TabsClick(tabsList[iter.Current.Key].go);
            return;
        }
    }
    void CloseClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIServerActivityPanel>();
    }
    void EnterBtnClick(GameObject _go)
    {

    }
    public override void SelectChildPanel(int type = 1)
    {
		OpenPanel((OpenActivityType)type);
	}

    public static int subType = 1;
    public override void SelectChildPanel(int type, int subtype)
    {
        subType = subtype;
        OpenPanel((OpenActivityType) type);
    }

    protected override void OnDestroy()
    {
        var iter = tabsList.GetEnumerator();
        while (iter.MoveNext())
        {
            if (iter.Current.Value.panelDefine.panel != null)
            {
                UIManager.Instance.ClosePanel(iter.Current.Value.panelDefine.panel.GetType());
            }
            iter.Current.Value.Dispose();
        }

        subType = 1;
        base.OnDestroy();
    }

    public static long GetEndTime(int _acId)
    {
        long gap = (SpecialActivityTableManager.Instance.GetSpecialActivityEventLast(_acId) - CSMainPlayerInfo.Instance.ServerOpenDay) * 24 * 60 * 60;
        long tim = CSServerTime.GetZeroClockGapSeconds();
        return gap + tim;
    }
}
public class AcPanelTabsItem : IDispose
{
    public GameObject go;
    public GameObject highLight;
    public AcPanelChildren panelDefine;
    GameObject parent;

    public AcPanelTabsItem(GameObject _go, AcPanelChildren _panel, GameObject _par)
    {
        go = _go;
        highLight = go.transform.Find("Checkmark").gameObject;
        panelDefine = _panel;
        parent = _par;
    }

    public void SetState(bool _state)
    {
        if (_state)
        {
            highLight.SetActive(true);
            if (panelDefine.panel == null)
            {
                UIManager.Instance.CreatePanel(panelDefine.paneltype, p =>
                {
                    UIBasePanel t_panel = p as UIBasePanel;
                    if (t_panel != null)
                    {
                        //t_panel.Init();
                        //t_panel.Show();
                        panelDefine.panel = t_panel;
						//FNDebug.Log("@@@@@@@@@@@t_panel:"+ t_panel);
                        panelDefine.panel.UIPrefab.transform.SetParent(parent.transform);
                        panelDefine.panel.UIPrefab.transform.localPosition = Vector3.zero;

                        switch (panelDefine.acType)
                        {
                            case OpenActivityType.ServerActivityGift:
                                (panelDefine.panel as UIServerActivityGiftPanel).SelectChildPanel(UIServerActivityPanel.subType);
                                break;
							case OpenActivityType.EquipRewards:
								(panelDefine.panel as UIServerActivityEquipRewardsPanel).SetItemClick(UIServerActivityPanel.subType);
								break;
							case OpenActivityType.PerEquipRewards:
								(panelDefine.panel as UIServerActivityEquipRewardsPanel).SetItemClick(UIServerActivityPanel.subType);
								break;
						}
                    }
                });
            }
            else
            {
                panelDefine.panel.UIPrefab.SetActive(true);
                panelDefine.panel.Show();
                panelDefine.panel.OnShow();
            }

            for (int i = 0; i < panelDefine.funcId.Count; i++)
            {
                Net.CSSpecialActiveDataMessage(panelDefine.funcId[i]);
            }

        }
        else
        {
            highLight.SetActive(false);
            if (panelDefine.panel != null)
            {
                panelDefine.panel.UIPrefab.SetActive(false);
                panelDefine.panel.OnHide();
            }
        }
    }

    public void Dispose()
    {
        if (panelDefine != null)
        {
            panelDefine.Dispose();
        }
    }
}

public class AcPanelChildren : IDispose
{
    public List<int> funcId;
    public Type paneltype;
    public UIBasePanel panel;
    public OpenActivityType acType;
    public RedPointType redType;

    public AcPanelChildren(Type _paneltype, List<int> _funcId, RedPointType _redType, OpenActivityType _acType = OpenActivityType.None)
    {
        paneltype = _paneltype;
        funcId = _funcId;
        redType = _redType;
        acType = _acType;
    }

    public void Dispose()
    {
        panel = null;
    }
}
