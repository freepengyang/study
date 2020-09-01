using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonthCardRedPointCheck : RedPointCheckBase
{

    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.MonthCardInfoChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.MonthCard, CSMonthCardInfo.Instance.MonthCardRedPoint());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.MonthCardInfoChange, OnCheckRedPoint);
    }
}
