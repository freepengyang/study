public class DreamRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.OpenDreamLand, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.ChangeDreamLandTime, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.MainPlayer_LevelChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.DreamLand,CSDreamLandInfo.Instance.IsEnterDreamLand());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OpenDreamLand, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.ChangeDreamLandTime, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.MainPlayer_LevelChange, OnCheckRedPoint);
    }
}