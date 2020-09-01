using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public partial class UIShortcutItemPanel : UIBasePanel
{
    ILBetterList<QuickUseItem> itemList = new ILBetterList<QuickUseItem>(5);
    Dictionary<int, QuickUseItem> itemDic = new Dictionary<int, QuickUseItem>(5);
    List<List<int>> potion1;
    List<List<int>> potion2;
    List<List<int>> potion3;
    int potionGroup1Index = 0;
    int potionGroup2Index = 0;
    int potionGroup3Index = 0;
    const int K4 = 50000500;
    const int K5 = 50000501;

    List<List<int>> costList = new List<List<int>>();
    List<List<int>> costDic = new List<List<int>>();
    //药水气泡相关
    int limitCount = 0;
    int limitLv = 0;
    List<int> limitIDs = new List<int>();
    List<List<int>> HpMpGroupIds = new List<List<int>>();
    int AutoBuyVipLimilt = 4;

    public override void Init()
    {
        base.Init();
        mClientEvent.AddEvent(CEvent.BagInit, GetBagInit);
        mClientEvent.AddEvent(CEvent.FastUseClose, GetFastUseClose);

        UIEventListener.Get(mbtn_arrow).onClick = ChangeItemsState;
        UIEventListener.Get(mobj_bubble).onClick = BubbleClick;
        mgrid.MaxCount = 5;
        for (int i = 0; i < mgrid.MaxCount; i++)
        {
            itemList.Add(new QuickUseItem());
            itemList[i].Init(mgrid.controlList[i]);
        }
        potion1 = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(1048));
        potion2 = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(1049));
        potion3 = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(1050));
        //药水气泡配置
        int.TryParse(SundryTableManager.Instance.GetSundryEffect(1023), out limitCount);
        int.TryParse(SundryTableManager.Instance.GetSundryEffect(1025), out limitLv);
        limitIDs = UtilityMainMath.SplitStringToIntList(SundryTableManager.Instance.GetSundryEffect(1024));
        HpMpGroupIds = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(1026));

        //自动买药
        int.TryParse(SundryTableManager.Instance.GetSundryEffect(1138), out AutoBuyVipLimilt);
        if (CSMainPlayerInfo.Instance.VipLevel < AutoBuyVipLimilt)
        {
            CSMainPlayerInfo.Instance.mClientEvent.AddEvent(CEvent.OnMainPlayerVipLevelChanged, GetVipLvChange);
        }
    }


    protected override void OnDestroy()
    {
        CSMainPlayerInfo.Instance.mClientEvent.UnReg(CEvent.OnMainPlayerVipLevelChanged, GetVipLvChange);
        for (int i = 0; i < mgrid.MaxCount; i++)
        {
            itemList[i].Uninit();
        }
        base.OnDestroy();
    }
    void GetItemChange(uint id, object data)
    {
        if (data is EventData eventData)
        {
            bag.BagItemInfo info = (bag.BagItemInfo)eventData.arg1;
            ItemChangeType type = (ItemChangeType)eventData.arg2;
            if (null != info)
            {
                RefreshSingle(info.configId);
            }
        }
    }
    void GetBagInit(uint id, object data)
    {
        RefreshShowItemIcon();
        RefreshShowItemCount();
        mClientEvent.AddEvent(CEvent.ItemChange, GetItemChange);
    }

    void GetFastUseClose(uint id, object data)
    {
        RefreshShowItemIcon();
    }

    void GetVipLvChange(uint id, object data)
    {
        if (AutoBuyVipLimilt == CSMainPlayerInfo.Instance.Level)
        {
            PlayerPrefs.SetInt($"{CSMainPlayerInfo.Instance.ID}AutoBuyLiquidSetKey", 1);
            CSMainPlayerInfo.Instance.mClientEvent.UnReg(CEvent.OnMainPlayerVipLevelChanged, GetVipLvChange);
        }

    }
    void ChangeItemsState(GameObject _go)
    {
        UIManager.Instance.CreatePanel<UILiquidSettingPanel>();
    }
    void RefreshShowItemCount()
    {
        bubbleIndex = -1;
        for (int i = 0; i < mgrid.MaxCount; i++)
        {
            itemList[i].RefreshCount();
            if (bubbleIndex == -1 && itemList[i].IsRunOut() && i <= 1)
            {
                bubbleIndex = i;
                mlb_bubble.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1881), itemList[i].currentItemCfg.name, limitCount);
                mobj_bubble.transform.localPosition = new Vector3(-205 + 57 * (i), 5, 0);
            }
        }
        if (CSMainPlayerInfo.Instance.Level >= 50)
        {
            mobj_bubble.SetActive(bubbleIndex != -1);
        }
        else
        {
            mobj_bubble.SetActive(false);
        }
    }
    void RefreshSingle(int _cfgId)
    {
        QuickUseItem item;
        if (itemDic.TryGetValue(_cfgId, out item))
        {
            item.RefreshCount();
        }
        bubbleIndex = -1;
        for (int i = 0; i < 2; i++)
        {
            if (bubbleIndex == -1 && itemList[i].IsRunOut() && i <= 1)
            {
                bubbleIndex = i;
                mlb_bubble.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1881), itemList[i].currentItemCfg.name, limitCount);
                mobj_bubble.transform.localPosition = new Vector3(-205 + 57 * (i), 5, 0);
            }
        }
        if (CSMainPlayerInfo.Instance.Level >= 50)
        {
            mobj_bubble.SetActive(bubbleIndex != -1);
        }
        else
        {
            mobj_bubble.SetActive(false);
        }
    }
    int bubbleIndex = -1;
    void RefreshShowItemIcon()
    {
        potionGroup1Index = PlayerPrefs.GetInt($"{CSMainPlayerInfo.Instance.Name}quickUseId{1}", 0);
        potionGroup2Index = PlayerPrefs.GetInt($"{CSMainPlayerInfo.Instance.Name}quickUseId{2}", 0);
        potionGroup3Index = PlayerPrefs.GetInt($"{CSMainPlayerInfo.Instance.Name}quickUseId{3}", 0);
        itemDic.Clear();
        for (int i = 0; i < mgrid.MaxCount; i++)
        {
            if (i == 0)
            {
                itemList[i].Refresh(i, potion1[potionGroup1Index][1]);
                itemDic.Add(potion1[potionGroup1Index][1], itemList[i]);
            }
            else if (i == 1)
            {
                itemList[i].Refresh(i, potion2[potionGroup2Index][1]);
                itemDic.Add(potion2[potionGroup2Index][1], itemList[i]);
            }
            else if (i == 2)
            {
                itemList[i].Refresh(i, potion3[potionGroup3Index][1]);
                itemDic.Add(potion3[potionGroup3Index][1], itemList[i]);
            }
            else if (i == 3)
            {
                itemList[i].Refresh(i, K4);
                itemDic.Add(K4, itemList[i]);
            }
            else
            {
                itemList[i].Refresh(i, K5);
                itemDic.Add(K5, itemList[i]);
            }
        }
    }
    void BubbleClick(GameObject _go)
    {
        UIManager.Instance.CreatePanel<UIBagPanel>(p =>
        {
            (p as UIBagPanel).SelectChildPanel(3);
        });
    }
}


public class QuickUseItem : IDispose
{
    public GameObject go;
    Transform trans;
    UISprite icon;
    UILabel lb_count;
    UISprite cdMask;
    UILabel cdTime;
    Action<QuickUseItem> action;
    public int cfgId = 0;
    public long count = 0;
    public TABLE.ITEM currentItemCfg { get; private set; }
    int index = 0;
    public QuickUseItem()
    {
    }
    public void Init(GameObject _go)
    {
        go = _go;
        trans = _go.transform;
        icon = trans.Find("icon").GetComponent<UISprite>();
        lb_count = trans.Find("count").GetComponent<UILabel>();
        cdMask = trans.Find("cdmask").GetComponent<UISprite>();
        cdTime = trans.Find("cdmask/time").GetComponent<UILabel>();
        UIEventListener.Get(go).onClick = ItemClick;
    }
    public void Refresh(int _index, int _info)
    {
        index = _index;
        cfgId = _info;
        StopCD();

        if (null != currentItemCfg)
        {
            ItemCDManager.Instance.RemoveAction(currentItemCfg.group, UpdateFillAmount, StopCD);
        }

        currentItemCfg = CSBagInfo.Instance.GetCfg(cfgId);

        if (null != currentItemCfg)
        {
            ItemCDManager.Instance.AddAction(currentItemCfg.group, UpdateFillAmount, StopCD);
        }
        icon.spriteName = currentItemCfg.icon;
        RefreshCount(false);
    }
    public void RefreshCount(bool _autoBuy = true)
    {
        count = CSBagInfo.Instance.GetItemCount(cfgId);
        if (_autoBuy)
        {
            if (count <= 0)
            {
                AutoBuy();
            }
        }
        lb_count.text = count.ToString();
        lb_count.color = (count > 0) ? CSColor.green : CSColor.red;
    }
    void AutoBuy()
    {
        if (index > 2)
        {
            return;
        }
        int value = PlayerPrefs.GetInt($"{CSMainPlayerInfo.Instance.ID}AutoBuyLiquidSetKey", 0);
        bool AutoBuyState = (value == 1) ? true : false;
        if (AutoBuyState)
        {
            TABLE.SHOP shopCfg = null;
            var arr = ShopTableManager.Instance.array.gItem.handles;
            for (int k = 0, max = arr.Length; k < max; ++k)
            {
                if ((arr[k].Value as TABLE.SHOP).itemId == cfgId)
                {
                    shopCfg = arr[k].Value as TABLE.SHOP;
                    break;
                }
            }
            Net.CSShopBuyItemMessage(shopCfg.id, 1);
        }
    }
    void ItemClick(GameObject _go)
    {
        if (count > 0)
        {
            CSBagInfo.Instance.UseItem(CSBagInfo.Instance.GetItemInfoByCfgId(cfgId));
        }
        else
        {
            Utility.ShowGetWay(cfgId);
        }
        //if (action != null) { action(this); }
    }
    public bool IsRunOut()
    {
        return (count > 0) ? false : true;
    }

    void UpdateFillAmount(float value)
    {
        //Debug.Log($"{currentItemCfg.name}    {currentItemCfg.group}      跑CD");
        cdMask.CustomActive(true);
        cdMask.fillAmount = (1.0f - value);
        cdTime.text = ((1.0f - value) * currentItemCfg.itemcd * 0.001f).ToString("F1");
    }

    void StopCD()
    {
        if (null != currentItemCfg)
            //Debug.Log($"{currentItemCfg.name}    {currentItemCfg.group}      结束结束结束结束结束结束结束结束结束结束");
            cdMask.CustomActive(false);
    }
    public void Uninit()
    {
        StopCD();
        ItemCDManager.Instance.RemoveAction(currentItemCfg.group, UpdateFillAmount, StopCD);
    }
    public void Dispose()
    {
    }
}