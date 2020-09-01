public partial class CSNetPaodian : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResUPaoDianChangeMessage:
				ECM_ResUPaoDianChangeMessage(obj);
			break;
			case ECM.SCPaoDianInfoMessage:
				ECM_SCPaoDianInfoMessage(obj);
			break;
			case ECM.SCRandomPaoDianMessage:
				ECM_SCRandomPaoDianMessage(obj);
			break;
			case ECM.SCPaoDianExpMessage:
				ECM_SCPaoDianExpMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
