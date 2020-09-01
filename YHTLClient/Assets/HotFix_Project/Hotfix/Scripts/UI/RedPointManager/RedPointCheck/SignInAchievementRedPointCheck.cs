using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInAchievementRedPointCheck : RedPointCheckBase
{
    public override int funcopenId => (int)FunctionType.funcP_signIn;
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.HonorChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.UltHonorReceive, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.SignInAchivement, CSSignCardInfo.Instance.AnyAchievementCanAccept());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.HonorChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.UltHonorReceive, OnCheckRedPoint);
    }
}
