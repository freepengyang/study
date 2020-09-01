using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSCharacter : CSAvatarBase<CSPlayerInfo>
{
    public float changeSceneJoyCanMoveTime;
    private CSCharacterPath mPathFind = null;
    private CSMisc.Dot2 mMoveRequestCoord;
    private CSBetterList<Node> mWillWalkPathList = new CSBetterList<Node>();//当前将要走的路径base.Path+NextNode
    private float mLastStandTime = 0;
    private bool mIsDetectContineMove = false;
    private CSMisc.Dot2 mMissionTargetCoord = CSMisc.Dot2.Zero;
    private CSSceneEffect effect6022;
    private bool oldeffect;
    private bool isActivePet = false;
    private CSMisc.Dot2 mWaitSkillContineMoveDot = CSMisc.Dot2.Zero;
    private CSAudio mMoveAudio = null;
    private CSSceneEffect effect6023;
    private EventHanlderManager mClient = new EventHanlderManager(EventHanlderManager.DispatchType.Event);
    private MainEventHanlderManager mMainEventHandler = new MainEventHanlderManager(MainEventHanlderManager.MainDispatchType.Event);
    private const int ActiveEffect6023 = 6023;
    private const int ActiveEffect6024 = 6024;
    private const int PermanentEffect6022 = 6022;
   

    public bool IsDetectContineMove
    {
        get { return mIsDetectContineMove; }
        set { mIsDetectContineMove = value; }
    }

    public float LastStandTime
    {
        get { return mLastStandTime; }
        set { mLastStandTime = value; }
    }

    public CSMisc.Dot2 MoveRequestCoord
    {
        set { mMoveRequestCoord = value; }
    }

    public override void Init(object data, Transform transAnchor)
    {
        CSMainPlayerInfo info = data as CSMainPlayerInfo;
        if(info == null)
        {
            return;
        }
        base.Init(data, transAnchor);
        base.Info = info;
        BaseInfo = base.Info;
        AvatarType = EAvatarType.MainPlayer;
        TouchMove = EControlState.Idle;

        if (ModelLoad == null) ModelLoad = new MainPlayerModelLoad();

        if (mFSM == null) mFSM = new FSMState();

        if (mBehaviour == null) mBehaviour = new CSCharacterBehavior(this);

        if (mPathFind == null) mPathFind = new CSCharacterPath(this);

        mBehaviour.InitializeFSM(mFSM);

        mFSM.Start(CSMotion.Stand);
 
        TouchEvent.Owner = this;
        InitModel();
        ResetPosition(BaseInfo.Coord);
        InitAvatarGo();
        InitSkillEngine();
        InitEvent();
        InitHead();
        SetStepTime((int)info.Speed);
        bool isDead = Info.HP <= 0;
        Initialize(true,0, isDead,true);

    }
    
    /// <summary>
    /// 护盾常驻特效
    /// </summary>
    public void InitShieldEffect(bool isShield)
    {
        if (Info.HasShield && !isActivePet)
        {
            if (effect6022 == null)
            {
                Transform anchor = Model.Effect.GoTrans;
                effect6022 = CSSceneEffectMgr.Instance.Create(anchor, PermanentEffect6022, Vector3.zero);
            }
            effect6022.SetAvtive(isShield);
        }
    }
    
    private void OnInitShieldEffect(uint id, object data)
    {
        if (data is bool isShield)
        {
            InitShieldEffect(isShield);
        }
    }
    
    /// <summary>
    /// 护盾激活
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void OnActiveShield(uint id, object data)
    {
        isActivePet = true;
        Transform anchor = Model.Effect.GoTrans;
        if (effect6023 == null)
        {
            effect6023 = CSSceneEffectMgr.Instance.Create(anchor, ActiveEffect6023, Vector3.zero);
            CSSceneEffectMgr.Instance.PlayEffect(anchor, ActiveEffect6024);
            effect6023.SetPlayFinishedCallBack(() =>
            {
                if (effect6022==null)
                    effect6022 = CSSceneEffectMgr.Instance.Create(anchor, PermanentEffect6022, Vector3.zero);
                isActivePet = false;
            });
        }
    }

    public override void InitModel()
    {
        base.InitModel();
        if (Model == null) Model = new CSModel(CacheRootTransform,box,AvatarType,IsDataSplit, mShaderType,replaceEquip);
        Model.SetDirection(CSDirection.Down);
        SetAction(CSMotion.Stand);
        Model.InitAnimationFPS(Info.BodyModel);
    }

    public override void InitAvatarGo()
    {
        if(Go != null)
        {
            base.InitAvatarGo();
            if (AvatarGo == null)
            {
                AvatarGo = Go.AddComponent<CSCharacterGo>();
            }
            AvatarGo.Init(this);
        }
    }

    private void InitEvent()
    {
        if(mClient == null)
        {
            mClient = new EventHanlderManager(EventHanlderManager.DispatchType.Event);
        }
        if(mMainEventHandler == null)
        {
            mMainEventHandler = new MainEventHanlderManager(MainEventHanlderManager.MainDispatchType.Event);
        }
        mMainEventHandler.AddEvent(MainEvent.Avatar_AttachBottom, OnAttachBottom);
        mMainEventHandler.AddEvent(MainEvent.Avatar_AttachBottomNPC, OnAttachBottomNPC);

        //mClient.AddEvent(CEvent.Avatar_AttachBottom, OnAttachBottom);
        //mClient.AddEvent(CEvent.Avatar_AttachBottomNPC, OnAttachBottomNPC);
        ClientHanlderManager clientEventHandler = BaseInfo.EventHandler;
        clientEventHandler.AddEvent(CEvent.HP_Change, OnHpChange);
        clientEventHandler.AddEvent(CEvent.Player_Relive, OnRelive);
        clientEventHandler.AddEvent(CEvent.Player_ReplaceEquip, OnReplaceEquip);
        clientEventHandler.AddEvent(CEvent.MP_Change, OnCharacterMpChange);
        clientEventHandler.AddEvent(CEvent.ActiveShield, OnActiveShield);
        clientEventHandler.AddEvent(CEvent.ShieldChange,OnInitShieldEffect);
    }

    public override void InitSkillEngine()
    {
        base.InitSkillEngine();
    }

    public override void InitHead()
    {
        if (head == null)
        {
            //TODO:ddn
            CSResourceManager.Singleton.AddQueue("actor_player",
                ResourceType.ResourceRes, OnLoadHead, ResourceAssistType.ForceLoad);
        }
        else
        {
            head.Init(this);
        }
    }

    public override void ResetPosition(CSMisc.Dot2 coord)
    {
        mNextNode = null;
        BaseInfo.Coord = coord;
        ResetServerCell(coord.x,coord.y);
        ResetOldCell(coord.x, coord.y);
        NewCell = OldCell;
        CSTerrain.Instance.ResetPosition(NewCell);
        SetPosition(NewCell.WorldPosition);
        MoveRequestCoord = OldCell.Coord;
        OnOldCellChange();
    }

    public void AdjustPosition(CSMisc.Dot2 coord)
    {
        changeSceneJoyCanMoveTime = Time.time + 0.6f;
        ClearPath();
        ResetPosition(coord);
        SetShaderName();
        CSCameraManager.Instance.ResetCameraPosition();
        CSTerrain.Instance.refreshDisplayMeshCoord(OldCell);
    }

    public void ChangeScene(CSMisc.Dot2 coord)
    {
        changeSceneJoyCanMoveTime = Time.time + 0.6f;
        ClearPath();
        ResetOldCell(coord.x, coord.y);
        ResetServerCell(coord.x, coord.y);
        MoveRequestCoord = coord;
        NewCell = null;
        CSCameraManager.Instance.IsInitCamera = false;
        actState.ResetAll();
        //MoveState = EMoveState.initiative;
        mPathFind = null;
        base.MoveTargetStop(true, true);
        if (Model != null)
        {
            Model.Destroy();
        }
        mAvatarBuffEffect?.Destroy();
        if (mBehaviour != null)
        {
            mBehaviour.Reset();
        }
        if(TouchEvent != null)
        {
            TouchEvent.Clear();
        }
        EndMoveAudio();
        UnSaveWaitData();

        CSSceneEffectMgr.Instance.Destroy(effect6022);
        effect6022?.Destroy();
        if (effect6022!=null)
            effect6022 = null;
    }

    public override void MoveInit()
    {
        if(Time.time < changeSceneJoyCanMoveTime) return;

        PathData.PathArray = GetPath();

        if (IsHasMoveToServerPos())
        {
            if (mPathFind != null)
            {
                mPathFind.AdjustAttackPath(OldCell);
            }
        }

        if (PathData.PathArray.Count <= 0)
        {
            if (mPaths != null)
            {
                int index = FindMoveRequesetReqIndex(mPaths);
                RemoveMoveRequestReqPaths(mPaths, index + 1);
            }
            return;
        }

        base.MoveInit();

        if (base.Paths != null)
        {
            AddWiatWalkPath();
        }
        if (mNextNode != null)
        {
            Model.SetDirection(mNextNode.position,OldCell);

            if (mNextNode.cell.isAttributes(MapEditor.CellType.Resistance))
            {
                SetAction(CSMotion.Stand);
                return;
            }
            SetAction(CSMotion.Run);
            mWillWalkPathList.Insert(0, mNextNode);
            if(!IsMoving)
            {
                DetectSendMoveRequest(true);
            }
            if(IsMoving != true)
            {
                PlayMoveAudio();
            }
            IsMoving = true;
           
        }
    }

    public override void Update()
    {
        base.Update();
       
        CSCameraManager.Instance.UpdateCameraPosition();

        UpdateTowardsMissionTarget();

        if (mIsDetectContineMove)
        {
            mIsDetectContineMove = false;
            ContineMove();
        }
    }

    void RemoveMoveRequestReqPaths(CSBetterList<Node> list, int index)
    {
        if (list == null) return;
        list.RemoveRange(index);
    }

    protected override void UpdatePosition()
    {
        if (IsBeControl)
        {
            return;
        }
        base.UpdatePosition();

        if (IsMoving && mNextNode != null)
        {
            mSpeed = CSPathData.GetAndAddSpeed(Model.Action.Direction, StepTime);
        }

        if (mIsMoveOne)
        {
            mIsMoveOne = false;

            OldCell = mNextNode.cell as CSCell;

            //去掉当前走过的Cell
            mNextNode = NextTarget();

            if(Paths != null)
            {
                AddWiatWalkPath();
            }

            if (mNextNode != null)
            {
                BaseInfo.EventHandler.SendEvent(CEvent.MainPlayer_CellChangeTrigger, OldCell);

                mWillWalkPathList.Insert(0,mNextNode);

                if (IsMoving)
                {
                    NewCell = mNextNode.cell as CSCell;

                    Model.SetDirection(mNextNode.position,OldCell);

                    if (mNextNode.cell.isAttributes((int)MapEditor.CellType.Resistance))
                    {
                        SetAction(CSMotion.Stand);
                        return;
                    }
                    else
                    {
                        SetAction(CSMotion.Run);
                    }
                }
                else
                {
                    mNextNode = null;
                    NewCell = OldCell;
                    return;
                }
            }
            else
            {
             
                IsMoving = false;
                NewCell = OldCell;
                SetAction(CSMotion.Stand);
                mClient.SendEvent(CEvent.Reach);
                BaseInfo.EventHandler.SendEvent(CEvent.MainPlayer_StopTrigger, OldCell);
            }
            DetectSendMoveRequest();
            CSTerrain.Instance.refreshDisplayMeshCoord(OldCell);
        }
    }

    public void TowardsTarget(int direction)
    {
        if (IsBeControl)
        {
            return;
        }
        if (!IsHasMoveToServerPos())
        {
            UnSaveWaitData();
            return;
        }
        UnSaveWaitData();
        TouchEvent.Type = ETouchType.Touch;
        TouchEvent.Direction = direction;
        CSAutoFightManager.Instance.Stop();
        FSM.Switch2(CSMotion.Run, false);
    }

    public void TowardsMissionTarget(CSMisc.Dot2 targetCoord)
    {
        if (IsBeControl)
        {
            return;
        }
        mMissionTargetCoord = targetCoord;
    }

    private void UpdateTowardsMissionTarget()
    {
        if (mMissionTargetCoord.x != 0 || mMissionTargetCoord.y != 0)
        {
            bool isTowardsMissionTarget = false;
            if (IsAttackMotion())
            {
                isTowardsMissionTarget = (!IsAttackPlaying());
            }
            else if (EndOfAction())
            {
                isTowardsMissionTarget = true;
            }

            if (isTowardsMissionTarget)
            {
                TowardsTarget(mMissionTargetCoord);
                mMissionTargetCoord = CSMisc.Dot2.Zero;
            }
        }
    }

    public void SaveTowardsTarget()
    {
        if (TouchMove == EControlState.Idle && IsMoving)
        {
            mWaitSkillContineMoveDot = TouchEvent.Coord;
        }
    }

    public void UnSaveTowardTarget()
    {
        mWaitSkillContineMoveDot = CSMisc.Dot2.Zero;
    }

    public void UnSaveWaitData()
    {
        mWaitSkillContineMoveDot = CSMisc.Dot2.Zero;
    }

    public void ContineMove()
    {
        if (!mWaitSkillContineMoveDot.Equal(CSMisc.Dot2.Zero))
        {
            if (TouchMove == EControlState.Idle)
            {
                TouchEvent.Type = ETouchType.Normal;
                TowardsTarget(mWaitSkillContineMoveDot);
            }
            mWaitSkillContineMoveDot = CSMisc.Dot2.Zero;
        }
    }

    public override void Stop(bool isForceStop = false, bool isSetAction = true)
    {
        if(!IsAttackPlaying() || isForceStop)
        {
            if (!IsHasMoveToServerPos())//由Update同步
            {
                if (mPaths != null)
                {
                    int index = FindMoveRequesetReqIndex(mPaths);
                    for (int i = mPaths.Count - 1; i > index; i--)
                    {
                        mPaths.RemoveAt(i);
                    }
                    AddWiatWalkPath();
                    mWillWalkPathList.Insert(0, NewCell.node);
                }

                TouchEvent.Coord = mMoveRequestCoord;
                return;
            }
            base.Stop(isForceStop, isSetAction);
            mWillWalkPathList.Clear();
        }
    }

    public void DetectSendMoveRequest(bool isMoveBeginDetect = false)
    {
        if(IsBeControl || IsDead || IsServerDead)
        {
            return;
        }
        if (mMoveRequestCoord.x == 0 && mMoveRequestCoord.y == 0)
        {
            MoveRequestCoord = OldCell.Coord;
        }

        bool isCanCrossScene = false;
        bool isCanSendTwo = TouchMove == EControlState.Idle;
        if (mMoveRequestCoord.Equal(OldCell.Coord))
        {
            if (mWillWalkPathList.Count > 0)
            {
                Node node_0 = mWillWalkPathList[0];
                CSMisc.Dot2 dir_0 = node_0.coord - OldCell.Coord;
                dir_0 = dir_0.Normal();
                if (!dir_0.Equal(CSMisc.Dot2.Zero))
                {
                    if (mWillWalkPathList.Count > 1 && !isMoveBeginDetect && isCanSendTwo)//刚启动时，第一个发一格
                    {
                        Node node_1 = mWillWalkPathList[1];
                        CSMisc.Dot2 dir_1 = node_1.coord - node_0.coord;
                        dir_1 = dir_1.Normal();
                        if (!dir_1.Equal(CSMisc.Dot2.Zero))
                        {
                            if (dir_0.Equal(dir_1) && node_1.isCanCrossNpc
                                && ((isCanCrossScene && node_1.isType18) || node_1.isProtect || node_1.avatarNum == 0))
                            {
                                bool isSendOne = false;
                                if (TouchEvent.Type == ETouchType.Attack && TouchEvent.Target != null && mPaths.Count > 0)
                                {
                                    TABLE.SKILL tblSkill = TouchEvent.GetTableSkill();
                                    if(tblSkill != null)
                                    {
                                        isSendOne = CSSkillTools.GetBestLaunchCoord(tblSkill.effectArea, tblSkill.clientRange, tblSkill.effectRange, node_1.coord, mPaths[mPaths.Count - 1].coord);
                                    }
                                }
                                if (isSendOne || CSPathFinderManager.IsAutoFinPath)
                                {
                                    SendMoveRequest(node_0, IsRun(node_0.coord, mMoveRequestCoord), EMoveRequestType.NormalSend);
                                }
                                else if (TouchEvent.Type == ETouchType.Attack && TouchEvent.Target != null && TouchEvent.Target.OldCell.Coord.Equal(node_1.coord))
                                {
                                    SendMoveRequest(node_0, IsRun(node_0.coord, mMoveRequestCoord), EMoveRequestType.NormalSend);
                                }
                                else
                                {
                                    SendMoveRequest(node_1, IsRun(node_1.coord, mMoveRequestCoord), EMoveRequestType.NormalSend);
                                }
                            }
                            else
                            {
                                SendMoveRequest(node_0, IsRun(node_0.coord, mMoveRequestCoord), EMoveRequestType.NormalSend);
                            }
                        }
                    }
                    else
                    {
                        SendMoveRequest(node_0, IsRun(node_0.coord, mMoveRequestCoord), EMoveRequestType.NormalSend);
                    }
                }
            }
        }
        else
        {
            if (mWillWalkPathList.Count > 0)
            {
                Node node_0 = mWillWalkPathList[0];
                CSMisc.Dot2 dir_0 = node_0.coord - OldCell.Coord;
                dir_0 = dir_0.Normal();
                if (!dir_0.Equal(CSMisc.Dot2.Zero))
                {
                    CSMisc.Dot2 dir_m = mMoveRequestCoord - OldCell.Coord;
                    dir_m = dir_m.Normal();
                    if (!dir_m.Equal(CSMisc.Dot2.Zero))
                    {
                        if (!dir_0.Equal(dir_m))
                        {
                            bool isSendTwo = false;
                            if (!isSendTwo)
                            {
                                SendMoveRequest(node_0, IsRun(node_0.coord, mMoveRequestCoord), EMoveRequestType.ChangePos);
                            }
                        }
                    }
                }
            }
        }
    }

    void SendMoveRequest(Node node, bool isRun, int type = EMoveRequestType.NormalSend)
    {
        if (IsBeControl)
        {
            return;
        }
        if (type == EMoveRequestType.NotSend)
        {
            return;
        }
        if (mMoveRequestCoord.Equal(node.coord))
        {
            return;
        }
       
        CSMisc.Dot2 dot = node.coord - mMoveRequestCoord;
        dot = dot.Abs();
        if (dot.x > 2 || dot.y > 2)
        {
            if (FNDebug.developerConsoleVisible) FNDebug.LogError("超过3格");
        }
        long time = CSServerTime.Instance.TotalMillisecond;
        mMoveRequestCoord = node.coord;
        ResetServerCell(node.coord.x, node.coord.y);
        HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_UpdateRoleMove, null);
        if (isRun)
            Net.ReqPlayerMoveMessage(node.coord.x, node.coord.y, time,type == EMoveRequestType.ChangePos ? OldCell.Coord.x : 0, type == EMoveRequestType.ChangePos ? OldCell.Coord.y : 0);
        else
            Net.ReqPlayerWalkMessage(node.coord.x, node.coord.y, time,type == EMoveRequestType.ChangePos ? OldCell.Coord.x : 0, type == EMoveRequestType.ChangePos ? OldCell.Coord.y : 0);
    }

    /// <summary>
    /// 检测如果距离上一个发包是1格的，强制发包，外部调用使用
    /// </summary>
    public override void DetectForceSendMoveRequest()
    {
        if (IsDead || IsBeControl)
        {
            return;

        }
        if (mMoveRequestCoord.x == 0 && mMoveRequestCoord.y == 0)
        {
            MoveRequestCoord = OldCell.Coord;
        }
        if (!mMoveRequestCoord.Equal(OldCell.Coord))
        {
            CSMisc.Dot2 dot = OldCell.Coord - mMoveRequestCoord;
            dot = dot.Abs();
            if (dot.x > 2 || dot.y > 2)
            {
                if (FNDebug.developerConsoleVisible) FNDebug.LogError("超过3格");
            }

            if (!mMoveRequestCoord.Equal(OldCell.Coord))
            {
                if (FNDebug.developerConsoleVisible) FNDebug.LogError("DetectForceSendMoveRequest 没有按照发包来走! MoveRequestCoord = " + mMoveRequestCoord.x 
                    + " " + mMoveRequestCoord.y + " OldCell = " + OldCell.Coord.x + " " + OldCell.Coord.y + " NewCell=" +NewCell.Coord.x + " " + NewCell.Coord.y);
            }
            mMoveRequestCoord = OldCell.Coord;
            int x = OldCell.Coord.x;
            int y = OldCell.Coord.y;
            ResetServerCell(mMoveRequestCoord.x, mMoveRequestCoord.y);
            bool isRun = IsRun(OldCell.Coord, mMoveRequestCoord);
            long time = CSServerTime.Instance.TotalMillisecond;
            if (isRun)
                Net.ReqPlayerMoveMessage(x, y, time,x, y);
            else
                Net.ReqPlayerWalkMessage(x, y, time,x, y);
            CSTouchEvent.Sington.LastStopTime = Time.time;
        }
    }

    int FindMoveRequesetReqIndex(CSBetterList<Node> list)
    {
        int beginCheckIndex = -1;

        if (list != null)
        {
            for (int i = 0; i < 2 && i < list.Count; i++)//最多向前搜索2格
            {
                if (list[i].coord.Equals(mMoveRequestCoord))
                {
                    beginCheckIndex = i;
                    break;
                }
            }
        }

        return beginCheckIndex;
    }

    bool IsRun(CSMisc.Dot2 f, CSMisc.Dot2 s)
    {
        CSMisc.Dot2 d = f - s;
        d = d.Abs();
        int max = d.x > d.y ? d.x : d.y;
        return max >= 2;
    }

    public override void TowardsTargetAttack(CSMisc.Dot2 targetCoord)
    {
        if(IsBeControl)
        {
            return;
        }

        if (TouchEvent.Type == ETouchType.Attack)
        {
            if(NewCell == null || OldCell == null)
            {
                return;
            }
            Node startNode =(IsMoving) ?  CSMesh.Instance.getNode(targetCoord) : CSMesh.Instance.getNode(targetCoord);
            if(startNode == null || startNode.cell == null)
            {
                return;
            }
            Node goalNode = CSMesh.Instance.getNode(targetCoord);
            if(goalNode == null || goalNode.cell == null)
            {
                return;
            }
            bool bObstacle = false;
            if ((goalNode.cell.isAttributes((int)MapEditor.CellType.Separate) && !startNode.cell.isAttributes((int)MapEditor.CellType.Separate)) ||
                (!goalNode.cell.isAttributes((int)MapEditor.CellType.Separate) && startNode.cell.isAttributes((int)MapEditor.CellType.Separate)))
            {
                bObstacle = true;
            }

            if (bObstacle)
            {
                if (CSAutoFightManager.Instance.IsAutoFight)
                {
                    CSAutoFightManager.Instance.IsAutoFight = false;
                }
                return;
            }
        }

        if (TouchMove != EControlState.Idle)
        {
            TABLE.SKILL tblSkill = TouchEvent.GetTableSkill();
            if(tblSkill != null)
            {
                if (!CSSkillTools.GetBestLaunchCoord(tblSkill.effectArea,tblSkill.clientRange,tblSkill.effectRange, OldCell.Coord, targetCoord))
                {
                    TouchEvent.Type = ETouchType.Normal;
                    return;
                }
            }
        }
        if (TouchEvent.Skill != null)
        {
            HotManager.Instance.EventHandler.SendEvent(CEvent.UIJoystick_Reset);
            TouchEvent.Type = ETouchType.Attack;
            TouchEvent.Coord = targetCoord;
            FSM.Switch2(CSMotion.Run, false);
        }
    }

    public override void TowardsTarget(CSMisc.Dot2 targetCoord)
    {
       if(IsBeControl)
        {
            return;
        }
        base.TowardsTarget(targetCoord);
    }

    public void ForceAttack(CSMisc.Dot2 targetCoord)
    {
        if (IsBeControl)
        {
            return;
        }
        if (TouchEvent.Skill != null)
        {
            TouchEvent.Type = ETouchType.Attack;
            TouchEvent.Coord = targetCoord;
            FSM.Switch2(CSMotion.Run, false);
        }
    }

    public override void ReplaceEquip()
    {
        int armor = Utility.GetBodyModel(Info.BodyModel, Info.FashionCloth, Info.Sex, Info.Career);
        int weaponModeID = Utility.GetWeaponModel(Info.Weapon, Info.FashionWeapon, Info.Sex, Info.Career, Info.AvatarType);
        int wingId = Utility.GetWingModelId(Info.WingID);
        ModelLoad.UpdateModel(mModel.Action, armor, weaponModeID, wingId, SetModelAtlas);
    }

    public override CSBetterList<Node> GetPath()
    {
        if (TouchEvent.Type == ETouchType.Touch)
        {
            return CSPaths.getPath(this, TouchEvent.Direction);
        }
        return base.GetPath();
    }

    public override void ClearPath()
    {
        base.ClearPath();
        mNextNode = null;
        IsMoving = false;
        EndMoveAudio();
        SetAction(CSMotion.Stand);
    }

    public void ResetTouchData(CSMisc.Dot2 coord)
    {
        TouchEvent.Type = ETouchType.Normal;
        TouchEvent.Coord = coord;
    }

    void AddWiatWalkPath()
    {
        mWillWalkPathList.Clear();
        for (int i = 0; i < 2 && i < Paths.Count; i++)
            mWillWalkPathList.Add(Paths[i]);
    }

    public override Node NextTarget()
    {
        Node node = null;
        if (mPathFind != null)
        {
            node = mPathFind.GetNextTarget(Paths,TouchEvent.Type, TouchMove);
        }
        if (node != null)
        {
            NewCell = node.cell as CSCell;
        }
        return node;
    }

    public override void Dead()
    {
        EndMoveAudio();
        UnSaveWaitData();
        UnSaveTowardTarget();
        CSSceneEffectMgr.Instance.Destroy(effect6022);
        effect6022?.Destroy();
        if (effect6022!=null)
            effect6022 = null;
        base.Dead();
    }

    public void UpdateIsCanCrossScene(bool val)
    {
        if(mPathFind != null)
        {
            mPathFind.IsCanCrossScene = val;
        }
    }

    void PlayMoveAudio()
    {
        TABLE.AUDIO tblAudio;
        if (AudioTableManager.Instance.TryGetValue(3006, out tblAudio))
        {
            float volume = tblAudio.Volume * CSConfigInfo.Instance.GetFloat(ConfigOption.EffectSound);
            mMoveAudio = CSAudioManager.Instance.PlayAudio(true, 3006,0,0,true);
        }
    }

    public void EndMoveAudio()
    {
        if (mMoveAudio != null)
        {
            if (CSAudioMgr.Instance != null) CSAudioMgr.Instance.RemoveAudio(mMoveAudio);
            mMoveAudio = null;
        }
    }

    public void OnLoadHead(CSResource res)
    {
        if (res == null || res.MirrorObj == null)
        {
            return;
        }
        GameObject go = res.GetObjInst() as GameObject;
        if (go == null)
        {
            return;
        }
        head = go.AddComponent<CSHeadPlayer>();
        head.Init(this);
    }

    private void OnAttachBottom(uint evtId, object obj)
    {
        if(obj == null)
        {
            return;
        }
        CSBottom bottom = (CSBottom)obj;
        if(bottom != null)
        {
            Model.AttachBottom(bottom);
            Model.Bottom.Go.SetActive(false);
        }
        else
        {
            FNDebug.Log("bottom is null");
        }
    }
    
    private void OnAttachBottomNPC(uint evtId, object obj)
    {
        if(obj == null)
        {
            return;
        }
        CSBottomNPC bottomNPC = (CSBottomNPC)obj;
        if(bottomNPC != null)
        {
            Model.AttachBottomNPC(bottomNPC);
            Model.BottomNPC.Go.SetActive(false);
        }
        else
        {
            FNDebug.Log("bottomNPC is null");
        }
    }

    public void OnRelive(uint evtId, object obj)
    {
        SetAction(CSMotion.Stand);
        if (head != null) head.Show();
        CSAutoFightManager.Instance.Stop();
        UIManager.Instance.ClosePanel<UIDeadGrayPanel>();
    }

    public void OnReplaceEquip(uint evtId, object obj)
    {
        IsReplaceEquip = true;
    }

    public void OnCharacterMpChange(uint evtId, object obj)
    {
        if (BaseInfo != null)
        {
            if (BaseInfo.Career != ECareer.Warrior)
            {
                if (CSAutoFightManager.Instance.IsAutoFight)
                {
                    bool isRelease = CSSkillPriorityInfo.Instance.IsCanAutoReleaseByMp(BaseInfo.MP);
                    CSSkillPriorityInfo.Instance.RefreshDefaultReleaseSkill(!isRelease);
                    //if (!CSSkillPriorityInfo.Instance.IsCanAutoReleaseByMp(BaseInfo.MP))
                    //{
                    //    //CSAutoFightManager.Instance.IsAutoFight = false;
                    //}
                }
            }
        }
    }

    public bool IsHasMoveToServerPos()
    {
        if (mMoveRequestCoord.Equal(NewCell.Coord))
        {
            return true;
        }
        if (!IsMoving)
        {
            return true;
        }
        return false;
    }

    public bool IsCanMove()
    {
        if(IsBeControl)
        {
            return false;
        }
        if (IsMoving && ((TouchEvent.Type == ETouchType.Attack) || (TouchEvent.Type == ETouchType.AttackTerrain) ))
        {
            return false;
        }
        if(IsAttackPlaying())
        {
            return false;
        }
        return true;
    }
    
    public void SetMoveStateControlled()
    {
        actState.IsBeControl = true;
    }

    public override void Destroy()
    {
        if(mClient != null)
        {
            mClient.UnRegAll();
        }
        if(mMainEventHandler != null)
        {
            mMainEventHandler.UnRegAll();
        }
        mIsDetectContineMove = false;
        CSConstant.IsLanuchMainPlayer = false;
        mMissionTargetCoord = CSMisc.Dot2.Zero;
        EndMoveAudio();
        UnSaveWaitData();
        CSSceneEffectMgr.Instance.Destroy(effect6022);
        CSSceneEffectMgr.Instance.Destroy(effect6023);
        effect6022?.Destroy();
        effect6022 = null;
        effect6023?.Destroy();
        effect6023 = null;
        base.Destroy();
    }
    
    public override void Release()
    {
        effect6022?.Release();
        effect6022 = null;
        effect6023?.Release();
        effect6023 = null;
        base.Release();
    }

}
