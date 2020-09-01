using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSCombineItemMessage(Int32 id,Int32 times)
	{
		combine.CombineItemRequest req = CSProtoManager.Get<combine.CombineItemRequest>();
		req.id = id;
		req.times = times;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSCombineItemMessage,req);
	}
}
