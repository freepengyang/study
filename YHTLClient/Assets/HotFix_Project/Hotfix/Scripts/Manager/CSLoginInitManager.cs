using System.Collections;
public class CSLoginInitManager : CSInfo<CSLoginInitManager>
{
    private bool isInit;

    public IEnumerator Init()
    {
        if (isInit)
        {
            ChangeSceneCheck();
            yield break;
        }
        isInit = true;
        yield return InitMethod();
        yield return InitManager();
        yield return InitNetMsg();

        var playerInfo = CSMainPlayerInfo.Instance.GetMyInfo();
        CSGuideManager.Instance.Initialize(playerInfo.newbieGuide);
    }

    private IEnumerator Enumerator(System.Action action, string desc)
    {
#if UNITY_EDITOR
        action?.Invoke();
#else
        action?.Invoke();
#endif
        yield return null;
    }

    void RequestUnionMessage()
    {
        Net.CSGetUnionInfoMessage();//请求公会信息
        if (CSMainPlayerInfo.Instance.GuildPos < (int)GuildPos.VicePresident)
        {
            //会长 副会长需要申请 公会申请列表
            Net.CSGetUnionTabMessage((int)UnionTab.UnionApplyInfos); //请求申请列表
        }
        if (CSMainPlayerInfo.Instance.GuildPos == (int)GuildPos.President)
        {
            Net.CSGetUnionTabMessage((int)UnionTab.UnionsList); //请求公会列表 处理合并红点
        }
    }

    private IEnumerator InitNetMsg()
    {
        yield return Enumerator(Net.ReqGetMailListMessage, @"ReqGetMailListMessage");//请求邮件列表
        yield return Enumerator(Net.CSGetWoLongInfoMessage, @"CSGetWoLongInfoMessage");//请求卧龙数据
        yield return Enumerator(Net.ReqGetTaskListMessage, @"ReqGetTaskListMessage");//请求任务数据
        yield return Enumerator(RequestUnionMessage, @"CSRequestUnionMessage");//请求公会信息
        yield return Enumerator(Net.CSImproveInfosMessage, @"CSImproveInfosMessage");//请求修炼数据
		yield return Enumerator(Net.CSDayChargeInfoMessage, @"CSDayChargeInfoMessage");//请求每日充值信息
        
        yield return Enumerator(Net.CSPetInfoMessage, @"CSPetInfoMessage");//请求宠物升级数据
        yield return Enumerator(Net.CSDailyRmbInfoMessage, @"CSDailyRmbInfoMessage");//请求充值商城
        
        
		if (UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_vigor)) 
        {
            yield return Enumerator(Net.CSEnergyInfoMessage, @"CSEnergyInfoMessage");//精力值
        }
        yield return Enumerator(UIItemManager.Instance.Init, @"UIItemManager.Instance.Init");//通用格子
        //yield return Enumerator(Net.ReqSignInfoMessage, @"ReqSignInfoMessage");//请求签到数据
        yield return Enumerator(CSDailyArenaInfo.Instance.Init, @"CSDailyArenaInfo.Instance.Init");//请求签到数据
    }

    private IEnumerator InitManager()
    {
        var playerInfo = CSMainPlayerInfo.Instance.GetMyInfo();
        yield return Enumerator(() =>
        {
            CSBagInfo.Instance.InitBagData(playerInfo.bag);
        }, @"CSBagInfo.InitBagData");//背包初始化

        yield return Enumerator(CSOpenServerACInfo.Instance.ServerACInfoInit, @"ServerACInfoInit");//开服信息;
        yield return Enumerator(CSArmRaceInfo.Instance.Init, @"CSArmRaceInfo.Init");//军备竞赛
        yield return Enumerator(CSNewFunctionUnlockManager.Instance.Initialize, @"CSNewFunctionUnlockManager.Initialize");//新功能解锁
        //yield return Enumerator(() =>
        //{
        //    CSGuideManager.Instance.Initialize(playerInfo.newbieGuide);
        //}, @"CSGuideManager.Initialize");//引导管理器初始化
        yield return Enumerator(CSItemRecycleInfo.Instance.Initialize, @"CSItemRecycleInfo.Initialize");//装备回收初始化
        yield return Enumerator(() =>
        {
            CSItemCountManager.Instance.Initialize(playerInfo.bag);
        }, @"CSItemCountManager.Initialize");//背包计数管理器初始化



        yield return Enumerator(() =>
        {
            CSSkillInfo.Instance.Initialize(playerInfo);
        }, @"CSSkillInfo.Initialize");//技能初始化

        yield return Enumerator(() =>
        {
            CSSkillInfo.Instance.InitializePet(playerInfo);
        }, @"CSSkillInfo.Initialize");//战宠技能初始化

        yield return Enumerator(CSRechargeInfo.Instance.Init, @"CSRechargeInfo.Init");//商城充值
        yield return Enumerator(CSDownloadGiftInfo.Instance.Initialize, @"CSDownloadGiftInfo.Initialize");//下载有礼

        yield return Enumerator(FunctionPromptManager.Instance.Init, @"FunctionPromptManager.Init");//气泡消息注册
        yield return Enumerator(UICheckManager.Instance.Init, @"UICheckManager.Init");
		yield return Enumerator(CSGiveMeIngotInfo.Instance.ShowIngot, @"CSGiveMeIngotInfo.ShowIngot");
		yield return Enumerator(()=> { CSMainFuncManager.Instance.Init(-1); }, @"CSMainFuncManager.Init");
        yield return Enumerator(CSNoticeManager.Instance.ShowAllNotice, @"CSNoticeManager.Init");        //检测公告显示
        yield return Enumerator(CSInstanceInfo.Instance.Init, @"CSInstanceInfo.Init");
        yield return Enumerator(CSGuildInfo.Instance.Initialize, @"CSGuildInfo.Init");
        yield return Enumerator(CSDropSystem.Instance.Init, @"CSDropSystem.Init");
        yield return Enumerator(CSGuildFightManager.Instance.Initialize, @"CSGuildFightManager.Init");    //拉取公会争霸信息
        yield return Enumerator(CSVigorInfo.Instance.Init, @"CSVigorInfo.Init");
        yield return Enumerator(VoiceChatManager.Instance.Init, @"VoiceChatManager.Init");
        yield return Enumerator(UIAuctionInfo.Instance.Init, @"UIAuctionInfo.Init");
        yield return Enumerator(CSExchangeShopInfo.Instance.Init, @"CSExchangeShopInfo.Init");
        yield return Enumerator(CSSignCardInfo.Instance.Initialize, @"CSSignCardInfo.Initialize");
        yield return Enumerator(CSGuildActivityInfo.Instance.Initialize, @"CSGuildActivityInfo.Initialize");//行会活动
        yield return Enumerator(CSActivityRemindInfo.Instance.Initialize, @"CSActivityRemindInfo.Initialize");//限时活动提醒

        CSMissionEffectManager.Instance.Init();

        //红点初始化尽可能放到所有系统之后 防止出现时序问题
        yield return Enumerator(UIRedPointManager.Instance.Init, @"UIRedPointManager.Init");
        //触发登录成功流程   (永远在协程最后一个处理)
        yield return Enumerator(() =>
        {
            OnceEventTrigger.Instance.Trigger(OnceEvent.OnLogginTrigger);
        }, @"OnceEventTrigger.Instance.Trigger(OnceEvent.OnLogginTrigger)");
    }

    private IEnumerator InitMethod()
    {
        UIManager.Instance.CreatePanel<UIChatPanel>(f => { (f as UIChatPanel).Show(ChatType.CT_COMPREHENSIVE); });
        //UIManager.Instance.CreatePanel<UIChatPanel>(f => { (f as UIChatPanel).Show(ChatType.CT_COMPREHENSIVE); });
        yield return null;
    }

    /// <summary>
    /// 每次切换场景需要判断的方法
    /// </summary>
    private void ChangeSceneCheck()
    {
        CSMainFuncManager.Instance.Init(-1,true);
        OnceEventTrigger.Instance.Trigger(OnceEvent.AutoFightTrigger);

        if(CSMainFuncManager.Instance.Mode == CSMainFuncManager.MT_MODE)
        {
            int mapId = CSMainPlayerInfo.Instance.MapID;
            int type = InstanceTableManager.Instance.GetInstanceType(mapId);
            if (type == (int)ECopyType.WorldBoss)
            {
                var blessInfo = CSInstanceInfo.Instance.GetWorldBossBlessInfo();
                UIManager.Instance.CreatePanel<UIWorldInspirePanel>(p =>
                {
                    (p as UIWorldInspirePanel).SetData(blessInfo.goldTimes, blessInfo.yuanbaoTimes);
                });
            }
        }
    }


    public override void Dispose()
    {
        isInit = false;
        
    }
}