using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonthRechargeRedPointCheck : RedPointCheckBase
{

    public override int funcopenId => (int)FunctionType.funcP_BiQiShop;

    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.FunctionOpenStateChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.MonthRechargeRedPointCheck, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.MonthRecharge, CSRechargeInfo.Instance.MonthRechargeRedPoint());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.FunctionOpenStateChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.MonthRechargeRedPointCheck, OnCheckRedPoint);
    }
}
