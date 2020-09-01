public class GuildPracticeRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.OnMainPlayerGuildIdChanged, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnMainPlayerGuildLvChanged, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnGuildPracticeImproved, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.GuildPractice,CSGuildInfo.Instance.CheckCanUpgradePractice());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnMainPlayerGuildIdChanged, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnMainPlayerGuildLvChanged, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnGuildPracticeImproved, OnCheckRedPoint);
    }
}