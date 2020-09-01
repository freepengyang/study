using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSBossInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSBossInfoMessage,null);
	}
}
