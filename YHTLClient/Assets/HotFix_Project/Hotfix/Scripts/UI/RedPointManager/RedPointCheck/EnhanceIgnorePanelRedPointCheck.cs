using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhanceIgnorePanelRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.EnhanceIgnorePanel, CSEnhanceInfo.Instance.CanEnhance());
    }

    public override void OnDestroy()
    {
        mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
    }
}
