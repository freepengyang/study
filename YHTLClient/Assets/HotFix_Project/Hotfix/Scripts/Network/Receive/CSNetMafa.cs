public partial class CSNetMafa : CSNetBase
{
	void ECM_ResMafaInfoMessage(NetInfo info)
	{
		mafa.MafaInfo msg = Network.Deserialize<mafa.MafaInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formafa.MafaInfo");
			return;
		}
		CSMaFaInfo.Instance.SetActiveInfo(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.RefreshMaFaLayerInfo);
	}
	void ECM_ResMafaExpChangeMessage(NetInfo info)
	{
		mafa.MafaExpChange msg = Network.Deserialize<mafa.MafaExpChange>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formafa.MafaExpChange");
			return;
		}
		CSMaFaInfo.Instance.MafaExpChange(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.RefreshMaFaLayerInfo);
	}
	void ECM_ResMafaLayerChangeMessage(NetInfo info)
	{
		mafa.MafaLayerList msg = Network.Deserialize<mafa.MafaLayerList>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formafa.MafaLayerList");
			return;
		}
		CSMaFaInfo.Instance.SetMafaLayerChange(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.RefreshMaFaLayerInfo);
	}
	void ECM_ResMafaSuperLayerUnlockMessage(NetInfo info)
	{
		mafa.MafaSuperLayerUnlock msg = Network.Deserialize<mafa.MafaSuperLayerUnlock>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formafa.MafaSuperLayerUnlock");
			return;
		}
		CSMaFaInfo.Instance.MafaSuperLayerUnlock(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.RefreshMaFaLayerInfo);
	}
	void ECM_ResMafaBoxRewardMessage(NetInfo info)
	{
		mafa.MafaBoxReward msg = Network.Deserialize<mafa.MafaBoxReward>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formafa.MafaBoxReward");
			return;
		}
		CSMaFaInfo.Instance.MafaBoxRewardMessage(msg);
		UIManager.Instance.CreatePanel<UIMaFaPromptPanel>(p=>
		{
			(p as UIMaFaPromptPanel).RefreshRewardsUI((int)UIMaFaPromptPanel.State.rewards);
		});
		HotManager.Instance.EventHandler.SendEvent(CEvent.RefreshMaFaBoxInfo);
	}
}
