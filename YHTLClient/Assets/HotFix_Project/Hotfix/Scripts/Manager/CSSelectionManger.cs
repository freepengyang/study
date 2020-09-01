using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using user;

/// <summary>
/// 选中面板类型
/// </summary>
public enum PanelSelcetType
{
    RoleChat = 374, //聊天界面点击玩家
    FriendList = 375, //好友列表点击玩家
    RoleTeam = 376, //队伍中的人物
    WorldBossRank = 389, //世界boss伤害排行
    RoleScenes = 391, //场景中的人物
    GuildPanel = 435, //公会界面打开
    Ranking = 633, //排行榜
}

public enum MenuType
{
    TYPE_CHECK_INFO = 1, //查看信息
    TYPE_CHAT, //私聊
    TYPE_ADD_FRIEND, //添加好友
    TYPE_DELETE_FRIEND, //删除好友
    TYPE_APPLY_TEAM, //申请组队
    TYPE_INVITE_TEAM, //邀请入队
    TYPE_KICK_TEAM, //踢出队伍
    TYPE_ASSIGN_CAPTAIN, //转让队长
    TYPE_QUIT_TEAM, //退出队伍
    TYPE_DISSOLVE_TEAM, //解散队伍
    TYPE_PULL_BLACKLIST, //拉入黑名单
    TYPE_CANCEL_BLACKLIST, //取消黑名单
    TYPE_ADD_ENEMYP, //添加仇人
    TYPE_DELETE_ENEMY, //删除仇人
    TYPE_INVITE_GUILD,

    ///邀请入会
    TYPE_KICK_GUILD, //踢出行会
    TYPE_APPLY_GIVESPEAKLIMITS, //给与说话权限
    TYPE_CANCEL_GIVESPEAKLIMITS, //取消说话权限
    TYPE_JUBAO, //举报
    TYPE_COPY_NAME, //复制名称
    TYPE_ADJUST_GUILD_POS, //调整职位
}

public class CSSelectionManger : CSInfo<CSSelectionManger>
{
    public CSSelectionManger()
    {
    }

    public override void Dispose()
    {

    }

    /// <summary>
    /// 统一剔除按钮流程
    /// </summary>
    /// <param name="info"></param>
    public void ScreenButton(MenuInfo info, List<MenuType> menus)
    {
        if (info == null) return;
        TABLE.SUNDRY ConfigAllButton = null;
        if (!SundryTableManager.Instance.TryGetValue(info.sundryId, out ConfigAllButton)) return;
        if (string.IsNullOrEmpty(ConfigAllButton.effect)) return;
        List<int> listButtons = UtilityMainMath.SplitStringToIntList(ConfigAllButton.effect);
        menus.Clear();
        for (int i = 0; i < listButtons.Count; i++)
        {
            menus.Add((MenuType) listButtons[i]);
        }

        switch ((PanelSelcetType) info.sundryId)
        {
            case PanelSelcetType.RoleScenes:
                ScreenButtonForFriend(info, menus);
                ScreenButtonForTeam(info, menus);
                ScreenButtonForGuildWorld(info, menus);
                break;
            case PanelSelcetType.GuildPanel:
                ScreenButtonForFriend(info, menus);
                ScreenButtonForGuild(info, menus);
                break;
            case PanelSelcetType.RoleChat:
            case PanelSelcetType.FriendList:
                ScreenButtonForFriend(info, menus);
				ScreenButtonForGuildWorld(info, menus);
				break;
            case PanelSelcetType.RoleTeam:
                ScreenButtonForTeam(info, menus);
                ScreenButtonForFriend(info, menus);
                break;
            case PanelSelcetType.WorldBossRank:
                ScreenButtonForFriend(info, menus);
                break;
            case PanelSelcetType.Ranking:
                ScreenButtonForFriend(info, menus);
                ScreenButtonForGuildWorld(info, menus);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 剔除组队相关按钮
    /// </summary>
    /// <param name="info"></param>
    /// <param name="menus"></param>
    void ScreenButtonForTeam(MenuInfo info, List<MenuType> menus)
    {
        if (info == null) return;
        if (info.teamId > 0)
            menus.Remove(MenuType.TYPE_INVITE_TEAM);

        if (CSTeamInfo.Instance.TeamId <= 0)
        {
            menus.Remove(MenuType.TYPE_INVITE_TEAM);
            menus.Remove(MenuType.TYPE_KICK_TEAM);
            menus.Remove(MenuType.TYPE_ASSIGN_CAPTAIN);
            menus.Remove(MenuType.TYPE_QUIT_TEAM);
            menus.Remove(MenuType.TYPE_DISSOLVE_TEAM);
        }
        else
        {
            menus.Remove(MenuType.TYPE_APPLY_TEAM);
            if (info.teamId > 0)
                menus.Remove(MenuType.TYPE_INVITE_TEAM);

            if (info.teamId != CSTeamInfo.Instance.TeamId)
            {
                menus.Remove(MenuType.TYPE_KICK_TEAM);
                menus.Remove(MenuType.TYPE_ASSIGN_CAPTAIN);
            }

            if (info.selfTeamLeaderId != CSMainPlayerInfo.Instance.ID)
            {
                menus.Remove(MenuType.TYPE_KICK_TEAM);
                menus.Remove(MenuType.TYPE_ASSIGN_CAPTAIN);
                menus.Remove(MenuType.TYPE_DISSOLVE_TEAM);
            }
        }
    }

    /// <summary>
    /// 剔除好友相关按钮
    /// </summary>
    /// <param name="info"></param>
    /// <param name="menus"></param>
    void ScreenButtonForFriend(MenuInfo info, List<MenuType> menus)
    {
        if (info == null) return;
        var relation = CSFriendInfo.Instance.GetRelation(info.roleId);
        if (relation == FriendType.FT_FRIEND)
        {
            menus.Remove(MenuType.TYPE_ADD_FRIEND);
            menus.Remove(MenuType.TYPE_CANCEL_BLACKLIST);
            menus.Remove(MenuType.TYPE_DELETE_ENEMY);
			menus.Remove(MenuType.TYPE_PULL_BLACKLIST);
			menus.Remove(MenuType.TYPE_ADD_ENEMYP);
		}
        else if (relation == FriendType.FT_ENEMY)
        {
            menus.Remove(MenuType.TYPE_DELETE_FRIEND);
            menus.Remove(MenuType.TYPE_CANCEL_BLACKLIST);
            menus.Remove(MenuType.TYPE_ADD_ENEMYP);
        }
        else if (relation == FriendType.FT_BLACK_LIST)
        {
            menus.Remove(MenuType.TYPE_DELETE_FRIEND);
            menus.Remove(MenuType.TYPE_DELETE_ENEMY);
            menus.Remove(MenuType.TYPE_PULL_BLACKLIST);
            menus.Remove(MenuType.TYPE_CHAT);
        }
        else
        {
            menus.Remove(MenuType.TYPE_DELETE_FRIEND);
            menus.Remove(MenuType.TYPE_DELETE_ENEMY);
            menus.Remove(MenuType.TYPE_CANCEL_BLACKLIST);
        }

        if (!CSFriendInfo.Instance.CanAddFriend())
            menus.Remove(MenuType.TYPE_ADD_FRIEND);
    }

    /// <summary>
    /// 剔除公会相关按钮(针对本公会成员)
    /// </summary>
    /// <param name="info"></param>
    /// <param name="mMenus"></param>
    void ScreenButtonForGuild(MenuInfo info, List<MenuType> mMenus)
    {
        //邀请行会
        if (info.position != 0)
        {
            mMenus.Remove(MenuType.TYPE_INVITE_GUILD); //邀请入会
        }

        int myPosition = (int) CSMainPlayerInfo.Instance.GuildPos;
        //踢出行会
        if (info.myPosition >= info.position || myPosition > (int) GuildPos.VicePresident)
        {
            mMenus.Remove(MenuType.TYPE_KICK_GUILD);
        }

        //调整职位
        if (myPosition >= info.position || myPosition > (int) GuildPos.Presbyter)
        {
            mMenus.Remove(MenuType.TYPE_ADJUST_GUILD_POS); //调整职位
        }

        //语音权限
        if (myPosition != (int)GuildPos.President/* || !info.isOnline*/)
        {
            mMenus.Remove(MenuType.TYPE_APPLY_GIVESPEAKLIMITS); //给与说话权限
            mMenus.Remove(MenuType.TYPE_CANCEL_GIVESPEAKLIMITS); //取消说话权限
        }
        else
        {
            if(info.isSpeakLimit)
            {
                mMenus.Remove(MenuType.TYPE_APPLY_GIVESPEAKLIMITS); //给与说话权限
            }
            else
            {
                mMenus.Remove(MenuType.TYPE_CANCEL_GIVESPEAKLIMITS); //取消说话权限
            }
        }
    }

    /// <summary>
    /// 剔除公会相关按钮(针对全世界成员)
    /// </summary>
    /// <param name="info"></param>
    /// <param name="mMenus"></param>
    void ScreenButtonForGuildWorld(MenuInfo info, List<MenuType> mMenus)
    {
        if (info.guildId == CSMainPlayerInfo.Instance.GuildId && info.guildId != 0)
        {
            ScreenButtonForGuild(info, mMenus);
        }
        else
        {
            int myPosition = (int) CSMainPlayerInfo.Instance.GuildPos;
            if (CSMainPlayerInfo.Instance.GuildId == 0|| info.guildId>0||myPosition<1||myPosition>3)
            {
                mMenus.Remove(MenuType.TYPE_INVITE_GUILD);//邀请入会
            }
            mMenus.Remove(MenuType.TYPE_KICK_GUILD);//踢出行会
        }
    }
}

/// <summary>
/// 选中信息
/// </summary>
public class MenuInfo
{
    public int sundryId; //杂项表Id
    public long roleId;
    public string roleName;
    public long teamId;
    public long selfTeamLeaderId; //所在队伍队长
    public long guildId;
    public int position;
    public int contribute;
    public bool isOnline;
    public bool isSpeakLimit;
    public int enemry;
    public int sex;
    public int nation;
    public bool isBaby;
    public int flowerId = 38000006;
    public int photo;
    public int lv;
    public int career;
    public int myPosition;
    public int hp;
    public int mp;
    public int maxHp;
    public int maxMp;

    public MenuInfo(long _roleId, string _roleName)
    {
        this.roleId = _roleId;
        this.roleName = _roleName;
        this.teamId = 0L;
        this.position = 0;
        this.contribute = 0;
        this.isOnline = true;
        this.isSpeakLimit = false;
        this.enemry = 0;
    }

    public void SetCountryInfo(long _roleId, string _roleName, int lv, int career, int photo, int Position)
    {
        this.roleId = _roleId;
        this.roleName = _roleName;
        this.lv = lv;
        this.career = career;
        this.photo = photo;
        this.position = Position;
    }


    public void SetKuafuInfo(long _roleId, string _roleName, int lv, int photo, int teamPosition, int career,
        int MyPosition = 0)
    {
        this.roleId = _roleId;
        this.roleName = _roleName;
        this.lv = lv;
        this.photo = photo;
        this.position = teamPosition;
        this.myPosition = MyPosition;
        this.career = career;
    }

    public MenuInfo(long _roleId, string _roleName, int photo, int lv, int career)
    {
        this.roleId = _roleId;
        this.roleName = _roleName;
        this.teamId = 0L;
        this.position = 0;
        this.contribute = 0;
        this.isOnline = true;
        this.isSpeakLimit = false;
        this.enemry = 0;
        this.photo = photo;
        this.lv = lv;
        this.career = career;
    }

    public MenuInfo(long _roleId, string _roleName, long _teamId, int _position, int sex, int enemry, int nation,
        int photo, int lv, int career)
    {
        this.roleId = _roleId;
        this.roleName = _roleName;
        this.teamId = _teamId;
        this.position = _position;
        this.contribute = 0;
        this.isOnline = true;
        this.isSpeakLimit = false;
        this.enemry = enemry;
        this.nation = nation;
        this.sex = sex;
        this.photo = photo;
        this.lv = lv;
        this.career = career;
    }

    public MenuInfo()
    {
    }

    public void SetFriendTips(long _roleId, string _roleName, int enemry, int sex, int flowerid, int lv, int career,
        int photo, int nationID)
    {
        this.roleId = _roleId;
        this.roleName = _roleName;
        this.teamId = 0L;
        this.position = 0;
        this.contribute = 0;
        this.isOnline = true;
        this.isSpeakLimit = false;
        this.enemry = enemry;
        this.sex = sex;
        this.flowerId = flowerid;
        this.lv = lv;
        this.career = career;
        this.photo = photo;
        nation = nationID;
    }

    public void SetRankTips(long _roleId, string _roleName, int sex, int head, int lv, int career)
    {
        this.roleId = _roleId;
        this.roleName = _roleName;
        this.teamId = 0L;
        this.position = 0;
        this.contribute = 0;
        this.isOnline = true;
        this.isSpeakLimit = false;
        this.enemry = 0;
        this.sex = sex;
        photo = head;
        this.lv = lv;
        this.career = career;
    }

    public void SetFamilyTips(long _roleId, string _roleName, long _teamId, int _position, int _contribute,
        bool isOnline, bool isSpeak, bool isBaby, int sex, int head, int lv, int career)
    {
        this.roleId = _roleId;
        this.roleName = _roleName;
        this.teamId = _teamId;
        this.position = _position;
        this.contribute = _contribute;
        this.isOnline = isOnline;
        this.isSpeakLimit = isSpeak;
        this.enemry = 0;
        this.isBaby = isBaby;
        this.sex = sex;
        this.photo = head;
        this.lv = lv;
        this.career = career;
    }

    public void SetChatTips(long _roleId, string _roleName, long _teamId, int _position, int sex, int enemry, int photo,
        int lv, int nation, int career)
    {
        this.roleId = _roleId;
        this.roleName = _roleName;
        this.teamId = _teamId;
        this.position = _position;
        this.contribute = 0;
        this.isOnline = true;
        this.isSpeakLimit = false;
        this.enemry = enemry;
        this.sex = sex;
        this.photo = photo;
        this.lv = lv;
        this.nation = nation;
        this.career = career;
    }

    public void SetTeamTips(long _roleId, string _roleName, int sex, int lv, int photo, int career, long teamID)
    {
        this.roleId = _roleId;
        this.roleName = _roleName;
        this.teamId = 0L;
        this.position = 0;
        this.contribute = 0;
        this.isOnline = true;
        this.isSpeakLimit = false;
        this.enemry = 0;
        this.sex = sex;
        this.lv = lv;
        this.photo = photo;
        this.career = career;
        this.teamId = teamID;
    }
}