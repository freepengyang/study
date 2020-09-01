using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bag;
public class NostalgiaRedPointCheck : RedPointCheckBase
{
    public override int funcopenId => 63;
    
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.NostalgiaEquipChange,OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.NostalGeziChange,OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.NostalgiaBagChange,OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }


    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.Nostalgia, CSNostalgiaEquipInfo.Instance.BagRedPointCheck());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.NostalgiaEquipChange,OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.NostalGeziChange,OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.NostalgiaBagChange,OnCheckRedPoint);
    }
    
}
