public class DayChargeMapRedPointCheck : RedPointCheckBase
{
	public override void Init()
	{
		mClientEvent.AddEvent(CEvent.GetEveryTimeDayChargeInfo, OnCheckMapRedPoint);
		mClientEvent.AddEvent(CEvent.GetDayChargeMapFirst, OnCheckMapRedPoint);
		mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckMapRedPoint);
	}

	public override void LoginOrFuncRedCheck()
	{
		OnCheckMapRedPoint(0, null);
	}

	protected void OnCheckMapRedPoint(uint id, object argv)
	{
		RefreshRed(RedPointType.DayChargeMap, CSDayChargeInfo.Instance.IsShowMapRedPoint());
	}

	public override void OnDestroy()
    {
		if(mClientEvent != null)
		{
			mClientEvent.RemoveEvent(CEvent.GetEveryTimeDayChargeInfo, OnCheckMapRedPoint);
			mClientEvent.RemoveEvent(CEvent.GetDayChargeMapFirst, OnCheckMapRedPoint);
			mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckMapRedPoint);
		}
	}
}