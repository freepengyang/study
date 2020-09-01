using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSGetWorldBossActivityInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSGetWorldBossActivityInfoMessage,null);
	}
	public static void CSJoinWorldBossActivityRequestMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSJoinWorldBossActivityRequestMessage,null);
	}
	public static void CSWorldBossRankInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSWorldBossRankInfoMessage,null);
	}
	public static void CSWorldBossBlessMessage(Int32 blessType)
	{
		worldboss.BlessRequest req = CSProtoManager.Get<worldboss.BlessRequest>();
		req.blessType = blessType;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSWorldBossBlessMessage,req);
	}
	public static void CSWorldBossBossInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSWorldBossBossInfoMessage,null);
	}
}
