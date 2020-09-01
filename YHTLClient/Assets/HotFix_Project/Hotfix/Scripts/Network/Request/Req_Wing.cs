using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSWingInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSWingInfoMessage,null);
	}
	public static void CSWingUpStarMessage(Int32 times)
	{
		wing.WingUpStarRequest req = CSProtoManager.Get<wing.WingUpStarRequest>();
		req.times = times;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSWingUpStarMessage,req);
	}
	public static void CSWingAdvanceMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSWingAdvanceMessage,null);
	}
	public static void CSDressColorWingMessage(Int32 itemId, Int32 type)
	{
		wing.DressColorWingRequest req = CSProtoManager.Get<wing.DressColorWingRequest>();
		req.itemId = itemId;
		req.type = type;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSDressColorWingMessage,req);
	}
	
	public static void CSYuLingUpgradeMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSYuLingUpgradeMessage,null);
	}
	
	
	public static void CSYuLingSoulSetMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSYuLingSoulSetMessage,null);
	}
	public static void CSYuLingSoulUpgradeMessage(Int32 position)
	{
		wing.ReqYuLingSoulUpgrade req = CSProtoManager.Get<wing.ReqYuLingSoulUpgrade>();
		req.position = position;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSYuLingSoulUpgradeMessage,req);
	}
	public static void CSYuLingSoulSingleSetMessage(Int32 position, Int32 bagIndex)
	{
		wing.ReqYuLingSoulSingleSet req = CSProtoManager.Get<wing.ReqYuLingSoulSingleSet>();
		req.position = position;
		req.bagIndex = bagIndex;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSYuLingSoulSingleSetMessage,req);
	}
}
