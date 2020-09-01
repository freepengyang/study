using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using unionbattle;


public enum GuildActivityType
{
    COMBAT = 1,//行会战
    BOSS = 2,//行会首领
}


public class CSGuildActivityInfo : CSInfo<CSGuildActivityInfo>
{
    const int combatInstanceId = 300005;

    public string openTimeLeftStr;
    public string closeTimeLeftStr;

    TABLE.ACTIVE combatActCfg;
    TABLE.ACTIVE bossActCfg;

    TABLE.TIMER guildCombatTimer;
    public TABLE.TIMER GuildCombatTimer
    {
        get => guildCombatTimer;
    }
    TABLE.TIMER guildBossTimer;
    public TABLE.TIMER GuildBossTimer
    {
        get => guildBossTimer;
    }

    /// <summary>
    /// 行会战奖励预览
    /// </summary>
    Dictionary<int, Dictionary<int, int>> guildCombatRewards;
    /// <summary>
    /// 行会首领奖励预览
    /// </summary>
    Dictionary<int, int> guildBossRewards;


    rank.RankInfo lastCombatRank;
    public rank.RankInfo LastCombatRank
    {
        get => lastCombatRank;
    }

    /// <summary> 首领副本id </summary>
    Dictionary<int, int> bossInstanceIds = new Dictionary<int, int>();


    Dictionary<int, UnionActivityInfo> allActivityState;

    public override void Dispose()
    {
        mClientEvent.RemoveEvent(CEvent.FunctionOpenStateChange, FuncOpenCheckActivites);
        //mClientEvent.RemoveEvent(CEvent.GetEnterInstanceInfo, CheckBubble);
        //mClientEvent.RemoveEvent(CEvent.LeaveInstance, CheckBubble);
        guildCombatRewards?.Clear();
        guildCombatRewards = null;
        bossInstanceIds?.Clear();
        bossInstanceIds = null;
    }


    public void Initialize()
    {
        Init();
        Net.CSUnionActivityInfoMessage();
    }

    public void Init()
    {
        openTimeLeftStr = ClientTipsTableManager.Instance.GetClientTipsContext(1928);
        closeTimeLeftStr = ClientTipsTableManager.Instance.GetClientTipsContext(1929);

        if (ActiveTableManager.Instance.TryGetValue(42, out combatActCfg))
        {
            TimerTableManager.Instance.TryGetValue(combatActCfg.time, out guildCombatTimer);
        }
        if (ActiveTableManager.Instance.TryGetValue(43, out bossActCfg))
        {
            TimerTableManager.Instance.TryGetValue(bossActCfg.time, out guildBossTimer);
        }        
        

        //行会战奖励
        if (guildCombatRewards == null) guildCombatRewards = new Dictionary<int, Dictionary<int, int>>();
        var arr = RankAwardsTableManager.Instance.array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var cfg = arr[i].Value as TABLE.RANKAWARDS;
            if (cfg.activityId != (int)RankType.GUILD_BATTLE) continue;
            Dictionary<int, int> awards = null;
            if (!guildCombatRewards.TryGetValue(cfg.rank, out awards))
            {
                awards = new Dictionary<int, int>();
                guildCombatRewards.Add(cfg.rank, awards);
            }
            else awards.Clear();

            var awardsStrAtt = cfg.awards.Split('&');
            for (int s = 0; s < awardsStrAtt.Length; s++)
            {
                var str = awardsStrAtt[s].Split('#');
                if (str.Length > 1)
                {
                    int id = 0;
                    int num = 0;
                    int.TryParse(str[0], out id);
                    int.TryParse(str[1], out num);
                    awards.Add(id, num);
                }
            }
        }

        //首领奖励预览
        if (guildBossRewards == null) guildBossRewards = new Dictionary<int, int>();
        else guildBossRewards.Clear();
        var bossAwardsStr = ActiveTableManager.Instance.GetActiveAwards(43);
        var bossAwards = bossAwardsStr.Split('&');
        for (int s = 0; s < bossAwards.Length; s++)
        {
            var str = bossAwards[s].Split('#');
            if (str.Length > 1)
            {
                int id = 0;
                int num = 0;
                int.TryParse(str[0], out id);
                int.TryParse(str[1], out num);
                guildBossRewards.Add(id, num);
            }
        }

        //首领副本id
        if (bossInstanceIds == null) bossInstanceIds = new Dictionary<int, int>();
        else bossInstanceIds.Clear();
        arr = InstanceTableManager.Instance.array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var cfg = arr[i].Value as TABLE.INSTANCE;
            if (cfg.type != (int)ECopyType.GuildBoss) continue;
            bossInstanceIds[cfg.level] = cfg.id;
        }


        mClientEvent.AddEvent(CEvent.FunctionOpenStateChange, FuncOpenCheckActivites);
        //mClientEvent.AddEvent(CEvent.GetEnterInstanceInfo, CheckBubble);
        //mClientEvent.AddEvent(CEvent.LeaveInstance, CheckBubble);
    }




    /// <summary>
    /// 上次公会战排行
    /// </summary>
    /// <param name="msg"></param>
    public void SC_LastUnionRankMessage(rank.RankInfo msg)
    {
        if (msg == null) return;
        lastCombatRank = msg;
        mClientEvent.SendEvent(CEvent.GuildActivityStateChange);
    }

    /// <summary>
    /// 所有活动时间状态信息
    /// </summary>
    /// <param name="msg"></param>
    public void SC_AllActivityInfo(unionbattle.UnionActivityInfoResponse msg)
    {
        if (msg == null) return;
        if (allActivityState == null) allActivityState = new Dictionary<int, UnionActivityInfo>();
        for (int i = 0; i < msg.unionActivityInfos.Count; i++)
        {
            var activity = msg.unionActivityInfos[i];
            allActivityState[activity.activityId] = activity;
            if (activity.state == 1) TryToPopSummon(activity.activityId);
        }
        mClientEvent.SendEvent(CEvent.GuildActivityStateChange);
    }

    /// <summary>
    /// 单活动状态变更
    /// </summary>
    /// <param name="msg"></param>
    public void SC_ActivityStateChange(unionbattle.UnionActivityInfo msg)
    {
        if (msg == null) return;
        if (allActivityState == null) allActivityState = new Dictionary<int, UnionActivityInfo>();
        allActivityState[msg.activityId] = msg;
        if (msg.state == 1) TryToPopSummon(msg.activityId);
        else 
            RemoveBubble(msg.activityId);
        mClientEvent.SendEvent(CEvent.GuildActivityStateChange);
    }


    public void SC_CombatFinish(unionbattle.UnionActivityReward msg)
    {
        if (msg == null || guildCombatRewards == null) return;
        int rank = msg.rank/* + 1*/;//后端表示该排名是从1开始
        int rewardRank = rank >= 1 && rank <= 3 ? rank : 0;
        Dictionary<int, int> dic = null;
        if (guildCombatRewards.TryGetValue(rewardRank, out dic))
        {
            UIManager.Instance.CreatePanel<UIGuildEndActivityPanel>((f) =>
            {
                (f as UIGuildEndActivityPanel).OpenPanel(dic, rank, msg.score);
            });
        }
    }


    void TryToPopSummon(int actId)
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funP_guild))
        {
            return;
        }

        if (IsOnTheInstance(actId)) return;

        string desc = "";
        if (actId == (int)GuildActivityType.COMBAT)
        {
            if (!Utility.HasGuild()) return;
            desc = CSString.Format(1632, combatActCfg.name);
            CSSummonMgr.Instance.ShowSummon(desc, (s, d) =>
            {
                if (s == (int)MsgBoxType.MBT_OK)
                {
                    UtilityPanel.JumpToPanel(11807);
                }
            }, SummonType.GuildCombat, 8, combatActCfg.id);
        }
        else if (actId == (int)GuildActivityType.BOSS)
        {
            desc = CSString.Format(1632, bossActCfg.name);
            CSSummonMgr.Instance.ShowSummon(desc, (s, d) =>
            {
                if (s == (int)MsgBoxType.MBT_OK)
                {
                    //if (Utility.HasGuild())
                    //    TryToJoinBoss();
                    //else
                    //    UtilityPanel.JumpToPanel(11803);
                    UtilityPanel.JumpToPanel(11808);
                }
            }, SummonType.GuildCombat, 8, bossActCfg.id);
        }

        ShowBubble(actId);
    }


    void ShowBubble(int actId)
    {
        return;
        if (actId == (int)GuildActivityType.COMBAT)
        {
            CSActivityRemindInfo.Instance.AddSpecialBubble(combatActCfg.id);
        }
        else if(actId == (int)GuildActivityType.BOSS)
        {
            CSActivityRemindInfo.Instance.AddSpecialBubble(bossActCfg.id);
        }
    }


    void RemoveBubble(int actId)
    {
        return;
        if (actId == (int)GuildActivityType.COMBAT)
        {
            CSActivityRemindInfo.Instance.RemoveSpecialBubble(combatActCfg.id);
        }
        else if (actId == (int)GuildActivityType.BOSS)
        {
            CSActivityRemindInfo.Instance.RemoveSpecialBubble(bossActCfg.id);
        }
    }


    void FuncOpenCheckActivites(uint id, object data)
    {
        if (allActivityState == null) return;
        int funcId = System.Convert.ToInt32(data);
        if (funcId != (int)FunctionType.funP_guild) return;
        for (var it = allActivityState.GetEnumerator(); it.MoveNext();)
        {
            var info = it.Current.Value;
            if (info.state == 1) TryToPopSummon(info.activityId);
        }
    }



    public Dictionary<int, Dictionary<int, int>> GetCombatRewardsDic()
    {
        return guildCombatRewards;
    }

    public Dictionary<int, int> GetBossRewardsDic()
    {
        return guildBossRewards;
    }



    public UnionActivityInfo GetActivityInfo(int id)
    {
        if (allActivityState == null || !allActivityState.ContainsKey(id)) return null;

        return allActivityState[id];
    }



    public void TryToJoinCombat()
    {
        if (!Utility.HasGuild()) return;
        if (IsOnTheInstance((int)GuildActivityType.COMBAT))
        {
            UtilityTips.ShowTips(1964);
            return;
        }

        Net.ReqEnterInstanceMessage(combatInstanceId);
    }


    public void TryToJoinBoss()
    {
        if (!Utility.HasGuild()) return;
        if (IsOnTheInstance((int)GuildActivityType.BOSS))
        {
            UtilityTips.ShowTips(1964);
            return;
        }

        Net.ReqEnterInstanceMessage(bossInstanceIds[1]);
    }


    public bool IsOnTheInstance(int actId)
    {
        var info = CSInstanceInfo.Instance.GetInstanceInfo();
        if (info == null) return false;

        if (actId == (int)GuildActivityType.COMBAT)
        {
            return info.instanceId == combatInstanceId;
        }
        else if (actId == (int)GuildActivityType.BOSS)
        {
            if (bossInstanceIds == null || !bossInstanceIds.ContainsKey(1)) return false;
            return bossInstanceIds.ContainsValue(info.instanceId);
        }

        return false;
    }


    public bool IsActivityOpen(int actId)
    {
        if (allActivityState == null || !allActivityState.ContainsKey(actId)) return false;
        return allActivityState[actId].state == 1;
    }


    void CheckBubble(uint id, object param)
    {
        if (allActivityState == null || !UICheckManager.Instance.DoCheckFunction(FunctionType.funP_guild)) return;

        for (var it = allActivityState.GetEnumerator(); it.MoveNext();)
        {
            var act = it.Current.Value;
            if (act.state == 1)
            {
                bool inTheIns = IsOnTheInstance(act.activityId);
                if (inTheIns) RemoveBubble(act.activityId);
                else ShowBubble(act.activityId);
            }
        }
    }

}
