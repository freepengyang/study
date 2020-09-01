using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
部位         格子           装备部位(type==2)           部位(卧龙)   格子           装备部位(type==5)

武器          1               1                         武器          101               1           
衣服          2               2                         衣服          102               2
头盔          3               3                         头盔          103               3
项链          4               4                         项链          104               4
护腕(左)      5               5                         护腕(左)      105               5
护腕(右)      6               5                         护腕(右)      106               5
戒指(左)      7               6                         戒指(左)      107               6
戒指(右)      8               6                         戒指(右)      108               6
靴子          9               7                         靴子          109               7
腰带          10              8                         腰带          110               8
勋章          11              9                         勋章          111               9
宝石          12              10                        宝石          112               10
 **/

public partial class UIEquipEnhancePanel : UIBasePanel
{
    private enum StarSuitState { Unactive, Active}

    #region var
    ILBetterList<EnhanceData> enhanceData;
    ILBetterList<GameObject> starObjs = new ILBetterList<GameObject>();
    EnhanceData selectedData;
    /// <summary>
    /// 自动升星下拉框选中的id
    /// </summary>
    int curDropDownId;
    /// <summary>
    /// 当前穿戴装备数
    /// </summary>
    int curWearingCount;

    Schedule scheduleRepeat;
    string timeCountDown = "";
    UIStarSuitTipPanel suitTipPanel;
    /// <summary>
    /// 预设的自动升星等级，，小于1则没有预设等级，也代表没有开启自动强化
    /// </summary>
    int presetAutoEnhanceLv;
    Schedule scheduleEnhance;
    bool isAutoEnhancing;
    #endregion

    #region const
    /// <summary>
    /// 最大星级
    /// </summary>
    private const int MaxStarLv = 15;
    /// <summary>
    /// 消耗货币id
    /// </summary>
    private const int CostMoneyId = 1;
    /// <summary>
    /// 强化消耗道具id
    /// </summary>
    private const int EnhanceItemId = 50000020;
    /// <summary>
    /// 提高概率道具id
    /// </summary>
    private const int AddOddsItemId = 50000021;
    /// <summary>
    /// 掉星保护道具id
    /// </summary>
    private const int ProtectItemId = 50000022;

    const float AutoEnhanceTimer = 2f;
    #endregion


    #region sundryString
    string enhanceStr;
    string autoEnhanceStr;
    string stopEnhanceStr;
    #endregion


    public void SetGo(GameObject _go)
    {
        UIPrefab = _go;
    }

    public override void Init()
    {
        base.Init();
        CSEffectPlayMgr.Instance.ShowUITexture(mObj_centerBg, "enhance_bg1");
        CSEffectPlayMgr.Instance.ShowUITexture(mObj_leftBg, "enhance_bg2");
        CSEffectPlayMgr.Instance.ShowUITexture(mObj_rightBg, "enhance_bg2");
        //CSEffectPlayMgr.Instance.ShowUIEffect(meffectAlways, "effect_upstar_always");

        mClientEvent.Reg((uint)CEvent.MoneyChange, OnMoneyOrItemChange);
        mClientEvent.Reg((uint)CEvent.ItemListChange, OnMoneyOrItemChange);
        mClientEvent.Reg((uint)CEvent.EnhanceResponse, EnhanceResponse);
        mClientEvent.Reg((uint)CEvent.WearEquip, EquipChanged);
        mClientEvent.Reg((uint)CEvent.UnWeatEquip, EquipChanged);
        mClientEvent.Reg((uint)CEvent.SuitStarLvChange, SuitStarLvChange);
        mClientEvent.Reg((uint)CEvent.SuitStarLvProtectStart, SuitStarLvProtectStart);

        mClientEvent.Reg((uint)CEvent.EnhanceBtnRedPointCheck, OnCheckRedPoint);

        //RegisterRed(mBtn_enhance.transform.GetChild(1).gameObject, RedPointType.EnhanceBtn);

        mBtn_enhance.onClick = EnhanceBtnClick;
        mBtn_help.onClick = HelpBtnClick;

        UIEventListener.Get(mBtn_addOdds.gameObject, 1).onClick = CheckBoxClick;
        UIEventListener.Get(mBtn_protect.gameObject, 2).onClick = CheckBoxClick;
        mBtn_auto.onClick = DropDownBoxClick;

        UIEventListener.Get(mBtn_costMoney.gameObject, CostMoneyId).onClick = ShowGetWayClick;
        UIEventListener.Get(mBtn_BuyCostItem, EnhanceItemId).onClick = ShowGetWayClick;
        UIEventListener.Get(mSp_costItemIcon.gameObject, EnhanceItemId).onClick = SupportItemClick;
        UIEventListener.Get(mBtn_leftItem, AddOddsItemId).onClick = SupportItemClick;
        UIEventListener.Get(mBtn_rightItem, ProtectItemId).onClick = SupportItemClick;

        UIEventListener.Get(mBtn_suitInfo).onClick = ShowSuitTipsClick;
        UIEventListener.Get(mSp_costMoney.gameObject).onClick = MoneySpClick;


        enhanceStr = SundryTableManager.Instance.GetSundryEffect(475);//强化
        autoEnhanceStr = SundryTableManager.Instance.GetSundryEffect(476);//自动强化
        stopEnhanceStr = SundryTableManager.Instance.GetSundryEffect(477);//停止强化

        InitUI();
    }


    public override void Show()
    {
        base.Show();

        RefreshLeftGrid(true);
        RefreshEquipPosChangedUI(true);
        RefreshSuitInfo();

        CSEnhanceInfo.Instance.OpenEnhancePanel();
    }

    public override void SelectChildPanel(int type, int subType)
    {
        //base.SelectChildPanel(type);
        
        if (type == 3 && subType == 1)//选择第一个可强化部位
        {
            int count = mGrid_wearingEquip.MaxCount;
            if (count < 12) return;
            int pos = CSEnhanceInfo.Instance.GetFirstCanEnhancePos();
            EnhanceData data = CSEnhanceInfo.Instance.GetDataByPos(pos);
            if (data == null) return;
            selectedData = data;
            var cellHeight = mGrid_wearingEquip.CellHeight;
            var viewHeight = mscroll_left.panel.height;
            float value = ((pos - 1) * cellHeight) / (count * cellHeight - viewHeight);
            value = Mathf.Clamp01(value);
            //mScrollBar.barSize = viewHeight / (count * cellHeight);
            //mScrollBar.value = value;
            TweenProgressBar.Begin(mScrollBar, 0.2f, mScrollBar.value, value);
            //mscroll_left.ScrollImmidate(value);
            mTrans_choose.localPosition = new Vector2(0, 0 - (pos - 1) * cellHeight);
            curDropDownId = 0;
            AutoUpStarChangedUI();
            RefreshEquipPosChangedUI();
        }
    }


    /// <summary>
    /// 初始化固定的ui部分
    /// </summary>
    void InitUI()
    {
        mSp_costMoney.spriteName = $"tubiao{CostMoneyId}";
        mSp_costItemIcon.spriteName = $"tubiao{EnhanceItemId}";
        //mSp_costItemBg.spriteName = ItemTableManager.Instance.GetItemQualityBG(ItemTableManager.Instance.GetItemQuality(EnhanceItemId));

        mSp_leftIcon.spriteName = ItemTableManager.Instance.GetItemIcon(AddOddsItemId);
        mSp_leftBg.spriteName = ItemTableManager.Instance.GetItemQualityBG(ItemTableManager.Instance.GetItemQuality(AddOddsItemId));

        mSp_rightIcon.spriteName = ItemTableManager.Instance.GetItemIcon(ProtectItemId);
        mSp_rightBg.spriteName = ItemTableManager.Instance.GetItemQualityBG(ItemTableManager.Instance.GetItemQuality(ProtectItemId));

        mGrid_star.MaxCount = MaxStarLv;
        for (int i = 0; i < mGrid_star.MaxCount; i++)
        {
            starObjs.Add(mGrid_star.controlList[i].transform.GetChild(0).gameObject);
        }

        mLb_suitTime.gameObject.SetActive(false);
        if (CSEnhanceInfo.Instance.ProtectSuitStarLv > 0)
        {
            scheduleRepeat = Timer.Instance.InvokeRepeating(0, 1, ScheduleReapeat);
        }

        mObj_suitTipPanel.SetActive(false);
    }


    /// <summary>
    /// 刷新左侧列表（左侧显示的是所有装备位）
    /// </summary>
    /// <param name="resetSelect">是否重置选中</param>
    void RefreshLeftGrid(bool resetSelect = false)
    {
        //mPoolHandleManager.RecycleAll();
        enhanceData = CSEnhanceInfo.Instance.GetSortedDataList();
        if (enhanceData.Count < 1) return;
        mGrid_wearingEquip.Bind<EnhanceData, UIEnhanceItem>(enhanceData, mPoolHandleManager);
        for (int i = 0; i < mGrid_wearingEquip.MaxCount; i++)
        {
            UIEventListener.Get(mGrid_wearingEquip.controlList[i], enhanceData[i]).onClick = LeftEnhanceItemClick;
        }

        if (resetSelect)
        {
            selectedData = enhanceData[0];
            mTrans_choose.localPosition = mGrid_wearingEquip.controlList[0].transform.localPosition;
            //mScrollBar.value = 0;
        }
    }

    /// <summary>
    /// 刷新选择的装备位变更时会改变的ui
    /// </summary>
    void RefreshEquipPosChangedUI(bool isReset = false)
    {
        if (selectedData == null) return;

        mSp_mainIcon.spriteName = selectedData.IconName;
        mLb_mainName.text = selectedData.Name;

        mLb_curAttr.text = $"{selectedData.CurAttr}%";
        mLb_nextAttr.text = $"{selectedData.NextAttr}%";

        RefreshOddsLabel(mObj_addOdds.activeSelf);

        //星星
        for (int i = 0; i < starObjs.Count; i++)
        {
            starObjs[i].CustomActive(i < selectedData.Lv);
        }

        if (selectedData.Lv < MaxStarLv)
        {
            mobj_autoenhance.CustomActive(true);
            mBtn_enhance.gameObject.CustomActive(true);
            //直升星级下拉框部分
            int count = MaxStarLv - selectedData.Lv > 8 ? 9 : MaxStarLv - selectedData.Lv + 1;
            mGrid_autoLvUp.MaxCount = count;
            mGrid_autoLvUp.controlList[count - 1].transform.GetChild(0).GetComponent<UILabel>().text = "关闭";
            UIEventListener.Get(mGrid_autoLvUp.controlList[count - 1], 0).onClick = AutoLvUpSelectClick;

            for (int i = mGrid_autoLvUp.MaxCount - 2; i >= 0; i--)
            {
                int autoLv = selectedData.Lv + (count - i - 1);
                mGrid_autoLvUp.controlList[i].transform.GetChild(0).GetComponent<UILabel>().text = $"直升{autoLv}星";
                UIEventListener.Get(mGrid_autoLvUp.controlList[i], count - i - 1).onClick = AutoLvUpSelectClick;
            }
            mSp_autoList.height = 30 * mGrid_autoLvUp.MaxCount;
            mSp_autoList.gameObject.SetActive(false);
            //mSp_autoArrow.flip = UIBasicSprite.Flip.Nothing;
            curDropDownId = 0;

            mTrans_autoSelect.localPosition = new Vector2(0, 18 + (curDropDownId) * 29);
        }
        else
        {
            mobj_autoenhance.CustomActive(false);
            mBtn_enhance.gameObject.CustomActive(false);
            mlb_hintMax.text = $"{selectedData.CurAttr}%";
        }

        mhint1.CustomActive(selectedData.Lv < MaxStarLv);
        mhint2.CustomActive(selectedData.Lv < MaxStarLv);
        mhintMax.CustomActive(selectedData.Lv >= MaxStarLv);

        RefreshCostItemUI(isReset);
        //AutoUpStarChangedUI();
    }

    /// <summary>
    /// 成功率文字显示
    /// </summary>
    /// <param name="showBonus">是否显示额外成功率</param>
    void RefreshOddsLabel(bool showBonus = false)
    {
        mLb_odds.text = string.Format("[dcd5b8]成功率：{0}%[00ff0c]{1}", selectedData.BaseOdds, showBonus ? $"+{selectedData.BonusOdds}%" : "");
    }

    /// <summary>
    /// 刷新左上角套装信息
    /// </summary>
    void RefreshSuitInfo()
    {
        curWearingCount = CSBagInfo.Instance.GetNormalEquipCount();
        string suitStr = SundryTableManager.Instance.GetSundryEffect(472);
        string str1 = "";
        string str2 = "";
        //if (curWearingCount < 12)
        //{
        //    //未穿满装备时显示最低级套装效果****旧规则。现在改成依然显示星级数量
        //    //str1 = string.Format(suitStr, 3);
        //    //str2 = $"({curWearingCount}/12)";
        //    ////mLb_suitInfo.text = string.Format("[ffcc00]{0}星套装[ff0000]({1}/12)", 3, curWearingCount);
        //    //mLb_suitInfo.text = $"{str1}{str2}";
        //    //RefreshSuitTipPanel(StarSuitState.Unactive, 3, curWearingCount);
        //}
        //else
        //{
            //穿戴满装备且到达满级
            if (CSEnhanceInfo.Instance.RealSuitStarLv >= 15)
            {
                str1 = string.Format(suitStr, 15);
                str2 = $"({12}/12)";
                mLb_suitInfo.text = $"{str1}{str2}";
                //mLb_suitInfo.text = "[ffcc00]15星套装[ff0000](12/12)";
                RefreshSuitTipPanel(StarSuitState.Active, 15, 12);
                return;
            }
            else if(CSEnhanceInfo.Instance.RealSuitStarLv < 3)
            {
                str1 = string.Format(suitStr, 3);
                str2 = $"({CSEnhanceInfo.Instance.SatisfyCurLvCount}/12)";
                mLb_suitInfo.text = $"{str1}{str2}";
                //mLb_suitInfo.text = string.Format("[ffcc00]{0}星套装[ff0000]({1}/12)", 3, CSEnhanceInfo.Instance.SatisfyCurLvCount);
                RefreshSuitTipPanel(StarSuitState.Unactive, 3, CSEnhanceInfo.Instance.SatisfyCurLvCount, CSEnhanceInfo.Instance.SatisfyNextLvCount, timeCountDown);
                return;
            }

            str1 = string.Format(suitStr, CSEnhanceInfo.Instance.RealSuitStarLv + 1);
            str2 = $"({CSEnhanceInfo.Instance.SatisfyNextLvCount}/12)";
            mLb_suitInfo.text = $"{str1}{str2}";
            //mLb_suitInfo.text = string.Format("[ffcc00]{0}星套装[ff0000]({1}/12)", CSEnhanceInfo.Instance.RealSuitStarLv + 1, CSEnhanceInfo.Instance.SatisfyNextLvCount);
            RefreshSuitTipPanel(StarSuitState.Active, CSEnhanceInfo.Instance.RealSuitStarLv, CSEnhanceInfo.Instance.SatisfyCurLvCount, CSEnhanceInfo.Instance.SatisfyNextLvCount, timeCountDown);
        //}
    }


    /// <summary>
    /// 下拉框选择刷新
    /// </summary>
    void AutoUpStarChangedUI()
    {
        mTrans_autoSelect.localPosition = new Vector2(0, 18 + (curDropDownId) * 29);
        presetAutoEnhanceLv = curDropDownId < 1 ? 0 : selectedData.Lv + curDropDownId;
        mLb_autoLv.text = curDropDownId < 1 ? SundryTableManager.Instance.GetSundryEffect(479) : CSString.Format(SundryTableManager.Instance.GetSundryEffect(478), selectedData.Lv + curDropDownId);
        EnhanceBtnText(presetAutoEnhanceLv < 1 ? 0 : 1);

    }


    void EnhanceBtnText(int _type = 0)
    {
        string str = "";
        switch (_type)
        {
            case 0:
                str = enhanceStr;
                break;
            case 1:
                str = autoEnhanceStr;
                break;
            case 2:
                str = stopEnhanceStr;
                break;
        }

        mLb_enhanceBtn.text = str;
    }


    /// <summary>
    /// 消耗道具和货币刷新
    /// </summary>
    void RefreshCostItemUI(bool isReset = false)
    {
        long hasMoney = CostMoneyId.GetItemCount();
        mLb_costMoney.text = $"{selectedData.CostMoney}".BBCode(hasMoney >= selectedData.CostMoney ? ColorType.Green : ColorType.Red);
        long hasItem = EnhanceItemId.GetItemCount();
        mLb_costItemNum.text = $"{hasItem}/{selectedData.CostItem}".BBCode(hasItem >= selectedData.CostItem ? ColorType.Green : ColorType.Red);

        //左右两个物品
        mObj_leftAdd.SetActive(AddOddsItemId.GetItemCount() < selectedData.CostAddOddsItem);
        mObj_rightAdd.SetActive(ProtectItemId.GetItemCount() < selectedData.CostProtectItem);

        mSp_leftIcon.color = AddOddsItemId.GetItemCount() < selectedData.CostAddOddsItem ? CSColor.gray : CSColor.white;
        mSp_rightIcon.color = ProtectItemId.GetItemCount() < selectedData.CostProtectItem ? CSColor.gray : CSColor.white;

        mLb_leftNum.text = $"{AddOddsItemId.GetItemCount()}/{selectedData.CostAddOddsItem}".BBCode(AddOddsItemId.GetItemCount() < selectedData.CostAddOddsItem ? ColorType.Red : ColorType.Green);
        mLb_rightNum.text = $"{ProtectItemId.GetItemCount()}/{selectedData.CostProtectItem}".BBCode(ProtectItemId.GetItemCount() < selectedData.CostProtectItem ? ColorType.Red : ColorType.Green);

        if (isReset)
        {
            TwoCheckBoxSwitch(1, false, true);
            mObj_protect.SetActive(false);
        }
    }



    /// <summary>
    /// 勾选框刷新
    /// </summary>
    /// <param name="boxId">1为增加成功率勾选框，2为掉星保护勾选框</param>
    /// <param name="needTips">无法勾选时是否弹出提示</param>
    /// <param name="forcedToCheck">是否强制勾选</param>
    void TwoCheckBoxSwitch(int boxId = 1, bool needTips = false, bool forcedToCheck = false)
    {
        if (boxId == 1)
        {
            if (AddOddsItemId.GetItemCount() <= 0 || AddOddsItemId.GetItemCount() < selectedData.CostAddOddsItem)
            {
                mObj_addOdds.SetActive(false);
                RefreshOddsLabel(false);
                if (needTips)
                    //UtilityTips.ShowRedTips(965, ItemTableManager.Instance.GetItemName(AddOddsItemId));
                    Utility.ShowGetWay(AddOddsItemId);
                return;
            }
            mObj_addOdds.SetActive(forcedToCheck ? true : !mObj_addOdds.activeSelf);
            RefreshOddsLabel(mObj_addOdds.activeSelf);
        }
        else
        {
            if (ProtectItemId.GetItemCount() <= 0 || ProtectItemId.GetItemCount() < selectedData.CostProtectItem)
            {
                mObj_protect.SetActive(false);
                if (needTips)
                    //UtilityTips.ShowRedTips(965, ItemTableManager.Instance.GetItemName(ProtectItemId));
                    Utility.ShowGetWay(ProtectItemId);
                return;
            }
            mObj_protect.SetActive(forcedToCheck ? true : !mObj_protect.activeSelf);
        }
    }


    void RefreshSuitTipPanel(StarSuitState state, int lv, int curCount, int nextCount = 0, string timeCountDown = "")
    {
        if (suitTipPanel == null && !mObj_suitTipPanel.activeSelf) return;
        if (state == StarSuitState.Unactive)
        {
            suitTipPanel.RefreshUnactived(lv, curCount);
        }
        else
        {
            suitTipPanel.RefreshActived(lv, curCount, nextCount, timeCountDown);
        }
    }


    #region BtnClick
    void HelpBtnClick(GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.Strength);
    }

    void EnhanceBtnClick(GameObject go)
    {
        if (isAutoEnhancing)
        {
            CancelAutoEnhance();
            return;
        }

        if (selectedData == null) return;
        if (CostMoneyId.GetItemCount() < selectedData.CostMoney)
        {
            //UtilityTips.ShowRedTips(965, ItemTableManager.Instance.GetItemName(CostMoneyId));
            Utility.ShowGetWay(CostMoneyId);
            return;
        }
        if (EnhanceItemId.GetItemCount() < selectedData.CostItem)
        {
            //UtilityTips.ShowRedTips(965, ItemTableManager.Instance.GetItemName(EnhanceItemId));
            Utility.ShowGetWay(EnhanceItemId);
            return;
        }
        if (presetAutoEnhanceLv > 0 && selectedData.Lv >= presetAutoEnhanceLv)
        {
            ShowTips(2);
            return;
        }

        if (selectedData.Lv <= 0 || mObj_protect.activeSelf)
            ConfirmEnhanceBtnClick();
        else
            UtilityTips.ShowPromptWordTips(45, ConfirmEnhanceBtnClick);
    }

    /// <summary>
    /// 左侧列表装备位点击
    /// </summary>
    /// <param name="go"></param>
    void LeftEnhanceItemClick(GameObject go)
    {
        if (CheckIsAutoEnhancing())
        {
            ShowTips();
            return;
        }
        mTrans_choose.localPosition = go.transform.localPosition;
        EnhanceData data = (EnhanceData)UIEventListener.Get(go).parameter;
        selectedData = data;
        curDropDownId = 0;
        AutoUpStarChangedUI();
        RefreshEquipPosChangedUI();
    }

    /// <summary>
    /// 勾选框点击
    /// </summary>
    /// <param name="go"></param>
    void CheckBoxClick(GameObject go)
    {
        if (CheckIsAutoEnhancing())
        {
            ShowTips();
            return;
        }
        int param = System.Convert.ToInt32(UIEventListener.Get(go).parameter);
        if (param == 2 && !mObj_protect.activeSelf)
        {
            if (ProtectItemId.GetItemCount() >= selectedData.CostProtectItem)
            {
                UtilityTips.ShowPromptWordTips(47, ()=> { TwoCheckBoxSwitch(2); });
                return;
            }
        }

        TwoCheckBoxSwitch(param, true);
    }


    /// <summary>
    /// 下拉框点击
    /// </summary>
    /// <param name="go"></param>
    void DropDownBoxClick(GameObject go)
    {
        if (CheckIsAutoEnhancing())
        {
            ShowTips();
            return;
        }
        mSp_autoList.gameObject.SetActive(!mSp_autoList.gameObject.activeSelf);
        //mSp_autoArrow.flip = mSp_autoList.gameObject.activeSelf ? UIBasicSprite.Flip.Vertically : UIBasicSprite.Flip.Nothing;
    }

    /// <summary>
    /// 下拉框中的选择点击
    /// </summary>
    /// <param name="go"></param>
    void AutoLvUpSelectClick(GameObject go)
    {
        if (CheckIsAutoEnhancing())
        {
            ShowTips();
            return;
        }
        int param = System.Convert.ToInt32(UIEventListener.Get(go).parameter);
        curDropDownId = param;
        mSp_autoList.gameObject.SetActive(false);
        //mSp_autoArrow.flip = UIBasicSprite.Flip.Nothing;
        AutoUpStarChangedUI();
    }

    /// <summary>
    /// 左下角消耗金币和道具的加号按钮
    /// </summary>
    /// <param name="go"></param>
    void ShowGetWayClick(GameObject go)
    {
        int itemId = System.Convert.ToInt32(UIEventListener.Get(go).parameter);
        Utility.ShowGetWay(itemId);
    }

    void MoneySpClick(GameObject go)
    {
        UITipsManager.Instance.CreateTips(TipsOpenType.Normal, CostMoneyId);
    }

    /// <summary>
    /// 左右两个和左下角物品框点击
    /// </summary>
    /// <param name="go"></param>
    void SupportItemClick(GameObject go)
    {
        int itemId = System.Convert.ToInt32(UIEventListener.Get(go).parameter);
        long hasNum = itemId.GetItemCount();
        int needNum = 0;
        switch (itemId)
        {
            case EnhanceItemId:
                UITipsManager.Instance.CreateTips(TipsOpenType.Normal, ItemTableManager.Instance.GetItemCfg(itemId));
                return;
            case AddOddsItemId:
                needNum = selectedData.CostAddOddsItem;
                break;
            case ProtectItemId:
                needNum = selectedData.CostProtectItem;
                break;
        }
        if (hasNum >= needNum)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, ItemTableManager.Instance.GetItemCfg(itemId));
        }
        else
        {
            Utility.ShowGetWay(itemId);
        }
    }

    /// <summary>
    /// 左右物品框移除按钮,,暂时不用
    /// </summary>
    /// <param name="go"></param>
    void SupportItemRemoveClick(GameObject go)
    {
        
    }

    /// <summary>
    /// 显示套装信息按钮
    /// </summary>
    /// <param name="go"></param>
    void ShowSuitTipsClick(GameObject go)
    {
        if (suitTipPanel == null)
        {
            suitTipPanel = new UIStarSuitTipPanel();
            suitTipPanel.SetGo(mObj_suitTipPanel);
            suitTipPanel.Init();
        }
        suitTipPanel.Show();
        mObj_suitTipPanel.SetActive(true);
        //UIManager.Instance.CreatePanel<UIStarSuitTipPanel>(f =>
        //{
        //    suitTipPanel = f as UIStarSuitTipPanel;
        //});

        RefreshSuitInfo();
    }

    #endregion


    #region event
    /// <summary>
    /// 货币及道具消耗回调
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void OnMoneyOrItemChange(uint id, object data)
    {
        RefreshCostItemUI();
    }

    /// <summary>
    /// 强化结果回调
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void EnhanceResponse(uint id, object data)
    {
        bool isSucc = (bool)data;
        //CSEffectPlayMgr.Instance.Recycle(mobj_resultSucc);
        if (isSucc)
        {
            CSEffectPlayMgr.Instance.ShowUIEffect(mobj_resultSucc, "effect_upstar_succ", 15, false);
        }
        else
        {
            CSEffectPlayMgr.Instance.ShowUIEffect(mobj_resultSucc, "effect_upstar_fail", 15, false);
        }

        RefreshSuitInfo();
        RefreshLeftGrid();
        RefreshEquipPosChangedUI();
    }

    /// <summary>
    /// 穿、脱装备回调
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void EquipChanged(uint id, object data)
    {
        curWearingCount = CSBagInfo.Instance.GetNormalEquipCount();
    }

    void SuitStarLvChange(uint id, object data)
    {
        RefreshSuitInfo();
        bool isHigher = (bool)data;
        if (isHigher && CSEnhanceInfo.Instance.RealSuitStarLv >= 3)
        {
            UIManager.Instance.CreatePanel<UIEnhanceResultPanel>((f) =>
            {
                (f as UIEnhanceResultPanel).RefreshUI(CSEnhanceInfo.Instance.RealSuitStarLv);
            });
        }
    }

    void SuitStarLvProtectStart(uint id, object data)
    {
        CancelDelayInvoke();
        scheduleRepeat = Timer.Instance.InvokeRepeating(0, 1, ScheduleReapeat);
    }

    void OnCheckRedPoint(uint id, object data)
    {
        mBtn_enhance.transform.GetChild(1).gameObject.SetActive(CSEnhanceInfo.Instance.HasProtectSuitLv());
    }

    #endregion



    void ScheduleReapeat(Schedule schedule)
    {
        timeCountDown = "";
        if (CSEnhanceInfo.Instance.ProtectSuitStarLv < 1)
        {
            mLb_suitTime.gameObject.SetActive(false);
            CancelDelayInvoke();
            return;
        }

        long curTimeStamp = CSServerTime.Instance.TotalSeconds;
        long endTimeStamp = CSEnhanceInfo.Instance.ProtectTimeStamp;
        mLb_suitTime.gameObject.SetActive(endTimeStamp > curTimeStamp);
        if (curTimeStamp >= endTimeStamp)
        {
            CancelDelayInvoke();
            return;
        }

        //刷新倒计时ui
        long leftSeconds = endTimeStamp - curTimeStamp;
        if (leftSeconds > 86400)//超过1天显示几天几时
        {
            int days = Mathf.FloorToInt(leftSeconds / 86400);
            int hours = Mathf.FloorToInt((leftSeconds % 86400) / 3600);
            timeCountDown = $"{days}天{hours}小时".BBCode(ColorType.Red);
        }
        else if(leftSeconds > 3600)//不足1天显示几时几分
        {
            int hours = Mathf.FloorToInt(leftSeconds / 3600);
            int minutes = Mathf.FloorToInt((leftSeconds % 3600) / 60);
            timeCountDown = $"{hours}小时{minutes}分".BBCode(ColorType.Red);
        }
        else//显示几分几秒
        {
            int minutes = Mathf.FloorToInt(leftSeconds / 60);
            int seconds = Mathf.FloorToInt(leftSeconds % 60);
            timeCountDown = $"{minutes}分{seconds}秒".BBCode(ColorType.Red);
        }
        mLb_suitTime.text = timeCountDown;
    }


    void ScheduleEnhance(Schedule schedule)
    {
        if (selectedData.Lv >= presetAutoEnhanceLv)
        {
            CancelAutoEnhance();
            return;
        }

        if (CostMoneyId.GetItemCount() < selectedData.CostMoney || EnhanceItemId.GetItemCount() < selectedData.CostItem)
        {
            CancelAutoEnhance();
            return;
        }

        DoEnhance();
    }


    void ConfirmEnhanceBtnClick()
    {

        if (presetAutoEnhanceLv < 1) DoEnhance();
        else
        {
            scheduleEnhance = Timer.Instance.InvokeRepeating(0, AutoEnhanceTimer, ScheduleEnhance);
            isAutoEnhancing = true;
            EnhanceBtnText(2);
        }
    }

    void DoEnhance()
    {
        if (selectedData == null) return;
        if (selectedData.Lv >= MaxStarLv)
        {
            return;
        }

        if (AddOddsItemId.GetItemCount() < selectedData.CostAddOddsItem)
        {
            mObj_addOdds.SetActive(false);
        }
        if (ProtectItemId.GetItemCount() < selectedData.CostProtectItem)
        {
            mObj_protect.SetActive(false);
        }

        Net.CSIntensifyMessage(selectedData.Pos, mObj_addOdds.activeSelf, mObj_protect.activeSelf);
    }

    public bool CheckIsAutoEnhancing()
    {
        return isAutoEnhancing;
    }

    public void TryToPauseAutoEnhance()
    {
        CancelAutoEnhance();
    }

    public void TryToResumeAutoEnhance()
    {
        if (!Timer.Instance.IsInvoking(scheduleEnhance))
        {
            scheduleEnhance = Timer.Instance.InvokeRepeating(0, AutoEnhanceTimer, ScheduleEnhance);
            isAutoEnhancing = true;
            EnhanceBtnText(2);
        }
    }

    void CancelDelayInvoke()
    {
        if (Timer.Instance.IsInvoking(scheduleRepeat))
        {
            Timer.Instance.CancelInvoke(scheduleRepeat);
        }
        CancelAutoEnhance();
    }


    void CancelAutoEnhance()
    {
        if (Timer.Instance.IsInvoking(scheduleEnhance))
        {
            Timer.Instance.CancelInvoke(scheduleEnhance);
            isAutoEnhancing = false;
        }
        if (selectedData != null || selectedData.Lv >= presetAutoEnhanceLv)
        {
            curDropDownId = 0;
            AutoUpStarChangedUI();
        }
        EnhanceBtnText(presetAutoEnhanceLv < 1 ? 0 : 1);
    }


    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mObj_centerBg);
        CSEffectPlayMgr.Instance.Recycle(mObj_leftBg);
        CSEffectPlayMgr.Instance.Recycle(mObj_rightBg);

        CSEffectPlayMgr.Instance.Recycle(mobj_resultSucc);
        //CSEffectPlayMgr.Instance.Recycle(meffectAlways);

        mGrid_wearingEquip.UnBind<UIEnhanceItem>();

        CancelDelayInvoke();

        if (suitTipPanel != null) { suitTipPanel.Destroy(); }
        base.OnDestroy();
    }


    void ShowTips(int tip = 1)
    {
        switch (tip)
        {
            case 1:
                UtilityTips.ShowRedTips(1150);
                break;
            case 2:
                UtilityTips.ShowRedTips(1149);
                break;
        }
    }
}


public class UIEnhanceItem : UIBinder
{
    UILabel lb_name;
    UILabel lb_starCount;
    UISprite sp_icon;
    GameObject redPoint;

    EnhanceData cfg;


    public override void Init(UIEventListener handle)
    {
        lb_name = Get<UILabel>("name");
        lb_starCount = Get<UILabel>("freeNum");
        sp_icon = Get<UISprite>("Item/icon");
        redPoint = Get<GameObject>("redpoint");
    }

    public override void Bind(object data)
    {
        cfg = data as EnhanceData;
        lb_name.text = $"[cbb694]{cfg.Name}[-]";
        lb_starCount.text = $"[cbb694]X{cfg.Lv}[-]";
        sp_icon.spriteName = cfg.IconName;
        HotManager.Instance.EventHandler.AddEvent(CEvent.EnhanceBtnRedPointCheck, CheckRedPoint);
        CheckRedPoint(0, null);
    }


    void CheckRedPoint(uint id, object arg)
    {
        if (cfg == null || redPoint == null) return;
        redPoint.SetActive(CSEnhanceInfo.Instance.SinglePosCanEnhance(cfg.Pos));
    }


    public override void OnDestroy()
    {
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.EnhanceBtnRedPointCheck, CheckRedPoint);
        cfg = null;
        lb_name = null;
        lb_starCount = null;
        sp_icon = null;
        redPoint = null;

    }

}
