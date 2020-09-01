public partial class CSNetMonthcard : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCBuyMonthCardMessage:
				ECM_SCBuyMonthCardMessage(obj);
			break;
			case ECM.SCMonthCardInfoMessage:
				ECM_SCMonthCardInfoMessage(obj);
			break;
			case ECM.SCReceiveMonthCardRewardMessage:
				ECM_SCReceiveMonthCardRewardMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
