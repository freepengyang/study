using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using user;

public partial class UIRolePanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }

    public enum PanelTye
    {
        role = 1,
        wolong = 2,
        Fashion = 3,
        lianti = 4, //炼体zk
    }

    public static int subType = 1;

    int[] tabsFuncOpenIds = { 0, 13, 25, 16 }; //页签的功能开放ID

    #region forms

    private GameObject _obj_closebtn;

    private GameObject obj_closebtn
    {
        get { return _obj_closebtn ?? (_obj_closebtn = Get("center/events/btn_close").gameObject); }
    }

    UIGrid _grid_tabsPar;

    UIGrid grid_tabsPar
    {
        get { return _grid_tabsPar ?? (_grid_tabsPar = Get<UIGrid>("center/events/grid")); }
    }

    GameObject _obj_view;

    GameObject obj_view
    {
        get { return _obj_view ?? (_obj_view = Get("center/view").gameObject); }
    }

    private GameObject _lianTiRedPointObj;

    private GameObject lianTiRedPointObj
    {
        get
        {
            return _lianTiRedPointObj ??
                   (_lianTiRedPointObj = Get("center/events/grid/btn_lianti/redpoint").gameObject);
        }
    }

    private GameObject _FashionRedPointObj;

    private GameObject FashionRedPointObj
    {
        get
        {
            return _FashionRedPointObj ??
                   (_FashionRedPointObj = Get("center/events/grid/btn_fashion/redpoint").gameObject);
        }
    }

    private GameObject _wlUpgradeRedPointObj;

    private GameObject wlUpgradePointObj
    {
        get
        {
            return _wlUpgradeRedPointObj ??
                   (_wlUpgradeRedPointObj = Get("center/events/grid/btn_wolong/redpoint").gameObject);
        }
    }

    #endregion

    #region variables

    Dictionary<PanelTye, PanelTabsItem> tabsList = new Dictionary<PanelTye, PanelTabsItem>();
    Dictionary<PanelTye, PanelChildren> childrenDic = new Dictionary<PanelTye, PanelChildren>();
    PanelTabsItem curTab;

    #endregion

    public UIRolePanel()
    {
        childrenDic.Add(PanelTye.role, new PanelChildren(typeof(UIRoleAttrInfoPanel), 0, typeTab: 1));
        childrenDic.Add(PanelTye.wolong, new PanelChildren(typeof(UIRoleDragonPanel), 13, typeTab: 2));
        childrenDic.Add(PanelTye.Fashion, new PanelChildren(typeof(UIFashionPanel), 25, typeTab: 3));
        childrenDic.Add(PanelTye.lianti, new PanelChildren(typeof(UILianTiPanel), 16, typeTab: 4));
    }

    public override void Init()
    {
        base.Init();
        AddCollider();
        mClientEvent.Reg((uint)CEvent.ChangeEquipShow, GetChangeEquipShow);
        InitTabs();
        RegisterRedPoint();
        UIEventListener.Get(obj_closebtn).onClick = p => { UIManager.Instance.ClosePanel<UIRolePanel>(); };
        SetMoneyIds(1, 4);
    }

    private void RegisterRedPoint()
    {
        RegisterRed(wlUpgradePointObj, RedPointType.WolongUpGrade); //卧龙升级红点
        RegisterRed(lianTiRedPointObj, RedPointType.LianTi); //炼体小红点
        RegisterRed(FashionRedPointObj, RedPointType.Fashion); //时装小红点
    }

    void InitTabs()
    {
        tabsList.Clear();
        List<PanelTye> temp_indList = new List<PanelTye>();
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
                grid_tabsPar.transform.GetChild((int)iter.Current.Key - 1).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < temp_indList.Count; i++)
        {
            int index = (int)temp_indList[i] - 1;
            //Debug.Log("index   " + index);
            // if (index!=2)//暂时屏蔽衣橱,等资源到位改回来
            // {
            grid_tabsPar.transform.GetChild(index).gameObject.SetActive(true);
            tabsList.Add(temp_indList[i],
                new PanelTabsItem(grid_tabsPar.transform.GetChild(index).gameObject, childrenDic[temp_indList[i]],
                    obj_view, TabsClick));
            // }
            // else
            // {
            //     grid_tabsPar.transform.GetChild(index).gameObject.SetActive(false);
            // }
        }

        grid_tabsPar.Reposition();
    }

    void GetChangeEquipShow(uint id, object data)
    {
        UIRoleEquipPanel.equipType etype = (UIRoleEquipPanel.equipType)data;
        //Debug.Log("收到 切换消息   " + etype);
        if (etype == UIRoleEquipPanel.equipType.Normal)
        {
            OpenPanel(PanelTye.wolong);
        }
        else if (etype == UIRoleEquipPanel.equipType.WoLong)
        {
            OpenPanel(PanelTye.role);
        }
    }

    public override void Show()
    {
        base.Show();
        // OpenPanel(PanelTye.role);
    }

    public void ShowRolePanel(PanelTye type = PanelTye.role)
    {
        OpenPanel(type);
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

    public override void SelectChildPanel(int type = 1)
    {
        OpenPanel((PanelTye)type);
    }

    public override void SelectChildPanel(int type, int subtype)
    {
        subType = subtype;
        OpenPanel((PanelTye)type);
    }

    void TabsClick(GameObject _go)
    {
        PanelTabsItem tabsItem = (PanelTabsItem)UIEventListener.Get(_go).parameter;
        if (curTab != null)
        {
            curTab.SetState(false);
        }

        curTab = tabsItem;
        curTab.SetState(true);
    }

    void OpenPanel(PanelTye _type)
    {
        if (tabsList.ContainsKey(_type))
        {
            TabsClick(tabsList[_type].go);
        }
        else
        {
            if (_type == PanelTye.wolong)
            {
                UtilityTips.ShowTips(107, 1.5f, ColorType.Red, FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(13), "!");
            }
            else
            {
                UtilityTips.ShowRedTips("功能未开放");
            }
        }
    }
}

public class PanelTabsItem : IDispose
{
    public GameObject go;
    public GameObject highLight;
    public PanelChildren panelDefine;
    GameObject parent;
    Action<GameObject> action;

    public PanelTabsItem(GameObject _go, PanelChildren _panel, GameObject _par, Action<GameObject> _action)
    {
        go = _go;
        highLight = go.transform.Find("Background/Checkmark").gameObject;
        panelDefine = _panel;
        parent = _par;
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
                UIManager.Instance.CreatePanel(panelDefine.paneltype, p =>
                {
                    UIBasePanel t_panel = p as UIBasePanel;
                    if (t_panel != null)
                    {
                        t_panel.Init();
                        t_panel.Show();
                        panelDefine.panel = t_panel;
                        panelDefine.panel.UIPrefab.transform.SetParent(parent.transform);
                        panelDefine.panel.UIPrefab.transform.localPosition = Vector3.zero;

                        switch (panelDefine.type)
                        {
                            case (int)UIRolePanel.PanelTye.Fashion:
                                (panelDefine.panel as UIFashionPanel).SelectChildPanel(UIRolePanel.subType);
                                break;
                        }
                    }
                });
            }
            else
            {
                if (panelDefine.panel != null)
                {
                    panelDefine.panel.UIPrefab.SetActive(true);
                    panelDefine.panel.Show();
                    panelDefine.panel.OnShow();
                }
            }
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
        if (action != null)
        {
            action(this.go);
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

public class PanelChildren : IDispose
{
    public int funcId;

    public Type paneltype;

    //public UIRolePanel.PanelTye type;
    public UIBasePanel panel;
    public int funcType;

    public int type;

    public PanelChildren(Type _paneltype, int _funcId, int _funcType = 1, int typeTab = 1)
    {
        paneltype = _paneltype;
        funcId = _funcId;
        funcType = _funcType;
        type = typeTab;
    }

    public void Dispose()
    {
        panel = null;
    }
}