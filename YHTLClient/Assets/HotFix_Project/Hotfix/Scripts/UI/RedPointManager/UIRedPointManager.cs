using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRedPointManager : CSInfo<UIRedPointManager>
{
    private Dictionary<int, bool> RedPointActivateDic = new Dictionary<int, bool>();

    public EventHanlderManager mRedEvent = new EventHanlderManager(EventHanlderManager.DispatchType.RedPoint); //小红点事件

    private readonly CSBetterList<IRedPointCheck> mRedPointCheckList = new CSBetterList<IRedPointCheck>();
    private readonly CSBetterList<Type> mRedPointRegisList = new CSBetterList<Type>();


    private Coroutine _coroutine;

    public void Init()
    {
        mClientEvent.AddEvent(CEvent.CloseAllRedPoint, CloseAllRedPoint);
        RegisterRedPointCheck();
        _coroutine = CoroutineManager.DoCoroutine(DispatchAllRedPointState());
    }

    /// <summary>
    /// 再此注册所有红点检测类
    /// </summary>
    private void RegisterRedPointCheck()
    {
        RegisterRedPointCheck(typeof(SkillRedPointCheck));
        RegisterRedPointCheck(typeof(PetSkillRedPointCheck));
        RegisterRedPointCheck(typeof(FriendRedPointCheck));
        RegisterRedPointCheck(typeof(HandBookUpgradeLevelRedPointCheck));
        RegisterRedPointCheck(typeof(HandBookUpgradeQualityRedPointCheck));
        RegisterRedPointCheck(typeof(HandBookSetupRedPointCheck));
        RegisterRedPointCheck(typeof(HandBookSetupedUpgradeLevelRedPointCheck));
        RegisterRedPointCheck(typeof(HandBookSetupedUpgradeQualityRedPointCheck));
        RegisterRedPointCheck(typeof(HandBookSlotUnlockRedPointCheck));
        RegisterRedPointCheck(typeof(TimeExpRedPointCheck));
        RegisterRedPointCheck(typeof(LianTiRedPointCheck));
        RegisterRedPointCheck(typeof(GuildPracticeRedPointCheck));
        RegisterRedPointCheck(typeof(GuildApplyListRedPointCheck));
        RegisterRedPointCheck(typeof(GuildListRedPointCheck));
        RegisterRedPointCheck(typeof(EnhanceBtnRedPointCheck));
        RegisterRedPointCheck(typeof(EnhanceIgnorePanelRedPointCheck));
        RegisterRedPointCheck(typeof(BossFirstKillRedPointCheck));
        RegisterRedPointCheck(typeof(SealCompetitionRedPointCheck));
        RegisterRedPointCheck(typeof(TombTreasureRedPointCheck));
        RegisterRedPointCheck(typeof(WildAdventureRedPointCheck));
        RegisterRedPointCheck(typeof(DayChargeRedPointCheck));
        RegisterRedPointCheck(typeof(ServerActivityRankRedPointCheck));
        RegisterRedPointCheck(typeof(SevenDayRedPointCheck));
        RegisterRedPointCheck(typeof(CombineRedPointCheck));
        RegisterRedPointCheck(typeof(VipPanelRedPointCheck));
        // RegisterRedPointCheck(typeof(PearlEvolutionRedPointCheck));
        // RegisterRedPointCheck(typeof(PearlSkillslotRedPointCheck));
        RegisterRedPointCheck(typeof(WingFunctionRedPointCheck));
        RegisterRedPointCheck(typeof(WingRedPointCheck));
        RegisterRedPointCheck(typeof(WingColorRedPointCheck));
        RegisterRedPointCheck(typeof(BossKuangHuanRedPointCheck));
        RegisterRedPointCheck(typeof(GemRedPointCheck));
        RegisterRedPointCheck(typeof(DailyBtnRedPointCheck));
        RegisterRedPointCheck(typeof(DayChargeMapRedPointCheck));
        RegisterRedPointCheck(typeof(ArmRaceRedPointCheck));
        RegisterRedPointCheck(typeof(MonsterSlayRedPointCheck));
        RegisterRedPointCheck(typeof(LifeTimeFundRedPointCheck));
        RegisterRedPointCheck(typeof(DreamRedPointCheck));
        RegisterRedPointCheck(typeof(SeekTreasureWarehouseRedPointCheck));
        RegisterRedPointCheck(typeof(FashionRedPointCheck));
        RegisterRedPointCheck(typeof(EquipCollectRedPointCheck));
        RegisterRedPointCheck(typeof(RechargeFirstRedPointCheck));
        RegisterRedPointCheck(typeof(WolongUpgradeRedPoint));
        RegisterRedPointCheck(typeof(EquipRecastRedPointCheck));
        RegisterRedPointCheck(typeof(EquipRefineRedpointCheck));
        RegisterRedPointCheck(typeof(PersonalBossRedPointCheck));
        RegisterRedPointCheck(typeof(VigorRedCheck));
        RegisterRedPointCheck(typeof(BagRedPointCheck));
        RegisterRedPointCheck(typeof(SignInRedPointCheck));
        RegisterRedPointCheck(typeof(SignInAchievementRedPointCheck));
        RegisterRedPointCheck(typeof(MonthCardRedPointCheck));
        RegisterRedPointCheck(typeof(AuctionSellRedpointCheck));
        RegisterRedPointCheck(typeof(DownloadGiftRedPointCheck));
        RegisterRedPointCheck(typeof(LongJiRefineRedCheck));
        RegisterRedPointCheck(typeof(LongLiRefineRedCheck));
        RegisterRedPointCheck(typeof(PetLevelUpRedPointCheck));
        RegisterRedPointCheck(typeof(PetTalentRedPointCheck));
        RegisterRedPointCheck(typeof(WarPetRefineRedPointCheck));
        RegisterRedPointCheck(typeof(AddUpRechargeRedPointCheck));
        RegisterRedPointCheck(typeof(DirectPurchaseGiftRedPointCheck));
        RegisterRedPointCheck(typeof(DirectPurchaseReceiveRedPointCheck));
        RegisterRedPointCheck(typeof(MonthCardMapRedPointCheck));
        RegisterRedPointCheck(typeof(DailyArenaRedCheck));
        RegisterRedPointCheck(typeof(DiscountGiftBagRedPointCheck));
        RegisterRedPointCheck(typeof(WearableWoLongEquipRedCheck));
		RegisterRedPointCheck(typeof(MaFaRedPointCheck));
        RegisterRedPointCheck(typeof(ExchangeShopRedPointCheck));
        RegisterRedPointCheck(typeof(MonthRechargeRedPointCheck));
        RegisterRedPointCheck(typeof(RechargeShopRedPointCheck));
        RegisterRedPointCheck(typeof(GemLevelUpRedPointCheck));
        RegisterRedPointCheck(typeof(FashionActiveRedPointCheck));
        RegisterRedPointCheck(typeof(FashionUpStarRedPointCheck));
        RegisterRedPointCheck(typeof(ZhuFuRedCheck));
        RegisterRedPointCheck(typeof(SevenLoginRedPointCheck));
        RegisterRedPointCheck(typeof(NostalgiaRedPointCheck));
        RegisterRedPointCheck(typeof(WingSpiritRedPointCheck));
		RegisterRedPointCheck(typeof(PerEquipRewardsPointCheck));
		RegisterRedPointCheck(typeof(EquipRewardsPointCheck));
	}

    public IEnumerator DispatchAllRedPointState()
    {
        mRedEvent.SendEvent(RedPointType.Init);

        int count = 0;
        for (int i = 0; i < mRedPointRegisList.Count; i++)
        {
            try
            {
                IRedPointCheck redPointCheck = Activator.CreateInstance(mRedPointRegisList[i]) as IRedPointCheck;
                if (redPointCheck != null)
                {
                    mRedPointCheckList.Add(redPointCheck);
                    redPointCheck.Init(mClientEvent);
                    redPointCheck.LoginOrFuncRedCheck();
                }
            }
            catch (Exception e)
            {
                //FNDebug.LogError(e);
                FNDebug.LogError("UIRedpointManager  DispatchAllRedPointState error" + e.StackTrace);
            }

            if (count < 5)
            {
                count++;
            }
            else
            {
                count = 0;
                yield return null;
            }
        }

        mRedPointRegisList.Clear();
        DispatchMemoryLow();
    }


    #region 红点刷新

    public void RefreshRedPoint(RedPointType type, bool bact)
    {
        bool bol = GetRedPointState(type);
        if (bol == bact) return;
        SetRedPointState(type, bact);
        mRedEvent.SendEvent(type);
    }

    public bool GetRedPointState(RedPointType type)
    {
        return GetRedPointState((int)type);
    }

    public bool GetRedPointState(int id)
    {
        if (RedPointActivateDic == null) return false;
        if (RedPointActivateDic.ContainsKey(id))
        {
            return RedPointActivateDic[id];
        }
        else
            return false;
    }

    public void SetRedPointState(RedPointType type, bool bact)
    {
        if (RedPointActivateDic == null) return;
        if (RedPointActivateDic.ContainsKey((int)type))
        {
            RedPointActivateDic[(int)type] = bact;
        }
        else
        {
            RedPointActivateDic.Add((int)type, bact);
        }
    }

    #endregion

    #region FPS,设置提示

    private readonly uint mMemoryCheckSec = 1; //s 
    private readonly uint mConfigCheckSec = 2; //s 

    private float mLastTime = 0; //FPS低于指定值 ， 持续秒数
    private bool isSettingCount = false;

    private float configCheckCountDown = 0;

    private float memoryCheckCountDown = 0;

    // fps 检测调用同步 FPS.Instance.UpdateInterval
    public void UpdateByFPS(float fps)
    {
        UpdateSettingFPS(fps);
        UpdateCheckMemoryLow();
    }

    public void UpdateSettingFPS(float fps)
    {
        if (configCheckCountDown <= mConfigCheckSec)
        {
            configCheckCountDown += Constant.UpdateInterval;
        }
        else
        {
            DispatchSettingFPS(fps);
            configCheckCountDown = 0;
        }
    }

    //0.5s调用一次
    public void DispatchSettingFPS(float fps)
    {
        ////执行条件1：没有被关闭过
        //if (!CSMapAreaDialogTips.Instance.typeList.Contains(RedPointType.MainSetting) || mPlayerInfo == null || mPlayerInfo.ServerType == ServerType.ServerMoba) return;      //防止频繁执行

        ////执行条件2：玩家是否勾选
        //if (!CSConfigInfo.Instance.GetBool(ConfigOption.PopupModeTips)) return;

        ////执行条件3：设置非流畅、极速状态
        //int v = CSConfigInfo.Instance.GetInt(ConfigOption.ShowOption);
        //if (v >= (int)ShowOptionType.Fluency)    //流畅、极速
        //{
        //    return;
        //}

        //if (configFPS == 0 || mPlayerInfo.Level < limitLevel) return;

        ////执行条件4：在活动内并在活动地图
        //ActiveData activeData = CSActivityInfo.Instance.GetCurActivity();
        //if (activeData == null || !activeData.IsInActivityMap())
        //{
        //    return;
        //}

        //if (fps < configFPS && !GetRedPointState(RedPointType.MainSetting))    //FPS.Instance.Fps >= configFPS &&
        //{
        //    isSettingCount = true;
        //}
        //else if (GetRedPointState(RedPointType.MainSetting) || fps >= configFPS)
        //{
        //    RestSettingFPS();
        //}

        //if (isSettingCount)
        //{
        //    mLastTime += Constant.UpdateInterval;         //2*0.5
        //    if (configWaitTime > 0 && mLastTime >= configWaitTime)
        //    {
        //        RefreshRedPoint(RedPointType.MainSetting, true);
        //        RestSettingFPS();
        //    }
        //}
    }

    private void RestSettingFPS()
    {
        if (mLastTime > 0) mLastTime = 0;
        if (isSettingCount) isSettingCount = false;
    }

    public void DispatchMemoryLow()
    {
        if (CSDynamicChangeSetting.Instance != null)
        {
            RefreshRedPoint(RedPointType.MemoryWarring, CSDynamicChangeSetting.Instance.IsLowMemory);
        }
    }

    private void UpdateCheckMemoryLow()
    {
        if (memoryCheckCountDown <= mMemoryCheckSec)
        {
            memoryCheckCountDown += Constant.UpdateInterval;
        }
        else
        {
            DispatchMemoryLow();
            memoryCheckCountDown = 0;
        }
    }

    #endregion

    private void CloseAllRedPoint(uint id, object data)
    {
        CSRedPointManager.Instance.CloseAllRedPoint();
    }

    private void RegisterRedPointCheck(Type check)
    {
        if (check == null) return;

        if (!mRedPointRegisList.Contains(check))
        {
            mRedPointRegisList.Add(check);
        }
    }

    public override void Dispose()
    {
        if (mClientEvent != null) mClientEvent.UnRegAll();
        if (mRedEvent != null) mRedEvent.UnRegAll();
        RedPointActivateDic?.Clear();
        if (_coroutine != null)
            CoroutineManager.StopCoroutine(_coroutine);
        mRedEvent = null;
        RedPointActivateDic = null;
    }
}