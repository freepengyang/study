public class SevenLoginRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.OnSevenDayLoginChanged, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.SevenLogin,CSSevenLoginInfo.Instance.HasRewardNeedFetch());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnSevenDayLoginChanged, OnCheckRedPoint);
    }
}