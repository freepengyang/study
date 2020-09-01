using UnityEngine;
using System.Collections;

/// <summary>
/// 一般用来放服务器数据的,也可以放一些数据先创建，Mono的创建有延迟的情况
/// 该类中不允许存在Mono脚本,可以开启Check检查
/// 数据改变影响MonoInfo时的逻辑处理了1 在Mono里面通过AddEvent挂回调，2 在数据改变的地方调用CSOnlyDataBase的SendEvent
/// </summary>
/// <typeparam name="T_MonoInfo">如果没有对应mono，设置成自己即可</typeparam>
public class CSDataBase : ICSMonoBase
{
    private CSResource mRes;
    public CSResource Res
    {
        get { return mRes; }
        set { mRes = value; }
    }

    private CSMonoBase mMonoInfo;
    /// <summary>
    /// 建议不要在外部用，后面有可能取消外部对其的访问权限
    /// </summary>
    public CSMonoBase MonoInfo
    {
        get { return mMonoInfo; }
        set { mMonoInfo = value; }
    }


    //public EventHanlderManager EventHandler = new EventHanlderManager(EventHanlderManager.DispatchType.Event);
    public delegate void OnLoadedCallBack(CSDataBase t);
    OnLoadedCallBack onLoadedCallBack;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path">相对于</param>
    public virtual void LoadResource(string name, int type, int assistType = ResourceAssistType.None, OnLoadedCallBack callBack = null)
    {
        if (callBack != null)
        {
            onLoadedCallBack = callBack;
        }
        mRes = CSResourceManager.Singleton.AddQueue(name, type, OnLoaded,assistType);
    }

    public virtual void OnLoaded(CSResource res)
    {
        mRes = res;
        if (onLoadedCallBack != null)
        {
            onLoadedCallBack(this);
        }
    }

    public void RemoveAllEvent()
    {
        //EventHandler.UnRegAll();
    }

    public virtual void CSUpdate()
    {

    }

    public virtual void CSOnDestroy() { }
}
