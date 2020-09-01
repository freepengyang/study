using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSReceiveFundRewardMessage(Int32 point)
	{
		lifelongfund.ReceiveFundRewardRequest req = CSProtoManager.Get<lifelongfund.ReceiveFundRewardRequest>();
		req.point = point;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSReceiveFundRewardMessage,req);
	}
	public static void CSBuylifelongFundMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSBuylifelongFundMessage,null);
	}
	public static void CSReceiveFundTaskRewardMessage(Int32 taskId)
	{
		lifelongfund.ReceiveFundTaskRewardRequest req = CSProtoManager.Get<lifelongfund.ReceiveFundTaskRewardRequest>();
		req.taskId = taskId;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSReceiveFundTaskRewardMessage,req);
	}
}
