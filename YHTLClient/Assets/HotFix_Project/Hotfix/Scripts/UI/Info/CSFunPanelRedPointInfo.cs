using System.Collections.Generic;

public class CSFunPanelRedPointInfo : CSInfo<CSFunPanelRedPointInfo>
{
    private Map<FuntionMuneType,  RedPointType[]> FucRedPointDic = new Map<FuntionMuneType, RedPointType[]>
    {
        {FuntionMuneType.RolePanel, new []{RedPointType.LianTi, RedPointType.WolongUpGrade, RedPointType.Fashion}},

        {FuntionMuneType.BagPanel, new []{RedPointType.Bag,RedPointType.PetLevelUp}},

        {FuntionMuneType.SkillPanel, new []{RedPointType.SkillUpgrade}},
     
        {FuntionMuneType.ForgePanel, new []{RedPointType.EquipRecast, RedPointType.EquipRefine, RedPointType.EnhanceForge, RedPointType.Combine,RedPointType.Gem}},
     
        {FuntionMuneType.TimeExpPanel, new []{RedPointType.TimeExp}},
     
        {FuntionMuneType.WingPanel, new []{RedPointType.WingFunction}},
     
        {FuntionMuneType.UIWarPetCombinedPanel, new []{RedPointType.PetAwake,RedPointType.PetSkillUpgrade, RedPointType.PetRefine,RedPointType.PetTalent}},
     
        {FuntionMuneType.HandBookPanel, new []{RedPointType.HandBookSetuped,RedPointType.HandBookUpgradeLevel,RedPointType.HandBookUpgradeQuality,RedPointType.HandBookSlotUnlock}},

        {FuntionMuneType.UnionPanel,new []{ RedPointType.GuildPractice, RedPointType.GuildApplyList, RedPointType.GuildList }},

        {FuntionMuneType.RongLian,new []{ RedPointType.LongLiRefine, RedPointType.LongJiRefine}},
    };

    public RedPointType[] GetFucResList(FuntionMuneType type)
    {
        if (FucRedPointDic != null && FucRedPointDic.ContainsKey(type))
            return FucRedPointDic[type];
        return null;
    }

    public Map<FuntionMuneType, RedPointType[]> GetFunRed()
    {
        return FucRedPointDic;
    }

    public override void Dispose()
    {
        FucRedPointDic?.Clear();
        FucRedPointDic = null;

    }
}

public enum FuntionMuneType
{
    RolePanel = 1, //角色
    BagPanel = 2, //背包
    SkillPanel = 3, //技能
    ForgePanel = 4, //锻造
    TimeExpPanel = 5, //泡点神符
    WingPanel = 6, //翅膀
    SocialPanel = 7, //社交
    UnionPanel = 8, //行会
    RankingPanel = 9, //排行榜
    SettingPanel = 10, //设置

    HandBookPanel = 17, //图鉴面板
	UIWarPetCombinedPanel = 18, //卧龙战宠zk
    RongLian = 52,//熔炼
}