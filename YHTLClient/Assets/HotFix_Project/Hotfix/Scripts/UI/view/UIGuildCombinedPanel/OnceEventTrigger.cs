public enum OnceEvent
{
    OnLogginTrigger = 1,//登录流程触发器
    AutoFightTrigger = 2,//自动战斗触发器
}

public class OnceEventTrigger : Singleton<OnceEventTrigger>
{
    EventHanlderManager eventHanlder = new EventHanlderManager(EventHanlderManager.DispatchType.OnceEventTrigger);
    /// <summary>
    /// 注册的一次性事件
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="action"></param>
    public void Register(OnceEvent eventId,System.Action action)
    {
        eventHanlder.Reg((uint)eventId, (id, argv) => { action?.Invoke(); });
    }

    /// <summary>
    /// 触发的一次性事件
    /// </summary>
    /// <param name="eventId"></param>
    public void Trigger(OnceEvent eventId)
    {
        eventHanlder.ProcEvent((uint)eventId,null);
        eventHanlder.UnReg((uint)eventId);
    }
}