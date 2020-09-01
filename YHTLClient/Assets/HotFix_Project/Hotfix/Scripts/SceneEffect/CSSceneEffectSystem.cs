using Google.Protobuf.Collections;
using map;
using paodian;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSceneEffectSystem : CSInfo<CSSceneEffectSystem>
{
    private int wearEquipEffectLevel = 60;
    public void Init()
    {
        mClientEvent.AddEvent(CEvent.MainPlayer_LevelChange, OnRoleUpgrade);
        mClientEvent.AddEvent(CEvent.SCRandomPaoDianMessage, OnSCRandomPaoDianMessage);
        mClientEvent.AddEvent(CEvent.MainTask_AddTransmitEvent, OnAddTransmitEvent);
        mClientEvent.AddEvent(CEvent.MainTask_RemoveTransmitEvent, OnRemoveTransmitEvent);
        mClientEvent.AddEvent(CEvent.Scene_EnterSceneAfter, OnEffectEnterSceneAfter);
        mClientEvent.AddEvent(CEvent.OnPickupItemPlayEffect, OnPickupItemPlayEffect);
        wearEquipEffectLevel = SundryTableManager.GetValue(634);
        if (CSMainPlayerInfo.Instance.Level < wearEquipEffectLevel)
        {
            mClientEvent.AddEvent(CEvent.WearEquip, OnWearEquip);
        }
        InitMainTaskTransmitEffect(CSMainPlayerInfo.Instance.MainTaskEventIds);
    }

    private void InitMainTaskTransmitEffect(RepeatedField<int> eventIds)
    {
        if (eventIds == null)
        {
            return;
        }
        int mapId = CSMainPlayerInfo.Instance.MapID;
        for (int i = 0; i < eventIds.Count; ++i)
        {
            TABLE.EVENT tblEvent = null;
            int eventId = eventIds[i];
            if (EventTableManager.Instance.TryGetValue(eventId, out tblEvent))
            {
                if (mapId == tblEvent.mapId)
                {
                    CSSceneEffectMgr.Instance.PlayEffectWaitDeal(CSPreLoadingBase.Instance.EffectAnchor, eventId, eventId, OnAddTransmit);
                }
            }
        }
    }

    private void OnRoleUpgrade(uint evtId, object obj)
    {
        if(CSAvatarManager.MainPlayer == null)
        {
            return;
        }
        Transform anchor =CSAvatarManager.MainPlayer.Model.Effect.GoTrans;
        CSSceneEffectMgr.Instance.PlayEffect(anchor, 6009);
        CSSceneEffectMgr.Instance.PlayEffect(anchor, 6010);
    }

	private void OnSCRandomPaoDianMessage(uint evtId, object obj)
    {
        if (obj == null)
        {
            return;
        }
        RandomPaoDian info = obj as RandomPaoDian;
        if(info == null)
        {
            return;
        }
        for (int i = 0; i < info.paoDianPoints.Count; ++i)
        {
            RandomPaoDianData randomPaoDian = info.paoDianPoints[i];
            CSSceneEffect effect = CSSceneEffectMgr.Instance.GetEffect(randomPaoDian.configId);
            if(effect != null)
            {
                effect.UpdatePosition(randomPaoDian.x,randomPaoDian.y);
            }
            else
            {
                TABLE.PAODIAN tblPaoDian = null;
                if(PaoDianTableManager.Instance.TryGetValue(randomPaoDian.configId, out tblPaoDian))
                {
                    CSSceneEffectMgr.Instance.PlayEffect(CSPreLoadingBase.Instance.EffectAnchor, randomPaoDian.configId, tblPaoDian.effectId,randomPaoDian.x,randomPaoDian.y);
                }
            }
        }
    }

    private void OnAddTransmitEvent(uint evetId, object obj)
    {
        if(obj != null)
        {
            RepeatedField<int> eventIds = (RepeatedField<int>)obj;
            InitMainTaskTransmitEffect(eventIds);
        }
    }

    private void OnRemoveTransmitEvent(uint evtId, object obj)
    {
        if (obj != null)
        {
            RepeatedField<int> eventIds = (RepeatedField<int>)obj;
            if (eventIds == null)
            {
                return;
            }
            for(int i = 0; i < eventIds.Count; ++i)
            {
                CSSceneEffectMgr.Instance.Remove(eventIds[i]);
            };
        }
    }

    private void OnEffectEnterSceneAfter(uint evtId, object obj)
    {
        InitMainTaskTransmitEffect(CSMainPlayerInfo.Instance.MainTaskEventIds);
    }

    private bool OnAddTransmit(object obj, object param)
    {
        if(obj != null)
        {
            int transmitId = (int)obj;

            TABLE.EVENT tblEvent;
            if(EventTableManager.Instance.TryGetValue(transmitId, out tblEvent))
            {
                CSSceneEffectMgr.Instance.PlayEffect(CSPreLoadingBase.Instance.EffectAnchor, transmitId, 6008, tblEvent.x, tblEvent.y);
                return true;
            }
        }
        return false;
    }


    void OnPickupItemPlayEffect(uint id, object param)
    {
        bag.PickupMsg info = param as bag.PickupMsg;
        if (info == null)
        {
            FNDebug.LogErrorFormat("[PickItem]:Failed info is Null");
            return;
        }
        var instance = CSDropManager.Instance;
        if (null == instance)
        {
            FNDebug.LogErrorFormat("[PickItem]:CSDropManager is Null");
            return;
        }
        CSItem item = instance.GetItem(info.id);
        if (null == item)
        {
            FNDebug.LogErrorFormat("[PickItem]:Failed id = {0} itemTbl is Null", info.id);
            return;
        }

        if (item.itemTbl.id != 50000029) return;
        Transform anchor =CSAvatarManager.MainPlayer.Model.Effect.GoTrans;
        CSSceneEffectMgr.Instance.PlayEffect(anchor, 6017);
    }

    private void OnWearEquip(uint uiEvtId, object obj)
    {
        if(CSMainPlayerInfo.Instance.Level < wearEquipEffectLevel)
        {
            Transform anchor = CSAvatarManager.MainPlayer.Model.Effect.GoTrans;
            CSSceneEffectMgr.Instance.PlayParticleEffect(anchor, 6066);
        }
    }

    public override void Dispose()
    {
    }
}
