using System;
using task;

public class CSMissionBranchLine : CSMissionBase
{
    public CSMissionBranchLine()
    {
    }


    public override void OnReachNpc(int NpcId)
    {
        base.OnReachNpc(NpcId);

        UIManager.Instance.CreatePanel<UINPCDialogPanel>((f) =>
        {
            CSNPCDialogData data = new CSNPCDialogData(this);
            data.NpcId = NpcId;
            UINPCDialogPanel panel = f as UINPCDialogPanel;
            panel?.Show(data);
        });
    }
}