using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSIntensifyMessage(Int32 position,Boolean useJingpoItem,Boolean useDingxingItem)
	{
		intensify.IntensifyRequest req = CSProtoManager.Get<intensify.IntensifyRequest>();
		req.position = position;
		req.useJingpoItem = useJingpoItem;
		req.useDingxingItem = useDingxingItem;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSIntensifyMessage,req);
	}
}
