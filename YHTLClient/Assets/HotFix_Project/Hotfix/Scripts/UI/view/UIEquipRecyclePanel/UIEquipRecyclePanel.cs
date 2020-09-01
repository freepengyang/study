using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemQualityFilterType
{
    IQFT_WHITE = 1,
    IQFT_GREEN = 2,
    IQFT_BLUE = 3,
    IQFT_PURPLE = 4,
    IQFT_ORANGE = 5,
    IQFT_RED = 6,
}

public enum ItemLevelLimit
{
    ILL_NONE = 10000,
    ILL_45 = 45,
    ILL_80 = 80,
    ILL_105 = 105,
    ILL_120 = 120,
    ILL_UNLIMITED = 9999,
}

public enum JobType
{
    JT_NONE = 0,
    JT_Warrior = 1,
    JT_Mage = 2,
    JT_Taoist = 3,
    JT_ALL_VALUE = (1 << 1) | (1 << 2) | (1 << 3),
    JT_Warrior_VALUE = (1 << 1),
    JT_Mage_VALUE = (1 << 2),
    JT_Taoist_VALUE = (1 << 3),
}

public class SuitItemData
{
    public uint JobId
    {
        get
        {
            return (uint)eJobType;
        }
    }
    public JobType eJobType;
    public TABLE.ZHANHUNSUIT suitItem;
    public int relationCount;
    public PoolHandleManager poolHandle;
    public void OnDestroy()
    {
        suitItem = null;
        poolHandle = null;
    }
}

public class EquipRecycleSuitItem : UIBinder
{
    protected UISprite icon;
    protected UILabel label;
    protected SuitItemData mData;

    public override void Init(UIEventListener handle)
    {
        icon = Get<UISprite>("sp_icon");
        label = Get<UILabel>("lb_count");
        Handle.onClick = OnClick;
    }

    public void OnClick(GameObject go)
    {
        if(null != mData)
        {
            //Debug.LogFormat("回收点击=>{0}", mData.suitItem.name);
            var equipItems = mData.poolHandle.GetSystemClass<List<bag.BagItemInfo>>();
            var iter = CSBagInfo.Instance.GetBagItemData().GetEnumerator();
            while (iter.MoveNext())
            {
                var current = iter.Current.Value;
                var itemCfg = ItemTableManager.Instance.GetItemCfg(current.configId);
                if (null == itemCfg)
                {
                    continue;
                }

                if(mData.suitItem.id != itemCfg.zhanHunSuit)
                {
                    continue;
                }

                if(itemCfg.career != 0 && mData.JobId != itemCfg.career)
                {
                    continue;
                }

                if (!CSItemRecycleInfo.Instance.CanAsNeigongRecycle(itemCfg))
                {
                    continue;
                }

                var suitCfg = ZhanHunSuitTableManager.Instance.GetSuitCfg((int)itemCfg.zhanHunSuit);
                equipItems.Add(current);
            }

            if(equipItems.Count <= 0)
            {
                mData.poolHandle.Recycle(equipItems);
                UtilityTips.ShowRedTips(735);
                return;
            }

            UIManager.Instance.CreatePanel<UIRecycleConfirmPanel>(f =>
            {
                (f as UIRecycleConfirmPanel).BindData(equipItems);
                equipItems.Clear();
                mData.poolHandle.Recycle(equipItems);
            });
        }
    }

    public override void Bind(object data)
    {
        mData = data as SuitItemData;
        var colorStr = UtilityColor.GetColorString(mData.relationCount <= 0 ? ColorType.WeakText : ColorType.Green);
        if (mData.relationCount <= 0)
            label.text = $"{colorStr}{mData.suitItem.name}({mData.relationCount})";
        else
            label.text = $"{colorStr}{mData.suitItem.name}({mData.relationCount})";
    }

    public override void OnDestroy()
    {
        Handle.onClick = null;
        icon = null;
        label = null;
        if(null != mData)
        {
            mData.OnDestroy();
            mData = null;
        }
    }
}

public class UIEquipRecyclePanel
{
    static ItemQualityFilterType[] ms_item_quality_values = new ItemQualityFilterType[]
    {
        ItemQualityFilterType.IQFT_WHITE,ItemQualityFilterType.IQFT_GREEN,ItemQualityFilterType.IQFT_BLUE,ItemQualityFilterType.IQFT_PURPLE
    };
    static Dictionary<int, ItemQualityFilterType> ms_item_quality_key2value = new Dictionary<int, ItemQualityFilterType>
    {
        {1291,ms_item_quality_values[0]},
        {1292,ms_item_quality_values[1]},
        {1293,ms_item_quality_values[2]},
        {1294,ms_item_quality_values[3]},
    };
    static readonly string ms_mode_neigong = @"neigong";
    static readonly string ms_mode_normal = @"normal";
    static ItemLevelLimit[] ms_item_level_values = new ItemLevelLimit[]
    {
        ItemLevelLimit.ILL_NONE,ItemLevelLimit.ILL_45,ItemLevelLimit.ILL_80, ItemLevelLimit.ILL_105, ItemLevelLimit.ILL_120,ItemLevelLimit.ILL_UNLIMITED,
    };
    static Dictionary<int, ItemLevelLimit> ms_item_level_key2value = new Dictionary<int, ItemLevelLimit>
    {
        {67,ms_item_level_values[0]},
        {68,ms_item_level_values[1]},
        {69,ms_item_level_values[2]},
        {70,ms_item_level_values[3]},
        {71,ms_item_level_values[4]},
        {72,ms_item_level_values[5]},
    };

    public int GetItemQualityFilter()
    {
        int idx = PlayerPrefs.GetInt("ItemQualityFilterType", 0);
        idx = Mathf.Clamp(idx, 0, ms_item_quality_key2value.Count - 1);
        return idx;
    }

    public void SetItemQualityFilter(int idx)
    {
        PlayerPrefs.SetInt("ItemQualityFilterType", idx);
    }

    public int GetItemLevelFilter()
    {
        int idx = PlayerPrefs.GetInt("ItemLevelLimit",1);
        idx = Mathf.Clamp(idx, 0, ms_item_quality_key2value.Count - 1);
        return idx;
    }

    public void SetItemLevelFilter(int idx)
    {
        PlayerPrefs.SetInt("ItemLevelLimit", idx);
    }

    public UIEquipRecyclePanel(GameObject handle,PoolHandleManager poolHandleManager)
    {
        Handle = handle;
        mPoolHandleManager = poolHandleManager;
    }

    public GameObject Handle
    {
        get; private set;
    }

    protected PoolHandleManager mPoolHandleManager;

    protected ScriptBinder ScriptBinder { get; set; }

    public T Get<T>(Transform parent, string path) where T : UnityEngine.Object
    {
        Transform objTrans = Get(parent, path);

        if (typeof(T) == typeof(Transform)) return objTrans as T;

        if (typeof(T) == typeof(GameObject)) return objTrans.gameObject as T;

        if (objTrans)
            return objTrans.gameObject.GetComponent<T>();
        return null;
    }

    public Transform Get(Transform parent, string _path)
    {
        if (parent == null)
        {
            if (Handle)
            {
                return Handle.transform.Find(_path);
            }
            else
                return null;
        }
        else
            return parent.Find(_path);
    }

    UIEventListener mbtn_NormalEquip;
    UIEventListener mbtn_NeiGong;
    UIEventListener mbtn_Quesition;
    UIEventListener mbtn_Recycle;
    UILabel mSelectedCount;
    UnityEngine.GameObject mQualityFilterHandle;
    UnityEngine.GameObject mLevelFilterHandle;
    UIGridContainer mRecycleAwards;
    UIEventListener mRecycleAll;
    UIGridContainer mGridSuit;
    UIGrid mGirdNormalEquips;
    UISprite mSpNormalSel;
    UISprite mSpNeigongSel;
    UIScrollView mNormalScrollView;
    protected void _InitScriptBinder()
    {
        mbtn_NormalEquip = ScriptBinder.GetObject("btn_NormalEquip") as UIEventListener;
        mbtn_NeiGong = ScriptBinder.GetObject("btn_NeiGong") as UIEventListener;
        mbtn_Quesition = ScriptBinder.GetObject("btn_Quesition") as UIEventListener;
        mbtn_Recycle = ScriptBinder.GetObject("btn_Recycle") as UIEventListener;
        mSelectedCount = ScriptBinder.GetObject("SelectedCount") as UILabel;
        mQualityFilterHandle = ScriptBinder.GetObject("QualityFilterHandle") as UnityEngine.GameObject;
        mLevelFilterHandle = ScriptBinder.GetObject("LevelFilterHandle") as UnityEngine.GameObject;
        mRecycleAwards = ScriptBinder.GetObject("RecycleAwards") as UIGridContainer;
        mRecycleAll = ScriptBinder.GetObject("RecycleAll") as UIEventListener;
        mGridSuit = ScriptBinder.GetObject("GridSuit") as UIGridContainer;
        mGirdNormalEquips = ScriptBinder.GetObject("GirdNormalEquips") as UIGrid;
        mSpNormalSel = ScriptBinder.GetObject("SpNormalSel") as UISprite;
        mSpNeigongSel = ScriptBinder.GetObject("SpNeigongSel") as UISprite;
        mNormalScrollView = ScriptBinder.GetObject("NormalScrollView") as UIScrollView;
    }

    CSPopList mQualityFilter;
    CSPopList mLevelFilter;
    protected int maxQuality = 1;
    protected int maxLevel = 1;
    protected EventHanlderManager mClientEvent;
    protected int maxPerLine = 0;
    protected const int minLine = 3;
    protected readonly int minUiItemsCount = 15;
    protected FastArrayElementFromPool<UIItemBase> mUiItems;
    protected FastArrayElementKeepHandle<bag.BagItemInfo> mItemDatas;
    protected Dictionary<long, UIItemBase> mItemDatasDic;
    protected List<SuitItemData> mSuitItemDatas;

    public void Init()
    {
        ScriptBinder = Handle.GetComponent<ScriptBinder>();
        _InitScriptBinder();
        maxPerLine = mGirdNormalEquips.maxPerLine;
        mQualityFilter = new CSPopList(mQualityFilterHandle,mPoolHandleManager);
        mLevelFilter = new CSPopList(mLevelFilterHandle,mPoolHandleManager);
        mbtn_NormalEquip.onClick = OnNormalClicked;// OnGMCmdAddEquip;
        mbtn_Quesition.onClick = OnOpenQuesitionPanel;
        mbtn_NeiGong.onClick = OnOpenNeiGongPanel;
        mRecycleAll.onClick = OnRecycleAll;
        mbtn_Recycle.onClick = OnNormalRecycle;
        mUiItems = new FastArrayElementFromPool<UIItemBase>(64, () =>
        {
            return UIItemManager.Instance.GetItem(PropItemType.Bag, mGirdNormalEquips.transform);
        },
        f =>
        {
            UIItemManager.Instance.RecycleSingleItem(f);
        });
        mItemDatas = new FastArrayElementKeepHandle<bag.BagItemInfo>(64);
        mItemDatasDic = new Dictionary<long, UIItemBase>(64);
        mUiItems.Clear();
        mItemDatas.Clear();
        mItemDatasDic.Clear();
        mSuitItemDatas = mPoolHandleManager.GetSystemClass<List<SuitItemData>>();
        if (null == mClientEvent)
        {
            mClientEvent = new EventHanlderManager(EventHanlderManager.DispatchType.Event);
            mClientEvent.AddEvent(CEvent.ItemListChange, OnRecycleItemChanged);
            mClientEvent.AddEvent(CEvent.BagItemDBClicked, OnBagItemDBClicked);
            mClientEvent.AddEvent(CEvent.TipsBtnRecycleUnSelectd, OnRecycleCancelSelected);
        }

        int[] datas = new int[] { 1291, 1292, 1293, 1294 };
        mQualityFilter.MaxCount = datas.Length;
        for (int i = 0; i < datas.Length; ++i)
        {
            mQualityFilter.mDatas[i].idxValue = datas[i];
            mQualityFilter.mDatas[i].value = ClientTipsTableManager.Instance.GetClientTipsContext(datas[i]);
        }
        mQualityFilter.InitList(OnQualityFilterChanged);

        datas = new int[] { 67, 68, 69, 70, 71, 72 };
        mLevelFilter.MaxCount = datas.Length;
        for (int i = 0; i < datas.Length; ++i)
        {
            mLevelFilter.mDatas[i].idxValue = datas[i];
            mLevelFilter.mDatas[i].value = ClientTipsTableManager.Instance.GetClientTipsContext(datas[i]);
        }
        mLevelFilter.InitList(OnLevelFilterChanged);

        int qualityIdx = GetItemQualityFilter();
        maxQuality = (int)ms_item_quality_values[qualityIdx];
        mQualityFilter.SetCurValue(qualityIdx, false);

        int levelIdx = GetItemLevelFilter();
        maxLevel = (int)ms_item_level_values[levelIdx];
        mLevelFilter.SetCurValue(levelIdx, false);
    }

    public void Hide()
    {
        if (RecycleNormalEquips())
        {
            mClientEvent.SendEvent(CEvent.OnRecycleSelectedRefresh);
        }
        AdjustScrollViewPos();
        Handle?.SetActive(false);
    }

    protected void AdjustScrollViewPos()
    {
        int line = (mUiItems.Count - 1) / maxPerLine + 1;
        if (line > minLine)
        {
            int ypos = (line - minLine) * 80 + 10;
            SpringPanel.Begin(mNormalScrollView.gameObject, new Vector3(mNormalScrollView.transform.localPosition.x, ypos, mNormalScrollView.transform.localPosition.z), 10);
        }
        else
        {
            mNormalScrollView.ResetPosition();
        }
    }

    protected void OnRecycleItemChanged(uint eventId, object args)
    {
        //Debug.LogFormat("OnRecycleItemChanged");
        if (Mode == RecycleMode.M_NORMAL)
        {
            //移除数据
            for(int i = 0; i < mItemDatas.Count; ++i)
            {
                var itemInfo = CSBagInfo.Instance.GetBagItemInfoById(mItemDatas[i].id);
                if(null == itemInfo)
                {
                    mItemDatas.SwapErase(i--);
                }
            }
            //移除组件
            for(int i = 0; i < mUiItems.Count; ++i)
            {
                if(null != mUiItems[i] && null != mUiItems[i].infos)
                {
                    var itemInfo = CSBagInfo.Instance.GetBagItemInfoById(mUiItems[i].infos.id);
                    if(null == itemInfo)
                    {
                        mUiItems[i].UnInit();
                    }
                }
            }
            //删除多余的行
            if(TryDeline())
            {
                AdjustScrollViewPos();
            }
            RefreshRecycleItems();
        }
        else
        {
            RefreshSuitInfo();
        }
    }

    protected void OnNormalRecycleItemDBClicked(UIItemBase item)
    {
        if(null != item && null != item.infos)
        {
            for (int i = 0; i < mItemDatas.Count; ++i)
            {
                if(mItemDatas[i] == item.infos)
                {
                    mItemDatas.SwapErase(i);
                    break;
                }
            }

            SelectedItem(item.infos.id, false);
            mClientEvent.SendEvent(CEvent.OnRecycleSelectedRefresh);
            item.UnInit();
            if(TryDeline())
                AdjustScrollViewPos();
            RefreshRecycleItems();
        }
    }
    protected void OnNormalRecycleItemClicked(UIItemBase item)
    {
        UITipsManager.Instance.CreateTips(TipsOpenType.Recycle2Bag, item.itemCfg, item.infos);
    }
    protected bool TryDeline()
    {
        //如果多出了一行向上移
        int line = (mUiItems.Count - 1) / maxPerLine + 1;
        int orgLine = line;
        while (line > minLine)
        {
            int s = maxPerLine * line;
            int e = s - maxPerLine;
            bool allEmpty = true;
            for (int i = e; i < s; ++i)
            {
                if (null != mUiItems[i] && mUiItems[i].infos != null)
                {
                    allEmpty = false;
                    break;
                }
            }
            if (!allEmpty)
            {
                break;
            }
            --line;
        }
        mUiItems.Count = line * maxPerLine;
        return orgLine > line;
    }

    protected void OnBagItemDBClicked(uint eventId, object args)
    {
        if (args is bag.BagItemInfo item)
        {
            if(Mode == RecycleMode.M_NORMAL)
            {
                PushItemToNormalRecycle(item, true);
                RefreshRecycleItems();
            }
            else if(Mode == RecycleMode.M_NEIGONG)
            {
                //CSItemRecycleInfo.Instance.RecycleNeigongEquip(item);
            }
        }
    }

    protected void OnRecycleCancelSelected(uint eventId, object args)
    {
        long id = (long)args;
        //移除数据
        for (int i = 0; i < mItemDatas.Count; ++i)
        {
            if(mItemDatas[i].id == id)
            {
                mItemDatas.SwapErase(i--);
                break;
            }
        }
        //移除组件
        for (int i = 0; i < mUiItems.Count; ++i)
        {
            if (null != mUiItems[i] && null != mUiItems[i].infos && id == mUiItems[i].infos.id)
            {
                SelectedItem(id, false);
                mClientEvent.SendEvent(CEvent.OnRecycleSelectedRefresh);
                mUiItems[i].UnInit();
                break;
            }
        }
        //删除多余的行
        if (TryDeline())
        {
            AdjustScrollViewPos();
        }
        RefreshRecycleItems();
    }

    protected void PushItemToNormalRecycle(bag.BagItemInfo item,bool tips)
    {
        if(null == item)
        {
            return;
        }
        
        for(int i = 0; i < mItemDatas.Count; ++i)
        {
            if(mItemDatas[i].id == item.id)
            {
                //如果物品已经存在
                return;
            }
        }

        TABLE.ITEM itemCfg = null;
        if(!ItemTableManager.Instance.TryGetValue(item.configId,out itemCfg))
        {
            //如果物品表里不存在
            return;
        }

        if(!CSItemRecycleInfo.Instance.CanAsNormalRecycle(itemCfg, tips))
        {
            return;
        }

        //获取空格子索引
        for(int i = 0; i < mUiItems.Count; ++i)
        {
            if(null == mUiItems[i].infos)
            {
                mItemDatas.Append(item);
                SelectedItem(item.id, true);
                mClientEvent.SendEvent(CEvent.OnRecycleSelectedRefresh);
                mUiItems[i].Refresh(item, OnNormalRecycleItemClicked);
                mUiItems[i].SetItemDBClickedCB(OnNormalRecycleItemDBClicked);
                return;
            }
        }

        if(mItemDatas.Count == mUiItems.Count)
        {
            int count = Mathf.Max(minUiItemsCount,mUiItems.Count + maxPerLine);
            int old = mUiItems.Count;
            mUiItems.Count = count;
            
            for (int i = old; i < mUiItems.Count; ++i)
            {
                mUiItems[i].UnInit();
            }
        }

        mItemDatas.Append(item);
        SelectedItem(item.id, true);
        mClientEvent.SendEvent(CEvent.OnRecycleSelectedRefresh);
        int idx = mItemDatas.Count - 1;
        if (idx < mUiItems.Count)
        {
            mUiItems[idx].Refresh(item, OnNormalRecycleItemClicked);
            mUiItems[idx].SetItemDBClickedCB(OnNormalRecycleItemDBClicked);
        }

        AdjustScrollViewPos();
    }

    protected void SelectedItem(long id,bool value)
    {
        //Debug.LogFormat("id = {0}",id);
        if(value)
            mClientEvent.SendEvent(CEvent.OnRecycleItemSelected, id);
        else
            mClientEvent.SendEvent(CEvent.OnRecycleItenUnSelected,id);
    }

    protected void RefreshRecycleItems()
    {
        mGirdNormalEquips.Reposition();
        //if (null != mSelectedCount)
        //{
        //    mSelectedCount.text = CSString.Format(291,mItemDatas.Count);
        //}
        RefreshRecycleAwards();
    }

    protected void RefreshRecycleAwards()
    {
        List<RecycleCollectionData> recycleCollectionDatas = mPoolHandleManager.GetSystemClass<List<RecycleCollectionData>>();
        CSItemRecycleInfo.Instance.BeginAwards();
        for(int i = 0; i < mItemDatas.Count; ++i)
        {
            var itemInfo = mItemDatas[i];
            if (null != itemInfo)
            {
                CSItemRecycleInfo.Instance.GetAwards(itemInfo.configId);
            }
        }
        CSItemRecycleInfo.Instance.EndAwards(recycleCollectionDatas);

        List<ItemBarData> itemBarDatas = mPoolHandleManager.GetSystemClass<List<ItemBarData>>();
        for (int i = 0; i < recycleCollectionDatas.Count; ++i)
        {
            var current = recycleCollectionDatas[i];
            if (null == current || null == current.item)
            {
                continue;
            }

            var itemData = UIItemBarManager.Instance.Get();
            itemData.cfgId = current.item.id;
            itemData.needed = current.count;
            itemData.owned = 0;
            itemData.flag = (int)ItemBarData.ItemBarType.IBT_SMALL_ICON | (int)ItemBarData.ItemBarType.IBT_ONLY_COST;
            itemData.eventHandle = mClientEvent;
            itemBarDatas.Add(itemData);
        }
        CSItemRecycleInfo.Instance.RecycleDatas(recycleCollectionDatas);
        recycleCollectionDatas.Clear();
        mPoolHandleManager.Recycle(recycleCollectionDatas);
        UIItemBarManager.Instance.Bind(mRecycleAwards, itemBarDatas);
        itemBarDatas.Clear();
        mPoolHandleManager.Recycle(itemBarDatas);
    }

    protected void OnOpenQuesitionPanel(GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.EQUIP_RECYCLE);
        //ScriptBinder._SetAction("neigong");
    }

    protected void OnOpenNeiGongPanel(GameObject go)
    {
        if(RecycleNormalEquips())
        {
            mClientEvent.SendEvent(CEvent.OnRecycleSelectedRefresh);
        }
        Mode = RecycleMode.M_NEIGONG;
        RefreshSuitInfo();
    }

    protected void RefreshSuitInfo()
    {
        //job_query_table
        Dictionary<int, int> mExistedSuitSet = mPoolHandleManager.GetSystemClass<Dictionary<int, int>>();
        var iter = CSBagInfo.Instance.GetBagItemData().GetEnumerator();
        while (iter.MoveNext())
        {
            var current = iter.Current.Value;
            var itemCfg = ItemTableManager.Instance.GetItemCfg(current.configId);
            if (null == itemCfg)
            {
                continue;
            }

            if (!CSItemRecycleInfo.Instance.CanAsNeigongRecycle(itemCfg))
            {
                continue;
            }

            var suitCfg = ZhanHunSuitTableManager.Instance.GetSuitCfg((int)itemCfg.zhanHunSuit);
            if (null == suitCfg)
            {
                continue;
            }

            if (!mExistedSuitSet.ContainsKey(suitCfg.id))
            {
                mExistedSuitSet.Add(suitCfg.id, 0);
            }

            int flag = mExistedSuitSet[suitCfg.id];
            int cnt = flag & 0x00FFFFFF;

            if (itemCfg.career == (int)JobType.JT_NONE)
            {
                cnt += 1;
                cnt += (1 << 8);
                cnt += (1 << 16);
            }
            else
            {
                cnt += (1 << 8 * ((int)itemCfg.career - 1));
            }

            flag = cnt & 0x00FFFFFF;

            mExistedSuitSet[suitCfg.id] = flag;
        }

        for (int i = 0; i < mSuitItemDatas.Count; ++i)
        {
            mPoolHandleManager.Recycle(mSuitItemDatas[i]);
        }
        mSuitItemDatas.Clear();
        //var it = ZhanHunSuitTableManager.Instance.dic.Values.GetEnumerator();
        //while(it.MoveNext())
        //{
        //    TABLE.ZHANHUNSUIT v = it.Current;
        //    if (v.recycle == 0)
        //        continue;

        //    int suitValue = 0;
        //    if (mExistedSuitSet.ContainsKey(v.id))
        //    {
        //        suitValue = mExistedSuitSet[v.id];
        //    }

        //    for (int i = (int)JobType.JT_Warrior; i <= (int)JobType.JT_Taoist; ++i)
        //    {
        //        int offset = (i - 1) * 8;
        //        int relationValue = (suitValue & (0xFF << offset)) >> offset;

        //        var suitItemData = mPoolHandleManager.GetSystemClass<SuitItemData>();
        //        suitItemData.eJobType = (JobType)i;
        //        suitItemData.suitItem = v;
        //        suitItemData.relationCount = relationValue;
        //        suitItemData.poolHandle = mPoolHandleManager;
        //        mSuitItemDatas.Add(suitItemData);
        //    }
        //}
        mGridSuit.Bind<SuitItemData, EquipRecycleSuitItem>(mSuitItemDatas, mPoolHandleManager);
        mExistedSuitSet.Clear();
        mPoolHandleManager.Recycle(mExistedSuitSet);
    }

    protected void OnRecycleAll(GameObject go)
    {
        //List<int> bagIndexs = mPoolHandleManager.GetSystemClass<List<int>>();
        var equipItems = mPoolHandleManager.GetSystemClass<List<bag.BagItemInfo>>();
        equipItems.Clear();
        var iter = CSBagInfo.Instance.GetBagItemData().GetEnumerator();
        while (iter.MoveNext())
        {
            var current = iter.Current.Value;
            var itemCfg = ItemTableManager.Instance.GetItemCfg(current.configId);
            if (null == itemCfg)
            {
                continue;
            }

            if (!CSItemRecycleInfo.Instance.CanAsNeigongRecycle(itemCfg))
            {
                continue;
            }

            //只能回收60级别以下的
            //if(itemCfg.level >= 60)
            //{
            //    continue;
            //}

            FNDebug.Log($"加入内功回收的装备{itemCfg.name} Lv={itemCfg.level}");
            equipItems.Add(current);
            //bagIndexs.Add(current.bagIndex);
        }

        if(equipItems.Count <= 0)
        {
            mPoolHandleManager.Recycle(equipItems);
            UtilityTips.ShowRedTips(352);
            return;
        }

        UIManager.Instance.CreatePanel<UIRecycleConfirmPanel>(f =>
        {
            (f as UIRecycleConfirmPanel).BindData(equipItems);
            equipItems.Clear();
            mPoolHandleManager.Recycle(equipItems);
        });
    }

    protected void OnNormalRecycle(GameObject go)
    {
        if(null == mItemDatas || mItemDatas.Count <= 0)
        {
            UtilityTips.ShowRedTips(351);
            return;
        }

        List<int> bagIndexs = mPoolHandleManager.GetSystemClass<List<int>>();
        for (int i = 0; i < mUiItems.Count; ++i)
        {
            if (mUiItems[i].infos == null)
            {
                continue;
            }
            //Debug.Log($"加入普通回收的装备{mUiItems[i].itemCfg.name} Lv={mUiItems[i].itemCfg.level}");
            bagIndexs.Add(mUiItems[i].infos.bagIndex);
        }
        //RecycleNormalEquips();
        Net.ReqCallBackItemMessage(bagIndexs);
        bagIndexs.Clear();
        mPoolHandleManager.Recycle(bagIndexs);
    }

    protected void OnNormalClicked(GameObject go)
    {
        Mode = RecycleMode.M_NORMAL;
        RecycleNormalEquips();
        mQualityFilter.SetCurValue(GetItemQualityFilter(), false);
        mLevelFilter.SetCurValue(GetItemLevelFilter(), true);
    }

    protected void OnGMCmdAddEquip(GameObject go)
    {
        int[] items = new int[]
        {
            520101005,520102005,520103005,520104005,520105005,520106005,520107005,520108005,520109005,520110005,
        };
        //items = new int[]
        //{
        //    210910821,210910841,210910861
        //};
        //int idx = UnityEngine.Random.Range(0, items.Length - 1);
        for(int i = 0; i < items.Length; ++i)
            Net.GMCommand($"@i {items[i]} 1 0");
    }

    protected enum RecycleMode
    {
        M_NONE = -1,
        M_NORMAL = 0,
        M_NEIGONG = 1,
    }

    RecycleMode _mode = RecycleMode.M_NORMAL;
    protected RecycleMode Mode 
    {
        get { return _mode; }
        set
        {
            _mode = value;
            CSItemRecycleInfo.Instance.recycleMode = value == RecycleMode.M_NORMAL ? CSItemRecycleInfo.RecycleMode.RM_NORMAL : 
                ((value == RecycleMode.M_NEIGONG) ? CSItemRecycleInfo.RecycleMode.RM_NEIGONG : CSItemRecycleInfo.RecycleMode.RM_NONE);
            ScriptBinder._SetAction(_mode == RecycleMode.M_NORMAL ? ms_mode_normal : ms_mode_neigong);
            if(null != mSpNormalSel)
                mSpNormalSel.gameObject.SetActive(_mode == RecycleMode.M_NORMAL);
            if (null != mSpNormalSel)
                mSpNeigongSel.gameObject.SetActive(_mode == RecycleMode.M_NEIGONG);
        }
    }

    protected bool RecycleNormalEquips()
    {
        bool ret = false;
        mUiItems.Clear();
        for(int i = 0; i < mItemDatas.Count; ++i)
        {
            SelectedItem(mItemDatas[i].id, false);
            ret = true;
        }
        mItemDatas.Clear();
        return ret;
    }

    public void Show()
    {
        Handle?.SetActive(true);
        Mode = RecycleMode.M_NORMAL;
        int levelIdx = GetItemLevelFilter();
        mLevelFilter.SetCurValue(levelIdx, true);
        mGirdNormalEquips.Reposition();
    }

    protected void OnQualityFilterChanged(CSPopListData current)
    {
        SetItemQualityFilter(current.idxValue - 1291);
        ItemQualityFilterType eQualityFilter = ItemQualityFilterType.IQFT_WHITE;
        if (ms_item_quality_key2value.ContainsKey(mQualityFilter.CurValue.idxValue))
        {
            eQualityFilter = ms_item_quality_key2value[mQualityFilter.CurValue.idxValue];
            maxQuality = (int)eQualityFilter;
            //Debug.LogFormat("OnQualityFilterChanged Value = {0}", maxQuality);
            OnFilterChanged(maxQuality, maxLevel);
        }
    }

    protected void OnLevelFilterChanged(CSPopListData current)
    {
        SetItemLevelFilter(current.idxValue - 67);
        ItemLevelLimit eLevelFilter = ItemLevelLimit.ILL_45;
        if (ms_item_level_key2value.ContainsKey(mLevelFilter.CurValue.idxValue))
        {
            eLevelFilter = ms_item_level_key2value[mLevelFilter.CurValue.idxValue];
            maxLevel = (int)eLevelFilter;
            //Debug.LogFormat("OnLevelFilterChanged Value = {0}", maxLevel);
            OnFilterChanged(maxQuality, maxLevel);
        }
    }

    protected void OnFilterChanged(int qua,int level)
    {
        RecycleNormalEquips();

        var iter = CSBagInfo.Instance.GetBagItemData().GetEnumerator();
        while (iter.MoveNext())
        {
            var current = iter.Current.Value;
            var itemCfg = ItemTableManager.Instance.GetItemCfg(current.configId);
            if (null == itemCfg)
            {
                continue;
            }

            if(level == (int)ItemLevelLimit.ILL_NONE)
            {
                continue;
            }

            if (itemCfg.level > level || iter.Current.Value.quality > qua)
            {
                continue;
            }

            if (!CSItemRecycleInfo.Instance.CanAsNormalRecycle(itemCfg, false))
            {
                continue;
            }

            mItemDatas.Append(current);
            SelectedItem(current.id, true);
            var itemBase = mUiItems.Append();
            if(null != itemBase)
            {
                itemBase.Refresh(current, OnNormalRecycleItemClicked);
                itemBase.SetItemDBClickedCB(OnNormalRecycleItemDBClicked);
            }
        }

        mClientEvent.SendEvent(CEvent.OnRecycleSelectedRefresh);

        //补齐15个 或者 N行
        int count = Mathf.Max(minUiItemsCount, ((mUiItems.Count / 5 + (mUiItems.Count % 5 == 0 ? 0 : 1)) *5));
        for (int i = mUiItems.Count; i < count; ++i)
        {
            var itemBase = mUiItems.Append();
            if(null != itemBase)
            {
                itemBase.UnInit();
            }
        }

        AdjustScrollViewPos();

        //设置过滤器显示隐藏
        mQualityFilter.Handle.SetActive((int)ItemLevelLimit.ILL_NONE != level);

        RefreshRecycleItems();
    }

    public void OnDestroy()
    {
        mGridSuit.UnBind<EquipRecycleSuitItem>();
        mGridSuit = null;
        mSuitItemDatas.Clear();
        mPoolHandleManager.Recycle(mSuitItemDatas);
        mSuitItemDatas = null;
        mQualityFilter.Destroy();
        mLevelFilter.Destroy();
        UIItemBarManager.Instance.UnBind(mRecycleAwards);
        mRecycleAwards = null;
        mItemDatas.Clear();
        mItemDatas = null;
        mUiItems.Clear();
        mUiItems = null;
        mPoolHandleManager.Recycle(mUiItems);
        mUiItems = null;
        mClientEvent.UnRegAll();
    }
}
