using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.Analytics;
using Object = UnityEngine.Object;

public abstract class GridContainerBase : IDispose
{
    private GameObject _game;

    public GameObject gameObject
    {
        get { return _game; }
        set
        {
            _game = value;
            if (_game != null)
                transform = _game.transform;
            else
                transform = null;
        }
    }

    public Transform transform { get; private set; }

    public abstract void Dispose();

    public virtual void Init()
    {
    }

    public T Get<T>(string path, Transform parent = null) where T : UnityEngine.Object
    {
        Transform objTrans = Get(path, parent);
        if (objTrans == null) return null; //防止下面return objTrans.gameObject时报错
        if (typeof(T) == typeof(Transform)) return objTrans as T;

        if (typeof(T) == typeof(GameObject)) return objTrans.gameObject as T;

        if (objTrans)
            return objTrans.gameObject.GetComponent<T>();
        return null;
    }

    public Transform Get(string _path, Transform parent = null)
    {
        if (parent == null)
        {
            if (gameObject)
            {
                return gameObject.transform.Find(_path);
            }
            else
                return null;
        }
        else
            return parent.Find(_path);
    }
}

public class UIGridContainerHot<T> : IDispose where T : GridContainerBase, new()
{
    private const float RADIANTOANGER = 57.3f;
    private const float ANGERTORADTAN = 0.01745329f;


    public enum Arrangement
    {
        Horizontal,
        Vertical,
        Circle, //圆形
    }

    private Arrangement _arrangement = Arrangement.Horizontal;

    //目前仅支持 UIWidget.Pivot.TopLeft
    private UIWidget.Pivot pivot = UIWidget.Pivot.TopLeft;

    //设置参数
    private int maxPerLine;
    private float cellWidth = 200f;
    private float cellHeight = 200f;

    private float cellAngle = 10; //角度间隔
    //----------

    private GameObject parentPrefab; //当前脚本的对象
    private GameObject controlTemplate; //脚本生成的对象
    private bool NoSetPosition; //是否需要代码设置位置

    private int maxCount = -1;
    private int oldMaxCount = 0;
    //private int maxwidth = 0;

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

    public float CellWidth
    {
        get { return cellWidth; }
    }

    public float CellHeight
    {
        get { return cellHeight; }
    }

    public ILBetterList<T> controlList = new ILBetterList<T>(16);
    private ILBetterList<T> mRestoreList = new ILBetterList<T>(16);

    public ILBetterList<T> RestoreList
    {
        get { return mRestoreList; }
    }

	#region 设置属性值

    public UIGridContainerHot<T> SetArrangement(Arrangement arrangement)
    {
        _arrangement = arrangement;
        return this;
    }

    public UIGridContainerHot<T> SetMaxPerLine(int maxPerLine)
    {
        this.maxPerLine = maxPerLine;
        return this;
    }

    public UIGridContainerHot<T> SetCellWidth(float cellWidth)
    {
        this.cellWidth = cellWidth;
        return this;
    }

    public UIGridContainerHot<T> SetCellHeight(float cellHeight)
    {
        this.cellHeight = cellHeight;
        return this;
    }

    public UIGridContainerHot<T> SetCellAngle(float cellAngle)
    {
        this.cellAngle = cellAngle;
        return this;
    }

    public UIGridContainerHot<T> SetGameObject(GameObject curPrefab, GameObject controlTemplate)
    {
        this.parentPrefab = curPrefab;
        this.controlTemplate = controlTemplate;
        return this;
    }

    public UIGridContainerHot<T> SetBuildPosition(bool _NoSetPosition)
    {
        this.NoSetPosition = _NoSetPosition;
        return this;
    }

	#endregion

    /// <summary>
    /// Recalculate the position of all elements within the grid, sorting them alphabetically if necessary.
    /// </summary>
    protected void ResetPosition(ILBetterList<T> list)
    {
        if (NoSetPosition) return;
        int x = 0;
        int y = 0;
        int maxX = 0;
        int maxY = 0;

        Vector3 posv = Vector3.zero;

        if (arrangement == Arrangement.Horizontal || arrangement == Arrangement.Vertical)
        {
            for (int i = 0, imax = list.Count; i < imax; ++i)
            {
                Transform t = list[i].gameObject.transform;

                float depth = t.localPosition.z;

                if (arrangement == Arrangement.Horizontal)
                    posv.Set(cellWidth * x, -cellHeight * y, depth);
                else
                    posv.Set(cellWidth * y, -cellHeight * x, depth);

                t.localPosition = posv;

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

                Transform t;
                for (int i = 0; i < list.Count; ++i)
                {
                    t = list[i].gameObject.transform;
                    SpringPosition sp = t.GetComponent<SpringPosition>();

                    if (sp != null)
                    {
                        sp.target.x -= fx;
                        sp.target.y -= fy;
                    }
                    else
                    {
                        posv = t.localPosition;
                        posv.x -= fx;
                        posv.y -= fy;
                        t.localPosition = posv;
                    }
                }
            }
        }
        else if (arrangement == Arrangement.Circle)
        {
            Vector3 anglev = Vector3.zero;
            posv = controlTemplate.transform.localPosition;
            float initAngle = RADIANTOANGER * Mathf.Atan2(posv.y, posv.x);
            float objAngle = controlTemplate.transform.localEulerAngles.z;
            float radius = posv.y / Mathf.Sin(initAngle * ANGERTORADTAN);

            Transform t;
            for (int i = 0; i < list.Count; i++)
            {
                t = list[i].gameObject.transform;
                anglev.Set(0, 0, objAngle);
                t.localEulerAngles = anglev;
                posv.Set(radius * Mathf.Cos(initAngle * ANGERTORADTAN), radius * Mathf.Sin(initAngle * ANGERTORADTAN),
                    0);
                t.localPosition = posv;
                initAngle = initAngle - cellAngle;
                objAngle = objAngle - cellAngle;
            }
        }
    }

    private void RebuildCells()
    {
        if (controlTemplate == null) return;
        oldMaxCount = controlList.Count;

        if (maxCount < oldMaxCount)
        {
            for (int i = oldMaxCount - 1; i >= maxCount; i--)
            {
                T c = controlList[i];
                controlList.RemoveAt(i);
                if (c == null)
                {
                    continue;
                }

                mRestoreList.Add(c);
                c.gameObject.SetActive(false);
                //c.Dispose();
            }
        }
        else
        {
            for (int i = oldMaxCount; i < maxCount; i++)
            {
                T c = null;
                if (mRestoreList.Count > 0)
                {
                    c = mRestoreList[0];
                    mRestoreList.RemoveAt(0);
                }
                else
                {
                    c = new T();
                    c.gameObject = Object.Instantiate(controlTemplate);
                    c.Init();
                }

                Transform transform = c.gameObject.transform;
                c.gameObject.name = controlTemplate.name + i + 1;
                transform.parent = parentPrefab.transform;
                transform.localScale = Vector3.one;
                transform.localPosition = Vector3.zero;
                controlList.Add(c);
                c.gameObject.SetActive(true);
            }
        }

        if (controlTemplate != null) controlTemplate.SetActive(!Application.isPlaying);
        ResetPosition(controlList);
    }

    public void Dispose()
    {
        for (int i = 0; i < controlList.Count; i++)
        {
            if (controlList[i].gameObject != null) controlList[i].Dispose();
        }

        controlList.Clear();
        controlList = null;
        for (int i = 0; i < mRestoreList.Count; i++)
        {
            if (mRestoreList[i].gameObject != null) mRestoreList[i].Dispose();
        }

        mRestoreList.Clear();
        mRestoreList = null;
        parentPrefab = null;
        controlTemplate = null;
        NoSetPosition = false;
    }
}