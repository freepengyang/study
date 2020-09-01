using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSAthleticsActivityInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSAthleticsActivityInfoMessage,null);
	}
	public static void CSReceiveAthleticsActivityRewardMessage(Int32 id)
	{
		athleticsactivity.ReceiveAthleticsActivityRewardRequest req = CSProtoManager.Get<athleticsactivity.ReceiveAthleticsActivityRewardRequest>();
		req.id = id;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSReceiveAthleticsActivityRewardMessage,req);
	}
}
