using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIWoLongRefineResultPanel : UIBasePanel
{
    #region  variable
    //List<int> costList = new List<int>();
    //old
    FastArrayElementFromPool<LongLiBaseAffixItem> baseAffixOld;
    FastArrayElementFromPool<LongLiIntenAffixItem> IntenAffixOld;
    //new
    FastArrayElementFromPool<LongLiBaseAffixItem> baseAffixNew;
    FastArrayElementFromPool<LongLiIntenAffixItem> IntenAffixNew;
    //消耗
    IntArray moneyCost;
    IntArray goodsCost;
    bool isMoneyEnough = false;
    bool isGoodsEnough = true;
    bag.BagItemInfo info;
    TABLE.ITEM itemcfg;
    wolong.WoLongXiLianResponse RefineMsg;
    int index = 0;
    //WaitForSeconds wait;
    bool HasBeenChoose = true;
    #endregion
    public override void Init()
    {
        base.Init();
        baseAffixOld = mPoolHandleManager.CreateGeneratePool<LongLiBaseAffixItem>();
        IntenAffixOld = mPoolHandleManager.CreateGeneratePool<LongLiIntenAffixItem>();
        baseAffixNew = mPoolHandleManager.CreateGeneratePool<LongLiBaseAffixItem>();
        IntenAffixNew = mPoolHandleManager.CreateGeneratePool<LongLiIntenAffixItem>();
        moneyCost = IntArray.Default;
        goodsCost = IntArray.Default;
        mClientEvent.AddEvent(CEvent.SCWoLongXiLianMessage, GetXiLianRes);
        mClientEvent.AddEvent(CEvent.SCWoLongXiLianSelectMessage, SCWoLongXiLianSelectMessage);
        mClientEvent.AddEvent(CEvent.ItemListChange, GetItemChange);
        UIEventListener.Get(mbtn_close).onClick = CloseBtnClick;
        UIEventListener.Get(mbtn_refine).onClick = RefineBtnClick;
        UIEventListener.Get(mbtn_confirm).onClick = ConfirmBtnClick;
        UIEventListener.Get(mbtn_moneyBuy).onClick = CostMoneyClick;
        UIEventListener.Get(mbtn_goodsBuy).onClick = CostGoodsClick;
        UIEventListener.Get(msp_moneyIcon.gameObject).onClick = TipsMoneyClick;
        UIEventListener.Get(msp_goodsIcon.gameObject).onClick = TipsGoodsClick;
    }

    public override void Show()
    {
        base.Show();
    }

    protected override void OnDestroy()
    {

        base.OnDestroy();
    }
    public void SetData(bag.BagItemInfo _info, wolong.WoLongXiLianResponse _msg, TABLE.ITEM _cfg, int _index)
    {
        info = _info;
        RefineMsg = _msg;
        itemcfg = _cfg;
        index = _index;
        RefreshOld();
        RefreshNew();
        RefreshCost();
    }
    #region  GetNetMessageBack
    void GetXiLianRes(uint id, object data)
    {
        wolong.WoLongXiLianResponse msg = (wolong.WoLongXiLianResponse)data;
        RefineMsg = msg;
        //RefreshOld();
        RefreshNew();
        HasBeenChoose = true;
    }
    void SCWoLongXiLianSelectMessage(uint id, object data)
    {
        wolong.WoLongXiLianSelectResponse msg = (wolong.WoLongXiLianSelectResponse)data;
        //CSBagInfo.Instance.GetEuqipRecastRes(msg.result);
        info = msg.result.equip;
        RefreshOld();
        RefreshNew(false);
        HasBeenChoose = false;
    }
    void GetItemChange(uint id, object data)
    {
        RefreshCost();
    }
    #endregion

    #region refresh
    void RefreshOld()
    {
        //强化词缀
        mgrid_oldInten.MaxCount = info.intensifyAffixs.Count;
        for (int i = 0; i < IntenAffixOld.Count; i++)
        {
            IntenAffixOld[i].Recycle();
        }
        IntenAffixOld.Count = 0;
        IntenAffixOld.Count = mgrid_oldInten.MaxCount;
        for (int i = 0; i < mgrid_oldInten.MaxCount; i++)
        {
            IntenAffixOld[i].SetObj(mgrid_oldInten.controlList[i]);
            IntenAffixOld[i].Refresh(info.intensifyAffixs[i]);
        }
        //基础词缀
        mgrid_oldBase.MaxCount = info.baseAffixs.Count;
        for (int i = 0; i < baseAffixOld.Count; i++)
        {
            baseAffixOld[i].Recycle();
        }
        baseAffixOld.Count = 0;
        baseAffixOld.Count = mgrid_oldBase.MaxCount;
        for (int i = 0; i < mgrid_oldBase.MaxCount; i++)
        {
            baseAffixOld[i].SetObj(mgrid_oldBase.controlList[i]);
            baseAffixOld[i].Refresh(info.baseAffixs[i], info.configId);
        }
    }
    void RefreshNew(bool _state = true)
    {
        if (_state)
        {
            //基础词缀
            mgrid_NewBase.MaxCount = RefineMsg.baseAffixXilians.Count;
            for (int i = 0; i < baseAffixNew.Count; i++)
            {
                baseAffixNew[i].Recycle();
            }
            baseAffixNew.Count = 0;
            baseAffixNew.Count = mgrid_NewBase.MaxCount;
            for (int i = 0; i < mgrid_NewBase.MaxCount; i++)
            {
                baseAffixNew[i].SetObj(mgrid_NewBase.controlList[i]);
                baseAffixNew[i].Refresh(RefineMsg.baseAffixXilians[i], info.configId, info.id, RefineMsg.baseAffixXilians, RefineMsg.intensifyAffixXianlians);
            }
            ScriptBinder.StartCoroutine(RefreshNewInten());
        }
        else
        {
            for (int i = 0; i < baseAffixNew.Count; i++)
            {
                baseAffixNew[i].UnRefresh();
            }
            for (int i = 0; i < mgrid_NewInten.MaxCount; i++)
            {
                IntenAffixNew[i].UnRefresh();
            }
        }
    }
    IEnumerator RefreshNewInten()
    {
        //强化词缀
        mgrid_NewInten.MaxCount = RefineMsg.intensifyAffixXianlians.Count;
        for (int i = 0; i < IntenAffixNew.Count; i++)
        {
            IntenAffixNew[i].Recycle();
        }
        IntenAffixNew.Count = 0;
        IntenAffixNew.Count = mgrid_NewInten.MaxCount;
        for (int i = 0; i < mgrid_NewInten.MaxCount; i++)
        {
            yield return null;
            IntenAffixNew[i].SetObj(mgrid_NewInten.controlList[i]);
            IntenAffixNew[i].Refresh(RefineMsg.intensifyAffixXianlians[i]);
            IntenAffixNew[i].ShowEffect();
        }
    }
    void RefreshCost()
    {
        isMoneyEnough = true;
        isGoodsEnough = true;
        if (itemcfg == null)
        {
            return;
        }
        moneyCost = ZhanChongXiLianCostNewTableManager.Instance.GetCostByLevClass(itemcfg.levClass, 3);
        goodsCost = ZhanChongXiLianCostNewTableManager.Instance.GetCostByLevClass(itemcfg.levClass, 4);
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
    #endregion

    #region Click
    void CloseBtnClick(GameObject _go)
    {
        if (HasBeenChoose && !Constant.ShowTipsOnceList.Contains(87))
        {
            UtilityTips.ShowPromptWordTips(87, () =>
            {
                UIManager.Instance.ClosePanel<UIWoLongRefineResultPanel>();
            });
        }
        else
        {
            UIManager.Instance.ClosePanel<UIWoLongRefineResultPanel>();
        }
    }
    void RefineBtnClick(GameObject _go)
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

        if (HasBeenChoose && !Constant.ShowTipsOnceList.Contains(87))
        {
            UtilityTips.ShowPromptWordTips(87, () =>
            {
                FNDebug.Log("请求的index  " + index);
                Net.CSWoLongXiLianMessage(index, 1);
            });
        }
        else
        {
            Net.CSWoLongXiLianMessage(index, 1);
        }
    }
    void ConfirmBtnClick(GameObject _go)
    {
        if (!HasBeenChoose)
        {
            UtilityTips.ShowTips(666, 1.5f, ColorType.Green);
            return;
        }
        if (!Constant.ShowTipsOnceList.Contains(88))
        {
            UtilityTips.ShowPromptWordTips(88, () =>
            {
                Net.CSWoLongXiLianSelectMessage(index, 1);
            });
        }
        else
        {
            Net.CSWoLongXiLianSelectMessage(index, 1);
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
    #endregion
}
