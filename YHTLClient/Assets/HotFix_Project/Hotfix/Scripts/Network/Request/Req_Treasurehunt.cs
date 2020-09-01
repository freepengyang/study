using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void ReqGetTreasureHuntMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqGetTreasureHuntMessage,null);
	}
	public static void ReqPointInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqPointInfoMessage,null);
	}
	public static void ReqTreasureHuntMessage(Int32 type)
	{
		treasurehunt.TreasureRequest req = CSProtoManager.Get<treasurehunt.TreasureRequest>();
		req.type = type;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqTreasureHuntMessage,req);
	}
	public static void ReqExchangePointMessage(Int32 id)
	{
		treasurehunt.ExchangePointRequest req = CSProtoManager.Get<treasurehunt.ExchangePointRequest>();
		req.id = id;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqExchangePointMessage,req);
	}
	public static void ReqGetItemMessage(Int32 index)
	{
		treasurehunt.GetTreasureItemRequest req = CSProtoManager.Get<treasurehunt.GetTreasureItemRequest>();
		req.index = index;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqGetItemMessage,req);
	}
	public static void ReqGetWholeItemMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqGetWholeItemMessage,null);
	}
	public static void ReqTreasureStorehouseMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqTreasureStorehouseMessage,null);
	}
	public static void ReqUseTreasureExpMessage(RepeatedField<Int32> indexList)
	{
		treasurehunt.ExpUseRequest req = CSProtoManager.Get<treasurehunt.ExpUseRequest>();
		req.indexList.Clear();
		req.indexList.AddRange(indexList);
		indexList.Clear();
		CSNetRepeatedFieldPool.Put(indexList);
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqUseTreasureExpMessage,req);
	}
	public static void ReqTreasureIdMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqTreasureIdMessage,null);
	}
	public static void ReqHuntCallBackMessage(RepeatedField<Int32> indexList)
	{
		treasurehunt.HuntCallbackRequest req = CSProtoManager.Get<treasurehunt.HuntCallbackRequest>();
		req.indexList.Clear();
		req.indexList.AddRange(indexList);
		indexList.Clear();
		CSNetRepeatedFieldPool.Put(indexList);
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqHuntCallBackMessage,req);
	}
}
