public partial class CSNetPlayer : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResRoleExpUpdatedMessage:
				ECM_ResRoleExpUpdatedMessage(obj);
			break;
			case ECM.ResRoleUpgradeMessage:
				ECM_ResRoleUpgradeMessage(obj);
			break;
			case ECM.ResRoleExtraValuesMessage:
				ECM_ResRoleExtraValuesMessage(obj);
			break;
			case ECM.ResDayPassedMessage:
				ECM_ResDayPassedMessage(obj);
			break;
			case ECM.ResOtherPlayerInfoMessage:
				ECM_ResOtherPlayerInfoMessage(obj);
			break;
			case ECM.ResCommonMessage:
				ECM_ResCommonMessage(obj);
			break;
			case ECM.ResOtherPlayerCommonInfoMessage:
				ECM_ResOtherPlayerCommonInfoMessage(obj);
			break;
			case ECM.RoleAttrNtfMessage:
				ECM_RoleAttrNtfMessage(obj);
			break;
			case ECM.ResPlayerDieMessage:
				ECM_ResPlayerDieMessage(obj);
			break;
			case ECM.ResPkValueUpdateMessage:
				ECM_ResPkValueUpdateMessage(obj);
			break;
			case ECM.SCRoleBriefMessage:
				ECM_SCRoleBriefMessage(obj);
			break;
			case ECM.SCPkValueChangeMessage:
				ECM_SCPkValueChangeMessage(obj);
			break;
			case ECM.SCPkGreyNameStateMessage:
				ECM_SCPkGreyNameStateMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
