public class TimeExpRedPointCheck : RedPointCheckBase
{
    public override int funcopenId
    {
        get { return 5; }
    }

    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.TimeExpChanged, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.TimeExpUprgaded, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.TimeExp, CSTimeExpManager.Instance.HasNotify);
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.TimeExpChanged, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.TimeExpUprgaded, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnCheckRedPoint);
    }
}