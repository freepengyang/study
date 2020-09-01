using Google.Protobuf.Collections;
using System.Collections.Generic;
using TABLE;
using UnityEngine;

public class UIItemBar : UIBinder, IRecycle
{
    protected UISprite bgFrame;
    protected UISprite itemIcon;
    protected UILabel itemCount;
    protected UISprite btnAddIcon;
    protected UILabel des1;
    protected UIEventListener btnAdd;
    protected UIEventListener btnIcon;


    const string KeyNameItemIcon = @"sp_icon";
    const string KeyNameItemCount = @"lb_value";
    const string KeyNameBtnAdd = @"btn_add";
    const string KeyNameBtnIcon = @"sp_icon";
    const string Des = @"lb_des";
    const string KeyNameOwnedAndNeedFmtString = @"{0}/{1}";
    const string KeyNameOwned = @"{0}";

    protected ItemBarData data;
    private bool HasComponentFetched = false;
    protected TABLE.ITEM item;

    public override void Init(UIEventListener handle)
    {
        if (!HasComponentFetched)
        {
            handle.onClick = OnClickBG;
            bgFrame = Handle.gameObject.GetComponent<UISprite>();
            itemIcon = Get<UISprite>(KeyNameItemIcon);
            itemCount = Get<UILabel>(KeyNameItemCount);
            btnAdd = Get<UIEventListener>(KeyNameBtnAdd);
            btnIcon = Get<UIEventListener>(KeyNameBtnIcon);
            btnAddIcon = Get<UISprite>(KeyNameBtnAdd);
            des1 = Get<UILabel>(Des);
            des1.CustomActive(false);
            HasComponentFetched = true;
        }
    }

    protected void OnClickBG(GameObject go)
    {
        if (null == this.data || !data.HasFlag(ItemBarData.ItemBarType.IBT_COMPARE))
        {
            return;
        }
        this.data.owned = this.data.cfgId.GetItemCount();
        if (this.data.owned < this.data.needed)
        {
            Utility.ShowGetWay(this.data.cfgId);
        }
    }

    public override void Bind(object data)
    {
        var now = data as ItemBarData;
        if (null == now)
        {
            FNDebug.LogError("[now is null]");
            return;
        }

        if (this.data != now)
        {
            UIItemBarManager.Instance.Put(this.data);
            this.data = now;
            item = null;
            ItemTableManager.Instance.TryGetValue(this.data.cfgId, out item);
        }

        HotManager.Instance.EventHandler.RemoveEvent(CEvent.ItemListChange, OnItemChanged);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.MoneyChange, OnItemChanged);
        HotManager.Instance.EventHandler.AddEvent(CEvent.ItemListChange, OnItemChanged);
        HotManager.Instance.EventHandler.AddEvent(CEvent.MoneyChange, OnItemChanged);
        Refresh();
    }

    public void OnRecycle()
    {
        //FNDebug.LogFormat("OnDestroy => {0}", this.GetHashCode());
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.ItemListChange, OnItemChanged);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.MoneyChange, OnItemChanged);

        if (null != data)
        {
            UIItemBarManager.Instance.Put(data);
            data.OnDestroy();
            this.data = null;
        }

        if (null != btnAdd)
        {
            btnAdd.onClick = null;
        }
        if (null != this.Handle)
        {
            this.Handle.onClick = OnClickBG;
        }
        if (null != this.btnAdd)
        {
            this.btnAdd.onDoubleClick = null;
            this.btnAdd.onClick = null;
        }
        if (null != btnIcon)
        {
            btnIcon.onClick = null;
        }
        if (null != this.btnIcon)
        {
            this.btnIcon.onDoubleClick = null;
            this.btnIcon.onClick = null;
        }
    }

    protected void OnItemChanged(uint uiEvtID, object data)
    {
        if (null == this.data)
        {
            FNDebug.LogError("this.data is null");
            return;
        }
        if (this.data.HasFlag(ItemBarData.ItemBarType.IBT_MONEYPANEL))
        {
            this.data.ResetOwned();
            Refresh();
        }
        else
        {
            if (data is MoneyType money)
            {
                this.data.owned = this.data.cfgId.GetItemCount();
                Refresh();
                //if ((int)money == this.data.cfgId)
                //{
                //    this.data.owned = CSBagInfo.Instance.GetMoneyCount(this.data.cfgId);
                //    Refresh();
                //}
            }
            else
            {
                this.data.owned = this.data.cfgId.GetItemCount();
                Refresh();
                //bag.BagItemInfo info = (bag.BagItemInfo)eventData.arg1;
                //ItemChangeType type = (ItemChangeType)eventData.arg2;
                //if (null != info && null != this.data && info.configId == this.data.cfgId)
                //{
                //    this.data.owned = this.data.cfgId.GetItemCount();
                //    Refresh();
                //}
            }

        }
    }

    protected void Refresh()
    {
        if (!HasComponentFetched)
        {
            return;
        }

        if (null == data)
        {
            FNDebug.LogError("[data is null]");
            return;
        }

        if (null != bgFrame)
        {
            if (null != this.data && this.data.bgWidth > 0)
            {
                bgFrame.width = this.data.bgWidth;
            }
            else
            {
                bgFrame.width = 136;
            }
        }

        if (data.HasFlag(ItemBarData.ItemBarType.IBT_ADD))
        {
            if (null != data && null != btnAdd)
            {
                btnAdd.onClick = f =>
                {
                    if (null != data && null != data.onAddAction)
                    {
                        data.onAddAction();
                    }
                    else
                    {
#if UNITY_EDITOR && ENABLE_DEBUG
                        data.GMCmdAddItem(100);
#else
                        Utility.ShowGetWay(data.cfgId);
#endif
                    }
                };

#if UNITY_EDITOR && ENABLE_DEBUG
                btnAdd.onDoubleClick = f => { data.GMCmdAddItem(100); };
#endif
            }
            if (null != btnAdd)
            {
                if (!btnAdd.gameObject.activeSelf)
                {
                    btnAdd.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            if (null != btnAdd)
            {
                btnAdd.onClick = null;
                if (btnAdd.gameObject.activeSelf)
                {
                    btnAdd.gameObject.SetActive(false);
                }
            }
        }
        if (data.HasFlag(ItemBarData.ItemBarType.IBT_MONEYPANEL))
        {
            if (btnIcon != null)
            {
                btnIcon.onClick = f =>
                {
                    Vector3 v3 = Vector3.zero;
                    if (itemIcon != null)
                    {
                        v3 = new Vector3(btnIcon.transform.position.x - 0.08f, btnIcon.transform.position.y - 0.025f);
                    }

                    UIManager.Instance.CreatePanel<UIMoneyTipsPanel>(p =>
                    {
                        (p as UIMoneyTipsPanel).SetId(data.cfgId, v3);
                    });
                };
            }
            if (data.desId != 0)
            {
                des1.gameObject.SetActive(true);
                des1.text = ClientTipsTableManager.Instance.GetClientTipsContext(data.desId);
            }
            else
            {
                des1.gameObject.SetActive(false);
            }
            if (null != data && null != btnAdd)
            {
                btnAdd.gameObject.SetActive(true);
                btnAdd.onClick = f =>
                {
                    if (null != data && null != data.onAddAction)
                    {
                        data.onAddAction();
                    }
                    else
                    {
                        //List<List<int>> content = UtilityMainMath.SplitStringToIntLists(MoneyTypeTableManager.Instance.GetMoneyTypeContent(data.cfgId));
                        //Utility.ShowGetWay(content[0][0]);
                        Utility.ShowGetWay(MoneyTypeTableManager.Instance.GetMoneyTypeItemid(data.cfgId));
                    }
                };
            }
        }
        else
        {
            if (btnIcon != null)
            {
                btnIcon.onClick = f =>
                {
                    UITipsManager.Instance.CreateTips(TipsOpenType.Normal, data.cfgId);
                };
            }
        }

        bgFrame.depth = data.bgDepth;
        itemIcon.depth = data.bgDepth + 1;
        itemCount.depth = data.bgDepth + 1;
        btnAddIcon.depth = data.bgDepth + 1;

        if (null != data)
        {
            if (data.HasFlag(ItemBarData.ItemBarType.IBT_MONEYPANEL))
            {
                itemIcon.spriteName = $"tubiao{ItemTableManager.Instance.GetItemIcon(MoneyTypeTableManager.Instance.GetMoneyTypeItemid(data.cfgId))}";
            }
            else
            {
                var itemCfg = ItemTableManager.Instance.GetItemCfg(data.cfgId);
                if (null != itemCfg && null != itemIcon)
                {
                    if (data.HasFlag(ItemBarData.ItemBarType.IBT_SMALL_ICON))
                    {
                        itemIcon.spriteName = $"tubiao{itemCfg.icon}";
                    }
                    else
                    {
                        itemIcon.spriteName = itemCfg.icon;
                    }
                }
            }

            string needExpress = string.Empty;

            if (null != itemCount)
            {
                if (data.HasFlag(ItemBarData.ItemBarType.IBT_COMPARE))
                {
                    if (data.HasFlag(ItemBarData.ItemBarType.IBT_ONLY_COST))
                    {
                        if (data.HasFlag(ItemBarData.ItemBarType.IBT_RED_GREEN))
                        {
                            itemCount.text = $"{(data.owned < data.needed ? UtilityColor.Red : UtilityColor.Green)}{Express(data.needed)}";
                        }
                        else
                        {
                            itemCount.text = $"{Express(data.needed)}".BBCode(ColorType.MainText);
                        }
                    }
                    else if (data.HasFlag(ItemBarData.ItemBarType.IBT_ONLY_OWNED))
                    {
                        if (data.HasFlag(ItemBarData.ItemBarType.IBT_RED_GREEN))
                        {
                            itemCount.text = $"{(data.owned < data.needed ? UtilityColor.Red : UtilityColor.Green)}{Express(data.owned)}";
                        }
                        else
                        {
                            itemCount.text = $"{(data.owned < data.needed ? UtilityColor.Red : UtilityColor.MainText)}{Express(data.owned)}";
                        }
                    }
                    else
                    {
                        if (data.HasFlag(ItemBarData.ItemBarType.IBT_RED_GREEN))
                        {
                            itemCount.text = $"{(data.owned < data.needed ? UtilityColor.Red : UtilityColor.Green)}{Express(data.owned)}/{Express(data.needed)}";
                        }
                        else
                        {
                            itemCount.text = $"{(data.owned < data.needed ? UtilityColor.Red : UtilityColor.MainText)}{Express(data.owned)}/{Express(data.needed)}";
                        }
                    }
                }
                else
                {
                    if (data.HasFlag(ItemBarData.ItemBarType.IBT_MONEYPANEL))
                    {
                        itemCount.text = $"{UtilityColor.MainText}{data.owned}";
                    }
                    else
                    {
                        if (data.HasFlag(ItemBarData.ItemBarType.IBT_ONLY_COST))
                        {
                            itemCount.text = $"{UtilityColor.MainText}{Express(data.needed)}";
                        }
                        else
                        {
                            itemCount.text = $"{UtilityColor.MainText}{Express(data.owned)}";
                        }
                    }

                }
            }
        }
    }

    public string Express(long value)
    {
        if (data.HasFlag(ItemBarData.ItemBarType.IBT_SHORT_EXPRESS))
        {
            return UtilityMath.GetDecimalValue(value);
        }
        else if (data.HasFlag(ItemBarData.ItemBarType.IBT_SHORT_EXPRESS_WITH_ONE_POINT))
        {
            return UtilityMath.GetDecimalValue(value, "F1");
        }
        else if (data.HasFlag(ItemBarData.ItemBarType.IBT_SHORT_EXPRESS_WITH_TWO_POINT))
        {
            return UtilityMath.GetDecimalValue(value, "F2");
        }
        else
        {
            return value.ToString();
        }
    }

    public override void OnDestroy()
    {
        this.OnRecycle();
    }
}

public static class ItemHelper
{
    public static string QualityIcon(this int _qua)
    {
        return $"quality{_qua}";
    }

    public static string QualityName(this int cfgId)
    {
        TABLE.ITEM item = null;
        if (!ItemTableManager.Instance.TryGetValue(cfgId, out item))
            return string.Empty;
        return item.QualityName();
    }

    public static string QualityIcon(this bag.BagItemInfo itemInfo)
    {
        TABLE.ITEM itemCfg = null;
        if (null != itemInfo && ItemTableManager.Instance.TryGetValue(itemInfo.configId, out itemCfg))
        {
            int quality = CSBagInfo.Instance.IsNormalEquip(itemCfg) ? itemInfo.quality : itemCfg.quality;
            return $"quality{quality}";
        }
        return string.Empty;
    }

    public static string QualityName(this bag.BagItemInfo itemInfo)
    {
        TABLE.ITEM itemCfg = null;
        if (null != itemInfo && ItemTableManager.Instance.TryGetValue(itemInfo.configId, out itemCfg))
        {
            int quality = CSBagInfo.Instance.IsNormalEquip(itemCfg) ? itemInfo.quality : itemCfg.quality;
            return itemCfg.name.BBCode(quality);
        }
        return string.Empty;
    }

    public static string ItemName(this int cfgId)
    {
        TABLE.ITEM item = null;
        if (!ItemTableManager.Instance.TryGetValue(cfgId, out item))
            return string.Empty;
        return item.name;
    }

    public static string QualityName(this TABLE.ITEM item)
    {
        var ret = string.Empty;
        if (null != item)
        {
            ret = item.name.BBCode(item.quality);
        }

        return ret;
    }

    public static string Icon(this int cfgId)
    {
        TABLE.ITEM item = null;
        if (!ItemTableManager.Instance.TryGetValue(cfgId, out item))
            return string.Empty;
        return item.icon;
    }

    public static string SmallIcon(this TABLE.ITEM item)
    {
        if (null != item)
            return $"tubiao{item.icon}";
        return string.Empty;
    }

    public static string SmallIcon(this int cfgId)
    {
        TABLE.ITEM item = null;
        if (!ItemTableManager.Instance.TryGetValue(cfgId, out item))
            return string.Empty;
        return $"tubiao{item.icon}";
    }

    public static long GetItemCount(this int cfgId)
    {
        return CSItemCountManager.Instance.GetItemCount(cfgId);
    }

    public static long GetItemCountByMoneyType(this int _moneyType)
    {
        long count = 0;
        List<List<int>> content = UtilityMainMath.SplitStringToIntLists(MoneyTypeTableManager.Instance.GetMoneyTypeContent(_moneyType));
        for (int i = 0; i < content.Count; i++)
        {
            for (int j = 0; j < content[i].Count; j++)
            {
                count = count + CSBagInfo.Instance.GetMoneyCount(content[i][j]);
            }
        }
        return count;
    }

    public static bool IsMainPlayerLevelEnough(this int level, bool callTips = false)
    {
        int playerLv = 0;
        playerLv = CSMainPlayerInfo.Instance.Level;
        if (playerLv < level)
        {
            if (callTips)
            {
                UtilityTips.ShowRedTips(CSString.Format(542, level));
            }

            return false;
        }

        return true;
    }

    public static System.Collections.Generic.List<CostItemData> GetCostItems(this LongArray kvs, PoolHandleManager poolHandle)
    {
        var datas = poolHandle.GetSystemClass<System.Collections.Generic.List<CostItemData>>();
        datas.Clear();
        //if (null != kvs)
        for (int i = 0; i < kvs.Count; ++i)
        {
            var kv = kvs[i];
            TABLE.ITEM item = null;
            if (!ItemTableManager.Instance.TryGetValue(kv.key(), out item) || null == item || kv.value() <= 0)
            {
                continue;
            }

            var costItem = poolHandle.GetSystemClass<CostItemData>();
            costItem.cfgId = kv.key();
            costItem.count = kv.value();
            costItem.item = item;
            datas.Add(costItem);
        }
        return datas;
    }

    public static System.Collections.Generic.List<CostItemData> GetCostItems(this RepeatedField<TABLE.KEYVALUE> kvs, PoolHandleManager poolHandle)
    {
        var datas = poolHandle.GetSystemClass<System.Collections.Generic.List<CostItemData>>();
        datas.Clear();
        if (null != kvs)
            for (int i = 0; i < kvs.Count; ++i)
            {
                var kv = kvs[i];
                TABLE.ITEM item = null;
                if (!ItemTableManager.Instance.TryGetValue(kv.key, out item) || null == item || kv.value <= 0)
                {
                    continue;
                }

                var costItem = poolHandle.GetSystemClass<CostItemData>();
                costItem.cfgId = kv.key;
                costItem.count = kv.value;
                costItem.item = item;
                datas.Add(costItem);
            }
        return datas;
    }

    public static bool IsItemEnough(this int cfgId, int count, int tipId = 0, bool showGetWay = true)
    {
        TABLE.ITEM item = null;
        if (!ItemTableManager.Instance.TryGetValue(cfgId, out item))
        {
            return false;
        }

        long owned = cfgId.GetItemCount();
        if (count > owned)
        {
            if (showGetWay)
            {
                Utility.ShowGetWay(cfgId);
                return false;
            }

            if (tipId != 0)
            {
                UtilityTips.ShowRedTips(CSString.Format(tipId, item.name.BBCode(ColorType.MainText)));
            }

            return false;
        }

        return true;
    }

    public static bool IsItemsEnough(this RepeatedField<TABLE.KEYVALUE> kvs, int tipId = 0, bool showGetWay = false)
    {
        if (null != kvs)
            for (int i = 0; i < kvs.Count; ++i)
            {
                var kv = kvs[i];
                TABLE.ITEM item = null;
                if (!ItemTableManager.Instance.TryGetValue(kv.key, out item))
                {
                    continue;
                }

                long owned = kv.key.GetItemCount();
                if (kv.value > owned)
                {
                    if (showGetWay)
                    {
                        Utility.ShowGetWay(kv.key);
                        return false;
                    }

                    if (tipId != 0)
                    {
                        UtilityTips.ShowRedTips(CSString.Format(tipId, item.name.BBCode(ColorType.MainText)));
                    }

                    return false;
                }
            }

        return true;
    }

    public static bool IsItemsEnough(this LongArray kvs, int tipId = 0, bool showGetWay = false)
    {
        for (int i = 0; i < kvs.Count; ++i)
        {
            var kv = kvs[i];
            TABLE.ITEM item = null;
            if (!ItemTableManager.Instance.TryGetValue(kv.key(), out item))
            {
                continue;
            }

            long owned = kv.key().GetItemCount();
            if (kv.value() > owned)
            {
                if (showGetWay)
                {
                    Utility.ShowGetWay(kv.key());
                    return false;
                }

                if (tipId != 0)
                {
                    UtilityTips.ShowRedTips(CSString.Format(tipId, item.name.BBCode(ColorType.MainText)));
                }

                return false;
            }
        }

        return true;
    }

    public static bool IsItemsEnough(IntArray ids, IntArray counts, int tipId = 0, bool showGetWay = false)
    {
        if (ids.Count == counts.Count)
            for (int i = 0; i < ids.Count; ++i)
            {
                var key = ids[i];
                TABLE.ITEM item = null;
                if (!ItemTableManager.Instance.TryGetValue(key, out item))
                {
                    continue;
                }

                var needed = counts[i];

                long owned = key.GetItemCount();
                if (needed > owned)
                {
                    if (showGetWay)
                    {
                        Utility.ShowGetWay(ids[i]);
                        return false;
                    }

                    if (tipId != 0)
                    {
                        UtilityTips.ShowRedTips(CSString.Format(tipId, item.name.BBCode(ColorType.MainText)));
                    }

                    return false;
                }
            }

        return true;
    }

    public static bool IsItemsEnough(RepeatedField<int> ids, RepeatedField<int> counts, int tipId = 0, bool showGetWay = false)
    {
        if (ids.Count == counts.Count)
            for (int i = 0; i < ids.Count; ++i)
            {
                var key = ids[i];
                TABLE.ITEM item = null;
                if (!ItemTableManager.Instance.TryGetValue(key, out item))
                {
                    continue;
                }

                var needed = counts[i];

                long owned = key.GetItemCount();
                if (needed > owned)
                {
                    if (showGetWay)
                    {
                        Utility.ShowGetWay(ids[i]);
                        return false;
                    }

                    if (tipId != 0)
                    {
                        UtilityTips.ShowRedTips(CSString.Format(tipId, item.name.BBCode(ColorType.MainText)));
                    }

                    return false;
                }
            }

        return true;
    }

    public static string ToCurrency(this string num)
    {
        for (int i = num.Length - 3; i > 0; i -= 3)
        {
            num = num.Insert(i, ",");
        }
        return num;
    }

    public static string ToCurrency(this int _num)
    {
        return _num.ToString().ToCurrency();
    }

    public static System.Collections.Generic.List<ItemBarData> GetItemBarDatas(this LongArray kvs,
        PoolHandleManager poolHandle, EventHanlderManager eventHandle)
    {
        System.Collections.Generic.List<ItemBarData> itemBarDatas =
            poolHandle.GetSystemClass<System.Collections.Generic.List<ItemBarData>>();
        itemBarDatas.Clear();
        //if (null == kvs)
        //    return itemBarDatas;
        for (int i = 0; i < kvs.Count; ++i)
        {
            var kv = kvs[i];

            TABLE.ITEM item = null;
            if (!ItemTableManager.Instance.TryGetValue(kv.key(), out item))
            {
                continue;
            }

            var itemData = UIItemBarManager.Instance.Get();
            itemData.bgWidth = 0;
            itemData.cfgId = kv.key();
            itemData.needed = kv.value();
            itemData.owned = itemData.cfgId.GetItemCount();
            itemData.flag = (int)ItemBarData.ItemBarType.IBT_GENERAL_COMPARE_SMALL;
            itemData.eventHandle = eventHandle;
            itemBarDatas.Add(itemData);
        }

        return itemBarDatas;
    }

    public static void OnRecycle(this RepeatedField<KEYVALUE> kvs, PoolHandleManager poolHandleManager)
    {
        if (null != kvs)
        {
            for (int k = 0; k < kvs.Count; ++k)
            {
                poolHandleManager.Recycle(kvs[k]);
            }
            kvs.Clear();
            poolHandleManager.Recycle(kvs);
        }
    }

    public static bool IsItemEnough(this PoolHandleManager poolHandleManager, string value, int tipId = 0, bool showGetWay = false)
    {
        var requiredItems = poolHandleManager.Split(value);

        for (int i = 0; i < requiredItems.Count; ++i)
        {
            TABLE.ITEM item = null;
            if (!ItemTableManager.Instance.TryGetValue(requiredItems[i].key, out item))
            {
                continue;
            }

            long needed = requiredItems[i].value;
            long owned = requiredItems[i].key.GetItemCount();
            if (needed > owned)
            {
                if (showGetWay)
                {
                    Utility.ShowGetWay(requiredItems[i].key);

                    requiredItems.OnRecycle(poolHandleManager);
                    return false;
                }

                if (tipId != 0)
                {
                    UtilityTips.ShowRedTips(CSString.Format(tipId, item.name.BBCode(ColorType.MainText)));
                }

                requiredItems.OnRecycle(poolHandleManager);
                return false;
            }
        }

        requiredItems.OnRecycle(poolHandleManager);
        return true;
    }

    public static RepeatedField<TABLE.KEYVALUE> Split(this PoolHandleManager poolHandleManager, string value)
    {
        var kvs = poolHandleManager.GetSystemClass<RepeatedField<TABLE.KEYVALUE>>();
        kvs.Clear();
        var tokens = value.Split('&');
        long key = 0L;
        int val = 0;
        for (int i = 0; i < tokens.Length; ++i)
        {
            var kv = tokens[i].Split('#');
            if (kv.Length != 2)
                continue;

            if (!long.TryParse(kv[0], out key) || !int.TryParse(kv[1], out val))
                continue;

            var keyValue = poolHandleManager.GetSystemClass<TABLE.KEYVALUE>();
            keyValue.key = (int)key;
            keyValue.value = val;

            kvs.Add(keyValue);
        }
        return kvs;
    }

    public static FastArrayElementKeepHandle<ItemBarData> Split(this UIItemBarManager itemBarManager, FastArrayElementKeepHandle<ItemBarData> itemDatas, string value, int flag = (int)ItemBarData.ItemBarType.IBT_SMALL_ICON | (int)ItemBarData.ItemBarType.IBT_ONLY_COST)
    {
        itemDatas.Clear();
        var tokens = value.Split('&');
        int key = 0;
        int val = 0;
        for (int i = 0; i < tokens.Length; ++i)
        {
            var kv = tokens[i].Split('#');
            if (kv.Length != 2)
                continue;

            if (!int.TryParse(kv[0], out key) || !int.TryParse(kv[1], out val))
                continue;

            var itemData = itemBarManager.Get();
            itemData.cfgId = key;
            itemData.needed = val;
            itemData.owned = CSItemCountManager.Instance.GetItemCount(key);
            itemData.flag = flag;

            itemDatas.Append(itemData);
        }
        return itemDatas;
    }

    public static RepeatedField<TABLE.KEYVALUE> GetMailItems(this PoolHandleManager poolHandleManager, int mailId)
    {
        TABLE.MAIL mailItem;
        if (!MailTableManager.Instance.TryGetValue(mailId, out mailItem))
        {
            return poolHandleManager.GetSystemClass<RepeatedField<TABLE.KEYVALUE>>();
        }
        return GetEffectItems(poolHandleManager, poolHandleManager.Split(mailItem.items));
    }

    public static RepeatedField<TABLE.KEYVALUE> GetEffectItems(this PoolHandleManager poolHandleManager, RepeatedField<TABLE.KEYVALUE> kvs)
    {
        for (int i = 0; i < kvs.Count; ++i)
        {
            TABLE.ITEM item;
            if (!ItemTableManager.Instance.TryGetValue(kvs[i].key, out item))
            {
                poolHandleManager.Recycle(kvs[i]);
                kvs.RemoveAt(i--);
                continue;
            }
        }
        return kvs;
    }
}