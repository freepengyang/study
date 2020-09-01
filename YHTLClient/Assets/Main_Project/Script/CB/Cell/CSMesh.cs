
//-------------------------------------------------------------------------
//CSTerrainMeshData
//Author jiabao
//Time 2016.1.11
//-------------------------------------------------------------------------
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
public class CSMesh : Singleton2<CSMesh>
{
    private Vector2 mPixelSize;            // 地图尺寸
    private Vector2 mMeshSize;             // 地图尺寸
    static int mVerticalCount = 10;       // 竖排
    static int mHorizontalCount = 10;     // 横排
    private MapEditor.MapInfoList mClient;
    private Dictionary<int, CSCell> mEditCells = new Dictionary<int, CSCell>(256);//x*100000+y // 编辑格子
    private int viewMaxx, viewMinx, viewMaxy, viewMiny;
    Dictionary<int, GameObject> AllDisplayMesh = new Dictionary<int, GameObject>();
    CSBetterList<int> HideCoord = new CSBetterList<int>();
    CSBetterList<Vector2> UpdateCoord = new CSBetterList<Vector2>();

    private CSMisc.Dot2 mDot2 = new CSMisc.Dot2();

    public static int VerticalCount
    {
        get { return mVerticalCount; }
    }

    public static int HorizontalCount
    {
        get { return mHorizontalCount; }
        set { mHorizontalCount = value; }
    }

    private Vector2 mXRange = Vector2.zero;
    public UnityEngine.Vector2 XRange
    {
        get
        {
            CSCell se_cell = getCell(0, 0);
            CSCell e_cell = getCell(mVerticalCount - 1, 0);
            if (se_cell != null && e_cell != null)
            {
                mXRange.x = Mathf.Min(se_cell.WorldPosition.x, e_cell.WorldPosition.x);
                mXRange.y = Mathf.Max(se_cell.WorldPosition.x, e_cell.WorldPosition.x);
            }
            return mXRange;
        }
        set { mXRange = value; }
    }
    private Vector2 mYRange = Vector2.zero;
    public UnityEngine.Vector2 YRange
    {
        get
        {
            CSCell se_cell = getCell(0, 0);
            CSCell e_cell = getCell(0, mHorizontalCount - 1);
            if (se_cell != null && e_cell != null)
            {
                mYRange.x = Mathf.Min(se_cell.WorldPosition.y, e_cell.WorldPosition.y);
                mYRange.y = Mathf.Max(se_cell.WorldPosition.y, e_cell.WorldPosition.y);
            }

            return mYRange;
        }
        set { mYRange = value; }
    }
    public MapEditor.MapInfoList ClientInfo
    {
        get { return mClient; }
        set { mClient = value; }
    }

    public Dictionary<int, CSCell> EditCells
    {
        get { return mEditCells; }
    }

    public static int mPadVisionCountX = 0;
    public static int PadVisionCountX
    {
        get { return mPadVisionCountX; }
        set { mPadVisionCountX = value; }
    }

    public static int mPadVisionCountY = 0;
    public static int PadVisionCountY
    {
        get { return mPadVisionCountY; }
        set { mPadVisionCountY = value; }
    }

    public int VisionCount = 9;
    public void Init(Vector2 terrainSize)
    {
        ClearData();
        mPixelSize = terrainSize;
        mMeshSize.x = mPixelSize.x;
        mMeshSize.y = mPixelSize.y;
        float exactX = mPixelSize.x % CSCell.Size.x;
        float exactY = mPixelSize.y % CSCell.Size.y;
        mHorizontalCount = (int)(mPixelSize.y / CSCell.Size.y) + (exactY > 0 ? 1 : 0);
        mVerticalCount = (int)(mPixelSize.x / CSCell.Size.x) + (exactX > 0 ? 1 : 0);

        //Debug.LogError(" terrainSize =  " + terrainSize + " mHorizontalCount =  "
        //    + mHorizontalCount + " mVerticalCount = " + mVerticalCount + " mPixelSize = " + mPixelSize);
        if (mClient != null && mClient.grid.Count > 0)
        {
        }
        else
        {
            Build();
        }
    }

    public void Build()
    {
        mPixelSize = CSConstant.mTerrainSize;
        mMeshSize.x = mPixelSize.x;
        mMeshSize.y = mPixelSize.y;
        // 数据编辑格子

        MakeCell();
    }

    public bool hasCell(int key)
    {
        return mEditCells != null && mEditCells.ContainsKey(key);
    }

    public int GetKey(int x, int y)
    {
        return x * 100000 + y;
    }

    public CSCell getCell(CSMisc.Dot2 dot)
    {
        return getCell(dot.x, dot.y);
    }

    public CSCell getCell(int x, int y)
    {
        if (x < 0 || y < 0 || x >= mVerticalCount || y >= mHorizontalCount)  //不加x>=mVerticalCount||y>=mHorizontalCount，会有这样的问题，56*75+20和55*75+95值是一样的，囧
        {
            return null;
        }
        if(mClient == null)
        {
            return null;
        }

        int key = GetKey(x, y);
        CSCell cell = getCell(key);

        if (cell == null)
        {
            int i = y * mVerticalCount + x;

            if (i < mClient.grid.Count)
            {
                MapEditor.MapInfo data = mClient.grid[i];

                if (data != null)
                {
                    cell = new CSCell();
                    cell.Data = data;
                    cell.mCell_x = x;
                    cell.mCell_y = y;
                    mDot2.Clear();
                    mDot2.x = cell.Coord.x;
                    mDot2.y = cell.Coord.y;
                    cell.node = new Node(cell.LocalPosition2, mDot2);

                    if (cell.isAttributes(MapEditor.CellType.Resistance) ||
                        cell.isAttributes(MapEditor.CellType.Separate_1))//隔断2楼
                    {
                        cell.node.MarkAsObstacle();
                    }
                    if (!hasCell(cell.getKey))
                    {
                        mEditCells.Add(cell.getKey, cell);
                    }
                }
            }
        }
        return cell;
    }


    public CSCell getCell(int key)
    {
        CSCell cell = null;
        if (mEditCells != null && mEditCells.TryGetValue(key, out cell))
        {
        }
        return cell;
    }

    //坐标是 （0，0）开始
    private void MakeCell()
    {
       
        if (mClient == null)
        {
            mClient = new MapEditor.MapInfoList();
        }
        if (mClient.grid != null)
        {
            mClient.grid.Clear();
        }

        float X_denominator = CSCell.Size.x;
        float Y_denominator = CSCell.Size.y;

        float exactX = mPixelSize.x % X_denominator;
        float exactY = mPixelSize.y % Y_denominator;

        mHorizontalCount = (int)(mPixelSize.y / Y_denominator) + (exactY > 0 ? 1 : 0);
        mVerticalCount = (int)(mPixelSize.x / X_denominator) + (exactX > 0 ? 1 : 0);

        MapEditor.MapInfo cellData = null;
        float x = 0, y = 0, cloneChild = -1;
        for (int w = 0; w < mHorizontalCount; ++w)    // y
        {
            for (int h = 0; h < mVerticalCount; ++h)  // x
            {

                // 横CLONE
                if (cloneChild != w)
                {
                    cloneChild = w;

                    y = w * CSCell.Size.y;
                }
                x = h * CSCell.Size.x;

                CSCell cell = new CSCell();

                cellData = new MapEditor.MapInfo();

                cell.LocalPosition = new Vector2(x, -y);
                //cell.mCell_x = w;
                //cell.mCell_y = h;
                cell.mCell_x = h;
                cell.mCell_y = w;
                mClient.grid.Add(cellData);
                cell.Data = cellData;
                mDot2.Clear();
                mDot2.x = cell.Coord.x;
                mDot2.y = cell.Coord.y;
                cell.node = new Node(cell.LocalPosition2, mDot2);
                if (!mEditCells.ContainsKey(cell.getKey))
                {
                    mEditCells.Add(cell.getKey, cell);
                }
                else
                {
                    if (FNDebug.developerConsoleVisible) FNDebug.Log("重复 key = " + cell.getKey);
                }
            }
        }
    }

    //-------------------------A-Star-Data---------------------------------------------

    public Node getNode(CSMisc.Dot2 coord)
    {
        return getNode(coord.x, coord.y);
    }

    public Node getNode(int x, int y)
    {
        CSCell cell = getCell(x, y);

        if (cell != null)
        {
            //TODO:ddn
            if(cell.node != null && cell.node.cell == null)
            {
                cell.node.cell = cell;
            }
            return cell.node;
        }

        return null;
    }

    public void GetNeighbours(Node node, CSBetterList<Node> neighbors, bool Separate = false)
    {
        int row = node.coord.x;
        int column = node.coord.y;

        //Left Top
        int leftNodeRow = row - 1;
        int leftNodeColumn = column - 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors, Separate);
        //Left Bootom
        leftNodeRow = row - 1;
        leftNodeColumn = column + 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors, Separate);
        //Right Top
        leftNodeRow = row + 1;
        leftNodeColumn = column - 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors, Separate);
        //Right Bootom
        leftNodeRow = row + 1;
        leftNodeColumn = column + 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors, Separate);

        //Bottom   
        leftNodeRow = row;
        leftNodeColumn = column + 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors, Separate);
        //Top   
        leftNodeRow = row;
        leftNodeColumn = column - 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors, Separate);
        //Right   
        leftNodeRow = row + 1;
        leftNodeColumn = column;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
        //Left   
        leftNodeRow = row - 1;
        leftNodeColumn = column;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
    }

    void AssignNeighbour(int row, int column, CSBetterList<Node> neighbors, bool Separate = false)
    {
        Node node = getNode(row, column);

        if (node != null)
        {
            if (!Separate)
            {
                if (!node.bObstacle)
                {
                    neighbors.Add(node);
                }
            }
            else
            {
                if (node.cell == null) node.cell = getCell(node.coord);
                if (node.cell != null && !node.cell.isAttributes(MapEditor.CellType.Resistance))
                {
                    neighbors.Add(node);
                }
            }
        }
    }

    public void Destroy()
    {
        if (mClient != null)
        {
            mClient.grid.Clear();
        }
        ClearData();
    }

    private void ClearData()
    {
        if (AllDisplayMesh != null)
            AllDisplayMesh.Clear();
        if (HideCoord != null)
            HideCoord.Clear();
        if (UpdateCoord != null)
            UpdateCoord.Clear();
        if (mEditCells != null)
        {
            mEditCells.Clear();
        }
    }


#if UNITY_EDITOR
    List<CSCell> list = new List<CSCell>();
    public List<CSCell> GetViewCell(CSCell OldCell)
    {
        if (list == null)
        {
            list = new List<CSCell>();
        }
        list.Clear();
        if (OldCell == null)
        {
            return list;
        }
        CSMisc.Dot2 coord;
        coord = OldCell.Coord;
        viewMinx = coord.x - VisionCount;
        viewMaxx = coord.x + VisionCount;
        viewMiny = coord.y - VisionCount;
        viewMaxy = coord.y + VisionCount;
        for (int x = ((viewMinx < 0) ? 0 : viewMinx); x < (viewMaxx > mVerticalCount ? mVerticalCount : viewMaxx); ++x)
        {
            for (int y = (viewMiny < 0 ? 0 : viewMiny); y < (viewMaxy > mHorizontalCount ? mHorizontalCount : viewMaxy); ++y)
            {
                CSCell cell = getCell(x, y);
                if (cell != null)
                {
                    list.Add(cell);
                }
                else
                {
                    //if (Debug.developerConsoleVisible) Debug.LogError(x + "|" + y);
                }
            }
        }
        return list;
    }
#endif

}
