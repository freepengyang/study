public partial class CSNetUnionbattle : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCUnionActivityInfoMessage:
				ECM_SCUnionActivityInfoMessage(obj);
			break;
			case ECM.SCUnionActivityInfoChangeMessage:
				ECM_SCUnionActivityInfoChangeMessage(obj);
			break;
			case ECM.SCLastUnionRankMessage:
				ECM_SCLastUnionRankMessage(obj);
			break;
			case ECM.SCUnionActivityRewardMessage:
				ECM_SCUnionActivityRewardMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
