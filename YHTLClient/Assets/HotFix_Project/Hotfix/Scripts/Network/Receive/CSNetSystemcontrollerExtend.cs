public partial class CSNetSystemcontroller : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SystemFunctionStateNtfMessage:
				ECM_SystemFunctionStateNtfMessage(obj);
			break;
			case ECM.SCRoleFunctionDataMessage:
				ECM_SCRoleFunctionDataMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
