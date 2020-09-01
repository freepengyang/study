public partial class CSNetSabac : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCSabacDataInfoMessage:
				ECM_SCSabacDataInfoMessage(obj);
			break;
			case ECM.SCNotifySabacStateMessage:
				ECM_SCNotifySabacStateMessage(obj);
			break;
			case ECM.SCSabacRankInfoMessage:
				ECM_SCSabacRankInfoMessage(obj);
			break;
			case ECM.SCSabacResultMessage:
				ECM_SCSabacResultMessage(obj);
			break;
			case ECM.SCSabacTransDoorInfoMessage:
				ECM_SCSabacTransDoorInfoMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
