using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSLianTiFieldMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSLianTiFieldMessage,null);
	}
	public static void CSLianTiUpLevelMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSLianTiUpLevelMessage,null);
	}
}
