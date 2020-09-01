using System;
using System.Collections.Generic;
using task;

public class MainMissionFindNpc : MissionFindNpc
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

            int sceneId = NpcTableManager.Instance.GetNpcSceneId(npcId);
            if (!_isReach && _mission.TasksTab.transfer == 1 &&
                CSScene.GetMapID() != sceneId)
            {
                HotManager.Instance.EventHandler.SendEvent(CEvent.OpenMissionGuidePanel, _mission.TaskId);
            }
        }
    }

    protected override void OnReachNpc(uint id, object data)
    {
        base.OnReachNpc(id, data);
        UIManager.Instance.ClosePanel<UIMissionGuildPanel>();
    }
}


public class MainMissionFindPath : MissionFindPath
{
    protected override EventState NextEventState()
    {
        var tgType = _mission.TaskGoalTab.type;
        TaskGoalType goalType = (TaskGoalType)tgType;
        if (goalType == TaskGoalType.FightMonster || goalType == TaskGoalType.MapKillMonster ||
            goalType == TaskGoalType.MapKillQualityMonster ||
            goalType == TaskGoalType.FightMoreMonsterOfOne || goalType == TaskGoalType.CompleteInstance)
        {
            return EventState.StartFight;
        }

        if (tgType == 60 || tgType == 62 || tgType == 3)
            return EventState.StartFight;

        return EventState.None;
    }

    protected override void FingPath()
    {
        if (_mission.PlayerOnClck)
        {
            if (CSScene.GetMapID() == _mission.TaskGoalTab.mapId && Utility.IsNearPlayerInMap(point.x, point.y, 10))
            {
                base.FingPath();
            }
            else
            {
                _mClientEvent.AddEvent(CEvent.Reach_Npc_Position, OnDeliverPoint);
                CSMissionManager.Instance.FlyToGoalRequest(_mission);
            }
        }
        else
        {
            base.FingPath();

            if (!_isReach && _mission.TasksTab.transfer == 1 &&
                CSScene.GetMapID() != _mission.TaskGoalTab.mapId)
            {
                HotManager.Instance.EventHandler.SendEvent(CEvent.OpenMissionGuidePanel, _mission.TaskId);
            }
        }
    }

    protected override bool SetFindPoint(out CSMisc.Dot2 dot)
    {
        dot = point;
        return true;
    }

    protected override void OnReachNpc(uint id, object data)
    {
        base.OnReachNpc(id, data);
        UIManager.Instance.ClosePanel<UIMissionGuildPanel>();
    }
    
    private void OnDeliverPoint(uint id, object data)
    {
        _mClientEvent.UnReg(CEvent.Reach_Npc_Position, OnDeliverPoint);
        OnReachNpc(0, null);
    }
}