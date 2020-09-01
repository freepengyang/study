using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VipPanelRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.VipInfoChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
       RefreshRed(RedPointType.Vip,CSVipInfo.Instance.VipInfoRedPointChenk());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.VipInfoChange, OnCheckRedPoint);
    }
}
