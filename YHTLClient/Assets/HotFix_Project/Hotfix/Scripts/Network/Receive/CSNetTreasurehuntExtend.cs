public partial class CSNetTreasurehunt : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResServerHistoryMessage:
				ECM_ResServerHistoryMessage(obj);
			break;
			case ECM.ResTreasureItemChangedMessage:
				ECM_ResTreasureItemChangedMessage(obj);
			break;
			case ECM.ResTreasureStorehouseMessage:
				ECM_ResTreasureStorehouseMessage(obj);
			break;
			case ECM.ResTreasureEndMessage:
				ECM_ResTreasureEndMessage(obj);
			break;
			case ECM.ResUseTreasureExpMessage:
				ECM_ResUseTreasureExpMessage(obj);
			break;
			case ECM.ResTreasureIdMessage:
				ECM_ResTreasureIdMessage(obj);
			break;
			case ECM.ResHuntCallBackMessage:
				ECM_ResHuntCallBackMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
