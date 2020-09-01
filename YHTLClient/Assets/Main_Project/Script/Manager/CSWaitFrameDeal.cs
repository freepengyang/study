using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSWaitFrameDeal : MonoBehaviour
{
    public class Data
    {
        public object param;
        public object data;
        public float waitFrame;
        public long uuid;
        public System.Func<object, object, bool> callBack;
    }
    public float waitFrames = 1;
    private float mLastFrame = 0;
    private CSBetterList<Data> mDataList = new CSBetterList<Data>();
    private CSBetterList<Data> mUselessDataList = new CSBetterList<Data>();
    //public delegate bool OnLoaded(object obj, object param);

    public bool isNeedReset = false;

    public void Release()
    {
        mDataList.Release();
        mUselessDataList.Release();
    }
    public void Add(object t, System.Func<object, object, bool> onload, float waitFrames = 0, object param = null, long uuid = 0)
    {
        if (mDataList.Count == 0)
        {
            this.waitFrames = waitFrames;
            mLastFrame = Time.time;
        }
        Data data = mUselessDataList.Count > 0 ? mUselessDataList[0] : new Data();
        if (mUselessDataList.Count > 0) mUselessDataList.RemoveAt(0);
        data.param = param;
        data.data = t;
        data.waitFrame = waitFrames;
        data.callBack = onload;
        data.uuid = uuid;
        mDataList.Add(data);
    }

    public void Insert(int index, object t, System.Func<object, object, bool> onload, float waitFrames = 0, object param = null, long uuid = 0)
    {
        if (mDataList.Count == 0)
        {
            this.waitFrames = waitFrames;
            mLastFrame = Time.time;
        }
        Data data = mUselessDataList.Count > 0 ? mUselessDataList[0] : new Data();
        if (mUselessDataList.Count > 0) mUselessDataList.RemoveAt(0);
        data.param = param;
        data.data = t;
        data.waitFrame = waitFrames;
        data.callBack = onload;
        data.uuid = uuid;
        mDataList.Insert(index,data);
    }

    public void Remove(object t)
    {
        if (t == null) return;
        for (int i = 0; i < mDataList.Count; i++)
        {
            Data data = mDataList[i];
            if (data == null||data.data == null) continue;
            if (data.data == t)
            {
                mDataList.RemoveAt(i);
                break;
            }
        }
    }

    public void InsertFront(long id)
    {
        if (id == 0) return;
        for (int i = 0; i < mDataList.Count; i++)
        {
            Data data = mDataList[i];
            if (data == null) continue;
            if (data.uuid == id)
            {
                mDataList.RemoveAt(i);
                mDataList.Insert(0, data);
                break;
            }
        }
    }

    void Update()
    {
        Deal();
    }

    void Deal()
    {
        if (mDataList.Count == 0) return;
        if (Time.time < mLastFrame + waitFrames) return;
        mLastFrame = Time.time;
        Data data = mDataList[0];
        mDataList.RemoveAt(0);
        mUselessDataList.Add(data);
        if (data.callBack != null)
        {
            bool isDo = data.callBack(data.data, data.param);
            data.callBack = null;
            if (isNeedReset)
            {
                if (!isDo)
                {
                    mLastFrame = 0;
                    Deal();
                }
            }
        }
    }
}


