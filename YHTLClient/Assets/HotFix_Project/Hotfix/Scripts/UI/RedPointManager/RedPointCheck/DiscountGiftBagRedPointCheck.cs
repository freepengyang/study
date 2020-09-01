public class DiscountGiftBagRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.DailyPurchaseInfo, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.DailyPurchaseBuyDiscount, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.GiftBagOpen, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.GiftBagClose, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.LookGift, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.LookPosition, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.DiscountGiftBag,CSDiscountGiftBagInfo.Instance.IsHasRedPoint());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.DailyPurchaseInfo, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.DailyPurchaseBuyDiscount, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.GiftBagOpen, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.GiftBagClose, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.LookGift, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.LookPosition, OnCheckRedPoint);
    }
}