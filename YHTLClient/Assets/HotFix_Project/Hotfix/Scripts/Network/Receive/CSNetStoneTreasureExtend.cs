public partial class CSNetStonetreasure : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCFloorInfoMessage:
				ECM_SCFloorInfoMessage(obj);
			break;
			case ECM.SCStoneLocationMessage:
				ECM_SCStoneLocationMessage(obj);
			break;
			case ECM.SCDownLocationMessage:
				ECM_SCDownLocationMessage(obj);
			break;
			case ECM.SCGetNormalAndDownMessage:
				ECM_SCGetNormalAndDownMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
