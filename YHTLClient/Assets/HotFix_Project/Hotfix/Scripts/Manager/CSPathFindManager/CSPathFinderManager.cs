using System;
using System.Collections.Generic;
using map;
using TABLE;
public enum DeliverType
{
    DT_NONE = 0,
    DT_POSITION = 1,
    DT_NPC = 2,
}
public class CSPathFinderManager : CSInfo<CSPathFinderManager>
{
    private PathGuideState _mPathGuideState = PathGuideState.None;

    /// <summary>
    /// 获取当前寻路状态
    /// </summary>
    public PathGuideState PathGuideState
    {
        set
        {
            if (_mPathGuideState == value) return;

            _mPathGuideState = value;
            mClientEvent.SendEvent(CEvent.UpdateFindState, value);
        }
        get { return _mPathGuideState; }
    }

    //private System.Action onReachNpc = null;
    private CSMisc.Dot2 _mTargetDot = new CSMisc.Dot2(0, 0);
    private bool _IsShowTextWord = true;
    private int _ReachNpcId;
    private int _TargetMapId;
    private System.Action _MoveInSameMapCallback;
    private DELIVER _DeliverTable = null;
    private NPC _npcTab = null;
    private CSBetterList<CSAStarScene.Node_Scene> _PointList = new CSBetterList<CSAStarScene.Node_Scene>();

    private static bool mIsAutoFinPath;

    //是否是自动寻路
    public static bool IsAutoFinPath
    {
        get { return mIsAutoFinPath; }
        set
        {
            if (mIsAutoFinPath == value) return;
            mIsAutoFinPath = value;
            if (!value) CSPathFinderManager.Instance.PathGuideState = PathGuideState.None;
            CSPlayerAutoActionInfo.Instance.SetAutoAction();
            if(mIsAutoFinPath)
            {
                CSAutoFightManager.Instance.IsAutoFight = false;
            }
        }
    }

    #region Handle

    private void OnReach(uint id, object data)
    {
        mClientEvent.UnReg(CEvent.Reach, OnReach);
        OnReachFindPathState sate;
        if (data == null) sate = OnReachFindPathState.ReachNpc;
        else sate = (OnReachFindPathState) data;
        switch (sate)
        {
            case OnReachFindPathState.CommonFight:
                CSAutoFightManager.Instance.BeginFight(0);
                break;
            case OnReachFindPathState.ReachNpc:
                if (!NpcTableManager.Instance.TryGetValue(_ReachNpcId, out NPC npcInfo)) return;
                if ((npcInfo.sceneId != 0 && npcInfo.sceneId != CSScene.GetMapID())) return;
                if(npcInfo.bornX == 0 && npcInfo.bornY == 0)
                {
                    if(!Utility.IsNearPlayerInMap(_mTargetDot.x, _mTargetDot.y)) return;
                }else
                {                    
                    if(!Utility.IsNearPlayerInMap(npcInfo.bornX, npcInfo.bornY)) return;
                }
                CSNpcGo.OpenNpcFun(npcInfo);
                break;
        }
    }

    private void OnSwitchScene(uint id, object data)
    {
        mClientEvent.UnReg(CEvent.Scene_EnterSceneAfter, OnSwitchScene);
        FindPath(_mTargetDot, _TargetMapId, _ReachNpcId, _MoveInSameMapCallback, _IsShowTextWord);
    }

    private void OnCancelPathFind(uint id, object data)
    {
        mClientEvent.UnReg((uint) CEvent.Scene_EnterSceneAfter, OnSwitchScene);
        mClientEvent.UnReg((uint) CEvent.Reach, OnReach);
    }

    private void OnMainSceneStateInit(uint id, object data)
    {
        mClientEvent.UnReg((uint) CEvent.Scene_EnterSceneAfter, OnMainSceneStateInit);
        MoveToTargetNpc();
    }

  #endregion

    #region Interface

    /// <summary>
    /// 重置数据
    /// </summary>
    /// <param name="isStopJoystick">角色是否停止移动</param>
    /// <param name="isStopAutoFight">是否停止自动战斗</param>
    /// <param name="isResetMission">是否充值任务</param>
    public void ReSetPath(bool isStopJoystick = true, bool isStopAutoFight = true, bool isResetMission = true)
    {
        mClientEvent.UnRegAll();
        if (isStopJoystick)
        {
            if (CSScene.IsLanuchMainPlayer &&CSAvatarManager.MainPlayer.TouchMove == EControlState.Idle)
               CSAvatarManager.MainPlayer.Stop();
        }

        PathGuideState = PathGuideState.None;
        if (isStopAutoFight)
            CSAutoFightManager.Instance.Stop();

        if (isResetMission)
        {
            CSMissionManager.Instance.CurSelectMission = null;
        }

        ClearFindPath();
        IsAutoFinPath = false;

        //取消寻路时，删除所有注册到达NPC的逻辑，，暂时不知道会不会有问题
        mClientEvent.UnReg((int) CEvent.Reach);
    }


    /// <summary>
    /// 基础寻路接口
    /// </summary>
    /// <param name="dot">目标点坐标</param>
    /// <param name="_targetMapId">目标点地图ID</param>
    /// <param name="_npcID">目标NPC ID，当此值不为0时，寻路到npc，无视dot和_targetMapID的值</param>
    /// <param name="MoveInSameMapCallback">当寻路进目标国家的目标地图时的回调函数，若此函数不为空则中断寻路系统，执行次函数的逻辑</param>
    /// <param name="IsShowWord">是否显示“正在寻路”字样</param>
    /// <param name="forceDot">传入npcid不为空时，是否强制使用传入的dot作为最终寻路点</param>
    public void FindPath(CSMisc.Dot2 dot, int _targetMapId, int _npcID = 0, System.Action MoveInSameMapCallback = null,
        bool IsShowWord = true, bool forceDot = false)
    {
        ClearFindPath();

        _IsShowTextWord = IsShowWord;
        _mTargetDot = dot;
        _TargetMapId = _targetMapId;
        _MoveInSameMapCallback = MoveInSameMapCallback;

        if (_npcID != 0)
        {
            if (!NpcTableManager.Instance.TryGetValue(_npcID, out NPC npcInfo)) return;
            if (!forceDot)
            {
                dot.x = npcInfo.bornX;
                dot.y = npcInfo.bornY;
            }

            _ReachNpcId = _npcID;
            if ((npcInfo.sceneId == 0 || npcInfo.sceneId == CSScene.GetMapID()) &&
                Utility.IsNearPlayerInMap(dot.x, dot.y))
            {
                _mTargetDot = dot;
                OnReach(0, OnReachFindPathState.ReachNpc);
                return;
            }

            mClientEvent.AddEvent(CEvent.Reach, OnReach);
        }

        TABLE.EVENT tbl_event = null;
        if (_targetMapId == 0 || _targetMapId == CSScene.GetMapID())
        {
            if (MoveInSameMapCallback != null)
            {
                mClientEvent.SendEvent(CEvent.CancelPathFind);
                IsAutoFinPath = false;
                MoveInSameMapCallback();
                return;
            }

            dot = CSMisc.GetNearPoint(dot.x, dot.y,CSAvatarManager.MainPlayer.NewCell.mCell_x,
               CSAvatarManager.MainPlayer.NewCell.mCell_y, 1);
            _mTargetDot = dot;
        }
        else
        {
            _PointList?.Clear();

            _PointList = CSAStarScene.FindPath(CSScene.GetMapID(), _targetMapId);
            if (_PointList == null) return;
            if (_PointList.Count > 0)
            {
                tbl_event = _PointList[0].EventTblToNext;
                _PointList.RemoveAt(0);
            }

            if (tbl_event == null) return;
            mClientEvent.AddEvent(CEvent.Scene_EnterSceneAfter, OnSwitchScene);
            dot.x = tbl_event.x;
            dot.y = tbl_event.y;
        }

        IsAutoFinPath = true;
        mClientEvent.AddEvent(CEvent.CancelPathFind, OnCancelPathFind);
       CSAvatarManager.MainPlayer.TowardsMissionTarget(dot);
    }

    /// <summary>
    /// 重载 寻路
    /// </summary>
    /// <param name="dot"></param>
    /// <param name="mapId">填0 读玩家当前地图id</param>
    public void FindPath(CSMisc.Dot2 dot, int mapId = 0)
    {
        FindPath(dot, mapId == 0 ? CSScene.GetMapID() : mapId, 0);
    }

    /// <summary>
    /// 寻路到npc上
    /// </summary>
    /// <param name="npcId"></param>
    public void FindPath(int npcId)
    {
        ClearFindPath();
        if (!NpcTableManager.Instance.TryGetValue(npcId, out _npcTab)) return;
        _ReachNpcId = npcId;
        MoveToTargetNpc();
    }

    /// <summary>
    /// 自动寻路到NPC
    /// npc坐标为0时，默认视野寻路，，此条件只能用于，npc只出现在视野内的寻路
    /// </summary>
    /// <param name="npcId">npc id</param>
    /// <param name="isShowWord">是否显示“正在寻路”字样</param>
    public void FindOpenNpc(int npcId, bool isShowWord = true)
    {
        TABLE.NPC npcInfo;
        if (!NpcTableManager.Instance.TryGetValue(npcId, out npcInfo)) return;
        if (npcInfo.bornX == 0 && npcInfo.bornY == 0)
        {
            var avatarList = CSAvatarManager.Instance.GetAvatarList(EAvatarType.NPC);
            for (var i = 0; i < avatarList.Count; i++)
            {
                if (avatarList[i] != null && avatarList[i].BaseInfo.ConfigId == npcId)
                {
                    _mTargetDot = avatarList[i].NewCell.Coord;
                    FindPath(_mTargetDot, npcInfo.sceneId, npcId, null, isShowWord, true);
                    return;
                }
            }
        }

        _mTargetDot.x = npcInfo.bornX;
        _mTargetDot.y = npcInfo.bornY;
        FindPath(_mTargetDot, npcInfo.sceneId, npcId, null, isShowWord);
    }

    /// <summary>
    /// 通过 deliver Id 寻路
    /// 传地图点时 自动战斗
    /// </summary>
    public void FlyWithDeliverFight(int deliverId)
    {
        FindWithDeliverId(deliverId, true);
    }

    /// <summary>
    /// 通过 deliver Id 寻路
    /// </summary>
    public void FindWithDeliverId(int deliverId)
    {
        FindWithDeliverId(deliverId, false);
    }

    /// <summary>
    /// 通过 deliver Id 寻路
    /// </summary>
    /// <param name="deliverId">npc对应 deliver的id</param>
    /// <param name="action">传地图点时 是否 自动战斗</param>
    public void FindWithDeliverId(int deliverId, bool action)
    {
        if (!Utility.IsCanTransfer()) return;
        ClearFindPath();
        mClientEvent.SendEvent(CEvent.CancelPathFind);
        if (!DeliverTableManager.Instance.TryGetValue(deliverId, out _DeliverTable)) return;


        if (_DeliverTable.item.Count < 2 ||
            _DeliverTable.item[0].GetItemCount() >= _DeliverTable.item[1])
        {
            if ((DeliverType)_DeliverTable.DeliverType == DeliverType.DT_NPC)
            {
                if (NpcTableManager.Instance.TryGetValue(_DeliverTable.deliverParameter, out NPC npcInfo))
                {
                    _ReachNpcId = _DeliverTable.deliverParameter;
                    if (CSScene.GetMapID() == npcInfo.sceneId)
                    {
                        //在最小传送距离内，需要寻路过去
                        if (Utility.IsNearPlayerInMap(npcInfo.bornX, npcInfo.bornY, 16))
                        {
                            DeliverFindPath();
                            return;
                        }

                        mClientEvent.AddEvent(CEvent.Reach_Npc_Position, OnDeliverOpenNpc);
                    }
                    else
                    {
                        mClientEvent.AddEvent(CEvent.Scene_EnterSceneAfter, OnDeliverOpenNpc);
                    }
                }
            }
            else
            {
                if (CSScene.GetMapID() == _DeliverTable.deliverParameter)
                {
                    //在最小传送距离内，需要寻路过去
                    if (Utility.IsNearPlayerInMap(_DeliverTable.x, _DeliverTable.y, 16))
                    {
                        if (action)
                            mClientEvent.Reg((uint)CEvent.Reach, OnReachPosition);
                        DeliverFindPath();
                        return;
                    }

                    if (action)
                        mClientEvent.AddEvent(CEvent.Reach_Npc_Position, OnDeliverPoint);
                }
                else
                {
                    if (action)
                        mClientEvent.AddEvent(CEvent.Scene_EnterSceneAfter, OnDeliverPoint);
                }
            }

            Net.ReqTransferByDeliverConfigMessage(deliverId, false, (int) PositionChangeReason.Transfer, false, 0);
        }
        else
        {
            DeliverFindPath();
        }
    }


    /// <summary>
    /// 取消监听寻路npc
    /// </summary>
    public void UnRegReachNpc()
    {
        mClientEvent.UnReg(CEvent.Reach, OnReach);
    }

  #endregion

    private void OnDeliverOpenNpc(uint id, object data)
    {
        mClientEvent.UnReg(CEvent.Scene_PlayerAdjustPosition, OnDeliverOpenNpc);
        mClientEvent.UnReg(CEvent.Scene_EnterSceneAfter, OnDeliverOpenNpc);
        OnReach(0, OnReachFindPathState.ReachNpc);
    }

    private void OnReachPosition(uint id, object data)
    {
        mClientEvent.UnReg(CEvent.Reach, OnReachPosition);
        OnReach(0, OnReachFindPathState.CommonFight);
    }

    private void OnDeliverPoint(uint id, object data)
    {
        mClientEvent.UnReg(CEvent.Scene_PlayerAdjustPosition, OnDeliverOpenNpc);
        mClientEvent.UnReg(CEvent.Scene_EnterSceneAfter, OnDeliverOpenNpc);
        OnReach(0, OnReachFindPathState.CommonFight);
    }

    private void DeliverFindPath()
    {
        if (_DeliverTable == null) return;

        if ((DeliverType)_DeliverTable.DeliverType == DeliverType.DT_POSITION)
        {
            _mTargetDot.x = _DeliverTable.x;
            _mTargetDot.y = _DeliverTable.y;
            FindPath(_mTargetDot, _DeliverTable.deliverParameter);
        }
        else if ((DeliverType)_DeliverTable.DeliverType == DeliverType.DT_NPC)
        {
            FindOpenNpc(_DeliverTable.deliverParameter);
        }
    }

    private void MoveToTargetNpc()
    {
        if (_npcTab == null) return;
        if (CSScene.GetMapID() == _npcTab.sceneId)
        {
            if (Utility.IsNearPlayerInMap(_npcTab.bornX, _npcTab.bornY))
            {
                CSNpcGo.OpenNpcFun(_npcTab);
                return;
            }

            _mTargetDot.x = _npcTab.bornX;
            _mTargetDot.y = _npcTab.bornY;
            IsAutoFinPath = true;
            CSAvatarManager.MainPlayer.TowardsMissionTarget(_mTargetDot);
            mClientEvent.AddEvent(CEvent.Reach, OnReach);
        }
        else
        {
            if (_PointList == null || _PointList.Count <= 0)
                _PointList = CSAStarScene.FindPath(CSScene.GetMapID(), _npcTab.sceneId);
            if (_PointList.Count > 0)
            {
                EVENT tbl_event = _PointList[0].EventTblToNext;
                _PointList.RemoveAt(0);
                if (tbl_event != null)
                {
                    mClientEvent.AddEvent(CEvent.Scene_EnterSceneAfter, OnMainSceneStateInit);
                    _mTargetDot.x = tbl_event.x;
                    _mTargetDot.y = tbl_event.y;
                    IsAutoFinPath = true;
                   CSAvatarManager.MainPlayer.TowardsMissionTarget(_mTargetDot);
                }
            }
        }
    }

    private void ClearFindPath()
    {
        mClientEvent.UnReg(CEvent.Reach, OnReach);
        //onReachNpc = null;
        _mTargetDot.Clear();
        _ReachNpcId = 0;
        _TargetMapId = 0;
        _MoveInSameMapCallback = null;
        _DeliverTable = null;
        _npcTab = null;
        _PointList?.Clear();
        mClientEvent.SendEvent(CEvent.CancelPathFind);
    }

    public override void Dispose()
    {

    }
}