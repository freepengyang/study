﻿public class FashionRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.AllFashionInfo, OnCheckRedPoint);
        // mClientEvent.AddEvent(CEvent.EquipFashion, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.FashionStarLevelUp, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.AddFashion, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.RemoveFashion, OnCheckRedPoint);
        // mClientEvent.AddEvent(CEvent.UnEquipFashion, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.Fashion,CSFashionInfo.Instance.HasActiveAndUpStar());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.AllFashionInfo, OnCheckRedPoint);
        // mClientEvent.RemoveEvent(CEvent.EquipFashion, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.FashionStarLevelUp, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.AddFashion, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.RemoveFashion, OnCheckRedPoint);
        // mClientEvent.RemoveEvent(CEvent.UnEquipFashion, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnCheckRedPoint);
    }
}