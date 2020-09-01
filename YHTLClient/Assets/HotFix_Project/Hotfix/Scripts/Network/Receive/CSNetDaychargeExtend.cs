public partial class CSNetDaycharge : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCDayChargeInfoMessage:
				ECM_SCDayChargeInfoMessage(obj);
			break;
			case ECM.SCDayChargeRewardGetMessage:
				ECM_SCDayChargeRewardGetMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
