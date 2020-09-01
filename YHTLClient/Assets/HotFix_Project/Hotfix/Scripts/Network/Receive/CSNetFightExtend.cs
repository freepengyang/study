public partial class CSNetFight : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResSkillEffectMessage:
				ECM_ResSkillEffectMessage(obj);
			break;
			case ECM.ResAddBufferMessage:
				ECM_ResAddBufferMessage(obj);
			break;
			case ECM.ResRemoveBufferMessage:
				ECM_ResRemoveBufferMessage(obj);
			break;
			case ECM.ResBufferDeltaHPMessage:
				ECM_ResBufferDeltaHPMessage(obj);
			break;
			case ECM.SCAddSkillMessage:
				ECM_SCAddSkillMessage(obj);
			break;
			case ECM.SCRemoveSkillMessage:
				ECM_SCRemoveSkillMessage(obj);
			break;
			case ECM.SCSkillShortCutMessage:
				ECM_SCSkillShortCutMessage(obj);
			break;
			case ECM.ResBufferInfoMessage:
				ECM_ResBufferInfoMessage(obj);
			break;
			case ECM.ResRemoveSkillMessage:
				ECM_ResRemoveSkillMessage(obj);
			break;
			case ECM.PkModeChangedNtfMessage:
				ECM_PkModeChangedNtfMessage(obj);
			break;
			case ECM.SCUpgradeSkillMessage:
				ECM_SCUpgradeSkillMessage(obj);
			break;
			case ECM.BufferRemoveRemindNtfMessage:
				ECM_BufferRemoveRemindNtfMessage(obj);
			break;
			case ECM.ClearSkillCDNtfMessage:
				ECM_ClearSkillCDNtfMessage(obj);
			break;
			case ECM.SCSetSkillAutoStateMessage:
				ECM_SCSetSkillAutoStateMessage(obj);
			break;
			case ECM.SCUpdateSkillRefixMessage:
				ECM_SCUpdateSkillRefixMessage(obj);
			break;
			case ECM.SCPlayerFsmStateMessage:
				ECM_SCPlayerFsmStateMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
