public partial class CSNetFengyin : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCFengYinOpenMessage:
				ECM_SCFengYinOpenMessage(obj);
			break;
			case ECM.SCFengYinTimeShortenMessage:
				ECM_SCFengYinTimeShortenMessage(obj);
			break;
			case ECM.SCFengYinCloseMessage:
				ECM_SCFengYinCloseMessage(obj);
			break;
			case ECM.SCHuanJingOpenMessage:
				ECM_SCHuanJingOpenMessage(obj);
			break;
			case ECM.SCHuanJingCloseMessage:
				ECM_SCHuanJingCloseMessage(obj);
			break;
			case ECM.SCHuanJingChangeMessage:
				ECM_SCHuanJingChangeMessage(obj);
			break;
			case ECM.ResWorldLevelMessage:
				ECM_ResWorldLevelMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
