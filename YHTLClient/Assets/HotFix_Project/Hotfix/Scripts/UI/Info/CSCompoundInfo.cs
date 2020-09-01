using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using UnityEngine;

public class CSCompoundInfo : CSInfo<CSCompoundInfo>
{
    PoolHandleManager mPoolHandleManager = new PoolHandleManager();

    public CSCompoundInfo()
    {
        mCachedCompoundGroupDatas = new FastArrayElementFromPool<CompoundGroupData>(256, Get, Put);
        SetCompoundGroupDatas();
    }

    CompoundGroupData Get()
    {
        return mPoolHandleManager.GetSystemClass<CompoundGroupData>();
    }

    void Put(CompoundGroupData compoundGroupData)
    {
        if (null != compoundGroupData)
        {
            compoundGroupData.OnRecycle();
            mPoolHandleManager.Recycle(compoundGroupData);
        }
    }

    public override void Dispose()
    {

    }

    FastArrayElementFromPool<CompoundGroupData> mCachedCompoundGroupDatas;
    public FastArrayElementFromPool<CompoundGroupData> MCachedCompoundGroupDatas => mCachedCompoundGroupDatas;

    FastArrayMeta<int> groupIds = new FastArrayMeta<int>(64);

    /// <summary>
    /// 所有符合条件的合成组信息(已排序)
    /// </summary>
    /// <returns></returns>
    void SetCompoundGroupDatas()
    {
        mCachedCompoundGroupDatas.Clear();
        groupIds.Clear();
        var arr = CombineTableManager.Instance.array.gItem.handles;
        if (arr.Length > 0)
        {
            for (int i = 0, max = arr.Length; i < max; ++i)
            {
                var tiem = arr[i].Value as TABLE.COMBINE;
                if (!groupIds.Contains(tiem.groupID))
                {
                    groupIds.Add(tiem.groupID);
                }
            }

            for (int i = 0; i < groupIds.Count; i++)
            {
                var compoundGroupData = mCachedCompoundGroupDatas.Append();
                compoundGroupData.InitData(groupIds[i]);
            }

            mCachedCompoundGroupDatas.Sort(Compare);
        }
    }

    protected int Compare(CompoundGroupData a, CompoundGroupData b)
    {
        return a.GroupId.CompareTo(b.GroupId);
    }

    /// <summary>
    /// 判断有无可合成装备
    /// </summary>
    /// <returns></returns>
    public bool HasCombined()
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_HeCheng)) return false;
        for (int i = 0; i < mCachedCompoundGroupDatas.Count; i++)
        {
            CompoundGroupData compoundGroupData = mCachedCompoundGroupDatas[i];
            if (compoundGroupData.IsHasCombined)
                return true;
        }

        return false;
    }

    /// <summary>
    /// 是否可以合成其他道具
    /// </summary>
    /// <param name="itemId">该道具Id</param>
    /// <returns></returns>
    public bool IsAbilityCombine(int itemId)
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_HeCheng)) return false;
        bool isIsAbilityCombine = false;
        var arr = CombineTableManager.Instance.array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var list = (arr[i].Value as TABLE.COMBINE).needItem;
            if (list.Count == 2)
            {
                if (itemId == list[0])
                {
                    isIsAbilityCombine = true;
                    break;
                }
            }
        }

        return isIsAbilityCombine;
    }

    /// <summary>
    /// 物品是否能被合成
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public bool IsCombined(int itemId)
    {
        if (itemId > 0)
        {
            for (int i = 0, max = mCachedCompoundGroupDatas.Count; i < max; i++)
            {
                CompoundGroupData compoundGroupData = mCachedCompoundGroupDatas[i];
                for (int j = 0, max1 = compoundGroupData.GenerateItems.Count; j < max1; j++)
                {
                    GenerateItemData generateItemData = compoundGroupData.GenerateItems[j];
                    if (generateItemData.ItemId == itemId)
                        return true;
                }
            }
        }
        return false;
    }
    #region 网络响应处理函数

    /// <summary>
    /// 处理装备已合成响应信息
    /// </summary>
    /// <param name="msg"></param>
    public void HandleCombineItemMessage(combine.CombineItemResponse msg)
    {
        if (msg == null) return;
    }

    #endregion
}

/// <summary>
/// 装备合成组数据结构
/// </summary>
public class CompoundGroupData
{
    public CompoundGroupData()
    {
        mPoolHandleManager = new PoolHandleManager();
        generateItems = mPoolHandleManager.CreateGeneratePool<GenerateItemData>();
    }

    public void OnRecycle()
    {
        generateItems.Clear();
        mPoolHandleManager.RecycleAll();
    }

    PoolHandleManager mPoolHandleManager;

    public void InitData(int id)
    {
        GroupId = id;
        SetMGenerateItemData();
    }

    /// <summary>
    /// 组Id
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// 组名字
    /// </summary>
    private string groupName;
    public string GroupName => groupName;

    /// <summary>
    /// 组中的符合条件的装备（已排序）
    /// </summary>
    private FastArrayElementFromPool<GenerateItemData> generateItems;
    public FastArrayElementFromPool<GenerateItemData> GenerateItems => generateItems;

    void SetMGenerateItemData()
    {
        generateItems.Clear();
        var arr = CombineTableManager.Instance.array.gItem.handles;
        if (arr.Length > 0)
        {
            for (int i = 0, max = arr.Length; i < max; ++i)
            {
                var item = arr[i].Value as TABLE.COMBINE;
                if (item.groupID == GroupId)
                {
                    groupName = item.groupName;
                    GenerateItemData generateItemData = generateItems.Append();
                    generateItemData.InitData(item.id);
                    if (!generateItemData.IsShow)
                        generateItems.RemoveAt(generateItems.Count - 1);
                }
            }

            //按subType从小到大排序
            generateItems.Sort(GenerateItemComparer);
        }
    }


    public int GenerateItemComparer(GenerateItemData a, GenerateItemData b)
    {
        return a.Item.SubType - b.Item.SubType;
    }

    /// <summary>
    /// 该组里是否有可合成的装备
    /// </summary>
    public bool IsHasCombined
    {
        get
        {
            for (int i = 0; i < generateItems.Count; i++)
            {
                GenerateItemData generateItemData = generateItems[i];
                if (generateItemData.IsCombine)
                    return true;
            }

            return false;
        }
    }
}

/// <summary>
/// 单个合成装备数据结构
/// </summary>
public class GenerateItemData
{
    public void InitData(int id)
    {
        ItemId = id;
        name = null == Item ? string.Empty : Item.SubTypeName;
        listNeedItem = Item.needItem;
        listNeedResource = Item.needResource;

        if (listNeedResource.Count == 2)
        {
            int type = ItemTableManager.Instance.GetItemType(ListNeedResource[0]);
            isMoney = type == 1;
        }
        else
        {
            isMoney = false;
        }

        SetIsShow();
    }

    public TABLE.COMBINE Item { get; private set; }

    /// <summary>
    /// 装备Id
    /// </summary>
    int itemId;

    public int ItemId
    {
        get { return itemId; }
        set
        {
            if (itemId != value)
            {
                itemId = value;
                TABLE.COMBINE item;
                CombineTableManager.Instance.TryGetValue(itemId, out item);
                Item = item;
            }
        }
    }


    /// <summary>
    /// 当前子分类名字
    /// </summary>
    private string name;

    public string Name => name;


    /// <summary>
    /// 需要消耗装备Id和数量
    /// </summary>
    private IntArray listNeedItem;

    public IntArray ListNeedItem => listNeedItem;


    /// <summary>
    /// 需要消耗装备已有数量
    /// </summary>
    private int needItemCount;

    public int NeedItemCount
    {
        get
        {
            needItemCount = ListNeedItem.Count == 2 ? (int)CSBagInfo.Instance.GetItemCount(ListNeedItem[0]) : 0;
            return needItemCount;
        }
    }


    /// <summary>
    /// 额外需要消耗的资源Id和数量
    /// </summary>
    private IntArray listNeedResource;

    public IntArray ListNeedResource => listNeedResource;


    /// <summary>
    /// 额外需要消耗资源已有数量（货币or装备）
    /// </summary>
    private long needResourceCount;

    public long NeedResourceCount
    {
        get
        {
            needResourceCount = listNeedResource.Count == 2 ? listNeedResource[0].GetItemCount() : 0;
            return needResourceCount;
        }
    }


    /// <summary>
    /// 额外消耗资源是否是货币
    /// </summary>
    private bool isMoney;

    public bool IsMoney => isMoney;


    /// <summary>
    /// 是否可合成
    /// </summary>
    public bool IsCombine
    {
        get
        {
            if (ListNeedItem.Count == 2)
            {
                if (NeedItemCount >= ListNeedItem[1])
                {
                    //没有额外消耗或者额外消耗数量不够
                    if (ListNeedResource.Count == 2 && NeedResourceCount < ListNeedResource[1])
                        return false;

                    return true;
                }
                else //道具不够
                    return false;
            }
            else
                return false; //配置有误
        }
    }


    /// <summary>
    /// 是否符合条件显示
    /// </summary>
    private bool isShow;

    public bool IsShow => isShow;


    void SetIsShow()
    {
        var openTime = Item.openTime;
        var combineTime = Item.combineTime;
        if (openTime.Count == 3
            && CSMainPlayerInfo.Instance.Level >= openTime[0]
            && CSWoLongInfo.Instance.GetWoLongLevel() >= openTime[1]
            && CSLianTiInfo.Instance.GetLianTiLv() >= openTime[2])
        {
            isShow = combineTime.Count == 2 ? CSMainPlayerInfo.Instance.ServerOpenDay >= combineTime[0] : true;
        }
        else
        {
            isShow = false;
        }
    }
}