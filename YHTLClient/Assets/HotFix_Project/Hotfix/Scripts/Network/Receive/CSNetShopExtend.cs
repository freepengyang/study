public partial class CSNetShop : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCShopBuyInfoMessage:
				ECM_SCShopBuyInfoMessage(obj);
			break;
			case ECM.SCShopBuyMessage:
				ECM_SCShopBuyMessage(obj);
			break;
			case ECM.SCShopInfoMessage:
				ECM_SCShopInfoMessage(obj);
			break;
			case ECM.SCDailyRmbInfoMessage:
				ECM_SCDailyRmbInfoMessage(obj);
			break;
			case ECM.SCDuiHuanShopInfoMessage:
				ECM_SCDuiHuanShopInfoMessage(obj);
			break;
			case ECM.SCDuiHuanShopInfoByIdMessage:
				ECM_SCDuiHuanShopInfoByIdMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
