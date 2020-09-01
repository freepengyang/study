using Google.Protobuf;
using Google.Protobuf.Collections;
using sabac;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TABLE;
using UnityEngine;

public class GuildFightActivity
{
    public int guildFightId;
    public long startTime;
    public long endTime;
    public int times;
    public long winGuildId;
    public string winGuildName;
    public long acquiredTime;
    public int totalConsume;
    public bool WillCome
    {
        get
        {
            return CSServerTime.Instance.TotalMillisecond < startTime;
        }
    }
    public bool IsEnd(bool main)
    {
        if (!main)
            return CSServerTime.Instance.TotalMillisecond > endTime;
        return CSGuildFightManager.Instance.Stage == (int)CSGuildFightManager.GuildFightStatus.GFS_ENDING;
    }
    public bool IsRunning(bool main)
    {
        long current = CSServerTime.Instance.TotalMillisecond;
        if (!main)
        {
            return startTime <= current && current <= endTime;
        }

        return CSGuildFightManager.Instance.Stage == (int)CSGuildFightManager.GuildFightStatus.GFS_RUNNING;
    }

    public string GetStartDay(bool main)
    {
        long current = CSServerTime.Instance.TotalMillisecond;

        if(!main)
        {
            if (startTime <= current)
                return CSString.Format(1037, 0);
            long delta = (startTime - current) / 1000;
            if (delta > 86400)
                return CSString.Format(1037, delta / 86400);
            if (delta > 3600)
                return CSString.Format(1038, delta / 3600, delta / 60 % 60);
            return CSString.Format(1039, delta / 60 % 60, delta % 60);
        }
        else
        {
            long delta = CSGuildFightManager.Instance.mTime;
            if (delta > 86400)
                return CSString.Format(1037, delta / 86400);
            if (delta > 3600)
                return CSString.Format(1038, delta / 3600 % 24, delta / 60 % 60);
            return CSString.Format(1039, delta / 60 % 60, delta % 60);
        }
    }
}

public class GuildFightRankInfo : IndexedItem
{
    public int Index { get; set; }
    public long guid;//角色ID
    public long unionId;//公会ID
    public int rank;
    public string guildName;
    public string playerName;
    public int score;
}

public class CSGuildFightManager : CSInfo<CSGuildFightManager>
{
    PoolHandleManager mPoolHandle = new PoolHandleManager();
    public void Initialize()
    {
        mFightItems = mPoolHandle.CreateGeneratePool<GuildFightItemData>();
        InitFightDescInfo();
        InitWeekDayName();
        InitTimes();
        QueryFightInfo();
        mPlayers = mPoolHandle.CreateGeneratePool<GuildFightPlayerInfo>(12);
        mPooledItems = mPoolHandle.CreateGeneratePool<GuildFightPlayerInfo>(8);
        mRankList = new FastArrayElementKeepHandle<GuildFightPlayerInfo>(8);
        InitRankList();
        mFights = mPoolHandle.CreateGeneratePool<GuildFightActivity>(8);
        Net.CSSabacStateMessage();
    }

    //请求行会活动信息
    public void QueryFightInfo()
    {
        Net.CSSabacDataInfoMessage();
    }

    //请求沙城战排行榜信息 取的是已经结束的信息
    public void RequestSabacLastRankInfo()
    {
        Net.CSSabacRankInfoMessage(mCurFightId, (int)RankUsage.RankUsageLastFight);
    }
    public void RequestSabacCurrentRankInfo()
    {
        Net.CSSabacRankInfoMessage(mCurFightId, (int)RankUsage.RankUsageCurrentFight);
    }

    // 历史战斗数据
    FastArrayElementFromPool<GuildFightActivity> mFights;
    // 本次活动 或 即将到来的 那次活动
    GuildFightActivity mLastFight;
    // 最后一次活动
    GuildFightActivity mCurrentFight;

    public GuildFightActivity CurrentFight
    {
        get
        {
            return mCurrentFight;
        }
    }

    public bool IsSabakeMember
    {
        get
        {
            return IsSabacMember(CSMainPlayerInfo.Instance.GuildId);
        }
    }

    public bool IsSabacOwner
    {
        get
        {
            if (!IsSabakeMember)
                return false;
            if (!Utility.HasGuild())
                return false;
            if (CSMainPlayerInfo.Instance.GuildPos != (int)GuildPos.President)
                return false;
            return true;
        }
    }

    public bool IsSabacMember(long guildId)
    {
        return guildId != 0 && guildId == SabacUnionId;
    }

    string _sabacUnionName = string.Empty;
    public string TakeUnionName
    {
        get
        {
            return _sabacUnionName;
        }
        set
        {
            _sabacUnionName = value;
            FNDebug.LogFormat("<color=#00ff00>[沙巴克公会名字]:[{0}]</color>", value);
        }
    }

    long _sabacUnintId = -1;
    public long SabacUnionId
    {
        get
        {
            return _sabacUnintId;
        }
        set
        {
            _sabacUnintId = value;
            FNDebug.LogFormat("<color=#00ff00>[沙巴克公会ID]:[{0}] => [主角公会ID]:[{1}]</color>",value,CSMainPlayerInfo.Instance.GuildId);
        }
    }
    public long mTime = 0;
    int mCurFightId;
    public void SetFightInfo(int curFightId, RepeatedField<OneSabacInfo> fights,long takenUnionId,string takenUnionName)
    {
        TakeUnionName = takenUnionName;
        SabacUnionId = takenUnionId;
        mCurFightId = curFightId;
        mCurrentFight = null;
        mLastFight = null;
        int lastFightIdx = -1;
        int maxPkId = ShaChengYuanBaoTableManager.Instance.array.gItem.id2offset.Count;

        mFights.Clear();
        for (int i = 0; i < fights.Count; ++i)
        {
            var sabacInfo = fights[i];
            var fight = mFights.Append();
            fight.startTime = sabacInfo.startTime;
            fight.endTime = sabacInfo.endTime;
            fight.times = sabacInfo.pkId;
            fight.guildFightId = Mathf.Clamp(sabacInfo.pkId,1,maxPkId);
            fight.winGuildId = sabacInfo.unionId;
            fight.winGuildName = sabacInfo.unionName;
            fight.totalConsume = sabacInfo.totalConsume;
            if (curFightId == fight.times)
            {
                if (fight.IsEnd(false) && i != fights.Count - 1)
                {
                    curFightId += 1;
                    continue;
                }

                mCurrentFight = fight;
                if (i > 0)
                {
                    if (mCurrentFight.winGuildId == 0)
                        lastFightIdx = i - 1;
                    else
                        lastFightIdx = i;
                }
                else
                    lastFightIdx = i;
            }
        }

        if(mFights.Count > 0 && null == mCurrentFight)
        {
            mCurrentFight = mFights[0];
        }

        if (lastFightIdx != -1)
            mLastFight = mFights[lastFightIdx];

        if (mFights.Count > 4)
            mFights.RemoveAt(0);

        //信息刷新重新请求排上榜信息
        RequestSabacLastRankInfo();
        RequestSabacCurrentRankInfo();
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnGuildFightDataChanged);
    }

    public int ExtraPercentValue
    {
        get
        {
            if(null == mCurrentFight && null == mLastFight)
            {
                int id = 1;
                TABLE.SHACHENGYUANBAO item;
                if (ShaChengYuanBaoTableManager.Instance.TryGetValue(id, out item))
                    return item.transform;
            }

            if(null != mCurrentFight)
            {
                TABLE.SHACHENGYUANBAO item;
                if (ShaChengYuanBaoTableManager.Instance.TryGetValue(mCurrentFight.guildFightId, out item))
                    return item.transform;
            }

            return 0;
        }
    }

    void UpdateStatus()
    {
        var current = CSServerTime.Instance.TotalMillisecond;
        if (mCurrentFight == null)
        {
            //活动没有
            Stage = (int)GuildFightStatus.GFS_CLOSED;
            mTime = 0;
        }
        else
        {
            if (mLastFight == null)
            {
                //如果一次攻沙战争都没有
                if (mCurrentFight.startTime > current)
                {
                    long delta = mCurrentFight.startTime - current;
                    delta /= 1000;//转换程秒数量
                    mTime = delta;
                    //第一次永远展示即将开启
                    //即将开启 [注意这里是 本次 攻沙的已经结束]
                    Stage = (int)GuildFightStatus.GFS_WILL_OPEN;
                }
                else if (mCurrentFight.endTime < current)
                {
                    //已经结束
                    Stage = (int)GuildFightStatus.GFS_ENDING;
                }
                else
                {
                    long delta = mCurrentFight.endTime - current;
                    delta /= 1000;//转换程秒数量
                    mTime = delta;
                    //进行中
                    Stage = (int)GuildFightStatus.GFS_RUNNING;
                }
            }
            else
            {
                //如果已经有一次攻沙战争了
                if (mCurrentFight.startTime > current)
                {
                    long delta = mCurrentFight.startTime - current;
                    delta /= 1000;//转换程秒数量
                    mTime = delta;

                    //即将开启
                    Stage = (int)GuildFightStatus.GFS_WILL_OPEN;
                    ////小于3天展示即将开启
                    //if (delta <= 86400 * 3)
                    //{
                    //    //即将开启
                    //    Stage = (int)GuildFightStatus.GFS_WILL_OPEN;
                    //}
                    //else
                    //{
                    //    //已经结束 [注意这里是 上次 攻沙的已经结束]
                    //    Stage = (int)GuildFightStatus.GFS_ENDING;
                    //}
                }
                else if (mCurrentFight.endTime < current)
                {
                    //已经结束 [注意这里是 本次 攻沙的已经结束]
                    Stage = (int)GuildFightStatus.GFS_ENDING;
                }
                else
                {
                    long delta = mCurrentFight.endTime - current;
                    delta /= 1000;//转换程秒数量
                    mTime = delta;
                    //进行中
                    Stage = (int)GuildFightStatus.GFS_RUNNING;
                }
            }
        }
    }

    public int ShowMode { get; set; }
    FastArrayElementFromPool<GuildFightItemData> mFightItems;
    public FastArrayElementFromPool<GuildFightItemData> GetFightItemDatas()
    {
        ShowMode = 0;
        //if(mFights.Count > 0 && mFights[mFights.Count - 1].times > 4)
        //{
        //    ShowMode = 1;
        //}

        mFightItems.Clear();

        if(ShowMode == 0)
        {
            for (int i = 0; i < mFights.Count; ++i)
            {
                var curFight = mFights[i];
                SHACHENGYUANBAO item = null;
                if (!ShaChengYuanBaoTableManager.Instance.TryGetValue(curFight.guildFightId, out item))
                {
                    continue;
                }
                var fightItem = mFightItems.Append();
                fightItem.item = item;
                fightItem.normal.value = item.basic;
                fightItem.extra.value = curFight.totalConsume;
                fightItem.fightActivity = curFight;
            }
        }
        else
        {
            for (int i = 2; i < mFights.Count; ++i)
            {
                var curFight = mFights[i];
                SHACHENGYUANBAO item = null;
                if (!ShaChengYuanBaoTableManager.Instance.TryGetValue(curFight.guildFightId, out item))
                {
                    continue;
                }
                var fightItem = mFightItems.Append();
                fightItem.item = item;
                fightItem.normal.value = item.basic;
                fightItem.extra.value = curFight.totalConsume;
                fightItem.fightActivity = curFight;
            }
        }

        return mFightItems;
    }

    public string GetCityStageDesc()
    {
        UpdateStatus();
        if (Stage >= 0 && Stage < mFightStageDesc.Length)
            return string.Format(mFightStageDesc[stage], CSServerTime.Instance.FormatLongToTimeStr(mTime, 11));
        return string.Empty;
    }

    public string GetStageDesc()
    {
        UpdateStatus();
        if (Stage >= 0 && Stage < stageTipsId.Length)
            return CSString.Format(stageTipsId[stage], CSServerTime.Instance.FormatLongToTimeStr(mTime,17));
        return CSString.Format(1040);
    }

    public string GetWeekName(System.DayOfWeek week)
    {
        int idx = (int)week;
        if (idx >= 0 && idx < mWeekName.Length)
            return mWeekName[idx];
        return string.Empty;
    }

    public string GetTimes(int v)
    {
        if (v >= 1 && v <= mTimesDesc.Length)
            return mTimesDesc[v - 1];
        return string.Empty;
    }

    public void OpenFightPanel()
    {
        UIManager.Instance.CreatePanel<UIGuildFightCombinedPanel>(f =>
        {
            (f as UIGuildFightCombinedPanel).OpenChildPanel((int)UIGuildFightCombinedPanel.ChildPanelType.CPT_FIGHT);
        });
    }

    #region table_info
    string[] mFightStageDesc = new string[0];
    void InitFightDescInfo(int sundryId = 571)
    {
        TABLE.SUNDRY sundryItem;
        if (!SundryTableManager.Instance.TryGetValue(sundryId, out sundryItem))
            return;

        mFightStageDesc = sundryItem.effect.Split('#');
    }
    string[] mWeekName = new string[0];
    void InitWeekDayName(int sundryId = 574)
    {
        TABLE.SUNDRY sundryItem;
        if (!SundryTableManager.Instance.TryGetValue(sundryId, out sundryItem))
            return;

        mWeekName = sundryItem.effect.Split('#');
    }
    string[] mTimesDesc = new string[0];
    void InitTimes(int sundryId = 576)
    {
        TABLE.SUNDRY sundryItem;
        if (!SundryTableManager.Instance.TryGetValue(sundryId, out sundryItem))
            return;

        mTimesDesc = sundryItem.effect.Split('#');
    }
    #endregion

    #region data drived
    public enum GuildFightStatus
    {
        GFS_CLOSED = -1,//关闭状态
        GFS_WILL_OPEN = 0,//即将开放
        GFS_RUNNING = 1,//进行中
        GFS_ENDING = 2,//已经结束
    }
    int[] stageTipsId = new int[] {1032,1031,1033};
    int stage = -1;
    public int Stage
    {
        get
        {
            return stage;
        }
        set
        {
            if(stage != value)
            {
                stage = value;
                HotManager.Instance.EventHandler.SendEvent(CEvent.OnSabacStageChanged);
            }
        }
    }
    public bool IsActivityOpened
    {
        get
        {
            if (null != SabacState)
                return SabacState.state == 1;
            if (null == mCurrentFight)
                return false;
            if(!mCurrentFight.IsRunning(true))
            {
                return false;
            }
            return true;
        }
    }

    public bool IsDoorDie
    {
        get
        {
            if (null != SabacState && SabacState.doorIsDie)
                return true;
            return false;
        }
    }
    #endregion

    #region guild fight list
    FastArrayElementFromPool<GuildFightPlayerInfo> mPlayers;
    void SetFightPlayers(RepeatedField<sabac.RankInfo> ranks,RepeatedField<PlayerModelInfo> models)
    {
        mPlayers.Clear();
        for(int i = 0; i < ranks.Count; ++i)
        {
            var rank = ranks[i];
            var player = mPlayers.Append();
            player.guid = rank.roleId;
            player.name = rank.name;
            player.guildName = rank.unionName;
            player.rank = rank.rank;
            player.model = i < models.Count ? models[i] : null;
        }
        mPlayers.Sort(FightListComparer);
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnGuildFightRankListChanged);
    }

    int FightListComparer(GuildFightPlayerInfo l, GuildFightPlayerInfo r)
    {
        return l.rank - r.rank;
    }

    public FastArrayElementFromPool<GuildFightPlayerInfo> FightList
    {
        get
        {
            return mPlayers;
        }
    }

    FastArrayElementFromPool<GuildFightPlayerInfo> mPooledItems;
    FastArrayElementKeepHandle<GuildFightPlayerInfo> mRankList;
    public FastArrayElementKeepHandle<GuildFightPlayerInfo> RankList
    {
        get
        {
            mRankList.Clear();
            mPooledItems.Clear();
            for (int i = 0; i < mPlayers.Count && i < 10; ++i)
            {
                mRankList.Append(mPlayers[i]);
            }

            for (int i = Mathf.Min(mRankList.Count,10); i < 10; ++i)
            {
                var data = mPooledItems.Append();
                data.rank = i + 1;
                data.name = string.Empty;
                data.guildName = string.Empty;
                data.guid = 0;
                mRankList.Append(data);
            }
            return mRankList;
        }
    }
    #endregion

    #region 公会争霸积分排行
    public string WinGuildName
    {
        get
        {
            return TakeUnionName;
            //return null == mCurrentFight ? string.Empty : mCurrentFight.winGuildName;
        }
    }
    int _rank = 0;
    public int MyRank
    {
        get
        {
            return _rank;
        }
        private set
        {
            _rank = value;
        }
    }
    int _score = 0;
    public int MyScore
    {
        get
        {
            return _score;
        }
        private set
        {
            _score = value;
        }
    }

    public void InitRankList()
    {
        mScoreRankList = mPoolHandle.CreateGeneratePool<GuildFightRankInfo>();
    }
    FastArrayElementFromPool<GuildFightRankInfo> mScoreRankList;
    public FastArrayElementFromPool<GuildFightRankInfo> ScoreRankList
    {
        get
        {
            return mScoreRankList;
        }
    }

    public enum RankUsage
    {
        RankUsageLastFight = 1,//已经结束的沙城争霸排行榜
        RankUsageCurrentFight = 2,//当前正在进行的
    }
    public void SetGuildFightList(RepeatedField<sabac.RankInfo> rankInfo,int _myscore = 0, int _myrank = 11)
    {
        mScoreRankList.Clear();
        MyRank = _myrank;
        MyScore = _myscore;
        for (int i = 0; i < rankInfo.Count; ++i)
        {
            var rank = rankInfo[i];
            var append = mScoreRankList.Append();
            append.guid = rank.roleId;
            append.unionId = rank.unionId;
            append.rank = rank.rank;
            append.guildName = rank.unionName;
            append.playerName = rank.name;
            append.score = rank.data;
            if (rank.roleId == CSMainPlayerInfo.Instance.ID)
                MyRank = rank.rank;
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnGuildFightScoreListChanged);
    }
    #endregion

    public void SetRankInfos(int pkId, RepeatedField<sabac.RankInfo> rankInfo, int usage, int _myscore, int _myrank,RepeatedField<PlayerModelInfo> models)
    {
        if (usage == (int)RankUsage.RankUsageCurrentFight)
        {
            SetGuildFightList(rankInfo, _myscore, _myrank);
        }
        else if(usage == (int)RankUsage.RankUsageLastFight)
        {
            SetFightPlayers(rankInfo,models);
        }
    }

    #region 沙城争霸状态更新
    public int GetStage()
    {
        if(null != _SabacState && _SabacState.doorIsDie)
        {
            return 2;
        }
        return 1;
    }
    /// <summary>
    /// 返回0-100之间的值
    /// </summary>
    /// <returns></returns>
    public int GetDoorBloodPercentValue()
    {
        if(null == _SabacState || null == _SabacState.doorInfo || _SabacState.state != 1)
        {
            return 0;
        }

        int prev = _SabacState.doorInfo.hp;
        int max = _SabacState.doorInfo.maxHp;
        if (max <= 0)
            return 0;

        int percentValue = Mathf.FloorToInt(Mathf.Clamp01(prev * 1.0f / max) * 100 + 0.5f);
        percentValue = Mathf.Clamp(percentValue, 0, 100);
        return percentValue;
    }

    void TryAddBulletMessage(sabac.SabacStateResponse prev, sabac.SabacStateResponse value)
    {
        if (null != prev && null != value && prev.doorIsDie != value.doorIsDie && value.doorIsDie)
        {
            var bulletinResponse = new tip.BulletinResponse();
            bulletinResponse.count = 1;
            bulletinResponse.display = (int)NoticeType.CenterTop;
            bulletinResponse.msg = CSString.Format(IsSabakeMember ? 1302 : 1301);
            bulletinResponse.bulletinId = 0;
            CSNoticeManager.Instance.ResBulletinMessage(bulletinResponse);
        }
    }

    sabac.SabacStateResponse _SabacState;
    public sabac.SabacStateResponse SabacState
    {
        get
        {
            return _SabacState;
        }
        set
        {
            var prev = _SabacState;
            _SabacState = value;
            if (null != _SabacState)
            {
                TakeUnionName = _SabacState.takenUnionName;
                SabacUnionId = _SabacState.takenUnionId;
            }

            //尝试加入城门攻破公告
            TryAddBulletMessage(prev, value);

            //重新请求行会元宝
            if(Utility.HasGuild())
            {
                Net.CSGetUnionTabMessage((int)UnionTab.MainInfo);
            }
            //状态变更请求沙巴克信息
            //QueryFightInfo();

            if(null != _SabacState && _SabacState.state == 2)
            {
                ////不为空则弹出结束界面
                //if(!string.IsNullOrEmpty(_SabacState.takenUnionName))
                //{
                //    var win = Utility.HasGuild() && _SabacState.takenUnionId == CSMainPlayerInfo.Instance.GuildId;
                //    UIManager.Instance.CreatePanel<UIGuildFightFinalAwardPanel>(
                //        f =>
                //        {
                //            (f as UIGuildFightFinalAwardPanel).Show(_SabacState.takenUnionName, string.Empty, win);
                //        });
                //}
            }
            else if(null != _SabacState && _SabacState.state == 1)
            {
                TriggerGuildFightMessageBox();
            }
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnGuildFightStateChanged);
            CSMainFuncManager.Instance.Init(-1);
        }
    }

    public void TryOpenSabacFightResultPanel(string value)
    {
        if (null != _SabacState)
        {
            //不为空则弹出结束界面
            if (!string.IsNullOrEmpty(_SabacState.takenUnionName))
            {
                var win = Utility.HasGuild() && _SabacState.takenUnionId == CSMainPlayerInfo.Instance.GuildId;
                UIManager.Instance.CreatePanel<UIGuildFightFinalAwardPanel>(
                    f =>
                    {
                        (f as UIGuildFightFinalAwardPanel).Show(_SabacState.takenUnionName, value, win);
                    });
            }
        }
    }
    #endregion

    bool mNeedHint = true;
    int sabacMapId = 301301;//盟重
    int sabacPalaceId = 301302;//沙巴克皇宫
    public bool IsInActivityMap
    {
        get
        {
            int mapId = CSScene.GetMapID();
            return mapId == sabacMapId || mapId == sabacPalaceId;
        }
    }
    public int SabacMapId
    {
        get
        {
            return sabacMapId;
        }
    }
    public int SabacPalaceId
    {
        get
        {
            return sabacPalaceId;
        }
    }
    public bool IsInSabacMap
    {
        get
        {
            int mapId = CSScene.GetMapID();
            return mapId == sabacMapId;
        }
    }
    public void TriggerGuildFightMessageBox()
    {
        if (!mNeedHint)
            return;

        if (!Utility.HasGuild())
        {
            return;
        }

        if(IsInActivityMap)
        {
            return;
        }

        if (!(null != _SabacState && _SabacState.state == 1 || null != mCurrentFight && mCurrentFight.IsRunning(true)))
        {
            return;
        }
        
        int rid = 1024;
        CSSummonMgr.Instance.ShowSummon(CSString.Format(1047), (s, d) =>
        {
            if (s == (int)MsgBoxType.MBT_OK)
            {
                DeliverToSabacFightZone();
            }
            else if (s == (int)MsgBoxType.MBT_CANCEL)
            {
                mNeedHint = false;
            }
        }, SummonType.Sabac, 10, rid);

        //UIManager.Instance.CreatePanel("UISummonPanel", UIManager.Instance.GetRoot(), (f) =>
        //{
        //    //CsUseCallItem.Instance.AddPanel(rid, f as UISummonPanel);
        //    (f as UISummonPanel).RefreshUI(CSString.Format(1047), (s, d) =>
        //    {
        //        if (s == (int)MsgBoxType.MBT_OK)
        //        {
        //            DeliverToSabacFightZone();
        //        }
        //        else if (s == (int)MsgBoxType.MBT_CANCEL)
        //        {
        //            mNeedHint = false;
        //        }
        //    }, 10, rid);
        //});
    }

    public void DeliverToSabacFightZone()
    {
        int deliverId = 110;
        //进攻方ID
        if (SabacUnionId == 0 || SabacUnionId != CSMainPlayerInfo.Instance.GuildId)
        {
            //城门已经坏了 第二阶段
            if(null != SabacState && SabacState.doorIsDie)
            {
                deliverId = 109;
            }
            else
            {
                deliverId = 108;
            }
        }
        else
        {
            deliverId = 110;
        }
        UtilityPath.FindWithDeliverId(deliverId);
    }

    public override void Dispose()
    {
        mRankList?.Clear();
        mRankList = null;
        mPooledItems?.Clear();
        mPooledItems = null;
        mPlayers?.Clear();
        mPlayers = null;
        mFightItems?.Clear();
        mFightItems = null;
        mPoolHandle?.OnDestroy();
        mPoolHandle = null;
    }
}
