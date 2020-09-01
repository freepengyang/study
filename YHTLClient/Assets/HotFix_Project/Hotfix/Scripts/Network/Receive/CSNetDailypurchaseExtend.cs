public partial class CSNetDailypurchase : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCDailyPurchaseInfoMessage:
				ECM_SCDailyPurchaseInfoMessage(obj);
			break;
			case ECM.SCDailyPurchaseBuyMessage:
				ECM_SCDailyPurchaseBuyMessage(obj);
			break;
			case ECM.SCEquipCompetitionPurchaseInfoMessage:
				ECM_SCEquipCompetitionPurchaseInfoMessage(obj);
			break;
			case ECM.SCZhiGouInfoMessage:
				ECM_SCZhiGouInfoMessage(obj);
			break;
			case ECM.SCZhiGouOrderMessage:
				ECM_SCZhiGouOrderMessage(obj);
			break;
			case ECM.SCGiftBagInfoMessage:
				ECM_SCGiftBagInfoMessage(obj);
			break;
			case ECM.SCGiftBagOpenMessage:
				ECM_SCGiftBagOpenMessage(obj);
			break;
			case ECM.SCGiftBagCloseMessage:
				ECM_SCGiftBagCloseMessage(obj);
			break;
			case ECM.SCLookGiftMessage:
				ECM_SCLookGiftMessage(obj);
			break;
			case ECM.SCLookPositionMessage:
				ECM_SCLookPositionMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
