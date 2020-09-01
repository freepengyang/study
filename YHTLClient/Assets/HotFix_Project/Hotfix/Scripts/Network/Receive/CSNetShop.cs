using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 商店网络响应
/// </summary>
public partial class CSNetShop : CSNetBase
{
	void ECM_SCShopBuyInfoMessage(NetInfo info)
	{
		shop.ShopBuyInfoResponse msg = Network.Deserialize<shop.ShopBuyInfoResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forshop.ShopBuyInfoResponse");
			return;
		}
		CSShopInfo.Instance.GetShopBuyInfo(msg);
	}
	void ECM_SCShopBuyMessage(NetInfo info)
	{
		shop.ShopBuyResponse msg = Network.Deserialize<shop.ShopBuyResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forshop.ShopBuyResponse");
			return;
		}
		CSShopInfo.Instance.AddBuyTimes(msg.shopId, msg.count);
	}
	void ECM_SCShopInfoMessage(NetInfo info)
	{
		shop.ShopInfoResponse msg = Network.Deserialize<shop.ShopInfoResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forshop.ShopInfoResponse");
			return;
		}
		HotManager.Instance.EventHandler.SendEvent(CEvent.UpdateShopMessage,msg);
	}
	void ECM_SCDailyRmbInfoMessage(NetInfo info)
	{
		shop.ResDailyRmbInfo msg = Network.Deserialize<shop.ResDailyRmbInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forshop.ResDailyRmbInfo");
			return;
		}

		CSShopInfo.Instance.dailyRmb = msg.rmb;
	}
	void ECM_SCDuiHuanShopInfoMessage(NetInfo info)
	{
		shop.ResDuiHuanShopInfo msg = Network.Deserialize<shop.ResDuiHuanShopInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forshop.ResDuiHuanShopInfo");
			return;
		}

        CSExchangeShopInfo.Instance.SC_AllInfo(msg);
	}
	void ECM_SCDuiHuanShopInfoByIdMessage(NetInfo info)
	{
		shop.DuiHuanShopInfo msg = Network.Deserialize<shop.DuiHuanShopInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forshop.DuiHuanShopInfo");
			return;
        }
        CSExchangeShopInfo.Instance.SC_SingleInfo(msg);
    }
}
