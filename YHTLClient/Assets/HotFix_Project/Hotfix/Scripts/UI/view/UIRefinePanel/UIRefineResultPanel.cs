using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UIRefineResultPanel : UIBase
{
    #region forms
    GameObject _btn_close;
    GameObject btn_close { get { return _btn_close ?? (_btn_close = Get("center/event/btn_close").gameObject); } }
    UIGridContainer _obj_curGrid;
    UIGridContainer obj_curGrid { get { return _obj_curGrid ?? (_obj_curGrid = Get<UIGridContainer>("center/view/CurValue/grid")); } }
    UIScrollView _sc_random;
    UIScrollView sc_random { get { return _sc_random ?? (_sc_random = Get<UIScrollView>("center/view/RandomValues")); } }
    UIGridContainer _grid_randomGrid;
    UIGridContainer grid_randomGrid { get { return _grid_randomGrid ?? (_grid_randomGrid = Get<UIGridContainer>("center/view/RandomValues/grid")); } }
    //cost
    UISprite _lb_coseIcon;
    UISprite lb_coseIcon { get { return _lb_coseIcon ?? (_lb_coseIcon = Get<UISprite>("center/view/cost/pay/money/icon")); } }
    UILabel _lb_coseMoney;
    UILabel lb_coseMoney { get { return _lb_coseMoney ?? (_lb_coseMoney = Get<UILabel>("center/view/cost/pay/money/num")); } }
    GameObject _btn_costMoney;
    GameObject btn_costMoney { get { return _btn_costMoney ?? (_btn_costMoney = Get<GameObject>("center/view/cost/pay/money/Btn_Buy")); } }
    UILabel _lb_coseGoods;
    UILabel lb_coseGoods { get { return _lb_coseGoods ?? (_lb_coseGoods = Get<UILabel>("center/view/cost/pay/goods/num")); } }
    UISprite _sp_goodsIcon;
    UISprite sp_goodsIcon { get { return _sp_goodsIcon ?? (_sp_goodsIcon = Get<UISprite>("center/view/cost/pay/goods/icon")); } }
    UISprite _sp_goodsBg;
    UISprite sp_goodsBg { get { return _sp_goodsBg ?? (_sp_goodsBg = Get<UISprite>("center/view/cost/pay/goods/Btn_Buy")); } }
    UILabel _lb_freeNum;
    UILabel lb_freeNum { get { return _lb_freeNum ?? (_lb_freeNum = Get<UILabel>("center/view/cost/free/lb_count")); } }
    GameObject _obj_pay;
    GameObject obj_pay { get { return _obj_pay ?? (_obj_pay = Get("center/view/cost/pay").gameObject); } }


    UILabel _lb_refineNum;
    UILabel lb_refineNum { get { return _lb_refineNum ?? (_lb_refineNum = Get<UILabel>("center/view/cost/lockNum")); } }
    GameObject _btn_add;
    GameObject btn_add { get { return _btn_add ?? (_btn_add = Get("center/view/cost/lockNum/add").gameObject); } }
    GameObject _btn_reduce;
    GameObject btn_reduce { get { return _btn_reduce ?? (_btn_reduce = Get("center/view/cost/lockNum/reduce").gameObject); } }
    GameObject _btn_Refine;
    GameObject btn_Refine { get { return _btn_Refine ?? (_btn_Refine = Get("center/event/btn_Refine").gameObject); } }
    GameObject _btn_Choose;
    GameObject btn_Choose { get { return _btn_Choose ?? (_btn_Choose = Get("center/event/btn_Choose").gameObject); } }
    GameObject _obj_close;
    GameObject obj_close { get { return _obj_close ?? (_obj_close = Get("center/window/mask").gameObject); } }
    GameObject _obj_successEffect;
    GameObject obj_successEffect { get { return _obj_successEffect ?? (_obj_successEffect = Get("center/window/title/successEffect").gameObject); } }
    #endregion
    #region variable
    FastArrayElementFromPool<RefineRandomValueShow> randomValueList;
    EquipRefineProDic ranattrList;
    RefineItem curItem;
    int chooseIndex = -1;
    bag.SCEquipRandomsNtf msg;
    public static int equipLevel;
    List<bool> lockState;
    List<bool> originalLockState = new List<bool>();
    List<RefineResultItem> resultItemList;
    bool isChoosed = false;
    #endregion
    public override void Init()
    {
        base.Init();
        resultItemList = new List<RefineResultItem>();
        mClientEvent.Reg((uint)CEvent.SCChooseXiLianResultNtf, GetChooseResult);
        mClientEvent.Reg((uint)CEvent.RefineResultRefresh, GetData);
        mClientEvent.AddEvent(CEvent.ItemListChange, GetItemChange);
        mClientEvent.AddEvent(CEvent.FastAccessJumpToPanel, FastAccessJumpToPanel);
        UIEventListener.Get(btn_close).onClick = CloseBtnClick;
        UIEventListener.Get(btn_add).onClick = RefineNumAddClick;
        UIEventListener.Get(btn_reduce).onClick = RefineNumReduceClick;
        UIEventListener.Get(btn_Refine).onClick = RefineBtnClick;
        UIEventListener.Get(btn_Choose).onClick = ChooseBtnClick;
        obj_curGrid.MaxCount = 8;
        randomValueList = mPoolHandleManager.CreateGeneratePool<RefineRandomValueShow>();
        randomValueList.Count = 8;
        for (int i = 0; i < 8; i++)
        {
            randomValueList[i].ResetObj(obj_curGrid.controlList[i], i);
        }
    }
    public override void Show()
    {
        base.Show();
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(obj_successEffect);
        for (int i = 0; i < randomValueList.Count; i++)
        {
            CSEffectPlayMgr.Instance.Recycle(randomValueList[i].GetEffectObj());
        }
        for (int i = 0; i < resultItemList.Count; i++)
        {
            resultItemList[i].Recycle();
        }
        curItem = null;
        chooseIndex = -1;
        msg = null;
        resultItemList = null;
        base.OnDestroy();
    }
    void CloseBtnClick(GameObject _go)
    {
        if (!isChoosed && !Constant.ShowTipsOnceList.Contains(8))
        {
            UtilityTips.ShowPromptWordTips(8, ComfirmGiveUp);
        }
        else
        {
            ComfirmGiveUp();
        }
    }
    void ComfirmGiveUp()
    {
        EventData data = CSEventObjectManager.Instance.SetValue(true, lockState);
        mClientEvent.SendEvent(CEvent.RefineResultClose, data);
        CSEventObjectManager.Instance.Recycle(data);
        UIManager.Instance.ClosePanel<UIRefineResultPanel>();
    }
    public void GetData(object data, RefineItem _curItem, List<bool> _list)
    {
        msg = (bag.SCEquipRandomsNtf)data;
        curItem = _curItem;
        lockState = _list;
        for (int i = 0; i < _list.Count; i++)
        {
            originalLockState.Add(_list[i]);
        }
        RefreshCurValue();
        RefreshResult();
        CSEffectPlayMgr.Instance.ShowUIEffect(obj_successEffect, "effect_instance_success_add");
    }
    public void GetData(uint id, object data)
    {
        CSEffectPlayMgr.Instance.ShowUIEffect(obj_successEffect, "effect_instance_success_add");
        msg = (bag.SCEquipRandomsNtf)data;
        curItem.info.freeXiLianCount = msg.freeXiLianRandomTimes;
        RefreshCurValue();
        for (int i = 0; i < lockState.Count; i++)
        {
            originalLockState[i] = lockState[i];
        }
        RefreshResult();

        //lb_freeNum.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1613), $"{curItem.info.freeXiLianCount}/{curItem.info.freeXiLianCountMax}");
    }
    void GetItemChange(uint id, object data)
    {
        RefreshCost();
    }
    void FastAccessJumpToPanel(uint id, object data)
    {
        int panelId = System.Convert.ToInt32(data);
        if (UtilityPanel.CheckGameModelPanelIsThis<UIRefineResultPanel>(panelId))
        {
            return;
        }
        UIManager.Instance.ClosePanel<UIRefineResultPanel>();
    }
    void RefreshCurValue()
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
                randomValueList[i].UnInit(true);
            }
        }
        lockNum = 0;
        for (int i = 0; i < lockState.Count; i++)
        {
            if (lockState[i])
            {
                lockNum++;
            }
        }
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
        lockState[_go.GetIndex()] = _go.isChoose;
        RefreshCost();
        for (int i = 0; i < resultItemList.Count; i++)
        {
            resultItemList[i].ChangeLockMask(_go.index, _go.isChoose);
        }
    }
    void RefreshResult()
    {
        grid_randomGrid.MaxCount = msg.xiLianList.Count;
        int gap = msg.xiLianList.Count - resultItemList.Count;
        if (gap > 0)
        {
            int length = resultItemList.Count;
            for (int i = 0; i < gap; i++)
            {
                resultItemList.Add(new RefineResultItem(grid_randomGrid.controlList[length + i], ChangeChooseState));
            }
        }
        sc_random.gameObject.SetActive(false);
        for (int i = 0; i < msg.xiLianList.Count; i++)
        {
            EquipRefineProDic temp_data = StructTipData.Instance.GetEquipRandomDisjunctData(curItem.cfg, msg.xiLianList[i].randAttrValues);

            resultItemList[i].Refresh(temp_data, i, lockState, originalLockState, curItem.cfg);
        }
        RefreshCost();
        if (msg.xiLianList.Count == 1)
        {
            resultItemList[0].ShowSuggest(false);
            chooseIndex = 0;
            obj_curGrid.transform.parent.transform.localPosition = new Vector3(-215, 0, 0);
            sc_random.transform.localPosition = new Vector3(202, 152, 0);

            //for (int i = 0; i < randomValueList.Count; i++)
            //{
            //    randomValueList[i].SetArrow(true);
            //}
            for (int i = 0; i < ranattrList.ts.Count; i++)
            {
                randomValueList[i].SetArrow(true);
            }
        }
        else
        {
            chooseIndex = -1;
            for (int i = 0; i < randomValueList.Count; i++)
            {
                randomValueList[i].SetArrow(false);
            }
            obj_curGrid.transform.parent.transform.localPosition = new Vector3(-278, 0, 0);
            sc_random.transform.localPosition = new Vector3(13, 151, 0);
            for (int i = 0; i < resultItemList.Count - 1; i++)
            {
                resultItemList[i].ChangeChooseState(false);
                resultItemList[i].ShowSuggest(true);
            }
        }
        sc_random.gameObject.SetActive(true);
    }
    void ChangeChooseState(int _index)
    {
        if (msg.xiLianList.Count == 1)
        {
            chooseIndex = 0;
            return;
        }
        if (chooseIndex == _index)
        {
            resultItemList[chooseIndex].ChangeChooseState(false);
            chooseIndex = -1;
            return;
        }
        if (chooseIndex != -1)
        {
            resultItemList[chooseIndex].ChangeChooseState(false);
        }
        chooseIndex = _index;
        resultItemList[chooseIndex].ChangeChooseState(true);
    }
    int lockNum = 0;
    TABLE.XILIANCOST costData;
    long money;
    long goods;
    bool canRefine = true;
    int refineNum = 1;
    bool isCostItemEnough = false;
    bool isMoneyEnough = false;
    int costMoneyId = 0;
    int costItemId = 0;
    void RefreshCost()
    {
        if (curItem.info.freeXiLianCount > 0 && lockNum == 0)
        {
            obj_pay.SetActive(false);
            lb_freeNum.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1613), $"{curItem.info.freeXiLianCount}/{curItem.info.freeXiLianCountMax}");
            isCostItemEnough = true;
            isMoneyEnough = true;
            canRefine = true;
        }
        else
        {
            obj_pay.SetActive(true);
            lb_freeNum.text = "";
            costData = XiLianCostTableManager.Instance.GetCfg(curItem.cfg.level);
            costMoneyId = (int)costData.payType;
            costItemId = (int)costData.costItemID;
            money = CSItemCountManager.Instance.GetItemCount(costMoneyId);
            goods = CSBagInfo.Instance.GetItemCount(costItemId);
            UIEventListener.Get(btn_costMoney, costMoneyId).onClick = ShowGetWay;
            UIEventListener.Get(lb_coseIcon.gameObject, costMoneyId).onClick = ShowTips;
            lb_coseMoney.text = (costData.price[lockNum] * refineNum).ToString();
            lb_coseIcon.spriteName = $"tubiao{costMoneyId}";
            lb_coseGoods.text = $"{goods}/{costData.num[lockNum] * refineNum}";
            lb_coseMoney.color = (money >= costData.price[lockNum] * refineNum) ? CSColor.green : CSColor.red;
            lb_coseGoods.color = (goods >= costData.num[lockNum] * refineNum) ? CSColor.green : CSColor.red;
            sp_goodsIcon.spriteName = $"tubiao{ItemTableManager.Instance.GetItemIcon(costItemId)}";
            UIEventListener.Get(sp_goodsBg.gameObject, costItemId).onClick = ShowGetWay;
            UIEventListener.Get(sp_goodsIcon.gameObject, costItemId).onClick = ShowTips;
            //UIEventListener.Get(sp_goodsIcon.gameObject).onClick = (p) =>
            //{
            //    Utility.ShowGetWay(costItemId);
            //};
            isMoneyEnough = money >= costData.price[lockNum] * refineNum ? true : false;
            isCostItemEnough = goods >= costData.num[lockNum] * refineNum ? true : false;
            canRefine = isMoneyEnough && isCostItemEnough ? true : false;
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
    int refineMaxNum = 0;
    void RefineNumAddClick(GameObject _go)
    {
        refineMaxNum = int.Parse(SundryTableManager.Instance.GetSundryEffect(29));
        refineNum++;
        refineNum = (refineNum >= refineMaxNum) ? refineMaxNum : refineNum;
        lb_refineNum.text = refineNum.ToString();
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
        lb_refineNum.text = refineNum.ToString();
    }
    void RefineBtnClick(GameObject _go)
    {
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
        if (!isChoosed && !Constant.ShowTipsOnceList.Contains(8))
        {
            UtilityTips.ShowPromptWordTips(8, ConfirmRefined);
            return;
        }
        else
        {
            ConfirmRefined();
            isChoosed = false;
        }
    }
    void ConfirmRefined()
    {
        bag.CSEquipXiLianReq data = CSProtoManager.Get<bag.CSEquipXiLianReq>();
        data.lockedAttrIndex.Clear();
        for (int i = 0; i < randomValueList.Count; i++)
        {
            if (randomValueList[i].GetIsChoose())
            {
                data.lockedAttrIndex.Add(i);
            }
        }
        data.xiLianNum = refineNum;
        data.equipIndex = curItem.index;
        Net.EquipXiLianReqMessage(data);
    }
    void ChooseBtnClick(GameObject _go)
    {
        if (chooseIndex == -1)
        {
            UtilityTips.ShowRedTips(216);
            return;
        }
        EventData data = CSEventObjectManager.Instance.SetValue(false, lockState);
        mClientEvent.SendEvent(CEvent.RefineResultClose, data);
        CSEventObjectManager.Instance.Recycle(data);
        Net.CSChooseXiLianResultReqMessage(chooseIndex, curItem.index);
    }
    void GetChooseResult(uint id, object data)
    {
        isChoosed = true;
        bag.EquipInfo info = (bag.EquipInfo)data;
        curItem.info = info.equip;
        RefreshCurValue();
        //RefreshResult();

        for (int i = 0; i < resultItemList.Count; i++)
        {
            resultItemList[i].UnInit();
        }
        for (int i = 0; i < lockState.Count; i++)
        {
            originalLockState[i] = lockState[i];
        }
        if (curItem.info.freeXiLianCount > 0 && lockNum == 0)
        {
            lb_freeNum.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1613), $"{curItem.info.freeXiLianCount}/{curItem.info.freeXiLianCountMax}");
        }
        else
        {
            lb_freeNum.text = "";
        }

    }

}





public class RefineResultItem
{
    public GameObject go;
    public UIGridContainer grid;
    public GameObject suggest;
    public GameObject choose;
    public GameObject tick;
    public GameObject tickChild;
    public List<RefineResultAttrItem> attrList;
    Action<int> action;
    int index = 0;
    bool isChoose = false;
    int totalCount = 8;
    int dataCount = 0;
    List<bool> lockMaskList = new List<bool>();
    public RefineResultItem(GameObject _obj, Action<int> _action)
    {
        go = _obj;
        action = _action;
        attrList = new List<RefineResultAttrItem>();
        grid = go.transform.Find("grid").GetComponent<UIGridContainer>();
        grid.MaxCount = totalCount;
        for (int i = 0; i < totalCount; i++)
        {
            attrList.Add(new RefineResultAttrItem(grid.controlList[i]));
        }
        suggest = go.transform.Find("suggest").gameObject;
        choose = go.transform.Find("suggest/choose").gameObject;
        tick = go.transform.Find("suggest/choose/tick").gameObject;
        tickChild = go.transform.Find("shield").gameObject;
    }

    public void Refresh(EquipRefineProDic _data, int _index, List<bool> _lockList, List<bool> _originalLock, TABLE.ITEM _cfg)
    {
        index = _index;
        suggest.SetActive(true);
        lockMaskList = _originalLock;
        UIEventListener.Get(choose).onClick = Click;
        UIEventListener.Get(tickChild).onClick = Click;
        dataCount = _data.ts.Count;
        for (int i = 0; i < totalCount; i++)
        {
            if (i < dataCount)
            {
                attrList[i].Refresh(_data.ts[i], i, _cfg);
                attrList[i].SetBgState((i % 2) == 0 ? true : false);
                attrList[i].SetLockState(_lockList[i]);
            }
            else
            {
                attrList[i].UnInit();
            }
        }
        choose.gameObject.SetActive(true);
        tick.SetActive(false);
    }
    public void ChangeLockMask(int _ind, bool _state)
    {
        if (_ind < totalCount && lockMaskList[_ind])
        {
            attrList[_ind].SetLockState(_state);
        }
    }
    public void UnInit()
    {
        UIEventListener.Get(choose).onClick = null;
        UIEventListener.Get(tickChild).onClick = null;
        suggest.SetActive(false);
        for (int i = 0; i < attrList.Count; i++)
        {
            attrList[i].UnRefresh();
        }
    }
    void Click(GameObject _go)
    {
        isChoose = !isChoose;
        action(index);
    }
    public void ChangeChooseState(bool _state)
    {
        tick.SetActive(_state);
    }
    public void ShowSuggest(bool _state)
    {
        suggest.SetActive(_state);
    }
    public void Recycle()
    {
        for (int i = 0; i < totalCount; i++)
        {
            attrList[i].Recycle();
        }
    }
}
public class RefineResultAttrItem
{
    public GameObject go;

    public UILabel name;
    public UILabel value;
    public UILabel maxValue;
    public UILabel skillDes;
    public GameObject mask;
    public GameObject bg;
    public GameObject choose;
    public GameObject effect;
    public GameObject suggest;
    int skillId = 0;
    Schedule schedule;
    float intervalTime = 0.08f;
    EquipRefineProperty data;
    TABLE.ITEM cfg;
    List<List<int>> suggestAttrId;
    Dictionary<int, List<int>> suggestIdDic;
    public RefineResultAttrItem(GameObject _obj)
    {
        go = _obj;
        name = go.transform.Find("name").GetComponent<UILabel>();
        value = go.transform.Find("value").GetComponent<UILabel>();
        maxValue = go.transform.Find("maxvalue").GetComponent<UILabel>();
        skillDes = go.transform.Find("skilldes").GetComponent<UILabel>();

        bg = go.transform.Find("bg").gameObject;
        mask = go.transform.Find("lockmask").gameObject;
        choose = go.transform.Find("choose").gameObject;
        effect = go.transform.Find("effect").gameObject;
        suggest = go.transform.Find("suggest").gameObject;
        effect.SetActive(false);
        suggest.SetActive(false);
        CSEffectPlayMgr.Instance.ShowUIEffect(effect, 17074);
        choose.SetActive(false);
        UIEventListener.Get(skillDes.gameObject).onClick = SkillShow;
        suggestIdDic = new Dictionary<int, List<int>>(12);
    }

    public void Refresh(EquipRefineProperty _data, int _ind, TABLE.ITEM _cfg)
    {
        cfg = _cfg;
        if (suggestAttrId == null)
        {
            if (_cfg.career == 1)
            {
                suggestAttrId = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(1151));
            }
            else if (_cfg.career == 2)
            {
                suggestAttrId = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(1155));
            }
            else if (_cfg.career == 3)
            {
                suggestAttrId = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(1156));
            }
            else if (_cfg.career == 0)
            {
                suggestAttrId = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(1157));
            }
            for (int i = 0; i < suggestAttrId.Count; i++)
            {
                suggestIdDic.Add(suggestAttrId[i][0], suggestAttrId[i]);
            }
        }
        data = _data;
        go.SetActive(false);
        if (!Timer.Instance.IsInvoking(schedule))
        {
            Timer.Instance.Invoke(intervalTime * _ind, PlayEff);
        }
        else
        {
            Timer.Instance.CancelInvoke(schedule);
            Timer.Instance.Invoke(intervalTime * _ind, PlayEff);
        }

    }
    void PlayEff(Schedule _schedule)
    {
        Timer.Instance.CancelInvoke(schedule);
        effect.SetActive(true);
        suggest.SetActive(false);
        CSEffectPlayMgr.Instance.ShowUIEffect(effect, 17074);
        name.text = data.name;
        if (data.data[0].type == 2)
        {
            skillDes.gameObject.SetActive(true);
            skillDes.text = data.strValue;
            value.text = "";
            maxValue.text = "";
            name.color = CSColor.white;
            skillId = data.data[0].param1;
        }
        else
        {
            skillDes.gameObject.SetActive(false);
            skillDes.text = "";
            value.text = data.strValue;
            maxValue.text = data.strMaxValue;
            name.color = UtilityCsColor.Instance.GetColor(data.quality);
            value.color = UtilityCsColor.Instance.GetColor(data.quality);
        }
        if (data.quality >= 4)
        {
            List<int> perPos;
            if (suggestIdDic.TryGetValue(cfg.subType, out perPos))
            {
                for (int j = 0; j < perPos.Count; j++)
                {
                    if (j > 0)
                    {
                        if (data.Id == perPos[j])
                        {
                            suggest.SetActive(true);
                            break;
                        }
                    }

                }
            }
        }
        go.SetActive(true);
    }
    public void SetLockState(bool _state)
    {
        mask.SetActive(_state);
    }
    void SkillShow(GameObject _go)
    {
        UISkillTipsPanel.CreateHelpTipsPanel(skillId);
    }
    public void SetBgState(bool _state)
    {
        bg.SetActive(_state);
    }
    public void UnInit()
    {
        suggest.SetActive(false);
        go.SetActive(false);
        name.text = "";
    }
    public void UnRefresh()
    {
        suggest.SetActive(false);
        name.text = "";
        name.color = CSColor.gray;
        value.text = "";
        mask.SetActive(false);
        maxValue.text = "";
        skillDes.text = "";
        skillDes.gameObject.SetActive(false);
    }
    public void Recycle()
    {
        CSEffectPlayMgr.Instance.Recycle(effect);
        Timer.Instance.CancelInvoke(schedule);
    }
}

