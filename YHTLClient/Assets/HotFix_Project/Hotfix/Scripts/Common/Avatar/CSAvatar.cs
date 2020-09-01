using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CSAvatar : AvatarUnit
{
    public TouchData TouchEvent ;
    protected CSSkillEngine mSkillEngine;
    protected BehaviourProvider mBehaviour;
    protected FSMState mFSM;
    protected CSAvatarBuffEffect mAvatarBuffEffect = null;
    protected CSSkillResult mSkillResult = null;
    private CSAvatarGo mAvatarGo;
    private CSAvatarInfo mBaseInfo;
    private ModelLoadBase mModelLoad;
    private int mTouchMove = EControlState.Idle;
    private CSDisplacementSkill mDisplacementSkill = null;
    private bool mIsReplaceEquip = false;
    private bool mIsServerDead = false;
  
    public CSHead head { get; set; }

    public bool isInView { get; set; }

    public string GetName() { return BaseInfo.Name; }

    public CSAvatarGo AvatarGo
    {
        get { return mAvatarGo; }
        set { mAvatarGo = value; }
    }
    
    public CSSkillEngine SkillEngine
    {
        get { return mSkillEngine; }
    }

    public virtual bool IsCanBeSelectAttack()
    {
        return true;
    }

    public CSAvatarInfo BaseInfo
    {
        get { return mBaseInfo; }
        set
        {
            mBaseInfo = value;
            if(mBaseInfo != null)
            {
                UnitID = mBaseInfo.ID;
            }
        }
    }

    public ModelLoadBase ModelLoad
    {
        get { return mModelLoad; }
        set { mModelLoad = value; }
    }

    public bool IsServerDead
    {
        get
        {
            return mIsServerDead;
        }
        set
        {
            mIsServerDead = value;
            if(mIsServerDead)
            {
                if(Model != null)
                {
                    Model.DestroyBottom();
                }
            }
        }
    }

    public FSMState FSM
    {
        get { return mFSM; }
        set { mFSM = value; }
    }

    public int TouchMove
    {
        get { return mTouchMove; }
        set{ mTouchMove = value;}
    }

    public bool IsReplaceEquip
    {
        get { return mIsReplaceEquip; }
        set{ mIsReplaceEquip = value; }
    }

    public CSDisplacementSkill DisplacementSkill
    {
        get
        {
            if (mDisplacementSkill == null)
            {
                mDisplacementSkill = new CSDisplacementSkill();
            }
            return mDisplacementSkill;
        }
    }

    public CSAvatarBuffEffect GetAvatarBuffEffect()
    {
        if(mAvatarBuffEffect == null)
        {
            mAvatarBuffEffect = new CSAvatarBuffEffect();
        }
        return mAvatarBuffEffect;
    }

    public CSSkillResult GetSkillResult()
    {
        if(mSkillResult == null)
        {
            mSkillResult = new CSSkillResult();
        }
        return mSkillResult;
    }

    public bool IsHiding
    {
        get
        {
            return (actState.IsHiding);
        }
    }

    public CSAvatar()
    {
        TouchEvent = new TouchData(this);
    }

    public virtual void Init(object data, Transform transAnchor)
    {
        if(data != null)
        {
            mAnchor = transAnchor;
        }
        onTowardsTarget = TowardsTarget;
        mIsServerDead = false;
        isInView = true;
    }

    public virtual void InitAvatarGo()
    {
    }

    public void InitState()
    {
        if (BaseInfo != null && BaseInfo.BuffInfo != null)
        {
            BaseInfo.BuffInfo.UpdateState(actState);
        }
    }

    public void ShowAvatarGo()
    {
        if (Go != null)
        {
            Go.SetActive(isInView);
            if(!isInView)
            {
                if(Model != null)
                {
                    Model.DestroyBottom();
                }
            }
        }
    }

    public void DestroyBottom()
    {
        if (Model != null)
        {
            Model.DestroyBottom();
        }
    }

    public virtual void InitHead()
    {
        
    }

    public virtual void InitSkillEngine()
    {
        if(mSkillEngine == null)
        {
            mSkillEngine = new CSSkillEngine(this);
            mSkillEngine.Init();
        }
    }

    public virtual void Update()
    {
        if (CSResourceManager.Instance.mIsChangingScene) return;

        //UpdateViewModel();

        if (!mIsLoad) return;

        if (!IsCanSetAction) return;

        //if (!IsDead) base.Update();

        if(!mIsServerDead)
        {
            UpdatePosition();

            UpdateAdjusetClientAndServerPos();

            mFSM?.UpdateBehaviors();

            mDisplacementSkill?.Update();
        }
        UpdateShaderName();

        if (mIsReplaceEquip)
        {
            mIsReplaceEquip = false;
            ReplaceEquip();
        }
    }

    protected virtual void UpdatePosition()
    {
        UpdatePosition1();
    }

    protected virtual void UpdateViewModel()
    {

    }

    public virtual void Show(bool value)
    {
        if (Model == null || CacheRootTransform == null)
        {
            return;
        }
        if (value)
        {
            if (OldCell != null)
            {
                SetPosition(OldCell.WorldPosition);
                OnOldCellChange();
                if (!IsServerDead)
                {
                    SetAction(CSMotion.Stand);
                }
            }
        }
        else
        {
            IsMoving = false;
            mIsMoveOne = false;
            if (SkillEngine != null)
            {
                SkillEngine.StopAll();//如果玩家不可见，停止技能特效
            }
        }
        Model.Show(value);
    }

    protected void replaceEquip(bool b)
    {
        IsReplaceEquip = b;
    }

    public virtual void Stop(bool isForceStop = false, bool isSetAction = true)//当走过一格格子的时候，如果能释放技能，强制将移动数据清空，停止移动
    {
        MoveTargetStop(isForceStop, isSetAction);
    }

    public virtual void MoveTargetStop(bool isForceStop = false, bool isSetAction = true)
    {
        if (mNextNode != null && !OldCell.Coord.Equal(mNextNode.coord))
        {
            PathData?.Clear();
            mPaths?.Clear();
            if (isForceStop)
            {
                mNextNode = null;
                IsMoving = false;
                if (isSetAction) SetAction(CSMotion.Stand);
            }
        }
        else
        {
            PathData?.Clear();
            mPaths?.Clear();
            IsMoving = false;
            if (Model != null)
            {
                if (isSetAction) SetAction(CSMotion.Stand);
            }
            else
            {
                if (FNDebug.developerConsoleVisible) FNDebug.Log(BaseInfo.Name + " Model = " + Model);
            }
        }
    }

    public void SetAction(int motion,int stopType = EActionStopFrameType.None)
    {
        if (mModel == null) return;

        if (motion != CSMotion.Dead && IsDead) return;

        if (AvatarType == EAvatarType.MainPlayer)
        {
            CSCharacter chr = this as CSCharacter;
            if (motion == CSMotion.Stand)
            {
                chr.EndMoveAudio();
                chr.LastStandTime = Time.time;
                CSConstant.lastStandTimeUnloadAsset = Time.time;
            }
            else
            {
                CSConstant.lastStandTimeUnloadAsset = 0;
                chr.LastStandTime = 0;
            }
        }
        mModel.SetAction(motion, stopType);
    }

    public virtual CSBetterList<Node> GetPath()
    {
        return CSPaths.getPath(this);
    }

    public virtual void MoveInit()
    {
        MoveInitBase();
    }

    public virtual void TowardsTarget(CSMisc.Dot2 targetCoord)
    {
        if (!IsAttackPlaying())
        {
            TouchEvent.Type = ETouchType.Normal;
            TouchEvent.Coord = targetCoord;
            FSM.Switch2(CSMotion.Run, false);
        }
    }

    public virtual void TowardsTargetAttack(CSMisc.Dot2 targetCoord)
    {
        if (!IsAttackPlaying())
        {
            TouchEvent.Type = ETouchType.Attack;
            TouchEvent.Coord = targetCoord;
            FSM.Switch2(CSMotion.Run, false);
        }
    }

    public virtual void Attack(Vector3 target)
    {
        Model.SetDirection(target,OldCell);
        SetAction(CSMotion.Attack);
    }

    public virtual void Attack2(Vector3 target)
    {
        Model.SetDirection(target, OldCell);
        SetAction(CSMotion.Attack2);
    }

    public virtual void RunToStand()
    {
        SetAction(CSMotion.RunToStand,EActionStopFrameType.End);
        FSM.Switch2(CSMotion.RunToStand, false);
    }

    public virtual void StandToRun()
    {
        SetAction(CSMotion.StandToRun, EActionStopFrameType.End);
        FSM.Switch2(CSMotion.StandToRun, false);
    }

    public virtual void Dead()
    {
        Stop(true);
        mDisplacementSkill?.StopYeMain();
        RemoveNodeAvatar();
        SetAction(CSMotion.Dead, EActionStopFrameType.LastFrame);
    }

    public virtual void ResetPosition(CSMisc.Dot2 coord)
    {

    }

    public virtual void DetectForceSendMoveRequest()
    {

    }

    /// <summary>
    /// 如果是移动的物体：Update中动态调整层次，如果是静态的，可以直接重写getPosition，在AvatarGO中使用
    /// </summary>
    public override void OnOldCellChange()
    {
        RefreshDepth();
    }

    public virtual void ReplaceEquip()
    {

    }

    public virtual void OnHpChange(uint evtId, object obj)
    {
        if(BaseInfo.HP <= 0)
        {
            SetIsDead(true);
            Dead();
        }
        else
        {
            SetIsDead(false);
        }
        IsServerDead = (BaseInfo.HP <= 0);
    }

    public bool EndOfAction()
    {
        if (Model != null)
        {
            return Model.EndOfAction();
        }
        return false;
    }

    public virtual Vector3 GetPosition()
    {
        if (OldCell != null)
        {
            return OldCell.LocalPosition2;
        }

        return Vector3.zero;
    }

    public virtual void ClearPath()
    {
        if (mPaths != null)
        {
            OldCell = null;
            NewCell = null;
            mPaths.Clear();
        }
    }

    public virtual void SetActive(bool value)
    {
        if(CacheTransform != null)
        {
            CacheTransform.localScale = (value) ? Vector3.one : Vector3.zero;
        }
    }

    public void SetTouchhCoord(CSMisc.Dot2 coord)
    {
        this.touchhCoord = coord;
    }

    public bool IsAttackMotion()
    {
        if (mModel == null) return false;

        return mModel.IsAttackMotion();
    }

    public void AddBuffEffect(int effectId)
    {
        if(mModel != null && mModel.Effect != null && mModel.Effect.GoTrans)
        {
            if (mAvatarBuffEffect == null)
            {
                mAvatarBuffEffect = new CSAvatarBuffEffect();
            }
            mAvatarBuffEffect.Add(mModel.Effect.GoTrans,effectId);
        }
    }

    public void RemoveBuffEffect(int effectId)
    {
        mAvatarBuffEffect?.Remove(effectId);
    }

    public void PlayHurtEffect()
    {
        mSkillResult?.OnSkillEffect();
    }

    public virtual void Destroy()
    {
        IsLoad  = false;
        IsServerDead = true;
        actState?.ResetAll();
        mModel?.Destroy();
        head?.Destroy();
        Go?.SetActive(false);
        mBaseInfo?.Release();
        mDisplacementSkill?.Destroy();
        mAvatarBuffEffect?.Destroy();
        mSkillResult?.Destroy();
        ModelLoad?.Destroy();
        SkillEngine?.Destroy();
        RemovePoolItem();
        RemoveNodeAvatar();
    }

    public virtual void Release()
    {
        mBaseInfo?.Release();
        PathData?.Release();
        mFSM?.Release();
        mModel?.Release();
        mPaths?.Release();
        head?.Release();
        mSkillEngine?.Release();
        mModelLoad?.Release();
        mModelLoad?.Destroy();
        mPoolItem = null;
    }
}
