using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UIEquipCombinePanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    private enum PanelTye
    {
        recast = 1,
        refine = 2,
        enhance = 3,
        stone = 4,
        combine = 5,
    }
    #region form
    GameObject _btn_Close;
    GameObject btn_Close { get { return _btn_Close ?? (_btn_Close = Get("center/events/btn_close").gameObject); } }
    GameObject _UIEquipRecast;
    GameObject UIEquipRecast { get { return _UIEquipRecast ?? (_UIEquipRecast = Get("UIEquipRecastPanel").gameObject); } }
    GameObject _UIEquipRefine;
    GameObject UIEquipRefine { get { return _UIEquipRefine ?? (_UIEquipRefine = Get("UIEquipRefinePanel").gameObject); } }

    GameObject _UIEquipEnhance;
    GameObject UIEquipEnhance { get { return _UIEquipEnhance ?? (_UIEquipEnhance = Get("UIEquipEnhancePanel").gameObject); } }

    GameObject _UIGemPanel;
    GameObject UIGemPanel { get { return _UIGemPanel ?? (_UIGemPanel = Get("UIGemPanel").gameObject); } }

    GameObject _UICompoundPanel;
    GameObject UICompoundPanel { get { return _UICompoundPanel ?? (_UICompoundPanel = Get("UICompoundPanel").gameObject); } }


    GameObject _tg_recst;
    GameObject tg_recast { get { return _tg_recst ?? (_tg_recst = Get("center/events/ToggleGroup/btn_recast/Background").gameObject); } }
    GameObject _obj_recst;
    GameObject obj_recast { get { return _obj_recst ?? (_obj_recst = Get("center/events/ToggleGroup/btn_recast/Background/Checkmark").gameObject); } }
    GameObject _tg_refine;
    GameObject tg_refine { get { return _tg_refine ?? (_tg_refine = Get("center/events/ToggleGroup/btn_refine/Background").gameObject); } }
    GameObject _obj_refine;
    GameObject obj_refine { get { return _obj_refine ?? (_obj_refine = Get("center/events/ToggleGroup/btn_refine/Background/Checkmark").gameObject); } }

    GameObject _btn_enhance;
    GameObject btn_enhance { get { return _btn_enhance ?? (_btn_enhance = Get("center/events/ToggleGroup/btn_enhance/Background").gameObject); } }
    GameObject _obj_enhance;
    GameObject obj_enhance { get { return _obj_enhance ?? (_obj_enhance = Get("center/events/ToggleGroup/btn_enhance/Background/Checkmark").gameObject); } }

    GameObject _btn_Stone;
    GameObject btn_Stone { get { return _btn_Stone ?? (_btn_Stone = Get("center/events/ToggleGroup/btn_Stone/Background").gameObject); } }

    GameObject _obj_Stone;
    GameObject obj_Stone { get { return _obj_Stone ?? (_obj_Stone = Get("center/events/ToggleGroup/btn_Stone/Background/Checkmark").gameObject); } }

    GameObject _btn_combine;
    GameObject btn_combine { get { return _btn_combine ?? (_btn_combine = Get("center/events/ToggleGroup/btn_combine/Background").gameObject); } }

    GameObject _obj_combine;
    GameObject obj_combine { get { return _obj_combine ?? (_obj_combine = Get("center/events/ToggleGroup/btn_combine/Background/Checkmark").gameObject); } }

    GameObject _obj_bg;
    GameObject obj_bg { get { return _obj_bg ?? (_obj_bg = Get("center/window/bgs/Sprite").gameObject); } }


    private GameObject _combine_redpoint;
    GameObject combine_redpoint
    {
        get { return _combine_redpoint ?? (_combine_redpoint = Get("center/events/ToggleGroup/btn_combine/redpoint").gameObject); }
    }

    private GameObject _gem_redpoint;
    GameObject Gem_redpoint
    {
        get { return _gem_redpoint ?? (_gem_redpoint = Get("center/events/ToggleGroup/btn_Stone/redpoint").gameObject); }
    }

    private GameObject _enhance_redpoint;
    GameObject enhance_redpoint
    {
        get { return _enhance_redpoint ?? (_enhance_redpoint = Get("center/events/ToggleGroup/btn_enhance/redpoint").gameObject); }
    }
    private GameObject _recast_redpoint;
    GameObject recast_redpoint
    {
        get { return _recast_redpoint ?? (_recast_redpoint = Get("center/events/ToggleGroup/btn_recast/redpoint").gameObject); }
    }
    private GameObject _refine_redpoint;
    GameObject refine_redpoint
    {
        get { return _refine_redpoint ?? (_refine_redpoint = Get("center/events/ToggleGroup/btn_refine/redpoint").gameObject); }
    }

    UIGrid _grid_tgGroup;
    UIGrid grid_tgGroup
    {
        get { return _grid_tgGroup ?? (_grid_tgGroup = Get<UIGrid>("center/events/ToggleGroup")); }
    }
    #endregion 

    #region variable
    UIEquipRecastPanel mUIEquipRecast;
    UIEquipRefinePanel mUIEquipRefine;
    UIEquipEnhancePanel mUIEquipEnhance;
    UIGemPanel mUIGemPanel;
    private UICompoundPanel mUICompoundPanel;
    private UIBasePanel curBasePanel;
    GameObject currentPanel;
    GameObject currentTab;
    PanelTye curPanelType;
    #endregion
    public override void Init()
    {
        base.Init();
        //AddCollider();
        mClientEvent.AddEvent(CEvent.FunctionOpenStateChange, FunctionOpenStateChange);
        mClientEvent.AddEvent(CEvent.FastAccessJumpToPanel, FastAccessJumpToPanel);

        UIEventListener.Get(btn_Close).onClick = CloseBtnClick;
        //UIEventListener.Get(obj_bg).onClick = CloseBtnClick;
        UIEventListener.Get(tg_recast, PanelTye.recast).onClick = TabClick;
        UIEventListener.Get(tg_refine, PanelTye.refine).onClick = TabClick;
        UIEventListener.Get(btn_enhance, PanelTye.enhance).onClick = TabClick;
        UIEventListener.Get(btn_Stone, PanelTye.stone).onClick = TabClick;
        UIEventListener.Get(btn_combine, PanelTye.combine).onClick = TabClick;

        FuncOpenBind();

        RegisterRed(combine_redpoint, RedPointType.Combine);
        RegisterRed(Gem_redpoint, RedPointType.Gem);
        RegisterRed(enhance_redpoint, RedPointType.EnhanceForge);
        RegisterRed(recast_redpoint, RedPointType.EquipRecast);
        RegisterRed(refine_redpoint, RedPointType.EquipRefine);
        SetMoneyIds(1, 4);

    }


    void FuncOpenBind()
    {
        UICheckManager.Instance.RegBtnAndCheck(FunctionType.funP_chongzhu, tg_recast.transform.parent.gameObject);
        UICheckManager.Instance.RegBtnAndCheck(FunctionType.funcP_xiLian, tg_refine.transform.parent.gameObject);
        UICheckManager.Instance.RegBtnAndCheck(FunctionType.funcP_enhance, btn_enhance.transform.parent.gameObject);
        UICheckManager.Instance.RegBtnAndCheck(FunctionType.funcP_baoshi, btn_Stone.transform.parent.gameObject);
        UICheckManager.Instance.RegBtnAndCheck(FunctionType.funcP_HeCheng, btn_combine.transform.parent.gameObject);
    }

    void FunctionOpenStateChange(uint id, object data)
    {
        if (grid_tgGroup != null) grid_tgGroup.Reposition();
    }
    void FastAccessJumpToPanel(uint id, object data)
    {
        int panelId = System.Convert.ToInt32(data);
        if (UtilityPanel.CheckGameModelPanelIsThis<UIEquipCombinePanel>(panelId))
        {
            return;
        }
        CloseBtnClick(btn_Close.gameObject);
    }

    public override void Show()
    {
        base.Show();

        FunctionOpenStateChange(0, null);

        UIEquipRecast.SetActive(curPanelType == PanelTye.recast);
        UIEquipRefine.SetActive(curPanelType == PanelTye.refine);
        UIEquipEnhance.SetActive(curPanelType == PanelTye.enhance);
        UIGemPanel.SetActive(curPanelType == PanelTye.stone);
        //OpenPanel(curPanelType);
    }
    protected override void OnDestroy()
    {
        if (mUIEquipRecast != null) { mUIEquipRecast.Destroy(); }
        if (mUIEquipRefine != null) { mUIEquipRefine.Destroy(); }
        if (mUIEquipEnhance != null) { mUIEquipEnhance.Destroy(); }
        if (mUIGemPanel != null) { mUIGemPanel.Destroy(); }
        if (mUICompoundPanel != null) { mUICompoundPanel.Destroy(); }
        base.OnDestroy();
    }

    void TabClick(GameObject _go)
    {
        var panelType = (PanelTye)UIEventListener.Get(_go).parameter;
        if (panelType == curPanelType) return;
        if (curPanelType == PanelTye.enhance && mUIEquipEnhance != null)
        {
            if (mUIEquipEnhance.CheckIsAutoEnhancing())
            {
                mUIEquipEnhance.TryToPauseAutoEnhance();
                UtilityTips.ShowPromptWordTips(46, mUIEquipEnhance.TryToResumeAutoEnhance, () => { OpenPanel(panelType); });
                return;
            }
        }

        OpenPanel(panelType);
    }


    public override void SelectChildPanel(int type = 1)
    {
        PanelTye panel = (PanelTye)type;

        switch (panel)
        {
            case PanelTye.refine:
                if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_xiLian)) { return; }
                break;
            case PanelTye.recast:
                if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funP_chongzhu)) { return; }
                break;
            case PanelTye.enhance:
                if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_enhance)) { return; }
                break;
            case PanelTye.stone:
                if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_baoshi)) { return; }
                break;
            case PanelTye.combine:
                if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_HeCheng)) { return; }
                break;
        }
        OpenPanel(panel);
    }

    public override void SelectChildPanel(int type, int subType)
    {
        SelectChildPanel(type);
        if (curBasePanel != null)
        {
            curBasePanel.SelectChildPanel(type, subType);
        }
    }

    public override void SelectItem(TipsBtnData _data)
    {
        if (curPanelType == PanelTye.recast)
        {
            if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funP_chongzhu)) { return; }
            if (mUIEquipRecast != null) { mUIEquipRecast.SelectItem(_data); }
        }
        else if (curPanelType == PanelTye.refine)
        {
            if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_xiLian)) { return; }
            if (mUIEquipRefine != null) { mUIEquipRefine.SelectItem(_data); }
        }
        else if (curPanelType == PanelTye.combine)
        {
            if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_HeCheng)) { return; }
            if (mUICompoundPanel != null) { mUICompoundPanel.ShowCompoundPanel(_data.info.configId); }
        }
    }

    void OpenPanel(PanelTye _type)
    {
        curPanelType = _type;

        //Debug.Log(_type.ToString());
        if (currentPanel != null) { currentPanel.SetActive(false); }
        if (currentTab != null) { currentTab.SetActive(false); }

        switch (_type)
        {
            case PanelTye.recast:
                if (mUIEquipRecast == null)
                {
                    mUIEquipRecast = new UIEquipRecastPanel();
                    mUIEquipRecast.SetGo(UIEquipRecast);
                    mUIEquipRecast.Init();
                }
                mUIEquipRecast.Show();
                currentPanel = UIEquipRecast;
                curBasePanel = mUIEquipRecast;
                currentTab = obj_recast;
                mClientEvent.SendEvent(CEvent.OpenPanel, mUIEquipRecast);
                break;
            case PanelTye.refine:

                if (mUIEquipRefine == null)
                {
                    mUIEquipRefine = new UIEquipRefinePanel();
                    mUIEquipRefine.SetGo(UIEquipRefine);
                    mUIEquipRefine.Init();
                }
                mUIEquipRefine.Show();
                currentPanel = UIEquipRefine;
                curBasePanel = mUIEquipRefine;
                currentTab = obj_refine;
                mClientEvent.SendEvent(CEvent.OpenPanel, mUIEquipRefine);
                break;
            case PanelTye.enhance:

                if (mUIEquipEnhance == null)
                {
                    mUIEquipEnhance = new UIEquipEnhancePanel();
                    mUIEquipEnhance.SetGo(UIEquipEnhance);
                    mUIEquipEnhance.Init();
                }
                mUIEquipEnhance.Show();
                currentPanel = UIEquipEnhance;
                curBasePanel = mUIEquipEnhance;
                currentTab = obj_enhance;
                mClientEvent.SendEvent(CEvent.OpenPanel, mUIEquipEnhance);
                break;
            case PanelTye.stone:
                //Debug.Log("stone");
                if (mUIGemPanel == null)
                {
                    mUIGemPanel = new UIGemPanel();
                    mUIGemPanel.SetGo(UIGemPanel);
                    mUIGemPanel.Init();
                }
                mUIGemPanel.Show();
                currentPanel = UIGemPanel;
                currentTab = obj_Stone;
                curBasePanel = mUIGemPanel;
                mClientEvent.SendEvent(CEvent.OpenPanel, UIGemPanel);
                break;
            case PanelTye.combine:
                if (mUICompoundPanel == null)
                {
                    mUICompoundPanel = new UICompoundPanel();
                    mUICompoundPanel.SetGo(UICompoundPanel);
                    mUICompoundPanel.Init();
                }
                mUICompoundPanel.Show();
                mUICompoundPanel.ShowCompoundPanel();
                mClientEvent.SendEvent(CEvent.OpenPanel, mUICompoundPanel);
                currentPanel = UICompoundPanel;
                currentTab = obj_combine;
                curBasePanel = mUICompoundPanel;
                break;
        }
        if (_type != PanelTye.recast && mUIEquipRecast != null)
        {
            mUIEquipRecast.OnHide();
        }
        currentPanel.SetActive(true);

        currentTab.SetActive(true);
    }

    void CloseBtnClick(GameObject _go)
    {
        if (curPanelType == PanelTye.enhance && mUIEquipEnhance != null)
        {
            if (mUIEquipEnhance.CheckIsAutoEnhancing())
            {
                mUIEquipEnhance.TryToPauseAutoEnhance();
                UtilityTips.ShowPromptWordTips(46, mUIEquipEnhance.TryToResumeAutoEnhance, DoClose);
                return;
            }
        }

        DoClose();
    }

    void RecastSpecialTaskOpen()
    {
        mUIEquipRecast.SpecialTaskOpenChoose(1);
    }
    void DoClose()
    {
        UIManager.Instance.ClosePanel<UIEquipCombinePanel>();
    }
    /// <summary>
    /// 任务特殊打开界面方法
    /// </summary>
    public static bool Link(string _str)
    {
        UIManager.Instance.CreatePanel<UIEquipCombinePanel>(p =>
        {
            (p as UIEquipCombinePanel).SelectChildPanel(1);
            (p as UIEquipCombinePanel).RecastSpecialTaskOpen();
        });
        return true;
    }
}
