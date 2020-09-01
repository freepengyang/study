public partial class CSNetMap : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResEnterMapMessage:
				ECM_ResEnterMapMessage(obj);
			break;
			case ECM.ResUpdateViewMessage:
				ECM_ResUpdateViewMessage(obj);
			break;
			case ECM.ResObjectExitViewMessage:
				ECM_ResObjectExitViewMessage(obj);
			break;
			case ECM.ResPlayerEnterViewMessage:
				ECM_ResPlayerEnterViewMessage(obj);
			break;
			case ECM.ResMonsterEnterViewMessage:
				ECM_ResMonsterEnterViewMessage(obj);
			break;
			case ECM.ResObjectMoveMessage:
				ECM_ResObjectMoveMessage(obj);
			break;
			case ECM.ResItemEnterViewMessage:
				ECM_ResItemEnterViewMessage(obj);
			break;
			case ECM.ResNPCEnterViewMessage:
				ECM_ResNPCEnterViewMessage(obj);
			break;
			case ECM.ResPositionChangeMessage:
				ECM_ResPositionChangeMessage(obj);
			break;
			case ECM.ResChangeMapMessage:
				ECM_ResChangeMapMessage(obj);
			break;
			case ECM.ResAdjustPositionMessage:
				ECM_ResAdjustPositionMessage(obj);
			break;
			case ECM.ResReliveMessage:
				ECM_ResReliveMessage(obj);
			break;
			case ECM.ResPlayerHPChangedMessage:
				ECM_ResPlayerHPChangedMessage(obj);
			break;
			case ECM.ResBufferEnterViewMessage:
				ECM_ResBufferEnterViewMessage(obj);
			break;
			case ECM.ResItemsDropMessage:
				ECM_ResItemsDropMessage(obj);
			break;
			case ECM.ResItemOwnerChangedMessage:
				ECM_ResItemOwnerChangedMessage(obj);
			break;
			case ECM.ResPetEnterViewMessage:
				ECM_ResPetEnterViewMessage(obj);
			break;
			case ECM.ResGuardEnterViewMessage:
				ECM_ResGuardEnterViewMessage(obj);
			break;
			case ECM.ResPetStateChangedMessage:
				ECM_ResPetStateChangedMessage(obj);
			break;
			case ECM.ResTriggerEnterViewMessage:
				ECM_ResTriggerEnterViewMessage(obj);
			break;
			case ECM.ResObjectChangePositionMessage:
				ECM_ResObjectChangePositionMessage(obj);
			break;
			case ECM.ResBossOwnerChangedMessage:
				ECM_ResBossOwnerChangedMessage(obj);
			break;
			case ECM.ResBoxEnterViewMessage:
				ECM_ResBoxEnterViewMessage(obj);
			break;
			case ECM.ResSafeAreaCoordEnterViewMessage:
				ECM_ResSafeAreaCoordEnterViewMessage(obj);
			break;
			case ECM.ResMapBossMessage:
				ECM_ResMapBossMessage(obj);
			break;
			case ECM.ResWeatherChangeMessage:
				ECM_ResWeatherChangeMessage(obj);
			break;
			case ECM.SmallViewTeammateNtfMessage:
				ECM_SmallViewTeammateNtfMessage(obj);
			break;
			case ECM.MonsterAppearanceChangedNtfMessage:
				ECM_MonsterAppearanceChangedNtfMessage(obj);
			break;
			case ECM.NpcsStatNtfMessage:
				ECM_NpcsStatNtfMessage(obj);
			break;
			case ECM.PlayerStateNtfMessage:
				ECM_PlayerStateNtfMessage(obj);
			break;
			case ECM.ResPetShapeChangeNtf:
				ECM_ResPetShapeChangeNtf(obj);
			break;
			case ECM.SCMapDetailsMessage:
				ECM_SCMapDetailsMessage(obj);
			break;
			case ECM.SCGoldKeyPickUpItemMessage:
				ECM_SCGoldKeyPickUpItemMessage(obj);
			break;
			case ECM.SCMainTaskTransmitEventMessage:
				ECM_SCMainTaskTransmitEventMessage(obj);
			break;
			case ECM.SCAddMainTaskTransmitEventMessage:
				ECM_SCAddMainTaskTransmitEventMessage(obj);
			break;
			case ECM.SCRemoveMainTaskTransmitEventMessage:
				ECM_SCRemoveMainTaskTransmitEventMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
