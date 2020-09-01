public partial class CSNetAuction : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResGetAuctionItemsMessage:
				ECM_ResGetAuctionItemsMessage(obj);
			break;
			case ECM.ResGetAuctionShelfMessage:
				ECM_ResGetAuctionShelfMessage(obj);
			break;
			case ECM.ResAddToShelfMessage:
				ECM_ResAddToShelfMessage(obj);
			break;
			case ECM.ResReAddToShelfMessage:
				ECM_ResReAddToShelfMessage(obj);
			break;
			case ECM.ResRemoveFromShelfMessage:
				ECM_ResRemoveFromShelfMessage(obj);
			break;
			case ECM.ResBuyAuctionItemMessage:
				ECM_ResBuyAuctionItemMessage(obj);
			break;
			case ECM.ResUnlockAuctionShelveMessage:
				ECM_ResUnlockAuctionShelveMessage(obj);
			break;
			case ECM.ResAttentionAuctionMessage:
				ECM_ResAttentionAuctionMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
