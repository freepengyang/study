﻿using System;
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
    /// </summary>

    public GameObject controlTemplate;

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













    /// <summary>
    //位置移动
    private float v = 0;
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













    /// <summary>
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
        if (MoveEnd != null)

    public void OnDragStart()

    public void OnDrag(Vector2 delta)
            Check(v);

    public void Check(float _v)
            for (int i = 0; i < Items.Count; i++)

            for (int i = Items.Count - 1; i > 0; i--)

    public void OnDragEnd()
        //职业选择做偏移判断，通用的话，把此处的值放入public
        float middleValue = end_point.x > start_point.x ? 0.3f : 0.7f;
            {
                v1 = 0.5f - Items[i].v;
            }
        }

        add_vect = Vector3.zero;

    public float GetApa(float v)
    }
        return PositionCurve.Evaluate(v) * Width;
    }
        return ScaleCurve.Evaluate(v) * MaxScale;
    }

    private void SetSpriteDepth()
    {
        if (Items.Count <= 0) return;
        Items[0].SetSpriteDepth(Items.Count);
        for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].sv > Items[i - 1].sv)
                    Items[i].SetSpriteDepth(Items[i - 1].curPanelDepth + 1);
                    Items[i].SetSpriteDepth(Items[i - 1].curPanelDepth - 1);
    }













    /// <summary>













    /// <summary>

    public void AnimToEnd(float k)


    }

    void Update()
            CurrentV = Time.deltaTime * _anim_speed * Vk;
            VT = Vtotal + CurrentV;
            if (Vk > 0 && VT >= AddV || Vk < 0 && VT <= AddV)
            {
                _anim = false; CurrentV = AddV - Vtotal;
            }
            //==============
            for (int i = 0; i < Items.Count; i++)
            }
                Check(CurrentV);
            SetSpriteDepth();

            if (!_anim)

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