using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildAdventureRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.WildAdventureInfoChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.WildAdventureBossInfoChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.WildAdventure, CSWildAdventureInfo.Instance.CanShowRedPoint());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.WildAdventureInfoChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.WildAdventureBossInfoChange, OnCheckRedPoint);
    }
}
