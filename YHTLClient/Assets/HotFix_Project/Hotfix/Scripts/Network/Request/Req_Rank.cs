using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void ReqRankInfoMessage(Int32 type)
	{
		rank.CSRankInfo req = CSProtoManager.Get<rank.CSRankInfo>();
		req.type = type;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqRankInfoMessage,req);
	}
}
