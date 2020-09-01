

public partial class CSNetTask : CSNetBase
{
    private void ECM_ResTaskListMessage(NetInfo obj)
    {
        task.TaskList rsp = Network.Deserialize<task.TaskList>(obj);
        if (rsp != null)
        {
            CSMissionManager.Instance.InitialMission(rsp);
        }
		//CSMissionManager.Instance.CheckResidentTask();
	}

    private void ECM_ResTaskStateChangedMessage(NetInfo obj)
    {
        task.TaskInfo rsp = Network.Deserialize<task.TaskInfo>(obj);
        CSMissionManager.Instance.UpdateMission(rsp);
    }
    
    private void ECM_ResTaskGoalUpdatedMessage(NetInfo obj)
    {
        task.TaskGoalUpdateResponse rsp = Network.Deserialize<task.TaskGoalUpdateResponse>(obj);
        CSMissionManager.Instance.UpdateMission(rsp);
    }
    
    private void ECM_ResAcceptTaskMessage(NetInfo obj)
    {
        task.TaskInfo rsp = Network.Deserialize<task.TaskInfo>(obj);
        CSMissionManager.Instance.AddMission(rsp);
		CSMissionManager.Instance.CheckResidentTask();
        HotManager.Instance.EventHandler.SendEvent(CEvent.Task_GoalUpdate,rsp.taskId);
        HotManager.Instance.EventHandler.SendEvent(CEvent.Task_AcceptEffect);
        CSMissionManager.Instance.TriggerAutoAcceptTask(rsp);
	}
    
    private void ECM_ResSubmitTaskMessage(NetInfo obj)
    {
        task.SubmitTaskResponse rsp = Network.Deserialize<task.SubmitTaskResponse>(obj);
        if(!CSScene.IsLanuchMainPlayer) return;
        CSMissionManager.Instance.UpdateMissionState(rsp.taskId);
        HotManager.Instance.EventHandler.SendEvent(CEvent.Task_FinshGuide, rsp.taskId);
        CSMissionManager.Instance.RemoveMission(rsp.taskId);
        CSMissionManager.Instance.AddMissions(rsp.newTasks);
		CSMissionManager.Instance.CheckResidentTask();
        HotManager.Instance.EventHandler.SendEvent(CEvent.Task_FinshEffect);
        
        HotManager.Instance.EventHandler.SendEvent(CEvent.Task_GoalUpdate, rsp.taskId);
        for(int i = 0,max = rsp.newTasks.Count;i < max;++i)
        {
            HotManager.Instance.EventHandler.SendEvent(CEvent.Task_GoalUpdate, rsp.newTasks[i].taskId);
        }
        
        CSMissionManager.Instance.SubmitTaskMessage(rsp);		
	}

    private void ECM_SCCycTaskMessage(NetInfo obj)
    {

    }
	void ECM_ResFlyToGoalMessage(NetInfo info)
    {
    }
	void ECM_SCNewTaskMessage(NetInfo info)
	{
		task.NewTaskResponse msg = Network.Deserialize<task.NewTaskResponse>(info);
		if(null == msg)
		{
			FNDebug.LogError("Deserialize Msg Failed Fortask.NewTaskResponse");
			return;
		}

        CSMissionManager.Instance.AddMissions(msg.newTasks);
        for (int i = 0, max = msg.newTasks.Count; i < max; ++i)
        {
            HotManager.Instance.EventHandler.SendEvent(CEvent.Task_GoalUpdate, msg.newTasks[i].taskId);
        }
    }
}