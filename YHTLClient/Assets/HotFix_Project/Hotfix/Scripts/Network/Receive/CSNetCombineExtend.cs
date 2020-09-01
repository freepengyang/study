public partial class CSNetCombine : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCCombineItemMessage:
				ECM_SCCombineItemMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
