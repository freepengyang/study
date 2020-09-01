public partial class CSNetSocial : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResSocialInfoMessage:
				ECM_ResSocialInfoMessage(obj);
			break;
			case ECM.ResAddRelationMessage:
				ECM_ResAddRelationMessage(obj);
			break;
			case ECM.ResFindPlayerByNameMessage:
				ECM_ResFindPlayerByNameMessage(obj);
			break;
			case ECM.ApplyFriendNotifyMessage:
				ECM_ApplyFriendNotifyMessage(obj);
			break;
			case ECM.ResDeleteRelationMessage:
				ECM_ResDeleteRelationMessage(obj);
			break;
			case ECM.ResGetAllSocialInfoMessage:
				ECM_ResGetAllSocialInfoMessage(obj);
			break;
			case ECM.RejectSingleAckMessage:
				ECM_RejectSingleAckMessage(obj);
			break;
			case ECM.QueryLatelyTouchAckMessage:
				ECM_QueryLatelyTouchAckMessage(obj);
			break;
			case ECM.SCSettingMessage:
				ECM_SCSettingMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
