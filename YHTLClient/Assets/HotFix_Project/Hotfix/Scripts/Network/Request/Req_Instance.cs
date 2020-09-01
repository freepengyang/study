using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void ReqEnterInstanceMessage(Int32 instanceId,Boolean enter = true,Int64 ownerId = 0)
	{
		instance.InstanceIdMsg req = CSProtoManager.Get<instance.InstanceIdMsg>();
		req.instanceId = instanceId;
		req.enter = enter;
		req.ownerId = ownerId;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqEnterInstanceMessage,req);
	}
	public static void ReqGetInstanceRewardMessage(Int32 count)
	{
		instance.GetInstanceRewardRequest req = CSProtoManager.Get<instance.GetInstanceRewardRequest>();
		req.count = count;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqGetInstanceRewardMessage,req);
	}
	public static void ReqBuyInstanceTimesMessage(Int32 instanceId,Boolean enter,Int64 ownerId)
	{
		instance.InstanceIdMsg req = CSProtoManager.Get<instance.InstanceIdMsg>();
		req.instanceId = instanceId;
		req.enter = enter;
		req.ownerId = ownerId;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqBuyInstanceTimesMessage,req);
	}
	public static void ReqLeaveInstanceMessage(Boolean isBackCity)
	{
		instance.LeaveInstanceRequest req = CSProtoManager.Get<instance.LeaveInstanceRequest>();
		req.isBackCity = isBackCity;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqLeaveInstanceMessage,req);
	}
	public static void ReqEnterNextInstanceMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqEnterNextInstanceMessage,null);
	}
	public static void ReqQuickInstanceRewardMessage(Int32 instanceId,Int32 subType,Int32 count)
	{
		instance.QuickInstanceRewardMsg req = CSProtoManager.Get<instance.QuickInstanceRewardMsg>();
		req.instanceId = instanceId;
		req.subType = subType;
		req.count = count;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqQuickInstanceRewardMessage,req);
	}
	public static void ReqDiLaoGuWuMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqDiLaoGuWuMessage,null);
	}
	public static void SCSuiJiDuoBaoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.SCSuiJiDuoBaoMessage, null);
	}
	public static void CSBossChallengeMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSBossChallengeMessage,null);
	}
}
