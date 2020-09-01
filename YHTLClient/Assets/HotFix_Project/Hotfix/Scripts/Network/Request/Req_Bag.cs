using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void ReqGetBagInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqGetBagInfoMessage,null);
	}
	public static void ReqEquipItemMessage(Int32 bagIndex,Int32 position,Int64 timeLimit,bag.BagItemInfo equip)
	{
		bag.EquipItemMsg req = CSProtoManager.Get<bag.EquipItemMsg>();
		req.bagIndex = bagIndex;
		req.position = position;
		req.timeLimit = timeLimit;
		req.equip = equip;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqEquipItemMessage,req);
	}
	public static void ReqSortItemsMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqSortItemsMessage,null);
	}
	public static void ReqSwapItemMessage(Int32 fromIndex,Int32 toIndex)
	{
		bag.SwapItemMsg req = CSProtoManager.Get<bag.SwapItemMsg>();
		req.fromIndex = fromIndex;
		req.toIndex = toIndex;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqSwapItemMessage,req);
	}
	public static void ReqUnEquipItemMessage(Int32 position)
	{
		bag.UnEquipItemRequest req = CSProtoManager.Get<bag.UnEquipItemRequest>();
		req.position = position;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqUnEquipItemMessage,req);
	}
	public static void ReqDestroyItemMessage(Int32 bagIndex,Int32 count,Int64 uId)
	{
		bag.DestroyItemRequest req = CSProtoManager.Get<bag.DestroyItemRequest>();
		req.bagIndex = bagIndex;
		req.count = count;
		req.uId = uId;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqDestroyItemMessage,req);
	}
	public static void ReqUseItemMessage(Int32 bagIndex,Int32 count,Boolean auto,Int32 data,Int64 uId)
	{
		bag.UseItemRequest req = CSProtoManager.Get<bag.UseItemRequest>();
		req.bagIndex = bagIndex;
		req.count = count;
		req.auto = auto;
		req.data = data;
		req.uId = uId;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqUseItemMessage,req);
	}
    public static void ReqCallBackItemMessage(List<int> bagIndex)
    {
        bag.CallbackItemRequest request = CSProtoManager.Get<bag.CallbackItemRequest>();
        request.bagIndices.Clear();
        request.bagIndices.AddRange(bagIndex);
        if (null != request && request.bagIndices.Count > 0)
        {
            CSHotNetWork.Instance.SendMsg((int)ECM.ReqCallBackItemMessage, request);
        }
        else
        {
            CSProtoManager.Recycle(request);
        }
    }
    public static void ReqCallBackItemMessage(int bagIndex)
    {
        bag.CallbackItemRequest request = CSProtoManager.Get<bag.CallbackItemRequest>();
        request.bagIndices.Clear();
        request.bagIndices.Add(bagIndex);
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqCallBackItemMessage, request);
	}
    public static void ReqPickupItemMessage(Int64 id)
	{
		bag.PickupMsg req = CSProtoManager.Get<bag.PickupMsg>();
		req.id = id;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqPickupItemMessage,req);
	}
	public static void ReqBagToStorehouseMessage(Int32 bagIndex)
	{
		bag.BagToStorehouseRequest req = CSProtoManager.Get<bag.BagToStorehouseRequest>();
		req.bagIndex = bagIndex;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqBagToStorehouseMessage,req);
	}
	public static void ReqSplitBagItemMessage(Int32 bagIndex,Int32 count)
	{
		bag.SplitBagItemRequest req = CSProtoManager.Get<bag.SplitBagItemRequest>();
		req.bagIndex = bagIndex;
		req.count = count;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqSplitBagItemMessage,req);
	}
	public static void EquipItemModifyNBValueReqMessage(RepeatedField<baglimit.EquipItemModifyNBValueReqItem> items)
	{
		baglimit.EquipItemModifyNBValueReq req = CSProtoManager.Get<baglimit.EquipItemModifyNBValueReq>();
		req.items.Clear();
		req.items.AddRange(items);
		items.Clear();
		CSNetRepeatedFieldPool.Put(items);
		CSHotNetWork.Instance.SendMsg((int)ECM.EquipItemModifyNBValueReqMessage,req);
	}
	public static void AddBagCountReqMessage(Int32 num)
	{
		baglimit.AddBagCountReq req = CSProtoManager.Get<baglimit.AddBagCountReq>();
		req.num = num;
		CSHotNetWork.Instance.SendMsg((int)ECM.AddBagCountReqMessage,req);
	}
	public static void EquipRebuildReqMessage(Int32 equipIndex)
	{
		bag.EquipRebuildReq req = CSProtoManager.Get<bag.EquipRebuildReq>();
		req.equipIndex = equipIndex;
		CSHotNetWork.Instance.SendMsg((int)ECM.EquipRebuildReqMessage,req);
	}
	public static void EquipXiLianReqMessage(bag.CSEquipXiLianReq _req)
	{
		//bag.CSEquipXiLianReq req = CSProtoManager.Get<bag.CSEquipXiLianReq>();
		//req.lockedAttrIndex.Clear();
		//req.lockedAttrIndex.AddRange(lockedAttrIndex);
		//lockedAttrIndex.Clear();
		//CSNetRepeatedFieldPool.Put(lockedAttrIndex);
		//req.xiLianNum = xiLianNum;
		//req.equipIndex = equipIndex;
		CSHotNetWork.Instance.SendMsg((int)ECM.EquipXiLianReqMessage, _req);
	}
	public static void CSChooseXiLianResultReqMessage(Int32 choosedResultIndex,Int32 equipIndex)
	{
		bag.CSChooseXiLianResultReq req = CSProtoManager.Get<bag.CSChooseXiLianResultReq>();
		req.choosedResultIndex = choosedResultIndex;
		req.equipIndex = equipIndex;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSChooseXiLianResultReqMessage,req);
	}
	public static void ReqEquipAutoRebuildMessage(Int32 equipIndex)
	{
		bag.EquipRebuildReq req = CSProtoManager.Get<bag.EquipRebuildReq>();
		req.equipIndex = equipIndex;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqEquipAutoRebuildMessage,req);
	}
}
