//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// All children added to the game object with this script will be repositioned to be on a grid of specified dimensions.
/// If you want the cells to automatically set their scale based on the dimensions of their content, take a look at UITable.
/// </summary>
public class UIMainActivitySort : MonoBehaviour
{
    public enum Sorting
    {
        None,
        Alphabetic,
        Horizontal,
        Vertical,
        Custom,
    }

    /// <summary>
    /// How to sort the grid's elements.
    /// </summary>
    public Sorting sorting = Sorting.None;

    /// <summary>
    /// Final pivot point for the grid's content.
    /// </summary>
    public UIWidget.Pivot pivot = UIWidget.Pivot.TopLeft;

    /// <summary>
    /// Maximum children per line.
    /// If the arrangement is horizontal, this denotes the number of columns.
    /// If the arrangement is vertical, this stands for the number of rows.
    /// </summary>
    public int maxPerLine = 0;

    /// <summary>
    /// The width of each of the cells.
    /// </summary>
    public float cellWidth = 200f;

    /// <summary>
    /// The height of each of the cells.
    /// </summary>
    public float cellHeight = 200f;

    /// <summary>
    /// Whether the grid will smoothly animate its children into the correct place.
    /// </summary>
    public bool animateSmoothly = false;

    /// <summary>
    /// Whether to ignore the disabled children or to treat them as being present.
    /// </summary>
    public bool hideInactive = false;

    /// <summary>
    /// Whether the parent container will be notified of the grid's changes.
    /// </summary>
    public bool keepWithinPanel = false;

    /// <summary>
    /// Custom sort delegate, used when the sorting method is set to 'custom'.
    /// </summary>
    public System.Comparison<Transform> onCustomSort;

    // Use the 'sorting' property instead
    [HideInInspector] [SerializeField] bool sorted = false;

    protected bool mReposition = false;
    protected UIPanel mPanel;
    protected bool mInitDone = false;
    private List<Transform> list = new List<Transform>();
    
    [SerializeField]
    protected List<int> startPosOffest = new List<int>();

    private Action ShowCountCallBack;
    public int mshowCount;//当前显示的个数

    public int showCount
    {
        get { return mshowCount; }
        set
        {
            if (!mshowCount.Equals(value))
            {                
                mshowCount = value;
                if (ShowCountCallBack != null)
                    ShowCountCallBack();
            }
        }
    }
    /// <summary>
    /// Reposition the children on the next Update().
    /// </summary>

    public bool repositionNow
    {
        set
        {
            if (value)
            {
                mReposition = true;
                enabled = true;
            }
        }
    }

    public void AddShowCountCallback(Action callback)
    {
        ShowCountCallBack = callback;
    }

    public void RemoveShowCountCallBack()
    {
        ShowCountCallBack = null;
    }

    /// <summary>
    /// Get the current list of the grid's children.
    /// </summary>
    public List<Transform> GetChildList()
    {
        Transform myTrans = transform;
        if(list == null) list = new List<Transform>();
        list.Clear();

        for (int i = 0; i < myTrans.childCount; ++i)
        {
            Transform t = myTrans.GetChild(i);
            UIWidget widget = t.GetComponent<UIWidget>();
            
            if(widget == null || widget.alpha <= 0.1f) continue;

            for (int j = 0; j < t.childCount; j++)
            {
                Transform ts = t.GetChild(j);
                if (!hideInactive || (ts && NGUITools.GetActive(ts.gameObject)))
                    list.Add(ts);
            }

        }

        if (sorting != Sorting.None)
        {
            if (sorting == Sorting.Alphabetic) list.Sort(SortByName);
            else if (sorting == Sorting.Horizontal) list.Sort(SortHorizontal);
            else if (sorting == Sorting.Vertical) list.Sort(SortVertical);
            else if (onCustomSort != null) list.Sort(onCustomSort);
            else Sort(list);
        }

        return list;
    }

    /// <summary>
    /// Convenience method: get the child at the specified index.
    /// Note that if you plan on calling this function more than once, it's faster to get the entire list using GetChildList() instead.
    /// </summary>
    public Transform GetChild(int index)
    {
        List<Transform> list = GetChildList();
        return (index < list.Count) ? list[index] : null;
    }

    /// <summary>
    /// Get the index of the specified item.
    /// </summary>
    public int GetIndex(Transform trans)
    {
        return GetChildList().IndexOf(trans);
    }

    /// <summary>
    /// Convenience method -- add a new child.
    /// </summary>
    public void AddChild(Transform trans)
    {
        AddChild(trans, true);
    }

    /// <summary>
    /// Convenience method -- add a new child.
    /// Note that if you plan on adding multiple objects, it's faster to GetChildList() and modify that instead.
    /// </summary>
    public void AddChild(Transform trans, bool sort)
    {
        if (trans != null)
        {
            trans.parent = transform;
            ResetPosition(GetChildList());
        }
    }

    /// <summary>
    /// Remove the specified child from the list.
    /// Note that if you plan on removing multiple objects, it's faster to GetChildList() and modify that instead.
    /// </summary>
    public bool RemoveChild(Transform t)
    {
        List<Transform> list = GetChildList();

        if (list.Remove(t))
        {
            ResetPosition(list);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Initialize the grid. Executed only once.
    /// </summary>
    protected virtual void Init()
    {
        mInitDone = true;
        mPanel = NGUITools.FindInParents<UIPanel>(gameObject);
    }

    /// <summary>
    /// Cache everything and reset the initial position of all children.
    /// </summary>
    protected virtual void Start()
    {
        if (!mInitDone) Init();
        bool smooth = animateSmoothly;
        animateSmoothly = false;
        Reposition();
        animateSmoothly = smooth;
        enabled = false;
    }

    /// <summary>
    /// Reset the position if necessary, then disable the component.
    /// </summary>
    protected virtual void Update()
    {
        Reposition();
        enabled = false;
    }

    /// <summary>
    /// Reposition the content on inspector validation.
    /// </summary>
    void OnValidate()
    {
        if (!Application.isPlaying && NGUITools.GetActive(this)) Reposition();
    }

    // Various generic sorting functions
    static public int SortByName(Transform a, Transform b)
    {
        return string.Compare(a.name, b.name);
    }

    static public int SortHorizontal(Transform a, Transform b)
    {
        return a.localPosition.x.CompareTo(b.localPosition.x);
    }

    static public int SortVertical(Transform a, Transform b)
    {
        return b.localPosition.y.CompareTo(a.localPosition.y);
    }

    /// <summary>
    /// You can override this function, but in most cases it's easier to just set the onCustomSort delegate instead.
    /// </summary>
    protected virtual void Sort(List<Transform> list)
    {
    }

    /// <summary>
    /// Recalculate the position of all elements within the grid, sorting them alphabetically if necessary.
    /// </summary>
    [ContextMenu("Execute")]
    public virtual void Reposition()
    {
        if (Application.isPlaying && !mInitDone && NGUITools.GetActive(gameObject)) Init();

        // Legacy functionality
        if (sorted)
        {
            sorted = false;
            if (sorting == Sorting.None)
                sorting = Sorting.Alphabetic;
            NGUITools.SetDirty(this);
        }

        // Get the list of children in their current stage
        List<Transform> list = GetChildList();

        // Reset the position and stage of all objects in the list
        ResetPosition(list);

        // Constrain everything to be within the panel's bounds
        if (keepWithinPanel) ConstrainWithinPanel();
    }

    /// <summary>
    /// Constrain the grid's content to be within the panel's bounds.
    /// </summary>
    public void ConstrainWithinPanel()
    {
        if (mPanel != null)
        {
            mPanel.ConstrainTargetToBounds(transform, true);
            UIScrollView sv = mPanel.GetComponent<UIScrollView>();
            if (sv != null) sv.UpdateScrollbars(true);
        }
    }

    /// <summary>
    /// Reset the position of all child objects based on the stage of items in the list.
    /// </summary>
    protected virtual void ResetPosition(List<Transform> list)
    {
        if(list == null) return;
        showCount = list.Count;
        mReposition = false;

        int x = 0;
        int y = 0;
        int maxX = 0;
        int maxY = 0;
        Transform myTrans = transform;

        int offset = 0;
        for (int i = 0, imax = list.Count; i < imax; ++i)
        {
            Transform t = list[i];
            Vector3 pos = t.localPosition;
            float depth = pos.z;

            pos = new Vector3(cellWidth * x + offset, -cellHeight * y, depth);

            if (animateSmoothly && Application.isPlaying)
            {
                SpringPosition sp = SpringPosition.Begin(t.gameObject, pos, 15f);
                sp.updateScrollView = true;
                sp.ignoreTimeScale = true;
            }
            else t.localPosition = pos;

            maxX = Mathf.Max(maxX, x);
            maxY = Mathf.Max(maxY, y);

            if (++x >= maxPerLine && maxPerLine > 0)
            {
                ++y;
                x = 0;
                if (startPosOffest.Count > y)
                {
                    offset = startPosOffest[y];
                }
                else
                {
                    offset = 0;
                }
            }
        }

        // Apply the origin offset
        /*if (pivot != UIWidget.Pivot.TopLeft)
        {
            Vector2 po = NGUIMath.GetPivotOffset(pivot);

            float fx = Mathf.Lerp(0f, maxX * cellWidth, po.x);
            float fy = Mathf.Lerp(-maxY * cellHeight, 0f, po.y);

            for (int i = 0; i < list.Count; ++i)
            {
                Transform t = list[i];
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
        }*/
    }
}