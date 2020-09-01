using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public partial class UIEquipRefinePanel : UIBasePanel
{
    enum panelType
    {
        bagEquip,
        selfEquip,
    }
    #region variable
    Dictionary<int, bag.BagItemInfo> bagequips = new Dictionary<int, bag.BagItemInfo>();
    Dictionary<int, bag.BagItemInfo> selfquips = new Dictionary<int, bag.BagItemInfo>();
    List<RefineItem> bagRefineList = new List<RefineItem>();
    List<RefineItem> selfRefineList = new List<RefineItem>();

    List<TABLE.XILIANLISTSHOW> maxRandomAttrList = new List<TABLE.XILIANLISTSHOW>();
    List<RefineMaxValueShow> maxRefinevalueList = new List<RefineMaxValueShow>();
    List<bool> lockState = new List<bool>();

    int lockNum = 0;
    TABLE.XILIANCOST costData;
    int refineNum = 1;
    bool canRefine = true;

    EquipRefineProDic ranattrList;
    //TipDataItem skillList;
    //panelType curType;

    RefineItem curItem;
    FastArrayElementFromPool<RefineRandomValueShow> randomValueList;
    UIItemBase itemShow;
    #endregion
    public override void Init()
    {
        base.Init();
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_texbg, "fr_xilian");
        mClientEvent.Reg((uint)CEvent.EquipXiLianNtfMessage, GetXiLianMesBack);
        mClientEvent.Reg((uint)CEvent.SCChooseXiLianResultNtf, GetChooseResult);
        mClientEvent.Reg((uint)CEvent.RefineResultClose, GetRefineResultClose);
        mClientEvent.AddEvent(CEvent.ItemListChange, GetItemChange);
        UIEventListener.Get(mbtn_waerEquips, 1).onClick = ChangeEuqipsBtnClick;
        UIEventListener.Get(mbtn_bagEquips, 2).onClick = ChangeEuqipsBtnClick;
        UIEventListener.Get(mbtn_add).onClick = RefineNumAddClick;
        UIEventListener.Get(mbtn_reduce).onClick = RefineNumReduceClick;
        UIEventListener.Get(mbtn_Refine).onClick = RefineBtnClick;
        UIEventListener.Get(mobj_MaxshowBg, false).onClick = MaxAttrShowClick;
        UIEventListener.Get(mbtn_MaxshowBg, true).onClick = MaxAttrShowClick;
        UIEventListener.Get(mbtn_help).onClick = HelpClick;
        UIEventListener.Get(mobj_getWay.gameObject).onClick = GetWayClick;
        mbar_attr.onChange.Add(new EventDelegate(OnAttrChange));
        mbar_skill.onChange.Add(new EventDelegate(OnSkillChange));
        mgrid_curGrid.MaxCount = 8;
        randomValueList = mPoolHandleManager.CreateGeneratePool<RefineRandomValueShow>();
        randomValueList.Count = 8;
        for (int i = 0; i < 8; i++)
        {
            randomValueList[i].ResetObj(mgrid_curGrid.controlList[i], i);
        }
        for (int i = 0; i < 8; i++)
        {
            lockState.Add(false);
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
            ChangeEuqipsBtnClick(mbtn_bagEquips);
            for (int i = 0; i < bagRefineList.Count; i++)
            {
                if (bagRefineList[i].info.id == _data.info.id)
                {
                    ItemClick(mgrid_BagEquip.controlList[i]);
                    temp_ind = i;
                    break;
                }
            }
            if (temp_ind > 4)
            {
                SpringPanel.Begin(mobj_BagEquip, new Vector3(-347, mobj_BagEquip.transform.localPosition.y + 90 * (temp_ind - 4)), 20f);
            }
        }
        else if (_data.openType == TipsOpenType.RoleEquip)
        {
            ChangeEuqipsBtnClick(mbtn_waerEquips);
            for (int i = 0; i < selfRefineList.Count; i++)
            {
                if (selfRefineList[i].info.id == _data.info.id)
                {
                    ItemClick(mgrid_SelfEquip.controlList[i]);
                    temp_ind = i;
                    break;
                }
            }
            if (temp_ind > 4)
            {
                SpringPanel.Begin(mobj_SelfEqiup, new Vector3(-347, mobj_SelfEqiup.transform.localPosition.y + 90 * (temp_ind - 4)), 20f);
            }
        }
    }
    public override void Show()
    {
        base.Show();
        selfquips.Clear();
        selfquips = CSBagInfo.Instance.GetEquipRefineItemData();
        if (selfquips.Count != 0)
        {
            ChangeEuqipsBtnClick(mbtn_waerEquips);
        }
        else
        {
            bagequips.Clear();
            CSBagInfo.Instance.GetAllRefineBagEquip(bagequips);
            if (bagequips.Count != 0)
            {
                ChangeEuqipsBtnClick(mbtn_bagEquips);
            }
            else
            {
                ChangeEuqipsBtnClick(mbtn_waerEquips);
            }
        }
    }
    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mtex_texbg);
        bagequips = null;
        selfquips = null;
        bagRefineList = null;
        selfRefineList = null;
        maxRandomAttrList = null;
        maxRefinevalueList = null;
        lockNum = 0;
        costData = null;
        refineNum = 1;
        canRefine = true;
        curItem = null;
        randomValueList = null;
        if (ranattrList != null) { StructTipData.Instance.RecycleSingle(ranattrList); };
        //if (skillList != null) { StructTipData.Instance.RecycleSingle(skillList); };
        if (itemShow != null) { UIItemManager.Instance.RecycleSingleItem(itemShow); }
        base.OnDestroy();
    }

    void ChangeEuqipsBtnClick(GameObject _go)
    {
        int type = (int)UIEventListener.Get(_go).parameter;
        if (type == 1)
        {
            mobj_SelfEqiup.SetActive(true);
            mbtn_waerEquipsHL.SetActive(true);
            mobj_BagEquip.SetActive(false);
            mbtn_bagEquipsHL.SetActive(false);
            RefreshSlefEquipGrid();
        }
        else
        {
            mobj_SelfEqiup.SetActive(false);
            mbtn_waerEquipsHL.SetActive(false);
            mobj_BagEquip.SetActive(true);
            mbtn_bagEquipsHL.SetActive(true);
            RefreshBagEquipGrid();
        }
    }
    void RefreshBagEquipGrid()
    {
        //curType = panelType.bagEquip;
        bagRefineList.Clear();
        bagequips.Clear();
        CSBagInfo.Instance.GetAllRefineBagEquip(bagequips);
        if (bagequips.Count == 0)
        {
            SwitchToNoEquipState(true);
            return;
        }
        SwitchToNoEquipState(false);
        mgrid_BagEquip.MaxCount = bagequips.Count;
        int i = 0;
        var iter = bagequips.GetEnumerator();
        while (iter.MoveNext())
        {
            bagRefineList.Add(new RefineItem(mgrid_BagEquip.controlList[i], ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId),
                iter.Current.Value, ItemClick, iter.Current.Key, 1));
            i++;
        }
        if (mgrid_BagEquip.controlList.Count > 0)
        {
            ItemClick(mgrid_BagEquip.controlList[0]);
        }
        RefreshBtnRed(panelType.bagEquip);
    }
    void RefreshSlefEquipGrid()
    {
        //curType = panelType.selfEquip;
        selfquips = CSBagInfo.Instance.GetEquipRefineItemData();
        if (selfquips.Count == 0)
        {
            SwitchToNoEquipState(true);
            return;
        }
        SwitchToNoEquipState(false);
        mgrid_SelfEquip.MaxCount = selfquips.Count;
        int i = 0;
        var iter = selfquips.GetEnumerator();
        while (iter.MoveNext())
        {
            selfRefineList.Add(new RefineItem(mgrid_SelfEquip.controlList[i], ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId),
                iter.Current.Value, ItemClick, -iter.Current.Key, 1));
            i++;
        }
        if (mgrid_SelfEquip.controlList.Count > 0)
        {
            ItemClick(mgrid_SelfEquip.controlList[0]);
        }
        RefreshBtnRed(panelType.selfEquip);
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
            //        hasRed = CSBagInfo.Instance.IsEquipRefineCostEnough(iter.Current.Value, ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId));
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
            mred_bag.SetActive(false);
        }
        else
        {
            var iter = selfquips.GetEnumerator();
            while (iter.MoveNext())
            {
                if (hasRed == false)
                {
                    hasRed = CSBagInfo.Instance.IsEquipRefineCostEnough(iter.Current.Value, ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId));
                }
                else
                {
                    break;
                }
            }
            mred_self.SetActive(hasRed);
            for (int i = 0; i < selfRefineList.Count; i++)
            {
                selfRefineList[i].RefreshRed();
            }
        }
    }

    void SwitchToNoEquipState(bool _state)
    {
        mbtn_MaxshowBg.SetActive(!_state);
        mlb_noEquip.gameObject.SetActive(_state);
        if (_state)
        {
            if (curItem != null)
            {
                curItem.ChangeChooseState(false);
            }
            curItem = null;
            if (itemShow != null) { itemShow.obj.SetActive(false); }
            if (randomValueList != null)
            {
                for (int i = 0; i < randomValueList.Count; i++)
                {
                    randomValueList[i].UnInit(true);
                    randomValueList[i].go.SetActive(true);
                }
            }
            mlb_coseMoney.text = "";
            mlb_coseGoods.text = "";
            msp_goodsIcon.spriteName = "";
            refineNum = 1;
            lockNum = 0;
            RefreshRefineBtn();
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
        if (itemShow == null) { itemShow = UIItemManager.Instance.GetItem(PropItemType.Normal, mobj_itemPar); }
        itemShow.obj.SetActive(true);
        itemShow.Refresh(curItem.info);
        lockNum = 0;
        RefreshCost();
        if (!isfresh)
        {
            for (int i = 0; i < lockState.Count; i++)
            {
                lockState[i] = false;
            }
        }
        curItem.RefreshRed();
        RefreshCurValue();
    }

    void RefreshMaxValue()
    {
        if (curItem != null)
        {
            maxRandomAttrList = XiLianListShowTableManager.Instance.GetCurrentData(curItem.cfg.subType, curItem.cfg.levClass, 1);
            mgrid_maxAttrGrid.MaxCount = maxRandomAttrList.Count;
            if (maxRefinevalueList.Count < maxRandomAttrList.Count)
            {
                int gap1 = maxRandomAttrList.Count - maxRefinevalueList.Count;
                for (int j = 1; j <= gap1; j++)
                {
                    maxRefinevalueList.Add(new RefineMaxValueShow());
                }
            }

            for (int i = 0; i < maxRandomAttrList.Count; i++)
            {
                maxRefinevalueList[i].Init(mgrid_maxAttrGrid.controlList[i], i, maxRandomAttrList[i]);
            }

            //List<TABLE.XILIANLISTSHOW> maxRandomSkillList = XiLianListShowTableManager.Instance.GetCurrentData(curItem.cfg.subType, curItem.cfg.levClass, 2);

            //int gap = maxRandomSkillList.Count - mgrid_maxSkill.GetChildList().Count;
            //if (gap > 0)
            //{
            //    for (int i = 0; i < gap; i++)
            //    {
            //        GameObject.Instantiate(mobj_skillPrefab, mgrid_maxSkill.transform);
            //    }
            //}
            //for (int i = 0; i < mgrid_maxSkill.GetChildList().Count; i++)
            //{
            //    UILabel name = mgrid_maxSkill.GetChildList()[i].transform.Find("name").GetComponent<UILabel>();
            //    UILabel des = mgrid_maxSkill.GetChildList()[i].transform.Find("maxValue").GetComponent<UILabel>();
            //    name.text = SkillTableManager.Instance.GetSkillName(maxRandomSkillList[i].keyid);
            //    des.text = SkillTableManager.Instance.GetDescription(maxRandomSkillList[i].keyid);
            //    GameObject line = mgrid_maxSkill.GetChildList()[i].transform.Find("line").gameObject;
            //    line.transform.localPosition = new Vector3(47, 6 - des.height, 0);
            //}
            //mgrid_maxSkill.Reposition();
            //if (curItem.info.quality == 5)
            //{
            //mmax_skill.gameObject.SetActive(true);
            //mmax_attr.transform.localPosition = new Vector3(-170, 0, 0);
            //}
            //else
            //{
            mmax_skill.gameObject.SetActive(false);
            mmax_attr.transform.localPosition = Vector3.zero;
            //}
            UIScrollView panel1 = mgrid_maxAttrGrid.transform.parent.GetComponent<UIScrollView>();
            //UIScrollView panel2 = mgrid_maxSkill.transform.parent.GetComponent<UIScrollView>();
            panel1.ResetPosition();
            //panel2.ResetPosition();
        }
    }

    void RefreshCurValue()
    {
        if (curItem != null)
        {
            ranattrList = StructTipData.Instance.GetEquipRandomDisjunctData(curItem.cfg, curItem.info.randAttrValues);
            for (int i = 0; i < randomValueList.Count; i++)
            {
                if (i < ranattrList.ts.Count)
                {
                    randomValueList[i].Init(ranattrList.ts[i], curItem, RandomLockClick, lockState[i]);
                }
                else
                {
                    randomValueList[i].UnInit(false);
                }
            }
        }
    }
    bool isCostItemEnough = false;
    bool isMoneyEnough = false;
    int costMoneyId = 0;
    int costItemId = 0;
    long money;
    long goods;
    void RefreshCost()
    {
        if (curItem == null)
        {
            return;
        }
        if (curItem.info.freeXiLianCount > 0 && lockNum == 0)
        {
            mlb_free.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1613), $"{curItem.info.freeXiLianCount}/{curItem.info.freeXiLianCountMax}");
            mobj_pay.SetActive(false);
            canRefine = true;
            isCostItemEnough = true;
            isMoneyEnough = true;

            mred_refineBtn.SetActive(false);
            if (curItem.info.quality != 5)
            {
                bool isAllTopAttr = true;
                int qua = 0;
                for (int i = 0; i < curItem.info.randAttrValues.Count; i++)
                {
                    bag.RandAttr attr = curItem.info.randAttrValues[i];
                    if (attr.value2 != 0)
                    {
                        qua = (attr.configId1 > attr.configId2) ? attr.configId1 : attr.configId2;
                    }
                    else
                    {
                        qua = attr.configId1;
                    }
                    if (qua < 5)
                    {
                        isAllTopAttr = false;
                        break;
                    }
                }
                mred_refineBtn.SetActive((isAllTopAttr == true) ? false : canRefine);
            }
        }
        else
        {
            mlb_free.text = "";
            mobj_pay.SetActive(true);

            costData = XiLianCostTableManager.Instance.GetCfg(curItem.cfg.level);
            costMoneyId = (int)costData.payType;
            costItemId = (int)costData.costItemID;

            money = CSItemCountManager.Instance.GetItemCount(costMoneyId);
            UIEventListener.Get(mbtn_coseMoney, costMoneyId).onClick = ShowGetWay;
            UIEventListener.Get(mlb_coseIcon.gameObject, costMoneyId).onClick = ShowTips;
            goods = CSBagInfo.Instance.GetItemCount(costItemId);
            mlb_coseMoney.text = (costData.price[lockNum] * refineNum).ToString();
            mlb_coseIcon.spriteName = $"tubiao{costMoneyId}";
            CSStringBuilder.Clear();
            mlb_coseGoods.text = $"{goods}/{costData.num[lockNum] * refineNum}";
            mlb_coseMoney.color = (money >= costData.price[lockNum] * refineNum) ? CSColor.green : CSColor.red;
            mlb_coseGoods.color = (goods >= costData.num[lockNum] * refineNum) ? CSColor.green : CSColor.red;
            msp_goodsIcon.spriteName = $"tubiao{ItemTableManager.Instance.GetItemIcon(costItemId)}";
            UIEventListener.Get(msp_goodsIcon.gameObject, costItemId).onClick = ShowTips;
            UIEventListener.Get(mbtn_goodsBg.gameObject, costItemId).onClick = ShowGetWay;
            //UIEventListener.Get(mbtn_goodsBg.gameObject).onClick = (p) =>
            //{
            //    Utility.ShowGetWay(costItemId);
            //};
            isMoneyEnough = money >= costData.price[lockNum] * refineNum ? true : false;
            isCostItemEnough = goods >= costData.num[lockNum] * refineNum ? true : false;
            canRefine = isMoneyEnough && isCostItemEnough ? true : false;
            mred_refineBtn.SetActive(false);

            if (curItem.info.quality != 5)
            {
                bool isAllTopAttr = true;
                int qua = 0;
                for (int i = 0; i < curItem.info.randAttrValues.Count; i++)
                {
                    bag.RandAttr attr = curItem.info.randAttrValues[i];
                    if (attr.value2 != 0)
                    {
                        qua = (attr.configId1 > attr.configId2) ? attr.configId1 : attr.configId2;
                    }
                    else
                    {
                        qua = attr.configId1;
                    }
                    if (qua < 5)
                    {
                        isAllTopAttr = false;
                        break;
                    }
                }
                mred_refineBtn.SetActive((isAllTopAttr == true) ? false : canRefine);
            }
        }

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
    void RandomLockClick(RefineRandomValueShow _go)
    {
        if (curItem == null) { return; }
        if (_go.tick.activeSelf == false)
        {
            if (lockNum + 1 >= (curItem.info.randAttrValues.Count))
            {
                UtilityTips.ShowRedTips(531);
                return;
            }
        }
        _go.tick.SetActive(!_go.tick.activeSelf);
        _go.isChoose = !_go.isChoose;
        lockNum = (_go.isChoose == true) ? lockNum + 1 : lockNum - 1;
        lockState[_go.GetIndex()] = _go.GetIsChoose();
        RefreshCost();
    }

    int refineMaxNum = 0;
    void RefineNumAddClick(GameObject _go)
    {
        refineMaxNum = int.Parse(SundryTableManager.Instance.GetSundryEffect(29));
        refineNum++;
        refineNum = (refineNum >= refineMaxNum) ? refineMaxNum : refineNum;
        mlb_refineNum.text = refineNum.ToString();
        RefreshRefineBtn();
        RefreshCost();
    }
    void RefineNumReduceClick(GameObject _go)
    {
        refineNum--;
        refineNum = (refineNum <= 1) ? 1 : refineNum;
        RefreshRefineBtn();
        RefreshCost();
    }
    void RefreshRefineBtn()
    {
        mlb_refineNum.text = refineNum.ToString();
        mlb_RefineBtn.text = (refineNum > 1) ? "连续洗练" : "开始洗练";
    }
    void RefineBtnClick(GameObject _go)
    {
        if (curItem == null) { return; }
        if (lockNum >= (ranattrList.ts.Count))
        {
            UtilityTips.ShowRedTips(531);
            return;
        }
        if (!canRefine)
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
        bag.CSEquipXiLianReq data = CSProtoManager.Get<bag.CSEquipXiLianReq>();
        data.lockedAttrIndex.Clear();
        for (int i = 0; i < randomValueList.Count; i++)
        {
            lockState[i] = randomValueList[i].GetIsChoose();
            if (randomValueList[i].GetIsChoose())
            {
                data.lockedAttrIndex.Add(i);
            }
        }
        data.xiLianNum = refineNum;
        data.equipIndex = curItem.index;
        Net.EquipXiLianReqMessage(data);
    }
    void GetXiLianMesBack(uint id, object data)
    {
        bag.SCEquipRandomsNtf msg = (bag.SCEquipRandomsNtf)data;
        bool isExit = UIManager.Instance.IsPanel<UIRefineResultPanel>();
        curItem.info.freeXiLianCount = msg.freeXiLianRandomTimes;
        if (!isExit)
        {
            UIManager.Instance.CreatePanel<UIRefineResultPanel>(p =>
            {
                (p as UIRefineResultPanel).GetData(data, curItem, lockState);
            });
        }
        else
        {
            mClientEvent.SendEvent(CEvent.RefineResultRefresh, data);
        }
        if (curItem.info.freeXiLianCount > 0 && lockNum == 0)
        {
            mlb_free.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1613), $"{curItem.info.freeXiLianCount}/{curItem.info.freeXiLianCountMax}");
        }
        else
        {
            mlb_free.text = "";
        }

        curItem.RefreshFreeNum();
        if (curItem.info.freeXiLianCount >= 0)
        {
            RefreshBtnRed(panelType.selfEquip);
            RefreshBtnRed(panelType.bagEquip);
            //for (int i = 0; i < selfRefineList.Count; i++)
            //{
            //    selfRefineList[i].RefreshRed();
            //}
            //for (int i = 0; i < bagRefineList.Count; i++)
            //{
            //    bagRefineList[i].RefreshRed();
            //}
        }
    }
    void GetChooseResult(uint id, object data)
    {
        UtilityTips.ShowTips(666, 1.5f, ColorType.Green);
        bag.EquipInfo info = (bag.EquipInfo)data;
        curItem.info = info.equip;
        isfresh = true;
        ItemClick(curItem.go);
        isfresh = false;
        if (curItem.info.freeXiLianCount > 0 && lockNum == 0)
        {
            mlb_free.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1613), $"{curItem.info.freeXiLianCount}/{curItem.info.freeXiLianCountMax}");
        }
        else
        {
            mlb_free.text = "";
        }

    }
    bool isfresh = false;
    void GetRefineResultClose(uint id, object data)
    {
        for (int i = 0; i < randomValueList.Count; i++)
        {
            randomValueList[i].ResetObj(mgrid_curGrid.transform.GetChild(i).gameObject, i);
        }
        EventData info = (EventData)data;
        isfresh = (bool)info.arg1;
        lockState = (List<bool>)info.arg2;
        if (isfresh) { ItemClick(curItem.go); }
        isfresh = false;
        lockNum = 0;
        for (int i = 0; i < lockState.Count; i++)
        {
            if (lockState[i]) { lockNum++; }
        }
        RefreshCost();
    }
    void GetItemChange(uint id, object data)
    {
        RefreshCost();
        RefreshBtnRed(panelType.selfEquip);
        RefreshBtnRed(panelType.bagEquip);
        for (int i = 0; i < selfRefineList.Count; i++)
        {
            selfRefineList[i].RefreshRed();
        }
    }
    void MaxAttrShowClick(GameObject _go)
    {
        bool state = (bool)UIEventListener.Get(_go).parameter;
        mobj_Maxshow.SetActive(state);
        if (state)
        {
            RefreshMaxValue();
        }
    }
    void HelpClick(GameObject _go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.EQUIP_REFINE);
    }
    void GetWayClick(GameObject go)
    {
        Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(422));
    }
    void OnAttrChange()
    {
        if (mbar_attr.value >= 0.95)
        {
            mobj_arrowAttr.SetActive(false);
        }
        else
        {
            mobj_arrowAttr.SetActive(true);
        }
    }
    void OnSkillChange()
    {
        if (mbar_skill.value >= 0.95)
        {
            mobj_arrowSkill.SetActive(false);
        }
        else
        {
            mobj_arrowSkill.SetActive(true);
        }
    }
}

public class RefineItem
{
    public GameObject go;
    public UILabel lb_name;
    public UILabel free_Num;
    public GameObject obj_bind;
    public GameObject obj_choose;
    public GameObject obj_item;
    public UISprite itembg;
    public UISprite itemicon;
    public UILabel itemSuit;
    public GameObject item;
    public GameObject redpoint;
    public bag.BagItemInfo info;
    public TABLE.ITEM cfg;
    Action<GameObject> dele;
    public int index;
    /// <summary>
    /// type 0 重铸  1洗练
    /// </summary>
    int type = 0;

    public UISprite bg;
    public UISprite icon;

    public RefineItem(GameObject _go, TABLE.ITEM _cfg, bag.BagItemInfo _info, Action<GameObject> _dele, int _index, int _type = 0)
    {
        go = _go;
        cfg = _cfg;
        info = _info;
        dele = _dele;
        index = _index;
        type = _type;
        Init();
    }
    public void Init()
    {
        lb_name = go.transform.Find("name").GetComponent<UILabel>();
        free_Num = go.transform.Find("freeNum").GetComponent<UILabel>();
        obj_bind = go.transform.Find("bind").gameObject;
        obj_choose = go.transform.Find("choose").gameObject;
        item = go.transform.Find("Item").gameObject;
        itembg = go.transform.Find("Item/frame").GetComponent<UISprite>();
        itemicon = go.transform.Find("Item/icon").GetComponent<UISprite>();
        redpoint = go.transform.Find("redPoint").gameObject;
        itemSuit = go.transform.Find("Item/lb_suit").GetComponent<UILabel>();
        UIEventListener.Get(go, this).onClick = dele;
        UIEventListener.Get(item).onClick = IconClick;
        Refresh();
    }
    public void UnInit()
    {

    }
    public void ResetQuality(int _quality)
    {
        info.quality = _quality;
        itembg.spriteName = ItemTableManager.Instance.GetItemQualityBG((int)info.quality);
        lb_name.color = UtilityCsColor.Instance.GetColor(info.quality);
    }
    public void Refresh()
    {
        CSStringBuilder.Clear();
        obj_bind.SetActive(info.bind == 1);
        lb_name.text = cfg.name;
        lb_name.color = UtilityCsColor.Instance.GetColor(info.quality);
        itembg.spriteName = ItemTableManager.Instance.GetItemQualityBG((int)info.quality);
        itemicon.spriteName = cfg.icon;
        if (CSBagInfo.Instance.IsNormalEquip(cfg) && cfg.level >= 30)
        {
            itemSuit.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(2027), UtilityMath.GetTenMultiple(cfg.level));
        }
        else
        {
            itemSuit.text = "";
        }
        RefreshFreeNum();
        //RefreshRed();
    }
    public void RefreshFreeNum()
    {
        if (type == 1)
        {
            if (info.freeXiLianCount > 0)
            {
                free_Num.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1613), info.freeXiLianCount);
            }
            else
            {
                free_Num.text = "";
            }
        }
        else
        {
            free_Num.text = "";
        }
    }
    public void RefreshRed()
    {
        if (type == 1)
        {
            redpoint.SetActive(CSBagInfo.Instance.IsEquipRefineCostEnough(info, cfg));
        }
        else
        {
            redpoint.SetActive(CSBagInfo.Instance.IsEquipRecastCostEnough(info, cfg));
        }
    }
    void IconClick(GameObject _go)
    {
        UITipsManager.Instance.CreateTips(TipsOpenType.Normal, cfg, info);
    }
    public void ChangeChooseState(bool _state)
    {
        obj_choose.SetActive(_state);
    }
}

public class RefineMaxValueShow
{
    public GameObject go;
    public UILabel name;
    public UILabel maxValue;
    public UILabel limit;
    GameObject sp;
    TABLE.XILIANLISTSHOW data;
    public void Init(GameObject _obj, int _index, TABLE.XILIANLISTSHOW _data)
    {
        go = _obj;
        data = _data;
        name = go.transform.Find("name").GetComponent<UILabel>();
        maxValue = go.transform.Find("maxValue").GetComponent<UILabel>();
        limit = go.transform.Find("limit").GetComponent<UILabel>();
        sp = go.transform.Find("Sprite").gameObject;

        sp.SetActive((_index % 2 == 0) ? true : false);
        name.text = $"[{data.color}]{ClientTipsTableManager.Instance.GetClientTipsContext(data.keyid)}";
        maxValue.text = $"[{data.color}]{data.parameter}";
        limit.text = $"[{data.color}]{data.Max}";
    }

    public void UnInit()
    {

    }
}

public class RefineRandomValueShow
{
    public RefineRandomValueShow()
    {

    }
    public GameObject go;

    public UILabel name;
    public UILabel value;
    public UILabel maxValue;
    public UILabel skillDes;
    public GameObject bg;



    public GameObject choose;
    public GameObject tick;
    public GameObject effect;
    public bool isChoose = false;
    public int index = 0;
    public int id = 0;
    public bool HasInit = false;
    public bool hasUninit = false;
    public int qua = 1;
    public int lockNum = 0;
    public RefineItem refine;
    Action<RefineRandomValueShow> action;
    EquipRefineProperty info;
    int skillId = 0;
    public RefineRandomValueShow(GameObject _obj)
    {
        go = _obj;
    }
    public void ResetObj(GameObject _obj, int _index)
    {
        go = _obj;
        name = go.transform.Find("name").GetComponent<UILabel>();
        value = go.transform.Find("value").GetComponent<UILabel>();
        maxValue = go.transform.Find("maxvalue").GetComponent<UILabel>();
        skillDes = go.transform.Find("skilldes").GetComponent<UILabel>();
        choose = go.transform.Find("choose").gameObject;
        tick = go.transform.Find("choose/tick").gameObject;
        bg = go.transform.Find("bg").gameObject;
        index = _index;
        bg.SetActive(index % 2 == 0);
        UIEventListener.Get(skillDes.gameObject).onClick = SkillShow;
    }
    public void Init(EquipRefineProperty _str, RefineItem _refine, Action<RefineRandomValueShow> _action, bool _lock = false)
    {
        info = _str;
        action = _action;
        refine = _refine;
        go.SetActive(true);
        name.text = info.name;
        if (info.data[0].type == 2)
        {
            skillDes.gameObject.SetActive(true);
            skillDes.text = info.strValue;
            value.text = "";
            maxValue.text = "";
            //name.color = UtilityCsColor.Instance.GetColor(_str.quality);
            //value.color = UtilityCsColor.Instance.GetColor(_str.quality);
            name.color = CSColor.white;
            skillId = info.data[0].param1;
        }
        else
        {
            skillDes.gameObject.SetActive(false);
            skillDes.text = "";
            value.text = info.strValue;
            maxValue.text = info.strMaxValue;
            name.color = UtilityCsColor.Instance.GetColor(info.quality);
            value.color = UtilityCsColor.Instance.GetColor(info.quality);
        }
        tick.SetActive(_lock);
        isChoose = _lock;
        UIEventListener.Get(choose).onClick = Click;
    }
    void SkillShow(GameObject _go)
    {
        UISkillTipsPanel.CreateHelpTipsPanel(skillId);
    }
    public void UnInit(bool _active = false)
    {
        name.text = "当前装备暂无此条属性";
        name.color = CSColor.gray;
        value.text = "";
        maxValue.text = "";
        skillDes.text = "";
        skillDes.gameObject.SetActive(false);
        isChoose = false;
        tick.SetActive(false);
        UIEventListener.Get(choose).onClick = null;
        //go.SetActive(false);
    }
    public void SetArrow(bool _state)
    {
        go.transform.Find("arrow").gameObject.SetActive(_state);
    }
    public bool GetIsChoose()
    {
        return isChoose;
    }
    public int GetIndex()
    {
        return index;
    }
    void Click(GameObject _go)
    {
        if (action != null)
        {
            action(this);
        }
    }
    public void ShowEffect()
    {
        effect = go.transform.Find("effect").gameObject;
        CSEffectPlayMgr.Instance.ShowUIEffect(effect, "effect_rune_levelup_add", 10, false);
    }
    public GameObject GetEffectObj()
    {
        return effect;
    }
}
