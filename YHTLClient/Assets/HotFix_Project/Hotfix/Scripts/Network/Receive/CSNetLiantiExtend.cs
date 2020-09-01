public partial class CSNetLianti : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCLianTiInfoMessage:
				ECM_SCLianTiInfoMessage(obj);
			break;
			case ECM.SCLianTiFieldMessage:
				ECM_SCLianTiFieldMessage(obj);
			break;
			case ECM.SCLianTiUpLevelMessage:
				ECM_SCLianTiUpLevelMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
