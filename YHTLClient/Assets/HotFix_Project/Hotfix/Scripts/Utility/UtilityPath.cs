using System;

public class UtilityPath
{
    /// <summary>
    /// 寻路到当前地图某一点
    /// </summary>
    public static void FindPath(CSMisc.Dot2 dot)
    {
        CSPathFinderManager.Instance.FindPath(dot);
    }

    /// <summary>
    /// 寻路到目标地图点
    /// </summary>
    public static void FindPath(int mapId, int x, int y)
    {
        CSMisc.Dot2 dot;
        dot.x = x;
        dot.y = y;
        FindPath(dot, mapId);
    }
    
    /// <summary>
    /// 寻路到目标地图点
    /// </summary>
    public static void FindPath(CSMisc.Dot2 dot, int mapId)
    {
        CSPathFinderManager.Instance.FindPath(dot, mapId);
    }

    /// <summary>
    /// 寻路到目标地图Npc位置
    /// </summary>
    public static void FindPath(CSMisc.Dot2 dot, int _targetMapId, int _npcID)
    {
        CSPathFinderManager.Instance.FindPath(dot, _targetMapId, _npcID);
    }
    
    /// <summary>
    /// 寻路到目标地图Npc位置， 添加进入当前地图回调，可以自行处理特殊情况
    /// </summary>
    public static void FindPath(CSMisc.Dot2 dot, int _targetMapId, int _npcID, System.Action MoveInSameMapCallback)
    {
        CSPathFinderManager.Instance.FindPath(dot, _targetMapId, _npcID, MoveInSameMapCallback);
    }
    
    /// <summary>
    /// 寻路到目标地图Npc位置， 添加进入当前地图回调，可以自行处理特殊情况， 小飞鞋是否显示正在寻路字样
    /// </summary>
    public static void FindPath(CSMisc.Dot2 dot, int _targetMapId, int _npcID, System.Action MoveInSameMapCallback,
        bool IsShowWord)
    {
        CSPathFinderManager.Instance.FindPath(dot, _targetMapId, _npcID, MoveInSameMapCallback, IsShowWord);
    }
    
    /// <summary>
    /// 寻路到目标地图， 添加进入当前地图回调，可以自行处理特殊情况
    /// </summary>
    public static void FindPath(CSMisc.Dot2 dot, int _targetMapId, System.Action MoveInSameMapCallback)
    {
        CSPathFinderManager.Instance.FindPath(dot, _targetMapId, 0, MoveInSameMapCallback);
    }
    
    /// <summary>
    /// 寻路到目标地图， 添加进入当前地图回调，可以自行处理特殊情况， 小飞鞋是否显示正在寻路字样
    /// </summary>
    public static void FindPath(CSMisc.Dot2 dot, int _targetMapId, System.Action MoveInSameMapCallback,
        bool IsShowWord)
    {
        CSPathFinderManager.Instance.FindPath(dot, _targetMapId, 0, MoveInSameMapCallback, IsShowWord);
    }
    
    
    /// <summary>
    /// 寻找NPC
    /// </summary>
    public static void FindNpc(int npcId)
    {
        CSPathFinderManager.Instance.FindOpenNpc(npcId);
    }
    
    /// <summary>
    /// 寻找NPC ， 小飞鞋是否显示正在寻路字样
    /// </summary>
    public static void FindNpc(int npcId, bool isShowWord)
    {
        CSPathFinderManager.Instance.FindOpenNpc(npcId, isShowWord);
    }
    
    /// <summary>
    /// 通过 deliver Id 寻路
    /// 传送到位置后，自动战斗
    /// </summary>
    /// <param name="deliverId">npc对应 deliver的id</param>
    public static void FindWithDeliverIdFight(int deliverId)
    {
        CSPathFinderManager.Instance.FlyWithDeliverFight(deliverId);
    }
    
    /// <summary>
    /// 通过 deliver Id 寻路
    /// </summary>
    /// <param name="deliverId">npc对应 deliver的id</param>
    public static void FindWithDeliverId(int deliverId)
    {
        CSPathFinderManager.Instance.FindWithDeliverId(deliverId);
    }
    
    /// <summary>
    /// 取消监听寻路npc
    /// </summary>
    public static void UnRegReachNpc()
    {
        CSPathFinderManager.Instance.UnRegReachNpc();
    }
    
    /// <summary>
    /// 通过 deliver Id 寻路
    /// </summary>
    /// <param name="npcId">npcid</param>
    public static void FlyToNpc(int npcId)
    {
        int deliverId = DeliverTableManager.Instance.GetDeliverIdByNpcId(npcId);
        if (deliverId > 0)
            CSPathFinderManager.Instance.FindWithDeliverId(deliverId);
        else 
            CSPathFinderManager.Instance.FindOpenNpc(npcId);
    }
    
    /// <summary>
    /// 重置数据
    /// </summary>
    /// <param name="isStopJoystick">角色是否停止移动</param>
    /// <param name="isStopAutoFight">是否停止自动战斗</param>
    /// <param name="isResetMission">是否充值任务</param>
    public static void ReSetPath(bool isStopJoystick = true, bool isStopAutoFight = true, bool isResetMission = true)
    {
        CSPathFinderManager.Instance.ReSetPath(isStopJoystick, isStopAutoFight, isResetMission);
    }

    public static bool IsAutoFind()
    {
        return CSPathFinderManager.IsAutoFinPath;
    }

}