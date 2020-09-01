public partial class CSNetMemory : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResMemoryInstanceInfoMessage:
				ECM_ResMemoryInstanceInfoMessage(obj);
			break;
			case ECM.ResMemoryBagMessage:
				ECM_ResMemoryBagMessage(obj);
			break;
			case ECM.ResMemoryEquipInfoMessage:
				ECM_ResMemoryEquipInfoMessage(obj);
			break;
			case ECM.ResMemoryAddMessage:
				ECM_ResMemoryAddMessage(obj);
			break;
			case ECM.ResMemoryEquipChangeMessage:
				ECM_ResMemoryEquipChangeMessage(obj);
			break;
			case ECM.ResMemoryEquipSuitMessage:
				ECM_ResMemoryEquipSuitMessage(obj);
			break;
			case ECM.ResMemoryEquipGeziChangeMessage:
				ECM_ResMemoryEquipGeziChangeMessage(obj);
			break;
			case ECM.ResMemoryRemoveMessage:
				ECM_ResMemoryRemoveMessage(obj);
			break;
			case ECM.ResDiscardMemoryEquipMessage:
				ECM_ResDiscardMemoryEquipMessage(obj);
			break;
			case ECM.ResMemorySummonTeamMessage:
				ECM_ResMemorySummonTeamMessage(obj);
			break;
			case ECM.ResMemorySummonTeamCdMessage:
				ECM_ResMemorySummonTeamCdMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
