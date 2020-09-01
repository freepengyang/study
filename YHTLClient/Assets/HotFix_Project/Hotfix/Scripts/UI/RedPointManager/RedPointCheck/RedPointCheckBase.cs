public abstract class RedPointCheckBase : IRedPointCheck
{
    public EventHanlderManager mClientEvent { get; set; }

    public virtual int funcopenId
    {
        get { return 0; }
    }
    
    public void Init(EventHanlderManager client)
    {
        mClientEvent = client;
        Init();
        if(funcopenId != 0)
        {
            if(!UICheckManager.Instance.DoCheckFunction(funcopenId))
            {
                client.AddEvent(CEvent.FunctionOpenStateChange, FunctionOpenStateChange);
            }
        }
    }
    
    public abstract void Init();

    public abstract void OnDestroy();
    
    protected void RefreshRed(RedPointType type, bool state)
    {
        if(funcopenId != 0)
        {
            if(!UICheckManager.Instance.DoCheckFunction(funcopenId))
            {
                state = false;
            }
        }
        UIRedPointManager.Instance.RefreshRedPoint(type, state);
    }
    
    private void FunctionOpenStateChange(uint id, object data)
    {
        if(data == null) return;
        if( (int) data == funcopenId && mClientEvent != null)
        {
            mClientEvent.RemoveEvent(CEvent.FunctionOpenStateChange, FunctionOpenStateChange);
            LoginOrFuncRedCheck();
        }
    }

    /// <summary>
    /// 需要登录 或者需要检测功能开放判断  检测小红点的，在此方法中调用
    /// </summary>
    public abstract void LoginOrFuncRedCheck();
}