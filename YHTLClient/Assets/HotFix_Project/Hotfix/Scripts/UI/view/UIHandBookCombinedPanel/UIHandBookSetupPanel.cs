using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CSAttributeInfo;

public partial class UIHandBookSetupPanel : UIBasePanel
{
    protected CSPool.Pool<SingleAttrData> starAttrDatas;

    public override void Init()
    {
        base.Init();
        starAttrDatas = GetListPool<SingleAttrData>();

        mClientEvent.AddEvent(CEvent.OnHandBookAdded, OnHandBookChanged);
        mClientEvent.AddEvent(CEvent.OnHandBookRemoved, OnHandBookChanged);
        mClientEvent.AddEvent(CEvent.OnHandBookUpgradeSucceed, OnHandBookChanged);
        mClientEvent.AddEvent(CEvent.OnHandBookInlayChanged, OnHandBookCounterChanged);
        mClientEvent.AddEvent(CEvent.OnHandBookSlotChanged, OnHandBookSlotChanged);
        mClientEvent.AddEvent(CEvent.OnPopUnlockHandBookMessageBox, OnPopUnlockHandBookMessageBox);
        mClientEvent.AddEvent(CEvent.OnAutoSetupHandBook, OnAutoSetupHandBook);

        mBtnHelp.onClick = OnClickHelp;
        mbtn_mypackage.onClick = OnOpenUIHandBookPackagePanel;
    }

    protected void OnOpenUIHandBookPackagePanel(UnityEngine.GameObject go)
    {
        UIManager.Instance.CreatePanel<UIHandBookPackagePanel>();
    }

    protected void OnClickHelp(UnityEngine.GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.HandBookSetup);
    }

    protected void OnHandBookChanged(uint id,object argv)
    {
        UpdateHandBookSlots();
        InitHandBookAttributes();
    }

    protected void OnHandBookCounterChanged(uint id, object argv)
    {
        UpdateHandBookSlots();
        InitHandBookAttributes();
        InitHandBookGroups();
    }

    protected void OnHandBookSlotChanged(uint id, object argv)
    {
        UpdateHandBookSlots();
    }

    protected void OnAutoSetupHandBook(uint id,object argv)
    {
        HandBookSlotData slot, card;
        if(CSHandBookManager.Instance.CheckHandBookSetupRedPoint(out slot, out card))
        {
            Net.CSTujianInlayMessage(card.Guid, slot.SlotID);
        }
    }

    protected void OnPopUnlockHandBookMessageBox(uint id,object argv)
    {
        HandBookSlotData handBookSlotData = null;
        if(CSHandBookManager.Instance.CheckExistNeedUnlockSlot(out handBookSlotData))
        {
            UICostPromptPanel.Open(392, CSHandBookManager.Instance.HandBookOpenSlotCostId, handBookSlotData.HandBookSlot.cost, () =>
            {
                if (!CSHandBookManager.Instance.HandBookOpenSlotCostId.IsItemEnough(handBookSlotData.HandBookSlot.cost))
                {
                    return true;
                }
                Net.CSActivateSlotWingMessage(handBookSlotData.SlotID);
                return true;
            });
        }
    }

    public override void Show()
    {
        base.Show();
        InitHandBookSlots();
        UpdateHandBookSlots();
        InitHandBookAttributes();
        InitHandBookGroups();
    }

    protected void InitHandBookSlots()
    {
        var datas = CSHandBookManager.Instance.HandBookSlotDatas;
        for(int i = 0; i < datas.Count; ++i)
        {
            datas[i].RemoveFlag(HandBookSlotData.CardFlag.CF_UNLOCK_EFFECT);
        }
    }

    protected void UpdateHandBookSlots()
    {
        var datas = CSHandBookManager.Instance.HandBookSlotDatas;
        for (int i = 0; i < datas.Count; ++i)
        {
            datas[i].RemoveFlag(HandBookSlotData.CardFlag.CF_ENABLE_SELECTED);
            datas[i].RemoveFlag(HandBookSlotData.CardFlag.CF_ENABLE_UNSELETED);
            datas[i].onChoiceChanged = null;
            if (datas[i].CheckCanSetupHandBookOnIt || datas[i].CheckCanUnlockForSlot)
                datas[i].AddFlag(HandBookSlotData.CardFlag.CF_REDPOINT);
            else
                datas[i].RemoveFlag(HandBookSlotData.CardFlag.CF_REDPOINT);
            datas[i].onClicked = OnCardClicked;
        }
        mgrid_cards.Bind<UIHandBookCardBinder, HandBookSlotData>(datas);
    }

    protected void InitHandBookAttributes()
    {
        CSHandBookManager.Instance.GetSetupedAttributes(mPoolHandleManager,OnAttributesVisible);
    }

    protected void InitHandBookGroups()
    {
        if(ScriptBinder.gameObject.activeInHierarchy)
        {
            BindCoroutine(1001, BindHandBookAnsyc());
        }
    }

    IEnumerator BindHandBookAnsyc()
    {
        var handBookDatas = CSHandBookManager.Instance.GetHandBookGroupDatas();
        void OnItemVisible(GameObject gameObject,int idx)
        {
            var binder = gameObject.GetOrAddBinder<HandBookGroupItemBinder>(mPoolHandleManager);
            binder.Bind(handBookDatas[idx]);
        }
        yield return mgrid_cardeffects.BindAsync(handBookDatas.Count,OnItemVisible);
        mgrid_cardeffects.UpdateWidgetCollider();
    }

    protected void OnAttributesVisible(RepeatedField<KeyValue> kvs)
    {
        var datas = mPoolHandleManager.GetSystemClass<List<SingleAttrData>>();
        for (int i = 0; i < kvs.Count; ++i)
        {
            if (kvs[i].IsZeroValue)
            {
                kvs[i].OnRecycle(mPoolHandleManager);
                continue;
            }
            var currentValue = starAttrDatas.Get();
            datas.Add(currentValue);
            currentValue.pooledHandle = mPoolHandleManager;
            currentValue.kv = kvs[i];
            currentValue.number = i;
        }
        if(null != mfixeHint)
            mfixeHint.SetActive(datas.Count <= 0);
        mgrid_attributes.Bind<SingleAttrData, SingleAttrItem>(datas, mPoolHandleManager);
        datas.Clear();
        mPoolHandleManager.Recycle(datas);
    }

    protected void OnCardClicked(HandBookSlotData data)
    {
        if (null == data)
            return;

        if(!data.Opened)
        {
            if (null == data.HandBookSlot)
            {
                UtilityTips.ShowRedTips(649);
                return;
            }

            if (!CSHandBookManager.Instance.CanUnlockSlot(data.SlotID,true))
            {
                return;
            }

            UICostPromptPanel.Open(392, CSHandBookManager.Instance.HandBookOpenSlotCostId, data.HandBookSlot.cost, () =>
            {
                if(!CSHandBookManager.Instance.HandBookOpenSlotCostId.IsItemEnough(data.HandBookSlot.cost))
                {
                    return true;
                }
                Net.CSActivateSlotWingMessage(data.SlotID);
                return true;
            });
            return;
        }

        if(null == data.HandBook)
        {
            UIManager.Instance.CreatePanel<UIHandBookCardSelectPanel>();
            return;
        }

        UIManager.Instance.CreatePanel<UIHandBookTipsPanel>(f =>
        {
            (f as UIHandBookTipsPanel).Show(data.HandBookId, data.Guid);
        });
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnHandBookAdded, OnHandBookChanged);
        mClientEvent.RemoveEvent(CEvent.OnHandBookRemoved, OnHandBookChanged);
        mClientEvent.RemoveEvent(CEvent.OnHandBookUpgradeSucceed, OnHandBookChanged);
        mClientEvent.RemoveEvent(CEvent.OnHandBookInlayChanged, OnHandBookCounterChanged);
        mClientEvent.RemoveEvent(CEvent.OnHandBookSlotChanged, OnHandBookSlotChanged);
        mClientEvent.RemoveEvent(CEvent.OnPopUnlockHandBookMessageBox, OnPopUnlockHandBookMessageBox);
        mClientEvent.RemoveEvent(CEvent.OnAutoSetupHandBook, OnAutoSetupHandBook);
        mgrid_cardeffects?.UnBind<HandBookGroupItemBinder>();
        mgrid_cardeffects = null;
        mgrid_cards?.UnBind<UIHandBookCardBinder>();
        mgrid_cards = null;
        mgrid_attributes.UnBind<SingleAttrItem>();
        mgrid_attributes = null;
        starAttrDatas = null;
        base.OnDestroy();
    }
}
