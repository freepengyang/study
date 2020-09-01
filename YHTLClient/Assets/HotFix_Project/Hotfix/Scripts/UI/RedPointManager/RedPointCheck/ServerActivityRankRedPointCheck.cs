using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ServerActivityRankRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        //Debug.LogError("ServerActivityRankRedPointCheck.Init");
        HotManager.Instance.EventHandler.AddEvent(CEvent.ResSpecialActivityDataMessage, OnCheckRedPoint);
       // mClientEvent.AddEvent(CEvent.ResSpecialActivityDataMessage, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.ServerActivityRank, CSOpenServerACInfo.Instance.ServerActivityRankRedPointChenk());
    }

    public override void OnDestroy()
    {
        //Debug.LogError("ServerActivityRankRedPointCheck.OnDestroy");
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.ResSpecialActivityDataMessage, OnCheckRedPoint);
        //mClientEvent.RemoveEvent(CEvent.ResSpecialActivityDataMessage, OnCheckRedPoint);
    }
}
