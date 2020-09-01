using Google.Protobuf;
using UnityEngine;

public class CSMapAvatarInfo
{
    public UIItemMiniMapPoint mMapPoint;

    protected Transform mParent;

    public GameObject mCloneObject;

    public virtual MapAvaterType AvatarType
    {
        get { return MapAvaterType.None; }
    }

    public long ID;
    public string AvatarName;

    public int mDotX;
    public int mDotY;
    private Vector2 mMainPlayerPos;

    public void Init(IMessage info, Transform parent, GameObject cloneObject)
    {
        mParent = parent;
        mCloneObject = cloneObject;
        Init(info);
    }

    public void Init(CSMapAvatarInfo info, Transform parent, GameObject cloneObject)
    {
        mParent = parent;
        mCloneObject = cloneObject;
        Init(info);
    }

    protected virtual void Init(IMessage info)
    {
    }

    protected virtual void Init(CSMapAvatarInfo info)
    {
    }

    public virtual void Show()
    {
        if (mMapPoint == null)
        {
            mMapPoint = new UIItemMiniMapPoint();
        }

        if (mMapPoint.gameObject == null && mCloneObject != null)
        {
            mMapPoint.gameObject = Object.Instantiate(mCloneObject);
            mMapPoint.Init();
        }

        if (mParent != null && mMapPoint != null)
        {
            NGUITools.SetParent(mParent, mMapPoint.gameObject);
        }

        if (mMainPlayerPos != Vector2.zero)
            SetMainPlayerPos(mMainPlayerPos);
        SetStartPos();
        RefreshUI();
    }


    public virtual void RefreshUI()
    {
    }

    public void SetMainPlayerPos(Vector2 mMainPlayerPos)
    {
        if (mMapPoint != null)
        {
            mMapPoint.SetMainPos(mMainPlayerPos);
        }
        else
        {
            this.mMainPlayerPos.x = mMainPlayerPos.x;
            this.mMainPlayerPos.y = mMainPlayerPos.y;
        }
    }

    public void ResetServerCell(int x, int y)
    {
        if (mMapPoint != null)
        {
            mDotX = x;
            mDotY = y;
            mMapPoint.SetTargetPos(x, y);
            mMapPoint.StartMove();
        }
    }

    public void SetStartPos()
    {
        if (mMapPoint != null)
        {
            mMapPoint.SetTargetPos(mDotX, mDotY);
            mMapPoint.SetFixedPos();
        }
    }

    public virtual void Dispose()
    {
        mMainPlayerPos = Vector2.zero;
        if (mMapPoint != null)
            mMapPoint.Dispose();
    }

    public virtual void Destroy()
    {
        if (mMapPoint != null)
            mMapPoint.Destroy();
        mMapPoint = null;
        mCloneObject = null;
        mParent = null;
    }
}