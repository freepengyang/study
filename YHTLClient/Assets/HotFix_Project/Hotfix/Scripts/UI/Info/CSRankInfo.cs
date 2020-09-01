using rank;

public enum RankType {
    DEFAULT = 0, //排行榜
    WORLD_BOSS = 1,  //世界boss
    ULTIMATE_CHALLENGE = 2, //极限挑战
    SABAC_SCORE = 3, //sabac积分
    BOSS_CHALLENGE_NEW_PLAYER = 5, //boss挑战新手组
    BOSS_CHALLENGE_ADVANCED = 6, //boss挑战进阶组
    BOSS_CHALLENGE_MASTER = 7, //boss挑战大师组
    GUILD_BATTLE = 12,//公会战
    EQUIT_SCORE = 101, //装备评分
    SealGrade = 102,//等级封印
}

public class CSRankInfo : CSInfo<CSRankInfo>
{
    
    #region 排行榜信息

    //rank.RankInfo rankinfo;

    Map<int, RankInfo> ranks = new Map<int, RankInfo>();

    public void RankInfoChange(RankInfo rankInfo) {

        if (!ranks.ContainsKey(rankInfo.type))
        {
            ranks.Add(rankInfo.type, rankInfo);
        }
        else
        {
            ranks[rankInfo.type] = rankInfo;
        }
        //Debug.Log("rankInfo.type" + rankInfo.type);
        mClientEvent.SendEvent(CEvent.RankInfoChange, rankInfo);
    }

    public RankInfo GetRankByType(int type) {
        if (ranks.ContainsKey(type))
        {
            return ranks[type];
        }
        else {
            return null;
        }
    }

    public override void Dispose()
    {
        
    }

    #endregion
}
