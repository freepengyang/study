

public class WolongUpgradeRedPoint : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.WoLongLevelUpgrade, OnCheckRedPoint);

    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        //Debug.Log("  卧龙修为红点  " + CSWoLongInfo.Instance.GetWolongUpGradeRedPointState());
        RefreshRed(RedPointType.WolongUpGrade, CSWoLongInfo.Instance.GetWolongUpGradeRedPointState());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.WoLongLevelUpgrade, OnCheckRedPoint);
    }
}
