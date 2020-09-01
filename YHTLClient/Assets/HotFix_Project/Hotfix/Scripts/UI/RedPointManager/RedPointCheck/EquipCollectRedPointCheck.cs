public class EquipCollectRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.SCCollectActivityData, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.ResSpecialActivityDataMessage, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.EquipCollect,CSOpenServerACInfo.Instance.HasCollectEquip());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.SCCollectActivityData, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.ResSpecialActivityDataMessage, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnCheckRedPoint);
    }
}