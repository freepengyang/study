using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    [SerializeField]
    protected Transform mTarget;
    [SerializeField]
    protected GameObject mTemplate;//飞行模型

    [SerializeField]
    [Range(0, 1)]
    protected float midRadio = 0.52f;
    [SerializeField]
    [Range(-1.0f, 1.0f)]
    protected float mXOffset = 0.41f;
    [SerializeField]
    [Range(0.1f, 5)]
    protected float mFlyTime = 0.6f;

    public void OnEnable()
    {
        //AddFly(new Vector3(12.5f, 13.5f, 0.0f),30030033, null, null, 1.3f);
        this.onEnable?.Invoke();
    }

    public void SetEnableCallback(System.Action onEnable)
    {
        this.onEnable = onEnable;
    }

    PickItem CloneOrGetFromPool()
    {
        Transform parent = mTemplate.transform.parent;
        if(mPickedObjectPools.Count <= 0)
        {
            GameObject handle = UnityEngine.Object.Instantiate(mTemplate);
            if (parent != null)
            {
                Transform t = handle.transform;
                t.parent = parent;
                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;
                handle.layer = parent.gameObject.layer;
            }

            var pickItem = new PickItem();
            pickItem.Handle = handle;
            return pickItem;
        }
        else
        {
            return mPickedObjectPools.Pop();
        }
    }

    System.Action onEnable;
    Stack<PickItem> mPickedObjectPools = new Stack<PickItem>(32);
    List<PickItem> mActivedPickItems = new List<PickItem>(32);

    class PickItem
    {
        public GameObject Handle { get; set; }
        public void Init(Vector3 start)
        {
            this.startPos = start;
            Handle.transform.position = start;
        }

        public void Active(bool value)
        {
            if (null != Handle && Handle.activeSelf != value)
                Handle.SetActive(value);
        }

        public void SetResult(bool succeed)
        {
            var action = succeed ? onSucceed : onFailed;
            action?.Invoke();
            onSucceed = null;
            onFailed = null;
        }

        // 二阶曲线
        Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            Vector3 p0p1 = (1 - t) * p0 + t * p1;
            Vector3 p1p2 = (1 - t) * p1 + t * p2;
            Vector3 result = (1 - t) * p0p1 + t * p1p2;
            return result;
        }

        public bool Update(float now)
        {
            var transform = Handle.transform;
            if (startTime + length <= now)
            {
                transform.position = targetPos;
                SetResult(true);
                Active(false);
                return false;
            }

            float deltaTime = Mathf.Clamp01((now - startTime) / length);
            Vector3 current = Bezier(startPos, midPos, targetPos, deltaTime);
            transform.position = current;
            return true;
        }

        public System.Action onFailed;
        public System.Action onSucceed;
        public float startTime;
        public float length;
        public Vector3 startPos;
        public Vector3 targetPos;
        public Vector3 midPos;
    }

    public void Pick(Vector3 startWorldPos,System.Action<GameObject> onCreate,float length = 1.3f,System.Action onFailed = null, System.Action onSucceed = null)
    {
        if (null == Camera.main)
        {
            Debug.LogError("[Item pick failed MainCamera not exist ...]");
            onFailed?.Invoke();
            return;
        }

        if (null == mTarget)
        {
            Debug.LogError("[Item pick failed ctrl has not target seted ...]");
            onFailed?.Invoke();
            return;
        }

        if (null == UICamera.mainCamera)
        {
            Debug.LogError("[Item pick failed uiCamera not found]");
            onFailed?.Invoke();
            return;
        }

        var pickItem = CloneOrGetFromPool();
        if(null == pickItem)
        {
            Debug.LogError("[Item pick failed clone template failed]");
            return;
        }

        var screenStartPos = Camera.main.WorldToScreenPoint(startWorldPos);
        screenStartPos.z = 0.0f;
        var uiWorldPos = UICamera.mainCamera.ScreenToWorldPoint(screenStartPos);

        pickItem.Active(true);
        pickItem.Init(uiWorldPos);
        pickItem.onFailed = onFailed;
        pickItem.onSucceed = onSucceed;
        pickItem.length = length;
        pickItem.startTime = Time.time;
        pickItem.targetPos = mTarget.transform.position;

        bool left = true;
        float radio = left ? 1 : -1;
        //人物头顶点
        pickItem.midPos = pickItem.startPos + (pickItem.targetPos - pickItem.startPos).normalized * (pickItem.targetPos - pickItem.startPos).magnitude * midRadio + new Vector3(UnityEngine.Random.Range(0.4f, mXOffset) * radio, 0.0f, 0.0f);

        onCreate?.Invoke(pickItem.Handle);
        mActivedPickItems.Add(pickItem);

        if (!this.enabled)
            this.enabled = true;
    }

	public void Pick(Vector3 startScreenPos,Vector3 targetScreenPos, System.Action<GameObject> onCreate, float length = 1.3f, System.Action onFailed = null, System.Action onSucceed = null)
	{
		var pickItem = CloneOrGetFromPool();
		if (null == pickItem)
		{
			Debug.LogError("[Item pick failed clone template failed]");
			return;
		}

		pickItem.Active(true);
		pickItem.Init(startScreenPos);
		pickItem.onFailed = onFailed;
		pickItem.onSucceed = onSucceed;
		pickItem.length = length;
		pickItem.startTime = Time.time;
		pickItem.targetPos = targetScreenPos;

		onCreate?.Invoke(pickItem.Handle);
		mActivedPickItems.Add(pickItem);

		if (!this.enabled)
			this.enabled = true;
	}

	private void Update()
    {
        float now = Time.time;
        for(int i = 0; i < mActivedPickItems.Count; ++i)
        {
            var item = mActivedPickItems[i];
            if(!item.Update(now))
            {
                mPickedObjectPools.Push(item);
                mActivedPickItems.RemoveAt(i--);
                continue;
            }
        }

        if (mActivedPickItems.Count <= 0)
            enabled = false;
    }
}