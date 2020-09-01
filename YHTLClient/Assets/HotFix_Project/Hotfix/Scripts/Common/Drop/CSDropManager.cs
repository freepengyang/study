using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSDropManager : Singleton<CSDropManager>
{
    public float mCreateItemInternal = 0.01f;
    private Dictionary<long, CSItem> mItemsDic = new Dictionary<long, CSItem>();
    public ILBetterList<CSItem> itemList = new ILBetterList<CSItem>(64);
    public void AddItem(CSItem item)
    {
        if (!mItemsDic.ContainsKey(item.ItemId))
        {
            mItemsDic.Add(item.ItemId, item);
            itemList.Add(item);
        }
    }

    public bool RemoveItem(long id)
    {
        CSItem item = GetItem(id);
        if (item != null)
        {
            item.Release();
            mItemsDic.Remove(id);
            itemList.Remove(item);
            return true;
        }
        return false;
    }

    public bool RemoveItem(CSItem item)
    {
        if (item != null)
        {
            item.Release();
            mItemsDic.Remove(item.ItemId);
            itemList.Remove(item);
            //Debug.LogErrorFormat("RemoveItem: {0}  {1}",item.ItemId,(item.itemTbl != null ? item.itemTbl.name : " item table is null"));
            return true;
        }
        return false;
    }

    private void DestroyItem()
    {

    }

    public void UpdateItem(CSItem item)
    {

    }

    public CSItem GetItem(long id)
    {
        if (mItemsDic.ContainsKey(id))
        {
            return mItemsDic[id];
        }
        return null;
    }

    public CSItem GetItem(CSMisc.Dot2 coord)
    {
        CSCell cell = CSMesh.Instance.getCell(coord.x, coord.y);
        if (cell != null)
        { 
            var itemIds = cell.node.AvatarIDDic.GetEnumerator();
            while(itemIds.MoveNext())
            {
                long key = itemIds.Current.Key;
                if (mItemsDic.ContainsKey(itemIds.Current.Key))
                {
                    return mItemsDic[key];
                }
            }
        }
        return null;
    }

    public void Create(Transform parent, map.RoundItem info)
    {
        if(info == null || parent == null)
        {
            return;
        }
        RemoveItem(info.itemId);
        CSItem item = AddPoolItem(); ;
        if (item == null)
        {
            item = new CSItem();
        }
        item.Init(info, parent);
        AddItem(item);
        AddWaitDeal(parent.gameObject, item,OnCreateItem, mCreateItemInternal);
    }

    /// <summary>
    /// 创建Avatar
    /// </summary>
    /// <param name="obj">avatar</param>
    /// <param name="param">回调参数</param>
    /// <returns></returns>
    public bool OnCreateItem(object obj, object param)
    {
        CSItem item = obj as CSItem;
        if (item == null || item.BaseInfo == null)
        {
            return false;
        }
        if (mItemsDic.ContainsKey(item.BaseInfo.itemId))
        {
            item.Refresh();
            CSDropSystem.Instance.DetectPickItem(item);
        }
        else
        {
            item.Release();
        }
        return true;
    }

    /// <summary>
    /// 添加等待加载的Item
    /// </summary>
    /// <param name="goAnchor">Item 对应的父节点</param>
    /// <param name="avater"></param>
    /// <param name="onload">加载完成回调</param>
    /// <param name="waitFrame">等待加载的时间</param>
    /// <param name="param">加载完成回调预留参数</param>
    /// <param name="isInsert">true:往等待加载队列前插</param>
    public void AddWaitDeal(GameObject goAnchor, CSItem item, Func<object, object, bool> onload, float waitFrame = 0, object param = null, bool isInsert = false)
    {
        if (!CSScene.IsLanuchMainPlayer) return;
        if (goAnchor == null) return;
        if (item != null && item.BaseInfo == null) return;

        bool isWaitCreate = true;
        if (!isWaitCreate) waitFrame = 0.01f;
        long tempId = (item != null) ? item.BaseInfo.itemId : 0; ;

        if (!CSWaitFrameDealManager.Instance.AnchorToWaitDealDic.ContainsKey(goAnchor) || CSWaitFrameDealManager.Instance.AnchorToWaitDealDic[goAnchor] == null)
        {
            CSWaitFrameDeal deal = goAnchor.AddComponent<CSWaitFrameDeal>();
            deal.isNeedReset = true;
            CSWaitFrameDealManager.Instance.AnchorToWaitDealDic.Add(goAnchor, deal);
            tempId = (item != null) ? item.BaseInfo.itemId : 0;
            if (isInsert)
            {
                deal.InsertFront(tempId);
            }
            else
            {
                deal.Add(item, onload, waitFrame, param, tempId);
            }
        }
        else
        {
            if (isInsert)
            {
                CSWaitFrameDealManager.Instance.AnchorToWaitDealDic[goAnchor].InsertFront(tempId);
            }
            else
            {
                CSWaitFrameDealManager.Instance.AnchorToWaitDealDic[goAnchor].Add(item, onload, waitFrame, param, tempId);
            }
        }
    }

    private CSItem AddPoolItem()
    {
        if (CSObjectPoolMgr.Instance == null)
        {
            return null;
        }
        CSObjectPoolItem poolItem = Utility.GetAndAddPoolItem_Class("CSItem", "CSItem", null, typeof(CSItem), null);
        CSItem item = poolItem.objParam as CSItem;
        if(item != null)
        {
            item.PoolItem = poolItem;
        }
        return item;
    }

    public void Destroy()
    {
        var dic = mItemsDic.GetEnumerator();
        while(dic.MoveNext())
        {
            dic.Current.Value?.Destroy();
        }
        mItemsDic.Clear();

        for(int i = 0; i < itemList.Count; ++i)
        {
            CSItem item = itemList[i];
            if(item != null)
            {
                item.Destroy();
            }
            item = null;
        }
        itemList.Clear();
    }
}
