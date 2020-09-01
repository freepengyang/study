using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.ExceptionServices;

public partial class UIActivitiesPanel : UIBasePanel
{
    //主页面显示的离开副本按钮
    private UIExitInstanceBtn _UIExitInstanceBtn;

    private bool isShowActivities = true;

    // vip图标使用
    private long randomTime = 0;
    private vip.ExperienceCardInfo experienceCardInfo;
    UILabel mlb_time;
    private GameObject FirstRechargeEffect;
    ILBetterList<int> instanceList = new ILBetterList<int>();
    /// <summary>
    /// 常驻永久按钮列表
    /// </summary>
    private Dictionary<GameObject, forEverBtnClass> dicForever = new Dictionary<GameObject, forEverBtnClass>();

    public bool IsShowActivities
    {
        get { return isShowActivities; }
        set
        {
            if (isShowActivities == value) return;
            isShowActivities = value;
        }
    }

    private int openSealLevel = 0;

    public override void Init()
    {
        base.Init();
        openSealLevel = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel((int) FunctionType.funcP_sealGrade);
        mClientEvent.AddEvent(CEvent.UI_RefreshUIMainButton, RefreshUIMainButtons);
        mClientEvent.AddEvent(CEvent.GetEnterInstanceInfo, MainShowExitInstance);
        mClientEvent.AddEvent(CEvent.Role_ChangeMapId, MainShowActivityIcon);
        mClientEvent.AddEvent(CEvent.FunctionOpenStateChange, FunctionOpenStateChange);
        mClientEvent.AddEvent(CEvent.FirstRechargeInfoChange, OnFirstRechargeChange);
        mClientEvent.AddEvent(CEvent.VipInfoChange, OnVipInfoChange);
        mClientEvent.AddEvent(CEvent.UICheckManagerInitCheckComplete, RefreshUIMainButtons);
        mClientEvent.AddEvent(CEvent.OpenSeal, OpenSeal);
        mClientEvent.AddEvent(CEvent.CloseSeal, CloseSeal);
        mClientEvent.AddEvent(CEvent.OpenDreamLand, OpenDreamLand);
        mClientEvent.AddEvent(CEvent.CloseDreamLand, CloseDreamLand);
        CSMainPlayerInfo.Instance.mClientEvent.AddEvent(CEvent.GetExp, MainPlayerExpChange);
        // mClientEvent.AddEvent(CEvent.MainPlayer_LevelChange, MainPlayerLevelChange);
        mClientEvent.AddEvent(CEvent.OnSabacStageChanged, OnSabacStageChanged);
        mClientEvent.AddEvent(CEvent.SeekTreasureBox, SeekTreasureEndTime);
        //绑定按钮的功能开放ID
        //FunctionIdBind();

        UIEventListener.Get(mBtn_retrue.gameObject).onClick = OnClickRetrue;
        UIEventListener.Get(mBtn_boss.gameObject).onClick = OnClickBoss;
        UIEventListener.Get(mbtn_seal_grade.gameObject).onClick = OnClickSealGrade;
        UIEventListener.Get(mbtn_dream_land.gameObject).onClick = OnClickDreamLand;
        //UIEventListener.Get(mbtn_shop.gameObject).onClick = OnClickShop;
        UIEventListener.Get(mbtn_activitiesCenter.gameObject).onClick = OnClickActivitiesCenter;
        UIEventListener.Get(mbtn_auction.gameObject).onClick = OnClickAuction;
        UIEventListener.Get(mbtn_TombTreasure.gameObject).onClick = OnClickTombTreasure;
        UIEventListener.Get(mbtn_guild_fight.gameObject).onClick = OnClickGuildFight;
        //SUIEventListener.Get(mbtn_daily).onClick = OnClickDaily;
        UIEventListener.Get(mbtn_seekTreasure.gameObject).onClick = OnClickSeekTreasure;
        UIEventListener.Get(mbtn_openServer.gameObject).onClick = OnClickOpenServer;
        UIEventListener.Get(mbtn_RechargeFirst).onClick = OnClickRechargeFirst;
        UIEventListener.Get(mbtn_wildAdventure).onClick = OnClickWildAdventure;
        UIEventListener.Get(mbtn_welfare).onClick = OnClickWelfare;
        UIEventListener.Get(mbtn_ArmRace).onClick = OnClickArmRace;
        UIEventListener.Get(mbtn_biqiShop).onClick = OnClickBiQiShop;
        UIEventListener.Get(mbtn_DailyArena).onClick = OnClickDailyArema;
        UIEventListener.Get(mbtn_MaFa).onClick = OnClickMaFa;
        UIEventListener.Get(mbtn_bestrong).onClick = OnClickBeStrong;

        mGuildFightStatus = Timer.Instance.InvokeRepeating(0.0f, 1.0f, GuildFightStatus);
        ScriptBinder.InvokeRepeating2(0.0f, 1.0f, SeekTreasureStatus);
        UIEventListener.Get(mbtn_SevenDay).onClick = OnClickSevenDay;
        //显示vip信息
        UIEventListener.Get(mbtn_vip).onClick = OnClickVip;
        GetInstanceList();
        OnVipInfoChange();

        mbtn_exit.SetActive(false);

        InitButtons();

        //判断是否在副本内,如果在副本内调用函数
        if (CSInstanceInfo.Instance.GetInstanceInfo() != null)
        {
            MainShowExitInstance();
        }

        CheckBtnReture();


        //生成常驻按钮
        RegisterForever();
        
        
        
    }
    /// <summary>
    /// 获得不需要退出按钮的副本id
    /// </summary>
    void GetInstanceList()
    {
        instanceList.Clear();
        var temp = SundryTableManager.Instance.GetSundryEffect(1134).Split('#');
        for (int i = 0; i < temp.Length; i++)
        {
            int id;
            if (int.TryParse(temp[i],out id))
             instanceList.Add(id);
        }


    }

    void ActivityButtonSorter(ref long sortValue, TableHandle r)
    {
        sortValue = (r.Value as TABLE.ACTIVITYBUTTON).order;
    }

    void InitButtons()
    {
        var handles = ActivityButtonTableManager.Instance.array.gItem.handles;
        //sort
        FastArrayElementKeepHandle<TableHandle> tempList = new FastArrayElementKeepHandle<TableHandle>(handles.Length);
        for (int i = 0, max = handles.Length; i < max; ++i)
        {
            tempList.Append(handles[i]);
        }

        tempList.Sort(ActivityButtonSorter);

        for (int i = 0, max = handles.Length; i < max; ++i)
        {
            var item = tempList[i].Value as TABLE.ACTIVITYBUTTON;
            var child = mpp.Find(item.name);
            if (null == child)
            {
                FNDebug.LogError($"child can not be found name = {item.name}");
                continue;
            }

            //setparent
            if (item.layer == 1)
                child.SetParent(mp1);
            else
                child.SetParent(mp2);

            //play resident effects
            if (child.childCount > 1 && item.residenteffect > 0)
                child.GetChild(1).gameObject.PlayEffect(item.residenteffect);

            //register function open
            if (item.functionId > 0)
                UICheckManager.Instance.RegBtnAndCheck((FunctionType) item.functionId, child.gameObject);
#if UNITY_EDITOR
            FNDebug.LogFormat("<color=#8f3dee>[功能开放]:[{0}]:[{1}]</color>", child.name, (FunctionType) item.functionId);
#endif

            for (int j = 0, mj = item.redpoints.Count; j < mj; ++j)
            {
                var point = item.redpoints[j];
                if (point > 0)
                {
                    var go = child.GetChild(0).gameObject;
                    RegisterRed(go, (RedPointType) point);
#if UNITY_EDITOR
                    FNDebug.LogFormat("<color=#3d8fee>[注册红点]:[{0}]:[{1}]</color>", child.name, (RedPointType) point);
#endif
                }
            }

            //注册常驻按钮 
            if (item.resident == 1)
            {
                dicForever.Add(child.gameObject,
                    new forEverBtnClass(item.residenteffect, (FunctionType) item.functionId, item.redpoints, item));
            }
        }

        tempList.Clear();
        tempList = null;
    }

    private void RegisterForever()
    {
        //copy 一份数据放入取消列表
        //给列表添加注册对应的参数Instantiate

        for (var it = dicForever.GetEnumerator(); it.MoveNext();)
        {
            var curObj = it.Current.Key;
            var forEverBtnClass = it.Current.Value;
            var clone = GameObject.Instantiate(curObj);
            if (forEverBtnClass.item.layer == 1)
                clone.transform.SetParent(mactivityHide);
            else
                clone.transform.SetParent(mh2);

            clone.SetActive(true);
            clone.transform.localScale = Vector3.one;
            var action = UIEventListener.Get(curObj).onClick;
            UIEventListener.Get(clone).onClick = action;
            RegisterRedList(clone.transform.GetChild(0).gameObject, forEverBtnClass._Redtype);

            UICheckManager.Instance.RegBtnAndCheck(forEverBtnClass._functionType, clone);
            if (forEverBtnClass.effectId != 0)
            {
                //需要预制体按钮第二个子物体放置effect节点，参考 btn_RechargeFirst
                if (clone.transform.childCount > 1)
                {
                    var effect = clone.transform.GetChild(1).gameObject;
                    CSEffectPlayMgr.Instance.ShowUIEffect(effect, forEverBtnClass.effectId);
                }
            }
        }
    }


    private void CheckBtnReture()
    {
        string levelShow = SundryTableManager.Instance.GetSundryEffect(1018);
        if (!string.IsNullOrEmpty(levelShow))
        {
            if (int.TryParse(levelShow, out int level))
            {
                if (CSMainPlayerInfo.Instance.Level < level)
                {
                    mBtn_retrue.CustomActive(false);
                    return;
                }
            }
        }

        mBtn_retrue.CustomActive(true);
    }

    private void OnVipInfoChange(uint uiEvtID = 0, object data = null)
    {
        //Debug.Log("OnVipInfoChange" + CSMainPlayerInfo.Instance.VipLevel);
        experienceCardInfo = CSVipInfo.Instance.ExperienceCardInfo;
        UtilityObj.Get<UILabel>(mbtn_vip.transform, "lb_viplv").text = CSMainPlayerInfo.Instance.VipLevel.ToString();
        ScriptBinder.InvokeRepeating(0, 1f, VipExperienceTimeDown);
    }

    private void VipExperienceTimeDown()
    {
        if (mlb_time == null)
        {
            mlb_time = UtilityObj.Get<UILabel>(mbtn_vip.transform, "lb_time");
        }

        if (experienceCardInfo != null)
        {
            mlb_time.gameObject.SetActive(true);
            randomTime = (experienceCardInfo.endTime - CSServerTime.DateTimeToStampForMilli(CSServerTime.Now)) / 1000;
            mlb_time.text = CSServerTime.Instance.FormatLongToTimeStr(randomTime, 3);
        }

        if (randomTime <= 0)
        {
            randomTime = 0;
            mlb_time.gameObject.SetActive(false);
            ScriptBinder.StopInvokeRepeating();
        }
    }


    private void OnFirstRechargeChange(uint uiEvtID, object data)
    {
        mbtn_RechargeFirst.SetActive(CSVipInfo.Instance.IsFirstRechargeFinish());
        if (CSVipInfo.Instance.IsFirstRechargeFinish() == false)
        {
            mClientEvent.SendEvent(CEvent.UI_RefreshUIMainButton);
        }
    }

    Schedule mGuildFightStatus;

    void GuildFightStatus(Schedule schedule)
    {
        if (null != mlb_status)
        {
            mlb_status.text = CSGuildFightManager.Instance.GetCityStageDesc();
        }
    }

    void SeekTreasureEndTime(uint id, object data)
    {
        SeekTreasureStatus();
    }

    void SeekTreasureStatus()
    {
        if (null != mlb_status_treasure)
            mlb_status_treasure.text = CSSeekTreasureInfo.Instance.GetTreasureEndTime();

        mlb_status_treasure.gameObject.SetActive(CSSeekTreasureInfo.Instance.EndTime > 0);
    }

    public override void Show()
    {
        base.Show();
        InitSealAndDreamButtons();
        RefreshUIMainButtons(0, null);
    }

    #region Icon Show Or Hide

    void InitSealAndDreamButtons()
    {
        mbtn_seal_grade.SetActive(false);
        mbtn_dream_land.SetActive(false);
        if (CSMainPlayerInfo.Instance.Level > openSealLevel)
        {
            mbtn_seal_grade.gameObject.SetActive(CSSealGradeInfo.Instance.MySealData != null &&
                                                 (CSMainPlayerInfo.Instance.Level >
                                                  CSSealGradeInfo.Instance.MySealLevel ||
                                                  (CSMainPlayerInfo.Instance.Level ==
                                                   CSSealGradeInfo.Instance.MySealLevel &&
                                                   CSMainPlayerInfo.Instance.Exp >=
                                                   CSMainPlayerInfo.Instance.GetCurLevelMaxExp())));
            mbtn_dream_land.gameObject.SetActive(CSDreamLandInfo.Instance.MyDreamLandData != null &&
                                                 CSSealGradeInfo.Instance.MySealData == null);
            RefreshUIMainButtons(0, null);
        }
    }

    void OpenSeal(uint id, object data)
    {
        if (CSMainPlayerInfo.Instance.Level > openSealLevel)
        {
            mbtn_seal_grade.gameObject.SetActive(CSSealGradeInfo.Instance.MySealData != null &&
                                                 (CSMainPlayerInfo.Instance.Level >
                                                  CSSealGradeInfo.Instance.MySealLevel ||
                                                  (CSMainPlayerInfo.Instance.Level ==
                                                   CSSealGradeInfo.Instance.MySealLevel &&
                                                   CSMainPlayerInfo.Instance.Exp >=
                                                   CSMainPlayerInfo.Instance.GetCurLevelMaxExp())));
            mbtn_dream_land.gameObject.SetActive(CSDreamLandInfo.Instance.MyDreamLandData != null &&
                                                 CSSealGradeInfo.Instance.MySealData == null);
            RefreshUIMainButtons(0, null);
        }
    }

    void CloseSeal(uint id, object data)
    {
        mbtn_seal_grade.gameObject.SetActive(false);
        mbtn_dream_land.gameObject.SetActive(CSDreamLandInfo.Instance.MyDreamLandData != null &&
                                             CSSealGradeInfo.Instance.MySealData == null);
        RefreshUIMainButtons(0, null);
    }


    void OpenDreamLand(uint id, object data)
    {
        mbtn_seal_grade.gameObject.SetActive(false);
        mbtn_dream_land.gameObject.SetActive(true);
        RefreshUIMainButtons(0, null);
    }

    void CloseDreamLand(uint id, object data)
    {
        mbtn_seal_grade.gameObject.SetActive(false);
        mbtn_dream_land.gameObject.SetActive(false);
        RefreshUIMainButtons(0, null);
    }

    void OnSabacStageChanged(uint id, object argv)
    {
        if (CSGuildFightManager.Instance.Stage == (int) CSGuildFightManager.GuildFightStatus.GFS_RUNNING)
        {
            mgo_sabac_open_effect.PlayEffect(17604);
        }
        else
        {
            mgo_sabac_open_effect.StopEffect();
        }
    }

    void MainPlayerExpChange(uint id, object data)
    {
        if(CSMainPlayerInfo.Instance.Level > openSealLevel)
        {
            mbtn_seal_grade.gameObject.SetActive(CSSealGradeInfo.Instance.MySealData != null &&
                                                 (CSMainPlayerInfo.Instance.Level >
                                                  CSSealGradeInfo.Instance.MySealLevel ||
                                                  (CSMainPlayerInfo.Instance.Level ==
                                                   CSSealGradeInfo.Instance.MySealLevel &&
                                                   CSMainPlayerInfo.Instance.Exp >=
                                                   CSMainPlayerInfo.Instance.GetCurLevelMaxExp())));
            mbtn_dream_land.gameObject.SetActive(CSDreamLandInfo.Instance.MyDreamLandData != null &&
                                                 CSSealGradeInfo.Instance.MySealData == null);
            RefreshUIMainButtons(0, null);
        }

        if (!mBtn_retrue.activeSelf)
            CheckBtnReture();
    }

    // void MainPlayerLevelChange(uint id, object data)
    // {
    //     if (CSMainPlayerInfo.Instance.Level > openSealLevel ||
    //         (CSMainPlayerInfo.Instance.Level == openSealLevel &&
    //          CSMainPlayerInfo.Instance.Exp >= CSMainPlayerInfo.Instance.GetCurLevelMaxExp()))
    //     {
    //         mbtn_seal_grade.gameObject.SetActive(CSSealGradeInfo.Instance.MySealData != null);
    //         if (mbtn_seal_grade.gameObject.activeSelf)
    //             mbtn_dream_land.gameObject.SetActive(false);
    //         else
    //             mbtn_dream_land.gameObject.SetActive(CSDreamLandInfo.Instance.MyDreamLandData != null);
    //
    //         RefreshUIMainButtons(0, null);
    //     }
    //
    //     if (!mBtn_retrue.activeSelf)
    //         CheckBtnReture();
    // }

    public void ShowActivities(bool value, bool isForce)
    {
        if (!IsShowActivities)
        {
            if (UIManager.Instance.IsPanel<UIRoleSelectionInfoPanel>())
                HotManager.Instance.EventHandler.SendEvent(CEvent.HideRoleSelectPanel);

            if (UIManager.Instance.IsPanel<UIMonsterSelectionInfoPanel>())
                HotManager.Instance.EventHandler.SendEvent(CEvent.HideMonsterSelectPanel);

            if (UIManager.Instance.IsPanel<UIMonsterSelectionDetailedInfoPanel>())
                UIManager.Instance.ClosePanel<UIMonsterSelectionDetailedInfoPanel>();
        }

        if (value)
        {
            if (!IsShowActivities)
            {
                if (isForce)
                {
                    ShowInstant(true);
                }
            }
        }
        else
        {
            if (IsShowActivities)
            {
                ShowInstant(false);
            }
        }
    }

    public void ShowInstant(bool value)
    {
        if (mPlayTween)
        {
            mPlayTween.Play(!value);
            IsShowActivities = value;
            if (!IsShowActivities)
            {
                if (UIManager.Instance.IsPanel<UIRoleSelectionInfoPanel>())
                    HotManager.Instance.EventHandler.SendEvent(CEvent.ShowRoleSelectPanel);

                if (UIManager.Instance.IsPanel<UIMonsterSelectionInfoPanel>())
                    HotManager.Instance.EventHandler.SendEvent(CEvent.ShowMonsterSelectPanel);
            }
        }
    }

    #endregion


    #region Event

    private void RefreshUIMainButtons(uint uiEvtID = 0, object data = null)
    {
        mgh1.repositionNow = true;
        mgh2.repositionNow = true;
        mgp1.repositionNow = true;
        mgp2.repositionNow = true;
    }

    private void MainShowExitInstance(uint id = 0, object data = null)
    {
        //Debug.Log("收到 进入副本");

        instance.InstanceInfo instanceinfo = data as instance.InstanceInfo;
        if (instanceinfo == null)
            return;

        if (instanceList.Contains(instanceinfo.instanceId))
            return;
        
        if (_UIExitInstanceBtn == null)
        {
            _UIExitInstanceBtn = new UIExitInstanceBtn();
            _UIExitInstanceBtn.UIPrefab = mbtn_exit;
            _UIExitInstanceBtn.Init();
        }

        _UIExitInstanceBtn.Show();
        //刷新按钮位置
        RefreshUIMainButtons();
        if (IsShowActivities)
            ShowActivities(false, true);
    }

    private void MainShowActivityIcon(uint id, object data)
    {
        uint type = MapInfoTableManager.Instance.GetMapInfoType(CSScene.GetMapID());
        if (type == 0)
            ShowActivities(true, true);
    }

    private void FunctionOpenStateChange(uint id, object data)
    {
        mgh1.repositionNow = true;
        mgh2.repositionNow = true;
        mgp1.repositionNow = true;
        mgp2.repositionNow = true;
    }

    #endregion


    #region Button

    private void OnClickRetrue(GameObject go)
    {
        ShowActivities(!IsShowActivities, true);
    }

    private void OnClickBoss(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIBossCombinePanel>(p => { (p as UIBossCombinePanel).SelectChildPanel(1); });
    }

    private void OnClickSealGrade(GameObject go)
    {
        UIManager.Instance.CreatePanel<UISealCombinedPanel>(f =>
        {
            (f as UISealCombinedPanel).SetActiveTogSealGrade(true);
            (f as UISealCombinedPanel).OpenChildPanel((int) UISealCombinedPanel.SealPanelTye.SealGrade);
        });
    }

    private void OnClickDreamLand(GameObject go)
    {
        UIManager.Instance.CreatePanel<UISealCombinedPanel>(f =>
        {
            (f as UISealCombinedPanel).SetActiveTogSealGrade(false);
            (f as UISealCombinedPanel).OpenChildPanel((int) UISealCombinedPanel.SealPanelTye.Dreamland);
        });
    }

    void OnClickShop(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIShopCombinePanel>();
    }

    void OnClickActivitiesCenter(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIActivityCombinedPanel>();
    }

    void OnClickAuction(GameObject go)
    {
        if (UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_Auction))
        {
            UIManager.Instance.CreatePanel<UIAuctionPanel>();
        }
    }

    void OnClickGuildFight(GameObject go)
    {
        CSGuildFightManager.Instance.OpenFightPanel();
    }

    void OnClickTombTreasure(GameObject go)
    {
        Net.CSFloorInfoMessage();
    }

    void OnClickDaily(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIDailyCombinedPanel>();
    }

    void OnClickSeekTreasure(GameObject go)
    {
        UIManager.Instance.CreatePanel<UISeekTreasureCombinedPanel>(f =>
            (f as UISeekTreasureCombinedPanel).OpenChildPanel(
                (int) UISeekTreasureCombinedPanel.ChildPanelType.CPT_SeekTreasure));
    }

    void OnClickOpenServer(GameObject go)
    {
        if (UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_OpenServerAc))
        {
            UIManager.Instance.CreatePanel<UIServerActivityPanel>();
        }
    }

    private void OnClickRechargeFirst(GameObject obj)
    {
        //UIManager.Instance.CreatePanel<UINostalgiaPromptPanel>();
        //UIManager.Instance.CreatePanel<UINostalgiaEquipPanel>();
        UIManager.Instance.CreatePanel<UIRechargeFirstPanel>();
    }

    private void OnClickVip(GameObject obj)
    {
        //UIManager.Instance.CreatePanel<UIPetLevelUpPanel>();
        UIManager.Instance.CreatePanel<UIVIPPanel>();
    }

    void OnClickWildAdventure(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIWildEscapadePanel>();
    }

    private void OnClickSevenDay(GameObject obj)
    {
        //Debug.Log("OnClickSevenDay");
        UIManager.Instance.CreatePanel<UISevenDayTrialPanel>();
        //UtilityPanel.JumpToPanel(11604);
    }

    void OnClickWelfare(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIWelfareActivityPanel>();
    }

    void OnClickArmRace(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIArmRacePanel>();
    }

    void OnClickBiQiShop(GameObject go)
    {
        UIManager.Instance.CreatePanel<UISpecialShopCombinePanel>();
    }

    void OnClickDailyArema(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIDailyArenaCombinePanel>();
    }

    void OnClickMaFa(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIMaFaPanel>();
    }

    void OnClickBeStrong(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIStrengthenCombinedPanel>();
    }

    #endregion

    protected override void OnDestroy()
    {
        CSMainPlayerInfo.Instance.mClientEvent.RemoveEvent(CEvent.GetExp, MainPlayerExpChange);
        if (null != mGuildFightStatus)
        {
            Timer.Instance.RemoveSchedule(mGuildFightStatus);
            mGuildFightStatus = null;
        }

        CSEffectPlayMgr.Instance.Recycle(FirstRechargeEffect);

        //mGrid_BtnActivity1.RemoveShowCountCallBack();

        if (_UIExitInstanceBtn != null)
            _UIExitInstanceBtn.Destroy();
        _UIExitInstanceBtn = null;
        base.OnDestroy();
    }
}

public class forEverBtnClass
{
    public RedPointType[] _Redtype;
    public int effectId;
    public FunctionType _functionType;
    public TABLE.ACTIVITYBUTTON item;


    public forEverBtnClass(int effectId, FunctionType functionType, IntArray pointRedtype, TABLE.ACTIVITYBUTTON item)
    {
        this.item = item;
        _Redtype = new RedPointType[pointRedtype.Length];
        for (int i = 0, max = pointRedtype.Length; i < max; ++i)
        {
            _Redtype[i] = (RedPointType) pointRedtype[i];
        }

        this.effectId = effectId;
        _functionType = functionType;
    }
}