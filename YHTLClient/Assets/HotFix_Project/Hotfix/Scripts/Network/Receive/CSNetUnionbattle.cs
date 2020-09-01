public partial class CSNetUnionbattle : CSNetBase
{
	void ECM_SCUnionActivityInfoMessage(NetInfo info)
	{
		unionbattle.UnionActivityInfoResponse msg = Network.Deserialize<unionbattle.UnionActivityInfoResponse>(info);
		if(null == msg)
		{
            FNDebug.LogError("Deserialize Msg Failed Forunionbattle.UnionActivityInfoResponse");
			return;
		}
        CSGuildActivityInfo.Instance.SC_AllActivityInfo(msg);

    }
	void ECM_SCUnionActivityInfoChangeMessage(NetInfo info)
	{
		unionbattle.UnionActivityInfo msg = Network.Deserialize<unionbattle.UnionActivityInfo>(info);
		if(null == msg)
		{
            FNDebug.LogError("Deserialize Msg Failed Forunionbattle.UnionActivityInfo");
			return;
		}
        CSGuildActivityInfo.Instance.SC_ActivityStateChange(msg);

    }
	void ECM_SCLastUnionRankMessage(NetInfo info)
	{
		rank.RankInfo msg = Network.Deserialize<rank.RankInfo>(info);
		if(null == msg)
		{
            FNDebug.LogError("Deserialize Msg Failed Forunionbattle.RankInfo");
			return;
		}
        CSGuildActivityInfo.Instance.SC_LastUnionRankMessage(msg);

    }
	void ECM_SCUnionActivityRewardMessage(NetInfo info)
	{
		unionbattle.UnionActivityReward msg = Network.Deserialize<unionbattle.UnionActivityReward>(info);
		if(null == msg)
		{
			FNDebug.LogError("Deserialize Msg Failed Forunionbattle.UnionActivityReward");
			return;
		}
        CSGuildActivityInfo.Instance.SC_CombatFinish(msg);
	}
}
