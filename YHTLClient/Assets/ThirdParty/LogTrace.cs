using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogTrace : MonoBehaviour
{
    public bool EnableLog = true;
    [SerializeField]
    protected UIScrollView scrollView;
    [SerializeField]
    protected UIWrapContent wrapContent;
    [SerializeField]
    protected UITexture mask;
    [SerializeField]
    protected BoxCollider boxCollider;
    [SerializeField]
    protected UIEventListener eventListener;
    [SerializeField]
    protected UILabel btnLockText;
    [SerializeField]
    protected UIEventListener btnLock;
    [SerializeField]
    protected UIEventListener btnAdd;
    [SerializeField]
    protected UIEventListener btnMinus;

    class LogItem
    {
        public bool dirty = false;
        string value = string.Empty;
        public string Value
        {
            get
            {
                if (dirty)
                {
                    dirty = false;
                    value = $"[{logType}]:[{condition}]:[{stackTrace}]";
                }
                return value;
            }
        }
        public string condition;
        public string stackTrace;
        public LogType logType;
    }

    bool locked = false;
    bool Locked
    {
        get
        {
            return locked;
        }
        set
        {
            locked = value;
            btnLockText.text = locked ? "取消锁定" : "锁定日志";
            mLogDirty = true;
        }
    }

    LogItem[] mLogPools = new LogItem[1024];
    int mCnt = 0;
    int mCursor = 0;
    int mStartPos = 0;
    LogItem Get()
    {
        LogItem logItem = null;
        if (mCnt < mLogPools.Length)
        {
            logItem = new LogItem();
            mLogPools[mCnt] = logItem;
            mCursor = ++mCnt;
            mStartPos = 0;
            return logItem;
        }
        else
        {
            mStartPos += 1;
            if (mStartPos == mLogPools.Length)
                mStartPos = 0;

            if (mCursor == mLogPools.Length)
                mCursor = 0;
            else
                mCursor += 1;
            logItem = mLogPools[mCursor];
        }
        return logItem;
    }
    bool mLogDirty = false;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (EnableLog)
            Application.logMessageReceivedThreaded += CaptureLog;

        wrapContent.onInitializeItem = OnInitLogItems;

        eventListener.onDoubleClick += this.OnDBClicked;
        btnAdd.onClick += OnAdd;
        btnMinus.onClick += OnMinus;
    }

    void OnAdd(GameObject go)
    {
        wrapContent.itemSize += 20;
        mLogDirty = true;
    }

    void OnMinus(GameObject go)
    {
        wrapContent.itemSize -= 20;
        mLogDirty = true;
    }

    void OnDBClicked(GameObject gameObject)
    {
        bool actived = scrollView.gameObject.activeSelf;
        scrollView.gameObject.SetActive(!actived);
        boxCollider.enabled = !actived;
        mask.enabled = !actived;
        btnLock.gameObject.SetActive(!actived);
        btnAdd.gameObject.SetActive(!actived);
        btnMinus.gameObject.SetActive(!actived);
    }

    void OnInitLogItems(GameObject go, int wrapIndex, int realIndex)
    {
        int idx = (mStartPos - realIndex) % mLogPools.Length;
        if(idx >= 0 && idx < mCnt)
        {
            go.transform.Find("lb_1").GetComponent<UILabel>().text = mLogPools[idx].Value;
            go.SetActive(true);
        }
        else
        {
            go.SetActive(false);
        }
    }

    private void Update()
    {
        if (!mLogDirty)
            return;

        mLogDirty = false;
        //wrapContent.enabled = false;
        wrapContent.minIndex = -mCnt + 1;
        wrapContent.maxIndex = 0;
        wrapContent.cullContent = false;
        wrapContent.SortBasedOnScrollMovement();
        //wrapContent.enabled = true;
    }

    void CaptureLog(string condition, string stackTrace, LogType type)
    {
        LogItem logItem = Get();
        logItem.condition = condition;
        logItem.stackTrace = stackTrace;
        logItem.logType = type;
        logItem.dirty = true;

        mLogDirty = true;
    }

    private void OnDestroy()
    {
        btnAdd.onClick -= OnAdd;
        btnMinus.onClick -= OnMinus;
        eventListener.onDoubleClick -= this.OnDBClicked;
        wrapContent.onInitializeItem = null;
        if (EnableLog)
            Application.logMessageReceivedThreaded -= CaptureLog;
    }
}
