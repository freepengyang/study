using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

public partial class UIMiniMapPanel : UIMapBase
{
    private UITexture _MapTexture;

    private UITexture MapTexture
    {
        get { return _MapTexture ? _MapTexture : (_MapTexture = mMaptexture.GetComponent<UITexture>()); }
    }

    private Material m_material;

    private Material MapMaterial
    {
        get { return m_material ? m_material : (m_material = _MapTexture.material); }
    }

    protected override bool mTextureSnap
    {
        get { return false; }
    }

    protected override bool mIsCanBeDelete
    {
        get { return true; }
    }

    private long MainPlayerID;

    Vector3 screenV3 = Vector3.zero;

    //Vector3 mainPlayerPos = Vector2.zero; //暂时都是0， 如果player点和其他点的相对坐标不同时，此处才修改
    private Rect CurMapRect = Rect.zero;
    private Vector2 mapTexOffset = Vector2.zero; //雷达显示区域所占uv值的一半
    private Vector2 CurMapSizeOfOne = Vector2.zero; //当前地图尺寸倒数，，用乘法代替除法

    protected override GameObject mMaptexture
    {
        get { return mMiniMapTex; }
    }


    public override void Init()
    {
        CSMapManager.Instance.Init(); //暂时放此处，解决登录数据空的情况，，后续可以修改流程

        mClientEvent.AddEvent(CEvent.InitMiniMapCallBack, InitMiniMapCallBack);
        mClientEvent.AddEvent(CEvent.OnMainPlayerTeamIdChanged, RefreahViewPlayer);
        mClientEvent.AddEvent(CEvent.Relive, UpdateRoleRelive);
        base.Init();
        mbg.onClick = OnOpenMapClick;

        MainPlayerID = CSMainPlayerInfo.Instance.ID;
        InitMiniMapCallBack(0, null);
    }

    public override void Show()
    {
        base.Show();
        GetScreenPosition(CSAvatarManager.MainPlayer.NewCell.LocalPosition2);
        RefreshCameraMapView(0, CSMapManager.Instance.initViewData);
    }

    public void InitMiniMapCallBack(uint id, object data)
    {
        mClientEvent.AddEvent(CEvent.Scene_EnterScene, UpdateMapInfoEnterScene);
        mClientEvent.AddEvent(CEvent.Scene_RefreshView, RefreshCameraMapView);
        mClientEvent.AddEvent(CEvent.Scene_ObjectMove, UpdateMapViewPosition);
        mClientEvent.AddEvent(CEvent.Scene_PlayerEnterView, EnterPlayerMapView);
        mClientEvent.AddEvent(CEvent.Scene_MonsterEnterView, EnterMonsterMapView);
        mClientEvent.AddEvent(CEvent.Scene_NpcEnterView, EnterNpcMapView);
        mClientEvent.AddEvent(CEvent.Scene_PlayerAdjustPosition, UpdateMoveCellCoord);
        mClientEvent.AddEvent(CEvent.Scene_ExitView, ExitCameraView);
    }

    #region CEvent

    private CSMapAvatarInfo Avater;
    private map.ObjectMoveResponse moveInfo;

    private void UpdateMapViewPosition(uint uiEvtID, object data)
    {
        if (data == null) return;
        moveInfo = data as map.ObjectMoveResponse;
        if (moveInfo == null) return;
        if (MainPlayerID != moveInfo.id)
        {
            Avater = CSMapManager.GetMapAvatar(moveInfo.id);
            if (Avater != null)
            {
                Avater.SetMainPlayerPos(MainPlayerMapPosition/*- mainPlayerPos 现在值是0*/);
                Avater.ResetServerCell(moveInfo.newX, moveInfo.newY);
            }
        }
    }

    protected override void UpdateMoveCellCoord(uint id, object data)
    {
        base.UpdateMoveCellCoord(id, data);
        UpdateUICoordinate();
        UpdateMapPosition();
    }

    private void ExitCameraView(uint id, object data)
    {
        map.ObjectExitViewResponse info = data as map.ObjectExitViewResponse;
        if (info == null) return;
        CSMapManager.RemoveAvatar(info.id);
    }

  #endregion

    private void UpdateUICoordinate()
    {
        mlb_coordinate.text = $"{playerPosition.x},{playerPosition.y}";
    }

    private void UpdateMapName()
    {
        mlb_cityname.text = CSMapManager.Instance.mTbleMapInfo.name;
    }

    private Vector2 offset = Vector2.zero;
    private Vector3 MainPlayerMapPosition = Vector3.zero;
    private Vector2 calOffset;
    private Vector2 oldOffset;

    private void UpdateMapPosition()
    {
        //更新地图位置

        //new
        GetScreenPosition(CSAvatarManager.MainPlayer.NewCell.LocalPosition2);
        offset.Set(screenV3.x / 640f, screenV3.y / 454f);
        offset = offset + Vector2.one * 0.5f;

        MainPlayerMapPosition = Vector3.zero;
        calOffset.x = Mathf.Clamp(offset.x, 0.11f, 0.89f);
        if (!offset.x.Equals(calOffset.x))
            MainPlayerMapPosition.x = (offset.x - calOffset.x) * CSMapManager.mMimSize.x;
        calOffset.y = Mathf.Clamp(offset.y, 0.143f, 0.857f);
        if (!offset.y.Equals(calOffset.y))
            MainPlayerMapPosition.y = (offset.y - calOffset.y) * CSMapManager.mMimSize.y;
        mmainPlayer.localPosition = MainPlayerMapPosition;
        calOffset -= mapTexOffset;
        CurMapRect.position = calOffset;
        MapTexture.uvRect = CurMapRect;

        PlayerMoveToUpdateAvatar(offset - oldOffset);

        oldOffset = offset;
    }

    protected override void MainPlayerDirectionChange(uint id, object data)
    {
        mmainPlayer.localRotation = CSMapManager.GetPlayerDirection();
    }

    private void ReSetMap()
    {
        //调用物体销毁  数据重置
        CSMapManager.RemoveAll();
    }


    protected override void UpdateMapInfo()
    {
        ReSetMap();
        UpdateMapRect();
        UpdateMoveCellCoord(0, null);
        UpdateMapName();
    }

    private void UpdateMapRect()
    {
        CurMapSizeOfOne.Set(1.0f / CSMapManager.mMimMapSize.x, 1.0f / CSMapManager.mMimMapSize.y);

        CurMapRect.width = MapTexture.width * CurMapSizeOfOne.x;
        CurMapRect.height = MapTexture.height * CurMapSizeOfOne.y;
        MapTexture.uvRect = CurMapRect;
        mapTexOffset.x = MapTexture.width * 0.5f * CurMapSizeOfOne.x;
        mapTexOffset.y = MapTexture.height * 0.5f * CurMapSizeOfOne.y;
    }

    protected override void PlayerStop(uint id, object data)
    {
        if (CSResourceManager.Singleton.IsChangingScene) return;
        CSPathFinderManager.IsAutoFinPath = false;
    }

    protected override void SetPostion(Transform tr, Vector3 pos)
    {
        base.SetPostion(tr, pos);
        tr.localPosition = MainPlayerMapPosition - HalfMapSize - screenV3;
    }


    private void GetScreenPosition(Vector2 pos)
    {
        if (MapScale.x <= 0 || MapScale.y <= 0) return;
        screenV3.x = pos.x * MapScale.x;
        screenV3.y = (pos.y - (CsTerrain.NewSize.y - CsTerrain.OldSize.y)) * MapScale.y;
        screenV3 -= HalfMapSize;
    }


    private void OnOpenMapClick(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIMapCombinePanel>();
    }


    #region 雷达刷新

    private void RefreshCameraMapView(uint id, object data)
    {
        if (data == null) return;
        map.UpdateViewResponse rsp = data as map.UpdateViewResponse;
        if (rsp == null) return;
        for (int i = 0, max = rsp.exitObjects.Count; i < max; ++i)
        {
            CSMapManager.RemoveAvatar(rsp.exitObjects[i]);
        }

        for (int i = 0, max =  rsp.enterPlayers.Count; i < max; ++i)
        {
            EnterPlayerMapView(id, rsp.enterPlayers[i]);
        }

        for (int i = 0, max = rsp.enterMonsters.Count; i < max; ++i)
        {
            EnterMonsterMapView(id, rsp.enterMonsters[i]);
        }

        for (int i = 0, max = rsp.enterNPC.Count; i < max; ++i)
        {
            EnterNpcMapView(id, rsp.enterNPC[i]);
        }
    }

    private map.RoundPlayer playerInfo;
    private void EnterPlayerMapView(uint id, object data)
    {
        playerInfo = data as map.RoundPlayer;
        if (playerInfo == null || playerInfo.player == null || playerInfo.player.roleId == MainPlayerID) return;
        AddAvatarView<CSMapPlayerInfo>(playerInfo.player, playerInfo.player.roleId, MapAvaterType.Player, mPlayerPoints);
    }

    private map.RoundMonster monsterInfo;
    private void EnterMonsterMapView(uint uiEvtID, object data)
    {
        monsterInfo = data as map.RoundMonster;
        if (monsterInfo == null) return;
        AddAvatarView<CSMapMonsterInfo>(monsterInfo, monsterInfo.monsterId, MapAvaterType.Monster, mMonsterPoints);
    }

    private map.RoundNPC npcInfo;
    private void EnterNpcMapView(uint id, object data)
    {
        npcInfo = data as map.RoundNPC;
        if (npcInfo == null) return;
        AddAvatarView<CSMapNpcInfo>(npcInfo, npcInfo.npcId, MapAvaterType.Npc, mNpcPoints);
    }

    private void AddAvatarView<T>(IMessage info, long Id, MapAvaterType type, Transform parent)
        where T : CSMapAvatarInfo, new()
    {
        CSMapManager.RemoveAvatar(Id);
        CSMapAvatarInfo avatar = CSMapManager.GetPoolItem<T>((int)type);
        avatar.Init(info, parent, mspr_itemPoint);
        avatar.SetMainPlayerPos(MainPlayerMapPosition/*- mainPlayerPos*/);
        CSMapManager.AddMapAvatarLoad(avatar, type);
    }

    private void QuitCameraMapView(uint id, object data)
    {
        map.ObjectExitViewResponse info = data as map.ObjectExitViewResponse;
        if (info == null) return;
        CSMapManager.RemoveAvatar(info.id);
    }

    private void UpdateRoleRelive(uint id, object data)
    {
        UpdateMapPosition();
        UpdateMoveCellCoord(0, null);
    }

    private void PlayerMoveToUpdateAvatar(Vector2 delDistance)
    {
        if (delDistance == Vector2.zero) return;
        List<long> infoList = CSMapManager.MapAvatarInfoList;
        if(infoList != null)
        {
            CSMapAvatarInfo info;
            for(int i = 0, max = infoList.Count; i < max; i++)
            {
                if(CSMapManager.MapAvatarInfoDic.TryGetValue(infoList[i], out info))
                {
                    info.SetMainPlayerPos(MainPlayerMapPosition/*- mainPlayerPos*/);
                    info.SetStartPos();
                }
            }
        }
    }

  #endregion

    private void RefreahViewPlayer(uint id, object data)
    {
        var avaterList = CSMapManager.GetAvaterByType(MapAvaterType.Player);
        if (avaterList == null || avaterList.Count == 0) return;
        for (var i = 0; i < avaterList.Count; i++)
        {
            if (avaterList[i] == null) continue;
            avaterList[i].RefreshUI();
        }
    }

    protected void UpdateMapInfoEnterScene(uint id, object data)
    {
        //UpdateMapInfo();
        UpdateMoveCellCoord(0, null);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}