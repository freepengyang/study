using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void ReqGMCommandMessage()
	{
		//gm.GMCommand req = CSProtoManager.Get<gm.GMCommand>();
		//CSHotNetWork.Instance.SendMsg((int)ECM.ReqGMCommandMessage,req);
	}
	public static void ReqCloseServerMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqCloseServerMessage,null);
	}
	public static void ReqReloadScriptMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqReloadScriptMessage,null);
	}
}
