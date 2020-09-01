using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyBtnRedPointCheck : RedPointCheckBase
{
    //日常活跃度
    
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.DailyBtnRedPointCheck, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.DailyBtn, CSActivityInfo.Instance.CheckActiveGet());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.DailyBtnRedPointCheck, OnCheckRedPoint);
    }
}
