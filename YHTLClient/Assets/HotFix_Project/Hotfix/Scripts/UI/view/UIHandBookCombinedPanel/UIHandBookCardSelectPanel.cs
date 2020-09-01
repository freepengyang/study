using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIHandBookCardSelectPanel : UIBasePanel
{
    public override void Init()
    {
        base.Init();
        AddCollider();

        InitCampFilters();
        InitZoneFilters();
        InitPositionFilters();

        mClientEvent.AddEvent(CEvent.OnHandBookUpgradeSucceed, OnHandBookChanged);
        mClientEvent.AddEvent(CEvent.OnHandBookAdded, OnHandBookChanged);
        mClientEvent.AddEvent(CEvent.OnHandBookRemoved, OnHandBookChanged);
        mClientEvent.AddEvent(CEvent.OnHandBookSlotChanged, OnHandBookSlotChanged);
        mClientEvent.AddEvent(CEvent.OnHandBookInlayChanged, OnHandBookChanged);

        mbtn_close.onClick = this.Close;
        mbtn_card_group.onClick = this.OnShowCardGroupTips;
        mbtn_attr_effect.onClick = this.OnShowCardEffectTips;
        mbtn_confirmed.onClick = this.OnConfirmChange;
    }

    public override void Show()
    {
        base.Show();

        InitChoicedDatas();
        BindCoroutine(9999, RefreshBookList());
        RefreshChoicedCardNums();
        BindCoroutine(9998, RefreshSetupedBookGroup());
    }

    #region CampFilter
    CSPopList mCampFilter;
    TABLE.SUNDRY mCampSundryItem;
    string[] mCampValues;
    int maxRaw = 7;

    protected void InitCampFilters()
    {
        mCampFilter = new CSPopList(mbtn_camp_filter,mPoolHandleManager);
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
        mPositionFilter.InitList(OnPositionChanged,true,maxRaw);
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
    protected void OnFilterChanged(int campId,int zoneId, int positionId)
    {
        mCardScrollView.ScrollImmidate(0);
        BindCoroutine(9999, RefreshBookList());
    }

    protected void OnHandBookChanged(uint id,object argv)
    {
        BindCoroutine(9999, RefreshBookList());
        RefreshChoicedCardNums();
    }

    protected void OnHandBookSlotChanged(uint id, object argv)
    {
        BindCoroutine(9999, RefreshBookList());
        RefreshChoicedCardNums();
    }

    protected void OnShowCardGroupTips(GameObject go)
    {

    }

    protected void OnShowCardEffectTips(GameObject go)
    {

    }

    protected void OnConfirmChange(GameObject go)
    {
        CSHandBookManager.Instance.RequestForChoicedCard();
        UIManager.Instance.ClosePanel(GetType());
    }

    List<HandBookSlotData> mChoicedData = null;
    protected void InitChoicedDatas()
    {
        mChoicedData = CSHandBookManager.Instance.GetChoicedDatas();
    }

    protected IEnumerator RefreshBookList()
    {
        int cachedCount = 0;
        var handBooks = CSHandBookManager.Instance.GetOwnedExpressDatas(ref cachedCount, this.campId,this.zoneId,this.positionId,0);
        CSHandBookManager.Instance.HandBookSelectQualityDescCompare(handBooks,cachedCount);
        yield return null;
        HandBookSlotData handBook = null;
        for (int i = 0,max = handBooks.Count; i < max; ++i)
        {
            handBook = handBooks[i];
            handBook.onClicked = null;
            handBook.onChoiceChanged = this.OnChoicedBookChanged;
            handBook.onChoiceFilter = this.OnChoicedFilter;
            handBook.onKeepPressed = this.OnKeepPressed;
            handBook.AddFlag(HandBookSlotData.CardFlag.CF_ENABLE_SELECTED);
            handBook.AddFlag(HandBookSlotData.CardFlag.CF_ENABLE_UNSELETED);
        }
        yield return null;

        void OnItemVisible(GameObject gameObject, int idx)
        {
            var binder = gameObject.GetOrAddBinder<UIHandBookCardBinder>(mPoolHandleManager);
            binder.Bind(handBooks[idx]);
        }

        yield return mgrid_cards.BindAsync(handBooks.Count, OnItemVisible);
        //mgrid_cards.Bind<UIHandBookCardBinder, HandBookSlotData>(handBooks);

        mbtnCardLink.CustomActive(handBooks.Count <= 0);
        mbtnCardLink.onClick = OnClickToOpenCardLink;
    }

    protected void OnKeepPressed(HandBookSlotData data)
    {
        if (null != data && null != data.HandBook)
        {
            UIManager.Instance.CreatePanel<UIHandBookTipsPanel>(f =>
            {
                (f as UIHandBookTipsPanel).Show(data.HandBookId, data.Guid, 1 << (int)UIHandBookTipsPanel.MenuType.MT_NO_MENU);
            });
        }
    }

    bool OnChoicedFilter()
    {
        int choicedCount = null == mChoicedData ? 0 : mChoicedData.Count;
        int openedSlotCount = CSHandBookManager.Instance.GetOpenedSlotCount();
        if(choicedCount >= openedSlotCount)
        {
            UtilityTips.ShowRedTips(1654);
            return true;
        }
        return false;
    }

    void OnClickToOpenCardLink(GameObject go)
    {
        Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(493));
    }

    protected IEnumerator RefreshSetupedBookGroup()
    {
        var handBookDatas = CSHandBookManager.Instance.GetHandBookGroupDatasForChoicedPreview();
        yield return null;

        void OnItemVisible(GameObject gameObject, int idx)
        {
            var binder = gameObject.GetOrAddBinder<HandBookGroupItemBinder>(mPoolHandleManager);
            binder.Bind(handBookDatas[idx]);
        }

        yield return mgrid_cardeffects.BindAsync(handBookDatas.Count, OnItemVisible);

        //mgrid_cardeffects.Bind<HandBookGroupItemBinder, HandBookGroupItemData>(handBookDatas);
        mgrid_cardeffects.UpdateWidgetCollider();
    }

    protected void OnChoicedBookChanged(HandBookSlotData data, bool value)
    {
        int openedSlotCount = CSHandBookManager.Instance.GetOpenedSlotCount();
        if (!value)
        {
            CSHandBookManager.Instance.RemoveChoicedData(data);
            BindCoroutine(9998, RefreshSetupedBookGroup());
        }
        else
        {
            if (mChoicedData.Count >= openedSlotCount)
            {
                return;
                //var removedChoicedData = mChoicedData[0];
                //removedChoicedData.RemoveFlag(HandBookSlotData.CardFlag.CF_SELECTED);
                //mClientEvent.SendEvent(CEvent.RemoveSelectedFlag, removedChoicedData.Guid);
                //CSHandBookManager.Instance.RemoveChoicedData(removedChoicedData);
            }
            CSHandBookManager.Instance.AddChoicedData(data);
            BindCoroutine(9998, RefreshSetupedBookGroup());
        }
        RefreshChoicedCardNums();
    }

    protected void RefreshChoicedCardNums()
    {
        int choicedCount = CSHandBookManager.Instance.GetSetupedCardCount();
        int openedSlotCount = CSHandBookManager.Instance.GetOpenedSlotCount();
        if(null != mlb_equiped_hint)
        {
            mlb_equiped_hint.text = CSString.Format(646, choicedCount, openedSlotCount);
        }
    }

    protected override void OnDestroy()
    {
        mZoneFilter?.Destroy();
        mZoneFilter = null;
        mCampFilter?.Destroy();
        mCampFilter = null;
        mPositionFilter?.Destroy();
        mPositionFilter = null;
        mgrid_cards?.UnBind<UIHandBookCardBinder>();
        mgrid_cards = null;
        mgrid_cardeffects?.UnBind<HandBookGroupItemBinder>();
        mgrid_cardeffects = null;
        mChoicedData = null;
        CSHandBookManager.Instance.ClearExpressDataChoicedFlag();
        mClientEvent.RemoveEvent(CEvent.OnHandBookUpgradeSucceed, OnHandBookChanged);
        mClientEvent.RemoveEvent(CEvent.OnHandBookAdded, OnHandBookChanged);
        mClientEvent.RemoveEvent(CEvent.OnHandBookRemoved, OnHandBookChanged);
        mClientEvent.RemoveEvent(CEvent.OnHandBookSlotChanged, OnHandBookSlotChanged);
        mClientEvent.RemoveEvent(CEvent.OnHandBookInlayChanged, OnHandBookChanged);
        base.OnDestroy();
    }
}