using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimeFundRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.LifeTimeFundRewardChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.LifeTimeFundChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.LifeTimeFundTabRed, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.LifeTimeFund, CSlifeTimeFundInfo.Instance.RedCheck());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.LifeTimeFundRewardChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.LifeTimeFundChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.LifeTimeFundTabRed, OnCheckRedPoint);
    }
}
