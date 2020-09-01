using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EquipRecastRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.WearEquip, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.UnWeatEquip, OnCheckRedPoint);

        mClientEvent.AddEvent(CEvent.EquipRebuildNtfMessage, OnCheckRedPoint);
        
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.EquipRecast, CSBagInfo.Instance.GetEquipRecastRedpointState());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.WearEquip, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.UnWeatEquip, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.EquipRebuildNtfMessage, OnCheckRedPoint);
    }
}
