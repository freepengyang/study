using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WearableWoLongEquipRedCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.WearEquip, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.UnWeatEquip, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.WoLongLevelUpgrade, OnCheckRedPoint);
        //mClientEvent.AddEvent(CEvent.GetItemPanelOpen, OnCheckRedPoint);
        //mClientEvent.AddEvent(CEvent.GetItemPanelClose, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        //Debug.Log("  卧龙可穿戴  " + CSBagInfo.Instance.HaveWearableWoLongEquip());
        RefreshRed(RedPointType.WearableWoLongEquip, CSItemCountManager.Instance.GetHaveBestEquipByPos());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.WearEquip, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.UnWeatEquip, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.WoLongLevelUpgrade, OnCheckRedPoint);
        //mClientEvent.RemoveEvent(CEvent.GetItemPanelOpen, OnCheckRedPoint);
        //mClientEvent.RemoveEvent(CEvent.GetItemPanelClose, OnCheckRedPoint);
    }
}
