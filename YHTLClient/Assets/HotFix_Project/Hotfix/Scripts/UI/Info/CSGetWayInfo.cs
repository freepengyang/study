using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// GetWay数据的管理类，由于策划会有特殊需求，有些即使配了getway字段也需要根据具体情况显示隐藏
/// </summary>
public class CSGetWayInfo : CSInfo<CSGetWayInfo>
{

    Dictionary<int, GetWayData> getWayDataLibs = new Dictionary<int, GetWayData>(256);


    PoolHandleManager mPoolHandle = new PoolHandleManager();


    public override void Dispose()
    {
        mPoolHandle?.OnDestroy();
        mPoolHandle = null;
        getWayDataLibs?.Clear();
        getWayDataLibs = null;
    }


    public void GetGetWays(string ids, ref ILBetterList<GetWayData> list)
    {
        if (string.IsNullOrEmpty(ids)) return;
        var idList = UtilityMainMath.SplitStringToIntList(ids);
        GetGetWays(idList, ref list);
    }

    public void GetGetWays(List<int> ids, ref ILBetterList<GetWayData> list)
    {
        if (ids == null || ids.Count < 1) return;
        if (list == null) list = new ILBetterList<GetWayData>();
        for (int i = 0; i < ids.Count; i++)
        {
            GetGetWays(ids[i], ref list);
        }

        SortShowDatas(list);
    }


    public void GetGetWays(ILBetterList<int> ids, ref ILBetterList<GetWayData> list)
    {
        if (ids == null || ids.Count < 1) return;
        if (list == null) list = new ILBetterList<GetWayData>();
        for (int i = 0; i < ids.Count; i++)
        {
            GetGetWays(ids[i], ref list);
        }

        SortShowDatas(list);
    }


    public void GetGetWays(int id, ref ILBetterList<GetWayData> list)
    {
        if (list == null) list = new ILBetterList<GetWayData>();
        var data = GetDataFromLib(id);
        if (IsGetWayCanShow(data))
            list.Add(data);
    }


    GetWayData GetDataFromLib(int id)
    {
        GetWayData data = null;
        if (!getWayDataLibs.TryGetValue(id, out data))
        {
            if (GetWayTableManager.Instance.TryGetValue(id, out TABLE.GETWAY cfg))
            {
                if (cfg.type == 3) return null;//类型3直接不用
                data = mPoolHandle.GetCustomClass<GetWayData>();
                data.Init(cfg);
                getWayDataLibs.Add(id, data);
            }
        }

        if (data == null) FNDebug.LogErrorFormat("GetWay表中不存在的id: {0}", id);

        return data;
    }

    bool IsGetWayCanShow(GetWayData data)//类型3的不存入库中，因此后面不再做判断
    {
        if (data == null || data.Config == null) return false;
        var cfg = data.Config;
        if (CSMainPlayerInfo.Instance.Level < cfg.showLevel) return false;
        var serverOpenDay = CSMainPlayerInfo.Instance.ServerOpenDay;
        int downTime = cfg.downTime <= 0 ? 9999 : cfg.downTime;
        if (serverOpenDay < cfg.upTime || serverOpenDay > downTime) return false;
        if (cfg.type == 4 || cfg.type == 5)
        {
            if (!CheckSpecialParams(data)) return false;
        }

        return true;
    }


    bool CheckSpecialParams(GetWayData data)
    {
        if (data == null || data.Config == null || data.specialParams == null) return false;
        var cfg = data.Config;
        ILBetterList<int> ids = data.specialParams;
        if (cfg.type == 4)
        {
            TABLE.GIFTBAG gifbag = null;
            for (int i = 0; i < ids.Count; i++)
            {
                if (!GiftBagTableManager.Instance.TryGetValue(ids[i], out gifbag)) continue;
                if (gifbag.type == 0)
                {
                    if (!CSDiscountGiftBagInfo.Instance.IsHasSpecifyGiftBag(ids[i])) return false;
                }
                if (gifbag.type == 3)
                {
                    if (!CSDirectPurchaseInfo.Instance.IsHasSpecifyGiftBag(ids[i])) return false;
                }
            }
        }

        if (cfg.type == 5)
        {
            for (int i = 0; i < ids.Count; i++)
            {
                if (!CSOpenServerACInfo.Instance.IsCanRewardGift(ids[i])) return false;
            }  
        }

        return true;
    }


    /// <summary>
    /// 对传进来的列表数据进行排序
    /// </summary>
    /// <param name="list"></param>
    void SortShowDatas(ILBetterList<GetWayData> list)
    {
        if (list == null || list.Count < 2) return;
        list.Sort((a, b) =>
        {
            var cfgA = a.Config;
            var cfgB = b.Config;
            if (cfgA.recommend == cfgB.recommend)
            {
                return cfgB.order - cfgA.order;
            }
            else
            {
                return cfgA.recommend == 1 ? -1 : 1;
            }
        });
    }
    
}


public class GetWayData : IDispose
{
    int _id;
    public int Id { get => _id; }

    TABLE.GETWAY _config;
    public TABLE.GETWAY Config { get => _config; }

    /// <summary>
    /// 特殊参数.如果config类型为4，则该列表为tips字段分割出的giftbag id。
    /// 类型为5，则为specialActiveReward id。
    /// </summary>
    public ILBetterList<int> specialParams;

    public void Dispose()
    {
        _config = null;
        specialParams?.Clear();
        specialParams = null;
    }

    public void Init(TABLE.GETWAY cfg)
    {
        if (cfg == null) return;
        _config = cfg;
        _id = cfg.id;
        if ((cfg.type == 4 || cfg.type == 5) && !string.IsNullOrEmpty(cfg.Tips))
        {
            if (specialParams == null) specialParams = new ILBetterList<int>();
            else specialParams.Clear();

            var ids = cfg.Tips.Split('#');
            for (int i = 0; i < ids.Length; i++)
            {
                int id = 0;
                if (int.TryParse(ids[i], out id)) specialParams.Add(id);
            }
        }
    }
}