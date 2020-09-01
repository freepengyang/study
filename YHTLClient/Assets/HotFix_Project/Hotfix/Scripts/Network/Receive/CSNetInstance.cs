using instance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class CSNetInstance : CSNetBase
{
    void ECM_ResBuyInstanceTimesMessage(NetInfo info)
    {
        player.RoleExtraValues msg = Network.Deserialize<player.RoleExtraValues>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forinstance.RoleExtraValues");
            return;
        }
    }
    void ECM_ResInstanceInfoMessage(NetInfo info)
    {
        instance.InstanceInfo msg = Network.Deserialize<instance.InstanceInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forinstance.InstanceInfo");
            return;
        }
        FNDebug.Log($"副本变动   {msg.success}   {msg.state}");
        CSInstanceInfo.Instance.GetInstanceInfo(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.ResInstanceInfo);
    }
    void ECM_SCEnterInstanceMessage(NetInfo info)
    {
        instance.InstanceInfo msg = Network.Deserialize<instance.InstanceInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forinstance.InstanceInfo");
            return;
        }
        FNDebug.Log("进入副本");
        CSInstanceInfo.Instance.GetInstanceInfo(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.GetEnterInstanceInfo, msg);
    }
    void ECM_SCInstanceFinishMessage(NetInfo info)
    {
        instance.InstanceInfo msg = Network.Deserialize<instance.InstanceInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forinstance.InstanceInfo");
            return;
        }
        FNDebug.Log("副本结束");
        CSInstanceInfo.Instance.GetInstanceInfo(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.ECM_SCInstanceFinishMessage);
    }
    void ECM_SCLeaveInstanceMessage(NetInfo info)
    {
        /*instance.InstanceInfo msg = Network.Deserialize<instance.InstanceInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forinstance.InstanceInfo");
            return;
        }
        Debug.Log("ECM_SCInstanceFinishMessage  -----------------------------");*/
        FNDebug.Log("离开副本");
        CSInstanceInfo.Instance.GetLeaveInstance();
        CSAutoFightManager.Instance.IsAutoFight = false;
        HotManager.Instance.EventHandler.SendEvent(CEvent.LeaveInstance);
    }
    void ECM_ResQuickInstanceMessage(NetInfo info)
    {
        instance.QuickInstanceInfoMsg msg = Network.Deserialize<instance.QuickInstanceInfoMsg>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forinstance.QuickInstanceInfoMsg");
            return;
        }
        FNDebug.Log("ResQuickInstance");
    }
    void ECM_ResInstanceCountMessage(NetInfo info)
    {
        instance.InstanceCount msg = Network.Deserialize<instance.InstanceCount>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forinstance.InstanceCount");
            return;
        }
        CSInstanceInfo.Instance.GetInstanceCountInfo(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.ResInstanceCountMessage, msg);
    }


    void ECM_ResDiLaoInfoMessage(NetInfo info)
    {
        instance.DiLaoInfo msg = Network.Deserialize<instance.DiLaoInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forinstance.DiLaoInfo");
            return;
        }
        //Debug.LogError("ECM_ResDiLaoInfoMessage :"+  msg.skillId);
        CSInstanceInfo.Instance.ShowDiLaoInfo(msg);

    }
    void ECM_SCUndergroundTreasureMessage(NetInfo info)
    {
        instance.UndergroundTreasureInstanceInfo msg = Network.Deserialize<instance.UndergroundTreasureInstanceInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forinstance.UndergroundTreasureInstanceInfo");
            return;
        }
        CSInstanceInfo.Instance.ShowUndergroundTreasureInfo(msg);
    }
    void ECM_SCBossChallengeMessage(NetInfo info)
    {
        instance.BossChallengeInfo msg = Network.Deserialize<instance.BossChallengeInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forinstance.BossChallengeInfo");
            return;
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.SCBossChallengeMessage, msg);
    }
	void ECM_SCDropLimitMessage(NetInfo info)
	{
		instance.ResDropLimit msg = Network.Deserialize<instance.ResDropLimit>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forinstance.ResDropLimit");
			return;
		}
		CSMapInstanceInfo.Instance.SetLimitMessage(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.RefreshMapMonsterInfo);
	}
}
