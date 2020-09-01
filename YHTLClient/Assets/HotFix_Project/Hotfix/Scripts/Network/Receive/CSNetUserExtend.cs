public partial class CSNetUser : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResLoginMessage:
				ECM_ResLoginMessage(obj);
			break;
			case ECM.ResRandomRoleNameMessage:
				ECM_ResRandomRoleNameMessage(obj);
			break;
			case ECM.ResPlayerInfoMessage:
				ECM_ResPlayerInfoMessage(obj);
			break;
			case ECM.ResDeleteRoleMessage:
				ECM_ResDeleteRoleMessage(obj);
			break;
			case ECM.ResPlayAttributeChangedMessage:
				ECM_ResPlayAttributeChangedMessage(obj);
			break;
			case ECM.ResPlayerEquipChangedMessage:
				ECM_ResPlayerEquipChangedMessage(obj);
			break;
			case ECM.ResLoginAnotherSessionMessage:
				ECM_ResLoginAnotherSessionMessage(obj);
			break;
			case ECM.ResDisconnectMessage:
				ECM_ResDisconnectMessage(obj);
			break;
			case ECM.ResPushMessageMessage:
				ECM_ResPushMessageMessage(obj);
			break;
			case ECM.ResExtractInvitationCodeMessage:
				ECM_ResExtractInvitationCodeMessage(obj);
			break;
			case ECM.ResLoginSignTimeoutMessage:
				ECM_ResLoginSignTimeoutMessage(obj);
			break;
			case ECM.CheckCreateRoleArgsVaildAckMessage:
				ECM_CheckCreateRoleArgsVaildAckMessage(obj);
			break;
			case ECM.ServerTimeNtfMessage:
				ECM_ServerTimeNtfMessage(obj);
			break;
			case ECM.ServerLoadNtfMessage:
				ECM_ServerLoadNtfMessage(obj);
			break;
			case ECM.ServerBusyNtfMessage:
				ECM_ServerBusyNtfMessage(obj);
			break;
			case ECM.CreateRoleNtfMessage:
				ECM_CreateRoleNtfMessage(obj);
			break;
			case ECM.SCPlayerMoveSpeedMessage:
				ECM_SCPlayerMoveSpeedMessage(obj);
			break;
			case ECM.SCRoleListMessage:
				ECM_SCRoleListMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
