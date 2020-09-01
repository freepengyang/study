using CSPool;
using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIMaterialBiner : UIBinder
{
    protected UISprite sp_quality;
    protected UISprite sp_icon;
    protected UILabel lb_count;
    public override void Init(UIEventListener handle)
    {
        sp_icon = Get<UISprite>("sp_icon");
        sp_quality = Get<UISprite>("sp_quality");
        lb_count = Get<UILabel>("lb_count");
        HotManager.Instance.EventHandler.AddEvent(CEvent.ItemChange, OnItemChanged);
        HotManager.Instance.EventHandler.AddEvent(CEvent.MoneyChange, OnItemChanged);
    }

    public override void OnDestroy()
    {
        if(null != Handle)
        {
            Handle.onClick = null;
            Handle.onDoubleClick = null;
        }
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.ItemChange, OnItemChanged);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.MoneyChange, OnItemChanged);
        sp_quality = null;
        sp_icon = null;
        lb_count = null;
    }

    protected void OnLink2AcquiredMethod(GameObject gameObject)
    {
        if(null != costItemData)
        {
#if UNITY_EDITOR && ENABLE_DEBUG
            Net.GMCommand($"@i {costItemData.cfgId} {100} 0");
#else
            Utility.ShowGetWay(costItemData.cfgId);
#endif
        }
    }

    protected void OnShowItemTips(GameObject gameObject)
    {
        if (null != costItemData)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal,costItemData.item);
        }
    }

    protected void OnGMCmd(GameObject go)
    {
        if(null != costItemData)
        {
            Net.GMCommand($"@i {costItemData.cfgId} {100} 0");
        }
    }

    protected void OnItemChanged(uint uiEvtID, object data)
    {
        if (null == costItemData)
            return;

        if (data is EventData eventData)
        {
            bag.BagItemInfo info = (bag.BagItemInfo)eventData.arg1;
            ItemChangeType type = (ItemChangeType)eventData.arg2;
            if (null != info && null != this.costItemData && info.configId == this.costItemData.cfgId)
            {
                Bind(costItemData);
            }
        }
        else if (data is MoneyType money)
        {
            if ((int)money == this.costItemData.cfgId)
            {
                Bind(costItemData);
            }
        }
    }

    protected CostItemData costItemData;
    public override void Bind(object data)
    {
        costItemData = data as CostItemData;
        if (null != costItemData)
        {
            if (null != sp_icon)
                sp_icon.spriteName = costItemData.item.icon;
            long owned = costItemData.cfgId.GetItemCount();
            long needed = costItemData.count;
            bool enough = owned >= needed;
            if (null != lb_count)
                lb_count.text = $"{owned}/{needed}".BBCode(enough ? ColorType.Green : ColorType.Red);
            if (null != sp_quality)
                sp_quality.spriteName = ItemTableManager.Instance.GetItemQualityBG(costItemData.item.quality);
            if (null != Handle)
            {
                if (!enough)
                    Handle.onClick = OnLink2AcquiredMethod;
                else
                    Handle.onClick = OnShowItemTips;
#if UNITY_EDITOR
                Handle.onDoubleClick = OnGMCmd;
#endif
            }
        }
        else
        {
            if (null != Handle)
                Handle.onClick = null;
            if (null != Handle)
                Handle.onDoubleClick = null;
        }
    }
}

public class UIMoneyBinder : UIBinder
{
    protected UISprite sp_icon;
    protected UILabel lb_count;
    protected UIEventListener btn_add;
#if UNITY_EDITOR
    protected UIEventListener btn_money;
#endif
    public override void Init(UIEventListener handle)
    {
        sp_icon = Get<UISprite>("sp_icon");
        lb_count = Get<UILabel>("lb_count");
        btn_add = Get<UIEventListener>("btn_add");
#if UNITY_EDITOR
        btn_money = UIEventListener.Get(sp_icon.gameObject);
        btn_money.onClick = OnGMCmd;
#endif
        HotManager.Instance.EventHandler.AddEvent(CEvent.ItemChange, OnItemChanged);
        HotManager.Instance.EventHandler.AddEvent(CEvent.MoneyChange, OnItemChanged);
    }

    public override void OnDestroy()
    {
        if(null != Handle)
            Handle.onClick = null;
        if (null != btn_add)
            btn_add.onClick = null;
        costItemData = null;
#if UNITY_EDITOR
        btn_money.onClick = null;
        btn_money = null;
#endif
        if(null != btn_add)
            btn_add.onClick = null;
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.ItemChange, OnItemChanged);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.MoneyChange, OnItemChanged);
        sp_icon = null;
        lb_count = null;
    }

    protected void OnGMCmd(GameObject go)
    {
        /*return;
        if (null != costItemData)
        {
            Net.GMCommand($"@i {costItemData.cfgId} 100 0");
        }*/
    }

    protected void OnItemChanged(uint uiEvtID, object data)
    {
        if (null == costItemData)
            return;

        if (data is EventData eventData)
        {
            bag.BagItemInfo info = (bag.BagItemInfo)eventData.arg1;
            ItemChangeType type = (ItemChangeType)eventData.arg2;
            if (null != info && null != this.costItemData && info.configId == this.costItemData.cfgId)
            {
                Bind(costItemData);
            }
        }
        else if (data is MoneyType money)
        {
            if ((int)money == this.costItemData.cfgId)
            {
                Bind(costItemData);
            }
        }
    }

    protected void OnLink2AcquiredMethod(GameObject gameObject)
    {
        if (null != costItemData)
        {
            Utility.ShowGetWay(costItemData.cfgId);
        }
    }

    protected CostItemData costItemData;
    public override void Bind(object data)
    {
        costItemData = data as CostItemData;
        if (null != costItemData)
        {
            if (null != sp_icon)
                sp_icon.spriteName = costItemData.item.SmallIcon();
            long owned = costItemData.cfgId.GetItemCount();
            long needed = costItemData.count;
            bool enough = owned >= needed;
            if (null != lb_count)
            {
                if (costItemData.HasMoneyFlag((int)CostItemMoneyFlag.CIMF_COMPARE))
                {
                    if(costItemData.HasMoneyFlag((int)CostItemMoneyFlag.CIMF_RED_GREEN))
                        lb_count.text = $"{Express(owned)}/{Express(needed)}".BBCode(enough ? ColorType.Green : ColorType.Red);
                    else
                        lb_count.text = $"{Express(owned)}/{Express(needed)}";
                }
                else
                {
                    if (costItemData.HasMoneyFlag((int)CostItemMoneyFlag.CIMF_RED_GREEN))
                        lb_count.text = $"{Express(needed)}".BBCode(enough ? ColorType.Green : ColorType.Red);
                    else
                        lb_count.text = $"{Express(needed)}";
                }
            }
            if (!enough)
                Handle.onClick = OnLink2AcquiredMethod;
            else
                Handle.onClick = null;
            if (null != btn_add)
            {
                btn_add.onClick = OnLink2AcquiredMethod;
            }
        }
        else
        {
            if (null != btn_add)
                btn_add.onClick = null;
        }
    }

    public string Express(long value)
    {
        if (costItemData.HasMoneyFlag((int)CostItemMoneyFlag.CIMF_SHORT_EXPRESS))
        {
            return UtilityMath.GetDecimalValue(value);
        }
        else if (costItemData.HasMoneyFlag((int)CostItemMoneyFlag.CIMF_SHORT_EXPRESS_ONE_POINT))
        {
            return UtilityMath.GetDecimalValue(value, "F1");
        }
        else if (costItemData.HasMoneyFlag((int)CostItemMoneyFlag.CIMF_SHORT_EXPRESS_TWO_POINT))
        {
            return UtilityMath.GetDecimalValue(value, "F2");
        }
        else
        {
            return value.ToString();
        }
    }
}

public static class UICostItemBinderExtend
{
    public static void BindCostItems(this GameObject gameObject, PoolHandleManager poolHandle,RepeatedField<TABLE.KEYVALUE> kvs,int moneyFlag = (int)CostItemMoneyFlag.CIMF_RED_GREEN)
    {
        if (null != gameObject)
        {
            var handle = UIEventListener.Get(gameObject);
            if (!(handle.parameter is UICostItemBinder costBinder))
            {
                costBinder = poolHandle.GetSystemClass<UICostItemBinder>();
                costBinder.Setup(handle, poolHandle);
            }
            costBinder.MoneyFlag = moneyFlag;
            costBinder.Bind(kvs);
        }
    }
    public static void BindCostItems(this GameObject gameObject, PoolHandleManager poolHandle, LongArray kvs, int moneyFlag = (int)CostItemMoneyFlag.CIMF_RED_GREEN)
    {
        if (null != gameObject)
        {
            var handle = UIEventListener.Get(gameObject);
            if (!(handle.parameter is UICostItemBinder costBinder))
            {
                costBinder = poolHandle.GetSystemClass<UICostItemBinder>();
                costBinder.Setup(handle, poolHandle);
            }
            costBinder.MoneyFlag = moneyFlag;
            costBinder.Bind(kvs);
        }
    }

    public static void UnBindCostItems(this GameObject gameObject)
    {
        if (null != gameObject)
        {
            UIEventListener handle = gameObject.GetComponent<UIEventListener>();
            if (null != handle && handle.parameter is UICostItemBinder costBinder)
            {
                costBinder.Destroy();
            }
        }
    }
}

public class UICostItemBinder : UIBinder
{
    protected UIGridContainer materialContainer;
    protected UIGridContainer moneyContainer;
    protected LongArray content;
    public int MoneyFlag
    {
        get;set;
    }
    List<CostItemData> mCostItems;

    public override void Init(UIEventListener handle)
    {
        //材料
        materialContainer = Get<UIGridContainer>("materials");
        //货币
        moneyContainer = Get<UIGridContainer>("moneys");
    }

    protected void Recycle()
    {
        //回收数据
        if (null != mCostItems)
        {
            for (int i = 0; i < mCostItems.Count; ++i)
                PoolHandle.Recycle(mCostItems[i]);
            mCostItems.Clear();
            PoolHandle.Recycle(mCostItems);
            mCostItems = null;
        }
    }

    public override void Bind(object data)
    {
        Recycle();
        materialContainer.MaxCount = 0;
        moneyContainer.MaxCount = 0;
        this.content = (LongArray)data;
        //if(null != this.content)
        {
            mCostItems = this.content.GetCostItems(PoolHandle);

            var materialDatas = PoolHandle.GetSystemClass<List<CostItemData>>();
            var moneyDatas = PoolHandle.GetSystemClass<List<CostItemData>>();
            //设置材料与货币
            for (int i = 0; i < mCostItems.Count; ++i)
            {
                if(mCostItems[i].item.type == 1)
                {
                    moneyDatas.Add(mCostItems[i]);
                }
                else
                {
                    materialDatas.Add(mCostItems[i]);
                }
            }
            materialContainer.MaxCount = materialDatas.Count;
            //设置材料
            for(int i = 0; i < materialContainer.MaxCount; ++i)
            {
                var handle = UIEventListener.Get(materialContainer.controlList[i]);
                if(!(handle.parameter is UIMaterialBiner materialBinder))
                {
                    materialBinder = PoolHandle.GetSystemClass<UIMaterialBiner>();
                    materialBinder.Setup(handle,PoolHandle);
                }
                materialBinder.Bind(materialDatas[i]);
            }
            materialDatas.Clear();
            PoolHandle.Recycle(materialDatas);

            moneyContainer.MaxCount = moneyDatas.Count;
            //设置货币
            for (int i = 0; i < moneyContainer.MaxCount; ++i)
            {
                var handle = UIEventListener.Get(moneyContainer.controlList[i]);
                if (!(handle.parameter is UIMoneyBinder moneyBinder))
                {
                    moneyBinder = PoolHandle.GetSystemClass<UIMoneyBinder>();
                    moneyBinder.Setup(handle,PoolHandle);
                }
                moneyDatas[i].moneyFlag = this.MoneyFlag;
                moneyBinder.Bind(moneyDatas[i]);
            }
            moneyDatas.Clear();
            PoolHandle.Recycle(moneyDatas);
        }
    }

    public override void OnDestroy()
    {
        Recycle();
        materialContainer?.UnBind<UIMaterialBiner>();
        materialContainer = null;
        moneyContainer?.UnBind<UIMoneyBinder>();
        moneyContainer = null;
    }
}
