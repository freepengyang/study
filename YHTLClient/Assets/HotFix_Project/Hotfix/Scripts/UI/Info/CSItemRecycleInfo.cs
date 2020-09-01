using bag;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RecycleItemData
{
    public int cfgId;
    public TABLE.ITEM item;
    public int count;
}

public class RecycleCollectionData
{
    public TABLE.ITEM item;
    public int count;
}

public class CSItemRecycleInfo : Singleton<CSItemRecycleInfo>
{
    protected Dictionary<int, RecycleItemData[]> mAwardsDic = new Dictionary<int, RecycleItemData[]>(128);
    protected string CONST_EMPTY = "0";
    protected int sundryId = 587;
    protected List<KeyValuePair<int, int>> mOpenServerDay2WolongMaxLv = new List<KeyValuePair<int, int>>(8);
    public int getMaxWolongLv()
    {
        var openServerDay = CSMainPlayerInfo.Instance.ServerOpenDay;
        for (int i = 0,max=mOpenServerDay2WolongMaxLv.Count - 1; i < max; ++i)
        {
            if (openServerDay <= mOpenServerDay2WolongMaxLv[i].Key)
                return mOpenServerDay2WolongMaxLv[i].Value;
        }
        return int.MaxValue;
    }

    protected void InitRecycleRule()
    {
        TABLE.SUNDRY sundryItem = null;
        if(SundryTableManager.Instance.TryGetValue(sundryId,out sundryItem))
        {
            var tokens = sundryItem.effect.Split('#');
            for(int i = 1; i < tokens.Length;i+=2)
            {
                int needWolongLv = 0;
                int openServerDay = 0;
                if(i+1 < tokens.Length && int.TryParse(tokens[i],out needWolongLv) && int.TryParse(tokens[i+1],out openServerDay))
                {
                    mOpenServerDay2WolongMaxLv.Add(new KeyValuePair<int, int>(openServerDay, needWolongLv));
                }
            }
        }
    }

    public void Initialize()
    {
        //初始化回收规则
        InitRecycleRule();
    }

    protected Dictionary<int, int> mAwards = new Dictionary<int, int>();
    protected List<RecycleCollectionData> mPooledDatas = new List<RecycleCollectionData>();
    protected List<RecycleCollectionData> mActivedDatas = new List<RecycleCollectionData>();
    protected RecycleCollectionData Get()
    {
        if(mPooledDatas.Count > 0)
        {
            RecycleCollectionData ret = mPooledDatas[0];
            mPooledDatas.RemoveAt(0);
            mActivedDatas.Add(ret);
            return ret;
        }
        else
        {
            RecycleCollectionData ret = new RecycleCollectionData();
            mActivedDatas.Add(ret);
            return ret;
        }
    }

    public void RecycleNeigongEquip(bag.BagItemInfo bagItemInfo)
    {
        if (null == bagItemInfo)
            return;
        TABLE.ITEM itemCfg = null;
        if(!ItemTableManager.Instance.TryGetValue(bagItemInfo.configId,out itemCfg))
        {
            return;
        }
        FNDebug.LogFormat("<color=#00ff00>回收内功装备 => [{0}]</color>", itemCfg.name);
        Net.ReqCallBackItemMessage(bagItemInfo.bagIndex);
    }

    public bool CanBeRecycle(TABLE.ITEM itemCfg)
    {
        return false;
        //return null != itemCfg && itemCfg.callback != CONST_EMPTY;
    }

    public bool IsJobLegal(TABLE.ITEM itemCfg)
    {
        if (!(itemCfg.career >= (int)JobType.JT_NONE && itemCfg.career <= (int)JobType.JT_Taoist))
        {
            return false;
        }
        return true;
    }

    public enum RecycleMode
    {
        RM_NONE = -1,
        RM_NORMAL = 0,
        RM_NEIGONG = 1,
    }

    RecycleMode _recycleMode = RecycleMode.RM_NONE;
    public RecycleMode recycleMode
    {
        get
        {
            return _recycleMode;
        }
        set
        {
            _recycleMode = value;
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnRecycleModeChanged);
        }
    }
     
    public bool CanAsNormalRecycle(TABLE.ITEM itemCfg,bool calltips = false)
    {
        if (!CSBagInfo.Instance.IsNormalEquip(itemCfg))
        {
            if(calltips)
                UtilityTips.ShowRedTips(533);
            return false;
        }

        if (!CanBeRecycle(itemCfg))
        {
            if (calltips)
                UtilityTips.ShowRedTips(534);
            return false;
        }

        if (!IsJobLegal(itemCfg))
        {
            if (calltips)
                UtilityTips.ShowRedTips(535);
            return false;
        }

        return true;
    }

    public bool CanAsNeigongRecycle(TABLE.ITEM itemCfg)
    {
        if(!CSBagInfo.Instance.IsWoLongEquip(itemCfg))
            return false;

        if (!CanBeRecycle(itemCfg))
        {
            return false;
        }

        if (itemCfg.wolongLv > getMaxWolongLv())
            return false;

        if (!IsJobLegal(itemCfg))
            return false;

        var suitCfg = ZhanHunSuitTableManager.Instance.GetSuitCfg((int)itemCfg.zhanHunSuit);
        if (null == suitCfg)
        {
            return false;
        }
        return true;
    }

    public bool CanAsNeigongRecycle(int cfgId)
    {
        TABLE.ITEM itemCfg = null;
        if(!ItemTableManager.Instance.TryGetValue(cfgId,out itemCfg))
        {
            return false;
        }

        return CanAsNeigongRecycle(itemCfg);
    }

    public void RecycleData(RecycleCollectionData data)
    {
        mActivedDatas.Remove(data);
        mPooledDatas.Add(data);
    }

    public void RecycleDatas(List<RecycleCollectionData> datas)
    {
        if(null != datas)
        {
            for (int i = 0; i < datas.Count; ++i)
            {
                mActivedDatas.Remove(datas[i]);
            }
            mPooledDatas.AddRange(datas);
            datas.Clear();
        }
    }

    public void BeginAwards()
    {
        mAwards.Clear();
    }

    public void GetAwards(int cfgId)
    {
        RecycleItemData[] awardsData = null;
        if (mAwardsDic.ContainsKey(cfgId))
        {
            awardsData = mAwardsDic[cfgId];
            for (int i = 0; i < awardsData.Length; ++i)
            {
                if(null == awardsData[i])
                {
                    continue;
                }

                if(mAwards.ContainsKey(awardsData[i].cfgId))
                {
                    mAwards[awardsData[i].cfgId] += awardsData[i].count;
                }
                else
                {
                    mAwards[awardsData[i].cfgId] = awardsData[i].count;
                }
            }
            return;
        }

        var itemCfg = ItemTableManager.Instance.GetItemCfg(cfgId);
        if (null != itemCfg)
        {
            // if (true)
            // {
            //     awardsData = new RecycleItemData[0];
            // }
            // else
            // {
            //     var tokens = itemCfg.callback.Split('&');
            //     awardsData = new RecycleItemData[tokens.Length];
            //     for (int i = 0; i < tokens.Length; ++i)
            //     {
            //         awardsData[i] = null;
            //         var award = tokens[i].Split('#');
            //         int id = 0;
            //         int cnt = 0;
            //         if(award.Length == 2 && int.TryParse(award[0],out id) && int.TryParse(award[1],out cnt) && cnt > 0)
            //         {
            //             var awardCfg = ItemTableManager.Instance.GetItemCfg(id);
            //             if (null != awardCfg)
            //             {
            //                 awardsData[i] = new RecycleItemData
            //                 {
            //                     cfgId = id,
            //                     count = cnt,
            //                     item = awardCfg,
            //                 };
            //             }
            //         }
            //     }
            //     mAwardsDic.Add(cfgId, awardsData);
            //
            //     for (int i = 0; i < awardsData.Length; ++i)
            //     {
            //         if (null == awardsData[i])
            //         {
            //             continue;
            //         }
            //
            //         if (mAwards.ContainsKey(awardsData[i].cfgId))
            //         {
            //             mAwards[awardsData[i].cfgId] += awardsData[i].count;
            //         }
            //         else
            //         {
            //             mAwards[awardsData[i].cfgId] = awardsData[i].count;
            //         }
            //     }
            // }
        }
    }

    public void EndAwards(List<RecycleCollectionData> datas)
    {
        if(null != datas)
        {
            var iter = mAwards.GetEnumerator();
            while (iter.MoveNext())
            {
                var recycleCollectionData = Get();
                recycleCollectionData.item = ItemTableManager.Instance.GetItemCfg(iter.Current.Key);
                recycleCollectionData.count = iter.Current.Value;
                datas.Add(recycleCollectionData);
            }
        }
    }
}