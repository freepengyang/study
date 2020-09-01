public class PearlEvolutionRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.GradeUpBaoZhu, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.BaoZhuBossCountChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.PearlEvolution,CSPearlInfo.Instance.HasEvolution());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.GradeUpBaoZhu, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.BaoZhuBossCountChange, OnCheckRedPoint);
    }
}