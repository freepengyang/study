using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleGetData : IndexedItem
{
    public int Index { get; set; }

    public TABLE.ITEM item;
    public int count;
}

public class RecycleGetDataBinder : UIBinder
{
    RecycleGetData data;
    UILabel lb_content;
    UISprite sp_icon;

    public override void Init(UIEventListener handle)
    {
        lb_content = Get<UILabel>("lb_content");
        sp_icon = Get<UISprite>("sp_icon");
    }

    public override void Bind(object data)
    {
        this.data = data as RecycleGetData;
        if(null != this.data)
        {
            if (null != lb_content)
                lb_content.text = $"{this.data.count}";
            if (null != sp_icon)
                sp_icon.spriteName = this.data.item.SmallIcon();
        }
    }

    public override void OnDestroy()
    {
        this.data = null;
        lb_content = null;
        sp_icon = null;
    }
}

public partial class UIRecycleGetPanel : UIDailyHideBasePanel
{
    FastArray<RecycleGetData> mItemDatas;
    public override void Init()
    {
        base.Init();
        AutoClose();
        AddCollider();
        mItemDatas = new FastArray<RecycleGetData>();
    }

    public override void Show()
    {
        base.Show();
    }

    public void Show(bag.CallbackItemResponse msg)
    {
        CSStringBuilder.Clear();
        var items = msg.callbackItems;
        var hashDic = mPoolHandleManager.GetSystemClass<Dictionary<int, int>>();
        hashDic.Clear();
        for (int i = 0; i < items.Count; ++i)
        {
            if (null == items[i])
            {
                continue;
            }

            var itemCfg = ItemTableManager.Instance.GetItemCfg(items[i].configId);
            if (null == itemCfg)
            {
                continue;
            }

            if(!hashDic.ContainsKey(itemCfg.id))
            {
                hashDic.Add(itemCfg.id, items[i].count);
            }
            else
            {
                hashDic[itemCfg.id] += items[i].count;
            }
        }
        var it = hashDic.GetEnumerator();
        while(it.MoveNext())
        {
            var itemCfg = ItemTableManager.Instance.GetItemCfg(it.Current.Key);
            if (null == itemCfg)
            {
                continue;
            }
            var data = mItemDatas.PushNewElementToTail();
            data.item = itemCfg;
            data.count = it.Current.Value;
        }
        hashDic.Clear();
        mPoolHandleManager.Recycle(hashDic);

        mAwardGrids.Bind<RecycleGetDataBinder,RecycleGetData>(mItemDatas);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
