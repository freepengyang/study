public class PetLevelUpRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.OnRecycleItemChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.PetLevelUp, CSPetLevelUpInfo.Instance.PetLevelUpRedPoint());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnRecycleItemChange, OnCheckRedPoint);
    }
}