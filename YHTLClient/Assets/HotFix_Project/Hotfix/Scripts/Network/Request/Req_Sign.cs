using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void ReqSignInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqSignInfoMessage,null);
	}
	public static void ReqExchangeCardMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqExchangeCardMessage,null);
	}
	public static void ReqChoseCardMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqChoseCardMessage,null);
	}
	public static void ReqSignComposeMessage(Int32 composeId,RepeatedField<Int64> lids)
	{
		sign.SignCompose req = CSProtoManager.Get<sign.SignCompose>();
		req.composeId = composeId;
		req.lids.Clear();
		req.lids.AddRange(lids);
		lids.Clear();
		CSNetRepeatedFieldPool.Put(lids);
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqSignComposeMessage,req);
	}
	public static void ReqSignAchievementMessage(Int32 honorId)
	{
		sign.SignAchievement req = CSProtoManager.Get<sign.SignAchievement>();
		req.honorId = honorId;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqSignAchievementMessage,req);
	}
	public static void ReqSignFragmentMessage(RepeatedField<Int64> lids)
	{
		sign.SignFragment req = CSProtoManager.Get<sign.SignFragment>();
		req.lids.Clear();
		req.lids.AddRange(lids);
		lids.Clear();
		CSNetRepeatedFieldPool.Put(lids);
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqSignFragmentMessage,req);
	}
	public static void ReqLockCardMessage(RepeatedField<Int32> cardGroups)
	{
		sign.LockCard req = CSProtoManager.Get<sign.LockCard>();
		req.cardGroups.Clear();
		req.cardGroups.AddRange(cardGroups);
		cardGroups.Clear();
		CSNetRepeatedFieldPool.Put(cardGroups);
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqLockCardMessage,req);
	}
	public static void ReqUnLockCardMessage(RepeatedField<Int32> cardGroups)
	{
		sign.LockCard req = CSProtoManager.Get<sign.LockCard>();
		req.cardGroups.Clear();
		req.cardGroups.AddRange(cardGroups);
		cardGroups.Clear();
		CSNetRepeatedFieldPool.Put(cardGroups);
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqUnLockCardMessage,req);
	}
}
