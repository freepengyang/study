public partial class CSNetDailypurchase : CSNetBase
{
	/// <summary>
	/// 每日限购信息的购买数据
	/// </summary>
	/// <param name="info"></param>
    void ECM_SCDailyPurchaseInfoMessage(NetInfo info)
    {
        dailypurchase.DailyPurchaseInfoResponse msg = Network.Deserialize<dailypurchase.DailyPurchaseInfoResponse>(info);
        if (null == msg)
        {
	        UnityEngine.Debug.LogError("Deserialize Msg Failed Fordailypurchase.DailyPurchaseInfoResponse");
	        return;
        }
        CSGiftBagInfo.Instance.SC_AllInfoMessage(msg);
        CSDirectPurchaseInfo.Instance.HandleGiftData(msg);
        CSDiscountGiftBagInfo.Instance.HandleGiftData(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.DailyPurchaseInfo, msg);
    }
	
	/// <summary>
	/// 购买响应
	/// </summary>
	/// <param name="info"></param>
    void ECM_SCDailyPurchaseBuyMessage(NetInfo info)
    {
        dailypurchase.DailyPurchaseBuyResponse msg = Network.Deserialize<dailypurchase.DailyPurchaseBuyResponse>(info);
        if (null != msg)
        {
	        CSGiftBagInfo.Instance.SC_BuyInfoResponse(msg);
	        switch (GiftBagTableManager.Instance.GetGiftBagType(msg.giftBuyInfo.giftId))
	        {
		        case 1:
			        CSArmRaceInfo.Instance.GetBuyArmGiftInfo(msg.giftBuyInfo);
			        break;
		        case 3:
			        CSDirectPurchaseInfo.Instance.HandleDailyPurchaseBuy(msg);
			        HotManager.Instance.EventHandler.SendEvent(CEvent.DailyPurchaseBuy, msg);
			        break;
	        }

	        switch (GiftBagTableManager.Instance.GetGiftBagSubType(msg.giftBuyInfo.giftId))
	        {
		        case 1:
			        CSDiscountGiftBagInfo.Instance.HandleBuylistOpenServiceBags(msg);
			        HotManager.Instance.EventHandler.SendEvent(CEvent.DailyPurchaseBuyDiscount, msg);
			        break;
		        case 2:
			        CSDiscountGiftBagInfo.Instance.HandleBuylistSpecialOfferBags(msg);
			        HotManager.Instance.EventHandler.SendEvent(CEvent.DailyPurchaseBuyDiscount, msg);
			        break;
		        case 3:
			        CSDiscountGiftBagInfo.Instance.HandleBuylistDiscountBags(msg);
			        HotManager.Instance.EventHandler.SendEvent(CEvent.DailyPurchaseBuyDiscount, msg);
			        break;
	        }
	        return;
        }
    }
	
	/// <summary>
	/// 装备竞赛购买数据
	/// </summary>
	/// <param name="info"></param>
    void ECM_SCEquipCompetitionPurchaseInfoMessage(NetInfo info)
    {
        dailypurchase.DailyPurchaseInfoResponse msg = Network.Deserialize<dailypurchase.DailyPurchaseInfoResponse>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Fordailypurchase.DailyPurchaseInfoResponse");
            return;
        }
        //Debug.Log("军备竞赛礼包数据  "+ msg.giftBuyInfos.Count);
        CSArmRaceInfo.Instance.GetAllArmGiftInfo(msg);
    }
	
	/// <summary>
	/// 直购礼包信息领取响应
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCZhiGouInfoMessage(NetInfo info)
	{
		dailypurchase.ZhiGouInfo msg = Network.Deserialize<dailypurchase.ZhiGouInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fordailypurchase.ZhiGouInfo");
			return;
		}

		CSDirectPurchaseInfo.Instance.HandleZhiGouInfo(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.DailyPurchaseReceive, msg);
	}
	
	/// <summary>
	/// 直购礼包充值订单号
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCZhiGouOrderMessage(NetInfo info)
	{
		dailypurchase.ZhiGouOrder msg = Network.Deserialize<dailypurchase.ZhiGouOrder>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fordailypurchase.ZhiGouOrder");
			return;
		}
		
		CSDirectPurchaseInfo.Instance.HandleZhiGouOrder(msg);
	}
	
	/// <summary>
	/// giftbag表活动信息
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCGiftBagInfoMessage(NetInfo info)
	{
		dailypurchase.GiftBagList msg = Network.Deserialize<dailypurchase.GiftBagList>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fordailypurchase.GiftBagList");
			return;
		}
		CSDirectPurchaseInfo.Instance.HandleGiftBagInfo(msg);
	}
	
	/// <summary>
	/// giftbag表活动开启
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCGiftBagOpenMessage(NetInfo info)
	{
		dailypurchase.GiftBagList msg = Network.Deserialize<dailypurchase.GiftBagList>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fordailypurchase.GiftBagList");
			return;
		}
		CSDirectPurchaseInfo.Instance.HandleGiftBagOpen(msg);
		CSDiscountGiftBagInfo.Instance.HandleGiftBagOpen(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.GiftBagOpen, msg);
	}
	
	/// <summary>
	/// giftbag表活动关闭
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCGiftBagCloseMessage(NetInfo info)
	{
		dailypurchase.GiftBagList msg = Network.Deserialize<dailypurchase.GiftBagList>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fordailypurchase.GiftBagList");
			return;
		}
		CSDirectPurchaseInfo.Instance.HandleGiftBagClose(msg);
		CSDiscountGiftBagInfo.Instance.HandleGiftBagClose(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.GiftBagClose, msg);
	}
	
	/// <summary>
	/// 查看礼包返回
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCLookGiftMessage(NetInfo info)
	{
		dailypurchase.DailyPurchaseInfoResponse msg = Network.Deserialize<dailypurchase.DailyPurchaseInfoResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fordailypurchase.DailyPurchaseInfoResponse");
			return;
		}
		CSDiscountGiftBagInfo.Instance.HandleLookGift(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.LookGift, msg);
	}
	
	/// <summary>
	/// 查看子页签红点返回
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCLookPositionMessage(NetInfo info)
	{
		dailypurchase.LookPositionInfo msg = Network.Deserialize<dailypurchase.LookPositionInfo>(info);
		if(null == msg)
		{
			FNDebug.LogError("Deserialize Msg Failed Fordailypurchase.LookPositionInfo");
			return;
		}
		CSDiscountGiftBagInfo.Instance.HandleLookPosition(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.LookPosition, msg);
	}
}
