using System.Collections.Generic;
using UnityEngine;

public class UICloneMapPoints : IDispose
{
    public GameObject controlTemplate = null; //克隆物体

    public int DelCount = 0; // 循环显示数量，，eg：DelCount=2，每两个显示一个循环间隔显示， DelCount = 3:每3个显示1个

    public List<GameObject> mCurControlList = new List<GameObject>();

    private Queue<GameObject> mCacheList = new Queue<GameObject>();

    private Transform transform;

    private int oldMaxCount = 0;

    private int curMaxCount;

    public int CurMaxCount
    {
        get { return curMaxCount; }
        set
        {
            if (curMaxCount == value) return;
            curMaxCount = value;
            RebuildCells();
        }
    }

    public void Init(Transform parnet, GameObject conObj, int delCount)
    {
        transform = parnet;
        controlTemplate = conObj;
        DelCount = delCount;
        if (controlTemplate != null) controlTemplate.SetActive(false);
    }

    private void RebuildCells()
    {
        if (controlTemplate == null) return;
        oldMaxCount = mCurControlList.Count;
        if (curMaxCount < oldMaxCount)
        {
            for (int i = curMaxCount; i < mCurControlList.Count; i++)
            {
                if (mCurControlList[i] == null)
                {
                    mCurControlList.RemoveAt(i);
                }
                else
                {
                    mCurControlList[i].SetActive(false);
                    mCacheList.Enqueue(mCurControlList[i]);
                    mCurControlList.RemoveAt(i);
                }
                i--;
            }
        }
        else
        {
            for (int i = oldMaxCount; i < curMaxCount; i++)
            {
                if (DelCount != 0 && i % DelCount != 0)
                {
                    mCurControlList.Add(null);
                }
                else
                {
                    GameObject obj = null;
                    if (mCacheList.Count > 0)
                        obj = mCacheList.Dequeue();
                    if (obj == null)
                    {
                        obj = Object.Instantiate(controlTemplate);
                        NGUITools.SetParent(transform, obj);
                    }

                    mCurControlList.Add(obj);
                    if (!obj.activeSelf) obj.SetActive(true);
                }
            }
        }
    }

    public void RemoveSingle()
    {
        if (mCurControlList.Count > 0)
        {
            Remove(0);
        }
    }

    public void RemoveAll()
    {
        while (mCurControlList.Count > 0)
        {
            Remove(0);
        }
    }


    private void Remove(int i)
    {
        if (mCurControlList[i] != null)
        {
            mCurControlList[i].SetActive(false);
            mCacheList.Enqueue(mCurControlList[i]);
        }

        if (mCurControlList.Count > i)
            mCurControlList.RemoveAt(i);
    }

    public void Dispose()
    {
        if (mCurControlList.Count > 0)
        {
            while (mCurControlList.Count > 0)
            {
                GameObject gp = mCurControlList[0];
                mCurControlList.RemoveAt(0);
                if (gp != null) Object.Destroy(gp);
            }
        }

        if (mCacheList.Count > 0)
        {
            while (mCacheList.Count > 0)
            {
                GameObject gp = mCacheList.Dequeue();
                if (gp != null) Object.Destroy(gp);
            }
        }

        controlTemplate = null;
        transform = null;
    }
}