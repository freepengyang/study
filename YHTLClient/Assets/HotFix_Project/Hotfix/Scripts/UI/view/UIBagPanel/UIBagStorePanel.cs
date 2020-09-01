using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UIBagStorePanel : UIBasePanel
{
    public UIBagStorePanel()
    {
        //Init();
    }
    #region forms

    UIScrollView item_Scroll;
    UIGridContainer mgrid_items;
    private CSBetterList<TABLE.SHOP> cfgList;

    #endregion

    protected override void _InitScriptBinder()
    {
        item_Scroll = ScriptBinder.GetObject("item_Scroll") as UIScrollView;
        mgrid_items = ScriptBinder.GetObject("grid_items") as UIGridContainer;
        mClientEvent.Reg((uint)CEvent.MoneyChange, OnMoneyChange);
        mClientEvent.Reg((uint)CEvent.player_levelChange, OnPlayerLvChange);
    }
 
    #region  variable

    #endregion


    public override void Init()
    {
        base.Init();
        RefreshShopList();
    }
    private void RefreshShopList()
    {
        cfgList = GetBagShopItemList();
        cfgList.Sort(ComppareItemCfg);

        mgrid_items.Bind<TABLE.SHOP, BagShopItem>(cfgList, mPoolHandleManager);
    }
    private int ComppareItemCfg(TABLE.SHOP cfgA, TABLE.SHOP cfgB)
    {
        if (int.Parse(cfgA.order) > int.Parse(cfgB.order)) return 1;
        else if (int.Parse(cfgA.order) < int.Parse(cfgB.order)) return -1;
        else if (cfgA.id > cfgB.id) return 1;
        else if (cfgA.id < cfgB.id) return -1;
        return 0;
    }
    void OnMoneyChange(uint id, object data)
    {
        RefreshShopList();
    }
    void OnPlayerLvChange(uint id, object data)
    {
        RefreshShopList();
    }
    protected override void OnDestroy()
    {
        mgrid_items.UnBind<BagShopItem>();
        mgrid_items = null;
        item_Scroll = null;
        base.OnDestroy();
    }
    public CSBetterList<TABLE.SHOP> GetBagShopItemList()
    {
        int roleLv = CSMainPlayerInfo.Instance.Level;
        CSBetterList<TABLE.SHOP> list = new CSBetterList<TABLE.SHOP>();
        var arr = ShopTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var value = arr[k].Value as TABLE.SHOP;
            TABLE.SHOP cfg = value;
            if (cfg.showLevel != 0 && cfg.showLevel > roleLv) continue;
            if (cfg.maxLevel != 0 && cfg.maxLevel < roleLv) continue;
            if (cfg.npcId == 2)
            {
                list.Add(cfg);
            }
        }
        return list;
    }
}
public class BagShopItem : UIBinder
{
    private UISprite sp_price_icon = null;
    private UILabel sp_item_price = null;
    private UILabel sp_item_name = null;
    private GameObject btn_buyItem = null;
    private UIItemBase item;
    private GameObject itemContent;
    private TABLE.SHOP cfg;
    private TABLE.ITEM itemCfg;
    private TABLE.ITEM costCfg;
    public override void Init(UIEventListener handle)
    {
        itemContent = Get<GameObject>("item");
        item = UIItemManager.Instance.GetItem(PropItemType.Normal, itemContent.transform);
        sp_item_price = Get<UILabel>("Price");
        sp_price_icon = Get<UISprite>("Price/Sprite");
        btn_buyItem = Get<GameObject>("buy");
        sp_item_name = Get<UILabel>("name");
    }
    public override void Bind(object data)
    {
        cfg = data as TABLE.SHOP;
        itemCfg = ItemTableManager.Instance.GetItemCfg((int)cfg.itemId);
        item.Refresh(itemCfg, ItemClick);
        sp_item_name.text = itemCfg.name;
        sp_item_name.color = UtilityCsColor.Instance.GetColor((int)itemCfg.quality);
        sp_item_price.text = cfg.value.ToString();

        costCfg = ItemTableManager.Instance.GetItemCfg((int)cfg.payType);
        long num = ((int)cfg.payType).GetItemCount();
        if (num < cfg.value)
            sp_item_price.color = CSColor.red;
        else
            sp_item_price.color = CSColor.beige;

        item.SetCount(cfg.num, CSColor.white);


        sp_price_icon.spriteName = $"tubiao{costCfg.icon}";

        UIEventListener.Get(btn_buyItem, cfg.id).onClick = OnClickBagShopItemBuy;
        UIEventListener.Get(sp_price_icon.gameObject).onClick = OnClickCostIcon;
    }
    private void OnClickBagShopItemBuy(GameObject go)
    {
        int indexId = (int)UIEventListener.Get(go).parameter;
        TABLE.SHOP cfg = GetShopItemById(indexId);
        if (cfg == null) return;
        //断格子是否满了

        //
        long num = ((int)cfg.payType).GetItemCount();
        if (num < cfg.value)
        {
            //货币不足  
            Utility.ShowGetWay((int)cfg.payType);
            return;
        }
        Net.CSShopBuyItemMessage(indexId, 1);

    }
    void OnClickCostIcon(GameObject go)
    {
        UITipsManager.Instance.CreateTips(TipsOpenType.Normal, costCfg.id);
    }
    private void ItemClick(UIItemBase item)
    {
        if (item != null)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, item.itemCfg, item.infos);
        }
    }
    public TABLE.SHOP GetShopItemById(int id)
    {
        TABLE.SHOP cfg = null;
        if (ShopTableManager.Instance.TryGetValue(id, out cfg))
        {
            return cfg;
        }
        else
        {
            return null;
        }
    }
    public override void OnDestroy()
    {
        UIItemManager.Instance.RecycleSingleItem(item);
        item = null;
        sp_price_icon = null;
        sp_item_price = null;
        sp_item_name = null;
        btn_buyItem = null;
        itemContent = null;
        cfg = null;
        costCfg = null;
        itemCfg = null;
    }
}