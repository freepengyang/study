public class WarPetRefineRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.PetTianFuPassiveSkillMessage, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.PetTianFuRandomPassiveSkill, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.PetTianFuChosePassiveSkill, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.PetRefine,CSWarPetRefineInfo.Instance.IsHasEnoughCost());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.PetTianFuPassiveSkillMessage, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.PetTianFuRandomPassiveSkill, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.PetTianFuChosePassiveSkill, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnCheckRedPoint);
    }
}