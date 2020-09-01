using auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum AuctionOperation
{
    /// <summary>
    /// 购买
    /// </summary>
    Buy,
    /// <summary>
    /// 下架
    /// </summary>
    SoldOut,
    /// <summary>
    /// 上架
    /// </summary>
    PutAway,
    /// <summary>
    /// 公示
    /// </summary>
    Show,
}
public partial class UIAuctionOperationPanel : UIBasePanel
{


    public override UILayerType PanelLayerType => UILayerType.Tips;
    AuctionOperation panelType;
    UIItemBase item;
    AuctionItemData sellItemInfo;
    AuctionItemInfo sellinfo;
    AuctionItemInfo buyinfo;
    AuctionItemInfo showinfo;
    int num = 1;
    //int price = 0;
    //int tax = 0;
    bool isAttention = false;

    List<int> serviceCharge = new List<int>();

    //上架最低价格
    int minSellPrice;

    public override void Init()
    {
        //Debug.Log(" Init  ");
        base.Init();
        UIEventListener.Get(mbtn_close).onClick = CloseClick;
        UIEventListener.Get(mobj_bg).onClick = CloseClick;
        //上架
        UIEventListener.Get(mbtn_cancelSell).onClick = CloseClick;
        UIEventListener.Get(mbtn_sellNumAdd).onClick = SellNumAdd;
        UIEventListener.Get(mbtn_sellNumReduce).onClick = SellNumReduce;
        UIEventListener.Get(mbtn_confirmSell).onClick = ConfirmPutAway;
        minput_sellNum.onChange.Add(new EventDelegate(OnSellNumChange));
        minput_sellPrice.onChange.Add(new EventDelegate(OnSellPriceChange));
        minput_sellPrice.onValidate = OnValidateInput;
        //下架
        UIEventListener.Get(mbtn_cancelSoldOut).onClick = CryOut;
        UIEventListener.Get(mbtn_SoldOut).onClick = SoldOut;
        //购买
        UIEventListener.Get(mbtn_buy).onClick = Buy;
        UIEventListener.Get(mbtn_buyAttention).onClick = Attention;
        //公示
        UIEventListener.Get(mbtn_showAtten).onClick = ShowAtten;


        string sundryStr = SundryTableManager.Instance.GetSundryEffect(408);
        if (!string.IsNullOrEmpty(sundryStr))
        {
            serviceCharge = UtilityMainMath.SplitStringToIntList(sundryStr);
        }

        sundryStr = SundryTableManager.Instance.GetSundryEffect(1016);
        if (!string.IsNullOrEmpty(sundryStr))
        {
            int.TryParse(sundryStr, out minSellPrice);
        }

        minput_sellNum.onValidate = OnValidateInput;
        minput_buyNum.onValidate = OnValidateInput;
        item = UIItemManager.Instance.GetItem(PropItemType.Normal, mobj_itemPar);
        item.SetMaskJudgeState(true);
    }

    public override void Show()
    {
        base.Show();
    }
    protected override void OnDestroy()
    {

        base.OnDestroy();
    }
    char OnValidateInput(string text, int charIndex, char addedChar)
    {
        int num = 0;
        if (int.TryParse(addedChar.ToString(), out num))
        {
            return addedChar;
        }
        else
        {
            return (char)0;
        }
        //if (addedChar == '-' || addedChar == '.')
        //    return (char)0;
    }
    void CloseClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIAuctionOperationPanel>();
    }
    #region  上架
    void OnSellNumChange()
    {
        if (item != null && item.itemCfg != null)
        {
            if (item.itemCfg.type == (int)ItemType.Equip || item.itemCfg.type == (int)ItemType.Handbook || item.itemCfg.type == (int)ItemType.NostalgicEquip)
            {
                num = 1;
                minput_sellNum.value = num.ToString();
            }
        }
    }
    void SellNumAdd(GameObject _go)
    {
        if (sellItemInfo.type == 1)
        {
            num++;
            num = (num >= sellItemInfo.bagInfo.count) ? sellItemInfo.bagInfo.count : num;
        }
        else
        {
            num = 1;
        }
        minput_sellNum.value = num.ToString();
    }
    void SellNumReduce(GameObject _go)
    {
        num--;
        num = (num < 1) ? 1 : num;
        minput_sellNum.value = num.ToString();
    }
    void OnSellPriceChange()
    {
        if (string.IsNullOrEmpty(minput_sellPrice.value))
        {
            return;
        }
        float serviceValue = int.Parse(minput_sellPrice.value) * num;
        if (serviceCharge != null && serviceCharge.Count > 2)
        {
            serviceValue = serviceValue * (serviceCharge[2] / 10000f);
            serviceValue = serviceValue < serviceCharge[0] ? serviceCharge[0] : serviceValue > serviceCharge[1] ? serviceCharge[1] : serviceValue;
        }
        mlb_sellCharge.text = (serviceValue).ToString("F0");
        int priceNum = 0;
        int.TryParse(minput_sellPrice.value, out priceNum);
        minput_sellPrice.activeTextColor = priceNum < minSellPrice ? CSColor.red : CSColor.green;
        minput_sellPrice.label.color = priceNum < minSellPrice ? CSColor.red : CSColor.green;
        int itemcfgId = sellItemInfo.type == 1 ? sellItemInfo.bagInfo.configId : sellItemInfo.tujianCfgId;
        int chargeNum = int.Parse(mlb_sellCharge.text);
        if (CSItemCountManager.Instance.GetItemCount((int)MoneyType.gold) < chargeNum)
        {
            mlb_sellCharge.color = CSColor.red;
        }
        else
        {
            mlb_sellCharge.color = CSColor.green;
        }
    }
    void ConfirmPutAway(GameObject _go)
    {
        int itemcfgId = sellItemInfo.type == 2 ? sellItemInfo.tujianCfgId : sellItemInfo.bagInfo.configId;
        int priceNum = 0;
        int.TryParse(minput_sellPrice.value, out priceNum);
        if (priceNum < minSellPrice)
        {
            UtilityTips.ShowTips(1871);
            return;
        }
        //判断上架手续费够不够
        int chargeNum = int.Parse(mlb_sellCharge.text);
        if (CSItemCountManager.Instance.GetItemCount((int)MoneyType.gold) < chargeNum)
        {
            UtilityTips.ShowTips(1749, 1.5f, ColorType.Red);
            Utility.ShowGetWay(1);
            return;
        }
        if (ItemTableManager.Instance.GetItemSaleType(itemcfgId) == 1)
        {
            //弹出珍贵道具上架提示
            UtilityTips.ShowPromptWordTips(22, () =>
            {
                int totalprice = int.Parse(minput_sellPrice.value) * num;
                //Debug.Log(sellItemInfo.bagIndex + "             " + sellItemInfo.count + "    " + totalprice);
                if (sellItemInfo.type == 1)
                {
                    Net.ReqAddToShelfMessage(sellItemInfo.bagInfo.bagIndex, sellItemInfo.bagInfo.count, totalprice, 0, 1);
                }
                else
                {
                    Net.ReqAddToShelfMessage(0, 1, totalprice, sellItemInfo.tujianId, 2);
                }
                UIManager.Instance.ClosePanel<UIAuctionOperationPanel>();
            }, $"{Math.Round(Convert.ToDecimal(UIAuctionInfo.Instance.ReturnSellMes().taxRate * 0.01f), 0, MidpointRounding.AwayFromZero)}%");
        }
        else
        {
            if (sellItemInfo != null)
            {
                int totalprice = int.Parse(minput_sellPrice.value) * num;
                //Debug.Log(sellItemInfo.bagIndex + "             " + sellItemInfo.count + "    " + totalprice);
                if (sellItemInfo.type == 1)
                {
                    Net.ReqAddToShelfMessage(sellItemInfo.bagInfo.bagIndex, sellItemInfo.bagInfo.count, totalprice, 0, 1);
                }
                else if (sellItemInfo.type == 2)
                {
                    Net.ReqAddToShelfMessage(0, 1, totalprice, sellItemInfo.tujianId, 2);
                }
                else if (sellItemInfo.type == 3)
                {
                    Net.ReqAddToShelfMessage(sellItemInfo.bagInfo.bagIndex, sellItemInfo.bagInfo.count, totalprice, sellItemInfo.bagInfo.id, 3);
                }
                UIManager.Instance.ClosePanel<UIAuctionOperationPanel>();
            }
        }
    }
    #endregion
    #region 下架（下架，吆喝）
    void SoldOut(GameObject _go)
    {
        if (sellinfo != null)
        {
            if (sellinfo.itemType == 1)
            {
                Net.ReqRemoveFromShelfMessage(sellinfo.lid, sellinfo.item.count);
            }
            else
            {
                Net.ReqRemoveFromShelfMessage(sellinfo.lid, 1);
            }
            UIManager.Instance.ClosePanel<UIAuctionOperationPanel>();
        }
    }
    void CryOut(GameObject _go)
    {
        if (sellinfo != null)
        {
            Net.ReqAuctionCryMessage(sellinfo.lid);
            UIManager.Instance.ClosePanel<UIAuctionOperationPanel>();
        }
    }
    #endregion
    #region 买（购买，关注）
    void Buy(GameObject _go)
    {
        if (buyinfo != null)
        {
            Net.ReqBuyAuctionAuctionMessage(buyinfo.lid, num);
            UIManager.Instance.ClosePanel<UIAuctionOperationPanel>();
        }
    }
    void Attention(GameObject _go)
    {
        if (buyinfo != null)
        {
            if (isAttention)
            {
                Net.ReqCancelAttentionAuctionMessage(buyinfo.lid);
            }
            else
            {
                Net.ReqAttentionAuctionMessage(buyinfo.lid);
            }
            UIManager.Instance.ClosePanel<UIAuctionOperationPanel>();
        }
    }
    #endregion
    #region 公示
    void ShowAtten(GameObject _go)
    {
        if (showinfo != null)
        {
            if (isAttention)
            {
                Net.ReqCancelAttentionAuctionMessage(showinfo.lid);
            }
            else
            {
                Net.ReqAttentionAuctionMessage(showinfo.lid);
            }
            UIManager.Instance.ClosePanel<UIAuctionOperationPanel>();
        }
    }
    #endregion


    public void ShowState(AuctionOperation _type, object data, bool _isAtten = false)
    {
        //Debug.Log(" ShowState  ");
        panelType = _type;
        isAttention = _isAtten;
        ChangeType(data);
    }
    void ChangeType(object data)
    {
        num = 1;
        mobj_buy.SetActive(panelType == AuctionOperation.Buy);
        mobj_soldOut.SetActive(panelType == AuctionOperation.SoldOut);
        mobj_putAway.SetActive(panelType == AuctionOperation.PutAway);
        mobj_Show.SetActive(panelType == AuctionOperation.Show);
        if (panelType == AuctionOperation.Buy)
        {
            buyinfo = (AuctionItemInfo)data;

            int itemCfgId = 0;
            if (buyinfo.itemType == 1 || buyinfo.itemType == 3)
            {
                itemCfgId = buyinfo.item.configId;
                item.Refresh(buyinfo.item);
                mlb_name.color = UtilityCsColor.Instance.GetColor(ItemTableManager.Instance.GetItemQuality(itemCfgId));

            }
            else if (buyinfo.itemType == 1)
            {
                itemCfgId = HandBookTableManager.Instance.GetHandBookItemID(buyinfo.tujianItem.handBookId);
                item.SetExtendData(buyinfo);
                item.Refresh(itemCfgId, ShowHandbook, false);
                int qua = (int)((buyinfo.tujianItem.handBookId << 0) >> 24);
                mlb_name.color = UtilityCsColor.Instance.GetColor(qua);
                item.SetQuality(qua);
            }
            mlb_name.text = ItemTableManager.Instance.GetItemName(itemCfgId);
            minput_buyNum.value = num.ToString();
            mlb_buyPrice.text = buyinfo.price.ToString();
            UILabel attenDes = mbtn_buyAttention.transform.Find("Label").GetComponent<UILabel>();
            attenDes.text = (isAttention == true) ? "取 关" : "关 注";
            mbtn_buyAttention.SetActive(false);
            UIEventListener.Get(msp_buyCost.gameObject, 3).onClick = ShowGetWay;
        }
        else if (panelType == AuctionOperation.SoldOut)
        {
            sellinfo = (AuctionItemInfo)data;
            int itemCfgId = 0;
            if (sellinfo.itemType == 1 || sellinfo.itemType == 3)
            {
                item.Refresh(sellinfo.item);
                itemCfgId = sellinfo.item.configId;
                mlb_soldOutNum.text = sellinfo.item.count.ToString();
                mlb_name.color = UtilityCsColor.Instance.GetColor(ItemTableManager.Instance.GetItemQuality(itemCfgId));
            }
            else if (sellinfo.itemType == 2)
            {
                itemCfgId = HandBookTableManager.Instance.GetHandBookItemID(sellinfo.tujianItem.handBookId);
                item.SetExtendData(sellinfo);
                item.Refresh(itemCfgId, ShowHandbook, false);
                mlb_soldOutNum.text = "1";
                int qua = (int)((sellinfo.tujianItem.handBookId << 0) >> 24);
                mlb_name.color = UtilityCsColor.Instance.GetColor(qua);
                item.SetQuality(qua);
            }
            mlb_name.text = ItemTableManager.Instance.GetItemName(itemCfgId);
            mlb_soldOutprice.text = sellinfo.price.ToString();
            if (sellinfo.showTime > 0)
            {
                mlb_soldOutTime.gameObject.SetActive(true);
                mlb_soldOutTime.text = $"{sellinfo.showTime}小时";
                mlb_soldOutNum.transform.parent.gameObject.SetActive(false);
                mlb_soldOutprice.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                mlb_soldOutTime.gameObject.SetActive(false);
                mlb_soldOutNum.transform.parent.gameObject.SetActive(true);
                mlb_soldOutprice.transform.parent.gameObject.SetActive(true);
            }
            UIEventListener.Get(msp_soldOutCost.gameObject, 3).onClick = ShowGetWay;
        }
        else if (panelType == AuctionOperation.PutAway)
        {
            sellItemInfo = (AuctionItemData)data;
            int itemcfgId = sellItemInfo.type == 2 ? sellItemInfo.tujianCfgId : sellItemInfo.bagInfo.configId;
            mlb_name.text = ItemTableManager.Instance.GetItemName(itemcfgId);
            if (sellItemInfo.type == 1)
            {
                mlb_name.color = UtilityCsColor.Instance.GetColor(ItemTableManager.Instance.GetItemQuality(itemcfgId));
                item.Refresh(sellItemInfo.bagInfo);
            }
            else if (sellItemInfo.type == 2)
            {
                item.SetExtendData(sellItemInfo);
                item.Refresh(itemcfgId, ShowSellHandbook, false);
                int qua = (int)((sellItemInfo.tujianId << 0) >> 24);
                mlb_name.color = UtilityCsColor.Instance.GetColor(qua);
                item.SetQuality(qua);
            }
            else if (sellItemInfo.type == 3)
            {
                mlb_name.color = UtilityCsColor.Instance.GetColor(ItemTableManager.Instance.GetItemQuality(itemcfgId));
                item.Refresh(sellItemInfo.bagInfo);
            }
            minput_sellNum.value = num.ToString();
            int recommend = ItemTableManager.Instance.GetItemRecommend(itemcfgId);
            if (recommend > 0)
            {
                minput_sellPrice.value = recommend.ToString();
            }
            //minput_sellPrice.value = recommend > 0 ? recommend.ToString() : "";
            UIEventListener.Get(msp_putawayCost.gameObject, 3).onClick = ShowGetWay;
            UIEventListener.Get(msp_putawayCharge.gameObject, 1).onClick = ShowGetWay;
        }
        else if (panelType == AuctionOperation.Show)
        {
            showinfo = (AuctionItemInfo)data;
            int itemCfgId = 0;
            if (showinfo.itemType == 1)
            {
                itemCfgId = showinfo.item.configId;
                item.Refresh(showinfo.item);
                mlb_name.color = UtilityCsColor.Instance.GetColor(ItemTableManager.Instance.GetItemQuality(itemCfgId));
            }
            else
            {
                itemCfgId = HandBookTableManager.Instance.GetHandBookItemID(showinfo.tujianItem.handBookId);
                item.Refresh(itemCfgId);
                int qua = (int)((showinfo.tujianItem.handBookId << 0) >> 24);
                mlb_name.color = UtilityCsColor.Instance.GetColor(qua);
                item.SetQuality(qua);
            }
            mlb_name.text = ItemTableManager.Instance.GetItemName(itemCfgId);
            mlb_showAtten.text = (isAttention == true) ? "取 关" : "关 注";
            mlb_showtime.text = $"{showinfo.showTime}小时";
        }
    }

    void ShowHandbook(UIItemBase _item)
    {
        AuctionItemInfo info = (AuctionItemInfo)_item.ExtendData;
        UIManager.Instance.CreatePanel<UIHandBookTipsPanel>(f =>
        {
            FNDebug.Log($"{info.tujianItem.handBookId}   {info.tujianItem.id}");
            (f as UIHandBookTipsPanel).Show(info.tujianItem.handBookId, info.tujianItem.id, 1 << (int)UIHandBookTipsPanel.MenuType.MT_NO_MENU);
        });
    }
    void ShowSellHandbook(UIItemBase _item)
    {
        AuctionItemData info = (AuctionItemData)_item.ExtendData;
        UIManager.Instance.CreatePanel<UIHandBookTipsPanel>(f =>
        {
            FNDebug.Log($"{info.tujianCfgId}   {info.tujianId}");
            (f as UIHandBookTipsPanel).Show(info.handbookId, info.tujianId, 1 << (int)UIHandBookTipsPanel.MenuType.MT_NO_MENU);
        });
    }
    void RefreshItem(TABLE.ITEM _cfg)
    {

    }
    void ShowGetWay(GameObject _go)
    {
        int id = (int)UIEventListener.Get(_go).parameter;
        UITipsManager.Instance.CreateTips(TipsOpenType.Normal, id);
    }
}
