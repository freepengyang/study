/*************************************************************************
** File: CSAvaterController.cs
** Author: jiabao
** Time: 2020.7.22
** Describe: CSAvater Controller控制器
*************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSAvaterController : MonoBehaviour
{
    protected Transform mAnchor;            //Go's Parent
    protected Transform mCacheTransform;    //Go.Transform
    protected Transform mCacheRootTransform;  //Go's Child
    protected CSModelModule mModelModule;
    private bool mIsUnitLoad;
    private long mUnitID;
    private CSCell mOldCell;
    private CSCell mNewCell;
    private CSCell mServerCell;
    protected EShareMatType mShaderType = EShareMatType.Normal;
    protected bool mIsMoving = false;
    protected bool mIsMoveOne = false;
    protected float mSpeed = 50f;
    protected float mAdjustStepTime = 0;
    protected Vector3 mCurMovePos = Vector3.zero;
    protected Node mNextNode;
    protected CSObjectPoolItem mPoolItem;
    public CSAction action { get; set; }
    Transform avaterTransform { get; set; }


    public Transform CacheTransform
    {
        get { return mCacheTransform; }
        set { mCacheTransform = value; }
    }

    public Transform CacheRootTransform
    {
        get { return mCacheRootTransform; }
        set { mCacheRootTransform = value; }
    }

    public CSModelModule ModelModule
    {
        get { return mModelModule; }
        set { mModelModule = value; }
    }

    public long UnitID
    {
        set { mUnitID = value; }
        get { return mUnitID; }
    }

    public CSCell OldCell
    {
        get { return mOldCell; }
        set
        {
            if (mOldCell != null && value != null &&
                (mOldCell == value) && (mOldCell.Coord.Equal(value.Coord))) return;

            if (value != null)
            {
                RemoveNodeAvatar(mOldCell);
                mOldCell = value;
                AddNodeAvatar();
                ShaderType = (mOldCell.isAttributes(MapEditor.CellType.Lucency)) ? EShareMatType.Transparent : EShareMatType.Normal;
                OnOldCellChange();
            }
        }
    }

    public CSCell NewCell
    {
        get
        {
            if (mNewCell == null)
                return mOldCell;
            if (mNextNode != null)
                mNewCell = mNextNode.cell as CSCell;
            return mNewCell;
        }
        set
        {
            mNewCell = value;
        }
    }

    public CSCell ServerCell
    {
        get
        {
            if (mServerCell == null)
                return NewCell;
            return mServerCell;
        }
        set
        {
            if ((mServerCell != null) && (value != null) && (mServerCell.Coord.Equal(value.Coord)))
                return;
            mServerCell = value;
        }
    }

    public bool IsUnitLoad
    {
        get { return mIsUnitLoad; }
        set
        {
            if (mIsUnitLoad != value)
            {
                mIsUnitLoad = value;
                if (mIsUnitLoad)
                {
                    AddNodeAvatar();
                }
            }
        }
    }


    void Awake()
    {
        avaterTransform = transform;
    }

    void Update()
    {
        UpdatePosition();
    }

    void OnDestroy()
    {

    }

    public void InitData()
    {

    }

    public void ResetOldCell(int x, int y)
    {
        CSCell cell = CSMesh.Instance.getCell(x, y);
        if (cell == null) cell = CSMesh.Instance.getCell(0, 0);
        if (cell != null)
        {
            OldCell = cell;
        }
    }

    public void ResetServerCell(int x, int y)
    {
        if (x == 0 && y == 0)
        {
            return;
        }
        CSCell cell = CSMesh.Instance.getCell(x, y);
        ServerCell = cell;
    }

    public bool IsMoving
    {
        get
        {
            return mIsMoving;
        }
        set
        {
            if (mIsMoving != value)
            {
                mIsMoving = value;
                if (avatarType == EAvatarType.MainPlayer)
                {
                    CSConstant.isMainPlayerMoving = value;
                }
            }
        }
    }

    public EShareMatType ShaderType
    {
        set
        {
            if (mShaderType != value)
            {
                mShaderType = value;
            }
        }
    }

    public int avatarType { get; set; }

    public Vector3 GetRealPosition2()
    {
        if (IsUnitLoad)
        {
            if (CacheTransform != null)
            {
                Vector3 vec = CacheTransform.position;
                vec.z = 0;
                return vec;
            }
        }
        else
        {
            Vector3 vec = OldCell.WorldPosition3;
            vec.z = 0;
            return vec;
        }
        return Vector3.zero;
    }

    public void OnOldCellChange()
    {

    }

    

    private void UpdatePosition()
    {
        if (CacheTransform == null) return;

        if (IsMoving && mNextNode != null)
        {
            if (OldCell.Coord.Equal(mNextNode.cell.Coord))
            {
                mIsMoveOne = true;
                return;
            }

            float f = Time.deltaTime * mSpeed;

            Vector3 targetPos = mNextNode.position;
            targetPos.z = 0;
            Vector3 vt3 = Vector3.MoveTowards(mCurMovePos, targetPos, f);
            mCurMovePos = vt3;
            vt3.z = CacheTransform.localPosition.z;

            vt3.x = (int)vt3.x;
            vt3.y = (int)vt3.y;

            CacheTransform.localPosition = vt3;

            float distanse = Vector3.Distance(mCurMovePos, targetPos);

            mIsMoveOne = (distanse <= 1);
        }
    }

    /// <summary>
    /// 只在Move开始的时候调用
    /// </summary>
    /// <param name="mSpeed"></param>
    /// <returns></returns>
    public float AdjustStepTime(float stepTime)
    {
        if (OldCell == null || ServerCell == null) return mSpeed;
        CSMisc.Dot2 dot = OldCell.Coord - ServerCell.Coord;
        dot = dot.Abs();
        int max = dot.x > dot.y ? dot.x : dot.y;
        if (max > 2)
        {
            stepTime = stepTime - stepTime * (max - 2) * 0.1f;
            if (stepTime < 0.1f) stepTime = 0.1f;
        }
        return stepTime;
    }

    public float GetAndAddSpeed(int direction, float stepTime, float speed)
    {
        switch (direction)
        {
            case CSDirection.Right:
            case CSDirection.Left:
                speed = (CSCell.Size.x / stepTime);
                break;
            case CSDirection.Up:
            case CSDirection.Down:
                speed = (CSCell.Size.y / stepTime);
                break;
            case CSDirection.Left_Down:
            case CSDirection.Left_Up:
            case CSDirection.Right_Up:
            case CSDirection.Right_Down:
                speed = (Mathf.Sqrt(CSCell.Size.y * CSCell.Size.y + CSCell.Size.x * CSCell.Size.x) / stepTime);
                break;
        }
        return speed;
    }

    protected void AddNodeAvatar()
    {
        if (mOldCell != null && mOldCell.node != null)
        {
            mOldCell.node.AddAvatarID(UnitID, avatarType, IsUnitLoad);
        }
    }

    protected void RemoveNodeAvatar(CSCell cell)
    {
        if (cell != null && cell.node != null)
        {
            cell.node.RemoveAvatarID(UnitID, avatarType);
        }
    }

}

