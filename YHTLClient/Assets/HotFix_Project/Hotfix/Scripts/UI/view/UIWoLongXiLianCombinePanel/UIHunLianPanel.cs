using Google.Protobuf.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIHunLianPanel : UIBasePanel
{
    enum panelType
    {
        bagEquip,
        selfEquip,
        none,
    }
    panelType curType = panelType.none;
    Dictionary<int, bag.BagItemInfo> bagequips = new Dictionary<int, bag.BagItemInfo>();
    Dictionary<int, bag.BagItemInfo> selfquips = new Dictionary<int, bag.BagItemInfo>();
    List<HunLianChooseItem> bagRefineList = new List<HunLianChooseItem>();
    List<HunLianChooseItem> selfRefineList = new List<HunLianChooseItem>();
    HunLianChooseItem curItem;
    UIItemBase showItem;
    int totalCount = 4;
    List<int> costList = new List<int>();
    //龙技
    FastArrayElementFromPool<WLRefineAttrValue> hunliOld;
    FastArrayElementFromPool<WLRefineAttrValue> hunliNew;
    //消耗
    IntArray moneyCost;
    IntArray goodsCost;
    bool isMoneyEnough = false;
    bool isGoodsEnough = true;
    Vector3 showNewLongJi = new Vector3(208, -287, 0);
    Vector3 notShowNewLongJi = new Vector3(108, -287, 0);
    float intervalTime = 0.08f;
    WaitForSeconds wait;

    public override void Init()
    {
        base.Init();
        mClientEvent.AddEvent(CEvent.SCWoLongXiLianMessage, GetXiLianRes);
        mClientEvent.AddEvent(CEvent.SCWoLongXiLianSelectMessage, SCWoLongXiLianSelectMessage);
        mClientEvent.AddEvent(CEvent.ItemListChange, GetItemChange);
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_bg, "wolong_bg");
        UIEventListener.Get(mbtn_help).onClick = HelpBtnClick;
        UIEventListener.Get(mbtn_waerEquips, panelType.selfEquip).onClick = ChangeEuqipsBtnClick;
        UIEventListener.Get(mbtn_bagEquips, panelType.bagEquip).onClick = ChangeEuqipsBtnClick;
        UIEventListener.Get(mbtn_Choose).onClick = ChooseClick;
        UIEventListener.Get(mbtn_refine).onClick = ConfirmClick;
        UIEventListener.Get(mbtn_moneyBuy).onClick = CostMoneyClick;
        UIEventListener.Get(msp_moneyIcon.gameObject).onClick = TipsMoneyClick;
        UIEventListener.Get(mbtn_goodsBuy).onClick = CostGoodsClick;
        UIEventListener.Get(msp_goodsIcon.gameObject).onClick = TipsGoodsClick;
        UIEventListener.Get(mobj_getway).onClick = GetWayClick;

        showItem = UIItemManager.Instance.GetItem(PropItemType.Normal, mtrans_itemshowPar);
        #region 龙技初始化
        mobj_hunliOld.MaxCount = totalCount;
        hunliOld = mPoolHandleManager.CreateGeneratePool<WLRefineAttrValue>();
        hunliOld.Count = totalCount;
        for (int i = 0; i < totalCount; i++)
        {
            hunliOld[i].SetObj(mobj_hunliOld.controlList[i]);
        }
        mobj_hunliNew.MaxCount = totalCount;
        hunliNew = mPoolHandleManager.CreateGeneratePool<WLRefineAttrValue>();
        hunliNew.Count = totalCount;
        for (int i = 0; i < totalCount; i++)
        {
            hunliNew[i].SetObj(mobj_hunliNew.controlList[i], true);
        }
        moneyCost = IntArray.Default;
        goodsCost = IntArray.Default;
        #endregion
        wait = new WaitForSeconds(intervalTime);
        RefreshBtnRed(panelType.selfEquip);
        mlb_noEquipDes.text = ClientTipsTableManager.Instance.GetClientTipsContext(1808);
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
    public override void OnShow(int typeId = 0)
    {
        base.OnShow(typeId);
        mClientEvent.AddEvent(CEvent.SCWoLongXiLianMessage, GetXiLianRes);
        mClientEvent.AddEvent(CEvent.SCWoLongXiLianSelectMessage, SCWoLongXiLianSelectMessage);
        mClientEvent.AddEvent(CEvent.ItemListChange, GetItemChange);
    }
    public override void OnHide()
    {
        base.OnHide();
        mClientEvent.RemoveEvent(CEvent.SCWoLongXiLianMessage, GetXiLianRes);
        mClientEvent.RemoveEvent(CEvent.SCWoLongXiLianSelectMessage, SCWoLongXiLianSelectMessage);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, GetItemChange);
    }
    public override void Show()
    {
        base.Show();
        selfquips.Clear();
        CSBagInfo.Instance.GetSelfWoLongEquipData(selfquips);
        if (selfquips.Count != 0)
        {
            ChangeEuqipsBtnClick(mbtn_waerEquips);
        }
        else
        {
            bagequips.Clear();
            CSBagInfo.Instance.GetBagWoLongEquipData(bagequips);
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
        for (int i = 0; i < bagRefineList.Count; i++)
        {
            bagRefineList[i].Dispose();
        }
        bagRefineList.Clear();
        bagRefineList = null;
        for (int i = 0; i < selfRefineList.Count; i++)
        {
            selfRefineList[i].Dispose();
        }
        selfRefineList.Clear();
        selfRefineList = null;
        //魂力
        for (int i = 0; i < hunliOld.Count; i++)
        {
            hunliOld[i].Recycle();
        }
        for (int i = 0; i < hunliNew.Count; i++)
        {
            hunliNew[i].Recycle();
        }
        curItem = null;
        curType = panelType.selfEquip;
        UIItemManager.Instance.RecycleSingleItem(showItem);
        CSEffectPlayMgr.Instance.Recycle(mtex_bg);
        base.OnDestroy();
    }
    void ChangeEuqipsBtnClick(GameObject _go)
    {
        panelType type = (panelType)UIEventListener.Get(_go).parameter;
        if (curType == type)
        {
            return;
        }
        if (type == panelType.selfEquip)
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
        curType = panelType.bagEquip;
        CSBagInfo.Instance.GetBagWoLongEquipData(bagequips);
        mgrid_BagEquip.MaxCount = bagequips.Count;
        if (bagequips.Count == 0) { SwitchToNoEquipState(true); return; }
        SwitchToNoEquipState(false);
        if (bagequips.Count > bagRefineList.Count)
        {
            int gap = bagequips.Count - bagRefineList.Count;
            for (int j = 0; j < gap; j++)
            {
                bagRefineList.Add(new HunLianChooseItem());
            }
        }
        int i = 0;
        var iter = bagequips.GetEnumerator();
        while (iter.MoveNext())
        {
            bagRefineList[i].Init(mgrid_BagEquip.controlList[i], ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId),
                iter.Current.Value, ItemClick, iter.Current.Key, 1);
            i++;
        }

        if (bagequips.Count != 0)
        {
            ItemClick(mgrid_BagEquip.controlList[0]);
        }
    }
    void RefreshSlefEquipGrid()
    {
        curType = panelType.selfEquip;
        selfquips.Clear();
        CSBagInfo.Instance.GetSelfWoLongEquipData(selfquips);
        if (selfquips.Count == 0) { SwitchToNoEquipState(true); return; }
        SwitchToNoEquipState(false);
        mgrid_SelfEquip.MaxCount = selfquips.Count;
        if (selfquips.Count > selfRefineList.Count)
        {
            int gap = selfquips.Count - selfRefineList.Count;
            for (int j = 0; j < gap; j++)
            {
                selfRefineList.Add(new HunLianChooseItem());
            }
        }
        int i = 0;
        var iter = selfquips.GetEnumerator();
        while (iter.MoveNext())
        {
            selfRefineList[i].Init(mgrid_SelfEquip.controlList[i], ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId),
                iter.Current.Value, ItemClick, -iter.Current.Key, 1);
            i++;
        }
        if (selfquips.Count != 0)
        {
            ItemClick(mgrid_SelfEquip.controlList[0]);
        }
        RefreshBtnRed(panelType.selfEquip);
    }
    void SwitchToNoEquipState(bool _state)
    {
        mobj_noEquip.gameObject.SetActive(_state);
        mobj_cost.SetActive(!_state);
        if (_state)
        {
            if (curItem != null)
            {
                curItem.ChangeChooseState(false);
            }
            curItem = null;
            if (showItem != null) { showItem.obj.SetActive(false); }
            mlb_showitemName.text = "";
            for (int i = 0; i < hunliOld.Count; i++)
            {

                hunliOld[i].UnInit(true);
            }
            for (int i = 0; i < hunliNew.Count; i++)
            {

                hunliNew[i].UnInit(true);
            }
            mobj_noHunLi.SetActive(false);
        }
    }
    void ItemClick(GameObject _go)
    {
        HunLianChooseItem item = (HunLianChooseItem)UIEventListener.Get(_go).parameter;
        if (curItem == item)
        {
            return;
        }
        if (curItem != null)
        {
            curItem.ChangeChooseState(false);
        }
        curItem = item;
        showItem.obj.SetActive(true);
        showItem.Refresh(curItem.info);
        mlb_showitemName.text = curItem.cfg.name;
        mlb_showitemName.color = UtilityCsColor.Instance.GetColor(curItem.cfg.quality);
        curItem.ChangeChooseState(true);

        ShowLongJi();
        RefreshHunLiCost();

    }

    void ChooseClick(GameObject _go)
    {
        if (!Constant.ShowTipsOnceList.Contains(88))
        {
            UtilityTips.ShowPromptWordTips(88, () =>
            {
                Net.CSWoLongXiLianSelectMessage(curItem.index, 2);
            });
        }
        else
        {
            Net.CSWoLongXiLianSelectMessage(curItem.index, 2);
        }
    }
    void ConfirmClick(GameObject _go)
    {
        if (!isMoneyEnough)
        {
            Utility.ShowGetWay(moneyCost[0]);
            return;
        }
        if (!isGoodsEnough)
        {
            Utility.ShowGetWay(goodsCost[0]);
            return;
        }
        if (curItem != null && curItem.info.longJiXiLians.Count > 0 && !Constant.ShowTipsOnceList.Contains(87))
        {
            UtilityTips.ShowPromptWordTips(87, () =>
            {
                FNDebug.Log(curItem.index);
                Net.CSWoLongXiLianMessage(curItem.index, 2);
            });
        }
        else
        {
            Net.CSWoLongXiLianMessage(curItem.index, 2);
        }
    }
    void GetXiLianRes(uint id, object data)
    {
        wolong.WoLongXiLianResponse msg = (wolong.WoLongXiLianResponse)data;
        curItem.info = msg.result.equip;
        ShowLongJi(true);
    }
    void SCWoLongXiLianSelectMessage(uint id, object data)
    {
        UtilityTips.ShowTips(666, 1.5f, ColorType.Green);
        wolong.WoLongXiLianSelectResponse msg = (wolong.WoLongXiLianSelectResponse)data;
        //CSBagInfo.Instance.GetEuqipRecastRes(msg.result);
        curItem.info = msg.result.equip;
        showItem.Refresh(curItem.info);
        ShowLongJi();
    }
    void GetItemChange(uint id, object data)
    {
        RefreshHunLiCost();
        RefreshBtnRed(panelType.selfEquip);
    }
    void ShowLongJi(bool _isShowEff = false)
    {
        //Debug.Log($"龙技count={curItem.info.longJis.Count}     缓存结果count={curItem.info.longJiXiLians.Count}");
        for (int i = 0; i < hunliOld.Count; i++)
        {
            if (i < curItem.info.longJis.Count)
            {
                hunliOld[i].Init(curItem.info.longJis[i]);
            }
            else
            {
                hunliOld[i].UnInit();
            }
        }
        ScriptBinder.StartCoroutine(ShowNewLongJiAttr(_isShowEff));
    }
    IEnumerator ShowNewLongJiAttr(bool _isShowEff = false)
    {
        if (curItem.info.longJiXiLians.Count > 0)
        {
            mobj_noHunLi.SetActive(false);
            ChaneNewLongJiState(true);
            for (int i = 0; i < hunliNew.Count; i++)
            {
                if (i < curItem.info.longJiXiLians.Count)
                {
                    hunliNew[i].Init(curItem.info.longJiXiLians[i], _isShowEff);
                }
                else
                {
                    hunliNew[i].UnInit();
                }
                if (_isShowEff)
                {
                    yield return wait;
                }
            }
        }
        else
        {
            ChaneNewLongJiState(false);
            mobj_noHunLi.SetActive(true);
            for (int i = 0; i < hunliNew.Count; i++)
            {
                hunliNew[i].UnInit(false);
            }
        }
    }
    void ChaneNewLongJiState(bool _state)
    {
        mbtn_Choose.SetActive(_state);
        mtable_btns.Reposition();
        //mbtn_refine.transform.localPosition = (_state == true) ? showNewLongJi : notShowNewLongJi;
    }
    void RefreshHunLiCost()
    {
        isMoneyEnough = true;
        isGoodsEnough = true;
        //moneyCost.Clear();
        //goodsCost.Clear();
        moneyCost = ZhanChongXiLianCostNewTableManager.Instance.GetCostByLevClass(curItem.cfg.levClass, 1);
        goodsCost = ZhanChongXiLianCostNewTableManager.Instance.GetCostByLevClass(curItem.cfg.levClass, 2);
        if (moneyCost.Count > 0)
        {
            mlb_moneyNum.text = moneyCost[1].ToString();
            msp_moneyIcon.spriteName = $"tubiao{ItemTableManager.Instance.GetItemIcon(moneyCost[0])}";
            isMoneyEnough = CSItemCountManager.Instance.GetItemCount(moneyCost[0]) >= moneyCost[1] ? true : false;
            mlb_moneyNum.color = (isMoneyEnough == true) ? CSColor.green : CSColor.red;
        }
        if (goodsCost.Count > 0)
        {
            mlb_goodsNum.text = $"{CSBagInfo.Instance.GetItemCount(goodsCost[0])}/{goodsCost[1]}";
            msp_goodsIcon.spriteName = $"tubiao{ItemTableManager.Instance.GetItemIcon(goodsCost[0])}";
            isGoodsEnough = CSBagInfo.Instance.GetItemCount(goodsCost[0]) >= goodsCost[1] ? true : false;
            mlb_goodsNum.color = (isGoodsEnough == true) ? CSColor.green : CSColor.red;
        }
    }
    void CostMoneyClick(GameObject item)
    {
        if (moneyCost.Count > 0)
        {
            Utility.ShowGetWay(moneyCost[0]);
        }
    }
    void CostGoodsClick(GameObject item)
    {
        if (goodsCost.Count > 0)
        {
            Utility.ShowGetWay(goodsCost[0]);
        }
    }
    void TipsMoneyClick(GameObject item)
    {
        if (moneyCost.Count > 0)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, moneyCost[0]);
        }
    }
    void TipsGoodsClick(GameObject item)
    {
        if (goodsCost.Count > 0)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, goodsCost[0]);
        }
    }
    void HelpBtnClick(GameObject _go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.HunLian);
    }
    void GetWayClick(GameObject go)
    {
        Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(602));
    }
    void RefreshBtnRed(panelType _type)
    {
        bool hasRed = false;
        if (_type == panelType.selfEquip)
        {
            var iter = selfquips.GetEnumerator();
            while (iter.MoveNext())
            {
                if (hasRed == false)
                {
                    hasRed = CSBagInfo.Instance.IsLongJiCostEnough(iter.Current.Value, ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId));
                }
                else
                {
                    break;
                }
            }
            mobj_selfRed.SetActive(hasRed);
            for (int i = 0; i < selfRefineList.Count; i++)
            {
                selfRefineList[i].RefreshRed();
            }
        }
    }
}
public class HunLianChooseItem : IDispose
{
    public void Dispose()
    {
        if (item != null) { UIItemManager.Instance.RecycleSingleItem(item); }
    }
    public GameObject go;
    public UILabel lb_name;
    public GameObject obj_choose;
    public Transform itemPar;
    public UIItemBase item;
    public UIGridContainer grid_skill;
    public GameObject redpoint;
    public bag.BagItemInfo info;
    public TABLE.ITEM cfg;
    Action<GameObject> dele;
    public int index;
    int type = 0;
    public HunLianChooseItem()
    {

    }
    public void Init(GameObject _go, TABLE.ITEM _cfg, bag.BagItemInfo _info, Action<GameObject> _dele, int _index, int _type)
    {
        go = _go;
        lb_name = go.transform.Find("name").GetComponent<UILabel>();
        obj_choose = go.transform.Find("choose").gameObject;
        itemPar = go.transform.Find("Item");
        grid_skill = go.transform.Find("grid_attr").GetComponent<UIGridContainer>();
        redpoint = go.transform.Find("redPoint").gameObject;
        if (item == null)
        {
            item = UIItemManager.Instance.GetItem(PropItemType.Normal, itemPar);
        }
        cfg = _cfg;
        info = _info;
        dele = _dele;
        index = _index;
        type = _type;
        UIEventListener.Get(go, this).onClick = dele;
        Refresh();
    }
    public void UnInit()
    {

    }

    public void Refresh()
    {
        lb_name.text = cfg.name;
        lb_name.color = UtilityCsColor.Instance.GetColor(cfg.quality);
        item.Refresh(info);
        grid_skill.MaxCount = info.baseAffixs.Count;
        for (int i = 0; i < info.baseAffixs.Count; i++)
        {
            UILabel name = grid_skill.controlList[i].GetComponent<UILabel>();
            WoLongRandomAttrTableManager ins = WoLongRandomAttrTableManager.Instance;
            name.text = SkillTableManager.Instance.GetNameByGroupId(ins.GetWoLongRandomAttrParameter(info.baseAffixs[i].id));
        }
    }
    public void RefreshRed()
    {
        //type =1 龙技   ==2 龙力
        if (type == 1)
        {
            redpoint.SetActive(CSBagInfo.Instance.IsLongJiCostEnough(info, cfg));
        }
        else
        {
            redpoint.SetActive(CSBagInfo.Instance.IsLongLiCostEnough(info, cfg));
        }
    }

    public void ChangeChooseState(bool _state)
    {
        obj_choose.SetActive(_state);
    }
}
public class WLRefineAttrValue
{
    public WLRefineAttrValue()
    {

    }
    public GameObject go;
    public UILabel name;
    public UILabel value;
    public GameObject bg;
    public GameObject effect;
    bag.WolongRandomEffect info;
    bool isShowEff = false;
    public WLRefineAttrValue(GameObject _obj)
    {
        go = _obj;
    }
    public void SetObj(GameObject _obj, bool _isSHowEff = false)
    {
        go = _obj;
        name = go.transform.Find("name").GetComponent<UILabel>();
        value = go.transform.Find("value").GetComponent<UILabel>();
        bg = go.transform.Find("bg").gameObject;
        effect = go.transform.Find("effect").gameObject;
        isShowEff = _isSHowEff;
        if (isShowEff)
        {
            effect.SetActive(false);
            CSEffectPlayMgr.Instance.ShowUIEffect(effect, 17074);
        }
    }
    public void Init(bag.WolongRandomEffect _str, bool _isSHowEff = false)
    {
        info = _str;
        bg.SetActive(true);
        if (isShowEff && _isSHowEff)
        {
            effect.SetActive(true);
            CSEffectPlayMgr.Instance.ShowUIEffect(effect, 17074);
        }
        WoLongRandomAttrTableManager ins = WoLongRandomAttrTableManager.Instance;
        string str = SkillTableManager.Instance.GetNameByGroupId(ins.GetWoLongRandomAttrParameter(info.id));
        name.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1731), str);
        value.text = $"+{info.effectValue}";
        //Debug.Log($"{info.name}  {info.strValue}");
        name.color = UtilityCsColor.Instance.GetColor(info.quality);
        value.color = UtilityCsColor.Instance.GetColor(info.quality);
    }
    public void UnInit(bool _active = true)
    {
        name.text = "";
        value.text = "";
        bg.SetActive(_active);
    }

    public void ShowEffect()
    {
        CSEffectPlayMgr.Instance.ShowUIEffect(effect, 17056, 10);
    }

    public void Recycle()
    {
        CSEffectPlayMgr.Instance.Recycle(effect);
    }
}
