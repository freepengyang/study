public partial class CSNetWildadventure : CSNetBase
{
	void ECM_SCWildAdventrueMessage(NetInfo info)
	{
		wildadventure.WildAdventureInfo msg = Network.Deserialize<wildadventure.WildAdventureInfo>(info);
		if(null != msg)
		{
            CSWildAdventureInfo.Instance.SC_AdventureInfo(msg);
			return;
		}
	}
	void ECM_SCTakeOutItemMessage(NetInfo info)
	{
		wildadventure.TakeOutItemResponse msg = Network.Deserialize<wildadventure.TakeOutItemResponse>(info);
		if(null != msg)
		{
            CSWildAdventureInfo.Instance.SC_AdventureInfo(msg.wildAdventureInfo);
            return;
		}
	}
	void ECM_SCBossItemMessage(NetInfo info)
	{
		wildadventure.BossItemNotify msg = Network.Deserialize<wildadventure.BossItemNotify>(info);
		if(null != msg)
		{
            CSWildAdventureInfo.Instance.SC_BossRewardInfo(msg);
            return;
		}
	}
}
