using map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSTriggerInfo : CSInfo<CSTriggerInfo>
{
    private Dictionary<long, RoundTrigger> mRoundTriggerDic = new Dictionary<long, RoundTrigger>();
    public void Add(RoundTrigger info)
    {
        if(!mRoundTriggerDic.ContainsKey(info.triggerId))
        {
            mRoundTriggerDic.Add(info.triggerId,info);
        }
    }

    public void Update(RoundTrigger info)
    {
        if (!mRoundTriggerDic.ContainsKey(info.triggerId))
        {
            mRoundTriggerDic.Add(info.triggerId, info);
        }
        else
        {
            mRoundTriggerDic[info.triggerId] = info;
        }
    }

    public void Remove(long id)
    {
        if(mRoundTriggerDic.ContainsKey(id))
        {
            mRoundTriggerDic.Remove(id);
        }
    }

    public override void Dispose()
    {
        mRoundTriggerDic.Clear();
    }
}
