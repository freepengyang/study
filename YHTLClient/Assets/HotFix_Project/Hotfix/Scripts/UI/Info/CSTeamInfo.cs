using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CSTeamInfo : CSInfo<CSTeamInfo>
{
    public CSTeamInfo()
    {
    }

    public override void Dispose()
    {

    }

    /// <summary>
    /// 队伍Id(数值为0时 没有队伍)
    /// </summary>
    long teamId = 0;
    public long TeamId { get { return teamId; } }

    /// <summary>
    /// 队伍满员人数
    /// </summary>
    public const int NumFullStarffed = 11;


    /// <summary>
    /// 队伍信息
    /// </summary>
    TeamData myTeamData;
    public TeamData MyTeamData { get{ return myTeamData; } }


    /// <summary>
    /// 附近队伍
    /// </summary>
    public ILBetterList<team.TeamBrief> listTeamNearby = new ILBetterList<team.TeamBrief>();
    /// <summary>
    /// 附近可组玩家
    /// </summary>
    public ILBetterList<team.TeamMember> listPlayersNearby = new ILBetterList<team.TeamMember>();
    /// <summary>
    /// 好友中可组玩家
    /// </summary>
    public ILBetterList<team.TeamMember> listPlayersFriend = new ILBetterList<team.TeamMember>();
    /// <summary>
    /// 行会中可组玩家
    /// </summary>
    public ILBetterList<team.TeamMember> listPlayersGuild = new ILBetterList<team.TeamMember>();
    /// <summary>
    /// 邀请我组队的玩家列表
    /// </summary>
    public ILBetterList<team.InviteTeamMsg> listPlayersInaitationForMe = new ILBetterList<team.InviteTeamMsg>();


    /// <summary>
    /// 我是不是队伍队长
    /// </summary>
    /// <returns></returns>
    public bool IsTeamLeader()
    {
        if (myTeamData != null)
        {
            if (myTeamData.teamInfo.leaderId == CSMainPlayerInfo.Instance.ID)
                return true;
        }
        return false;
    }


    /// <summary>
    /// 是否是我的队友
    /// </summary>
    /// <param name="roleId">该角色唯一Id</param>
    /// <returns></returns>
    public bool IsMyTeamPlayer(long roleId)
    {
        if (roleId <= 0) return false;
        for (int i = 0; i < myTeamData.teamInfo.members.Count; i++)
        {
            team.TeamMember teamMember = myTeamData.teamInfo.members[i];
            if (teamMember.roleId == roleId)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 获取我的队伍中某个玩家的信息
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public team.TeamMember GetMyTeamPlayerMember(long roleId)
    {
        if (myTeamData == null) return null;
        for (int i = 0; i < myTeamData.teamInfo.members.Count; i++)
        {
            if (myTeamData.teamInfo.members[i].roleId == roleId)
            {
                return myTeamData.teamInfo.members[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 获取附近某个队伍的信息
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    public team.TeamBrief GetNearbyTeamBrief(long teamId)
    {
        if (listTeamNearby.Count == 0) return null;
        for (int i = 0; i < listTeamNearby.Count; i++)
        {
            if (listTeamNearby[i].id == teamId)
            {
                return listTeamNearby[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 获取我附近某个可组玩家的信息
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public team.TeamMember GetMyNearbyPlayerMember(long roleId)
    {
        if (listPlayersNearby.Count == 0) return null;
        for (int i = 0; i < listPlayersNearby.Count; i++)
        {
            if (listPlayersNearby[i].roleId == roleId)
            {
                return listPlayersNearby[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 获取我好友中某个可组好友的信息
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public team.TeamMember GetMyFriendPlayerMember(long roleId)
    {
        if (listPlayersFriend.Count == 0) return null;
        for (int i = 0; i < listPlayersFriend.Count; i++)
        {
            if (listPlayersFriend[i].roleId == roleId)
            {
                return listPlayersFriend[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 获取我的行会中某个可组玩家的信息
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public team.TeamMember GetMyGuildPlayerMember(long roleId)
    {
        if (listPlayersGuild.Count == 0) return null;
        for (int i = 0; i < listPlayersGuild.Count; i++)
        {
            if (listPlayersGuild[i].roleId == roleId)
            {
                return listPlayersGuild[i];
            }
        }
        return null;
    }

    
    /// <summary>
    /// 更新排序我的队伍玩家显示顺序列表
    /// </summary>
    public ILBetterList<team.TeamMember> SortDicTeamShowOrder(team.TeamInfo teaminfo)
    {
        if (teaminfo == null) return null;
        ILBetterList<team.TeamMember> listTeamShowOrder = new ILBetterList<team.TeamMember>();
        //Sort
        //队长放在第一个
        for (int i = 0; i < teaminfo.members.Count; i++)
        {
            if (teaminfo.members[i].roleId == teaminfo.leaderId)
            {
                listTeamShowOrder.Add(teaminfo.members[i]);
                //teaminfo.members.Remove(teaminfo.members[i]);
                break;
            }
        }
        //队员按等级、加入时间的优先级排序
        for (int i = 0; i < teaminfo.members.Count; i++)
        {
            for (int j = i+1; j < teaminfo.members.Count; j++)
            {
                if (teaminfo.members[j].level > teaminfo.members[i].level)
                {
                    team.TeamMember tempTeamMember = teaminfo.members[j];
                    teaminfo.members[j] = teaminfo.members[i];
                    teaminfo.members[i] = tempTeamMember;
                }
                else if (teaminfo.members[j].level == teaminfo.members[i].level)
                {
                    if (teaminfo.members[j].addTime < teaminfo.members[i].addTime)
                    {
                        team.TeamMember tempTeamMember = teaminfo.members[j];
                        teaminfo.members[j] = teaminfo.members[i];
                        teaminfo.members[i] = tempTeamMember;
                    }
                }
            }
        }

        for (int i = 0; i < teaminfo.members.Count; i++)
        {
            if (teaminfo.members[i].roleId != teaminfo.leaderId)
            {
                listTeamShowOrder.Add(teaminfo.members[i]);
            }
        }
            
        return listTeamShowOrder;
    }


    #region 接收网络消息处理数据函数
    /// <summary>
    /// 入队获取当前我的队伍信息
    /// </summary>
    /// <param name="teamInfo"></param>
    public void GetMyTeamData(team.TeamInfo msg)
    {
        if (null == msg) return;
        if (teamId == 0 && myTeamData == null)//当前没有队伍
        {
            myTeamData = new TeamData();
        }
        myTeamData.teamInfo = msg;
        teamId = msg.id;
        CSMainPlayerInfo.Instance.TeamId = msg.id;
        //HotManager.Instance.EventHandler.SendEvent(CEvent.OnMainPlayerTeamIdChanged, msg);
    }

    /// <summary>
    /// 获取我的队伍最新信息
    /// </summary>
    /// <param name="msg"></param>
    public void UpdateMyTeamData(team.GetTeamInfoResponse msg)
    {
        if (null == msg) return;
        if (myTeamData!=null)
        {
            myTeamData.teamInfo = msg.myTeam;
        }
    }

    /// <summary>
    /// 队长收到申请消息处理
    /// </summary>
    /// <param name="msg"></param>
    public void LeaderApplyMessage(team.TeamMember msg)
    {
        if (null == msg) return;
        //如果我是队长，才做处理
        if (myTeamData!=null && myTeamData.teamInfo.leaderId == CSMainPlayerInfo.Instance.ID)
        {
            if (!myTeamData.listLeaderApplyMessage.Contains(msg))//不能重复
            {
                myTeamData.listLeaderApplyMessage.Add(msg);
            }
        }
    }
    
    /// <summary>
    /// 入队响应处理
    /// </summary>
    /// <param name="msg"></param>
    public void JoinTeamMessage(team.JoinTeamResponse msg)
    {
        if (null == msg) return;
        //自己加入不需要处理，因为同时发了一个teamInfo大包，在GetMyTeamData那里处理就行
        //别人入队，只发这个包，所以要处理
        if (myTeamData!=null && msg.joiner.roleId != CSMainPlayerInfo.Instance.ID)
        {
            myTeamData.teamInfo.leaderId = msg.leaderId;
            myTeamData.teamInfo.members.Add(msg.joiner);
        }
    }

    /// <summary>
    /// 离队响应处理
    /// </summary>
    /// <param name="msg"></param>
    public void LeaveTeam(team.LeaveTeamResponse msg)
    {
        if (null == msg) return;
        //如果是本人离开队伍
        if (teamId!=0 && myTeamData!=null&&msg.roleId == CSMainPlayerInfo.Instance.ID)
        {
            myTeamData = null;
            teamId = 0;
            CSMainPlayerInfo.Instance.TeamId = 0;
            //HotManager.Instance.EventHandler.SendEvent(CEvent.OnMainPlayerTeamIdChanged, msg);
            return;
        }
        //其他情况
        if (myTeamData!=null)
        {
            myTeamData.teamInfo.leaderId = msg.leaderId;
            for (int i = 0; i < myTeamData.teamInfo.members.Count; i++)
            {
                if (myTeamData.teamInfo.members[i].roleId == msg.roleId)
                {
                    myTeamData.teamInfo.members.Remove(myTeamData.teamInfo.members[i]);
                    break;
                }
            }    
        }
    }

    /// <summary>
    /// 队长变更响应处理
    /// </summary>
    /// <param name="msg"></param>
    public void LeaderChangedMessage(team.TeamLeaderChanged msg)
    {
        if (null == msg) return;
        if (myTeamData!=null)
        {
            myTeamData.teamInfo.leaderId = msg.newLeaderId;
        }
    }


    /// <summary>
    /// 获取（附近、好友、行会）队伍信息
    /// </summary>
    /// <param name="msg"></param>
    public void GetTeamTabMessage(team.TeamTabInfo msg)
    {
        if (null == msg) return;
  
        switch ((TeamTab)msg.tab)
        {
            case TeamTab.TabNone:
                break;
            case TeamTab.RoundPlayers:
                //附近可组玩家
                listPlayersNearby.Clear();
                for (int i = 0; i < msg.players.Count; i++)
                {
                    listPlayersNearby.Add(msg.players[i]);
                }
                break;
            case TeamTab.Friends:
                //可组好友列表
                listPlayersFriend.Clear();
                for (int i = 0; i < msg.players.Count; i++)
                {
                    listPlayersFriend.Add(msg.players[i]);
                }
                break;
            case TeamTab.Union:
                //可组行会成员
                listPlayersGuild.Clear();
                for (int i = 0; i < msg.players.Count; i++)
                {
                    listPlayersGuild.Add(msg.players[i]);
                }
                break;
            case TeamTab.Applies:
                break;
            case TeamTab.RoundTeams:
                //附近队伍
                listTeamNearby.Clear();
                for (int i = 0; i < msg.teams.Count; i++)
                {
                    listTeamNearby.Add(msg.teams[i]);
                }
                break;
            case TeamTab.QuickJoinTeam:
                break;
            default:
                break;
        }
     
    }

    /// <summary>
    /// 更改组队模式响应
    /// </summary>
    /// <param name="msg"></param>
    public void ChangeTeamMode(team.SetTeamModeRequest msg)
    {
        if (null == msg) return;
        CSMainPlayerInfo.Instance.TeamMode = msg.teamMode;
    }

    /// <summary>
    /// 更新玩家（最大）血量、（最大）蓝量回包
    /// </summary>
    /// <param name="msg"></param>
    public void UpdatePlayerHpMpInfoMessage(team.PlayerHpMpInfo msg)
    {
        if (null == msg) return;
        for (int i = 0; i < myTeamData.teamInfo.members.Count; i++)
        {
            if (msg.id == myTeamData.teamInfo.members[i].roleId)
            {
                myTeamData.teamInfo.members[i].hp = msg.hp;
                myTeamData.teamInfo.members[i].maxHp = msg.maxHp;
                myTeamData.teamInfo.members[i].mp = msg.mp;
                myTeamData.teamInfo.members[i].maxMp = msg.maxMp;
                break;
            }
        }
    }

    /// <summary>
    /// 更新玩家等级回包
    /// </summary>
    /// <param name="msg"></param>
    public void UpdatePlayerLevelInfoMessage(team.PlayerLevelInfo msg)
    {
        if (null == msg) return;
        for (int i = 0; i < myTeamData.teamInfo.members.Count; i++)
        {
            if (msg.id == myTeamData.teamInfo.members[i].roleId)
            {
                myTeamData.teamInfo.members[i].level = msg.level;
                break;
            }
        }
    }

    /// <summary>
    /// 收到组队邀请时信息处理
    /// </summary>
    /// <param name="msg"></param>
    public void GetInviteTeamMessage(team.InviteTeamMsg msg)
    {
        if (null == msg) return;
        bool isExist = false;
        for (int i = 0; i < listPlayersInaitationForMe.Count; i++)
        {
            if (listPlayersInaitationForMe[i].inviter == msg.inviter)
            {
                isExist = true;
                break;
            }
        }
        
        if (!isExist)//不能重复
        {
            listPlayersInaitationForMe.Add(msg);
        }
    }

    #endregion

    public void ClearListPlayersInaitationForMe()
    {
        if (listPlayersInaitationForMe!=null&& listPlayersInaitationForMe.Count!=0)
        {
            listPlayersInaitationForMe.Clear();
        }
    }
}

public class TeamData
{
    /// <summary>
    /// 我的队伍信息
    /// </summary>
    public team.TeamInfo teamInfo = new team.TeamInfo();
    /// <summary>
    /// 队长收到的申请消息
    /// </summary>
    public ILBetterList<team.TeamMember> listLeaderApplyMessage = new ILBetterList<team.TeamMember>();
    ///// <summary>
    ///// 附近可组玩家列表、可组好友列表、可组行会列表、附近队伍列表
    ///// </summary>
    //public team.TeamTabInfo teamTabInfo = new team.TeamTabInfo();
    
}
