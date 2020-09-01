using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSTujianUpLevelMessage(Int64 id)
	{
		tujian.TujianUpLevelRequest req = CSProtoManager.Get<tujian.TujianUpLevelRequest>();
		req.id = id;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSTujianUpLevelMessage,req);
	}
	public static void CSTujianUpQualityMessage(RepeatedField<Int64> ids)
	{
		tujian.TujianUpQualityRequest req = CSProtoManager.Get<tujian.TujianUpQualityRequest>();
		req.ids.Clear();
		req.ids.AddRange(ids);
		ids.Clear();
		CSNetRepeatedFieldPool.Put(ids);
		CSHotNetWork.Instance.SendMsg((int)ECM.CSTujianUpQualityMessage,req);
	}
	public static void CSTujianInlayMessage(Int64 id,Int32 slotId)
	{
		tujian.TujianInlayRequest req = CSProtoManager.Get<tujian.TujianInlayRequest>();
		req.id = id;
		req.slotId = slotId;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSTujianInlayMessage,req);
	}
	public static void CSActivateSlotWingMessage(Int32 slotId)
	{
		tujian.ActivateSlotRequest req = CSProtoManager.Get<tujian.ActivateSlotRequest>();
		req.slotId = slotId;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSActivateSlotWingMessage,req);
	}
}
