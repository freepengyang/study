using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSDayChargeInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSDayChargeInfoMessage,null);
	}
	public static void CSDayChargeRewardGetMessage(Int32 id)
	{
		daycharge.ConfigId req = CSProtoManager.Get<daycharge.ConfigId>();
		req.id = id;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSDayChargeRewardGetMessage,req);
	}
}
