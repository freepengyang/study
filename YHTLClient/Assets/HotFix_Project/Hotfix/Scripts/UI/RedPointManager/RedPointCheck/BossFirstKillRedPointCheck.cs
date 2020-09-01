using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BossFirstKillRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.ResSpecialActivityDataMessage, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        //Debug.Log("  boss首杀红点判断 " +CSOpenServerACInfo.Instance.BossFisrtRedPointChenk());
        RefreshRed(RedPointType.BossFirstKill, CSOpenServerACInfo.Instance.BossFisrtRedPointChenk());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ResSpecialActivityDataMessage, OnCheckRedPoint);
    }
}
