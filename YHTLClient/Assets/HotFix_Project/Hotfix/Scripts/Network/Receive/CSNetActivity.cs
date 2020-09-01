public partial class CSNetActivity : CSNetBase
{
    void ECM_ResActivityDataMessage(NetInfo info)
    {
        activity.ActivityInfo msg = Network.Deserialize<activity.ActivityInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Foractivity.ActivityInfo");
            return;
        }
        if (null != msg)
        {
            CSActivityInfo.Instance.SetDatas(msg);
        }

    }
    /// <summary>
    /// 装备收集的每档列表
    /// </summary>
    /// <param name="info"></param>
    void ECM_SCCollectActivityDataMessage(NetInfo info)
    {
        activity.CollectActivityDatas msg = Network.Deserialize<activity.CollectActivityDatas>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Foractivity.CollectActivityDatas");
            return;
        }
        CSOpenServerACInfo.Instance.SCCollectActivityData(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.SCCollectActivityData);
    }
    void ECM_ResActiveRewardMessage(NetInfo info)
    {
        activity.ResActiveReward msg = Network.Deserialize<activity.ResActiveReward>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Foractivity.ResActiveReward");
            return;
        }
        CSActivityInfo.Instance.SetReceivedActive(msg);
    }
    void ECM_ResActiveMessage(NetInfo info)
    {
        activity.ResActive msg = Network.Deserialize<activity.ResActive>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Foractivity.ResActive");
            return;
        }

        CSActivityInfo.Instance.SetActive(msg);
    }
    void ECM_ResFengYinDataMessage(NetInfo info)
    {
        activity.ResFengYinData msg = Network.Deserialize<activity.ResFengYinData>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Foractivity.ResFengYinData");
            return;
        }
        CSOpenServerACInfo.Instance.SetFengYinMes(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.ResFengYinDataMessage);
    }
    /// <summary>
    /// 返回特殊活动数据信息，通用，但是需要在各自活动特殊协议之后发送
    /// </summary>
    /// <param name="info"></param>
    void ECM_ResSpecialActivityDataMessage(NetInfo info)
    {
        activity.SpecialActivityData msg = Network.Deserialize<activity.SpecialActivityData>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Foractivity.SpecialActivityData");
            return;
        }
        CSOpenServerACInfo.Instance.ResSpecialActivityData(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.ResSpecialActivityDataMessage, msg);
    }
    void ECM_SCBossFirstKillDatasMessage(NetInfo info)
    {
        activity.BossFirstKillDatasResponse msg = Network.Deserialize<activity.BossFirstKillDatasResponse>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Foractivity.BossFirstKillDatasResponse");
            return;
        }
        CSOpenServerACInfo.Instance.SetBossFirstKillMes(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.SCBossFirstKillDatasMessage);
    }
	void ECM_SCEquipXuanShangMessage(NetInfo info)
	{
		activity.ResEquipXuanShang msg = Network.Deserialize<activity.ResEquipXuanShang>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Foractivity.ResEquipXuanShang");
			return;
		}
		CSEquipRewardsInfo.Instance.SetEquipRewardsMessage(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.GetEquipRewardsInfo);
	}
	void ECM_ResSevenDayDataMessage(NetInfo info)
	{
		activity.SevenDayData msg = Network.Deserialize<activity.SevenDayData>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Foractivity.SevenDayData");
			return;
		}

        CSActivityInfo.Instance.SevenDayData = msg;

    }
	void ECM_ResEquipCompetitionMessage(NetInfo info)
	{
		activity.EquipCompetition msg = Network.Deserialize<activity.EquipCompetition>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Foractivity.EquipCompetition");
			return;
		}
        //Debug.Log("收到军备竞赛数据   " + msg.curGroup + "   " + msg.groupRewards.Count + "    " + msg.datas.Count);
        CSArmRaceInfo.Instance.GetArmRaceDataBack(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.ResEquipCompetitionMessage, msg);
    }

	void ECM_SCBossKuangHuanMessage(NetInfo info)
	{
		activity.ResBossKuangHuan msg = Network.Deserialize<activity.ResBossKuangHuan>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Foractivity.ResBossKuangHuan");
			return;
		}
		CSBossKuangHuanInfo.Instance.SetBossKuangHuanMessage(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.GetBossKuangHuanUpdateInfo);
	}
	void ECM_SCKillDemonMessage(NetInfo info)
	{
		activity.ResKillDemon msg = Network.Deserialize<activity.ResKillDemon>(info);
		if(null != msg)
		{
            CSSpecialActivityMonsterSlayInfo.Instance.SC_ResKillDemon(msg);
            return;
		}
	}
	// void ECM_SCEquipCompetitionBoxMessage(NetInfo info)
	// {
	// 	activity.EquipCompetitionBox msg = Network.Deserialize<activity.EquipCompetitionBox>(info);
	// 	if(null == msg)
	// 	{
	// 		UnityEngine.Debug.LogError("Deserialize Msg Failed Foractivity.EquipCompetitionBox");
	// 		return;
	// 	}
	// }
	void ECM_SCSpecialActivityOpenInfoMessage(NetInfo info)
	{
		activity.SCSpecialActivityOpenInfo msg = Network.Deserialize<activity.SCSpecialActivityOpenInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Foractivity.SCSpecialActivityOpenInfo");
			return;
		}
        CSOpenServerACInfo.Instance.GetServerACtivityOpenState(msg);

    }
	void ECM_SCSevenLoginMessage(NetInfo info)
	{
		activity.ResSevenLogin msg = Network.Deserialize<activity.ResSevenLogin>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Foractivity.ResSevenLogin");
			return;
		}

        FNDebug.LogFormat("<color=#00ff00>[SevenLogin]:ECM_SCSevenLoginMessage</color>");
        CSSevenLoginInfo.Instance.Initialize(msg);
    }
}
