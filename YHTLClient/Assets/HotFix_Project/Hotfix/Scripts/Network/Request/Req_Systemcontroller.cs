using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSRoleGetFunctionDataMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSRoleGetFunctionDataMessage,null);
	}
}
