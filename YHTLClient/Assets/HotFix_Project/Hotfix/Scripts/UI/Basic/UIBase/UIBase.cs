using UnityEngine;
using System;
using Object = UnityEngine.Object;

public class UIBase
{
	private ScriptBinder _binder;
    public ScriptBinder ScriptBinder 
	{
		get 
		{
			if (null != _binder)
				return _binder;
			_binder = UIPrefabTrans.GetComponent<ScriptBinder>();
			if (null == _binder)
				_binder = UIPrefabTrans.gameObject.AddComponent<ScriptBinder>();
			return _binder;
		} 
	}

	protected EventHanlderManager mClientEvent;
	protected PoolHandleManager mPoolHandleManager;
	public bool Disposed
	{
		get;private set;
	}


	public UIBase()
	{
		Disposed = false;
		name = this.GetType().Name;
		mClientEvent = new EventHanlderManager(EventHanlderManager.DispatchType.Event);
		mPoolHandleManager = new PoolHandleManager(32,this);
	}

	protected bool mSpecial = false;

	public bool Special
	{
		get { return mSpecial; }
	}

	/// <summary>
	/// 面板层级
	/// </summary>
	protected UILayerType mPanelLayerType = UILayerType.Window;

	public virtual UILayerType PanelLayerType
	{
		get { return mPanelLayerType; }
	}

	/// <summary>
	/// 脚本名字
	/// </summary>
	public string name { get; set; }

	/// <summary>
	/// 是否缓存面板
	/// </summary>
	private bool mCached = false;

	public virtual bool Cached
	{
		get { return mCached; }
	}

	/// <summary>
	/// 是否背景模糊
	/// </summary>
	public virtual bool ShowGaussianBlur
	{
		get { return true; }
	}

	//面板层级
	public int mPanelDeep = 0;

	private GameObject mUIPrefab;

	public GameObject UIPrefab
	{
		get { return mUIPrefab; }
		set { mUIPrefab = value; }
	}

	private Transform mUIPrefabTrans;

	public Transform UIPrefabTrans
	{
		get
		{
			if (mUIPrefabTrans == null)
				mUIPrefabTrans = UIPrefab.transform;
			return mUIPrefabTrans;
		}
		set { mUIPrefabTrans = value; }
	}
	
	/// <summary>
	/// 面板打开动画
	/// </summary>
	public virtual PrefabTweenType PanelTweenType { get; protected set; }

	private UIPanel mPanel;

	public UIPanel Panel
	{
		get { return mPanel ? mPanel : (mPanel = UIPrefab.GetComponent<UIPanel>()); }
	}

	/// <summary>
	/// 不要重写此方法,自己调用时不要调用OnDestroy
	/// </summary>
	public void Destroy()
	{
		if(this.Disposed)
		{
			return;
		}
		Disposed = true;
		CSGuideManager.Instance.Remove(this);
		OnDestroy();
		//销毁的时候需清空对象
		if (mClientEvent != null) mClientEvent.UnRegAll();
		if (mPoolHandleManager != null) mPoolHandleManager.RecycleAll();
		mClientEvent = null;
#if MEMORY_TRACE
		mPoolHandleManager.TryReportPoolBalance();
#endif
		mPoolHandleManager = null;
		if (null != _binder)
        {
            _binder.DestroyWithFrame();
            _binder = null;
        }
		if(mUIPrefab)
		{
			Object.Destroy(mUIPrefab);
			mUIPrefab = null;
		}
	}

	/// <summary>
	/// 销毁时，重写改方法
	/// </summary>
	protected virtual void OnDestroy()
	{
	}
	
	/// <summary>
	/// 当对象需要缓存时，在Dispose中，调用该方法
	/// </summary>
	public virtual void Dispose()
	{
		if (mClientEvent != null) mClientEvent.UnRegAll();
		mClientEvent = null;
		if (mPoolHandleManager != null) mPoolHandleManager.RecycleAll();
		mPoolHandleManager = null;
		UIPrefab = null;
		UIPrefabTrans = null;
	}

	public virtual void Init()
	{
		if (Panel) Panel.alpha = 0;
		_InitScriptBinder();
		CSGuideManager.Instance.Register(this);
		//Timer.Instance.Invoke()
    }

	protected virtual void _InitScriptBinder()
    {

    }

    public virtual void Show()
	{
		if(mUIPrefab) mUIPrefab.CustomActive(true);
		if (Panel) Panel.alpha = 1;
	}
    
    public virtual void Conceal()
    {
	    if (Panel) Panel.alpha = 0;
    }

	public T Get<T>(string path, Transform parent = null) where T : UnityEngine.Object
	{
		if (parent == null)
		{
			parent = UIPrefabTrans;
		}
		return UtilityObj.GetObject<T>(parent, path);
	}

	public Transform Get(string _path, Transform parent = null)
	{
		if (parent == null)
		{
			parent = UIPrefabTrans;
		}

		return UtilityObj.Get(_path, parent);
	}

	public virtual void OnRecycle()
	{
		ScriptBinder.CancelInvoke();
		mUIPrefab.CustomActive(false);
	}


	public void AddCollider(GameObject go)
	{
		Collider col = go.GetComponent<Collider>();
		BoxCollider box = col as BoxCollider;
		if (box == null)
		{
			if (col != null)
			{
				if (Application.isPlaying) Object.Destroy(col);
				else Object.DestroyImmediate(col);
			}

			box = go.AddComponent<BoxCollider>();
		}

		box.center = new Vector3(0, 0, 10);
		box.size = new Vector3(2000, 1000);

		UIEventListener.Get(box.gameObject).onClick = Close;
	}

	public void AddCollider()
	{
		if (mUIPrefab)
			AddCollider(mUIPrefab);
	}

	protected void Close(GameObject go)
	{
		Close();
	}
	
	protected virtual void Close()
	{
		UIManager.Instance.ClosePanel(this.GetType(), true);
	}


	//对于子界面打开选择
	public virtual void SelectChildPanel(int type = 1)
	{
	}

	public virtual void SelectChildPanel(int type, int subType)
	{
	}

    public virtual void SelectChildPanel(int type, params object[] obj)
    {
    }

    public virtual void RefreshData(params object[] obj)
	{
	}
}