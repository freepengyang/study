using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSPopListData
{
    public string value;
    public int idxValue;
}

public class CSPopList
{
    PoolHandleManager poolHandle;
    public CSPopList ()
    {

    }

    public CSPopList(GameObject handle,PoolHandleManager poolHandle)
    {
        Handle = handle;
        this.poolHandle = poolHandle;
    }

    public void Destroy()
    {
        if(null != mDatas)
        {
            for(int i = 0; i < mDatas.Count; ++i)
            {
                poolHandle.Recycle(mDatas[i]);
            }
            mDatas.Clear();
            poolHandle.Recycle(mDatas);
            mDatas = null;
        }
        Handle = null;
        poolHandle = null;
        mCurValue = null;
        mBtnToggle = null;
        mValue = null;
        mCheck = null;
        mGrid = null;
        mDi = null;
        mBg = null;
        cb = null;
    }

    public GameObject Handle
    {
        get;private set;
    }

    public T Get<T>(Transform parent, string path) where T : UnityEngine.Object
    {
        Transform objTrans = Get(parent,path);

        if (typeof(T) == typeof(Transform)) return objTrans as T;

        if (typeof(T) == typeof(GameObject)) return objTrans.gameObject as T;

        if (objTrans)
            return objTrans.gameObject.GetComponent<T>();
        return null;
    }

    public Transform Get(Transform parent,string _path)
    {
        if (parent == null)
        {
            if (Handle)
            {
                return Handle.transform.Find(_path);
            }
            else
                return null;
        }
        else
            return parent.Find(_path);
    }

    private UIToggle mBtnToggle;
    public UIToggle BtnToggle { get { return mBtnToggle ?? (mBtnToggle = Get<UIToggle>(Handle.transform, "btn")); } }
    private UILabel mValue;
    private UILabel Value { get { return mValue ?? (mValue = Get<UILabel>(Handle.transform, "btn/value")); } }
    private GameObject mCheck;
    private GameObject Check { get { return mCheck ?? (mCheck = Get(Handle.transform, "listPanel/check").gameObject); } }
    private UIGridContainer mGrid;
    private UIGridContainer Grid { get { return mGrid ?? (mGrid = Get<UIGridContainer>(Handle.transform, "listPanel/grid")); } }
    private GameObject mDi;
    private GameObject Di { get { return mDi ?? (mDi = Get(Handle.transform, "listPanel/di").gameObject); } }
    private UISprite mBg;
    private UISprite Bg { get { return mBg ?? (mBg = Get<UISprite>(Handle.transform, "listPanel")); } }
    public System.Action<CSPopListData> cb;
    private CSPopListData mCurValue;
    public CSPopListData CurValue
    {
        set
        {
            if (mCurValue != value)
            {
                mCurValue = value;
                Value.text = value.value;
                if(null != cb)
                {
                    cb(mCurValue);
                }
            }
        }
       get
        {
            return mCurValue;
        }
    }

    public void SetCurValue(int idx,bool markDirty)
    {
        if(idx >= 0 && idx < mDatas.Count)
        {
            var value = mDatas[idx];
            mCurValue = value;
            Value.text = value.value;
            if (markDirty)
                cb?.Invoke(mCurValue);
        }
    }

    public List<CSPopListData> mDatas = null;
    public int MaxCount
    {
        get
        {
            return null == mDatas ? 0 : mDatas.Count;
        }
        set
        {
            if(null == mDatas)
            {
                mDatas = poolHandle.GetSystemClass<List<CSPopListData>>();
            }

            if(mDatas.Count < value)
            {
                for(int i = mDatas.Count; i < value; ++i)
                {
                    mDatas.Add(poolHandle.GetSystemClass<CSPopListData>());
                }
            }
            else if(mDatas.Count > value)
            {
                int n = mDatas.Count;
                for (int i = value; i < n; ++i)
                {
                    poolHandle.Recycle(mDatas[i]);
                }
                mDatas.RemoveRange(value, mDatas.Count - value);
            }
        }
    }

    Color mNormalColor = new Color(0.588f, 0.588f, 0.588f);
    Color mSelectedColor = new Color(0.78f, 0.78f, 0.78f);
    bool mSpecificed = false;

    public void SpecificColor(Color normal,Color selected)
    {
        mSpecificed = true;
        mNormalColor = normal;
        mSelectedColor = selected;
    }

    public void InitList(System.Action<CSPopListData> action,bool specified = false,int maxRaw = 0,bool needLastBg = true)
    {
        mSpecificed = specified;
        cb = action;
        Grid.MaxCount = mDatas.Count;

        if (maxRaw <= 0 || Grid.MaxCount <= maxRaw)
        {
            Bg.width = Mathf.CeilToInt(Mathf.Abs(Grid.transform.localPosition.x * 2));
            Bg.height = Mathf.CeilToInt((Grid.MaxCount - 1) * Mathf.Abs((Grid.CellHeight)) + Mathf.Abs(Grid.transform.localPosition.y * 2));
            Grid.MaxPerLine = 0;
        }
        else
        {
            int coloums = Grid.MaxCount / maxRaw + ((Grid.MaxCount % maxRaw != 0) ? 1 : 0);
            int raws = maxRaw;
            Bg.width = Mathf.CeilToInt(Mathf.Abs(Grid.transform.localPosition.x * 2) + (coloums - 1) * (Grid.CellWidth));
            Bg.height = Mathf.CeilToInt((maxRaw - 1) * Mathf.Abs((Grid.CellHeight)) + Mathf.Abs(Grid.transform.localPosition.y * 2));
            Grid.MaxPerLine = maxRaw;
        }

        for (int i = 0; i < Grid.MaxCount; i++)
        {
            var item = Grid.controlList[i];
            if(null != item)
            {
                if(!needLastBg && i + 1 == Grid.MaxCount)
                {
                    UISprite bg = Get<UISprite>(item.transform, "bg");
                    bg.CustomActive(false);
                }

                var transform = item.transform.Find("value");
                if (transform != null)
                {
                    UILabel value = transform.gameObject.GetComponent<UILabel>();
                    if (null != value)
                    {
                        value.text = mDatas[i].value;
                        var handle = mDatas[i];
                        UIEventListener.Get(item,mDatas[i]).onClick = (go) =>
                        {
                            CurValue = handle;
                            Check.SetActive(true);
                            Check.transform.SetParent(item.transform, false);
                            BtnToggle.value = false;
                        };
                    }
                }
            }
        }
        EventDelegate.Add(BtnToggle.onChange, () =>
        {
            if (BtnToggle.value)
            {
                for (int i = 0; i < Grid.MaxCount; i++)
                {
                    if (Grid.controlList[i] != null)
                    {
                        GameObject item = Grid.controlList[i];
                        UILabel value = item.transform.Find("value").GetComponent<UILabel>();
                        if (CurValue.value == value.text)
                        {
                            if(mSpecificed)
                                value.color = mSelectedColor;
                            Check.SetActive(true);
                            Check.transform.SetParent(item.transform, false);
                            Check.transform.localPosition = Vector3.zero;
                        }
                        else
                        {
                            if (mSpecificed)
                                value.color = mNormalColor;
                        }
                    }
                }
            }
        });
        UIEventListener.Get(Di).onClick = (go) => { DiOnClick(); };

    }


    public void DiOnClick(bool state = false)
    {
        if (BtnToggle != null) BtnToggle.value = state;
    }

}