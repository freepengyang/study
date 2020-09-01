public partial class CSNetBoss : CSNetBase
{
	void ECM_SCBossInfoMessage(NetInfo info)
	{
		boss.ChallengeBossInfoResponse msg = Network.Deserialize<boss.ChallengeBossInfoResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forboss.ChallengeBossInfoResponse");
			return;
		}
        HotManager.Instance.EventHandler.SendEvent(CEvent.ECM_SCBossInfoMessage, msg);
	}
}
