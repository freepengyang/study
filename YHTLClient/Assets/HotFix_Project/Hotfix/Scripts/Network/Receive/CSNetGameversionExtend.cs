public partial class CSNetGameversion : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ClientVersionNtfMessage:
				ECM_ClientVersionNtfMessage(obj);
			break;
			case ECM.ClientUpdateNtfMessage:
				ECM_ClientUpdateNtfMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
