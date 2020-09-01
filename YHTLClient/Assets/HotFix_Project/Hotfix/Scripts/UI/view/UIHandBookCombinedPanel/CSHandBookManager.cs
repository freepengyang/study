using Google.Protobuf.Collections;
using Spine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using TABLE;
using UnityEngine;
using static CSAttributeInfo;

public delegate bool HandBookFilter(HandBookSlotData handBook);

public class CSHandBookManager : CSInfo<CSHandBookManager>
{
    protected FastArrayElementFromPool<HandBookSlotData> mHandBookSlotDatas;
    protected PoolHandleManager mPoolHandle = new PoolHandleManager();
    protected Dictionary<long, HandBookSlotData> mOwnedBookDatas = new Dictionary<long, HandBookSlotData>();
    protected FastArrayElementFromPool<HandBookSlotData> mCachedEmptyDatas;
    protected FastArrayElementKeepHandle<HandBookSlotData> mExpressData = new FastArrayElementKeepHandle<HandBookSlotData>(32);
    protected List<HandBookSlotData> mChoicedDatas = new List<HandBookSlotData>(16);
    protected Dictionary<int, FastArrayElementKeepHandle<TABLE.HANDBOOKSUIT>> mHandBookSuits;
    protected Dictionary<int, int> mQuality2Count;
    protected FastArrayElementFromPool<HandBookSlotData> mHandBookMarkDatas;
    protected FastArrayElementKeepHandle<HandBookSlotData> mExpressMarkDatas = new FastArrayElementKeepHandle<HandBookSlotData>(32);
    protected FastArrayElementFromPool<HandBookSlotData> mCachedBookMarkDatas;

    public enum HandBookOpMode
    {
        HBOM_CAMP = 0,
        HBOM_MAP,
        HBOM_POSITION,
        HBOM_COUNT,
    }

    //opmode => id => itemid => cnt
    protected Dictionary<HandBookOpMode, Dictionary<int, Dictionary<int,int>>> mCounterForChoiced = new Dictionary<HandBookOpMode, Dictionary<int, Dictionary<int, int>>>((int)HandBookOpMode.HBOM_COUNT);
    protected Dictionary<HandBookOpMode, Dictionary<int, Dictionary<int, int>>> mCounterForSetuped = new Dictionary<HandBookOpMode, Dictionary<int, Dictionary<int, int>>>((int)HandBookOpMode.HBOM_COUNT);
    protected Dictionary<HandBookOpMode, Dictionary<int, Dictionary<int, int>>> mCountersForOwned = new Dictionary<HandBookOpMode, Dictionary<int, Dictionary<int, int>>>((int)HandBookOpMode.HBOM_COUNT);
    protected bool mCountersForOwnedDirty = true;

    public FastArrayElementFromPool<HandBookSlotData> HandBookSlotDatas
    {
        get
        {
            return mHandBookSlotDatas;
        }
    }

    public bool UpgradeLevelFilter(HandBookSlotData handBook)
    {
        return null == handBook || handBook.levelFull;
    }

    public bool CheckHandBookSetupRedPoint(out HandBookSlotData slot,out HandBookSlotData card)
    {
        slot = null;
        card = null;
        for(int i = 0; i < mHandBookSlotDatas.Count; ++i)
        {
            if(null == mHandBookSlotDatas[i].HandBook && mHandBookSlotDatas[i].Opened)
            {
                slot = mHandBookSlotDatas[i];
                break;
            }
        }
        if (null == slot)
            return false;
        for (var it = mOwnedBookDatas.GetEnumerator(); it.MoveNext();)
            if (it.Current.Value.SlotID == 0)
            {
                card = it.Current.Value;
                return true;
            }
        return false;
    }

    public bool CheckHandBookUpgradeLevelRedPoint()
    {
        for (var it = mOwnedBookDatas.GetEnumerator(); it.MoveNext();)
            if (it.Current.Value.CanUpgrade)
                return true;
        return false;
    }

    public bool CheckHandBookSetupedUpgradeLevelRedPoint(out HandBookSlotData handBookSlotData)
    {
        handBookSlotData = null;
        for (var it = mOwnedBookDatas.GetEnumerator(); it.MoveNext();)
            if (it.Current.Value.CanUpgrade && it.Current.Value.Setuped)
            {
                handBookSlotData = it.Current.Value;
                return true;
            }
        return false;
    }

    public bool CheckHandBookUpgradeQualityRedPoint()
    {
        for (var it = mOwnedBookDatas.GetEnumerator(); it.MoveNext();)
            if (it.Current.Value.CanUpgradeQuality)
                return true;
        return false;
    }
    public bool CheckHandBookSetupedUpgradeQualityRedPoint(out HandBookSlotData handBookSlotData)
    {
        handBookSlotData = null;
        for (var it = mOwnedBookDatas.GetEnumerator(); it.MoveNext();)
            if (it.Current.Value.CanUpgradeQuality && it.Current.Value.Setuped)
            {
                handBookSlotData = it.Current.Value;
                return true;
            }
        return false;
    }

    /// <summary>
    /// 是否存在可以解锁的卡槽
    /// </summary>
    /// <returns></returns>
    public bool CheckExistNeedUnlockSlot(out HandBookSlotData handBookSlotData)
    {
        handBookSlotData = null;
        for (int i = 0; i < mHandBookSlotDatas.Count; ++i)
        {
            var handbookSlotData = mHandBookSlotDatas[i];
            if (null == handbookSlotData || null == handbookSlotData.HandBookSlot || handbookSlotData.Opened)
            {
                continue;
            }

            if(!CanUnlockSlot(handbookSlotData.SlotID,false))
            {
                continue;
            }

            if(!HandBookOpenSlotCostId.IsItemEnough(handbookSlotData.HandBookSlot.cost,0,false))
            {
                continue;
            }

            handBookSlotData = handbookSlotData;
            return true;
        }
        return false;
    }

    public int HandBookOpenSlotCostId
    {
        get
        {
            return 30042031;//幻灵钥匙
        }
    }

    public bool CanUnlockSlot(int slotId,bool message)
    {
        if(slotId <= 0 || slotId > mHandBookSlotDatas.Count)
        {
            if(message)
                UtilityTips.ShowRedTips(649);
            return false;
        }

        if(slotId > 0 && slotId <= mHandBookSlotDatas.Count)
        {
            if(null == mHandBookSlotDatas[slotId - 1].HandBookSlot)
            {
                if (message)
                    UtilityTips.ShowRedTips(649);
                return false;
            }

            for (int i = 0; i < slotId - 1; ++i)
            {
                if(!mHandBookSlotDatas[i].Opened)
                {
                    if(message)
                        UtilityTips.ShowRedTips(648);
                    return false;
                }
            }

            var openDays = CSMainPlayerInfo.Instance.RoleExtraValues?.openServerDays;
            if (mHandBookSlotDatas[slotId - 1].HandBookSlot.kaiFuDay > 0 && mHandBookSlotDatas[slotId - 1].HandBookSlot.kaiFuDay > openDays)
            {
                if (message)
                    UtilityTips.ShowRedTips(650,openDays);
                return false;
            }

            return true;
        }
        return false;
    }

    public TABLE.HANDBOOK NextLevel(TABLE.HANDBOOK current)
    {
        if (null != current)
        {
            int id = HandBookTableManager.Instance.make_id(current.ItemID,current.Level + 1, current.Quality);
            TABLE.HANDBOOK next = null;
            if (!HandBookTableManager.Instance.TryGetValue(id, out next))
                return current;
            return next;
        }
        return null;
    }

    public TABLE.HANDBOOK NextQuality(TABLE.HANDBOOK current)
    {
        if (null != current)
        {
            int id = HandBookTableManager.Instance.make_id(current.ItemID,current.Level, current.Quality + 1);
            TABLE.HANDBOOK next = null;
            if (!HandBookTableManager.Instance.TryGetValue(id, out next))
                return current;
            return next;
        }
        return null;
    }

    string[] campNames;
    string[] mapNames;
    string[] positionNames;
    string[] qualityNames;
    string[] campShortNames;
    string[] mapShortNames;
    string[] positionShortNames;
    void InitSuitNames()
    {
        TABLE.SUNDRY sundryItem = null;
        if (SundryTableManager.Instance.TryGetValue(53, out sundryItem))
        {
            campNames = sundryItem.effect.Split('#');
        }
        if (SundryTableManager.Instance.TryGetValue(54, out sundryItem))
        {
            mapNames = sundryItem.effect.Split('#');
        }
        if (SundryTableManager.Instance.TryGetValue(55, out sundryItem))
        {
            positionNames = sundryItem.effect.Split('#');
        }
        if (SundryTableManager.Instance.TryGetValue(56, out sundryItem))
        {
            qualityNames = sundryItem.effect.Split('#');
        }
        if (SundryTableManager.Instance.TryGetValue(57, out sundryItem))
        {
            campShortNames = sundryItem.effect.Split('#');
        }
        if (SundryTableManager.Instance.TryGetValue(58, out sundryItem))
        {
            mapShortNames = sundryItem.effect.Split('#');
        }
        if (SundryTableManager.Instance.TryGetValue(59, out sundryItem))
        {
            positionShortNames = sundryItem.effect.Split('#');
        }
    }

    public string GetHandBookQualityName(HANDBOOK bookItem)
    {
        if (null == bookItem || null == qualityNames)
            return string.Empty;
        int quality = bookItem.Quality - 1;
        if (quality < 0 || quality >= qualityNames.Length)
            return string.Empty;
        return qualityNames[quality];
    }

    public string GetSuitConditionName(int judgeType, int judgeValue)
    {
        if (!(judgeType > (int)HandBookOpMode.HBOM_CAMP && judgeType <= (int)HandBookOpMode.HBOM_COUNT))
        {
            return string.Empty;
        }
        HandBookOpMode opMode = (HandBookOpMode)judgeType - 1;
        string[] values = null;
        if (opMode == HandBookOpMode.HBOM_CAMP)
            values = campNames;
        if (opMode == HandBookOpMode.HBOM_MAP)
            values = mapNames;
        if (opMode == HandBookOpMode.HBOM_POSITION)
            values = positionNames;
        if (null == values || judgeValue < 0 || judgeValue >= values.Length)
            return string.Empty;
        return values[judgeValue];
    }

    public string GetCampShortName(int judgeValue)
    {
        if (null == campShortNames || judgeValue < 0 || judgeValue >= campShortNames.Length)
            return string.Empty;
        return campShortNames[judgeValue];
    }

    public string GetMapShortName(int judgeValue)
    {
        if (null == mapShortNames || judgeValue < 0 || judgeValue >= mapShortNames.Length)
            return string.Empty;
        return mapShortNames[judgeValue];
    }

    public string GetPositionShortName(int judgeValue)
    {
        if (null == positionShortNames || judgeValue < 0 || judgeValue >= positionShortNames.Length)
            return string.Empty;
        return positionShortNames[judgeValue];
    }

    public int GetSuitCount(int judgeType,int judgeValue)
    {
        if (!(judgeType > (int)HandBookOpMode.HBOM_CAMP && judgeType <= (int)HandBookOpMode.HBOM_COUNT))
        {
            return 0;
        }
        HandBookOpMode opMode = (HandBookOpMode)judgeType - 1;
        if (!mCountersForOwned.ContainsKey(opMode))
        {
            return 0;
        }
        var dic = mCountersForOwned[opMode];
        if (null == dic || !dic.ContainsKey(judgeValue))
            return 0;
        return dic[judgeValue].Count;
    }

    public bool IsActived(TABLE.HANDBOOKSUIT suit)
    {
        if (null == suit)
            return false;
        int owned = CSHandBookManager.Instance.GetSetupedSuitCount(suit.judgeType, suit.judgeValue);
        return suit.requirenum <= owned;
    }

    public TABLE.HANDBOOKSUIT GetActivedSuit(TABLE.HANDBOOKSUIT suit)
    {
        if (null != suit)
        {
            if (IsActived(suit))
                return suit;

            int prevId = HandBookSuitTableManager.Instance.make_id(suit.Lv - 1, suit.Group);
            TABLE.HANDBOOKSUIT prevSuit = null;
            HandBookSuitTableManager.Instance.TryGetValue(prevId, out prevSuit);
            return null == prevSuit ? suit : prevSuit;
        }
        return suit;
    }

    public TABLE.HANDBOOKSUIT NextSuit(TABLE.HANDBOOKSUIT suit)
    {
        if(null != suit)
        {
            int nextId = HandBookSuitTableManager.Instance.make_id(suit.Lv + 1,suit.Group);
            TABLE.HANDBOOKSUIT nextSuit = null;
            HandBookSuitTableManager.Instance.TryGetValue(nextId, out nextSuit);
            return null == nextSuit ? suit : nextSuit;
        }
        return suit;
    }

    public int GetSetupedSuitCount(int judgeType, int judgeValue)
    {
        if (!(judgeType > (int)HandBookOpMode.HBOM_CAMP && judgeType <= (int)HandBookOpMode.HBOM_COUNT))
        {
            return 0;
        }
        HandBookOpMode opMode = (HandBookOpMode)judgeType - 1;
        if (!mCounterForSetuped.ContainsKey(opMode))
        {
            return 0;
        }
        var dic = mCounterForSetuped[opMode];
        if (null == dic || !dic.ContainsKey(judgeValue))
            return 0;

        return dic[judgeValue].Count;
    }

    public void GetSetupedAttributes(PoolHandleManager poolHandle,System.Action<RepeatedField<KeyValue>> onItemVisible)
    {
        var attrIds = poolHandle.GetSystemClass<RepeatedField<int>>();
        var attrNums = poolHandle.GetSystemClass<RepeatedField<int>>();
        attrIds.Clear();
        attrNums.Clear();
        for (int i = 0,maxi = mHandBookSlotDatas.Count; i < maxi; ++i)
        {
            var slotData = mHandBookSlotDatas[i];
            if(null == slotData || null == slotData.HandBook)
            {
                continue;
            }
            var handBook = slotData.HandBook;

            for (int k = 0,max = handBook.parameter.Count;k < max;++k)
            {
                attrIds.Add(handBook.parameter[k]);
            }
            for (int k = 0, max = handBook.factor.Count; k < max; ++k)
            {
                attrNums.Add(handBook.factor[k]);
            }
        }
        RepeatedField<KeyValue> kvs = CSAttributeInfo.Instance.GetAttributes(poolHandle, attrIds, attrNums);
        attrIds.Clear();
        attrNums.Clear();
        poolHandle.Recycle(attrIds);
        poolHandle.Recycle(attrNums);
        onItemVisible?.Invoke(kvs);
        kvs.Clear();
        poolHandle.Recycle(kvs);
    }

    public enum UpgradeMode
    {
        UM_LEVEL = 1,
        UM_QUALITY = 2,
    }

    public void GetUpgradeAttributes(TABLE.HANDBOOK handbook,PoolHandleManager poolHandle, System.Action<RepeatedField<KeyValue>,RepeatedField<KeyValue>,bool> onItemVisible, UpgradeMode eMode,bool playEffect = false)
    {
        var attrIds = poolHandle.GetSystemClass<RepeatedField<int>>();
        var attrNums = poolHandle.GetSystemClass<RepeatedField<int>>();
        attrIds.Clear();
        attrNums.Clear();
        if (null != handbook)
        {
            attrIds.AddRange(handbook.parameter);
            attrNums.AddRange(handbook.factor);
        }

        var nextAttrIds = poolHandle.GetSystemClass<RepeatedField<int>>();
        var nextAttrNums = poolHandle.GetSystemClass<RepeatedField<int>>();
        nextAttrIds.Clear();
        nextAttrNums.Clear();
        var nextHandBook = eMode == UpgradeMode.UM_LEVEL ? NextLevel(handbook) : NextQuality(handbook);
        if (null != nextHandBook)
        {
            nextAttrIds.AddRange(nextHandBook.parameter);
            nextAttrNums.AddRange(nextHandBook.factor);
        }

        RepeatedField<KeyValue> kvs = CSAttributeInfo.Instance.GetAttributes(poolHandle, attrIds, attrNums);
        RepeatedField<KeyValue> nextKvs = CSAttributeInfo.Instance.GetAttributes(poolHandle, nextAttrIds, nextAttrNums);
        CSAttributeInfo.Instance.AlignAttributes(poolHandle,kvs, nextKvs);

        onItemVisible?.Invoke(kvs, nextKvs,playEffect);

        kvs.Clear();
        poolHandle.Recycle(kvs);
        nextKvs.Clear();
        poolHandle.Recycle(nextKvs);

        attrIds.Clear();
        attrNums.Clear();
        poolHandle.Recycle(attrIds);
        poolHandle.Recycle(attrNums);
        nextAttrIds.Clear();
        nextAttrNums.Clear();
        poolHandle.Recycle(nextAttrIds);
        poolHandle.Recycle(nextAttrNums);
    }

    FastArrayElementFromPool<HandBookGroupItemData> mGroupDatasForChoicedPreview;
    protected void RebuildChoicedHandBookGroupDatas()
    {
        RebuildGroupDatas(mGroupDatasForChoicedPreview, mCounterForChoiced);
    }

    FastArrayElementFromPool<HandBookGroupItemData> mGroupDatasForSetuped;
    protected void RebuildSetupedHandBookGroupDatas()
    {
        RebuildGroupDatas(mGroupDatasForSetuped, mCounterForSetuped);
    }

    FastArrayElementFromPool<HandBookGroupItemData> mOwnedGroupDatas;
    protected void RebuildOwnedHandBookGroupDatas()
    {
        if(!mCountersForOwnedDirty)
        {
            return;
        }
        mCountersForOwnedDirty = false;
        RebuildGroupDatas(mOwnedGroupDatas,mCountersForOwned);
    }

    protected void RebuildGroupDatas(FastArrayElementFromPool<HandBookGroupItemData> groupDatas, Dictionary<HandBookOpMode, Dictionary<int, Dictionary<int,int>>> counter)
    {
        groupDatas.Clear();
        var it = mHandBookSuits.GetEnumerator();
        while (it.MoveNext())
        {
            var suits = it.Current.Value;
            TABLE.HANDBOOKSUIT suit = null;
            for (int i = 0; i < suits.Count; ++i)
            {
                suit = suits[i];
                if (!(suit.judgeType > (int)HandBookOpMode.HBOM_CAMP && suit.judgeType <= (int)HandBookOpMode.HBOM_COUNT))
                {
                    continue;
                }

                HandBookOpMode opMode = (HandBookOpMode)(suit.judgeType - 1);
                var judgeGroup = counter[opMode];

                int reachedValue = 0;
                if (judgeGroup.ContainsKey(suit.judgeValue))
                {
                    reachedValue = judgeGroup[suit.judgeValue].Count;
                }

                if (reachedValue < suit.requirenum || i == suits.Count - 1)
                {
                    var handle = groupDatas.Append();
                    handle.bookSuitItem = suit;
                    handle.owned = reachedValue;
                    handle.max = suit.requirenum;
                    handle.hasEffect = i > 0 || reachedValue >= suit.requirenum;
                    break;
                }
            }
        }

        GroupItemDataCompare(groupDatas);
        //groupDatas.Sort(GroupItemDataComparer);
    }

    public void GroupItemDataCompare(FastArrayElementFromPool<HandBookGroupItemData> datas)
    {
        var datasList = SortHelper.GetComparersList(32);
        datasList.AddCompare(SortHelper.IntLess, 0);
        datasList.AddCompare(SortHelper.IntGreat, 0);

        var handles = SortHelper.GetSortHandle(datas.Count);
        int v = 0;
        SortHelper.SortHandle handle = null;
        HandBookGroupItemData data = null;
        for (int i = 0, max = datas.Count; i < max; ++i)
        {
            handle = handles[i];
            data = datas[i];
            handle.handle = data;

            v = data.bookSuitItem.Lv * 1000000;
            v += data.owned > 0 ? 100000 : 0;
            v += (100 - (data.max - data.owned)) * 100;
            v += (100 - data.bookSuitItem.Group);

            handle.intValue[0] = v;
            handle.intValue[1] = data.bookSuitItem.id;
        }

        SortHelper.Sort(handles, datasList);

        for (int i = 0, max = datas.Count; i < max; ++i)
        {
            datas[i] = handles[i].handle as HandBookGroupItemData;
        }

        handles.OnRecycle();
        datasList.OnRecycle();
    }

    protected int GroupItemDataComparer(HandBookGroupItemData l, HandBookGroupItemData r)
    {
        if (l.bookSuitItem.Lv != r.bookSuitItem.Lv)
            return r.bookSuitItem.Lv - l.bookSuitItem.Lv;
        if ((l.owned > 0) != (r.owned > 0))
            return l.owned > 0 ? -1 : 1;
        int lneed = l.max - l.owned;
        int rneed = r.max - r.owned;
        if (lneed != rneed)
            return lneed - rneed;
        if (l.bookSuitItem.Group != r.bookSuitItem.Group)
            return l.bookSuitItem.Group - r.bookSuitItem.Group;
        return l.bookSuitItem.id - r.bookSuitItem.id;
    }

    public FastArrayElementFromPool<HandBookGroupItemData> GetHandBookGroupDatas()
    {
        return mGroupDatasForSetuped;
    }

    public FastArrayElementFromPool<HandBookGroupItemData> GetHandBookGroupDatasForChoicedPreview()
    {
        RebuildChoicedHandBookGroupDatas();
        return mGroupDatasForChoicedPreview;
    }

    protected int GetOwnedGroupMaxLv(HandBookSlotData l)
    {
        int lv = 0;
        int v = 14;
        for (int i = 0; i < mOwnedGroupDatas.Count && v > 0; ++i)
        {
            var groupData = mOwnedGroupDatas[i];

            if (groupData.bookSuitItem.judgeType == ((int)HandBookOpMode.HBOM_CAMP + 1) && groupData.bookSuitItem.judgeValue == l.HandBook.camp ||
                groupData.bookSuitItem.judgeType == ((int)HandBookOpMode.HBOM_MAP + 1) && groupData.bookSuitItem.judgeValue == l.HandBook.sitemap ||
                groupData.bookSuitItem.judgeType == ((int)HandBookOpMode.HBOM_POSITION + 1) && groupData.bookSuitItem.judgeValue == l.HandBook.status)
            {
                int shiftV = 1 << groupData.bookSuitItem.judgeValue;
                if ((v & shiftV) > 0)
                {
                    v &= ~shiftV;
                    if (lv < groupData.bookSuitItem.Lv)
                        lv = groupData.bookSuitItem.Lv;
                }
            }
        }
        return lv;
    }

    public void HandBookSelectQualityDescCompare(FastArrayElementKeepHandle<HandBookSlotData> datas,int cachedCount)
    {
        var datasList = SortHelper.GetComparersList(32);
        datasList.AddCompare(SortHelper.IntLess, 0);
        datasList.AddCompare(SortHelper.IntGreat, 0);

        var handles = SortHelper.GetSortHandle(datas.Count - cachedCount);
        int v = 0;
        SortHelper.SortHandle handle = null;
        HandBookSlotData data = null;
        for (int i = 0, max = handles.Count; i < max; ++i)
        {
            handle = handles[i];
            data = datas[i];
            handle.handle = data;
            v = data.HandBook.Quality * 1000;
            v += data.HandBook.Level * 10;
            handle.intValue[0] = v;
            handle.intValue[1] = data.HandBookId;
        }
        SortHelper.Sort(handles, datasList);

        for (int i = 0, max = handles.Count; i < max; ++i)
        {
            datas[i] = handles[i].handle as HandBookSlotData;
        }

        handles.OnRecycle();
        datasList.OnRecycle();
    }

    public void HandBookSelectQualityAscCompare(FastArrayElementKeepHandle<HandBookSlotData> datas,int cachedCount)
    {
        var datasList = SortHelper.GetComparersList(32);
        datasList.AddCompare(SortHelper.IntGreat, 0);
        datasList.AddCompare(SortHelper.IntGreat, 0);

        var handles = SortHelper.GetSortHandle(datas.Count - cachedCount);
        int v = 0;
        SortHelper.SortHandle handle = null;
        HandBookSlotData data = null;
        for (int i = 0, max = handles.Count; i < max; ++i)
        {
            handle = handles[i];
            data = datas[i];
            handle.handle = data;
            v = data.HandBook.Quality * 1000;
            v += (100 - data.HandBook.Level) * 10;
            handle.intValue[0] = v;
            handle.intValue[1] = data.HandBookId;
        }
        SortHelper.Sort(handles, datasList);

        for (int i = 0, max = handles.Count; i < max; ++i)
        {
            datas[i] = handles[i].handle as HandBookSlotData;
        }

        handles.OnRecycle();
        datasList.OnRecycle();
    }

    public void HandBookChoiceForUpgradeCompare(FastArrayElementKeepHandle<HandBookSlotData> datas)
    {
        var datasList = SortHelper.GetComparersList(32);
        datasList.AddCompare(SortHelper.IntLess, 0);
        datasList.AddCompare(SortHelper.IntGreat, 0);

        var handles = SortHelper.GetSortHandle(datas.Count);
        int v = 0;
        SortHelper.SortHandle handle = null;
        HandBookSlotData data = null;
        for (int i = 0, max = datas.Count; i < max; ++i)
        {
            handle = handles[i];
            data = datas[i];
            handle.handle = data;
            v = data.CanUpgrade ? 100000 : 0;
            v += data.Setuped ? 10000 : 0;
            v += data.HandBook.Quality * 1000;
            v += (100 - data.HandBook.Level) * 10;
            handle.intValue[0] = v;
            handle.intValue[1] = data.HandBookId;
        }
        SortHelper.Sort(handles, datasList);

        for (int i = 0, max = datas.Count; i < max; ++i)
        {
            datas[i] = handles[i].handle as HandBookSlotData;
        }

        handles.OnRecycle();
        datasList.OnRecycle();
    }

    public void HandBookChoicedForMergeCompare(FastArrayElementKeepHandle<HandBookSlotData> datas)
    {
        var datasList = SortHelper.GetComparersList(32);
        datasList.AddCompare(SortHelper.IntLess, 0);
        datasList.AddCompare(SortHelper.IntGreat, 0);

        var handles = SortHelper.GetSortHandle(datas.Count);
        int v = 0;
        SortHelper.SortHandle handle = null;
        HandBookSlotData data = null;
        for (int i = 0, max = datas.Count; i < max; ++i)
        {
            handle = handles[i];
            data = datas[i];
            handle.handle = data;
            v = data.CanUpgradeQuality ? 100000 : 0;
            v += data.Setuped ? 10000 : 0;
            v += (10 - data.HandBook.Quality) * 1000;
            v += data.HandBook.Level * 10;
            handle.intValue[0] = v;
            handle.intValue[1] = data.HandBookId;
        }
        SortHelper.Sort(handles, datasList);

        for (int i = 0, max = datas.Count; i < max; ++i)
        {
            datas[i] = handles[i].handle as HandBookSlotData;
        }

        handles.OnRecycle();
        datasList.OnRecycle();
    }

    public int GetSetupedCardCount()
    {
        return mChoicedDatas.Count;
    }

    public int GetOpenedSlotCount()
    {
        int cnt = 0;
        for (int i = 0; i < mHandBookSlotDatas.Count; ++i)
            if (mHandBookSlotDatas[i].Opened)
                ++cnt;
        return cnt;
    }

    public List<HandBookSlotData> GetChoicedDatas()
    {
        mChoicedDatas.Clear();
        mCounterForChoiced[HandBookOpMode.HBOM_CAMP].Clear();
        mCounterForChoiced[HandBookOpMode.HBOM_MAP].Clear();
        mCounterForChoiced[HandBookOpMode.HBOM_POSITION].Clear();
        for (int j = 0; j < mHandBookSlotDatas.Count; ++j)
        {
            var slotData = mHandBookSlotDatas[j];
            HandBookSlotData choicedData = null;
            if (mOwnedBookDatas.ContainsKey(slotData.Guid))
            {
                choicedData = mOwnedBookDatas[slotData.Guid];
                choicedData.AddFlag(HandBookSlotData.CardFlag.CF_SELECTED);
                mChoicedDatas.Add(choicedData);
                AddCounterForChoiced(choicedData.HandBook);
            }
        }
        return mChoicedDatas;
    }

    public void AddChoicedData(HandBookSlotData choiceData)
    {
        if(!mChoicedDatas.Contains(choiceData))
        {
            mChoicedDatas.Add(choiceData);
            AddCounterForChoiced(choiceData.HandBook);
        }
    }

    public void RemoveChoicedData(HandBookSlotData choiceData)
    {
        if(mChoicedDatas.Remove(choiceData))
        {
            RemoveCounterForChoiced(choiceData.HandBook);
        }
    }

    public HandBookSlotData GetOwnedHandBook(long guid)
    {
        if(mOwnedBookDatas.ContainsKey(guid))
        {
            return mOwnedBookDatas[guid];
        }
        return null;
    }

    public void SortMergeComparer(FastArrayElementKeepHandle<HandBookSlotData> datas, HandBookSlotData lockedData)
    {
        var compareLists = SortHelper.GetComparersList(32);
        if(null == lockedData)
        {
            compareLists.AddCompare(SortHelper.IntLess, 0);
            compareLists.AddCompare(SortHelper.LongGreat, 0);
        }
        else
        {
            compareLists.AddCompare(SortHelper.IntLess, 0);
            compareLists.AddCompare(SortHelper.LongGreat, 0);
        }

        var handles = SortHelper.GetSortHandle(datas.Count);
        int v = 0;
        for(int i = 0,max = handles.Count;i < max;++i)
        {
            var handle = handles[i];
            var data = datas[i];
            var handbook = data.HandBook;

            if(null == lockedData)
            {
                v = data.CanUpgradeQuality ? 1000000 : 0;
                v += data.CanUpgradeQualityIgnoreItemEnough ? 100000 : 0;
                v += data.Setuped ? 10000 : 0;
                v += (10 - handbook.Quality) * 100;
                v += handbook.Level;
                handle.intValue[0] = v;//IntLess 0
                handle.longValue[0] = handbook.id;//LongGreat 0
            }
            else
            {
                v = lockedData.Guid == data.Guid ? 1000000000 : 0;
                v += lockedData.HandBook.Quality == handbook.Quality ? 100000000 : 0;
                v += lockedData.HandBook.itemID == handbook.itemID ? 10000000 : 0;
                v += data.CanUpgradeQuality ? 1000000 : 0;
                v += data.CanUpgradeQualityIgnoreItemEnough ? 100000 : 0;
                v += data.Setuped ? 10000 : 0;
                v += (10 - handbook.Quality) * 100;
                v += handbook.Level;
                handle.intValue[0] = v;//IntLess 0
                handle.longValue[0] = handbook.id;//LongGreat 0
            }

            handle.handle = data;
        }
        SortHelper.Sort(handles, compareLists);
        for (int i = 0, max = handles.Count; i < max; ++i)
        {
            datas[i] = handles[i].handle as HandBookSlotData;
        }
        handles.OnRecycle();
        compareLists.OnRecycle();
    }

    public FastArrayElementKeepHandle<HandBookSlotData>  GetOwnedExpressDatas(ref int cachedCount,int campId = 0,int zoneId = 0,int positionId = 0,int minValue = 8, HandBookFilter filter = null)
    {
        cachedCount = 0;
        RebuildOwnedHandBookGroupDatas();
        mExpressData.Clear();
        for(var it = mOwnedBookDatas.GetEnumerator();it.MoveNext();)
        {
            var data = it.Current.Value;
            var handBook = data.HandBook;
            if (null == handBook)
            {
                continue;
            }
            if (handBook.camp != campId && campId != 0)
            {
                continue;
            }

            if (handBook.sitemap != zoneId && zoneId != 0)
            {
                continue;
            }

            if (handBook.status != positionId && positionId != 0)
            {
                continue;
            }

            if (null != filter && filter(data))
                continue;

            data.RemoveFlag(HandBookSlotData.CardFlag.CF_ENABLE_UPGRADE_QUALITY_FLAG);
            data.RemoveFlag(HandBookSlotData.CardFlag.CF_ENABLE_UPGRADE_LEVEL_FLAG);
            data.RemoveFlag(HandBookSlotData.CardFlag.CF_ENABLE_UPGRADE_QUALITY_FILTER);
            mExpressData.Append(data);
        }

        mCachedEmptyDatas.Count = minValue - mExpressData.Count;
        cachedCount = mCachedEmptyDatas.Count;

        for (int i = 0; i < cachedCount; ++i)
            mExpressData.Append(mCachedEmptyDatas[i]);
        return mExpressData;
    }

    public void ClearExpressDataChoicedFlag()
    {
        mChoicedDatas.Clear();
        mCounterForChoiced[HandBookOpMode.HBOM_CAMP].Clear();
        mCounterForChoiced[HandBookOpMode.HBOM_MAP].Clear();
        mCounterForChoiced[HandBookOpMode.HBOM_POSITION].Clear();
        var it = mOwnedBookDatas.GetEnumerator();
        HandBookSlotData handBookSlotData = null;
        while(it.MoveNext())
        {
            handBookSlotData = it.Current.Value;
            handBookSlotData.RemoveFlag((int)(HandBookSlotData.CardFlag.CF_ENABLE_SELECTED | HandBookSlotData.CardFlag.CF_ENABLE_UNSELETED | HandBookSlotData.CardFlag.CF_SELECTED));
            handBookSlotData.onChoiceChanged = null;
            handBookSlotData.onClicked = null;
            handBookSlotData.onKeepPressed = null;
        }
    }

    public class SlotBindData
    {
        public int slotIdx;
        public long guid;
        public void Reset()
        {

        }
    }
    FastArrayElementFromPool<SlotBindData> mCachedValues;

    public void RequestForChoicedCard()
    {
        mCachedValues.Clear();
        //设置取消选中
        for (int i = 0; i < mHandBookSlotDatas.Count; ++i)
        {
            var slotData = mHandBookSlotDatas[i];
            if (slotData.Guid == 0)
            {
                continue;
            }

            int findIdx = -1;
            for (int j = 0; j < mChoicedDatas.Count; ++j)
            {
                if (mChoicedDatas[j].Guid == slotData.Guid)
                {
                    findIdx = j;
                    break;
                }
            }

            if (findIdx != -1)
            {
                mChoicedDatas.RemoveAt(findIdx);
                continue;
            }

            FNDebug.LogFormat($"Request Cancel Inlay [{slotData.SlotID}][<color=#00ff00>{slotData.Guid}</color>]");
            var data = mCachedValues.Append();
            data.guid = slotData.Guid;
            data.slotIdx = i;
            slotData.Guid = 0;

            //取消镶嵌
            Net.CSTujianInlayMessage(data.guid,0);
        }

        //设置选中的数据
        for (int i = 0; i < mHandBookSlotDatas.Count && mChoicedDatas.Count > 0; ++i)
        {
            if(mHandBookSlotDatas[i].Opened && mHandBookSlotDatas[i].Guid == 0)
            {
                var choicedData = mChoicedDatas[0];
                mChoicedDatas.RemoveAt(0);
                FNDebug.LogFormat($"Request Inlay [{mHandBookSlotDatas[i].SlotID}][<color=#00ff00>{choicedData.Guid}</color>]");
                //设置镶嵌
                Net.CSTujianInlayMessage(choicedData.Guid, mHandBookSlotDatas[i].SlotID);
            }
        }
        
        //恢复本地数据
        for (int i = 0; i < mCachedValues.Count; ++i)
        {
            mHandBookSlotDatas[mCachedValues[i].slotIdx].Guid = mCachedValues[i].guid;
        }
    }

    public void InlayHandBookOnEmptySlot(long guid)
    {
        bool hasInlayed = false;
        for (int i = 0; i < mHandBookSlotDatas.Count; ++i)
        {
            var slotData = mHandBookSlotDatas[i];
            if (slotData.Guid == guid && guid > 0)
            {
                hasInlayed = true;
                break;
            }
        }

        if (hasInlayed)
        {
            FNDebug.LogFormat($"<color=#00ff00>[设置镶嵌]:[{guid}]已经镶嵌，无须重复镶嵌</color>]");
            return;
        }

        for (int i = 0; i < mHandBookSlotDatas.Count; ++i)
        {
            var slotData = mHandBookSlotDatas[i];
            if (slotData.Guid == 0 && slotData.Opened)
            {
                FNDebug.LogFormat($"<color=#00ff00>[设置镶嵌]:[{guid}]</color>]");
                //设置镶嵌
                Net.CSTujianInlayMessage(guid,slotData.SlotID);
                break;
            }
        }
    }

    protected HandBookSlotData OnCreate()
    {
        var ret = mPoolHandle.GetSystemClass<HandBookSlotData>();
        return ret;
    }

    protected void OnRecycle(HandBookSlotData data)
    {
        data.Reset();
        mPoolHandle.Recycle(data);
    }

    protected HandBookGroupItemData OnCreateGroupData()
    {
        var ret = mPoolHandle.GetSystemClass<HandBookGroupItemData>();
        return ret;
    }

    protected void OnRecycleGroupData(HandBookGroupItemData data)
    {
        data.Reset();
        mPoolHandle.Recycle(data);
    }

    protected SlotBindData OnCreateSlotBindData()
    {
        var ret = mPoolHandle.GetSystemClass<SlotBindData>();
        return ret;
    }

    protected void OnRecycleSlotBindData(SlotBindData data)
    {
        data.Reset();
        mPoolHandle.Recycle(data);
    }

    public CSHandBookManager()
    {
        mHandBookSlotDatas = new FastArrayElementFromPool<HandBookSlotData>(32,OnCreate,OnRecycle);
        mCachedEmptyDatas = new FastArrayElementFromPool<HandBookSlotData>(16,OnCreate, OnRecycle);
        mGroupDatasForSetuped = new FastArrayElementFromPool<HandBookGroupItemData>(64, OnCreateGroupData, OnRecycleGroupData);
        mOwnedGroupDatas = new FastArrayElementFromPool<HandBookGroupItemData>(64, OnCreateGroupData, OnRecycleGroupData);
        mGroupDatasForChoicedPreview = new FastArrayElementFromPool<HandBookGroupItemData>(64, OnCreateGroupData, OnRecycleGroupData);
        mCachedValues = new FastArrayElementFromPool<SlotBindData>(32, OnCreateSlotBindData, OnRecycleSlotBindData);
        mQuality2Count = new Dictionary<int, int>(32);
        mCounterForChoiced.Add(HandBookOpMode.HBOM_CAMP, new Dictionary<int, Dictionary<int, int>>(8));
        mCounterForChoiced.Add(HandBookOpMode.HBOM_POSITION, new Dictionary<int, Dictionary<int, int>>(8));
        mCounterForChoiced.Add(HandBookOpMode.HBOM_MAP, new Dictionary<int, Dictionary<int, int>>(8));
        mCounterForSetuped.Add(HandBookOpMode.HBOM_CAMP, new Dictionary<int, Dictionary<int, int>>(8));
        mCounterForSetuped.Add(HandBookOpMode.HBOM_POSITION, new Dictionary<int, Dictionary<int, int>>(8));
        mCounterForSetuped.Add(HandBookOpMode.HBOM_MAP, new Dictionary<int, Dictionary<int, int>>(8));
        mCountersForOwned.Add(HandBookOpMode.HBOM_CAMP, new Dictionary<int, Dictionary<int, int>>(8));
        mCountersForOwned.Add(HandBookOpMode.HBOM_POSITION, new Dictionary<int, Dictionary<int, int>>(8));
        mCountersForOwned.Add(HandBookOpMode.HBOM_MAP, new Dictionary<int, Dictionary<int, int>>(8));
        mHandBookMarkDatas = new FastArrayElementFromPool<HandBookSlotData>(128, OnCreate, OnRecycle);
        mCachedBookMarkDatas = new FastArrayElementFromPool<HandBookSlotData>(32, OnCreate, OnRecycle);
        InitSuitNames();
        InitSuitDatas();
        RebuildSetupedHandBookGroupDatas();
    }

    IEnumerator InitHandBookMark()
    {
        mHandBookMarkDatas.Clear();
        var handles = HandBookTableManager.Instance.array.gItem.handles;
        for (int i = 0,max = handles.Length; i < max;++i)
        {
            var handBook = handles[i].Value as TABLE.HANDBOOK;
            if(null != handBook && handBook.bookMarkScore > 0)
            {
                var handBookData = mHandBookMarkDatas.Append();
                handBookData.SetupHandBook(handBook);
                handBookData.AddFlag(HandBookSlotData.CardFlag.CF_ENABLE_CLICKED);
            }
        }
        yield return null;
        SortHandBookMarks(mHandBookMarkDatas);
        yield return null;
    }

    public FastArrayElementKeepHandle<HandBookSlotData> HandBookMarkDatas
    {
        get
        {
            return mExpressMarkDatas;
        }
    }

    public IEnumerator GetHandBookMarkDatas(int campId = 0, int zoneId = 0, int positionId = 0,int minValue = 21)
    {
        if (mHandBookMarkDatas.Count == 0)
        {
            yield return InitHandBookMark();
        }
        var bookMarkDatas = mHandBookMarkDatas;
        mExpressMarkDatas.Clear();
        for(int i = 0,max = bookMarkDatas.Count;i < max;++i)
        {
            var handBookData = bookMarkDatas[i];
            if (null == handBookData || null == handBookData.HandBook)
            {
                continue;
            }
            var handBook = handBookData.HandBook;
            if (handBook.camp != campId && campId != 0)
            {
                continue;
            }

            if (handBook.sitemap != zoneId && zoneId != 0)
            {
                continue;
            }

            if (handBook.status != positionId && positionId != 0)
            {
                continue;
            }

            mExpressMarkDatas.Append(handBookData);
        }

        mCachedBookMarkDatas.Clear();
        for (int i = mExpressMarkDatas.Count; i < minValue; ++i)
        {
            mExpressMarkDatas.Append(mCachedBookMarkDatas.Append());
        }
        yield return null;
    }

    void SortHandBookMarks(FastArrayElementFromPool<HandBookSlotData> datas)
    {
        var datasList = SortHelper.GetComparersList(32);
        datasList.AddCompare(SortHelper.IntLess, 0);

        var handles = SortHelper.GetSortHandle(datas.Count);
        SortHelper.SortHandle handle = null;
        HandBookSlotData data = null;
        for (int i = 0, max = datas.Count; i < max; ++i)
        {
            handle = handles[i];
            data = datas[i];
            handle.handle = data;
            handle.intValue[0] = data.HandBook.bookMarkScore;
        }

        SortHelper.Sort(handles, datasList);

        for (int i = 0, max = datas.Count; i < max; ++i)
        {
            datas[i] = handles[i].handle as HandBookSlotData;
        }

        handles.OnRecycle();
        datasList.OnRecycle();
    }

    public int GetHandBookCountByMergeKey(int mergeKey)
    {
        if (mQuality2Count.ContainsKey(mergeKey))
            return mQuality2Count[mergeKey];
        return 0;
    }

    protected void AddCount(HandBookSlotData handBook)
    {
        if(null != handBook && null != handBook.HandBook)
        {
            int key = handBook.maked_key;
            if (mQuality2Count.ContainsKey(key))
            {
                mQuality2Count[key] += 1;
            }
            else
            {
                mQuality2Count.Add(key, 1);
            }
        }
    }

    protected void RemoveCount(HandBookSlotData handBook)
    {
        if (null != handBook && null != handBook.HandBook)
        {
            int key = handBook.maked_key;
            if (mQuality2Count.ContainsKey(key))
            {
                mQuality2Count[key] -= 1;
            }
        }
    }

    void InitSlotDatas()
    {
        var arr = HandBookSlotTableManager.Instance.array.gItem.handles;
        for(int k = 0,max = arr.Length;k < max;++k)
        {
            mHandBookSlotDatas.Count = mHandBookSlotDatas.Count + 1;
            var value = mHandBookSlotDatas[mHandBookSlotDatas.Count - 1];
            value.SlotID = arr[k].key;
            value.HandBookId = 0;
            value.AddFlag(HandBookSlotData.CardFlag.CF_LOCKED);
            value.AddFlag(HandBookSlotData.CardFlag.CF_SETUP_MODE);
            value.AddFlag(HandBookSlotData.CardFlag.CF_ENABLE_CLICKED);
        }
    }

    public void ResetSlotInfos(long slotId,RepeatedField<tujian.TujianInfo> books)
    {
        if(mHandBookSlotDatas.Count == 0)
        {
            InitSlotDatas();
        }

        for(int i = 0; i < mHandBookSlotDatas.Count; ++i)
        {
            var data = mHandBookSlotDatas[i];
            data.Opened = ((slotId >> (data.SlotID - 1)) & 1) == 1;
        }

        mQuality2Count.Clear();
        mOwnedBookDatas.Clear();
        for (int i = 0; i < books.Count; ++i)
        {
            var book = books[i];
            HandBookSlotData handBookData = null;
            if (mOwnedBookDatas.ContainsKey(book.id))
                continue;
            handBookData = mPoolHandle.GetSystemClass<HandBookSlotData>();
            mOwnedBookDatas.Add(book.id, handBookData);
            handBookData.Reset();
            handBookData.Bind = book.bind == 1;
            handBookData.HandBookId = book.handBookId;
            handBookData.SlotID = book.slotId;
            handBookData.Guid = book.id;
            handBookData.AddFlag(HandBookSlotData.CardFlag.CF_ENABLE_CLICKED);
            AddCount(handBookData);
            AddCounterForOwned(handBookData.HandBook);
            BindBookLinkedSlot(book,false);
        }
        RebuildSetupedHandBookGroupDatas();
    }

    void InitSuitDatas()
    {
        var arr = HandBookSuitTableManager.Instance.array.gItem.handles;
        mHandBookSuits = new Dictionary<int, FastArrayElementKeepHandle<TABLE.HANDBOOKSUIT>>(arr.Length);
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.HANDBOOKSUIT;
            FastArrayElementKeepHandle<TABLE.HANDBOOKSUIT> group = null;
            if (!mHandBookSuits.ContainsKey(item.Group))
            {
                group = new FastArrayElementKeepHandle<TABLE.HANDBOOKSUIT>(8);
                mHandBookSuits.Add(item.Group, group);
            }
            else
            {
                group = mHandBookSuits[item.Group];
            }
            group.Append(item);
        }
        var itGroup = mHandBookSuits.GetEnumerator();
        while(itGroup.MoveNext())
        {
            itGroup.Current.Value.Sort(HandBookSuitComparer);
        }
    }

    void HandBookSuitComparer(ref long sortValue, TABLE.HANDBOOKSUIT r)
    {
        sortValue = ((long)r.Lv) << 32;
        sortValue += r.id;
    }

    void AddCounter(Dictionary<HandBookOpMode, Dictionary<int, Dictionary<int, int>>>  counter,HandBookOpMode opMode,int opValue,TABLE.HANDBOOK handBook)
    {
        Dictionary<int, int> opDic = null;
        if (!counter[opMode].TryGetValue(opValue,out opDic))
        {
            opDic = new Dictionary<int, int>(16);
            counter[opMode].Add(opValue, opDic);
        }

        if(!opDic.ContainsKey(handBook.itemID))
        {
            opDic[handBook.itemID] = 1;
        }
        else
        {
            opDic[handBook.itemID] += 1;
        }
    }

    void RemoveCounter(Dictionary<HandBookOpMode, Dictionary<int, Dictionary<int, int>>> counter, HandBookOpMode opMode, int opValue, TABLE.HANDBOOK handBook)
    {
        Dictionary<int, int> opDic = null;
        if (!counter[opMode].TryGetValue(opValue, out opDic))
        {
            return;
        }

        if (!opDic.ContainsKey(handBook.itemID))
        {
            return;
        }

        opDic[handBook.itemID] -= 1;
        if (opDic[handBook.itemID] <= 0)
            opDic.Remove(handBook.itemID);
    }

    public void AddCounterForOwned(TABLE.HANDBOOK handBook)
    {
        if (null != handBook)
        {
            AddCounter(mCountersForOwned, HandBookOpMode.HBOM_CAMP, handBook.camp, handBook);
            AddCounter(mCountersForOwned, HandBookOpMode.HBOM_MAP, handBook.sitemap, handBook);
            AddCounter(mCountersForOwned, HandBookOpMode.HBOM_POSITION, handBook.status, handBook);
            mCountersForOwnedDirty = true;
        }
    }

    public void RemoveCounterForOwned(TABLE.HANDBOOK handBook)
    {
        if (null != handBook)
        {
            RemoveCounter(mCountersForOwned, HandBookOpMode.HBOM_CAMP, handBook.camp, handBook);
            RemoveCounter(mCountersForOwned, HandBookOpMode.HBOM_MAP, handBook.sitemap, handBook);
            RemoveCounter(mCountersForOwned, HandBookOpMode.HBOM_POSITION, handBook.status, handBook);

            mCountersForOwnedDirty = true;
        }
    }

    public void AddCounterForChoiced(TABLE.HANDBOOK handBook)
    {
        if (null != handBook)
        {
            AddCounter(mCounterForChoiced, HandBookOpMode.HBOM_CAMP, handBook.camp,handBook);
            AddCounter(mCounterForChoiced, HandBookOpMode.HBOM_MAP, handBook.sitemap, handBook);
            AddCounter(mCounterForChoiced, HandBookOpMode.HBOM_POSITION, handBook.status, handBook);
        }
    }

    public void RemoveCounterForChoiced(TABLE.HANDBOOK handBook)
    {
        if (null != handBook)
        {
            RemoveCounter(mCounterForChoiced, HandBookOpMode.HBOM_CAMP, handBook.camp, handBook);
            RemoveCounter(mCounterForChoiced, HandBookOpMode.HBOM_MAP, handBook.sitemap, handBook);
            RemoveCounter(mCounterForChoiced, HandBookOpMode.HBOM_POSITION, handBook.status, handBook);
        }
    }

    public void AddCounterForSetuped(TABLE.HANDBOOK handBook,bool markDirty)
    {
        if(null != handBook)
        {
            AddCounter(mCounterForSetuped, HandBookOpMode.HBOM_CAMP, handBook.camp, handBook);
            AddCounter(mCounterForSetuped, HandBookOpMode.HBOM_MAP, handBook.sitemap, handBook);
            AddCounter(mCounterForSetuped, HandBookOpMode.HBOM_POSITION, handBook.status, handBook);

            if (markDirty)
            {
                RebuildSetupedHandBookGroupDatas();
                HotManager.Instance.EventHandler.SendEvent(CEvent.OnHandBookInlayChanged);
            }
        }
    }
    public void RemoveCounterForSetuped(TABLE.HANDBOOK handBook)
    {
        if (null != handBook)
        {
            RemoveCounter(mCounterForSetuped, HandBookOpMode.HBOM_CAMP, handBook.camp, handBook);
            RemoveCounter(mCounterForSetuped, HandBookOpMode.HBOM_MAP, handBook.sitemap, handBook);
            RemoveCounter(mCounterForSetuped, HandBookOpMode.HBOM_POSITION, handBook.status, handBook);
        }
    }

    protected void BindBookLinkedSlot(tujian.TujianInfo book,bool markDirty)
    {
        if (null != book && book.slotId > 0 && book.slotId <= mHandBookSlotDatas.Count)
        {
            mHandBookSlotDatas[book.slotId - 1].HandBookId = book.handBookId;
            mHandBookSlotDatas[book.slotId - 1].Guid = book.id;
            AddCounterForSetuped(mHandBookSlotDatas[book.slotId - 1].HandBook, markDirty);
        }
    }

    protected void UnbindBookLinkedSlot(tujian.TujianInfo book)
    {
        if (null != book && book.slotId > 0 && book.slotId <= mHandBookSlotDatas.Count)
        {
            RemoveCounterForSetuped(mHandBookSlotDatas[book.slotId - 1].HandBook);
            mHandBookSlotDatas[book.slotId - 1].HandBookId = 0;
            mHandBookSlotDatas[book.slotId - 1].Guid = 0;

            RebuildSetupedHandBookGroupDatas();
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnHandBookInlayChanged);
        }
    }

    public void UpgradeHandBook(tujian.TujianInfo book)
    {
        if(null != book)
        {
            HandBookSlotData handBookData = null;
            if (!mOwnedBookDatas.ContainsKey(book.id))
            {
                return;
            }
            handBookData = mOwnedBookDatas[book.id];
            handBookData.Bind = book.bind == 1;
            RemoveCount(handBookData);
            handBookData.HandBookId = book.handBookId;
            AddCount(handBookData);
            for(int i = 0; i < mHandBookSlotDatas.Count; ++i)
            {
                if(mHandBookSlotDatas[i].Guid == book.id)
                {
                    mHandBookSlotDatas[i].HandBookId = book.handBookId;
                    HotManager.Instance.EventHandler.SendEvent(CEvent.OnHandBookInlayChanged);
                    break;
                }
            }
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnHandBookUpgradeSucceed, book.id);
        }
    }

    public void InlayHandBook(tujian.TujianInfo book)
    {
        if (null != book)
        {
            HandBookSlotData handBookData = null;
            if (!mOwnedBookDatas.ContainsKey(book.id))
            {
                return;
            }
            handBookData = mOwnedBookDatas[book.id];
            handBookData.Bind = book.bind == 1;

            //取消镶嵌
            if (book.slotId == 0)
            {
                UtilityTips.ShowRedTips(736, handBookData.HandBook.ItemID.ItemName());
                for (int i = 0; i < mHandBookSlotDatas.Count; ++i)
                {
                    var slotData = mHandBookSlotDatas[i];
                    if(slotData.Guid == book.id)
                    {
                        FNDebug.LogFormat($"Cancel Inlay Succeed [{book.slotId}][<color=#00ff00>{book.id}</color>]");
                        handBookData.SlotID = 0;
                        book.slotId = slotData.SlotID;
                        UnbindBookLinkedSlot(book);
                        break;
                    }
                }
            }
            else
            {
                UtilityTips.ShowGreenTips(737, handBookData.HandBook.ItemID.ItemName());
                FNDebug.LogFormat($"Inlay Succeed [{book.slotId}][<color=#00ff00>{book.id}</color>]");
                handBookData.SlotID = book.slotId;
                BindBookLinkedSlot(book,true);
            }
        }
    }

    public void UpdateSlotInfo(long slotId)
    {
        for (int i = 0; i < mHandBookSlotDatas.Count; ++i)
        {
            var data = mHandBookSlotDatas[i];
            bool orgOpend = data.Opened;
            data.Opened = ((slotId >> (data.SlotID - 1)) & 1) == 1;
            if(!data.Opened)
            {
                data.HandBookId = 0;
            }
            else
            {
                if(!orgOpend)
                {
                    data.AddFlag(HandBookSlotData.CardFlag.CF_UNLOCK_EFFECT);
                }
            }
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnHandBookSlotChanged);
    }

    public void AddHandBook(tujian.TujianInfo book)
    {
        if (mOwnedBookDatas.ContainsKey(book.id))
        {
            return;
        }
        HandBookSlotData handBookData = mPoolHandle.GetSystemClass<HandBookSlotData>();
        handBookData.Reset();
        handBookData.HandBookId = book.handBookId;
        handBookData.Guid = book.id;
        handBookData.Bind = book.bind == 1;
        handBookData.AddFlag(HandBookSlotData.CardFlag.CF_ENABLE_CLICKED);

        UtilityTips.ShowGreenTips(1980, handBookData.HandBook.ItemID.ItemName());

        mOwnedBookDatas.Add(book.id,handBookData);
        AddCount(handBookData);
        AddCounterForOwned(handBookData.HandBook);
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnHandBookAdded, book.id);
    }

    public void RemoveHandBook(tujian.TujianInfo book)
    {
        if (null != book)
        {
            HandBookSlotData handBookData = null;
            if (!mOwnedBookDatas.ContainsKey(book.id))
            {
                return;
            }
            handBookData = mOwnedBookDatas[book.id];
            RemoveCount(handBookData);
            RemoveCounterForOwned(handBookData.HandBook);
            UnbindBookLinkedSlot(book);
            handBookData.Reset();
            mPoolHandle.Recycle(handBookData);
            mOwnedBookDatas.Remove(book.id);
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnHandBookRemoved, book.id);
        }
    }

    public Dictionary<long, HandBookSlotData> GetHandbookData()
    {
        return mOwnedBookDatas;
    }
    public override void Dispose()
    {
        mQuality2Count?.Clear();
        mQuality2Count = null;
        mExpressData?.Clear();
        mExpressData = null;
        mChoicedDatas?.Clear();
        mChoicedDatas = null;
        mCachedEmptyDatas?.Clear();
        mCachedEmptyDatas = null;
        mGroupDatasForSetuped?.Clear();
        mGroupDatasForSetuped = null;
        mHandBookSuits?.Clear();
        mHandBookSuits = null;
        mOwnedBookDatas.Clear();
        mOwnedBookDatas = null;
        mHandBookSlotDatas.Clear();
        mHandBookSlotDatas = null;
        mPoolHandle?.OnDestroy();
        mPoolHandle = null;
    }
}
