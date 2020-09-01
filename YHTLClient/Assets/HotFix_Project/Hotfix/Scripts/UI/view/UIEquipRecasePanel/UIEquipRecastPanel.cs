using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIEquipRecastPanel : UIBasePanel
{
    public override void Init()
    {
        base.Init();
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_texbg, "fr_chongzhu");
        mClientEvent.Reg((uint)CEvent.EquipRebuildNtfMessage, GetRecastBack);
        mClientEvent.AddEvent(CEvent.ItemListChange, GetItemChange);
        UIEventListener.Get(mbtn_wearEquip, 1).onClick = ChangeEuqipsBtnClick;
        UIEventListener.Get(mbtn_bagEquip, 2).onClick = ChangeEuqipsBtnClick;
        UIEventListener.Get(mbtn_recast).onClick = ReqRecast;
        UIEventListener.Get(mbtn_Conrecast).onClick = ConeqRecast;
        UIEventListener.Get(mbtn_cancelConrecast).onClick = ConeqCancelRecast;
        UIEventListener.Get(mbtn_help).onClick = InstructionClick;
        UIEventListener.Get(mobj_settingBg).onClick = SettingBtnClick;
        UIEventListener.Get(mbtn_setting).onClick = SettingBtnClick;
        UIEventListener.Get(mobj_getEquip.gameObject).onClick = GetWayClick;
        playerPrefKey = $"{CSMainPlayerInfo.Instance.Name}EquipRecast";
        quaSttr = SundryTableManager.Instance.GetSundryEffect(33).Split('#');
        InitSetting();
        mgrid_curGrid.MaxCount = 8;
        for (int j = 0; j < 8; j++)
        {
            attrList.Add(new RecastAttrItem(mgrid_curGrid.controlList[j], j));
        }
        RefreshBtnRed(panelType.bagEquip);
        RefreshBtnRed(panelType.selfEquip);
    }
    public void SetGo(GameObject _go)
    {
        UIPrefab = _go;
    }
    public override void SelectItem(TipsBtnData _data)
    {
        int temp_ind = 0;
        if (_data.openType == TipsOpenType.Bag)
        {
            ChangeEuqipsBtnClick(mbtn_bagEquip);
            for (int i = 0; i < bagRefineList.Count; i++)
            {
                if (bagRefineList[i].info.id == _data.info.id)
                {
                    ItemClick(mgrid_bagEquip.controlList[i]);
                    temp_ind = i;
                    break;
                }
            }
            if (temp_ind > 4)
            {
                SpringPanel.Begin(mobj_bagEquip, new Vector3(-347, mobj_bagEquip.transform.localPosition.y + 90 * (temp_ind - 4)), 20f);
            }
        }
        else if (_data.openType == TipsOpenType.RoleEquip)
        {
            ChangeEuqipsBtnClick(mbtn_wearEquip);
            for (int i = 0; i < selfRefineList.Count; i++)
            {
                if (selfRefineList[i].info.id == _data.info.id)
                {
                    ItemClick(mgrid_selfEquip.controlList[i]);
                    temp_ind = i;
                    break;
                }
            }
            if (temp_ind > 4)
            {
                SpringPanel.Begin(mobj_selfEquip, new Vector3(-347, mobj_selfEquip.transform.localPosition.y + 90 * (temp_ind - 4)), 20f);
            }
        }
    }
    enum panelType
    {
        bagEquip,
        selfEquip,
    }
    #region variable
    panelType curType;
    Dictionary<int, bag.BagItemInfo> bagequips = new Dictionary<int, bag.BagItemInfo>();
    Dictionary<int, bag.BagItemInfo> selfquips = new Dictionary<int, bag.BagItemInfo>();
    List<RefineItem> bagRefineList = new List<RefineItem>();
    List<RefineItem> selfRefineList = new List<RefineItem>();
    RefineItem curItem;
    UIItemBase showItem;
    List<RecastAttrItem> attrList = new List<RecastAttrItem>();
    EquipRefineProDic ranattrList;
    bool isContinue = false;
    /// <summary>
    /// 1 关闭  2紫色  3橙色
    /// </summary>
    int setValue = 2;
    Schedule schedule;
    float intervalTime = 0.8f;//特效播放时间
    string[] quaSttr;
    bool isConstEnough = false;
    bool isMoneyEnough = false;
    bool isCostItemEnough = false;
    string playerPrefKey = "";
    bool isSpecialChoose = false;
    #endregion
    public override void Show()
    {
        base.Show();
        selfquips.Clear();
        selfquips = CSBagInfo.Instance.GetEquipRecastItemData();
        if (selfquips.Count != 0)
        {
            ChangeEuqipsBtnClick(mbtn_wearEquip);
        }
        else
        {
            bagequips.Clear();
            CSBagInfo.Instance.GetAllRecastBagEquip(bagequips);
            if (bagequips.Count != 0)
            {
                ChangeEuqipsBtnClick(mbtn_bagEquip);
            }
            else
            {
                ChangeEuqipsBtnClick(mbtn_wearEquip);
            }
        }

    }
    public void SpecialTaskOpenChoose(int _type)
    {
        isSpecialChoose = true;
        if (curType == panelType.selfEquip)
        {
            if (selfquips.Count != 0)
            {
                if (SelfpurpleIndex != -1)
                {
                    ItemClick(mgrid_selfEquip.controlList[SelfpurpleIndex]);
                }
                else
                {
                    if (SelfnotPurpleIndex != -1)
                    {
                        ItemClick(mgrid_selfEquip.controlList[SelfnotPurpleIndex]);
                    }
                    else
                    {
                        ItemClick(mgrid_selfEquip.controlList[0]);
                    }
                }
            }
        }
        else
        {
            if (bagequips.Count != 0)
            {
                if (BagpurpleIndex != -1)
                {
                    ItemClick(mgrid_bagEquip.controlList[BagpurpleIndex]);
                }
                else
                {
                    if (BagnotPurpleIndex != -1)
                    {
                        ItemClick(mgrid_bagEquip.controlList[BagnotPurpleIndex]);
                    }
                    else
                    {
                        ItemClick(mgrid_bagEquip.controlList[0]);
                    }
                }
            }
        }
        isSpecialChoose = false;

    }
    public override void OnHide()
    {
        base.OnHide();
        mbtn_cancelConrecast.SetActive(false);
        isContinue = false;
        Timer.Instance.CancelInvoke(schedule);
        mbtn_recast.SetActive(setValue != 2);
        mbtn_Conrecast.SetActive(setValue == 2);
    }
    protected override void OnDestroy()
    {
        isSpecialChoose = false;
        Timer.Instance.CancelInvoke(schedule);
        curType = panelType.selfEquip;
        bagequips = null;
        selfquips = null;
        bagRefineList = null;
        selfRefineList = null;
        curItem = null;
        showItem = null;
        attrList = null;

        if (ranattrList != null) { StructTipData.Instance.RecycleSingle(ranattrList); };
        if (showItem != null) { UIItemManager.Instance.RecycleSingleItem(showItem); }
        //CSEffectPlayMgr.Instance.Recycle(mobj_residentEffect);
        CSEffectPlayMgr.Instance.Recycle(mtex_texbg);
        CSEffectPlayMgr.Instance.Recycle(meff_reacast);
        base.OnDestroy();
    }
    void ChangeEuqipsBtnClick(GameObject _go)
    {
        int type = (int)UIEventListener.Get(_go).parameter;
        if (type == 1)
        {
            mobj_selfEquip.SetActive(true);
            mobj_wearEquipHl.SetActive(true);
            mobj_bagEquip.SetActive(false);
            mobj_bagEquipHl.SetActive(false);
            RefreshSlefEquipGrid();
        }
        else
        {
            mobj_selfEquip.SetActive(false);
            mobj_wearEquipHl.SetActive(false);
            mobj_bagEquip.SetActive(true);
            mobj_bagEquipHl.SetActive(true);
            RefreshBagEquipGrid();
        }
    }

    int BagnotPurpleIndex = -1;
    int BagpurpleIndex = -1;
    void RefreshBagEquipGrid()
    {
        curType = panelType.bagEquip;
        bagRefineList.Clear();
        CSBagInfo.Instance.GetAllRecastBagEquip(bagequips);
        mgrid_bagEquip.MaxCount = bagequips.Count;
        if (bagequips.Count == 0) { SwitchToNoEquipState(true); return; }
        SwitchToNoEquipState(false);
        BagnotPurpleIndex = -1;
        BagpurpleIndex = -1;
        int i = 0;
        var iter = bagequips.GetEnumerator();
        while (iter.MoveNext())
        {
            bagRefineList.Add(new RefineItem(mgrid_bagEquip.controlList[i], ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId),
                iter.Current.Value, ItemClick, iter.Current.Key));
            if (iter.Current.Value.quality == 4 && BagpurpleIndex == -1)
            {
                BagpurpleIndex = i;
            }
            if (iter.Current.Value.quality < 4 && BagnotPurpleIndex == -1)
            {
                BagnotPurpleIndex = i;
            }
            i++;
        }

        if (bagequips.Count != 0)
        {
            if (BagnotPurpleIndex != -1)
            {
                ItemClick(mgrid_bagEquip.controlList[BagnotPurpleIndex]);
            }
            else
            {
                if (BagpurpleIndex != -1)
                {
                    ItemClick(mgrid_bagEquip.controlList[BagpurpleIndex]);
                }
                else
                {
                    ItemClick(mgrid_bagEquip.controlList[0]);
                }
            }
        }

        RefreshBtnRed(panelType.bagEquip);
    }
    void RefreshBtnRed(panelType _type)
    {
        bool hasRed = false;
        if (_type == panelType.bagEquip)
        {
            //var iter = bagequips.GetEnumerator();
            //while (iter.MoveNext())
            //{
            //    if (hasRed == false)
            //    {
            //        hasRed = CSBagInfo.Instance.IsEquipRecastCostEnough(iter.Current.Value, ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId));
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
            //Debug.Log("背包装备红点   "+ hasRed);
            mred_bag.SetActive(false);
        }
        else
        {
            var iter = selfquips.GetEnumerator();
            while (iter.MoveNext())
            {
                if (hasRed == false)
                {
                    hasRed = CSBagInfo.Instance.IsEquipRecastCostEnough(iter.Current.Value, ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId));
                }
                else
                {
                    break;
                }
            }
            //Debug.Log("身上装备红点    " + hasRed);
            mred_equip.SetActive(hasRed);
            for (int i = 0; i < selfRefineList.Count; i++)
            {
                selfRefineList[i].RefreshRed();
            }
        }
    }
    int SelfnotPurpleIndex = -1;
    int SelfpurpleIndex = -1;
    void RefreshSlefEquipGrid()
    {
        curType = panelType.selfEquip;
        selfRefineList.Clear();
        selfquips = CSBagInfo.Instance.GetEquipRecastItemData();
        mgrid_selfEquip.MaxCount = selfquips.Count;
        if (selfquips.Count == 0) { SwitchToNoEquipState(true); return; }
        SwitchToNoEquipState(false);
        SelfnotPurpleIndex = -1;
        SelfpurpleIndex = -1;
        int i = 0;
        var iter = selfquips.GetEnumerator();
        while (iter.MoveNext())
        {
            selfRefineList.Add(new RefineItem(mgrid_selfEquip.controlList[i], ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId),
                iter.Current.Value, ItemClick, -iter.Current.Key));
            if (iter.Current.Value.quality == 4 && SelfpurpleIndex == -1)
            {
                SelfpurpleIndex = i;
            }
            if (iter.Current.Value.quality < 4 && SelfnotPurpleIndex == -1)
            {
                SelfnotPurpleIndex = i;
            }
            i++;
        }

        if (selfquips.Count != 0)
        {
            if (SelfnotPurpleIndex != -1)
            {
                ItemClick(mgrid_selfEquip.controlList[SelfnotPurpleIndex]);
            }
            else
            {
                if (SelfpurpleIndex != -1)
                {
                    ItemClick(mgrid_selfEquip.controlList[SelfpurpleIndex]);
                }
                else
                {
                    ItemClick(mgrid_selfEquip.controlList[0]);
                }
            }
        }
        RefreshBtnRed(panelType.selfEquip);
    }
    void SwitchToNoEquipState(bool _state)
    {
        mlb_Noequip.gameObject.SetActive(_state);
        if (_state)
        {
            if (curItem != null)
            {
                curItem.ChangeChooseState(false);
            }
            curItem = null;
            if (showItem != null) { showItem.obj.SetActive(false); }
            if (attrList != null)
            {
                for (int i = 0; i < attrList.Count; i++)
                {
                    attrList[i].UnInit();
                    attrList[i].go.SetActive(true);
                }
            }
            mlb_moneyNum.text = "";
            msp_goodsIcon.spriteName = "";
            msp_goodsBg.spriteName = "";
            mlb_goodsNum.text = "";
            mobj_cost.SetActive(false);
        }
        else
        {
            mobj_cost.SetActive(true);
        }
    }
    void ItemClick(GameObject _go)
    {
        if (curItem != null)
        {
            curItem.ChangeChooseState(false);
        }
        RefineItem item = (RefineItem)UIEventListener.Get(_go).parameter;
        curItem = item;
        curItem.ChangeChooseState(true);
        Showitem();
        ShowRandomAttr();
        ShowCost();
        Timer.Instance.CancelInvoke(schedule);
    }
    void Showitem()
    {
        if (showItem == null) { showItem = UIItemManager.Instance.GetItem(PropItemType.Normal, mobj_showItem); }
        showItem.obj.SetActive(true);
        showItem.Refresh(curItem.info);
    }
    void ShowRandomAttr()
    {
        ranattrList = StructTipData.Instance.GetEquipRandomDisjunctData(curItem.cfg, curItem.info.randAttrValues);
        for (int i = 0; i < attrList.Count; i++)
        {
            if (i < ranattrList.ts.Count)
            {
                attrList[i].Refresh(ranattrList.ts[i], curItem.info.quality);
            }
            else
            {
                attrList[i].UnInit();
            }
        }
    }
    int costMoneyId = 0;
    int costItemId = 0;
    void ShowCost()
    {
        if (curItem == null)
        {
            return;
        }
        if (curItem.info.quality == 4)
        {
            SetValueChange(3);
        }
        else if (curItem.info.quality < 4)
        {
            SetValueChange(2);
        }
        else if (curItem.info.quality == 5)
        {
            SetValueChange(1);
        }
        if (setValue == 3)
        {
            TABLE.CHONGZHUTOPCOST costData;
            if (ChongZhuTopCostTableManager.Instance.TryGetValue(curItem.cfg.levClass * 10000 + curItem.cfg.quality, out costData))
            {
                costMoneyId = costData.payType;
                costItemId = costData.costItemID;
                long money = CSItemCountManager.Instance.GetItemCount(costMoneyId);
                UIEventListener.Get(mobj_moneyAdd, costMoneyId).onClick = ShowGetWay;
                UIEventListener.Get(msp_moneyIcon.gameObject, costMoneyId).onClick = ShowTips;
                long goods = CSBagInfo.Instance.GetItemCount(costItemId);
                mlb_moneyNum.text = costData.price.ToString();
                msp_moneyIcon.spriteName = $"tubiao{costData.payType}";
                msp_goodsIcon.spriteName = $"tubiao{ItemTableManager.Instance.GetItemIcon(costItemId)}";
                UIEventListener.Get(mbtn_goods1Buy, costItemId).onClick = ShowGetWay;
                UIEventListener.Get(msp_goodsIcon.gameObject, costItemId).onClick = ShowTips;

                mlb_moneyNum.color = (money >= costData.price) ? CSColor.green : CSColor.red;
                mlb_goodsNum.text = $"{UtilityMath.GetEquipRecastDecimalValue(goods)}/{UtilityMath.GetEquipRecastDecimalValue(costData.num)}";
                mlb_goodsNum.color = (goods >= costData.num) ? CSColor.green : CSColor.red;
                UIEventListener.Get(mbtn_goods2Buy, costData.costItemID2).onClick = ShowGetWay;
                UIEventListener.Get(mobj_costgoodsIcon2.gameObject, costData.costItemID2).onClick = ShowTips;

                mobj_costgoods2.SetActive(true);
                long goods2 = CSBagInfo.Instance.GetItemCount(costData.costItemID2);
                mobj_costgoodsIcon2.spriteName = $"tubiao{ItemTableManager.Instance.GetItemIcon(costData.costItemID2)}";

                mobj_costgoodsNum2.text = $"{UtilityMath.GetEquipRecastDecimalValue(goods2)}/{UtilityMath.GetEquipRecastDecimalValue(costData.num2)}";
                mobj_costgoodsNum2.color = (goods2 >= costData.num2) ? CSColor.green : CSColor.red;


                isMoneyEnough = (money < costData.price) ? false : true;
                isCostItemEnough = (goods < costData.num || goods2 < costData.num2) ? false : true;
                isConstEnough = isMoneyEnough && isCostItemEnough ? true : false;
                if (curType == panelType.selfEquip)
                {
                    if (isConstEnough && curItem.info.quality != 5)
                    {
                        mred_recastBtn.SetActive(true);
                    }
                    else
                    {
                        mred_recastBtn.SetActive(false);
                    }
                }
                else
                {
                    mred_recastBtn.SetActive(false);
                }
            }
        }
        else
        {
            TABLE.CHONGZHUCOST costData;
            if (ChongZhuCostTableManager.Instance.TryGetValue(curItem.cfg.levClass + 1, out costData))
            {
                costMoneyId = costData.payType;
                costItemId = costData.costItemID;
                long money = CSItemCountManager.Instance.GetItemCount(costMoneyId);
                UIEventListener.Get(mobj_moneyAdd, costMoneyId).onClick = ShowGetWay;
                UIEventListener.Get(msp_moneyIcon.gameObject, costMoneyId).onClick = ShowTips;
                long goods = CSBagInfo.Instance.GetItemCount(costItemId);
                mlb_moneyNum.text = costData.price.ToString();
                msp_moneyIcon.spriteName = $"tubiao{costData.payType}";
                msp_goodsIcon.spriteName = $"tubiao{ItemTableManager.Instance.GetItemIcon(costItemId)}";
                UIEventListener.Get(mbtn_goods1Buy, costItemId).onClick = ShowGetWay;
                UIEventListener.Get(msp_goodsIcon.gameObject, costItemId).onClick = ShowTips;
                mobj_costgoods2.SetActive(false);
                mlb_goodsNum.text = $"{UtilityMath.GetEquipRecastDecimalValue(goods)}/{UtilityMath.GetEquipRecastDecimalValue(costData.num)}";
                mlb_moneyNum.color = (money >= costData.price) ? CSColor.green : CSColor.red;
                mlb_goodsNum.color = (goods >= costData.num) ? CSColor.green : CSColor.red;

                isMoneyEnough = (money < costData.price) ? false : true;
                isCostItemEnough = (goods < costData.num) ? false : true;
                isConstEnough = isMoneyEnough && isCostItemEnough ? true : false;
                if (curType == panelType.selfEquip)
                {
                    if (isConstEnough && curItem.info.quality != 5)
                    {
                        mred_recastBtn.SetActive(true);
                    }
                    else
                    {
                        mred_recastBtn.SetActive(false);
                    }
                }
                else
                {
                    mred_recastBtn.SetActive(false);
                }
            }
        }
        mtable_costTable.Reposition();
    }
    void GetRecastBack(uint id, object data)
    {
        if (data == null) return;
        bag.EquipRebuildNtf msg = (bag.EquipRebuildNtf)data;

        curItem.info = msg.equip.equip;
        curItem.cfg = ItemTableManager.Instance.GetItemCfg(curItem.info.configId);
        curItem.index = msg.equip.position;
        ShowRandomAttr();
        ShowCost();
        Showitem();
        curItem.ResetQuality(msg.equip.equip.quality);
        if (curType == panelType.selfEquip)
        {
            curItem.RefreshRed();
        }
        FNDebug.Log("   重铸结果返回   " + msg.equip.equip.quality + "   " + isConstEnough);
        if (curType == panelType.selfEquip)
        {
            selfquips = CSBagInfo.Instance.GetEquipRecastItemData();
        }
        else
        {
            CSBagInfo.Instance.GetAllRecastBagEquip(bagequips);
        }
        UtilityTips.ShowTips(CSString.Format(528, quaSttr[msg.equip.equip.quality - 1]), 1.5f, ColorType.White);
        RefreshBtnRed(panelType.selfEquip);
        int result = 0;
        if (setValue == 2) { result = 4; }
        else { isContinue = false; }
        CSEffectPlayMgr.Instance.ShowUIEffect(meff_reacast, "gz_boom", 15, false);
        if (msg.equip.equip.quality >= result)
        {
            if (isContinue)
            {
                ConeqCancelRecast(mbtn_cancelConrecast);
            }
            return;
        }
        if (!isConstEnough)
        {
            if (isContinue)
            {
                ConeqCancelRecast(mbtn_cancelConrecast);
            }
            return;
        }
        if (isContinue)
        {
            if (!Timer.Instance.IsInvoking(schedule))
            {
                schedule = Timer.Instance.Invoke(intervalTime, DealyReqRecast);
            }
        }
    }
    void GetItemChange(uint id, object data)
    {
        ShowCost();
        RefreshBtnRed(panelType.bagEquip);
        RefreshBtnRed(panelType.selfEquip);
    }
    void ReqRecast(GameObject go)
    {
        if (curItem.info.quality == 5)
        {
            UtilityTips.ShowTips(string.Concat(CSString.Format(529, quaSttr[curItem.info.quality - 1])), 1.5f, ColorType.White);
            return;
        }
        if (!isConstEnough)
        {
            if (!isCostItemEnough)
            {
                Utility.ShowGetWay(costItemId);
                return;

            }
            if (!isMoneyEnough)
            {
                Utility.ShowGetWay(costMoneyId);
                return;
            }
        }
        if (curItem == null)
        {
            return;
        }

        int result = 0;
        if (setValue == 2) { result = 4; }
        if (curItem.info.quality == result)
        {
            UtilityTips.ShowTips(string.Concat(CSString.Format(529, quaSttr[curItem.info.quality - 1])), 1.5f, ColorType.White);
            return;
        }
        isContinue = false;
        if (setValue == 3)
        {
            Net.ReqEquipAutoRebuildMessage(curItem.index);
        }
        else
        {
            Net.EquipRebuildReqMessage(curItem.index);
        }
    }
    void ConeqRecast(GameObject go)
    {
        if (curItem == null) { return; }
        if (curItem.info.quality == 5)
        {
            UtilityTips.ShowTips(string.Concat(CSString.Format(529, quaSttr[curItem.info.quality - 1])), 1.5f, ColorType.White);
            return;
        }
        if (!isConstEnough)
        {
            if (!isCostItemEnough)
            {
                Utility.ShowGetWay(costItemId);
                return;

            }
            if (!isMoneyEnough)
            {
                Utility.ShowGetWay(costMoneyId);
                return;
            }
        }
        if (isContinue) { return; }

        int result = 0;
        if (setValue == 2) { result = 4; }
        if (curItem.info.quality == result)
        {
            UtilityTips.ShowTips(string.Concat(CSString.Format(529, quaSttr[curItem.info.quality - 1])), 1.5f, ColorType.White);
            return;
        }
        isContinue = true;
        mbtn_Conrecast.SetActive(false);
        mbtn_cancelConrecast.SetActive(true);
        Net.EquipRebuildReqMessage(curItem.index);
    }
    void ConeqCancelRecast(GameObject go)
    {
        mbtn_Conrecast.SetActive(true);
        mbtn_cancelConrecast.SetActive(false);
        isContinue = false;
        Timer.Instance.CancelInvoke(schedule);
    }

    void ChangeAutoStete(bool _state)
    {
        mbtn_Conrecast.SetActive(!_state);
        mbtn_recast.SetActive(_state);
        mbtn_cancelConrecast.SetActive(false);
    }
    void DealyReqRecast(Schedule _schedule)
    {
        Timer.Instance.CancelInvoke(schedule);
        schedule = null;
        Net.EquipRebuildReqMessage(curItem.index);
    }
    void InstructionClick(GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.EQUIP_RECAST);
    }
    string[] setStr;
    void SettingBtnClick(GameObject go)
    {
        if (isContinue) { return; }
        mobj_setting.gameObject.SetActive(!mobj_setting.activeSelf);
    }
    void GetWayClick(GameObject go)
    {
        Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(423));
    }
    void ShowGetWay(GameObject _go)
    {
        int id = (int)UIEventListener.Get(_go).parameter;
        Utility.ShowGetWay(id);
    }
    void ShowTips(GameObject _go)
    {
        int id = (int)UIEventListener.Get(_go).parameter;
        UITipsManager.Instance.CreateTips(TipsOpenType.Normal, id);
    }
    void InitSetting()
    {
        // 1 关闭  2紫色  3橙色
        if (setStr == null) { setStr = SundryTableManager.Instance.GetSundryEffect(28).Split('#'); }
        for (int i = 0; i < mobj_settingPar.transform.childCount; i++)
        {
            UIEventListener.Get(mobj_settingPar.transform.GetChild(i).gameObject, i + 1).onClick = SetValueClick;
            UILabel label = mobj_settingPar.transform.GetChild(i).Find("value").GetComponent<UILabel>();
            label.text = setStr[i];
        }
    }
    void SetValueClick(GameObject _go)
    {
        mobj_setting.gameObject.SetActive(!mobj_setting.activeSelf);
        int value = (int)UIEventListener.Get(_go).parameter;
        SetValueChange(value);
        ShowCost();
    }
    void SetValueChange(int _setValue)
    {
        setValue = _setValue;
        if (setValue == 1)
        {
            isContinue = false;
            Timer.Instance.CancelInvoke(schedule);
        }
        mlb_setting.text = setStr[setValue - 1];
        ChangeAutoStete(setValue == 2 ? false : true);
    }

}
public class RecastAttrItem
{
    public GameObject go;
    public UILabel name;
    public UILabel value;
    public UILabel maxValue;
    public UILabel skillDes;
    public GameObject bg;
    bool HasInit = false;
    int skillId = 0;
    int ind = 0;
    public RecastAttrItem(GameObject _obj, int _ind)
    {
        go = _obj;
        ind = _ind;
    }
    void GetComponent()
    {
        if (HasInit)
        {
            return;
        }
        name = go.transform.Find("name").GetComponent<UILabel>();
        value = go.transform.Find("value").GetComponent<UILabel>();
        maxValue = go.transform.Find("maxvalue").GetComponent<UILabel>();
        skillDes = go.transform.Find("skilldes").GetComponent<UILabel>();
        bg = go.transform.Find("bg").gameObject;
        bg.SetActive(ind % 2 == 0 ? true : false);
        HasInit = true;
        UIEventListener.Get(skillDes.gameObject).onClick = SkillShow;
    }
    public void Refresh(EquipRefineProperty _str, int _qua)
    {
        GetComponent();
        if (!go.activeSelf) { go.SetActive(true); }
        name.text = _str.name;

        if (_str.data[0].type == 4)
        {
            skillDes.text = _str.strValue;
            skillDes.gameObject.SetActive(true);
            value.text = "";
            maxValue.text = "";
            name.color = CSColor.white;
            skillId = _str.data[0].param1;
            //name.color = UtilityCsColor.Instance.GetColor(_str.quality);
            //value.color = UtilityCsColor.Instance.GetColor(_str.quality);
        }
        else
        {
            skillDes.text = "";
            skillDes.gameObject.SetActive(false);
            value.text = _str.strValue;
            maxValue.text = _str.strMaxValue;
            name.color = UtilityCsColor.Instance.GetColor(_str.quality);
            value.color = UtilityCsColor.Instance.GetColor(_str.quality);
        }
        value.transform.localPosition = new Vector3(name.transform.localPosition.x + name.width + 4, 0, 0);
        skillDes.transform.localPosition = new Vector3(name.transform.localPosition.x + name.width + 6, 0, 0);
    }
    void SkillShow(GameObject _go)
    {
        UISkillTipsPanel.CreateHelpTipsPanel(skillId);
    }
    public void UnInit()
    {
        GetComponent();
        go.SetActive(false);
        name.text = "暂无属性：";
        name.color = CSColor.gray;
        value.text = "";
        maxValue.text = "";
        skillDes.text = "";
        skillDes.gameObject.SetActive(false);
        UIEventListener.Get(skillDes.gameObject).onClick = null;
    }
}
