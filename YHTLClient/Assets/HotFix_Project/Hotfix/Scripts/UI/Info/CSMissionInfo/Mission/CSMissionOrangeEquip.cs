using System.Collections.Generic;
using Google.Protobuf.Collections;
public class CSMissionOrangeEquip : CSMissionBase
{
    MonsterSlayData mData;

    public CSMissionOrangeEquip()
    {
       
    }

    public override void Init(int id)
    {
		base.Init(id);

		if (!TasksTableManager.Instance.TryGetValue(id, out TABLE.TASKS item))
			return;
		TasksTab = item;

		if(!TaskGoalTableManager.Instance.TryGetValue(item.goals, out TABLE.TASKGOAL goal))
			return;
		TaskGoalTab = goal;

		TaskState = TaskState.Accepted;

        //注册你本活动相关消息
        mClientEvent.AddEvent(CEvent.MonsterSlayInfoChange, OnActivityChange);

        OnActivityChange(0, null);
    }

	void OnActivityChange(uint id,object argv)
	{
        mData = CSSpecialActivityMonsterSlayInfo.Instance.AnyCanComplete();

        if (mData == null)
        {
            //活动关闭
            //CSMissionManager.Instance.RemoveMission(TaskId);
            mClientEvent.SendEvent(CEvent.ActivityLinkChanged, TaskId);
            mClientEvent.SendEvent(CEvent.Task_GoalUpdate, TaskId);
            return;
        }

        if (mData.StateInt == 0)
        {
            TaskState = TaskState.Completed;
        }
        else if (mData.StateInt == 1)
        {
            TaskState = TaskState.Accepted;
        }
        CSMissionManager.SetMissionSortId(CSMissionManager.Instance.GetMissionByTaskId(TaskId));
        mClientEvent.SendEvent(CEvent.Task_GoalUpdate, TaskId);
    }

	public override string ShowContent()
	{
		string content = "";
        if (mData != null && mData.StateInt == 1)
        {
            content = $"{TaskGoalTab.goalTips} [eee5c3]({mData.curNum}/{mData.goalNum})";
        }
		else
            content = TasksTableManager.Instance.GetTasksTip1(TaskId);
        return content;
	}

	public override void OnClick(bool playerClick = false)
	{
        //base.OnClick(playerClick);
        if (mData == null || mData.StateInt != 1)
        {
            if (playerClick)
                UIManager.Instance.ClosePanel<UIMissionGuildPanel>();
            UtilityPanel.JumpToPanel(23107);
        }
        else base.OnClick(playerClick);
    }


    public override void Dispose()
    {
        mData = null;
        base.Dispose();
    }
}