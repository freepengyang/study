using System.Collections.Generic;
using rankalonetable;

/// <summary>
/// 排行榜主页签
/// </summary>
public enum RankingType
{
    Grade=101, //等级
    FightingPower, //战力
    Wing, //翅膀
    Guild, //公会
}

/// <summary>
/// 排行榜子页签
/// </summary>
public enum RankingSubType
{
    All, //全服
    Zhan, //战士
    Fa, //法师
    Dao, //道士
}

public class CSRankingInfo : CSInfo<CSRankingInfo>
{
    public CSRankingInfo()
    {
    }

    public override void Dispose()
    {

    }

    /// <summary>
    /// 当前所有已有数据的排行榜信息
    /// </summary>
    private Map<string, RankingData> mapRankingInfo = new Map<string, RankingData>();

    public Map<string, RankingData> MapRankingInfo => mapRankingInfo;

    /// <summary>
    /// 所有大页签对应顺序
    /// </summary>
    private List<RankingType> rankingTypes = new List<RankingType>
    {
        RankingType.Grade,
        RankingType.FightingPower,
        RankingType.Wing,
        RankingType.Guild,
    };

    public List<RankingType> RankingTypes => rankingTypes;

    /// <summary>
    /// 所有小页签对应顺序
    /// </summary>
    Map<RankingType, List<RankingSubType>> mapRankingSubTypes = new Map<RankingType, List<RankingSubType>>
    {
        [RankingType.Grade] = new List<RankingSubType>
        {
            RankingSubType.All,
            RankingSubType.Zhan,
            RankingSubType.Fa,
            RankingSubType.Dao,
        },

        [RankingType.FightingPower] = new List<RankingSubType>
        {
            RankingSubType.All,
            RankingSubType.Zhan,
            RankingSubType.Fa,
            RankingSubType.Dao,
        },

        [RankingType.Wing] = new List<RankingSubType>
        {
            RankingSubType.All,
        },

        [RankingType.Guild] = new List<RankingSubType>
        {
            RankingSubType.All,
        },
    };

    public Map<RankingType, List<RankingSubType>> MapRankingSubTypes => mapRankingSubTypes;

    /// <summary>
    /// 上一次请求排行榜信息的时间(整个排行榜不共用,单位:秒)
    /// </summary>
    private Dictionary<RankingType, long> dicLastReqRankingInfoTime = new Dictionary<RankingType, long>();

    public Dictionary<RankingType, long> DicLastReqRankingInfoTime => dicLastReqRankingInfoTime;


    #region 网络响应处理函数

    /// <summary>
    /// 处理非公会排行榜列表数据
    /// </summary>
    /// <param name="msg"></param>
    public void HandleSCRoleRankInfoMessage(RoleRankInfoResponse msg)
    {
        if (msg == null) return;
        RankingData rankingData = new RankingData();
        rankingData.RoleRankInfo = msg;

        string id = $"{msg.type}{msg.subType}";
        if (!mapRankingInfo.ContainsKey(id))
        {
            mapRankingInfo.Add(id, rankingData);
        }
        else
        {
            mapRankingInfo[id] = rankingData;
        }
    }

    /// <summary>
    /// 处理公会排行榜列表信息
    /// </summary>
    /// <param name="msg"></param>
    public void HandleSCUnionRankInfoMessage(UnionRankInfoResponse msg)
    {
        if (msg == null) return;
        RankingData rankingData = new RankingData();
        rankingData.UnionRankInfo = msg;

        string id = $"{(int) RankingType.Guild}{(int) RankingSubType.All}";
        if (!mapRankingInfo.ContainsKey(id))
        {
            mapRankingInfo.Add(id, rankingData);
        }
        else
        {
            mapRankingInfo[id] = rankingData;
        }
    }

    #endregion
}

/// <summary>
/// 排行榜数据结构
/// </summary>
public class RankingData
{
    /// <summary>
    /// 等级,战力,翅膀排行信息
    /// </summary>
    public RoleRankInfoResponse RoleRankInfo { get; set; }

    /// <summary>
    /// 公会排行信息
    /// </summary>
    public UnionRankInfoResponse UnionRankInfo { get; set; }
}