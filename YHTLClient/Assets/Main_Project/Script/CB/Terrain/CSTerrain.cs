
//-------------------------------------------
//Terrain Data
//author jiabao
//time 2015.12.28
//-------------------------------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CSTerrain : Singleton2<CSTerrain>
{
    private Transform mCameraTrans;
    public Transform CameraTrans
    {
        get
        {
            if (mCameraTrans == null && Camera.main != null)
                mCameraTrans = Camera.main.transform;
            return mCameraTrans;
        }
    }

    private Transform mEntity;                 // 地图
    private Transform mTextureCellsParent;     // 纹理格子父类 
    private Vector2 mSize;                      // 地图尺寸
    public Dictionary<int, CSCell> mTerrainCells = new Dictionary<int, CSCell>();  // 格子
    private CSMisc.Dot2 mDot2 = new CSMisc.Dot2();
    private Vector3 vt3 = new Vector3();
    public static int VisionX = 2;
    public static int VisionY = 2;
    public int Columns = 10;     // 竖排
    public int Rows = 10;        // 横排

    private int viewMaxx, viewMinx, viewMaxy, viewMiny;
    private CSBetterDic<int, CSTerrainCell> AllDisplayMesh = new CSBetterDic<int, CSTerrainCell>(true, false);
    private Dictionary<int, CSTerrainCell> AllDisplayFinishLoad = new Dictionary<int, CSTerrainCell>();
    private CSBetterList<int> HideCoord = new CSBetterList<int>();
    private CSBetterList<Vector2> UpdateCoord = new CSBetterList<Vector2>();
    private CSMisc.Dot2 coord = new CSMisc.Dot2();
    private CSMisc.Dot2 temCoord = new CSMisc.Dot2();
    private Vector2 minLocalPosition = new Vector2(float.MaxValue, float.MaxValue);

    //private bool mIsCheckRefreshTerrain = false;
    private bool mIsCheckRefreshTerrain = true;

    public bool IsCheckRefreshTerrain
    {
        get { return mIsCheckRefreshTerrain; }
        set { mIsCheckRefreshTerrain = value; }
    }

    private bool mIsWaitLoad = true;
    public bool IsWaitLoad
    {
        get { return mIsWaitLoad; }
        set { mIsWaitLoad = value; }
    }

    public Transform Entity
    {
        get { return mEntity; }
        set { mEntity = value; }
    }

    public Vector2 OldSize
    {
        get { return mSize; }
    }

    public Vector2 NewSize
    {
        get { return new Vector2(Columns * CSTerrainCell.Size.x, Rows * CSTerrainCell.Size.y); }
    }

    public void Build(Transform parent, string map_size = null)
    {
        mEntity = parent;
        // 初始化地图尺寸
        string[] strs = map_size.Split('#');
        float.TryParse(strs[0], out mSize.x);
        float.TryParse(strs[1], out mSize.y);

        float remain_x = mSize.x % CSTerrainCell.Size.x;
        float remain_y = mSize.y % CSTerrainCell.Size.y;

        Columns = (int)(mSize.x / CSTerrainCell.Size.x) + (remain_x > 0 ? 1 : 0);
        Rows = (int)(mSize.y / CSTerrainCell.Size.y) + (remain_y > 0 ? 1 : 0);

        if (mTextureCellsParent != null)
        {
            UnityEngine.Object.DestroyImmediate(mTextureCellsParent.gameObject);
            mTextureCellsParent = null;
        }
        CSConstant.mTerrainSize = NewSize;
        float x = CSTerrainCell.Size.x / 2;
        float y = (Rows * CSTerrainCell.Size.y - CSTerrainCell.Size.y / 2);

        if (mEntity != null)
        {
            mEntity.localPosition = new Vector3(x, y, 1f);
            mEntity.localScale = Vector3.one;
        }

        mTextureCellsParent = new GameObject("TextureCells").transform;

        NGUITools.SetParent(mEntity, mTextureCellsParent.gameObject);

        // 地形纹理格子
        if (mTerrainCells != null)
        {
            mTerrainCells.Clear();
            mTerrainCells = null;
            AllDisplayMesh.Clear();
            AllDisplayFinishLoad.Clear();
            coord.Clear();
            temCoord.Clear();
        }
        CreateCellData(out mTerrainCells);
    }

    public void Update()
    {
        if (mIsCheckRefreshTerrain == true)
        {
            mIsCheckRefreshTerrain = false;
            //refreshDisplayMeshCoord();
        }
    }

    public void Destroy()
    {
        if (AllDisplayMesh != null)
        {
            for (int i = 0; i < AllDisplayMesh.Count; i++)
            {
                CSTerrainCell tCell = AllDisplayMesh.GetValue(i);
                if (tCell != null)
                {
                    CSResourceManager.Singleton.RemoveWaitingQueueDic(tCell.resPath);
                    tCell.onFinish -= Finsh;
                }
            }
        }
        mTerrainCells = null;
        AllDisplayMesh = null;
        AllDisplayFinishLoad = null;
        HideCoord = null;
        UpdateCoord = null;
    }
    private void CreateCellData(out Dictionary<int, CSCell> cells)
    {
        cells = new Dictionary<int, CSCell>();

        CSCell cell = null;
        float x_x = 0, y_y = 0, cloneChild = -1;
        for (int x = 0; x < Rows; ++x)
        {
            for (int y = 0; y < Columns; ++y)
            {
                cell = new CSCell();
                // 横CLONE

                if (cloneChild != x)
                {
                    cloneChild = x;
                    y_y = x * CSTerrainCell.Size.y;
                }

                x_x = y * CSTerrainCell.Size.x;

                cell.Data = new MapEditor.MapInfo();

                cell.mCell_x = x;
                cell.mCell_y = y;
                cell.LocalPosition = new Vector2(x_x, -y_y);
                if (cell.LocalPosition2.x < minLocalPosition.x || cell.LocalPosition2.y < minLocalPosition.y)
                    minLocalPosition = cell.LocalPosition2;
                int key = cell.getKey;

                if (cells.ContainsKey(key))
                    cells[key] = cell;
                else
                    cells.Add(key, cell);
            }
        }

    }
    public void refreshDisplayMeshCoord(CSCell curCell)
    {
        //TODO:ddn
        //if (CSScene.getMapID() == 14103)
        //{
        //    VisionX = 4;
        //    VisionY = 4;
        //}
        //else
        //{
        //    VisionX = 2;
        //    VisionY = 2;
        //}


        HideCoord.Clear();
        for (int i = 0; i < AllDisplayMesh.Count; i++)
        {
            int key = AllDisplayMesh.GetKey(i);
            HideCoord.Add(key);
        }

        UpdateCoord.Clear();

        vt3.x = curCell.LocalPosition2.x + CameraTrans.localPosition.x;
        vt3.y = curCell.LocalPosition2.y + CameraTrans.localPosition.y;

        temCoord = dichotomyFind(vt3, (int)CSTerrainCell.Size.x, (int)CSTerrainCell.Size.y);


        //temCoord = dichotomyFind(curCell.LocalPosition2, (int)CSTerrainCell.Size.x, (int)CSTerrainCell.Size.y);
        //coord.Clear();

        //Debug.LogFormat("======> refreshDisplayMeshCoord: temCoord = ({0},{1})  coord = ({2},{3})", temCoord.x, temCoord.y, coord.x, coord.y);

        if (!coord.Equal(temCoord))
        {
            coord = temCoord;
            viewMinx = coord.x - VisionX;
            viewMaxx = coord.x + VisionX;
            viewMiny = coord.y - VisionY;
            viewMaxy = coord.y + VisionY;
            for (int x = ((viewMinx < 0) ? 0 : viewMinx); x <= (viewMaxx >= Rows ? Rows - 1 : viewMaxx); ++x)
            {
                for (int y = (viewMiny < 0 ? 0 : viewMiny); y <= (viewMaxy >= Columns ? Columns - 1 : viewMaxy); ++y)
                {
                    int key = CSMesh.Instance.GetKey(x, y);
                    CSCell cell;
                    if (mTerrainCells.TryGetValue(key, out cell))
                    {
                        cell = mTerrainCells[key];
                    }
                    if (HideCoord.Contains(key))
                    {
                        HideCoord.Remove(key);
                    }
                    else
                    {
                        UpdateCoord.Add(new Vector2(x, y));
                    }
                }
            }

            DisplayMapChip();
        }
        else
        {
            mIsWaitLoad = true;
        }
    }

    public void ResetPosition(CSCell curCell)
    {
        mIsWaitLoad = false;
        refreshDisplayMeshCoord(curCell);
        mIsCheckRefreshTerrain = true;
    }

    public bool isInDisplayMapDic(int key)
    {
        return AllDisplayMesh != null ? AllDisplayMesh.ContainsKey(key) : false;
    }

    private void DisplayMapChip(bool isChangeScene = false)
    {
        for (int i = 0; i < HideCoord.Count; i++)
        {
            int key = HideCoord[i];
            if (mTerrainCells.ContainsKey(key))
            {
                CSTerrainCell t_cell = AllDisplayMesh[key];
                if (t_cell == null) continue;

                t_cell.Destroy();
                AllDisplayMesh.Remove(key);
                AllDisplayFinishLoad.Remove(key);
                CSResourceManager.Singleton.RemoveWaitingQueueDic(t_cell.resPath);
            }
        }


        HideCoord.Clear();

        for (int i = 0; i < UpdateCoord.Count; ++i)
        {
            int key = (int)UpdateCoord[i].x * 100000 + (int)UpdateCoord[i].y;
            if (mTerrainCells.ContainsKey(key))
            {
                CSCell cell = mTerrainCells[key];

                CSTerrainCell t_cell;

                t_cell = AddTerrainCellPoolItem(typeof(CSTerrainCell));
                if (t_cell == null) t_cell = new CSTerrainCell();
                t_cell.Init(mTextureCellsParent, cell, mIsWaitLoad);
                t_cell.onFinish += Finsh;

                if (!AllDisplayMesh.ContainsKey(t_cell.Cell.getKey))
                {
                    AllDisplayMesh.Add(t_cell.Cell.getKey, t_cell);
                    AllDisplayFinishLoad.Add(t_cell.Cell.getKey, t_cell);
                }
            }
            else
            {
                if (FNDebug.developerConsoleVisible) FNDebug.Log("Terrain no have key = " + key);
            }
        }
        mIsWaitLoad = true;
    }

    private void Finsh(CSTerrainCell t)
    {
        t.onFinish -= Finsh;
        if (t != null && AllDisplayFinishLoad != null && t.Cell != null)
        {
            AllDisplayFinishLoad.Remove(t.Cell.getKey);
        }
    }

    public CSMisc.Dot2 dichotomyFind(Vector3 dot, int x, int y)
    {
        int cellX = Mathf.CeilToInt((dot.x - minLocalPosition.x) / x) - 1;
        int cellY = Rows - Mathf.CeilToInt((dot.y - minLocalPosition.y) / y) - 1;
        int key = GetKey(cellX, cellY);
        if (mTerrainCells.ContainsKey(key))
        {
            return mTerrainCells[key].Coord;
        }
        mDot2.Clear();
        return mDot2;
    }

    public int GetKey(int cellX, int cellY)
    {
        return cellY * 100000 + cellX;
    }

    public void SetTerrainColor(Vector4 color)
    {
        if (AllDisplayMesh != null && AllDisplayMesh.Dic != null)
        {
            CSTerrainCell tCell;
            int key;
            for (int i = 0; i < AllDisplayMesh.Count; i++)
            {
                key = AllDisplayMesh.GetKey(i);
                if (AllDisplayMesh.Dic.TryGetValue(key, out tCell) && tCell.sprite)
                {
                    if (tCell.sprite)
                    {
                        tCell.sprite.SetShader(tCell.sprite.MainMaterial, color, Vector4.one);
                    }
                }
            }
        }
    }

    public CSTerrainCell AddTerrainCellPoolItem(Type type)
    {
        if (CSObjectPoolMgr.Instance == null) return null;
        CSTerrainCell effect = null;
        string s = type.ToString();
        CSObjectPoolItem poolItem = CSObjectPoolMgr.Instance.GetAndAddPoolItem_Class(s, s, null, type, null);
        effect = poolItem.objParam as CSTerrainCell;
        effect.PoolItem = poolItem;
        return effect;
    }
}


