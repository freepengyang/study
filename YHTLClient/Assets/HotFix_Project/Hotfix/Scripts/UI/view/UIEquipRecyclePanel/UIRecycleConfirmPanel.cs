using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRecycleConfirmPanel : UIBasePanel
{
    UIEventListener mBtnOK;
    UIEventListener mBtnCancel;
    UIEventListener mBtnCheck;
    UnityEngine.Transform mGridItems;
    UIGridContainer mGridIncome;
    UIEventListener mBtnClose;
    UnityEngine.GameObject mGoMark;
    UIGrid mUIGrid;
    UnityEngine.GameObject mItemTemplate;
    protected override void _InitScriptBinder()
    {
        mBtnOK = ScriptBinder.GetObject("BtnOK") as UIEventListener;
        mBtnCancel = ScriptBinder.GetObject("BtnCancel") as UIEventListener;
        mBtnCheck = ScriptBinder.GetObject("BtnCheck") as UIEventListener;
        mGridItems = ScriptBinder.GetObject("GridItems") as UnityEngine.Transform;
        mGridIncome = ScriptBinder.GetObject("GridIncome") as UIGridContainer;
        mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
        mGoMark = ScriptBinder.GetObject("GoMark") as UnityEngine.GameObject;
        mUIGrid = ScriptBinder.GetObject("UIGrid") as UIGrid;
        mItemTemplate = ScriptBinder.GetObject("ItemTemplate") as UnityEngine.GameObject;
    }
    bool _mark = false;
    protected bool Marked
    {
        get
        {
            return _mark;
        }
        set
        {
            if (_mark != value)
            {
                _mark = value;
                mGoMark.SetActive(value);

                mChoicedItems.Clear();
                for (int i = 0; i < mUiItems.Count; ++i)
                {
                    var item = mUiItems[i];
                    if (null == item)
                    {
                        item.ShowSelect(false);
                    }
                    else if (null == item.infos)
                    {
                        item.ShowSelect(false);
                    }
                    else
                    {
                        if(value)
                            mChoicedItems.Add(item.infos.id);
                        item.ShowSelect(value);
                    }
                }

                RefreshAwards();
            }
        }
    }
    protected List<UIItemBase> mUiItems;

    public override void Init()
    {
        base.Init();
        _mark = false;
        mUiItems = mPoolHandleManager.GetSystemClass<List<UIItemBase>>();

        mBtnCancel.onClick = mBtnClose.onClick = f =>
        {
            UIManager.Instance.ClosePanel<UIRecycleConfirmPanel>();
        };

        mBtnCheck.onClick = go =>
        {
            Marked = !Marked;
        };
        if(null != mGridIncome && null != mGridIncome.controlTemplate)
        {
            mGridIncome.controlTemplate.SetActive(false);
            mGridIncome.controlTemplate = null;
        }
        mBtnOK.onClick = OnRecycle;
    }

    protected void OnRecycle(GameObject go)
    {
        List<int> bagIndexs = mPoolHandleManager.GetSystemClass<List<int>>();
        for (int i = 0; i < mUiItems.Count; ++i)
        {
            var itemBase = mUiItems[i];
            if(null != itemBase && null != itemBase.infos)
            {
                if(mChoicedItems.Remove(itemBase.infos.id))
                {
                    itemBase.ShowSelect(false);
                    bagIndexs.Add(itemBase.infos.bagIndex);
                }
            }
        }

        if(bagIndexs.Count <= 0)
        {
            UtilityTips.ShowRedTips(353);
            bagIndexs.Clear();
            mChoicedItems.Clear();
            mPoolHandleManager.Recycle(bagIndexs);
            return;
        }
        Net.ReqCallBackItemMessage(bagIndexs);
        bagIndexs.Clear();
        mChoicedItems.Clear();
        mPoolHandleManager.Recycle(bagIndexs);
        UIManager.Instance.ClosePanel<UIRecycleConfirmPanel>();
    }

    public override void Show()
    {
        base.Show();

        mItemTemplate.SetActive(false);
        mGoMark.SetActive(_mark);
    }

    protected List<bag.BagItemInfo> mEquipItems;
    public void BindData(List<bag.BagItemInfo> equipItems)
    {
        this.mEquipItems = mPoolHandleManager.GetSystemClass<List<bag.BagItemInfo>>();
        this.mEquipItems.AddRange(equipItems);
        Rebuild();
    }

    protected void Rebuild()
    {
        RefreshEquips();
        RefreshAwards();
    }

    protected void RefreshEquips()
    {
        if(mUiItems.Count > mEquipItems.Count)
        {
            for(int i = mEquipItems.Count; i < mUiItems.Count;++i)
            {
                UIItemManager.Instance.RecycleSingleItem(mUiItems[i]);
                mUiItems.RemoveRange(mEquipItems.Count, mUiItems.Count - mEquipItems.Count);
            }
        }
        else if(mUiItems.Count < mEquipItems.Count)
        {
            for (int i = mUiItems.Count; i < mEquipItems.Count; ++i)
            {
                var itemBase = UIItemManager.Instance.GetItem(PropItemType.Recycle, mGridItems);
                mUiItems.Add(itemBase);
            }
        }

        for (int i = 0; i < mUiItems.Count; i++)
        {
            mUiItems[i].Refresh(mEquipItems[i], OnItemClicked);
        }
        mUIGrid.Reposition();
    }

    protected HashSet<long> mChoicedItems = new HashSet<long>();
    protected void OnItemClicked(UIItemBase item)
    {
        if(null != item && null != item.infos)
        {
            long id = item.infos.id;
            if(mChoicedItems.Contains(item.infos.id))
            {
                mChoicedItems.Remove(id);
                item.ShowSelect(false);
            }
            else
            {
                mChoicedItems.Add(id);
                item.ShowSelect(true);
            }
            RefreshAwards();
        }
    }

    protected void RefreshAwards()
    {
        FNDebug.LogFormat("RefreshAwards");
        //TODO:2 奖励是0的不需要过滤掉
        //TODO:3 奖励要做战斗力比对(这个需要在外面做)
        List<ItemBarData> itemBarDatas = mPoolHandleManager.GetSystemClass<List<ItemBarData>>();
        CSItemRecycleInfo.Instance.BeginAwards();
        List<RecycleCollectionData> recycleCollectionDatas = mPoolHandleManager.GetSystemClass<List<RecycleCollectionData>>();
        for (int i = 0; i < mUiItems.Count; ++i)
        {
            var item = mUiItems[i];
            if(null != item && null != item.infos && mChoicedItems.Contains(item.infos.id))
            {
                CSItemRecycleInfo.Instance.GetAwards(mUiItems[i].infos.configId);
            }
        }
        CSItemRecycleInfo.Instance.EndAwards(recycleCollectionDatas);
        for(int i = 0; i < recycleCollectionDatas.Count; ++i)
        {
            var itemData = UIItemBarManager.Instance.Get();
            itemData.cfgId = recycleCollectionDatas[i].item.id;
            itemData.needed = recycleCollectionDatas[i].count;
            itemData.owned = 0;
            itemData.flag = (int)ItemBarData.ItemBarType.IBT_SMALL_ICON | (int)ItemBarData.ItemBarType.IBT_ONLY_COST;
            itemData.eventHandle = null;
            itemBarDatas.Add(itemData);
        }
        CSItemRecycleInfo.Instance.RecycleDatas(recycleCollectionDatas);
        mPoolHandleManager.Recycle(recycleCollectionDatas);
        UIItemBarManager.Instance.Bind(mGridIncome,itemBarDatas);
        itemBarDatas.Clear();
        mPoolHandleManager.Recycle(itemBarDatas);
    }

    protected override void OnDestroy()
    {
        UIItemBarManager.Instance.UnBind(mGridIncome);
        mGridIncome = null;
        for(int i = 0; i < mUiItems.Count; ++i)
        {
            UIItemManager.Instance.RecycleSingleItem(mUiItems[i]);
        }
        mUiItems.Clear();
        mPoolHandleManager.Recycle(mUiItems);
        mUiItems = null;
        mEquipItems.Clear();
        mPoolHandleManager.Recycle(mEquipItems);
        mEquipItems = null;
        base.OnDestroy();
    }
}
