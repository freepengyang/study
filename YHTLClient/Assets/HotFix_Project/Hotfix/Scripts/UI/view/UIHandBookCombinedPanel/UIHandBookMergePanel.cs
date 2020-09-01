using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CSAttributeInfo;

public partial class UIHandBookMergePanel : UIBasePanel
{
    private const int itemBarWidth = 190;
    CSPool.Pool<AttrItemData> starAttrDatas;

    UIHandBookCardBinder mChoicedCardBinder;
    HandBookSlotData mSlotData = new HandBookSlotData();

    UIHandBookCardBinder mSlotLBinder;
    HandBookSlotData mSlotLData = new HandBookSlotData();

    UIHandBookCardBinder mSlotRBinder;
    HandBookSlotData mSlotRData = new HandBookSlotData();

    int mFlag = (int)ItemBarData.ItemBarType.IBT_SMALL_ICON | (int)ItemBarData.ItemBarType.IBT_COMPARE | (int)ItemBarData.ItemBarType.IBT_RED_GREEN | (int)ItemBarData.ItemBarType.IBT_ADD | (int)ItemBarData.ItemBarType.IBT_SHORT_EXPRESS_WITH_ONE_POINT;
    FastArrayElementKeepHandle<ItemBarData> mItemDatas;

    public override void Init()
    {
        base.Init();
        starAttrDatas = GetListPool<AttrItemData>();
        mChoicedCardBinder = new UIHandBookCardBinder();
        mChoicedCardBinder.Setup(UIEventListener.Get(mChoicedCard));
        mSlotData.AddFlag(HandBookSlotData.CardFlag.CF_ADDED);
        mSlotData.AddFlag(HandBookSlotData.CardFlag.CF_ENABLE_CLICKED);
        mSlotData.onClicked = this.OnMergeTargetSelected;
        mSlotData.onKeepPressed = this.OnKeepPressed;

        mSlotLBinder = new UIHandBookCardBinder();
        mSlotLBinder.Setup(UIEventListener.Get(mSlotA));
        mSlotLData.AddFlag(HandBookSlotData.CardFlag.CF_ADDED);
        mSlotLData.AddFlag(HandBookSlotData.CardFlag.CF_ENABLE_CLICKED);
        mSlotLData.onClicked = this.OnLSlotSelected;
        mSlotLData.onKeepPressed = this.OnKeepPressed;

        mSlotRBinder = new UIHandBookCardBinder();
        mSlotRBinder.Setup(UIEventListener.Get(mSlotC));
        mSlotRData.AddFlag(HandBookSlotData.CardFlag.CF_ADDED);
        mSlotRData.AddFlag(HandBookSlotData.CardFlag.CF_ENABLE_CLICKED);
        mSlotRData.onClicked = this.OnRSlotSelected;
        mSlotRData.onKeepPressed = this.OnKeepPressed;

        mbtn_help.onClick = this.OnClickHelp;
        mBtnUpgarde.onClick = this.OnClickUpgradeQuality;
        mClientEvent.AddEvent(CEvent.OnHandBookChoicedForMerge, OnCardChoiced);
        mClientEvent.AddEvent(CEvent.OnHandBookUpgradeSucceed, OnHandBookUpgradeSucceed);
        mClientEvent.AddEvent(CEvent.ItemListChange, OnItemChanged);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnItemChanged);
        mClientEvent.AddEvent(CEvent.OnHandBookInlayChanged, OnHandBookInlayChanged);
        mClientEvent.AddEvent(CEvent.OnHandBookRemoved, OnHandBookRemoved);

        mItemDatas = new FastArrayElementKeepHandle<ItemBarData>(8);
        CSEffectPlayMgr.Instance.ShowUITexture(mbg, "handbookdecorate2");
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

    protected void OnHandBookRemoved(uint id, object argv)
    {
        RefreshChoicedData(mSlotData);
        RefreshLData(mSlotLData);
        RefreshRData(mSlotRData);
        CheckUpgradeRedPoint();
    }

    protected void OnHandBookUpgradeSucceed(uint id, object argv)
    {
        if (argv is long guid)
        {
            if (guid == mSlotData.Guid || guid == mSlotLData.Guid || guid == mSlotRData.Guid)
            {
                //选择主卡
                HandBookSlotData queryed = CSHandBookManager.Instance.GetOwnedHandBook(mSlotData.Guid);
                if(null == queryed)
                    queryed = CSHandBookManager.Instance.GetOwnedHandBook(mSlotLData.Guid);
                if (null == queryed)
                    queryed = CSHandBookManager.Instance.GetOwnedHandBook(mSlotRData.Guid);

                mSlotData.SlotID = queryed.SlotID;
                mSlotData.HandBookId = queryed.HandBookId;
                mSlotData.Guid = queryed.Guid;

                mSlotLData.SlotID = 0;
                mSlotLData.HandBookId = 0;
                mSlotLData.Guid = 0;

                mSlotRData.SlotID = 0;
                mSlotRData.HandBookId = 0;
                mSlotRData.Guid = 0;

                RefreshChoicedData(mSlotData);
                RefreshLData(mSlotLData);
                RefreshRData(mSlotRData);
                CheckUpgradeRedPoint();

                //设置自动镶嵌
                AutoSetupMergedCard(queryed);
                //if(guid == mSlotData.Guid)
                //{
                //    mChoicedCardBinder.PlayUpgradeQualityEffect(true);
                //}
                //else if(guid == mSlotLData.Guid)
                //{
                //    mSlotLBinder.PlayUpgradeQualityEffect(false);
                //}
                //else if(guid == mSlotRData.Guid)
                //{
                //    mSlotRBinder.PlayUpgradeQualityEffect(false);
                //}
            }
        }
    }

    public void AutoSetupMergedCard(HandBookSlotData queryed)
    {
        if(null != queryed)
        {
            FNDebug.LogFormat($"<color=#00ff00>[设置镶嵌]:[{queryed.Guid}]</color>]");
            CSHandBookManager.Instance.InlayHandBookOnEmptySlot(queryed.Guid);
        }
    }

    protected void OnItemChanged(uint id, object argv)
    {
        mredpoint?.SetActive(mSlotData.CanUpgradeQuality);
    }

    protected void OnHandBookInlayChanged(uint id,object argv)
    {
        RefreshChoicedData(mSlotData);
        RefreshLData(mSlotLData);
        RefreshRData(mSlotRData);
    }

    public override void Show()
    {
        base.Show();
        Bind(null);
    }

    bool MergeFilter(HandBookSlotData handBook)
    {
        return null == handBook || handBook.qualityFull;
    }

    protected HandBookSlotData mChoicedData;
    public void Bind(HandBookSlotData choicedData)
    {
        mChoicedData = choicedData;
        int cachedCount = 0;
        FastArrayElementKeepHandle<HandBookSlotData> handBooks = CSHandBookManager.Instance.GetOwnedExpressDatas(ref cachedCount, 0, 0, 0, 0, MergeFilter);
        CSHandBookManager.Instance.SortMergeComparer(handBooks,mChoicedData);
        mChoicedData = null;
        if (handBooks.Count > 0)
            RefreshChoicedData(handBooks[0]);
        else
            RefreshChoicedData(mSlotData);
        if (handBooks.Count > 1 && handBooks[0].CanMergeWith(handBooks[1]))
            RefreshLData(handBooks[1]);
        else
            RefreshLData(mSlotLData);

        if (handBooks.Count > 2 && handBooks[0].CanMergeWith(handBooks[2]))
            RefreshRData(handBooks[2]);
        else
            RefreshRData(mSlotRData);

        CheckUpgradeRedPoint();
        CSHandBookManager.Instance.GetUpgradeAttributes(mSlotData.HandBook, mPoolHandleManager, OnAttributesVisible, CSHandBookManager.UpgradeMode.UM_QUALITY);
    }

    protected void OnMergeTargetSelected(HandBookSlotData mSlotData)
    {
        UIManager.Instance.CreatePanel<UIHandBookChoicePanel>(f =>
        {
            (f as UIHandBookChoicePanel).Show(mSlotData.Guid,mSlotLData.Guid,mSlotRData.Guid,
                UIHandBookChoicePanel.ChoicedOpMode.COM_Merge,UIHandBookChoicePanel.ChoicedSubMode.CSM_MERGE,
                (int)HandBookSlotData.CardFlag.CF_ENABLE_UPGRADE_QUALITY_FLAG);
        });
    }

    protected bool CheckMainCardExisted()
    {
        if (null == mSlotData || null == mSlotData.HandBook || mSlotData.Guid == 0)
        {
            UtilityTips.ShowRedTips(673);
            return false;
        }
        return true;
    }

    protected void OnLSlotSelected(HandBookSlotData slotData)
    {
        if (!CheckMainCardExisted())
            return;

        UIManager.Instance.CreatePanel<UIHandBookChoicePanel>(f =>
        {
            (f as UIHandBookChoicePanel).Show(mSlotData.Guid, mSlotLData.Guid, mSlotRData.Guid, UIHandBookChoicePanel.ChoicedOpMode.COM_Merge, UIHandBookChoicePanel.ChoicedSubMode.CSM_L,
                (int)HandBookSlotData.CardFlag.CF_ENABLE_UPGRADE_QUALITY_FILTER);
        });
    }

    protected void OnRSlotSelected(HandBookSlotData slotData)
    {
        if (!CheckMainCardExisted())
            return;

        UIManager.Instance.CreatePanel<UIHandBookChoicePanel>(f =>
        {
            (f as UIHandBookChoicePanel).Show(mSlotData.Guid, mSlotLData.Guid, mSlotRData.Guid, UIHandBookChoicePanel.ChoicedOpMode.COM_Merge, UIHandBookChoicePanel.ChoicedSubMode.CSM_R,
                (int)HandBookSlotData.CardFlag.CF_ENABLE_UPGRADE_QUALITY_FILTER);
        });
    }

    protected void OnClickHelp(UnityEngine.GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.HandBookMerge);
    }

    protected void OnAttributesVisible(RepeatedField<KeyValue> kvs, RepeatedField<KeyValue> nextKvs,bool playEffect)
    {
        var datas = mPoolHandleManager.GetSystemClass<List<AttrItemData>>();
        for (int i = 0; i < kvs.Count; ++i)
        {
            if (kvs[i].IsZeroValue && nextKvs[i].IsZeroValue)
            {
                kvs[i].OnRecycle(mPoolHandleManager);
                nextKvs[i].OnRecycle(mPoolHandleManager);
                continue;
            }

            var currentValue = starAttrDatas.Get();
            datas.Add(currentValue);
            currentValue.needEffect = playEffect && kvs[i].HasDiff(nextKvs[i]);
            currentValue.pooledHandle = mPoolHandleManager;
            currentValue.keyValue = kvs[i];
            currentValue.nKeyValue = nextKvs[i];
        }
        if (null != mfixeHint)
            mfixeHint.SetActive(datas.Count <= 0);
        mGridAttributes.Bind<AttrItemData, AttrItem>(datas, mPoolHandleManager);
        datas.Clear();
        mPoolHandleManager.Recycle(datas);
    }

    protected void OnClickUpgradeQuality(UnityEngine.GameObject go)
    {
        if (null != mCoroutine)
            return;

        mCoroutine = CSGame.Sington.StartCoroutine(AnsyMergeCard());
    }

    Coroutine mCoroutine;
    protected IEnumerator AnsyMergeCard()
    {
        if (null == this.mSlotData || null == this.mSlotData.HandBook || this.mSlotData.Guid == 0)
        {
            UtilityTips.ShowRedTips(670);
            mCoroutine = null;
            yield break;
        }

        if (null == this.mSlotLData || null == this.mSlotLData.HandBook || this.mSlotLData.Guid == 0 || !this.mSlotData.CanMergeWith(this.mSlotLData))
        {
            UtilityTips.ShowRedTips(1891);
            mCoroutine = null;
            yield break;
        }

        if (null == this.mSlotRData || null == this.mSlotRData.HandBook || this.mSlotRData.Guid == 0 || !this.mSlotLData.CanMergeWith(this.mSlotRData))
        {
            UtilityTips.ShowRedTips(1891);
            mCoroutine = null;
            yield break;
        }

        if (!this.mSlotData.CallUpgradeQuality)
        {
            mCoroutine = null;
            yield break;
        }

        long guidMain = mSlotData.Guid;
        //if (mSlotData.HandBook.Level < mSlotLData.HandBook.Level)
        //    guidMain = mSlotLData.Guid;
        //if (mSlotLData.HandBook.Level < mSlotRData.HandBook.Level)
        //    guidMain = mSlotRData.Guid;
        mChoicedCardBinder.PlayUpgradeQualityEffect(guidMain == mSlotData.Guid);
        mSlotLBinder.PlayUpgradeQualityEffect(guidMain == mSlotLData.Guid);
        mSlotRBinder.PlayUpgradeQualityEffect(guidMain == mSlotRData.Guid);

        float begin = Time.time;
        while (Time.time < begin + 0.5f)
            yield return null;

        RefreshChoicedData(mSlotData, true);
        Net.CSTujianUpQualityMessage(this.mSlotData.Guid.ToGoogleList(this.mSlotLData.Guid, this.mSlotRData.Guid));

        while (Time.time < begin + 18.0f/12)
            yield return null;
        mCoroutine = null;
    }

    protected void RefreshQualityFull()
    {
        if (null != mqualityFull)
        {
            mqualityFull.SetActive(null != mSlotData && null != mSlotData.HandBook && mSlotData.qualityFull);
        }
    }

    protected void OnCardChoiced(uint id, object argv)
    {
        if(argv is object[] argvs && argvs.Length == 2)
        {
            if (argvs[0] is HandBookSlotData hb && argvs[1] is UIHandBookChoicePanel.ChoicedSubMode choicedSubMode)
            {
                if(choicedSubMode == UIHandBookChoicePanel.ChoicedSubMode.CSM_MERGE)
                {
                    RefreshChoicedData(hb);
                    if(!hb.CanMergeWith(mSlotLData))
                    {
                        mSlotLData.SlotID = 0;
                        mSlotLData.Guid = 0;
                        mSlotLData.HandBookId = 0;
                    }
                    RefreshLData(mSlotLData);
                    if (!hb.CanMergeWith(mSlotRData))
                    {
                        mSlotRData.SlotID = 0;
                        mSlotRData.Guid = 0;
                        mSlotRData.HandBookId = 0;
                    }
                    RefreshRData(mSlotRData);
                }
                else if (choicedSubMode == UIHandBookChoicePanel.ChoicedSubMode.CSM_L)
                {
                    RefreshChoicedData(mSlotData);
                    RefreshLData(hb);
                    RefreshRData(mSlotRData);
                }
                else if (choicedSubMode == UIHandBookChoicePanel.ChoicedSubMode.CSM_R)
                {
                    RefreshChoicedData(mSlotData);
                    RefreshLData(mSlotLData);
                    RefreshRData(hb);
                }
                CheckUpgradeRedPoint();
            }
        }
    }

    protected void RefreshBinderData(HandBookSlotData dst, HandBookSlotData src)
    {
        HandBookSlotData queryed = CSHandBookManager.Instance.GetOwnedHandBook(src.Guid);
        if (null == queryed)
        {
            dst.SlotID = 0;
            dst.HandBookId = 0;
            dst.Guid = 0;
        }
        else
        {
            dst.SlotID = queryed.SlotID;
            dst.HandBookId = queryed.HandBookId;
            dst.Guid = queryed.Guid;
        }
    }

    protected void RefreshChoicedData(HandBookSlotData hb,bool playEffect = false)
    {
        RefreshBinderData(mSlotData,hb);
        mChoicedCardBinder.Bind(mSlotData);
        RefreshQualityFull();
        CSHandBookManager.Instance.GetUpgradeAttributes(mSlotData.HandBook, mPoolHandleManager, OnAttributesVisible, CSHandBookManager.UpgradeMode.UM_QUALITY,playEffect);
        RebuildCostItems(mSlotData);
    }

    protected void RefreshLData(HandBookSlotData hb)
    {
        RefreshBinderData(mSlotLData, hb);
        if (mSlotLData.Guid != 0 && mSlotData.Guid != 0)
        {
            //主槽位ID与本槽位相同|与主槽位品质不一样
            if (!mSlotData.CanMergeWith(mSlotLData) || mSlotData.Guid == mSlotLData.Guid)
            {
                mSlotLData.ResetSlotData();
            }
        }
        mSlotLBinder.Bind(mSlotLData);
    }

    protected void RefreshRData(HandBookSlotData hb)
    {
        RefreshBinderData(mSlotRData, hb);
        if(mSlotRData.Guid != 0 && mSlotData.Guid != 0)
        {
            //主槽位ID与本槽位相同|与主槽位品质不一样
            if (!mSlotData.CanMergeWith(mSlotRData) || mSlotData.Guid == mSlotRData.Guid)
            {
                mSlotRData.ResetSlotData();
            }
        }
        mSlotRBinder.Bind(mSlotRData);
    }

    protected void RebuildCostItems(HandBookSlotData slotData)
    {
        if (null != slotData && null != slotData.HandBook)
        {
            mItemDatas.Clear();
            var costs = slotData.HandBook.qualityUpCost;
            for (int i = 0,max = costs.Count;i < max;++i)
            {
                var itemData = UIItemBarManager.Instance.Get();
                itemData.cfgId = costs[i].key();
                itemData.needed = costs[i].value();
                itemData.owned = CSItemCountManager.Instance.GetItemCount(itemData.cfgId);
                itemData.flag = mFlag;
                itemData.bgWidth = itemBarWidth;
                mItemDatas.Append(itemData);
            }
            UIItemBarManager.Instance.Bind(mcost_items, mItemDatas);
        }
    }

    protected void CheckUpgradeRedPoint()
    {
        if(null != mredpoint)
        {
            if(null == mSlotData || null == mSlotData.HandBook || mSlotData.Guid == 0)
            {
                mredpoint.SetActive(false);
                return;
            }
            if (null == mSlotLData || null == mSlotLData.HandBook || mSlotLData.Guid == 0 || !mSlotData.CanMergeWith(mSlotLData))
            {
                mredpoint.SetActive(false);
                return;
            }
            if (null == mSlotRData || null == mSlotRData.HandBook || mSlotRData.Guid == 0 || !mSlotLData.CanMergeWith(mSlotRData))
            {
                mredpoint.SetActive(false);
                return;
            }
            mredpoint.SetActive(mSlotData.CanUpgradeQuality);
        }
    }

    protected override void OnDestroy()
    {
        if(null != mCoroutine)
        {
            CSGame.Sington.StopCoroutine(mCoroutine);
            mCoroutine = null;
        }
        mItemDatas?.Clear();
        mItemDatas = null;
        UIItemBarManager.Instance.UnBind(mcost_items);
        mcost_items = null;
        mGridAttributes?.UnBind<AttrItem>();
        mGridAttributes = null;
        mSlotData = null;
        mChoicedCardBinder?.Destroy();
        mChoicedCardBinder = null;
        mSlotLBinder?.Destroy();
        mSlotLBinder = null;
        mSlotLData = null;
        mSlotRBinder?.Destroy();
        mSlotRBinder = null;
        mSlotRData = null;
        starAttrDatas = null;
        CSEffectPlayMgr.Instance.Recycle(mbg);
        mClientEvent.RemoveEvent(CEvent.OnHandBookChoicedForMerge, OnCardChoiced);
        mClientEvent.RemoveEvent(CEvent.OnHandBookUpgradeSucceed, OnHandBookUpgradeSucceed);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnItemChanged);
        mClientEvent.RemoveEvent(CEvent.OnHandBookInlayChanged, OnHandBookInlayChanged);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnItemChanged);
        mClientEvent.RemoveEvent(CEvent.OnHandBookRemoved, OnHandBookRemoved);
        base.OnDestroy();
    }
}