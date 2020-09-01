public partial class CSNetTujian : CSNetBase
{
	void ECM_SCTujianInfoMessage(NetInfo info)
	{
		tujian.TujianInfoResponse msg = Network.Deserialize<tujian.TujianInfoResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fortujian.TujianInfoResponse");
			return;
		}

		CSHandBookManager.Instance.ResetSlotInfos(msg.tujianSlot,msg.tujianInfos);
	}
	void ECM_SCTujianUpLevelMessage(NetInfo info)
	{
		tujian.TujianUpLevelResponse msg = Network.Deserialize<tujian.TujianUpLevelResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fortujian.TujianUpLevelResponse");
			return;
		}

		UtilityTips.ShowGreenTips(682);
		CSHandBookManager.Instance.UpgradeHandBook(msg.tujianInfo);
	}
	void ECM_SCTujianUpQualityMessage(NetInfo info)
	{
		tujian.TujianUpQualityResponse msg = Network.Deserialize<tujian.TujianUpQualityResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fortujian.TujianUpQualityResponse");
			return;
		}

		UtilityTips.ShowGreenTips(683);
		CSHandBookManager.Instance.UpgradeHandBook(msg.tujianInfo);
	}
	void ECM_SCTujianInlayMessage(NetInfo info)
	{
		tujian.TujianInlayResponse msg = Network.Deserialize<tujian.TujianInlayResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fortujian.TujianInlayResponse");
			return;
		}
		CSHandBookManager.Instance.InlayHandBook(msg.tujianInfo);
	}
	void ECM_SCActivateSlotWingMessage(NetInfo info)
	{
		tujian.ActivateSlotResponse msg = Network.Deserialize<tujian.ActivateSlotResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fortujian.ActivateSlotResponse");
			return;
		}
		UtilityTips.ShowGreenTips(742);
		CSHandBookManager.Instance.UpdateSlotInfo(msg.tujianSlot);
	}
	void ECM_SCTujianAddMessage(NetInfo info)
	{
		tujian.TujianAddResponse msg = Network.Deserialize<tujian.TujianAddResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fortujian.TujianAddResponse");
			return;
		}
		CSHandBookManager.Instance.AddHandBook(msg.tujianInfo);
	}
	void ECM_SCTujianRemoveMessage(NetInfo info)
	{
		tujian.TujianRemoveResponse msg = Network.Deserialize<tujian.TujianRemoveResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Fortujian.TujianRemoveResponse");
			return;
		}
		CSHandBookManager.Instance.RemoveHandBook(msg.tujianInfo);
	}
}
