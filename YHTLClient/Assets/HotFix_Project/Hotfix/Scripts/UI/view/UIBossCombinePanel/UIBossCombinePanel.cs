using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIBossCombinePanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    #region variable
    Dictionary<BossPanelTye, StaticPanelTabsItem> tabsList = new Dictionary<BossPanelTye, StaticPanelTabsItem>();
    Dictionary<BossPanelTye, StaticPanelChildren> childrenDic = new Dictionary<BossPanelTye, StaticPanelChildren>();
    StaticPanelTabsItem curTab;
    #endregion
    public enum BossPanelTye
    {
        wildBoss = 1,
        baozhuang = 2,
        personalBoss = 3,
        worldBoss = 4,
        lianTiBoss = 5,
    }
    public UIBossCombinePanel()
    {
        childrenDic.Add(BossPanelTye.wildBoss, new StaticPanelChildren(typeof(UIWildBossPanel), 42));
        childrenDic.Add(BossPanelTye.baozhuang, new StaticPanelChildren(typeof(UIEquipBossPanel), 56));
        childrenDic.Add(BossPanelTye.personalBoss, new StaticPanelChildren(typeof(UIPersonalBossPanel), 11));
        childrenDic.Add(BossPanelTye.worldBoss, new StaticPanelChildren(typeof(UIWolrdBossPanel), 14));
        childrenDic.Add(BossPanelTye.lianTiBoss, new StaticPanelChildren(typeof(UILianTiBossPanel), 16));
    }

    public override void Init()
    {
        base.Init();
        InitTabs();
        UIEventListener.Get(mbtn_close).onClick = CloseBtnClick;

        UICheckManager.Instance.RegBtnAndCheck(FunctionType.funcP_wildBoss, mgrid_tabsPar.transform.GetChild(0).gameObject);
        UICheckManager.Instance.RegBtnAndCheck(FunctionType.funcP_BaoZhuang, mgrid_tabsPar.transform.GetChild(1).gameObject);
        UICheckManager.Instance.RegBtnAndCheck(FunctionType.funP_personalBoss, mgrid_tabsPar.transform.GetChild(2).gameObject);
        UICheckManager.Instance.RegBtnAndCheck(FunctionType.funcP_worldBoss, mgrid_tabsPar.transform.GetChild(3).gameObject);
        UICheckManager.Instance.RegBtnAndCheck(FunctionType.funcP_lianTi, mgrid_tabsPar.transform.GetChild(4).gameObject);
        RegisterRedPoint();
        SetMoneyIds(1, 4);
    }
    void RegisterRedPoint()
    {
        RegisterRed(mobj_wildRedpoint, RedPointType.PersonalBoss);
    }
    public override void Show()
    {
        base.Show();
        //OpenPanel(BossPanelTye.wildBoss);
    }

    protected override void OnDestroy()
    {
        var iter = tabsList.GetEnumerator();
        while (iter.MoveNext())
        {
            iter.Current.Value.Dispose();
        }
        base.OnDestroy();
    }
    void InitTabs()
    {
        tabsList.Clear();
        List<BossPanelTye> temp_indList = new List<BossPanelTye>();
        temp_indList.Clear();
        var iter = childrenDic.GetEnumerator();
        while (iter.MoveNext())
        {
            if (iter.Current.Value.funcId == 0 || UICheckManager.Instance.DoCheckFunction(iter.Current.Value.funcId))
            {
                //Debug.Log("开放的是  " + iter.Current.Key);
                temp_indList.Add(iter.Current.Key);
            }
            else
            {
                mgrid_tabsPar.transform.GetChild((int)iter.Current.Key - 1).gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < temp_indList.Count; i++)
        {
            int index = (int)temp_indList[i] - 1;
            //Debug.Log("index   " + index);
            mgrid_tabsPar.transform.GetChild(index).gameObject.SetActive(true);
            tabsList.Add(temp_indList[i], new StaticPanelTabsItem(mgrid_tabsPar.transform.GetChild(i).gameObject, mobj_view.transform.GetChild(i).gameObject, childrenDic[temp_indList[i]], TabClick));
        }
        mgrid_tabsPar.Reposition();
    }
    void TabClick(GameObject _go)
    {
        StaticPanelTabsItem tabsItem = (StaticPanelTabsItem)UIEventListener.Get(_go).parameter;
        if (curTab != null)
        {
            curTab.SetState(false);
        }
        curTab = tabsItem;
        curTab.SetState(true);
    }
    public override void SelectChildPanel(int type = 1)
    {
        OpenPanel((BossPanelTye)type);
    }

    public override void SelectChildPanel(int type, int subType)
    {

    }
    void OpenPanel(BossPanelTye _type)
    {
        if (tabsList.ContainsKey(_type))
        {
            TabClick(tabsList[_type].go);
        }
        else
        {
            UtilityTips.ShowRedTips("功能未开放");
        }
    }

    void CloseBtnClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIBossCombinePanel>();
    }
}
public class StaticPanelTabsItem : IDispose
{
    public GameObject go;
    public GameObject highLight;
    public GameObject panelPrefab;
    public StaticPanelChildren panelDefine;
    GameObject parent;
    Action<GameObject> action;
    public StaticPanelTabsItem(GameObject _go, GameObject _panelPrefab, StaticPanelChildren _panel, Action<GameObject> _action)
    {
        go = _go;
        highLight = go.transform.Find("Background/Checkmark").gameObject;
        panelDefine = _panel;
        panelPrefab = _panelPrefab;
        action = _action;
        UIEventListener.Get(go, this).onClick = Click;
    }
    public void SetState(bool _state)
    {
        if (_state)
        {
            highLight.SetActive(true);
            if (panelDefine.panel == null)
            {
                panelDefine.panel = Activator.CreateInstance(panelDefine.paneltype) as UIBasePanel;
                panelDefine.panel.UIPrefab = panelPrefab;
                panelDefine.panel.Init();
            }
            panelDefine.panel.UIPrefab.SetActive(true);
            panelDefine.panel.Show();
            panelDefine.panel.OnShow();
            HotManager.Instance.EventHandler.SendEvent(CEvent.OpenPanel,panelDefine.panel);
        }
        else
        {
            highLight.SetActive(false);
            panelDefine.panel.UIPrefab.SetActive(false);
            panelDefine.panel.OnHide();
        }
    }
    void Click(GameObject _go)
    {
        if (action != null) { action(this.go); }
    }
    public void Dispose()
    {
        if (panelDefine != null) { panelDefine.Dispose(); }
    }
}
public class StaticPanelChildren : IDispose
{
    public int funcId;
    public Type paneltype;
    //public UIBossCombinePanel.BossPanelTye type;
    public UIBasePanel panel;

    public StaticPanelChildren(Type _paneltype, int _funcId)
    {
        paneltype = _paneltype;
        //type = _pType;
        funcId = _funcId;
    }
    public void Dispose()
    {
        if (panel != null) { panel.Destroy(); }
        panel = null;
    }
}
