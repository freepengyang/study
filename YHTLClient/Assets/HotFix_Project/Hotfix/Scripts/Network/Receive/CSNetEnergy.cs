using energy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class CSNetEnergy : CSNetBase
{
	void ECM_SCEnergyInfoMessage(NetInfo info)
	{
		energy.EnergyInfoResponse msg = Network.Deserialize<energy.EnergyInfoResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forenergy.EnergyInfoResponse");
			return;
		}

        CSVigorInfo.Instance.SetVigorInfo(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.SCEnergyInfoMessage);
    }
	void ECM_SCEnergyFreeGetInfoMessage(NetInfo info)
	{
		energy.EnergyFreeGetInfoResponse msg = Network.Deserialize<energy.EnergyFreeGetInfoResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forenergy.EnergyFreeGetInfoResponse");
			return;
		}

        CSVigorInfo.Instance.SetVigorFreeIds(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.SCEnergyFreeGetInfoMessage);
    }
	void ECM_SCGetFreeEnergyMessage(NetInfo info)
	{
		energy.GetFreeEnergyRequest msg = Network.Deserialize<energy.GetFreeEnergyRequest>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forenergy.GetFreeEnergyRequest");
			return;
		}
        CSVigorInfo.Instance.GetExchangeVigor(msg.timerId);
        HotManager.Instance.EventHandler.SendEvent(CEvent.SCGetFreeEnergyMessage, msg.timerId);
    }
	void ECM_SCNotifyEnergyChangeMessage(NetInfo info)
	{
		energy.NotifyEnergyChangeResponse msg = Network.Deserialize<energy.NotifyEnergyChangeResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forenergy.NotifyEnergyChangeResponse");
			return;
		}
        CSVigorInfo.Instance.SetVigorValue(msg.energy);
        HotManager.Instance.EventHandler.SendEvent(CEvent.SCNotifyEnergyChangeMessage);
    }
	void ECM_SCEnergyExchangeInfoMessage(NetInfo info)
	{
		energy.EnergyExchangeInfo msg = Network.Deserialize<energy.EnergyExchangeInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forenergy.EnergyExchangeInfo");
			return;
		}
        CSVigorInfo.Instance.SetEnergyExchangeInfo(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.SCEnergyExchangeInfoMessage);
    }
}
