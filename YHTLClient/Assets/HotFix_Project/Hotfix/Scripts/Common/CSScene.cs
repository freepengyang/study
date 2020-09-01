

//-------------------------------------------------------------------------
//scene manager
//Author jiabao
//Time 2015.12.15
//-------------------------------------------------------------------------

using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using map;

public class CSScene : MonoBehaviour
{
    public EventHanlderManager mSocket = null;
    public EventHanlderManager mClient = null;
    private static CSCharacter mMainPlayer = null;
    public static CSScene Sington = null;
    private CSMisc.Dot2 mDot2;
    private TABLE.MAPINFO mTblMapInfo = null;
    private bool isCanCrossServerDays;
    public static float loadMainPlayerTime = 0;

    public static bool IsLanuchMainPlayer
    {
        get { return CSScene.Sington != null && CSAvatarManager.MainPlayer != null; }
    }

    public static int GetMapID()
    {
        return CSMainPlayerInfo.Instance.MapID;
    }

    public static bool IsCanCrossScene
    {
        get
        {
            if (CSScene.Sington == null) return false;
            if (!CSScene.Sington.isCanCrossServerDays) return false;
            return CSScene.Sington.mTblMapInfo != null && CSScene.Sington.mTblMapInfo.cancross == 0 ? true : false;
        }
    }

    void Awake()
    {
        Sington = this;
        mSocket = new EventHanlderManager(EventHanlderManager.DispatchType.Socket);
        mClient = new EventHanlderManager(EventHanlderManager.DispatchType.Event);
    }

    void Start()
    {
        Init();
    }

    public void Init()
    {
        mClient.AddEvent(CEvent.Scene_EnterScene, EnterCameraView);
        mClient.AddEvent(CEvent.Scene_ExitView, ExitCameraView);
        mClient.AddEvent(CEvent.Scene_ChangeMap, ChangeScene);
        mClient.AddEvent(CEvent.Scene_RefreshView, RefreshCameraView);
        mClient.AddEvent(CEvent.Scene_PlayerEnterView, EnterPlayerView);
        mClient.AddEvent(CEvent.Scene_MonsterEnterView, EnterMonsterView);
        mClient.AddEvent(CEvent.Scene_NpcEnterView, EnterNpcView);
        mClient.AddEvent(CEvent.Scene_PetEnterView, EnterPetView);
        mClient.AddEvent(CEvent.Scene_ItemEnterView, EnterItemView);
        mClient.AddEvent(CEvent.Scene_BufferEnterView, EnterBufferView);
        mClient.AddEvent(CEvent.Scene_TriggerEnterView, EnterTriggerView);
        mClient.AddEvent(CEvent.Scene_SafeAreaCoordView, EnterRoundSafeAreaCoordView);
        mClient.AddEvent(CEvent.Scene_PlayerAdjustPosition, ResetPlayerPosition);
        mClient.AddEvent(CEvent.Scene_ObjectMove, UpdateViewPosition);
        mClient.AddEvent(CEvent.Scene_ObjectChangePosition, OnObjectChangePosition);
        mClient.AddEvent(CEvent.SabacDoorChange, OnSabacDoorChange);
        CSSceneEffectSystem.Instance.Init();
        CSSelectTargetManager.Instance.Init();
    }

    private void RemoveEvent()
    {
        if (mClient != null)
        {
            mClient.RemoveEvent(CEvent.Scene_EnterScene);
            mClient.RemoveEvent(CEvent.Scene_ExitView);
            mClient.RemoveEvent(CEvent.Scene_ChangeMap);
            mClient.RemoveEvent(CEvent.Scene_RefreshView);
            mClient.RemoveEvent(CEvent.Scene_PlayerEnterView);
            mClient.RemoveEvent(CEvent.Scene_MonsterEnterView);
            mClient.RemoveEvent(CEvent.Scene_NpcEnterView);
            mClient.RemoveEvent(CEvent.Scene_PetEnterView);
            mClient.RemoveEvent(CEvent.Scene_ItemEnterView);
            mClient.RemoveEvent(CEvent.Scene_BufferEnterView);
            mClient.RemoveEvent(CEvent.Scene_TriggerEnterView);
            mClient.RemoveEvent(CEvent.Scene_PlayerAdjustPosition);
            mClient.RemoveEvent(CEvent.Scene_ObjectMove);
            mClient.RemoveEvent(CEvent.SabacDoorChange, OnSabacDoorChange);
        }
    }

    private void Update()
    {
        CSAutoFightManager.Instance.Update();
    }

    public void OnDestroy()
    {
        Destroy();
        if (mMainPlayer != null)
        {
            mMainPlayer.Destroy();
        }
        mMainPlayer = null;

        if (CSAvatarManager.MainPlayer != null)
        {
            CSAvatarManager.MainPlayer.Destroy();
        }
        CSAvatarManager.MainPlayer = null;
    }

    public static void ForceDestroy()
    {
        if (CSScene.Sington == null) return;
        GameObject.Destroy(CSScene.Sington.gameObject);
        CSScene.Sington = null;
    }

    public void Destroy()
    {
        CSConstant.IsLanuchMainPlayer = false;
        if (this == null) return;
        //if(mMainPlayer != null)
        // {
        //     mMainPlayer.Destroy();
        // }

        CSPreLoadingBase.Instance.DestroyWorld();
        RemoveEvent();
        CSSkillPool.Instance.Clear();
        CSMesh.Instance.Destroy();
        CSAutoFightManager.Instance.Destroy();
        CSSelectTargetManager.Instance.Destroy();
        CSSceneEffectMgr.Instance.Destroy();
        CSDropManager.Instance.Destroy();
        CSBuffManager.Instance.Destroy();
        CSSkillFlagManager.Instance.Destroy();
        CSNpcTaskEffectMgr.Instance.Destroy();
        ModelLoadBase.Clear();
        CSPreLoadingBase.Instance.Clear();
    }

    public void Reconnect()
    {
        if (CSAvatarManager.Instance != null)
        {
            CSAvatarManager.Instance.RemoveAllAvatar();
        }
        CSSelectTargetManager.Instance.Destroy();
        CSSceneEffectMgr.Instance.Destroy();
        CSDropManager.Instance.Destroy();
        CSBuffManager.Instance.Destroy();
        CSSkillFlagManager.Instance.Destroy();
        CSNpcTaskEffectMgr.Instance.Destroy();
    }

    /// <summary>
    /// 请求进入场景
    /// </summary>
    private void ReqEnterScene()
    {
        //FNDebug.Log("冬天102101" +"||" + System.DateTime.Now);
        Net.ReqEnterMapMessage();
    }

    /// <summary>
    /// 进入场景准备,切换到场景用场景中挂的Mono启动
    /// </summary>
    public IEnumerator StartEnterScene()
    {
        CSMainParameterManager.StartEnterSceneComplete = false;
        yield return LoadingScene();
        PlayBGM();
        CSMainParameterManager.StartEnterSceneComplete = true;
    }

    /// <summary>
    ///  加载场景资源
    /// </summary>
    private IEnumerator LoadingScene()
    {
        CSPreLoadingBase.Instance.CreateSceneActhor();
        CSAvatarDirector.Instance.CreateMainPlayer(CSPreLoadingBase.Instance.WorldRoot.parent);
        UIManager.Instance.OpenPanel<UIMainSceneManager>();
        ReqEnterScene();
        yield return null;
        yield return ChangeSceneData();
    }

    public IEnumerator ChangeSceneData()
    {
        loadMainPlayerTime = Time.time;
        UIManager.Instance.ClosePanel<UILoading>(false);
        CSResourceManager.Singleton.IsChangingScene = false;

        yield return null;
        InitMapInfoTable();
        UpdateServerCrossDays();
        CSCameraManager.Instance.IsInitCamera = false;
        CSDropSystem.Instance.DetectPickItem();
        CSAutoFightManager.Instance.Stop();
        CSConstant.lastStandTimeUnloadAsset = 0;
        CSConstant.isMainPlayerMoving = false;
        isChangeScene = false;
        //页面逻辑Init在下面管理类中添加，，不要写到此处
        yield return CSLoginInitManager.Instance.Init();
    }

    private void InitMapInfoTable()
    {
        mTblMapInfo = MapInfoTableManager.Instance[CSScene.GetMapID()];
    }

    /// <summary>
    /// 进入相应的地图场景
    /// </summary>
    /// <param name="uiEvtID"></param>
    /// <param name="data"></param>
    private void EnterCameraView(uint uiEvtID, object data)
    {
        map.EnterMapResponse enterScene = data as map.EnterMapResponse;
        if (enterScene == null)
        {
            FNDebug.Log("=======> enterScene is null");
            return;
        }
        CSServerTime.Instance.refreshTime(enterScene.serverTime);

        if (enterScene.mapId == GetMapID())
        {
            CoroutineManager.DoCoroutine(DelayEnterSceneMission());
            PositionChangeResponse rsp = new PositionChangeResponse();
            rsp.newX = enterScene.x;
            rsp.newY = enterScene.y;
            rsp.time = enterScene.serverTime;
            if (IsLanuchMainPlayer)
            {
                rsp.reason = CSMainPlayerInfo.Instance.PosChangeReason;
            }
            ResetPlayerPosition(0, rsp);
            CSMainPlayerInfo.Instance.PkModeMap = MapInfoTableManager.Instance.GetMapInfoMapPKMode(enterScene.mapId);
        }
        else
        {
            ChangeScene(0, data);
        }
    }

    /// <summary>
    /// 退出视野
    /// </summary>
    /// <param name="uiEvtID">key</param>
    /// <param name="data"></param>
    private void ExitCameraView(uint uiEvtID, object data)
    {
        map.ObjectExitViewResponse info = data as map.ObjectExitViewResponse;
        if (info == null) return;
        RemoveObject(info.id);
    }

    /// <summary>
    /// 切换地图（场景）
    /// </summary>
    /// <param name="uiEvtID"></param>
    /// <param name="data"></param>
    public void ChangeScene(uint uiEvtID, object data)
    {
        
        map.EnterMapResponse rsp = data as map.EnterMapResponse;
        if (rsp == null) return;
       
        CSMainPlayerInfo.Instance.ResetWhenChangeMap(rsp.x, rsp.y, rsp.mapId, rsp.reason);

        if (CSAvatarManager.MainPlayer != null)
        {
            CSAvatarManager.MainPlayer.ChangeScene(CSMainPlayerInfo.Instance.Coord);
        }
        isChangeScene = true;
        CSSceneLoadManager.Instance.LoadScenePassEmptyScene("MainScene");
    }

    //ILRunTime情况下，比较卡， ChangeSceneData调用晚于 EnterCameraView 导致调用 EnterMao_After 事件后，又暂停自动战斗
    private bool isChangeScene;

    private void RefreshCameraView(uint evtId, object data)
    {
        //if (data == null)
        //    return;
        map.UpdateViewResponse rsp = data as map.UpdateViewResponse;
        //if (rsp == null)
        //    return;
        for (int i = 0; i < rsp.enterMonsters.Count; ++i)
        {
            EnterMonsterView(evtId, rsp.enterMonsters[i]);
        }
        for (int i = 0; i < rsp.enterPlayers.Count; ++i)
        {
            EnterPlayerView(evtId, rsp.enterPlayers[i]);
        }
        for (int i = 0; i < rsp.enterNPC.Count; ++i)
        {
            EnterNpcView(evtId, rsp.enterNPC[i]);
        }
        for (int i = 0; i < rsp.enterItems.Count; ++i)
        {
            EnterItemView(evtId, rsp.enterItems[i]);
        }
        for (int i = 0; i < rsp.enterPets.Count; ++i)
        {
            EnterPetView(evtId, rsp.enterPets[i]);
        }
        for (int i = 0; i < rsp.enterBuffers.Count; ++i)
        {
            EnterBufferView(evtId, rsp.enterBuffers[i]);
        }
        for (int i = 0; i < rsp.enterTriggers.Count; ++i)
        {
            EnterTriggerView(evtId, rsp.enterTriggers[i]);
        }
        for(int i= 0;i < rsp.safeAreaeffects.Count; ++i)
        {
            EnterRoundSafeAreaCoordView(evtId, rsp.safeAreaeffects[i]);
        }

        for (int i = 0; i < rsp.exitObjects.Count; ++i)
        {
            long id = rsp.exitObjects[i];
            RemoveObject(id);
        }
    }

    /// <summary>
    /// 怪物视野
    /// </summary>
    /// <param name="uiEvtID"></param>
    /// <param name="data"></param>
    private void EnterMonsterView(uint uiEvtID, object data)
    {
        if (CSResourceManager.Instance.IsChangingScene) return;
        map.RoundMonster info = data as map.RoundMonster;
        if (info == null) return;
        CSAvatarDirector.Instance.CreateAvatar<CSMonster>(info.monsterId, info,CSPreLoadingBase.Instance.MonsterAnchor);
    }

    private void EnterPlayerView(uint uiEvtID, object data)
    {
        if (CSResourceManager.Instance.IsChangingScene) return;
        map.RoundPlayer info = data as map.RoundPlayer;
        if (info == null || info.player == null) return;
        CSAvatarDirector.Instance.CreateAvatar<CSPlayer>(info.player.roleId, info, CSPreLoadingBase.Instance.PlayerAnchor);
    }

    private void EnterNpcView(uint uiEvtID, object data)
    {
        if (CSResourceManager.Instance.IsChangingScene) return;
        map.RoundNPC info = data as map.RoundNPC;
        if (info == null) return;
        CSAvatarDirector.Instance.CreateAvatar<CSNpc>(info.npcId, info, CSPreLoadingBase.Instance.NpcAnchor);
    }

    private void EnterPetView(uint evtId, object obj)
    {
        if (CSResourceManager.Instance.IsChangingScene) return;
        map.RoundPet info = obj as map.RoundPet;
        if (info == null) return;
        CSAvatarDirector.Instance.CreateAvatar<CSPet>(info.petId, info, CSPreLoadingBase.Instance.PetAnchor);
    }

    private void EnterItemView(uint uiEvtID, object data)
    {
        if (CSResourceManager.Instance.IsChangingScene) return;
        map.RoundItem info = data as map.RoundItem;
        if (info == null) return;
        CSDropManager.Instance.Create(CSPreLoadingBase.Instance.ItemAnchor,info);
    }

    private void EnterRoundSafeAreaCoordView(uint uiEvtID, object data)
    {
        if (CSResourceManager.Instance.IsChangingScene) return;
        map.RoundSafeAreaCoord info = data as map.RoundSafeAreaCoord;
        if (info == null) return;
        CSSceneEffectMgr.Instance.PlayEffect(CSPreLoadingBase.Instance.EffectAnchor, info.id, info.effectConfigId, info.x, info.y, true);
    }

    private void EnterBufferView(uint evtId, object obj)
    {
        if (CSResourceManager.Instance.IsChangingScene) return;
        RoundBuffer info = obj as RoundBuffer;
        if (info == null)
        {
            return;
        }

        TABLE.BUFFER tblBuff = null;
        if (BufferTableManager.Instance.TryGetValue(info.bufferConfigId, out tblBuff))
        {
            CSSceneEffectMgr.Instance.PlayEffect(CSPreLoadingBase.Instance.EffectAnchor, info.bufferId, tblBuff.effectId,info.x, info.y,true);
        }
    }

    private void EnterTriggerView(uint evtId, object obj)
    {
        if (CSResourceManager.Instance.IsChangingScene) return;
        RoundTrigger info = obj as RoundTrigger;
        if (info == null)
        {
            return;
        }
        CSTriggerInfo.Instance.Update(info);
        CSSceneEffectMgr.Instance.PlayEffectWaitDeal(CSPreLoadingBase.Instance.EffectAnchor, info.triggerId, obj, OnLoadTrigger);

        if(!info.isSabacDoor)
        {
            if (SceneTriggerTableManager.Instance.TryGetValue(info.triggerConfigId, out TABLE.SCENETRIGGER sceneTrigger))
            {
                CSSceneEffectMgr.Instance.PlayEffect(CSPreLoadingBase.Instance.EffectAnchor,info.triggerId, sceneTrigger.effectId,info.x, info.y);
            }
        }
    }

    private void OnSabacDoorChange(uint evtId, object obj)
    {
        if (CSResourceManager.Instance.IsChangingScene) return;
        RoundTrigger info = obj as RoundTrigger;
        if (info == null)
        {
            return;
        }
        CSTriggerInfo.Instance.Update(info);
        CSSceneEffectMgr.Instance.PlayEffectWaitDeal(CSPreLoadingBase.Instance.EffectAnchor, info.triggerId, obj, OnLoadSabacDoor);
    }

    public void ResetPlayerPosition(uint uiEvtID, object data)
    {
        map.PositionChangeResponse rsp = data as map.PositionChangeResponse;

        if (rsp == null) return;

        CSServerTime.Instance.refreshTime(rsp.time);
        CSCharacter MainPlayer = CSAvatarManager.MainPlayer;

        rsp.time = 0;
        if (rsp != null && MainPlayer != null)
        {
            mDot2.x = rsp.newX;
            mDot2.y = rsp.newY;
            MainPlayer.AdjustPosition(mDot2);
            StopAutoFightTransform(rsp.reason);
            if (rsp.reason == (int)PositionChangeReason.Crnull || rsp.reason == (int)PositionChangeReason.Transfer)
            {
                CoroutineManager.DoCoroutine(DelayReachNpcMission());
            }
            else if (rsp.reason == (int)PositionChangeReason.TaskFly)
            {
                CoroutineManager.DoCoroutine(DelayTaskFlyMission());
            }
            CSMainPlayerInfo.Instance.EventHandler.SendEvent(CEvent.MainPlayer_StopTrigger, MainPlayer.OldCell);
            HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_UpdateRoleMove, null);
            ShowTransformEffect(rsp.reason);
        }
    }

    private void UpdateViewPosition(uint uiEvtID, object data)
    {
        map.ObjectMoveResponse info = data as map.ObjectMoveResponse;

        if (info == null) return;

        long mainPlayerID = CSMainPlayerInfo.Instance.ID;

        if (mainPlayerID != info.id)
        {
            mDot2.x = info.newX;
            mDot2.y = info.newY;

            CSAvatar avatar = CSAvatarManager.Instance.GetAvatar(info.id);

            if (avatar != null)
            {
                avatar.ResetServerCell(mDot2.x, mDot2.y);

                if (avatar.IsLoad)
                {
                    if (avatar.IsDead)
                    {
                        avatar.ResetPosition(mDot2);
                    }
                }
                else
                {
                    avatar.ResetOldCell(mDot2.x, mDot2.y);
                }
            }
        }
    }

    private void OnObjectChangePosition(uint evtId, object obj)
    {
        map.ObjectMoveResponse info = obj as map.ObjectMoveResponse;
        if (info == null)
        {
            return;
        }
        CSAvatar avatar = CSAvatarManager.Instance.GetAvatar(info.id);
        if (avatar != null)
        {
            avatar.Stop(true);
            mDot2.x = info.newX;
            mDot2.y = info.newY;
            avatar.ResetPosition(mDot2);
        }
    }

    public void RemoveObject(long id)
    {
        bool ret = CSAvatarManager.Instance.RemoveAvatar(id);
        if (!ret)
        {
            ret = CSDropManager.Instance.RemoveItem(id);
        }
        if (!ret)
        {
            ret = CSSceneEffectMgr.Instance.Remove(id);
        }
        CSTriggerInfo.Instance.Remove(id);
    }

    public Vector3 WorldTransformPoint(Vector3 position)
    {
        return CSPreLoadingBase.CahceWorldTrans.TransformPoint(position);
    }

    public bool OnLoadTrigger(object obj, object param)
    {

        RoundTrigger info = obj as RoundTrigger;
        if (info == null)
        {
            return false;
        }

        CSSceneEffect effect = CSSceneEffectMgr.Instance.GetEffect(info.triggerId);
        if (effect == null)
        {
            return false;
        }
        TABLE.SCENETRIGGER tblTrigger = null;
        if (SceneTriggerTableManager.Instance.TryGetValue(info.triggerConfigId, out tblTrigger))
        {
            int effectid = 0;
            int x = 0;
            int y = 0;
            if (info.triggerConfigId == 721001)
            {
                effectid = info.isDoorOpen ? 17607 : 17608;
            }
            else
            {
                effectid = tblTrigger.effectId;
                x = info.x;
                y = info.y;
            }
            effect.Play(CSPreLoadingBase.Instance.EffectAnchor, effectid,x,y, false,null,ResourceAssistType.Terrain);
        }
        return false;
    }

    public bool OnLoadSabacDoor(object obj, object param)
    {

        RoundTrigger info = obj as RoundTrigger;
        if (info == null)
        {
            return false;
        }

        CSSceneEffect effect = CSSceneEffectMgr.Instance.GetEffect(info.triggerId);
        if (effect == null)
        {
            return false;
        }
        TABLE.SCENETRIGGER tblTrigger = null;
        if (SceneTriggerTableManager.Instance.TryGetValue(info.triggerConfigId, out tblTrigger))
        {
            int effectid = 0;
            if (info.triggerConfigId == 721001)
            {
                effectid = info.isDoorOpen ? 17605 : 17606;
            }
            else
            {
                effectid = tblTrigger.effectId;
            }
            effect.Play(CSPreLoadingBase.Instance.EffectAnchor, effectid, 0, 0, false,null,ResourceAssistType.Terrain);
        }
        return false;
    }

    public void CheckAvatarInView(CSAvatar avatar)
    {
        if (!IsLanuchMainPlayer)
        {
            return;
        }
        if (IsInViewRange(avatar))
        {
            if (!CSAvatarManager.Instance.IsAvatarInViewDic(avatar))
            {
                //if (!CSScene.IsPlayerMaxShowLimit || CSScene.GetLowPriorityViewNum(this) > 0)
                if (!CSAvatarManager.Instance.IsPlayerMaxShowLimit)
                {
                    //ViewShowAvatar(avatar);
                }
                ShowAvatarInView(avatar);
            }
            else
            {
                if (CSAvatarManager.IsNeedRemoveViewNum(avatar))
                {
                    //ViewShowAvatar(avatar);
                    HideAvatarOutView(avatar);
                }
            }
        }
        else
        {
            if (CSAvatarManager.Instance.IsAvatarInViewDic(avatar))
            {
                HideAvatarOutView(avatar);
            }
        }
    }

    public void ShowAvatarInView(CSAvatar avatar)
    {
        CSAvatarManager.Instance.AddAvatarInView(avatar);
        Transform anchor = CSPreLoadingBase.Instance.GetAvatarAnchor(avatar.AvatarType);
        CSAvatarDirector.Instance.ShowAvatar(anchor, avatar);
    }

    public void HideAvatarOutView(CSAvatar avatar)
    {
        CSAvatarManager.Instance.RemoveAvatarInView(avatar);
        Transform anchor = CSPreLoadingBase.Instance.GetAvatarAnchor(avatar.AvatarType);
        CSAvatarDirector.Instance.RemoveAvatar(anchor, avatar);
    }

    public static bool IsInViewRange(CSAvatar avatar)
    {
        if(avatar.OldCell == null)
        {
            return false;
        }
        if (UtilityMain.IsInViewRange(avatar.OldCell.Coord))
        {
            return true;
        }
        return false;
    }

    private void PlayBGM()
    {
        if (mTblMapInfo != null)
        {
            if (CSAudioMgr.Instance != null)
            {
                CSAudioManager.Instance.Play(true, mTblMapInfo.bgm);
            }
        }
    }

    void UpdateServerCrossDays()
    {
        isCanCrossServerDays = CSSundryData.IsCanCrossServerDay(CSScene.GetMapID(), 0);

        if (CSAvatarManager.MainPlayer != null)
        {
            CSAvatarManager.MainPlayer.UpdateIsCanCrossScene(IsCanCrossScene);
        }
    }

    private void ShowTransformEffect(int reason)
    {
        switch ((PositionChangeReason)reason)
        {
            case PositionChangeReason.RandomStone:
            case PositionChangeReason.Transfer:
                {
                    Transform anchor = CSAvatarManager.MainPlayer.Model.Effect.GoTrans;
                    CSSceneEffectMgr.Instance.PlayEffect(anchor, 6031);
                    CSSceneEffectMgr.Instance.PlayEffect(anchor, 6032);
                }
                break;
        }
    }

    private void StopAutoFightTransform(int reason)
    {
        switch ((PositionChangeReason)reason)
        {
            case PositionChangeReason.RandomStone:
            case PositionChangeReason.Transfer:
            case PositionChangeReason.Gate:
                {
                    CSInstanceInfo.Instance.DetectInstanceAutofight(CSMainPlayerInfo.Instance.MapID);
                }
                break;
        }
    }

    public static void AddWaitDeal_Object_Insert(int index, object obj, Func<object, object, bool> onLoad, float waitFrame = 0, object param = null)
    {
        if (Sington == null) return;
        CSWaitFrameDeal t = CSPreLoadingBase.Instance.WaitFrameDeal_Object;
        if (t == null) t = Sington.gameObject.AddComponent<CSWaitFrameDeal>();
        CSPreLoadingBase.Instance.WaitFrameDeal_Object = t;
        t.Insert(index, obj, onLoad, waitFrame, param);
    }

    public static void RemoveWaitDeal_Object(object obj)
    {
        if (Sington == null) return;
        CSWaitFrameDeal t = CSPreLoadingBase.Instance.WaitFrameDeal_Object;
        if (t == null) return;
        t.Remove(obj);
    }

    IEnumerator DelayEnterSceneMission()
    {
        while (isChangeScene)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.7f);
        mClient.SendEvent(CEvent.Scene_EnterSceneAfter);
    }

    IEnumerator DelayReachNpcMission()
    {
        while (isChangeScene)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.7f);
        mClient.SendEvent(CEvent.Reach_Npc_Position);
    }

    IEnumerator DelayTaskFlyMission()
    {
        while (isChangeScene)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.7f);
        CSMissionManager.Instance.FlyToGoalMessage();
    }

}


