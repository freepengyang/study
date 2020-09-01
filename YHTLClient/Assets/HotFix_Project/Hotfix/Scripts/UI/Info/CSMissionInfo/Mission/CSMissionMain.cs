using System;
using System.Collections.Generic;

public class CSMissionMain : CSMissionBase
{
    public CSMissionMain()
    {
        _executeEventDic = new Dictionary<EventState, Type>(8);
        _executeEventDic.Add(EventState.FindPath, typeof(MainMissionFindPath));
        _executeEventDic.Add(EventState.FindNpc, typeof(MainMissionFindNpc));
    }


    public override void OnReachNpc(int NpcId)
    {
        base.OnReachNpc(NpcId);
        UIManager.Instance.CreatePanel<UINPCDialogPanel>((f) =>
        {
            CSNPCDialogData data = new CSNPCDialogData(this);
            data.NpcId = NpcId;
            data.Time = 8;
            UINPCDialogPanel panel = f as UINPCDialogPanel;
            panel?.Show(data);
        });
    }
}