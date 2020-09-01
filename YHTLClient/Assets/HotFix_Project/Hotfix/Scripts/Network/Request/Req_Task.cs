using Google.Protobuf.Collections;
// 包结构集合点
// author: jiabao
// time：  2016.2.17
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using task;

public partial class Net
{
    /// <summary>
    /// 获取任务列表
    /// </summary>
    public static void ReqGetTaskListMessage()
    {
        CSHotNetWork.Instance.SendMsg(ECM.ReqGetTaskListMessage, null);
    }

    /// <summary>
    /// 接任务请求
    /// </summary>
    /// <param name="taskId"></param>
    public static void ReqAcceptTaskMessage(int taskId)
    {
        task.AcceptTaskRequest req = CSProtoManager.Get<AcceptTaskRequest>();
        req.taskId = taskId;
        CSHotNetWork.Instance.SendMsg(ECM.ReqAcceptTaskMessage,req);
    }
    
    /// <summary>
    /// 交任务请求
    /// </summary>
    /// <param name="taskId"></param>
    public static void ReqSubmitTaskMessage(int taskId)
    {
        task.SubmitTaskRequest req = CSProtoManager.Get<SubmitTaskRequest>();
        req.taskId = taskId;
        CSHotNetWork.Instance.SendMsg(ECM.ReqSubmitTaskMessage,req);
    }
    
    /// <summary>
    /// npc对话任务
    /// </summary>
    /// <param name="taskId"></param>
    public static void ReqTalkToNPCMessage(int npcId)
    {
        task.TalkToNPCRequest req = CSProtoManager.Get<TalkToNPCRequest>();
        req.npcId = npcId;
        CSHotNetWork.Instance.SendMsg(ECM.ReqTalkToNPCMessage,req);
    }
    
    /// <summary>
    /// 直接传送请求
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="goalId"></param>
    public static void ReqFlyToGoalMessage(int taskId, int goalId)
    {
        task.FlyToGoalRequest req = CSProtoManager.Get<FlyToGoalRequest>();
        req.goalId = goalId;
        req.taskId = taskId;
        CSHotNetWork.Instance.SendMsg(ECM.ReqFlyToGoalMessage, req);
    }

    /// <summary>
    /// 购买任务
    /// </summary>
    /// <param name="taskId"></param>
    public static void CSBuyTaskMessage(int taskId)
    {
        task.BuyTaskRequest req = CSProtoManager.Get<task.BuyTaskRequest>();
        req.taskId = taskId;
        CSHotNetWork.Instance.SendMsg(ECM.CSBuyTaskMessage, req);
    }
}
