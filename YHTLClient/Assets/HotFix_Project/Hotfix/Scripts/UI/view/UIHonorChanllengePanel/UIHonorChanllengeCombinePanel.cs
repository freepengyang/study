using System.Collections.Generic;
using UnityEngine;

public partial class UIHonorChanllengeCombinePanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    List<StaticPanelTabsItem> tabsList = new List<StaticPanelTabsItem>();
    List<StaticPanelChildren> childrenDic = new List<StaticPanelChildren>();
    StaticPanelTabsItem curTab;
    public UIHonorChanllengeCombinePanel()
    {
        childrenDic.Add(new StaticPanelChildren(typeof(UIHonorChanllengePanel), 0));

    }
    public override void Init()
    {
        base.Init();
        InitTabs();
        UIEventListener.Get(mbtn_close).onClick = CloseBtnClick;
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
            if (tabsList[i].panelDefine.panel != null)
            {
                UIManager.Instance.ClosePanel(tabsList[i].panelDefine.panel.GetType());
            }

            tabsList[i].Dispose();
        }
        base.OnDestroy();
    }
    void InitTabs()
    {
        tabsList.Clear();
        for (int i = 0; i < mgrid_tabsPar.transform.childCount; i++)
        {
            if (UICheckManager.Instance.DoCheckFunctionExtend(childrenDic[i].funcId))
            {
                mgrid_tabsPar.transform.GetChild(i).gameObject.SetActive(true);
                tabsList.Add(new StaticPanelTabsItem(mgrid_tabsPar.transform.GetChild(i).gameObject, mobj_view.transform.GetChild(i).gameObject, childrenDic[i], TabClick));
            }
            else
            {
                mgrid_tabsPar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        //for (int i = 0; i < tabsList.Count; i++)
        //{
        //    UIEventListener.Get(tabsList[i].go, tabsList[i]).onClick = TabClick;
        //}
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
        TabClick(tabsList[type - 1].go);
    }

    public override void SelectChildPanel(int type, int subType)
    {

    }


    void CloseBtnClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIHonorChanllengeCombinePanel>();
    }
}
