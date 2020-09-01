public partial class CSNetInstance : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResBuyInstanceTimesMessage:
				ECM_ResBuyInstanceTimesMessage(obj);
			break;
			case ECM.ResInstanceInfoMessage:
				ECM_ResInstanceInfoMessage(obj);
			break;
			case ECM.SCEnterInstanceMessage:
				ECM_SCEnterInstanceMessage(obj);
			break;
			case ECM.SCInstanceFinishMessage:
				ECM_SCInstanceFinishMessage(obj);
			break;
			case ECM.SCLeaveInstanceMessage:
				ECM_SCLeaveInstanceMessage(obj);
			break;
			case ECM.ResQuickInstanceMessage:
				ECM_ResQuickInstanceMessage(obj);
			break;
			case ECM.ResInstanceCountMessage:
				ECM_ResInstanceCountMessage(obj);
			break;
			case ECM.ResDiLaoInfoMessage:
				ECM_ResDiLaoInfoMessage(obj);
			break;
			case ECM.SCUndergroundTreasureMessage:
				ECM_SCUndergroundTreasureMessage(obj);
			break;
			case ECM.SCBossChallengeMessage:
				ECM_SCBossChallengeMessage(obj);
			break;
			case ECM.SCDropLimitMessage:
				ECM_SCDropLimitMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
