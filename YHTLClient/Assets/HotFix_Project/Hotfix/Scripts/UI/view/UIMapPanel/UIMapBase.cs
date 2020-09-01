using TABLE;
using UnityEngine;

public abstract class UIMapBase : UIBasePanel
{
    protected abstract GameObject mMaptexture { get; }


    protected int mMapId
    {
        get { return CSScene.GetMapID(); }
    }

    protected virtual bool mTextureSnap
    {
        get { return true; }
    }

    protected virtual bool mIsCanBeDelete
    {
        get { return false; }
    }

    protected Vector3 position = Vector3.zero;
    protected Vector3 MapScale { get; set; }
    protected Vector3 HalfMapSize { get; set; }

    protected CSMapManager CSMapManager { get; set; }

    protected CSTerrain CsTerrain { get; set; }

    protected CSMisc.Dot2 playerPosition = CSMisc.Dot2.Zero;

    private bool isShow;
    public override void Init()
    {
        base.Init();
        CSMapManager = CSMapManager.Instance;
        CsTerrain = CSTerrain.Instance;
        mClientEvent.AddEvent(CEvent.UpdateMapInfo, UpdateMapInfo);
        mClientEvent.AddEvent(CEvent.Scene_EnterSceneAfter, UpdateMapInfoAfterScene);
        mClientEvent.AddEvent(CEvent.Scene_UpdateRoleMove, UpdateMoveCellCoord);
        CSAvatarManager.MainPlayer.BaseInfo.EventHandler.AddEvent(CEvent.MainPlayer_StopTrigger, PlayerStop);
        HotManager.Instance.MainEventHandler.AddEvent(MainEvent.MainPlayer_DirectionChange, MainPlayerDirectionChange);

    }

    public override void Show()
    {
        base.Show();
        if(!isShow)
        {
            isShow = true;
            UpdateMapInfo(0, null);
        }
    }

    public void SetMapInUI()
    {
        if (mMaptexture != null)
        {
            CSEffectPlayMgr.Instance.ShowUITexture(mMaptexture.gameObject,
                $"m_{CSMapManager.mTbleMapInfo.img}", ResourceType.MiniMap, mTextureSnap, null, mIsCanBeDelete);
        }
    }

    private void UpdateMapInfo(uint id, object data)
    {
        SetMapInUI();
        UpdateInfo();
        UpdateMapInfo();
    }

    protected virtual void UpdateMoveCellCoord(uint id, object data)
    {
        playerPosition = CSAvatarManager.MainPlayer.NewCell.Coord;
    }

    protected virtual void UpdateMapInfoAfterScene(uint id, object data)
    {
    }

    protected virtual void MainPlayerDirectionChange(uint id, object data)
    {
    }

    protected abstract void UpdateMapInfo();

    private void UpdateInfo()
    {
        MapScale = CSMapManager.mMapScale;
        HalfMapSize = CSMapManager.mMimSize / 2;
    }


    protected virtual void SetPostion(Transform tr, Vector3 pos)
    {
        if (tr == null || MapScale == Vector3.zero) return;
        position.x = pos.x * MapScale.x;
        position.y = (pos.y - (CsTerrain.NewSize.y - CsTerrain.OldSize.y)) * MapScale.y;
    }

    protected virtual void PlayerStop(uint id, object data)
    {
    }

    protected override void OnDestroy()
    {
        if (mMaptexture != null)
            CSEffectPlayMgr.Instance.Recycle(mMaptexture.gameObject);
        CSAvatarManager.MainPlayer.BaseInfo.EventHandler.RemoveEvent(CEvent.MainPlayer_StopTrigger, PlayerStop);
        HotManager.Instance.MainEventHandler.UnReg((uint) MainEvent.MainPlayer_DirectionChange,
            MainPlayerDirectionChange);
        base.OnDestroy();
    }
}