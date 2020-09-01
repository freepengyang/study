using System;
using System.Collections.Generic;
using UnityEngine;
public class LoopRoolScrollView : MonoBehaviour
{
    [HideInInspector]
    public int MaxShowCount
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
    private int maxCount = 0;
    private int oldMaxCount = 0;

    /// <summary>
    /// 间隔坐标值
    /// </summary>
    [Range(0, 1)]
    [HideInInspector]
    public float AddValue = 0.2f;

    /// <summary>
    /// 显示范围，宽度
    /// </summary>
    [HideInInspector]
    public float Width = 600;

    /// <summary>
    ///是否循环滚动
    /// </summary>    public bool IsLoop = false;    private float MaxScale = 1;

    public GameObject controlTemplate;    private Transform Rect;    private List<LoopRoolScrollViewItem> Items;

    //曲线可以任意使用，不用非要按名字取值，重写Drag时，进行赋值就行
    [HideInInspector]
    public AnimationCurve ScaleCurve;
    [HideInInspector]
    public AnimationCurve ApaCurve;
    [HideInInspector]
    public AnimationCurve PositionCurve;
    ///// <summary>
    /////  StartValue开始坐标值
    ///// </summary>
    [HideInInspector]
    public float StartValue = 0.1f;













    /// <summary>    ///小于vmian 达到最左，大于vmax达到最右    /// </summary>    private float VMin = 0.1f, VMax = 0.9f;
    //位置移动
    private float v = 0;    private List<LoopRoolScrollViewItem> GotoFirstItems;
    private List<LoopRoolScrollViewItem> GotoLaserItems;
    private List<GameObject> controlList;


    public List<GameObject> ControlList
    {
        get
        {
            return controlList;
        }
    }

    private void Awake()
    {
        Rect = transform;
        Items = new List<LoopRoolScrollViewItem>();
        GotoFirstItems = new List<LoopRoolScrollViewItem>();
        GotoLaserItems = new List<LoopRoolScrollViewItem>();
        controlList = new List<GameObject>();
        VMin = 0.1f;
        VMax = 0.9f;
    }













    /// <summary>    /// 计算值    /// </summary>    private Vector2 start_point, add_vect;    public Action<LoopRoolScrollViewItem> MoveEnd;    public void Refresh(int middleIten = 1)    {        Items.Clear();        int middleindex = controlList.Count / 2;        for (int i = 0; i < controlList.Count; i++)        {            LoopRoolScrollViewItem item = controlList[i].GetComponent<LoopRoolScrollViewItem>();            if (item == null)            {
                item = controlList[i].AddComponent<LoopRoolScrollViewItem>();
            }
            Items.Add(item);
            item.Init(this);
            int value = i - middleIten;
            if (Mathf.Abs(value) > middleindex)
                value = value > 0 ? (value - controlList.Count) : (value + controlList.Count);
            item.Drag(0.5f + value * AddValue);
        }
        Current = Items[middleIten];

        Items.Sort((LoopRoolScrollViewItem item1, LoopRoolScrollViewItem item2) => { return item1.v.CompareTo(item2.v); });

        SetSpriteDepth();
        //if(IsLoop)
        //    if (controlList.Count < 5)
        //    {
        //        VMax = StartValue + 4 * AddValue;
        //    }
        //    else
        //    {
        //        VMax = StartValue + (controlList.Count - 1) * AddValue;
        //    }
        if (MoveEnd != null)        {            MoveEnd(Current);        }    }

    public void OnDragStart()    {        start_point = Input.mousePosition;        add_vect = Vector3.zero;        _anim = false;    }

    public void OnDrag(Vector2 delta)    {        add_vect = delta - start_point;        v = delta.x * 1.00f / Width;        for (int i = 0; i < Items.Count; i++)        {            Items[i].Drag(v);        }        SetSpriteDepth();        if (IsLoop)
            Check(v);    }

    public void Check(float _v)    {        if (_v < 0)        {//向左运动
            for (int i = 0; i < Items.Count; i++)            {                if (Items[i].v < (VMin - AddValue / 2))                {                    GotoLaserItems.Add(Items[i]);                }            }            if (GotoLaserItems.Count > 0)            {                for (int i = 0; i < GotoLaserItems.Count; i++)                {                    GotoLaserItems[i].v = Items[Items.Count - 1].v + AddValue;                    Items.Remove(GotoLaserItems[i]);                    Items.Add(GotoLaserItems[i]);                }                GotoLaserItems.Clear();            }        }        else if (_v > 0)        {//向右运动，需要把右边的放到前面来

            for (int i = Items.Count - 1; i > 0; i--)            {                if (Items[i].v >= VMax)                {                    GotoFirstItems.Add(Items[i]);                }            }            if (GotoFirstItems.Count > 0)            {                for (int i = 0; i < GotoFirstItems.Count; i++)                {                    GotoFirstItems[i].v = Items[0].v - AddValue;                    Items.Remove(GotoFirstItems[i]);                    Items.Insert(0, GotoFirstItems[i]);                }                GotoFirstItems.Clear();            }        }    }

    public void OnDragEnd()    {        Vector2 end_point = Input.mousePosition;
        //职业选择做偏移判断，通用的话，把此处的值放入public
        float middleValue = end_point.x > start_point.x ? 0.3f : 0.7f;        float v1 = 0;        for (int i = 0; i < Items.Count; i++)        {            if (v1 == 0 || Mathf.Abs(Items[i].v - middleValue) < v1)
            {
                v1 = 0.5f - Items[i].v;
            }
        }

        add_vect = Vector3.zero;        AnimToEnd(v1);    }

    public float GetApa(float v)    {        return ApaCurve.Evaluate(v);
    }    public float GetPosition(float v)    {
        return PositionCurve.Evaluate(v) * Width;
    }    public float GetItemScale(float v)    {
        return ScaleCurve.Evaluate(v) * MaxScale;
    }

    private void SetSpriteDepth()
    {
        if (Items.Count <= 0) return;
        Items[0].SetSpriteDepth(Items.Count);
        for (int i = 0; i < Items.Count; i++)        {            if (i != 0)
            {
                if (Items[i].sv > Items[i - 1].sv)
                    Items[i].SetSpriteDepth(Items[i - 1].curPanelDepth + 1);                else
                    Items[i].SetSpriteDepth(Items[i - 1].curPanelDepth - 1);            }        }
    }













    /// <summary>    /// 是否开启动画    /// </summary>    private bool _anim = false;    private float AddV = 0, Vk = 0, CurrentV = 0, Vtotal = 0, VT = 0;    private float _v1 = 0, _v2 = 0;













    /// <summary>    /// 动画速度    /// </summary>    private float _anim_speed = 2f;    public LoopRoolScrollViewItem Current;

    public void AnimToEnd(float k)    {        AddV = k;        if (AddV > 0) { Vk = 1; }        else if (AddV < 0) { Vk = -1; }        else        {            return;        }
        Vtotal = 0;        _anim = true;

    }

    void Update()    {        if (_anim)        {
            CurrentV = Time.deltaTime * _anim_speed * Vk;
            VT = Vtotal + CurrentV;
            if (Vk > 0 && VT >= AddV || Vk < 0 && VT <= AddV)
            {
                _anim = false; CurrentV = AddV - Vtotal;
            }
            //==============
            for (int i = 0; i < Items.Count; i++)            {                Items[i].Drag(CurrentV);                if (Items[i].v - 0.5 < 0.05f)                {                    Current = Items[i];                }
            }            if (IsLoop)
                Check(CurrentV);            Vtotal = VT;
            SetSpriteDepth();

            if (!_anim)            {                if (MoveEnd != null) { MoveEnd(Current); }            }        }    }

    private BetterList<GameObject> mRestoreList = new BetterList<GameObject>();
    bool mStarted = false;
    private void RebuildCells()
    {
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
                c.name = controlTemplate.name + i + 1;
                c.transform.parent = this.transform;
                c.transform.localScale = Vector3.one;
                c.transform.localPosition = controlTemplate.transform.localPosition;
                controlList.Add(c);
                c.SetActive(true);
            }
        }

        if (mStarted == false) mStarted = true;
        if (controlTemplate != null) controlTemplate.SetActive(!Application.isPlaying);
        controlList.Sort((item1, item2) => { return item1.transform.localPosition.x > item2.transform.localPosition.x ? 1 : 0; });
        Refresh();
    }
}


public class LoopRoolScrollViewItem : MonoBehaviour
{
    public LoopRoolScrollView parent; //父物体

    public Transform Rect;

    public int curPanelDepth; //Items之间的层级

    public int startPanelDepth; //初始时的层级

    public float sv; //缩放值

    public float v = 0;//移动的x轴坐标



    public virtual void Init(LoopRoolScrollView _parent)
    {
        parent = _parent;
        Rect = transform;
        startPanelDepth = NGUITools.FindInParents<UIPanel>(transform.parent).depth;
        v = 0;
    }

    public virtual void Drag(float value)
    {

    }

    public virtual int SetSpriteDepth(int depth)
    {
        curPanelDepth = depth;
        return startPanelDepth + curPanelDepth;
    }

    public void OnDragStart()
    {
        parent.OnDragStart();
    }

    public void OnDrag(Vector2 delta)
    {
        parent.OnDrag(delta);
    }

    public void OnDragEnd()
    {
        parent.OnDragEnd();
    }

    public void OnClick()
    {
        if (v == 0.5f) return;
        float k = v;
        k = 0.5f - k;
        if (parent != null)
        {
            parent.AnimToEnd(k);
            //parent.mClientEvent.SendEvent(CEvent.OnDragLoopScrollItemState, true);
        }
    }
}
