using System.Collections;
using UnityEngine;

public partial class UIHandBookMarkPanel : UIBasePanel
{
	public override void Init()
	{
		base.Init();

        InitCampFilters();
        InitZoneFilters();
        InitPositionFilters();
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
        mCampFilter.InitList(OnCampChanged, true, maxRaw);
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
        mZoneFilter.InitList(OnZoneChanged, true, maxRaw);
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
        mPositionFilter.InitList(OnPositionChanged, true, maxRaw);
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

    protected IEnumerator UpdateHandBookMarks()
    {
        yield return CSHandBookManager.Instance.GetHandBookMarkDatas(this.campId, this.zoneId, this.positionId);
        var datas = CSHandBookManager.Instance.HandBookMarkDatas;
        for (int i = 0,max = datas.Count; i < max; ++i)
        {
            datas[i].onClicked = OnCardClicked;
            datas[i].onChoiceChanged = null;
            datas[i].onKeepPressed = null;
        }

        void OnItemVisible(GameObject gameObject, int idx)
        {
            var binder = gameObject.GetOrAddBinder<UIHandBookCardBinder>(mPoolHandleManager);
            binder.Bind(datas[idx]);
        }

        yield return mgrid_cards.BindAsync(datas.Count, OnItemVisible);
    }

    protected void OnCardClicked(HandBookSlotData data)
    {
        if (null == data)
            return;

        UIManager.Instance.CreatePanel<UIHandBookTipsPanel>(f =>
        {
            (f as UIHandBookTipsPanel).Show(data.HandBookId, data,1 << (int)UIHandBookTipsPanel.MenuType.MT_MENU_FROM_GETWAY);
        });
    }

    public override void Show()
	{
		base.Show();
        BindCoroutine(8888, UpdateHandBookMarks());
    }

    protected void OnFilterChanged(int campId, int zoneId, int positionId)
    {
        BindCoroutine(8888, UpdateHandBookMarks());
    }

    protected override void OnDestroy()
	{
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
