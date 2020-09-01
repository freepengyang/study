
/// <summary>
/// Task  type  任务类型
/// </summary>
public enum TaskType
{
    None,//空
    MainLine,//主线
    Daily,//日常
    BranchLine,//支线
	TodayCanDo,//今日可做
	WantIngot,//我要元宝
	GetEquip,//获取装备
	WantStronger,//我要变强
    ActivityLink,//活动链接
}

public enum TaskGoalType
{
    None = 0, //空
    FightMonster = 1,//杀怪
    CollectItem = 2,//收集道具
    ReachLevel = 3,//达到等级
    CompleteInstance = 4,//完成副本
    PK = 6,//pk
    TimeLimit = 7,//时间限制
    Dialogue = 8,//对话
    MapKillMonster = 9,//地图杀怪
    EquipLevel = 10, //装备等级
    MapKillQualityMonster = 11,//指定地图指定品质怪物
    FightMoreMonsterOfOne = 12,//杀多个怪物中的某个怪物
    GetVigorValue = 13, //领取精力值
	GetEquipWay = 14, //获得X件Y级以上某品质装备
}