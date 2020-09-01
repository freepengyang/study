using bag;
using System.Collections;
using UnityEngine;

public partial class UIGetItemPanel : UIBasePanel
{
    UIItemBase mItemBase;

    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Panel; }
    }

    public override void Init()
    {
        base.Init();
        mbtn_close.onClick = this.OnCloseClicked;
        mbtn_equip.onClick = OnUseItem;
        mItemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, mItemParent.transform);
        if (null == mItemBase)
        {
            FNDebug.LogFormat("UIItemManager.Instance.GetItem 失败");
        }
        mClientEvent.AddEvent(CEvent.OnBetterEquipRemoved, OnBetterEquipRemoved);
        mClientEvent.AddEvent(CEvent.OnHintUsedItemCountChanged, OnHintUsedItemCountChanged);
        mClientEvent.AddEvent(CEvent.OnItemUsedTimesChanged, OnItemUsedTimesChanged);
        //mClientEvent.SendEvent(CEvent.GetItemPanelOpen);
        CSEffectPlayMgr.Instance.ShowUITexture(mBg, "main_getitem_bg");
        //Panel.depth = 80;
    }

    void OnCloseClicked(GameObject go)
    {
        if (null != mItemBase && null != mItemBase.infos)
        {
            var itemInfo = mItemBase.infos;
            var itemCfg = mItemBase.itemCfg;
            this.Close();

            if (itemCfg.type == 2)
            {
                CSItemCountManager.Instance.MarkBetterEquipOld(itemInfo);
            }
            else
            {
                CSItemCountManager.Instance.RemoveUseItemFromQueue(itemInfo.id);
            }
        }
        else
        {
            this.Close();
        }
    }

    void OnUseItem(UnityEngine.GameObject go)
    {
        DealUse();
    }

    void DealUse()
    {
        if (null == mItemBase || null == mItemBase.infos)
        {
            return;
        }

        var itemInfo = mItemBase.infos;
        TABLE.ITEM item = null;
        if (!ItemTableManager.Instance.TryGetValue(itemInfo.configId, out item))
        {
            return;
        }

        if (item.type == 2)
        {
            WearEquip(itemInfo, item);
        }
        else
        {
            UseItem(itemInfo, item);
        }
    }

    void WearEquip(BagItemInfo itemInfo, TABLE.ITEM itemCfg)
    {
        this.Close();
        int wearPos = CSBagInfo.Instance.GetEquipWearPos(itemCfg);
        //这里立即压入穿戴的装备
        CSItemCountManager.Instance.PushEquipedList(itemInfo);
        Net.ReqEquipItemMessage(itemInfo.bagIndex, wearPos, 0, itemInfo);
        CSItemCountManager.Instance.MarkBetterEquipOld(itemInfo);
    }

    void UseItem(BagItemInfo itemInfo, TABLE.ITEM itemCfg)
    {
        this.Close();
        CSBagInfo.Instance.UseItem(itemInfo,itemInfo.count);
        CSItemCountManager.Instance.RemoveUseItemFromQueue(itemInfo.id);
    }

    public void Show(bag.BagItemInfo bagItemInfo,bool autoClose,int state)
    {
        mItemBase?.Refresh(bagItemInfo);
        //FNDebug.Log($"[getitem]:usecount = {bagItemInfo.count}");
        mItemBase.SetCount(bagItemInfo.count,false,false);
        mItemBase.ShowArrow(true);
        mItemBase.obj.SetActive(false);
        if (null != bagItemInfo && null != mName && null != mItemBase.itemCfg)
        {
            int quality = CSBagInfo.Instance.IsNormalEquip(mItemBase.itemCfg) ? bagItemInfo.quality : mItemBase.itemCfg.quality;
            mName.text = mItemBase.itemCfg.name.BBCode(quality);
        }

        if (null != mUsage)
        {
            if (null != mItemBase && null != mItemBase.itemCfg)
            {
                bool isEquip = mItemBase.itemCfg.type == 2;
                mUsage.text = CSString.Format(isEquip ? 1521 : 1522);
            }
        }

        if(null != mTitleUsage)
        {
            mTitleUsage.text = CSString.Format(state == 1 ? 2023 : 2024);
        }

        if (autoClose)
        {
            ScriptBinder.Invoke(6.0f, DealUse);
        }

        BindCoroutine(9988, DelayShow());

        if (null != mItemBase && !CSItemCountManager.Instance.HasUseTimes(mItemBase.itemCfg))
        {
            this.Close();
            CSItemCountManager.Instance.TryCallNextBetterEquip();
        }
    }
    IEnumerator DelayShow()
    {
        yield return null;
        mItemBase.obj.SetActive(true);

        if(null != mItemBase && null != mItemBase.infos && null != mItemBase.itemCfg)
        {
            if(mItemBase.itemCfg.type == 2)
            {
                CSGuideManager.Instance.Trigger(4, this, true);
            }
        }
    }

    protected void OnBetterEquipRemoved(uint id, object argv)
    {
        if (argv is bag.BagItemInfo bagItemInfo)
        {
            if (null != mItemBase && null != mItemBase.infos && mItemBase.infos.id == bagItemInfo.id)
            {
                this.Close();
                CSItemCountManager.Instance.TryCallNextBetterEquip();
            }
        }
    }

    protected void OnHintUsedItemCountChanged(uint id, object argv)
    {
        if (null == mItemBase)
            return;
        if (null == mItemBase.infos)
            return;
        var itemInfo = CSBagInfo.Instance.GetBagItemInfoById(mItemBase.infos.id);
        if(null != itemInfo)
        {
            if(!CSItemCountManager.Instance.HasUseTimes(mItemBase.itemCfg))
            {
                this.Close();
                CSItemCountManager.Instance.TryCallNextBetterEquip();
                return;
            }
            mItemBase?.Refresh(itemInfo);
            //FNDebug.Log($"[getitem]:usecount = {bagItemInfo.count}");
            mItemBase.SetCount(itemInfo.count, false, false);
            mItemBase.ShowArrow(true);
            mItemBase.obj.SetActive(false);
            BindCoroutine(9988, DelayShow());
        }
    }

    protected void OnItemUsedTimesChanged(uint id,object argv)
    {
        if (null == mItemBase)
            return;

        if (null == mItemBase.infos)
            return;

        var itemInfo = CSBagInfo.Instance.GetBagItemInfoById(mItemBase.infos.id);
        if (null != itemInfo)
        {
            if (!CSItemCountManager.Instance.HasUseTimes(mItemBase.itemCfg))
            {
                this.Close();
                CSItemCountManager.Instance.TryCallNextBetterEquip();
            }
        }
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mBg);
        //mClientEvent.SendEvent(CEvent.GetItemPanelClose);
        mClientEvent.RemoveEvent(CEvent.OnBetterEquipRemoved, OnBetterEquipRemoved);
        mClientEvent.RemoveEvent(CEvent.OnHintUsedItemCountChanged, OnHintUsedItemCountChanged);
        mClientEvent.RemoveEvent(CEvent.OnItemUsedTimesChanged, OnItemUsedTimesChanged);
        if (null != mItemBase)
        {
            UIItemManager.Instance.RecycleSingleItem(mItemBase);
            mItemBase = null;
        }
        base.OnDestroy();
    }
}
