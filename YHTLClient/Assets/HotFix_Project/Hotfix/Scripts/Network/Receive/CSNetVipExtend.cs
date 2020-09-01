public partial class CSNetVip : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResVipMessage:
				ECM_ResVipMessage(obj);
			break;
			case ECM.ResVipTasteCardMessage:
				ECM_ResVipTasteCardMessage(obj);
			break;
			case ECM.SCVipTasteCardMessage:
				ECM_SCVipTasteCardMessage(obj);
			break;
			case ECM.SCFirstRechargeInfoMessage:
				ECM_SCFirstRechargeInfoMessage(obj);
			break;
			case ECM.FirstRechargeNtfMessage:
				ECM_FirstRechargeNtfMessage(obj);
			break;
			case ECM.AccumulatedRechargeInfoMessage:
				ECM_AccumulatedRechargeInfoMessage(obj);
			break;
			case ECM.SCMonthRechargeInfoMessage:
				ECM_SCMonthRechargeInfoMessage(obj);
			break;
			case ECM.SCMonthRechargeInfoByIdMessage:
				ECM_SCMonthRechargeInfoByIdMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
