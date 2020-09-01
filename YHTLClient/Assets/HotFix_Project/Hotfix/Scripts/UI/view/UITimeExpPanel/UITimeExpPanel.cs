using FlyBirds.Model;
using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TABLE;
using UnityEngine;

public class StarItemData : CSPool.IPoolItem
{
    public bool isON;
    public bool needEffect;
    public void OnRecycle()
    {
        isON = false;
        needEffect = false;
    }
}
public class StarItem :UIBinder
{
    protected UISprite bg;
    protected UISprite front;
    protected UISpriteAnimation effect;
    protected StarItemData mData;

    public override void Init(UIEventListener handle)
    {
        bg = Get<UISprite>("sp_bg");
        front = Get<UISprite>("sp_front");
        effect = Get<UISpriteAnimation>("sp_effect");
    }

    public override void Bind(object data)
    {
        mData = data as StarItemData;
        if(null != front)
        {
            front.gameObject.SetActive(mData.isON);
        }
        if(null != effect)
        {
            if (mData.needEffect)
            {
                mData.needEffect = false;
                effect.gameObject.SetActive(true);
                CSEffectPlayMgr.Instance.ShowUIEffect(effect.gameObject, "effect_star_add", 10, false);
                effect.OnFinish = OnPlayFinish;
            }
        }
    }

    protected void OnPlayFinish()
    {
        effect.gameObject.SetActive(false);
    }

    public override void OnDestroy()
    {
        effect.OnFinish = null;
        effect = null;
        bg = null;
        front = null;
        mData = null;
    }
}

public class AttrItemData : CSPool.IPoolItem
{
    public PoolHandleManager pooledHandle;
    public CSAttributeInfo.KeyValue keyValue;
    public CSAttributeInfo.KeyValue nKeyValue;
    public bool needEffect = true;
    public bool isLevelFull = false;
    public AttrType AttrType
    {
        get
        {
            if (null != keyValue && null != keyValue.clientAttribute)
                return (AttrType)keyValue.clientAttribute.AttrType;
            return AttrType.Absolute;
        }
    }
    public void OnRecycle()
    {
        needEffect = false;
        isLevelFull = false;
        if (null != keyValue)
        {
            keyValue.OnRecycle(pooledHandle);
            pooledHandle.Recycle(keyValue);
            keyValue = null;
        }

        if (null != nKeyValue)
        {
            nKeyValue.OnRecycle(pooledHandle);
            pooledHandle.Recycle(nKeyValue);
            nKeyValue = null;
        }
        pooledHandle = null;
    }
}

public class AttrItem : UIBinder
{
    protected UILabel lb_name;
    protected UILabel lb_value;
    protected UILabel lb_nextName;
    protected UILabel lb_nextValue;
    protected UISprite sp_arrow;
    protected AttrItemData mData;
    protected UISpriteAnimation mSpriteAnimation;
    protected UISprite mSprite;

    public override void Init(UIEventListener handle)
    {
        lb_name = Get<UILabel>("lb_name");
        lb_value = Get<UILabel>("lb_value");
        lb_nextName = Get<UILabel>("lb_nextName");
        lb_nextValue = Get<UILabel>("lb_nextValue");
        sp_arrow = Get<UISprite>("sp_arrow");
        mSpriteAnimation = Get<UISpriteAnimation>("lb_upgrade_effect");
        mSprite = Get<UISprite>("lb_upgrade_effect");
        if (null != mSpriteAnimation)
        {
            mSpriteAnimation.OnFinish = OnUpgradeEffectPlayFinished;
            mSpriteAnimation.enabled = false;
            mSprite.enabled = false;
        }
    }

    void OnUpgradeEffectPlayFinished()
    {
        if(null != mSpriteAnimation)
        {
            mSpriteAnimation.enabled = false;
            mSprite.enabled = false;
            CSEffectPlayMgr.Instance.Recycle(mSpriteAnimation.gameObject);
        }
    }

    public override void Bind(object data)
    {
        mData = data as AttrItemData;
        if (null == mData)
            return;

        if (null != mSpriteAnimation)
        {
            if (mData.needEffect && !mSpriteAnimation.enabled)
            {
                mSpriteAnimation.enabled = mData.needEffect;
                mSprite.enabled = mData.needEffect;
                CSEffectPlayMgr.Instance.ShowUIEffect(mSpriteAnimation.gameObject, "effect_dragon_levelup_add", 10, false, false);
            }
        }

        var attrType = mData.AttrType;
        if((AttrType)attrType == AttrType.SkillDesc)
        {
            if(mData.keyValue.IsZeroValue)
            {
                //如果右侧技能为新增
                if (null != lb_name)
                {
                    lb_name.text = CSString.Format(685, mData.nKeyValue.Value).BBCode(ColorType.SecondaryText);
                }
                if (null != lb_value)
                {
                    lb_value.text = string.Empty;
                }
                if (null != lb_nextName)
                {
                    lb_nextName.text = string.Empty;
                }
                if (null != lb_nextValue)
                {
                    lb_nextValue.text = string.Empty;
                }
                if (null != sp_arrow && sp_arrow.gameObject.activeSelf)
                    sp_arrow.gameObject.SetActive(false);
            }
            else
            {
                if (null != lb_name)
                {
                    lb_name.text = string.Format(ClientTipsTableManager.Instance[684].context, mData.keyValue.Value).BBCode(ColorType.SecondaryText);
                }
                if (null != lb_value)
                {
                    lb_value.text = string.Empty;
                }
                if (null != lb_nextName)
                {
                    lb_nextName.text = string.Format(ClientTipsTableManager.Instance[684].context, mData.nKeyValue.Value).BBCode(ColorType.SecondaryText);
                }
                if (null != lb_nextValue)
                {
                    lb_nextValue.text = string.Empty;
                }
                if (null != sp_arrow && !sp_arrow.gameObject.activeSelf)
                    sp_arrow.gameObject.SetActive(true);
            }
        }
        else
        {
            if (null != lb_name)
            {
                lb_name.text = string.Format(ClientTipsTableManager.Instance[54].context, mData.keyValue.Key).BBCode(ColorType.SecondaryText);
            }
            if (null != lb_value)
            {
                lb_value.text = mData.keyValue.Value.BBCode(ColorType.MainText);
            }
            if (null != lb_nextName)
            {
                lb_nextName.text = string.Format(ClientTipsTableManager.Instance[54].context, mData.nKeyValue.Key).BBCode(ColorType.SecondaryText);
            }
            if (null != lb_nextValue)
            {
                lb_nextValue.text = (mData.isLevelFull ? CSString.Format(954) : mData.nKeyValue.Value).BBCode(ColorType.Green);
            }
            if (null != sp_arrow && !sp_arrow.gameObject.activeSelf)
                sp_arrow.gameObject.SetActive(true);
        }
    }

    public override void OnDestroy()
    {
        if(null != mSpriteAnimation)
        {
            CSEffectPlayMgr.Instance.Recycle(mSpriteAnimation.gameObject);
            mSpriteAnimation.OnFinish = null;
            mSpriteAnimation = null;
        }
        mSprite = null;
        lb_name = null;
        lb_value = null;
        lb_nextName = null;
        lb_nextValue = null;
        mData = null;
    }
}

public partial class UITimeExpPanel : UIBasePanel
{
    protected TABLE.PAODIANSHENFU mItem = null;
    protected int Stage = 0;
    protected CSPool.Pool<StarItemData> starDatas;
    protected CSPool.Pool<AttrItemData> starAttrDatas;
    protected Vector3 singleLineLinePos = new Vector3(219,-150,0);
    protected Vector3 singleLineGoMaxPos = new Vector3(225, -186, 0);
    protected Vector3 doubleLineLinePos = new Vector3(219, -186, 0);
    protected Vector3 doubleLineGoMaxPos = new Vector3(225, -240, 0);
    protected FastArrayElementFromPool<Transform> mCostItems;

    public override void Init()
    {
        base.Init();

        mBtnUpStar.onClick = OnClickUpStar;
        mBtnLeft.onClick = OnLeft;
        mBtnRight.onClick = OnRight;
        mBtnStageUpEffect.onClick = OnClickStageUpEffect;
        mBtnHelp.onClick = OnQuesition;
        starDatas = GetListPool<StarItemData>();
        starAttrDatas = GetListPool<AttrItemData>();
        var template = UIItemBarManager.Instance.Create(mGridUpStarCosts.gameObject);
        mCostItems = template.Handle.transform.CreateClonePool(mGridUpStarCosts.transform,4,f=>
        {
            UIItemBarManager.Instance.Recycle(f.gameObject);
        },true);

        mClientEvent.AddEvent(CEvent.TimeExpChanged, TimeExpChanged);
        mClientEvent.AddEvent(CEvent.TimeExpUprgaded, TimeExpUpgraded);
        mClientEvent.AddEvent(CEvent.ItemListChange, TimeExpChanged);
        mClientEvent.AddEvent(CEvent.MonthCardInfoChange, OnMonthCardInfoChanged);
        mClientEvent.AddEvent(CEvent.DailyPurchaseInfo, OnLinkVisibleChanged);
        mClientEvent.AddEvent(CEvent.DailyPurchaseBuyDiscount, OnLinkVisibleChanged);
    }

    protected void LoopAlways()
    {
        FNDebug.Log("LoopAlways");
    }

    protected void LoopFixedTimes()
    {
        FNDebug.Log("LoopFixedTimes");
    }

    protected void LoopOnce()
    {
        FNDebug.Log("LoopOnce");
    }

    void InitLinks()
    {
        if(null != mlb_getWay)
        {
            mlb_getWay.text = CSString.Format(1951);
            mlb_getWay.SetupLink(()=>
            {
                UIManager.Instance.ClosePanel<UITimeExpCombinedPanel>();
            });
        }
    }

    const int linkGiftId = 1094;
    void CheckLinkVisible()
    {
        mlb_getWay.CustomActive(CSDiscountGiftBagInfo.Instance.IsHasSpecifyGiftBag(linkGiftId));
    }

    public void SetStage(int stage,bool upragde,bool upgradeStage)
    {
        Stage = stage;
        mItem = null;
        int id = PaoDianShenFuTableManager.Instance.make_id(0, Stage);
        if(PaoDianShenFuTableManager.Instance.TryGetValue(id, out mItem))
        {
            OnStageChanged(upragde,upgradeStage);
        }
    }

    public void RefreshLeftAndRightButton()
    {
        int id = PaoDianShenFuTableManager.Instance.make_id(0, Stage - 1);
        TABLE.PAODIANSHENFU item = null;
        bool enableLeft = PaoDianShenFuTableManager.Instance.TryGetValue(id, out item);
        if(enableLeft)
        {

        }
        id = PaoDianShenFuTableManager.Instance.make_id(0, Stage + 1);
        bool enableRight = PaoDianShenFuTableManager.Instance.TryGetValue(id, out item);
        if(enableRight)
        {

        }
    }

    public void RefreshStars(bool upragde)
    {
        int prev = CSTimeExpManager.Instance.Star;
        int stage = mItem.Rank;
        int max = CSTimeExpManager.Instance.stars[stage - 1] - 1;
        //reached
        if (CSTimeExpManager.Instance.Rank > Stage)
        {
            prev = max;
        }
        else if(CSTimeExpManager.Instance.Rank == Stage)
        {
            prev = CSTimeExpManager.Instance.Star;
        }
        else
        {
            prev = 0;
        }

        if(null != mItem)
        {
            List<StarItemData> datas = CSListPool.Get<StarItemData>();
            starDatas.RecycleAllItems();
            for (int i = 0; i < max; ++i)
            {
                datas.Add(starDatas.Get());
                datas[i].isON = i < prev;
                datas[i].needEffect = upragde && i == prev - 1;
            }
            mGridStars.Bind<StarItemData, StarItem>(datas,mPoolHandleManager);
            CSListPool.Put(datas);
        }
    }

    public void RefreshAttributes(bool needEffect)
    {
        var datas = mPoolHandleManager.GetSystemClass<List<AttrItemData>>();
        starAttrDatas.RecycleAllItems();
        TABLE.PAODIANSHENFU current = null;
        if(true)
        {
            int id = PaoDianShenFuTableManager.Instance.make_id(CSTimeExpManager.Instance.Star, CSTimeExpManager.Instance.Rank);
            PaoDianShenFuTableManager.Instance.TryGetValue(id, out current);
        }
        bool levelFull = false;
        TABLE.PAODIANSHENFU next = CSTimeExpManager.Instance.NextLevel(CSTimeExpManager.Instance.Star, CSTimeExpManager.Instance.Rank);
        if(null != current && null != next)
        {
            levelFull = current.id == next.id;
            var nextAttrParam = CSTimeExpManager.Instance.GetAttrParamByOccur(next, CSMainPlayerInfo.Instance.Career);
            var curAttrParam = CSTimeExpManager.Instance.GetAttrParamByOccur(current, CSMainPlayerInfo.Instance.Career);
            var kvs = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, curAttrParam, current.attrNum);
            var nextKvs = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, nextAttrParam, next.attrNum);
            for (int i = 0; i < kvs.Count; ++i)
            {
                if(kvs[i].IsZeroValue && nextKvs[i].IsZeroValue)
                {
                    kvs[i].OnRecycle(mPoolHandleManager);
                    nextKvs[i].OnRecycle(mPoolHandleManager);
                    continue;
                }

                var currentValue = starAttrDatas.Get();
                datas.Add(currentValue);
                currentValue.isLevelFull = levelFull;
                currentValue.pooledHandle = mPoolHandleManager;
                currentValue.keyValue = kvs[i];
                currentValue.nKeyValue = nextKvs[i];
                currentValue.needEffect = needEffect && kvs[i].HasDiff(nextKvs[i]);
            }
            kvs.Clear();
            mPoolHandleManager.Recycle(kvs);
            nextKvs.Clear();
            mPoolHandleManager.Recycle(nextKvs);
        }
        mGridLowStageEffects.Bind<AttrItemData,AttrItem>(datas,mPoolHandleManager);
        datas.Clear();
        mPoolHandleManager.Recycle(datas);
        mBtnUpStar.CustomActive(!levelFull);
        mlb_getWay.CustomActive(!levelFull);
        mgoMax.CustomActive(levelFull);

        if (null != current && null != next)
        {
            if (current.Rank != next.Rank)
            {
                //本次为升阶
                if(null != mStartUpDesc)
                {
                    mStartUpDesc.text = ClientTipsTableManager.Instance[57].context;
                }
            }
            else
            {
                //本次为升星
                if (null != mStartUpDesc)
                {
                    mStartUpDesc.text = ClientTipsTableManager.Instance[58].context;
                }
            }
        }

        bool singleLine = true;

        //更新升阶描述
        if(current.rank < 2)
        {
            int rank = current.rank + 1;
            int star = 0;
            int id = PaoDianShenFuTableManager.Instance.make_id(star, rank);
            TABLE.PAODIANSHENFU stageItem;
            if (PaoDianShenFuTableManager.Instance.TryGetValue(id,out stageItem))
            {
                mStageUpEffect.CustomActive(true);
                if (null != mStageUpEffect)
                {
                    mStageUpEffect.text = CSAttributeInfo.Instance.GetStageUpDesc(mPoolHandleManager, stageItem,56);
                }
                mlb_stage_up_effect2.CustomActive(false);
            }
            else
            {
                mStageUpEffect.CustomActive(false);
                mlb_stage_up_effect2.CustomActive(false);
            }

            singleLine = true;
        }
        else
        {
            mStageUpEffect.CustomActive(false);
            mlb_stage_up_effect2.CustomActive(true);
            //计算附加的属性值
            int attrCount = CalAttachedAttributes(current.rank);

            singleLine = attrCount <= 2;
        }

        //调整线与GOMAX对象位置
        if (singleLine)
        {
            //两行
            if (null != mline)
                mline.transform.localPosition = singleLineLinePos;
            if (null != mgoMax)
                mgoMax.transform.localPosition = singleLineGoMaxPos;
        }
        else
        {
            //单行
            if (null != mline)
                mline.transform.localPosition = doubleLineLinePos;
            if (null != mgoMax)
                mgoMax.transform.localPosition = doubleLineGoMaxPos;
        }

        //当前显示阶段大于拥有的阶段
        if (null != mStage)
        {
            mStage.text = string.Format(ClientTipsTableManager.Instance[Stage > CSTimeExpManager.Instance.Rank ? 73 : 52].context, Stage);
        }
    }

    FastArrayElementFromPool<UILabel> mAttrGameObjects;
    Stack<UILabel> mCachedGameObjects = new Stack<UILabel>(4);
    public int CalAttachedAttributes(int reachedRank)
    {
        if (null == mAttrGameObjects)
            mAttrGameObjects = new FastArrayElementFromPool<UILabel>(4,
                () =>
             {
                 UILabel handle = null;
                 if (mCachedGameObjects.Count > 0)
                 {
                     handle = mCachedGameObjects.Pop();
                     handle.CustomActive(true);
                     return handle;
                 }
                 handle = Object.Instantiate(mTemplate) as UILabel;
                 handle.CustomActive(true);
                 var transform = handle.transform;
                 transform.SetParent(mTemplate.transform.parent);
                 transform.localScale = Vector3.one;
                 transform.localRotation = Quaternion.identity;
                 transform.localPosition = Vector3.zero;
                 return handle;
             },
             (UILabel handle) =>
             {
                 handle.CustomActive(false);
                 mCachedGameObjects.Push(handle);
             });

        int attrCount = 0;
        var attrIds = mPoolHandleManager.GetSystemClass<RepeatedField<int>>();
        var attrValues = mPoolHandleManager.GetSystemClass<RepeatedField<int>>();
        int rank = 2;
        int star = 0;
        int id = PaoDianShenFuTableManager.Instance.make_id(star, rank);
        Dictionary<int, int> kvDics = mPoolHandleManager.GetSystemClass<Dictionary<int, int>>();
        kvDics.Clear();
        attrIds.Clear();
        attrValues.Clear();
        attrIds.Add(28);
        attrIds.Add(39);
        attrValues.Add(0);
        attrValues.Add(0);
        TABLE.PAODIANSHENFU current = null;
        string mainColor = UtilityColor.GetColorString(ColorType.MainText);
        while (rank <= reachedRank && PaoDianShenFuTableManager.Instance.TryGetValue(id, out current) && null != current)
        {
            attrIds.Add(current.clientRankPara);
            attrValues.Add(current.rankaddNum);
            var attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, attrIds, attrValues);
            for(int i = attrItems.Count - 1;i >= 0;--i)
            {
                if(attrItems[i].IsZeroValue)
                {
                    attrItems.RemoveAt(i);
                }
            }
            mAttrGameObjects.Count = attrItems.Count;
            //mattributeContainer.MaxCount = attrItems.Count;
            attrCount = attrItems.Count;
            for (int i = 0; i < mAttrGameObjects.Count; ++i)
            {
                var label = mAttrGameObjects[i];
                if (attrCount <= 1)
                {
                    label.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(61), mainColor, attrItems[i].Key, UtilityColor.GetColorString(ColorType.Green), attrItems[i].Value);
                    label.transform.localPosition = Vector3.zero;
                    label.pivot = UIWidget.Pivot.Center;
                    label.alignment = NGUIText.Alignment.Center;
                }
                else
                {
                    label.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(61), mainColor, attrItems[i].Key, UtilityColor.GetColorString(ColorType.Green), attrItems[i].Value);
                    float posX = 0.0f;
                    float posY = 0 + (i / 2) * (-30.0f);
                    if (0 == (i & 1))
                    {
                        posX = -172.0f;
                    }
                    else
                    {
                        posX = 39.0f;
                    }
                    label.pivot = UIWidget.Pivot.Left;
                    label.transform.localPosition = new Vector3(posX, posY, 0);
                    label.alignment = NGUIText.Alignment.Left;
                }
            }

            attrItems.Clear();
            mPoolHandleManager.Recycle(attrItems);
            id = PaoDianShenFuTableManager.Instance.make_id(star, ++rank);
        }
        mPoolHandleManager.Recycle(attrIds);
        mPoolHandleManager.Recycle(attrValues);
        return attrCount;
    }

    public void RefreshCostItems()
    {
        TABLE.PAODIANSHENFU current = null;
        int id = PaoDianShenFuTableManager.Instance.make_id(CSTimeExpManager.Instance.Star, CSTimeExpManager.Instance.Rank);
        List<ItemBarData> itemBarDatas = mPoolHandleManager.GetSystemClass<List<ItemBarData>>();
        itemBarDatas.Clear();
        var next = CSTimeExpManager.Instance.NextLevel(CSTimeExpManager.Instance.Star, CSTimeExpManager.Instance.Rank);
        if (PaoDianShenFuTableManager.Instance.TryGetValue(id, out current) && null != next && next.id != current.id)
        {
            for (int i = 0; i < current.costItem.Count; ++i)
            {
                TABLE.ITEM cfgItem = null;
                if (!ItemTableManager.Instance.TryGetValue(current.costItem[i], out cfgItem))
                {
                    continue;
                }

                if(i >= current.costNum.Count)
                {
                    continue;
                }

                var itemData = UIItemBarManager.Instance.Get();
                itemData.cfgId = current.costItem[i];
                itemData.needed = current.costNum[i];
                itemData.owned = itemData.cfgId.GetItemCount();
                itemData.flag = (int)ItemBarData.ItemBarType.IBT_GENERAL_COMPARE_SMALL |  (int)ItemBarData.ItemBarType.IBT_RED_GREEN;
                if(cfgItem.type == (int)ItemType.Money)
                {
                    itemData.flag |= (int)ItemBarData.ItemBarType.IBT_ONLY_COST;
                }
                itemData.eventHandle = mClientEvent;
                if (i == 0)
                    itemData.bgWidth = 176;
                else
                    itemData.bgWidth = 116;
                itemBarDatas.Add(itemData);
            }
        }
        mCostItems.Count = itemBarDatas.Count;
        for(int i = 0; i < itemBarDatas.Count; ++i)
        {
            var binder = mCostItems[i].gameObject.GetOrAddBinder<UIItemBar>(null);
            if (null != binder)
                binder.Bind(itemBarDatas[i]);
        }
        mGridUpStarCosts.Reposition();
        //UIItemBarManager.Instance.Bind(mGridUpStarCosts, itemBarDatas);
        itemBarDatas.Clear();
        mPoolHandleManager.Recycle(itemBarDatas);
    }

    public void RefreshExpTime()
    {
        int fixedTime = CSTimeExpManager.Instance.GetFixedAddTime();
        int monthAddTime = CSTimeExpManager.Instance.GetMonthCardAddTime();
        int monthAddTimes = CSMonthCardInfo.Instance.GetShenFuAddTimes();
        if(null != mL)
        {
            mL.text = CSString.Format(1525, fixedTime);
        }
        if (null != mR)
        {
            if (monthAddTimes < 2)
            {
                mR.text = CSString.Format(1526, monthAddTime);
            }
            else
            {
                mR.text = CSString.Format(1527, monthAddTime);
            }
            mR.SetupLink();
        }
        mmTimeExpTable?.Reposition();

        //if (null != mStoreTime)
        //{
        //    mStoreTime.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(63),CSTimeExpManager.Instance.GetMaxOfflineTime());
        //}
    }

    protected void TimeExpChanged(uint uiEvtID, object data)
    {
        CheckRedPoint();
        SetStage(CSTimeExpManager.Instance.Rank,false,false);
    }

    protected void OnMonthCardInfoChanged(uint uiEvtID, object data)
    {
        RefreshExpTime();
    }

    protected void OnLinkVisibleChanged(uint uiEvtID, object data)
    {
        CheckLinkVisible();
    }

    protected void TimeExpUpgraded(uint uiEvtID, object data)
    {
        CheckRedPoint();
        bool upstage = data is PAODIANSHENFU;
        SetStage(CSTimeExpManager.Instance.Rank, !upstage, upstage);
    }

    protected void CheckRedPoint()
    {
        if (null != mRedPoint)
        {
            bool value = CSTimeExpManager.Instance.HasNotify;
            mRedPoint.SetActive(value);
        }
    }

    protected void OnLeft(GameObject go)
    {
        int id = PaoDianShenFuTableManager.Instance.make_id(0,Stage - 1);
        TABLE.PAODIANSHENFU item = null;
        if (PaoDianShenFuTableManager.Instance.TryGetValue(id, out item))
        {
            SetStage(Stage - 1,false,false);
        }
    }

    protected void OnRight(GameObject go)
    {
        int id = PaoDianShenFuTableManager.Instance.make_id(0,Stage + 1);
        TABLE.PAODIANSHENFU item = null;
        if (PaoDianShenFuTableManager.Instance.TryGetValue(id,out item))
        {
            SetStage(Stage + 1,false,false);
        }
    }

    protected void OnQuesition(GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.HELP_TIME_EXP);
    }

    protected void OnClickStageUpEffect(GameObject go)
    {
        UIManager.Instance.CreatePanel<UITimeExpHighStageEffectPanel>();
    }

    protected void OnStageChanged(bool upgradeStar,bool upgradeStage)
    {
        RefreshStars(upgradeStar);
        RefreshLeftAndRightButton();
        RefreshAttributes(false);
        RefreshCostItems();
        RefreshExpTime();
        if(upgradeStage)
        {
            PlayBaoDianEffect();
        }
    }

    protected void OnClickUpStar(GameObject go)
    {
        int id = PaoDianShenFuTableManager.Instance.make_id(CSTimeExpManager.Instance.Star,CSTimeExpManager.Instance.Rank);
        TABLE.PAODIANSHENFU item = null;
        if (!PaoDianShenFuTableManager.Instance.TryGetValue(id, out item))
        {
            return;
        }

        if(!ItemHelper.IsItemsEnough(item.costItem,item.costNum,0,true))
        {
            return;
        }

        RefreshAttributes(true);
        Net.ReqUpgradePaoDianMessage();
    }

    public override void Show()
    {
        base.Show();

        if (null != mStageEffectBg)
        {
            CSEffectPlayMgr.Instance.ShowUITexture(mStageEffectBg, "shenfubg_cq_1");
        }

        SetShenfuEffect(CSTimeExpManager.Instance.Rank);
        CheckRedPoint();
        SetStage(CSTimeExpManager.Instance.Rank,false,false);
        InitLinks();
        CheckLinkVisible();
    }

    protected void OnBaoDianFinished()
    {
        SetShenfuEffect(CSTimeExpManager.Instance.Rank);
        mEffectBaoDian.CustomActive(false);

        PAODIANSHENFU prev = null;
        int prevMaxStar = CSTimeExpManager.Instance.MaxStar(CSTimeExpManager.Instance.Rank - 1);
        int prevId = PaoDianShenFuTableManager.Instance.make_id(prevMaxStar, CSTimeExpManager.Instance.Rank - 1);
        if (!PaoDianShenFuTableManager.Instance.TryGetValue(prevId, out prev))
        {
            return;
        }

        PAODIANSHENFU current = null;
        int curId = PaoDianShenFuTableManager.Instance.make_id(CSTimeExpManager.Instance.Star, CSTimeExpManager.Instance.Rank);
        if (!PaoDianShenFuTableManager.Instance.TryGetValue(curId, out current))
        {
            return;
        }

        UIManager.Instance.CreatePanel<UITimeExpStageUpPanel>(f =>
        {
            (f as UITimeExpStageUpPanel).Show(prev, current);
        });
    }

    protected void SetShenfuEffect(int stage)
    {
        int id = PaoDianShenFuTableManager.Instance.make_id(0, stage);
        TABLE.PAODIANSHENFU item = null;
        if (PaoDianShenFuTableManager.Instance.TryGetValue(id, out item))
        {
            if (null != mStageEffect && null != item)
            {
                CSEffectPlayMgr.Instance.ShowUIEffect(mStageEffect, item.icon, 10, true, false);
            }
        }
    }

    protected void PlayBaoDianEffect()
    {
        if (null != mEffectBaoDian)
        {
            //mEffectBaoDian.OnFinish = OnBaoDianFinished;
            OnBaoDianFinished();
            mEffectBaoDian.CustomActive(true);
            CSEffectPlayMgr.Instance.ShowUIEffect(mEffectBaoDian.gameObject, "effect_rune_point_normal", 10, false, false);
        }
    }


    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.TimeExpChanged, TimeExpChanged);
        mClientEvent.RemoveEvent(CEvent.TimeExpUprgaded, TimeExpUpgraded);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, TimeExpChanged);
        mClientEvent.RemoveEvent(CEvent.MonthCardInfoChange, OnMonthCardInfoChanged);
        mClientEvent.RemoveEvent(CEvent.DailyPurchaseInfo, OnLinkVisibleChanged);
        mClientEvent.RemoveEvent(CEvent.DailyPurchaseBuyDiscount, OnLinkVisibleChanged);

        mAttrGameObjects?.Clear();
        mAttrGameObjects = null;
        mCachedGameObjects?.Clear();
        mCachedGameObjects = null;

        CSEffectPlayMgr.Instance.Recycle(mStageEffectBg);
        CSEffectPlayMgr.Instance.Recycle(mStageEffect);
        CSEffectPlayMgr.Instance.Recycle(mEffectBaoDian.gameObject);

        if (null != mEffectBaoDian)
        {
            mEffectBaoDian.OnFinish = null;
            mEffectBaoDian = null;
        }
        mGridStars.UnBind<StarItem>();
        mGridStars = null;
        mGridLowStageEffects.UnBind<AttrItem>();
        mGridLowStageEffects = null;
        mCostItems?.Destroy();
        mCostItems = null;
        //UIItemBarManager.Instance.UnBind(mGridUpStarCosts);
        mGridUpStarCosts = null;
        starDatas = null;
        starAttrDatas = null;

        base.OnDestroy();
    }
}