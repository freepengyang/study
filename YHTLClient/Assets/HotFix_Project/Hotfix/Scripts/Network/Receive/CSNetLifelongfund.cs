public partial class CSNetLifelongfund : CSNetBase
{
	void ECM_SCReceiveFundRewardMessage(NetInfo info)
	{
		lifelongfund.ReceiveFundRewardResponse msg = Network.Deserialize<lifelongfund.ReceiveFundRewardResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forlifelongfund.ReceiveFundRewardResponse");
			return;
		}
		CSlifeTimeFundInfo.Instance.SetRewardData(msg.unreceivedRewards);	}
	void ECM_SCLifelongFundInfoMessage(NetInfo info)
	{
		lifelongfund.LifelongFundInfo msg = Network.Deserialize<lifelongfund.LifelongFundInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forlifelongfund.LifelongFundInfo");
			return;
		}
			CSlifeTimeFundInfo.Instance.LifelongFundInfo = msg;	
	}
	void ECM_SCFundTaskInfoChangeMessage(NetInfo info)
	{
		lifelongfund.FundTaskInfoChange msg = Network.Deserialize<lifelongfund.FundTaskInfoChange>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forlifelongfund.FundTaskInfoChange");
			return;
		}

		CSlifeTimeFundInfo.Instance.SetTaskData(msg.fundTaskInfos);
	}
}
