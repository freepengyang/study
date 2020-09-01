using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LongJiRefineRedCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.WearEquip, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.UnWeatEquip, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.LongJiRefine, CSBagInfo.Instance.GetLongJiRefineRedState());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.WearEquip, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.UnWeatEquip, OnCheckRedPoint);
    }
}
