﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UIDebugPanel : UIBase 
{
    public override bool ShowGaussianBlur { get { return false; } }
    private GameObject _content;
    private UIGridContainer _checkGroup;
    private UIScrollView _scroll;
    private GameObject _close;
    private GameObject _toHide;
    private UIPopupList _popList;

    private GameObject _goSearch;
    private GameObject _goBgAlpha;

    private GameObject _hide;
    private GameObject _toOpen;
    private GameObject _butRefresh;
    private UIToggle _checkDisableBox;
    private GameObject _hideMark;
    private GameObject _butClearCurPage;
    private GameObject _butClear;

    private UIDebugScrollServerMsg_Normal _scrollMsgNormal;
    private UIDebugScrollServerMsg_Frequency _scrollMsgFrequency;
    private UIDebugScrollExcetion _scrollExcetion;
    private UIDebugScrollNormalLog _scrollNormalLog;
    private UIDebug_Search _search;
    private UIDebug_AdjustBgAlpha _adjustBgAlpha;
    private UIDebug_FilterMsg _filterMsg;

    private GameObject content { get { return _content ? _content : (_content = Get<GameObject>("content")); } }
    private UIGridContainer checkGroup { get { return _checkGroup ? _checkGroup : (_checkGroup = Get<UIGridContainer>("content/checkGroup")); } }
    private UIScrollView scroll { get { return _scroll ? _scroll : (_scroll = Get<UIScrollView>("content/scroll")); } }
    private GameObject close { get { return _close ? _close : (_close = Get<GameObject>("content/close")); } }
    private GameObject toHide { get { return _toHide ? _toHide : (_toHide = Get<GameObject>("content/toHide")); } }
    private UIPopupList popList { get { return _popList ? _popList : (_popList = Get<UIPopupList>("content/typeSelect/popList")); } }
    private GameObject goSearch { get { return _goSearch ? _goSearch : (_goSearch = Get<GameObject>("content/1_search")); } }
    private GameObject goBgAlpha { get { return _goBgAlpha ? _goBgAlpha : (_goBgAlpha = Get<GameObject>("content/0_goBgAlpha")); } }
    private GameObject hide { get { return _hide ? _hide : (_hide = Get<GameObject>("hide")); } }
    private GameObject toOpen { get { return _toOpen ? _toOpen : (_toOpen = Get<GameObject>("hide/toOpen")); } }
    private GameObject hideMark { get { return _hideMark ? _hideMark : (_hideMark = Get<GameObject>("hide/toOpen/hideMark")); } }
    private GameObject butRefresh { get { return _butRefresh ? _butRefresh : (_butRefresh = Get<GameObject>("content/butRefresh")); } }
    private UIToggle checkDisableBox { get { return _checkDisableBox ? _checkDisableBox : (_checkDisableBox = Get<UIToggle>("content/checkDisableBox")); } }
    private GameObject butClearCurPage { get { return _butClearCurPage ? _butClearCurPage : (_butClearCurPage = Get<GameObject>("content/butClearCurPage")); } }
    private GameObject butClear { get { return _butClear ? _butClear : (_butClear = Get<GameObject>("content/butClear")); } }

    private List<UIDebugScrollBase> scrollBaseList = new List<UIDebugScrollBase>();

    private UIDebug_Search search { get { return _search ? _search : (_search = Get<UIDebug_Search>("content/typeSelect/1_search")); } }
    private UIDebug_AdjustBgAlpha adjustBgAlpha { get { return _adjustBgAlpha ? _adjustBgAlpha : (_adjustBgAlpha = Get<UIDebug_AdjustBgAlpha>("content/typeSelect/0_bgAlpha")); } }
    private UIDebug_FilterMsg filterMsg { get { return _filterMsg ? _filterMsg : (_filterMsg = Get<UIDebug_FilterMsg>("content/typeSelect/2_filterMsg")); } }


    public override UILayerType PanelLayerType { get { return UILayerType.Hint; } }
    public override void Init()
    {
        base.Init();
        content.SetActive(true);
        hide.SetActive(false);
        UIEventListener.Get(close).onClick = OnClickClose;
        UIEventListener.Get(toHide).onClick = OnClickHide;
        UIEventListener.Get(toOpen).onClick = OnClickHide;
        UIEventListener.Get(butRefresh).onClick = OnClickRefresh;
        UIEventListener.Get(butClearCurPage).onClick = OnClickClearCurPage;
        UIEventListener.Get(butClear).onClick = OnClickClearAll;
        checkDisableBox.value = UIDebugInfo.isCheckDisableBoxCollider;
        EventDelegate.Add(checkDisableBox.onChange, () => { OnCheckDisableChange(checkDisableBox.gameObject, checkDisableBox.value); });
        InitToggleAndScrollBase();
        RefreshToggle();
        RefreshPopList();

        RefreshPopInfo();
        RefreshScroll();
        HotManager.Instance.EventHandler.AddEvent(CEvent.UIDebugLogNotify,OnLogAddNotify);
        FNDebug.developerConsoleVisible = true;
    }

    void OnClickClearCurPage(GameObject go)
    {
        UIDebugScrollBase scroll = GetCurScrollBase();
        scroll.ClearData();
        scroll.Clear();
        scroll.IsNeedUpdateData = false;
        UIDebugInfo.Clear((int)scroll.togType);
    }

    void OnClickClearAll(GameObject go)
    {
        UIDebugInfo.Clear();
        for (int i = 0; i < scrollBaseList.Count; i++)
        {
            scrollBaseList[i].ClearData();
            scrollBaseList[i].IsNeedUpdateData = false;
        }
        UIDebugScrollBase scroll = GetCurScrollBase();
        scroll.Clear();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.UIDebugLogNotify);
        FNDebug.developerConsoleVisible = false;
    }

    void OnLogAddNotify(uint uiEvtID, object data)
    {
        if (data == null) return;
        int togType = (int)data;
        if (togType >= scrollBaseList.Count) return;
        UIDebugScrollBase scroll = scrollBaseList[togType];
        if (scroll == null) return;
        scroll.IsNeedUpdateData = true;
        if (!content.activeSelf)
        {
            if (togType == (int)ELogToggleType.Exception)
            {
                if (!hideMark.activeSelf)
                    hideMark.SetActive(true);
            }
        }
    }

    void OnCheckDisableChange(GameObject go, bool value)
    {
        if (UIDebugInfo.isCheckDisableBoxCollider != value)
        {
            UIDebugInfo.isCheckDisableBoxCollider = value;
            UIDebugScrollBase scroll = GetCurScrollBase();
            scroll.SetBoxColliderEnable(!value);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            object obj = null;
            obj.ToString();
        }
    }

    void OnClickRefresh(GameObject go)
    {
        RefreshScroll(true);
    }

    void OnClickClose(GameObject go)
    {
        UIManager.Instance.ClosePanel<UIDebugPanel>();
        CSDebug.IsOpenDebug = false;
    }

    void OnClickHide(GameObject go)
    {
        if (toHide == go)
        {
            content.SetActive(false);
            hide.SetActive(true);
        }
        else
        {
            content.SetActive(true);
            hide.SetActive(false);
            if (hideMark.activeSelf)
            {
                hideMark.SetActive(false);
                UIDebugInfo.selectTogIndex = 2;
                scrollBaseList[2].IsNeedUpdateData = false;
                RefreshToggle();
            }
            RefreshScroll(true);
        }
    }

    public void HideToLeft(bool b)
    {
        if (b)
        {
            content.SetActive(false);
            hide.SetActive(true);
        }
        else
        {
            content.SetActive(true);
            hide.SetActive(false);
            if (hideMark.activeSelf)
            {
                hideMark.SetActive(false);
                UIDebugInfo.selectTogIndex = 2;
                scrollBaseList[2].IsNeedUpdateData = false;
                RefreshToggle();
            }
            RefreshScroll(true);
        }
    }

    void InitToggleAndScrollBase()
    {
        checkGroup.MaxCount = UIDebugInfo.togNameList.Count;
        for (int i = 0; i < checkGroup.MaxCount; i++)
        {
            GameObject go = checkGroup.controlList[i];
            GameObject mark = go.transform.Find("mark").gameObject;
            UIDebugScrollBase scrollbase = null;
            if (i == 0) scrollbase = scroll.GetComponent<UIDebugScrollServerMsg_Normal>();
            if (i == 1) scrollbase = scroll.GetComponent<UIDebugScrollServerMsg_Frequency>();
            if (i == 2) scrollbase = scroll.GetComponent<UIDebugScrollExcetion>();
            if (i == 3) scrollbase = scroll.GetComponent<UIDebugScrollNormalLog>();
            scrollbase.togType = (ELogToggleType)i;
            if (scrollbase != null)
            {
                scrollbase.mark = mark;
                scrollBaseList.Add(scrollbase);
            }
        }
    }

    void RefreshToggle()
    {
        for (int i = 0; i < checkGroup.MaxCount; i++)
        {
            GameObject go = checkGroup.controlList[i];
            UILabel lab = go.transform.Find("Label").GetComponent<UILabel>();
            UIToggle tog = go.GetComponent<UIToggle>();
            EventDelegate.Add(tog.onChange, () => { OnTogChange(tog.gameObject, tog,tog.value); });
            lab.text = UIDebugInfo.togNameList[i];
            tog.value = i == UIDebugInfo.selectTogIndex;
        }
    }

    void RefreshPopList()
    {
        popList.Clear();
        for (int i = 0; i < UIDebugInfo.popNameList.Count;i++ )
        {
            popList.AddItem(UIDebugInfo.popNameList[i]);
        }
        if (UIDebugInfo.selectPopIndex < 0 || UIDebugInfo.selectPopIndex >= UIDebugInfo.popNameList.Count) UIDebugInfo.selectPopIndex = 0;
        popList.value = UIDebugInfo.popNameList[UIDebugInfo.selectPopIndex];
        EventDelegate.Add(popList.onChange, () => { onPopChange(popList.gameObject, popList.value); });
        
    }

    private void OnTogChange(GameObject go, UIToggle tog,bool value)
    {
        if (value)
        {
            UIDebugInfo.selectTogIndex = checkGroup.controlList.IndexOf(go);
            RefreshScroll();
        }
    }

    void RefreshScroll(bool isUpdateData = false)
    {
        if (isUpdateData)
        {
            for (int i = 0; i < scrollBaseList.Count; i++)
            {
                scrollBaseList[i].UpdateData();
                scrollBaseList[i].IsNeedUpdateData = false;
            }
        }
        UIDebugScrollBase scroll = GetCurScrollBase();
        scroll.RefreshScroll(isUpdateData);
    }

    UIDebugScrollBase GetCurScrollBase()
    {
        if (UIDebugInfo.selectTogIndex >= scrollBaseList.Count) return null;
        UIDebugScrollBase scroll = scrollBaseList[UIDebugInfo.selectTogIndex];
        return scroll;
    }

    private void onPopChange(GameObject go, string value)
    {
        UIDebugInfo.selectPopIndex = UIDebugInfo.popNameList.IndexOf(popList.value);
        RefreshPopInfo();
    }

    void RefreshPopInfo()
    {
        if (UIDebugInfo.selectPopIndex < 0 || UIDebugInfo.selectPopIndex >= UIDebugInfo.popNameList.Count) UIDebugInfo.selectPopIndex = 0;
        if (UIDebugInfo.selectPopIndex == 0)
        {
            filterMsg.gameObject.SetActive(false);
            search.gameObject.SetActive(false);
            adjustBgAlpha.gameObject.SetActive(true);
        }
        else if (UIDebugInfo.selectPopIndex == 1)
        {
            filterMsg.gameObject.SetActive(false);
            adjustBgAlpha.gameObject.SetActive(false);
            search.gameObject.SetActive(true);
        }
        else
        {
            adjustBgAlpha.gameObject.SetActive(false);
            search.gameObject.SetActive(false);
            filterMsg.gameObject.SetActive(true);
        }
        adjustBgAlpha.Show();
        search.Show();
        filterMsg.Show();
    }

    public override void Show()
    {
        base.Show();
    }

    public int Search(string str)
    {
        UIDebugScrollBase scroll = GetCurScrollBase();
        return scroll.Search(str);
    }
}
