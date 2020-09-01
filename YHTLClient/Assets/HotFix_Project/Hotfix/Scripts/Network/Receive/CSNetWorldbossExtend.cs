public partial class CSNetWorldboss : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCWorldBossActivityInfoResponseMessage:
				ECM_SCWorldBossActivityInfoResponseMessage(obj);
			break;
			case ECM.SCJoinWorldBossActivityResponseMessage:
				ECM_SCJoinWorldBossActivityResponseMessage(obj);
			break;
			case ECM.SCNotifyWorldBossRankInfoMessage:
				ECM_SCNotifyWorldBossRankInfoMessage(obj);
			break;
			case ECM.SCWorldBossBlessInfoMessage:
				ECM_SCWorldBossBlessInfoMessage(obj);
			break;
			case ECM.SCWorldBossBossInfoMessage:
				ECM_SCWorldBossBossInfoMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
