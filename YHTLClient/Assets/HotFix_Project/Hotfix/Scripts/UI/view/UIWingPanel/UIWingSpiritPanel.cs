using System.Collections.Generic;
using bag;
using Google.Protobuf.Collections;
using UnityEngine;

/// <summary>
/// 羽灵界面分页签
/// </summary>
public enum TypeWingSpirit
{
    WingSpiritAttr = 1, //羽灵属性
    WingSoul = 2, //羽灵之魂
}

public partial class UIWingSpiritPanel : UIBasePanel
{
    private TypeWingSpirit curTab = TypeWingSpirit.WingSpiritAttr;
    private WingSpiritData wingSpiritData;
    private ILBetterList<ILBetterList<int>> listAttrAddition;

    private int curWingSpiritLevel = 0;

    /// <summary>
    /// 羽翼之魂解锁羽灵等级
    /// </summary>
    private int wingUnLockLevel = 0;

    /// <summary>
    /// 幻彩之魂解锁羽灵等级
    /// </summary>
    private int wingColorUnLockLevel = 0;

    /// <summary>
    /// 羽技之魂解锁羽灵等级
    /// </summary>
    private int wingTechniqueUnLockLevel = 0;


    private Dictionary<int, WingSoulData> dicSlotData;
    private int slotId1 = 0;
    private int slotId2 = 0;
    private int slotId3 = 0;

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.YuLingInfo, RefreshData);
        mClientEvent.Reg((uint) CEvent.ItemListChange, RefreshData);
        UIEventListener.Get(mbtn_wingSpiritAttr.gameObject, TypeWingSpirit.WingSpiritAttr).onClick = OnClickTab;
        UIEventListener.Get(mbtn_wingSoul.gameObject, TypeWingSpirit.WingSoul).onClick = OnClickTab;
        UIEventListener.Get(mbtn_Add1.gameObject, 1).onClick = OnClickAdd;
        UIEventListener.Get(mbtn_Add2.gameObject, 2).onClick = OnClickAdd;
        UIEventListener.Get(mbtn_Add3.gameObject, 3).onClick = OnClickAdd;
        UIEventListener.Get(mlock1.gameObject, 1).onClick = OnClickLock;
        UIEventListener.Get(mlock2.gameObject, 2).onClick = OnClickLock;
        UIEventListener.Get(mlock3.gameObject, 3).onClick = OnClickLock;

        UIEventListener.Get(mlb_wingAttr.gameObject, 1).onClick = OnClickSpecialAttr;
        UIEventListener.Get(mlb_wingColorAttr.gameObject, 2).onClick = OnClickSpecialAttr;
        UIEventListener.Get(mlb_wingTechniqueAttr.gameObject, 3).onClick = OnClickSpecialAttr;

        UIEventListener.Get(msp_icon1.gameObject, 1).onClick = OnClickIcon;
        UIEventListener.Get(msp_icon2.gameObject, 2).onClick = OnClickIcon;
        UIEventListener.Get(msp_icon3.gameObject, 3).onClick = OnClickIcon;

        mbtn_rule.onClick = OnClickRule;
        mbtn_upGrade.onClick = OnClickUpGrade;
        mbtn_mosaic.onClick = OnClickMosaic;
        mbtn_des.onClick = OnClickSeeDetails;
        mbar.onChange.Add(new EventDelegate(OnChangeBar));
        mbar_max.onChange.Add(new EventDelegate(OnChangeBarMax));

        mItemBarDatas = mPoolHandleManager.CreateGeneratePool<ItemBarData>(8);
        
        CSEffectPlayMgr.Instance.ShowUITexture(mwing_spirit, "wing_spirit");
        CSEffectPlayMgr.Instance.ShowUIEffect(msp_effect_icon, 17805);

        wingUnLockLevel = CSWingInfo.Instance.WingUnLockLevel;
        wingColorUnLockLevel = CSWingInfo.Instance.WingColorUnLockLevel;
        wingTechniqueUnLockLevel = CSWingInfo.Instance.WingTechniqueUnLockLevel;
        listAttrAddition = CSWingInfo.Instance.ListAttrAddition;
        marrow.SetActive(false);
        wingSpiritData = CSWingInfo.Instance.WingSpiritData;
        curWingSpiritLevel = wingSpiritData.YuLingLevelId;

        dicSlotData = CSWingInfo.Instance.DicSlotData;
        slotId1 = dicSlotData[1].yuLingSoulId;
        slotId2 = dicSlotData[2].yuLingSoulId;
        slotId3 = dicSlotData[3].yuLingSoulId;
        
        marrow1.SetActive(false);
        marrow2.SetActive(false);
        marrow3.SetActive(false);
    }

    void RefreshData(uint id, object data)
    {
        if (data == null) return;
        wing.ResYuLingInfo msg = (wing.ResYuLingInfo) data;
        InitData();
        if (curWingSpiritLevel < msg.id) //升级
        {
            curWingSpiritLevel = msg.id;
            for (int i = 0, max = listBinders.Count; i < max; i++)
            {
                UIWingSpiritAttrBinder binder = listBinders[i];
                if (binder.NeedStarEffect)
                    binder.PlayStarEffect();
            }
        }
        
        //镶嵌成功则播放爆点特效
        dicSlotData = CSWingInfo.Instance.DicSlotData;
        if (slotId1!=dicSlotData[1].yuLingSoulId)
        {
            slotId1 = dicSlotData[1].yuLingSoulId;
            CSEffectPlayMgr.Instance.ShowUIEffect(meffect_burst_point1, 17085);
            UIManager.Instance.ClosePanel<UIWingSoulChangeMosaicPanel>();
        }
        
        if (slotId2!=dicSlotData[2].yuLingSoulId)
        {
            slotId2 = dicSlotData[2].yuLingSoulId;
            CSEffectPlayMgr.Instance.ShowUIEffect(meffect_burst_point2, 17085);
            UIManager.Instance.ClosePanel<UIWingSoulChangeMosaicPanel>();
        }
        
        if (slotId3!=dicSlotData[3].yuLingSoulId)
        {
            slotId3 = dicSlotData[3].yuLingSoulId;
            CSEffectPlayMgr.Instance.ShowUIEffect(meffect_burst_point3, 17085);
            UIManager.Instance.ClosePanel<UIWingSoulChangeMosaicPanel>();
        }
    }

    void OnChangeBar()
    {
        marrow.SetActive(mbar.value < 0.95 && mgrid_effects.MaxCount > 5);
    }

    void OnChangeBarMax()
    {
        marrow_max.SetActive(mbar_max.value < 0.95 && mgrid_effects.MaxCount > 5);
    }

    public override void Show()
    {
        base.Show();
        curTab = TypeWingSpirit.WingSpiritAttr;
        InitData();
    }

    void InitData()
    {
        ShowLeftInfo();
        ShowRightInfo(curTab);
        RefreshRedPoint();
    }

    void RefreshRedPoint()
    {
        bool isEnoughPromote = CSWingInfo.Instance.IsEnoughPromote();
        mredpoint_Attr.SetActive(isEnoughPromote);
        mredpoint_upGrade.SetActive(isEnoughPromote);
        mredpoint_wingSoul.SetActive(CSWingInfo.Instance.IsHasBetterYulingSoul());
        mredpoint_mosaic.SetActive(CSWingInfo.Instance.IsHasBetterYulingSoul());
    }

    void OnClickTab(GameObject go)
    {
        if (go == null) return;
        TypeWingSpirit type = (TypeWingSpirit) UIEventListener.Get(go).parameter;
        if (curTab == type) return;
        curTab = type;
        ShowRightInfo(curTab);
    }

    void ShowLeftInfo()
    {
        mlb_title.text = CSString.Format(1931, wingSpiritData.YuLingLevelId).BBCode(ColorType.MainText);
        for (int i = 0, max = wingSpiritData.WingSoulDatas.Count; i < max; i++)
        {
            WingSoulData wingSoulData = wingSpiritData.WingSoulDatas[i];
            switch (wingSoulData.Position)
            {
                case 1:
                    if (wingSoulData.yuLingSoulId > 0)
                    {
                        msp_icon1.spriteName = ItemTableManager.Instance.GetItemIcon(wingSoulData.yuLingSoulId);
                        msp_icon1.gameObject.SetActive(true);
                        mlock1.gameObject.SetActive(false);
                        mbtn_Add1.gameObject.SetActive(false);
                        mlb_name1.text = wingSoulData.YulingsoulCfg.name; //带品质色
                        mlb_name1.color =
                            UtilityCsColor.Instance.GetColor(
                                ItemTableManager.Instance.GetItemQuality(wingSoulData.yuLingSoulId));
                        // marrow1.SetActive(CSWingInfo.Instance.IsHasBetterWing());
                        mredpoint_slot1.SetActive(CSWingInfo.Instance.IsHasBetterWing());
                        // CSEffectPlayMgr.Instance.Recycle(meffect_add1);
                    }
                    else
                    {
                        msp_icon1.gameObject.SetActive(false);
                        mlock1.gameObject.SetActive(wingSoulData.lockFlag == 0);
                        mbtn_Add1.gameObject.SetActive(wingSoulData.lockFlag == 1);
                        if (wingSoulData.lockFlag == 1)
                        {
                            if (CSWingInfo.Instance.IsHasBetterWing())
                            {
                                CSEffectPlayMgr.Instance.ShowUIEffect(meffect_add1, 17086);
                                mredpoint_slot1.SetActive(true);
                            }
                            else
                            {
                                CSEffectPlayMgr.Instance.Recycle(meffect_add1);
                                mredpoint_slot1.SetActive(false);
                            }
                        }
                        else
                        {
                            mredpoint_slot1.SetActive(false);
                        }
                        
                        if (wingSoulData.lockFlag == 0)
                            mlb_name1.text = CSString.Format(1934, wingUnLockLevel).BBCode(ColorType.WeakText);
                        else if (wingSoulData.lockFlag == 1)
                            mlb_name1.text = CSString.Format(1936).BBCode(ColorType.SecondaryText);
                        marrow1.SetActive(false);
                    }

                    break;
                case 2:
                    if (wingSoulData.yuLingSoulId > 0)
                    {
                        msp_icon2.spriteName = ItemTableManager.Instance.GetItemIcon(wingSoulData.yuLingSoulId);
                        msp_icon2.gameObject.SetActive(true);
                        mlock2.gameObject.SetActive(false);
                        mbtn_Add2.gameObject.SetActive(false);
                        mlb_name2.text = wingSoulData.YulingsoulCfg.name; //带品质色
                        mlb_name2.color =
                            UtilityCsColor.Instance.GetColor(
                                ItemTableManager.Instance.GetItemQuality(wingSoulData.yuLingSoulId));
                        // marrow2.SetActive(CSWingInfo.Instance.IsHasBetterWingColor());
                        mredpoint_slot2.SetActive(CSWingInfo.Instance.IsHasBetterWingColor());
                        // CSEffectPlayMgr.Instance.Recycle(meffect_add2);
                    }
                    else
                    {
                        msp_icon2.gameObject.SetActive(false);
                        mlock2.gameObject.SetActive(wingSoulData.lockFlag == 0);
                        mbtn_Add2.gameObject.SetActive(wingSoulData.lockFlag == 1);
                        if (wingSoulData.lockFlag == 1)
                        {
                            if (CSWingInfo.Instance.IsHasBetterWingColor())
                            {
                                CSEffectPlayMgr.Instance.ShowUIEffect(meffect_add2, 17086);    
                                mredpoint_slot2.SetActive(true);
                            }
                            else
                            {
                                CSEffectPlayMgr.Instance.Recycle(meffect_add2);
                                mredpoint_slot2.SetActive(false);
                            }
                        }
                        else
                        {
                            mredpoint_slot2.SetActive(false);
                        }
                        
                        if (wingSoulData.lockFlag == 0)
                            mlb_name2.text = CSString.Format(1934, wingColorUnLockLevel).BBCode(ColorType.WeakText);
                        else if (wingSoulData.lockFlag == 1)
                            mlb_name2.text = CSString.Format(1937).BBCode(ColorType.SecondaryText);
                        marrow2.SetActive(false);
                    }

                    break;
                case 3:
                    if (wingSoulData.yuLingSoulId > 0)
                    {
                        msp_icon3.spriteName = ItemTableManager.Instance.GetItemIcon(wingSoulData.yuLingSoulId);
                        msp_icon3.gameObject.SetActive(true);
                        mlock3.gameObject.SetActive(false);
                        mbtn_Add3.gameObject.SetActive(false);
                        mlb_name3.text = wingSoulData.YulingsoulCfg.name; //带品质色
                        mlb_name3.color =
                            UtilityCsColor.Instance.GetColor(
                                ItemTableManager.Instance.GetItemQuality(wingSoulData.yuLingSoulId));
                        // marrow3.SetActive(CSWingInfo.Instance.IsHasBetterWingTechnique());
                        mredpoint_slot3.SetActive(CSWingInfo.Instance.IsHasBetterWingTechnique());
                        // CSEffectPlayMgr.Instance.Recycle(meffect_add3);
                    }
                    else
                    {
                        msp_icon3.gameObject.SetActive(false);
                        mlock3.gameObject.SetActive(wingSoulData.lockFlag == 0);
                        mbtn_Add3.gameObject.SetActive(wingSoulData.lockFlag == 1);
                        if (wingSoulData.lockFlag == 1)
                        {
                            if (CSWingInfo.Instance.IsHasBetterWingTechnique())
                            {
                                CSEffectPlayMgr.Instance.ShowUIEffect(meffect_add3, 17086);
                                mredpoint_slot3.SetActive(true);
                            }
                            else
                            {
                                CSEffectPlayMgr.Instance.Recycle(meffect_add3);
                                mredpoint_slot3.SetActive(false);
                            }
                        }
                        else
                        {
                            mredpoint_slot3.SetActive(false);
                        }
                        
                        if (wingSoulData.lockFlag == 0)
                            mlb_name3.text = CSString.Format(1934, wingTechniqueUnLockLevel).BBCode(ColorType.WeakText);
                        else if (wingSoulData.lockFlag == 1)
                            mlb_name3.text = CSString.Format(1938).BBCode(ColorType.SecondaryText);
                        marrow3.SetActive(false);
                    }

                    break;
            }
        }
    }

    void ShowRightInfo(TypeWingSpirit type)
    {
        switch (type)
        {
            case TypeWingSpirit.WingSpiritAttr:
                mobj_wingSpiritAttr.SetActive(true);
                mobj_wingSoul.SetActive(false);
                ShowWingSpiritAttr();
                break;
            case TypeWingSpirit.WingSoul:
                mobj_wingSpiritAttr.SetActive(false);
                mobj_wingSoul.SetActive(true);
                ShowWingSoul();
                break;
        }
    }

    private LongArray attrInfo;
    private LongArray nextAttrInfo;
    FastArrayElementFromPool<ItemBarData> mItemBarDatas;
    ILBetterList<UIWingSpiritAttrBinder> listBinders = new ILBetterList<UIWingSpiritAttrBinder>();

    /// <summary>
    /// 羽灵属性信息
    /// </summary>
    void ShowWingSpiritAttr()
    {
        if (!wingSpiritData.isMax) //非满级
        {
            mobj_noMax.SetActive(true);
            mobj_max.SetActive(false);

            if (wingSpiritData.YulinglevelCfg != null && wingSpiritData.NextYulinglevelCfg != null)
            {
                attrInfo = CSWingInfo.Instance.GetAttrParaByCareer(CSMainPlayerInfo.Instance.Career,
                    wingSpiritData.YulinglevelCfg);
                nextAttrInfo = CSWingInfo.Instance.GetAttrParaByCareer(CSMainPlayerInfo.Instance.Career,
                    wingSpiritData.NextYulinglevelCfg);

                //当前属性加成
                RepeatedField<CSAttributeInfo.KeyValue> attrItems = null;
                if (attrInfo.Count > 0)
                    attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, attrInfo);

                //下一星级属性加成
                RepeatedField<CSAttributeInfo.KeyValue> attrItemsNext = null;
                if (nextAttrInfo.Count > 0)
                    attrItemsNext = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, nextAttrInfo);

                GameObject gp;
                mgrid_effects.MaxCount = attrItems.Count;
                mbar.value = 0;
                mScrollView_effects.verticalScrollBar = mbar;
                for (int i = 0; i < mgrid_effects.MaxCount; i++)
                {
                    gp = mgrid_effects.controlList[i];
                    var eventHandle = UIEventListener.Get(gp);
                    UIWingSpiritAttrBinder Binder;
                    if (eventHandle.parameter == null)
                    {
                        Binder = new UIWingSpiritAttrBinder();
                        Binder.Setup(eventHandle);
                        listBinders.Add(Binder);
                    }
                    else
                    {
                        Binder = eventHandle.parameter as UIWingSpiritAttrBinder;
                    }

                    Binder.keyValue = attrItems[i];
                    Binder.keyValueNext = attrItemsNext[i];
                    Binder.Bind(null);
                }

                mScrollView_effects.ResetPosition();
                marrow.SetActive(mbar.value < 0.95 && mgrid_effects.MaxCount > 5);

                //下一级加成提示
                if (listAttrAddition.Count > 0)
                {
                    for (int i = 0, max = CSWingInfo.Instance.ListAttrAddition.Count; i < max; i++)
                    {
                        ILBetterList<int> listInt = CSWingInfo.Instance.ListAttrAddition[i];
                        if (i == 0)
                        {
                            if (wingSpiritData.YuLingLevelId < listInt[0])
                            {
                                ILBetterList<int> listIntNext = CSWingInfo.Instance.ListAttrAddition[0];
                                mlb_tips.text =
                                    $"{CSString.Format(1932, listIntNext[0], $"{listIntNext[1] * 1f / 100}%".BBCode(ColorType.Green))}";
                                break;
                            }
                        }


                        if (wingSpiritData.YuLingLevelId >= listInt[0] &&
                            wingSpiritData.YuLingLevelId < CSWingInfo.Instance.ListAttrAddition[i + 1][0] &&
                            i < max - 1)
                        {
                            ILBetterList<int> listIntNext = CSWingInfo.Instance.ListAttrAddition[i + 1];
                            mlb_tips.text =
                                $"{CSString.Format(1932, listIntNext[0], $"{listIntNext[1] * 1f / 100}%".BBCode(ColorType.Green))}";
                            break;
                        }
                    }
                }
            }


            //消耗品条
            mItemBarDatas.Clear();
            if (wingSpiritData.YulinglevelCfg != null)
            {
                for (int i = 0, max = wingSpiritData.YulinglevelCfg.levelUp.Count; i < max; i++)
                {
                    var cost = wingSpiritData.YulinglevelCfg.levelUp[i];
                    ItemBarData itemData = mItemBarDatas.Append();
                    itemData.cfgId = cost.key();
                    itemData.needed = cost.value();
                    itemData.owned = cost.key().GetItemCount();
                    itemData.flag = (int) ItemBarData.ItemBarType.IBT_GENERAL_COMPARE_SMALL_REDGREEN;
                    if (ItemTableManager.Instance.TryGetValue(itemData.cfgId, out TABLE.ITEM cfgItem))
                    {
                        if (cfgItem.type == (int) ItemType.Money)
                            itemData.flag |= (int) ItemBarData.ItemBarType.IBT_ONLY_COST;
                        else
                            itemData.flag |= (int) ItemBarData.ItemBarType.IBT_SHORT_EXPRESS_WITH_ONE_POINT;
                    }
                }

                mgrid_UIItemBar.MaxCount = mItemBarDatas.Count;
                mgrid_UIItemBar.Bind<ItemBarData, UIItemBar>(mItemBarDatas, mPoolHandleManager);
            }
        }
        else //满级
        {
            mobj_noMax.SetActive(false);
            mobj_max.SetActive(true);

            if (wingSpiritData.YulinglevelCfg != null)
            {
                attrInfo = CSWingInfo.Instance.GetAttrParaByCareer(CSMainPlayerInfo.Instance.Career,
                    wingSpiritData.YulinglevelCfg);

                //当前属性加成
                RepeatedField<CSAttributeInfo.KeyValue> attrItems = null;
                if (attrInfo.Count > 0)
                    attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, attrInfo);

                GameObject gp;
                mgrid_effects.MaxCount = attrItems.Count;
                mbar_max.value = 0;
                mScrollView_effects.verticalScrollBar = mbar_max;
                for (int i = 0; i < mgrid_effects.MaxCount; i++)
                {
                    gp = mgrid_effects.controlList[i];
                    var eventHandle = UIEventListener.Get(gp);
                    UIWingSpiritAttrBinder Binder;
                    if (eventHandle.parameter == null)
                    {
                        Binder = new UIWingSpiritAttrBinder();
                        Binder.Setup(eventHandle);
                    }
                    else
                    {
                        Binder = eventHandle.parameter as UIWingSpiritAttrBinder;
                    }

                    Binder.keyValue = attrItems[i];
                    Binder.keyValueNext = null;
                    Binder.Bind(null);
                }

                mScrollView_effects.ResetPosition();
                marrow_max.SetActive(mbar_max.value < 0.95 && mgrid_effects.MaxCount > 5);
            }
        }
    }


    private RepeatedField<int> ids = new RepeatedField<int>();
    private RepeatedField<int> values = new RepeatedField<int>();

    /// <summary>
    /// 羽灵之魂信息
    /// </summary>
    void ShowWingSoul()
    {
        int addition = wingSpiritData.Addition;
        mScrollView_BaseAttr.ResetPosition();
        //基础属性
        ids.Clear();
        values.Clear();
        //没有任何羽灵之魂的时候
        Dictionary<int, WingSoulData> dicWingSoulData = CSWingInfo.Instance.DicSlotData;
        if (dicWingSoulData[1].yuLingSoulId <= 0 && dicWingSoulData[2].yuLingSoulId <= 0 &&
            dicWingSoulData[3].yuLingSoulId <= 0)
        {
            List<List<int>> listIds = CSWingInfo.Instance.GetListNoWingSoulAttrs(CSMainPlayerInfo.Instance.Career);
            RepeatedField<CSAttributeInfo.KeyValue> attrItems = null;
            if (listIds != null && listIds.Count > 0)
                attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, listIds);

            if (attrItems != null)
            {
                mgrid_BaseAttr.MaxCount = Mathf.CeilToInt((float) attrItems.Count / 2);
                GameObject gp;
                for (int i = 0; i < mgrid_BaseAttr.MaxCount; i++)
                {
                    gp = mgrid_BaseAttr.controlList[i];
                    var eventHandle = UIEventListener.Get(gp);
                    UIWingSoulBaseAttrBinder Binder;
                    if (eventHandle.parameter == null)
                    {
                        Binder = new UIWingSoulBaseAttrBinder();
                        Binder.Setup(eventHandle);
                    }
                    else
                    {
                        Binder = eventHandle.parameter as UIWingSoulBaseAttrBinder;
                    }

                    Binder.index = i;
                    Binder.maxCount = mgrid_BaseAttr.MaxCount;
                    Binder.attrItems = attrItems;
                    Binder.Bind(null);
                }
            }
        }
        else //有羽灵之魂镶嵌的时候
        {
            for (int i = 0, max = wingSpiritData.WingSoulDatas.Count; i < max; i++)
            {
                WingSoulData wingSoulData = wingSpiritData.WingSoulDatas[i];
                if (wingSoulData.yuLingSoulId > 0)
                {
                    LongArray tempAttrInfo =
                        CSWingInfo.Instance.GetBaseAttrParaByCareer(CSMainPlayerInfo.Instance.Career,
                            wingSoulData.YulingsoulCfg);
                    for (int j = 0, max1 = tempAttrInfo.Count; j < max1; j++)
                    {
                        ids.Add(tempAttrInfo[j].key());
                        values.Add((int) (tempAttrInfo[j].value() * ((addition + 10000) * 1f / 10000)));
                    }
                }
            }

            RepeatedField<CSAttributeInfo.KeyValue> attrItems = null;
            if (ids.Count > 0 && values.Count > 0)
                attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, ids, values);

            if (attrItems != null)
            {
                mgrid_BaseAttr.MaxCount = Mathf.CeilToInt((float) attrItems.Count / 2);
                GameObject gp;
                for (int i = 0; i < mgrid_BaseAttr.MaxCount; i++)
                {
                    gp = mgrid_BaseAttr.controlList[i];
                    var eventHandle = UIEventListener.Get(gp);
                    UIWingSoulBaseAttrBinder Binder;
                    if (eventHandle.parameter == null)
                    {
                        Binder = new UIWingSoulBaseAttrBinder();
                        Binder.Setup(eventHandle);
                    }
                    else
                    {
                        Binder = eventHandle.parameter as UIWingSoulBaseAttrBinder;
                    }

                    Binder.index = i;
                    Binder.maxCount = mgrid_BaseAttr.MaxCount;
                    Binder.attrItems = attrItems;
                    Binder.Bind(null);
                }
            }
        }

        //特殊属性
        for (int i = 0, max = wingSpiritData.WingSoulDatas.Count; i < max; i++)
        {
            WingSoulData wingSoulData = wingSpiritData.WingSoulDatas[i];
            switch (wingSoulData.Position)
            {
                case 1:
                    if (wingSoulData.lockFlag == 0) //未解锁
                        mlb_wingAttr.text = CSString.Format(1934, wingUnLockLevel).BBCode(ColorType.Red);
                    else if (wingSoulData.lockFlag == 1) //前往获取
                        mlb_wingAttr.text = CSString.Format(1942);
                    else if (wingSoulData.lockFlag == 2 && wingSoulData.yuLingSoulId > 0) //已镶嵌
                    {
                        //多条
                        mlb_wingAttr.text = CSString.Format(1943);
                    }

                    break;
                case 2:
                    if (wingSoulData.lockFlag == 0) //未解锁
                        mlb_wingColorAttr.text = CSString.Format(1934, wingColorUnLockLevel).BBCode(ColorType.Red);
                    else if (wingSoulData.lockFlag == 1) //前往获取
                        mlb_wingColorAttr.text = CSString.Format(1942);
                    else if (wingSoulData.lockFlag == 2 && wingSoulData.yuLingSoulId > 0) //已镶嵌
                    {
                        //多条
                        mlb_wingColorAttr.text = CSString.Format(1943);
                    }

                    break;
                case 3:
                    if (wingSoulData.lockFlag == 0) //未解锁
                        mlb_wingTechniqueAttr.text =
                            CSString.Format(1934, wingTechniqueUnLockLevel).BBCode(ColorType.Red);
                    else if (wingSoulData.lockFlag == 1) //前往获取
                        mlb_wingTechniqueAttr.text = CSString.Format(1942);
                    else if (wingSoulData.lockFlag == 2 && wingSoulData.yuLingSoulId > 0) //已镶嵌
                    {
                        //一条
                        if (wingSoulData.YulingsoulCfg.exattr > 0) //暂时只有羽技综述性会配一条的加成
                        {
                            mlb_wingTechniqueAttr.text = CSString.Format(1950,
                                    $"+{(wingSoulData.YulingsoulCfg.exattr * 1f / 100) * ((10000 + addition) * 1f / 10000)}%")
                                .BBCode(ColorType.Green);
                        }
                        else //多条
                        {
                            mlb_wingTechniqueAttr.text = CSString.Format(1943);
                        }
                    }

                    break;
            }
        }
    }

    void OnClickRule(GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.WingSpirit);
    }

    /// <summary>
    /// 点击升级
    /// </summary>
    /// <param name="go"></param>
    void OnClickUpGrade(GameObject go)
    {
        if (wingSpiritData.YulinglevelCfg != null &&
            wingSpiritData.YulinglevelCfg.levelUp.IsItemsEnough(showGetWay: true))
            Net.CSYuLingUpgradeMessage();
    }

    /// <summary>
    /// 点击一键镶嵌
    /// </summary>
    /// <param name="go"></param>
    void OnClickMosaic(GameObject go)
    {
        if (dicSlotData[1]?.lockFlag == 0 && dicSlotData[2]?.lockFlag == 0 && dicSlotData[3]?.lockFlag == 0)
        {
            UtilityTips.ShowRedTips(2005);
            return;
        }

        if (!CSWingInfo.Instance.IsHasBetterWing() &&
            !CSWingInfo.Instance.IsHasBetterWingColor() &&
            !CSWingInfo.Instance.IsHasBetterWingTechnique())
        {
            Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(1068));
        }
        else
        {
            //直接镶嵌更好的中最好的
            Net.CSYuLingSoulSetMessage();
        }
    }

    private ILBetterList<BagItemInfo> bagItemInfo = new ILBetterList<BagItemInfo>();

    /// <summary>
    /// 点击槽位添加(1羽翼 2幻彩 3羽技)
    /// </summary>
    /// <param name="go"></param>
    void OnClickAdd(GameObject go)
    {
        if (go == null) return;
        int index = (int) UIEventListener.Get(go).parameter;
        CSBagInfo.Instance.SetItemsByTypeAndSubType(bagItemInfo, 9, index + 5);
        switch (index)
        {
            case 1:
                if (bagItemInfo.Count > 0)
                    UIManager.Instance.CreatePanel<UIWingSoulChangeMosaicPanel>(f =>
                        (f as UIWingSoulChangeMosaicPanel).OpenWingSoulChangeMosaicPanel(1));
                else
                    Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(1065));
                break;
            case 2:
                if (bagItemInfo.Count > 0)
                    UIManager.Instance.CreatePanel<UIWingSoulChangeMosaicPanel>(f =>
                        (f as UIWingSoulChangeMosaicPanel).OpenWingSoulChangeMosaicPanel(2));
                else
                    Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(1066));
                break;
            case 3:
                if (bagItemInfo.Count > 0)
                    UIManager.Instance.CreatePanel<UIWingSoulChangeMosaicPanel>(f =>
                        (f as UIWingSoulChangeMosaicPanel).OpenWingSoulChangeMosaicPanel(3));
                else
                    Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(1067));
                break;
        }
    }

    /// <summary>
    /// 点击未解锁槽位
    /// </summary>
    /// <param name="go"></param>
    void OnClickLock(GameObject go)
    {
        if (go == null) return;
        // int index = (int) UIEventListener.Get(go).parameter;
        UtilityTips.ShowRedTips(1935);
    }

    void OnClickSpecialAttr(GameObject go)
    {
        if (go == null) return;
        int index = (int) UIEventListener.Get(go).parameter;
        switch (index)
        {
            case 1:
                if (mlb_wingAttr.text == CSString.Format(1943)) //查看详情
                {
                    UIManager.Instance.CreatePanel<UIWingSoulDetailedAttrsPanel>(f =>
                        (f as UIWingSoulDetailedAttrsPanel).OpenWingSoulDetailedAttrsPanel(index,
                            mlb_wingAttr.transform.position));
                }
                else if (mlb_wingAttr.text == CSString.Format(1942)) //前往获取
                {
                    Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(1065));
                }

                break;
            case 2:
                if (mlb_wingColorAttr.text == CSString.Format(1943)) //查看详情
                {
                    UIManager.Instance.CreatePanel<UIWingSoulDetailedAttrsPanel>(f =>
                        (f as UIWingSoulDetailedAttrsPanel).OpenWingSoulDetailedAttrsPanel(index,
                            mlb_wingColorAttr.transform.position));
                }
                else if (mlb_wingColorAttr.text == CSString.Format(1942)) //前往获取
                {
                    Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(1066));
                }

                break;
            case 3:
                if (mlb_wingTechniqueAttr.text == CSString.Format(1943)) //查看详情
                {
                    UIManager.Instance.CreatePanel<UIWingSoulDetailedAttrsPanel>(f =>
                        (f as UIWingSoulDetailedAttrsPanel).OpenWingSoulDetailedAttrsPanel(index,
                            mlb_wingTechniqueAttr.transform.position));
                }
                else if (mlb_wingTechniqueAttr.text == CSString.Format(1942)) //前往获取
                {
                    Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(1067));
                }

                break;
        }
    }

    void OnClickIcon(GameObject go)
    {
        if (go == null) return;
        int index = (int) UIEventListener.Get(go).parameter;
        switch (index)
        {
            case 1:
                if (CSWingInfo.Instance.DicSlotData.ContainsKey(1))
                {
                    if (CSWingInfo.Instance.DicSlotData[1].IsSoulMax)
                    {
                        UtilityTips.ShowRedTips(1969);
                    }
                    else
                    {
                        if (CSWingInfo.Instance.IsHasBetterWing())
                            UIManager.Instance.CreatePanel<UIWingSoulChangeMosaicPanel>(f =>
                                (f as UIWingSoulChangeMosaicPanel).OpenWingSoulChangeMosaicPanel(1));
                        else
                            UIManager.Instance.CreatePanel<UIWingSoulPromotePanel>(f =>
                                (f as UIWingSoulPromotePanel).OpenWingSoulPromotePanel(1));
                    }
                }

                break;
            case 2:
                if (CSWingInfo.Instance.DicSlotData.ContainsKey(2))
                {
                    if (CSWingInfo.Instance.DicSlotData[2].IsSoulMax)
                    {
                        UtilityTips.ShowRedTips(1969);
                    }
                    else
                    {
                        if (CSWingInfo.Instance.IsHasBetterWingColor())
                            UIManager.Instance.CreatePanel<UIWingSoulChangeMosaicPanel>(f =>
                                (f as UIWingSoulChangeMosaicPanel).OpenWingSoulChangeMosaicPanel(2));
                        else
                            UIManager.Instance.CreatePanel<UIWingSoulPromotePanel>(f =>
                                (f as UIWingSoulPromotePanel).OpenWingSoulPromotePanel(2));
                    }
                }

                break;
            case 3:
                if (CSWingInfo.Instance.DicSlotData.ContainsKey(3))
                {
                    if (CSWingInfo.Instance.DicSlotData[3].IsSoulMax)
                    {
                        UtilityTips.ShowRedTips(1969);
                    }
                    else
                    {
                        if (CSWingInfo.Instance.IsHasBetterWingTechnique())
                            UIManager.Instance.CreatePanel<UIWingSoulChangeMosaicPanel>(f =>
                                (f as UIWingSoulChangeMosaicPanel).OpenWingSoulChangeMosaicPanel(3));
                        else
                            UIManager.Instance.CreatePanel<UIWingSoulPromotePanel>(f =>
                                (f as UIWingSoulPromotePanel).OpenWingSoulPromotePanel(3));
                    }
                }

                break;
        }
    }

    /// <summary>
    /// 点击查看详情面板
    /// </summary>
    /// <param name="go"></param>
    void OnClickSeeDetails(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIWingSoulSeeDetailsPanel>();
    }

    public override void OnHide()
    {
        mbtn_wingSpiritAttr.Set(true);
        mbtn_wingSoul.Set(false);
        base.OnHide();
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mwing_spirit);
        CSEffectPlayMgr.Instance.Recycle(meffect_burst_point1);
        CSEffectPlayMgr.Instance.Recycle(meffect_burst_point2);
        CSEffectPlayMgr.Instance.Recycle(meffect_burst_point3);
        CSEffectPlayMgr.Instance.Recycle(meffect_add1);
        CSEffectPlayMgr.Instance.Recycle(meffect_add2);
        CSEffectPlayMgr.Instance.Recycle(meffect_add3);
        mgrid_effects.UnBind<UIWingSpiritAttrBinder>();
        mgrid_BaseAttr.UnBind<UIWingSoulBaseAttrBinder>();
        base.OnDestroy();
    }
}