public class PetSkillRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.PetSkillUpgradeSucceed, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnPetSkillAdded, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnPetSkillRemoved, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnPetJobChanged, OnCheckRedPoint);
        //mClientEvent.AddEvent(CEvent.OnPetLevelChanged, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.PetTalentLvChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.GetWarPetBaseInfo, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.PetSkillUpgrade, CSSkillInfo.Instance.CheckExistCanUpgradePetSkill());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.PetSkillUpgradeSucceed, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnPetSkillAdded, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnPetSkillRemoved, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnPetJobChanged, OnCheckRedPoint);
        //mClientEvent.RemoveEvent(CEvent.OnPetLevelChanged, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.PetTalentLvChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.GetWarPetBaseInfo, OnCheckRedPoint);
    }
}