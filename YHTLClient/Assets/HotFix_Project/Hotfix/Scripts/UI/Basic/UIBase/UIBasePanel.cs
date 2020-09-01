using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public partial class UIBasePanel : UIBase
{
    protected List<CSPool.IRelease> mCachedList = null;
    protected int mLastUpdateFrame = -1;
    public int LastUpdateFrame
    {
        get
        {
            return mLastUpdateFrame;
        }
        set
        {
            if(mLastUpdateFrame != value)
            {
                Show();
                mLastUpdateFrame = value;
            }
        }
    }
    public CSPool.Pool<T> GetListPool<T>() where T : class, CSPool.IPoolItem, new()
    {
        if(null == mCachedList)
        {
			mCachedList = mPoolHandleManager.GetSystemClass<List<CSPool.IRelease>>();
		}
        var handle = CSPool.ListPoolHandle.CreateList<T>(mPoolHandleManager);
        mCachedList.Add(handle);
        return handle;
    }

    #region childPanels
    public delegate bool TabFunctionOpened();
    public delegate UIBasePanel DelegateCreateChildPanel(int typeId);
    protected Dictionary<int, DelegateCreateChildPanel> mChildsCreateSlot;
    protected Dictionary<int, UIToggle> mChildToggles;
    protected UIBasePanel mLastSelectedPanel;
    protected UIBasePanel mCurSelectedPanel;
    protected int mLastTypeID = -1;
    public void RegChildPanel<T>(int typeId,GameObject go,UIToggle btnTab,DelegateCreateChildPanel create = null, TabFunctionOpened checkFunction = null) where T : UIBasePanel, new()
    {
        go.CustomActive(false);
        if (typeId == -1)
            return;
        if (null != btnTab)
        {
            if (null == mChildToggles)
                mChildToggles = new Dictionary<int, UIToggle>(8);

            if (!mChildToggles.ContainsKey(typeId))
            {
                mChildToggles.Add(typeId, btnTab);
            }

            EventDelegate.Add(btnTab.onChange, () =>
            {
                if (btnTab.value)
                {
                    if(null != checkFunction && !checkFunction())
                    {
                        mCurSelectedPanel = null;
                        if(mChildToggles.ContainsKey(mLastTypeID))
                            mChildToggles[mLastTypeID].Set(true);
                        return;
                    }
                    mCurSelectedPanel = OpenChildPanel(typeId,true);
                    mLastTypeID = typeId;
                }
            });
        }

        if (null == mChildsCreateSlot)
        {
            mChildsCreateSlot = new Dictionary<int, DelegateCreateChildPanel>(8);
        }

        if (!mChildsCreateSlot.ContainsKey(typeId))
        {
            if(null == create)
            {
                mChildsCreateSlot.Add(typeId, (f) =>
                {
                    T child = new T();
                    child.UIPrefab = go;
                    child.Init();
                    child.Show();
                    child.OnShow(typeId);
                    mClientEvent.SendEvent(CEvent.OpenPanel,child);
                    return child;
                });
            }
            else
            {
                mChildsCreateSlot.Add(typeId,create);
            }
        }
    }

    protected Dictionary<int, UIBasePanel> mChildsPanels;

    public virtual UIBasePanel OpenChildPanel(int type,bool fromToggle = false)
    {
        if (!fromToggle && null != mChildToggles && mChildToggles.ContainsKey(type))
        {
            mChildToggles[type].value = true;
            return mCurSelectedPanel;
        }

        if (null == mChildsPanels)
        {
            mChildsPanels = new Dictionary<int, UIBasePanel>(8);
        }

        DelegateCreateChildPanel actionCreate = null;
        if (null != mChildsCreateSlot && mChildsCreateSlot.ContainsKey(type))
            actionCreate = mChildsCreateSlot[type];
        if (null == actionCreate)
        {
            FNDebug.LogErrorFormat("child has not registed for type = {0}", type);
            return null;
        }

        //hide all other panels
        var it = mChildsPanels.GetEnumerator();
        while (it.MoveNext())
        {
            if (it.Current.Key != type && null != it.Current.Value)
            {
                it.Current.Value.Hide();
            }
        }

        UIBasePanel childPanel = null;
        if (mChildsPanels.ContainsKey(type))
        {
            childPanel = mChildsPanels[type];
            if (null != childPanel)
            {
                childPanel.Show();
                childPanel.OnShow(type);
            }
        }
        else
        {
            childPanel = actionCreate.Invoke(type);
            if (null != childPanel)
            {
                mChildsPanels.Add(type, childPanel);
            }
        }

        return childPanel;
    }
    #endregion

    #region Head Moneys
    bool mHasPushed = false;
    protected void SetMoneyIds(params int[] moneyIds)
    {
        if(!(null != moneyIds && moneyIds.Length > 0 && moneyIds.Length <= 4))
        {
            FNDebug.LogError("aruments error");
            return;
        }

        if(mHasPushed)
        {
            CSMoneyInfo.Instance.Pop();            
        }

        if (moneyIds.Length == 1)
        {
            CSMoneyInfo.Instance.Push(moneyIds[0]);
        }
        else if(moneyIds.Length == 2)
        {
            CSMoneyInfo.Instance.Push(moneyIds[0], moneyIds[1]);
        }
        else if(moneyIds.Length >= 3)
        {
            CSMoneyInfo.Instance.Push(moneyIds[0], moneyIds[1], moneyIds[2]);
        }

        mHasPushed = true;
    }
    #endregion

    #region  小红点
    private List<GameObject> _RedPointAddObj;
    public void RegisterRed(GameObject go, RedPointType pointType)
    {
        if(_RedPointAddObj == null) _RedPointAddObj = new List<GameObject>();
        _RedPointAddObj.Add(go);
        CSRedPointManager.Instance.RegisterRedPoint(go,pointType);
    }
    public void RegisterRedList(GameObject go, params RedPointType[] pointType)
    {
        if(_RedPointAddObj == null) _RedPointAddObj = new List<GameObject>();
        _RedPointAddObj.Add(go);
        CSRedPointManager.Instance.RegisterRedPoint(go,pointType);
    }
    #endregion

    #region bindAsync co
    Dictionary<int,IEnumerator> mBindAnsycDic;
    public void BindCoroutine(int functionId,IEnumerator enumerator)
    {
        if(!ScriptBinder.gameObject.activeInHierarchy)
        {
            FNDebug.LogWarning($"[携程启动失败]:[{functionId}]");
            return;
        }

        if (null == mBindAnsycDic)
            mBindAnsycDic = new Dictionary<int, IEnumerator>(8);
        if (mBindAnsycDic.ContainsKey(functionId))
        {
            ScriptBinder.StopCoroutine(mBindAnsycDic[functionId]);
            mBindAnsycDic.Remove(functionId);
        }
        mBindAnsycDic.Add(functionId,enumerator);
        ScriptBinder.StartCoroutine(enumerator);
    }
    #endregion
    //#region Timer
    //FastArrayElementKeepHandle<TimerEventHandle> mTimers;
    //const int maxTimer = 8;
    //protected void AddTimer(int id,int interval,System.Action action, TimerType eTimer = TimerType.ONCE)
    //{
    //    if(!(id >= 1 && id <= maxTimer))
    //    {
    //        Debug.LogErrorFormat("定时器只支持:最多[{0}]个,不要特意去改这个值,ID范围[1,{0}]", maxTimer);
    //        return;
    //    }

    //    if (null == mTimers)
    //    {
    //        mTimers = new FastArrayElementKeepHandle<TimerEventHandle>(maxTimer);
    //        for(int i = 0; i < maxTimer; ++i)
    //        {
    //            mTimers.Append(null);
    //        }
    //    }

    //    TimerEventHandle timer = mTimers[id - 1];
    //    if(null != timer)
    //    {
    //        CSTimer.Instance.remove_timer(timer);
    //    }
    //    mTimers[id - 1] = CSTimer.Instance.Invoke(interval, action, eTimer);
    //}
    //void StopAllTimers()
    //{
    //    if(null != mTimers)
    //    {
    //        for(int i = 0,max = mTimers.Count;i < max;++i)
    //        {
    //            var timer = mTimers[i];
    //            if(null != timer)
    //            {
    //                CSTimer.Instance.remove_timer(timer);
    //            }
    //        }
    //        mTimers.Clear();
    //        mTimers = null;
    //    }
    //}
    //#endregion

    public void SetVisible(bool visible)
    {
        if(null != UIPrefab)
        {
            //Debug.LogFormat("<color=#00ff00>[新手引导]:[name:{0}]:[visible:{1}]</color>", UIPrefab.gameObject.name, visible);
            if(visible != UIPrefab.gameObject.activeSelf)
                UIPrefab.gameObject.SetActive(visible);
        }
    }

    public override void SelectChildPanel(int type = 1)
    {
        OpenChildPanel(type);
    }

    public override void SelectChildPanel(int type, int subType)
    {
        OpenChildPanel(type, false)?.OpenChildPanel(subType);
    }

    public override void SelectChildPanel(int type, params object[] obj)
    {
        
    }

    public override void Show()
    {
        base.Show();

        if (null != UIPrefab && !UIPrefab.gameObject.activeSelf)
            UIPrefab.gameObject.SetActive(true);

        if (null != ScriptBinder && ScriptBinder.moneyIds.Length > 0)
        {
            SetMoneyIds(ScriptBinder.moneyIds);
        }

        if(null != mCurSelectedPanel)
            mCurSelectedPanel.LastUpdateFrame = Time.frameCount;
        //if (mLastSelectedPanel != null && mCurSelectedPanel != mLastSelectedPanel)
        //    mCurSelectedPanel?.Show();
        //mLastSelectedPanel = mCurSelectedPanel;
    }

    public virtual void OnShow(int typeId=0)
    {

    }

    public virtual void OnHide()
    {

    }
    public virtual void SelectItem(TipsBtnData _data)
    {

    }
    protected void Hide(GameObject go)
    {
        Hide();
    }

    public void Hide()
    {
        if (null != ScriptBinder && ScriptBinder.gameObject.activeSelf)
        {
            ScriptBinder.gameObject.SetActive(false);
        }
        OnHide();
    }

    protected void Show(GameObject go)
    {
        if (null != ScriptBinder)
        {
            ScriptBinder.gameObject.SetActive(true);
        }
    }

    protected void ReverseVisible(GameObject go)
    {
        if (null != ScriptBinder)
        {
            ScriptBinder.gameObject.SetActive(!ScriptBinder.gameObject.activeSelf);
        }
    }

    protected bool destroyed = false;
    protected override void OnDestroy()
    {
        if (destroyed)
            return;
        destroyed = true;

        //StopAllTimers();

        if (mHasPushed)
        {
            mHasPushed = false;
            CSMoneyInfo.Instance.Pop();
        }

        if (null != mChildsPanels)
        {
            var it = mChildsPanels.GetEnumerator();
            while(it.MoveNext())
            {
                it.Current.Value.Destroy();
            }
            mChildsPanels.Clear();
            mChildsPanels = null;
        }

        if(null != mChildToggles)
        {
            mChildToggles.Clear();
            mChildToggles = null;
        }

        if (null != mChildsCreateSlot)
        {
            mChildsCreateSlot.Clear();
            mChildsCreateSlot = null;
        }

        if(null != mCachedList)
        {
            for (int i = 0; i < mCachedList.Count; ++i)
            {
                CSPool.ListPoolHandle.DestroyList(mCachedList[i]);
                mPoolHandleManager.Recycle(mCachedList[i]);
            }
            mCachedList.Clear();
            mPoolHandleManager.Recycle(mCachedList);
            mCachedList = null;
        }

        if (_RedPointAddObj != null)
        {
            for (int i = 0; i < _RedPointAddObj.Count; i++)
            {
                CSRedPointManager.Instance.Recycle(_RedPointAddObj[i]);
            }
        }

        mLastSelectedPanel = null;
        mCurSelectedPanel = null;

        mBindAnsycDic?.Clear();
        mBindAnsycDic = null;
        base.OnDestroy();
    }
}