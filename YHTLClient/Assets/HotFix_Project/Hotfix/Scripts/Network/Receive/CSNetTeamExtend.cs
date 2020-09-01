public partial class CSNetTeam : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResTeamInfoMessage:
				ECM_ResTeamInfoMessage(obj);
			break;
			case ECM.ResApplyTeamMessage:
				ECM_ResApplyTeamMessage(obj);
			break;
			case ECM.ResInviteTeamMessage:
				ECM_ResInviteTeamMessage(obj);
			break;
			case ECM.ResJoinTeamMessage:
				ECM_ResJoinTeamMessage(obj);
			break;
			case ECM.ResLeaveTeamMessage:
				ECM_ResLeaveTeamMessage(obj);
			break;
			case ECM.ResTeamLeaderChangedMessage:
				ECM_ResTeamLeaderChangedMessage(obj);
			break;
			case ECM.ResGetTeamInfoMessage:
				ECM_ResGetTeamInfoMessage(obj);
			break;
			case ECM.ResGetTeamTabMessage:
				ECM_ResGetTeamTabMessage(obj);
			break;
			case ECM.ResTeamCallBackMessage:
				ECM_ResTeamCallBackMessage(obj);
			break;
			case ECM.TeamTargetAckMessage:
				ECM_TeamTargetAckMessage(obj);
			break;
			case ECM.TeamCallBackAckMessage:
				ECM_TeamCallBackAckMessage(obj);
			break;
			case ECM.SCSetTeamModeMessage:
				ECM_SCSetTeamModeMessage(obj);
			break;
			case ECM.SCPlayerHpMpInfoMessage:
				ECM_SCPlayerHpMpInfoMessage(obj);
			break;
			case ECM.SCPlayerLevelInfoMessage:
				ECM_SCPlayerLevelInfoMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
