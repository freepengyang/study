public partial class CSNetRankalonetable : CSNetBase
{
	void ECM_SCRoleRankInfoMessage(NetInfo info)
	{
		rankalonetable.RoleRankInfoResponse msg = Network.Deserialize<rankalonetable.RoleRankInfoResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forrankalonetable.RoleRankInfoResponse");
			return;
		}
		CSRankingInfo.Instance.HandleSCRoleRankInfoMessage(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.RoleRankInfo, msg);
	}
	void ECM_SCUnionRankInfoMessage(NetInfo info)
	{
		rankalonetable.UnionRankInfoResponse msg = Network.Deserialize<rankalonetable.UnionRankInfoResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forrankalonetable.UnionRankInfoResponse");
			return;
		}
		CSRankingInfo.Instance.HandleSCUnionRankInfoMessage(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.UnionRankInfo, msg);
	}
}
