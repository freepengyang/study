public partial class CSNetTujian : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCTujianInfoMessage:
				ECM_SCTujianInfoMessage(obj);
			break;
			case ECM.SCTujianUpLevelMessage:
				ECM_SCTujianUpLevelMessage(obj);
			break;
			case ECM.SCTujianUpQualityMessage:
				ECM_SCTujianUpQualityMessage(obj);
			break;
			case ECM.SCTujianInlayMessage:
				ECM_SCTujianInlayMessage(obj);
			break;
			case ECM.SCActivateSlotWingMessage:
				ECM_SCActivateSlotWingMessage(obj);
			break;
			case ECM.SCTujianAddMessage:
				ECM_SCTujianAddMessage(obj);
			break;
			case ECM.SCTujianRemoveMessage:
				ECM_SCTujianRemoveMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
