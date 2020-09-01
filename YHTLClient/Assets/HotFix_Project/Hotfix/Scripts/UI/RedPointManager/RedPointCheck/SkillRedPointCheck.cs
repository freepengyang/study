public class SkillRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.SkillUpgradeSucceed, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnSkillAdded, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnSkillRemoved, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.SkillUpgrade, CSSkillInfo.Instance.CheckHasCanUpgradeSkill());
        
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.SkillUpgradeSucceed, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnSkillAdded, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnSkillRemoved, OnCheckRedPoint);
    }
}