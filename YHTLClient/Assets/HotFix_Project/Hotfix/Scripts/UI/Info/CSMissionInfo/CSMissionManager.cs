using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using TABLE;
using task;
using UnityEngine;
using UnityEngine.Analytics;

/// <summary>
/// 更新任务列表中特殊发下的数据
/// </summary>
public class MissionExtraData
{
    //日常任务循环次数
    public int DayCycleNum { get; set; }

    public void Init(TaskList taskList)
    {
        DayCycleNum = taskList.cycleNum;
    }
}

public class CSMissionManager : CSInfo<CSMissionManager>
{
    private Map<int, CSMissionBase> _mMissionDic = new Map<int, CSMissionBase>();

    private FastArrayElementKeepHandle<CSMissionBase> _sortedMissions =
        new FastArrayElementKeepHandle<CSMissionBase>(32);

    private bool _orderDirty = false;

    private CSMissionBase _mission = null;
    private CSMissionBase _curSelectMission;
    private List<CSMissionBase> _mNpcMissionList;
    private Map<TaskType, Type> _mRegisterMissionType;
    public MissionExtraData _MissionExtraData = new MissionExtraData();

    private int autoDoMissionLevel;
    private int autoDoMissionTime;
    private int mainLineDontAutoLevel;

    public CSMissionBase CurSelectMission
    {
        get { return _curSelectMission; }
        set
        {
            if (_curSelectMission == value) return;
            _curSelectMission = value;
            mClientEvent.SendEvent(CEvent.SetSelectMissionState, value != null ? value.TaskId : 0);
        }
    }

    /// <summary>
    /// 注册任务类型对应的脚本
    /// </summary>
    public CSMissionManager()
    {
        _mRegisterMissionType = new Map<TaskType, Type>()
        {
            {TaskType.MainLine, typeof(CSMissionMain)},
            {TaskType.Daily, typeof(CSMissionDaily)},
            {TaskType.BranchLine, typeof(CSMissionBranchLine)},
            {TaskType.TodayCanDo, typeof(CSMissionResident)},
            {TaskType.WantIngot, typeof(CSMissionResident)},
            {TaskType.GetEquip, typeof(CSMissionResident)},
            {TaskType.WantStronger, typeof(CSMissionResident)},
        };

        mClientEvent.AddEvent(CEvent.ActivityLinkChanged, OnActivityLinkChanged);
        mClientEvent.AddEvent(CEvent.MainPlayer_LevelChange, OnMainPlayerLvChanged);
    }

    #region 刷新

    public void InitialMission(task.TaskList taskInfos)
    {
        if (_mMissionDic == null) _mMissionDic = new Map<int, CSMissionBase>();
        _mMissionDic.Clear();
        _sortedMissions.Clear();
        InitSundry();
        AddMissions(taskInfos.tasks);
        _MissionExtraData.Init(taskInfos);
        CheckResidentTask(); //常驻任务

        InitActivityLinks();
        CheckActivityLinks(); //常驻任务

        mClientEvent.SendEvent(CEvent.Task_GoalUpdate);
    }

    public void AddMission(TaskInfo taskInfo)
    {
        if (!_mMissionDic.ContainsKey(taskInfo.taskId))
        {
            var missionBase = Mission(taskInfo);
            if (null == missionBase)
                return;
            _mMissionDic.Add(taskInfo.taskId, missionBase);
            _sortedMissions.Append(missionBase);
            _orderDirty = true;
        }
        else
        {
            var missionBase = _mMissionDic[taskInfo.taskId];
            if ((int) missionBase.TaskState != taskInfo.state)
            {
                _orderDirty = true;
            }

            _mMissionDic[taskInfo.taskId].Init(taskInfo);
        }
    }

    public void TriggerAutoAcceptTask(TaskInfo taskInfo)
    {
        if (null != taskInfo)
        {
            //如果是接受任务响应，就自动做任务
            _mission = _mMissionDic[taskInfo.taskId];
            CurSelectMission = _mission;
            if (_mission.TasksTab.type == (int) TaskType.Daily)
            {
                _mission?.OnClick(true);
                return;
            }

            _mission?.OnClick(false);
        }
    }

    public void AddMissions(RepeatedField<TaskInfo> taskInfos)
    {
        if (taskInfos == null || taskInfos.Count <= 0) return;
        for (int i = 0, max = taskInfos.Count; i < max; i++)
        {
            AddMission(taskInfos[i]);
        }
    }

    public void UpdateMission(TaskGoalUpdateResponse rsp)
    {
        if (_mMissionDic.ContainsKey(rsp.taskId))
        {
            _mMissionDic[rsp.taskId].ChangeGoalInfo(rsp.goal);
            SetMissionSortId(_mMissionDic[rsp.taskId]);
        }
        else
        {
            FNDebug.LogError("Dont have mission");
        }

        mClientEvent.SendEvent(CEvent.Task_GoalUpdate, rsp.taskId);
    }

    List<int> mShowList = new List<int>(4);

    public bool NeedShowCount(int kv)
    {
        if (mShowList.Count == 0)
        {
            int v = 0;
            var tokens = SundryTableManager.Instance.GetSundryEffect(1003).Split('#');
            for (int i = 0, max = tokens.Length; i < max; ++i)
                if (int.TryParse(tokens[i], out v))
                    mShowList.Add(v);
        }

        return mShowList.Contains(kv);
    }

    public void UpdateMission(TaskInfo taskInfo)
    {
        if (_mMissionDic.ContainsKey(taskInfo.taskId))
        {
            _mMissionDic[taskInfo.taskId].UpdateMission(taskInfo);
            SetMissionSortId(_mMissionDic[taskInfo.taskId]);
        }
        else
        {
            FNDebug.LogError("不存在的任务ID == " + taskInfo.taskId);
            return;
        }

        mClientEvent.SendEvent(CEvent.Task_GoalUpdate, taskInfo.taskId);

        //任务状态从消耗变为可接时 或者 完成时，自动做任务
        if (taskInfo.state == (int) TaskState.Acceptable || taskInfo.state == (int) TaskState.Completed)
        {
            _mission = _mMissionDic[taskInfo.taskId];
            CurSelectMission = _mission;

            //日常/支线 任务完成不自动任务
            if (_mission.TasksTab.type == (int) TaskType.Daily || _mission.TasksTab.type == (int) TaskType.BranchLine)
            {
                return;
            }

            CurSelectMission = _mission;
            _mission?.OnClick(false);
        }
    }

    /// <summary>
    /// 提交任务响应回包
    /// </summary>
    /// <param name="rsp"></param>
    public void SubmitTaskMessage(SubmitTaskResponse rsp)
    {
        CSPathFinderManager.Instance.PathGuideState = PathGuideState.None;

        if (rsp == null || rsp.newTasks.Count <= 0) return;
        _mission = null;
        for (int i = 0; i < rsp.newTasks.Count; i++)
        {
            if (_mMissionDic.ContainsKey(rsp.newTasks[i].taskId))
                _mission = _mMissionDic[rsp.newTasks[i].taskId];
            if (_mission != null && _mission.TasksTab.type == (int) TaskType.MainLine)
                break;
        }

        if (_mission == null || _mission.TasksTab == null) return;

        if (_mission.TasksTab.yindao != 0)
        {
            UtilityPath.ReSetPath();
            UtilityPanel.JumpToPanel(_mission.TasksTab.yindao);
            return;
        }

        if (_mission.TasksTab.type == (int) TaskType.Daily)
            _MissionExtraData.DayCycleNum = rsp.cycleNum;

        CSPathFinderManager.Instance.PathGuideState = PathGuideState.AutoMission;

        if (CSMainPlayerInfo.Instance.Level < mainLineDontAutoLevel)
        {
            CSMissionBase missionBase = GetMainMission();
            if (missionBase != null)
            {
                //自动做任务
                CurSelectMission = missionBase;
                missionBase.OnClick(false);
                return;
            }
        }
        else
        {
            return;
        }


        CurSelectMission = _mission;

        //自动做任务
        _mission.OnClick(false);
    }

    delegate bool AcLinkOpen();

    delegate CSMissionBase AcLinkFactory(int activityId);

    Dictionary<int, AcLinkOpen> mAcOpenActions = new Dictionary<int, AcLinkOpen>(8);
    Dictionary<int, AcLinkFactory> mAcLinkFactories = new Dictionary<int, AcLinkFactory>(8);

    void RegisterActivityLink(int activityId, AcLinkFactory acLinkFactory, AcLinkOpen funcOpen)
    {
        if (!mAcOpenActions.ContainsKey(activityId))
        {
            mAcOpenActions.Add(activityId, funcOpen);
        }

        if (!mAcLinkFactories.ContainsKey(activityId))
        {
            mAcLinkFactories.Add(activityId, acLinkFactory);
        }
    }

    void InitActivityLinks()
    {
        RegisterActivityLink(10121, f => { return new CSMissionOrangeEquip(); },
            () => { return CSSpecialActivityMonsterSlayInfo.Instance.IsActivityOpen(); });
    }

    public void CheckActivityLinks()
    {
        var arr = TasksTableManager.Instance.array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as TABLE.TASKS;
            int taskType = item.taskType;
            if (taskType != 5 || item.type != 10)
                continue;

            int activityId = item.id / 100;
            if (!SpecialActivityTableManager.Instance.TryGetValue(activityId, out TABLE.SPECIALACTIVITY specialActivity)
            )
            {
                continue;
            }

            if (_mMissionDic.ContainsKey(item.id))
                continue;

            if (!mAcOpenActions.TryGetValue(activityId, out AcLinkOpen funcOpen))
            {
                continue;
            }

            if (!funcOpen())
                continue;

            if (!mAcLinkFactories.TryGetValue(activityId, out AcLinkFactory acLinkFactory))
            {
                continue;
            }

            if (!TaskGoalTableManager.Instance.TryGetValue(item.goals, out TABLE.TASKGOAL goal))
                continue;

            var mission = acLinkFactory(activityId);
            _mMissionDic.Add(item.id, mission);
            mission.Init(item.id);
            SetMissionSortId(mission);
            _sortedMissions.Append(mission);
            _orderDirty = true;
            FNDebug.Log($"<color=#00ff00>[链接任务]:{item.name} id:{item.id} activityId:{activityId}</color>");
        }
    }

    void OnActivityLinkChanged(uint id, object argv)
    {
        if (!(argv is int taskId))
            return;

        if (!TasksTableManager.Instance.TryGetValue(taskId, out TABLE.TASKS item))
            return;

        if (item.taskType != 5)
            return;

        int activityId = taskId / 100;
        if (!SpecialActivityTableManager.Instance.TryGetValue(activityId, out TABLE.SPECIALACTIVITY specialActivity))
            return;

        if (!mAcOpenActions.TryGetValue(activityId, out AcLinkOpen funcOpen))
            return;

        if (!mAcLinkFactories.TryGetValue(activityId, out AcLinkFactory acLinkFactory))
            return;

        if (!TaskGoalTableManager.Instance.TryGetValue(item.goals, out TABLE.TASKGOAL goal))
            return;

        if (funcOpen())
        {
            if (_mMissionDic.ContainsKey(taskId))
                return;

            var mission = acLinkFactory(activityId);
            if (null == mission)
                return;

            _mMissionDic.Add(item.id, mission);
            mission.Init(item.id);
            SetMissionSortId(mission);
            _sortedMissions.Append(mission);
            _orderDirty = true;
            mClientEvent.SendEvent(CEvent.Task_GoalUpdate, taskId);
        }
        else
        {
            if (!_mMissionDic.ContainsKey(taskId))
                return;
            if (RemoveMission(taskId))
                mClientEvent.SendEvent(CEvent.Task_GoalUpdate, taskId);
        }
    }

    void OnMainPlayerLvChanged(uint id, object argv)
    {
        for (var it = mAcOpenActions.GetEnumerator(); it.MoveNext();)
        {
            mClientEvent.SendEvent(CEvent.ActivityLinkChanged, it.Current.Key * 100);
        }
    }

    /// <summary>
    /// 常驻任务
    /// </summary>
    public void CheckResidentTask()
    {
        bool isShowStronger = true;
        bool isShowGetEquip = true;
        string tempLimit = SundryTableManager.Instance.GetSundryEffect(706);
        int lvLimit = 0;
        int.TryParse(tempLimit, out lvLimit);
        if (CSMainPlayerInfo.Instance.Level < lvLimit)
        {
            isShowStronger = false;
            isShowGetEquip = false;
        }

        for (_mMissionDic.Begin(); _mMissionDic.Next();)
        {
            var missionBase = _mMissionDic.Value;
            if (missionBase == null || missionBase.TasksTab == null) continue;
            if (isShowStronger && missionBase.TasksTab.taskType == (int) TaskType.BranchLine)
            {
                isShowStronger = false;
            }

            if (isShowGetEquip && missionBase.TasksTab.taskType == (int) TaskType.Daily)
            {
                isShowGetEquip = false;
            }

            if (!isShowStronger && !isShowGetEquip)
                break;
        }

        AddResidentTask(isShowStronger, (int) TaskType.WantIngot,
            (int) TaskType.GetEquip, (int) TaskType.WantStronger);

        AddResidentTask(isShowGetEquip, (int) TaskType.TodayCanDo);
    }

    private void AddResidentTask(bool isShow, int idx1, int idx2 = 0, int idx3 = 0)
    {
        var arr = TasksTableManager.Instance.array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as TABLE.TASKS;
            int taskType = item.taskType;
            if (taskType == (int) TaskType.TodayCanDo ||
                taskType == (int) TaskType.GetEquip ||
                taskType == (int) TaskType.WantStronger ||
                taskType == (int) TaskType.WantIngot)
            {
                int type = item.type;
                int id = item.id;
                if (type == idx1 || type == idx2 || type == idx3)
                {
                    if (isShow)
                    {
                        if (!_mMissionDic.ContainsKey(id))
                        {
                            _mMissionDic.Add(id, Mission(id));
                        }
                        else
                            _mMissionDic[id].Init(id);
                    }
                    else
                    {
                        _mMissionDic.Remove(id);
                    }
                }
            }
        }
    }

    public bool RemoveMission(int taskId)
    {
        if (!_mMissionDic.Remove(taskId))
        {
#if UNITY_EDITOR
            FNDebug.LogError("不存在的任务ID ： " + taskId);
#endif
            return false;
        }
        else
        {
            for (int i = 0, max = _sortedMissions.Count; i < max; ++i)
            {
                if (_sortedMissions[i].TaskId == taskId)
                {
                    //这里用这个删除就不需要排序了 不能用SwapErase
                    _sortedMissions.RemoveAt(i);
                    return true;
                }
            }
        }

        return false;
    }

    public void UpdateMissionState(int taskId)
    {
        if (_mMissionDic.ContainsKey(taskId))
        {
            _mMissionDic[taskId].TaskState = TaskState.Submitted;
        }
    }

    #endregion

    #region 接口

    void CmpMissionBase(ref long sortValue, CSMissionBase r)
    {
        sortValue = r.SortId;
    }

    public FastArrayElementKeepHandle<CSMissionBase> GetMission()
    {
        if (_orderDirty)
        {
            _sortedMissions.Sort(CmpMissionBase);
            _orderDirty = true;
        }

        return _sortedMissions;
    }

    /// <summary>
    /// 获取任务目标为NPC的任务，用于处理功能
    /// </summary>
    /// <param name="npcId"></param>
    /// <param name="_mission"></param>
    /// <returns></returns>
    public bool GetNpcMission(int npcId, ref List<CSMissionBase> _mission)
    {
        if (_mNpcMissionList == null) _mNpcMissionList = new List<CSMissionBase>();
        _mNpcMissionList.Clear();
        for (_mMissionDic.Begin(); _mMissionDic.Next();)
        {
            var missionBase = _mMissionDic.Value;
            if (missionBase == null || missionBase.TasksTab == null) continue;
            switch (missionBase.TaskState)
            {
                case TaskState.Acceptable:
                case TaskState.Cost:
                    if (missionBase.TasksTab.fromNPC.Equals(npcId))
                        _mNpcMissionList.Add(missionBase);
                    break;
                case TaskState.Accepted:
                    //if (missionBase.TasksTab.type == (int)TaskType.MainLine)
                    //{
                    if (missionBase.TasksTab.toNPC.Equals(npcId) || missionBase.TasksTab.fromNPC.Equals(npcId))
                        _mNpcMissionList.Add(missionBase);
                    // }
                    break;
                case TaskState.Completed:
                    if (missionBase.TasksTab.toNPC.Equals(npcId))
                        _mNpcMissionList.Add(missionBase);
                    break;
            }
        }

        _mission = _mNpcMissionList;
        if (_mNpcMissionList.Count > 0)
            return true;
        return false;
    }

    /// <summary>
    /// 获取与NPC相关的任务， 用于NPC头顶状态显示
    /// </summary>
    /// <param name="npcId"></param>
    /// <returns></returns>
    public List<CSMissionBase> GetNpcMission(int npcId)
    {
        if (_mNpcMissionList == null) _mNpcMissionList = new List<CSMissionBase>();
        _mNpcMissionList.Clear();
        for (_mMissionDic.Begin(); _mMissionDic.Next();)
        {
            var missionBase = _mMissionDic.Value;
            if (missionBase.TasksTab.fromNPC == npcId)
            {
                _mNpcMissionList.Add(missionBase);
            }
        }

        return _mNpcMissionList;
    }

    /// <summary>
    /// 根据类型获取任务
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public CSMissionBase GetMissionByType(int type)
    {
        for (_mMissionDic.Begin(); _mMissionDic.Next();)
        {
            if ((int) _mMissionDic.Value.TasksTab.type == type)
                return _mMissionDic.Value;
        }

        return null;
    }

    public CSMissionBase GetMissionByTaskId(int taskId)
    {
        if (_mMissionDic != null && _mMissionDic.ContainsKey(taskId))
        {
            return _mMissionDic[taskId];
        }

        return null;
    }

    /// <summary>
    /// 获取任务类型对应文字描述
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public string TaskStateStr(TaskState state)
    {
        if (state == TaskState.Acceptable || state == TaskState.Cost)
        {
            return CSString.Format(525);
        }

        if (state == TaskState.Accepted)
        {
            return "";
        }

        if (state == TaskState.Completed)
        {
            return CSString.Format(526);
        }

        if (state == TaskState.UnAcceptable)
        {
            return CSString.Format(1173);
        }

        return "";
    }

    /// <summary>
    /// 获取主线任务
    /// </summary>
    /// <returns></returns>
    public CSMissionBase GetMainMission()
    {
        for (_mMissionDic.Begin(); _mMissionDic.Next();)
        {
            if (_mMissionDic.Value.TasksTab.type == (int) TaskType.MainLine &&
                _mMissionDic.Value.TaskState == TaskState.Completed)
            {
                return _mMissionDic.Value;
            }
        }

        return null;
    }

    public CSMissionBase GetAcceptedHintedMainMission()
    {
        for (_mMissionDic.Begin(); _mMissionDic.Next();)
        {
            var value = _mMissionDic.Value;
            var taskItem = value.TasksTab;
            if (null == taskItem)
                continue;
            if (taskItem.type != (int) TaskType.MainLine)
                continue;
            if (value.TaskState != TaskState.Accepted)
                continue;
            if (string.IsNullOrEmpty(taskItem.tip4))
                continue;
            return _mMissionDic.Value;
        }

        return null;
    }

    public CSMissionBase GetMissionByType(TaskType eTaskType)
    {
        for (_mMissionDic.Begin(); _mMissionDic.Next();)
        {
            if (_mMissionDic.Value.TasksTab.type == (int) eTaskType)
            {
                return _mMissionDic.Value;
            }
        }

        return null;
    }

    /// <summary>
    /// 小飞鞋传送后自动执行任务逻辑
    /// </summary>
    /// <param name="missionBase"></param>
    public void FlyToGoalRequest(CSMissionBase missionBase)
    {
        if (CSResourceManager.Instance.IsChangingScene) return;
        Net.ReqFlyToGoalMessage(missionBase.TaskId, missionBase.TaskGoalTab.id);
    }

    /// <summary>
    /// 小飞鞋传送后响应
    /// </summary>
    public void FlyToGoalMessage()
    {
        if (CurSelectMission != null)
        {
            CurSelectMission.OnClick(false);
        }
    }

    /// <summary>
    /// 如果NPC身上有任务，则执行对应任务
    /// </summary>
    /// <returns>是否执行了NPC任务</returns>
    public bool DoNpcMission(int npcId)
    {
        List<CSMissionBase> missionBases = null;
        if (!GetNpcMission(npcId, ref missionBases)) return false;
        if (missionBases == null || missionBases.Count == 0) return false;
        UtilityPath.ReSetPath();
        _mission = missionBases[0];
        if (_mission == null) return false;
        _mission.OnReachNpc(npcId);
        return true;
    }

    /// <summary>
    /// 自动开始主线任务
    /// </summary>
    public bool AutoDoMainMission()
    {
        _mission = GetMainMission();
        return AutoDoMission(_mission);
    }

    /// <summary>
    /// 自动开始任务
    /// </summary>
    public bool AutoDoMission(CSMissionBase missionBase)
    {
        if (missionBase != null)
        {
            CurSelectMission = missionBase;
            missionBase.OnClick(false);
            return true;
        }

        return false;
    }

    public void CheckAutoMainMission(Schedule schedule)
    {
        if (!CSScene.IsLanuchMainPlayer) return;
        if (CSMainPlayerInfo.Instance.Level > autoDoMissionLevel)
        {
            Timer.Instance.CancelInvoke(schedule);
            return;
        }

        if (CSAvatarManager.MainPlayer.LastStandTime > 0.1f &&
            Time.time - CSAvatarManager.MainPlayer.LastStandTime > autoDoMissionTime)
        {
            CSAvatarManager.MainPlayer.LastStandTime = Time.time;
            if (CSAvatarManager.MainPlayer.IsDead) return;
            if (MapInfoTableManager.Instance.GetMapInfoType(CSScene.GetMapID()) == 0)
            {
                CSMissionManager.Instance.AutoDoMainMission();
            }
        }
    }

    #endregion

    #region Private

    private CSMissionBase Mission(TaskInfo taskInfo)
    {
        TABLE.TASKS taskItem = null;
        if (!TasksTableManager.Instance.TryGetValue(taskInfo.taskId, out taskItem))
            return null;
        int type = (int) taskItem.type;
        if (_mRegisterMissionType.ContainsKey((TaskType) type))
        {
            _mission = Activator.CreateInstance(_mRegisterMissionType[(TaskType) type]) as CSMissionBase;
            if (null == _mission)
                return null;
        }
        else
            _mission = new CSMissionBase();

        _mission.Init(taskInfo);
        _mission.TasksTab = taskItem;
        SetMissionSortId(_mission);
        return _mission;
    }

    private CSMissionBase Mission(int id)
    {
        int type = TasksTableManager.Instance.GetTasksType(id);
        if (_mRegisterMissionType.ContainsKey((TaskType) type))
            _mission = Activator.CreateInstance(_mRegisterMissionType[(TaskType) type]) as CSMissionBase;
        else
            _mission = new CSMissionBase();
        if (_mission == null) return null;
        _mission.Init(id);
        if (_mission.TasksTab == null) return null;
        SetMissionSortId(_mission);
        return _mission;
    }

    static int[] sortIdMap = new int[]
    {
        1, 2, 4, 5, 3,
    };

    public static void SetMissionSortId(CSMissionBase mission)
    {
        if (mission == null || mission.TasksTab == null)
            return;

        int idx = mission.TasksTab.taskType - 1;
        int sortValue = 0;
        if (idx >= 0 && idx < sortIdMap.Length)
            sortValue = sortIdMap[idx];
        long order = sortValue * 1000000;
        order += (10 - (int) mission.TaskState) * 100000;
        order += mission.TasksTab.subtype * 10000;
        order = order << 32 | (long) mission.TaskId;

        mission.SortId = order;
    }

    private Schedule _schedule;

    private void InitSundry()
    {
        string setting = SundryTableManager.Instance.GetSundryEffect(1019);
        if (!string.IsNullOrEmpty(setting))
        {
            List<int> dataList = UtilityMainMath.SplitStringToIntList(setting);
            if (dataList.Count >= 2)
            {
                autoDoMissionLevel = dataList[0];
                autoDoMissionTime = dataList[1];

                _schedule = Timer.Instance.InvokeRepeating(0, 1, CheckAutoMainMission);
            }
        }

        string level = SundryTableManager.Instance.GetSundryEffect(1054);
        int.TryParse(level, out mainLineDontAutoLevel);
    }

    #endregion

    public override void Dispose()
    {
        if (_mMissionDic != null)
        {
            for (_mMissionDic.Begin(); _mMissionDic.Next();)
            {
                _mMissionDic.Value.Dispose();
            }
        }

        Timer.Instance.CancelInvoke(_schedule);
        _mRegisterMissionType = null;
        _mission = null;
    }
}