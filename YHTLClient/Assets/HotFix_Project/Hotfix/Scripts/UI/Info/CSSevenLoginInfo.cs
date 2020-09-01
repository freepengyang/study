using System.Collections.Generic;
using TABLE;
using UnityEngine.Experimental.AI;

public class SevenLoginItem
{
    public TABLE.SPECIALACTIVEREWARD reward;
    public int status;//0:未达标 1:可领 2:已领 3:已经过期
    public bool running;//是否正在进行中
    public bool choiced;//是否选中
}

public class CSSevenLoginInfo : CSInfo<CSSevenLoginInfo>
{
    public override void Dispose()
    {

    }

    PoolHandleManager mPoolHandleManager;
    FastArrayElementFromPool<SevenLoginItem> mLoginItems;
    public FastArrayElementFromPool<SevenLoginItem> LoginItems
    {
        get
        {
            return mLoginItems;
        }
    }

    public SevenLoginItem Current
    {
        get;private set;
    }

    public bool HasRewardNeedFetch()
    {
        for(int i = 0,max = mLoginItems.Count;i < max;++i)
        {
            if (mLoginItems[i].status == 1)
                return true;
        }
        return false;
    }

    public int choicedId = -1;

    public CSSevenLoginInfo()
    {
        mPoolHandleManager = new PoolHandleManager(32);
        mLoginItems = mPoolHandleManager.CreateGeneratePool<SevenLoginItem>(8);
        Current = null;
        choicedId = -1;
        InitTableDatas();
    }

    //day => type => tips
    Dictionary<int, Dictionary<int,List<TABLE.SPECIALACTIVITYTIP>>> mId2tips = new Dictionary<int, Dictionary<int, List<TABLE.SPECIALACTIVITYTIP>>>(8);
    void InitTableDatas()
    {
        var handles = SpecialActivityTipTableManager.Instance.array.gItem.handles;
        for (int i = 0,max = handles.Length; i < max;++i)
        {
            var handle = handles[i].Value as TABLE.SPECIALACTIVITYTIP;
            for(int j = 0,mj = handle.openday.Length;j < mj;++j)
            {
                int v = handle.openday[j];
                if (!mId2tips.TryGetValue(v,out Dictionary<int, List<TABLE.SPECIALACTIVITYTIP>> type2ItemsDic))
                {
                    type2ItemsDic = new Dictionary<int, List<SPECIALACTIVITYTIP>>(8);
                    mId2tips.Add(v, type2ItemsDic);
                }

                if(!type2ItemsDic.TryGetValue(handle.type,out List<SPECIALACTIVITYTIP> acItem))
                {
                    acItem = new List<SPECIALACTIVITYTIP>(8);
                    type2ItemsDic.Add(handle.type, acItem);
                }

                if (!acItem.Contains(handle))
                    acItem.Add(handle);
            }
        }
    }

    Dictionary<int, List<TABLE.SPECIALACTIVITYTIP>> mDefaultTips = new Dictionary<int, List<TABLE.SPECIALACTIVITYTIP>>();
    public Dictionary<int, List<TABLE.SPECIALACTIVITYTIP>> GetTipItems(int openDay)
    {
        if(!mId2tips.TryGetValue(openDay,out Dictionary<int, List<TABLE.SPECIALACTIVITYTIP>> type2ItemsDic))
        {
            return mDefaultTips;
        }
        return type2ItemsDic;
    }

    public int MakeChoicedId(activity.ResSevenLogin msg)
    {
        var datas = msg.sevenLoginInfo;
        var datasList = SortHelper.GetComparersList(32);
        datasList.AddCompare(SortHelper.IntGreat, 0);
        datasList.AddCompare(SortHelper.IntGreat, 0);
        //datasList.AddCompare(SortHelper.IntGreat, 0);

        var handles = SortHelper.GetSortHandle(datas.Count);
        SortHelper.SortHandle handle = null;
        activity.SevenLoginInfo data = null;
        //0:未达标 1:可领 2:已领 3:已经过期
        
        for (int i = 0, max = datas.Count; i < max; ++i)
        {
            handle = handles[i];
            data = datas[i];
            handle.handle = data;

            handle.intValue[0] = data.rewardType == 1 ? 0 : 1;
            handle.intValue[1] = data.id;
        }

        SortHelper.Sort(handles, datasList);

        //for (int i = 0, max = datas.Count; i < max; ++i)
        //{
        //    datas[i] = handles[i].handle as activity.SevenLoginInfo;
        //}
        int ret = -1;

        var firstSevenLoginInfo = handles[0].handle as activity.SevenLoginInfo;
        if(firstSevenLoginInfo.rewardType != 1)
        {
            if(choicedId == -1)
            {
                //today
                for (int i = 0, max = datas.Count; i < max; ++i)
                {
                    if(datas[i].id % 100 == msg.openDay)
                    {
                        ret = datas[i].id;
                        FNDebug.LogFormat($"[七日登录]:没有奖励可领取的，没有原来的 选择今天的id:{ret}");
                        break;
                    }
                }

                if(ret == -1)
                {
                    ret = datas[datas.Count - 1].id;
                    FNDebug.LogFormat($"[七日登录]:没有奖励可领取的，没有原来的 没有今天的 选择最后一天id:{ret}");
                }
            }
            else
            {
                ret = choicedId;
                FNDebug.LogFormat($"[七日登录]:没有奖励可领取的，选择原来的 id:{ret}");
            }
        }
        else
        {
            ret = firstSevenLoginInfo.id;
            FNDebug.LogFormat($"[七日登录]:有奖励可领取的，选有奖励的 id:{ret}");
        }

        handles.OnRecycle();
        datasList.OnRecycle();

        return ret;
    }

    public void Initialize(activity.ResSevenLogin msg)
    {
        mLoginItems.Clear();
        choicedId = MakeChoicedId(msg);

        for (int i = 0,max = msg.sevenLoginInfo.Count;i < max;++i)
        {
            var loginInfo = msg.sevenLoginInfo[i];
            if (null == loginInfo)
                continue;

            if(!SpecialActiveRewardTableManager.Instance.TryGetValue(loginInfo.id,out SPECIALACTIVEREWARD rewardItem))
            {
                FNDebug.LogError($"query SPECIALACTIVEREWARD table error which id is {loginInfo.id}");
                continue;
            }

            var current = mLoginItems.Append();
            current.reward = rewardItem;
            current.status = loginInfo.rewardType;
            if(current.status == 0)
            {
                if (rewardItem.id % 100 < msg.openDay)
                    current.status = 3;
            }
            current.running = msg.openDay == loginInfo.id % 100;
            current.choiced = loginInfo.id == choicedId;

            if (current.running)
            {
                Current = current;
                FNDebug.Log($"[current]:{current.reward.id}");
            }
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnSevenDayLoginChanged);
    }
}