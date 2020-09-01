using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class CSNetMap: CSNetBase
{
	public void ECM_ResUpdateViewMessage(NetInfo obj)
    {
        map.UpdateViewResponse rsp = Network.Deserialize<map.UpdateViewResponse>(obj);
        HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_RefreshView, rsp);
        CSMapManager.Instance.InitViewResponse(rsp);
    }
    public void ECM_ResObjectExitViewMessage(NetInfo obj)
    {
        map.ObjectExitViewResponse rsp = Network.Deserialize<map.ObjectExitViewResponse>(obj);
        HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_ExitView, rsp);
    }
    public void ECM_ResPlayerEnterViewMessage(NetInfo obj)
    {
        map.RoundPlayer rsp = Network.Deserialize<map.RoundPlayer>(obj);
        HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_PlayerEnterView, rsp);
    }
    public void ECM_ResMonsterEnterViewMessage(NetInfo obj)
    {
        map.RoundMonster rsp = Network.Deserialize<map.RoundMonster>(obj);
        HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_MonsterEnterView, rsp);
    }

    public void ECM_ResNPCEnterViewMessage(NetInfo obj)
    {
        map.RoundNPC rsp = Network.Deserialize<map.RoundNPC>(obj);
        HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_NpcEnterView, rsp);
    }

    public void ECM_ResAdjustPositionMessage(NetInfo obj)
    {
        map.PositionChangeResponse rsp = Network.Deserialize<map.PositionChangeResponse>(obj);
        HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_PlayerAdjustPosition, rsp);
    }

    public void ECM_ResObjectMoveMessage(NetInfo obj)
    {
        map.ObjectMoveResponse rsp = Network.Deserialize<map.ObjectMoveResponse>(obj);
        HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_ObjectMove, rsp);
    }

    public void ECM_ResPositionChangeMessage(NetInfo obj)
    {
        map.PositionChangeResponse rsp = Network.Deserialize<map.PositionChangeResponse>(obj);
        //Debug.LogFormat("======> ECM_ResPositionChangeMessage: Reason = {0}  Coord = ({1},{2})", rsp.reason, rsp.newX, rsp.newY);
        HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_PlayerAdjustPosition, rsp);
    }
    public void ECM_ResChangeMapMessage(NetInfo obj)
    {
        map.EnterMapResponse rsp = Network.Deserialize<map.EnterMapResponse>(obj);
        //Debug.LogFormat("======> ECM_ResChangeMapMessage: MapID = {0}  Coord = ({1},{2}) reason = {3}", rsp.mapId, rsp.x,rsp.y,rsp.reason);
        HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_ChangeMap, rsp);
    }

	void ECM_ResEnterMapMessage(NetInfo info)
	{
        map.EnterMapResponse msg = Network.Deserialize<map.EnterMapResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.EnterMapResponse");
			return;
		}

        HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_EnterScene, msg);
	}
	void ECM_ResItemEnterViewMessage(NetInfo info)
	{
		map.RoundItem msg = Network.Deserialize<map.RoundItem>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.RoundItem");
			return;
		}
        HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_ItemEnterView, msg);
    }
	
	/// <summary>
	/// 死亡复活响应
	/// </summary>
	/// <param name="info"></param>
	void ECM_ResReliveMessage(NetInfo info)
	{
		map.ReliveResponse msg = Network.Deserialize<map.ReliveResponse>(info);
		// CSReliveInfo.Instance.GetReliveMessage(msg);
		CSAvatar avatar = CSAvatarManager.Instance.GetAvatar(msg.id);

		if(avatar != null)
		{
			avatar.BaseInfo.HP = msg.hp;
            avatar.BaseInfo.RealHP = msg.hp;
		}
		HotManager.Instance.EventHandler.SendEvent(CEvent.Relive, msg);
		if (msg.id == CSMainPlayerInfo.Instance.ID)
			HotManager.Instance.EventHandler.SendEvent(CEvent.ReliveMainPlayer, msg);
	}
	void ECM_ResPlayerHPChangedMessage(NetInfo info)
	{
		map.PlayerHPChanged msg = Network.Deserialize<map.PlayerHPChanged>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.PlayerHPChanged");
			return;
		}
		if(CSAvatarManager.Instance == null) return;
        CSAvatar avatar = CSAvatarManager.Instance.GetAvatar(msg.id);
        if(avatar != null)
        {
            CSAvatarInfo avatarInfo = avatar.BaseInfo;
            if (avatar.BaseInfo != null)
            {
                avatarInfo.RealHP = msg.hp;
                avatarInfo.MP = msg.mp;
                if(msg.maxHp > 0)
                {
                    avatarInfo.MaxHP = msg.maxHp;
                }
                if (msg.reason == (int)EHpChangeReason.Fusion || msg.reason == (int)EHpChangeReason.BuffDamage)
                {
                    CSHurtManager.Instance.ShowHp(avatar, msg.hp - avatarInfo.HP,false);
                }
                if (msg.reason == (int)EHpChangeReason.NpcRecover)
                {
                    UtilityTips.ShowTips(2046);
                }
                avatarInfo.HP = msg.hp;

            }
        }
	}
	void ECM_ResBufferEnterViewMessage(NetInfo info)
	{
		map.RoundBuffer msg = Network.Deserialize<map.RoundBuffer>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.RoundBuffer");
			return;
		}
        HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_BufferEnterView, msg);
    }
	void ECM_ResItemsDropMessage(NetInfo info)
	{
		map.ItemsDropResponse msg = Network.Deserialize<map.ItemsDropResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.ItemsDropResponse");
			return;
		}
	}
	void ECM_ResItemOwnerChangedMessage(NetInfo info)
	{
		map.RoundItem msg = Network.Deserialize<map.RoundItem>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.RoundItem");
			return;
		}
	}
	void ECM_ResPetEnterViewMessage(NetInfo info)
	{
		map.RoundPet msg = Network.Deserialize<map.RoundPet>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.RoundPet");
			return;
		}
		CSAvatar avatar = CSAvatarManager.Instance.GetAvatar(msg.petId);
		if (avatar != null && avatar.BaseInfo != null)
		{
			CSPetInfo petInfo = avatar.BaseInfo as CSPetInfo;
			if (petInfo != null)
			{
				petInfo.Init(msg);
			}
		}
        HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_PetEnterView, msg);
    }
	void ECM_ResGuardEnterViewMessage(NetInfo info)
	{
		map.RoundGuard msg = Network.Deserialize<map.RoundGuard>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.RoundGuard");
			return;
		}
	}
	void ECM_ResPetStateChangedMessage(NetInfo info)
	{
		map.PetStateChanged msg = Network.Deserialize<map.PetStateChanged>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.PetStateChanged");
			return;
		}

        CSAvatarInfo avatarInfo = CSAvatarManager.Instance.GetAvatarInfo(msg.id);
        if(avatarInfo != null)
        {
            CSPetInfo petInfo = avatarInfo as CSPetInfo;
            if(petInfo != null)
            {
                petInfo.State = msg.state;
            }
        }
	}
	void ECM_ResTriggerEnterViewMessage(NetInfo info)
	{
		map.RoundTrigger msg = Network.Deserialize<map.RoundTrigger>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.RoundTrigger");
			return;
		}

		FNDebug.Log($"<color=#00ff00>[触发器进入视野]:id:{msg.triggerId} configId:{msg.triggerConfigId} isSabacDoor:{msg.isSabacDoor}</color>");
		HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_TriggerEnterView, msg);
	}
	void ECM_ResObjectChangePositionMessage(NetInfo info)
	{
		map.ObjectMoveResponse msg = Network.Deserialize<map.ObjectMoveResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.ObjectMoveResponse");
			return;
		}
        HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_ObjectChangePosition, msg);

    }
    void ECM_ResBossOwnerChangedMessage(NetInfo info)
	{
		map.RoundMonster msg = Network.Deserialize<map.RoundMonster>(info);
		if(CSAvatarManager.Instance == null) return;
        CSAvatarInfo avatarInfo = CSAvatarManager.Instance.GetAvatarInfo(msg.monsterId);
        if(avatarInfo != null)
        {
            CSMonsterInfo monsterInfo = avatarInfo as CSMonsterInfo;
            if(monsterInfo != null)
            {
                monsterInfo.MonsterOwner = msg.owner;
                monsterInfo.EventHandler.SendEvent(CEvent.MonsterOwner_Change);
            }
        }
	}

	void ECM_ResBoxEnterViewMessage(NetInfo info)
	{
		map.RoundBox msg = Network.Deserialize<map.RoundBox>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.RoundBox");
			return;
		}
	}
	void ECM_ResMapBossMessage(NetInfo info)
	{
		map.MapBossInfo msg = Network.Deserialize<map.MapBossInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.MapBossInfo");
			return;
		}
	}
	void ECM_ResWeatherChangeMessage(NetInfo info)
	{
		map.WeatherChangeResponse msg = Network.Deserialize<map.WeatherChangeResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.WeatherChangeResponse");
			return;
		}
	}
	void ECM_SmallViewTeammateNtfMessage(NetInfo info)
	{
		map.SmallViewTeammateNtf msg = Network.Deserialize<map.SmallViewTeammateNtf>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.SmallViewTeammateNtf");
			return;
		}
		CSMapManager.Instance.UpdateTeamViewPosition(msg);
	}
	void ECM_MonsterAppearanceChangedNtfMessage(NetInfo info)
	{
		map.RoundMonster msg = Network.Deserialize<map.RoundMonster>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.RoundMonster");
			return;
		}
	}
	void ECM_NpcsStatNtfMessage(NetInfo info)
	{
		map.NpcsStatNtf msg = Network.Deserialize<map.NpcsStatNtf>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.NpcsStatNtf");
			return;
		}
	}
	void ECM_PlayerStateNtfMessage(NetInfo info)
	{
		map.PlayerStateNtf msg = Network.Deserialize<map.PlayerStateNtf>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.PlayerStateNtf");
			return;
		}
	}
	void ECM_ResPetShapeChangeNtf(NetInfo info)
	{
		map.RoundPet msg = Network.Deserialize<map.RoundPet>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.RoundPet");
			return;
		}
	}
	void ECM_SCMapDetailsMessage(NetInfo info)
	{
		map.MapDetails msg = Network.Deserialize<map.MapDetails>(info);
		if (null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.MapDetails");
			return;
		}
		CSRandomThingInstanceInfo.Instance.SetMapDetailsMessage(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.GetMapDescInfo);
	}
	void ECM_SCGoldKeyPickUpItemMessage(NetInfo info)
	{
		map.GoldKeyPickUpItems msg = Network.Deserialize<map.GoldKeyPickUpItems>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.GoldKeyPickUpItems");
			return;
		}
		CSRandomThingInstanceInfo.Instance.SetGoldKeyPickUpItemMessage(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.GetGoldKeyPickUpItemList);
	}
	void ECM_SCMainTaskTransmitEventMessage(NetInfo info)
	{
		map.MainTaskTransmitEvent msg = Network.Deserialize<map.MainTaskTransmitEvent>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.MainTaskTransmitEvent");
			return;
		}
        CSMainPlayerInfo.Instance.MainTaskEventIds = msg.evevtIds;
    }
	void ECM_SCAddMainTaskTransmitEventMessage(NetInfo info)
	{
		map.AddMainTaskTransmitEvent msg = Network.Deserialize<map.AddMainTaskTransmitEvent>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.AddMainTaskTransmitEvent");
			return;
		}
        CSMainPlayerInfo.Instance.AddEventId(msg.evevtIds);
        HotManager.Instance.EventHandler.SendEvent(CEvent.MainTask_AddTransmitEvent,msg.evevtIds);

    }
    void ECM_SCRemoveMainTaskTransmitEventMessage(NetInfo info)
	{
		map.RemoveMainTaskTransmitEvent msg = Network.Deserialize<map.RemoveMainTaskTransmitEvent>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Formap.RemoveMainTaskTransmitEvent");
			return;
		}
        CSMainPlayerInfo.Instance.RemoveEventId(msg.evevtIds);
        HotManager.Instance.EventHandler.SendEvent(CEvent.MainTask_RemoveTransmitEvent, msg.evevtIds);

    }
	void ECM_ResSafeAreaCoordEnterViewMessage(NetInfo info)
	{
		map.RoundSafeAreaCoord msg = Network.Deserialize<map.RoundSafeAreaCoord>(info);
		if(null == msg)
		{
			FNDebug.LogError("Deserialize Msg Failed Formap.RoundSafeAreaCoord");
			return;
		}
        HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_SafeAreaCoordView, msg);
    }
}