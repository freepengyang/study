public class HandBookSetupedUpgradeLevelRedPointCheck : RedPointCheckBase
{
    public override int funcopenId
    {
        get { return 17; }
    }

    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnHandBookAdded, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnHandBookRemoved, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnHandBookUpgradeSucceed, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnHandBookInlayChanged, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        HandBookSlotData handBookSlotData = null;
        RefreshRed(RedPointType.HandBookSetupedUpgradeLevel, CSHandBookManager.Instance.CheckHandBookSetupedUpgradeLevelRedPoint(out handBookSlotData));
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnHandBookAdded, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnHandBookRemoved, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnHandBookUpgradeSucceed, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnHandBookInlayChanged, OnCheckRedPoint);
    }
}