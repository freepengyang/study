public partial class CSNetFashion : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCAllFashionInfoMessage:
				ECM_SCAllFashionInfoMessage(obj);
			break;
			case ECM.SCEquipFashionMessage:
				ECM_SCEquipFashionMessage(obj);
			break;
			case ECM.SCFashionStarLevelUpMessage:
				ECM_SCFashionStarLevelUpMessage(obj);
			break;
			case ECM.SCAddFashionMessage:
				ECM_SCAddFashionMessage(obj);
			break;
			case ECM.SCRemoveFashionMessage:
				ECM_SCRemoveFashionMessage(obj);
			break;
			case ECM.SCUnEquipFashionMessage:
				ECM_SCUnEquipFashionMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
