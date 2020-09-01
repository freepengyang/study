using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : ActionBase
{
    public class Factory : ActionFactory<ActionQueue,ActionQueueParam>
    {

    }

    IAction[] mActionLists;
    IAction mRunningAction;
    int mActionIdx = -1;
    ActionQueueParam actionQueueParam;

    public override int ID 
    {
        get
        {
            return (int)EnumAction.ActionQueue;
        }
    }

    public override void Init(IActionParam argv)
    {
        base.Init(argv);
        actionQueueParam = argv as ActionQueueParam;
        mActionLists = actionQueueParam.mActionLists;
        mActionIdx = 0;
        Succeed = false;
        mRunningAction = null;
        if (null != mActionLists && mActionLists.Length > 0)
        {
            mRunningAction = mActionLists[mActionIdx];
            mActionLists[mActionIdx] = null;
        }
        Succeed = null != mRunningAction;
    }

    public override bool IsDone()
    {
        if(null == mRunningAction)
        {
            return true;
        }

        if(!mRunningAction.IsDone())
        {
            return false;
        }

        if(!mRunningAction.Succeed)
        {
            OnFailed();
            return false;
        }

        mRunningAction.OnRecycle();
        mRunningAction = null;

        if(++mActionIdx == mActionLists.Length)
        {
            OnSucceed();
            return false;
        }

        mRunningAction = mActionLists[mActionIdx];
        mActionLists[mActionIdx] = null;
        return false;
    }

    void Clear()
    {
        if(null != mActionLists)
        {
            for (int i = 0; i < mActionLists.Length; ++i)
            {
                var action = mActionLists[i];
                if (null != action)
                {
                    action.OnRecycle();
                    mActionLists[i] = null;
                }
            }
            mActionLists = null;
        }
        mRunningAction = null;
        actionQueueParam = null;
    }

    void OnSucceed()
    {
        Succeed = true;
    }

    void OnFailed()
    {
        Succeed = false;
    }

    public override void OnRecycle()
    {
        Clear();
        base.OnRecycle();
    }
}