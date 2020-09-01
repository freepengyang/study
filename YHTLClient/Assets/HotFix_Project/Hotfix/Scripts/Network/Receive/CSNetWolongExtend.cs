public partial class CSNetWolong : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCWoLongInfoMessage:
				ECM_SCWoLongInfoMessage(obj);
			break;
			case ECM.SCWoLongLevelUpMessage:
				ECM_SCWoLongLevelUpMessage(obj);
			break;
			case ECM.SCSkillGroupInfoMessage:
				ECM_SCSkillGroupInfoMessage(obj);
			break;
			case ECM.SCWoLongXiLianMessage:
				ECM_SCWoLongXiLianMessage(obj);
			break;
			case ECM.SCWoLongXiLianSelectMessage:
				ECM_SCWoLongXiLianSelectMessage(obj);
			break;
			case ECM.SCSoldierSoulInfoAwakenMessage:
				ECM_SCSoldierSoulInfoAwakenMessage(obj);
			break;
			case ECM.SCSoldierSoulInfoMessage:
				ECM_SCSoldierSoulInfoMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
