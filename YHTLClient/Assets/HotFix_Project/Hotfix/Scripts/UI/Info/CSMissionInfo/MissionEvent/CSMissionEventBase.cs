using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Google.Protobuf.Collections;
using TABLE;
using task;

/**
 * 此脚本提供基础逻辑，有其他逻辑时，在此添加，需要特殊条件时，在自己脚本中重写方法
 * CSMissionEventTask 中提供 通用重写逻辑，特殊逻辑 自己重写
 */
[Flags]
public enum EventState
{
    None = 0,
    Acceptable = 1 << 1,
    Accepted = 1 << 2,
    Completed = 1 << 3,
    Cost = 1 << 4,
    UnAcceptable = 1 << 5,


    Delay = 1 << 7, //有此属性时，需要延迟判断
    FindPath = 1 << 8, //寻路
    FindNpc = 1 << 9, //寻找NPC
    OpenPanel = 1 << 10, //打开功能面板
    StartFight = 1 << 11, //开始战斗
    Special = 1 << 12, //特殊情况
    PickItem = 1 << 13, //拣取副本物品
}

public interface IMissionEvent
{
    EventState StartMission(CSMissionBase mission);

    void Stop();
}

/// <summary>
/// 任务--可接状态
/// </summary>
public class MissionAcceptable : IMissionEvent, IDispose
{
    public virtual EventState StartMission(CSMissionBase mission)
    {
        return EventState.FindNpc;
    }

    public virtual void Stop()
    {
    }

    public virtual void Dispose()
    {
    }
}

/// <summary>
/// 任务--已接取状态
/// </summary>
public class MissionAccepted : IMissionEvent, IDispose
{
    public virtual EventState StartMission(CSMissionBase mission)
    {
        return EventState.None;
    }

    public virtual void Stop()
    {
    }

    public virtual void Dispose()
    {
    }
}

/// <summary>
/// 任务--已完成状态
/// </summary>
public class MissionCompleted : IMissionEvent, IDispose
{
    public virtual EventState StartMission(CSMissionBase mission)
    {
        return EventState.FindNpc;
    }

    public virtual void Stop()
    {
    }

    public virtual void Dispose()
    {
    }
}

/// <summary>
/// 任务--寻路
/// </summary>
public class MissionFindPath : IMissionEvent, IDispose
{
    protected CSMisc.Dot2 point = new CSMisc.Dot2();
    protected CSMissionBase _mission;
    protected List<int> taskGoalX;
    protected List<int> taskGoalY;
    protected EventHanlderManager _mClientEvent = new EventHanlderManager(EventHanlderManager.DispatchType.Event);
    protected bool _isReach;
    private bool isReturn; //如果已经在当期位置时，发送事件会比方法返回快导致死循环

    public virtual EventState StartMission(CSMissionBase mission)
    {
        Reset();
        if (mission == null)
        {
            FNDebug.Log("Mission FindPath   StartMission is null!!!!!!");
            return EventState.None;
        }
        _mission = mission;
        CSPathFinderManager.Instance.ReSetPath(true, true, false);
        CSPathFinderManager.Instance.PathGuideState = PathGuideState.AutoMission;
        MoveToTarget();
        EventState state = NextEventState();
        isReturn = true;
        if (state == EventState.None || _isReach)
            return state;
        return EventState.Delay | state;
    }

    /// <summary>
    /// 返回下一个状态
    /// </summary>
    protected virtual EventState NextEventState()
    {
        return EventState.None;
    }

    public virtual void Stop()
    {
        if (_mission != null)
            _mission.mClientEvent.SendEvent(CEvent.CancelPathFind);
        Reset();
    }

    protected virtual void MoveToTarget()
    {
        if (CSMainPlayerInfo.Instance == null) return;
        taskGoalX = UtilityMainMath.SplitStringToIntList(_mission.TaskGoalTab.x, '#');
        taskGoalY = UtilityMainMath.SplitStringToIntList(_mission.TaskGoalTab.y, '#');
        if (taskGoalX.Count <= 0 || taskGoalY.Count <= 0) return;
        point.x = taskGoalX[0];
        point.y = taskGoalY[0];

        FingPath();
    }

    protected virtual void FingPath()
    {
        if (CSAvatarManager.MainPlayer.TouchMove != EControlState.Idle) return;
        UtilityPath.FindPath(point, _mission.TaskGoalTab.mapId, 0, MoveInSameMap, true);
    }

    /// <summary>
    /// 移动到相同地图，可根据情况，SetFindPoint设置寻路点， 重写MoveInSameMap，可以做别的操作（eg：战斗）
    /// </summary>
    protected virtual void MoveInSameMap()
    {
        if (CSScene.GetMapID() == _mission.TaskGoalTab.mapId)
        {
            if (!SetFindPoint(out point)) return;

            if (Utility.IsNearPlayerInMap(point.x, point.y))
            {
                OnReachNpc(0, null);
                return;
            }
        }
        else
        {
            MoveToTarget();
            return;
        }

        _mClientEvent.AddEvent(CEvent.Reach, OnReachNpc);
        UtilityPath.FindPath(point, _mission.TaskGoalTab.mapId, 0, null, true);
    }

    /// <summary>
    /// 寻路到相同地图时，设置最终目的点， 如果不需要寻路则返回false
    /// </summary>
    /// <param name="dot"></param>
    /// <returns></returns>
    protected virtual bool SetFindPoint(out CSMisc.Dot2 dot)
    {
        dot = point;
        return true;
    }

    protected virtual void OnReachNpc(uint id, object data)
    {
        _isReach = true;
        if (isReturn)
            _mClientEvent.SendEvent(CEvent.StartNextMissionEvent, _mission.TaskId);
    }

    protected virtual void Reset()
    {
        _mission = null;
        _isReach = false;
        point.Clear();
        taskGoalX?.Clear();
        taskGoalY?.Clear();
        _mClientEvent.UnRegAll();
        isReturn = false;
    }

    public virtual void Dispose()
    {
        Reset();
    }
}

/// <summary>
/// 任务--传送到NPC
/// </summary>
public class MissionDeliver : IMissionEvent, IDispose
{
    public virtual EventState StartMission(CSMissionBase mission)
    {
        return EventState.None;
    }

    protected void Deliver(int deliverId)
    {
        UtilityPath.FindWithDeliverId(deliverId);
    }

    public virtual void Stop()
    {
    }

    public virtual void Dispose()
    {
    }
}

/// <summary>
/// 任务--打开面板
/// </summary>
public class MissionOpenPanel : IMissionEvent, IDispose
{
    public virtual EventState StartMission(CSMissionBase mission)
    {
        return EventState.None;
    }

    protected void OpenPanel(int panelId)
    {
        UtilityPanel.JumpToPanel(panelId);
    }

    public virtual void Stop()
    {
    }

    public virtual void Dispose()
    {
    }
}

/// <summary>
/// 任务--开始战斗
/// </summary>
public class MissionStartFight : IMissionEvent, IDispose
{
    public virtual EventState StartMission(CSMissionBase mission)
    {
        CSPathFinderManager.Instance.PathGuideState = PathGuideState.AutoMission;

        if (mission.TaskGoalTab != null)
            StartFight(mission.TaskGoalTab);
        return EventState.None;
    }

    protected virtual void StartFight(TASKGOAL taskgoalTab)
    {
    }

    public virtual void Stop()
    {
    }

    public virtual void Dispose()
    {
    }
}

/// <summary>
/// 任务--寻找NPC
/// </summary>
public class MissionFindNpc : IMissionEvent, IDispose
{
    protected CSMissionBase _mission;

    //是否到达目的地，用于已经在目的地的判断，否则事件结束，监听还没开始
    protected bool _isReach;
    private bool isReturn;

    public virtual EventState StartMission(CSMissionBase mission)
    {
        Reset();
        if (mission == null) return EventState.None;
        _mission = mission;
        UtilityPath.ReSetPath(true, true, false);
        StartMission();
        CSPathFinderManager.Instance.PathGuideState = PathGuideState.AutoMission;
        mission.mClientEvent.AddEvent(CEvent.Reach, OnReachNpc);
        EventState state = NextEventState();
        isReturn = true;
        if (state == EventState.None || _isReach)
            return state;
        return EventState.Delay | state;
    }

    /// <summary>
    /// 自己执行 FindNpc 方法
    /// </summary>
    protected virtual void StartMission()
    {
        if (_mission.TaskState == TaskState.Acceptable)
            FindNpc(_mission.TasksTab.fromNPC);
        else
            FindNpc(_mission.TasksTab.toNPC);
    }

    protected virtual EventState NextEventState()
    {
        return EventState.None;
    }


    public virtual void Stop()
    {
        if (_mission != null)
            _mission.mClientEvent.SendEvent(CEvent.CancelPathFind);
    }


    protected virtual void FindNpc(int npcId)
    {
        UtilityPath.FindNpc(npcId);
    }

    protected virtual void OnReachNpc(uint id, object data)
    {
        _isReach = true;
        if (_mission != null && isReturn)
        {
            _mission.mClientEvent.RemoveEvent(CEvent.Reach, OnReachNpc);
            _mission.mClientEvent.SendEvent(CEvent.StartNextMissionEvent, _mission.TaskId);
        }
    }

    protected virtual void Reset()
    {
        _mission = null;
        _isReach = false;
        isReturn = false;
    }

    public virtual void Dispose()
    {
        Reset();
    }
}

/// <summary>
/// 任务-物品拾取
/// </summary>
public class MissionPickItem : IMissionEvent, IDispose
{
    private bool isPick = false;
    private bool isReturn = false;
    CSMissionBase _mission;
    public virtual EventState StartMission(CSMissionBase mission)
    {
        Reset();
        CSAutoFightManager.Instance.Stop();
        if (null != mission)
        {
            _mission = mission;
            isPick = true;
            FNDebug.LogFormat("<color=#00ff00>[PickItem]:注册事件</color>");
            mission.mClientEvent.AddEvent(CEvent.AutoPickItem_Over, OnPickItemOver);
            CSAutoFightManager.Instance.IsAutoPickItem = true;
            EventState state = NextEventState();

            isReturn = true;
            if (!isPick)
            {
                return state;
            }
            return EventState.Delay | state;
        }
        return EventState.None;
    }

    protected virtual void Reset()
    {
        FNDebug.LogFormat("<color=#00ff00>[PickItem]:移除事件</color>");
        if (_mission != null)
        {
            _mission.mClientEvent.RemoveEvent(CEvent.AutoPickItem_Over, OnPickItemOver);
            _mission = null;
        }
        isPick = false;
        isReturn = false;
    }

    protected virtual void OnPickItemOver(uint id, object data)
    {
        FNDebug.LogFormat("<color=#00ff00>[PickItem]:OnPickItemOver</color>");
        if (_mission != null)
        {
            _mission.mClientEvent.RemoveEvent(CEvent.AutoPickItem_Over, OnPickItemOver);
            if(isReturn)
                _mission.mClientEvent.SendEvent(CEvent.StartNextMissionEvent, _mission.TaskId);
        }
        isPick = false;
    }

    protected virtual EventState NextEventState()
    {
        return EventState.None;
    }

    public virtual void Stop()
    {

    }

    public virtual void Dispose()
    {
        Reset();
    }
}

// <summary>
/// 任务--开始战斗
/// </summary>
public class MissionSpecial : IMissionEvent, IDispose
{
    public virtual EventState StartMission(CSMissionBase mission)
    {
        if (mission.TaskGoalTab != null)
            Execute(mission);
        return EventState.None;
    }

    protected virtual void Execute(CSMissionBase mission)
    {
    }

    public virtual void Stop()
    {
    }

    public virtual void Dispose()
    {
    }
}