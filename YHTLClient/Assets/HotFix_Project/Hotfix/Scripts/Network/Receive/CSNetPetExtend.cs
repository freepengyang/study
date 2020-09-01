public partial class CSNetPet : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCPetHpMessage:
				ECM_SCPetHpMessage(obj);
			break;
			case ECM.SCWoLongPetActiveMessage:
				ECM_SCWoLongPetActiveMessage(obj);
			break;
			case ECM.SCNotifyWoLongPetStateMessage:
				ECM_SCNotifyWoLongPetStateMessage(obj);
			break;
			case ECM.SCWoLongPetInfoMessage:
				ECM_SCWoLongPetInfoMessage(obj);
			break;
			case ECM.SCPlayerWoLongViewInfoMessage:
				ECM_SCPlayerWoLongViewInfoMessage(obj);
			break;
			case ECM.SCPetInfoMessage:
				ECM_SCPetInfoMessage(obj);
			break;
			case ECM.SCItemCallBackInfoMessage:
				ECM_SCItemCallBackInfoMessage(obj);
			break;
			case ECM.SCPetTianFuInfoMessage:
				ECM_SCPetTianFuInfoMessage(obj);
			break;
			case ECM.SCPetSkillUpgradeMessage:
				ECM_SCPetSkillUpgradeMessage(obj);
			break;
			case ECM.SCPetTianFuPassiveSkillMessage:
				ECM_SCPetTianFuPassiveSkillMessage(obj);
			break;
			case ECM.SCPetTianFuRandomPassiveSkillMessage:
				ECM_SCPetTianFuRandomPassiveSkillMessage(obj);
			break;
			case ECM.SCPetTianFuChosePassiveSkillMessage:
				ECM_SCPetTianFuChosePassiveSkillMessage(obj);
			break;
			case ECM.SCPetTianFuChangeMessage:
				ECM_SCPetTianFuChangeMessage(obj);
			break;
			case ECM.SCPetHejiPointMessage:
				ECM_SCPetHejiPointMessage(obj);
			break;
			case ECM.SCPetActivePvpMessage:
				ECM_SCPetActivePvpMessage(obj);
			break;
			case ECM.SCCallBackSettingMessage:
				ECM_SCCallBackSettingMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
