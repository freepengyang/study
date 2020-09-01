public partial class CSNetVip : CSNetBase
{
	void ECM_ResVipMessage(NetInfo info)
	{
        //Debug.Log("ECM_ResVipMessage");
		vip.VipInfo msg = Network.Deserialize<vip.VipInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forvip.VipInfo");
			return;
		}
        //Debug.Log(msg.exp);
        CSVipInfo.Instance.SetRewardInfos(msg);
        CSVipInfo.Instance.VipInfo = msg;
    }

	void ECM_ResVipTasteCardMessage(NetInfo info)
	{
        //Debug.Log("ECM_ResVipTasteCardMessage");
		vip.ExperienceCardInfo msg = Network.Deserialize<vip.ExperienceCardInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forvip.ExperienceCardInfo");
			return;
		}
        CSVipInfo.Instance.GetVipTasteCardInfo(msg);

    }

	void ECM_SCFirstRechargeInfoMessage(NetInfo info)
	{
        //Debug.Log("ECM_SCFirstRechargeInfoMessage");
		vip.FirstRechargeInfoResponse msg = Network.Deserialize<vip.FirstRechargeInfoResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forvip.FirstRechargeInfoResponse");
			return;
		}

        CSVipInfo.Instance.FirstRechargeInfo = msg;
    }
	void ECM_SCVipTasteCardMessage(NetInfo info)
	{
		vip.ExperienceCardInfo msg = Network.Deserialize<vip.ExperienceCardInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forvip.ExperienceCardInfo");
			return;
		}
        CSVipInfo.Instance.ExperienceCardInfo = msg;
    }
	void ECM_FirstRechargeNtfMessage(NetInfo info)
	{
		vip.FirstRechargeNtf msg = Network.Deserialize<vip.FirstRechargeNtf>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forvip.FirstRechargeNtf");
			return;
		}
	}
	void ECM_AccumulatedRechargeInfoMessage(NetInfo info)
	{
		vip.AccumulatedRechargeInfo msg = Network.Deserialize<vip.AccumulatedRechargeInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forvip.AccumulatedRechargeInfo");
			return;
		}

		CSVipInfo.Instance.GetAccumulateInfo(msg);

	}
	void ECM_SCMonthRechargeInfoMessage(NetInfo info)
	{
		vip.ResMonthRechargeInfo msg = Network.Deserialize<vip.ResMonthRechargeInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forvip.ResMonthRechargeInfo");
			return;
		}

        CSRechargeInfo.Instance.SC_RechargeInfo(msg);
	}
	void ECM_SCMonthRechargeInfoByIdMessage(NetInfo info)
	{
		vip.MonthRechargeInfo msg = Network.Deserialize<vip.MonthRechargeInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forvip.MonthRechargeInfo");
			return;
		}

        CSRechargeInfo.Instance.SC_RechargeRes(msg);
	}
}
