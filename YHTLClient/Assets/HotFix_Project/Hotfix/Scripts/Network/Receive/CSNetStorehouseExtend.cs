public partial class CSNetStorehouse : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResGetStorehouseInfoMessage:
				ECM_ResGetStorehouseInfoMessage(obj);
			break;
			case ECM.ResStorehouseItemChangedMessage:
				ECM_ResStorehouseItemChangedMessage(obj);
			break;
			case ECM.ResSortStorehouseMessage:
				ECM_ResSortStorehouseMessage(obj);
			break;
			case ECM.ResExchangeItemMessage:
				ECM_ResExchangeItemMessage(obj);
			break;
			case ECM.ResStorehouseToBagMessage:
				ECM_ResStorehouseToBagMessage(obj);
			break;
			case ECM.ResAddStorehouseCountMessage:
				ECM_ResAddStorehouseCountMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
