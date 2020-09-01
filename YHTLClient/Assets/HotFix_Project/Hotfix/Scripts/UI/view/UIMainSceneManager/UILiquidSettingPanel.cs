using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UILiquidSettingPanel : UIBasePanel
{
    public override UILayerType PanelLayerType { get { return UILayerType.Tips; } }
    ILBetterList<ShortPotionGroupItem> potionGroupItems = new ILBetterList<ShortPotionGroupItem>(3);
    bool AutoBuyState = false;
    string AutoBuyLiquidSetKey = "";
    int AutoBuyVipLimilt = 4;
    public override void Init()
    {
        base.Init();
        AddCollider();
        mClientEvent.AddEvent(CEvent.ItemListChange, GetItemListChange);
        UIEventListener.Get(mbtn_close).onClick = CloseBtnClick;
        UIEventListener.Get(mbtn_buy).onClick = ConfirmClick;
        UIEventListener.Get(mbtn_autobuy.gameObject).onClick = AutoBuyBtnClick;
        potionGroupItems.Add(new ShortPotionGroupItem(mgrid1, 1048, 1, mshowitem));
        potionGroupItems.Add(new ShortPotionGroupItem(mgrid2, 1049, 2, mshowitem));
        potionGroupItems.Add(new ShortPotionGroupItem(mgrid3, 1050, 3, mshowitem));
        AutoBuyLiquidSetKey = $"{CSMainPlayerInfo.Instance.ID}AutoBuyLiquidSetKey";
        int.TryParse(SundryTableManager.Instance.GetSundryEffect(1138), out AutoBuyVipLimilt);
        // 0 false 1 true
        int value = PlayerPrefs.GetInt(AutoBuyLiquidSetKey, 0);
        AutoBuyState = (value == 1) ? true : false;
        mbtn_autobuy.spriteName = (AutoBuyState == true) ? "check_nor2" : "";
    }

    public override void Show()
    {
        base.Show();
        for (int i = 0; i < potionGroupItems.Count; i++)
        {
            potionGroupItems[i].Refresh();
        }
    }

    protected override void OnDestroy()
    {
        for (int i = 0; i < potionGroupItems.Count; i++)
        {
            potionGroupItems[i].Recycle();
        }
        base.OnDestroy();
    }
    void GetItemListChange(uint id, object data)
    {
        for (int i = 0; i < potionGroupItems.Count; i++)
        {
            potionGroupItems[i].RefreshCount();
        }
    }
    void CloseBtnClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UILiquidSettingPanel>();
    }
    void ConfirmClick(GameObject _go)
    {
        for (int i = 0; i < potionGroupItems.Count; i++)
        {
            potionGroupItems[i].StockpileValue();
            if (AutoBuyState)
            {
                potionGroupItems[i].AutoBuy();
            }
        }
        mClientEvent.SendEvent(CEvent.FastUseClose);
        UIManager.Instance.ClosePanel<UILiquidSettingPanel>();
    }
    void AutoBuyBtnClick(GameObject _go)
    {
        if (CSMainPlayerInfo.Instance.VipLevel < AutoBuyVipLimilt)
        {
            UtilityTips.ShowPromptWordTips(101, () => { UtilityPanel.JumpToPanel(19000); UIManager.Instance.ClosePanel<UILiquidSettingPanel>(); });
        }
        else
        {
            AutoBuyState = !AutoBuyState;
            mbtn_autobuy.spriteName = (AutoBuyState == true) ? "check_nor2" : "";
            PlayerPrefs.SetInt(AutoBuyLiquidSetKey, (AutoBuyState == true) ? 1 : 0);
        }
    }



    class ShortPotionGroupItem
    {
        public GameObject go;
        UIGrid grid;
        List<List<int>> potion;
        ILBetterList<ShortPotionItem> potionItems = new ILBetterList<ShortPotionItem>(5);
        ShortPotionItem curItem;
        int index = 0;
        int ChooseIndex = 0;
        GameObject temp;
        string liquidKey = "";
        int liquidValue = 0;
        int mlvSuggestIndex = 0;
        TABLE.SHOP ShopCfg;
        public ShortPotionGroupItem(UIGrid _go, int _ids, int _index, GameObject _temp)
        {
            liquidKey = $"{CSMainPlayerInfo.Instance.Name}quickUseId{_index}";
            liquidValue = PlayerPrefs.GetInt(liquidKey, 0);
            go = _go.gameObject;
            grid = _go;
            index = _index;
            temp = _temp;
            potion = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(_ids));
            mlvSuggestIndex = GetSuggestPotionIndex();
        }
        public void Refresh()
        {
            for (int i = 0; i < potion.Count; i++)
            {
                GameObject go = GameObject.Instantiate(temp, grid.transform);
                potionItems.Add(new ShortPotionItem(go));
                potionItems[i].Refresh(index, i, potion[i][1], (mlvSuggestIndex == i) ? true : false, ShowItemClick);
            }
            grid.Reposition();

            curItem = potionItems[liquidValue];
            curItem.ChangeSelect(true);
        }
        public void StockpileValue()
        {
            PlayerPrefs.SetInt(liquidKey, (curItem == null) ? liquidValue : curItem.index);
        }
        public void AutoBuy()
        {
            if (curItem.cfgid.GetItemCount() <= 0)
            {
                TABLE.SHOP shopCfg = null;
                var arr = ShopTableManager.Instance.array.gItem.handles;
                for (int k = 0, max = arr.Length; k < max; ++k)
                {
                    if ((arr[k].Value as TABLE.SHOP).itemId == curItem.cfgid)
                    {
                        shopCfg = arr[k].Value as TABLE.SHOP;
                        break;
                    }
                }
                
                Net.CSShopBuyItemMessage(shopCfg.id, 1);
            }
        }
        public void RefreshCount()
        {
            for (int i = 0; i < potion.Count; i++)
            {
                potionItems[i].RefreshCount();
            }
        }
        public void Recycle()
        {
            for (int i = 0; i < potionItems.Count; i++)
            {
                potionItems[i].Recycle();
            }
        }
        void ShowItemClick(ShortPotionItem _item)
        {
            //Debug.Log($"{_item.item.itemCfg.name}   {_item.type}   {_item.index}");
            if (curItem != null)
            {
                curItem.ChangeSelect(false);
            }
            curItem = _item;
            curItem.ChangeSelect(true);
            curItem.ShowGetWay();
        }
        int GetSuggestPotionIndex()
        {
            int mLv = CSMainPlayerInfo.Instance.Level;
            if (mLv >= potion[potion.Count - 1][0])
            {
                return potion.Count - 1;
            }
            for (int i = 0; i < potion.Count; i++)
            {
                if (i <= potion.Count - 2)
                {
                    if (potion[i][0] <= mLv && mLv < potion[i + 1][0])
                    {
                        return i;
                    }
                }
            }
            return 0;
        }
    }




    class ShortPotionItem
    {
        public GameObject go;
        public Transform trans;
        public GameObject seal;
        public UIItemBase item;
        Action<ShortPotionItem> action;
        long count = 0;
        public int type = 0;
        public int index = 0;
        public int cfgid = 0;

        public ShortPotionItem(GameObject _go)
        {
            go = _go;
            trans = go.transform;
            seal = trans.Find("seal").gameObject;
            item = UIItemManager.Instance.GetItem(PropItemType.Normal, trans, itemSize.Size64);
            go.SetActive(true);
            UIEventListener.Get(go).onClick = Click;
            UIEventListener.Get(go).onKeepPress = Press;
        }
        public void Refresh(int _type, int _index, int _cfgID, bool _choose, Action<ShortPotionItem> _action)
        {
            type = _type;
            index = _index;
            cfgid = _cfgID;
            action = _action;
            item.HasCD = true;
            item.Refresh(cfgid, null, false);
            count = cfgid.GetItemCount();
            if (count > 0)
            {
                item.SetCount(count, CSColor.green);
            }
            else
            {
                item.SetCount(count, CSColor.red);
            }
            seal.SetActive(_choose);
        }
        public void RefreshCount()
        {
            count = cfgid.GetItemCount();
            if (count > 0)
            {
                item.SetCount(count, CSColor.green);
            }
            else
            {
                item.SetCount(count, CSColor.red);
            }
        }
        void Click(GameObject _go)
        {
            if (action != null)
            {
                action(this);
            }
        }
        void Press(GameObject _go)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, item.itemCfg);
        }
        public void ChangeSelect(bool _state)
        {
            if (item != null)
            {
                item.ShowSelect(_state);
            }
        }
        public void ShowGetWay()
        {
            if (count <= 0)
            {
                Utility.ShowGetWay(cfgid);
            }
        }
        public void Recycle()
        {
            UIItemManager.Instance.RecycleSingleItem(item);
        }
    }
}
