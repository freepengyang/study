using System;
using task;

public class CSMissionDaily : CSMissionBase
{
    public CSMissionDaily()
    {
        //_executeEventDic = new DictionaryHot<EventState, Type>();
        //_executeEventDic.Add(EventState.FindPath, typeof(DailyMissionFindPath));
    }


    public override void OnReachNpc(int NpcId)
    {
        base.OnReachNpc(NpcId);

        if (TaskState == TaskState.Cost)
        {
            UIManager.Instance.CreatePanel<UIDailyNPCCostPanel>((f) =>
            {
                UIDailyNPCCostPanel panel = f as UIDailyNPCCostPanel;
                panel?.Show(TasksTab);
            });
            return;
        }
        
        UIManager.Instance.CreatePanel<UINPCDialogPanel>((f) =>
        {
            CSNPCDialogData data = new CSNPCDialogData(this);
            data.NpcId = NpcId;
            UINPCDialogPanel panel = f as UINPCDialogPanel;
            panel?.Show(data);
        });
    }
}