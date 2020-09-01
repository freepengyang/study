using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using bag;

public partial class UIGuildBagDonatePanel : UIBasePanel
{
    public enum FamilyBagType
    {
        Donate, //捐献
        Exchange, //物品
    }

    private int CurDisposeCount = 1;
    private BagItemInfo mBagItemInfo;
    private FamilyBagType mBagType;
    protected UIItemBase mUIItem;

    public override void Init()
    {
        base.Init();

        mUIItem = UIItemManager.Instance.GetItem(PropItemType.Normal,mItemParent.transform);

        mbtnBG.onClick = this.Close;
        mBtnadd.onClick = OnAddItemClick;
        mBtnreduce.onClick = OnReduceItemClick;
        mBtndispose.onClick = OnDisposeClick;
        mBtncancel.onClick = this.Close;

        EventDelegate.Add(mInputCount.onChange, OnRefreshCount);
    }

    public void RefreshUI(FamilyBagType bagType, BagItemInfo itemInfo)
    {
        if (itemInfo == null) return;

        mBagItemInfo = itemInfo;
        mBagType = bagType;
        mUIItem.Refresh(itemInfo);
        mLabitemValue.text = GetSingleEquipContribute().ToString();
        switch (bagType)
        {
            case FamilyBagType.Donate:
                mLabtitle.text = CSString.Format(912);
                mLabdispose.text = CSString.Format(912);
                break;
            case FamilyBagType.Exchange:
                mLabtitle.text = CSString.Format(913);
                mLabdispose.text = CSString.Format(913);
                break;
        }
    }

    public override void Show()
    {
        base.Show();
        
    }

    private void OnAddItemClick(GameObject go)
    {
        if (CurDisposeCount < mBagItemInfo.count)
        {
            CurDisposeCount+=1;
        }
        else if (CurDisposeCount > mBagItemInfo.count)
        {
            CurDisposeCount = mBagItemInfo.count;
        }
        else
        {
            CurDisposeCount = 1;
        }

        mInputCount.value = CurDisposeCount.ToString();
        mLabitemValue.text = (CurDisposeCount * GetSingleEquipContribute()).ToString();
    }

    private void OnReduceItemClick(GameObject go)
    {
        if (mInputCount.value == "1")
        {
            CurDisposeCount = mBagItemInfo.count;
        }
        else
        {
            if (CurDisposeCount > 1)
            {
                CurDisposeCount -= 1;
            }
            else
            {
                CurDisposeCount = 1;
            }
        }

        mInputCount.value = CurDisposeCount.ToString();
        mLabitemValue.text = (CurDisposeCount * GetSingleEquipContribute()).ToString();
    }

    private void OnRefreshCount()
    {
        if (string.IsNullOrEmpty(mInputCount.value)) return;
        int count = 0;
        if (!int.TryParse(mInputCount.value, out count) || count < 0)
        {
            count = 1;
            mInputCount.value = "1";
        }

        if (count > mBagItemInfo.count)
        {
            count = mBagItemInfo.count;
            mInputCount.value = mBagItemInfo.count.ToString();
        }

        count = int.Parse(mInputCount.value);
        mLabitemValue.text = (count * GetSingleEquipContribute()).ToString();
        CurDisposeCount = count;
    }

    private void OnDisposeClick(GameObject go)
    {
        if (!int.TryParse(mInputCount.value, out CurDisposeCount))
        {
            CurDisposeCount = 1;
            mInputCount.value = 1.ToString();
        }

        switch (mBagType)
        {
            case FamilyBagType.Donate:
                Net.CSUnionDonateEquipMessage(mBagItemInfo.bagIndex, CurDisposeCount);
                break;
            case FamilyBagType.Exchange:
                int contribute = GetSingleEquipContribute() * CurDisposeCount;
                long owned = CSItemCountManager.Instance.GetItemCount((int)MoneyType.unionAttribute);
                if (contribute > owned)
                {
                    UtilityTips.ShowRedTips(914);
                    return;
                }
                Net.CSUnionExchangeEquipMessage(mBagItemInfo.id, CurDisposeCount);
                break;
        }

        this.Close();
    }

    private int GetSingleEquipContribute()
    {
        TABLE.ITEM tblItem;
        if (ItemTableManager.Instance.TryGetValue(mBagItemInfo.configId, out tblItem))
        {
            return tblItem.uniondonate;
        }
        return 0;
    }

    protected override void OnDestroy()
    {
        if(null != mUIItem)
        {
            UIItemManager.Instance.RecycleSingleItem(mUIItem);
            mUIItem = null;
        }
        base.OnDestroy();
    }
}
