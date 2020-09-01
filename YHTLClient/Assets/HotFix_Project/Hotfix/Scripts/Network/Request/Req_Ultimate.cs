using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSUltimateInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSUltimateInfoMessage,null);
	}
	public static void CSResetUltimateMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSResetUltimateMessage,null);
	}
	public static void CSSelectAdditionIndexMessage(Int32 index)
	{
		ultimate.SelectIndex req = CSProtoManager.Get<ultimate.SelectIndex>();
		req.index = index;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSSelectAdditionIndexMessage,req);
	}
	public static void CSRankInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSRankInfoMessage,null);
	}
	public static void CSRefreshCardMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSRefreshCardMessage,null);
	}
	public static void CSRequestCardMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSRequestCardMessage,null);
	}
	public static void CSOpenCardMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSOpenCardMessage,null);
	}
	public static void CSSelectCardIndexMessage(Int32 index)
	{
		ultimate.SelectIndex req = CSProtoManager.Get<ultimate.SelectIndex>();
		req.index = index;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSSelectCardIndexMessage,req);
	}
}
