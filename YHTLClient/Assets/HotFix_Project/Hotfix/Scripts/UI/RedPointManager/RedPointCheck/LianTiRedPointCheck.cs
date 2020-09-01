public class LianTiRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
		mClientEvent.AddEvent(CEvent.GetUpgrade, OnCheckRedPoint);
		mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
		mClientEvent.AddEvent(CEvent.GetLianTiInfo, OnCheckRedPoint);
	}

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
		if(id == (uint)CEvent.MoneyChange &&  argv != null)
		{
			if(argv.GetType() == typeof(MoneyType) && (int)argv == (int)MoneyType.zhenqi)
			{
				RefreshRed(RedPointType.LianTi, CSLianTiInfo.Instance.HasNotify);
			}
		}
		else
		{
			RefreshRed(RedPointType.LianTi, CSLianTiInfo.Instance.HasNotify);
		}
    }

    public override void OnDestroy()
    {
		mClientEvent.RemoveEvent(CEvent.GetUpgrade, OnCheckRedPoint);
		mClientEvent.RemoveEvent(CEvent.MoneyChange, OnCheckRedPoint);
		mClientEvent.RemoveEvent(CEvent.GetLianTiInfo, OnCheckRedPoint);
	}
}