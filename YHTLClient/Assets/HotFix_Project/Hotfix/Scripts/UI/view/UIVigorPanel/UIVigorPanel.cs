using energy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIVigorPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    #region
    Dictionary<int, TimerState> freeIds = new Dictionary<int, TimerState>();
    List<FreeVigorItem> freeItems = new List<FreeVigorItem>();
    List<ExChangeVigor> exchangeItems = new List<ExChangeVigor>();
    int exNum = 0;
    double curvalue = 0;
    int vigorDanId = 610;
    int vigorBuffId = 0;
    int vigorMultiples = 0;
    TABLE.ITEM vigorDanCfg;
    fight.BufferInfo buffinfo;
    WaitForSeconds wait;
    int freeTimerCount = 1;
    ILBetterList<GetWayData> getWayList = new ILBetterList<GetWayData>();
    #endregion
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.SCEnergyInfoMessage, SCEnergyInfoMessage);
        mClientEvent.AddEvent(CEvent.SCEnergyFreeGetInfoMessage, SCEnergyFreeGetInfoMessage);
        mClientEvent.AddEvent(CEvent.SCGetFreeEnergyMessage, SCGetFreeEnergyMessage);
        mClientEvent.AddEvent(CEvent.SCNotifyEnergyChangeMessage, SCNotifyEnergyChangeMessage);
        mClientEvent.AddEvent(CEvent.SCEnergyExchangeInfoMessage, SCEnergyExchangeInfoMessage);
        mClientEvent.AddEvent(CEvent.MoneyChange, GetMoneyChange);
        mClientEvent.AddEvent(CEvent.FastAccessJumpToPanel, GetFastAccessClose);
        mClientEvent.AddEvent(CEvent.Buff_Add, GetBuffChange);
        mClientEvent.AddEvent(CEvent.Buff_Remove, GetBuffChange);


        base.Init();
        UIEventListener.Get(mbtn_introduce).onClick = HelpBtnClick;
        UIEventListener.Get(mbtn_close).onClick = CloseBtnClick;
        SetMoneyIds(1, 4);
        freeItems.Add(new FreeVigorItem(mobj_free1));
        exchangeItems.Add(new ExChangeVigor(mobj_exchange));
        CSEffectPlayMgr.Instance.ShowUITexture(mobj_bg2, "vigorglass");
        CSEffectPlayMgr.Instance.ShowUIEffect(mlb_pro.gameObject, "effect_vigor_ball_normal");
        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_lb1.gameObject, "effect_vigor_level_normal");
        CSEffectPlayMgr.Instance.ShowUITexture(mobj_tex1.gameObject, "vigor_circle");
        CSEffectPlayMgr.Instance.ShowUITexture(mobj_tex2.gameObject, "vigor_label_bg");

        wait = new WaitForSeconds(1f);
        vigorDanCfg = ItemTableManager.Instance.GetItemCfg(vigorDanId);
        List<int> idList = UtilityMainMath.SplitStringToIntList(vigorDanCfg.bufferParam);
        if (idList.Count > 0)
        {
            vigorBuffId = idList[0];
        }
        msp_buffIcon.spriteName = BufferTableManager.Instance.GetBufferIcon(vigorBuffId);
        vigorMultiples = int.Parse(SundryTableManager.Instance.GetSundryEffect(95));
        mlb_des2.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1779), EnergyStorageTableManager.Instance.GetEnergyStorageEnergyExp(CSMainPlayerInfo.Instance.Level));
        RefreshVigorBuff();
        UIEventListener.Get(mlb_buff.gameObject).onClick = VigorBuffClick;
        RegisterRed(mobj_red, RedPointType.Vigor);
    }
    public override void Show()
    {
        Net.CSEnergyInfoMessage();
        Net.CSEnergyFreeGetInfoMessage();
        Net.CSGetEnergeExchangeInfoMessage();
        base.Show();
        TimerTableManager.Instance.GetTimerDesc(1);

        getWayList.Clear();
        CSGetWayInfo.Instance.GetGetWays(ItemTableManager.Instance.GetItemGetWay(12), ref getWayList);
        mgrid_getway.Bind<GetWayData, GetWayBtn>(getWayList, mPoolHandleManager);
    }

    protected override void OnDestroy()
    {
        wait = null;
        mgrid_getway.UnBind<GetWayBtn>();
        CSEffectPlayMgr.Instance.Recycle(mobj_bg);
        CSEffectPlayMgr.Instance.Recycle(mobj_bg2);
        CSEffectPlayMgr.Instance.Recycle(mlb_pro.gameObject);
        CSEffectPlayMgr.Instance.Recycle(mobj_tex1.gameObject);
        CSEffectPlayMgr.Instance.Recycle(mobj_tex2.gameObject);
        base.OnDestroy();
    }
    #region 精力丹
    Coroutine buffCountCor;
    long remainingTime;
    void RefreshVigorBuff()
    {
        buffinfo = CSMainPlayerInfo.Instance.BuffInfo.GetBuff(vigorBuffId);
        if (buffinfo != null)
        {
            mlb_des.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(538), vigorMultiples * (1 + (vigorDanCfg.data * 0.0001)));
            mlb_buff.color = CSColor.green;
            buffCountCor = ScriptBinder.StartCoroutine(BuffCountDown());
            remainingTime = (buffinfo.totalTime + buffinfo.addTime - CSServerTime.Instance.TotalMillisecond) / 1000;
            mlb_buff.text = CSServerTime.Instance.FormatLongToTimeStr(remainingTime);
        }
        else
        {
            mlb_des.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(538), vigorMultiples);
            mlb_buff.text = $"[u]{ClientTipsTableManager.Instance.GetClientTipsContext(1903)}[-]";
            mlb_buff.color = CSColor.green;
            if (buffCountCor != null)
            {
                ScriptBinder.StopCoroutine(buffCountCor);
            }
        }
    }
    void VigorBuffClick(GameObject _go)
    {
        FNDebug.Log(vigorBuffId);
        UIManager.Instance.CreatePanel<UIBuyVigorPanel>();
    }
    IEnumerator BuffCountDown()
    {
        yield return wait;
        RefreshVigorBuff();
    }
    void GetBuffChange(uint id, object data)
    {
        RefreshVigorBuff();
    }
    #endregion
    void SCEnergyInfoMessage(uint id, object data)
    {
        RefreshVigorValue();
    }
    void SCEnergyFreeGetInfoMessage(uint id, object data)
    {
        CSVigorInfo.Instance.GetOpenIds(out freeIds);
        for (int i = 0; i < freeIds.Count; i++)
        {
            freeItems[i].Refresh(freeIds[i + 1]);
        }
    }
    void SCGetFreeEnergyMessage(uint id, object data)
    {
        CSVigorInfo.Instance.GetOpenIds(out freeIds);
        for (int i = 0; i < freeIds.Count; i++)
        {
            freeItems[i].Refresh(freeIds[i + 1]);
        }
    }
    void SCNotifyEnergyChangeMessage(uint id, object data)
    {
        //策划需求，暂时去掉此界面的飘字
        //int gap = CSVigorInfo.Instance.GetCurrentVigorExp() - curvalue;
        //if (gap > 0)
        //{
        //    mlb_getVigor.text = "精力经验+"+(CSVigorInfo.Instance.GetCurrentVigorExp() - curvalue).ToString();
        //    mtween_getVigor.PlayTween();
        //}
        RefreshVigorValue();
    }
    void SCEnergyExchangeInfoMessage(uint id, object data)
    {
        RefreshExchangeValue();
        RefreshExchangeInfo();
    }
    void GetMoneyChange(uint id, object data)
    {
        RefreshExchangeValue();
        RefreshExchangeInfo();
    }
    void GetFastAccessClose(uint id, object data)
    {
        int panelId = System.Convert.ToInt32(data);
        if (UtilityPanel.CheckGameModelPanelIsThis<UIVigorPanel>(panelId))
        {
            return;
        }
        UIManager.Instance.ClosePanel<UIVigorPanel>();
    }
    void RefreshVigorValue()
    {
        int ex = EnergyStorageTableManager.Instance.GetEnergyStorageEnergyExp(CSMainPlayerInfo.Instance.Level);
        mlb_levelExp.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(538), ex);
        curvalue = (int)CSVigorInfo.Instance.GetCurrentVigorExp();
        double maxvalue = CSVigorInfo.Instance.GetMaxChangeCount();
        mlb_levelValue.text = string.Concat(UtilityMath.GetDecimalValue(curvalue, "F1"), "/", UtilityMath.GetDecimalValue(maxvalue, "F1"));
        mlb_levelValue.color = (curvalue == 0) ? CSColor.red : CSColor.green;
        mlb_pro.fillAmount = (float)(curvalue / maxvalue) * 1f;
        if (0.05f <= mlb_pro.fillAmount && mlb_pro.fillAmount <= 0.98)
        {
            mobj_lb1.SetActive(true);
            mobj_lb1.transform.localPosition = new Vector3(0, -106 + (98 + 106) * mlb_pro.fillAmount, 0);
        }
        else
        {
            mobj_lb1.SetActive(false);
        }
        int lv = CSMainPlayerInfo.Instance.Level;
        mlb_nowLv.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(570), lv);
        int exp = EnergyStorageTableManager.Instance.GetEnergyStorageEnergyExp(lv);//当前一点精力值换算的经验
        long cur_exp = CSMainPlayerInfo.Instance.Exp; //当前经验值
        long expectVigorExp = cur_exp + exp * (int)curvalue;//当前精力值能换算的总经验值 + curexp
        int expectedLv = lv;
        long max_exp = LevelTableManager.Instance.GetExpByLevel(expectedLv);//lv等级时升级所需最大经验
        while (expectVigorExp >= max_exp && max_exp > 0)
        {
            if (expectVigorExp <= 0)
            {
                break;
            }
            //Debug.Log($"{expectedLv}  {max_exp}  {expectVigorExp}");
            expectVigorExp = expectVigorExp - max_exp;
            if (expectVigorExp > 0)
            {
                expectedLv++;
            }
            max_exp = LevelTableManager.Instance.GetExpByLevel(expectedLv);
        }
        mlb_expectedLv.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(570), expectedLv);
    }
    bool isCostEnough = false;
    void RefreshExchangeValue()
    {
        exNum = CSVigorInfo.Instance.GetDoneChangeCount();
        mlb_todayNum.text = string.Concat(exNum, "/", CSVigorInfo.Instance.GetTodayTotalCount());
        mlb_todayNum.color = (exNum == CSVigorInfo.Instance.GetTodayTotalCount()) ? CSColor.red : CSColor.green;
        isCostEnough = (exNum == CSVigorInfo.Instance.GetTodayTotalCount()) ? false : true;
        List<int> totalNum = UtilityMainMath.SplitStringToIntList(SundryTableManager.Instance.GetSundryEffect(91), '#');
    }
    void RefreshExchangeInfo()
    {
        for (int i = 0; i < exchangeItems.Count; i++)
        {
            exchangeItems[i].Refresh(exNum, isCostEnough);
        }
    }
    void HelpBtnClick(GameObject _go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.VIGOR);
    }
    void CloseBtnClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIVigorPanel>();
    }
    public override void SelectChildPanel(int type = 1)
    {

    }

    public override void SelectChildPanel(int type, int subType)
    {

    }
}
public class FreeVigorItem : IDispose
{
    public GameObject go;
    GameObject btn;
    UILabel time;
    UISprite hasGet;
    UISprite btn_bg;
    UIItemBase item;
    TimerState state;
    UILabel btn_lb;
    GameObject effect;
    UILabel lb_des;
    Schedule schedule;

    /// <summary>
    /// 1已领取  2可领  3未到时  4待充值  5补领
    /// </summary>
    int getType = 0;

    public FreeVigorItem(GameObject _go)
    {
        go = _go;
        btn = go.transform.Find("btn_get").gameObject;
        time = go.transform.Find("time").GetComponent<UILabel>();
        hasGet = go.transform.Find("sp_get").GetComponent<UISprite>();
        btn_bg = go.transform.Find("btn_get/Background").GetComponent<UISprite>();
        item = UIItemManager.Instance.GetItem(PropItemType.Normal, go.transform, itemSize.Size64);
        btn_lb = go.transform.Find("btn_get/Label").GetComponent<UILabel>();
        effect = go.transform.Find("effect").gameObject;
        lb_des = go.transform.Find("lb_des").GetComponent<UILabel>();
        lb_des.text = ClientTipsTableManager.Instance.GetClientTipsContext(2032);
        CSEffectPlayMgr.Instance.ShowUIEffect(effect, 17903);
        Init();
    }
    public void Refresh(TimerState _state)
    {
        TimerTableManager ins = TimerTableManager.Instance;
        state = _state;
        time.text = ins.GetTimerDesc(state.timerId);
        item.Refresh(ItemTableManager.Instance.GetItemCfg(12));
        item.SetCount(ins.GetTimerParam(state.timerId));
        lb_des.gameObject.SetActive(false);
        btn_lb.text = ClientTipsTableManager.Instance.GetClientTipsContext(2034);
        int startTime = 0;
        int endTime = 0;
        int startSecond = 0;
        int nowSecond = 0;
        if (state.state == 3)
        {
            getType = 1;
        }
        else if (state.state == 2)
        {
            getType = 2;
        }
        else if (state.state == 1)
        {
            startTime = UtilityMath.CronTimeStringParseToHMS(ins.GetTimerStartTime(state.timerId));
            endTime = UtilityMath.CronTimeStringParseToHMS(ins.GetTimerEndTime(state.timerId));
            if (startTime != 0 || endTime != 0)
            {
                int startH = startTime / 10000;
                int startM = (startTime - startH * 10000) / 100;
                int startS = startTime - startH * 10000 - startM * 100;
                startSecond = startH * 3600 + startM * 60 + startS;

                int endH = endTime / 10000;
                int endM = (endTime - endH * 10000) / 100;
                int endS = endTime - endH * 10000 - endM * 100;
                int endSecond = endH * 3600 + endM * 60 + endS;

                //Debug.Log($"{ins.GetTimerStartTime(state.timerId)}   {ins.GetTimerEndTime(state.timerId)}  {startTime}    {endTime}");
                //Debug.Log($"{startH}  {startM}   {startS}   {endH}  {endM}  {endS}");

                int nowH = CSServerTime.Instance.ServerNows.Hour;
                int nowM = CSServerTime.Instance.ServerNows.Minute;
                int nowS = CSServerTime.Instance.ServerNows.Second;
                nowSecond = nowH * 3600 + nowM * 60 + nowS;

                if (nowSecond < startSecond)//未到点
                {
                    getType = 3;
                }
                if (nowSecond > endSecond)//已过时
                {
                    if (!CSVigorInfo.Instance.hasChargeEnergy)
                    {
                        getType = 4;
                    }
                    else
                    {
                        if (!CSVigorInfo.Instance.isGetChargeEnergy)
                        {
                            getType = 5;
                        }
                        else
                        {
                            getType = 1;
                        }
                    }
                }
            }
        }

        if (getType == 1)
        {
            hasGet.gameObject.SetActive(true);
            hasGet.spriteName = "yilingqu3";
            btn.SetActive(false);
            effect.SetActive(false);
            time.color = CSColor.beige;
        }
        else if (getType == 2)
        {
            hasGet.gameObject.SetActive(false);
            btn.SetActive(true);
            effect.SetActive(true);
            time.color = CSColor.beige;
        }
        else if (getType == 3)
        {
            hasGet.gameObject.SetActive(true);
            hasGet.spriteName = "weidaodian";
            btn.SetActive(false);
            effect.SetActive(false);
            time.color = CSColor.red;
            Timer.Instance.CancelInvoke(schedule);
            schedule = Timer.Instance.Invoke(startSecond - nowSecond, WaitOpen);
        }
        else if (getType == 4)
        {
            btn.SetActive(false);
            effect.SetActive(false);
            time.color = CSColor.red;
            hasGet.gameObject.SetActive(false);
            lb_des.gameObject.SetActive(true);
        }
        else if (getType == 5)
        {
            hasGet.gameObject.SetActive(false);
            btn.SetActive(true);
            btn_lb.text = ClientTipsTableManager.Instance.GetClientTipsContext(2033);
            effect.SetActive(true);
            time.color = CSColor.beige;
        }
    }

    void Init()
    {
        UIEventListener.Get(btn).onClick = GetBtnClick;
        UIEventListener.Get(lb_des.gameObject).onClick = RechargeClick;
    }
    void GetBtnClick(GameObject go)
    {
        if (CSVigorInfo.Instance.hasChargeEnergy && !CSVigorInfo.Instance.isGetChargeEnergy)
        {
            if (CSVigorInfo.Instance.IsVigorFull())
            {
                if (!Constant.ShowTipsOnceList.Contains(99))
                {
                    UtilityTips.ShowPromptWordTips(99, LeftAction, RightAction);
                }
                else
                {
                    Net.CSGetFreeEnergyMessage(state.timerId);
                }
            }
            else
            {
                Net.CSGetFreeEnergyMessage(state.timerId);
            }
            return;
        }
        if (state.timerId != 0 && state.state == 2)
        {
            if (CSVigorInfo.Instance.IsVigorFull())
            {
                if (!Constant.ShowTipsOnceList.Contains(99))
                {
                    UtilityTips.ShowPromptWordTips(99, LeftAction, RightAction);
                }
                else
                {
                    Net.CSGetFreeEnergyMessage(state.timerId);
                }
            }
            else
            {
                Net.CSGetFreeEnergyMessage(state.timerId);
            }
        }
        else
        {
            Net.CSEnergyInfoMessage();
            Net.CSEnergyFreeGetInfoMessage();
            Net.CSGetEnergeExchangeInfoMessage();
        }
    }
    void RechargeClick(GameObject _go)
    {
        UtilityPanel.JumpToPanel(12305);
    }
    void LeftAction()
    {
        UtilityPanel.JumpToPanel(12605);
        UIManager.Instance.ClosePanel<UIVigorPanel>();
    }
    void RightAction()
    {
        UtilityPath.FindWithDeliverId(DeliverTableManager.Instance.GetSuggestDeliverId(CSMainPlayerInfo.Instance.Level));
        UIManager.Instance.ClosePanel<UIVigorPanel>();
    }

    void WaitOpen(Schedule schedule)
    {
        Timer.Instance.CancelInvoke(schedule);
        Net.CSEnergyInfoMessage();
        Net.CSEnergyFreeGetInfoMessage();
        Net.CSGetEnergeExchangeInfoMessage();
    }
    public void Dispose()
    {
        Timer.Instance.CancelInvoke(schedule);
        CSEffectPlayMgr.Instance.Recycle(effect);
    }
}

public class ExChangeVigor : IDispose
{
    public GameObject go;
    GameObject btn;
    UILabel count;
    UISprite icon;
    GameObject add;
    GameObject effect;
    UIItemBase item;
    int cfgId = 0;
    int num = 0;
    public ExChangeVigor(GameObject _go)
    {
        go = _go;
        btn = go.transform.Find("btn_get").gameObject;
        count = go.transform.Find("gold/lb_goldcount").GetComponent<UILabel>();
        icon = go.transform.Find("gold/icon").GetComponent<UISprite>();
        add = go.transform.Find("gold/btn_add").gameObject;
        effect = go.transform.Find("btn_get/effect").gameObject;
        effect.SetActive(false);
        CSEffectPlayMgr.Instance.ShowUIEffect(effect, 17904);
        item = UIItemManager.Instance.GetItem(PropItemType.Normal, go.transform, itemSize.Size64);
        UIEventListener.Get(btn).onClick = GetBtnClick;
        UIEventListener.Get(add).onClick = AddBtnClick;
        UIEventListener.Get(icon.gameObject).onClick = ShowBtnClick;
    }
    bool costState = false;
    bool isOverLimit = false;
    public void Refresh(int _exCount, bool _state = false)
    {
        _exCount = _exCount + 1;
        TABLE.ENERGYEXCHANGE msg = EnergyExchangeTableManager.Instance.GetSingleCfgByTimes(_exCount);
        if (msg != null)
        {
            cfgId = (msg.costType1 == 0) ? msg.costType2 : msg.costType1;
            num = (msg.costType1 == 0) ? msg.costNum2 : msg.costNum1;
            icon.spriteName = string.Concat("tubiao", ItemTableManager.Instance.GetItemIcon(cfgId));
            count.text = num.ToString();
            count.color = (num <= CSItemCountManager.Instance.GetItemCount(cfgId)) ? CSColor.green : CSColor.red;
            costState = (num <= CSItemCountManager.Instance.GetItemCount(cfgId)) ? true : false;
            isOverLimit = CSVigorInfo.Instance.GetCurrentVigorExp() >= CSVigorInfo.Instance.GetMaxChangeCount();
            effect.SetActive(_state && costState && !isOverLimit);
            item.Refresh(ItemTableManager.Instance.GetItemCfg(12));
            item.SetCount(msg.energy);
        }
    }
    void GetBtnClick(GameObject go)
    {

        if (num > CSItemCountManager.Instance.GetItemCount(cfgId))
        {
            Utility.ShowGetWay(cfgId);
            return;
        }
        if (CSVigorInfo.Instance.IsVigorFull())
        {
            if (!Constant.ShowTipsOnceList.Contains(99))
            {
                UtilityTips.ShowPromptWordTips(99,
                    () =>
                    {
                        UtilityPanel.JumpToPanel(12605);
                        UIManager.Instance.ClosePanel<UIVigorPanel>();
                    },
                () =>
                {
                    UtilityPath.FindWithDeliverId(DeliverTableManager.Instance.GetSuggestDeliverId(CSMainPlayerInfo.Instance.Level));
                    UIManager.Instance.ClosePanel<UIVigorPanel>();
                });
            }
            else
            {
                Net.CSExchageEnergyMessage();
            }
        }
        else
        {
            Net.CSExchageEnergyMessage();
        }
    }
    void AddBtnClick(GameObject go)
    {
        Utility.ShowGetWay(cfgId);
    }
    void ShowBtnClick(GameObject go)
    {
        UITipsManager.Instance.CreateTips(TipsOpenType.Normal, cfgId);
    }
    public void Dispose()
    {
        CSEffectPlayMgr.Instance.Recycle(effect);
        if (item != null)
        {
            UIItemManager.Instance.RecycleSingleItem(item);
        }
    }
}