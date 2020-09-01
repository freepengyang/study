public partial class CSNetRank : CSNetBase
{
	void ECM_ResRankInfoMessage(NetInfo info)
	{
		rank.RankInfo msg = Network.Deserialize<rank.RankInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forrank.RankInfo");
			return;
		}
        CSRankInfo.Instance.RankInfoChange(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.RankInfo, msg);
    }
}
