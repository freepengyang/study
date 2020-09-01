
//-------------------------------------------------------------------------
// Cell
// Author jiabao
// Time 2016.1.4
//-------------------------------------------------------------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CSCell 
{
    static public CSMisc.Dot2 Size = new CSMisc.Dot2(60, 40); 

    static public CSMisc.Dot2 HalfSize = new CSMisc.Dot2(30, 20); // 1.5倍 
    public static float width { get { return HalfSize.x; } }
    public static float height { get { return HalfSize.y; } }

    private float worldWidth = 0;
    private float worldHeight = 0;

    public MapEditor.MapInfo Data;

    public Node node;
    public long mXianJiangBuffID = 0;
    public long mBingTianXueDi = 0;
    private Vector2 mWorldPostion;
    private Vector2 Position = Vector2.zero;

    public CSMisc.Dot2 Coord
    {
        get
        {
            CSMisc.Dot2 dot = CSMisc.Dot2.Zero;
            if (Data == null || Data == null) return dot;
            dot.x = mCell_x;
            dot.y = mCell_y;
            return dot;
        }
    }

    public Vector2 WorldPosition
    {
        get
        {
            if (CSPreLoadingBase.CahceWorldTrans != null)
            {
                Vector3 vec = CSPreLoadingBase.CahceWorldTrans.TransformPoint(LocalPosition2);
                mWorldPostion.x = vec.x;
                mWorldPostion.y = vec.y;
            }
            return mWorldPostion;
        }
    }

    public Vector3 WorldPosition3
    {
        get
        {
            Vector3 vec = WorldPosition;
            return vec;
        }
    }
    /// <summary>
    ///渲染地图
    /// </summary>
    public Vector2 LocalPosition
    {
        get
        {
            if (Position == Vector2.zero)
            {
                Position.x = Coord.x * Size.x;
                Position.y = -Coord.y * Size.y;
            }

            return Position;
        }

        set
        {
            Position = value;
        }
    }

    /// <summary>
    /// 格子中心点
    /// </summary>
    public Vector2 LocalPosition2
    {
        get
        {
            Vector2 mVt2;
            mVt2.x = LocalPosition.x + CSCell.HalfSize.x;
            //if (CSScene.Sington == null || CSScene.Sington.Terrain == null) return Vector2.zero;
            //if (HotFix_Invoke.MCSSceneIType == null || HotFix_Invoke.Terrain == null) return Vector2.zero;
            //mVt2.y = CSScene.Sington.Terrain.NewSize.y + LocalPosition.y - CSCell.HalfSize.y;
            mVt2.y = CSConstant.mTerrainSize.y + LocalPosition.y - CSCell.HalfSize.y;

            return mVt2;
        }
    }

    public int mCell_x;

    public int mCell_y;

    public int getKey
    {
        get
        {
            return Coord.x * 100000 + Coord.y;
        }
    }

    public float MaxX
    {
        get
        {
            float value = LocalPosition2.x + width;
            return value;
        }
    }

    public float MinX
    {
        get
        {
            float value = LocalPosition2.x - width;
            return value;
        }
    }

    public float MaxY
    {
        get
        {
            float value = LocalPosition2.y + height;
            return value;
        }
    }

    public float MinY
    {
        get
        {
            float value = LocalPosition2.y - height;
            return value;
        }
    }

    public float WorldMaxX
    {
        get
        {
            float value = WorldPosition.x + worldWidth;
            return value;
        }
    }

    public float WorldMinX
    {
        get
        {
            float value = WorldPosition.x - worldWidth;
            return value;
        }
    }

    public float WorldMaxY
    {
        get
        {
            float value = WorldPosition.y + worldHeight;
            return value;
        }
    }

    public float WorldMinY
    {
        get
        {
            float value = WorldPosition.y - worldHeight;
            return value;
        }
    }

    public CSCell()
    {
        worldWidth = width * CSConstant.PixelRatio;
        worldHeight = height * CSConstant.PixelRatio;
    }

    private bool IsHave(int num, int n)
    {
        return (num & (1 << n)) != 0;
    }

    public bool isAttributes(MapEditor.CellType type)
    {
        if (type == MapEditor.CellType.Normal && Data.Type == 0)
        {
            return true;
        }
        return IsHave(Data.Type, (int)type);
    }

    public bool InX(float value)
    {
        if (MinX <= value)
        {
            if (value <= MaxX)
            {
                return true;
            }
        }
        return false;
    }

    public bool InY(float value)
    {
        if (MinY <= value)
        {
            if (value <= MaxY)
            {
                return true;
            }
        }
        return false;
    }

    public bool isAttributes(int type)
    {
        return isAttributes((MapEditor.CellType)type);
    }

    /// <summary>
    /// 获得指定网格下的localposition2，直接取LocalPosition2，会用CSScene中载入的Terrain来计算
    /// </summary>
    /// <param name="Terrain"></param>
    /// <returns></returns>
    public Vector3 GetLocalPosition2(float YOffset)
    {
            Vector2 mVt2;
            mVt2.x = LocalPosition.x + CSCell.HalfSize.x;
            mVt2.y = YOffset + LocalPosition.y - CSCell.HalfSize.y;
            return mVt2;
    }
}
