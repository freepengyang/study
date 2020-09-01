using System;
using System.Collections.Generic;
using System.Linq;
using TABLE;
using task;

public class CSNPCDialogData : IDispose
{
    public CSNPCDialogData(CSMissionBase mission)
    {
        Mission = mission;
        CurTaskId = mission.TaskId;
        CurTaskState = mission.TaskState;
        CurTaskTab = mission.TasksTab;
        MissionList.Add(mission);
    }

    public CSNPCDialogData(List<CSMissionBase> missionList, int npcId)
    {
        if (missionList != null && missionList.Count > 1)
        {
            Mission = missionList[0];
            CurTaskId = missionList[0].TaskId;
            CurTaskState = missionList[0].TaskState;
            CurTaskTab = missionList[0].TasksTab;
        }
    }

    public CSNPCDialogData()
    {
    }

    private List<CSMissionBase> _mMissionList = new List<CSMissionBase>();

    public List<CSMissionBase> MissionList
    {
        get { return _mMissionList; }
        set { _mMissionList = value; }
    }

    public CSMissionBase Mission { get; set; }
    public int CurTaskId { get; set; }
    public TaskState CurTaskState { get; set; }

    private TASKS _curTaskTab;

    public TASKS CurTaskTab
    {
        get
        {
            if (CurTaskId == -1)
                return null;
            if (_curTaskTab != null)
                return _curTaskTab;
            TasksTableManager.Instance.TryGetValue(CurTaskId, out _curTaskTab);
            return _curTaskTab;
        }
        set { _curTaskTab = value; }
    }

    private int _npcId;

    public int NpcId
    {
        get
        {
            if (Mission == null || Mission.TasksTab == null)
                return _npcId;
            if (_npcId == 0)
            {
                if (CurTaskState == TaskState.Acceptable)
                    _npcId = CurTaskTab.fromNPC;
                else if (CurTaskState == TaskState.Accepted || CurTaskState == TaskState.Completed)
                    _npcId = CurTaskTab.toNPC;
            }

            return _npcId;
        }
        set { _npcId = value; }
    }

    private NPC _curNpc;

    public NPC CurNPC
    {
        get
        {
            if (NpcId == 0)
                return null;
            if (_curNpc == null || _curNpc.id != NpcId)
                NpcTableManager.Instance.TryGetValue(NpcId, out _curNpc);
            return _curNpc;
        }
    }

    /// <summary>
    /// 固定时间后关闭面板
    /// </summary>
    public int Time { get; set; }

    private int _mTaskProcess = -1;

    /// <summary>
    /// 0-3:有选择 4:接受任务
    /// </summary>
    public int TaskProcess
    {
        get
        {
            if (_mTaskProcess == -1)
            {
                if (CurNPC != null)
                {
                    if (!string.IsNullOrEmpty(CurNPC.openPanel))
                        _mTaskProcess = 0;
                    else
                        _mTaskProcess = 4;
                }
            }

            return _mTaskProcess;
        }
        set { _mTaskProcess = value; }
    }

    public string TaskTitle
    {
        get
        {
            if (CurTaskTab == null)
                return string.Empty;
            return CurTaskTab.name;
        }
    }

    public string NpcName
    {
        get
        {
            if (CurNPC == null)
                return string.Empty;
            return CurNPC.name;
        }
    }

    /// <summary>
    /// 绑银数
    /// </summary>
    public int SilverCount { get; set; }

    public string TaskDescription
    {
        get
        {
            if (CurTaskTab == null)
                return NormalContent;

            switch (CurTaskState)
            {
                case TaskState.Acceptable:
                    return CurTaskTab.tip1;
                case TaskState.Accepted:
                    return CurTaskTab.tip3;
                case TaskState.Completed:
                    return CurTaskTab.tip2;
                default:
                    return NormalContent;
            }
        }
    }

    public string NormalContent
    {
        get
        {
            if (CurNPC == null)
                return string.Empty;
            return CurNPC.say;
        }
    }

    /// <summary>
    /// 用于显示 活动剩余次数
    /// </summary>
    public string LimitContext
    {
        get
        {
            if (CurNPC == null)
                return string.Empty;

            return string.Empty;
        }
    }

    private readonly List<NPCTaskReward> _taskRewards = new List<NPCTaskReward>();

    public List<NPCTaskReward> TaskRewards
    {
        get
        {
            _taskRewards.Clear();
            if (CurTaskTab == null || CurTaskTab.rewards == null) return _taskRewards;
            List<int> rewardList = UtilityMainMath.SplitStringToIntList(CurTaskTab.rewards, '#');
            if (rewardList == null || rewardList.Count == 0) return _taskRewards;
            for (int i = 0; i < rewardList.Count; i++)
            {
                if (TaskRewardsTableManager.Instance.TryGetValue(rewardList[i], out TASKREWARDS reward))
                {
                    if(!IsEqual(reward.sex, reward.career)) continue;
                    if (reward.rewardType == 1)
                    {
                        Dictionary<int, int> BoxReward = new Dictionary<int, int>();
                        BoxTableManager.Instance.GetBoxAwardById(reward.itemId, BoxReward);
                        if (BoxReward.Count > 0)
                        {
                            for (var it = BoxReward.GetEnumerator();it.MoveNext();)
                            {
                                if (ItemTableManager.Instance.TryGetValue(it.Current.Key, out ITEM boxItem))
                                {
                                    _taskRewards.Add(new NPCTaskReward()
                                        {TableItem = boxItem, Count = it.Current.Value});
                                }
                            }
                        }
                    }
                    else
                    {
                        if (ItemTableManager.Instance.TryGetValue(reward.itemId, out ITEM item))
                        {
                            if (IsEqual(item.sex, item.career))
                            {
                                int count = 0;
                                if(CSMissionManager.Instance.NeedShowCount(item.id))
                                {
                                    count = reward.count;
                                }

                                _taskRewards.Add(new NPCTaskReward() {TableItem = item, Count = count });
                                continue;
                            }

                            _taskRewards.Add(new NPCTaskReward() {TableItem = item, Count = reward.count});
                        }
                    }
                }
            }

            return _taskRewards;
        }
    }

    private CSNPCDialogButtonBase _NpcButtons = null;

    public CSNPCDialogButtonBase NpcButtons
    {
        get
        {
            if (_NpcButtons == null)
            {
                try
                {
                    Type buttonType;
                    if (CurTaskTab != null)
                    {
                        buttonType = CSNpcOperaButtonMgr.GetDialogButtonForTask((TaskType)CurTaskTab.type);
                    }
                    else
                    {
                        buttonType = CSNpcOperaButtonMgr.GetDialogButtonForNpc(NpcId);
                    }

                    if (buttonType != null)
                        _NpcButtons = Activator.CreateInstance(buttonType) as CSNPCDialogButtonBase;
					else
                        _NpcButtons = new CSNPCDialogCommonButton();

                    _NpcButtons?.InitTasks(this);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.Log("CSNpcDialogButton create error:" + ex);
                }
            }

            return _NpcButtons;
        }
    }


    private bool IsEqual(int sex, int careers)
    {
        if ((sex == 2 || CSMainPlayerInfo.Instance.Sex.Equals(sex)) && (careers == 0 || CSMainPlayerInfo.Instance.Career.Equals(careers)))
        {
            return true;
        }

        return false;
    }

    public void Dispose()
    {
        _mMissionList.Clear();
        Mission = null;
        CurTaskId = 0;
        CurTaskState = TaskState.TaskStateNone;
        _npcId = 0;
        CurTaskTab = null;
        _mTaskProcess = -1;
        _taskRewards.Clear();
    }
}

public class NPCTaskReward
{
    public int Count { get; set; }
    public ITEM TableItem { get; set; }
}