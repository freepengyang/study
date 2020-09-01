using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using TABLE;
using UnityEngine;

public partial class UIGuildPracticePanel : UIBasePanel
{
    protected CSPool.Pool<AttrItemData> starAttrDatas;
    protected FastArrayElementFromPool<GuildBufferItemData> mGuildBufferDatas;

    public override void Init()
	{
		base.Init();

        mClientEvent.AddEvent(CEvent.OnGuildPracticeInitialized, OnGuildPracticeInitialized);
        mClientEvent.AddEvent(CEvent.OnGuildPracticeImproved, OnGuildPracticeImproved);
        mbtn_upAttr.onClick = OnUprgadeAttr;

        starAttrDatas = GetListPool<AttrItemData>();
        mGuildBufferDatas = mPoolHandleManager.CreateGeneratePool<GuildBufferItemData>();

        InitPanelBg();

        Net.CSImproveInfosMessage();

        RegisterRed(mPractiseRedPoint, RedPointType.GuildPractice);
    }

    public override void Show()
    {
        base.Show();

        RebuildCostItems();
        RefreshPropList();
        RefreshAttrList();
        RefreshBaseItems();
    }

    void OnGuildPracticeInitialized(uint id,object argv)
    {
        RebuildCostItems();
        RefreshPropList();
        RefreshAttrList();
        RefreshBaseItems();
    }

    private void OnGuildPracticeImproved(uint id,object argv)
    {
        TABLE.UNIONBUFF improveItem = null;
        if (!UnionBuffTableManager.Instance.TryGetValue(CSGuildInfo.Instance.CurImproveId, out improveItem) || null == improveItem)
        {
            return;
        }

        //UtilityTips.ShowLeftDownTips
        //if (unionBuff != null)
        //    Utility.ShowGameInfo(100304, unionBuff.cost);

        RebuildCostItems();
        RefreshPropList();
        RefreshAttrList();
        RefreshBaseItems();
    }

    protected override void OnDestroy()
    {
        mcost_items?.UnBindCostItems();
        mcost_items = null;
        mGuildBufferDatas?.Clear();
        mGuildBufferDatas = null;
        starAttrDatas = null;
        if (null != mgrid_effects)
        {
            mgrid_effects.UnBind<AttrItem>();
            mgrid_effects = null;
        }
        var cachedTransform = mGoPropsRoot.transform;
        for (int i = 0,max = cachedTransform.childCount;i < max;++i)
        {
            cachedTransform.GetChild(i).gameObject.DestroyBinder<UIGuildBufferItemBinder>();
        }
        //CSEffectPlayMgr.Instance.Recycle(mTexBg1.gameObject);
        CSEffectPlayMgr.Instance.Recycle(mTexBg2.gameObject);
        mClientEvent.RemoveEvent(CEvent.OnGuildPracticeInitialized, OnGuildPracticeInitialized);
        mClientEvent.RemoveEvent(CEvent.OnGuildPracticeImproved, OnGuildPracticeImproved);
        base.OnDestroy();
    }

    protected void RefreshBaseItems()
    {
        bool isLevelFull = CSGuildInfo.Instance.ImproveFull;
        mbtn_upAttr.CustomActive(!isLevelFull);
        mGoMaxLV.CustomActive(isLevelFull);
    }

    protected void RefreshAttrList(bool needEffect = false)
    {
        bool isLevelFull = CSGuildInfo.Instance.ImproveFull;
        var datas = mPoolHandleManager.GetSystemClass<List<AttrItemData>>();
        starAttrDatas.RecycleAllItems();
        RepeatedField<KEYVALUE> pairKvs = mPoolHandleManager.GetSystemClass<RepeatedField<KEYVALUE>>();
        pairKvs.Clear();
        RepeatedField<KEYVALUE> nextPairKvs = mPoolHandleManager.GetSystemClass<RepeatedField<KEYVALUE>>();
        nextPairKvs.Clear();
        int career = CSMainPlayerInfo.Instance.Career;
        var positions = CSGuildInfo.Instance.PracticePositions;
        List<int> linkIds = mPoolHandleManager.GetSystemClass<List<int>>();
        linkIds.Clear();
        int nextPosition = CSGuildInfo.Instance.NextPosition(CSGuildInfo.Instance.CurImproveId);
        for (int i = 0; i < positions.Count; ++i)
        {
            var position = positions[i];
            int v = CSGuildInfo.Instance.GetImproveLevel(position);
            int curImproveId = UnionBuffTableManager.Instance.make_id(position, v);
            TABLE.UNIONBUFF curImproveItem = null;
            if(!UnionBuffTableManager.Instance.TryGetValue(curImproveId, out curImproveItem))
            {
                continue;
            }

            int nextImproveId = position == nextPosition ? CSGuildInfo.Instance.NextLevelImprove(curImproveId) : curImproveId;
            TABLE.UNIONBUFF nextImproveItem = null;
            if (!UnionBuffTableManager.Instance.TryGetValue(nextImproveId, out nextImproveItem))
            {
                continue;
            }

            if(curImproveItem.attr.Count <= 0)
            {
                continue;
            }

            if(curImproveItem.attr.Count != nextImproveItem.attr.Count)
            {
                FNDebug.LogErrorFormat("行会UnionBuff表填写错误,属性列ATTR填写错误，列数量不一致 ID = {0} 与 ID = {1}", curImproveItem.id, nextImproveItem.id);
                continue;
            }

            int idx = career - 1;
            if (idx < 0)
                continue;
            idx = Math.Min(idx,curImproveItem.attr.Count - 1);

            if(position == nextPosition && needEffect)
            {
                if(!linkIds.Contains(curImproveItem.attr[idx].key()))
                {
                    linkIds.Add(curImproveItem.attr[idx].key());
                }
            }

            pairKvs.Add(curImproveItem.attr[idx],mPoolHandleManager);
            nextPairKvs.Add(nextImproveItem.attr[idx], mPoolHandleManager);
        }

        var kvs = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, pairKvs,true,false);
        var nextKvs = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, nextPairKvs, true, false);
        for (int i = 0; i < kvs.Count; ++i)
        {
            //if (kvs[i].IsZeroValue && nextKvs[i].IsZeroValue)
            //{
            //    kvs[i].OnRecycle(mPoolHandleManager);
            //    nextKvs[i].OnRecycle(mPoolHandleManager);
            //    continue;
            //}

            bool hasLinkedId = false;
            if(needEffect)
            {
                for (int j = 0; j < kvs[i].keyValues.Count && !hasLinkedId; ++j)
                {
                    hasLinkedId = linkIds.Contains(kvs[i].keyValues[j].attrItem.id);
                }
            }

            var currentValue = starAttrDatas.Get();
            datas.Add(currentValue);
            currentValue.isLevelFull = isLevelFull;
            currentValue.pooledHandle = mPoolHandleManager;
            currentValue.keyValue = kvs[i];
            currentValue.nKeyValue = nextKvs[i];
            currentValue.needEffect = needEffect && kvs[i].HasDiff(nextKvs[i]) && hasLinkedId;
        }

        linkIds.Clear();
        mPoolHandleManager.Recycle(linkIds);

        mgrid_effects.Bind<AttrItemData, AttrItem>(datas, mPoolHandleManager);
        datas.Clear();
        mPoolHandleManager.Recycle(datas);

        kvs.Clear();
        mPoolHandleManager.Recycle(kvs);
        nextKvs.Clear();
        mPoolHandleManager.Recycle(nextKvs);

        pairKvs.OnRecycle(mPoolHandleManager);
        nextPairKvs.OnRecycle(mPoolHandleManager);
    }

    protected void RefreshPropList()
    {
        mGuildBufferDatas.Clear();
        var positions = CSGuildInfo.Instance.PracticePositions;
        int career = CSMainPlayerInfo.Instance.Career;
        for (int i = 0; i < positions.Count; ++i)
        {
            var position = positions[i];
            int v = CSGuildInfo.Instance.GetImproveLevel(position);
            int curImproveId = UnionBuffTableManager.Instance.make_id(position, v);
            TABLE.UNIONBUFF curImproveItem = null;
            if (!UnionBuffTableManager.Instance.TryGetValue(curImproveId, out curImproveItem))
            {
                continue;
            }

            if (curImproveItem.attr.Count <= 0)
            {
                continue;
            }

            int idx = career - 1;
            if (idx < 0)
                continue;
            idx = Math.Min(idx, curImproveItem.attr.Count - 1);

            var attr = curImproveItem.attr[idx];
            //if(null == attr)
            //    continue;

            var propNode = mGoPropsRoot.transform.GetChild(i);
            if(null == propNode)
            {
                continue;
            }

            var binder = propNode.gameObject.GetOrAddBinder<UIGuildBufferItemBinder>(mPoolHandleManager);
            if(null == binder)
            {
                continue;
            }

            var data = mGuildBufferDatas.Append();
            data.buffer = curImproveItem;
            data.position = position;
            data.attr = attr;
            binder.Bind(data);
        }
    }

    protected void RebuildCostItems()
    {
        if(CSGuildInfo.Instance.ImproveFull)
        {
            mcost_items.CustomActive(false);
        }
        else
        {
            TABLE.UNIONBUFF improveItem = null;
            if(!UnionBuffTableManager.Instance.TryGetValue(CSGuildInfo.Instance.NextImproveId,out improveItem))
            {
                mcost_items.CustomActive(false);
                return;
            }
            
            mcost_items.BindCostItems(mPoolHandleManager, improveItem.cost,(int)CostItemMoneyFlag.CIMF_COMPARE| (int)CostItemMoneyFlag.CIMF_RED_GREEN | (int)CostItemMoneyFlag.CIMF_SHORT_EXPRESS);
        }
    }

    void InitPanelBg()
    {
        //CSEffectPlayMgr.Instance.ShowUITexture(mTexBg1.gameObject, "fmxiulian1");
        CSEffectPlayMgr.Instance.ShowUITexture(mTexBg2.gameObject, "guild_bg1");
    }

    private void OnUprgadeAttr(GameObject button)
    {
        if (!CSGuildInfo.Instance.CheckImprove(true))
        {
            return;
        }

        RefreshAttrList(true);

        Net.CSImproveMessage(CSGuildInfo.Instance.NextPosition(CSGuildInfo.Instance.CurImproveId));
    }
}