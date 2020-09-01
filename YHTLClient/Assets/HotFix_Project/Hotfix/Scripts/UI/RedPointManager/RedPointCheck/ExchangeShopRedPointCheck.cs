using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeShopRedPointCheck : RedPointCheckBase
{
    public override int funcopenId => (int)FunctionType.funcP_BiQiShop;
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.FunctionOpenStateChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.ExchangeShopRedPointCheck, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.ExchangeShop, CSExchangeShopInfo.Instance.CheckRedPoint());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.FunctionOpenStateChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.ExchangeShopRedPointCheck, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnCheckRedPoint);
    }
}
