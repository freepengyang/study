using System;
using System.Collections.Generic;

public partial class ActionManager : CSInfo<ActionManager>
{
    Dictionary<int, ActionFactory> mActionFactories = new Dictionary<int, ActionFactory>(32);
    FastArrayElementKeepHandle<IAction> mRunningActionList = new FastArrayElementKeepHandle<IAction>(16);

    public ActionManager()
    {
        Register();
    }
    
    void Log(string fmt,params object[] argvs)
    {
        try
        {
            FNDebug.LogFormat("<color=#00ff00>[action]:{0}</color>", string.Format(fmt, argvs));
        }
        catch(Exception e)
        {
            FNDebug.LogFormat("<color=#00ff00>[action]:Action Log Error : {0}</color>", e.Message);
        }
    }

    void LogError(string fmt, params object[] argvs)
    {
        try
        {
            FNDebug.LogFormat("<color=#ff0000>[action]:{0}</color>", string.Format(fmt, argvs));
        }
        catch (Exception e)
        {
            FNDebug.LogFormat("<color=#00ff00>[action]:Action LogError Error : {0}</color>", e.Message);
        }
    }

    IAction Create(EnumAction eAction)
    {
        int actionId = (int)eAction;
        if (!mActionFactories.ContainsKey(actionId))
        {
            LogError("Start Action Failed Has No Factory For {0}", actionId);
            return null;
        }
        var factory = mActionFactories[actionId];
        if(null == factory)
        {
            LogError("Start Action Failed Factory Is Null For {0}", actionId);
            return null;
        }
        IAction action = factory.Create();
        return action;
    }

    public void Run<A>(EnumAction eAction,A a)
    {
        var action = Create(eAction);
        if(null != action)
        {
            if(action.ActionParam is IActionParam<A> ap)
            {
                ap.Run(a);
                mRunningActionList.Append(action);
            }
        }
    }

    public void Run<A,B>(EnumAction eAction, A a,B b)
    {
        var action = Create(eAction);
        if (null != action)
        {
            if (action.ActionParam is IActionParam<A,B> ap)
            {
                ap.Run(a,b);
                mRunningActionList.Append(action);
            }
        }
    }

    public void Run<A, B, C>(EnumAction eAction, A a, B b, C c)
    {
        var action = Create(eAction);
        if (null != action)
        {
            if (action.ActionParam is IActionParam<A, B, C> ap)
            {
                ap.Run(a, b, c);
                mRunningActionList.Append(action);
            }
        }
    }

    public void Run<A, B, C, D>(EnumAction eAction, A a, B b, C c, D d)
    {
        var action = Create(eAction);
        if (null != action)
        {
            if (action.ActionParam is IActionParam<A, B, C, D> ap)
            {
                ap.Run(a, b, c, d);
                mRunningActionList.Append(action);
            }
        }
    }

    public IAction Create<A>(EnumAction eAction, A a)
    {
        var action = Create(eAction);
        if (null != action)
        {
            if (action.ActionParam is IActionParam<A> ap)
            {
                ap.Run(a);
            }
        }
        return action;
    }

    public IAction Create<A,B>(EnumAction eAction, A a,B b)
    {
        var action = Create(eAction);
        if (null != action)
        {
            if (action.ActionParam is IActionParam<A,B> ap)
            {
                ap.Run(a,b);
            }
        }
        return action;
    }

    public IAction Create<A, B,C>(EnumAction eAction, A a, B b,C c)
    {
        var action = Create(eAction);
        if (null != action)
        {
            if (action.ActionParam is IActionParam<A, B,C> ap)
            {
                ap.Run(a, b,c);
            }
        }
        return action;
    }

    public IAction Create<A, B, C, D>(EnumAction eAction, A a, B b, C c, D d)
    {
        var action = Create(eAction);
        if (null != action)
        {
            if (action.ActionParam is IActionParam<A, B, C, D> ap)
            {
                ap.Run(a, b, c, d);
            }
        }
        return action;
    }

    public IAction Create(EnumAction eAction, params IAction[] actions)
    {
        IAction action = Create(eAction);
        if (null != action && action.ActionParam is IActionParam<IAction[]> ap)
        {
            ap.Run(actions);
        }
        return action;
    }

    public void Run(EnumAction eAction,params IAction[] actions)
    {
        IAction action = Create(eAction);
        if (null != action && action.ActionParam is IActionParam<IAction[]> ap)
        {
            ap.Run(actions);
            mRunningActionList.Append(action);
        }
    }

    public void Update()
    {
        for(int i = mRunningActionList.Count - 1;i >= 0;--i)
        {
            var action = mRunningActionList[i];
            if(action.IsDone())
            {
                if(action.Succeed)
                {
                    var next = action.Next;
                    if(null != next)
                    {
                        mRunningActionList[i] = next;
                        continue;
                    }
                }
                mRunningActionList.SwapErase(i);
                ActionPoolManager.Instance.Recycle(action);
            }
        }
    }

    public override void Dispose()
    {
        //throw new System.NotImplementedException();
    }
}