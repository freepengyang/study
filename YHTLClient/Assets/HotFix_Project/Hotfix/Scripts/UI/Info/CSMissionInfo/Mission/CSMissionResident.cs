using System.Collections.Generic;
using Google.Protobuf.Collections;
public class CSMissionResident : CSMissionBase
{
    public CSMissionResident()
    {
       
    }
	public override string ShowContent()
	{
		string content = "";
		content = TasksTableManager.Instance.GetTasksTip1(TaskId);
		return content;
	}
	public override void OnClick(bool playerClick = false)
	{
		int type = TasksTableManager.Instance.GetTasksType(TaskId);
		int taskGoalId = TasksTableManager.Instance.GetTasksGoals(TaskId);
		var goalParam = TaskGoalTableManager.Instance.GetTaskGoalGoalParam(taskGoalId);
		int gameModel = goalParam[0];
		if (type == (int)TaskType.TodayCanDo)
			UtilityPanel.JumpToPanel(gameModel);
		else if (type == (int)TaskType.GetEquip)
		{
			int playerLv = CSMainPlayerInfo.Instance.Level;
			int deliverId = 0;
			int minLv, maxLv;
			string tempStr = SundryTableManager.Instance.GetSundryEffect(705);
			List<List<int>> strList = UtilityMainMath.SplitStringToIntLists(tempStr);
			if (strList.Count > 0)
			{
				if (playerLv < strList[0][0])
					deliverId = 0;
				else if (playerLv >= strList[strList.Count - 1][1])
					deliverId = strList[strList.Count - 1][2];
				else
				{
					for (int i = 0; i < strList.Count; i++)
					{
						List<int> numList = strList[i];
						minLv = numList[0];
						maxLv = numList[1];
						if (minLv < playerLv && playerLv <= maxLv)
						{
							deliverId = numList[2];
							break;
						}
					}
				}
			}
			if (deliverId > 0)
				UtilityPath.FindWithDeliverId(deliverId);
		}
		else if (type == (int)TaskType.WantStronger)
		{
			UtilityPanel.JumpToPanel(gameModel);
		}
		else if (type == (int)TaskType.WantIngot)
		{
			UtilityPanel.JumpToPanel(gameModel);
		}
		PlayerOnClck = false;
	}
}