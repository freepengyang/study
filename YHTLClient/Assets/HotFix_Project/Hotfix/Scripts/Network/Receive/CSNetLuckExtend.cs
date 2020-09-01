public partial class CSNetLuck : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
