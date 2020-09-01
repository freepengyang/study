using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void ReqGetAuctionItemsMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqGetAuctionItemsMessage,null);
	}
	public static void ReqGetAuctionShelfMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqGetAuctionShelfMessage,null);
	}
	public static void ReqAddToShelfMessage(Int32 bagIndex,Int32 count,Int32 price,long tujianId,int itemType)
	{
		auction.AddToShelfRequest req = CSProtoManager.Get<auction.AddToShelfRequest>();
		req.bagIndex = bagIndex;
		req.count = count;
		req.price = price;
        req.tujianId = tujianId;
        req.itemType = itemType;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqAddToShelfMessage,req);
	}
	public static void ReqReAddToShelfMessage(Int64 lid)
	{
		auction.ReqReAddToShelf req = CSProtoManager.Get<auction.ReqReAddToShelf>();
		req.lid = lid;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqReAddToShelfMessage,req);
	}
	public static void ReqRemoveFromShelfMessage(Int64 lid,Int32 count)
	{
		auction.ItemIdMsg req = CSProtoManager.Get<auction.ItemIdMsg>();
		req.lid = lid;
		req.count = count;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqRemoveFromShelfMessage,req);
	}
	public static void ReqBuyAuctionAuctionMessage(Int64 lid,Int32 count)
	{
		auction.ItemIdMsg req = CSProtoManager.Get<auction.ItemIdMsg>();
		req.lid = lid;
		req.count = count;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqBuyAuctionAuctionMessage,req);
	}
	public static void ReqUnlockAuctionShelveMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqUnlockAuctionShelveMessage,null);
	}
	public static void ReqAttentionAuctionMessage(Int64 lid)
	{
        FNDebug.Log("��ע");
		auction.ItemId req = CSProtoManager.Get<auction.ItemId>();
		req.lid = lid;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqAttentionAuctionMessage,req);
	}
	public static void ReqAuctionCryMessage(Int64 lid)
	{
        FNDebug.Log("ߺ��");
        auction.ItemId req = CSProtoManager.Get<auction.ItemId>();
		req.lid = lid;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqAuctionCryMessage,req);
	}
	public static void ReqCancelAttentionAuctionMessage(Int64 lid)
	{
        FNDebug.Log("ȡ����ע");
        auction.ItemId req = CSProtoManager.Get<auction.ItemId>();
		req.lid = lid;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqCancelAttentionAuctionMessage,req);
	}
    public static void ReqGetAttentionAuctionMessage()
    {
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqGetAttentionAuctionMessage, null);
    }
}
