public partial class CSNetAuction : CSNetBase
{
	void ECM_ResGetAuctionItemsMessage(NetInfo info)
	{
		auction.AllAuctionItems msg = Network.Deserialize<auction.AllAuctionItems>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forauction.AuctionItemList");
			return;
		}
        UIAuctionInfo.Instance.GetBuyMes(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.ECM_ResGetAuctionItemsMessage);
    }
	void ECM_ResGetAuctionShelfMessage(NetInfo info)
	{
		auction.SelfAuctionItems msg = Network.Deserialize<auction.SelfAuctionItems>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forauction.AuctionItemList");
			return;
		}
        UIAuctionInfo.Instance.GetSellMes(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.ECM_ResGetAuctionShelfMessage);
	}
	void ECM_ResAddToShelfMessage(NetInfo info)
	{
		auction.AddToShelfResponse msg = Network.Deserialize<auction.AddToShelfResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forauction.AddToShelfResponse");
			return;
		}
        UIAuctionInfo.Instance.GetSelfSellListAdd(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.ECM_ResAddToShelfMessage,msg.item);

    }
	void ECM_ResReAddToShelfMessage(NetInfo info)
	{
		auction.AuctionItemInfo msg = Network.Deserialize<auction.AuctionItemInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forauction.AuctionItemInfo");
			return;
		}
	}
	void ECM_ResRemoveFromShelfMessage(NetInfo info)
	{
		auction.ItemIdMsg msg = Network.Deserialize<auction.ItemIdMsg>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forauction.ItemIdMsg");
			return;
		}
        UIAuctionInfo.Instance.GetSelfSellListARemove(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.ECM_ResRemoveFromShelfMessage);

    }
	void ECM_ResBuyAuctionItemMessage(NetInfo info)
	{
		auction.ItemIdMsg msg = Network.Deserialize<auction.ItemIdMsg>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forauction.ItemIdMsg");
			return;
		}
        UIAuctionInfo.Instance.GetBuyResult(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.ECM_ResBuyAuctionItemMessage);
    }
	
	void ECM_ResUnlockAuctionShelveMessage(NetInfo info)
	{
		auction.UnlockAuctionShelve msg = Network.Deserialize<auction.UnlockAuctionShelve>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forauction.UnlockAuctionShelve");
			return;
		}
        UIAuctionInfo.Instance.GetUnlockShelve(msg.shelve);
        HotManager.Instance.EventHandler.SendEvent(CEvent.ECM_ResUnlockAuctionShelveMessage);
    }
	void ECM_ResAttentionAuctionMessage(NetInfo info)
	{
		auction.ItemIdList msg = Network.Deserialize<auction.ItemIdList>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forauction.ItemIdList");
			return;
		}
        UIAuctionInfo.Instance.GetAttentionIdList(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.ResAttentionAuctionMessage);
    }
}
