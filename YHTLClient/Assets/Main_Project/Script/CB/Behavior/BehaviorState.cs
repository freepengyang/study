
//-------------------------------------------------------------------------
//状态
//Author LiZongFu
//Time 2015.12.15
//-------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class BehaviorState
{
    int mBehaviorType;
    System.Action<BehaviorState> mBehaviorStartDel = null;
    System.Func<int> mBehaviorUpdateDel = null;
    System.Action<BehaviorState> mBehaviorEndDel = null;

    public int Behavior
    {
        get { return mBehaviorType; }
    }

    public BehaviorState(int behv, System.Action<BehaviorState> startDel, System.Func<int> updateDel, System.Action<BehaviorState> endDel)
    {
        mBehaviorType = behv;
        mBehaviorStartDel = startDel;
        mBehaviorUpdateDel = updateDel;
        mBehaviorEndDel = endDel;
    }

    public void Start(BehaviorState lastBehv)
    {
        if (mBehaviorStartDel != null)
        {
            mBehaviorStartDel(lastBehv);
        }
    }

    public int Update()
    {
        if (mBehaviorUpdateDel != null)
        {
            return mBehaviorUpdateDel();
        }

        return Behavior;
    }

    public void End(BehaviorState nextBehv)
    {
        if (mBehaviorEndDel != null)
        {
            mBehaviorEndDel(nextBehv);
        }
    }
}