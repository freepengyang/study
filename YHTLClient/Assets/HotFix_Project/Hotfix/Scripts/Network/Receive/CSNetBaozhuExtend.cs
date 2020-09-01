public partial class CSNetBaozhu : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCLevelUpBaoZhuMessage:
				ECM_SCLevelUpBaoZhuMessage(obj);
			break;
			case ECM.SCGradeUpBaoZhuMessage:
				ECM_SCGradeUpBaoZhuMessage(obj);
			break;
			case ECM.SCBaoZhuSkillsMessage:
				ECM_SCBaoZhuSkillsMessage(obj);
			break;
			case ECM.SCChoseBaoZhuSkillMessage:
				ECM_SCChoseBaoZhuSkillMessage(obj);
			break;
			case ECM.SCBaoZhuBossCountChangeMessage:
				ECM_SCBaoZhuBossCountChangeMessage(obj);
			break;
			case ECM.SCBaoZhuSlotSkillsMessage:
				ECM_SCBaoZhuSlotSkillsMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
