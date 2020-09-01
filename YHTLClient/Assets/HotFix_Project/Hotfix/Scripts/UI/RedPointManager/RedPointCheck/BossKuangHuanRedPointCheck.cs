public class BossKuangHuanRedPointCheck : RedPointCheckBase
{
	public override void Init()
    {
		mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
		mClientEvent.AddEvent(CEvent.GetBossKuangHuanUpdateInfo, OnCheckRedPoint);
	}
    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
	}
	
	protected void OnCheckRedPoint(uint id, object argv)
    {
		RefreshRed(RedPointType.BossKuangHuan, CSBossKuangHuanInfo.Instance.IsShowRedPoint());
	}
	public override void OnDestroy()
    {
		mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
		mClientEvent.RemoveEvent(CEvent.GetBossKuangHuanUpdateInfo, OnCheckRedPoint);
	}
}