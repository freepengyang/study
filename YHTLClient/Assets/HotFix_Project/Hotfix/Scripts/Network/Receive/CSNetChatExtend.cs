public partial class CSNetChat : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResChatMessage:
				ECM_ResChatMessage(obj);
			break;
			case ECM.LeftCornerTipNtfMessage:
				ECM_LeftCornerTipNtfMessage(obj);
			break;
			case ECM.ReleaseNtfMessage:
				ECM_ReleaseNtfMessage(obj);
			break;
			case ECM.VoiceRoomNtfMessage:
				ECM_VoiceRoomNtfMessage(obj);
			break;
			case ECM.RoleDetailNtfMessage:
				ECM_RoleDetailNtfMessage(obj);
			break;
			case ECM.ForbidChatNtfMessage:
				ECM_ForbidChatNtfMessage(obj);
			break;
			case ECM.BigExpressionNtfMessage:
				ECM_BigExpressionNtfMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
