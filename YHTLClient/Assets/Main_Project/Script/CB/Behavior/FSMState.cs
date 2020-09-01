﻿
// author LiZongFu
// date 2016.4

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FSMState
{
    bool mIsWaitToState = false;
    float mBeginWaitTime = 0;
    float mWaitTime = 0;
    int mWaitState = -1;
    BehaviorState mOverrideBehavior = null;
    BehaviorState mCurrentBehaivor = null;
    BehaviorState mDefaultBehavior = null;
    BetterList<BehaviorState> mBehaviors = new BetterList<BehaviorState>();
    public BetterList<BehaviorState> Behaviors
    {
        get { return mBehaviors; }
        set { mBehaviors = value; }
    }

    public int CurrentBehavior
    {
        get
        {
            return mCurrentBehaivor != null ? mCurrentBehaivor.Behavior : -1;
        }
    }

    public BehaviorState OverrideBehavior
    {
        get
        {
            return mOverrideBehavior;
        }
    }

    public void InitialAddBehavior(BehaviorState dpb)
    {
        if (mBehaviors.Contains(dpb))
        {
            return;
        }

        mBehaviors.Add(dpb);
    }

    public void Start(int defaultType)
    {
        mDefaultBehavior = GetBehavior(defaultType);
    }

    public BehaviorState GetBehavior(int type)
    {
        for (int i = 0; i < mBehaviors.Count; i++)
        {
            BehaviorState dbp = mBehaviors[i];

            if (dbp.Behavior == type)
            {
                return dbp;
            }
        }

        return null;
    }

    public void Switch2(int bt, bool voidRepeat)
    {
        BehaviorState bh = GetBehavior(bt);

        if (bh == null) return;

        ResetWait();

        if (bh == mCurrentBehaivor)
        {
            if (!voidRepeat)
            {
                mCurrentBehaivor.End(mCurrentBehaivor);
                mCurrentBehaivor.Start(mCurrentBehaivor);
            }
            return;
        }

        if (mCurrentBehaivor != null)
        {
            mCurrentBehaivor.End(bh);
        }
        BehaviorState lastBehv = mCurrentBehaivor;
        mCurrentBehaivor = bh;
        mCurrentBehaivor.Start(lastBehv);
    }

    public void SetWait(int ret, float waitTime, bool isSetOnce = false)
    {
        if (!mIsWaitToState || !isSetOnce)
        {
            mIsWaitToState = true;
            mWaitTime = waitTime;
            mBeginWaitTime = Time.time;
            mWaitState = ret;
        }
    }

    public void ResetWait()
    {
        mIsWaitToState = false;
    }

    public void UpdateBehaviors()
    {
        if (mCurrentBehaivor == null)
        {
            if (mDefaultBehavior != null)
            {
                mCurrentBehaivor = mDefaultBehavior;
                mCurrentBehaivor.Start(null);
            }
            return;
        }

        int ret = mCurrentBehaivor.Update();

        ret = GetWaitState(ret);
       
        if (ret != mCurrentBehaivor.Behavior)
        {
            if (mCurrentBehaivor == mOverrideBehavior)
            {
                mOverrideBehavior = null;
            }

            if (ret == -1 && mDefaultBehavior != null)
            {
                ret = mDefaultBehavior.Behavior;
            }

            BehaviorState nextBh = GetBehavior(ret);
   
            if (nextBh == null)
            {
                nextBh = mDefaultBehavior;
            }

            if (nextBh != null)
            {
                Switch2(nextBh.Behavior, true);
            }
            else
            {
                mCurrentBehaivor = null;
            }
        }
    }

    int GetWaitState(int ret)
    {
        if (!mIsWaitToState || Time.time - mBeginWaitTime < mWaitTime) return ret;
        ResetWait();
        return mWaitState;
    }

    public void Reset()
    {
        ResetWait();
        mBehaviors.Clear();
    }

    public void Release()
    {
        mBehaviors.Release();
    }
}
