using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSHuanJingTimeMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSHuanJingTimeMessage,null);
	}
}
