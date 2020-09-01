using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIWoLongXiLianCombinePanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    #region variable
    List<WolongXLStaticPanelTabsItem> tabsList = new List<WolongXLStaticPanelTabsItem>();
    List<WolongXLStaticPanelChildren> childrenDic = new List<WolongXLStaticPanelChildren>();
    WolongXLStaticPanelTabsItem curTab;
    #endregion
    public enum WolongXLPanelTye
    {
        longli = 1,
        longji = 2,
    }
    public UIWoLongXiLianCombinePanel()
    {
        childrenDic.Add(new WolongXLStaticPanelChildren(typeof(UIHunLianPanel), WolongXLPanelTye.longli, 0));
        childrenDic.Add(new WolongXLStaticPanelChildren(typeof(UILongLiPanel), WolongXLPanelTye.longji, 0));
    }
    public override void Init()
    {
        base.Init();
        InitTabs();
        RegisterRedPoint();
        UIEventListener.Get(mbtn_close).onClick = CloseBtnClick;
        SetMoneyIds(1, 4);
    }

    public override void Show()
    {
        base.Show();
        TabClick(tabsList[0].go);
    }
    protected override void OnDestroy()
    {
        for (int i = 0; i < tabsList.Count; i++)
        {
            tabsList[i].Dispose();
        }
        base.OnDestroy();
    }
    void RegisterRedPoint()
    {
        RegisterRed(mlongJi_red, RedPointType.LongJiRefine); 
        RegisterRed(mlongli_red, RedPointType.LongLiRefine); 
    }
    void InitTabs()
    {
        tabsList.Clear();
        for (int i = 0; i < mgrid_tabsPar.transform.childCount; i++)
        {
            if (childrenDic[i].funcId != 0)
            {
                if (UICheckManager.Instance.DoCheckFunction((FunctionType)childrenDic[i].funcId))
                {
                    mgrid_tabsPar.transform.GetChild(i).gameObject.SetActive(true);
                    tabsList.Add(new WolongXLStaticPanelTabsItem(mgrid_tabsPar.transform.GetChild(i).gameObject, mobj_view.transform.GetChild(i).gameObject, childrenDic[i]));
                }
                else
                {
                    mgrid_tabsPar.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            else
            {
                mgrid_tabsPar.transform.GetChild(i).gameObject.SetActive(true);
                tabsList.Add(new WolongXLStaticPanelTabsItem(mgrid_tabsPar.transform.GetChild(i).gameObject, mobj_view.transform.GetChild(i).gameObject, childrenDic[i]));
            }
        }
        for (int i = 0; i < tabsList.Count; i++)
        {
            UIEventListener.Get(tabsList[i].go, tabsList[i]).onClick = TabClick;
        }
        mgrid_tabsPar.Reposition();
    }
    void TabClick(GameObject _go)
    {
        WolongXLStaticPanelTabsItem tabsItem = (WolongXLStaticPanelTabsItem)UIEventListener.Get(_go).parameter;
        if (curTab != null)
        {
            curTab.SetState(false);
        }
        curTab = tabsItem;
        curTab.SetState(true);
    }
    public override void SelectChildPanel(int type = 1)
    {
        TabClick(tabsList[type - 1].go);
    }

    public override void SelectChildPanel(int type, int subType)
    {

    }
    public override void SelectItem(TipsBtnData _data)
    {
        if (curTab != null)
        {
            curTab.SelectItem(_data);
        }
    }
    void CloseBtnClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIWoLongXiLianCombinePanel>();
    }
}
public class WolongXLStaticPanelTabsItem : IDispose
{
    public GameObject go;
    public GameObject highLight;
    public GameObject panelPrefab;
    public WolongXLStaticPanelChildren panelDefine;
    GameObject parent;

    public WolongXLStaticPanelTabsItem(GameObject _go, GameObject _panelPrefab, WolongXLStaticPanelChildren _panel)
    {
        go = _go;
        highLight = go.transform.Find("Background/Checkmark").gameObject;
        panelDefine = _panel;
        panelPrefab = _panelPrefab;
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
        }
        else
        {
            highLight.SetActive(false);
            panelDefine.panel.UIPrefab.SetActive(false);
            panelDefine.panel.OnHide();
        }
    }
    public void SelectItem(TipsBtnData _data)
    {
        panelDefine.panel.SelectItem(_data);
    }

    public void Dispose()
    {
        if (panelDefine != null) { panelDefine.Dispose(); }
    }
}
public class WolongXLStaticPanelChildren : IDispose
{
    public int funcId;
    public Type paneltype;
    public UIWoLongXiLianCombinePanel.WolongXLPanelTye type;
    public UIBasePanel panel;

    public WolongXLStaticPanelChildren(Type _paneltype, UIWoLongXiLianCombinePanel.WolongXLPanelTye _pType, int _funcId)
    {
        paneltype = _paneltype;
        type = _pType;
        funcId = _funcId;
    }
    public void Dispose()
    {
        if (panel != null) { panel.Destroy(); }
        panel = null;
    }
}
