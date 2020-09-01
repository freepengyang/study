using UnityEngine;
using System.Collections.Generic;

public class BagWrapContent : MonoBehaviour {

    /// <summary>
    /// Width or height of the child items for positioning purposes.
    /// </summary>
    public int itemSize = 100;

    /// <summary>
    /// Maximum allowed index for items. If "min" is equal to "max" then there is no limit.
    /// For vertical scroll views indices increment with the Y position (towards top of the screen).
    /// </summary>

    public int maxIndex = 0;

    public int minIndex = 0;

    protected bool mHorizontal = false;

    protected Transform mTrans;
    protected UIPanel mPanel;
    protected UIScrollView mScroll;
    List<Transform> mChildren = new List<Transform>();
    // Use this for initialization
    void Start()
    {
        if (!CacheScrollView()) return;

        mPanel.onClipMove = WrapContent;
        mChildren.Clear();
        for (int i = 0; i < mTrans.childCount; ++i)
            mChildren.Add(mTrans.GetChild(i));
    }


    /// <summary>
    /// Cache the scroll view and return 'false' if the scroll view is not found.
    /// </summary>

    protected bool CacheScrollView()
    {
        mTrans = transform;
        mPanel = NGUITools.FindInParents<UIPanel>(gameObject);
        mScroll = mPanel.GetComponent<UIScrollView>();
        if (mScroll == null) return false;
        if (mScroll.movement == UIScrollView.Movement.Horizontal) mHorizontal = true;
        else if (mScroll.movement == UIScrollView.Movement.Vertical) mHorizontal = false;
        else return false;
        return true;
    }

    public void WrapContent(UIPanel panel)
    {
        float extents = itemSize * mChildren.Count * 0.5f;
        Vector3[] corners = mPanel.worldCorners;

        for (int i = 0; i < 4; ++i)
        {
            Vector3 v = corners[i];
            v = mTrans.InverseTransformPoint(v);
            corners[i] = v;
        }

        Vector3 center = Vector3.Lerp(corners[0], corners[2], 0.5f);
        float ext2 = extents * 2f;

        if (mHorizontal)
        {
            //float min = corners[0].x - itemSize;  //Remove Unused
            //float max = corners[2].x + itemSize;  //Remove Unused

            for (int i = 0, imax = mChildren.Count; i < imax; ++i)
            {
                Transform t = mChildren[i];
                float distance = t.localPosition.x - center.x;

                if (distance < -extents)
                {
                    Vector3 pos = t.localPosition;
                    pos.x += ext2;
                    distance = pos.x - center.x;
                    int realIndex = Mathf.RoundToInt(pos.x / itemSize);

                    if (minIndex == maxIndex || (minIndex <= realIndex && realIndex <= maxIndex))
                    {
                        t.localPosition = pos;
                        UpdateItem(t, i);
                    }
                }
                else if (distance > extents)
                {
                    Vector3 pos = t.localPosition;
                    pos.x -= ext2;
                    distance = pos.x - center.x;
                    int realIndex = Mathf.RoundToInt(pos.x / itemSize);

                    if (minIndex == maxIndex || (minIndex <= realIndex && realIndex <= maxIndex))
                    {
                        t.localPosition = pos;
                        UpdateItem(t, i);
                    }
                }
            }
        }
        else 
        {
            

            for (int i = 0, imax = mChildren.Count; i < imax; ++i)
            {
                Transform t = mChildren[i];
                float distance = t.localPosition.y - center.y;

                if (distance < -extents)
                {
                    Vector3 pos = t.localPosition;
                    pos.y += ext2;
                    distance = pos.y - center.y;
                    int realIndex = Mathf.RoundToInt(pos.y / itemSize);

                    if (minIndex == maxIndex || (minIndex <= realIndex && realIndex <= maxIndex))
                    {
                        t.localPosition = pos;
                        UpdateItem(t, i);
                    }
                }
                else if (distance > extents)
                {
                    Vector3 pos = t.localPosition;
                    pos.y -= ext2;
                    distance = pos.y - center.y;
                    int realIndex = Mathf.RoundToInt(pos.y / itemSize);

                    if (minIndex == maxIndex || (minIndex <= realIndex && realIndex <= maxIndex))
                    {
                        t.localPosition = pos;
                        UpdateItem(t, i);
                    }
                }
            }
        }
    }

    protected virtual void UpdateItem(Transform item, int index)
    {
        if (onInitializeItem != null)
        {
            int realIndex = (mScroll.movement == UIScrollView.Movement.Vertical) ?
                Mathf.Abs(Mathf.RoundToInt(item.localPosition.y / itemSize)) :
                Mathf.RoundToInt(item.localPosition.x / itemSize);
            item.GetComponent<UIData>().mIndex = realIndex + 1;
            onInitializeItem(item.gameObject, realIndex + 1);
        }
    }
    public delegate void OnInitializeItem(GameObject go, int realIndex);

    public OnInitializeItem onInitializeItem;
}
