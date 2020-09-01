public class TombTreasureRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
		mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.TombTreasure, CSStonetreasureInfo.Instance.HasNotify);
    }

    public override void OnDestroy()
    {
		mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
    }
}