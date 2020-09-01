//----------------------------------------------
//            Author: JiaBao
//            Time :  2016.1.13
//----------------------------------------------
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/UIGridBinderContainer")]
public class UIGridBinderContainer : MonoBehaviour
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

    private bool widthDependLength = false;
    public ScriptBinder controlTemplate;

    public bool WidthDependLength
    {
        get { return widthDependLength; }
        set
        {
            if (widthDependLength == value)
                return;
            widthDependLength = value;
            RebuildCells();
        }
    }
    private int maxwidth = 0;
    public int MaxWidth
    {
        get { return maxwidth; }
        set
        {
            if (maxwidth == value)
                return;
            maxwidth = value;
            RebuildCells();
        }
    }

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

    //protected List<object> datas;
    //public delegate void OnItemVisible(ScriptBinder binder,object param);
    //public OnItemVisible onItemVisible;

    //public void BindDatas(List<object> datas)
    //{
    //    this.datas = datas;
    //    int count = null == datas ? 0 : datas.Count;
    //    if(MaxCount != count)
    //    {
    //        MaxCount = count;
    //    }
    //    else
    //    {
    //        for (int i = 0; i < count; ++i)
    //        {
    //            onItemVisible?.Invoke(controlList[i], datas[i]);
    //        }
    //    }
    //}

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

    public List<ScriptBinder> controlList = new List<ScriptBinder>();
    private CSBetterList<ScriptBinder> mRestoreList = new CSBetterList<ScriptBinder>();
    bool mStarted = false;

    void Start()
    {
        if (controlTemplate != null) controlTemplate.gameObject.SetActive(!Application.isPlaying);
        if(mStarted == false)
        {
            mStarted = true;
            RebuildCells();
        }
    }


    /// <summary>
    /// Recalculate the position of all elements within the grid, sorting them alphabetically if necessary.
    /// </summary>

    protected void ResetPosition(List<ScriptBinder> list)
    {
        int x = 0;
        int y = 0;
        int maxX = 0;
        int maxY = 0;
        //Transform myTrans = transform;
        if (!widthDependLength)
        {
            for (int i = 0, imax = list.Count; i < imax; ++i)
            {
                Transform t = list[i].transform;

                float depth = t.localPosition.z;
                Vector3 pos = (arrangement == Arrangement.Horizontal) ?
                    new Vector3(cellWidth * x, -cellHeight * y, depth) :
                    new Vector3(cellWidth * y, -cellHeight * x, depth);

                t.localPosition = pos;

                maxX = Mathf.Max(maxX, x);
                maxY = Mathf.Max(maxY, y);

                if (++x >= maxPerLine && maxPerLine > 0)
                {
                    x = 0;
                    ++y;
                }
            }
        }
        else
        {
            int frontX = 0;

            for (int i = 0, imax = list.Count; i < imax; ++i)
            {
                Transform t = list[i].transform;

                float depth = t.localPosition.z;

                int itemWidth = frontX;

                frontX = frontX + list[i].GetComponent<UISprite>().width + 20;
                Vector3 pos = new Vector3(itemWidth, -cellHeight * y, depth);

                t.localPosition = pos;

                maxX = Mathf.Max(maxX, x);
                maxY = Mathf.Max(maxY, y);

                if ((frontX + list[i].GetComponent<UISprite>().width / 2) >= maxwidth && maxwidth > 0)
                {
                    x = 0;
                    frontX = 0;
                    ++y;
                }
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

    private void RebuildCells()
    {
        if (controlTemplate == null) return;
        oldMaxCount = controlList.Count;

        if (maxCount < oldMaxCount)
        {
            //for(int i = 0; i < maxCount; ++i)
            //{
            //    onItemVisible?.Invoke(controlList[i],datas[i]);
            //}

            for (int i = oldMaxCount-1; i >= maxCount; i--)
            {
                ScriptBinder c = controlList[i];
                controlList.RemoveAt(i);
                if (c == null) { continue; }
                if (Application.isPlaying)
                {
                    mRestoreList.Add(c);
                    c.gameObject.SetActive(false);
                }
                else
                {
                    DestroyImmediate(c);
                }
            }
        }
        else
        {
            //for (int i = 0; i < oldMaxCount; ++i)
            //{
            //    onItemVisible?.Invoke(controlList[i], datas[i]);
            //}

            for (int i = oldMaxCount; i < maxCount; i++)
            {
                ScriptBinder c = null;
                if (mRestoreList.Count > 0)
                {
                    c = mRestoreList[0];
                    mRestoreList.RemoveAt(0);
                }
                else
                {
                    c = GameObject.Instantiate(controlTemplate) as ScriptBinder;
                }
                c.name = controlTemplate.name + i + 1;
                c.transform.parent = this.transform;
                c.transform.localScale = Vector3.one;
                c.transform.localPosition = Vector3.zero;
                controlList.Add(c);
                //onItemVisible?.Invoke(c,datas[i]);
                c.gameObject.SetActive(true);
            }
        }

        if (mStarted == false) mStarted = true;
        if (controlTemplate != null) controlTemplate.gameObject.SetActive(!Application.isPlaying);
        ResetPosition(controlList);
    }
}
