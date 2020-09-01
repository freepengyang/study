using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bag;
public class GemLevelUpRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.ItemListChange,OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }


    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.GemlevelUp, CSGemInfo.Instance.GemLevelUpCheck());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ItemListChange,OnCheckRedPoint);
    }
    
}
