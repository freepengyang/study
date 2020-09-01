using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSBuyMonthCardMessage(Int32 id)
	{
		monthcard.BuyMonthCardRequest req = CSProtoManager.Get<monthcard.BuyMonthCardRequest>();
		req.id = id;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSBuyMonthCardMessage,req);
	}
	public static void CSReceiveMonthCardRewardMessage(Int32 id)
	{
		monthcard.ReceiveMonthCardRewardRequest req = CSProtoManager.Get<monthcard.ReceiveMonthCardRewardRequest>();
		req.id = id;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSReceiveMonthCardRewardMessage,req);
	}
}
