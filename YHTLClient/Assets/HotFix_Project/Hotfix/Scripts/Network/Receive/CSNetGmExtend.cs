public partial class CSNetGm : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResCloseServerMessage:
				ECM_ResCloseServerMessage(obj);
			break;
			case ECM.ResReloadScriptMessage:
				ECM_ResReloadScriptMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
