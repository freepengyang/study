public class DayChargeRedPointCheck : RedPointCheckBase
{
	public override void Init()
    {
		mClientEvent.AddEvent(CEvent.GetEveryTimeDayChargeInfo, OnCheckRedPoint);
		mClientEvent.AddEvent(CEvent.GetDayChargeInfo, OnCheckRedPoint);
	}

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
		RefreshRed(RedPointType.DayCharge, CSDayChargeInfo.Instance.HasNotify);
	}

	public override void OnDestroy()
    {
		mClientEvent.RemoveEvent(CEvent.GetEveryTimeDayChargeInfo, OnCheckRedPoint);
		mClientEvent.RemoveEvent(CEvent.GetDayChargeInfo, OnCheckRedPoint);
	}
}