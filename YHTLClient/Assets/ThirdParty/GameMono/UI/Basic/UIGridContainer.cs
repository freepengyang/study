//----------------------------------------------
//            Author: JiaBao
//            Time :  2016.1.13
//----------------------------------------------
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/GridContainer")]
public class UIGridContainer : MonoBehaviour
{
    public delegate void OnReposition();

    public enum Arrangement
    {
        Horizontal,
        Vertical,
        CircleAnticlock, //圆形逆时针
		CircleClock, //圆形顺时针
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
    private float cellAngle;

    public GameObject controlTemplate;
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
    public float CellAngle
    {
        get { return cellAngle; }
        set
        {
            if (cellAngle == value)
                return;
            cellAngle = value;
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
    public CSBetterList<GameObject> RestoreList
    {
        get
        {
            return mRestoreList;
        }
    }
    bool mStarted = false;

    void Start()
    {
        if (controlTemplate != null) controlTemplate.SetActive(!Application.isPlaying);
        if(mStarted == false)
        {
            mStarted = true;
            RebuildCells();
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
        if(arrangement == Arrangement.Horizontal || arrangement == Arrangement.Vertical)
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
        else if(arrangement == Arrangement.CircleClock || arrangement == Arrangement.CircleAnticlock)
		{
			Vector3 anglev = Vector3.zero;
			Vector3 posv = controlTemplate.transform.localPosition;
			float initAngle = RADIANTOANGER * Mathf.Atan2(posv.y, posv.x);
			float objAngle = controlTemplate.transform.localEulerAngles.z;
			float radius = posv.y / Mathf.Sin(initAngle * ANGERTORADTAN);
			Transform t;
			float tanAngle;
			for (int i = 0; i < list.Count; i++)
			{
				t = list[i].transform;
				anglev.Set(0,0, objAngle);
				t.localEulerAngles = anglev;
				tanAngle = initAngle * ANGERTORADTAN;
				posv.Set(radius * Mathf.Cos(tanAngle),radius * Mathf.Sin(tanAngle),0);
				t.localPosition = posv;
                initAngle = initAngle - cellAngle;
                objAngle = objAngle - cellAngle;
			}

		}
    }
    private const float RADIANTOANGER = 57.3f;
    private const float ANGERTORADTAN = 0.01745329f;
    private void RebuildCells()
    {
        if (controlTemplate == null) return;
        oldMaxCount = controlList.Count;

        if (maxCount < oldMaxCount)
        {
            for (int i = oldMaxCount-1; i >= maxCount; i--)
            {
                GameObject c = controlList[i];
                controlList.RemoveAt(i);
                if (c == null) { continue; }
                if (Application.isPlaying)
                {
                    mRestoreList.Insert(0, c);
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
                c.name = $"{i + 1}";
                c.transform.parent = this.transform;
                c.transform.localScale = Vector3.one;
                c.transform.localPosition = Vector3.zero;
                controlList.Add(c);
                c.SetActive(true);
            }
        }

        if (mStarted == false) mStarted = true;
        if (controlTemplate != null) controlTemplate.SetActive(!Application.isPlaying);
        ResetPosition(controlList);
    }

    public Vector2 GetPivotOffset(int count)
    {
        Vector2 range = Vector2.zero;
        if (maxPerLine > 0)
        {
            range.x = count - 1;
        }

        if (maxPerLine > 0)
        {
            range.y = (count / maxPerLine == 0 ? count / maxPerLine : count / maxPerLine + 1) - 1;
        }

        Vector2 pivotOffset = Vector2.zero;
        if (pivot != UIWidget.Pivot.TopLeft)
        {
            Vector2 po = NGUIMath.GetPivotOffset(pivot);
            if (arrangement == Arrangement.Horizontal)
            {
                pivotOffset.x = Mathf.Lerp(0f, range.x * cellWidth, po.x);
                pivotOffset.y = Mathf.Lerp(-range.y * cellHeight, 0f, po.y);
            }
            else
            {
                pivotOffset.x = Mathf.Lerp(0f, range.y * cellWidth, po.x);
                pivotOffset.y = Mathf.Lerp(-range.x * cellHeight, 0f, po.y);
            }
        }

        return pivotOffset;
    }

    public IEnumerator BindAsync(int count,System.Action<GameObject,int> onItemVisible)
    {
        if (controlTemplate == null)
        {
            Debug.LogError("[BindAsync]:Failed controlTemplate is Null");
            yield break;
        }

        oldMaxCount = controlList.Count;

        Vector2 pivotOffset = GetPivotOffset(count);

        if (count <= oldMaxCount)
        {
            //注意这里的回收必须是一次性的
            for (int i = oldMaxCount - 1; i >= count; i--)
            {
                GameObject c = controlList[i];
                controlList.RemoveAt(i);
                if (c == null) { continue; }
                if (Application.isPlaying)
                {
                    mRestoreList.Insert(0, c);
                    if(c.activeSelf)
                        c.SetActive(false);
                }
                else
                {
                    DestroyImmediate(c);
                }
            }
            maxCount = count;

            //这里分步执行没有问题
            for (int i = 0; i < count; ++i)
            {
                GameObject c = controlList[i];
                Transform t = c.transform;
                if (!c.activeSelf)
                    c.SetActive(true);
                onItemVisible.Invoke(c, i);
                yield return null;
            }
        }
        else
        {
            //对于已经存在的不刷新位置
            //注意这里的maxCount 是每帧加的 不能直接 maxCount = count
            int x = 0;
            int y = 0;
            for (int i = 0; i < oldMaxCount; ++i)
            {
                if (arrangement == Arrangement.Horizontal || arrangement == Arrangement.Vertical)
                {
                    if (++x >= maxPerLine && maxPerLine > 0)
                    {
                        x = 0;
                        ++y;
                    }
                }

                GameObject c = controlList[i];
                Transform t = c.transform;
                if (!c.activeSelf)
                    c.SetActive(true);
                onItemVisible.Invoke(c, i);
                yield return null;
            }

            for (int i = oldMaxCount; i < count; i++)
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
                var t = c.transform;
                t.parent = this.transform;
                t.localScale = Vector3.one;
                t.localPosition = Vector3.zero;
                controlList.Add(c);

                Vector3 pos = t.localPosition;

                if (arrangement == Arrangement.Horizontal || arrangement == Arrangement.Vertical)
                {
                    if(arrangement == Arrangement.Horizontal)
                    {
                        pos.x = cellWidth * x;
                        pos.y = -cellHeight * y;
                    }
                    else
                    {
                        pos.x = cellWidth * y;
                        pos.y = -cellHeight * x;
                    }
                    t.localPosition = pos;

                    SpringPosition sp = t.GetComponent<SpringPosition>();
                    if (sp != null)
                    {
                        sp.target.x -= pivotOffset.x;
                        sp.target.y -= pivotOffset.y;
                    }
                    else
                    {
                        pos.x -= pivotOffset.x;
                        pos.y -= pivotOffset.y;
                        t.localPosition = pos;
                    }

                    if (++x >= maxPerLine && maxPerLine > 0)
                    {
                        x = 0;
                        ++y;
                    }
                }

                if (!c.activeSelf)
                    c.SetActive(true);

                onItemVisible.Invoke(c, i);
                maxCount += 1;
                yield return null;
            }
        }
        
        mStarted = true;
        if (controlTemplate != null)
        {
            if(controlTemplate.activeSelf != !Application.isPlaying)
                controlTemplate.SetActive(!Application.isPlaying);
        }
    }
}
