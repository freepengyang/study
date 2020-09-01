public class MaFaRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
		mClientEvent.AddEvent(CEvent.RefreshMaFaLayerInfo, OnCheckRedPoint);
		mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
	}

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
		RefreshRed(RedPointType.MaFaBox, CSMaFaInfo.Instance.GetBoxRedPoint());
		RefreshRed(RedPointType.MaFaRewards, CSMaFaInfo.Instance.GetRewardsRedPoint());
	}
    public override void OnDestroy()
    {
		mClientEvent.RemoveEvent(CEvent.RefreshMaFaLayerInfo, OnCheckRedPoint);
		mClientEvent.RemoveEvent(CEvent.MoneyChange, OnCheckRedPoint);
	}
}