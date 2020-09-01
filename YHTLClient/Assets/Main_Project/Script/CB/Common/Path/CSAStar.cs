
//-----------------------------------------
//A星
//author jiabao
//time 2016.3.11
//-----------------------------------------

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : IComparable
{
    public static bool isHaveNpcWall;
    public float nodeTotalCost;
    public float estimatedCost;
    public int Depth = 0;//用来排序用，相同数据排序问题
    public bool bObstacle = false;
    public CSCell cell;
    public Node parent;
    public CSMisc.Dot2 coord;
    public Vector3 position;
    public bool isInOpenList = false;
    public bool isInClonseList = false;

    private long npcID = 0;
    public bool isCanCrossNpc
    {
        get
        {
            return npcID == 0;
        }
    }

    public bool isProtect
    {
        get
        {
            //if (cell == null) cell = SFOut.IScene.getiMesh.getCellByISfCell(coord.x, coord.y);
            if (cell == null) cell = CSMesh.Instance.getCell(coord.x, coord.y);
            if (cell == null) return false;
            if (cell.isAttributes(MapEditor.CellType.Protect)) return true;
            return false;
        }
    }

    public bool isType18
    {
        get
        {
            //if (cell == null) cell = SFOut.IScene.getiMesh.getCellByISfCell(coord.x, coord.y);
            if (cell == null) cell = CSMesh.Instance.getCell(coord.x, coord.y);

            if (cell == null) return false;
            if (cell.isAttributes(MapEditor.CellType.Special_8)) return true;
            return false;
        }
    }

    public bool IsType7
    {
        get
        {
            if (cell == null) cell = CSMesh.Instance.getCell(coord.x, coord.y);
            if (cell == null) return true;
            if (cell.isAttributes(MapEditor.CellType.Special_7))
            {
                return true;
            }
            return false;
        }
    }

    public bool IsCanCrossWall()
    {
        if (cell == null) cell = CSMesh.Instance.getCell(coord.x, coord.y);
        if (cell == null) return true;
        if (cell.isAttributes(MapEditor.CellType.Special_7) && isHaveNpcWall)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 玩家数量
    /// </summary>
    public byte avatarNum
    {
        get { return UpdateAvatarNum(); }
    }

    public byte GetAvatarNum(int type)
    {
        if (!CSConstant.IsLanuchMainPlayer) return 0;
        if (AvatarIDDic == null) return 0;
        byte num = 0;
        var dic = AvatarIDDic.GetEnumerator();
        while (dic.MoveNext())
        {
            if (dic.Current.Value == type)
            {
                num++;
            }
        }
        return num;
    }
  
    private Dictionary<long, int> avatarIDDic = null;
    public Dictionary<long, int> AvatarIDDic
    {
        get
        {
            if (avatarIDDic == null) avatarIDDic = new Dictionary<long, int>();
            return avatarIDDic;
        }
    }

    private Dictionary<long, int> notLoadAvatarIDDic = null;
    public Dictionary<long, int> NotLoadAvatarIDDic
    {
        get
        {
            if (notLoadAvatarIDDic == null) notLoadAvatarIDDic = new Dictionary<long, int>();
            return notLoadAvatarIDDic;
        }
    }

    public Node(Vector3 pos, CSMisc.Dot2 coord)
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.parent = null;
        this.position = pos;
        this.coord = coord;
    }

    public void AddAvatarID( long id, int type, bool isLoad)
    {
        if(id == 0)
        {
            return;
        }
        if (type == EAvatarType.NPC)
        {
            npcID = id;
        }
        if (isLoad)
        {
            if (!AvatarIDDic.ContainsKey(id))
            {
                AvatarIDDic.Add(id, type);
            }
        }
        else
        {
            if (!NotLoadAvatarIDDic.ContainsKey(id))
            {
                NotLoadAvatarIDDic.Add(id, type);
            }
        }
    }

    byte UpdateAvatarNum()
    {
        if (!CSConstant.IsLanuchMainPlayer) return 0;
        if (AvatarIDDic == null) return 0;
        byte num = 0;

        var dic = AvatarIDDic.GetEnumerator();
        while (dic.MoveNext())
        {
            if (dic.Current.Value == EAvatarType.Player /*|| dic.Current.Value == EAvatarType.ZhanHun*/)
            {
                num++;
            }
        }
        return num;
    }

    public void AddItemID(long itemId)
    {
        if (itemId == 0) return;

        if (!AvatarIDDic.ContainsKey(itemId))
        {
            AvatarIDDic.Add(itemId, EAvatarType.Item);
        }
    }

    public void RemoveAvatarID(long avatarID, int avatarType)
    {
        if (avatarID == 0) return;
        if (avatarType == EAvatarType.NPC && avatarID == npcID)
            npcID = 0;

        if (AvatarIDDic.ContainsKey(avatarID))//玩家死亡时，只是倒地，isLoad==true
        {
            AvatarIDDic.Remove(avatarID);
        }
        if (NotLoadAvatarIDDic.ContainsKey(avatarID))//玩家死亡时，只是倒地，isLoad==true
        {
            NotLoadAvatarIDDic.Remove(avatarID);
        }

    }

    public void RemoveItemID(long itemId)
    {
        if (itemId == 0) return;

        if (AvatarIDDic.ContainsKey(itemId))
        {
            AvatarIDDic.Remove(itemId);
        }
    }

    public void MarkAsObstacle(bool b = true)
    {
        this.bObstacle = b;
    }

    public int CompareTo(object obj)
    {
        Node node = (Node)obj;

        if (this.estimatedCost < node.estimatedCost)
            return -1;
        if (this.estimatedCost > node.estimatedCost)
            return 1;

        if (this.Depth < node.Depth)
            return -1;
        if (this.Depth > node.Depth)
            return 1;
        return 0;
    }
}

public class PriorityQueue
{
    private CSBetterList<Node> nodes = new CSBetterList<Node>();

    public int Length
    {
        get { return this.nodes.Count; }
    }

    public Node First()
    {
        if (this.nodes.Count > 0)
        {
            float temp_estimatedCost = float.MaxValue;
            float temp_depth = float.MaxValue;
            int index = -1;
            for (int i = 0; i < nodes.Count; i++)
            {
                Node node = nodes[i];
                if (node.estimatedCost < temp_estimatedCost)
                {
                    index = i;
                    temp_estimatedCost = node.estimatedCost;
                    continue;
                }
                if (node.Depth < temp_depth)
                {
                    index = 0;
                    temp_depth = node.Depth;
                    continue;
                }
            }
            if (index != -1)
            {
                Node node = nodes[index];
                nodes.RemoveAt(index);
                return node;
            }
        }
        return null;
    }

    public void Push(Node node, bool isSort = true)
    {
        node.Depth = nodes.Count;
        this.nodes.Add(node);
    }
   

    public void Reset()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            Node node = nodes[i];
            if (node != null)
            {
                node.isInOpenList = false;
                node.isInClonseList = false;
            }
        }
        nodes.Clear();
    }
}

public class CSAStar
{
    public static PriorityQueue openList = new PriorityQueue();

    public static PriorityQueue closedList = new PriorityQueue();

    private static float HeuristicCloselyCost(Node curNode, Node goalNode)
    {
        Vector2 dir = Vector2.zero;
        dir = Direction(curNode, goalNode, ref dir);
        dir.x = Mathf.Abs(dir.x);
        dir.y = Mathf.Abs(dir.y);
        if (dir.x == 1 && dir.y == 1) return 1.4f;
        return 1;
    }

    private static float HeuristicEstimateCost(Node curNode, Node goalNode)
    {
        Vector2 dir = Vector2.zero;
        dir = Direction(curNode, goalNode, ref dir);
        dir.x = Mathf.Abs(dir.x);
        dir.y = Mathf.Abs(dir.y);
        if (dir.x == 1 && dir.y == 1) return 1.4f;
        return dir.x + dir.y;
    }

    private static Vector2 Direction(Node curNode, Node goalNode, ref Vector2 dir)
    {
        dir.x = goalNode.coord.x - curNode.coord.x;
        dir.y = goalNode.coord.y - curNode.coord.y;
        return dir;
    }

    public static CSBetterList<Node> FindPathSeparateArea(Node start, Node goal, bool isAssist = false, bool isMainPlayer = false, CSBetterList<Node> assistList = null)
    {
        CSBetterList<Node> list = isAssist ? assistList : normalList;
        if (assistList != null) list = assistList;
        if (start.cell == null) start.cell = CSMesh.Instance.getCell(start.coord.x, start.coord.y);
        if (goal.cell == null) goal.cell = CSMesh.Instance.getCell(goal.coord.x, goal.coord.y);
        if (goal == null || start == null || goal.cell.isAttributes(MapEditor.CellType.Resistance) ||
            start.cell.isAttributes(MapEditor.CellType.Resistance) || goal.coord.Equal(start.coord))
        {
            if (list != null) list.Clear();
            return list;
        }

        start.parent = null;
        openList.Push(start);

        start.nodeTotalCost = 0.0f;
        start.estimatedCost = HeuristicEstimateCost(start, goal);
        Node node = null;
        CSBetterList<Node> neighbours = new CSBetterList<Node>();
        while (openList.Length != 0)
        {
            node = openList.First();
            node.isInOpenList = false;
            if (node.coord.Equal(goal.coord))
            {
                Reset();
                return CalculatePath(node);
            }
            neighbours.Clear();

            CSMesh.Instance.GetNeighbours(node, neighbours);

            for (int i = 0; i < neighbours.Count; i++)
            {
                Node neighbourNode = (Node)neighbours[i];

                if (isMainPlayer && isAssist)
                {
                    if (!neighbourNode.coord.Equal(goal.coord))
                    {
                        if (!neighbourNode.isCanCrossNpc) continue;

                        if (!neighbourNode.isProtect && neighbourNode.avatarNum > 0
                            && CSConstant.IsLanuchMainPlayer) continue;
                    }
                }
                if (neighbourNode.cell == null) neighbourNode.cell = CSMesh.Instance.getCell(neighbourNode.coord);
                if (!neighbourNode.isInClonseList && neighbourNode.cell != null && !neighbourNode.cell.isAttributes(MapEditor.CellType.Resistance))
                {
                    float cost = HeuristicCloselyCost(node, neighbourNode);
                    float totalCost = node.nodeTotalCost + cost;
                    float neighbourNodeEstCost = HeuristicEstimateCost(neighbourNode, goal);

                    if (!neighbourNode.isInOpenList || totalCost < neighbourNode.nodeTotalCost)
                    {
                        neighbourNode.nodeTotalCost = totalCost;
                        neighbourNode.parent = node;
                        neighbourNode.estimatedCost = totalCost + neighbourNodeEstCost;
                    }
                    if (!neighbourNode.isInOpenList)
                    {
                        neighbourNode.isInOpenList = true;
                        openList.Push(neighbourNode);
                    }
                }
            }
            node.isInClonseList = true;
            closedList.Push(node, false);
        }
        Reset();
        if (node.position != goal.position)
        {
            if (list != null) list.Clear();
            return list;
        }
        return CalculatePath(node, isAssist);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="start"></param>
    /// <param name="goal"></param>
    /// <param name="isAssist">true:使用辅助列表，不然会把已有的列表数据冲掉</param>
    /// <returns></returns>
    public static CSBetterList<Node> FindPath(Node start, Node goal, bool isAssist = false, bool isMainPlayer = false)
    {
        CSBetterList<Node> list = isAssist ? assistList : normalList;
        if (assistList != null) list = assistList;
        if (goal == null || start == null || goal.bObstacle || start.bObstacle || goal.coord.Equal(start.coord))
        {
            if (list != null) list.Clear();
            return list;
        }
        if (start.cell == null) start.cell = CSMesh.Instance.getCell(start.coord.x, start.coord.y);
        if (goal.cell == null) goal.cell = CSMesh.Instance.getCell(goal.coord.x, goal.coord.y);
        if (start != null && goal != null)
        {
            if (goal.cell.isAttributes(MapEditor.CellType.Separate) && start.cell.isAttributes(MapEditor.CellType.Normal))
            {
                if (list != null) list.Clear();
                return list;
            }
            if (goal.cell.isAttributes(MapEditor.CellType.Normal) && start.cell.isAttributes(MapEditor.CellType.Separate))
            {
                if (list != null) list.Clear();
                return list;
            }
        }
       
        start.parent = null;
        openList.Push(start);

        start.nodeTotalCost = 0.0f;
        start.estimatedCost = HeuristicEstimateCost(start, goal);
        Node node = null;
        CSBetterList<Node> neighbours = new CSBetterList<Node>();
        while (openList.Length != 0)
        {
            node = openList.First();
            node.isInOpenList = false;
            if (node.coord.Equal(goal.coord) || !node.IsCanCrossWall())
            {
                Reset();
                return CalculatePath(node);
            }
            neighbours.Clear();

            CSMesh.Instance.GetNeighbours(node, neighbours);

            for (int i = 0; i < neighbours.Count; i++)
            {
                Node neighbourNode = (Node)neighbours[i];
                if (isMainPlayer && isAssist)
                {
                    if (!neighbourNode.coord.Equal(goal.coord))
                    {
                        if (!neighbourNode.isProtect &&
                            neighbourNode.avatarNum > 0)
                        {
                            continue;
                        }
                    }
                }

                if (!neighbourNode.isInClonseList && !neighbourNode.bObstacle)
                {
                    float cost = HeuristicCloselyCost(node, neighbourNode);
                    float totalCost = node.nodeTotalCost + cost;
                    float neighbourNodeEstCost = HeuristicEstimateCost(neighbourNode, goal);

                    if (!neighbourNode.isInOpenList || totalCost < neighbourNode.nodeTotalCost)
                    {
                        neighbourNode.nodeTotalCost = totalCost;
                        neighbourNode.parent = node;
                        neighbourNode.estimatedCost = totalCost + neighbourNodeEstCost;
                    }
                    if (!neighbourNode.isInOpenList)
                    {
                        neighbourNode.isInOpenList = true;
                        openList.Push(neighbourNode);
                    }
                }
            }
            node.isInClonseList = true;
            closedList.Push(node, false);
        }
        Reset();
        if (node.position != goal.position)
        {
            if (list != null) list.Clear();
            return list;
        }
        return CalculatePath(node, isAssist);
    }

    public static CSBetterList<Node> FindPathInGrassScene(Node start, Node goal, bool isAssist = false, bool isMainPlayer = false, CSBetterList<Node> assistList = null)
    {
        CSBetterList<Node> list = isAssist ? assistList : normalList;
        if (assistList != null) list = assistList;
        if (goal == null || start == null || goal.bObstacle
            || start.bObstacle || goal.coord.Equal(start.coord))
        {
            if (list != null) list.Clear();
            return list;
        }
        if (start.cell == null) start.cell = CSMesh.Instance.getCell(start.coord.x, start.coord.y);
        if (goal.cell == null) goal.cell = CSMesh.Instance.getCell(goal.coord.x, goal.coord.y);
        if (start != null && goal != null)
        {
            if (goal.cell.isAttributes(MapEditor.CellType.Separate) && start.cell.isAttributes(MapEditor.CellType.Normal))
            {
                if (list != null) list.Clear();
                return list;
            }

            if (goal.cell.isAttributes(MapEditor.CellType.Normal) && start.cell.isAttributes(MapEditor.CellType.Separate))
            {
                if (list != null) list.Clear();
                return list;
            }
        }
        start.parent = null;
        openList.Push(start);

        start.nodeTotalCost = 0.0f;
        start.estimatedCost = HeuristicEstimateCost(start, goal);
        Node node = null;
        CSBetterList<Node> neighbours = new CSBetterList<Node>();
        while (openList.Length != 0)
        {
            node = openList.First();
            node.isInOpenList = false;
            if (node.coord.Equal(goal.coord))
            {
                Reset();
                return CalculatePath(node);
            }
            neighbours.Clear();
            // SFOut.IScene.getiMesh.GetNeighbours(node, neighbours);
            CSMesh.Instance.GetNeighbours(node, neighbours);
            for (int i = 0; i < neighbours.Count; i++)
            {
                Node neighbourNode = (Node)neighbours[i];
                if (isMainPlayer && isAssist)
                {
                    if (!neighbourNode.coord.Equal(goal.coord))
                    {
                        if (!neighbourNode.isCanCrossNpc) continue;

                        if (!neighbourNode.isProtect && neighbourNode.avatarNum > 0 && CSConstant.IsLanuchMainPlayer) continue;
                    }
                }

                if (!neighbourNode.isInClonseList && !neighbourNode.bObstacle)
                {
                    float cost = HeuristicCloselyCost(node, neighbourNode);
                    float totalCost = node.nodeTotalCost + cost;
                    float neighbourNodeEstCost = HeuristicEstimateCost(neighbourNode, goal);

                    if (!neighbourNode.isInOpenList || totalCost < neighbourNode.nodeTotalCost)
                    {
                        neighbourNode.nodeTotalCost = totalCost;
                        neighbourNode.parent = node;
                        neighbourNode.estimatedCost = totalCost + neighbourNodeEstCost;
                    }
                    if (!neighbourNode.isInOpenList)
                    {
                        neighbourNode.isInOpenList = true;
                        openList.Push(neighbourNode);
                    }
                }
            }
            node.isInClonseList = true;
            closedList.Push(node, false);
        }
        Reset();
        if (node.position != goal.position)
        {
            if (list != null) list.Clear();
            return list;
        }
        return CalculatePath(node, isAssist);
    }

    static void Reset()
    {
        if (openList != null)
        {
            openList.Reset();
        }
        if (closedList != null)
        {
            closedList.Reset();
        }
    }

    public static CSBetterList<Node> normalList = new CSBetterList<Node>();
    public static CSBetterList<Node> assistList = new CSBetterList<Node>();
    private static CSBetterList<Node> CalculatePath(Node node, bool isAssist = false)
    {
        CSBetterList<Node> data = isAssist ? assistList : normalList;
        data.Clear();
        while (node != null)
        {
            data.Add(node);
            node = node.parent;

            if (data.Count > 1000)
            {
                return null;
            }
        }
        data.Reverse();
        return data;
    }

   
}