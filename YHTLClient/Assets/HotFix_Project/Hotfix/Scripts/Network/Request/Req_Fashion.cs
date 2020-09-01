using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSAllFashionInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSAllFashionInfoMessage,null);
	}
	public static void CSEquipFashionMessage(Int32 id)
	{
		fashion.FashionId req = CSProtoManager.Get<fashion.FashionId>();
		req.id = id;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSEquipFashionMessage,req);
	}
	public static void CSFashionStarLevelUpMessage(Int32 id)
	{
		fashion.FashionId req = CSProtoManager.Get<fashion.FashionId>();
		req.id = id;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSFashionStarLevelUpMessage,req);
	}
	public static void CSActiveFashionMessage(Int32 chipItemId)
	{
		fashion.ActiveFashion req = CSProtoManager.Get<fashion.ActiveFashion>();
		req.chipItemId = chipItemId;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSActiveFashionMessage,req);
	}
	public static void CSUnEquipFashionMessage(Int32 id)
	{
		fashion.FashionId req = CSProtoManager.Get<fashion.FashionId>();
		req.id = id;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSUnEquipFashionMessage,req);
	}
}
