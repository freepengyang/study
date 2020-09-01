﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public static class UIDebugInfo
{
    public class Data
    {
        public GroupData parent;
        public Color color;
        public string log;
        public bool isSelect = false;
        public int gcIndex = 0;
        public void Reset()
        {
            parent = null;
            color = Color.white;
            log = "";
            isSelect = false;
            gcIndex = 0;
        }
    }

    public class GroupData
    {
        public GroupData(string gName)
        {
            groupName = gName;
        }
        public bool isFolder = true;
        public string groupName;
        public Color color;
        public int groupIndex = 0;
        public int gcIndex = 0;
        public bool isSelect = false;
        public long groupNum = 0;//用来查看包的先后顺序
        public CSBetterList<Data> list = new CSBetterList<Data>();
        public void Reset()
        {
            groupName = "";
            color = Color.white;
            isFolder = true;
            list.Clear();
            gcIndex = 0;
            groupIndex = 0;
            isSelect = false;
            groupNum = 0;
        } 
    }


    public static string FilterMsg = string.Empty;
    public static int maxGroupNum = 100;
    public static Dictionary<int, CSBetterList<GroupData>> dataDic = new Dictionary<int, CSBetterList<GroupData>>();
    public static Dictionary<int, CSBetterList<GroupData>> copyDataDic = new Dictionary<int, CSBetterList<GroupData>>();
    public static int selectTogIndex = 0;
    public static float bgAlpha = 0.2f;
    public static bool isCheckDisableBoxCollider = false;
    public static long groupMaxNum = 0;
    public static List<string> togNameList = new List<string>()
    {
        "消息包MSG",
        "频繁MSG",
        "异常",
        "普通日志"
    };

    public static int selectPopIndex = 0;
    public static List<string> popNameList = new List<string>()
    {
        "调整背景透明度",
        "搜索",
        "过滤(消息包MSG)"
    };

    private static GroupData curGroupData = null;

    public static void BeginGroup(ELogToggleType type,string gName,ELogColorType colorType = ELogColorType.White)
    {
        if (!FNDebug.developerConsoleVisible) return;
        if (type == ELogToggleType.NormalMSG && !gName.ToLower().Contains(FilterMsg.ToLower())) return;
#if UNITY_EDITOR
        FNDebug.Log(type + " " + gName);
#endif
        if (!dataDic.ContainsKey((int)type))
            dataDic.Add((int)type, new CSBetterList<GroupData>());
        if (dataDic[(int)type].Count > maxGroupNum) dataDic[(int)type].RemoveAt(0);
        curGroupData = dataDic[(int)type].GetUselessData();
        if (curGroupData != null)
        {
            curGroupData.Reset();
        }
        else
        {
            curGroupData = new GroupData(gName);
        }
        curGroupData.groupNum = groupMaxNum++;
        curGroupData.groupName = gName;
        curGroupData.color = GetColor(colorType);
        dataDic[(int)type].Add(curGroupData);
    }

    public static void EndGroup()
    {
        curGroupData = null;
    }

    public static void AddLog(ELogToggleType type, string log, ELogColorType colorType = ELogColorType.White)
    {
        if (!FNDebug.developerConsoleVisible) return;
        if (!dataDic.ContainsKey((int)type))
            dataDic.Add((int)type, new CSBetterList<GroupData>());
        GroupData gData = AddGroupData(type,dataDic[(int)type], log, colorType);
        if (gData == null) return;
        //HotManager.Instance.EventHandler.SendEvent(CEvent.UIDebugLogNotify, (int)type);
    }

    static GroupData AddGroupData(ELogToggleType type,CSBetterList<GroupData> groupList, string log, ELogColorType colorType = ELogColorType.White)
    {
        GroupData gdata = null;
        if (type == ELogToggleType.NormalMSG && curGroupData == null) return null;//普通消息包必须用BeginGroup EndGroup包裹
        if (curGroupData != null)
        {
            gdata = curGroupData;
        }
        else
        {
            if (groupList.Count > maxGroupNum) groupList.RemoveAt(0);
            gdata = groupList.GetUselessData();
            if (gdata != null)
            {
                gdata.Reset();
                gdata.color = GetColor(colorType);
                gdata.groupName = log;
            }
        }

        if (gdata == null)
        {
            gdata = new GroupData(log);
            gdata.color = GetColor(colorType);
            groupList.Add(gdata);
        }
        
        gdata.groupNum = groupMaxNum++;
        Data data = gdata.list.GetUselessData();
        if (data != null) data.Reset();
        if (data == null) data = new Data();
        data.parent = gdata;
        data.log = log;
        data.color = GetColor(colorType);
        gdata.list.Add(data);
        return gdata;
    }

    static Color GetColor(ELogColorType colorType)
    {
        if (colorType == ELogColorType.White) return Color.white;
        if (colorType == ELogColorType.Yellow) return Color.yellow;
        if (colorType == ELogColorType.Red) return Color.red;
        if (colorType == ELogColorType.Green) return Color.green;
        return Color.white;
    }

    public static CSBetterList<GroupData> GetGroupList(ELogToggleType type)
    {
        if (!copyDataDic.ContainsKey((int)type))
            copyDataDic.Add((int)type, new CSBetterList<GroupData>());
        copyDataDic[(int)type].Clear();
        if (dataDic.ContainsKey((int)type))
        {
            dataDic[(int)type].GetRange(0, dataDic[(int)type].Count, copyDataDic[(int)type]);
        }
        return copyDataDic[(int)type];
    }

    public static void Clear(int togType = -1)
    {
        if (togType == -1)
        {
            dataDic.Clear();
            copyDataDic.Clear();
        }
        else
        {
            if (dataDic.ContainsKey(togType))
                dataDic[togType].Clear();
            if (copyDataDic.ContainsKey(togType))
                copyDataDic[togType].Clear();
        }
    }

    public static void AddStackTrace(string info, ELogToggleType logType = ELogToggleType.Exception,ELogColorType groupColorType = ELogColorType.Green, ELogColorType colorType = ELogColorType.Yellow)
    {
        string s = StackTraceUtility.ExtractStackTrace();
        string[] strs = s.Split('\n');
        if (strs.Length <= 1) return;
        for (int i = 1; i < strs.Length; i++)
        {
            if (i == 1)
            {
                UIDebugInfo.BeginGroup(logType, strs[i], groupColorType);
                UIDebugInfo.AddLog(logType, "Custom Info = " + info + " 时间=" /* +CSServerTime.Instance.ServerNows.ToString()*/, colorType);
            }
            else
            {
                UIDebugInfo.AddLog(logType, strs[i],colorType);
            }
        }
        UIDebugInfo.EndGroup();
    }
}
