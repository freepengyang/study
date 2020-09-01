using System.Collections;
using System.Collections.Generic;
using TABLE;
using UnityEngine;

public partial class UIHandBookPackagePanel : UIBasePanel
{
    public override void Init()
    {
        base.Init();

        mbtn_close.onClick = this.Close;
        mbtn_quality.onClick = f =>
        {
            SortByQualityDesc(!mSortByQualityDesc, true);
        };

        InitCampFilters();
        InitZoneFilters();
        InitPositionFilters();

        mClientEvent.AddEvent(CEvent.OnHandBookAdded, OnHandBookChanged);
        mClientEvent.AddEvent(CEvent.OnHandBookRemoved, OnHandBookChanged);
        mClientEvent.AddEvent(CEvent.OnHandBookUpgradeSucceed, OnHandBookChanged);
        mClientEvent.AddEvent(CEvent.OnHandBookInlayChanged, OnHandBookChanged);

        SortByQualityDesc(true,false);
    }

    #region CampFilter
    CSPopList mCampFilter;
    TABLE.SUNDRY mCampSundryItem;
    string[] mCampValues;
    int maxRaw = 7;

    protected void InitCampFilters()
    {
        mCampFilter = new CSPopList(mbtn_camp_filter, mPoolHandleManager);
        if (!SundryTableManager.Instance.TryGetValue(53, out mCampSundryItem))
            return;
        mCampValues = mCampSundryItem.effect.Split('#');
        mCampFilter.MaxCount = mCampValues.Length;
        for (int i = 0; i < mCampValues.Length; ++i)
        {
            mCampFilter.mDatas[i].idxValue = i;
            mCampFilter.mDatas[i].value = mCampValues[i];
        }
        mCampFilter.InitList(OnCampChanged,true, maxRaw);
        mCampFilter.SetCurValue(0, false);
    }

    protected void OnCampChanged(CSPopListData data)
    {
        this.campId = data.idxValue;
        OnFilterChanged(this.campId, this.zoneId, this.positionId);
    }
    #endregion

    #region ZoneFilter
    CSPopList mZoneFilter;
    TABLE.SUNDRY mZoneSundryItem;
    string[] mZoneValues;

    protected void InitZoneFilters()
    {
        mZoneFilter = new CSPopList(mbtn_map_filter, mPoolHandleManager);
        if (!SundryTableManager.Instance.TryGetValue(54, out mZoneSundryItem))
            return;
        mZoneValues = mZoneSundryItem.effect.Split('#');
        mZoneFilter.MaxCount = mZoneValues.Length;
        for (int i = 0; i < mZoneValues.Length; ++i)
        {
            mZoneFilter.mDatas[i].idxValue = i;
            mZoneFilter.mDatas[i].value = mZoneValues[i];
        }
        mZoneFilter.InitList(OnZoneChanged,true, maxRaw);
        mZoneFilter.SetCurValue(0, false);
    }

    protected void OnZoneChanged(CSPopListData data)
    {
        this.zoneId = data.idxValue;
        OnFilterChanged(this.campId, this.zoneId, this.positionId);
    }
    #endregion

    #region PositionFilter
    CSPopList mPositionFilter;
    TABLE.SUNDRY mPositionSundryItem;
    string[] mPositionValues;

    protected void InitPositionFilters()
    {
        mPositionFilter = new CSPopList(mbtn_position_filter, mPoolHandleManager);
        if (!SundryTableManager.Instance.TryGetValue(55, out mPositionSundryItem))
            return;
        mPositionValues = mPositionSundryItem.effect.Split('#');
        mPositionFilter.MaxCount = mPositionValues.Length;
        for (int i = 0; i < mPositionValues.Length; ++i)
        {
            mPositionFilter.mDatas[i].idxValue = i;
            mPositionFilter.mDatas[i].value = mPositionValues[i];
        }
        mPositionFilter.InitList(OnPositionChanged,true, maxRaw);
        mPositionFilter.SetCurValue(0, false);
    }

    protected void OnPositionChanged(CSPopListData data)
    {
        this.positionId = data.idxValue;
        OnFilterChanged(this.campId, this.zoneId, this.positionId);
    }
    #endregion

    int campId;
    int zoneId;
    int positionId;

    #region QualitySort
    protected Vector3 mUpScale = new Vector3(1, -1, 1);
    //是否安品质降序
    bool mSortByQualityDesc = false;
    public void SortByQualityDesc(bool value,bool needUpdate)
    {
        if (mSortByQualityDesc != value)
        {
            mSortByQualityDesc = value;
            if (null != mbtn_quality_transform.localScale)
            {
                if (value)
                    mbtn_quality_transform.localScale = Vector3.one;
                else
                    mbtn_quality_transform.localScale = mUpScale;
            }
            if(needUpdate)
            {
                BindCoroutine(8888, RefreshBookList());
            }
        }
    }
    #endregion

    public override void Show()
    {
        base.Show();

        BindCoroutine(8888, RefreshBookList());
    }

    protected void OnHandBookChanged(uint id,object argv)
    {
        BindCoroutine(8888, RefreshBookList());
    }

    protected IEnumerator RefreshBookList()
    {
        FastArrayElementKeepHandle<HandBookSlotData> handBooks = null;
        if(mSortByQualityDesc)
        {
            int cachedCount = 0;
            handBooks = CSHandBookManager.Instance.GetOwnedExpressDatas(ref cachedCount, this.campId, this.zoneId, this.positionId, 21);
            CSHandBookManager.Instance.HandBookSelectQualityDescCompare(handBooks, cachedCount);
        }
        else
        {
            int cachedCount = 0;
            handBooks = CSHandBookManager.Instance.GetOwnedExpressDatas(ref cachedCount, this.campId, this.zoneId, this.positionId, 21);
            CSHandBookManager.Instance.HandBookSelectQualityAscCompare(handBooks, cachedCount);
        }

        if (null != mfixeHint)
            mfixeHint.SetActive(handBooks.Count <= 0);

        yield return null;
        
        void OnItemVisible(GameObject gameObject, int idx)
        {
            var binder = gameObject.GetOrAddBinder<UIHandBookCardBinder>(mPoolHandleManager);
            binder.Bind(handBooks[idx]);
        }

        for (int i = 0,max = handBooks.Count; i < max; ++i)
        {
            var handBook = handBooks[i];
            handBook.onClicked = OnHandBookClicked;
            handBook.onChoiceChanged = null;
            handBook.onKeepPressed = null;
            handBook.RemoveFlag(HandBookSlotData.CardFlag.CF_ENABLE_SELECTED);
            handBook.RemoveFlag(HandBookSlotData.CardFlag.CF_ENABLE_UNSELETED);
        }

        yield return mgrid_cards.BindAsync(handBooks.Count, OnItemVisible);
        mgrid_cards.UpdateWidgetCollider();
    }

    protected void OnHandBookClicked(HandBookSlotData bookData)
    {
        if (null == bookData)
            return;
        UIManager.Instance.CreatePanel<UIHandBookTipsPanel>(f =>
        {
            (f as UIHandBookTipsPanel).Show(bookData.HandBookId, bookData.Guid, (1 << (int)UIHandBookTipsPanel.MenuType.MT_UPGRADE_LEVEL)| (1 << (int)UIHandBookTipsPanel.MenuType.MT_UPGRADE_QUALITY),this.Close);
        });
        FNDebug.LogFormat("OnHandBookClicked id = {0} name = {1}", bookData.HandBookId, bookData.HandBook.ItemID.ItemName());
    }

    protected void OnFilterChanged(int campId, int zoneId, int positionId)
    {
        BindCoroutine(8888,RefreshBookList());
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnHandBookAdded, OnHandBookChanged);
        mClientEvent.RemoveEvent(CEvent.OnHandBookRemoved, OnHandBookChanged);
        mClientEvent.RemoveEvent(CEvent.OnHandBookUpgradeSucceed, OnHandBookChanged);
        mClientEvent.RemoveEvent(CEvent.OnHandBookInlayChanged, OnHandBookChanged);

        mCampFilter?.Destroy();
        mCampFilter = null;
        mZoneFilter?.Destroy();
        mZoneFilter = null;
        mPositionFilter?.Destroy();
        mPositionFilter = null;

        mgrid_cards?.UnBind<UIHandBookCardBinder>();
        mgrid_cards = null;
        base.OnDestroy();
    }
}
