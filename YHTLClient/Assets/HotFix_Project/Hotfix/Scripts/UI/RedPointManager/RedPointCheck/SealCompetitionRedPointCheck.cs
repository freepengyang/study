using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SealCompetitionRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.ResSpecialActivityDataMessage, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        //Debug.Log(CSOpenServerACInfo.Instance.SealCompetitionRedPointChenk());
        RefreshRed(RedPointType.SealComPetition, CSOpenServerACInfo.Instance.SealCompetitionRedPointChenk());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ResSpecialActivityDataMessage, OnCheckRedPoint);
    }
}
