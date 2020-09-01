public partial class CSNetBag : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResGetBagInfoMessage:
				ECM_ResGetBagInfoMessage(obj);
			break;
			case ECM.ResBagItemChangedMessage:
				ECM_ResBagItemChangedMessage(obj);
			break;
			case ECM.ResWealthAmountChangeMessage:
				ECM_ResWealthAmountChangeMessage(obj);
			break;
			case ECM.ResEquipItemMessage:
				ECM_ResEquipItemMessage(obj);
			break;
			case ECM.ResSortItemsMessage:
				ECM_ResSortItemsMessage(obj);
			break;
			case ECM.ResSwapItemMessage:
				ECM_ResSwapItemMessage(obj);
			break;
			case ECM.ResUnEquipItemMessage:
				ECM_ResUnEquipItemMessage(obj);
			break;
			case ECM.ResCallBackItemMessage:
				ECM_ResCallBackItemMessage(obj);
			break;
			case ECM.ResPickupItemMessage:
				ECM_ResPickupItemMessage(obj);
			break;
			case ECM.ResBagIsFullMessage:
				ECM_ResBagIsFullMessage(obj);
			break;
			case ECM.ResBagToStorehouseMessage:
				ECM_ResBagToStorehouseMessage(obj);
			break;
			case ECM.ResOpenDebugMsgMessage:
				ECM_ResOpenDebugMsgMessage(obj);
			break;
			case ECM.ResChangeBagCountMessage:
				ECM_ResChangeBagCountMessage(obj);
			break;
			case ECM.ItemUseLimitNtfMessage:
				ECM_ItemUseLimitNtfMessage(obj);
			break;
			case ECM.LimitItemInfoNtfMessage:
				ECM_LimitItemInfoNtfMessage(obj);
			break;
			case ECM.EquipItemModifyNBValueNtfMessage:
				ECM_EquipItemModifyNBValueNtfMessage(obj);
			break;
			case ECM.EquipRebuildNtfMessage:
				ECM_EquipRebuildNtfMessage(obj);
			break;
			case ECM.EquipXiLianNtfMessage:
				ECM_EquipXiLianNtfMessage(obj);
			break;
			case ECM.SCChooseXiLianResultNtfMessage:
				ECM_SCChooseXiLianResultNtfMessage(obj);
			break;
			case ECM.SCNotifyBagItemCdInfoMessage:
				ECM_SCNotifyBagItemCdInfoMessage(obj);
			break;
			case ECM.SCItemUsedDailyMessage:
				ECM_SCItemUsedDailyMessage(obj);
			break;
			case ECM.SCItemUsedDailyTotalMessage:
				ECM_SCItemUsedDailyTotalMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
