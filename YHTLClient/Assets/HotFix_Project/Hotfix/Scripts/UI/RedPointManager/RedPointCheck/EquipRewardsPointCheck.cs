public class EquipRewardsPointCheck : RedPointCheckBase
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
		RefreshRed(RedPointType.EquipRewards, CSEquipRewardsInfo.Instance.WoLongRedPoint());
	}

    public override void OnDestroy()
    {
		if(mClientEvent != null)
			mClientEvent.RemoveEvent(CEvent.GetEquipRewardsInfo, OnCheckRedPoint);
	}
}