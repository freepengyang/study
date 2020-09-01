using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSEquipGemMessage(Int32 subType,Int32 pos,Int32 bagIndex)
	{
		gem.EquipGem req = CSProtoManager.Get<gem.EquipGem>();
		req.subType = subType;
		req.pos = pos;
		req.bagIndex = bagIndex;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSEquipGemMessage,req);
	}
	public static void CSUpgradePosGemMessage(Int32 subType,Int32 pos)
	{
		gem.UpgradePosGem req = CSProtoManager.Get<gem.UpgradePosGem>();
		req.subType = subType;
		req.pos = pos;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSUpgradePosGemMessage,req);
	}
	public static void CSUnlockGemPositionMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSUnlockGemPositionMessage,null);
	}
}
