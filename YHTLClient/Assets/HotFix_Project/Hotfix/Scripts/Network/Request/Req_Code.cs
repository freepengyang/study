using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSCodeMessage(string _code)
	{
		code.ReqCode req = CSProtoManager.Get<code.ReqCode>();
        req.code = _code;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSCodeMessage,req);
	}
}
