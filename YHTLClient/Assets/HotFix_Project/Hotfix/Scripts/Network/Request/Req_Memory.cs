using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void ReqPutOnMemoryEquipMessage(Int64 lid,Int64 oldLid)
	{
		memory.PutOnMemoryEquip req = CSProtoManager.Get<memory.PutOnMemoryEquip>();
		req.lid = lid;
		req.oldLid = oldLid;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqPutOnMemoryEquipMessage,req);
	}
	public static void ReqMemoryEquipLevelUpMessage(Int64 lid,RepeatedField<Int64> eatIds)
	{
		memory.ReqMemoryEquipLevelUp req = CSProtoManager.Get<memory.ReqMemoryEquipLevelUp>();
		req.lid = lid;
		req.eatIds.Clear();
		req.eatIds.AddRange(eatIds);
		eatIds.Clear();
		CSNetRepeatedFieldPool.Put(eatIds);
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqMemoryEquipLevelUpMessage,req);
	}
	public static void ReqUnlockMemoryEquipGeziMessage()
	{
		memory.UnlockMemoryEquipGezi req = CSProtoManager.Get<memory.UnlockMemoryEquipGezi>();
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqUnlockMemoryEquipGeziMessage,req);
	}
	public static void ReqDiscardMemoryEquipMessage(Int64 lid)
	{
		memory.MemoryEquipId req = CSProtoManager.Get<memory.MemoryEquipId>();
		req.lid = lid;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqDiscardMemoryEquipMessage,req);
	}
	public static void ReqMemorySummonTeamMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqMemorySummonTeamMessage,null);
	}
	public static void ReqMemoryGotoSummonMessage(Int64 rid)
	{
		memory.MemoryGotoSummon req = CSProtoManager.Get<memory.MemoryGotoSummon>();
		req.rid = rid;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqMemoryGotoSummonMessage,req);
	}
}
