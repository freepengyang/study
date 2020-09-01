public class PerEquipRewardsPointCheck : RedPointCheckBase
{
    public override void Init()
    {
		mClientEvent.AddEvent(CEvent.GetEquipRewardsInfo, OnCheckRedPoint);
	}

	public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
		RefreshRed(RedPointType.PerEquipRewards, CSEquipRewardsInfo.Instance.PerRedPoint());
	}

    public override void OnDestroy()
    {
		if(mClientEvent != null)
			mClientEvent.RemoveEvent(CEvent.GetEquipRewardsInfo, OnCheckRedPoint);
	}
}