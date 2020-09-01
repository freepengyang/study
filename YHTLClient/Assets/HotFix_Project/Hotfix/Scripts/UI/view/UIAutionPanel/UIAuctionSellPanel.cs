using auction;
using bag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIAuctionSellPanel : UIBasePanel
{
    SelfAuctionItems sellMes;
    List<AuctionSellItem> sellItemList = new List<AuctionSellItem>();
    List<AuctionItemData> sellBagMes = new List<AuctionItemData>();
    List<UIItemBase> bagitems = new List<UIItemBase>();
    int bagitemCount = 6;
    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint)CEvent.ECM_ResGetAuctionShelfMessage, GetSellMes);
        mClientEvent.Reg((uint)CEvent.ECM_ResAddToShelfMessage, GetSellListAdddMes);
        mClientEvent.Reg((uint)CEvent.ECM_ResRemoveFromShelfMessage, GetSellListChangeMes);
        mClientEvent.Reg((uint)CEvent.ECM_ResUnlockAuctionShelveMessage, GetShelveUnlockMes);
        mClientEvent.Reg((uint)CEvent.ECM_ResBuyAuctionItemMessage, GetAuctionBuyMes);
        UIEventListener.Get(mbtn_help).onClick = (p => { UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.Auction); });
        mgrid_sellItems.MaxCount = 8;
        for (int i = 0; i < 8; i++)
        {
            sellItemList.Add(new AuctionSellItem(mgrid_sellItems.transform.GetChild(i).gameObject));
        }
        RefreshBagPart();
        Net.ReqGetAuctionShelfMessage();


    }

    public override void Show()
    {
        base.Show();
    }
    protected override void OnDestroy()
    {
        for (int i = 0; i < bagitems.Count; i++)
        {
            UIItemManager.Instance.RecycleSingleItem(bagitems[i]);
        }
        for (int i = 0; i < sellItemList.Count; i++)
        {
            sellItemList[i].UnInit();
        }
        sellItemList = null;
        bagitemCount = 6;
        base.OnDestroy();
    }
    public override void SelectItem(TipsBtnData _data)
    {
        UIManager.Instance.CreatePanel<UIAuctionOperationPanel>(p =>
        {
            AuctionItemData aucData = mPoolHandleManager.GetCustomClass<AuctionItemData>();
            aucData.type = 1;
            aucData.bagInfo = _data.info;
            (p as UIAuctionOperationPanel).ShowState(AuctionOperation.PutAway, aucData);
        });

    }

    public override void OnShow(int typeId = 0)
    {
        base.OnShow(typeId);
    }
    public override void OnHide()
    {
        base.OnHide();
    }
    void GetSellMes(uint id, object data)
    {
        sellMes = UIAuctionInfo.Instance.ReturnSellMes();
        ReFreshSellPart();
    }
    /// <summary>
    /// 上架返回
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void GetSellListAdddMes(uint id, object data)
    {
        auction.AuctionItemInfo info = (auction.AuctionItemInfo)data;
        int itemCfgId = info.itemType == 2 ? HandBookTableManager.Instance.GetHandBookItemID(info.tujianItem.handBookId) : info.item.configId;

        if (ItemTableManager.Instance.GetItemSaleType(itemCfgId) == 1)
        {
            UtilityTips.ShowPromptWordTips(23, null, $"{Math.Round(Convert.ToDecimal(sellMes.taxRate * 0.01f), 0, MidpointRounding.AwayFromZero)}%");

        }
        else if (ItemTableManager.Instance.GetItemSaleType(itemCfgId) == 2)
        {
            UtilityTips.ShowPromptWordTips(21, null, $"{Math.Round(Convert.ToDecimal(sellMes.taxRate * 0.01f), 0, MidpointRounding.AwayFromZero)}%");
        }
        sellMes = UIAuctionInfo.Instance.ReturnSellMes();
        Debug.Log($"上架返回  {sellMes.items.Count}");
        ReFreshSellPart();
        RefreshBagPart();
    }
    void GetSellListChangeMes(uint id, object data)
    {
        sellMes = UIAuctionInfo.Instance.ReturnSellMes();
        Debug.Log("下架返回   "+sellMes.items.Count);
        ReFreshSellPart();
        RefreshBagPart();
    }
    void GetShelveUnlockMes(uint id, object data)
    {
        sellMes = UIAuctionInfo.Instance.ReturnSellMes();
        //FNDebug.Log("收到解锁返回   " + sellMes.shelve);
        ReFreshSellPart();
    }
    void GetAuctionBuyMes(uint id,object data)
    {
        RefreshBagPart();
    }
    void ReFreshSellPart()
    {
        //UnityEngine.Debug.Log("货架数量  " + sellMes.shelve);
        //1：锁定   2：空闲   3：使用中
        //货架数量 1~4
        for (int i = 0; i < sellItemList.Count; i++)
        {
            if (i >= 4)
            {
                if (i < sellMes.items.Count)
                {
                    sellItemList[i].Refresh(3, sellMes.items[i]);
                }
                else
                {
                    if ((i - 3) <= sellMes.shelve)
                    {
                        sellItemList[i].Refresh(2);
                    }
                    else
                    {
                        sellItemList[i].Refresh(1, null, i);
                    }
                }
            }
            else
            {
                if (i >= sellMes.items.Count)
                {
                    sellItemList[i].Refresh(2);
                }
                else
                {
                    sellItemList[i].Refresh(3, sellMes.items[i]);
                }
            }
        }
    }
    void RefreshBagPart()
    {
        UIAuctionInfo.Instance.GetSellBagMes(sellBagMes);
        if (sellBagMes.Count == 0)
        {
            for (int i = 0; i < bagitemCount * 3; i++)
            {
                bagitems.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, mgrid_bagItems.transform, itemSize.Size66));
                bagitems[i].UnInit();
            }
        }
        else
        {
            int count = sellBagMes.Count / 3 + 1;
            bagitemCount = count > bagitemCount ? count : bagitemCount;

            if (bagitems.Count < bagitemCount * 3)
            {
                int gap = bagitemCount * 3 - bagitems.Count;
                for (int i = 0; i < gap; i++)
                {
                    bagitems.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, mgrid_bagItems.transform, itemSize.Size66));
                }
                for (int i = 0; i < bagitemCount * 3; i++)
                {
                    if (i < sellBagMes.Count)
                    {
                        bagitems[i].SetMaskJudgeState(true);
                        if (sellBagMes[i].type == 1)
                        {
                            bagitems[i].SetExtendData(sellBagMes[i]);
                            bagitems[i].SetExtend1Data(sellBagMes[i]);
                            bagitems[i].Refresh(sellBagMes[i].bagInfo, Click, false);
                        }
                        else if (sellBagMes[i].type == 2)
                        {
                            bagitems[i].SetExtendData(sellBagMes[i]);
                            bagitems[i].SetExtend1Data(sellBagMes[i]);
                            bagitems[i].Refresh(sellBagMes[i].tujianCfgId, Click, false);
                        }
                        else if (sellBagMes[i].type == 3)
                        {
                            bagitems[i].SetExtendData(sellBagMes[i]);
                            bagitems[i].SetExtend1Data(sellBagMes[i]);
                            bagitems[i].Refresh(sellBagMes[i].bagInfo, Click, false);
                        }
                    }
                    else
                    {
                        bagitems[i].UnInit();
                    }
                }
            }
            else
            {
                int gap = bagitems.Count - bagitemCount * 3;
                for (int i = 0; i < gap; i++)
                {
                    bagitems.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, mgrid_bagItems.transform, itemSize.Size66));
                }
                for (int i = 0; i < bagitems.Count; i++)
                {
                    if (i < sellBagMes.Count)
                    {
                        bagitems[i].SetMaskJudgeState(true);
                        if (sellBagMes[i].type == 1)
                        {
                            bagitems[i].SetExtendData(sellBagMes[i]);
                            bagitems[i].SetExtend1Data(sellBagMes[i]);
                            bagitems[i].Refresh(sellBagMes[i].bagInfo, Click, false);
                        }
                        else if (sellBagMes[i].type == 2)
                        {
                            bagitems[i].SetExtendData(sellBagMes[i]);
                            bagitems[i].SetExtend1Data(sellBagMes[i]);
                            bagitems[i].Refresh(sellBagMes[i].tujianCfgId, Click, false);
                        }
                        else if (sellBagMes[i].type == 3)
                        {
                            bagitems[i].SetExtendData(sellBagMes[i]);
                            bagitems[i].SetExtend1Data(sellBagMes[i]);
                            bagitems[i].Refresh(sellBagMes[i].bagInfo, Click, false);
                        }
                    }
                    else
                    {
                        bagitems[i].UnInit();
                    }
                }
            }
        }
        mgrid_bagItems.Reposition();
    }
    void Click(UIItemBase _item)
    {
        UIManager.Instance.CreatePanel<UIAuctionOperationPanel>(p =>
        {
            (p as UIAuctionOperationPanel).ShowState(AuctionOperation.PutAway, _item.ExtendData);
        });

    }

}

public class AuctionSellRowItem
{
    List<UIItemBase> itemList = new List<UIItemBase>(3);
    GameObject go;

    public AuctionSellRowItem(GameObject _go)
    {
        go = _go;
        itemList = UIItemManager.Instance.GetUIItems(3, PropItemType.Normal, go.transform, itemSize.Size64);

    }
    public void ReSet(GameObject _go, List<AuctionItemData> _list)
    {
        go = _go;

    }

}

public class AuctionSellItem
{
    public GameObject go;
    public UILabel lb_price;
    public UILabel lb_time;
    public UILabel lb_name;
    public UILabel lb_state;
    public GameObject obj_isFree;
    public GameObject obj_lock;
    public UILabel lb_lockdes;
    public GameObject obj_used;
    public GameObject itemPar;
    public UIItemBase item;
    public UISprite priceIcon;
    public UISprite bg;
    public AuctionItemInfo info;
    int state = 0;
    int index = 0;
    public AuctionSellItem(GameObject _go)
    {
        go = _go;
        obj_isFree = go.transform.Find("isFree").gameObject;
        obj_lock = go.transform.Find("lock").gameObject;
        lb_lockdes = go.transform.Find("lock/LockSign/Label").GetComponent<UILabel>();
        obj_used = go.transform.Find("Used").gameObject;
        lb_time = go.transform.Find("Used/time").GetComponent<UILabel>();
        lb_name = go.transform.Find("Used/name").GetComponent<UILabel>();
        lb_state = go.transform.Find("Used/state").GetComponent<UILabel>();
        lb_price = go.transform.Find("Used/Price").GetComponent<UILabel>();
        itemPar = go.transform.Find("Used/itemPar").gameObject;
        priceIcon = go.transform.Find("Used/Price/ingotSymbol").GetComponent<UISprite>();
        bg = go.transform.Find("backFrame").GetComponent<UISprite>();
        UIEventListener.Get(go).onClick = Click;
    }
    int GetSellIndByVipLevel(int _index)
    {
        var arr = VIPTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.VIP;
            if ((item.auctionCount) > _index)
            {
                return item.id;
            }
        }
        return 0;
    }
    void Click(GameObject _go)
    {
        if (state == 1)
        {
            List<List<int>> lsit = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(409));
            if (index >= 4)
            {
                int ind = index - 4;
                int itemId = lsit[ind][0];
                int itemNum = lsit[ind][1];
                TABLE.ITEM cfg = CSYuanBaoInfo.Instance.GetYuanBaoConfig();
                if (cfg == null) return;
                string str = "";
                if (CSYuanBaoInfo.Instance.GetYuanBao(itemId) >= itemNum)
                {
                    str = $"[00ff00]{itemNum}个{cfg.name}[-]";
                }
                else
                {
                    str = $"[ff0000]{itemNum}个{cfg.name}[-]";
                }
                UtilityTips.ShowPromptWordTips(19, ConfirmUnlockItems, str);
            }
        }
        else if (state == 2)
        {
            //UIManager.Instance.CreatePanel<UIAuctionOperationPanel>(p =>
            //{
            //    (p as UIAuctionOperationPanel).ShowState(AuctionOperation.PutAway,null);
            //});
        }
        else if (state == 3)
        {
            UIManager.Instance.CreatePanel<UIAuctionOperationPanel>(p =>
            {
                (p as UIAuctionOperationPanel).ShowState(AuctionOperation.SoldOut, info);
            });
        }
    }
    public void Refresh(int _state, AuctionItemInfo _sellinfo = null, int _index = 0)
    {
        info = _sellinfo;
        state = _state;
        index = _index;
        //1：锁定   2：空闲   3：使用中
        obj_lock.SetActive(state == 1);
        obj_isFree.SetActive(state == 2);
        obj_used.SetActive(state == 3);
        if (state == 1)
        {
            List<List<int>> lsit = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(409));
            int ind = index - 4;
            int itemId = lsit[ind][0];
            int itemNum = lsit[ind][1];

            bg.alpha = 0.5f;
            int vipLV = GetSellIndByVipLevel(index);
            lb_lockdes.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1718), vipLV, itemNum);
            //Debug.Log($"{index}    {vipLV}   {itemNum}");
        }
        else if (state == 2)
        {
            bg.alpha = 1f;
        }
        else
        {
            bg.alpha = 1f;
            if (item == null)
            {
                item = UIItemManager.Instance.GetItem(PropItemType.Normal, itemPar.transform, itemSize.Size64);
                item.SetMaskJudgeState(true);
            }
            int itemCfgId = 0;
            if (info.itemType == 1 || info.itemType == 3)
            {
                item.Refresh(info.item);
                itemCfgId = info.item.configId;
                lb_name.color = UtilityCsColor.Instance.GetColor(ItemTableManager.Instance.GetItemQuality(itemCfgId));
            }
            else
            {
                itemCfgId = HandBookTableManager.Instance.GetHandBookItemID(info.tujianItem.handBookId);
                item.SetExtendData(info);
                item.Refresh(itemCfgId, ShowHandbook, false);
                int qua = (int)((info.tujianItem.handBookId << 0) >> 24);
                lb_name.color = UtilityCsColor.Instance.GetColor(qua);
                item.SetQuality(qua);
            }
            lb_name.text = ItemTableManager.Instance.GetItemName(itemCfgId);
            lb_price.text = info.price.ToString();
            priceIcon.spriteName = (info.priceType == 1) ? $"tubiao{1}" : $"tubiao{3}";
            //Debug.Log(info.showTime + "  <=show    add=>  " + info.addTime);
            if (info.showTime > 0)
            {

                lb_time.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(63), info.showTime);
                lb_state.text = ClientTipsTableManager.Instance.GetClientTipsContext(1683);
                lb_state.color = CSColor.green;
            }
            else
            {
                //小于7天显示出售  大于7天显示已下架
                if (259200000 > (CSServerTime.Instance.TotalMillisecond - info.addTime))
                {
                    long a = info.addTime + (259200000) - CSServerTime.Instance.TotalMillisecond;
                    long hour = a / (60 * 60 * 1000);
                    lb_time.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(63), hour);
                    //lb_time.text = $"{hour}小时";
                    lb_state.text = ClientTipsTableManager.Instance.GetClientTipsContext(1684);
                    lb_state.color = CSColor.green;
                }
                else
                {
                    lb_time.text = "";
                    lb_state.text = ClientTipsTableManager.Instance.GetClientTipsContext(1685);
                    lb_state.color = CSColor.red;
                }
            }
        }
    }
    void ShowHandbook(UIItemBase _item)
    {
        AuctionItemInfo info = (AuctionItemInfo)_item.ExtendData;
        UIManager.Instance.CreatePanel<UIHandBookTipsPanel>(f =>
        {
            (f as UIHandBookTipsPanel).Show(info.tujianItem.handBookId, info.tujianItem.id, 1 << (int)UIHandBookTipsPanel.MenuType.MT_NO_MENU);
        });
    }
    void ConfirmUnlockItems()
    {
        List<List<int>> lsit = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(409));
        int keyId = index - 4;
        if (keyId < 0 || lsit.Count <= keyId) return;
        if (lsit[keyId] == null || lsit[keyId].Count < 2) return;
        long num = CSYuanBaoInfo.Instance.GetYuanBao(lsit[keyId][0]);
        if (num >= lsit[keyId][1])
        {
            Net.ReqUnlockAuctionShelveMessage();
        }
        else CSYuanBaoInfo.Instance.ShowGetWay(lsit[keyId][0]);
    }
    public void UnInit()
    {
        if (item != null)
        {
            UIItemManager.Instance.RecycleSingleItem(item);
        }
        go = null;
        obj_isFree = null;
        obj_lock = null;
        obj_used = null;
        lb_time = null;
        lb_name = null;
        lb_price = null;
    }
}

