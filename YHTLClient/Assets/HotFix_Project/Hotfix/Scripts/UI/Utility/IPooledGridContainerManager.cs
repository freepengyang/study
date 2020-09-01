using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRecycle
{
    void OnRecycle();
}

public interface IMonoSingletonRegister
{
    MonoSingletonRegister monoSingletonRegister { get; set; }
    void Awake();
    void Start();
    void Destroy();
}

public static class MonoSingletonRegisterExtend
{
    public static void Register<T>(this T monoSingleton, Transform parent) where T : class, IMonoSingletonRegister, new()
    {
        if(null != monoSingleton)
        {
            monoSingleton.monoSingletonRegister = MonoSingletonRegister.Register(monoSingleton.GetType().Name, monoSingleton.Awake, monoSingleton.Start, monoSingleton.Destroy);
            monoSingleton.monoSingletonRegister.CachedTransform.SetParent(parent);
        }
    }
}

public abstract class IPooledGridContainerManager<IComponent,IData,T> : IMonoSingletonRegister
    where T : IPooledGridContainerManager<IComponent, IData,T>,new()
    where IComponent : UIBinder, IRecycle,new() 
    where IData : class,new()
{
    protected List<IComponent> mPooledComponents = new List<IComponent>(32);
    protected abstract string GetPrefabName();
    protected System.WeakReference<GameObject> weakHandle;
    protected List<IData> mPooledDatas = new List<IData>(32);

    public void Bind(UIGridContainer container, FastArrayElementKeepHandle<IData> datas)
    {
        if (null != container)
        {
            //创建一个模板
            if (null == container.controlTemplate && datas.Count > 0)
            {
                container.controlTemplate = Create(container.gameObject).Handle.gameObject;
            }
            //回收模板
            container.MaxCount = 0;
            //向缓冲区压入预制体
            if (container.RestoreList.Count < datas.Count)
            {
                int cnt = datas.Count - container.RestoreList.Count;
                for (int i = 0; i < cnt; ++i)
                {
                    var itemBar = Create(container.gameObject);
                    container.RestoreList.Add(itemBar.Handle.gameObject);
                }
            }
            //生成预制体
            container.MaxCount = datas.Count;
            int n = container.controlList.Count;
            for (int i = 0; i < n; ++i)
            {
                var p = UIEventListener.Get(container.controlList[i]).parameter as IComponent;
                p.BindData(null, datas[i]);
            }
        }
    }

    public void Bind(UIGridContainer container, List<IData> datas)
    {
        if (null != container)
        {
            //创建一个模板
            if (null == container.controlTemplate && datas.Count > 0)
            {
                container.controlTemplate = Create(container.gameObject).Handle.gameObject;
            }
            //回收模板
            container.MaxCount = 0;
            //向缓冲区压入预制体
            if (container.RestoreList.Count < datas.Count)
            {
                int cnt = datas.Count - container.RestoreList.Count;
                for (int i = 0; i < cnt; ++i)
                {
                    var itemBar = Create(container.gameObject);
                    container.RestoreList.Add(itemBar.Handle.gameObject);
                }
            }
            //生成预制体
            container.MaxCount = datas.Count;
            int n = container.controlList.Count;
            for (int i = 0; i < n; ++i)
            {
                var p = UIEventListener.Get(container.controlList[i]).parameter as IComponent;
                p.BindData(null,datas[i]);
            }
        }
    }

    public void UnBind(UIGridContainer container)
    {
        if (null != container)
        {
            int n = container.controlList.Count;
            for (int i = 0; i < n; ++i)
            {
                Recycle(container.controlList[i]);
            }
            container.controlList.Clear();
            n = container.RestoreList.Count;
            for (int i = 0; i < n; ++i)
            {
                Recycle(container.RestoreList[i]);
            }
            container.RestoreList.Clear();
            if (null != container.controlTemplate)
            {
                Recycle(container.controlTemplate);
                container.controlTemplate = null;
            }
        }
    }

    public virtual IData Get()
    {
        IData ret = null;
        if (mPooledDatas.Count > 0)
        {
            ret = mPooledDatas[0];
            mPooledDatas.RemoveAt(0);
        }
        else
        {
            ret = new IData();
        }
        return ret;
    }

    public void Put(IData data)
    {
        if (null != data)
        {
            if(!mPooledDatas.Contains(data))
            {
                mPooledDatas.Add(data);
            }
            else
            {
                FNDebug.LogError("recycle repeated ...");
            }
        }
    }

    public IComponent Create(GameObject parent)
    {
        IComponent handle = null;
        if (null != parent)
        {
            if (mPooledComponents.Count > 0)
            {
                handle = mPooledComponents[0];
                mPooledComponents.RemoveAt(0);
            }
            else
            {
                GameObject obj = null;
                if (null == weakHandle || !weakHandle.TryGetTarget(out obj))
                {
                    bool isMiniLoad = CSGame.Sington.IsMiniApp && !UIManager.Instance.ignoreABList.Contains(GetPrefabName());
                    obj = AssetBundleLoad.LoadUIAsset(GetPrefabName(), isMiniLoad);
                    if (null != obj)
                    {
                        if (null == weakHandle)
                        {
                            weakHandle = new System.WeakReference<GameObject>(obj);
                        }
                        else
                        {
                            weakHandle.SetTarget(obj);
                        }
                    }
                }

                if (null != obj)
                {
                    GameObject inst = GameObject.Instantiate(obj) as GameObject;
                    if (null != inst)
                    {
                        var listener = UIEventListener.Get(inst);
                        handle = new IComponent();
                        handle.Setup(listener);
                    }
                }
            }
            if (null != handle)
            {
                handle.Handle.transform.SetParent(parent.transform);
                handle.Handle.transform.localPosition = Vector3.zero;
                handle.Handle.transform.localScale = Vector3.one;
                if (!handle.Handle.gameObject.activeSelf)
                {
                    handle.Handle.gameObject.SetActive(true);
                }
            }
        }
        return handle;
    }

    public void Recycle(GameObject go)
    {
        var itemBar = UIEventListener.Get(go).parameter as IComponent;
        if(null != itemBar)
        {
            itemBar.OnRecycle();
            mPooledComponents.Add(itemBar);
            go.transform.SetParent(cachedTransform);
            go.transform.localPosition = Vector3.zero;
            go.SetActive(false);
        }
    }

    void Clear()
    {
        for(int i = 0; i < mPooledComponents.Count; ++i)
        {
            Object.Destroy(mPooledComponents[i].Handle.gameObject);
        }
        mPooledComponents.Clear();
        mPooledDatas.Clear();
        weakHandle = null;
        monoSingletonRegister = null;
    }

    public MonoSingletonRegister monoSingletonRegister { get; set; }

    public Transform cachedTransform
    {
        get
        {
            return monoSingletonRegister.CachedTransform;
        }
    }

    static T ms_instance = null;
    public static T Instance 
    {
        get
        {
            return ms_instance;
        }
        set
        {
            ms_instance = value;
        }
    }

    public void Awake()
    {

    }

    public void Start()
    {
        cachedTransform.transform.localPosition = new Vector3(-10000, -10000, 0);
    }

    public void Destroy()
    {
        Clear();
        ms_instance = null;
    }

    public static void CreateInstance(Transform parent)
    {
        if (ms_instance != null)
            return;
        ms_instance = new T();
        ms_instance.Register(parent);
    }
}