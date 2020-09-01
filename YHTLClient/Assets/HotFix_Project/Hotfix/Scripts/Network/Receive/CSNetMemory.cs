public partial class CSNetMemory : CSNetBase
{
	void ECM_ResMemoryInstanceInfoMessage(NetInfo info)
	{
		memory.MemoryInstanceInfo msg = Network.Deserialize<memory.MemoryInstanceInfo>(info);
		if (null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formemory.MemoryInstanceInfo");
			return;
		}

		CSSecretAreaInfo.Instance.SetMemoryInstanceInfo(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.SecretAreaFreeInstance);
	}

	void ECM_ResMemoryBagMessage(NetInfo info)
	{
		memory.MemoryBag msg = Network.Deserialize<memory.MemoryBag>(info);
		if (null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formemory.MemoryBag");
			return;
		}
		CSNostalgiaEquipInfo.Instance.GetziIndex = msg.gezi;
		CSNostalgiaEquipInfo.Instance.AddBagList(msg.bagItems, true);
		
	}

	void ECM_ResMemoryEquipInfoMessage(NetInfo info)
	{
		memory.MemoryEquipInfo msg = Network.Deserialize<memory.MemoryEquipInfo>(info);
		if (null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formemory.MemoryEquipInfo");
			return;
		}

		CSNostalgiaEquipInfo.Instance.AddEquipList(msg.equips, true);
		HotManager.Instance.EventHandler.SendEvent(CEvent.NostalgiaEquipChange);
	}

	void ECM_ResMemoryAddMessage(NetInfo info)
	{
		memory.MemoryAdd msg = Network.Deserialize<memory.MemoryAdd>(info);
		if (null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formemory.MemoryAdd");
			return;
		}

		CSNostalgiaEquipInfo.Instance.BagListChange(msg);

		HotManager.Instance.EventHandler.SendEvent(CEvent.NostalgiaBagChange);
	}

	void ECM_ResMemoryEquipChangeMessage(NetInfo info)
	{
		memory.MemoryEquipChange msg = Network.Deserialize<memory.MemoryEquipChange>(info);
		if (null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formemory.MemoryEquipChange");
			return;
		}
	    //装备穿脱
		CSNostalgiaEquipInfo.Instance.EquipListChange(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.NostalgiaEquipChange);
	}

	void ECM_ResMemoryEquipSuitMessage(NetInfo info)
	{
		memory.MemoryEquipSuit msg = Network.Deserialize<memory.MemoryEquipSuit>(info);
		if (null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formemory.MemoryEquipSuit");
			return;
		}

		CSNostalgiaEquipInfo.Instance.SuitChange(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.NostalsuitChange);

	}

	void ECM_ResMemoryEquipGeziChangeMessage(NetInfo info)
	{
		memory.MemoryEquipGeziChange msg = Network.Deserialize<memory.MemoryEquipGeziChange>(info);
		if (null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formemory.MemoryEquipGeziChange");
			return;
		}
		
		CSNostalgiaEquipInfo.Instance.GetziIndex = msg.gezi;
		HotManager.Instance.EventHandler.SendEvent(CEvent.NostalGeziChange);
	}

	void ECM_ResMemoryRemoveMessage(NetInfo info)
		{
			memory.MemoryRemove msg = Network.Deserialize<memory.MemoryRemove>(info);
			if (null == msg)
			{
				FNDebug.LogError("Deserialize Msg Failed Formemory.MemoryRemove");
				return;
			}
			CSNostalgiaEquipInfo.Instance.RemoveBagList(msg.changeList);
			HotManager.Instance.EventHandler.SendEvent(CEvent.NostalgiaBagChange);
		}
	void ECM_ResDiscardMemoryEquipMessage(NetInfo info)
	{
		memory.MemoryEquipId msg = Network.Deserialize<memory.MemoryEquipId>(info);
		if(null == msg)
		{
			FNDebug.LogError("Deserialize Msg Failed Formemory.MemoryEquipId");
			return;
		}
		CSNostalgiaEquipInfo.Instance.RemoveItem(msg.lid);
		HotManager.Instance.EventHandler.SendEvent(CEvent.NostalgiaRemove);
	}
	void ECM_ResMemorySummonTeamMessage(NetInfo info)
	{
		memory.MemorySummonTeam msg = Network.Deserialize<memory.MemorySummonTeam>(info);
		if(null == msg)
		{
			FNDebug.LogError("Deserialize Msg Failed Formemory.MemorySummonTeam");
			return;
		}

		var mapName = MapInfoTableManager.Instance.GetMapInfoName(msg.mapId);
		string str = CSString.Format(1993,msg.name,mapName);
		
		CSSummonMgr.Instance.ShowSummon(str, (s, d) =>
		{
			if (s == (int)MsgBoxType.MBT_OK)
			{
				Net.ReqMemoryGotoSummonMessage(msg.rid);
			}
		}, SummonType.NostalgiaTeam, 8,msg.rid);
		
		
	}
	void ECM_ResMemorySummonTeamCdMessage(NetInfo info)
	{
		memory.MemorySummonTeamCd msg = Network.Deserialize<memory.MemorySummonTeamCd>(info);
		if(null == msg)
		{
			FNDebug.LogError("Deserialize Msg Failed Formemory.MemorySummonTeamCd");
			return;
		}
		CSNostalgiaEquipInfo.Instance.nextSummonTime = msg.nextSummonTime;
		HotManager.Instance.EventHandler.SendEvent(CEvent.NostalgiaRefreshTime);
		
	}
}
