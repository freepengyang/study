public partial class CSNetLianti : CSNetBase
{
	/// <summary>
	/// 请求炼体数据回应
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCLianTiInfoMessage(NetInfo info)
	{
		lianti.LianTiInfoResponse msg = Network.Deserialize<lianti.LianTiInfoResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forlianti.LianTiInfoResponse");
			return;
		}
		CSLianTiInfo.Instance.GetLianTiInfoMessage(msg);
	}
	/// <summary>
	/// 请求炼体之地数据回应
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCLianTiFieldMessage(NetInfo info)
	{
		lianti.LianTiFieldResponse msg = Network.Deserialize<lianti.LianTiFieldResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forlianti.LianTiFieldResponse");
			return;
		}
		CSLianTiInfo.Instance.GetLianTiLandInfoMessage(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.GetLianTiLandInfo);
	}
	/// <summary>
	/// 请求炼体升级数据回应
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCLianTiUpLevelMessage(NetInfo info)
	{
		lianti.LianTiInfoResponse msg = Network.Deserialize<lianti.LianTiInfoResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forlianti.LianTiInfoResponse");
			return;
		}
		CSLianTiInfo.Instance.GetLianTiInfoMessage(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.GetLianTiInfo);
	}
}
