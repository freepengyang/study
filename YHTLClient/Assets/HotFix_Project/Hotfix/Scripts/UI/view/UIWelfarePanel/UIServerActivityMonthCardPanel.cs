using System;
using System.Collections.Generic;
using UnityEngine;
using monthcard;

public partial class UIServerActivityMonthCardPanel : UIBasePanel
{

	public override void Init()
	{
		base.Init();

        CSEffectPlayMgr.Instance.ShowUITexture(mtex_left, "banner4");
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_right, "banner5");

        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_effectBuyBtnL, 17904);
        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_effectBuyBtnR, 17904);
        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_effectGetBtnL, 17903);
        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_effectGetBtnR, 17903);
        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_cardL, 17505);
        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_cardR, 17504);

        mClientEvent.Reg((uint)CEvent.MonthCardInfoChange, MonthCardInfoChange);

        UIEventListener.Get(mobj_itemL, 1).onClick = BoxIconClick;
        UIEventListener.Get(mobj_itemR, 2).onClick = BoxIconClick;
        UIEventListener.Get(mbtn_buyLeft.gameObject, 1).onClick = BuyCardClick;
        UIEventListener.Get(mbtn_buyRight.gameObject, 2).onClick = BuyCardClick;
        UIEventListener.Get(mbtn_getLeft.gameObject, 1).onClick = ReceiveRewardClick;
        UIEventListener.Get(mbtn_getRight.gameObject, 2).onClick = ReceiveRewardClick;

        //mscroll_L.SetDynamicArrowVertical(mbtn_scrollL);
        //mscroll_R.SetDynamicArrowVertical(mbtn_scrollR);
        mbtn_scrollL.CustomActive(false);
        mbtn_scrollR.CustomActive(false);


        RefreshTips();

        TABLE.RECHARGE recharge = CSMonthCardInfo.Instance.cardRechargeA;
        if (recharge != null)
        {
            mlb_moneyLeft.text = $"￥{recharge.money}";
        }

        recharge = CSMonthCardInfo.Instance.cardRechargeB;
        if (recharge != null)
        {
            mlb_moneyRight.text = $"￥{recharge.money}";
        }
    }
	
	public override void Show()
	{
		base.Show();

        RefreshUI();
    }
	
	protected override void OnDestroy()
	{
        RecycleAllEffect();
        base.OnDestroy();
	}

    //public override void OnHide()
    //{
    //    RecycleAllEffect();
    //}


    void RecycleAllEffect()
    {
        CSEffectPlayMgr.Instance.Recycle(mtex_left);
        CSEffectPlayMgr.Instance.Recycle(mtex_right);

        CSEffectPlayMgr.Instance.Recycle(mobj_effectBuyBtnL);
        CSEffectPlayMgr.Instance.Recycle(mobj_effectBuyBtnR);
        CSEffectPlayMgr.Instance.Recycle(mobj_effectGetBtnL);
        CSEffectPlayMgr.Instance.Recycle(mobj_effectGetBtnR);
        CSEffectPlayMgr.Instance.Recycle(mobj_cardL);
        CSEffectPlayMgr.Instance.Recycle(mobj_cardR);
    }


    void RefreshTips()
    {
        string[] leftStr = UtilityMainMath.StrToStrArr(MonthCardTableManager.Instance.GetMonthCardTip(1));
        string[] rightStr = UtilityMainMath.StrToStrArr(MonthCardTableManager.Instance.GetMonthCardTip(2));

        if (leftStr == null || rightStr == null) return;
        
        string[] strTemp;
        mGrid_left.MaxCount = leftStr.Length;
        for (int i = 0; i < mGrid_left.MaxCount; i++)
        {
            UILabel descK = mGrid_left.controlList[i].transform.GetChild(0).GetComponent<UILabel>();
            UILabel descV = mGrid_left.controlList[i].transform.GetChild(1).GetComponent<UILabel>();
            strTemp = leftStr[i].Split('&');
            if (strTemp.Length > 0) descK.text = strTemp[0];
            if (strTemp.Length > 1)
            {
                descV.text = strTemp[1];
                if (strTemp[1].Contains("[/url]")) descV.SetupLink();
            }
            
        }

        mGrid_right.MaxCount = rightStr.Length;
        for (int i = 0; i < mGrid_right.MaxCount; i++)
        {
            UILabel descK = mGrid_right.controlList[i].transform.GetChild(0).GetComponent<UILabel>();
            UILabel descV = mGrid_right.controlList[i].transform.GetChild(1).GetComponent<UILabel>();
            strTemp = rightStr[i].Split('&');
            if (strTemp.Length > 0) descK.text = strTemp[0];
            if (strTemp.Length > 1)
            {
                descV.text = strTemp[1];
                if (strTemp[1].Contains("[/url]")) descV.SetupLink();
            }
        }
    }


    void RefreshUI()
    {
        //mlb_moneyLeft.text = MonthCardTableManager.Instance.GetMonthCardPrice(1).ToString();
        //mlb_moneyRight.text = MonthCardTableManager.Instance.GetMonthCardPrice(2).ToString();

        MonthCardInfo leftCard = CSMonthCardInfo.Instance.GetOneCardInfo(1);
        MonthCardInfo rightCard = CSMonthCardInfo.Instance.GetOneCardInfo(2);

        long curTimeStamp = CSServerTime.Instance.TotalMillisecond;

        if (leftCard != null)
        {
            //剩余天数
            int leftTimeL = Mathf.CeilToInt((leftCard.endTime - curTimeStamp) / (1000 * 86400));
            mobj_leftDayL.SetActive(leftCard.endTime - curTimeStamp >= 0);
            //mlb_timeLeft.text = CSString.Format(1037, $"{leftTimeL}".BBCode(leftTimeL <= 3 ? ColorType.Red : ColorType.Green));
            mlb_timeLeft.text = $"{leftTimeL}".BBCode(leftTimeL <= 3 ? ColorType.Red : ColorType.Green);
            //保留天数
            mlb_dayLeft.gameObject.SetActive(leftCard.keepRewardDay > 0);
            mlb_dayLeft.text = CSString.Format(1122, leftCard.keepRewardDay);

            //按钮
            mobj_buyLeft.gameObject.SetActive(leftCard.endTime - curTimeStamp < 0);

            mobj_receiveLeft.gameObject.SetActive(leftCard.endTime - curTimeStamp >= 0 && !leftCard.isReceive);
            mobj_receivedL.SetActive(leftCard.endTime - curTimeStamp >= 0 && leftCard.isReceive);
        }

        if (rightCard != null)
        {
            //剩余天数
            int leftTimeR = Mathf.CeilToInt((rightCard.endTime - curTimeStamp) / (1000 * 86400));
            mobj_leftDayR.SetActive(rightCard.endTime - curTimeStamp >= 0);
            //mlb_timeRight.text = CSString.Format(1037, $"{leftTimeR}".BBCode(leftTimeR <= 3 ? ColorType.Red : ColorType.Green));
            mlb_timeRight.text = $"{leftTimeR}".BBCode(leftTimeR <= 3 ? ColorType.Red : ColorType.Green);

            //保留天数
            mlb_dayRight.gameObject.SetActive(rightCard.keepRewardDay > 0);
            mlb_dayRight.text = CSString.Format(1122, rightCard.keepRewardDay);

            //按钮
            mobj_buyRight.gameObject.SetActive(rightCard.endTime - curTimeStamp < 0);
            mobj_receiveRight.gameObject.SetActive(rightCard.endTime - curTimeStamp >= 0 && !rightCard.isReceive);
            mobj_receivedR.SetActive(rightCard.endTime - curTimeStamp >= 0 && rightCard.isReceive);
        }
        
    }


    void MonthCardInfoChange(uint id, object data)
    {
        RefreshUI();
    }


    void BoxIconClick(GameObject go)
    {
        int param = System.Convert.ToInt32(UIEventListener.Get(go).parameter);
        int boxId = MonthCardTableManager.Instance.GetMonthCardRewardDay(param);

        UIManager.Instance.CreatePanel<UIUnsealRewardPanel>(f =>
        {
            (f as UIUnsealRewardPanel).Show(boxId);
        });
        //UIManager.Instance.CreatePanel<UIDailyActivityRewardPanel>((f) =>
        //{
        //    (f as UIDailyActivityRewardPanel).OpenPanel(boxId);
        //});
    }


    void BuyCardClick(GameObject go)
    {
        int param = System.Convert.ToInt32(UIEventListener.Get(go).parameter);
        CSMonthCardInfo.Instance.TryToBuyCard(param);
    }


    void ReceiveRewardClick(GameObject go)
    {
        int param = System.Convert.ToInt32(UIEventListener.Get(go).parameter);
        CSMonthCardInfo.Instance.TryToReceiveRewards(param);
    }

}
