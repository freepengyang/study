using System;
using System.Collections.Generic;
using Google.Protobuf;
using TABLE;
using UnityEngine;
using Object = UnityEngine.Object;

public partial class UIMapPanel : UIMapBase
{
    //private readonly int CHECK_AROUD_LIMMIT = 5; //不可走区域周边点搜索上限

    private const float UPDATE_DELAY = 2; //小地图刷新时间
    private const int RANDOM_SHOP_ID = 50000500; //随机石ShopId     item id : 50000001
    private const int BACK_SHOP_ID = 50000501; //回城石ShopId     item id : 50000003


    protected Vector2 mStartPosition = Vector2.zero;
    protected Vector2 mEndPosition = Vector2.zero;
    protected Vector2 mCenterPosition = Vector2.zero;
    private Vector3 MainPlayerMapPosition = Vector3.zero;
    private UICloneMapPoints _PathPoint;
    private UIGridContainerHot<UIItemMiniMapPoint> _NpcPoint;
    private UIGridContainerHot<UIItemMiniMapPoint> _TransferPoint;
    private UIGridContainerHot<UIItemMiniMapPoint> _SpecialPoint;
    private UIGridContainerHot<UIItemMiniMapPoint> _MonsterPoint;
    private ILBetterList<MINIMAP> _NpcTableList;
    private ILBetterList<MINIMAP> _TranferTableList;
    private ILBetterList<MINIMAP> _MonsterTableList;
    

    List<MapFirstTabs> firstitemList = new List<MapFirstTabs>();
    MapSecondTabs curSecondTab;
    private long _CurRandomCount;
    private long _CurBackCount;
    private long MainPlayerID;
    private bool isInit;

    protected override GameObject mMaptexture
    {
        get { return mTexture; }
    }

    public override void Init()
    {
        base.Init();
        mClientEvent.AddEvent(CEvent.InitMiniMapCallBack, InitMiniMapCallBack);
        mClientEvent.AddEvent(CEvent.Task_GoalUpdate, OnUpdateMissionState);
        mClientEvent.AddEvent(CEvent.ItemListChange, BagItemChange);
        mClientEvent.AddEvent(CEvent.UpdateMapSpecialPlayer, UpdateMapSpecialPlayer);

        mbtn_back.onClick = OnBackClick;
        mbtn_random.onClick = OnRandomClick;
        UIEventListener.Get(mbtn_show.gameObject).onClick = OnShowLeftClick;
        SetMapTextureClick();
        InitMiniMapCallBack(0, null);
        RefreshBtn();
        InitList();
        MainPlayerID = CSMainPlayerInfo.Instance.ID;
        

    }

    public void InitMiniMapCallBack(uint id, object data)
    {
        mClientEvent.AddEvent(CEvent.Scene_PlayerAdjustPosition, UpdateMoveCellCoord);
        
        mClientEvent.AddEvent(CEvent.Scene_RefreshView, RefreshCameraMapView);
        mClientEvent.AddEvent(CEvent.Scene_ObjectMove, UpdateMapViewPosition);
        mClientEvent.AddEvent(CEvent.Scene_MonsterEnterView, EnterMonsterMapView);
        mClientEvent.AddEvent(CEvent.Scene_ExitView, ExitCameraView);
    }

    private void SetTextureRootPos()
    {
        Vector3 textureRootPos = CSMapManager.mHalfScreenSize + GetInPanelPos(mTexture.transform);
        mStartPosition.Set(textureRootPos.x - HalfMapSize.x, textureRootPos.y - HalfMapSize.y);
        mEndPosition.Set(textureRootPos.x + HalfMapSize.x, textureRootPos.y + HalfMapSize.y);
        mCenterPosition.Set((mStartPosition.x + mEndPosition.x) / 2, (mStartPosition.y + mEndPosition.y) / 2);
    }

    private void SetMapTextureClick()
    {
        if (mTexture != null)
        {
            UIEventListener ev = UIEventListener.Get(mTexture);
            if (ev != null)
            {
                ev.ClickIntervalTime = 0.5f;
                ev.onClick = OnClickSmallMap;
#if UNITY_EDITOR
                ev.onDoubleClick = OnGMClickSmallMap;
#endif
            }
        }
    }

    private void InitList()
    {
        string[] FristTabs = {"NPC", CSString.Format(899)};
        firstitemList.Add(new MapFirstTabs(Object.Instantiate(mmainTemp, mTable.transform),1, FristTabs[0],
            FirstTabsClick, msubTemp, SecondClick));
        firstitemList.Add(new MapFirstTabs(Object.Instantiate(mmainTemp, mTable.transform),2, FristTabs[1],
            FirstTabsClick, msubTemp, SecondClick));
        mTable.Reposition();
    }

    public override void Show()
    {
        base.Show();
    }

    protected override void UpdateMapInfo()
    {
        ReSetMap();
        ShowMapName();
        UpdateMoveCellCoord(0, null);
        SetTextureRootPos();
        ResetPointTabList();
        RefreshRightList();
        MainPlayerDirectionChange(0, null);
        if (!isInit)
        {
            isInit = true;
            UpdateMapInfoAfterScene(0, null);
            InitViewPoint();
        }
    }
    
    private void ReSetMap()
    {
        //调用物体销毁  数据重置
        CSMapManager.RemoveBigMapAll();
    }
#region Events

    protected override void UpdateMoveCellCoord(uint id, object data)
    {
        base.UpdateMoveCellCoord(id, data);
        UpdatePlayerPos();
        RemovelastPath();
    }

    protected override void UpdateMapInfoAfterScene(uint id, object data)
    {
        base.UpdateMapInfoAfterScene(id, data);
        DelayDrawPaths();
        SetMapPoint();
    }

    protected override void PlayerStop(uint id, object data)
    {
        if (mendPoint != null)
        {
            mendPoint.SetActive(false);
        }

        if (_PathPoint != null)
        {
            _PathPoint.RemoveAll();
        }
    }

    private void OnUpdateMissionState(uint id, object data)
    {
        DelayDrawPaths();
    }

    private void UpdateMapSpecialPlayer(uint id, object data)
    {
        ShowSpecialPoint();
    }

#endregion

    private void ShowMapName()
    {
        mcityName.text = CSMapManager.Instance.mTbleMapInfo.name;
    }

    private void UpdatePlayerPos()
    {
        SetPostion(mmainPlayer,CSAvatarManager.MainPlayer.NewCell.LocalPosition2);
        mposLabel.text = $"({playerPosition.x},{playerPosition.y})";
        MainPlayerMapPosition = mmainPlayer.localPosition;
        mposLabel.transform.localPosition = MainPlayerMapPosition + Vector3.up * 20;
    }

    protected override void MainPlayerDirectionChange(uint id, object data)
    {
        mmainPlayer.localRotation = CSMapManager.GetPlayerDirection();
    }
    
    protected override void SetPostion(Transform tr, Vector3 pos)
    {
        base.SetPostion(tr, pos);
        tr.localPosition = position - HalfMapSize;
    }


    private void OnClickSmallMap(GameObject go)
    {
        if(CSAvatarManager.MainPlayer.IsBeControl)
        {
            return;
        }
        Vector2 pos = UICamera.currentTouch.pos;
        if (pos.x < mStartPosition.x || pos.y < mStartPosition.y || pos.x > mEndPosition.x || pos.y > mEndPosition.y)
        {
            UtilityTips.ShowRedTips(890);
            return;
        }
        CSMisc.Dot2 dot2 = GetHitPos();

        CSCell cell = CSMesh.Instance.getCell(dot2.x, dot2.y);
        if (cell.isAttributes(MapEditor.CellType.Resistance))
        {
            UtilityTips.ShowRedTips(890);
            return;
        }
        UtilityTips.ShowCenterMoveUpInfo(CSString.Format(889));

        //List<CSMisc.Dot2> coordList = CheckAroundMachDot(mHitDot2, 5);
        CSPathFinderManager.Instance.FindPath(dot2);
        //ShowEndPoint(dot2);
        DelayDrawPaths();
    }

    private void OnGMClickSmallMap(GameObject go)
    {
        CSMisc.Dot2 mHitDot2 = GetHitPos();
        Net.GMCommand("@mv " + mHitDot2.x + " " + mHitDot2.y + " " + CSScene.GetMapID());
    }

    private CSMisc.Dot2 GetHitPos()
    {
        Vector3 mapPos = UICamera.currentTouch.pos - mStartPosition;
        Vector3 hitpos = new Vector3(mapPos.x / MapScale.x, mapPos.y / MapScale.y);
        CSMisc.Dot2 mHitDot2 = CSTouchEvent.dichotomyFind(hitpos, CSCell.Size.x, CSCell.Size.y);

        return mHitDot2;
    }

    private void DelayDrawPaths()
    {
        //DrawPaths(null);
        if(CSAvatarManager.MainPlayer.TouchMove == EControlState.JoyStick)
            return;
        ScriptBinder.Invoke(0.2f, DrawPaths);
    }

    private void DrawPaths()
    {
        if (_PathPoint == null)
        {
            _PathPoint = new UICloneMapPoints();
            _PathPoint.Init(mPathPoint.transform, mpathPoint, 3);
        }

        CSBetterList<Node> list =CSAvatarManager.MainPlayer.Paths;
        _PathPoint.CurMaxCount = list == null ? 0 : list.Count;
        for (int i = 0; i < _PathPoint.CurMaxCount; i++)
        {
            if(_PathPoint.mCurControlList.Count <= i) continue;
            GameObject go = _PathPoint.mCurControlList[i];
            if (go == null) continue;
            CSCell cell = CSMesh.Instance.getCell(list[i].coord.x, list[i].coord.y);
            if (cell != null)
                SetPostion(go.transform, cell.LocalPosition2);
        }

        if (list != null && list.Count > 0)
            ShowEndPoint(list[list.Count - 1].coord);
        else
        {
            mendPoint.SetActive(false);
        }
    }

    private void ShowEndPoint(CSMisc.Dot2 dot2)
    {
        CSCell cell = CSMesh.Instance.getCell(dot2.x, dot2.y);
        if (cell != null)
        {
            SetPostion(mendPoint.transform, cell.LocalPosition2);
            mendLabel.text = $"({dot2.x},{dot2.y})";
            mendPoint.SetActive(true);
        }
    }

    #region ShowPoint

    private void SetMapPoint()
    {
        ShowNpcPoint();
        ShowTranferPoint();
        ShowMonsterPoint();
        ShowSpecialPoint();
    }

    private void ResetPointTabList()
    {
        var arr = MiniMapTableManager.Instance.array.gItem.handles;
        int mapId = mMapId;
        if (_NpcTableList == null) _NpcTableList = new ILBetterList<MINIMAP>(32);
        _NpcTableList.Clear();
        if (_TranferTableList == null) _TranferTableList = new ILBetterList<MINIMAP>(8);
        _TranferTableList.Clear();
        if(_MonsterTableList == null) _MonsterTableList = new ILBetterList<MINIMAP>(16);
        _MonsterTableList.Clear();
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            MINIMAP map = arr[i].Value as TABLE.MINIMAP;
            if (map.mid == mapId)
            {
                if (map.type == 1)
                    _NpcTableList.Add(map);
                else if (map.type == 3)
                    _TranferTableList.Add(map);
                else if(map.type == 2)
                    _MonsterTableList.Add(map);
            }
        }
    }

    private void ShowNpcPoint()
    {
        if (_NpcPoint == null)
        {
            _NpcPoint = new UIGridContainerHot<UIItemMiniMapPoint>();
            _NpcPoint.SetGameObject(mStaticPoints.gameObject, mitemPoint.gameObject).SetBuildPosition(true);
        }

        ShowStaticPoint(_NpcPoint, _NpcTableList);
    }

    private void ShowTranferPoint()
    {
        if (_TransferPoint == null)
        {
            _TransferPoint = new UIGridContainerHot<UIItemMiniMapPoint>();
            _TransferPoint.SetGameObject(mStaticPoints.gameObject, mitemPoint.gameObject).SetBuildPosition(true);
        }
        
        ShowStaticPoint(_TransferPoint, _TranferTableList);
    }
    private void ShowMonsterPoint()
    {
        if (_MonsterPoint == null)
        {
            _MonsterPoint = new UIGridContainerHot<UIItemMiniMapPoint>();
            _MonsterPoint.SetGameObject(mStaticPoints.gameObject, mitemPoint.gameObject).SetBuildPosition(true);
        }
        
        ShowStaticPoint(_MonsterPoint, _MonsterTableList);
    }

    private void ShowStaticPoint(UIGridContainerHot<UIItemMiniMapPoint> _StaticPoint,
        ILBetterList<MINIMAP> _StaticTableList)
    {
        if (_StaticPoint == null) return;

        _StaticPoint.MaxCount = _StaticTableList.Count;
        Vector3 offset = Vector3.zero;
        UIItemMiniMapPoint point;
        MINIMAP minimap;
        for (int i = 0; i < _StaticPoint.MaxCount; i++)
        {
            point = _StaticPoint.controlList[i];
            minimap = _StaticTableList[i];
            if (point == null || minimap == null) continue;
            point.Init();
            if (minimap.type == 1)
            {
                point.SetUIInfo(UIMiniMapType.NPC, i.ToString());
            }
            else if (minimap.type == 3)
                point.SetUIInfo(UIMiniMapType.WayPoint, i.ToString(), 0, minimap.name);
            else if(minimap.type == 2)
            {
                point.SetUIInfo(UIMiniMapType.Monster, i.ToString(), (int)UIMiniMapMonsterType.MapBoss, minimap.name);
            }

            if (minimap.coordOffset.Count >= 2)
                offset.Set(minimap.coordOffset[0], minimap.coordOffset[1], 0);
            else
                offset = Vector3.zero;
            CSCell cell = CSMesh.Instance.getCell(minimap.x, minimap.y);
            if (cell != null)
                SetOffsetPos(point.gameObject.transform, cell.LocalPosition2, offset);
        }
    }

    /// <summary>
    /// 显示特殊点： 队友
    /// </summary>
    private void ShowSpecialPoint()
    {
        if (_SpecialPoint == null)
        {
            _SpecialPoint = new UIGridContainerHot<UIItemMiniMapPoint>();
            _SpecialPoint.SetGameObject(mSpecialPlayerPoints.gameObject, mitemPoint.gameObject).SetBuildPosition(true);
        }

        if (CSMapManager.mActMapViewPoint == null) return;

        _SpecialPoint.MaxCount = CSMapManager.mActMapViewPoint.Count;
        Vector3 offset = Vector3.zero;
        var mapPoint = CSMapManager.mActMapViewPoint.GetEnumerator();
        int index = 0;
        while (mapPoint.MoveNext())
        {
            map.SmallViewTeammateNtf data = mapPoint.Current.Value;
            if (mapPoint.Current.Value == null || index >= _SpecialPoint.controlList.Count) continue;
            UIItemMiniMapPoint point = _SpecialPoint.controlList[index];
            point.Init();
            point.SetUIInfo(UIMiniMapType.SpecialTeam, index.ToString());

            CSCell cell = CSMesh.Instance.getCell(data.x, data.y);
            if (cell != null)
                SetPostion(point.gameObject.transform, cell.LocalPosition2);
            index++;
        }
    }

    #endregion

    private void SetOffsetPos(Transform tr, Vector3 pos, Vector3 offset)
    {
        SetPostion(tr, pos);
        tr.localPosition += offset;
    }

    private void RemovelastPath()
    {
        if (_PathPoint != null)
            _PathPoint.RemoveSingle();
    }

    //获取相对于预制的位置
    private Vector3 GetInPanelPos(Transform obj)
    {
        Vector3 pos = Vector3.zero;
        while (obj != null && obj.name != UIPrefabTrans.name)
        {
            pos += obj.localPosition;
            obj = obj.parent;
        }

        return pos;
    }


    #region 右侧列表

    private void FirstTabsClick(MapFirstTabs _go)
    {
        _go.SetSelect();
        mTable.Reposition();
    }

    private void SecondClick(MapSecondTabs _go)
    {
        if (curSecondTab != null)
        {
            curSecondTab.SetHighLight(false);
        }

        curSecondTab = _go;
        curSecondTab.SetHighLight(true);

        MINIMAP minimap = curSecondTab.GetMiniMap();
        if (minimap != null)
        {
            CSMisc.Dot2 dot;
            dot.x = minimap.x;
            dot.y = minimap.y;
            CSPathFinderManager.Instance.FindPath(dot);
            ShowEndPoint(dot);
            DelayDrawPaths();
        }
    }

    private void RefreshBtn()
    {
        _CurRandomCount = CSBagInfo.Instance.GetItemCount(RANDOM_SHOP_ID);
        //bug单号9862
        _CurRandomCount = _CurRandomCount > 999 ? 999 : _CurRandomCount;
        mlbRandom.text = $"({_CurRandomCount})";
        mlbRandom.text = _CurRandomCount > 0
            ? mlbRandom.text.BBCode(ColorType.Green)
            : mlbRandom.text.BBCode(ColorType.Red);
        _CurBackCount = CSBagInfo.Instance.GetItemCount(BACK_SHOP_ID);
        _CurBackCount = _CurBackCount > 999 ? 999 : _CurBackCount;
        mlbBack.text = $"({_CurBackCount})";
        mlbBack.text = _CurBackCount > 0 ? mlbBack.text.BBCode(ColorType.Green) : mlbBack.text.BBCode(ColorType.Red);
    }


    private void OnBackClick(GameObject go)
    {
        if (_CurBackCount > 0)
        {
            Net.CSBackCityMessage();
        }
        else
        {
            Utility.ShowGetWay(BACK_SHOP_ID);
            //UtilityTips.ShowRedTips(916);
        }
    }

    private void OnRandomClick(GameObject go)
    {
        if (_CurRandomCount > 0)
        {
            Net.CSRandomMapMessage();
        }
        else
        {
            Utility.ShowGetWay(RANDOM_SHOP_ID);
            //UtilityTips.ShowRedTips(915);
        }
    }

    private void BagItemChange(uint id, object data)
    {
        RefreshBtn();
    }

    private bool isShow = false;

    private void OnShowLeftClick(GameObject go)
    {
        isShow = !isShow;
        mright.Play(isShow);
        mbtn_show.Play(isShow);
    }

    private void RefreshRightList()
    {
        if (firstitemList.Count < 2) return;
        firstitemList[0].Refresh(_NpcTableList);
        firstitemList[1].Refresh(_TranferTableList);
        mTable.Reposition();
    }

    #endregion
    
    #region 小地图视野处理
    private void InitViewPoint()
    {
        var avaterList = CSMapManager.GetAvaterByType(MapAvaterType.Monster);
        if (avaterList == null || avaterList.Count == 0) return;
        CSMapAvatarInfo info;
        for (var i = 0; i < avaterList.Count; i++)
        {
            info = avaterList[i];
            if (info == null) continue;
            CSMapAvatarInfo avatar = CSMapManager.GetPoolItem<CSBigMapMonsterInfo>(MapAvaterType.MapMonster);
            avatar.Init(info, mActMonsterPoints, mitemPoint);
            avatar.SetMainPlayerPos(MainPlayerMapPosition);
            CSMapManager.AddBigMapAvatarLoad(avatar, MapAvaterType.MapMonster);
        }
    }
    
    private void RefreshCameraMapView(uint id, object data)
    {
        if(data == null) return;
        map.UpdateViewResponse rsp = data as map.UpdateViewResponse;
        if (rsp == null) return;
        for (int i = 0; i < rsp.exitObjects.Count; ++i)
        {
            CSMapManager.RemoveBigAvatar(rsp.exitObjects[i]);
        }
        for (int i = 0; i < rsp.enterMonsters.Count; ++i)
        {
            EnterMonsterMapView(id, rsp.enterMonsters[i]);
        }
    }

    private void EnterMonsterMapView(uint uiEvtID, object data)
    {
        map.RoundMonster info = data as map.RoundMonster;
        if (info == null) return;
        AddAvatarView<CSBigMapMonsterInfo>(info, info.monsterId, MapAvaterType.MapMonster, mActMonsterPoints);
    }

    private void AddAvatarView<T>(IMessage info, long Id, MapAvaterType type, Transform parent)
        where T : CSMapAvatarInfo, new()
    {
        CSMapManager.RemoveBigAvatar(Id);
        CSMapAvatarInfo avatar = CSMapManager.GetPoolItem<T>((int)type);
        avatar.Init(info, parent, mitemPoint);
        avatar.SetMainPlayerPos(MainPlayerMapPosition/*- mainPlayerPos*/);
        CSMapManager.AddBigMapAvatarLoad(avatar, type);
    }

    private void ExitCameraView(uint id, object data)
    {
        map.ObjectExitViewResponse info = data as map.ObjectExitViewResponse;
        if (info == null) return;
        CSMapManager.RemoveBigAvatar(info.id);
    }
    
    private CSMapAvatarInfo Avater;
    private map.ObjectMoveResponse info;

    private void UpdateMapViewPosition(uint uiEvtID, object data)
    {
        if (data == null) return;
        info = data as map.ObjectMoveResponse;
        if (info == null) return;
        if (MainPlayerID != info.id)
        {
            Avater = CSMapManager.GetBigMapAvatar(info.id);
            if (Avater != null)
            {
                Avater.SetMainPlayerPos(MainPlayerMapPosition);
                Avater.ResetServerCell(info.newX, info.newY);
            }
        }
    }
    #endregion

    protected override void OnDestroy()
    {
        if (_PathPoint != null) _PathPoint.Dispose();
        _PathPoint = null;
        if (firstitemList != null && firstitemList.Count > 0)
        {
            for (var i = 0; i < firstitemList.Count; i++)
            {
                firstitemList[i].Dispose();
            }

            firstitemList.Clear();
        }

        firstitemList = null;
        CSMapManager.RemoveBigMapAll();
        base.OnDestroy();
    }
}

public class MapFirstTabs
{
    public Transform go;
    UITable table;
    UILabel name;
    GameObject obj_highlight;
    UILabel nameHl;
    GameObject arrow;
    private UISprite spIcon;
    bool IsSelected = false;
    Action<MapFirstTabs> action;
    Action<MapSecondTabs> Childaction;
    private UIGridContainer grid;
    private ILBetterList<MapSecondTabs> MapSecondTabsList;

    public MapFirstTabs(GameObject _go,int index, string tabName, Action<MapFirstTabs> _action, GameObject _secondGo,
        Action<MapSecondTabs> _cAction)
    {
        go = _go.transform;
        action = _action;
        Childaction = _cAction;
        table = go.Find("Table").GetComponent<UITable>();
        arrow = go.Find("arrow").gameObject;
        obj_highlight = go.Find("checkmark").gameObject;
        name = go.Find("name").GetComponent<UILabel>();
        nameHl = go.Find("checkmark/name").GetComponent<UILabel>();
        spIcon = go.Find("sp_icon").GetComponent<UISprite>();
        grid = table.transform.GetComponent<UIGridContainer>();
        name.text = tabName;
        nameHl.text = tabName;
        spIcon.spriteName = index == 1 ? "map_NCP" : "map_teleport2";
        MapSecondTabsList = new ILBetterList<MapSecondTabs>();
        UIEventListener.Get(_go).onClick = Click;
    }

    public void Refresh(ILBetterList<MINIMAP> _MapTab)
    {
        if (_MapTab == null) return;
        grid.MaxCount = _MapTab.Count;

        for (int i = 0; i < grid.MaxCount; i++)
        {
            if (MapSecondTabsList.Count < i + 1) MapSecondTabsList.Add(new MapSecondTabs());
            MapSecondTabs data = MapSecondTabsList[i];
            if (!data.isInit)
            {
                data.gameObject = grid.controlList[i];
                data.Init();
            }
            data.Refresh(_MapTab[i], Childaction);
        }

        table.Reposition();
        IsSelected = false;
        SetSelect();
    }

    public void SetSelect()
    {
        IsSelected = !IsSelected;
        obj_highlight.SetActive(IsSelected);
        arrow.transform.localRotation = IsSelected? Quaternion.Euler(Vector3.forward * 180) : Quaternion.Euler(Vector3.zero);
        table.gameObject.SetActive(IsSelected);
        if (IsSelected)
        {
            table.Reposition();
        }
    }

    void Click(GameObject _go)
    {
        if (action != null)
        {
            action(this);
        }
    }

    public void Dispose()
    {
        go = null;
        table = null;
        name = null;
        obj_highlight = null;
        nameHl = null;
        arrow = null;
        action = null;
        Childaction = null;
    }
}

public class MapSecondTabs : GridContainerBase
{
    public UILabel name;
    public UILabel lb_Hlname;
    GameObject obj_Hl;
    Action<MapSecondTabs> action;
    bool state = false;
    private MINIMAP mINIMAP;
    public bool isInit;

    public override void Init()
    {
        base.Init();
        isInit = true;
        name = transform.Find("name").GetComponent<UILabel>();
        obj_Hl = transform.Find("subCheckmark").gameObject;
        lb_Hlname = transform.Find("subCheckmark/name").GetComponent<UILabel>();

        UIEventListener.Get(gameObject).onClick = Click;
    }

    public void Refresh(MINIMAP _minimap, Action<MapSecondTabs> _action)
    {
        action = _action;
        mINIMAP = _minimap;
        lb_Hlname.text = _minimap.name;
        name.text = _minimap.name;
    }

    void Click(GameObject _go)
    {
        if (action != null)
        {
            action(this);
        }
    }

    public MINIMAP GetMiniMap()
    {
        return mINIMAP;
    }

    public void SetHighLight(bool _state)
    {
        state = _state;
        obj_Hl.SetActive(state);
    }

    
    
    public override void Dispose()
    {
        name = null;
        lb_Hlname = null;
        obj_Hl = null;
        action = null;
        mINIMAP = null;
        isInit = false;
    }
}