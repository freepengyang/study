using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingColorRedPointCheck :RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.GetWingInfo, OnCheckRedPoint);
        // mClientEvent.AddEvent(CEvent.WingStarUp, OnCheckRedPoint);
        // mClientEvent.AddEvent(CEvent.WingAdvance, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.WingColorChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.WingColor,CSWingInfo.Instance.HasActive());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.GetWingInfo, OnCheckRedPoint);
        // mClientEvent.RemoveEvent(CEvent.WingStarUp, OnCheckRedPoint);
        // mClientEvent.RemoveEvent(CEvent.WingAdvance, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.WingColorChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnCheckRedPoint);
    }
}
