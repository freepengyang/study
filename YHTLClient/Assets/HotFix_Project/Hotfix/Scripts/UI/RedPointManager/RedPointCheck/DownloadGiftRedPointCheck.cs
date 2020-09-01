using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadGiftRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.DownloadResComplete, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.PlayerRoleExtraValues, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.DownloadGift, CSDownloadGiftInfo.Instance.CheckDownloadRewardRedpoint());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.DownloadResComplete, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.PlayerRoleExtraValues, OnCheckRedPoint);
    }
}
