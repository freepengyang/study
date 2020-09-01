using System;
using System.Collections;
using System.Collections.Generic;
public partial class ItemTableManager : TableManager<TABLE.ITEMARRAY, TABLE.ITEM, int, ItemTableManager>
{
    //protected override void OnResourceLoaded(CSResourceWWW res)
    //{
    //    base.OnResourceLoaded(res);

    //    if (array != null)
    //    {
    //        for (int i = 0; i < array.Rows.Length; i++)
    //        {
    //            AddTables(array.Rows[i].id, array.Rows[i] as TABLE.ITEM);
    //        }
    //    }
    //    base.OnDealOver();
    //}



    string[] quaArr = { "quality1", "quality1", "quality2", "quality3", "quality4", "quality5" };

    public string GetItemName(int itemId)
    {
        TABLE.ITEM itemTb;
        if (TryGetValue(itemId, out itemTb))
        {
            if (itemTb != null)
            {
                return itemTb.name;
            }
        }
        return "";
    }

    public TABLE.ITEM GetItemCfg(int _cfgId)
    {
        TABLE.ITEM itemTb;
        if (TryGetValue(_cfgId, out itemTb))
        {
            return itemTb;
        }
        return null;
    }
    public string GetItemQualityBG(int _qua)
    {
        if (_qua < quaArr.Length)
        {
            return quaArr[_qua];
        }
        return "";
    }
    public int GetItemType(long itemId)
    {
        TABLE.ITEM itemTb;
        if (TryGetValue((int)itemId, out itemTb))
        {
            return (int)itemTb.type;
        }
        return 0;
    }
    public int GetItemSubType(long itemId)
    {
        TABLE.ITEM itemTb;
        if (TryGetValue((int)itemId, out itemTb))
        {
            return (int)itemTb.subType;
        }
        return 0;
    }

    Dictionary<int, int> cdDic;
    public float GetItemCDTimeByGroupId(int _groupId)
    {
        if (cdDic == null)
        {
            cdDic = new Dictionary<int, int>();
            var arr = array.gItem.handles;
            for (int i = 0, max = arr.Length; i < max; ++i)
            {
                var item = arr[i].Value as TABLE.ITEM;
                if (item.group != 0)
                {
                    if (!cdDic.ContainsKey(item.group))
                    {
                        cdDic.Add(item.group, item.itemcd);
                    }
                }
            }
        }
        if (cdDic.ContainsKey(_groupId))
        {
            return cdDic[_groupId];
        }
        return 0;
    }

}
