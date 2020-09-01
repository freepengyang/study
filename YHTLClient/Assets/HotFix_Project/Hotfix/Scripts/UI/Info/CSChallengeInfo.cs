public class CSChallengeInfo : CSInfo<CSChallengeInfo>
{

    //冒险等级
    public int Level { get; set; }
    //冒险经验
    public int Experience { get; set; }
    //原地复活次数
    public int ResurgenceCount { get; set; }
    //赛季结束时间
    public long SeasonCloseTime { get; set; }
    //当前关卡
    public int CurCustomsPass { get; set; }
    
    
    
    
    
    public override void Dispose()
    {
        
    }
}