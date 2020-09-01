using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SevenDayRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.SevenDayDataChange,OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.SevenDay, CSActivityInfo.Instance.SevenDayRedPointChenk());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.SevenDayDataChange,OnCheckRedPoint);
    }
}
