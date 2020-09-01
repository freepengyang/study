using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSSabacDataInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSSabacDataInfoMessage,null);
	}
	public static void CSSabacRankInfoMessage(Int32 pkId,Int32 usage)
	{
		sabac.SabacId req = CSProtoManager.Get<sabac.SabacId>();
		req.pkId = pkId;
		req.usage = usage;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSSabacRankInfoMessage,req);
	}
	public static void CSSabacStateMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSSabacStateMessage,null);
	}
}
