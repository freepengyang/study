using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSShopBuyItemMessage(Int32 id,Int32 count,Boolean auto = false, Boolean autoUse = false)
	{
		shop.ShopBuyItemRequest req = CSProtoManager.Get<shop.ShopBuyItemRequest>();
		req.id = id;
		req.auto = auto;
		req.count = count;
		req.autoUse = autoUse;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSShopBuyItemMessage,req);
	}
	
	public static void CSShopInfoMessage(int type, int subType)
	{
		shop.ShopInfoRequest req = CSProtoManager.Get<shop.ShopInfoRequest>();
		req.type = type;
		req.subType = subType;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSShopInfoMessage,req);
	}
	public static void CSDailyRmbInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSDailyRmbInfoMessage,null);
	}
	public static void CSDuiHuanShopInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSDuiHuanShopInfoMessage,null);
	}
	public static void CSDuiHuanItemMessage(int id)
	{
		shop.ReqDuiHuanItem req = CSProtoManager.Get<shop.ReqDuiHuanItem>();
        req.id = id;
        CSHotNetWork.Instance.SendMsg((int)ECM.CSDuiHuanItemMessage,req);
	}
	public static void CSDuiHuanQuanMessage(int id, int count)
	{
		shop.ReqDuiHuanQuan req = CSProtoManager.Get<shop.ReqDuiHuanQuan>();
        req.quanId = id;
        req.quanNum = count;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSDuiHuanQuanMessage,req);
	}
}
