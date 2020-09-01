public class DirectPurchaseReceiveRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.DailyPurchaseInfo, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.DailyPurchaseBuy, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.DailyPurchaseReceive, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.GiftBagOpen, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.GiftBagClose, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.DirectPurchaseReceive, CSDirectPurchaseInfo.Instance.IsHasEnableReceive());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.DailyPurchaseInfo, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.DailyPurchaseBuy, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.DailyPurchaseReceive, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.GiftBagOpen, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.GiftBagClose, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnCheckRedPoint);
    }
}