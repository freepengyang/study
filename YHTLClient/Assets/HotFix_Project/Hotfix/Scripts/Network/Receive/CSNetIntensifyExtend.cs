public partial class CSNetIntensify : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCIntensifyMessage:
				ECM_SCIntensifyMessage(obj);
			break;
			case ECM.SCIntensifyInfoMessage:
				ECM_SCIntensifyInfoMessage(obj);
			break;
			case ECM.SCIntensifySuitInfoMessage:
				ECM_SCIntensifySuitInfoMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
