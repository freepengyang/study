using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIConfigHangUpPanel : UIBasePanel
{
    private CSConfigInfo mConfigInfo;

    ILBetterList<UIConfigPropPickUpBinder> propToggles = new ILBetterList<UIConfigPropPickUpBinder>();

    CSPopList mBYEquipLvFilter;
    CSPopList mBYEquipQualityFilter;
    CSPopList mWLEquipLvFilter;
    CSPopList mAutoDrugFilter;
    CSPopList mAttackLvFilter;
    CSPopList mAutoMpFilter;
    CSPopList mAutoInstantFilter;
    CSPopList mReturnTimeFilter;
    CSPopList mTransferTimeFilter;

    CSPopList mSkillSingleFilter;
    CSPopList mSkillGroupFilter;

    ILBetterList<int> drugList;
    ILBetterList<int> mpList;
    ILBetterList<int> instantList;

    string takeDrugByOrderStr;

    List<ConfigAutoReleaseSkill> skills;

    List<int> skillSingleIds = new List<int>();
    List<string> skillSingleNames = new List<string>();

    List<int> skillGroupIds = new List<int>();
    List<string> skillGroupNames = new List<string>();


    string autoHpStr = "";
    string autoMpStr = "";
    string autoInstantStr = "";
    string autoReturnStr = "";
    string autoTransferStr = "";


    public void SetGo(GameObject _go)
    {
        UIPrefab = _go;
    }

    public override void Init()
    {
        base.Init();

        mConfigInfo = CSConfigInfo.Instance;

        takeDrugByOrderStr = ClientTipsTableManager.Instance.GetClientTipsContext(1895);
        autoHpStr = ClientTipsTableManager.Instance.GetClientTipsContext(1258);
        autoMpStr = ClientTipsTableManager.Instance.GetClientTipsContext(1259);
        autoInstantStr = ClientTipsTableManager.Instance.GetClientTipsContext(2042);
        autoReturnStr = ClientTipsTableManager.Instance.GetClientTipsContext(2043);
        autoTransferStr = ClientTipsTableManager.Instance.GetClientTipsContext(2043);
        InitDeliveryAndReturnItemName();

        //AddToggleEvent(mtg_PickupEquip, ConfigOption.AllEquipPickUp);
        //AddToggleEvent(mtg_PickUpItem, ConfigOption.AllItemPickUp);
        EventDelegate.Add(mtg_PickupEquip.onChange, AllEquipPickUpToggleEvent);
        UIEventListener.Get(mtg_BYEquipLv.gameObject, ConfigOption.BYEquipPickUpLvSwitch).onClick = EquipPickUpDetailsSwitchClick;
        UIEventListener.Get(mtg_BYEquipQuality.gameObject, ConfigOption.BYEquipPickUpQualitySwitch).onClick = EquipPickUpDetailsSwitchClick;
        UIEventListener.Get(mtg_WLEquipLv.gameObject, ConfigOption.WLEquipPickUpLvSwitch).onClick = EquipPickUpDetailsSwitchClick;
        EventDelegate.Add(mtg_PickUpItem.onChange, AllItemPickUpToggleEvent);

        UIEventListener.Get(mobj_closePopList, 0).onClick = ClosePopList;

        AddToggleEvent(mtg_autoDrug, ConfigOption.AutoTakeDrugSwitch);
        AddToggleEvent(mtg_autoMp, ConfigOption.AutoTakeMpDrugSwitch);
        AddToggleEvent(mtg_autoInstant, ConfigOption.AutoInstantDrugSwitch);
        AddToggleEvent(mtg_autoGoBack, ConfigOption.AutoGoBackSwitch);
        AddToggleEvent(mtg_autoRandom, ConfigOption.AutoRandomDeliverySwitch);

        AddSliderEvent(mslider_autoEatHp, ConfigOption.AutoTakeDrugHp);
        AddSliderEvent(mslider_autoEatMp, ConfigOption.AutoTakeDrugMp);
        AddSliderEvent(mslider_autoInstant, ConfigOption.AutoInstantDrugHp);
        AddSliderEvent(mslider_autoGoBack, ConfigOption.AutoGoBackHp);
        AddSliderEvent(mslider_autoRandom, ConfigOption.AutoRandomDeliveryHp);

        InitPropPickUpToggles();

        InitFilters();

        InitAutoReleaseSkill();



        mscroll.SetDynamicArrowVertical(mobj_arrow);
    }


    void InitDeliveryAndReturnItemName()
    {
        string idstr = SundryTableManager.Instance.GetSundryEffect(1144);
        int cfgId = 0;
        if (int.TryParse(idstr, out cfgId))
        {
            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(cfgId);
            if (cfg != null)
            {
                var color = UtilityColor.GetColorTypeByQuality(cfg.quality);
                //mlb_goBackItem.text = cfg.name.BBCode(color);
                autoReturnStr = $"{autoReturnStr}{cfg.name.BBCode(color)}";
            }
        }

        idstr = SundryTableManager.Instance.GetSundryEffect(1143);
        if (int.TryParse(idstr, out cfgId))
        {
            TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(cfgId);
            if (cfg != null)
            {
                var color = UtilityColor.GetColorTypeByQuality(cfg.quality);
                //mlb_randomItem.text = cfg.name.BBCode(color);
                autoTransferStr = $"{autoTransferStr}{cfg.name.BBCode(color)}";
            }
        }
    }


    public override void Show()
    {
        base.Show();

        RefreshUI();
    }


    void RefreshUI()
    {
        RefreshToggleUI(mtg_PickupEquip, ConfigOption.AllEquipPickUp);
        RefreshToggleUI(mtg_PickUpItem, ConfigOption.AllItemPickUp);
        RefreshToggleUI(mtg_autoDrug, ConfigOption.AutoTakeDrugSwitch);
        RefreshToggleUI(mtg_autoMp, ConfigOption.AutoTakeMpDrugSwitch);
        RefreshToggleUI(mtg_autoInstant, ConfigOption.AutoInstantDrugSwitch);
        RefreshToggleUI(mtg_autoGoBack, ConfigOption.AutoGoBackSwitch);
        RefreshToggleUI(mtg_autoRandom, ConfigOption.AutoRandomDeliverySwitch);

        RefreshEquipPickUpUI();
        RefreshItemPickUpUI();

        RefreshSliderUI(mslider_autoEatHp, ConfigOption.AutoTakeDrugHp);
        RefreshSliderUI(mslider_autoEatMp, ConfigOption.AutoTakeDrugMp);
        RefreshSliderUI(mslider_autoInstant, ConfigOption.AutoInstantDrugHp);
        RefreshSliderUI(mslider_autoGoBack, ConfigOption.AutoGoBackHp);
        RefreshSliderUI(mslider_autoRandom, ConfigOption.AutoRandomDeliveryHp);

        RefreshFilters();

        RefreshSkillGrid();
    }


    void InitPropPickUpToggles()
    {
        propToggles?.Clear();
        int maxCount = mConfigInfo.PropPickUpIdCount;
        mGrid_PropPickUp.MaxCount = maxCount;
        for (int i = 0; i < mGrid_PropPickUp.MaxCount; i++)
        {
            UIConfigPropPickUpBinder binder = new UIConfigPropPickUpBinder();
            var handle = UIEventListener.Get(mGrid_PropPickUp.controlList[i]);
            binder.Setup(handle);
            propToggles.Add(binder);
        }
    }

    /// <summary>
    /// 装备拾取总开关
    /// </summary>
    void AllEquipPickUpToggleEvent()
    {
        mConfigInfo.SetBool(ConfigOption.AllEquipPickUp, mtg_PickupEquip.value);
        RefreshEquipPickUpUI();
    }


    void RefreshEquipPickUpUI()
    {
        bool allEnabled = mConfigInfo.GetBool(ConfigOption.AllEquipPickUp);
        Color color = UtilityColor.GetColor(allEnabled ? ColorType.SubTitleColor : ColorType.WeakText);

        mtg_BYEquipLv.value = mConfigInfo.GetEquipPickUpBool(ConfigOption.BYEquipPickUpLvSwitch);
        mtg_BYEquipLv.GetComponent<BoxCollider>().enabled = allEnabled;
        mlb_BYEquipLv.color = color;

        mtg_BYEquipQuality.value = mConfigInfo.GetEquipPickUpBool(ConfigOption.BYEquipPickUpQualitySwitch);
        mtg_BYEquipQuality.GetComponent<BoxCollider>().enabled = allEnabled;
        mlb_BYEquipQuality.color = color;

        mtg_WLEquipLv.value = mConfigInfo.GetEquipPickUpBool(ConfigOption.WLEquipPickUpLvSwitch);
        mtg_WLEquipLv.GetComponent<BoxCollider>().enabled = allEnabled;
        mlb_WLEquipLv.color = color;
    }


    void EquipPickUpDetailsSwitchClick(GameObject go)
    {
        ConfigOption param = (ConfigOption)UIEventListener.Get(go).parameter;
        switch (param)
        {
            case ConfigOption.BYEquipPickUpLvSwitch:
                mConfigInfo.SetBool(param, mtg_BYEquipLv.value);
                break;
            case ConfigOption.BYEquipPickUpQualitySwitch:
                mConfigInfo.SetBool(param, mtg_BYEquipQuality.value);
                break;
            case ConfigOption.WLEquipPickUpLvSwitch:
                mConfigInfo.SetBool(param, mtg_WLEquipLv.value);
                break;
        }
    }


    /// <summary>
    /// 道具拾取总开关
    /// </summary>
    void AllItemPickUpToggleEvent()
    {
        mConfigInfo.SetBool(ConfigOption.AllItemPickUp, mtg_PickUpItem.value);
        RefreshItemPickUpUI();
    }

    void RefreshItemPickUpUI()
    {
        #region 旧代码
        //bool allEnabled = mConfigInfo.GetBool(ConfigOption.AllItemPickUp);

        //mtg_PickupGold.value = mConfigInfo.GetItemPickUpBool(ConfigOption.MoneyPickUp);
        //UpdateToggleEnbled(mtg_PickupGold, allEnabled);

        //mtg_PickupMaterial.value = mConfigInfo.GetItemPickUpBool(ConfigOption.ForgingMaterialsPickUp);
        //UpdateToggleEnbled(mtg_PickupMaterial, allEnabled);

        //mtg_PickupCloth.value = mConfigInfo.GetItemPickUpBool(ConfigOption.FashionPiecesPickUp);
        //UpdateToggleEnbled(mtg_PickupCloth, allEnabled);

        //mtg_PickUpDrug.value = mConfigInfo.GetItemPickUpBool(ConfigOption.DrugsPickUp);
        //UpdateToggleEnbled(mtg_PickUpDrug, allEnabled);

        //mtg_PickUpOther.value = mConfigInfo.GetItemPickUpBool(ConfigOption.OthersPickUp);
        //UpdateToggleEnbled(mtg_PickUpOther, allEnabled);
        #endregion

        if (propToggles != null && propToggles.Count == mConfigInfo.PropPickUpIdCount)
        {
            for (int i = 0; i < propToggles.Count; i++)
            {
                propToggles[i].Bind(i);
            }
        }


    }


    void AddToggleEvent(UIToggle toggle, ConfigOption configOption)
    {
        EventDelegate.Add(toggle.onChange, () =>
        {
            mConfigInfo.SetBool(configOption, toggle.value);
        });
    }

    void AddSliderEvent(UISlider slider, ConfigOption configOption)
    {
        EventDelegate.Add(slider.onChange, () =>
        {
            mConfigInfo.SetFloat(configOption, slider.value);
            if (configOption == ConfigOption.AutoTakeDrugHp)
            {
                mlb_autoTakeDrugHp.text = CSString.Format(autoHpStr, Mathf.RoundToInt(slider.value * 100));
            }
            if (configOption == ConfigOption.AutoTakeDrugMp)
            {
                mlb_takeMp.text = CSString.Format(autoMpStr, Mathf.RoundToInt(slider.value * 100));
            }
            if (configOption == ConfigOption.AutoInstantDrugHp)
            {
                mlb_autoInstant.text = CSString.Format(autoInstantStr, Mathf.RoundToInt(slider.value * 100));
            }
            if (configOption == ConfigOption.AutoGoBackHp)
            {
                mlb_autoGoBack.text = CSString.Format(autoReturnStr, Mathf.RoundToInt(slider.value * 100));
            }
            if (configOption == ConfigOption.AutoRandomDeliveryHp)
            {
                mlb_autoRandom.text = CSString.Format(autoTransferStr, Mathf.RoundToInt(slider.value * 100));
            }
        });
    }


    void RefreshToggleUI(UIToggle toggle, ConfigOption configOption)
    {
        toggle.value = mConfigInfo.GetBool(configOption);
    }

    void RefreshSliderUI(UISlider slider, ConfigOption configOption)
    {
        slider.value = mConfigInfo.GetFloat(configOption) / 100f;
    }

    void UpdateToggleEnbled(UIToggle toggle, bool isEnbled)
    {
        if (toggle == null) return;
        toggle.GetComponent<BoxCollider>().enabled = isEnbled;
        UILabel label = toggle.transform.Find("Label").GetComponent<UILabel>();
        Color color = UtilityColor.GetColor(isEnbled ? ColorType.SubTitleColor : ColorType.WeakText);
        label.color = color;
    }

    


    void InitFilters()
    {
        mBYEquipLvFilter = new CSPopList(mpoplist_benYuanLv, mPoolHandleManager);
        mBYEquipQualityFilter = new CSPopList(mpoplist_benYuanQuality, mPoolHandleManager);
        mWLEquipLvFilter = new CSPopList(mpoplist_wolongLv, mPoolHandleManager);
        mAutoDrugFilter = new CSPopList(mpoplist_DrugLevel, mPoolHandleManager);
        mAttackLvFilter = new CSPopList(mpoplist_SelectRoleLevel, mPoolHandleManager);
        mAutoMpFilter = new CSPopList(mpoplist_MpLevel, mPoolHandleManager);
        mAutoInstantFilter = new CSPopList(mpoplist_autoInstant, mPoolHandleManager);
        mReturnTimeFilter = new CSPopList(mpoplist_returnTime, mPoolHandleManager);
        mTransferTimeFilter = new CSPopList(mpoplist_transferTime, mPoolHandleManager);
        mSkillSingleFilter = new CSPopList(mpoplist_singleSkill, mPoolHandleManager);
        mSkillGroupFilter = new CSPopList(mpoplist_groupSkill, mPoolHandleManager);

        string[] datas = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(461));
        InitPopList(mBYEquipLvFilter, datas, ConfigOption.BYEquipPickUpLv, ClientTipsTableManager.Instance.GetClientTipsContext(937));
        UIEventListener.Get(mBYEquipLvFilter.BtnToggle.gameObject, (int)ConfigOption.BYEquipPickUpLv).onClick = ClosePopList;

        datas = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(33));
        InitPopList(mBYEquipQualityFilter, datas, ConfigOption.BYEquipPickUpQuality);
        UIEventListener.Get(mBYEquipQualityFilter.BtnToggle.gameObject, (int)ConfigOption.BYEquipPickUpQuality).onClick = ClosePopList;

        datas = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(668));
        InitPopList(mWLEquipLvFilter, datas, ConfigOption.WLEquipPickUpLv);
        UIEventListener.Get(mWLEquipLvFilter.BtnToggle.gameObject, (int)ConfigOption.WLEquipPickUpLv).onClick = ClosePopList;

        datas = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(463));
        InitPopList(mAttackLvFilter, datas, ConfigOption.AutoAttackPlayerLv, ClientTipsTableManager.Instance.GetClientTipsContext(940));
        UIEventListener.Get(mAttackLvFilter.BtnToggle.gameObject, (int)ConfigOption.AutoAttackPlayerLv).onClick = ClosePopList;

        datas = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(1147));
        InitPopList(mReturnTimeFilter, datas, ConfigOption.AutoGoBackTime, ClientTipsTableManager.Instance.GetClientTipsContext(2044));
        UIEventListener.Get(mReturnTimeFilter.BtnToggle.gameObject, (int)ConfigOption.AutoGoBackTime).onClick = ClosePopList;

        datas = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(1146));
        InitPopList(mTransferTimeFilter, datas, ConfigOption.AutoRandomDeliveryTime, ClientTipsTableManager.Instance.GetClientTipsContext(2044));
        UIEventListener.Get(mTransferTimeFilter.BtnToggle.gameObject, (int)ConfigOption.AutoRandomDeliveryTime).onClick = ClosePopList;

        if (drugList == null) drugList = new ILBetterList<int>();
        datas = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(654));
        if (datas != null)
        {
            mAutoDrugFilter.MaxCount = datas.Length + 1;
            mAutoDrugFilter.mDatas[0].idxValue = -1;
            mAutoDrugFilter.mDatas[0].value = takeDrugByOrderStr;
            drugList.Add(-1);
            for (int i = 1; i < datas.Length + 1; i++)
            {
                int id = 0;
                if(int.TryParse(datas[i - 1], out id))
                {
                    TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(id);
                    if (cfg == null) continue;
                    mAutoDrugFilter.mDatas[i].idxValue = id;
                    var color = UtilityColor.GetColorTypeByQuality(cfg.quality);
                    mAutoDrugFilter.mDatas[i].value = cfg.name.BBCode(color);
                    drugList.Add(id);
                }               
            }
            AddPopListEvent(mAutoDrugFilter, ConfigOption.AutoTakeDrug);
            UIEventListener.Get(mAutoDrugFilter.BtnToggle.gameObject, (int)ConfigOption.AutoTakeDrug).onClick = ClosePopList;
        }

        if (mpList == null) mpList = new ILBetterList<int>();
        datas = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(655));
        if (datas != null)
        {
            mAutoMpFilter.MaxCount = datas.Length + 1;
            mAutoMpFilter.mDatas[0].idxValue = -1;
            mAutoMpFilter.mDatas[0].value = takeDrugByOrderStr;
            mpList.Add(-1);
            for (int i = 1; i < datas.Length + 1; i++)
            {
                int id = 0;
                if (int.TryParse(datas[i - 1], out id))
                {
                    TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(id);
                    if (cfg == null) continue;
                    mAutoMpFilter.mDatas[i].idxValue = id;
                    var color = UtilityColor.GetColorTypeByQuality(cfg.quality);
                    mAutoMpFilter.mDatas[i].value = cfg.name.BBCode(color);
                    mpList.Add(id);
                }
            }
            AddPopListEvent(mAutoMpFilter, ConfigOption.AutoTakeMpDrug);
            UIEventListener.Get(mAutoMpFilter.BtnToggle.gameObject, (int)ConfigOption.AutoTakeMpDrug).onClick = ClosePopList;
        }

        if (instantList == null) instantList = new ILBetterList<int>();
        datas = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(1142));
        if (datas != null)
        {
            mAutoInstantFilter.MaxCount = datas.Length + 1;
            mAutoInstantFilter.mDatas[0].idxValue = -1;
            mAutoInstantFilter.mDatas[0].value = takeDrugByOrderStr;
            instantList.Add(-1);
            for (int i = 1; i < datas.Length + 1; i++)
            {
                int id = 0;
                if (int.TryParse(datas[i - 1], out id))
                {
                    TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(id);
                    if (cfg == null) continue;
                    mAutoInstantFilter.mDatas[i].idxValue = id;
                    var color = UtilityColor.GetColorTypeByQuality(cfg.quality);
                    mAutoInstantFilter.mDatas[i].value = cfg.name.BBCode(color);
                    instantList.Add(id);
                }
            }
            AddPopListEvent(mAutoInstantFilter, ConfigOption.AutoInstantDrug);
            UIEventListener.Get(mAutoInstantFilter.BtnToggle.gameObject, (int)ConfigOption.AutoInstantDrug).onClick = ClosePopList;
        }


        InitSkillSingleAndGroupFilters();
    }


    void InitSkillSingleAndGroupFilters()
    {
        mRoleSkill.CustomActive(CSMainPlayerInfo.Instance.Career == ECareer.Master);

        if (CSMainPlayerInfo.Instance.Career == ECareer.Master)
        {
            //string single = SundryTableManager.Instance.GetSundryEffect(720);
            //var datas = single.Split('&');
            //for (int i = 0; i < datas.Length; i++)
            //{
            //    var str = datas[i].Split('#');
            //    if (str.Length < 2) continue;
            //    int id = 0;
            //    if( int.TryParse(str[0], out id))
            //    {
            //        skillSingleIds.Add(id);
            //        skillSingleNames.Add(str[1]);
            //    }
            //}
            skillSingleIds = CSConfigInfo.Instance.skillSingleIds;
            skillSingleNames = CSConfigInfo.Instance.skillSingleNames;
            mSkillSingleFilter.MaxCount = skillSingleIds.Count;
            for (int i = 0; i < skillSingleIds.Count; i++)
            {
                mSkillSingleFilter.mDatas[i].idxValue = skillSingleIds[i];
                if (skillSingleNames.Count > i)
                {
                    mSkillSingleFilter.mDatas[i].value = skillSingleNames[i];
                }
            }
            AddPopListEvent(mSkillSingleFilter, ConfigOption.SkillSingle);
            UIEventListener.Get(mSkillSingleFilter.BtnToggle.gameObject, (int)ConfigOption.SkillSingle).onClick = ClosePopList;

            //string group = SundryTableManager.Instance.GetSundryEffect(721);
            //datas = group.Split('&');
            //for (int i = 0; i < datas.Length; i++)
            //{
            //    var str = datas[i].Split('#');
            //    if (str.Length < 2) continue;
            //    int id = 0;
            //    if (int.TryParse(str[0], out id))
            //    {
            //        skillGroupIds.Add(id);
            //        skillGroupNames.Add(str[1]);
            //    }
            //}
            skillGroupIds = CSConfigInfo.Instance.skillGroupIds;
            skillGroupNames = CSConfigInfo.Instance.skillGroupNames;
            mSkillGroupFilter.MaxCount = skillGroupIds.Count;
            for (int i = 0; i < skillGroupIds.Count; i++)
            {
                mSkillGroupFilter.mDatas[i].idxValue = skillGroupIds[i];
                if (skillGroupNames.Count > i)
                {
                    mSkillGroupFilter.mDatas[i].value = skillGroupNames[i];
                }
            }
            AddPopListEvent(mSkillGroupFilter, ConfigOption.SkillGroup);
            UIEventListener.Get(mSkillGroupFilter.BtnToggle.gameObject, (int)ConfigOption.SkillGroup).onClick = ClosePopList;
        }
    }


    void InitPopList(CSPopList popList, string[] datas, ConfigOption configOption, string addStr = "")
    {
        if (datas == null) return;
        popList.MaxCount = datas.Length;
        for (int i = 0; i < datas.Length; i++)
        {
            popList.mDatas[i].idxValue = i;
            if (!string.IsNullOrEmpty(addStr)) popList.mDatas[i].value = string.Format(addStr, datas[i]);
            else popList.mDatas[i].value = datas[i];
        }
        AddPopListEvent(popList, configOption);
    }

    void AddPopListEvent(CSPopList popList, ConfigOption configOption)
    {
        popList.InitList((x) =>
        {
            mConfigInfo.SetInt(configOption, x.idxValue);
        }, false, 0, false);
    }


    void RefreshFilters()
    {
        mBYEquipLvFilter.SetCurValue(mConfigInfo.GetInt(ConfigOption.BYEquipPickUpLv), false);
        mBYEquipQualityFilter.SetCurValue(mConfigInfo.GetInt(ConfigOption.BYEquipPickUpQuality), false);
        mWLEquipLvFilter.SetCurValue(mConfigInfo.GetInt(ConfigOption.WLEquipPickUpLv), false);
        mAttackLvFilter.SetCurValue(mConfigInfo.GetInt(ConfigOption.AutoAttackPlayerLv), false);
        mReturnTimeFilter.SetCurValue(mConfigInfo.GetInt(ConfigOption.AutoGoBackTime), false);
        mTransferTimeFilter.SetCurValue(mConfigInfo.GetInt(ConfigOption.AutoRandomDeliveryTime), false);

        if (drugList != null && drugList.Count > 0)
        {
            mAutoDrugFilter.SetCurValue(0, false);
            int drugId = mConfigInfo.GetInt(ConfigOption.AutoTakeDrug);
            for (int i = 0; i < drugList.Count; i++)
            {
                if (drugList[i] == drugId)
                {
                    mAutoDrugFilter.SetCurValue(i, false);
                    break;
                }
            }
        }

        if (mpList != null && mpList.Count > 0)
        {
            mAutoMpFilter.SetCurValue(0, false);
            int drugId = mConfigInfo.GetInt(ConfigOption.AutoTakeMpDrug);
            for (int i = 0; i < mpList.Count; i++)
            {
                if (mpList[i] == drugId)
                {
                    mAutoMpFilter.SetCurValue(i, false);
                    break;
                }
            }
        }

        if (instantList != null && instantList.Count > 0)
        {
            mAutoInstantFilter.SetCurValue(0, false);
            int drugId = mConfigInfo.GetInt(ConfigOption.AutoInstantDrug);
            for (int i = 0; i < instantList.Count; i++)
            {
                if (instantList[i] == drugId)
                {
                    mAutoInstantFilter.SetCurValue(i, false);
                    break;
                }
            }
        }

        if (CSMainPlayerInfo.Instance.Career == ECareer.Master)
        {
            if (skillSingleIds != null && skillSingleIds.Count > 0)
            {
                mSkillSingleFilter.SetCurValue(0, false);
                int groupId = mConfigInfo.GetInt(ConfigOption.SkillSingle);
                for (int i = 0; i < skillSingleIds.Count; i++)
                {
                    if (skillSingleIds[i] == groupId)
                    {
                        mSkillSingleFilter.SetCurValue(i, false);
                        break;
                    }
                }
            }

            if (skillGroupIds != null && skillGroupIds.Count > 0)
            {
                mSkillGroupFilter.SetCurValue(0, false);
                int groupId = mConfigInfo.GetInt(ConfigOption.SkillGroup);
                for (int i = 0; i < skillGroupIds.Count; i++)
                {
                    if (skillGroupIds[i] == groupId)
                    {
                        mSkillGroupFilter.SetCurValue(i, false);
                        break;
                    }
                }
            }
        }
            
    }



    void InitAutoReleaseSkill()
    {
        if (skills == null) skills = new List<ConfigAutoReleaseSkill>();
        else skills.Clear();

        int career = CSMainPlayerInfo.Instance.Career;
        var list = CSConfigInfo.Instance.SkillList;
        if (list == null) return;

        for (int i = 0; i < list.Count; i++)
        {
            var x = list[i];
            if (x.Career == career) skills.Add(x);
        }

        
    }


    void RefreshSkillGrid()
    {
        if (skills == null) return;
        mGrid_AutoSkill.Bind<ConfigAutoReleaseSkill, UIConfigAutoSkillItem>(skills, mPoolHandleManager);
    }



    void ClosePopList(GameObject go)
    {
        int param = System.Convert.ToInt32(UIEventListener.Get(go).parameter);

        mBYEquipLvFilter?.DiOnClick(param == (int)ConfigOption.BYEquipPickUpLv);
        mBYEquipQualityFilter?.DiOnClick(param == (int)ConfigOption.BYEquipPickUpQuality);
        mWLEquipLvFilter?.DiOnClick(param == (int)ConfigOption.WLEquipPickUpLv);
        mAttackLvFilter?.DiOnClick(param == (int)ConfigOption.AutoAttackPlayerLv);
        mAutoDrugFilter?.DiOnClick(param == (int)ConfigOption.AutoTakeDrug);
        mAutoMpFilter?.DiOnClick(param == (int)ConfigOption.AutoTakeMpDrug);
        mSkillSingleFilter?.DiOnClick(param == (int)ConfigOption.SkillSingle);
        mSkillGroupFilter?.DiOnClick(param == (int)ConfigOption.SkillGroup);
        mAutoInstantFilter?.DiOnClick(param == (int)ConfigOption.AutoInstantDrug);
        mReturnTimeFilter?.DiOnClick(param == (int)ConfigOption.AutoGoBackTime);
        mTransferTimeFilter?.DiOnClick(param == (int)ConfigOption.AutoRandomDeliveryTime);
    }


    protected override void OnDestroy()
    {
        mBYEquipLvFilter?.Destroy();
        mBYEquipQualityFilter?.Destroy();
        mWLEquipLvFilter?.Destroy();
        mAutoDrugFilter?.Destroy();
        mAttackLvFilter?.Destroy();
        mAutoMpFilter?.Destroy();
        mSkillSingleFilter?.Destroy();
        mSkillGroupFilter?.Destroy();
        mAutoInstantFilter?.Destroy();
        mReturnTimeFilter?.Destroy();
        mTransferTimeFilter?.Destroy();

        if (mGrid_AutoSkill != null)
        {
            mGrid_AutoSkill.UnBind<UIConfigAutoSkillItem>();
        }
        if (propToggles != null)
        {
            for (int i = 0; i < propToggles.Count; i++)
            {
                propToggles[i]?.Destroy();
            }
            propToggles.Clear();
            propToggles = null;
        }

        skills?.Clear();
        skills = null;

        drugList?.Clear();
        drugList = null;
        mpList?.Clear();
        mpList = null;
        instantList?.Clear();
        instantList = null;

        base.OnDestroy();
    }

}


public class UIConfigAutoSkillItem : UIBinder
{
    UIToggle tg;
    UILabel lb_name;


    ConfigAutoReleaseSkill mData;

    public override void Init(UIEventListener handle)
    {
        tg = handle.GetComponent<UIToggle>();
        lb_name = Get<UILabel>("Label");
    }


    public override void Bind(object data)
    {
        mData = data as ConfigAutoReleaseSkill;
        if (mData != null)
        {
            //tg.value = mData.AutoRelease;
            tg.value = mData.AutoRelease;
            lb_name.text = mData.showName;
            EventDelegate.Add(tg.onChange, () =>
            {
                CSConfigInfo.Instance.SetAutoReleaseSkill(mData, tg.value);
            });
        }
    }


    //public void RefreshValue()
    //{
    //    if (mData != null)
    //    {
    //        tg.value = CSConfigInfo.Instance.GetAutoReleaseSkill(mData.Career, mData.SkillGroup);
    //    }
    //}


    public override void OnDestroy()
    {
        mData = null;
        tg = null;
        lb_name = null;
    }
}



public class UIConfigPropPickUpBinder : UIBinder
{
    UIToggle toggle;
    UILabel lb_name;
    BoxCollider col;


    int pickUpId;


    public override void Init(UIEventListener handle)
    {
        toggle = handle.GetComponent<UIToggle>();
        col = handle.GetComponent<BoxCollider>();
        lb_name = Get<UILabel>("Label");

        handle.onClick = OnClick;
    }


    public override void Bind(object data)
    {
        int index = System.Convert.ToInt32(data);
        pickUpId = CSConfigInfo.Instance.GetPropPickUpTypeIdByIndex(index);
        if (pickUpId <= 0) return;

        bool allEnabled = CSConfigInfo.Instance.GetBool(ConfigOption.AllItemPickUp);
        if (col != null)
        {
            col.enabled = allEnabled;
        }

        if (lb_name != null)
        {
            lb_name.text = CSConfigInfo.Instance.GetPropPickUpNameByIndex(index);
            Color color = UtilityColor.GetColor(allEnabled ? ColorType.SubTitleColor : ColorType.WeakText);
            lb_name.color = color;
        }

        if (toggle != null)
        {
            toggle.value = CSConfigInfo.Instance.GetPropPickUpBool(pickUpId);
        }

    }


    void OnClick(GameObject go)
    {
        if (pickUpId <= 0) return;
        CSConfigInfo.Instance.SetPropPickUpBool(pickUpId, toggle.value);
    }


    public override void OnDestroy()
    {
        toggle = null;
        lb_name = null;
        col = null;
    }
}