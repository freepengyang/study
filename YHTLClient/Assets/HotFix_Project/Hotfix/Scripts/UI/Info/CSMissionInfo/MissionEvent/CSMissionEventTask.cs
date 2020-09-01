using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using TABLE;
using task;

/**
* 此脚本是将 TaskGoal 中的 逻辑统一写到此处，后续有新增类型都在此处写，避免重复写
* 
* 此脚本仅用于 通用重写逻辑，特殊逻辑 自己新增脚本重写
*/
/// <summary>
/// 任务--不可接状态
/// </summary>
public class MissionTaskUnAcceptable : MissionAcceptable
{
    public override EventState StartMission(CSMissionBase mission)
    {
		//int level = mission.TasksTab.level;
		//UtilityTips.ShowRedTips(CSString.Format(1261, level));
        HotManager.Instance.EventHandler.SendEvent(CEvent.ShowMissionUnAccept, mission.TaskId);
        return EventState.None;
    }
}

/// <summary>
/// 任务--可接状态
/// </summary>
public class MissionTaskAcceptable : MissionAcceptable
{
    public override EventState StartMission(CSMissionBase mission)
    {
        return EventState.FindNpc;
    }
}

/// <summary>
/// 任务--已接取状态
/// </summary>
public class MissionTaskAccepted : MissionAccepted
{
    static System.Text.RegularExpressions.Regex ms_match = new System.Text.RegularExpressions.Regex(@"(\w+):(\S+)");
    public override EventState StartMission(CSMissionBase mission)
    {
        if(null == mission || null == mission.TaskGoalTab)
            return EventState.None;

        int tgType = mission.TaskGoalTab.type;
        bool needLink = true;
        if (tgType == 60)
            needLink = mission.PlayerOnClck;

        if(tgType == 64)
        {
            needLink = CSScene.GetMapID() != mission.TaskGoalTab.mapId;
        }

        if (needLink && ms_match.IsMatch(mission.TaskGoalTab.links))
        {
            var links = mission.TaskGoalTab.links.Split('#');
            for(int i = 0,max = links.Length;i < max;++i)
            {
                if(!ms_match.IsMatch(links[i]))
                {
                    continue;
                }
                var match = ms_match.Match(links[i]);
                int usage = -1;
                if (int.TryParse(match.Groups[1].Value, out usage) && usage == (int)TaskState.Accepted)
                {
                    if (CSSuperLink.Instance.Link(match.Groups[2].Value))
                    {
                        if (tgType == 60 || tgType == 62 || tgType == 3)
                        {
                            return EventState.FindPath;
                        }
                        return EventState.None;
                    }
                }
            }
        }

        if (tgType == 60 || tgType == 62 || tgType == 3)
        {
            return EventState.FindPath;
        }

        bool fightNow = false;
        var goal = mission.TaskGoalTab;
        var deliver = goal.deliver;
        if(!mission.PlayerOnClck && deliver.Count > 0 && deliver[0] > 0)
        {
            fightNow = true;
        }

        TaskGoalType goalType = (TaskGoalType) mission.TaskGoalTab.type;
        if (goalType == TaskGoalType.FightMonster || goalType == TaskGoalType.MapKillMonster ||
            goalType == TaskGoalType.MapKillQualityMonster ||
            goalType == TaskGoalType.FightMoreMonsterOfOne || (int)goalType == 64)
        {
            if(!fightNow)
                return EventState.FindPath;
            return EventState.StartFight;
        }

        //CollectItem 还没遇到 暂定为NPC  后续遇到再视情况修改
        if (goalType == TaskGoalType.Dialogue || goalType == TaskGoalType.CollectItem)
            return EventState.FindNpc;

        if (goalType == TaskGoalType.GetVigorValue)
            return EventState.OpenPanel;

        if (goalType == TaskGoalType.CompleteInstance)
        {
            if (mission.TasksTab.transfer == 0 || CSScene.GetMapID() == mission.TaskGoalTab.mapId)
            {
                return EventState.FindPath;
            }
            return EventState.Special;
        }

		if (goalType == TaskGoalType.GetEquipWay)
			return EventState.Special;

		return EventState.None;
    }
}

/// <summary>
/// 任务--已完成状态
/// </summary>
public class MissionTaskCompleted : MissionCompleted
{
    public override EventState StartMission(CSMissionBase mission)
    {
        if (null != mission && null != mission.TasksTab && mission.TasksTab.toNPC == 0)
        {
            FNDebug.Log($"<color=#00ff00>Submit Task {mission.TasksTab.id} For ToNPC = 0</color>");
            Net.ReqSubmitTaskMessage(mission.TasksTab.id);
            return EventState.None;
        }

        TaskGoalType goalType = (TaskGoalType)mission.TaskGoalTab.type;
        if (goalType == TaskGoalType.CompleteInstance)
        {
            FNDebug.LogFormat("<color=#00ff00>[PickItem]</color>");
            return EventState.PickItem;
        }

        return EventState.FindNpc;
    }
}


/// <summary>
/// 任务--寻路
/// </summary>
public class MissionTaskFindPath : MissionFindPath
{
    protected override EventState NextEventState()
    {
        TaskGoalType goalType = (TaskGoalType) _mission.TaskGoalTab.type;
        if (goalType == TaskGoalType.FightMonster || goalType == TaskGoalType.MapKillMonster ||
            goalType == TaskGoalType.MapKillQualityMonster ||
            goalType == TaskGoalType.FightMoreMonsterOfOne || goalType == TaskGoalType.CompleteInstance)
        {
            return EventState.StartFight;
        }

        int tgType = _mission.TaskGoalTab.type;
        if (tgType == 60 || tgType == 62 || tgType == 3 || (int)goalType == 64)
        {
            return EventState.StartFight;
        }

        return EventState.None;
    }

    protected override void FingPath()
    {
        var goal = _mission.TaskGoalTab;
        var deliver = goal.deliver;
        if (_mission.PlayerOnClck)
        {
            if(deliver.Count > 0 && deliver[0] > 0)
            {
                CSMissionManager.Instance.FlyToGoalRequest(_mission);
            }
            else
            {
                if (Utility.IsNearPlayerInMap(mapId: goal.mapId, goalX: point.x, goalY: point.y))
                {
                    base.FingPath();
                }
                else
                {
                    CSMissionManager.Instance.FlyToGoalRequest(_mission);
                }
            }
        }
        else
        {
            base.FingPath();
        }
    }
}


/// <summary>
/// 任务--寻找NPC
/// </summary>
public class MissionTaskFindNpc : MissionFindNpc
{
    protected override EventState NextEventState()
    {
        return EventState.None;
    }

    protected override void FindNpc(int npcId)
    {
        if (_mission.PlayerOnClck)
        {
            UtilityPath.FlyToNpc(npcId);
        }
        else
        {
            UtilityPath.FindNpc(npcId);
        }
    }
}


/// <summary>
/// 任务--开始战斗
/// </summary>
public class MissionTaskStartFight : MissionStartFight
{
    protected override void StartFight(TASKGOAL taskgoalTab)
    {
        if (/*taskgoalTab.goalParam == null || */taskgoalTab.goalParam.Count == 0)
        {
            FNDebug.LogError($"MissionTaskStartFight  TaskGoal golaParam is Null    TaskGoalId : {taskgoalTab.id}");
            return;
        }

        var tgType = taskgoalTab.type;
        TaskGoalType goalType = (TaskGoalType) taskgoalTab.type;
        if (goalType == TaskGoalType.FightMonster || goalType == TaskGoalType.FightMoreMonsterOfOne)
        {
            CSAutoFightManager.Instance.BeginFight(taskgoalTab.goalParam[0]);
        }
        else if (goalType == TaskGoalType.MapKillMonster)
        {
            CSAutoFightManager.Instance.BeginFight(0);
        }
        else if (goalType == TaskGoalType.MapKillQualityMonster)
        {
            if (taskgoalTab.goalParam.Count > 2)
                CSAutoFightManager.Instance.BeginFightByQuality(taskgoalTab.goalParam[1]);
        }
        else if(goalType == TaskGoalType.CompleteInstance || tgType == 60 || tgType == 62 || tgType == 3 || tgType == 64)
        {
            //随便搞，想打谁打谁
            CSAutoFightManager.Instance.BeginFight(0,false);
        }
    }
}

/// <summary>
/// 任务--打开面板
/// </summary>
public class MissionTaskOpenPanel : MissionOpenPanel
{
    public override EventState StartMission(CSMissionBase missionBase)
    {
        TaskGoalType goalType = (TaskGoalType) missionBase.TaskGoalTab.type;
        if (goalType == TaskGoalType.GetVigorValue)
            OpenPanel(10600);

        return EventState.None;
    }
}

public class MissionTaskSpecial : MissionSpecial
{
    protected override void Execute(CSMissionBase mission)
    {
        TaskGoalType goalType = (TaskGoalType) mission.TaskGoalTab.type;
        if (goalType == TaskGoalType.CompleteInstance)
        {
            if (mission.TaskGoalTab.goalParam.Count > 0)
                Net.ReqEnterInstanceMessage(mission.TaskGoalTab.goalParam[0]);
        }
		else if (goalType == TaskGoalType.GetEquipWay)
		{
			HotManager.Instance.EventHandler.SendEvent(CEvent.ShowMissionUnAccept, mission.TaskId);
		}
	}
}


public class MissionTaskPickItem : MissionPickItem
{
    protected override EventState NextEventState()
    {
        return EventState.FindNpc;
    }
}