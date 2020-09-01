/// <summary>
/// User
/// </summary>
public enum GoingDownReason
{
    Maintain = 0, //维护
    Block = 1, //封禁
    AnotherSession = 2, //其他设备登录
    FangChenMi = 3,		//防沉迷
}

/// <summary>
/// Team
/// </summary>
public enum TeamTab {
    TabNone=0,
    RoundPlayers = 1,//附近的玩家
    Friends = 2,//好友
    Union = 3,//行会
    Applies = 4,//申请
    RoundTeams = 5,//附近的队伍
    QuickJoinTeam = 6,//快捷组队
}

/// <summary>
/// Team
/// </summary>
public enum ConfirmTeamApplyType {
    TypeNone=0,
    Accept = 1,//同意
    Refuse = 2,//拒绝
    Shield = 3,//屏蔽
}

/// <summary>
/// Union
/// </summary>
public enum UnionTab {
    TabNone=0,
    UnionsList = 1,//帮会列表
    MainInfo = 2,//帮会信息
    UnionStoreHouse = 3,//帮会仓库
    SouvenirWealthPacks = 4,//帮会红包
    UnionLogMessages = 7,//帮会消息
    UnionApplyInfos = 8,//申请信息
    UnionMemberInfo = 10,//帮会成员
}

/// <summary>
/// Map
/// </summary>
public enum PositionChangeReason {
    Crnull = 0,
    MoveTooQuick = 1,//移动间隔太短
    MoveDistanceTooLong = 2,//单次移动距离过长
    MoveAfterDie = 3,//死亡后移动
    MoveUnderControl = 4,//有控制buff
    RandomStone = 5,//随机石
    Transfer = 6,//传送
    TaskFly = 7,//任务小飞鞋
    Gate = 8,//传送门
    Relive = 9,//复活
    TargetCannotCross = 10,//目标点不可走
    FriendSummon = 11,//好友召唤
    GateGM = 12,//GM传送
    CrossServerOver = 13, //5v5结束播放动画
    WaiGua = 14,  //外挂
    BrotherRescue = 15, //结拜救援
    UnionCallBack = 16, //家族召唤令
    NationCallBack = 17,//国家召唤令
    NationTransfer = 18,//国家传送
    BiaoCheTransfer = 19,//押镖传送
    DragonflyTransfer = 20,//红蜻蜓传送
    UnionHuntTransfer = 21,//家族狩猎传送
    NationTaskFlay = 22,//跨国任务传送
    ClearToPreMap = 23,	//传送到地图配置的pre点上
    CoupleTransfer = 24,	//夫妻技能传送
    TeamCallBack = 25,		//队伍召唤令传送
    JianYuMap	= 26,		//监狱传送
}

/// <summary>
/// Map
/// </summary>
public enum ReliveType {
    RtNone=0,
    BornPoint = 1, //出生点复活
    InPlace = 2, //原地复活
    LastMap = 3, //退出地图
}

/// <summary>
/// Map
/// </summary>
public enum HPChangeReson {
    HPRNone=0,
    AutoRecover = 1, //自动回复
    SkillDamage = 2, //技能伤害
    Fusion = 3, //药品回复
    BufferDamage = 4, //持续性buff回复或伤害
    ReflectDamage = 5,	//反伤
    RemoveTunShiBuff = 6,		//吞噬buff移除（不漂去血的字）
}

/// <summary>
/// Map
/// </summary>
public enum TransferByDeliverExpenseState{
    TSNone=0,
    NeedItem= 1, //需要物品
    NoItem = 2, //不需要物品
    DataError = 3, //数据错误
    NoInto = 4, //无法进入
}

/// <summary>
/// Task  任务状态
/// </summary>
public enum TaskState {
	TaskStateNone=0,
	UnAcceptable = 1, //不可接
	Acceptable = 2, //可接
	Accepted = 3, //已接,进行中
	Completed = 4, //已完成,未交
	Submitted = 5, //已交
	Failed = 6, //已失败
	GiveUp=7,//放弃
	Cost=8,//需要i消耗的状态
}