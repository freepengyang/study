public partial class CSNetActivity : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResActivityDataMessage:
				ECM_ResActivityDataMessage(obj);
			break;
			case ECM.SCCollectActivityDataMessage:
				ECM_SCCollectActivityDataMessage(obj);
			break;
			case ECM.ResActiveRewardMessage:
				ECM_ResActiveRewardMessage(obj);
			break;
			case ECM.ResActiveMessage:
				ECM_ResActiveMessage(obj);
			break;
			case ECM.ResFengYinDataMessage:
				ECM_ResFengYinDataMessage(obj);
			break;
			case ECM.ResSpecialActivityDataMessage:
				ECM_ResSpecialActivityDataMessage(obj);
			break;
			case ECM.SCBossFirstKillDatasMessage:
				ECM_SCBossFirstKillDatasMessage(obj);
			break;
			case ECM.SCEquipXuanShangMessage:
				ECM_SCEquipXuanShangMessage(obj);
			break;
			case ECM.ResSevenDayDataMessage:
				ECM_ResSevenDayDataMessage(obj);
			break;
			case ECM.ResEquipCompetitionMessage:
				ECM_ResEquipCompetitionMessage(obj);
			break;
			case ECM.SCBossKuangHuanMessage:
				ECM_SCBossKuangHuanMessage(obj);
			break;
			case ECM.SCKillDemonMessage:
				ECM_SCKillDemonMessage(obj);
			break;
			case ECM.SCSpecialActivityOpenInfoMessage:
				ECM_SCSpecialActivityOpenInfoMessage(obj);
			break;
			case ECM.SCSevenLoginMessage:
				ECM_SCSevenLoginMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
