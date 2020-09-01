using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TABLE;

public partial class RankAwardsTableManager : TableManager<TABLE.RANKAWARDSARRAY, TABLE.RANKAWARDS, int, RankAwardsTableManager>
{
    public void GetWorldBossReward(int type ,Dictionary<string, CSBetterLisHot<TABLE.RANKAWARDS>> dataDic)
    {
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.RANKAWARDS;
            if (item.activityId == type)
            {
                if (!dataDic.ContainsKey(item.awards))
                {
                    dataDic.Add(item.awards, new CSBetterLisHot<TABLE.RANKAWARDS>());
                }
                dataDic[item.awards].Add(item);
            }
        }
    }
    public void GetRankStep(int type, Dictionary<int, CSBetterLisHot<TABLE.RANKAWARDS>> dataDic)
    {
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.RANKAWARDS;
            if (item.activityId == type)
            {
                if (!dataDic.ContainsKey(item.box))
                {
                    dataDic.Add(item.box, new CSBetterLisHot<TABLE.RANKAWARDS>());
                }
                dataDic[item.box].Add(item);
            }
        }
    }

    /// <summary>
    /// 根据活动Id  获取 该类型对应所有奖励
    /// </summary>
    /// <param name="activityid"></param>
    /// <returns>string: 排名，  Map<int, int>： <道具id， 道具数量></returns>
    public void GetRankAwardTable(int activityid, Dictionary<string, Dictionary<int, int>> awardDic)
    {
        Dictionary<int, int> awardListDic = new Dictionary<int, int>(12);
        RANKAWARDS lastAward = null;
        int startRank = 0;
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var award = arr[k].Value as TABLE.RANKAWARDS;
            if (award.activityId == activityid)
            {
                if (award.min != 0 && award.max != 0 && (CSSealGradeInfo.Instance.MySealLevel< award.min ||
                                                         CSSealGradeInfo.Instance.MySealLevel > award.max)) continue;


                if (lastAward != null && (lastAward.awards == award.awards && lastAward.box == award.box))
                {
                    lastAward = award;
                    continue;
                }

                if (awardListDic.Count > 0)
                {
                    Dictionary<int, int> awardMap = new Dictionary<int, int>(awardListDic);
                    if (lastAward != null && lastAward.rank != startRank)
                        awardDic.Add(string.Format("{0}-{1}", startRank, lastAward.rank), awardMap);
                    else
                        awardDic.Add(startRank.ToString(), awardMap);
                }

                lastAward = award;
                startRank = award.rank;
                awardListDic.Clear();
                if (!string.IsNullOrEmpty(award.awards))
                {
                    List<List<int>> awardListl = UtilityMainMath.SplitStringToIntLists(award.awards);
                    for (var i = 0; i < awardListl.Count; i++)
                    {
                        if (awardListl[i].Count < 2) continue;
                        awardListDic.Add(awardListl[i][0], awardListl[i][1]);
                    }
                }

                if (award.box != 0)
                {
                    BoxTableManager.Instance.GetBoxAwardById(award.box, awardListDic);
                }

            }
        }
        if (awardListDic.Count > 0)
        {
            if (lastAward != null && lastAward.rank != startRank)
                awardDic.Add(string.Format("{0}-{1}",startRank,  lastAward.rank), awardListDic);
            else
                awardDic.Add(startRank.ToString(), awardListDic);
        }
    }

    public Dictionary<int, CSBetterLisHot<TABLE.RANKAWARDS>> GetRewardByType(int _type)
    {
        Dictionary<int, CSBetterLisHot<TABLE.RANKAWARDS>> temp_dic = new Dictionary<int, CSBetterLisHot<TABLE.RANKAWARDS>>();
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.RANKAWARDS;
            if (item.activityId == _type)
            {
                if (!temp_dic.ContainsKey(item.box))
                {
                    temp_dic.Add(item.box, new CSBetterLisHot<TABLE.RANKAWARDS>());
                }
                temp_dic[item.box].Add(item);
            }
        }
        return temp_dic;
    }
}
