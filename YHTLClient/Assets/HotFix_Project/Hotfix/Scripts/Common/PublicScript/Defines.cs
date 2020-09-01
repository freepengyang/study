using UnityEngine;
using System.Collections;

//定义客户端全局事件
public enum CEvent
{
    None,
    BATTERY_CHANGED,
    ClosePanel,
    OpenPanel,

    ResLoginMessage,
    Connect,     //连接成功
    ConnectFail,  //连接失败
    Updae_ServerId, //更新服务器id
    ResPlayerInfoMessage,//角色信息
    ResDeleteRoleMessage,//返回删除角色
    ResRandomRoleNameMessage,//返回随机名字
    CreateRoleNtfMessage,//创建角色反馈
    Disconnect,//正常断线   
    CheckCreateRoleArgsVaildAckMessage,//创建角色检测角色参数回应
    CloseAllRedPoint,

    EffectLoaderSuccess,
    HitTerrainMoveOver,
    MainPlayerMapPointsClear,
    MainPlayerTargetPosChange,
    MainPlayer_StopTrigger,      //角色 Move=>Stop
    MainPlayer_CellChangeTrigger,//移动格子变化
    MainPlayer_DirectionChange, //角色方向改变
    MainPlayer_OnLoaded,        //角色创建完成
    Player_ReplaceEquip,        //换装
    Pet_StateChange,            //神兽状态改变
    Scene_Change,
    Scene_EnterScene,           //进入场景
    Scene_ChangeMap,            //切换地图
    Scene_RefreshView,          //刷新视野
    Scene_ExitView,             //离开视野
    Scene_PlayerEnterView,      //玩家进入视野
    Scene_MonsterEnterView,     //怪物进入视野 
    Scene_NpcEnterView,         //NPC进入视野
    Scene_PetEnterView,         //Pet进入视野
    Scene_ItemEnterView,        //物品进入视野 
    Scene_BufferEnterView,      //buff进入视野
    Scene_TriggerEnterView,     //Trigger进入视野
    Scene_SafeAreaCoordView,    //场景静态特效进入视野
    Scene_PlayerAdjustPosition, //纠正玩家位置
    Scene_ObjectChangePosition, //物体瞬移
    Scene_ObjectMove,           //怪物移动
    Scene_UpdateRoleMove,       //人物移动
    Scene_EnterSceneAfter,      //进入场景之后（角色已创建）
    Fight_SkillEffect,          //技能使用效果
    Avatar_AttachBottom,        //重新挂载bottom
    Avatar_AttachBottomNPC,     //重新挂载bottomNPC
    HP_Change,                  //血量改变
    MP_Change,                  //MP_change
    Buff_Add,                   //玩家自己buff添加
    Buff_Remove,                //玩家自己buff移除
    Buff_Add_OtherPlayer,       //其他人buff添加
    Buff_Remove_OtherPlayer,    //其他人buff删除
    AutoFight_Change,           //自动战斗状态改变
    AutoPickItem_Over,          //自动拾取结束
    MainTask_AddTransmitEvent,  //添加主线传送id
    MainTask_RemoveTransmitEvent,  //移除主线传送id

    UIJoystick_Reset,           //摇杆数据重置
    UIJoystick_Move,            //摇杆移动
    UIJoystick_Stop,            //摇杆停止

    OnPickupItemMessage,        //拾取道具
    OnPickupItemPlayEffect,     //道具拾取动画
    Reach_Npc_Position,       //同地图位置纠正
    OnUnRarFinish,
    UIDebugLogNotify,
    UI_Login,
    UI_LoginBack,
    YvVoiceLoginType,
    DownloadFinish,
    YvVoiceSpeakState,
    YvVoiceUpmicPlayerChanged,//语音上麦人员变更
    Role_ChangeMapId, //玩家地图更改

    #region 局部事件更新
    player_GuildNameChange, //人物公会名字变化--（其他玩家）
    player_NameChange,//任务名字变化
    player_pkValueChange,//人物PK值变更
    player_greyStateChange,//人物灰名状态变更
    player_GuildIdChange,//公会Id变更
    player_levelChange, //人物等级变化--（其他玩家）
    Player_Relive,      //玩家复活
    MainPlayerCanSpeak,
    MonsterOwner_Change,//怪物归属人变化
    Player_TeamIdChange,//场景人物地图更改--（其他玩家）
    Player_TitleChange,//称号改变
    pet_awaked,//战魂觉醒
    #endregion

    BagInit,                                //BagInfo初始化
    WearEquip,                              //穿装备
    UnWeatEquip,                            //脱装备
    BagItemsStartDrag,                           //背包拖拽
    BagItemsDrag,                           //背包拖拽
    ItemShowArrow,                          //格子显示箭头
    BagBoxItemClick,                           //背包格子单击（宝箱道具获得后显示红点，点击消失。其他地方不要监听这个消息）
    GetNewBox,                              //获得新宝箱
    ItemChange,                             //道具变动
    BagMaxCountChange,                      //背包最大数量变化
    ItemListChange,                             //道具列表变动
    //ItemCounterChanged,                         //道具计数变动
    BagItemDBClicked,                          //背包道具被双击了
    OnMailStateChanged,                        //邮件的状态改了
    MailListChanged,                        //邮件变动
    OnMailRead,                             //邮件已经读取
    OnRecycleItemChanged,                   //装备回收
    OnRecycleItemSelected,                  //回收物品选中
    OnRecycleItenUnSelected,                //回收物品取消选中
    OnRecycleSelectedRefresh,               //刷新选中的物品
    GetExp,                                 //收到经验变动
    GetUpgrade,                             //收到升级
    TimeExpChanged,
    TimeExpUprgaded,                        //收到升星消息                    
    CloseTips,                              //tips关闭
    RefineCurValueClick,                    //洗练锁定当前值
    EquipXiLianNtfMessage,                  //收到装备洗练消息
    EquipRebuildNtfMessage,                 //收到装备重铸消息
    TipsBtnRecycleUnSelectd,                //tips页签点击撤销回收

    MoveUIMainScenePanel,//打开功能面板，主场景版面移动
    UI_RefreshUIMainButton,////重新刷新主界面活动按钮
    SCChooseXiLianResultNtf,//收到洗练最终结果消息

    ShowChatPanel,  //显示聊天面板
    HideChatPanel,  //隐藏聊天面板
    OnRecievedNewChatMessage, //收到聊天消息
    OnChatChannelMessageCleared,//聊天频道消息被清楚
    CloseChatAddPanel, //关闭聊天弹出面板
    OnChatChannelChanged,//聊天面板专用
    OnVoiceRoomNtfMessage,//聊天语音消息
    OnRoleDetailNtfMessage,//聊天语音消息请求语音成员列表信息
    OnLeaderCallMessage,//召集令 行会，国家等召集令

    OnChatSettingChanged,//选项改变时

    OnMainPlayerTeamIdChanged,//主角队伍
    OnMainPlayerGuildIdChanged,//主角行会变更
    OnMainPlayerGuildLvChanged,//主角行会等级变更
    OnMainPlayerGuildNameChanged,//主角名字变更
    OnMainPlayerGuildPosChanged,//主角职位变更
    OnMainPlayerVipLevelChanged,//主角会员等级变更
    OnGuildInfoChanged,//行会信息变更
    OnGuildDonateSucceed,
    OnGuildTabDataChanged,//行会数据信息更新
    OnGuildBulletChanged,//行会公告变更
    OnGuildBagChange,//公会背包变更
    OnGuildPracticeInitialized,//行会个人修为初始化
    OnGuildPracticeImproved,//行会个人修为提升了
    OnImpeachBubbleVisible,//显示弹劾气泡
    OnGuildApplyUnionListChanged,//行会申请列表更新
    OnGuildInviteMessage,//收到公会邀请
    OnGuildWarDeclare,//收到公会宣战消息
    OnGuildFightRankListChanged,//公会战列表变更
    OnGuildFightScoreListChanged,//公会战积分列表更新
    OnGuildFightStateChanged,//公会战状态变更
    OnJoinedGuildSucceed,//收到加入公会消息
    OnTipsGuildDonate,//行会捐赠
    OnTipsGuildExchange,//行会兑换
    OnRecievedRedPackges,//收到红包消息
    GetWarehouseData,//收到仓库数据
    ItemBaseClick,//itembase点击
    MainPlayer_LevelChange, //玩家等级变化
    GetWarehouseItemsChange,//仓库道具变动
    GetWarehouseSort,//仓库整理
    WarehouseCountDown,//仓库整理倒计时

    CloseSelectionPanel,       //关闭玩家/怪物选中面板

    ChangeGuidePanelVisible,//改变引导可视化

    //组队相关
    GetMyTeamInfo,//获取我的队伍信息
    AddTeamForMe,//我加入队伍
    AddTeamOther,//其他人加入队伍
    QuitTeam,//退队或踢人
    ChangeLeader,//队长变更
    TeamTabMessage,//对应类型面板信息
    UpdatePlayerHpMpInfoMessage,//更新队伍中玩家血量信息
    UpdatePlayerLevelInfoMessage,//更新队伍中玩家等级信息
    ResInviteTeam,//接收到组队邀请
    HandledInviteTeam,//已处理组队邀请
    EnrollmentApplication,//队长收到入队申请
    HandleEnrollmentApplication,//队长处理完入队申请


    //好友相关
    SocialInfoUpdate,//好友信息变更
    OnFriendRelationChanged,//好友关系变更
    OnApplyListChanged,//好友申请列表变更
    AddSearchFriendsInfo,//查找信息
    DelSearchFriendsInfo,//删除查找信息
    OnRecvNewPrivateChatMsg,//私人聊天时间变更
    OnPrivateChatMessageBeRead,//好友红点
    PrivateChatMessage,//收到私人聊天消息
    RemoveTouchPlayer,//移除最近联系人
    PrivateChatToMessage,//给人私聊
    OnPrivateChatTween,//私人聊天面板更改位置

    /// <summary>晚上12点之后数据更新</summary>
    ResDayPassedMessage,
    PlayerRoleExtraValues,//玩家额外数据变更
    //货币变动
    MoneyChange,
    RefineResultClose,//装备洗练结果面板关闭
    RefineResultRefresh,//洗练结果刷新
    Task_GoalUpdate,//任务状态更新  ----状态变更或者进度变更时，发送任务id其他不发送，用于减少主页面刷新
    Task_FinshEffect,          // 完成任务特效
    Task_FinshGuide,          // 完成任务触发引导
    Task_AcceptEffect,         // 接收任务特效
    OpenFuncRecommond, //等级开放提示
    ResetMainTeamSelect,//重置主页面组队面板选中效果
    SkillUpgradeSucceed,//技能升级成功
    AttachedSkillModified,//技能修正数据变更
    OnSkillAdded,//获得了新技能
    OnSkillRemoved,//移除技能
    OnSkillDragStart,//技能拖动开始
    OnSkillSlotChanged,//技能槽技能变更
    OnSkillEnterCD,//技能进入CD
    OnSkillCoolDown,//技能冷却
    OnSkillCDChanged,//技能CD变更
    OnSkillAutoPlayChanged,//技能自动施放变更
    SetSkillSelectedEffect,//设置技能选中特效
    PetSkillUpgradeSucceed,//战宠技能升级成功
    PetSkillSelected,//战宠技能
    OnPetSkillAdded,//获得了新战宠技能
    OnPetSkillRemoved,//移除战宠技能
    PetBdjnChanged,//战宠被动技能变更
    OnPetJobChanged,//战宠职业变更
    //OnPetLevelChanged,//战宠等级变更
    GuWuSkill,//设置鼓舞技能
    SetSkilInfo,

    OnMoneyStackChanged,//货币栈变动了
    BagSort,//背包整理
    Rename,//改名卡使用返回

    //精力值
    SCEnergyInfoMessage,//获得精力值数据
    SCEnergyFreeGetInfoMessage,//获得免费领取数据
    SCGetFreeEnergyMessage,//获得已领取id
    SCNotifyEnergyChangeMessage,//获得精力值变化
    SCEnergyExchangeInfoMessage,//玩家兑换返回
    UpdateFindState,//寻路状态改变
    CancelPathFind,//取消自动寻路
    Reach,// 抵达目的地
    StartNextMissionEvent,//执行任务下一个行为
    WoLongLevelUpgrade,//卧龙经验等级升级回应

    //封印
    OpenSeal,//开启封印
    ShortenSeal,//缩短封印时间
    CloseSeal,//关闭封印
    ChangeEquipShow,//装备展示界面
    GameModelSealPanel,//跳转成功到封印界面

    //七日登录
    OnSevenDayLoginChanged,//活动信息变更

    //幻境
    OpenDreamLand,//开启幻境
    CloseDreamLand,//关闭幻境
    ChangeDreamLandTime,//幻境时间变化
    GameModelDreamPanel,//跳转成功到幻境界面
    //EnterDreamLand,//进入幻境
    //ExitDream,//退出幻境

    //图鉴
    OnHandBookUpgradeSucceed,//图鉴升级升品
    OnHandBookRemoved,//图鉴移除
    OnHandBookAdded,//图鉴添加
    OnHandBookSlotChanged,//图鉴槽位变动
    RemoveSelectedFlag,//给图鉴内部界面使用
    OnHandBookInlayChanged,//图鉴镶嵌变动
    OnChoicedItemChanged,//给图鉴内部界面使用
    OnHandBookChoicedForUpgrade,//给图鉴内部界面使用
    OnHandBookChoicedForMerge,//给图鉴内部界面使用
    OnHandBookTabChanged,//图鉴TAB页变更
    OnAutoSetupHandBook,//自动装载图鉴
    OnPopUnlockHandBookMessageBox,//自动弹出图鉴解锁提示框

    //复活
    Death,
    Relive,
    ReliveMainPlayer,

    FunctionOpenStateChange,//功能开放状态更改，需要监听功能开启状态改变，监听此协议， object 传 functionId
    OnFunctionPromptChange,//气泡变动
    BubbleGuide,//气泡引导

    SCWorldBossActivityInfoResponseMessage,//服务器发送世界boss活动信息
    SCNotifyWorldBossRankInfoMessage, //服务器发送世界boss排行榜信息
    ECM_SCWorldBossBlessInfoMessage,//世界boss祝福次数信息
    MainFuncShowRanking,// 主页面 将组队按钮换成排行榜
    ECM_SCWorldBossBossInfoMessage,//世界boss信息
    ResInstanceInfo,//收到副本信息
    GetEnterInstanceInfo,//进入副本
    LeaveInstance,//离开副本
    ECM_SCInstanceFinishMessage,//副本结束信息
    ECM_SCBossInfoMessage,//野外boss消息
    ResInstanceCountMessage,//副本次数变动
    SCBossChallengeMessage,//荣耀挑战响应
    SetSelectMissionState,//设置任务选中状态
    PkModeChangedNtf,//玩家PK模式变更
    PkModeTips,//PK模式变动飘字
    PkValueUpdate,//玩家pk值变化	
    DungeonInfo,//显示地牢围攻波
    ChangeHeadSkillInfo, //玩家头顶地牢信息改变
    UndergroundTreasure, //地下寻宝信息
    FightPowerChange,//战斗力变化
    ShieldChange,//护盾值变化
    MaxShieldChange,//最大护盾值变化
    ActiveShield,//激活护盾
    MainFuncModeChanged,
    ActivityLinkChanged,

    OnHintUsedItemCountChanged,

    ShopBuyTimesChange,//商城购买次数变动
    ShopInfoChange,//商店信息变动
    SCRandomPaoDianMessage,//随机泡点位置
    SCCurrentPaoDianMessage,//当前站的泡点位置
    SCPaoDianExpMessage,//当前站的泡点经验

    //时装
    AllFashionInfo,//获取时装数据
    EquipFashion,//穿上时装
    FashionStarLevelUp,//升星
    AddFashion,//添加时装
    RemoveFashion,//删除时装
    UnEquipFashion,//卸下时装

    MainShowExitInstance, //主页面显示离开副本按钮

    //翅膀
    GetWingInfo,//获取翅膀数据
    WingStarUp,//翅膀升星
    WingAdvance,//翅膀进阶
    DressWingColor,//穿戴幻彩
    WingExpItemUse,//经验丹使用
    WingColorChange,//幻彩道具变更

    //羽灵
    YuLingInfo,//羽灵信息

    //极限挑战
    UltimateRankInfo,//服务器下发排行榜数据
    UltimateData,//极限挑战数据更新
    UltimateOpenCardInfoMessage,//发送翻牌结果
    UltimateSelectCardIndexMessage,//选择卡片回应
    UltimateSelectAdditionIndexMessage,//选择属性效果回应

    //炼体
    GetLianTiInfo,//获取炼体数据
    GetLianTiLandInfo,//获取炼体之地数据
    //战宠
    GetPetStateInfo,    //出战等状态
    //随机夺宝
    GetRandomThingInfo,//获取随机夺宝数据
    GetMapDescInfo,     //获取地图详情（目前只有当前人数）
    GetGoldKeyPickUpItemList,//获取金钥匙后拾取的道具列表，吸道具动画用
    //石墓寻宝
    GetTombTreasureGridInfo,        //获取格子信息
    GetTombTreasureDoorInfo,        //传送门信息
    GetTombTreasureUpdateInfo,      //石阵刷新
    GetTombTreasureNormalInfo,      //石阵普通奖励，渐隐渐现

    //宠物基础信息
    GetWarPetBaseInfo,              //获取宠物基础信息
    GetWarPetBaseHpInfo,            //获取宠物血量信息
    GetWarPetBaseActiveEffect,      //战宠激活特效
    GetWarPetBaseActive,            //战宠激活
    GetWarPetCombinedSkill,         //战宠合体技能更新

    //每日充值
    GetEveryTimeDayChargeInfo,      //每次充值，通知信息处理
    GetDayChargeInfo,               //刷新每日充值面板
    GetDayChargeMapFirst,			//每次进入游戏，是否打开过每日充值地图面板，小红点用

    //装备悬赏
    GetEquipRewardsInfo,            //装备悬赏面板刷新用

    //boss狂欢
    GetBossKuangHuanUpdateInfo,     //boss狂欢面板刷新

    //玛法通行证
    RefreshMaFaLayerInfo,       //刷新玛法层数信息
    RefreshMaFaBoxInfo,         //刷新玛法宝箱信息

    //每日充值地图，勇者之地，王者之地
    RefreshMapMonsterInfo,      //刷新杀怪数量

    //回忆秘境
    SetSecretAreaIndex,         //设置地图选项
    SecretAreaFreeInstance,     //地图免费信息

    //拿元宝
    SetIngotInfo,       //设置拿元宝信息
	FirstShowIngotHead,//登录游戏检测头像是否显示

	//军备竞赛
	ResEquipCompetitionMessage,//军备竞赛
                               //交易行
    ECM_ResGetAuctionShelfMessage,//自己已上架商品列表响应
    ECM_ResAddToShelfMessage,//上架响应
    ECM_ResRemoveFromShelfMessage,//下架响应
    ECM_ResGetAuctionItemsMessage,//商品列表响应    
    UpdateShopMessage,// 请求商城数据后回应    
    ECM_ResUnlockAuctionShelveMessage,//解锁货架返回
    ECM_ResBuyAuctionItemMessage,//购买返回
    AuctionBuyItemDrag,//交易行购买格子拖动
    ResAttentionAuctionMessage,//关注列表返回

    OpenZhuFuYou,//打开祝福油界面

    //雷达- 小地图
    InitMiniMapCallBack,//重新注册雷达事件
    UpdateMapInfo,//地图刷新重置数据后发送事件
    UpdateMapSpecialPlayer,//更新地图特殊显示点

    EnhanceResponse, //强化结果
    SuitStarLvChange,//强化套装等级变更
    SuitStarLvProtectStart,//强化套装保护效果开启
    EnhanceBtnRedPointCheck,
    OpenEnhancePanel,//强化界面打开

    Role_UpdateChangeAttributeValue,//玩家属性变化

    PlayerAutoActionChange,//玩家当前自动行为状态变更


    //设置部分
    Setting_BgmValueChange,//bgm音量
    Setting_BgmSwitchChange,//bgm开关
    Setting_SoundEffectValueChange,//音效音量
    Setting_SoundEffectSwitchChange,//音效开关
    Setting_VoiceValueChange,//语音音量
    Setting_VoiceSwitchChange,//语音开关
    Setting_FixJoystickChange,//固定摇杆开关
    Setting_PushActivityChange,//活动推送开关
    Setting_ForbidGuildChange,//拒绝行会开关
    Setting_ForbidFriendChange,//拒绝好友开关
    Setting_ForbidStrangerChange,//拒绝陌生人开关
    Setting_AllEquipPickUpChange,//装备拾取开关
    Setting_AllItemPickUpChange,//道具拾取开关
    Setting_BYEquipPickUpLvChange,//本元装备拾取等级
    Setting_BYEquipPickUpLvSwitchChange,//本元装备拾取等级开关
    Setting_BYEquipPickUpQualityChange,//本元装备拾取品质
    Setting_BYEquipPickUpQualitySwitchChange,//本元装备拾取品质开关
    Setting_WLEquipPickUpLvChange,//卧龙装备拾取等级
    Setting_WLEquipPickUpLvSwitchChange,//卧龙装备拾取等级开关
    Setting_MoneyPickUpChange,//货币拾取开关
    Setting_ForgingMaterialsPickUpChange,//锻造材料拾取开关
    Setting_FashionPiecesPickUpChange,//时装碎片拾取开关
    Setting_DrugPickUpChange,//药品拾取开关
    Setting_OtherPickUpChange,//其他道具拾取开关

    Setting_AutoTakeDrugSwitchChange,//自动吃药开关
    Setting_AutoTakeDrugChange,//自动吃药药品
    Setting_AutoTakeDrugHpChange,//自动吃药生命值条件开关
    Setting_AutoTakeMpDrugSwitchChange,//自动吃药开关
    Setting_AutoTakeMpDrugChange,//自动吃药药品
    Setting_AutoTakeDrugMpChange,//自动吃药生命值条件开关
    Setting_AutoTakeInstantDrugSwitchChange,//自动瞬回药开关
    Setting_AutoTakeInstantDrugChange,//自动瞬回药品
    Setting_AutoTakeInstantDrugHpChange,//自动瞬回药生命值条件开关

    Setting_AutoRandomDeliverySwitch,//自动随机传送开关
    Setting_AutoRandomDeliveryHpChange,//自动随机传送生命值
    Setting_AutoRandomDeliveryTimeChange,//自动随机传送时间
    Setting_AutoGoBackSwitch,//自动回城传送开关
    Setting_AutoGoBackHpChange,//自动回城传送生命值
    Setting_AutoGoBackTimeChange,//自动回城传送时间

    Setting_AutoAttackPlayerLvChange,//自动攻击玩家等级开关
    Setting_PopGraphicsModeTipsChange,//画面卡顿时弹出提示开关
    Setting_GraphicsModeChange,//画面模式切换
    Setting_HideAllPlayersChange,//隐藏所有玩家开关
    Setting_HideMyGuildPlayersChange,//隐藏本行会玩家开关
    Setting_HideMonstersChange,//隐藏所有怪物开关
    Setting_HideSkillEffectChange,//隐藏技能特效开关
    Setting_HideTaoistMonsterChange,//隐藏道士召唤兽开关
    Setting_HideWarPetChange,//隐藏战魂开关
    Setting_HideAllNameChange,//隐藏全部名称
    Setting_AutoReleaseSkillChange,//自动释放技能设置变更
    Setting_SmartHalfMoonMachetesSwitch,//智能半月弯刀开关
    Setting_SkillSingleChange,//单体技能设置更改
    Setting_SkillGroupChange,//群体技能设置更改

    //进化宝珠
    LevelUpBaoZhu,//宝珠升级
    GradeUpBaoZhu,//宝珠升阶
    RefreshBaoZhuSkills,//返回宝珠技能槽的技能
    ReplaceBaoZhuSkills,//返回宝珠技能槽替换的技能
    BaoZhuBossCountChange,//已装备宝珠打怪数量变化
    BaoZhuSlotSkills,//获取技能槽技能信息

    //宝石
    ECM_SCPosGemInfoMessage,
    CSUnlockGemPositionMessage, //宝石解锁
    CSGemRefresh,
    GemSuitInfoChange,//宝石套装改变
    GemBossKillCount,//宝石boss击杀数量改变
    GemTabRedChange,//宝石tab红点改变

    //行会争霸
    OnGuildFightDataChanged,//公会争霸阶段变更
    //卧龙洗练
    SCWoLongXiLianMessage,//卧龙洗练响应
    SCWoLongXiLianSelectMessage,//通知卧龙洗练确定

    //活跃度
    SCResActiveMessageRefresh, //活跃度信息变化
    DailyActiveTaskChange,
    DailyBtnRedPointCheck,
    //SCResActiveRewardMessage, //已领取奖励变化

    //寻宝
    SeekTreasureHistory,//寻宝历史信息
    SeekTreasureItemChanged,//仓库物品变动
    SeekTreasureStorehouse,//寻宝仓库信息
    SeekTreasureEnd,//寻宝结束响应信息
    SeekTreasureUseExp,//使用经验丹回应
    SeekTreasureBox,//宝箱响应
    SeekTreasureHuntCallBack,//寻宝仓库中回收装备响应

    //流程触发器
    OnLogginTrigger,//登录请触发器

    //vip
    FirstRechargeInfoChange,//首充状态变化
    FirstRechargeRedChange,//首充红点变化
    VipInfoChange, //vip信息改变
    VipExperienceInfoChange, //vip体验卡信息改变，登录时接收的信息

    //卡牌签到
    CardPoolUpdate,//卡池更新
    PlayerCardChange,//玩家卡牌更新
    CollectionLockInfoChange,//组合锁定信息变更
    PiecesCountChange,//碎片数量变化
    CollectionReachedInfoChange,//组合兑换信息变更
    HonorChange,//普通成就变更
    UltHonorReceive,//领取最终成就


    //开服活动
    SCBossFirstKillDatasMessage,//boss首杀
    ResFengYinDataMessage,//返回封印比拼信息
    ResSpecialActivityDataMessage,//开服活动领奖fanhui
    SCCollectActivityData,//装备收集的每档列表

    //野外探险
    WildAdventureInfoChange,//探险数据更新
    WildAdventureBossInfoChange,//boss信息更新
    WildAdventureMonsterAttack,//怪物开始攻击
    WildAdventureMonsterDead,//怪物死亡
    //EquipRankChange,

    RankInfoChange,//排行榜更新
    MonthCardInfoChange,//月卡信息更新
    SevenDayDataChange,//七日试炼数据更新
    GiftBagAllChange,//限购礼包更新
    GiftBagBuyCHange,//限购礼包购买更新
    MonsterSlayInfoChange,//屠魔活动信息更新
    //更好装备提示
    OnBetterEquipRemoved,//获得更好装备
    GetItemPanelOpen,//获得更好装备界面打开
    GetItemPanelClose,//获得更好装备界面关闭
    //装备回收加入模式
    OnRecycleModeChanged,

    //合成功能
    CombineItem,//装备合成

    //终身基金
    LifeTimeFundChange,//终身基金信息改变
    LifeTimeFundRewardChange,//终身基金奖励信息改变
    LifeTimeFundTabRed, //tab红点事件

    //排行榜
    RoleRankInfo,//人物相关排行
    UnionRankInfo,//公会排行
    UICheckManagerInitCheckComplete,//UI检测管理类初始化检测完成

    // CheckPlayerInfo,//查看他人信息
    ShowMissionUnAccept,//等级卡点任务，点击时，弹出任务功能
    HideMissionTips,//隐藏主页面点击等级卡点面板

    //新手引导
    OnTaskGuideNameChanged,
    OnSabacStageChanged,

    //已阅读用户协议
    UserAgreementRead,
    UpdateGame,//更新游戏

    ActivityBubbleAdded,//新增活动气泡。可能是开启预告气泡，也可能是进行中的活动气泡
    ActivityBubbleRemoved,//移除活动气泡
    ActivityBubbleChange,//活动气泡变更
    TimeLimitBubbleInit,//限时活动气泡初始化

    FastUseClick,//快捷使用点击
    FastUseClose,//快捷使用关闭

    UIServerListSelectTable,//服务器列表页面，选中页签


    DownloadResComplete,//下载完成
    DownloadGiftReceived,//下载奖励领取

    OpenMissionGuidePanel,
    CSPetInfoMessage, //战宠升级信息
    SelectEquip, //宠物升级界面选择装备回调
    ConfigCallback, //装备设置回调

    PetTianFuPassiveSkillMessage,//宠物被动技能总信息(宠物升阶的时候也会推)
    PetTianFuRandomPassiveSkill,//生成宠物被动新技能
    PetTianFuChosePassiveSkill,//替换宠物被动技能

    PetTalentLvChange,//宠物天赋等级变更
    PetTalentEquipPointChange,//来自装备的天赋点变化
    PetTalentStarSelect,//宠物天赋星星选择
    PetTalentCorePreviewSelect,//宠物天赋核心预览选择
    PetTalentCheckRedpoint,//宠物天赋红点检测事件
    BreathRedMask,//世界boss红色呼吸遮罩
    CloseSummonPanel,//关闭活动提示弹框

    SabacDoorChange, //玩家改变门状态消息
    FastAccessJumpToPanel,//获取途径跳转面板回调
    FastAccessTransferNpc,//获取途径npc面板回调

    RechargeInfoUpdate,//充值信息更新
    AddUpInfoChange,//累计充值

    //直购礼包
    DailyPurchaseInfo,//直购礼包总数据
    DailyPurchaseBuy,//直购礼包元宝购买
    DailyPurchaseReceive,//直购礼包领取响应
    GiftBagOpen,//直购礼包领取响应
    GiftBagClose,//直购礼包领取响应

    //优惠礼包
    DailyPurchaseBuyDiscount,//优惠礼包购买响应
    LookGift,//查看新礼包响应
    LookPosition,//查看优惠礼包子页签

    SCAthleticsActivityInfoMessage,//竞技活动信息
    SCReceiveAthleticsActivityRewardMessage,//领取竞技活动奖励


    //充值商店
    DailyRmbChange,

    ExchangeShopDataChange,//兑换商城数据变更
    ExchangeShopRefresh,//
    ExchangeShopSingleChange,//兑换商城单个信息变化
    ExchangeShopRedPointCheck,

    MonthRechargeRedPointCheck,

    CardMapPanelOpened,//月卡地图打开
    OnVoiceStateChanged,//刷新语音按钮状态
    MainFuncPanelTweenFinish,//主页面任务动画移动完毕


    //排行榜
    RankInfo,

    //回收物品遍布啊
    OnRecycleItemChange,
    LongLiBubbleClick,//龙力气泡点击
    RequestRequestRoleNum,//服务器列表请求玩家服务器数量

    HideRoleSelectPanel,//隐藏人物选中面板
    HideMonsterSelectPanel,//隐藏怪物选中面板
    ShowRoleSelectPanel,//显示人物选中面板
    ShowMonsterSelectPanel,//显示怪物选中面板

    //怀旧装备
    NostalgiaBagChange,
    NostalgiaEquipChange,
    NostalGeziChange,
    NostalsuitChange,
    NostalgiaChoose,
    NostalgiaRemove,//丢弃装备
    NostalgiaSelectItem,
    NostalgiaRefreshHint, //升级界面刷新下方hint
    NostalgiaRefreshTime, //刷新队友时间


    GuildActivityStateChange,//行会活动状态变更
    GuildCombatOpen,//行会战开始
    GuildCombatClose,//行会战结束
    GuildBossOpen,//行会首领开始
    GuildBossClose,//行会首领结束
    
    PrompClose, //promp 关闭事件

    PlayHejiEffect, // 播放合计特效
    ResetServerListToggle,//重置服务器列表toogle选择
    ZhenQiAdd,//获得真气
    
    SelectMyTeamPlayer,//场景中选中我的队伍里的玩家
    NoSelectLastMyTeamPlayer,//场景中原来选中我的队伍里的玩家变为未选中
    SelectLastMyTeamPlayer,//场景中当前选中的玩家
    ReqChoicedTeamPlayer,//向UIRoleSelectionInfoPanel询问

    NoSelectMonster,
    SelectMonster,
    ReqChoicedMonster,

    OnPlayFightPowerChanged,
    OnNewFunctionPlayOver,

    OnItemUsedTimesChanged,//物品使用次数变更
}
//小红点数据类型


public enum ServerType : int
{
    GameServer = 1,//游戏服
    SharedService = 3,//共享服
    CrossService = 4, //3v3服
    SldgService = 5, //神龙帝国
}

public enum RedPointType
{
    None = 0,
    Init = 1,
    MemoryWarring = 2,
    FlyShoe = 3,
    SkillUpgrade = 4, //技能升级
    PetSkillUpgrade = 5,//战宠技能升级
    Friend = 6,//好友红点
    TimeExp = 7,//泡点神符升级升星红点
    HandBookUpgradeLevel = 8,//图鉴升级
    HandBookSetupedUpgradeLevel = 9,//已经装配的图鉴可以升级
    HandBookUpgradeQuality = 10,//图鉴升品
    HandBookSetupedUpgradeQuality = 11,//已经装配的图鉴可以升品
    HandBookSetuped = 12,//图鉴镶嵌
    HandBookSlotUnlock = 13,//图鉴解锁
    LianTi = 14,//炼体
    EnhanceForge = 15,//强化(打开过页面不再出现红点)
    EnhanceIgnorePanel = 16,//强化(无视是否打开过页面)
    GuildPractice = 17,//公会修炼
    GuildApplyList = 18,//公会申请列表
    GuildList = 19,//公会列表
    DailyBtn = 20, //活跃度
    TombTreasure = 21,//石墓寻宝
    BossFirstKill = 22,//Boss首杀
    SealComPetition = 23,//封印比拼
    OpenServerAc = 24,//开服互动按钮
    PetAwake = 25,//战宠觉醒
    WildAdventure = 26,//野外探险
    DayCharge = 27,//每日充值
    DayChargeMap = 28,//每日充值地图
    ServerActivityRank = 29,//装备评分
    SevenDay = 30, //七日试炼
    Combine = 31,//合成功能
    Vip = 32, //vip功能

    PearlEvolution = 33,//宝珠进化
    PearlSkillslot = 34,//宝珠技能槽

    WingFunction = 35,//羽翼入口
    Wing = 36,//翅膀
    WingColor = 37,//幻彩
    WingSpirit = 38,//羽灵
    BossKuangHuan = 39,//boss狂欢
    Gem = 40, //宝石
    
    ArmRace = 41,//军备竞赛

    MonsterSlay = 42,//屠魔活动
    LifeTimeFund = 43,//终身基金
    WolongUpGrade = 44,//卧龙升级

    DreamLand = 45,//幻境

    SeekTreasureWarehouse = 46,//寻宝仓库

    Fashion = 47,//时装
    FashionActive = 48,//时装激活
    FashionUpStar = 49,//时装升星

    EquipCollect = 50,//装备收集
    RechargeFirst = 51,//首充

    EquipRecast = 52,//装备重铸
    EquipRefine = 53,//装备洗练
    PersonalBoss = 54,//个人boss
    Vigor = 55,//精力值
    Bag = 56,//背包
    ZhuFu = 57,//祝福油

    SignIn = 58,//签到
    MonthCard = 59,//月卡
    AuctionSell = 60,//交易行出售页签
    DownloadGift = 61,//下载有礼
    LongJiRefine = 62,//卧龙龙技洗练
    LongLiRefine = 63,//卧龙龙力洗练
    PetLevelUp = 64,//宠物升级
    PetTalent = 65,//宠物天赋
    PetRefine = 66,//宠物洗炼
    AddUpRecharge = 67,//累充
    DirectPurchaseGift = 68,//直购礼包(元宝足够)
    DirectPurchaseReceive = 69,//直购礼包可领取回馈奖励
    MonthCardMap = 70,//月卡地图
    DailyArena = 71,//每日竞技
    
    DiscountGiftBag = 72,//优惠礼包
    WearableWoLongEquip = 74,//有可穿戴的卧龙装备
    MaFaRewards = 75,//玛法通行证奖励列表
    MaFaBox = 76,//玛法通行证宝箱

    RechargeShop = 77,
    ExchangeShop = 78,
    MonthRecharge = 79,
    GemLevelUp = 80, //宝石升级

    SevenLogin = 81,//七日登录
    Nostalgia = 82, //怀旧装备红点
    GemlevelUp =  83, //宝石升级

	EquipRewards = 84,//卧龙装备悬赏
	PerEquipRewards = 85,//普通装备悬赏
    SignInAchivement = 86,//签到成就
}

/// <summary>
/// 提示状态
/// </summary>
public enum PromptState
{
    rightBtn,             //右按钮
    leftBtn,              //左按钮
    close,               //关闭
}

/// <summary>
/// 色值类型，，，不要自己添加，找UE或者陈颖要对应字色，游戏中统一字色
/// </summary>
public enum ColorType
{
    None,
    //品质色
    White,              //白
    Green,              //绿
    Blue,               //蓝
    Purple,             //紫
    Orange,             //橙
    Red,                //红
    Yellow,             //黄

    //常用字色
    MainText,       //主要文本字色
    SecondaryText,  //次要文本字色
    ImportantText,  //重点文本字色
    WeakText,       //弱文本字色（弱一级文本字色）  灰色
    ToolTipDone,    //提示文本，如完成、获得、充足等， 绿色
    ToolTipUnDone,  //提示文本，如未完成、未获得、不足等。 红色
    ProperyColor,
    TitleColor,
    SubTitleColor,
	TabCheck,         //Tab标签选中字色
	TabBackground,    //Tab标签未选中字色

	NPCMainText,
    NPCSecondaryText,
    NPCImportantText,

    /// <summary>
    /// 策划灰色--按钮置灰字体颜色
    /// </summary>
    CommonButtonGrey,
    /// <summary>
    /// 策划棕色--按钮正常字体颜色
    /// </summary>
    CommonButtonBrown,
    /// <summary>
    /// 策划绿色--绿色按钮正常字体颜色
    /// </summary>
    CommonButtonGreen,

    SabacTeam,
}

//角色性别
public class ESex
{
    /// <summary>
    /// 女
    /// </summary>
    public const int WoMan = 0;
    /// <summary>
    /// 男
    /// </summary>
    public const int Man = 1;
    /// <summary>
    /// 通用
    /// </summary>
    public const int Common = 2;
}

public class ECareer
{
    /// <summary>
    /// 通用
    /// </summary>
    public const int Common = 0;
    /// <summary>
    /// 战士
    /// </summary>
    public const int Warrior = 1;
    /// <summary>
    /// 法师
    /// </summary>
    public const int Master = 2;
    /// <summary>
    /// 道士
    /// </summary>
    public const int Taoist = 3;
    /// <summary>
    /// 村民
    /// </summary>
    public const int CunMin = 5;
    /// <summary>
    /// 弓手
    /// </summary>
    public const int GongShou = 6;
    /// <summary>
    /// 自由职业
    /// </summary>
    public const int Freedom = 7;
}

public class EControlState
{
    public const int Idle = 0;
    public const int JoyStick = 1;
    public const int Mouse = 2;
    public const int Key = 3;
}

public class ETouchType
{
    public const int None = 0;
    public const int Normal = 1;
    public const int Attack = 2;
    public const int Touch = 3;
    public const int Task = 4;
    public const int WaKuang = 5;
    public const int AttackTerrain = 6;
    public const int GuWu = 7;
    public const int RunOverDoSomething = 8;
}

//移动请求类型
public class EMoveRequestType
{
    /// <summary>
    /// 不发送
    /// </summary>
    public const int NotSend = 0;
    /// <summary>
    /// 正常移动
    /// </summary>
    public const int NormalSend = 1;
    /// <summary>
    /// 修正位置
    /// </summary>
    public const int ChangePos = 2;
}

public class ESkillGroup
{
    /// <summary>
    /// 攻杀
    /// </summary>
    public const int GongSha = 3;
    /// <summary>
    /// 刺杀
    /// </summary>
    public const int CiSha = 4;
    /// <summary>
    /// 半月
    /// </summary>
    public const int BanYue = 5;
    /// <summary>
    /// 野蛮冲撞
    /// </summary>
    public const int YeMan = 6;
    /// <summary>
    /// 烈火
    /// </summary>
    public const int LieHuo = 7;
    /// <summary>
    /// 雷电术
    /// </summary>
    public const int LeiDianShu = 10;
    /// <summary>
    /// 火墙术
    /// </summary>
    public const int HuoQiangShu = 11;
    /// <summary>
    /// 魔法盾
    /// </summary>
    public const int MagicShield = 12;
    /// <summary>
    /// 冰咆哮
    /// </summary>
    public const int BingPaoXiao = 13;
    /// <summary>
    /// 寒冰掌
    /// </summary>
    public const int HangBingZhang = 14;
    /// <summary>
    /// 流星火雨
    /// </summary>
    public const int LiuXingHuoYu = 15;
    /// <summary>
    /// 抗拒火环
    /// </summary>
    public const int KangJuHuoHuan = 16;
    /// <summary>
    /// 治愈术
    /// </summary>
    public const int Healing = 18;
    /// <summary>
    /// 施毒术
    /// </summary>
    public const int ShiDuShu = 19;
    /// <summary>
    /// 灵魂火符
    /// </summary>
    public const int LingHunHuoFu = 20;
    /// <summary>
    /// 隐身术
    /// </summary>
    public const int Hiding = 21;
    /// <summary>
    /// 召唤神兽
    /// </summary>
    public const int ZhaoHuanShenShou = 23;
    /// <summary>
    /// 无极真气
    /// </summary>
    public const int WuJiZhenQi = 24;
    /// <summary>
    /// 气波功
    /// </summary>
    public const int QiBoGong = 25;
    /// <summary>
    /// 战魂烈火
    /// </summary>
    public const int ZhanHunLieHuo = 74;
    /// <summary>
    /// 战魂烈火
    /// </summary>
    public const int ZhanHunCiSha = 72;
    /// <summary>
    /// 战魂烈火
    /// </summary>
    public const int ZhanHunBanYue = 73;
    /// <summary>
    /// 战士普攻
    /// </summary>
    public const int WarriorAttack = 97;
    /// <summary>
    /// 法师普攻
    /// </summary>
    public const int MasterAttack = 98;
    /// <summary>
    /// 道士普攻
    /// </summary>
    public const int TaoistAttack = 99;
}

public class ESpecialSkillID
{
    public const int GongSha = 31001;   //攻杀剑法
    public const int ShenShou5 = 23005;    //5级召唤神兽
    public const int ShenShou10 = 23010;    //10级召唤神兽
    public const int ShenShou15 = 23015;    //15级召唤神兽
    public const int ShenShou20 = 23020;    //20级召唤神兽

}

public class ESkillTargetType
{
    /// <summary>
    /// 对敌（所有可攻击目标）
    /// </summary>
    public const int Enemy = 1;
    /// <summary>
    /// 对己
    /// </summary>
    public const int Self = 2;
    /// <summary>
    /// 宠物
    /// </summary>
    public const int Pet = 3;        //宠物
    /// <summary>
    /// 友方
    /// </summary>
    public const int Friend = 4;
    /// <summary>
    /// 召唤兽
    /// </summary>
    public const int SummonPet = 5;
    /// <summary>
    /// 除自己外
    /// </summary>
    public const int AllOther = 6;
    /// <summary>
    /// 怪物
    /// </summary>
    public const int Monster = 7;
    /// <summary>
    /// 目标对敌，但是对战魂生效
    /// </summary>
    public const int ZhanHunInteadOfEnemy = 8;

    /// <summary>
    /// 位移技能
    /// </summary>
    public const int Displacement = 9;

    /// <summary>
    ///有目标对目标释放，无目标对自己释放
    /// </summary>
    public const int SelfAndOther = 100;  
}

public class ESkillLaunchType
{
    /// <summary>
    /// 寻目标攻击
    /// </summary>
    public const int Target = 0;
    /// <summary>
    /// 原地施法有施法动作
    /// </summary>
    public const int InSitu = 1;
    /// <summary>
    /// 原地施法没有动作
    /// </summary>
    public const int InSituWithoutActtion = 2;
}

public class ESkillEffectID
{
    /// <summary>
    /// 攻杀技能特效
    /// </summary>
    public const int GongSha = 1;
    /// <summary>
    /// 烈火技能特效
    /// </summary>
    public const int LieHuo = 4;
    /// <summary>
    /// 烈焰斩
    /// </summary>
    public const int LieYanZhan = 24;

}

public class EBuffType
{
    public const int None = 0;
    /// <summary>
    /// 眩晕:无法移动无法释放技能
    /// </summary>
    public const int Vertigo = 4;
    /// <summary>
    /// 隐身
    /// </summary>
    public const int Hiding = 8;
    /// <summary>
    /// 定身：不能移动可攻击
    /// </summary>
    public const int Hold = 13;
}

public class ESpecialBuff
{
    /// <summary>
    /// 烈火buff
    /// </summary>
    public const int LieHuo = 210026;
    /// <summary>
    /// 烈焰斩buff
    /// </summary>
    public const int LieYanZhan = 210027;
    /// <summary>
    /// 攻杀buff
    /// </summary>
    public const int GongSha = 210031;
}

public class EPetChangeState
{
    public const int None = 0;
    /// <summary>
    /// 跟随
    /// </summary>
    public const int Follow = 2;
    /// <summary>
    ///  战斗
    /// </summary>
    public const int Fight = 3;
}

/// <summary>
///玩家战斗状态
/// </summary>
public class EPlayerFsmState
{
    public const int None = 0;
    /// <summary>
    /// 激活
    /// </summary>
    public const int Active = 2;
    /// <summary>
    /// 战斗
    /// </summary>
    public const int Fight = 3;
}




public class EPkMode
{
    /// <summary>和平</summary>
    public const int Peace = 1;
    /// <summary>全体</summary>
     public const int All = 2;
    /// <summary>帮会</summary>
    public const int Union = 3;
    /// <summary>队伍</summary>
    public const int Team = 4;
    /// <summary>善恶</summary>
    public const int Red = 5;
}


public enum MoneyType
{
    gold = 1,               //金币
    bindGold = 2,           //绑金
    yuanbao = 3,            //元宝
    liquan = 4,             //券
    exp = 6,                //经验
    shengwang = 7,          //声望
    hunlijingpo = 8,        //魂力精魄
    zhenqi = 9,             //真气
    unionAttribute = 10,    //行会贡献
    wolongxiuwei = 11,      //卧龙修为
    jinglizhi = 12,         //精力值
    xunbaojifen = 13,       //寻宝积分
    dilaojifen = 14,        //地牢围攻积分
}

public enum TipBtnType
{
    XiLian = 1,
    ChongZhu,
    Wear,
    TakeOff,
    Replace,
    Use,
    Discard,
    ReplaceLeft,
    ReplaceRight,
    Split,
    PutIn,
    TakeOut,
    Donate,
    Exchange,
    Recycle,
    CancelRecycle,
    Compound,
    ZhuFu = 18,
    Inlaid = 19,
    PearlUpgrade = 20,
    PearlEvolution = 21,
    Intensify = 22,
    BatchUse = 23,
    Forge = 24,
    Deal = 25,
    HuaijiuWear = 26,  //怀旧装备放入
    HuaijiuTakeOff, //怀旧装备取出
    HuaijiuLevelUp, //怀旧装备升阶
    HuaijiuRemove, //怀旧装备丢弃
    Huaijiureplace, //怀旧装备替换
}

/// <summary>
/// FuncOpen 表中的 id
/// </summary>
public enum FunctionType
{
    //functionPanel
    none = 0,
    funP_role = 1,
    funP_bag = 2,
    funP_skill = 3,
    funP_chongzhu = 4,
    funP_timeExp = 5,
    funP_wing = 6,
    funP_socail = 7,
    funP_guild = 8,
    funP_rank = 9,
    funnP_setting = 10,
    funP_personalBoss = 11,
    funcP_vigor = 12,
    funcp_wolong = 13,
    funcP_worldBoss = 14,
    funcP_shop = 15,
    funcP_lianTi = 16,
    funcP_handbook = 17,
    funcP_Auction = 19,
    funcP_tombTreasure = 20,
    funcP_OpenServerAc = 21,
    funcP_dailyActivities = 22,
    funcP_SevenDay = 23,
    funcP_HeCheng = 24,
    funcP_HonorChanllenge = 25,
    funcP_petHead = 26,
    funcP_seekTreasure = 29,
    funcP_wildAdventure = 30,
    funcP_monthCard = 31,
    funcP_ArmRace = 33,

    funcP_xiLian = 35,
    funcP_sabacFight = 36,
    funcP_rechargefirst = 38,
    funcP_welfare = 39,
    funcP_vip = 40,
    funcP_sealGrade = 41,
    funcP_wildBoss = 42,
    funcP_enhance = 44,

    funcP_baoshi = 47,

    funcP_signIn = 50,
    funcP_faseUse = 51,
    funcP_LongLi = 52,
    funcP_LongJi = 53,
    funcP_WarPetSkill = 54,
    funcP_WarPetRefine = 55,
    funcP_BaoZhuang = 56,
    funcP_DailyArena = 57,
    funcP_FaMa = 58,
    funcP_BiQiShop = 59,
    funcP_wolongRecycle = 60,
    funcP_WingSoul = 61,
    funcP_Nostalgia = 63,
    funcP_GiveIngot = 64,
    funcP_BeStrong = 66,
    funcP_Mail = 67,
    funcP_Team = 68,
    funcP_Friend = 69,//社交
}

/// <summary>
/// 设置选项
/// </summary>
public enum ConfigOption
{
    /// <summary>自动播放行会语音</summary>
    AutoPlayGuildAudio = 1,
    /// <summary>自动播放团队语音</summary>
    AutoPlayTeamAudio = 2,
    /// <summary>自动播放私聊语音</summary>
    AutoPlayPrivateAudio = 3,
    /// <summary>自动播放附近语音</summary>
    AutoPlayFuJinAudio = 4,
    /// <summary>自动播放彩世语音</summary>
    AutoPlayColorWorldAudio = 5,
    /// <summary>自动播放世界频道语音</summary>
    AutoPlayWorldAudio = 6,
    /// <summary>展示世界频道聊天信息</summary>
    AutoPlayWorldText = 7,
    /// <summary>展示组队频道聊天信息</summary>
    AutoPlayTeamText = 8,
    /// <summary>展示帮会频道聊天信息</summary>
    AutoPlayGuildText = 9,
    /// <summary>展示附近频道聊天信息</summary>
    AutoPlayNearbyText = 10,
    /// <summary>展示私聊频道聊天信息</summary>
    AutoPlayPrivateText = 11,
    /// <summary>是否显示自身会员等级值</summary>
    MakeVipLevelVisible = 12,

    /// <summary>背景音乐开关</summary>
    BgMusic,
    /// <summary>背景音乐音量</summary>
    BgMusicSlider,
    /// <summary>技能音效开关</summary>
    EffectSound,
    /// <summary>技能音效音量</summary>
    EffectSoundSlider,
    /// <summary>游戏语音开关</summary>
    VoiceSound,
    /// <summary>游戏语音音量</summary>
    VoiceSoundSlider,
    /// <summary>固定摇杆</summary>
    FixJoystick,
    /// <summary>活动推送</summary>
    PushActivity,
    /// <summary>拒绝行会邀请</summary>
    ForbidGuild,
    /// <summary>拒绝好友邀请</summary>
    ForbidFriend,
    /// <summary>拒绝陌生人消息</summary>
    ForbidStranger,

    /// <summary>装备拾取总开关</summary>
    AllEquipPickUp,
    /// <summary>道具拾取总开关</summary>
    AllItemPickUp,
    /// <summary>本元装备拾取等级</summary>
    BYEquipPickUpLv,
    /// <summary>本元装备拾取等级开关</summary>
    BYEquipPickUpLvSwitch,
    /// <summary>本元装备拾取品质</summary>
    BYEquipPickUpQuality,
    /// <summary>本元装备拾取品质开关</summary>
    BYEquipPickUpQualitySwitch,
    /// <summary>卧龙装备拾取等级</summary>
    WLEquipPickUpLv,
    /// <summary>卧龙装备拾取等级开关</summary>
    WLEquipPickUpLvSwitch,
    /// <summary>货币拾取</summary>
    MoneyPickUp,
    /// <summary>锻造材料拾取</summary>
    ForgingMaterialsPickUp,
    /// <summary>时装碎片拾取</summary>
    FashionPiecesPickUp,
    /// <summary>药品拾取</summary>
    DrugsPickUp,
    /// <summary>其他拾取</summary>
    OthersPickUp,
    /// <summary>自动吃药开关</summary>
    AutoTakeDrugSwitch,
    /// <summary>自动吃药药品</summary>
    AutoTakeDrug,
    /// <summary>自动吃药生命值</summary>
    AutoTakeDrugHp,

    /// <summary>自动吃蓝药开关</summary>
    AutoTakeMpDrugSwitch,
    /// <summary>自动吃蓝药药品</summary>
    AutoTakeMpDrug,
    /// <summary>自动吃蓝药法力值</summary>
    AutoTakeDrugMp,

    /// <summary>自动瞬回药开关</summary>
    AutoInstantDrugSwitch,
    /// <summary>自动瞬回药品</summary>
    AutoInstantDrug,
    /// <summary>自动瞬回药生命值</summary>
    AutoInstantDrugHp,

    /// <summary>自动随机传送开关</summary>
    AutoRandomDeliverySwitch,
    /// <summary>自动随机传送生命值</summary>
    AutoRandomDeliveryHp,
    /// <summary>自动随机传送时间</summary>
    AutoRandomDeliveryTime,

    /// <summary>自动回城开关</summary>
    AutoGoBackSwitch,
    /// <summary>自动回城生命值</summary>
    AutoGoBackHp,
    /// <summary>自动回城时间</summary>
    AutoGoBackTime,

    /// <summary>自动攻击玩家等级</summary>
    AutoAttackPlayerLv,

    /// <summary>画面卡顿时弹出模式提示</summary>
    PopGraphicsModeTips,
    /// <summary>画面模式</summary>
    GraphicsMode,
    /// <summary>隐藏所有玩家</summary>
    HideAllPlayers,
    /// <summary>隐藏本行会玩家</summary>
    HideMyGuildPlayers,
    /// <summary>隐藏所有怪物</summary>
    HideMonsters,
    /// <summary>隐藏技能特效</summary>
    HideSkillEffect,
    /// <summary>隐藏道士召唤兽</summary>
    HideTaoistMonster,
    /// <summary>隐藏战魂</summary>
    HideWarPet,
    /// <summary>隐藏全部名称</summary>
    HideAllName,

    /// <summary>战士自动释放技能</summary>
    WarriorAutoReleaseSkill,
    /// <summary>法师自动释放技能</summary>
    MasterAutoReleaseSkill,
    /// <summary>道士自动释放技能</summary>
    TaoistAutoReleaseSkill,

    /// <summary> 智能半月弯刀 </summary>
    SmartHalfMoonMachetes,
    /// <summary> 自动释放魔法盾 </summary>
    AutoReleasMagicShield,
    /// <summary> 自动释放道士宠物 </summary>
    AutoReleasTaoistPet,
    /// <summary> 自动释放无极真气 </summary>
    AutoReleasInfinityQi,

    /// <summary> 单体技能 </summary>
    SkillSingle,
    /// <summary> 群体技能 </summary>
    SkillGroup,
}

public enum EShowOptionType
{
    /// <summary>正常</summary>
    Normal,
    /// <summary>流畅</summary>
    Fluency,
    /// <summary>极速</summary>
    TopSpeed,

    SpeedBest,
}

/// <summary>
/// 副本类型
/// </summary>
public enum ECopyType
{
    None = 0,
    /// <summary> 普通副本 </summary>
    Normal = 1,
    /// <summary> 幻境副本 </summary>
    Dreamland = 2,
    /// <summary> 世界boss </summary>
    WorldBoss = 3,
    /// <summary> 炼体之地</summary>
    DevilBody = 4,
    /// <summary> 极限挑战</summary>
    UltimateChallenge = 5,
    /// <summary> 地牢围攻 </summary>
    Dungeon = 7,
    /// <summary> 泉水泡点 </summary>
    SpringPaoDian = 10,
    /// <summary>个人Boss</summary>
    SelfBoss = 13,
    /// <summary>地下寻宝 </summary>
    UndergroundTreasure = 14,
    /// <summary>随机夺宝 </summary>
    RandomThing = 15,
    /// <summary>每日地图 </summary>
    DailyMap = 19,
    /// <summary>
    /// 荣耀boss挑战
    /// </summary>
    HonorBossChanllenge = 20,
    /// <summary>行会战 </summary>
    GuildCombat = 24,
    /// <summary>行会Boss </summary>
    GuildBoss = 25,
}

/// <summary>
/// 寻路自动状态
/// </summary>
public enum PathGuideState
{
    None,
    AutoMission,
    AutoFight,
    AutoYaBiao,
}

/// <summary>
/// 寻路结束，监听事件执行的操作
/// </summary>
public enum OnReachFindPathState
{
    None, //不做处理
    CommonFight,//普通战斗
    ReachNpc,//点击npc
}

public enum FunctionPromptType
{
    HasUnreadedMail = 10000, //新邮件
    FriendRequst, //好友请求
    TeamInaitation, //组队邀请
    PrivateChat, //私聊消息
    AuctionSell, //交易行
    FriendPrivateChat, //好友私聊气泡
    InviteUnion, //邀请入会
    EnrollmentApplication, //入队申请
    UpdateGame, //更新

    GuildCombat,//行会战
    GuildBoss,//行会首领

    TimeLimitActivity = 90000,//限时活动(特殊)
}

/// <summary>//公告位置</summary>
public enum NoticeType
{
    None,
    Top = 1, //顶部
    CenterTop = 2,  //中上
    Chat = 3,  //聊天框
    CenterTopAndChat = 4, //中上和聊天框同时显示
    Bottom = 5,  // 底部
    BottomAndChat = 6,//底部和聊天框同时显示
    CenterTopAndBotton = 7,//中上和底部同时显示
    Below = 8,//中下和聊天框同时显示
    TopAndChat = 9, //顶部和聊天框同时显示

    ColoursWorld = 10,//彩世（以公告形式存放）
}

/// <summary>
///  雷达显示类型
/// </summary>
public enum MapAvaterType
{
    None,
    Player,
    Monster,
    Npc,
    MapMonster,//小地图显示
}
/// <summary>
/// 小地图 点的显示类型
/// </summary>
public enum UIMiniMapType
{
    None,
    NPC,
    Monster,  //怪物
    WayPoint,       //传送点
    Player,         //玩家
    SpecialTeam,    //特殊显示-队伍成员
}
/// <summary>
/// 小地图  玩家点的显示类型
/// </summary>
public enum UIMiniMapPlayerType
{
    None,
    TeamPlayer, //队友
    OtherPlayer,//其他玩家
}
/// <summary>
/// 小地图怪物类型
/// </summary>
public enum UIMiniMapMonsterType
{
    None,
    Normal,//小怪
    Boss,//boss
    MapBoss,//地图Boss
}

//怪物类型
public class EMonsterType
{
    public const int Wall = 7;   //城门
}


/// <summary>
/// 画面模式
/// </summary>
public enum ShowOptionType
{
    /// <summary>正常</summary>
    Normal,
    /// <summary>流畅</summary>
    Fluency,
    /// <summary>极速</summary>
    TopSpeed,
}

/// <summary>
/// 道具类型
/// </summary>
public class BagItemType
{
    public const int All = 0;
    public const int Property = 1;        //all
    public const int Equip = 2;          //装备
    public const int Assist = 3;         //辅助
    public const int Drug = 4;           //药品
    public const int Else = 5;           //其他
    public const int Skill = 6;//技能
    public const int Material = 7;//材料
}

public class PickUpType
{
    public const int None = 0;               //无
    public const int Currency = 1;     //货币
    public const int Materials = 2;    //锻造材料 
    public const int RecoverDrug = 3;  //回复药品  
    public const int NormalEuip = 4;    //普通装备
    public const int WoLongEquip = 5;  //卧龙装备
    public const int Other = 6;        //其他
    public const int RecoverDrugMidle = 7;  //回复药品(中)
    public const int RecoverDrugMBig = 8;  //回复药品(大)
    public const int TaiYangShui = 9;       //太阳水
    public const int XueShen = 10;           //万年血参
    public const int XueLian = 11;          //雪莲
    public const int WoLongDrug = 12;       //卧龙神药
    public const int ZhuFuYou = 13;         //祝福油
}

public enum EHpChangeReason
{
    AutoRecover = 1,   //自动回复
    SkillDamage = 2,    //技能伤害
    Fusion = 3,         //药品回复
    BuffDamage = 4,     //持续性buff回复或伤害
    ReflectDamage = 5,  //反伤
    RemoveTunShiBUff = 6,  //吞噬buff移除
    GodBless = 7,           //神祝福
    NpcRecover = 8,     //npc一键恢复
}

public enum ItemType
{
    Money = 1,      //货币
    Equip = 2,      //装备
    Box = 3,        //宝箱
    Medicine = 4,   //药品
    Others = 5,     //其他
    SkillBook = 6,  //技能书
    Meterials = 7,  //材料
    Handbook = 8,   //图鉴
    Gem = 9,        //宝石
    NostalgicEquip = 10,//怀旧装备
}

public class PlayerPkMode
{
    /// <summary>
    /// 白名
    /// </summary>
    public const int White = 0;
    /// <summary>
    /// 黄名
    /// </summary>
    public const int Yellow = 1;
    /// <summary>
    /// 灰名
    /// </summary>
    public const int Gray = 2;
    /// <summary>
    /// 红名
    /// </summary>
    public const int Red = 3;
}

public enum EInstanceState
{
    /// <summary>
    /// 创建副本
    /// </summary>
    Create,
    /// <summary>
    /// 等待副本开启
    /// </summary>
    Wait,
    /// <summary>
    /// 副本开始
    /// </summary>
    Started,
    /// <summary>
    /// 副本结束
    /// </summary>
    Finished,
    /// <summary>
    /// 关闭副本
    /// </summary>
    Closed,
}

public class ESpecialMap
{
    /// <summary>
    /// 地下寻宝
    /// </summary>
    public const int DiXiaXunBao = 300002;
}


public enum AnchorType
{
    TopLeft = 11,
    TopCenter = 21,
    TopRight = 31,
    Left = 12,
    Center = 22,
    Right = 32,
    BottomLeft = 13,
    BottomCenter = 23,
    BottomRight = 33,
}
