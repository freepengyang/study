using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSDailyPurchaseBuyMessage(Int32 id,Int32 count)
	{
		dailypurchase.DailyPurchaseBuyRequest req = CSProtoManager.Get<dailypurchase.DailyPurchaseBuyRequest>();
		req.id = id;
		req.count = count;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSDailyPurchaseBuyMessage,req);
	}
	public static void CSLookGiftMessage(RepeatedField<Int32> ids)
	{
		dailypurchase.LookGiftRequest req = CSProtoManager.Get<dailypurchase.LookGiftRequest>();
		req.ids.Clear();
		req.ids.AddRange(ids);
		ids.Clear();
		CSNetRepeatedFieldPool.Put(ids);
		CSHotNetWork.Instance.SendMsg((int)ECM.CSLookGiftMessage,req);
	}
	public static void CSLookPositionMessage(Int32 position)
	{
		dailypurchase.LookPositionRequest req = CSProtoManager.Get<dailypurchase.LookPositionRequest>();
		req.position = position;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSLookPositionMessage,req);
	}
}
