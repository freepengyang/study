public class WingSpiritRedPointCheck : RedPointCheckBase
{
    public override int funcopenId => 61;
    
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.YuLingInfo, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.WingSpirit,CSWingInfo.Instance.IsActiveRedPointYuLing());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.YuLingInfo, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
    }
}