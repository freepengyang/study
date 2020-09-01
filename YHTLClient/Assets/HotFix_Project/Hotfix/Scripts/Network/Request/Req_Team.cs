using System;
using Google.Protobuf.Collections;
public partial class Net
{
    /// <summary>
    /// 创建队伍请求
    /// </summary>
    public static void ReqCreateTeamMessage()
    {
        CSNetwork.Instance.SendMsg((int)ECM.ReqCreateTeamMessage, null);
    }

    /// <summary>
    /// 加入队伍请求
    /// </summary>
    public static void ReqApplyTeamMessage(long teamId)
    {
        team.ApplyTeamRequest data = CSProtoManager.Get<team.ApplyTeamRequest>();
        data.teamId = teamId;
        CSNetwork.Instance.SendMsg((int)ECM.ReqApplyTeamMessage, data);
    }

    /// <summary>
    /// 审核队伍请求
    /// </summary>
    public static void ReqConfirmTeamApplyMessage(long roleId, ConfirmTeamApplyType type)
    {
        team.ConfirmTeamApplyRequest data = CSProtoManager.Get<team.ConfirmTeamApplyRequest>();
        data.roleId = roleId;
        data.confirm = (int)type;
        CSNetwork.Instance.SendMsg((int)ECM.ReqConfirmTeamApplyMessage, data);
    }

    /// <summary>
    /// 邀请组队请求
    /// </summary>
    public static void ReqInviteTeamMessage(long roleId, TeamTab teamTab)
    {
        team.InviteTeamRequest data = CSProtoManager.Get<team.InviteTeamRequest>();
        data.roleId = roleId;
        data.tab = (int)teamTab;
        CSNetwork.Instance.SendMsg((int)ECM.ReqInviteTeamMessage, data);
    }

    /// <summary>
    /// 审核组队邀请
    /// </summary>
    public static void ReqConfirmTeamInviteMessage(long teamId, ConfirmTeamApplyType type,long inviter)
    {
        team.ConfirmTeamInviteRequest data = CSProtoManager.Get<team.ConfirmTeamInviteRequest>();
        data.teamId = teamId;
        data.confirm = (int)type;
        data.inviter = inviter;
        CSNetwork.Instance.SendMsg((int)ECM.ReqConfirmTeamInviteMessage, data);
        HotManager.Instance.EventHandler.SendEvent(CEvent.HandledInviteTeam);
        CSTeamInfo.Instance.ClearListPlayersInaitationForMe();
    }

    /// <summary>
    /// 转让队长请求
    /// </summary>
    public static void ReqChangeTeamLeaderMessage(long roleId)
    {
        team.ChangeTeamLeaderRequest data = CSProtoManager.Get<team.ChangeTeamLeaderRequest>();
        data.roleId = roleId;
        CSNetwork.Instance.SendMsg((int)ECM.ReqChangeTeamLeaderMessage, data);
    }

    /// <summary>
    /// 退队或踢人请求(退队或踢人,自己退填自己的id)
    /// </summary>
    public static void ReqLeaveTeamMessage(long roleId)
    {
        team.LeaveTeamRequest data = CSProtoManager.Get<team.LeaveTeamRequest>();
        data.roleId = roleId;
        CSNetwork.Instance.SendMsg((int)ECM.ReqLeaveTeamMessage, data);
    }
    
    /// <summary>
    /// 请求组队面板信息
    /// </summary>
    public static void ReqGetTeamInfoMessage()
    {
        CSNetwork.Instance.SendMsg((int)ECM.ReqGetTeamInfoMessage, null);
    }

    /// <summary>
    /// 设置组队模式请求(teamMode  0自动 1手动 2拒绝)
    /// </summary>
    public static void ReqSetTeamModeMessage(int teamMode)
    {
        team.SetTeamModeRequest data = CSProtoManager.Get<team.SetTeamModeRequest>();
        data.teamMode = teamMode;
        CSNetwork.Instance.SendMsg((int)ECM.ReqSetTeamModeMessage, data);
    }

    /// <summary>
    /// 获取组队面板tab请求（taskId暂时不用，0是默认值，传不过去，暂时传0值就行）
    /// </summary>
    public static void ReqGetTeamTabMessage(TeamTab tab, int taskId)
    {
        team.GetTeamTabRequest data = CSProtoManager.Get<team.GetTeamTabRequest>();
        data.tab = (int)tab;
        data.taskId = taskId;
        CSNetwork.Instance.SendMsg((int)ECM.ReqGetTeamTabMessage, data);
    }

    /// <summary>
    /// 解散队伍请求
    /// </summary>
    public static void ReqDisposeTeamsMessage()
    {
        CSNetwork.Instance.SendMsg((int)ECM.ReqDisposeTeamsMessage, null);
    }

    /// <summary>
    /// 帮会召唤令请求
    /// </summary>
    public static void ReqTeamCallBackInfoMessage(long callBackId, bool agree)
    {
        team.CallBackInfo data = CSProtoManager.Get<team.CallBackInfo>();
        data.callBackId = callBackId;
        data.agree = agree;
        CSNetwork.Instance.SendMsg((int)ECM.ReqTeamCallBackInfoMessage, data);

    }

    /// <summary>
    /// 队伍目标请求
    /// </summary>
    public static void TeamTargetReqMessage(int taskId)
    {
        team.TeamTargetReq data = CSProtoManager.Get<team.TeamTargetReq>();
        data.taskId = taskId;
        CSNetwork.Instance.SendMsg((int)ECM.TeamTargetReqMessage, data);
    }

    /// <summary>
    /// 家族拉取玩家加入队伍（type 0家族 1国家）
    /// </summary>
    public static void UnionJoinTeamReqMessage(int type)
    {
        team.UnionJoinTeamReq data = CSProtoManager.Get<team.UnionJoinTeamReq>();
        data.type = type;
        CSNetwork.Instance.SendMsg((int)ECM.UnionJoinTeamReqMessage, data);
    }
}
