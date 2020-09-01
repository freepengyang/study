
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 基础事件
/// </summary>
public class BaseEvent
{
    public delegate void Callback(uint uiEvtID, object data);

    /// <summary>
    /// 事件的集合
    /// </summary>
    public Dictionary<uint, EventDelegate> mDicEvtDelegate = new Dictionary<uint, EventDelegate>();

    /// <summary>
    /// 事件委托类
    /// </summary>
    public class EventDelegate
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        private uint uiEvtID;

        Callback arrCallBack;
        //Callback arr2Process;
        //private List<Callback> arrCallBack = new List<Callback>();
        //private List<Callback> arr2Process = new List<Callback>();

        public EventDelegate(uint _uiEvtId)
        {
            uiEvtID = _uiEvtId;
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="cb">回调函数</param>
        public void AddCallBack(Callback cb)
        {
            if (null == arrCallBack)
                arrCallBack = cb;
            else
            {
                arrCallBack -= cb;
                arrCallBack += cb;
            }
            //if (!arrCallBack.Contains(cb))
            //{
            //    arrCallBack.Add(cb);
            //}
        }


        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="cb">回调函数</param>
        public void RemoveCallBack(Callback cb)
        {
            if (null != arrCallBack)
                arrCallBack -= cb;
            //arrCallBack.Remove(cb);
            //if (null != arr2Process)
            //    arr2Process -= cb;
            //arr2Process.Remove(cb);
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="objData">参数</param>

        public void ProcEvent(object objData)
        {
            if(null != arrCallBack)
                arrCallBack(uiEvtID, objData);
            //arr2Process -= arrCallBack;
            //arr2Process += arrCallBack;
            //arr2Process.AddRange(arrCallBack);

            //while(arr2Process.Count > 0)
            //{
            //    var cb = arr2Process[0];
            //    arr2Process.RemoveAt(0);
            //    cb(uiEvtID, objData);
            //}
            //for (int i = 0; i < arr2Process.Count;i++ )
            //{
            //    Callback cb = arr2Process[i] as Callback;
            //    cb(uiEvtID, objData);
            //}

            //arr2Process.Clear();
        }
    }

    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="uiEvtID"></param>
    /// <param name="callBack"></param>
    public void Reg(uint uiEvtID, Callback callBack)
    {
        EventDelegate evtDelegate;
        if (!mDicEvtDelegate.ContainsKey(uiEvtID))
        {
            evtDelegate = new EventDelegate(uiEvtID);
            mDicEvtDelegate.Add(uiEvtID, evtDelegate);
        }
        else
        {
            evtDelegate = mDicEvtDelegate[uiEvtID];
        }

        if (null != evtDelegate)
        {
            evtDelegate.AddCallBack(callBack);
        }
    }

    /// <summary>
    /// 删除事件
    /// </summary>
    /// <param name="uiEvtID"></param>
    /// <param name="callBack"></param>
    public void UnReg(uint uiEvtID, Callback callBack)
    {
        if (mDicEvtDelegate.TryGetValue(uiEvtID,out EventDelegate evtDelegate))
        {
            evtDelegate.RemoveCallBack(callBack);
        }
    }

    /// <summary>
    /// 删除事件
    /// </summary>
    /// <param name="uiEvtId"></param>
    public void UnReg(uint uiEvtId)
    {
        mDicEvtDelegate.Remove(uiEvtId);
    }

    /// <summary>
    /// 分发事件
    /// </summary>
    /// <param name="uiEvtID"></param>
    /// <param name="objData"></param>
    /// <returns></returns>
    public void ProcEvent(uint uiEvtID, object objData)
    {
        if (mDicEvtDelegate.TryGetValue(uiEvtID,out EventDelegate evtDelegate))
        {
            evtDelegate.ProcEvent(objData);
        }
    }
}

// 用于保证成对注册和注销
public class EventHanlderManager
{
    public enum DispatchType
    {
        Socket = 0,
        Event,
        RedPoint,
        CombineRedPoint,
        OnceEventTrigger,
    }

    //private List<MsgCBPair> mCBPairs = new List<MsgCBPair>();
    private Dictionary<uint, BaseEvent.Callback> mCBPairs = new Dictionary<uint, BaseEvent.Callback>(32);

    public BaseEvent mDispatcher = null;

    static BaseEvent msSocketDispatcher = new BaseEvent();
    static BaseEvent msEventDispatcher = new BaseEvent();
    static BaseEvent msRedPointDispatcher = new BaseEvent();
    static BaseEvent msCombineRedPointDispatcher = new BaseEvent();
    static BaseEvent msEventTriggerDispatcher = new BaseEvent();

    public EventHanlderManager(DispatchType dt)
    {
        switch (dt)
        {
            case DispatchType.Event:
                mDispatcher = msEventDispatcher;
                break;
            case DispatchType.RedPoint:
                mDispatcher = msRedPointDispatcher;
                break;
            case DispatchType.Socket:
                mDispatcher = msSocketDispatcher;
                break;
            case DispatchType.CombineRedPoint:
                mDispatcher = msCombineRedPointDispatcher;
                break;
            case DispatchType.OnceEventTrigger:
                mDispatcher = msEventTriggerDispatcher;
                break;
        }
    }

    public void Reg(ECM msgId, BaseEvent.Callback cb)
    {
        Reg((uint)msgId, cb);
    }

    public void Reg(uint msgId, BaseEvent.Callback cb)
    {
        //for (int i = 0; i < mCBPairs.Count; ++i)
        //{
        //    if (mCBPairs[i].ID == msgId && mCBPairs[i].CB == cb)
        //    {
        //        if (mDispatcher.IsExistKeyAndCallBack(msgId, cb))
        //        {
        //            return;
        //        }
        //    }
        //}

        if(mCBPairs.TryGetValue(msgId,out BaseEvent.Callback msgCb))
        {
            msgCb -= cb;
            if (null == msgCb)
            {
                msgCb = cb;
                mCBPairs[msgId] = msgCb;
            }
            else
                msgCb += cb;
        }
        else
        {
            mCBPairs.Add(msgId, cb);
        }

        //MsgCBPair pair;
        //pair.ID = msgId;
        //pair.CB = cb;
        //mCBPairs.Add(pair);
        mDispatcher.Reg(msgId, cb);
    }

    public void UnReg(uint msgId, BaseEvent.Callback cb)
    {
        //for (int i = 0,max = mCBPairs.Count; i < max; ++i)
        //{
        //    if (mCBPairs[i].ID == msgId && mCBPairs[i].CB == cb)
        //    {
        //        mCBPairs.RemoveAt(i);
        //        break;
        //    }
        //}
        if(mCBPairs.TryGetValue(msgId,out BaseEvent.Callback msgCb))
        {
            msgCb -= cb;
            if (null == msgCb)
                mCBPairs.Remove(msgId);
        }

        mDispatcher.UnReg(msgId, cb);
    }

    public void UnReg(CEvent id, BaseEvent.Callback cb)
    {
        UnReg((uint)id, cb);
    }

    //针对UnReg会造成删去全局所有msgId事件回调的问题，优化成只删去和mCBPairs相关的回调
    public void UnRegMsg(uint msgId)
    {
        //for (int i = 0; i < mCBPairs.Count; i++)
        //{
        //    if (mCBPairs[i].ID == msgId)
        //    {
        //        mDispatcher.UnReg(msgId, mCBPairs[i].CB);
        //        mCBPairs.RemoveAt(i);
        //    }
        //}

        if(mCBPairs.TryGetValue(msgId,out BaseEvent.Callback msgCb))
        {
            mDispatcher.UnReg(msgId, msgCb);
            mCBPairs.Remove(msgId);
        }
    }

    /// <summary>
    /// 此处取消注册是 取消该该消息ID对应的所有回调，包括其他事件注册的回调
    /// </summary>
    public void UnReg(uint msgId)
    {
        //for (int i = 0; i < mCBPairs.Count; i++)
        //{
        //    if (mCBPairs[i].ID == msgId)
        //    {
        //        mCBPairs.RemoveAt(i);
        //    }
        //}
        //mDispatcher.UnReg(msgId);
        if (mCBPairs.TryGetValue(msgId, out BaseEvent.Callback msgCb))
        {
            mDispatcher.UnReg(msgId, msgCb);
            mCBPairs.Remove(msgId);
        }
    }

    public void UnRegAll()
    {
        for(var it = mCBPairs.GetEnumerator();it.MoveNext();)
        {
            mDispatcher.UnReg(it.Current.Key,it.Current.Value);
        }
        mCBPairs.Clear();
        //for (int i = 0; i < mCBPairs.Count; ++i)
        //{
        //    mDispatcher.UnReg(mCBPairs[i].ID, mCBPairs[i].CB);
        //}

        //mCBPairs.Clear();
    }

    public void ProcEvent(uint uiEvtID, object objData)
    {
         mDispatcher.ProcEvent(uiEvtID, objData);
    }


    #region 针对客户端事件
    public void AddEvent(CEvent uiEvtID, BaseEvent.Callback callback)
    {
        Reg((uint)uiEvtID, callback);
    }

    /// <summary>
    /// 此处取消注册是 取消该该消息ID对应的所有回调，包括其他事件注册的回调
    /// </summary>
    public void RemoveEvent(CEvent uiEvitID)
    {
        UnReg((uint)uiEvitID);
    }
    public void RemoveEvent(CEvent uiEvitID, BaseEvent.Callback cb)
    {
        UnReg((uint)uiEvitID, cb);
    }

    public void SendEvent(CEvent uiEvtID, object objData = null)
    {
        ProcEvent((uint)uiEvtID, objData);
    }
    #endregion

    #region 针对客户端小红点事件
    public void AddEvent(RedPointType uiEvtID, BaseEvent.Callback callback)
    {
        Reg((uint)uiEvtID, callback);
    }

    public void RemoveEvent(RedPointType uiEvitID)
    {
        UnReg((uint)uiEvitID);
    }

    public void RemoveEvent(RedPointType uiEvitID, BaseEvent.Callback cb)
    {
        UnReg((uint)uiEvitID,cb);
    }

    public void SendEvent(RedPointType uiEvtID,  object objData = null)
    {
        ProcEvent((uint)uiEvtID, objData);
    }
    #endregion
    
}

/// <summary>
/// EventHanlderManager 无法对同一个枚举区分调用
/// </summary>
public class ClientHanlderManager
{
    public delegate void OnCallBack(uint id, object data);
    public Dictionary<int,OnCallBack> dic = new Dictionary<int,OnCallBack>();
    public void AddEvent(CEvent e, OnCallBack callBack)
    {
        int ie = (int)e;
        if (!dic.ContainsKey(ie))
        {
            dic.Add(ie, null);
        }
        dic[ie] -= callBack;
        dic[ie] += callBack;
    }

    public void RemoveEvent(CEvent e,OnCallBack callBack)
    {
        int ie = (int)e;
        if (dic.ContainsKey(ie))
        {
            dic[ie] -= callBack;
        }
    }

    public void RemoveAll()
    {
        dic.Clear();
    }

    public void SendEvent(CEvent e, object datas = null)
    {
        int ie = (int)e;
        if (dic.ContainsKey(ie))
        {
            if(dic[ie]!=null)
                dic[ie]((uint)e,datas);
        }
    }
}


public class  EventData{
    public object arg1;
    public object arg2;
}

public class EventData2{
    public object arg1;
    public object arg2;
    public object arg3;
}

public class CSEventObjectManager : Singleton<CSEventObjectManager>
{
    private Queue<EventData> eventDataQue = new Queue<EventData>();
    private Queue<EventData2> eventDataQue2 = new Queue<EventData2>();

    public EventData SetValue(object arg1, object arg2)
    {
        EventData data;
        if (eventDataQue.Count > 0)
        {
            data = eventDataQue.Dequeue();
        }
        else
            data = new EventData();

        data.arg1 = arg1;
        data.arg2 = arg2;
        return data;
    }

    public EventData2 SetValue(object arg1, object arg2, object arg3)
    {
        EventData2 data;
        if (eventDataQue2.Count > 0)
        {
            data = eventDataQue2.Dequeue();
        }
        else
            data = new EventData2();

        data.arg1 = arg1;
        data.arg2 = arg2;
        data.arg3 = arg3;
        return data;
    }

    public void Recycle(EventData message)
    {
        if(eventDataQue != null) eventDataQue.Enqueue(message);
    }

    public void Recycle(EventData2 message)
    {
        if(eventDataQue2 != null) eventDataQue2.Enqueue(message);
    }

    public void Dispose()
    {
        if (eventDataQue != null) eventDataQue.Clear();
        if (eventDataQue2 != null) eventDataQue2.Clear();
    }
}
