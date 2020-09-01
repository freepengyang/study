public partial class CSNetDaycharge : CSNetBase
{
	void ECM_SCDayChargeInfoMessage(NetInfo info)
	{
		daycharge.DayChargeResponse msg = Network.Deserialize<daycharge.DayChargeResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fordaycharge.DayChargeResponse");
			return;
		}
		CSDayChargeInfo.Instance.SetDayChargeInfoMessage(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.GetEveryTimeDayChargeInfo);
	}
	void ECM_SCDayChargeRewardGetMessage(NetInfo info)
	{
		daycharge.GetRewardResponse msg = Network.Deserialize<daycharge.GetRewardResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fordaycharge.GetRewardResponse");
			return;
		}
		CSDayChargeInfo.Instance.ResetDayChargeInfoMessage(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.GetDayChargeInfo);
	}
}
