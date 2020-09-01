public partial class CSNetWing : CSNetBase
{
	/// <summary>
	/// 翅膀数据
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCWingInfoMessage(NetInfo info)
	{
		wing.WingInfoResponse msg = Network.Deserialize<wing.WingInfoResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forwing.WingInfoResponse");
			return;
		}
		CSWingInfo.Instance.GetWingInfo(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.GetWingInfo, msg);
	}
	/// <summary>
	/// 翅膀升星
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCWingUpStarMessage(NetInfo info)
	{
		wing.WingUpStarResponse msg = Network.Deserialize<wing.WingUpStarResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forwing.WingUpStarResponse");
			return;
		}
		CSWingInfo.Instance.HandleWingUpStar(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.WingStarUp, msg);
	}
	/// <summary>
	/// 翅膀进阶
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCWingAdvanceMessage(NetInfo info)
	{
		wing.WingAdvanceResponse msg = Network.Deserialize<wing.WingAdvanceResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forwing.WingAdvanceResponse");
			return;
		}
		CSWingInfo.Instance.HandleWingAdvance(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.WingAdvance, msg);
	}
	/// <summary>
	/// 穿戴幻彩道具
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCDressColorWingMessage(NetInfo info)
	{
		wing.DressColorWingResponse msg = Network.Deserialize<wing.DressColorWingResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forwing.DressColorWingResponse");
			return;
		}
		CSWingInfo.Instance.HandleDressColorWing(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.DressWingColor, msg);
	}
	/// <summary>
	/// 翅膀经验丹使用
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCWingExpItemUseMessage(NetInfo info)
	{
		wing.WingExpItemUseResponse msg = Network.Deserialize<wing.WingExpItemUseResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forwing.WingExpItemUseResponse");
			return;
		}
		CSWingInfo.Instance.HandleWingExpItemUse(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.WingExpItemUse, msg);
	}
	/// <summary>
	/// 幻彩变动(激活,到期)
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCColorWingChangeMessage(NetInfo info)
	{
		wing.WingColorChange msg = Network.Deserialize<wing.WingColorChange>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forwing.WingColorChange");
			return;
		}
		CSWingInfo.Instance.HandleColorWingChange(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.WingColorChange, msg);
	}
	
	/// <summary>
	/// 羽灵信息
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCYuLingInfoMessage(NetInfo info)
	{
		wing.ResYuLingInfo msg = Network.Deserialize<wing.ResYuLingInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forwing.ResYuLingInfo");
			return;
		}
		
		CSWingInfo.Instance.HandleYuLingInfo(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.YuLingInfo, msg);
	}
}
