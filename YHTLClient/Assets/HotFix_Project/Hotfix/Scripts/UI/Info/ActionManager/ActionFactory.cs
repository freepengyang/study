using System;

public class ActionFactory : IActionFactory
{
    Func<IAction> factory;
    public ActionFactory(Func<IAction> factory)
    {
        this.factory = factory;
    }
    public IAction Create()
    {
        return factory();
    }
}

public class ActionPoolManager : CSInfo<ActionPoolManager>
{
    PoolHandleManager mPoolHandleManager = new PoolHandleManager();

    public IAction Get<T>() where T : class,IAction,new()
    {
        return mPoolHandleManager.GetSystemClass<T>();
    }

    public T GetParam<T>() where T : class, IActionParam, new()
    {
        return mPoolHandleManager.GetSystemClass<T>();
    }

    public void Recycle(IAction action)
    {
        if(null != action)
        {
            action.OnRecycle();
            mPoolHandleManager.Recycle(action);
        }
    }

    public void Recycle(IActionParam action)
    {
        if (null != action)
        {
            mPoolHandleManager.Recycle(action);
        }
    }

    public override void Dispose()
    {
        mPoolHandleManager?.RecycleAll();
        mPoolHandleManager = null;
    }
}

/// <summary>
/// IA = IAction
/// IP = IParam
/// </summary>
/// <typeparam name="IA"></typeparam>
/// <typeparam name="IP"></typeparam>
public class ActionNode<IA,IP> : ActionBase
{
    public override int ID 
    {
        get
        {
            return (int)EnumAction.Node;
        }
    }

    int status = 0;
    ActionNodeParam nodeParam;

    public override IActionParam ActionParam
    {
        get
        {
            return nodeParam.param;
        }
    }

    bool succeed = false;
    public override bool Succeed
    {
        get
        {
            return succeed;
        }
        set
        {
            succeed = value;
        }
    }

    public override void Init(IActionParam argv)
    {
        base.Init(argv);
        nodeParam = argv as ActionNodeParam;
        status = 0;
        Succeed = true;
    }

    public override bool IsDone()
    {
        if (status == 2)
            return true;

        if(status == 0)
        {
            if (null == nodeParam || null == nodeParam.action || null == nodeParam.param)
            {
                OnFailed();
                return false;
            }
            nodeParam.action.Init(nodeParam.param);
            status = 1;
            return false;
        }

        if(status == 1)
        {
            if (!nodeParam.action.IsDone())
            {
                return false;
            }

            Succeed = nodeParam.action.Succeed;

            if(!Succeed)
            {
                OnFailed();
            }
            else
            {
                OnSucceed();
            }

            return false;
        }

        return false;
    }

    void OnFailed()
    {
        Succeed = false;
        status = 2;
    }

    void OnSucceed()
    {
        Succeed = true;
        status = 2;
    }

    public override void OnRecycle()
    {
        if(null != nodeParam)
        {
            nodeParam.action?.OnRecycle();
            nodeParam.action = null;
            if(null != nodeParam.param)
            {
                ActionPoolManager.Instance.Recycle(nodeParam.param);
                nodeParam.param = null;
            }
            nodeParam = null;
        }
        base.OnRecycle();
    }
}

public class ActionFactory<T,K> : ActionFactory where T : class,IAction, new() where K : class,IActionParam,new()
{
    public ActionFactory() : base(()=> { return Factory(); })
    {

    }

    static IAction Factory()
    {
        IAction actionNode = ActionPoolManager.Instance.Get<ActionNode<T,K>>();
        var actionParam = ActionPoolManager.Instance.GetParam<K>();
        var action = ActionPoolManager.Instance.Get<T>();
        var actionNodeParam = ActionPoolManager.Instance.GetParam<ActionNodeParam>();
        actionNodeParam.action = action;
        actionNodeParam.param = actionParam;
        actionNode.Init(actionNodeParam);
        return actionNode;
    }
}