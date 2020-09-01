using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSRoleRankInfoMessage(Int32 type,Int32 subType)
	{
		rankalonetable.RankInfoRequrst req = CSProtoManager.Get<rankalonetable.RankInfoRequrst>();
		req.type = type;
		req.subType = subType;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSRoleRankInfoMessage,req);
	}
}
