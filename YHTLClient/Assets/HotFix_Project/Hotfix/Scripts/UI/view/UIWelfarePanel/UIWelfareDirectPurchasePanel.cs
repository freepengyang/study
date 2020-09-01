using dailypurchase;
using System.Collections.Generic;
using UnityEngine;

public partial class UIWelfareDirectPurchasePanel : UIBasePanel
{
    private ILBetterList<DirectPurchaseData> listDirectPurchaseDatas;

    Dictionary<int, int> rewardsDic = new Dictionary<int, int>();

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.DailyPurchaseReceive, RefreshRewardRedPoint);
        mClientEvent.Reg((uint) CEvent.DailyPurchaseInfo, RefreshDailyPurchaseInfo);
        mClientEvent.Reg((uint) CEvent.GiftBagOpen, RefreshData);
        mClientEvent.Reg((uint) CEvent.GiftBagClose, RefreshData);
        mClientEvent.Reg((uint) CEvent.DailyPurchaseBuy, DailyPurchaseBuy);
        mbtn_repay.onClick = OnClickRepay;

        CSEffectPlayMgr.Instance.ShowUITexture(mtx_bg, "banner22");
        CSEffectPlayMgr.Instance.ShowUIEffect(meffect, 17524);
    }

    void RefreshRewardRedPoint(uint id, object data)
    {
        mredpoint_repay.SetActive(CSDirectPurchaseInfo.Instance.IsHasEnableReceive());
    }

    void RefreshData(uint id, object data)
    {
        InitData();
    }

    void RefreshDailyPurchaseInfo(uint id, object data)
    {
        mScrollView_gift.ResetPosition();
        InitData();
    }

    void DailyPurchaseBuy(uint id, object data)
    {
        if (data == null) return;
        DailyPurchaseBuyResponse msg = (DailyPurchaseBuyResponse) data;
        TABLE.GIFTBAG giftbag;
        if (GiftBagTableManager.Instance.TryGetValue(msg.giftBuyInfo.giftId, out giftbag))
        {
            rewardsDic?.Clear();
            BoxTableManager.Instance.GetBoxAwardById(giftbag.rewards, rewardsDic);
            Utility.OpenGiftPrompt(rewardsDic);
            InitData();
        }
    }

    public override void Show()
    {
        base.Show();
        ScriptBinder.InvokeRepeating(0f, 1f, ScheduleReapeat);
        InitData();
        mScrollView_gift.ResetPosition();
    }

    void ScheduleReapeat()
    {
        mlb_time.text = CSString.Format(2014,
            CSServerTime.Instance.FormatLongToTimeStr(CSServerTime.Instance.SecondsLeaveNextDay, 3));
    }

    void InitData()
    {
        listDirectPurchaseDatas = CSDirectPurchaseInfo.Instance.ListDirectPurchaseDatas;
        if (listDirectPurchaseDatas == null || listDirectPurchaseDatas.Count <= 0) return;
        mredpoint_repay.SetActive(CSDirectPurchaseInfo.Instance.IsHasEnableReceive());
        mgrid_gift.MaxCount = listDirectPurchaseDatas.Count;
        GameObject gp;
        for (int i = 0; i < mgrid_gift.MaxCount; i++)
        {
            gp = mgrid_gift.controlList[i];
            var eventHandle = UIEventListener.Get(gp);
            UIWelfareDirectPurchaseBinder Binder;
            if (eventHandle.parameter == null)
            {
                Binder = new UIWelfareDirectPurchaseBinder();
                Binder.Setup(eventHandle);
            }
            else
            {
                Binder = eventHandle.parameter as UIWelfareDirectPurchaseBinder;
            }

            DirectPurchaseData directPurchaseData = listDirectPurchaseDatas[i];
            Binder.Bind(directPurchaseData);
        }
    }

    void OnClickRepay(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIDirectPurchaseRewardPanel>();
    }

    public override void OnHide()
    {
        base.OnHide();
        ScriptBinder.StopInvoke();
        mgrid_gift.UnBind<UIWelfareDirectPurchaseBinder>();
    }

    protected override void OnDestroy()
    {
        mgrid_gift.UnBind<UIWelfareDirectPurchaseBinder>();
        CSEffectPlayMgr.Instance.Recycle(mtx_bg);
        CSEffectPlayMgr.Instance.Recycle(meffect);
        base.OnDestroy();
    }
}