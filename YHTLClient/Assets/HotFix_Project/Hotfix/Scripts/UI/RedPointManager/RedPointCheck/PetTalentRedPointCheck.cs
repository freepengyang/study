using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetTalentRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.PetTalentLvChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.PetTalentCheckRedpoint, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.PetTalent, CSPetTalentInfo.Instance.CheckRedPoint());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.PetTalentLvChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.PetTalentCheckRedpoint, OnCheckRedPoint);
    }
}
