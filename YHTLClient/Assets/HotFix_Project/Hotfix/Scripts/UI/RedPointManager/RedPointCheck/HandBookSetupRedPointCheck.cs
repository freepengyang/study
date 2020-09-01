public class HandBookSetupRedPointCheck : RedPointCheckBase
{
    public override int funcopenId
    {
        get { return 17; }
    }

    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.OnHandBookAdded, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnHandBookRemoved, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnHandBookInlayChanged, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnHandBookSlotChanged, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        HandBookSlotData slot, card;
        RefreshRed(RedPointType.HandBookSetuped, CSHandBookManager.Instance.CheckHandBookSetupRedPoint(out slot,out card));
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnHandBookAdded, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnHandBookRemoved, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnHandBookInlayChanged, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnHandBookSlotChanged, OnCheckRedPoint);
    }
}