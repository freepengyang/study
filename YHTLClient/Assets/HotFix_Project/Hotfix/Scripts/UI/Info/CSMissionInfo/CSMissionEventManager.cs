using System;
using System.Collections;
using System.Collections.Generic;
using task;

public class CSMissionEventManager : CSInfo<CSMissionEventManager>
{
    private Dictionary<EventState, Type> ExecuteEventDic { set; get; }

    //基类行为，不重写的话，自动调用 CSMissionEventTask
    private Dictionary<EventState, Type> _BaseEventDic;
    private Dictionary<Type, IMissionEvent> LoadMissionDic = new Dictionary<Type, IMissionEvent>();

    public CSMissionBase _CurMission { get; set; }
    public IMissionEvent _CurMissionEvent { get; set; }

    private EventState _nextEventState;

    public CSMissionEventManager()
    {
        _BaseEventDic = new Dictionary<EventState, Type>(16);
        _BaseEventDic.Add(EventState.UnAcceptable, typeof(MissionTaskUnAcceptable));
        _BaseEventDic.Add(EventState.Acceptable, typeof(MissionTaskAcceptable));
        _BaseEventDic.Add(EventState.Accepted, typeof(MissionTaskAccepted));
        _BaseEventDic.Add(EventState.Completed, typeof(MissionTaskCompleted));
        _BaseEventDic.Add(EventState.Cost, typeof(MissionTaskAcceptable));
        _BaseEventDic.Add(EventState.FindPath, typeof(MissionTaskFindPath));
        _BaseEventDic.Add(EventState.OpenPanel, typeof(MissionTaskOpenPanel));
        _BaseEventDic.Add(EventState.StartFight, typeof(MissionTaskStartFight));
        _BaseEventDic.Add(EventState.FindNpc, typeof(MissionTaskFindNpc));
        _BaseEventDic.Add(EventState.Special, typeof(MissionTaskSpecial));
        _BaseEventDic.Add(EventState.PickItem, typeof(MissionTaskPickItem));
    }

    public void StartMission(CSMissionBase mission)
    {
        _CurMission = mission;
        if (mission == null)
        {
            FNDebug.LogError("CSMissionEventManager 传入mission = null");
            return;
        }

        ExecuteEventDic = mission.ExecuteEventDic;

        switch (_CurMission.TaskState)
        {
            case TaskState.UnAcceptable:
                _nextEventState = EventState.UnAcceptable;
                break;
            case TaskState.Acceptable:
            case TaskState.Cost:
                _nextEventState = EventState.Acceptable;
                break;
            case TaskState.Accepted:
                _nextEventState = EventState.Accepted;
                break;
            case TaskState.Completed:
                _nextEventState = EventState.Completed;
                break;
        }

        Start();
    }

    private void Start()
    {
        IMissionEvent missionEvent = GetMissionEvent(_nextEventState);
        if (missionEvent == null)
        {
            ExecuteEventDic = null;
            _CurMissionEvent = null;
            FNDebug.Log("CSMissionEventManager missionEvent = null");
            return;
        }

        if (_CurMissionEvent != null)
        {
            _CurMissionEvent.Stop();
        }

        _CurMissionEvent = missionEvent;
        _nextEventState = missionEvent.StartMission(_CurMission);

        CheckNextEvent();
    }

    private void CheckNextEvent()
    {
        if (_nextEventState == EventState.None)
        {
            ExecuteEventDic = null;
            _CurMission = null;
            _CurMissionEvent = null;
            mClientEvent.UnRegMsg((int)CEvent.StartNextMissionEvent);
            FNDebug.Log("CSMissionEventManager CheckNextEvent  _nextEventState= null");
            return;
        }

        EventState isDelay = _nextEventState & EventState.Delay;
        _nextEventState &= ~EventState.Delay;

        if (isDelay != EventState.Delay)
        {
            _CurMissionEvent = null;
            Start();
        }
        else
            mClientEvent.AddEvent(CEvent.StartNextMissionEvent, StartNextMissionEvent);
    }


    private IMissionEvent GetMissionEvent(EventState state)
    {
        Type type;
        if ((ExecuteEventDic != null && ExecuteEventDic.ContainsKey(state)) || (_BaseEventDic != null &&  _BaseEventDic.ContainsKey(state)))
        {
            if (ExecuteEventDic != null &&ExecuteEventDic.ContainsKey(state))
                type = ExecuteEventDic[state];
            else
                type = _BaseEventDic[state];

            if (LoadMissionDic.ContainsKey(type))
            {
                return LoadMissionDic[type];
            }
            else
            {
                try
                {
                    IMissionEvent missionEvent = Activator.CreateInstance(type) as IMissionEvent;
                    LoadMissionDic.Add(type, missionEvent);
                    return missionEvent;
                }
                catch (Exception e)
                {
                    FNDebug.LogError(e);
                    FNDebug.LogError($"CSMissionEventManager GetMissionEvent  {type} dont  IMissionEvent");
                }
            }
        }

        return null; 
    }

    private void StartNextMissionEvent(uint id, object data)
    {
        int taskId = (int) data;
        if (_CurMission != null && taskId != _CurMission.TaskId)
        {
            FNDebug.LogError($"CSMissionEventManager    taskId != _CurMission.TaskId !! taskId:{taskId}  --  _CurMission.TaskId:{_CurMission.TaskId}");
            return;
        }
        _CurMissionEvent = null;
        mClientEvent.UnReg(CEvent.StartNextMissionEvent, StartNextMissionEvent);
        Start();
    }


    public override void Dispose()
    {
        ExecuteEventDic = null;
        LoadMissionDic?.Clear();
        _BaseEventDic?.Clear();
        _BaseEventDic = null;
        LoadMissionDic = null;

        _CurMissionEvent = null;
    }
}