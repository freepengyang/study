public partial class CSNetUltimate : CSNetBase
{
	void ECM_SCUltimateInfoMessage(NetInfo info)
	{
		ultimate.RoleUltimateData msg = Network.Deserialize<ultimate.RoleUltimateData>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forultimate.RoleUltimateData");
			return;
		}
		CSUltimateInfo.Instance.InitUltimate(msg);
	}
	void ECM_SCResetUltimateMessage(NetInfo info)
	{
		ultimate.RoleUltimateData msg = Network.Deserialize<ultimate.RoleUltimateData>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forultimate.RoleUltimateData");
			return;
		}
		CSUltimateInfo.Instance.InitUltimate(msg);
	}
	
	//发送增益效果属性选择
	void ECM_SCSelectAdditionEffectMessage(NetInfo info)
	{
		ultimate.SelectAdditionEffect msg = Network.Deserialize<ultimate.SelectAdditionEffect>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forultimate.SelectAdditionEffect");
			return;
		}

		CSUltimateInfo.Instance.RefreshSelectAddition(msg);
	}
	void ECM_SCRankInfoMessage(NetInfo info)
	{
		ultimate.ResponseRankInfo msg = Network.Deserialize<ultimate.ResponseRankInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forultimate.ResponseRankInfo");
			return;
		}
		CSUltimateInfo.Instance.InitRankInfo(msg);
	}
	void ECM_SCSelectCardEffectMessage(NetInfo info)
	{
		ultimate.SelectAdditionEffect msg = Network.Deserialize<ultimate.SelectAdditionEffect>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forultimate.SelectAdditionEffect");
			return;
		}
	}
	void ECM_SCSelectAdditionIndexMessage(NetInfo info)
	{
		ultimate.OpState msg = Network.Deserialize<ultimate.OpState>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forultimate.OpState");
			return;
		}
		HotManager.Instance.EventHandler.SendEvent(CEvent.UltimateSelectAdditionIndexMessage, msg.state);
	}
	void ECM_SCResponseCardMessage(NetInfo info)
	{
		ultimate.SelectAdditionEffect msg = Network.Deserialize<ultimate.SelectAdditionEffect>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forultimate.SelectAdditionEffect");
			return;
		}

		CSUltimateInfo.Instance.ShowUltimateCard(msg);
	}
	void ECM_SCOpenCardInfoMessage(NetInfo info)
	{
		ultimate.OpState msg = Network.Deserialize<ultimate.OpState>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forultimate.OpState");
			return;
		}
		HotManager.Instance.EventHandler.SendEvent(CEvent.UltimateOpenCardInfoMessage, msg.state);
	}
	void ECM_SCSelectCardIndexMessage(NetInfo info)
	{
		ultimate.OpState msg = Network.Deserialize<ultimate.OpState>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forultimate.OpState");
			return;
		}
		CSUltimateInfo.Instance.SelectCardFinish(msg.state);
		HotManager.Instance.EventHandler.SendEvent(CEvent.UltimateSelectCardIndexMessage, msg.state);
	}
	void ECM_SCGodBlessMessage(NetInfo info)
	{
		ultimate.AddHp msg = Network.Deserialize<ultimate.AddHp>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forultimate.AddHp");
			return;
		}
	}
	void ECM_SCUltimatePassInfoMessage(NetInfo info)
	{
		ultimate.UltimatePassInfo msg = Network.Deserialize<ultimate.UltimatePassInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forultimate.UltimatePassInfo");
			return;
		}

		CSUltimateInfo.Instance.RefreshPassInfo(msg);
	}
}
