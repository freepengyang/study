using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void ReqEnterMapMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqEnterMapMessage,null);
	}
	public static void ReqPlayerMoveMessage(Int32 x,Int32 y,Int64 startTime,Int32 currentX,Int32 currentY)
	{
		map.PlayerMoveRequest req = CSProtoManager.Get<map.PlayerMoveRequest>();
		req.x = x;
		req.y = y;
		req.startTime = startTime;
        req.currentX = currentX;
        req.currentY = currentY;
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqPlayerMoveMessage,req);
	}
	public static void ReqPlayerWalkMessage(Int32 x,Int32 y,Int64 startTime,Int32 currentX,Int32 currentY)
	{
		map.PlayerMoveRequest req = CSProtoManager.Get<map.PlayerMoveRequest>();
		req.x = x;
		req.y = y;
		req.startTime = startTime;
        req.currentX = currentX;
        req.currentY = currentY;
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqPlayerWalkMessage,req);
	}
	public static void ReqReliveMessage(Int32 type,Boolean useYuanbao)
	{
		map.ReliveRequest req = CSProtoManager.Get<map.ReliveRequest>();
		req.type = type;
		req.useYuanbao = useYuanbao;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqReliveMessage,req);
	}
	public static void ReqTransferMessage(Int32 mapId,Int32 line)
	{
		map.TransferRequest req = CSProtoManager.Get<map.TransferRequest>();
		req.mapId = mapId;
		req.line = line;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqTransferMessage,req);
	}
	
	/// <summary>
	/// 根据配置传送请求
	/// </summary>
	/// <param name="deliverId"></param>
	/// <param name="reason">传送原因</param>
	/// <param name="transferType">传送类型</param>
	/// <param name="useStone">是否使用传送石---始终false</param>
	/// <param name="useZhuQingTing">是否使用竹蜻蜓</param>
	public static void ReqTransferByDeliverConfigMessage(Int32 deliverId,Boolean useStone,Int32 reason,Boolean useZhuQingTing,Int32 transferType)
	{
		map.TransferByDeliverConfigRequest req = CSProtoManager.Get<map.TransferByDeliverConfigRequest>();
		req.deliverId = deliverId;
		req.useStone = useStone;
		req.reason = reason;
		req.useZhuQingTing = useZhuQingTing;
		req.transferType = transferType;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqTransferByDeliverConfigMessage,req);
	}
	public static void ReqBoxOperatelMessage(Int64 boxId,Int32 type,Int32 data)
	{
		map.BoxReq req = CSProtoManager.Get<map.BoxReq>();
		req.boxId = boxId;
		req.type = type;
		req.data = data;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqBoxOperatelMessage,req);
	}
	public static void ReqMoonIslandRandomTransferMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqMoonIslandRandomTransferMessage,null);
	}
	public static void MapBossInfoReqMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.MapBossInfoReqMessage,null);
	}
	public static void TongBuQiDataReqMessage(RepeatedField<map.TongBuQiRoleData> roleDataList)
	{
		map.TongBuQiDataReq req = CSProtoManager.Get<map.TongBuQiDataReq>();
		req.roleDataList.Clear();
		req.roleDataList.AddRange(roleDataList);
		roleDataList.Clear();
		CSNetRepeatedFieldPool.Put(roleDataList);
		CSHotNetWork.Instance.SendMsg((int)ECM.TongBuQiDataReqMessage,req);
	}
	public static void CSRandomMapMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSRandomMapMessage,null);
	}
	public static void CSBackCityMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSBackCityMessage,null);
	}
	public static void CSRecoverHpMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSRecoverHpMessage,null);
	}
}
