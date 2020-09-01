using UnityEngine;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using System.Collections;

public abstract class UIBinder// : UIBase
{
    public PoolHandleManager PoolHandle { get; set; }
    public EventHanlderManager EventHandle { get; set; }
    public UIEventListener Handle { get; set; }
    protected Transform CachedTransform;
    
    //当前item的index
    public int index { get; set; }

    public T Get<T>(string path, Transform parent = null) where T : UnityEngine.Object
    {
        Transform objTrans = Get(path, parent);
        if (objTrans == null) return null;

        if (typeof(T) == typeof(Transform)) return objTrans as T;

        if (typeof(T) == typeof(GameObject)) return objTrans.gameObject as T;

        if (objTrans)
            return objTrans.GetComponent<T>();
        return null;
    }

    public Transform Get(string _path, Transform parent = null)
    {
        if (parent == null)
        {
            if (CachedTransform)
            {
                return CachedTransform.Find(_path);
            }
            else
                return null;
        }
        else
            return parent.Find(_path);
    }

    public UIBinder()
    {

    }

    public void Setup(UIEventListener handle, PoolHandleManager poolHandleManager)
    {
        Handle = handle;
        CachedTransform = handle.transform;
        handle.parameter = this;
        PoolHandle = poolHandleManager;
        Init(handle);
    }

    public void Setup(UIEventListener handle)
    {
        Handle = handle;
        CachedTransform = handle.transform;
        Handle.parameter = this;
        Init(handle);
    }

    public void BindData(PoolHandleManager poolHandle,object data)
    {
        if(null != PoolHandle && PoolHandle != poolHandle)
        {
            FNDebug.LogError("使用不同的池子进行了BindData");
        }
        PoolHandle = poolHandle;
        Bind(data);
    }

    public abstract void Init(UIEventListener handle);

    public abstract void Bind(object data);

    public void Destroy()
    {
        OnDestroy();
        if (Handle != null)
        {
            Handle.parameter = null;
            Handle = null;
        }        
        if (null != PoolHandle)
        {
            PoolHandle.Recycle(this);
            PoolHandle = null;
        }
    }

    public abstract void OnDestroy();
}

public static class UIBinderExtend
{
    public static T AddBinder<T>(this GameObject go,PoolHandleManager poolHandle) where T : UIBinder,new()
    {
        T handle = null == poolHandle ? new T() : poolHandle.GetSystemClass<T>();
        handle.Setup(UIEventListener.Get(go),poolHandle);
        return handle;
    }

    public static T GetOrAddBinder<T>(this GameObject go,PoolHandleManager poolHandle) where T : UIBinder, new()
    {
        var eventHandler = UIEventListener.Get(go);
        if (eventHandler.parameter is T binder)
            return binder;

        T addBinder = null == poolHandle ? new T() : poolHandle.GetSystemClass<T>();
        addBinder.Setup(eventHandler, poolHandle);
        return addBinder;
    }

    public static void DestroyBinder<T>(this GameObject go) where T : UIBinder, new()
    {
        var eventHandler = UIEventListener.Get(go);
        if (eventHandler.parameter is T binder)
        {
            binder.Destroy();
        }
    }

    public static void CustomActive(this GameObject go,bool visible)
    {
        if (null != go && go.activeSelf != visible)
            go.SetActive(visible);
    }

    public static void CustomActive(this Component component, bool visible)
    {
        if (null != component && component.gameObject.activeSelf != visible)
            component.gameObject.SetActive(visible);
    }

    public static IEnumerator BindAsync<T, K>(this UIGridContainer container, FastArrayElementFromPool<T> datas, PoolHandleManager poolHandle = null) where K : UIBinder, new() where T : class, new()
    {
        if (null != container)
        {
            void OnItemVisible(GameObject gameObject, int idx)
            {
                var binder = gameObject.GetOrAddBinder<K>(poolHandle);
                if (null != datas && idx >= 0 && idx < datas.Count)
                    binder.Bind(datas[idx]);
            }
            int count = null == datas ? 0 : datas.Count;
            yield return container.BindAsync(count, OnItemVisible);
        }
    }

    public static void Bind<T,K>(this UIGridContainer container,List<T> datas, PoolHandleManager poolHandle = null,EventHanlderManager eventHandle = null) where K : UIBinder,new()
    {
        if(null != container)
        {
            if(null == datas)
            {
                container.MaxCount = 0;
            }
            else
            {
                container.MaxCount = datas.Count;
                for (int i = 0; i < container.controlList.Count; ++i)
                {
                    var handle = container.controlList[i];
                    var listener = UIEventListener.Get(handle);
                    if (listener.parameter is K binder)
                    {
                        binder.index = i;
                        binder.EventHandle = eventHandle;
                        binder.BindData(poolHandle,datas[i]);
                    }
                    else
                    {
                        if (null != poolHandle)
                        {
                            listener.parameter = poolHandle.GetSystemClass<K>();
                        }
                        else
                        {
                            listener.parameter = new K();
                        }

                        if (listener.parameter is K binderNew)
                        {
                            binderNew.index = i;
                            binderNew.EventHandle = eventHandle;
                            binderNew.Setup(listener);
                            binderNew.BindData(poolHandle,datas[i]);
                        }
                        
                    }
                }
            }
        }
    }

    public static void Bind<T, K>(this UIGridContainer container, FastArrayElementFromPool<T> datas, PoolHandleManager poolHandle = null) where K : UIBinder, new() where T : class, new()
    {
        if (null != container)
        {
            if (null == datas)
            {
                container.MaxCount = 0;
            }
            else
            {
                container.MaxCount = datas.Count;
                for (int i = 0; i < container.controlList.Count; ++i)
                {
                    var handle = container.controlList[i];
                    var listener = UIEventListener.Get(handle);
                    if (listener.parameter is K binder)
                    {
                        binder.index = i;
                        binder.BindData(poolHandle, datas[i]);
                    }
                    else
                    {
                        if (null != poolHandle)
                        {
                            listener.parameter = poolHandle.GetSystemClass<K>();
                        }
                        else
                        {
                            listener.parameter = new K();
                        }

                        if (listener.parameter is K binderNew)
                        {
                            binderNew.index = i;
                            binderNew.Setup(listener);
                            binderNew.BindData(poolHandle, datas[i]);
                        }

                    }
                }
            }
        }
    }

    public static void Bind<T, K>(this UIGridContainer container, FastArrayElementKeepHandle<T> datas, PoolHandleManager poolHandle = null) where K : UIBinder, new() where T : class
    {
        if (null != container)
        {
            if (null == datas)
            {
                container.MaxCount = 0;
            }
            else
            {
                container.MaxCount = datas.Count;
                for (int i = 0; i < container.controlList.Count; ++i)
                {
                    var handle = container.controlList[i];
                    var listener = UIEventListener.Get(handle);
                    if (listener.parameter is K binder)
                    {
                        binder.index = i;
                        binder.BindData(poolHandle, datas[i]);
                    }
                    else
                    {
                        if (null != poolHandle)
                        {
                            listener.parameter = poolHandle.GetSystemClass<K>();
                        }
                        else
                        {
                            listener.parameter = new K();
                        }

                        if (listener.parameter is K binderNew)
                        {
                            binderNew.index = i;
                            binderNew.Setup(listener);
                            binderNew.BindData(poolHandle, datas[i]);
                        }

                    }
                }
            }
        }
    }

    public static void Bind<Component,Value>(this UIGridContainer container, FastArray<Value> datas,EventHanlderManager eventHandle = null) where Value : class, IndexedItem, new() where Component : UIBinder,new()
    {
        if (null != container)
        {
            if (null == datas)
            {
                container.MaxCount = 0;
            }
            else
            {
                container.MaxCount = datas.Count;
                for (int i = 0; i < container.controlList.Count; ++i)
                {
                    var handle = container.controlList[i];
                    var listener = UIEventListener.Get(handle);
                    if (listener.parameter is Component binder)
                    {
                        binder.index = i;
                        binder.EventHandle = eventHandle;
                        binder.Bind(datas[i]);
                    }
                    else
                    {
                        listener.parameter = new Component();
                        binder = listener.parameter as Component;
                        binder.EventHandle = eventHandle;
                        binder.index = i;
                        binder.Setup(listener);
                        binder.Bind(datas[i]);
                    }
                }
            }
        }
    }
    public static void Bind<Component, Value>(this UIGridContainer container, FastArrayElementKeepHandle<Value> datas, EventHanlderManager eventHandle = null) where Value : class, IndexedItem, new() where Component : UIBinder, new()
    {
        if (null != container)
        {
            if (null == datas)
            {
                container.MaxCount = 0;
            }
            else
            {
                container.MaxCount = datas.Count;
                for (int i = 0; i < container.controlList.Count; ++i)
                {
                    var handle = container.controlList[i];
                    var listener = UIEventListener.Get(handle);
                    if (listener.parameter is Component binder)
                    {
                        binder.index = i;
                        binder.EventHandle = eventHandle;
                        binder.Bind(datas[i]);
                    }
                    else
                    {
                        listener.parameter = new Component();
                        binder = listener.parameter as Component;
                        binder.EventHandle = eventHandle;
                        binder.index = i;
                        binder.Setup(listener);
                        binder.Bind(datas[i]);
                    }
                }
            }
        }
    }

    public static void Bind<Component, Value>(this UIGridContainer container, FastArrayElementFromPool<Value> datas, EventHanlderManager eventHandle = null) where Value : class, IndexedItem, new() where Component : UIBinder, new()
    {
        if (null != container)
        {
            if (null == datas)
            {
                container.MaxCount = 0;
            }
            else
            {
                container.MaxCount = datas.Count;
                for (int i = 0; i < container.controlList.Count; ++i)
                {
                    var handle = container.controlList[i];
                    var listener = UIEventListener.Get(handle);
                    if (listener.parameter is Component binder)
                    {
                        binder.index = i;
                        binder.EventHandle = eventHandle;
                        binder.Bind(datas[i]);
                    }
                    else
                    {
                        listener.parameter = new Component();
                        binder = listener.parameter as Component;
                        binder.EventHandle = eventHandle;
                        binder.index = i;
                        binder.Setup(listener);
                        binder.Bind(datas[i]);
                    }
                }
            }
        }
    }
    
    public static void Bind<T,K>(this UIGridContainer container,CSBetterList<T> datas, PoolHandleManager poolHandle = null) where K : UIBinder,new()
    {
        if(null != container)
        {
            if(null == datas)
            {
                container.MaxCount = 0;
            }
            else
            {
                container.MaxCount = datas.Count;
                for (int i = 0; i < container.controlList.Count; ++i)
                {
                    var handle = container.controlList[i];
                    var listener = UIEventListener.Get(handle);
                    if (listener.parameter is K binder)
                    {
                        binder.index = i;
                        binder.BindData(poolHandle,datas[i]);
                    }
                    else
                    {
                        if (null != poolHandle)
                        {
                            listener.parameter = poolHandle.GetSystemClass<K>();
                        }
                        else
                        {
                            listener.parameter = new K();
                        }

                        if (listener.parameter is K binderNew)
                        {
                            binderNew.index = i;
                            binderNew.Setup(listener);
                            binderNew.BindData(poolHandle,datas[i]);
                        }
                        
                    }
                }
            }
        }
    }
    
    public static void Bind<T,K>(this UIGridContainer container,RepeatedField<T> datas, PoolHandleManager poolHandle = null) where K : UIBinder,new()
    {
        if(null != container)
        {
            if(null == datas)
            {
                container.MaxCount = 0;
            }
            else
            {
                container.MaxCount = datas.Count;
                for (int i = 0; i < container.controlList.Count; ++i)
                {
                    var handle = container.controlList[i];
                    var listener = UIEventListener.Get(handle);
                    if (listener.parameter is K binder)
                    {
                        binder.index = i;
                        binder.BindData(poolHandle,datas[i]);
                    }
                    else
                    {
                        if (null != poolHandle)
                        {
                            listener.parameter = poolHandle.GetSystemClass<K>();
                        }
                        else
                        {
                            listener.parameter = new K();
                        }

                        if (listener.parameter is K binderNew)
                        {
                            binderNew.index = i;
                            binderNew.Setup(listener);
                            binderNew.BindData(poolHandle,datas[i]);
                        }
                        
                    }
                }
            }
        }
    }

    public static void Bind<T, K>(this UIGridContainer container, CSBetterLisHot<T> datas, PoolHandleManager poolHandle = null) where K : UIBinder, new()
    {
        if (null != container)
        {
            if (null == datas)
            {
                container.MaxCount = 0;
            }
            else
            {
                container.MaxCount = datas.Count;
                for (int i = 0; i < container.controlList.Count; ++i)
                {
                    var handle = container.controlList[i];
                    var listener = UIEventListener.Get(handle);
                    if (listener.parameter is K binder)
                    {
                        binder.index = i;
                        binder.BindData(poolHandle, datas[i]);
                    }
                    else
                    {
                        if (null != poolHandle)
                        {
                            listener.parameter = poolHandle.GetSystemClass<K>();
                        }
                        else
                        {
                            listener.parameter = new K();
                        }

                        if (listener.parameter is K binderNew)
                        {
                            binderNew.index = i;
                            binderNew.Setup(listener);
                            binderNew.BindData(poolHandle, datas[i]);
                        }

                    }
                }
            }
        }
    }

    public static void Bind<T, K>(this UIGridContainer container, ILBetterList<T> datas, PoolHandleManager poolHandle = null) where K : UIBinder, new()
    {
        if (null != container)
        {
            if (null == datas)
            {
                container.MaxCount = 0;
            }
            else
            {
                container.MaxCount = datas.Count;
                for (int i = 0; i < container.controlList.Count; ++i)
                {
                    var handle = container.controlList[i];
                    var listener = UIEventListener.Get(handle);
                    if (listener.parameter is K binder)
                    {
                        binder.index = i;
                        binder.BindData(poolHandle, datas[i]);
                    }
                    else
                    {
                        if (null != poolHandle)
                        {
                            listener.parameter = poolHandle.GetSystemClass<K>();
                        }
                        else
                        {
                            listener.parameter = new K();
                        }

                        if (listener.parameter is K binderNew)
                        {
                            binderNew.index = i;
                            binderNew.Setup(listener);
                            binderNew.BindData(poolHandle, datas[i]);
                        }

                    }
                }
            }
        }
    }

    
    public static void UnBind<K>(this UIGridContainer container) where K : UIBinder, new()
    {
        for (int i = 0; i < container.controlList.Count; ++i)
        {
            var handle = container.controlList[i];
            var listener = UIEventListener.Get(handle);
            if (listener.parameter is K binder)
            {
                binder.Destroy();
                listener.parameter = null;
            }
        }
    }

    public static void UpdateWidgetCollider(this UIGridContainer container)
    {
        if(null != container)
        {
            var bc = container.gameObject.GetComponent<BoxCollider>();
            if (null == bc)
            {
                bc = container.gameObject.AddComponent<BoxCollider>();
            }
            NGUITools.UpdateWidgetCollider(container.gameObject);
        }
    }
}