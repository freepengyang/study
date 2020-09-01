using UnityEngine;
using System.Collections;

public class WrapContent : MonoBehaviour
{

    public delegate void OnInitializeItem(int wrapIndex);
    /// <summary>
    /// Width or height of the child items for positioning purposes.
    /// </summary>
    public int itemSize = 100;

    /// <summary>
    /// Maximum allowed index for items. If "min" is equal to "max" then there is no limit.
    /// For vertical scroll views indices increment with the Y position (towards top of the screen).
    /// </summary>

    public int maxIndex = 0;

    /// <summary>
    /// 偏移值
    /// </summary>
    public int deviant = 0;


    public int initialValue = 0;

    private int mRepeatNum;

    private Vector3 initalPanelPostion = new Vector3();
    private Vector3 initalPostion = new Vector3();

    protected bool mHorizontal = false;

    public OnInitializeItem onInitializeItem;
    protected Transform mTrans;
    protected UIPanel mPanel;
    protected UIScrollView mScroll;

    // Use this for initialization
    void Start()
    {

        if (!CacheScrollView()) return;

        mPanel.onClipMove = OnMove;

        initalPanelPostion = mPanel.transform.localPosition;
        initalPostion = mTrans.localPosition;

        mRepeatNum = Mathf.CeilToInt(mPanel.finalClipRegion.w / itemSize);
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


    protected void OnMove(UIPanel panel)
    {
        float f =  (panel.transform.localPosition.y - initalPanelPostion.y);
        TowerOffset = (int)((f + deviant) / itemSize);
    }

    private int mTowerOffset;
    /// <summary>TowerMiddle往上移动的层数</summary>
    private int TowerOffset
    {
        get { return mTowerOffset; }
        set
        {
            value = Mathf.Clamp(value, 0, maxIndex - mRepeatNum);
            if (value != mTowerOffset)
            {
                if (Debug.developerConsoleVisible) Debug.Log("mTowerOffset " + mTowerOffset);
                mTowerOffset = value;
                mTrans.localPosition = initalPostion + Vector3.down * value * itemSize;
                if (onInitializeItem != null) 
                {
                    onInitializeItem(value);
                }
            }
        }
    }
}
