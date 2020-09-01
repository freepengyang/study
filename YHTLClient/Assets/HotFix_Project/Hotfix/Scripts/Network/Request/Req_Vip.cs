using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void ReqDrawVipPackMessage(Int32 id)
	{
		vip.DrawVipSouvenirPackRequest req = CSProtoManager.Get<vip.DrawVipSouvenirPackRequest>();
		req.id = id;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqDrawVipPackMessage,req);
	}
	public static void CSDrawFirstRechargeRewardMessage(Int32 day)
	{
		vip.ReceiveFirstRechargeRewardRequest req = CSProtoManager.Get<vip.ReceiveFirstRechargeRewardRequest>();
		req.day = day;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSDrawFirstRechargeRewardMessage,req);
	}
	public static void ReqFirstRechargeEntranceMessage(Int32 entranceId)
	{
		vip.FirstRechargeEntrance req = CSProtoManager.Get<vip.FirstRechargeEntrance>();
		req.entranceId = entranceId;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqFirstRechargeEntranceMessage,req);
	}
	public static void CSReceiveAccumulatedRechargeRewardMessage(Int32 id)
	{
		vip.ReceiveAccumulatedRechargeRewardRequest req = CSProtoManager.Get<vip.ReceiveAccumulatedRechargeRewardRequest>();
		req.id = id;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSReceiveAccumulatedRechargeRewardMessage,req);
	}
	public static void CSMonthRechargeInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSMonthRechargeInfoMessage,null);
	}
	public static void CSMonthRechargeMessage(Int32 id)
	{
		vip.ReqMonthRecharge req = CSProtoManager.Get<vip.ReqMonthRecharge>();
		req.id = id;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSMonthRechargeMessage,req);
	}
}
