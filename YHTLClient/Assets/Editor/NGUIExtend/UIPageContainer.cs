﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Page Container")]
public class UIPageContainer : MonoBehaviour
{
    public delegate void OnReposition();

    public enum Arrangement
    {
        Horizontal,
        Vertical,
    }

    public enum Sorting
    {
        None,
        Alphabetic,
        Horizontal,
        Vertical,
        Custom,
    }

    public OnReposition onReposition;

    [HideInInspector]
    [SerializeField]
    private Arrangement _arrangement = Arrangement.Horizontal;

    public UIWidget.Pivot pivot = UIWidget.Pivot.TopLeft;
    public Sorting sorting = Sorting.None;
    [HideInInspector]
    [SerializeField]
    private int maxPerLine = 0;
    [HideInInspector]
    [SerializeField]
    private float cellWidth = 200f;
    [HideInInspector]
    [SerializeField]
    private float cellHeight = 200f;
    protected UIPanel mPanel;
    [HideInInspector]
    [SerializeField]
    private int maxCount = 0;
    private int oldMaxCount = 0;

    [HideInInspector]
    [SerializeField]
    private int maxPage = 0;//最多几页

    public Transform[] pages;       //页列表
    public int perPageItemNumber;   //每一页最多显示物体的数量

    public GameObject controlTemplate;
    public float cellOffsetX=0;
    public float cellOffsetY = 0;

    [Tooltip("底部显示的toggle切换列表->从零开始eg:Panel#0")]
    public UIToggle[] togglePointList;

    private UICenterOnChild mCenterOnChild;
    private UICenterOnChild ConterOnChild { get { return mCenterOnChild ?? (mCenterOnChild =transform.GetComponent<UICenterOnChild>()); } }

    public int MaxPerLine
    {
        get { return maxPerLine; }
        set
        {
            if (maxPerLine == value)
                return;
            maxPerLine = value;
            RebuildCells();
        }
    }

    public float CellWidth
    {
        get { return cellWidth; }
        set
        {
            if (cellWidth == value)
                return;
            cellWidth = (int)value;
            RebuildCells();
        }
    }
    public float CellHeight
    {
        get { return cellHeight; }
        set
        {
            if (cellHeight == value)
                return;
            cellHeight = (int)value;
            RebuildCells();
        }
    }

    public int MaxPageCount
    {
        get { return maxPage; }
        set
        {
            maxPage = value;
        }
    }

    public int MaxCount
    {
        get { return maxCount; }
        set
        {
            if (maxCount == value)
            {
                return;
            }
            maxCount = value;
            RebuildCells();
        }
    }

    public Arrangement arrangement
    {
        get { return _arrangement; }
        set
        {
            if (_arrangement == value) return;
            _arrangement = value;
            RebuildCells();
        }
    }

    public List<GameObject> controlList = new List<GameObject>();
    private CSBetterList<GameObject> mRestoreList = new CSBetterList<GameObject>();
    bool mStarted = false;

    void Start()
    {
        if (ConterOnChild != null) ConterOnChild.onCenter = TogglePoint;
        if (controlTemplate != null) controlTemplate.SetActive(!Application.isPlaying);
        if(mStarted == false)
        {
            mStarted = true;
            RebuildCells();
        }
    }

    void TogglePoint(GameObject go)
    {
        string[] s = go.name.Split('#');
        if (s.Length > 1)
        {
            int index = 0;
            if (int.TryParse(s[1], out index))
            {
                if (togglePointList.Length > index)
                    togglePointList[index].value = true;
                else
                {
                    #if ClientDebug
                    Debug.LogWarning("未找到索引！");
                    #endif
                }
            }
        }
    }


    /// <summary>
    /// Recalculate the position of all elements within the grid, sorting them alphabetically if necessary.
    /// </summary>
    protected void ResetPosition(List<GameObject> list)
    {
        int x = 0;
        int y = 0;
        int maxX = 0;
        int maxY = 0;
        //Transform myTrans = transform;

        for (int i = 0, imax = list.Count; i < imax; ++i)
        {
            Transform t = list[i].transform;

            float depth = t.localPosition.z;

            Vector3 pos = (arrangement == Arrangement.Horizontal) ?
                new Vector3(cellWidth * x, -cellHeight * y, depth) :
                new Vector3(cellWidth * y, -cellHeight * x, depth);

            pos.x += cellOffsetX;
            pos.y += cellOffsetY;

            t.localPosition = pos;

            maxX = Mathf.Max(maxX, x);
            maxY = Mathf.Max(maxY, y);

            if (++x >= maxPerLine && maxPerLine > 0)
            {
                x = 0;
                ++y;
            }
        }

        // Apply the origin offset
        if (pivot != UIWidget.Pivot.TopLeft)
        {
            Vector2 po = NGUIMath.GetPivotOffset(pivot);

            float fx, fy;

            if (arrangement == Arrangement.Horizontal)
            {
                fx = Mathf.Lerp(0f, maxX * cellWidth, po.x);
                fy = Mathf.Lerp(-maxY * cellHeight, 0f, po.y);
            }
            else
            {
                fx = Mathf.Lerp(0f, maxY * cellWidth, po.x);
                fy = Mathf.Lerp(-maxX * cellHeight, 0f, po.y);
            }

            for (int i = 0; i < list.Count; ++i)
            {
                Transform t = list[i].transform;
                SpringPosition sp = t.GetComponent<SpringPosition>();

                if (sp != null)
                {
                    sp.target.x -= fx;
                    sp.target.y -= fy;
                }
                else
                {
                    Vector3 pos = t.localPosition;
                    pos.x -= fx;
                    pos.y -= fy;
                    t.localPosition = pos;
                }
            }
        }
    }

    private int currentPageIndex;
    private List<GameObject> tempPageList=new List<GameObject>();
    private void RebuildCells()
    {
        tempPageList.Clear();
        currentPageIndex = 0;
        if (controlTemplate == null) return;
        oldMaxCount = controlList.Count;

        if (maxCount < oldMaxCount)
        {
            for (int i = oldMaxCount - 1; i >= maxCount; i--)
            {
                GameObject c = controlList[i];
                controlList.RemoveAt(i);
                if (c == null) { continue; }
                if (Application.isPlaying)
                {
                    mRestoreList.Add(c);
                    c.SetActive(false);
                }
                else
                {
                    DestroyImmediate(c);
                }
            }
        }
        else
        {
            for (int i = oldMaxCount; i < maxCount; i++)
            {
                GameObject c = null;
                if (mRestoreList.Count > 0)
                {
                    c = mRestoreList[0];
                    mRestoreList.RemoveAt(0);
                }
                else
                {
                    c = GameObject.Instantiate(controlTemplate) as GameObject;
                }
                c.SetActive(true);
                controlList.Add(c);
            }
        }

        //将物体分配到不同页面
        for (int i = 0; i < controlList.Count; i++)
        {
            if (i >= (currentPageIndex + 1) * perPageItemNumber)
            {
                ResetPosition(tempPageList);//将元素排布
                tempPageList.Clear();
                currentPageIndex++;
            }

            controlList[i].name = "item" + i;
            controlList[i].transform.parent = pages[currentPageIndex];
            tempPageList.Add(controlList[i]);
            controlList[i].transform.localScale = Vector3.one;
            controlList[i].transform.localPosition = Vector3.zero;

        }

        if (tempPageList != null)
        {
            ResetPosition(tempPageList);
            tempPageList.Clear();
        }

        if (mStarted == false) mStarted = true;
        if (controlTemplate != null) controlTemplate.SetActive(!Application.isPlaying);
        //ResetPosition(controlList);
    }

    //初始化页面
    public void InitlizePage<T>(List<T> combines/*, int maxCount*/)
    {
        //设置物体数量
        MaxCount = combines.Count;

        for (int i = 0; i < combines.Count; i++)
        {
            if (i < perPageItemNumber)
            {
                StartCoroutine(StartInitButton(combines[i], controlList[i]));
            }
            else
            {
                StartCoroutine(StartInitButton(combines[i], controlList[i]));
            }
        }
    }

    IEnumerator StartInitButton(object table,GameObject go)
    {
        //yield return new WaitForSeconds(time);
        yield return new WaitForEndOfFrame();

        if (InitInfo != null)
            InitInfo(table, go);
        else
        {
        }

        //go.SetActive(true);
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    public delegate void InitButtonInfo (object table, GameObject go);

    public InitButtonInfo  InitInfo;
}
