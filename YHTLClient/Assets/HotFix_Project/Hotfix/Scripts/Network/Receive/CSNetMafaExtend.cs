public partial class CSNetMafa : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResMafaInfoMessage:
				ECM_ResMafaInfoMessage(obj);
			break;
			case ECM.ResMafaExpChangeMessage:
				ECM_ResMafaExpChangeMessage(obj);
			break;
			case ECM.ResMafaLayerChangeMessage:
				ECM_ResMafaLayerChangeMessage(obj);
			break;
			case ECM.ResMafaSuperLayerUnlockMessage:
				ECM_ResMafaSuperLayerUnlockMessage(obj);
			break;
			case ECM.ResMafaBoxRewardMessage:
				ECM_ResMafaBoxRewardMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
