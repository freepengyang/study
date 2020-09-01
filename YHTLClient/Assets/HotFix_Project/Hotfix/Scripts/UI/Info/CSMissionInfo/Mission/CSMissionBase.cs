using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using TABLE;
using task;
using UnityEngine;

public class CSMissionBase : IDispose
{
    public EventHanlderManager mClientEvent = new EventHanlderManager(EventHanlderManager.DispatchType.Event);
    public float lastGuideTime = -1.0f;

    private int _taskId;

    public int TaskId
    {
        get { return _taskId; }
        set { _taskId = value; }
    }

    private TASKS _tasksTab;

    public TASKS TasksTab
    {
        get
        {
            if (_tasksTab == null)
            {
                TasksTableManager.Instance.TryGetValue(TaskId, out _tasksTab);
            }
            return _tasksTab;
        }
        set { _tasksTab = value; }
    }

    private TASKGOAL _taskGoalTab;

    public TASKGOAL TaskGoalTab
    {
        get
        {
            if (_taskGoalTab == null)
            {
                if(TaskGoalInfoList != null && TaskGoalInfoList.Count > 0)
                {
                    TaskGoalTableManager.Instance.TryGetValue(TaskGoalInfoList[0].goalId, out _taskGoalTab);
                }else
                {
                    TaskGoalTableManager.Instance.TryGetValue(TasksTab.goals, out _taskGoalTab);
                }
            }
            return _taskGoalTab;
        }
        set { _taskGoalTab = value; }
    }

    private TaskState _taskState;

    /// <summary>
    /// 任务状态
    /// </summary>
    public TaskState TaskState
    {
        get { return _taskState; }
        set 
        {
            _taskState = value; 
            if(_taskState == TaskState.Completed)
            {
                lastGuideTime = Time.realtimeSinceStartup;
            }
        }
    }

    private RepeatedField<GoalInfo> _taskGoalInfoList;

    /// <summary>
    /// 任务目标
    /// </summary>
    public RepeatedField<GoalInfo> TaskGoalInfoList
    {
        get { return _taskGoalInfoList; }
        set { _taskGoalInfoList = value; }
    }

    protected Dictionary<EventState, Type> _executeEventDic;

    //任务执行队列
    public virtual Dictionary<EventState, Type> ExecuteEventDic
    {
        get { return _executeEventDic; }
    }

    //排序id
    public long SortId { get; set; }

    //是否是玩家点击做任务  玩家点直接飞，自动做任务寻路
    public bool PlayerOnClck { get; set; }

    public virtual void Init(TaskInfo taskInfo)
    {
        PlayerOnClck = false;
        TaskId = taskInfo.taskId;
        TaskState = (TaskState) taskInfo.state;
        TaskGoalInfoList = taskInfo.goals;
    }
	public virtual void Init(int id)
	{
		PlayerOnClck = false;
		TaskId = id;
		TaskState = TaskState.TaskStateNone;
		TaskGoalInfoList = null;
	}
	public virtual string ShowTitle()
    {
        return TasksTab.name;
    }

    public virtual string ShowState()
    {
        if (TaskState == TaskState.Completed)
        {
            return "";
        }
        else if (TaskState == TaskState.UnAcceptable)
        {
            return CSString.Format(1173, TasksTab.level);
        }
        else
        {
            return CSMissionManager.Instance.TaskStateStr(TaskState);
        }
    }

    public virtual string ShowContent()
    {
        string content = "";

        if (TaskState == TaskState.Acceptable || TaskState == TaskState.Cost)
        {
            content = CSString.Format(530, NpcTableManager.Instance.GetNpcName(TasksTab.fromNPC));
        }
        else if (TaskState == TaskState.Accepted)
        {
            if (TaskGoalTab == null) return content;
            CSStringBuilder.Clear();
            CSStringBuilder.Append(TaskGoalTab.goalTips, " ", "[eee5c3](", TaskGoalInfoList[0].currentCount.ToString(),
                "/", TaskGoalTab.goalCount.ToString(), ")");
            content = CSStringBuilder.ToString();
        }
        else if (TaskState == TaskState.UnAcceptable)
        {
            content = CSString.Format(1266);
        }
        else
        {
            content = TasksTab.goalTips2;
        }

        return content;
    }

    /// <summary>
    /// 特殊文字，用于显示特殊需求，eg：倒计时
    /// </summary>
    /// <returns></returns>
    public virtual string ShowSpecial()
    {
        return "";
    }

    /// <summary>
    /// 默认点击事件，直接寻路
    /// playerClick : 是否是玩家点击  目前当 TasksTab.transfer != 1 时，即使true 也设置为false，，后面有其他需求再改
    /// </summary>
    public virtual void OnClick(bool playerClick = false)
    {
        if (TasksTab.transfer != 1) playerClick = false;
        if (playerClick)
            UIManager.Instance.ClosePanel<UIMissionGuildPanel>();
        PlayerOnClck = playerClick;
        CSMissionEventManager.Instance.StartMission(this);
    }

    /// <summary>
    /// 当寻路到当前NPC有任务时，执行此功能
    /// （直接点击NPC时，不走寻路逻辑，为了统一都调用此处方法）
    /// </summary>
    public virtual void OnReachNpc(int NpcId)
    {
    }


    public void ChangeGoalInfo(GoalInfo goalInfo)
    {
        if (goalInfo == null || TaskGoalInfoList == null) return;
        for (int i = 0; i < TaskGoalInfoList.Count; i++)
        {
            if (TaskGoalInfoList[i].goalId == goalInfo.goalId)
                TaskGoalInfoList[i].currentCount = goalInfo.currentCount;
        }
    }

    public void UpdateMission(TaskInfo taskInfo)
    {
        Init(taskInfo);
    }

    public virtual void Dispose()
    {
        TasksTab = null;
        TaskGoalTab = null;
        TaskState = TaskState.TaskStateNone;
        TaskGoalInfoList?.Clear();
        TaskGoalInfoList = null;
        mClientEvent?.UnRegAll();
        PlayerOnClck = false;
    }
}