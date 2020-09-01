using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bag;
public class RechargeShopRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.DailyRmbChange,OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }


    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.RechargeShop, CSShopInfo.Instance.RechargeShopRed());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.DailyRmbChange,OnCheckRedPoint);
    }
    
}
