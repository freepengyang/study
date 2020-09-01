public partial class CSNetAthleticsactivity : CSNetBase
{
    void ECM_SCAthleticsActivityInfoMessage(NetInfo info)
    {
        athleticsactivity.AthleticsActivityInfoResponse msg = Network.Deserialize<athleticsactivity.AthleticsActivityInfoResponse>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forathleticsactivity.AthleticsActivityInfoResponse");
            return;
        }
        //Debug.Log("���û��Ϣ����  " + msg.athleticsActivityInfos.Count);
        CSDailyArenaInfo.Instance.SCAthleticsActivityInfoMessage(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.SCAthleticsActivityInfoMessage);
    }
    void ECM_SCReceiveAthleticsActivityRewardMessage(NetInfo info)
    {
        athleticsactivity.ActivityRewardInfo msg = Network.Deserialize<athleticsactivity.ActivityRewardInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forathleticsactivity.ActivityRewardInfo");
            return;
        }

        //Debug.Log("���û��Ϣ�������� " + msg.id);
        CSDailyArenaInfo.Instance.SCReceiveAthleticsActivityRewardMessage(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.SCReceiveAthleticsActivityRewardMessage);
    }
    void ECM_ActivityRewardInfoChangeNotify(NetInfo info)
    {
        athleticsactivity.ActivityRewardInfoChange msg = Network.Deserialize<athleticsactivity.ActivityRewardInfoChange>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forathleticsactivity.ActivityRewardInfo");
            return;
        }
        //Debug.Log("��ȡ�������Ϣ�������� " + msg.id);
        CSDailyArenaInfo.Instance.ECM_ActivityRewardInfoChangeNotify(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.SCReceiveAthleticsActivityRewardMessage);
    }
}
