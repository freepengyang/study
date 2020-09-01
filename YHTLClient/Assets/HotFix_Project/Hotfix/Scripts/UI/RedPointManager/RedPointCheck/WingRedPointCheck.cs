public class WingRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.GetWingInfo, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.WingStarUp, OnCheckRedPoint);
        // mClientEvent.AddEvent(CEvent.WingAdvance, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.WingExpItemUse, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.Wing,CSWingInfo.Instance.IsUpStar());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.GetWingInfo, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.WingStarUp, OnCheckRedPoint);
        // mClientEvent.RemoveEvent(CEvent.WingAdvance, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.WingExpItemUse, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnCheckRedPoint);
    }
}