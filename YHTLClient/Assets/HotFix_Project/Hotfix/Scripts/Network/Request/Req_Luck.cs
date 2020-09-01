using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void ReqUseLuckOilMessage(Int32 index)
	{
		luck.UseLuckOil req = CSProtoManager.Get<luck.UseLuckOil>();
		req.index = index;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqUseLuckOilMessage,req);
	}
}
