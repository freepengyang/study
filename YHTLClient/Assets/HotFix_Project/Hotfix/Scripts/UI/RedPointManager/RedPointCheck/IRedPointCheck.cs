public interface IRedPointCheck
{
    EventHanlderManager mClientEvent { get; set; }

    void Init(EventHanlderManager mClientEvent);
    
    void Init();
    
    void OnDestroy();

    /// <summary>
    /// 需要登录检测小红点的，在此方法中调用
    /// </summary>
    void LoginOrFuncRedCheck();
}