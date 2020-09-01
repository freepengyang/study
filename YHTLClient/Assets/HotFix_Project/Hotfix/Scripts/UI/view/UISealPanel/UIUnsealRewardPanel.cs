using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIUnsealRewardPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get
        {
            return false;
        }
    }

    FastArrayElementFromPool<UIItemBase> mUIItems;

    Dictionary<int, int> boxItems;

    public override void Init()
    {
        base.Init();
        mbtn_determine.onClick = this.Close;
        mbtn_close.onClick = this.Close;
        mUIItems = mPoolHandleManager.CreateItemPool(PropItemType.Normal, mgrid_reward.transform);
    }

    public void Show(string str)
    {
        var kvs = mPoolHandleManager.GetEffectItems(mPoolHandleManager.Split(str));
        mUIItems.Clear();
        for (int i = 0; i < kvs.Count; ++i)
        {
            TABLE.ITEM itemCfg = ItemTableManager.Instance.GetItemCfg(kvs[i].key);
            var item = mUIItems.Append();
            item.Refresh(itemCfg, ItemClick);
            item.SetCount(kvs[i].value);
        }
        mgrid_reward.Reposition();
    }


    

    public void Show(int boxId)
    {
        if (boxItems == null) boxItems = new Dictionary<int, int>();
        else boxItems.Clear();
        BoxTableManager.Instance.GetBoxAwardById(boxId, boxItems);
        Show(boxItems);
    }

    public void Show(Dictionary<int,int> items)
    {
        mUIItems.Clear();
        for (var it = items.GetEnumerator(); it.MoveNext();)
        {
            TABLE.ITEM itemCfg = ItemTableManager.Instance.GetItemCfg(it.Current.Key);
            var item = mUIItems.Append();
            item.Refresh(itemCfg, ItemClick);
            item.SetCount(it.Current.Value);
        }
        mgrid_reward.Reposition();
    }


    void ItemClick(UIItemBase item)
    {
        if (item != null)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, item.itemCfg, item.infos);
        }
    }

    protected override void OnDestroy()
    {
        mUIItems?.Clear();
        mUIItems = null;
        boxItems?.Clear();
        boxItems = null;
        base.OnDestroy();
    }

}
