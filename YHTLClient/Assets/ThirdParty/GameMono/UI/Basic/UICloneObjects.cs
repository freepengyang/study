//----------------------------------------------
//            Author: JiaBao
//            Time :  2016.5.4
//----------------------------------------------
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class UICloneObjects : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        if (controlTemplate != null)
        {
            if (controlTemplate.activeSelf)
            {
                controlTemplate.SetActive(false);
            }
        }
    }

    //克隆物体
    public GameObject controlTemplate = null;
    //当前最新个数
    private int curMaxCount = 0;
    //保存老的个数
    private int oldMaxCount = 0;

    [HideInInspector]
    [SerializeField]
    public List<GameObject> mCurControlList = new List<GameObject>();

    public int CurMaxCount
    {
        get { return curMaxCount; }
        set
        {
            if (curMaxCount == value)
            {
                return;
            }

            curMaxCount = value;
            RebuildCells();
        }
    }

    private void RebuildCells()
    {
        if (controlTemplate == null)
        {
            return;
        }

        oldMaxCount = mCurControlList.Count;

        if (curMaxCount < oldMaxCount)
        {
            for (int i = curMaxCount; i < oldMaxCount; i++)
            {
                GameObject g = mCurControlList[curMaxCount];
                mCurControlList.RemoveAt(curMaxCount);
                if (g == null) { continue; }
                NGUITools.Destroy(g);
                g = null;
            }
        }
        else
        {
            for (int i = oldMaxCount; i < curMaxCount; i++)
            {
                GameObject c = GameObject.Instantiate(controlTemplate) as GameObject;
                NGUITools.SetParent(this.transform, c);
                mCurControlList.Add(c);
                c.SetActive(true);
            }
        }

    }

    private void OnDestroy()
    {
        if (mCurControlList.Count > 0)
        {
            for (int i = 0; i > mCurControlList.Count; i++)
            {
                GameObject.DestroyImmediate(mCurControlList[i]);
            }
        }

        mCurControlList.Clear();

        controlTemplate = null;
    }

    public void Destroy()
    {
        if (mCurControlList.Count > 0)
        {
            GameObject gp = mCurControlList[0];
            mCurControlList.RemoveAt(0);
            GameObject.Destroy(gp);
        }
    }

    public void DestroyAll()
    {
        if (mCurControlList.Count > 0)
        {
            while (mCurControlList.Count>0)
            {
                GameObject gp = mCurControlList[0];
                mCurControlList.RemoveAt(0);
                GameObject.Destroy(gp);
            }
        }
    }
}
