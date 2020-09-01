using Google.Protobuf.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TABLE;
using UnityEngine;
public enum GuildPos
{
    Member = 5,//会员
    Elite = 4,//精英
    Presbyter = 3,//长老
    VicePresident = 2,//副会长
    President = 1,//会长
}

//enum UnionTab
//{
//    TabNone = 0,
//    UnionsList = 1,//帮会列表
//    MainInfo = 2,//帮会信息
//    UnionStoreHouse = 3,//帮会仓库
//    SouvenirWealthPacks = 4,//帮会红包
//    UnionLogMessages = 7,//帮会消息
//    UnionApplyInfos = 8,//申请信息
//    UnionMemberInfo = 10,//帮会成员
//}

public class CSGuildInfo : CSInfo<CSGuildInfo>
{
    public int CreateGuildNeedVip
    {
        get;private set;
    }
    public int CreateBuildMoneyCost
    {
        get;private set;
    }
    public long GoldCount
    {
        get
        {
            return CSItemCountManager.Instance.GetItemCount(MoneyId);
        }
    }
    public int MoneyId
    {
        private set;
        get;
    }

    public int DonateMoneyID
    {
        get
        {
            return (int)MoneyType.gold;
        }
    }

    public int GuildRenameId
    {
        get
        {
            return 50000502;
        }
    }

    int warTimes = 0;
    public int WarTimes
    {
        get
        {
            return warTimes;
        }
        private set
        {
            warTimes = value;
        }
    }

    public int WarCost
    {
        get
        {
            return 100; 
        }
    }


    public void Initialize()
    {
        _storeReincarnation = -1;
        InitCreateGuildCost();
        InitVipLevel();
        InitDonateValues();
        InitGuildPosName();
        InitGuildRebuild();
        ParseRedPkgValues();

        CurImproveId = UnionBuffTableManager.Instance.make_id(1, 0);
        WarTimes = 0;
    }

    protected Dictionary<long, chat.RoleInfo> mUpMicPlayers = new Dictionary<long, chat.RoleInfo>(16);
    public void RefreshUpMicPlayers(RepeatedField<chat.RoleInfo> roleInfos)
    {
        mUpMicPlayers.Clear();
        for(int i = 0,max = roleInfos.Count; i < max; ++i)
        {
            var roleInfo = roleInfos[i];
            mUpMicPlayers.Add(roleInfo.id,roleInfo);
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnRoleDetailNtfMessage);
    }

    public bool IsPlayerUpMic(long roleId)
    {
        return mUpMicPlayers.ContainsKey(roleId);
    }

    public bool IsPresident
    {
        get
        {
            return CSMainPlayerInfo.Instance.GuildPos == (int)GuildPos.President;
        }
    }

    public int RedPacketMinNum
    {
        get;private set;
    }
    public int RedPacketMaxNum
    {
        get; private set;
    }
    public int RedGoldMinValue
    {
        get; private set;
    }
    public int RedGoldMaxValue
    {
        get; private set;
    }
    protected void ParseRedPkgValues()
    {
        int a, b, c, d;
        a = b = c = d = 0;
        TABLE.SUNDRY sundryItem;
        if (SundryTableManager.Instance.TryGetValue(450, out sundryItem))
        {
            var tokens = sundryItem.effect.Split('#');
            if (tokens.Length > 0)
                int.TryParse(tokens[0], out a);
            if (tokens.Length > 1)
                int.TryParse(tokens[1], out b);
            if (tokens.Length > 2)
                int.TryParse(tokens[2], out c);
            if (tokens.Length > 3)
                int.TryParse(tokens[3], out d);
        }
        RedGoldMinValue = a;
        RedGoldMaxValue = b;
        RedPacketMinNum = c;
        RedPacketMaxNum = d;
    }


    int mergeDay, mergeH, mergeM;

    private void InitGuildRebuild()
    {
        mergeDay = 0;
        mergeH = 0;
        mergeM = 0;

        TABLE.SUNDRY tblSundry;
        if (!SundryTableManager.Instance.TryGetValue(437, out tblSundry))
        {
            return;
        }

        var times = tblSundry.effect.Split('#');
        if (times.Length != 3)
            return;

        int.TryParse(times[0], out mergeDay);
        int.TryParse(times[1], out mergeH);
        int.TryParse(times[2], out mergeM);
    }

    float lastCallTime = -1.0f;
    public bool IsCallGuildInCD(bool needTips = false)
    {
        int delta = (int)(Time.realtimeSinceStartup - lastCallTime);
        if(lastCallTime > 0 && delta < 300)
        {
            delta = 300 - delta;
            if (needTips)
            {
                int min = delta / 60;
                int sec = delta % 60;
                UtilityTips.ShowRedTips(1851, min,sec);
            }    
            return true;
        }
        lastCallTime = Time.realtimeSinceStartup;
        return false;
    }

    public bool CanRebuild()
    {
        int serverOpenTime = 0;
        //var openDays = CSMainPlayerInfo.Instance.RoleExtraValues?.openServerDays;
        long milliSecond = serverOpenTime + mergeDay * 24 * 3600 * 1000 + mergeH * 3600 * 1000 + mergeM * 60 * 1000;
        return CSServerTime.Instance.TotalMillisecond >= milliSecond;
    }

    public void InvitePlayerJoinGuild(long roleId,string playerName)
    {
        if (!Utility.HasGuild())
            UtilityTips.ShowRedTips(970);
        else
        {
            if (CSMainPlayerInfo.Instance.GuildPos > (int)GuildPos.Presbyter)
                UtilityTips.ShowRedTips(960);
            else
            {
                UtilityTips.ShowGreenTips(1523,playerName);
                Net.CSInviteUnionMessage(roleId);
            }
        }
    }

    public void KickGuildPlayer(int contribute,long roleId,string roleName)
    {
        if (contribute >= 10000)
        {
            UtilityTips.ShowPromptWordTips(57, () =>
             {
                 Net.CSLeaveUnionMessage(roleId);
                 Net.CSGetUnionTabMessage((int)UnionTab.UnionMemberInfo);
             }, contribute);
        }
        else
        {
            UtilityTips.ShowPromptWordTips(58, null, () =>
              {
                  Net.CSLeaveUnionMessage(roleId);
                  Net.CSGetUnionTabMessage((int)UnionTab.UnionMemberInfo);
              }, roleName);
        }
    }

    protected void InitCreateGuildCost()
    {
        MoneyId = 1;
        CreateBuildMoneyCost = 10000;
        int sundryId = 281;
        TABLE.SUNDRY sundryItem;
        if (!SundryTableManager.Instance.TryGetValue(sundryId, out sundryItem))
        {
            return;
        }
        var tokens = sundryItem.effect.Split('#');
        if(tokens.Length != 2)
        {
            return;
        }
        int moneyId, moneyCnt;
        if (!int.TryParse(tokens[0], out moneyId) || !int.TryParse(tokens[1], out moneyCnt))
            return;
        MoneyId = moneyId;
        CreateBuildMoneyCost = moneyCnt;
    }

    protected void InitVipLevel()
    {
        int sundryId = 282;
        TABLE.SUNDRY sundryItem;
        CreateGuildNeedVip = 3;
        int vipLv;
        if (!SundryTableManager.Instance.TryGetValue(sundryId, out sundryItem))
        {
            return;
        }

        if (!int.TryParse(sundryItem.effect, out vipLv))
        {
            return;
        }

        CreateGuildNeedVip = vipLv;
    }

    protected int donateMinValue = 10000;
    public int DonateMinValue
    {
        get
        {
            return donateMinValue;
        }
    }
    protected int donateUnionAttribute = 150;
    public int DonateGetGuildAttribute
    {
        get
        {
            return donateUnionAttribute;
        }
    }
    protected int donateGetUnionGold = 150;
    public int DonateGetGuildGold
    {
        get
        {
            return donateGetUnionGold;
        }
    }
    protected void InitDonateValues()
    {
        int sundryId = 287;
        TABLE.SUNDRY sundryItem;
        if (!SundryTableManager.Instance.TryGetValue(sundryId, out sundryItem))
        {
            return;
        }
        var tokens = sundryItem.effect.Split('#');
        if (tokens.Length != 3)
            return;
        int v1, v2, v3;
        if (!int.TryParse(tokens[0], out v1))
            return;
        if (!int.TryParse(tokens[1], out v2))
            return;
        if (!int.TryParse(tokens[2], out v3))
            return;
        donateMinValue = v1;
        donateUnionAttribute = v2;
        donateGetUnionGold = v3;
    }

    string[] mPositions = new string[5];
    string[] mPosColors = new string[5];
    protected void InitGuildPosName()
    {
        int sundryId = 433;
        TABLE.SUNDRY sundryItem;
        if (!SundryTableManager.Instance.TryGetValue(sundryId, out sundryItem))
        {
            return;
        }
        var tokens = sundryItem.effect.Split('&');
        if (tokens.Length != 5)
            return;
        for(int i = 0; i < tokens.Length; ++i)
        {
            var kv = tokens[i].Split('#');
            if (kv.Length > 0)
                mPosColors[i] = kv[0];
            if (kv.Length > 1)
                mPositions[i] = kv[1];
        }
    }

    public string GetGuildPos(int pos)
    {
        if (pos > 0 && pos <= mPositions.Length)
            return mPositions[pos - 1];
        return string.Empty;
    }

    public string GetPosColor(int pos)
    {
        if (pos >= 0 && pos < mPosColors.Length)
            return mPosColors[pos];
        return string.Empty;
    }

    int mImpeachmentId = 0;
    public int ImpeachmentCostItemId
    {
        get
        {
            if(0 == mImpeachmentId)
            {
                InitImpeachment();
            }
            return mImpeachmentId;
        }
    }
    int mImpeachmentCount = 0;
    public int ImpeachmentCostItemCount
    {
        get
        {
            if (0 == mImpeachmentCount)
            {
                InitImpeachment();
            }
            return mImpeachmentCount;
        }
    }
    protected void InitImpeachment()
    {
        mImpeachmentId = 0;
        mImpeachmentCount = 500;
        int sundryId = 284;
        TABLE.SUNDRY sundryItem;
        if (!SundryTableManager.Instance.TryGetValue(sundryId, out sundryItem))
        {
            return;
        }
        var tokens = sundryItem.effect.Split('#');
        if (tokens.Length != 2)
        {
            return;
        }
        int costId, costCount;
        if (!int.TryParse(tokens[0], out costId) || !int.TryParse(tokens[1], out costCount))
            return;
        mImpeachmentId = costId;
        mImpeachmentCount = costCount;
    }

    public void SetGuildInfo(union.UnionInfo info)
    {
        if(null != info)
        {
            union.GetUnionTabResponse tabData = null;
            if (!mUnionTabDatas.ContainsKey(UnionTab.MainInfo))
            {
                tabData = new union.GetUnionTabResponse();
                tabData.tab = (int)UnionTab.MainInfo;
                mUnionTabDatas.Add(UnionTab.MainInfo, tabData);
            }
            else
            {
                tabData = mUnionTabDatas[UnionTab.MainInfo];
            }
            tabData.unionInfo = info;
            long guildId = info.brief.unionId;
            string guildName = info.brief.name;
            int guildLevel = info.brief.level;
            long guildCreateId = info.brief.leaderId;
            CSMainPlayerInfo.Instance.GuildId = guildId;
            CSMainPlayerInfo.Instance.GuildName = guildName;
            CSMainPlayerInfo.Instance.GuildLevel = guildLevel;
            CSMainPlayerInfo.Instance.GuildCreateId = guildCreateId;
            if (null != info.members)
            {
                for (int i = 0; i < info.members.Count; ++i)
                {
                    if (info.members[i].roleId == CSMainPlayerInfo.Instance.ID)
                    {
                        CSMainPlayerInfo.Instance.GuildPos = info.members[i].position;
                    }
                    else
                    {
                        if(CSAvatarManager.Instance.GetAvatarInfo(info.members[i].roleId) is CSPlayerInfo playerInfo)
                        {
                            playerInfo.GuildId = guildId;
                            playerInfo.GuildName = guildName;
                            playerInfo.GuildLevel = guildLevel;
                            playerInfo.GuildCreateId = guildCreateId;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 所有的家族的信息
    /// </summary>
    private union.ApplyUnionListResponse _applyUnionList;
    public union.ApplyUnionListResponse applyUnionList
    {
        get { return _applyUnionList; }
        set
        {
            if (_applyUnionList != value)
            {
                _applyUnionList = value;
                HotManager.Instance.EventHandler.SendEvent(CEvent.OnGuildApplyUnionListChanged);
            }
        }
    }

#if UNITY_EDITOR
    RepeatedField<union.UnionBrief> mDebugUnionList = new RepeatedField<union.UnionBrief>();
    public void MakeDebugUnionBriefs()
    {
        var guildInfo = GetGuildInfo();
        mDebugUnionList.Clear();
        int cnt = UnityEngine.Random.Range(1,10);
        for(int i = 0; i < cnt; ++i)
        {
            union.UnionBrief brief = new union.UnionBrief();
            brief.createTime = guildInfo.brief.createTime;
            brief.declareWarTime = guildInfo.brief.declareWarTime;
            brief.isLeaderOnline = UnityEngine.Random.Range(1, 10) % 2 == 1;
            brief.leaderId = guildInfo.brief.leaderId;
            brief.leaderName = guildInfo.brief.leaderName;
            brief.level = guildInfo.brief.level;
            brief.name = guildInfo.brief.name + i.ToString();
            brief.unionId = guildInfo.brief.unionId;
            mDebugUnionList.Add(brief);
        }
    }
#endif
    public RepeatedField<union.UnionBrief> UnionList
    {
        get
        {
            //MakeDebugUnionBriefs();
            //return mDebugUnionList;
            var unionList = CSGuildInfo.Instance.GetTabInfo(UnionTab.UnionsList);
            if (null != unionList)
                return unionList.unions;
            return null;
        }
    }

    union.InviteUnionMsg _inviteMsg;
    public union.InviteUnionMsg inviteUnion
    {
        get
        {
            return _inviteMsg;
        }
        set
        {
            if (CSConfigInfo.Instance.GetBool(ConfigOption.ForbidGuild))
            {
                _inviteMsg = null;
                FNDebug.LogFormat("<color=#ff0000>[公会邀请]:[禁止公会邀请]</color>");
            }
            else
            {
                _inviteMsg = value;
            }
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnGuildInviteMessage);
        }
    }

    private List<long> enemyUnionIds = new List<long>();//敌对家族id
    public List<long> EnemyUnionIds
    {
        get { return enemyUnionIds; }
        set { enemyUnionIds = value; }
    }

    /// <summary>
    /// 加入家族的时间（毫秒级）
    /// </summary>
    private long joinFamilyTime = 0;
    public long JoinFamilyTime
    {
        set { joinFamilyTime = value; }
        get { return joinFamilyTime; }
    }

    /// <summary>
    /// 当前修炼类型
    /// </summary>
    public int CurPosition
    {
        get
        {
            return mCurImproveId & 0xFF;
        }
    }

    /// <summary>
    /// 当前修炼等级
    /// </summary>
    public int CurLevel
    {
        get
        {
            return (mCurImproveId >> 8) & 0xFF;
        }
    }

    /// <summary>
    /// 当前已经修炼的ID
    /// </summary>
    int mCurImproveId;
    public int CurImproveId
    {
        get
        {
            return mCurImproveId;
        }
        private set
        {
            mCurImproveId = value;
            mNextImproveId = NextImprove(mCurImproveId);
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnGuildPracticeImproved);
        }
    }
    /// <summary>
    /// 下一级修炼ID
    /// </summary>
    int mNextImproveId = 257;
    public int NextImproveId
    {
        get
        {
            return mNextImproveId;
        }
    }
    Dictionary<int, int> mImproveDic = new Dictionary<int, int>(6);
    public void InitImproves(RepeatedField<union.Improve> infos)
    {
        mImproveDic.Clear();
        int curImproveId = UnionBuffTableManager.Instance.make_id(1, 0);
        for (int i = 0; i < infos.Count; i++)
        {
            var positon = infos[i].position;
            if (mImproveDic.ContainsKey(positon))
                continue;

            mImproveDic.Add(positon, infos[i].level);
            curImproveId = Math.Max(UnionBuffTableManager.Instance.make_id(positon, infos[i].level), curImproveId);
        }
        CurImproveId = curImproveId;
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnGuildPracticeInitialized);
    }

    readonly List<int> mPracticePositions = new List<int>
    {
        1,2,3,4,5,6,
    };

    public List<int> PracticePositions
    {
        get
        {
            return mPracticePositions;
        }
    }

    int guildPracticeNeedGuildLv = -1;
    public int GuildPracticeNeedGuildLv
    {
        get
        {
            if (guildPracticeNeedGuildLv == -1)
            {
                guildPracticeNeedGuildLv = 0;
                int sundryId = 471;
                TABLE.SUNDRY sundryItem;
                if (SundryTableManager.Instance.TryGetValue(sundryId, out sundryItem))
                {
                    int.TryParse(sundryItem.effect, out guildPracticeNeedGuildLv);
                }
            }
            return guildPracticeNeedGuildLv;
        }
    }

    public bool IsGuildPractiseOpened(bool callTips)
    {
        var guildInfo = GetGuildInfo();
        if (null == guildInfo || guildInfo.brief.level < GuildPracticeNeedGuildLv)
        {
            if(callTips)
                UtilityTips.ShowRedTips(941,GuildPracticeNeedGuildLv);
            return false;
        }
        return true;
    }


    public int GetImproveLevel(int position)
    {
        if (mImproveDic.ContainsKey(position))
            return mImproveDic[position];
        return 0;
    }

    public void Improve(int position,int level)
    {
        var improveId = UnionBuffTableManager.Instance.make_id(position,level);
        TABLE.UNIONBUFF buffer;
        if(!UnionBuffTableManager.Instance.TryGetValue(improveId,out buffer))
        {
            return;
        }
        if (mImproveDic.ContainsKey(position))
        {
            mImproveDic[position] = level;
        }
        else
        {
            mImproveDic.Add(position, level);
        }
        CurImproveId = improveId;
    }

    public int NextPosition(int improveId)
    {
        int position = improveId & 0xFF;
        int level = (improveId >> 8) & 0xFF;
        if (level > 0)
            position += 1;
        return position > 6 ? 1 : position;
    }
    /// <summary>
    /// 升级时候用
    /// </summary>
    /// <param name="curImproveId"></param>
    /// <returns></returns>
    public int NextImprove(int curImproveId)
    {
        TABLE.UNIONBUFF improveItem;
        if (!UnionBuffTableManager.Instance.TryGetValue(curImproveId, out improveItem))
        {
            return UnionBuffTableManager.Instance.make_id(1,1);
        }

        int position = NextPosition(curImproveId);
        bool stageUp = position < improveItem.Position;

        int level = improveItem.Level <= 0 ? 1 : (stageUp ? improveItem.Level + 1 : improveItem.Level);

        int nextImproveId = UnionBuffTableManager.Instance.make_id(position,level);

        TABLE.UNIONBUFF nextImproveItem;
        if (!UnionBuffTableManager.Instance.TryGetValue(nextImproveId, out nextImproveItem))
        {
            return curImproveId;
        }

        return nextImproveId;
    }
    /// <summary>
    /// 同等级的用
    /// </summary>
    /// <param name="curImproveId"></param>
    /// <returns></returns>
    public int NextLevelImprove(int curImproveId)
    {
        TABLE.UNIONBUFF improveItem;
        if (!UnionBuffTableManager.Instance.TryGetValue(curImproveId, out improveItem))
        {
            return curImproveId;
        }

        int position = improveItem.Position;
        int level = improveItem.Level + 1;
        int nextImproveId = UnionBuffTableManager.Instance.make_id(position, level);

        TABLE.UNIONBUFF nextImproveItem;
        if (!UnionBuffTableManager.Instance.TryGetValue(nextImproveId, out nextImproveItem))
        {
            return curImproveId;
        }

        return nextImproveId;
    }


    public bool ImproveFull
    {
        get
        {
            return mCurImproveId == mNextImproveId;
        }
    }

    public bool CheckGuildListRedPoint()
    {
        if (!Utility.HasGuild())
        {
            return false;
        }

        if (CSMainPlayerInfo.Instance.GuildPos != (int)GuildPos.President)
            return false;

        var unionList = CSGuildInfo.Instance.GetTabInfo(UnionTab.UnionsList);
        if (null == unionList || null == unionList.unions)
            return false;

        var unions = unionList.unions;
        for(int i = 0; i < unions.Count; ++i)
        {
            if (CanCombinedUnion(unions[i].unionId))
                return true;
        }

        return false;
    }

    public bool CheckApplyListRedPoint()
    {
        if(!Utility.HasGuild())
        {
            return false;
        }

        if (CSMainPlayerInfo.Instance.GuildPos > (int)GuildPos.Presbyter)
            return false;

        var applyInfos = CSGuildInfo.Instance.GetTabInfo(UnionTab.UnionApplyInfos);
        if (null == applyInfos)
            return false;

        if (applyInfos.applies.Count <= 0)
            return false;

        return true;
    }

    public bool CheckCanUpgradePractice()
    {
        return Utility.HasGuild() && IsGuildPractiseOpened(false) && CheckImprove(false);
    }

    public bool CheckImprove(bool callTips)
    {
        if (ImproveFull)
        {
            if(callTips)
                UtilityTips.ShowRedTips(875);
            return false;
        }

        TABLE.UNIONBUFF unionBuff = null;
        if (!UnionBuffTableManager.Instance.TryGetValue(CSGuildInfo.Instance.NextImproveId, out unionBuff))
        {
            return false;
        }

        if (!unionBuff.cost.IsItemsEnough(0, callTips))
        {
            return false;
        }

        //家族修改注释掉等级要求--黄杰
        if (unionBuff.Level > CSMainPlayerInfo.Instance.GuildLevel)
        {
            if (callTips)
                UtilityTips.ShowRedTips(877);
            return false;
        }

        return true;
    }

    protected Dictionary<UnionTab, union.GetUnionTabResponse> mUnionTabDatas = new Dictionary<UnionTab, union.GetUnionTabResponse>(16);
    /// <summary>
    /// 数据刷新
    /// </summary>
    public void SetUnionTabMessage(union.GetUnionTabResponse data)
    {
        if (data == null) return;

        var tab = (UnionTab)data.tab;
        if (!mUnionTabDatas.ContainsKey(tab))
        {
            mUnionTabDatas.Add(tab, data);
        }
        else
        {
            mUnionTabDatas[tab] = data;
        }

        switch (tab)
        {
            case UnionTab.UnionsList:
                HotManager.Instance.EventHandler.SendEvent(CEvent.OnGuildApplyUnionListChanged);
                break;
            case UnionTab.MainInfo:
                SetGuildInfo(data.unionInfo);
                break;
            case UnionTab.UnionStoreHouse:
                SetBagItemInfos(data.storehouse,data.storeReincarnation);
                break;
            case UnionTab.SouvenirWealthPacks:
                break;
            case UnionTab.UnionLogMessages:
                break;
            case UnionTab.UnionApplyInfos:
                break;
            case UnionTab.UnionMemberInfo:
                //CSMainPlayerInfo.Instance.GuildPos = null == info ? 0 : info
                //SetFamilyNumber(data);
                //UIRedPointManager.Instance.DispatchOpenUnionRedPoint();
                break;
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnGuildTabDataChanged, tab);
    }

    public void SetUnionList(union.UnionList unionList)
    {
        var tabId = UnionTab.UnionsList;
        if (!mUnionTabDatas.ContainsKey(tabId))
        {
            union.GetUnionTabResponse tab = new union.GetUnionTabResponse();
            tab.unions.Clear();
            tab.unions.AddRange(unionList.unions);
            mUnionTabDatas.Add(tabId,tab);
        }
        else
        {
            var tab = mUnionTabDatas[tabId];
            tab.unions.Clear();
            tab.unions.AddRange(unionList.unions);
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnGuildApplyUnionListChanged);
    }

    public void SortRedPacketList(ref long sortValue,union.UnionRedPackageInfo info)
    {
        sortValue = (!info.haveTaken && !info.empty) ? 1 : info.haveTaken ? 2 : 3;
    }

    public bool IsCanSendRedPacket()
    {
        return true;
    }

    const long maxOffLineTime = 3600 * 24 * 3;
    public bool CheckCanAccuseChief()
    {
        if (!Utility.HasGuild())
            return false;
        var guildInfo = GetGuildInfo();
        if (null == guildInfo)
            return false;
        if (CSMainPlayerInfo.Instance.GuildPos == (int)GuildPos.President)
            return false;
        if (null == guildInfo.members)
            return false;
        var members = guildInfo.members;
        for(int i = 0; i < members.Count; ++i)
        {
            if(members[i].roleId != guildInfo.brief.leaderId)
            {
                continue;
            }

            if(members[i].isOnline)
            {
                return false;
            }

            long destime = CSServerTime.DateTimeToStamp(CSServerTime.Now) - members[i].lastLogoutTime / 1000;
            if (destime < maxOffLineTime)
                return false;
        }
        return true;
    }

    public bool CanCombinedUnion(long unionID)
    {
        if (!Utility.HasGuild())
            return false;

        if (unionID == CSMainPlayerInfo.Instance.GuildId)
        {
            return false;
        }

        var tabInfo = GetTabInfo(UnionTab.UnionsList);
        if(null == tabInfo || null == tabInfo.unions || tabInfo.unions.Count <= 0)
            return false;

        var noCombineID = tabInfo.noCombineUnionId;

        if (noCombineID != null && noCombineID.Count > 1)
        {
            if (noCombineID[0] != -1)
            {
                if (noCombineID.Contains(unionID) && noCombineID.Contains(CSMainPlayerInfo.Instance.GuildId))
                {
                    return false;
                }
            }
            else
            {
                if (noCombineID.Contains(unionID) || noCombineID.Contains(CSMainPlayerInfo.Instance.GuildId))
                {
                    return false;
                }
            }
        }

        return false;
    }

    public union.GetUnionTabResponse GetTabInfo(UnionTab tab)
    {
        if (mUnionTabDatas.ContainsKey(tab))
            return mUnionTabDatas[tab];
        return null;
    }

    public union.UnionInfo GetGuildInfo()
    {
        var tabInfo = GetTabInfo(UnionTab.MainInfo);
        if (null != tabInfo)
            return tabInfo.unionInfo;
        return null;
    }

    public bool CanWar(union.UnionBrief brief)
    {
        if (null == brief)
            return false;

        if(!Utility.HasGuild())
        {
            return false;
        }

        if(CSMainPlayerInfo.Instance.GuildPos != (int)GuildPos.President)
        {
            return false;
        }

        //不能向自己宣战
        if (brief.unionId == CSMainPlayerInfo.Instance.GuildId)
            return false;

        //已经进入宣战状态，无法宣战
        if (brief.declareWarTime > 0)
            return false;

        return true;
    }

    public bool IsInApplyColdTime(long time)
    {
        return CSServerTime.StampToDateTime(time).AddHours(2) > CSServerTime.Now;
    }

    UnionTab prevTab = UnionTab.UnionsList;
    public UnionTab PrevTab
    {
        get
        {
            return prevTab;
        }
    }
    UnionTab tab = UnionTab.UnionsList;
    public UnionTab Tab
    {
        get
        {
            return tab;
        }
        set
        {
            prevTab = tab;
            tab = value;
        }
    }
    public void RefreshCurrentTab()
    {
        Net.CSGetUnionTabMessage((int)Tab);
    }

    #region 家族弹劾
    private union.ImpeachementMsg ImpeachMsg;
    public union.ImpeachementMsg mImpeachMsg { get { return ImpeachMsg; } set { ImpeachMsg = value; } }

    /// <summary>
    /// 是否显示弹劾气泡
    /// </summary>
    public bool mIsShowImpeachBubble = false;
    #endregion

    #region 家族背包
    private FastArrayElementKeepHandle<bag.BagItemInfo> _mbagItemInfos = new FastArrayElementKeepHandle<bag.BagItemInfo>(32);
    public FastArrayElementKeepHandle<bag.BagItemInfo> mbagItemInfos
    {
        get { return _mbagItemInfos; }
    }

    int _storeReincarnation = -1;
    public int storeReincarnation
    {
        get
        {
            return _storeReincarnation;
        }
        private set
        {
            _storeReincarnation = value;
        }
    }
    void SetBagItemInfos(RepeatedField<bag.BagItemInfo> bagItems,int storeReincarnation)
    {
        this.storeReincarnation = storeReincarnation;
        _mbagItemInfos.Clear();
        if(null != bagItems)
        {
            _mbagItemInfos.AddRange(bagItems);
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnGuildBagChange);
    }

    public void OnDonateEquipAddBagItemInfo(bag.BagItemInfo oldBag, bag.BagItemInfo newBag)
    {
        if (newBag != null)
        {
            _mbagItemInfos.Append(newBag);
        }

        if (oldBag != null)
        {
            int index = mbagItemInfos.FindIndex(p => p.id == oldBag.id);
            if (index != -1)
            {
                mbagItemInfos[index] = oldBag;
            }
            else
            {
                mbagItemInfos.Append(oldBag);
            }
        }
    }

    public void RemoveItemByCount(long uId, int count)
    {
        for (int i = 0; i < mbagItemInfos.Count; i++)
        {
            if (mbagItemInfos[i].id == uId)
            {
                if (mbagItemInfos[i].count > count)
                {
                    mbagItemInfos[i].count -= count;
                }
                else
                {
                    mbagItemInfos.RemoveAt(i);
                }
                break;
            }
        }

        HotManager.Instance.EventHandler.SendEvent(CEvent.OnGuildBagChange);
    }

    private Map<int, string> MapExchangeDiYuan;
    public Map<int, string> GetExchangeDiYuan()
    {
        return MapExchangeDiYuan;
    }

    /// <summary>
    /// 家族仓库可物品的道具
    /// </summary>
    private List<int> LisFamilyBagDonateItem = new List<int>();
    public List<int> mLisFamilyBagDonateItem
    {
        get
        {
            LisFamilyBagDonateItem.Clear();
            TABLE.SUNDRY tblSundry;
            if (SundryTableManager.Instance.TryGetValue(459, out tblSundry))
            {
                List<int> lis = UtilityMainMath.SplitStringToIntList(tblSundry.effect, '#');
                if (lis != null) LisFamilyBagDonateItem.AddRange(lis);
            }
            return LisFamilyBagDonateItem;
        }
    }

    public string GetOptionName(int id)
    {
        if(null == MapExchangeDiYuan)
        {
            MapExchangeDiYuan = new Map<int, string>
            {
                { -1, CSString.Format(904)},
                { 3, CSString.Format(905)},
                { 4, CSString.Format(906)},
                { 5, CSString.Format(907)},
                { 6, CSString.Format(908)},
                { 7, CSString.Format(909)},
                { 8, CSString.Format(910)},
                { 9, CSString.Format(911)},
            };
        }
        if (MapExchangeDiYuan.ContainsKey(id))
            return MapExchangeDiYuan[id];
        return MapExchangeDiYuan[-1];
    }

    public void RemoveItem(RepeatedField<long> list)
    {
        for (int i = 0; i < list.Count; ++i)
        {
            RemoveItem(list[i]);
        }
    }

    public bag.BagItemInfo GetItemBagInfo(long id)
    {
        for (int i = 0; i < mbagItemInfos.Count; i++)
        {
            var bagItemInfo = mbagItemInfos[i];
            if (null != bagItemInfo && bagItemInfo.id == id)
                return bagItemInfo;
        }
        return null;
    }

    void RemoveItem(long ID)
    {
        for (int i = 0; i < mbagItemInfos.Count; i++)
        {
            var bagItemInfo = mbagItemInfos[i];
            if (null == bagItemInfo)
                mbagItemInfos.RemoveAt(i--);

            if(bagItemInfo.id != ID)
            {
                continue;
            }

            mbagItemInfos.RemoveAt(i);
            UtilityTips.ShowTips(CSString.Format(972, bagItemInfo.QualityName()),1.5f,ColorType.White);
            break;
        }
    }
    #endregion

    //本行会拥有的元宝(通过工会战获得的元宝)
    public long OwnedYuanbao
    {
        get
        {
            if (!Utility.HasGuild())
                return 0;
            var mainInfo = GetGuildInfo();
            if (null == mainInfo)
                return 0;
            return mainInfo.yuanbao;
        }
    }

    #region 行会召集令
    int _guildCallTimer = -1;
    int guildCallTimer
    {
        get
        {
            if(_guildCallTimer < 0)
            {
                int sundryId = 1133;
                if (!SundryTableManager.Instance.TryGetValue(sundryId, out TABLE.SUNDRY sundryItem)
                    || !int.TryParse(sundryItem.effect, out _guildCallTimer))
                    _guildCallTimer = 10;
            }
            return _guildCallTimer;
        }
    }

    public void CallMap(string playerName,int mapId,int posx,int posy)
    {
        FNDebug.Log($"<color=#00ff00>行会召集令:[{playerName}]:[mapId:{mapId}]:[({posx},{posy})]</color>");
        int rid = 1024;
        CSSummonMgr.Instance.ShowSummon(CSString.Format(2041,playerName), (s, d) =>
        {
            if (s == (int)MsgBoxType.MBT_OK)
            {
                Fly2Goal(playerName,mapId, posx,posy);
                return;
            }
        }, SummonType.GuildCall, guildCallTimer, rid);
    }

    protected void Fly2Goal(string playerName,int mapId, int posx, int posy)
    {
        FNDebug.Log($"<color=#00ff00>行会召集令:[mapId:{mapId}]:[({posx},{posy})]</color>");
        Net.ReqUnionCallInfoMessage(playerName, mapId, posx, posy);
    }
    #endregion


    public override void Dispose()
    {

    }
}