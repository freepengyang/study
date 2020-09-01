using Google.Protobuf.Collections;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CSAttributeInfo;

public partial class UIHandBookUpgradePanel : UIBasePanel
{
    private const int itemBarWidth = 190;
    UIHandBookCardBinder mChoicedCardBinder;
    HandBookSlotData mSlotData = new HandBookSlotData();
    protected CSPool.Pool<AttrItemData> starAttrDatas; 
    int mFlag = (int)ItemBarData.ItemBarType.IBT_SMALL_ICON | (int)ItemBarData.ItemBarType.IBT_COMPARE | (int)ItemBarData.ItemBarType.IBT_RED_GREEN | (int)ItemBarData.ItemBarType.IBT_ADD |(int)ItemBarData.ItemBarType.IBT_SHORT_EXPRESS_WITH_ONE_POINT;
    FastArrayElementKeepHandle<ItemBarData> mItemDatas;
    public override void Init()
    {
        base.Init();
        starAttrDatas = GetListPool<AttrItemData>();
        mChoicedCardBinder = new UIHandBookCardBinder();
        mChoicedCardBinder.Setup(UIEventListener.Get(mChoicedCard));
        mSlotData.AddFlag(HandBookSlotData.CardFlag.CF_ADDED);
        mSlotData.AddFlag(HandBookSlotData.CardFlag.CF_ENABLE_CLICKED);
        mSlotData.onClicked = this.OnSlotClicked;
        mSlotData.onKeepPressed = this.OnKeepPressed;
        mBtnUpgarde.onClick = this.OnClickUpgrade;
        mBtnHelp.onClick = this.OnClickHelp;
        mClientEvent.AddEvent(CEvent.OnHandBookChoicedForUpgrade, OnCardChoiced);
        mClientEvent.AddEvent(CEvent.OnHandBookRemoved, OnHandBookRemoved);
        mClientEvent.AddEvent(CEvent.OnHandBookUpgradeSucceed, OnHandBookUpgradeSucceed);
        mClientEvent.AddEvent(CEvent.ItemListChange, OnItemChanged);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnItemChanged);
        mClientEvent.AddEvent(CEvent.OnHandBookInlayChanged, OnHandBookInlayChanged);

        mItemDatas = new FastArrayElementKeepHandle<ItemBarData>(4);

        CSEffectPlayMgr.Instance.ShowUITexture(mbg, "handbookdecorate1");
    }

    protected void OnHandBookInlayChanged(uint id, object argv)
    {
        if(null != mSlotData && mSlotData.Guid != 0)
        {
            var lockedData = CSHandBookManager.Instance.GetOwnedHandBook(mSlotData.Guid);
            RefreshChoicedData(lockedData, false);
        }
    }

    protected void OnItemChanged(uint id,object argv)
    {
        mredPoint?.SetActive(mSlotData.CanUpgrade);
    }

    public void Bind(HandBookSlotData slotData)
    {
        RefreshChoicedData(slotData,false);
        CSHandBookManager.Instance.GetUpgradeAttributes(mSlotData.HandBook, mPoolHandleManager, OnAttributesVisible, CSHandBookManager.UpgradeMode.UM_LEVEL);
    }

    public override void Show()
    {
        base.Show();

        if(null == mSlotData.HandBook)
        {
            HandBookSlotData card = null;
            if (CSHandBookManager.Instance.CheckHandBookSetupedUpgradeLevelRedPoint(out card))
            {
                mSlotData.Guid = card.Guid;
                mSlotData.HandBookId = card.HandBookId;
                mSlotData.SlotID = card.SlotID;
            }
        }
        Bind(mSlotData);
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
        mgrid_attributes.Bind<AttrItemData,AttrItem>(datas, mPoolHandleManager);
        datas.Clear();
        mPoolHandleManager.Recycle(datas);
    }

    Coroutine mCoroutine;
    protected void OnClickUpgrade(UnityEngine.GameObject go)
    {
        if (null != mCoroutine)
            return;
        mCoroutine = CSGame.Sington.StartCoroutine(AnsyUpgrade());
    }

    protected IEnumerator AnsyUpgrade()
    {
        if (null == this.mSlotData || null == this.mSlotData.HandBook || this.mSlotData.Guid == 0)
        {
            UtilityTips.ShowRedTips(669);
            mCoroutine = null;
            yield break;
        }

        if (!this.mSlotData.CheckUpgrade)
        {
            mCoroutine = null;
            yield break;
        }

        mChoicedCardBinder.PlayUpgradeLevelEffect();
        float beginTime = Time.time;
        while (beginTime + 0.4f > Time.time)
            yield return null;

        RefreshChoicedData(mSlotData, true);
        Net.CSTujianUpLevelMessage(this.mSlotData.Guid);

        while (beginTime + 1.50f > Time.time)
            yield return null;
        mCoroutine = null;
    }

    protected void OnKeepPressed(HandBookSlotData data)
    {
        if(null != data && null != data.HandBook)
        {
            UIManager.Instance.CreatePanel<UIHandBookTipsPanel>(f =>
            {
                (f as UIHandBookTipsPanel).Show(data.HandBookId, data.Guid, (int)UIHandBookTipsPanel.MenuType.MT_NO_MENU);
            });
        }
    }

    protected void OnClickHelp(UnityEngine.GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.HandBookUpgrade);
    }

    protected void OnSlotClicked(HandBookSlotData mSlotData)
    {
        UIManager.Instance.CreatePanel<UIHandBookChoicePanel>(f=>
        {
            (f as UIHandBookChoicePanel).Show(mSlotData.Guid,0,0,
                UIHandBookChoicePanel.ChoicedOpMode.COM_Upgrade,
                UIHandBookChoicePanel.ChoicedSubMode.CSM_NONE,
                (int)HandBookSlotData.CardFlag.CF_ENABLE_UPGRADE_LEVEL_FLAG);
        });
    }

    protected void OnCardChoiced(uint id,object argv)
    {
        if(argv is HandBookSlotData hb)
        {
            RefreshChoicedData(hb,false);
        }
    }

    protected void RefreshChoicedData(HandBookSlotData hb,bool playEffect)
    {
        if(null != hb)
        {
            mSlotData.SlotID = hb.SlotID;
            mSlotData.HandBookId = hb.HandBookId;
            mSlotData.Guid = hb.Guid;
        }
        else
        {
            mSlotData.SlotID = 0;
            mSlotData.HandBookId = 0;
            mSlotData.Guid = 0;
        }
        mredPoint?.SetActive(mSlotData.CanUpgrade);
        RefreshLevelFull();
        mChoicedCardBinder.Bind(mSlotData);
        CSHandBookManager.Instance.GetUpgradeAttributes(mSlotData.HandBook, mPoolHandleManager, OnAttributesVisible,CSHandBookManager.UpgradeMode.UM_LEVEL, playEffect);
        RebuildCostItems(mSlotData);
        RefreshHint();
    }

    protected void RefreshHint()
    {
        mquality_hint.CustomActive(null != mSlotData && mSlotData.Guid != 0);
        if (null != mSlotData)
        {
            int maxLv = mSlotData.maxLv;
            if (!mSlotData.qualityFull)
            {
                mquality_hint.text = CSString.Format(1715, maxLv);
            }
            else
            {
                mquality_hint.text = CSString.Format(1716, maxLv);
            }
        }
    }

    protected void RefreshLevelFull()
    {
        if (null != mlevelFull)
        {
            mlevelFull.SetActive(null != mSlotData && null != mSlotData.HandBook && mSlotData.levelFull);
        }
    }

    protected void OnHandBookRemoved(uint id, object argv)
    {
        var ownedData = CSHandBookManager.Instance.GetOwnedHandBook(mSlotData.Guid);
        if(null == ownedData)
        {
            mSlotData.ResetSlotData();
            RefreshChoicedData(mSlotData,false);
        }
    }

    protected void OnHandBookUpgradeSucceed(uint id,object argv)
    {
        if(argv is long guid)
        {
            var ownedData = CSHandBookManager.Instance.GetOwnedHandBook(guid);
            //注意升品也会走这里
            if (null != mSlotData && mSlotData.Guid == guid && null != ownedData)
            {
                if (mSlotData.HandBook.Level + 1 == ownedData.HandBook.Level)
                {
                    //这里才是升级了
                    RefreshChoicedData(ownedData,false);
                    //mChoicedCardBinder.PlayUpgradeLevelEffect();
                }
                else
                {
                    //可能被升品了
                    RefreshChoicedData(ownedData,false);
                }
            }
        }
    }

    protected void RebuildCostItems(HandBookSlotData slotData)
    {
        if(null != slotData && null != slotData.HandBook)
        {
            mItemDatas.Clear();
            var costs = slotData.HandBook.levelUpCost;
            for (int i = 0, max = costs.Count; i < max; ++i)
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

    protected override void OnDestroy()
    {
        if(null != mCoroutine)
        {
            CSGame.Sington.StopCoroutine(mCoroutine);
            mCoroutine = null;
        }
        mgrid_attributes?.UnBind<AttrItem>();
        mgrid_attributes = null;
        mChoicedCardBinder?.Destroy();
        mChoicedCardBinder = null;
        mItemDatas?.Clear();
        mItemDatas = null;
        UIItemBarManager.Instance.UnBind(mcost_items);
        mcost_items = null;
        starAttrDatas = null;
        CSEffectPlayMgr.Instance.Recycle(mbg);
        mClientEvent.RemoveEvent(CEvent.OnHandBookRemoved, OnHandBookRemoved);
        mClientEvent.RemoveEvent(CEvent.OnHandBookChoicedForUpgrade, OnCardChoiced);
        mClientEvent.RemoveEvent(CEvent.OnHandBookUpgradeSucceed, OnHandBookUpgradeSucceed);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnItemChanged);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnItemChanged);
        mClientEvent.RemoveEvent(CEvent.OnHandBookInlayChanged, OnHandBookInlayChanged);
        base.OnDestroy();
    }
}