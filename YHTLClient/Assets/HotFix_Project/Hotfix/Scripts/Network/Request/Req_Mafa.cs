using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void ReqMafaLayerRewardMessage(Int32 isSuper,Int32 layer,Int32 oneKey)
	{
		mafa.ReqMafaLayerReward req = CSProtoManager.Get<mafa.ReqMafaLayerReward>();
		req.isSuper = isSuper;
		req.layer = layer;
		req.oneKey = oneKey;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqMafaLayerRewardMessage,req);
	}
	public static void ReqMafaBoxRewardMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqMafaBoxRewardMessage,null);
	}
}
