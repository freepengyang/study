using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using energy;
using fight;
using FlyBirds.Model;

public class UIRoleHead : UIBase
{
    #region variable
    bool bubble1Show = false;
    bool bubble3Show = false;
    bool bubble4Show = false;
    bool isShowBubble = true;
    #endregion

    #region form
    private GameObject _btn_gm;
    private GameObject btn_gm
    {
        get { return _btn_gm ?? (_btn_gm = Get("btn_gm").gameObject); }
    }
    GameObject _obj_head;
    GameObject obj_head { get { return _obj_head ?? (_obj_head = Get("rolehead/headSprite/rolehead").gameObject); } }
    GameObject _obj_vigor;
    GameObject obj_vigor { get { return _obj_vigor ?? (_obj_vigor = Get("rolehead/btn_vim").gameObject); } }
    UISprite _sp_vigorPro;
    UISprite sp_vigorPos { get { return _sp_vigorPro ?? (_sp_vigorPro = Get<UISprite>("rolehead/btn_vim/Sprite/Panel/sp_vigor")); } }
    UISprite _sp_vigorPro2;
    UISprite sp_vigorPos2 { get { return _sp_vigorPro2 ?? (_sp_vigorPro2 = Get<UISprite>("rolehead/btn_vim/Sprite/Panel/sp_vigor2")); } }
    GameObject _sp_vigorPro1;
    GameObject sp_vigorPos1 { get { return _sp_vigorPro1 ?? (_sp_vigorPro1 = Get("rolehead/btn_vim/Sprite/Panel/sp_pro1").gameObject); } }
    GameObject _redP_vigor;
    GameObject redP_vigor { get { return _redP_vigor ?? (_redP_vigor = Get("rolehead/btn_vim/redpoint").gameObject); } }
    GameObject _obj_vigorBubble;
    GameObject obj_vigorBubble { get { return _obj_vigorBubble ?? (_obj_vigorBubble = Get("rolehead/btn_vim/fullBubble").gameObject); } }
    UILabel _lb_vigorDes;
    UILabel lb_vigorDes { get { return _lb_vigorDes ?? (_lb_vigorDes = Get("Label", obj_vigorBubble.transform).GetComponent<UILabel>()); } }
    GameObject _btn_vigorBubbleClose;
    GameObject btn_vigorBubbleClose { get { return _btn_vigorBubbleClose ?? (_btn_vigorBubbleClose = Get("rolehead/btn_vim/fullBubble/BubbleClose").gameObject); } }
    GameObject _obj_vigorLock;
    GameObject obj_vigorLock { get { return _obj_vigorLock ?? (_obj_vigorLock = Get("rolehead/btn_vim/Sprite/PanelEffect/sp_mask").gameObject); } }
    GameObject _obj_vigorLockEff;
    GameObject obj_vigorLockEff { get { return _obj_vigorLockEff ?? (_obj_vigorLockEff = Get("rolehead/btn_vim/Sprite/PanelEffect/effect").gameObject); } }

    //精力值功能解锁气泡
    GameObject _obj_vigorOpenBubble;
    GameObject obj_vigorOpenBubble { get { return _obj_vigorOpenBubble ?? (_obj_vigorOpenBubble = Get("rolehead/VigorOpenBubble/Bubble").gameObject); } }
    UILabel _lb_vigorOpen;
    UILabel lb_vigorOpen { get { return _lb_vigorOpen ?? (_lb_vigorOpen = Get("Label", obj_vigorOpenBubble.transform).GetComponent<UILabel>()); } }

    #endregion


    public override void Init()
    {
        base.Init();
        InitGM();
        UIEventListener.Get(obj_head).onClick = OpenRolePanel;
        InitVigor();
    }
    private void InitGM()
    {
#if UNITY_EDITOR
        btn_gm.SetActive(true);
        UIEventListener.Get(btn_gm).onClick = OnOpenGMClick;
#else
        btn_gm.SetActive(false);
#endif
    }
    public override void Show()
    {
        base.Show();
    }
    protected override void OnDestroy()
    {
        if (timer != null)
        {
            CSTimer.Instance.remove_timer(timer);
            timer = null;
        }
        if (timer2 != null)
        {
            CSTimer.Instance.remove_timer(timer2);
            timer2 = null;
        }
        CSEffectPlayMgr.Instance.Recycle(obj_vigorLockEff);
        CSRedPointManager.Instance.Recycle(redP_vigor);
        CSEffectPlayMgr.Instance.Recycle(sp_vigorPos.gameObject);
        base.OnDestroy();
    }
    public void OnOpenGMClick(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIGMMenuPanel>();
    }
    void OpenRolePanel(GameObject _go)
    {
        UIManager.Instance.CreatePanel<UIRolePanel>(p =>
        {
            (p as UIRolePanel).ShowRolePanel();
        });
    }

    #region  精力值
    void OpenVigorClick(GameObject _go)
    {
        if (UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_vigor))
        {
            UIManager.Instance.CreatePanel<UIVigorPanel>();
        }
        else
        {
            UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.VigorOpen);
        }
    }
    void CloseVigorBubble(GameObject _go)
    {
        isShowBubble = false;
        obj_vigorBubble.SetActive(false);
    }
    void CloseVigorOpenBubble(GameObject _go)
    {
        obj_vigorOpenBubble.SetActive(false);
    }
    void InitVigor()
    {
        //UICheckManager.Instance.RegBtnAndCheck(FunctionType.funcP_vigor, obj_vigor);
        UIEventListener.Get(obj_vigor).onClick = OpenVigorClick;
        CSEffectPlayMgr.Instance.ShowUIEffect(sp_vigorPos.gameObject, 17051);
        CSEffectPlayMgr.Instance.ShowUIEffect(sp_vigorPos1.gameObject, 17052);
        mClientEvent.AddEvent(CEvent.SCEnergyInfoMessage, SCEnergyInfoMessage);
        mClientEvent.AddEvent(CEvent.SCEnergyFreeGetInfoMessage, SCEnergyInfoMessage);
        mClientEvent.AddEvent(CEvent.SCGetFreeEnergyMessage, SCEnergyInfoMessage);
        mClientEvent.AddEvent(CEvent.SCNotifyEnergyChangeMessage, SCEnergyInfoMessage);
        mClientEvent.AddEvent(CEvent.SCEnergyExchangeInfoMessage, SCEnergyInfoMessage);
        mClientEvent.AddEvent(CEvent.Buff_Add, GetBuffChange);
        mClientEvent.AddEvent(CEvent.Buff_Remove, GetBuffChange);
        mClientEvent.AddEvent(CEvent.OnNewFunctionPlayOver, OnNewFunctionPlayOver);

        UIEventListener.Get(btn_vigorBubbleClose).onClick = CloseVigorBubble;
        UIEventListener.Get(obj_vigorOpenBubble).onClick = CloseVigorOpenBubble;
        obj_vigorLock.SetActive(!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_vigor));
        RefreshVigor();
        ChangeBubbleState();
        ShowBubble();
        CSRedPointManager.Instance.RegisterRedPoint(redP_vigor, RedPointType.Vigor);
    }
    void SCEnergyInfoMessage(uint id, object data)
    {
        RefreshVigor();
        ChangeBubbleState();
        ShowBubble();
    }
    void GetBuffChange(uint id, object data)
    {
        BufferChanged info = (BufferChanged)data;
        if (info.buffer.bufferId != buffid)
        {
            return;
        }
        ChangeBubbleState();
        ShowBubble();
    }

    void OnNewFunctionPlayOver(uint id, object data)
    {
        TABLE.FUNCOPEN mFuncItem = (TABLE.FUNCOPEN)data;
        if (mFuncItem.functionId == (int)FunctionType.funcP_vigor)
        {
            CSEffectPlayMgr.Instance.ShowUIEffect(obj_vigorLockEff, 17937, 8);
            timer = CSTimer.Instance.Invoke(0.5f, DelayCloseMask);
            timer2 = CSTimer.Instance.Invoke(2f, DelayOpenBubble);
        }
    }
    TimerEventHandle timer;
    TimerEventHandle timer2;
    void DelayCloseMask()
    {
        CSTimer.Instance.remove_timer(timer);
        obj_vigorLock.SetActive(false);
    }
    void DelayOpenBubble()
    {
        CSTimer.Instance.remove_timer(timer2);
        lb_vigorOpen.text = ClientTipsTableManager.Instance.GetClientTipsContext(2045);
        obj_vigorOpenBubble.SetActive(true);
    }
    int maxCount = 0;
    void RefreshVigor()
    {
        //Debug.Log($"{CSVigorInfo.Instance.GetCurrentVigorExp()}  {CSVigorInfo.Instance.GetMaxChangeCount()}");
        maxCount = CSVigorInfo.Instance.GetMaxChangeCount();
        if (maxCount > 0)
        {
            sp_vigorPos2.fillAmount = sp_vigorPos.fillAmount = (float)(CSVigorInfo.Instance.GetCurrentVigorExp() * 5 / (maxCount));
        }
        else
        {
            sp_vigorPos2.fillAmount = sp_vigorPos.fillAmount = 0;
        }
        if (0.05 <= sp_vigorPos.fillAmount && sp_vigorPos.fillAmount <= 0.95)
        {
            sp_vigorPos1.gameObject.SetActive(true);
            sp_vigorPos1.transform.localPosition = new Vector3(-0, (-29 + 66 * sp_vigorPos.fillAmount), 0);
        }
        else
        {
            sp_vigorPos1.gameObject.SetActive(false);
        }
    }
    Dictionary<int, TimerState> freeIds = new Dictionary<int, TimerState>();
    double curVigorValue = 0;
    fight.BufferInfo buffinfo;
    WaitForSeconds wait;
    Coroutine cor;
    int buffid = 210019;
    bool isAdd = false;
    void ChangeBubbleState()
    {
        //bubble1 有可领取就显示    bubble3精力值超过上限30%就一直显示   显示顺序3>1
        //state 1:不可领,2:可以领,3:已经领取
        bubble1Show = false;
        bubble3Show = false;
        bubble4Show = false;
        buffinfo = CSMainPlayerInfo.Instance.BuffInfo.GetBuff(buffid);
        bubble4Show = (buffinfo != null) ? true : false;
        if (curVigorValue != CSVigorInfo.Instance.GetCurrentVigorExp())
        {
            isAdd = CSVigorInfo.Instance.GetCurrentVigorExp() > curVigorValue;
            curVigorValue = CSVigorInfo.Instance.GetCurrentVigorExp();
            if (isShowBubble && curVigorValue >= CSVigorInfo.Instance.GetMaxChangeCount() * 0.3)
            {
                bubble3Show = true;
                return;
            }
        }
        CSVigorInfo.Instance.GetOpenIds(out freeIds);
        var iter = freeIds.GetEnumerator();
        while (iter.MoveNext())
        {
            if (iter.Current.Value.state == 2)
            {
                bubble1Show = true;
                return;
            }
        }
    }
    void ShowBubble()
    {

        if (bubble4Show)
        {
            obj_vigorBubble.SetActive(false);
            if (cor != null) { ScriptBinder.StopCoroutine(cor); }
            return;
        }
        if (bubble3Show)
        {
            obj_vigorBubble.SetActive(true);
            lb_vigorDes.text = ClientTipsTableManager.Instance.GetClientTipsContext(1840);
            if (cor != null) { ScriptBinder.StopCoroutine(cor); }
        }
        else
        {
            if (bubble1Show)
            {
                lb_vigorDes.text = ClientTipsTableManager.Instance.GetClientTipsContext(1839);
                obj_vigorBubble.SetActive(true);
                if (cor != null) { ScriptBinder.StopCoroutine(cor); }
            }
            else
            {

                if (cor != null) { ScriptBinder.StopCoroutine(cor); }
                obj_vigorBubble.SetActive(false);
            }
        }
    }
    IEnumerator DelayShowBubble()
    {
        if (wait == null) { wait = new WaitForSeconds(8f); }
        yield return wait;
        obj_vigorBubble.SetActive(false);
    }
    #endregion

}