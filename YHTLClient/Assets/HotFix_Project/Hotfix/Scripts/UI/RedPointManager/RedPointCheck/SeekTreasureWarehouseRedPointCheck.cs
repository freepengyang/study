public class SeekTreasureWarehouseRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.SeekTreasureItemChanged, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.SeekTreasureStorehouse, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.SeekTreasureHuntCallBack, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.SeekTreasureWarehouse,CSSeekTreasureInfo.Instance.HasExtractEquip());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.SeekTreasureItemChanged, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.SeekTreasureStorehouse, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.SeekTreasureHuntCallBack, OnCheckRedPoint);
    }
}