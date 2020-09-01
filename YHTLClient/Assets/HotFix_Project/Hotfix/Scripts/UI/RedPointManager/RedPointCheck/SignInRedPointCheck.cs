using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInRedPointCheck : RedPointCheckBase
{
    public override int funcopenId => (int)FunctionType.funcP_signIn;
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.FunctionOpenStateChange, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.CardPoolUpdate, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.SignIn, CSSignCardInfo.Instance.CanSignIn());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.FunctionOpenStateChange, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.CardPoolUpdate, OnCheckRedPoint);
    }
}
