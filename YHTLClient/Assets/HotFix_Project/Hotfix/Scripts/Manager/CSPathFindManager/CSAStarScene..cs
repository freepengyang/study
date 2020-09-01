using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景寻路 A*
/// </summary>
public class CSAStarScene
{
    public class Node_Scene
    {
        public Node_Scene Parent;
        public int SceneID;
        public TABLE.EVENT EventTblToNext = null; //前往下个节点的EventID，如果没有下一个节点，此处为null
        public int cost = 0;
        public readonly List<TableData> linkSceneData = new List<TableData>();
    }

    public struct TableData
    {
        public int id;
        public TABLE.EVENT tbl;
    }

    private static readonly CSBetterList<Node_Scene> closedList = new CSBetterList<Node_Scene>();
    private static readonly CSBetterList<Node_Scene> openList = new CSBetterList<Node_Scene>();
    private static readonly CSBetterList<Node_Scene> list = new CSBetterList<Node_Scene>();
    private static readonly Dictionary<int, Node_Scene> dic = new Dictionary<int, Node_Scene>();
    private static readonly CSBetterList<Node_Scene> nodeSceneList = new CSBetterList<Node_Scene>();


    public static CSBetterList<Node_Scene> FindPath(int StartSceneID, int goalStartSceneID ,bool IsMainPlayer = true)
    {
        if (StartSceneID == goalStartSceneID)
        {
            list.Clear();
            return list;
        }

        if (!MapInfoTableManager.Instance.ContainsKey(StartSceneID))
        {
            if (FNDebug.developerConsoleVisible) FNDebug.LogError("can not find the SceneID = " + StartSceneID);
        }

        if (!MapInfoTableManager.Instance.ContainsKey(goalStartSceneID))
        {
            if (FNDebug.developerConsoleVisible) FNDebug.LogError("can not find the SceneID = " + goalStartSceneID);
        }

        InitData();
        ResetData();
        if (!dic.ContainsKey(StartSceneID) || !dic.ContainsKey(goalStartSceneID))
        {
            FNDebug.LogError("Can not find Start or Goal SceneID = " + StartSceneID + " " + goalStartSceneID);
            list.Clear();
            return list;
        }
        Node_Scene start = dic[StartSceneID];
        if (IsMainPlayer)
        {
            start.linkSceneData.Sort((lhs, rhs) =>
            {
                return Utility.DistanceFromPlayer(lhs.tbl.x, lhs.tbl.y) -
                       Utility.DistanceFromPlayer(rhs.tbl.x, rhs.tbl.y);
            });
        }


        start.cost = 0;
        openList.Clear();
        closedList.Clear();
        openList.Add(start);
        //注意死循环
        while (openList.Count > 0)
        {
            Node_Scene node = openList[0];
            openList.RemoveAt(0);
            for (int i = 0; i < node.linkSceneData.Count; i++)
            {
                int sceneId = node.linkSceneData[i].id;
                Node_Scene next;
                if (!dic.ContainsKey(sceneId))
                {
                    if (FNDebug.developerConsoleVisible) FNDebug.LogError("Can not find Scene id = " + sceneId);
                    continue;
                }
                next = dic[sceneId];

                if (sceneId == goalStartSceneID)
                {
                    next.Parent = node;
                    next.EventTblToNext = node.linkSceneData[i].tbl;
                    return CalculatePath(next);
                } 
                if (!IsContain(closedList, next))
                {
                    int cost = node.cost + 1;
                    if (next != null && cost < next.cost)
                    {
                        next.cost = cost;
                        next.Parent = node;
                        next.EventTblToNext = node.linkSceneData[i].tbl;
                        if (!IsContain(openList, next))
                            openList.Add(next);
                    }
                }
            }

            closedList.Add(node);
        }

        return list;
    }

    static bool IsContain(CSBetterList<Node_Scene> q, Node_Scene n)
    {
        if (n == null) return false;
        for (int i = 0; i < q.Count; i++)
        {
            Node_Scene ns = q[i] as Node_Scene;
            if (ns != null && ns.SceneID == n.SceneID) return true;
        }

        return false;
    }


    static void InitData()
    {
        if (dic != null && dic.Count > 0) 
            return;

        var arr = EventTableManager.Instance.array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            TABLE.EVENT tbl = arr[i].Value as TABLE.EVENT;

            Node_Scene data = null;
            if (!dic.ContainsKey(tbl.mapId))
            {
                data = new Node_Scene();
                data.SceneID = tbl.mapId;
                dic.Add(tbl.mapId, data);
                nodeSceneList.Add(data);
            }
            else
            {
                data = dic[tbl.mapId];
            }

            string[] strs = tbl.param.Split('_');
            if(strs.Length <= 0) continue;

            int linkSceneID = 0;

            if (strs.Length > 5)
            {
                linkSceneID = Convert.ToInt32(strs[5]);
                AddContainTableData(data.linkSceneData, linkSceneID, tbl);
            }

            linkSceneID = Convert.ToInt32(strs[0]);
            AddContainTableData(data.linkSceneData, linkSceneID, tbl);
        }
    }

    static void AddContainTableData(List<TableData> list, int id, TABLE.EVENT table)
    {
        if (list == null) return;
        bool isAdd = false;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].id == id)
            {
                isAdd = true;
                
            }
        }
        if (!isAdd)
        {
            TableData tbldata = new TableData();
            tbldata.id = id;
            tbldata.tbl = table;
            list.Add(tbldata);
        }
    }


    static void ResetData()
    {
        for (int i = 0; i < nodeSceneList.Count; i++)
        {
            Node_Scene data = nodeSceneList[i];
            data.cost = int.MaxValue;
            data.Parent = null;
            data.EventTblToNext = null;
        }
    }

    private static CSBetterList<Node_Scene> CalculatePath(Node_Scene node)
    {
        list.Clear();
        while (node != null)
        {
            list.Add(node);
            node = node.Parent;

            if (list.Count > 1000)
            {
                return list;
            }
        }

        list.Reverse();
        list.RemoveAt(0);
        return list;
    }
}