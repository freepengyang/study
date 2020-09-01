public partial class CSNetTreasurehunt : CSNetBase
{
	/// <summary>
	/// 历史信息
	/// </summary>
	/// <param name="info"></param>
	void ECM_ResServerHistoryMessage(NetInfo info)
	{
		treasurehunt.ServerHistory msg = Network.Deserialize<treasurehunt.ServerHistory>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fortreasurehunt.ServerHistory");
			return;
		}
		CSSeekTreasureInfo.Instance.HandleServerHistory(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.SeekTreasureHistory, msg);
	}
	/// <summary>
	/// 寻宝仓库物品变动信息
	/// </summary>
	/// <param name="info"></param>
	void ECM_ResTreasureItemChangedMessage(NetInfo info)
	{
		treasurehunt.TreasureItemChangeList msg = Network.Deserialize<treasurehunt.TreasureItemChangeList>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fortreasurehunt.TreasureItemChangeList");
			return;
		}
		CSSeekTreasureInfo.Instance.HandleItemChanged(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.SeekTreasureItemChanged, msg);
	}
	/// <summary>
	/// 寻宝仓库信息
	/// </summary>
	/// <param name="info"></param>
	void ECM_ResTreasureStorehouseMessage(NetInfo info)
	{
		treasurehunt.TreasureHuntInfo msg = Network.Deserialize<treasurehunt.TreasureHuntInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fortreasurehunt.TreasureHuntInfo");
			return;
		}
		CSSeekTreasureInfo.Instance.HandleStorehouse(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.SeekTreasureStorehouse, msg);
	}
	
	/// <summary>
	/// 寻宝结束信息
	/// </summary>
	/// <param name="info"></param>
	void ECM_ResTreasureEndMessage(NetInfo info)
	{
		treasurehunt.TreasureEndResponse msg = Network.Deserialize<treasurehunt.TreasureEndResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fortreasurehunt.TreasureEndResponse");
			return;
		}
		CSSeekTreasureInfo.Instance.HandleEnd(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.SeekTreasureEnd, msg);
	}	
	
	/// <summary>
	/// 使用寻宝经验丹响应
	/// </summary>
	/// <param name="info"></param>
	void ECM_ResUseTreasureExpMessage(NetInfo info)
	{
		treasurehunt.ExpUseRequest msg = Network.Deserialize<treasurehunt.ExpUseRequest>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fortreasurehunt.ExpUseRequest");
			return;
		}
		
		CSSeekTreasureInfo.Instance.HandleUseTreasureExp(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.SeekTreasureUseExp, msg);
	}
	
	/// <summary>
	/// 寻宝界面宝箱响应
	/// </summary>
	/// <param name="info"></param>
	void ECM_ResTreasureIdMessage(NetInfo info)
	{
		treasurehunt.TreasureIdResponse msg = Network.Deserialize<treasurehunt.TreasureIdResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fortreasurehunt.TreasureIdResponse");
			return;
		}
		CSSeekTreasureInfo.Instance.HandleTreasureBox(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.SeekTreasureBox, msg);
	}
	
	/// <summary>
	/// 寻宝仓库中回收装备响应
	/// </summary>
	/// <param name="info"></param>
	void ECM_ResHuntCallBackMessage(NetInfo info)
	{
		treasurehunt.HuntCallbackResponse msg = Network.Deserialize<treasurehunt.HuntCallbackResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fortreasurehunt.HuntCallbackResponse");
			return;
		}
		CSSeekTreasureInfo.Instance.HandleHuntCallBack(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.SeekTreasureHuntCallBack, msg);
	}
}
