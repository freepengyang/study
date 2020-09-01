public partial class CSNetWorldboss : CSNetBase
{
    void ECM_SCWorldBossActivityInfoResponseMessage(NetInfo info)
    {
        worldboss.ActivityInfo msg = Network.Deserialize<worldboss.ActivityInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forworldboss.ActivityInfo");
            return;
        }
        CSInstanceInfo.Instance.SetWorldBossBlessInfo(msg.blessInfo);
        HotManager.Instance.EventHandler.SendEvent(CEvent.SCWorldBossActivityInfoResponseMessage, msg);
    }
    void ECM_SCJoinWorldBossActivityResponseMessage(NetInfo info)
    {
        worldboss.JoinActivityResponse msg = Network.Deserialize<worldboss.JoinActivityResponse>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forworldboss.JoinActivityResponse");
            return;
        }

    }
    void ECM_SCNotifyWorldBossRankInfoMessage(NetInfo info)
    {
        worldboss.DamageRank msg = Network.Deserialize<worldboss.DamageRank>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forworldboss.DamageRank");
            return;
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.SCNotifyWorldBossRankInfoMessage, msg);
    }
    void ECM_SCWorldBossBlessInfoMessage(NetInfo info)
    {
        worldboss.BlessInfo msg = Network.Deserialize<worldboss.BlessInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forworldboss.BlessInfo");
            return;
        }
        CSInstanceInfo.Instance.SetWorldBossBlessInfo(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.ECM_SCWorldBossBlessInfoMessage, msg);
    }
    void ECM_SCWorldBossBossInfoMessage(NetInfo info)
    {
        worldboss.BossInfo msg = Network.Deserialize<worldboss.BossInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forworldboss.BossInfo");
            return;
        }
        CSInstanceInfo.Instance.SetWorldBossBuffInfo(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.ECM_SCWorldBossBossInfoMessage, msg);
    }
}
