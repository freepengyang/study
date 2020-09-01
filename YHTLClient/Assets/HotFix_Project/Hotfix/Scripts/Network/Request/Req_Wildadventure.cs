using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSWildAdventrueMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSWildAdventrueMessage,null);
	}
	public static void CSTakeOutItemMessage(Int32 bagIndex)
	{
		wildadventure.TakeOutItemRequest req = CSProtoManager.Get<wildadventure.TakeOutItemRequest>();
		req.bagIndex = bagIndex;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSTakeOutItemMessage,req);
	}
}
