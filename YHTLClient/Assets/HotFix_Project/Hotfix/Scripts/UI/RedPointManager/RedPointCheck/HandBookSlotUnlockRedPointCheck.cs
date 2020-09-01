public class HandBookSlotUnlockRedPointCheck : RedPointCheckBase
{
    public override int funcopenId
    {
        get { return 17; }
    }

    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnHandBookSlotChanged, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        HandBookSlotData handBookSlotData = null;
        RefreshRed(RedPointType.HandBookSlotUnlock, CSHandBookManager.Instance.CheckExistNeedUnlockSlot(out handBookSlotData));
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnHandBookSlotChanged, OnCheckRedPoint);
    }
}