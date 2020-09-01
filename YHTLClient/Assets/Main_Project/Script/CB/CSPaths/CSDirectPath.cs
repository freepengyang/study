/********************************************************************
	created:	2016/06/02
	created:	2:6:2016   14:59
	filename: 	E:\GameProject\Client\Branch\Client\Assets\Script\Common\CSDirectPath.cs
	file path:	E:\GameProject\Client\Branch\Client\Assets\Script\Common
	file base:	CSDirectPath
	file ext:	cs
	author:		Jzx
	
	purpose:	direct path use this,not use astar
*********************************************************************/
using UnityEngine;
using System.Collections;

public class CSDirectPath
{
    public class StepData
    {
        public CSMisc.Dot2 start;
        public CSMisc.Dot2 target;
        public CSMisc.Dot2 step;
        public Node startNode;
        public StepData(CSMisc.Dot2 start, CSMisc.Dot2 target)
        {
            this.start = start;
            this.target = target;
            step = (target - start).Normal();
        }

        public void UpdateData(CSMisc.Dot2 start, CSMisc.Dot2 target)
        {
            this.start = start;
            this.target = target;
            step = (target - start).Normal();
        }
    }

    static CSBetterList<Node> normallist = new CSBetterList<Node>();
    static CSBetterList<Node> lastCacheList = new CSBetterList<Node>();
    static CSBetterList<Node> cacheList = new CSBetterList<Node>();

    static Node startNode;
    static Node goalNode;
    static StepData data0 = new StepData(new CSMisc.Dot2(0, 0), new CSMisc.Dot2(0, 0));
    static StepData data1 = new StepData(new CSMisc.Dot2(0, 0), new CSMisc.Dot2(0, 0));

    public static CSBetterList<Node> FindPath(Node start, Node goal)
    {
        CSBetterList<Node> list = normallist;

        if (start == null || goal == null || goal.bObstacle || start.bObstacle || goal.coord.Equal(start.coord))
        {
            list.Clear();
            return list;
        }

        startNode = start;
        goalNode = goal;
        CSMisc.Dot2 dist = goal.coord - start.coord;
        CSMisc.Dot2 distAbs = dist.Abs();
        int leftDist = Mathf.Abs(distAbs.x - distAbs.y);

        CSMisc.Dot2 midStepTarget = start.coord + (distAbs.x > distAbs.y ? new CSMisc.Dot2(leftDist * dist.NormalX(), 0) : new CSMisc.Dot2(0, leftDist * dist.NormalY()));
        data0.UpdateData(start.coord, midStepTarget);
        data1.UpdateData(midStepTarget, goal.coord);

        list.Clear();
        if (IsObstacle(data0, list)) { list.Clear(); return list; }
        if (IsObstacle(data1, list)) { list.Clear(); return list; }
        lastCacheList.Clear();
        lastCacheList.AddRange(cacheList);
        cacheList.Clear();
        cacheList.AddRange(list);
        return list;
    }

    public static CSBetterList<Node> GetDirectionPath(int direction, CSCell s_cell, ref CSBetterList<Node> pathArray)
    {
        CSCell e_cell = s_cell;
        CSCell from_cell = null;
        if (s_cell == null) return pathArray;
        CSMisc.Dot2 mTargetCoord = e_cell.Coord;
        Node node;

        while (!e_cell.isAttributes(MapEditor.CellType.Resistance))
        {
            node = CSMesh.Instance.getNode(e_cell.Coord);

            if (node == null) return pathArray;
            if (!node.IsCanCrossWall()) return pathArray;

            pathArray.Add(node);

            switch (direction)
            {
                case CSDirection.Right:
                    mTargetCoord.x++;
                    break;
                case CSDirection.Right_Up:
                    mTargetCoord.x++;
                    mTargetCoord.y--;
                    break;
                case CSDirection.Up:
                    mTargetCoord.y--;
                    break;
                case CSDirection.Left_Up:
                    mTargetCoord.x--;
                    mTargetCoord.y--;
                    break;
                case CSDirection.Left:
                    mTargetCoord.x--;
                    break;
                case CSDirection.Left_Down:
                    mTargetCoord.x--;
                    mTargetCoord.y++;
                    break;
                case CSDirection.Down:
                    mTargetCoord.y++;
                    break;
                case CSDirection.Right_Down:
                    mTargetCoord.x++;
                    mTargetCoord.y++;
                    break;
            }

            from_cell = e_cell;

            e_cell = CSMesh.Instance.getCell(mTargetCoord.x, mTargetCoord.y);

            if (e_cell == null) return pathArray;

        }

        return pathArray;
    }

    public static Node FindObstructPath(Node start, int direction)
    {
        CSMisc.Dot2 mTargetCoord = start.coord;
        CSCell cell = null;
        switch (direction)
        {
            case CSDirection.Up:
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Up);
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Left_Up);
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Right_Up);
                break;
            case CSDirection.Right_Up:
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Right_Up);
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Up);
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Right);
                break;
            case CSDirection.Right:
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Right);
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Right_Up);
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Right_Down);
                break;
            case CSDirection.Right_Down:
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Right_Down);
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Down);
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Right);
                break;
            case CSDirection.Down:
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Down);
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Right_Down);
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Left_Down);
                break;
            case CSDirection.Left:
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Left);
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Left_Up);
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Left_Down);
                break;
            case CSDirection.Left_Down:
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Left_Down);
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Down);
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Left);
                break;
            case CSDirection.Left_Up:
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Left_Up);
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Left);
                if (cell == null) cell = getAssignNeighbour(start.coord, CSDirection.Up);
                break;
        }

        return (cell != null) ? cell.node : null;
    }

    private static CSCell getAssignNeighbour(CSMisc.Dot2 coord, int direction)
    {
        CSMisc.Dot2 mTargetCoord = coord;
        switch (direction)
        {
            case CSDirection.Right:
                mTargetCoord.x++;
                break;
            case CSDirection.Right_Up:
                mTargetCoord.x++;
                mTargetCoord.y--;
                break;
            case CSDirection.Up:
                mTargetCoord.y--;
                break;
            case CSDirection.Left_Up:
                mTargetCoord.x--;
                mTargetCoord.y--;
                break;
            case CSDirection.Left:
                mTargetCoord.x--;
                break;
            case CSDirection.Left_Down:
                mTargetCoord.x--;
                mTargetCoord.y++;
                break;
            case CSDirection.Down:
                mTargetCoord.y++;
                break;
            case CSDirection.Right_Down:
                mTargetCoord.x++;
                mTargetCoord.y++;
                break;
        }
        CSCell cell = null;
        cell = CSMesh.Instance.getCell(mTargetCoord.x, mTargetCoord.y);
        if (cell != null && !cell.node.bObstacle && cell.node.IsCanCrossWall()) return cell;
        return null;
    }

    static bool IsObstacle(StepData data, CSBetterList<Node> list, bool isMainPlayer = false)
    {
        CSMisc.Dot2 temp = data.start;
       
        CSMisc.Dot2 temp2 = data.start;
        while (!temp.Equal(data.target))
        {
            Node node = CSMesh.Instance.getNode(temp);

            if (node == null || node.bObstacle) return true;

            if (list.Count == 0 || list[list.Count - 1] != node)
                list.Add(node);

            temp = temp + data.step;

        }

        Node target = CSMesh.Instance.getNode(data.target);
        if (list.Count == 0 || list[list.Count - 1] != target)
            list.Add(target);

        return false;
    }

    public static int GetDirection(CSCell cell, Vector3 target,int dir)
    {
        int direction = CSDirection.None;
        int xValue = 0;
        int yValue = 0;
        if (cell.InX(target.x))
        {
            xValue = 0;
        }
        else if (target.x > cell.LocalPosition2.x)
        {
            xValue = 1;
        }
        else if (target.x < cell.LocalPosition2.x)
        {
            xValue = -1;
        }

        if (cell.InY(target.y))
        {
            yValue = 0;
        }
        else if (target.y > cell.LocalPosition2.y)
        {
            yValue = 1;
        }
        else if (target.y < cell.LocalPosition2.y)
        {
            yValue = -1;
        }

        if (xValue == 0 && yValue == 0)      //  本身
        {
            direction = dir;
        }
        else
        {
            byte b = (byte)((xValue + 1) * 10 + (yValue + 1));
            return CSMisc.dirDic[b];
        }
        return direction;
    }

    public static void Clear()
    {
        lastCacheList.Clear();
    }

    public static void DrawLastPaths()
    {
        Color old = Gizmos.color;
        Gizmos.color = Color.red;
        DrawPaths(lastCacheList);
        Gizmos.color = old;
    }

    public static void DrawPaths()
    {
        Color old = Gizmos.color;
        Gizmos.color = Color.red;

        DrawPaths(cacheList);

        if (cacheList.Count > 0)
        {
            Gizmos.color = Color.green;
            CSMiscGizmos.DrawCell(startNode.coord.x, startNode.coord.y);
            Gizmos.color = Color.blue;
            CSMiscGizmos.DrawCell(goalNode.coord.x, goalNode.coord.y);
        }
        Gizmos.color = old;
    }

    public static void DrawPaths(CSBetterList<Node> list)
    {
        if (list.Count == 0) return;

        for (int i = 0; i < list.Count; i++)
        {
            Node node = list[i] as Node;
            CSMiscGizmos.DrawCell(node.coord.x, node.coord.y);
        }
    }
}
