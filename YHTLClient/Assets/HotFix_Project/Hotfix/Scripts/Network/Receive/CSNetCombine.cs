public partial class CSNetCombine : CSNetBase
{
	/// <summary>
	/// 合成响应
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCCombineItemMessage(NetInfo info)
	{
		combine.CombineItemResponse msg = Network.Deserialize<combine.CombineItemResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forcombine.CombineItemResponse");
			return;
		}

		CSCompoundInfo.Instance.HandleCombineItemMessage(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.CombineItem, msg);
	}
}
