using Google.Protobuf.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UILongLiPanel : UIBasePanel
{
    #region  variable
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
    List<int> costList = new List<int>();
    //龙技
    FastArrayElementFromPool<LongLiBaseAffixItem> baseAffix;
    FastArrayElementFromPool<LongLiIntenAffixItem> IntenAffix;
    //消耗
    IntArray moneyCost;
    IntArray goodsCost;
    bool isMoneyEnough = false;
    bool isGoodsEnough = true;
    int intenAffixMaxCount = 12;
    string playerPrefKey = "";
    int bubbleSet = 0;  //0 没有出现过气泡 1 出现过不再提示
    #endregion
    public override void Init()
    {
        base.Init();
        playerPrefKey = $"{CSMainPlayerInfo.Instance.Name}LongLiBubble";
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_bg, "wolong_bg");
        showItem = UIItemManager.Instance.GetItem(PropItemType.Normal, mtrans_itemshowPar);
        moneyCost = IntArray.Default;
        goodsCost = IntArray.Default;
        UIEventListener.Get(mbtn_help).onClick = HelpBtnClick;
        UIEventListener.Get(mbtn_waerEquips, panelType.selfEquip).onClick = ChangeEuqipsBtnClick;
        UIEventListener.Get(mbtn_bagEquips, panelType.bagEquip).onClick = ChangeEuqipsBtnClick;
        UIEventListener.Get(mbtn_refine).onClick = ConfirmClick;
        UIEventListener.Get(mbtn_moneyBuy).onClick = CostMoneyClick;
        UIEventListener.Get(msp_moneyIcon.gameObject).onClick = TipsMoneyClick;
        UIEventListener.Get(mbtn_goodsBuy).onClick = CostGoodsClick;
        UIEventListener.Get(msp_goodsIcon.gameObject).onClick = TipsGoodsClick;
        UIEventListener.Get(mobj_getway).onClick = GetWayClick;

        mgrid_intenAffix.MaxCount = intenAffixMaxCount;
        IntenAffix = mPoolHandleManager.CreateGeneratePool<LongLiIntenAffixItem>();
        IntenAffix.Count = intenAffixMaxCount;
        for (int i = 0; i < intenAffixMaxCount; i++)
        {
            IntenAffix[i].SetObj(mgrid_intenAffix.controlList[i]);
        }
        baseAffix = mPoolHandleManager.CreateGeneratePool<LongLiBaseAffixItem>();
        RefreshBtnRed(panelType.selfEquip);
        mlb_noEquipDes.text = ClientTipsTableManager.Instance.GetClientTipsContext(1809);
        if (PlayerPrefs.HasKey(playerPrefKey))
        {
            bubbleSet = PlayerPrefs.GetInt(playerPrefKey);
        }
        mClientEvent.AddEvent(CEvent.LongLiBubbleClick, LongLiBubbleClick);

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
        CSBagInfo.Instance.GetSelfWoLongEquipData(selfquips, true);
        if (selfquips.Count != 0)
        {
            ChangeEuqipsBtnClick(mbtn_waerEquips);
        }
        else
        {
            bagequips.Clear();
            CSBagInfo.Instance.GetBagWoLongEquipData(bagequips, true);
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
                    temp_ind = i;
                    ItemClick(mgrid_BagEquip.controlList[i]);
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
                    temp_ind = i;
                    ItemClick(mgrid_SelfEquip.controlList[i]);
                    break;
                }
            }
            if (temp_ind > 4)
            {
                SpringPanel.Begin(mobj_SelfEqiup, new Vector3(-347, mobj_SelfEqiup.transform.localPosition.y + 90 * (temp_ind - 4)), 20f);
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
        UIItemManager.Instance.RecycleSingleItem(showItem);
        CSEffectPlayMgr.Instance.Recycle(mtex_bg);
        base.OnDestroy();
    }
    #region leftPart
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
        CSBagInfo.Instance.GetBagWoLongEquipData(bagequips, true);
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
                iter.Current.Value, ItemClick, iter.Current.Key, 2);
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
        CSBagInfo.Instance.GetSelfWoLongEquipData(selfquips, true);
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
                iter.Current.Value, ItemClick, -iter.Current.Key, 2);
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
        mgrid_baseAffix.gameObject.SetActive(!_state);
        mobj_bgs.SetActive(!_state);
        if (_state)
        {
            if (curItem != null)
            {
                curItem.ChangeChooseState(false);
            }
            curItem = null;
            if (showItem != null) { showItem.obj.SetActive(false); }
            mlb_showitemName.text = "";
            for (int i = 0; i < IntenAffix.Count; i++)
            {
                IntenAffix[i].UnRefresh();
            }
        }
    }
    #endregion

    #region  NetMessageBack
    void GetXiLianRes(uint id, object data)
    {
        wolong.WoLongXiLianResponse msg = (wolong.WoLongXiLianResponse)data;
        //curItem.info = msg.result.equip;
        //RefreshBaseAffix();
        //RefreshIntenAffix();
        UIManager.Instance.CreatePanel<UIWoLongRefineResultPanel>(p =>
        {
            (p as UIWoLongRefineResultPanel).SetData(curItem.info, msg, curItem.cfg, curItem.index);
        });
    }
    void SCWoLongXiLianSelectMessage(uint id, object data)
    {
        //UtilityTips.ShowTips("洗练成功");
        wolong.WoLongXiLianSelectResponse msg = (wolong.WoLongXiLianSelectResponse)data;
        //CSBagInfo.Instance.GetEuqipRecastRes(msg.result);
        curItem.info = msg.result.equip;
        curItem.Refresh();
        showItem.Refresh(curItem.info);
        RefreshBaseAffix();
        RefreshIntenAffix();
    }
    void GetItemChange(uint id, object data)
    {
        RefreshCost();
        RefreshBtnRed(panelType.selfEquip);
    }
    void LongLiBubbleClick(uint id, object data)
    {
        if (mbubble.activeSelf)
        {
            mbubble.SetActive(false);
        }
    }
    #endregion

    #region Click
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
        RefreshBaseAffix();
        RefreshIntenAffix();
        RefreshCost();
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
        FNDebug.Log("请求的index  " + curItem.index);
        Net.CSWoLongXiLianMessage(curItem.index, 1);
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
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.LongLiXiLian);
    }
    void GetWayClick(GameObject go)
    {
        Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(602));
    }
    #endregion

    void RefreshBaseAffix()
    {
        mgrid_baseAffix.MaxCount = curItem.info.baseAffixs.Count;
        for (int i = 0; i < baseAffix.Count; i++)
        {
            baseAffix[i].Recycle();
        }
        baseAffix.Count = 0;
        baseAffix.Count = mgrid_baseAffix.MaxCount;
        for (int i = 0; i < mgrid_baseAffix.MaxCount; i++)
        {
            baseAffix[i].SetObj(mgrid_baseAffix.controlList[i]);
            baseAffix[i].Refresh(curItem.info.baseAffixs[i], curItem.info.configId);
        }
        if (baseAffix.Count > 0 && bubbleSet == 0)
        {
            mbubble.transform.SetParent(baseAffix[baseAffix.Count - 1].go.transform);
            mbubble.transform.localPosition = new Vector3(60, 45, 0);
            mbubble.SetActive(true);
        }
    }
    void RefreshIntenAffix()
    {
        for (int i = 0; i < IntenAffix.Count; i++)
        {
            if (i < curItem.info.intensifyAffixs.Count)
            {
                IntenAffix[i].Refresh(curItem.info.intensifyAffixs[i]);
            }
            else
            {
                IntenAffix[i].UnRefresh();
            }
        }
    }
    void RefreshCost()
    {
        isMoneyEnough = true;
        isGoodsEnough = true;
        //moneyCost.Clear();
        //goodsCost.Clear();
        moneyCost = ZhanChongXiLianCostNewTableManager.Instance.GetCostByLevClass(curItem.cfg.levClass, 3);
        goodsCost = ZhanChongXiLianCostNewTableManager.Instance.GetCostByLevClass(curItem.cfg.levClass, 4);
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
                    hasRed = CSBagInfo.Instance.IsLongLiCostEnough(iter.Current.Value, ItemTableManager.Instance.GetItemCfg(iter.Current.Value.configId));
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
public class LongLiBaseAffixItem
{
    public GameObject go;
    UILabel name;
    UILabel count;
    GameObject bg;
    long id = 0;
    int configId = 0;
    int effectId = 0;
    RepeatedField<bag.WolongRandomEffect> IntenmesList;
    string playerPrefKey = "";
    public LongLiBaseAffixItem()
    {

    }
    public void SetObj(GameObject _go)
    {
        playerPrefKey = $"{CSMainPlayerInfo.Instance.Name}LongLiBubble";
        go = _go;
        name = go.transform.Find("key").GetComponent<UILabel>();
        count = go.transform.Find("value").GetComponent<UILabel>();
        bg = go.transform.Find("bg").gameObject;
        UIEventListener.Get(bg).onClick = OnClick;
    }
    public void Refresh(bag.WolongRandomEffect _mes, int _configId, long _id = 0, RepeatedField<bag.WolongRandomEffect> _mesList = null, RepeatedField<bag.WolongRandomEffect> _IntenmesList = null)
    {
        id = _id;
        configId = _configId;
        IntenmesList = _IntenmesList;
        go.SetActive(true);
        effectId = WoLongRandomAttrTableManager.Instance.GetWoLongRandomAttrParameter(_mes.id);
        name.text = $"[u]{SkillTableManager.Instance.GetNameByGroupId(effectId)}[-]";

        int affixCount = CSBagInfo.Instance.GetWoLongLongLiAffixCount(effectId, id);
        int selfCount = 0;
        if (_mesList != null)
        {
            for (int i = 0; i < _mesList.Count; i++)
            {
                int temp_skillId = WoLongRandomAttrTableManager.Instance.GetWoLongRandomAttrParameter(_mesList[i].id);
                if (temp_skillId == effectId)
                {
                    selfCount++;
                }
            }
        }
        count.text = $"{affixCount + selfCount}/{12}";
        count.color = ((affixCount + selfCount) >= 12) ? CSColor.green : CSColor.red;
    }
    void OnClick(GameObject _go)
    {
        UIManager.Instance.CreatePanel<UIWoLongSkillTipsPanel>(p =>
        {
            (p as UIWoLongSkillTipsPanel).SetData(effectId, id,configId, IntenmesList);
        });
        int setvalue = 0;
        if (PlayerPrefs.HasKey(playerPrefKey))
        {
            setvalue = PlayerPrefs.GetInt(playerPrefKey);
        }
        if (setvalue == 0)
        {
            PlayerPrefs.SetInt(playerPrefKey, 1);
            HotManager.Instance.EventHandler.SendEvent(CEvent.LongLiBubbleClick);
        }
    }
    public void UnRefresh()
    {
        go.SetActive(false);
    }
    public void Recycle()
    {
        go = null;
        name = null;
        count = null;
    }
}

public class LongLiIntenAffixItem
{
    GameObject go;
    public UILabel des;
    public GameObject effect;
    public LongLiIntenAffixItem()
    {

    }
    public void SetObj(GameObject _go)
    {
        go = _go;
        des = go.GetComponent<UILabel>();
        effect = go.transform.Find("effect").gameObject;
        effect.SetActive(false);
        CSEffectPlayMgr.Instance.ShowUIEffect(effect, 17074);
    }
    public void Refresh(bag.WolongRandomEffect _mes)
    {
        int effectId = WoLongRandomAttrTableManager.Instance.GetWoLongRandomAttrParameter(_mes.id);
        int value = ZhanChongCiZhuiEffectTableManager.Instance.GetZhanChongCiZhuiEffectPer(effectId);
        int point = ZhanChongCiZhuiEffectTableManager.Instance.GetZhanChongCiZhuiEffectPoint(effectId);//判断取小数点几位
        string str = $"{_mes.effectValue}";
        //10000的除以100,1000的除以1000
        if (value == 10000)
        {
            str = $"{Math.Round(Convert.ToDecimal(_mes.effectValue * 0.01f), point, MidpointRounding.AwayFromZero)}";
        }
        else if (value == 1000)
        {
            str = $"{Math.Round(Convert.ToDecimal((float)_mes.effectValue / value), point, MidpointRounding.AwayFromZero)}";
        }
        //Debug.Log($"{_mes.id}    {_mes.effectValue}   {effectId}  {value}  {str}   {_mes.quality}");
        des.text = string.Format(ZhanChongCiZhuiEffectTableManager.Instance.GetZhanChongCiZhuiEffectDesc(effectId), str);
        des.color = UtilityCsColor.Instance.GetColor(_mes.quality);
    }
    public void ShowEffect()
    {
        effect.SetActive(true);
        CSEffectPlayMgr.Instance.ShowUIEffect(effect, 17074);
    }
    public void UnRefresh()
    {
        des.text = "";
    }
    public void Recycle()
    {
        CSEffectPlayMgr.Instance.Recycle(effect);
        go = null;
        des = null;
    }
}