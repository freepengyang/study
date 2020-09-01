using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSGetWoLongInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSGetWoLongInfoMessage,null);
	}
	public static void CSWoLongLevelUpMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSWoLongLevelUpMessage,null);
	}
	public static void CSWoLongEquipXiLianMessage()
	{
		
	}
	
	public static void CSWoLongXiLianMessage(Int32 position,Int32 xilianType)
	{
		wolong.WoLongXiLianRequest req = CSProtoManager.Get<wolong.WoLongXiLianRequest>();
		req.position = position;
		req.xilianType = xilianType;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSWoLongXiLianMessage,req);
	}
	public static void CSWoLongXiLianSelectMessage(Int32 position,Int32 xilianType)
	{
		wolong.SelectEquip req = CSProtoManager.Get<wolong.SelectEquip>();
		req.position = position;
		req.xilianType = xilianType;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSWoLongXiLianSelectMessage,req);
	}
	public static void CSSoldierSoulInfoAwakenMessage(Int64 equipId)
	{
		wolong.SoldierSoulAwakenRequest req = CSProtoManager.Get<wolong.SoldierSoulAwakenRequest>();
		req.equipId = equipId;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSSoldierSoulInfoAwakenMessage,req);
	}
	
	public static void CSSkillGroupInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSSkillGroupInfoMessage,null);
	}
}
