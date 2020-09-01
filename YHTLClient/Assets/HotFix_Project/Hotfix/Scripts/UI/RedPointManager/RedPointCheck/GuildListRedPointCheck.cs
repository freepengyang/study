﻿public class GuildListRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.OnMainPlayerGuildIdChanged, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnMainPlayerGuildPosChanged, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnGuildTabDataChanged, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.GuildList,CSGuildInfo.Instance.CheckGuildListRedPoint());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnMainPlayerGuildIdChanged, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnMainPlayerGuildPosChanged, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnGuildTabDataChanged, OnCheckRedPoint);
    }
}