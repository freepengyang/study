public partial class CSNetMonthcard : CSNetBase
{
	void ECM_SCBuyMonthCardMessage(NetInfo info)
	{
		monthcard.BuyMonthCardResponse msg = Network.Deserialize<monthcard.BuyMonthCardResponse>(info);
		if(null != msg)
		{
            CSMonthCardInfo.Instance.SC_BuyCardRes(msg.monthCardInfo);
            return;
		}
	}
	void ECM_SCMonthCardInfoMessage(NetInfo info)
	{
		monthcard.MonthCardInfoResponse msg = Network.Deserialize<monthcard.MonthCardInfoResponse>(info);
		if(null != msg)
		{
            CSMonthCardInfo.Instance.SC_MonthCardInfo(msg);
			return;
		}
	}
	void ECM_SCReceiveMonthCardRewardMessage(NetInfo info)
	{
		monthcard.ReceiveMonthCardRewardResponse msg = Network.Deserialize<monthcard.ReceiveMonthCardRewardResponse>(info);
		if(null != msg)
        {
            CSMonthCardInfo.Instance.SC_ReceiveRewardsRes(msg.monthCardInfo);
            return;
		}
	}
}
