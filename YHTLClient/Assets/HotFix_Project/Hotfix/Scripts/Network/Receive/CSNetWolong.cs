using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wolong;

public partial class CSNetWolong : CSNetBase
{
    void ECM_SCWoLongInfoMessage(NetInfo info)
    {
        wolong.WoLongInfo msg = Network.Deserialize<wolong.WoLongInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forwolong.WoLongInfo");
            return;
        }
        CSWoLongInfo.Instance.GetWoLongInfo(msg);
        CSItemCountManager.Instance.InitWolongEquips();
        HotManager.Instance.EventHandler.SendEvent(CEvent.WoLongLevelUpgrade, msg);
    }
    void ECM_SCWoLongLevelUpMessage(NetInfo info)
    {
        wolong.WoLongLevelUpgradeResponse msg = Network.Deserialize<wolong.WoLongLevelUpgradeResponse>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forwolong.WoLongLevelUpgradeResponse");
            return;
        }
        CSWoLongInfo.Instance.GetWoLongInfo(msg.info);
        HotManager.Instance.EventHandler.SendEvent(CEvent.WoLongLevelUpgrade, msg);
    }

    void ECM_SCWoLongXiLianMessage(NetInfo info)
    {
        wolong.WoLongXiLianResponse msg = Network.Deserialize<wolong.WoLongXiLianResponse>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forwolong.WoLongXiLianResponse");
            return;
        }
        CSBagInfo.Instance.GetWoLongLongJiBack(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.SCWoLongXiLianMessage, msg);
    }
    void ECM_SCWoLongXiLianSelectMessage(NetInfo info)
    {
        wolong.WoLongXiLianSelectResponse msg = Network.Deserialize<wolong.WoLongXiLianSelectResponse>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forwolong.WoLongXiLianSelectResponse");
            return;
        }
        CSBagInfo.Instance.GetWoLongLongJiConfirmBack(msg);
        //CSBagInfo.Instance.GetEuqipRecastRes(msg.result);
        HotManager.Instance.EventHandler.SendEvent(CEvent.SCWoLongXiLianSelectMessage, msg);
    }
	void ECM_SCSoldierSoulInfoAwakenMessage(NetInfo info)
	{
		wolong.SoldierSoulInfo msg = Network.Deserialize<wolong.SoldierSoulInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forwolong.SoldierSoulInfo");
			return;
		}
		CSWoLongInfo.Instance.GetPetAwakeDataListNew(msg);
		//HotManager.Instance.EventHandler.SendEvent(CEvent.GetPetAwakeUpdateInfo);
	}
	void ECM_SCSoldierSoulInfoMessage(NetInfo info)
	{
		wolong.SoldierSoulInfoResponse msg = Network.Deserialize<wolong.SoldierSoulInfoResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forwolong.SoldierSoulInfoResponse");
			return;
		}
		CSWoLongInfo.Instance.GetPetAwakeDataList(msg);
	}
	void ECM_SCSkillGroupInfoMessage(NetInfo info)
	{
		wolong.SkillGroupInfoResponse msg = Network.Deserialize<wolong.SkillGroupInfoResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forwolong.SkillGroupInfoResponse");
			return;
		}
        CSWoLongInfo.Instance.GetWoLongEnabledSkill(msg);
    }
}
