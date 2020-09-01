using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ArmRaceRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.ResEquipCompetitionMessage, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        //Debug.Log(CSArmRaceInfo.Instance.GetArmRaceRedPoint());
        RefreshRed(RedPointType.ArmRace, CSArmRaceInfo.Instance.GetArmRaceRedPoint());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ResEquipCompetitionMessage, OnCheckRedPoint);
    }
}
