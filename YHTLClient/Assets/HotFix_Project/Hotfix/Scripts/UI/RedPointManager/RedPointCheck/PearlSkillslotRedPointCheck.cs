public class PearlSkillslotRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.ReplaceBaoZhuSkills, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.GradeUpBaoZhu, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.PearlSkillslot,CSPearlInfo.Instance.HasNonSkillSlot());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ReplaceBaoZhuSkills, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.GradeUpBaoZhu, OnCheckRedPoint);
    }
}