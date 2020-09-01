using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DailyArenaRedCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.SCAthleticsActivityInfoMessage, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.SCReceiveAthleticsActivityRewardMessage, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.FunctionOpenStateChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        //Debug.Log("每日竞技红点  " + CSDailyArenaInfo.Instance.DailyAranaRedState());
        RefreshRed(RedPointType.DailyArena, CSDailyArenaInfo.Instance.DailyAranaRedState());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.SCAthleticsActivityInfoMessage, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.SCReceiveAthleticsActivityRewardMessage, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.FunctionOpenStateChange, OnCheckRedPoint);
    }
}
