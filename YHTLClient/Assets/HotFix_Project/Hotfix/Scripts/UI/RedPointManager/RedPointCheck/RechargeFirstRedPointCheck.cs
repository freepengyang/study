using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RechargeFirstRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.FirstRechargeInfoChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.FirstRechargeRedChange,OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        //Debug.Log(CSOpenServerACInfo.Instance.SealCompetitionRedPointChenk());
        RefreshRed(RedPointType.RechargeFirst, CSVipInfo.Instance.RechargeFirstRedCheck());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.FirstRechargeInfoChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.FirstRechargeRedChange, OnCheckRedPoint);
    }
}
