using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 组队系统网络响应
/// </summary>
public partial class CSNetTeam : CSNetBase
{
    /// <summary>
    /// 队伍信息
    /// </summary>
    /// <param name="info"></param>
    void ECM_ResTeamInfoMessage(NetInfo info)
    {
        team.TeamInfo msg = Network.Deserialize<team.TeamInfo>(info);
        CSTeamInfo.Instance.GetMyTeamData(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.AddTeamForMe, msg);
    }

    /// <summary>
    /// 队长收到的申请消息
    /// </summary>
    /// <param name="info"></param>
    void ECM_ResApplyTeamMessage(NetInfo info)
    {
        team.TeamMember msg = Network.Deserialize<team.TeamMember>(info);
        CSTeamInfo.Instance.LeaderApplyMessage(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.EnrollmentApplication, msg);
    }

    /// <summary>
    /// 被邀请者收到的消息
    /// </summary>
    /// <param name="info"></param>
    void ECM_ResInviteTeamMessage(NetInfo info)
    {
        team.InviteTeamMsg msg = Network.Deserialize<team.InviteTeamMsg>(info);
        CSTeamInfo.Instance.GetInviteTeamMessage(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.ResInviteTeam, msg);
    }

    /// <summary>
    /// 加入队伍响应
    /// </summary>
    /// <param name="info"></param>
    void ECM_ResJoinTeamMessage(NetInfo info)
    {
        team.JoinTeamResponse msg = Network.Deserialize<team.JoinTeamResponse>(info);
        CSTeamInfo.Instance.JoinTeamMessage(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.AddTeamOther, msg);
    }

    /// <summary>
    /// 离开队伍响应
    /// </summary>
    /// <param name="info"></param>
    void ECM_ResLeaveTeamMessage(NetInfo info)
    {
        team.LeaveTeamResponse msg = Network.Deserialize<team.LeaveTeamResponse>(info);
        CSTeamInfo.Instance.LeaveTeam(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.QuitTeam, msg);
    }

    /// <summary>
    /// 队长变更响应(此条已弃用)
    /// </summary>
    /// <param name="info"></param>
    void ECM_ResTeamLeaderChangedMessage(NetInfo info)
    {
        team.TeamLeaderChanged msg = Network.Deserialize<team.TeamLeaderChanged>(info);
        CSTeamInfo.Instance.LeaderChangedMessage(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.ChangeLeader, msg);
    }

    /// <summary>
    /// 组队面板信息响应
    /// </summary>
    /// <param name="info"></param>
    void ECM_ResGetTeamInfoMessage(NetInfo info)
    {
        team.GetTeamInfoResponse msg = Network.Deserialize<team.GetTeamInfoResponse>(info);
        CSTeamInfo.Instance.UpdateMyTeamData(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.GetMyTeamInfo, msg);
    }

    /// <summary>
    /// 组队面板tab响应
    /// </summary>
    /// <param name="info"></param>
    void ECM_ResGetTeamTabMessage(NetInfo info)
    {
        team.TeamTabInfo msg = Network.Deserialize<team.TeamTabInfo>(info);
        CSTeamInfo.Instance.GetTeamTabMessage(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.TeamTabMessage, msg);
    }

    ///// <summary>
    ///// 组队面板附近的队伍tab响应
    ///// </summary>
    ///// <param name="info"></param>
    //void ResRoundTeamsMessage(NetInfo info)
    //{
    //    team.TeamList msg = Network.Deserialize<team.TeamList>(info);
    //}

    /// <summary>
    /// 帮会召唤令响应
    /// </summary>
    /// <param name="info"></param>
    void ECM_ResTeamCallBackMessage(NetInfo info)
    {
        team.CallBack msg = Network.Deserialize<team.CallBack>(info);
    }

    /// <summary>
    /// 队伍目标响应
    /// </summary>
    /// <param name="info"></param>
    void ECM_TeamTargetAckMessage(NetInfo info)
    {
        team.TeamTargetAck msg = Network.Deserialize<team.TeamTargetAck>(info);
    }

    /// <summary>
    /// 队伍令使用响应
    /// </summary>
    /// <param name="info"></param>
    void ECM_TeamCallBackAckMessage(NetInfo info)
    {
        team.TeamCallBackAck msg = Network.Deserialize<team.TeamCallBackAck>(info);
    }

    /// <summary>
    /// 设置组队模式回应
    /// </summary>
    /// <param name="info"></param>
    void ECM_SCSetTeamModeMessage(NetInfo info)
    {
        team.SetTeamModeRequest msg = Network.Deserialize<team.SetTeamModeRequest>(info);
        //Debug.Log("----------------------更改组队模式回包" +msg.teamMode);
        CSTeamInfo.Instance.ChangeTeamMode(msg);
    }


    /// <summary>
    /// 更新玩家（最大）血量、（最大）蓝量
    /// </summary>
    /// <param name="info"></param>
    void ECM_SCPlayerHpMpInfoMessage(NetInfo info)
    {
        team.PlayerHpMpInfo msg = Network.Deserialize<team.PlayerHpMpInfo>(info);
        CSTeamInfo.Instance.UpdatePlayerHpMpInfoMessage(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.UpdatePlayerHpMpInfoMessage, msg);
    }

    /// <summary>
    /// 更新玩家等级
    /// </summary>
    /// <param name="info"></param>
    void ECM_SCPlayerLevelInfoMessage(NetInfo info)
    {
        team.PlayerLevelInfo msg = Network.Deserialize<team.PlayerLevelInfo>(info);
        CSTeamInfo.Instance.UpdatePlayerLevelInfoMessage(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.UpdatePlayerLevelInfoMessage, msg);
    }
}
