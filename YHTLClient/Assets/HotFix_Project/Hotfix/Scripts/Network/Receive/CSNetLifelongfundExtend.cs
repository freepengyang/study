public partial class CSNetLifelongfund : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCReceiveFundRewardMessage:
				ECM_SCReceiveFundRewardMessage(obj);
			break;
			case ECM.SCLifelongFundInfoMessage:
				ECM_SCLifelongFundInfoMessage(obj);
			break;
			case ECM.SCFundTaskInfoChangeMessage:
				ECM_SCFundTaskInfoChangeMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
