using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StarUpAttrItemData : CSPool.IPoolItem
{
    public PoolHandleManager pooledHandle;
    public CSAttributeInfo.KeyValue keyValue;
    public void OnRecycle()
    {
        if (null != keyValue)
        {
            keyValue.OnRecycle(pooledHandle);
            pooledHandle.Recycle(keyValue);
            keyValue = null;
        }
        pooledHandle = null;
    }
}

public class StarUpAttrItem : UIBinder
{
    protected UILabel lb_name;
    protected UILabel lb_value;
    protected StarUpAttrItemData mData;

    public override void Init(UIEventListener handle)
    {
        lb_name = Get<UILabel>("lb_name");
        lb_value = Get<UILabel>("lb_value");
    }

    public override void Bind(object data)
    {
        mData = data as StarUpAttrItemData;
        if (null != lb_name)
        {
            lb_name.text = string.Format(ClientTipsTableManager.Instance[55].context, mData.keyValue.Key);
        }
        if (null != lb_value)
        {
            lb_value.text = mData.keyValue.Value;
        }
    }

    public override void OnDestroy()
    {
        lb_name = null;
        lb_value = null;
        mData = null;
    }
}

public partial class UITimeExpStageUpPanel : UIDailyHideBasePanel
{
    protected CSPool.Pool<AttrItemData> starAttrDatas;

    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    public override void Init()
    {
        AutoClose();
        base.Init();
        AddCollider();
        starAttrDatas = GetListPool<AttrItemData>();

        CSEffectPlayMgr.Instance.ShowUIEffect(mgo_effect,17750);
    }

    public void Show(TABLE.PAODIANSHENFU prevItem, TABLE.PAODIANSHENFU currentItem)
    {
        if(null != prevItem && null != currentItem)
        {
            if(null != mlb_name)
            {
                mlb_name.text = CSString.Format(64, prevItem.rank, prevItem.star);
            }

            if(null != mlb_nextName)
            {
                mlb_nextName.text = CSString.Format(64, currentItem.rank, currentItem.star);
            }

            var datas = mPoolHandleManager.GetSystemClass<List<AttrItemData>>();
            var prevAttrParam = CSTimeExpManager.Instance.GetAttrParamByOccur(prevItem, CSMainPlayerInfo.Instance.Career);
            var curAttrParam = CSTimeExpManager.Instance.GetAttrParamByOccur(currentItem, CSMainPlayerInfo.Instance.Career);
            var kvs = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, prevAttrParam, prevItem.attrNum);
            var nextKvs = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, curAttrParam, currentItem.attrNum);
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
                currentValue.isLevelFull = false;
                currentValue.pooledHandle = mPoolHandleManager;
                currentValue.keyValue = kvs[i];
                currentValue.nKeyValue = nextKvs[i];
                currentValue.needEffect = false;
            }
            kvs.Clear();
            mPoolHandleManager.Recycle(kvs);
            nextKvs.Clear();
            mPoolHandleManager.Recycle(nextKvs);
            mGridExtraAddEffects.Bind<AttrItemData,AttrItem>(datas,mPoolHandleManager);
            datas.Clear();
            mPoolHandleManager.Recycle(datas);

            if(prevItem.Rank + 1 == currentItem.Rank)
            {
                if(null != mExtraAddEffects)
                    mExtraAddEffects.gameObject.SetActive(true);
                if (null != mExtraAddEffectsLine)
                    mExtraAddEffectsLine.gameObject.SetActive(true);
                if (null != mExtraAddEffectsContent)
                {
                    mExtraAddEffectsContent.gameObject.SetActive(true);
                    mExtraAddEffectsContent.text = CSAttributeInfo.Instance.GetStageUpDesc(mPoolHandleManager,currentItem,74);
                }
            }
            else
            {
                if (null != mExtraAddEffectsLine)
                    mExtraAddEffectsLine.gameObject.SetActive(false);
                if (null != mExtraAddEffects)
                    mExtraAddEffects.gameObject.SetActive(false);
                if (null != mExtraAddEffectsContent)
                    mExtraAddEffectsContent.gameObject.SetActive(false);
            }
        }
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mgo_effect);
        mGridExtraAddEffects.UnBind<StarUpAttrItem>();
        starAttrDatas = null;
        base.OnDestroy();
    }
}