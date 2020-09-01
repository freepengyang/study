using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIHandBookChoicePanel : UIBasePanel
{
    public override void Init()
    {
        base.Init();
        AddCollider();

        InitCampFilters();
        InitZoneFilters();
        InitPositionFilters();

        mbtn_close.onClick = this.Close;
        mClientEvent.AddEvent(CEvent.OnHandBookInlayChanged, OnHandBookChanged);
    }

    protected void OnHandBookChanged(uint id,object argv)
    {
        BindCoroutine(9999, RefreshBookList());
    }

    public enum ChoicedOpMode
    {
        COM_Upgrade = 0,
        COM_Merge = 1,
    }

    public enum ChoicedSubMode
    {
        CSM_NONE = 0,
        CSM_MERGE = 1,
        CSM_L = 2,
        CSM_R = 3,
    }

    long mGuid;
    long mL;
    long mR;
    ChoicedOpMode mChoicedOpMode = ChoicedOpMode.COM_Upgrade;
    ChoicedSubMode mChoicedSubMode = ChoicedSubMode.CSM_NONE;
    int mFlag = 0;
    public void Show(long guid,long l,long r,ChoicedOpMode choicedOpMode, ChoicedSubMode choicedSubMode,int flag = 0)
    {
        mChoicedSubMode = choicedSubMode;
        mChoicedOpMode = choicedOpMode;
        mGuid = guid;
        mL = l;
        mR = r;
        mFlag = flag;
        if(null != mlb_hint)
        {
            mlb_hint.text = CSString.Format(choicedOpMode == ChoicedOpMode.COM_Upgrade ? 671 : 672);
        }
        BindCoroutine(9999, RefreshBookList());
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
    protected void OnFilterChanged(int campId,int zoneId, int positionId)
    {
        BindCoroutine(9999, RefreshBookList());
    }

    protected void OnConfirmChange(GameObject go)
    {
        CSHandBookManager.Instance.RequestForChoicedCard();
        UIManager.Instance.ClosePanel(GetType());
    }

    protected bool UpgradeQualitySecondaryCardFilter(HandBookSlotData handBook)
    {
        if (null == handBook || null == handBook.HandBook)
            return true;
        var mainCard = CSHandBookManager.Instance.GetOwnedHandBook(mGuid);
        if (null == mainCard || null == mainCard.HandBook)
            return true;
        if (!mainCard.CanMergeWith(handBook))
            return true;
        if (mainCard.Guid == handBook.Guid)
            return true;
        var lCard = CSHandBookManager.Instance.GetOwnedHandBook(mL);
        if(null != lCard)
        {
            if (lCard.Guid == handBook.Guid)
                return true;
        }
        var rCard = CSHandBookManager.Instance.GetOwnedHandBook(mR);
        if (null != rCard)
        {
            if (rCard.Guid == handBook.Guid)
                return true;
        }
        return false;
    }

    bool MergeFilter(HandBookSlotData handBook)
    {
        if (null == handBook || handBook.qualityFull)
            return true;

        return false;
    }

    void OnClickToOpenCardLink(GameObject go)
    {
        Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(493));
    }

    FastArrayElementKeepHandle<HandBookSlotData> mHandBooks;
    protected IEnumerator RefreshBookList()
    {
        if (mChoicedOpMode == ChoicedOpMode.COM_Upgrade)
        {
            int cachedCount = 0;
            mHandBooks = CSHandBookManager.Instance.GetOwnedExpressDatas(ref cachedCount,
                this.campId, this.zoneId, this.positionId, 0, CSHandBookManager.Instance.UpgradeLevelFilter);
            CSHandBookManager.Instance.HandBookChoiceForUpgradeCompare(mHandBooks);
        }
        else
        {
            if (mChoicedSubMode == ChoicedSubMode.CSM_MERGE)
            {
                int cachedCount = 0;
                mHandBooks = CSHandBookManager.Instance.GetOwnedExpressDatas(ref cachedCount,
                    this.campId, this.zoneId, this.positionId, 0, MergeFilter);
                CSHandBookManager.Instance.HandBookChoicedForMergeCompare(mHandBooks);
            }
            else
            {
                int cachedCount = 0;
                mHandBooks = CSHandBookManager.Instance.GetOwnedExpressDatas(ref cachedCount,
                    this.campId, this.zoneId, this.positionId, 0, UpgradeQualitySecondaryCardFilter);
                CSHandBookManager.Instance.HandBookChoicedForMergeCompare(mHandBooks);
            }
        }
        mbtnCardLink.CustomActive(mHandBooks.Count <= 0);
        mbtnCardLink.onClick = OnClickToOpenCardLink;

        yield return null;

        HandBookSlotData handBook = null;
        for (int i = 0,max = mHandBooks.Count; i < max; ++i)
        {
            handBook = mHandBooks[i];
            handBook.onClicked = this.OnMaskClicked;
            handBook.onChoiceChanged = this.OnChoicedBookChanged;
            handBook.onKeepPressed = this.OnKeepPressed;
            handBook.RemoveFlag(HandBookSlotData.CardFlag.CF_SELECTED);
            handBook.AddFlag(HandBookSlotData.CardFlag.CF_ENABLE_SELECTED);
            handBook.RemoveFlag(HandBookSlotData.CardFlag.CF_ENABLE_UNSELETED);
            handBook.RemoveFlag(HandBookSlotData.CardFlag.CF_UPGRADE_LEVEL);
            handBook.RemoveFlag(HandBookSlotData.CardFlag.CF_UPGRADE_QUALITY);
            handBook.AddExtraFlag(mFlag);

            if(handBook.HasFlag(HandBookSlotData.CardFlag.CF_ENABLE_UPGRADE_QUALITY_FILTER))
            {
                handBook.onChoiceFilter = handBook.SetupedChoiceFilter;
            }
            else
            {
                handBook.onChoiceFilter = null;
            }

            if (mChoicedOpMode == ChoicedOpMode.COM_Upgrade)
            {
                handBook.AddFlag(HandBookSlotData.CardFlag.CF_UPGRADE_LEVEL);
                if (handBook.levelFull)
                {
                    handBook.AddFlag(HandBookSlotData.CardFlag.CF_DISABLE_MASK_MODE);
                }
                else
                {
                    handBook.RemoveFlag(HandBookSlotData.CardFlag.CF_DISABLE_MASK_MODE);
                }
            }
            else
            {
                handBook.AddFlag(HandBookSlotData.CardFlag.CF_UPGRADE_QUALITY);
                if (handBook.qualityFull)
                {
                    handBook.AddFlag(HandBookSlotData.CardFlag.CF_DISABLE_MASK_MODE);
                }
                else
                {
                    handBook.RemoveFlag(HandBookSlotData.CardFlag.CF_DISABLE_MASK_MODE);
                }
            }
            if(handBook.Guid == mGuid)
            {
                handBook.AddFlag(HandBookSlotData.CardFlag.CF_ENABLE_UNSELETED);
            }
        }
        yield return null;

        void OnItemVisible(GameObject gameObject, int idx)
        {
            var binder = gameObject.GetOrAddBinder<UIHandBookCardBinder>(mPoolHandleManager);
            binder.Bind(mHandBooks[idx]);
        }

        yield return mgrid_cards.BindAsync(mHandBooks.Count, OnItemVisible);
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

    protected void OnChoicedBookChanged(HandBookSlotData data, bool value)
    {
        //移除先前的选择项目
        if(value)
        {
            mGuid = data.Guid;
            for (int i = 0; i < mHandBooks.Count; ++i)
            {
                mHandBooks[i].onClicked = null;
                mHandBooks[i].onChoiceChanged = null;
                mHandBooks[i].RemoveFlag(HandBookSlotData.CardFlag.CF_SELECTED);
                mHandBooks[i].RemoveFlag(HandBookSlotData.CardFlag.CF_ENABLE_SELECTED);
                mHandBooks[i].RemoveFlag(HandBookSlotData.CardFlag.CF_ENABLE_UNSELETED);
                mHandBooks[i].RemoveFlag(HandBookSlotData.CardFlag.CF_UPGRADE_LEVEL);
                mHandBooks[i].RemoveFlag(HandBookSlotData.CardFlag.CF_UPGRADE_QUALITY);
                mHandBooks[i].RemoveFlag(HandBookSlotData.CardFlag.CF_DISABLE_MASK_MODE);
            }

            if (mChoicedOpMode == ChoicedOpMode.COM_Upgrade)
            {
                mClientEvent.SendEvent(CEvent.OnHandBookChoicedForUpgrade, data);
            }
            else
            {
                mClientEvent.SendEvent(CEvent.OnHandBookChoicedForMerge, new object[] { data, mChoicedSubMode });
            }

            Close();
        }
    }
    protected void OnMaskClicked(HandBookSlotData data)
    {
        if(!data.HasFlag(HandBookSlotData.CardFlag.CF_DISABLE_MASK_MODE))
        {
            return;
        }
        if(mChoicedOpMode == ChoicedOpMode.COM_Upgrade)
            UtilityTips.ShowRedTips(659);
        else
            UtilityTips.ShowRedTips(660);
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnHandBookInlayChanged, OnHandBookChanged);
        mHandBooks = null;
        mZoneFilter?.Destroy();
        mZoneFilter = null;
        mCampFilter?.Destroy();
        mCampFilter = null;
        mPositionFilter?.Destroy();
        mPositionFilter = null;

        mgrid_cards?.UnBind<UIHandBookCardBinder>();
        mgrid_cards = null;
        base.OnDestroy();
    }
}