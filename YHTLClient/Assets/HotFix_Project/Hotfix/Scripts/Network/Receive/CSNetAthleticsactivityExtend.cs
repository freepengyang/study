public partial class CSNetAthleticsactivity : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCAthleticsActivityInfoMessage:
				ECM_SCAthleticsActivityInfoMessage(obj);
			break;
			case ECM.SCReceiveAthleticsActivityRewardMessage:
				ECM_SCReceiveAthleticsActivityRewardMessage(obj);
			break;
			case ECM.ActivityRewardInfoChangeNotify:
				ECM_ActivityRewardInfoChangeNotify(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
