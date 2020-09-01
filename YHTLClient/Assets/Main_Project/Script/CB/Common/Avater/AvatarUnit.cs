using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarUnit
{
    public CSPathData PathData = new CSPathData();
    public CSAvatarState actState = new CSAvatarState();
    public GameObject Go;
    public BoxCollider box;
    protected Transform mAnchor;            //Go's Parent
    protected Transform mCacheTransform;    //Go.Transform
    protected Transform mCacheRootTransform;  //Go's Child
    protected CSModelModule mModelModule;
    protected EShareMatType mShaderType = EShareMatType.Normal;
    protected bool mIsShaderChange = false;
    protected bool mIsMoving = false;
    protected bool mIsMoveOne = false;
    protected bool mIsLoad = false;
    protected bool mIsDead = false;
    protected bool mIsMasterSelf = false;
    protected float mSpeed = 50f;
    protected float mAdjustStepTime = 0;
    protected int mModelHeight = 0;
    protected CSAvaterController avaterController;
    protected Vector3 mCurMovePos = Vector3.zero;
    protected Node mNextNode;
    protected int mAvatarType;
    protected CSObjectPoolItem mPoolItem;
    protected CSModel mModel;
    protected CSBetterList<Node> mPaths;
    private bool mIsUnitLoad;
    private long mUnitID;
    private CSCell mOldCell;
    private CSCell mNewCell;
    private CSCell mServerCell;
    public static Vector2 viewRange2 = new Vector2(11, 9);
    public System.Action<CSMisc.Dot2> onTowardsTarget = null;

    public CSMisc.Dot2 touchhCoord { get; set; }

    public int touchhDirection = CSDirection.None;// 触摸摇杆使用

    public float StepTime { get; set; }

    public bool IsDataSplit { get; set; }

    public bool IsDead
    {
        get { return mIsDead; }
    }

    public bool IsBeControl
    {
        get
        {
            return (actState.IsBeControl || actState.IsVertigo);
        }
    }

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

    public CSAvaterController AvaterController
    {
        set { avaterController = value; }
        get { return avaterController; }
    }

    public CSModel Model
    {
        get { return mModel; }
        set { mModel = value; }
    }

    public long UnitID
    {
        set { mUnitID = value; }
        get { return mUnitID; }
    }

    public long ID
    {
        get
        {
            return mUnitID;
        }
    }

    public bool IsMasterSelf
    {
        get
        {
            return mIsMasterSelf;
        }
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

                if(mAvatarType == EAvatarType.MainPlayer)
                {
                    CSMainParameterManager.mainPlayerOldCell = value;
                }
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
   
    public bool IsLoad
    {
        get { return mIsLoad; }
        set
        {
            if (mIsLoad != value)
            {
                mIsLoad = value;
                if (mIsLoad)
                {
                    AddNodeAvatar();
                }
            }
        }
    }

    public CSBetterList<Node> Paths
    {
        get { return mPaths; }
        set { mPaths = value; }
    }

    public virtual bool IsMoving
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
                if(AvatarType == EAvatarType.MainPlayer)
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
                IsShaderChange = true;
            }
        }
    }

    public bool IsShaderChange
    {
        get { return mIsShaderChange; }
        set { mIsShaderChange = value; }
    }

    public int AvatarType
    {
        get { return mAvatarType; }
        set { mAvatarType = value; }
    }

    public int GetAvatarTypeInt()
    {
        return mAvatarType;
    }

    public CSObjectPoolItem PoolItem
    {
        get { return mPoolItem; }
        set { mPoolItem = value; }
    }

    public Vector3 GetRealPosition2()
    {
        if (IsLoad)
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

    public bool IsCanSetAction
    {
        get
        {
            if (mModel == null) return false;
            if (mModel.Action.Motion == CSMotion.Dead) return true;
            if (IsDead) return false;
            return true;
        }
    }

    public virtual void OnOldCellChange()
    {
       
    }

    public void RefreshDepth()
    {
        RefreshDepthByCell(OldCell);
    }

    public void ResetDepth(int coordX, int cooordY)
    {
        CSCell cell = CSMesh.Instance.getCell(coordX, cooordY);
        RefreshDepthByCell(cell);
    }

    protected void RefreshDepthByCell(CSCell cell)
    {
        if (!IsLoad) return;
        if (CacheTransform == null) return;
        bool isDeadActionOver = IsDead && Model.EndOfAction("Dead", CSMotion.Dead);
        float depth = isDeadActionOver ? -5 : CSMisc.GetDepth(cell, 0, AvatarType);
        if (CacheTransform.localPosition.z != depth)
        {
            Vector3 p = CacheTransform.localPosition;
            CacheTransform.localPosition = new Vector3(p.x, p.y, depth);
        }
    }

    public virtual void InitModel()
    {
        if (Go == null)
        {
            Go = new GameObject();
            mCacheTransform = Go.transform;
            NGUITools.SetParent(mAnchor, Go);
            //root
            Transform rootTrans = CacheRootTransform != null ? CacheRootTransform : new GameObject("root").transform;
            CacheRootTransform = rootTrans;
            rootTrans.parent = mCacheTransform;
            rootTrans.localPosition = Vector3.zero;
            rootTrans.localScale = Vector3.one;
            if (mModelModule == null) mModelModule = rootTrans.gameObject.AddComponent<CSModelModule>();
            if (avaterController == null) avaterController = Go.AddComponent<CSAvaterController>();
            mModelModule.init(true, true, true, true, false, false, false, mAvatarType);
        }
    }

    public void UpdatePosition1()
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

    public void UpdatePosition2()
    {
        if (IsMoving && mNextNode != null)
        {
            mSpeed = CSPathData.GetAndAddSpeed(Model.Action.Direction, mAdjustStepTime);
        }

        if (mIsMoveOne)
        {
            mIsMoveOne = false;

            OldCell = mNextNode.cell as CSCell;

            mNextNode = NextTarget();

            if (mNextNode != null)
            {
                NewCell = mNextNode.cell as CSCell;

                Model.SetDirection(mNextNode.position, OldCell);

                if (mNextNode.cell.isAttributes(MapEditor.CellType.Resistance))
                {
                    SetAction2(CSMotion.Stand);
                    return;
                }
                else
                {
                    SetAction2(CSMotion.Run);
                }
            }

            if (mNextNode == null)
            {
                IsMoving = false;
                NewCell = OldCell;
            }
        }
    }

    public void MoveInitBase()
    {
        //TODO:ddn
        if (CacheTransform == null) return;
        mPaths = PathData.GetPathNode();
        mAdjustStepTime = AdjustStepTime();
        if (IsMoving) return;
        mNextNode = NextTarget();
        mCurMovePos = CacheTransform.localPosition;
        mCurMovePos.z = 0;
    }

    public void MoaveInItBase1()
    {
        if (mNextNode != null)
        {
            Model.SetDirection(mNextNode.position, OldCell);

            if (mNextNode.cell.isAttributes(MapEditor.CellType.Resistance))
            {
                SetAction2(CSMotion.Stand);
                return;
            }
            SetAction2(CSMotion.Run);
            IsMoving = true;
        }
    }

    public virtual Node NextTarget()
    {
        Node node = null;
        if (mPaths != null)
        {
            if (mPaths.Count > 0)
            {
                node = mPaths[0];
                node.cell = CSMesh.Instance.getCell(node.coord.x, node.coord.y);
                NewCell = node.cell as CSCell;
                mPaths.RemoveAt(0);
            }
        }
        return node;
    }

    //TODO:ddn
    public void UpdateAdjusetClientAndServerPos()
    {
        if (!IsMoving)
        {
            if (IsBeControl) return;

            if (OldCell == null || ServerCell == null) return;

            if (!IsCellEqual())
            {
                if (!IsAttackPlaying() && !IsDead)
                {
                    CSMisc.Dot2 dot = OldCell.Coord - ServerCell.Coord;
                    dot = dot.Abs();
                    if (dot.x >= viewRange2.x || dot.y >= viewRange2.y)
                    {
                        ResetOldCell(ServerCell.Coord.x, ServerCell.Coord.y);
                        NewCell = OldCell;
                        SetPosition(NewCell.WorldPosition);
                        OnOldCellChange();
                        SetShaderName();
                    }
                    else
                    {
                        if (IsLoad)
                        {
                            onTowardsTarget?.Invoke(ServerCell.Coord);
                        }
                        else
                        {
                            ResetOldCell(ServerCell.Coord.x, ServerCell.Coord.y);
                        }
                    }
                }
            }
        }
    }

    public void UpdateShaderName()
    {
        if (mIsShaderChange)
        {
            mIsShaderChange = false;
            SetShaderName();
        }
    }

    /// <summary>
    /// 只在Move开始的时候调用
    /// </summary>
    /// <param name="mSpeed"></param>
    /// <returns></returns>
    public  float AdjustStepTime(float stepTime)
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

    public void InitBox()
    {
        if (box == null)
        {
            box = mCacheRootTransform.gameObject.AddComponent<BoxCollider>();
        }
    }

    public void RemovePoolItem()
    {
        if (CSObjectPoolMgr.Instance != null)
        {
            CSObjectPoolMgr.Instance.RemovePoolItem(mPoolItem);
        }
        mPoolItem = null;
    }

    protected void AddNodeAvatar()
    {
        if (mOldCell != null && mOldCell.node != null)
        {
            mOldCell.node.AddAvatarID(UnitID, AvatarType,IsLoad);
        }
    }

    protected void RemoveNodeAvatar()
    {
        RemoveNodeAvatar(OldCell);
        RemoveNodeAvatar(ServerCell);
    }

    protected void RemoveNodeAvatar(CSCell cell)
    {
        if (cell != null && cell.node != null)
        {
            cell.node.RemoveAvatarID(UnitID, AvatarType);
        }
    }

    public bool IsCellEqual()
    {
        return OldCell.Coord.Equal(ServerCell.Coord);
    }

    public bool IsAttackPlaying()
    {
        if (mModel == null) return false;

        return mModel.IsAttackPlaying();
    }

    public void SetPosition(Vector2 worldPosition)
    {
        if (CacheTransform != null)
        {
            CacheTransform.position = worldPosition;
        }
        else
        {
            UnityEngine.Debug.LogError("======> CSAvatar CacheTransform is null");
        }
    }

    public float GetModelHeight()
    {
        if(mModelHeight != 0)
        {
            return mModelHeight;
        }
        if (Model != null)
        {
            return Model.GetModelHeight();
        }
        return 0;
    }

    public int GetDirection()
    {
        if(Model != null)
        {
           return Model.GetDirection();
        }
        return CSDirection.None;
    }

    public void SetModelAtlas(ModelLoadBase p)
    {
        mModel.SetModelAtlas(p);
        OnOldCellChange();
        SetShaderName();
        if(AvatarType == EAvatarType.MainPlayer)
        {
            CSConstant.IsLanuchMainPlayer = true;
        }
    }

    public  void SetShaderName()
    {
        EShareMatType type = mShaderType;

        if (actState.IsHiding)
        {
            type = EShareMatType.Transparent;
        }

        CSMisc.color.r = CSMisc.color.g = CSMisc.color.b = CSMisc.color.a = 1;

        if (type == EShareMatType.Transparent)
        {
            CSMisc.color.a = 0.45f;
        }
        else if (type == EShareMatType.DeadTransparent)
        {
            CSMisc.color.a = 0.8f;
        }
        else
        {
            CSMisc.color.a = 1;
        }

        CSMisc.blackColor.a = type == EShareMatType.Transparent ? 0.3f : 0.5f;
        CSMisc.greyColor.r = CSMisc.greyColor.g = CSMisc.greyColor.b = CSMisc.greyColor.a = 1;
        EShareMatType newType = type == EShareMatType.DeadTransparent ? EShareMatType.Transparent : type;

        switch (actState.ColorType)
        {
            case 1:
                {
                    newType = type == EShareMatType.Transparent ? EShareMatType.ColorSet_Red_Transparent : EShareMatType.ColorSet_Red;
                    CSMisc.color = CSMisc.ColorRed;
                }
                break;
            case 2:
                {
                    newType = type == EShareMatType.Transparent ? EShareMatType.ColorSet_Green_Transparent : EShareMatType.ColorSet_Green;
                    CSMisc.color = CSMisc.ColorGreen;
                }
                break;
            case 3:
                {
                    newType = type == EShareMatType.Transparent ? EShareMatType.ColorSet_Grey_Transparent : EShareMatType.ColorSet_Grey;
                    CSMisc.greyColor = CSMisc.ColorGrey;
                }
                break;
            default:
                break;
        }


        if (Model != null)
        {
            EShareMatType blackType = type == EShareMatType.Transparent ? EShareMatType.Balck_Transparent : EShareMatType.Balck;
            if (Model.Body != null)
            {
                Model.Body.SetShareMat(blackType, CSShaderManager.GetShareMaterial(Model.Body.GetShareMatObj(), blackType), CSMisc.blackColor, CSMisc.greyColor);
                Model.Body.SetShareMat(type, CSShaderManager.GetShareMaterial(Model.Body.GetShareMatObj(), newType), CSMisc.color, CSMisc.greyColor);
            }
            if (Model.Weapon != null)
            {
                Model.Weapon.SetShareMat(blackType, CSShaderManager.GetShareMaterial(Model.Weapon.GetShareMatObj(), blackType), CSMisc.blackColor, CSMisc.greyColor);
                Model.Weapon.SetShareMat(type, CSShaderManager.GetShareMaterial(Model.Weapon.GetShareMatObj(), newType), CSMisc.color, CSMisc.greyColor);
            }
            if (Model.Wing != null)
            {
                Model.Wing.SetShareMat(blackType, CSShaderManager.GetShareMaterial(Model.Wing.GetShareMatObj(), blackType), CSMisc.blackColor, CSMisc.greyColor);
                Model.Wing.SetShareMat(type, CSShaderManager.GetShareMaterial(Model.Wing.GetShareMatObj(), newType), CSMisc.color, CSMisc.greyColor);
            }
        }
    }

    public void SetStepTime(int speed)
    {
        StepTime = speed * 0.001f;
    }

    public void SetIsDead(bool isDead)
    {
        mIsDead = isDead;
    }

    public float AdjustStepTime()
    {
        if (IsBeControl) return mSpeed;
        return AdjustStepTime(StepTime);
    }

    public void SetAction2(int motion, int stopType = EActionStopFrameType.None)
    {
        if (mModel == null) return;

        if (motion != CSMotion.Dead && IsDead) return;

        mModel.SetAction(motion, stopType);
    }

    public void Initialize(bool isDataSplit, int modelHeight, bool isDead, bool isMasterSelf)
    {
        IsDataSplit = isDataSplit;
        mModelHeight = modelHeight;
        mIsDead = isDead;
        mIsMasterSelf = isMasterSelf;
    }
}
