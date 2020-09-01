public class AddUpRechargeRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.AddUpInfoChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.AddUpRecharge, CSVipInfo.Instance.AddUpReChargeRedCheck());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.AddUpInfoChange, OnCheckRedPoint);
    }
}