public partial class CSNetSign : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResCardInfoMessage:
				ECM_ResCardInfoMessage(obj);
			break;
			case ECM.ResSignInfoMessage:
				ECM_ResSignInfoMessage(obj);
			break;
			case ECM.ResCardChangeMessage:
				ECM_ResCardChangeMessage(obj);
			break;
			case ECM.ResFinalSignRewardMessage:
				ECM_ResFinalSignRewardMessage(obj);
			break;
			case ECM.ResFragmentChangeMessage:
				ECM_ResFragmentChangeMessage(obj);
			break;
			case ECM.ResLockCardMessage:
				ECM_ResLockCardMessage(obj);
			break;
			case ECM.ResCollectionChangeMessage:
				ECM_ResCollectionChangeMessage(obj);
			break;
			case ECM.ResHonorChangeMessage:
				ECM_ResHonorChangeMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
