public partial class CSNetUnion : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCGetUnionInfoMessage:
				ECM_SCGetUnionInfoMessage(obj);
			break;
			case ECM.SCUnionListMessage:
				ECM_SCUnionListMessage(obj);
			break;
			case ECM.SCCreateUnionMessage:
				ECM_SCCreateUnionMessage(obj);
			break;
			case ECM.SCInviteUnionMessage:
				ECM_SCInviteUnionMessage(obj);
			break;
			case ECM.SCUnionPositionChangedMessage:
				ECM_SCUnionPositionChangedMessage(obj);
			break;
			case ECM.SCLeaveUnionMessage:
				ECM_SCLeaveUnionMessage(obj);
			break;
			case ECM.SCUnionDonateGoldMessage:
				ECM_SCUnionDonateGoldMessage(obj);
			break;
			case ECM.SCUnionDonateEquipMessage:
				ECM_SCUnionDonateEquipMessage(obj);
			break;
			case ECM.SCJoinUnionMessage:
				ECM_SCJoinUnionMessage(obj);
			break;
			case ECM.SCUnionExchangeEquipMessage:
				ECM_SCUnionExchangeEquipMessage(obj);
			break;
			case ECM.SCUnionDeclareWarMessage:
				ECM_SCUnionDeclareWarMessage(obj);
			break;
			case ECM.SCUnionWarTimeoutMessage:
				ECM_SCUnionWarTimeoutMessage(obj);
			break;
			case ECM.SCGetUnionTabMessage:
				ECM_SCGetUnionTabMessage(obj);
			break;
			case ECM.SCGetSouvenirWealthMessage:
				ECM_SCGetSouvenirWealthMessage(obj);
			break;
			case ECM.SCUnionInfoUpdatedMessage:
				ECM_SCUnionInfoUpdatedMessage(obj);
			break;
			case ECM.SCUnionJoinInfoMessage:
				ECM_SCUnionJoinInfoMessage(obj);
			break;
			case ECM.SCImpeachementMessage:
				ECM_SCImpeachementMessage(obj);
			break;
			case ECM.SCCanApplyUnionMessage:
				ECM_SCCanApplyUnionMessage(obj);
			break;
			case ECM.SCChangeLeaderMessage:
				ECM_SCChangeLeaderMessage(obj);
			break;
			case ECM.SCQueryCombineUnionMessage:
				ECM_SCQueryCombineUnionMessage(obj);
			break;
			case ECM.SCUnionBulletinAckMessage:
				ECM_SCUnionBulletinAckMessage(obj);
			break;
			case ECM.SCImpeachmentAckMessage:
				ECM_SCImpeachmentAckMessage(obj);
			break;
			case ECM.SCImpeachmentEndNtfMessage:
				ECM_SCImpeachmentEndNtfMessage(obj);
			break;
			case ECM.SCRoleApplyUnionMessage:
				ECM_SCRoleApplyUnionMessage(obj);
			break;
			case ECM.SCUnionRecommendAckMessage:
				ECM_SCUnionRecommendAckMessage(obj);
			break;
			case ECM.SCImproveInfosMessage:
				ECM_SCImproveInfosMessage(obj);
			break;
			case ECM.SCImproveMessage:
				ECM_SCImproveMessage(obj);
			break;
			case ECM.SCUnionDestroyItemMessage:
				ECM_SCUnionDestroyItemMessage(obj);
			break;
			case ECM.SCSplitYuanbaoMessage:
				ECM_SCSplitYuanbaoMessage(obj);
			break;
			case ECM.ResUpdateSpeakLimitsMessage:
				ECM_ResUpdateSpeakLimitsMessage(obj);
			break;
			case ECM.ResUnionCallInfoMessage:
				ECM_ResUnionCallInfoMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
