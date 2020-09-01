using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIAuctionPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    List<AuctionStaticPanelTabsItem> tabsList = new List<AuctionStaticPanelTabsItem>();
    List<AutionStaticPanelChildren> childrenDic = new List<AutionStaticPanelChildren>();
    public enum AutionPanelTye
    {
        buy = 1,
        sell = 2,
        show = 3,
    }
    public UIAuctionPanel()
    {
        childrenDic.Add(new AutionStaticPanelChildren(typeof(UIAuctionBuyPanel), AutionPanelTye.buy));
        childrenDic.Add(new AutionStaticPanelChildren(typeof(UIAuctionSellPanel), AutionPanelTye.sell));
        //childrenDic.Add(new AutionStaticPanelChildren(typeof(UIAuctionFocusBuyPanel), AutionPanelTye.show));
    }
    AuctionStaticPanelTabsItem curTab;
    public override void Init()
    {
        base.Init();
        UIEventListener.Get(mbtn_close).onClick = CloseBtnClick;
        InitTabs();
        RegisterRed(mred_sellRed,RedPointType.AuctionSell);
        SetMoneyIds(2, 3, 4);
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
    void InitTabs()
    {
        tabsList.Clear();
        for (int i = 0; i < mobj_tabs.transform.childCount; i++)
        {
            if (mobj_tabs.transform.GetChild(i).gameObject.activeSelf)
            {
                tabsList.Add(new AuctionStaticPanelTabsItem(mobj_tabs.transform.GetChild(i).gameObject, mobj_views.transform.GetChild(i).gameObject, childrenDic[i]));
            }
            //mobj_tabs.transform.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 0; i < tabsList.Count; i++)
        {
            UIEventListener.Get(tabsList[i].go, tabsList[i]).onClick = TabClick;
        }
    }
    void TabClick(GameObject _go)
    {
        AuctionStaticPanelTabsItem tabsItem = (AuctionStaticPanelTabsItem)UIEventListener.Get(_go).parameter;
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

    public override void SelectChildPanel(int type, params object[] obj)
    {
        TabClick(tabsList[type - 1].go);
        TipsBtnData _data = (TipsBtnData)obj[0];
        curTab.OpenItem(_data);
    }

    void CloseBtnClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIAuctionPanel>();
    }
}
public class AuctionStaticPanelTabsItem : IDispose
{
    public GameObject go;
    public GameObject highLight;
    public GameObject panelPrefab;
    public AutionStaticPanelChildren panelDefine;
    GameObject parent;

    public AuctionStaticPanelTabsItem(GameObject _go, GameObject _panelPrefab, AutionStaticPanelChildren _panel)
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
    public void OpenItem(TipsBtnData _data)
    {
        panelDefine.panel.SelectItem(_data);
    }
    public void Dispose()
    {
        if (panelDefine != null) { panelDefine.Dispose(); }
    }
}
public class AutionStaticPanelChildren : IDispose
{
    public int funcId;
    public Type paneltype;
    public UIAuctionPanel.AutionPanelTye type;
    public UIBasePanel panel;

    public AutionStaticPanelChildren(Type _paneltype, UIAuctionPanel.AutionPanelTye _pType, int _funcId = 0)
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

