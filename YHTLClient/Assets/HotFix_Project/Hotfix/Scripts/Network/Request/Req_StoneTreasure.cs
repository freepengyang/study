using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSFloorInfoMessage()
	{
		stonetreasure.FloorInfoRequest req = CSProtoManager.Get<stonetreasure.FloorInfoRequest>();
		CSHotNetWork.Instance.SendMsg((int)ECM.CSFloorInfoMessage,req);
	}
	public static void CSStoneLocationMessage(string _location)
	{
		stonetreasure.StoneLocationRequest req = CSProtoManager.Get<stonetreasure.StoneLocationRequest>();
		req.location = _location;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSStoneLocationMessage,req);
	}
	public static void CSStoneMailMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSStoneMailMessage,null);
	}
	public static void CSDownStoneMessage()
	{
		stonetreasure.DownStoneRequest req = CSProtoManager.Get<stonetreasure.DownStoneRequest>();
		CSHotNetWork.Instance.SendMsg((int)ECM.CSDownStoneMessage,req);
	}
}
