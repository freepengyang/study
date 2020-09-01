using System;
using TABLE;
using UnityEngine;

public class CSMapPanelManager : CSInfo<CSMapPanelManager>
{
    private MAPINFO _tblMapInfo;
    public MAPINFO mTbleMapInfo
    {
        get
        {
            if (_tblMapInfo != null && _tblMapInfo.id == CSScene.GetMapID())
                return _tblMapInfo;
            MapInfoTableManager.Instance.TryGetValue(CSScene.GetMapID(), out _tblMapInfo);
            return _tblMapInfo;
        }
    }

    protected Vector3 mMimSize = new Vector3();
    protected Vector3 mMapScale = new Vector3();
    protected Vector3 mHalfScreenSize;
    protected Vector3 mMapSize = new Vector3();

    public void Init()
    {
        mHalfScreenSize = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    public void ResetMap()
    {
        ReinitMapInfo();
    }

    private void ReinitMapInfo()
    {
        if(mTbleMapInfo == null) return;
        string[] str = mTbleMapInfo.mapSize.Split('#');
        if(str.Length < 4) return;
        mMapSize.x = Convert.ToSingle(str[0]);
        mMapSize.y = Convert.ToSingle(str[1]);
        mMimSize.x = Convert.ToSingle(str[2]);
        mMimSize.y = Convert.ToSingle(str[3]);

        if (mMimSize.x != 0) mMapScale.x = mMapSize.x / mMimSize.x;
        if (mMimSize.y != 0) mMapScale.y = mMapSize.y / mMimSize.y;
    }

    
    public override void Dispose()
    {
    }
}