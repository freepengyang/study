using System;
using System.Collections.Generic;
using activity;
using Google.Protobuf.Collections;
using TABLE;
using UnityEngine;

public partial class UIWingPanel : UIBasePanel
{
    /// <summary>
    /// 满星或者满阶或者其他显示
    /// </summary>
    private enum ShowType
    {
        Normal, //普通
        // MaxStar,//满星
        MaxAdvance, //满阶
    }

    public override bool ShowGaussianBlur => false;

    private ShowType curShowType = ShowType.Normal;

    private WingData myWingData;

    /// <summary>
    /// 当前选择的翅膀阶数
    /// </summary>
    private int selectWingIndex = 0;

    private UIItemBarPrefab[] uiItemBarPrefabs;

    /// <summary>
    /// 所有阶数的0星翅膀列表
    /// </summary>
    private ILBetterList<TABLE.WING> listWingAdvance = new ILBetterList<WING>();

    private ILBetterList<ILBetterList<int>> listEquipStarUp = new ILBetterList<ILBetterList<int>>();

    /// <summary>
    /// 翅膀最大阶数
    /// </summary>
    private int maxAdvance = 0;

    /// <summary>
    /// 当前星级
    /// </summary>
    private int starLevel = 0;

    /// <summary>
    /// 当前翅膀阶数
    /// </summary>
    private int curRank = 0;

    /// <summary>
    /// 当前翅膀最大星级(从0开始)
    /// </summary>
    private int curMaxStarLevel = 0;

    /// <summary>
    /// 当前翅膀配置
    /// </summary>
    WING wing;

    /// <summary>
    /// 当前升星升阶所需装备列表
    /// </summary>
    private LongArray equipInfo;

    UISpriteAnimation starEffect;
    private bool needStarEffect = true;

    ILBetterList<UISpriteAnimation> listAttrEffect;
    ILBetterList<bool> listNeedAttrEffect;

    /// <summary>
    /// 是否勾选上缴10次
    /// </summary>
    private bool isSelectTrunin10 = false;

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.WingStarUp, WingStarUp);
        mClientEvent.Reg((uint) CEvent.GetWingInfo, RefreshData);
        // mClientEvent.Reg((uint)CEvent.WingAdvance, RefreshData);//翅膀升阶弃用
        mClientEvent.Reg((uint) CEvent.DressWingColor, RefreshData);
        mClientEvent.Reg((uint) CEvent.WingExpItemUse, WingStarUp);
        mClientEvent.Reg((uint) CEvent.ItemListChange, RefreshDataItemChange);
        mClientEvent.Reg((uint) CEvent.MoneyChange, RefreshDataItemChange);
        mClientEvent.Reg((uint) CEvent.ResSpecialActivityDataMessage, RefreshGameModel);
        uiItemBarPrefabs = new UIItemBarPrefab[2]
        {
            new UIItemBarPrefab(mUIItemBarPrefab1, msp_icon1, mlb_value1, mbtn_add1, mbtn_sp1),
            new UIItemBarPrefab(mUIItemBarPrefab2, msp_icon2, mlb_value2, mbtn_add2, mbtn_sp2),
        };
        // mbtn_left.onClick = OnClickLeft;
        // mbtn_right.onClick = OnClickRight;
        mbtn_turnin.onClick = OnClickTurnIn;
        mbtn_turnin10.onClick = OnClickmTurnin10;
        mbtn_check10.onClick = OnClickmCheck10;
        mbtn_gameModel.onClick = OnClickmGameModel;
        mbtn_tips_bg.onClick = OnClickCloseTips;

        // msp_stage_effect_icon.gameObject.GetComponent<UIEventListener>().onDragStart = OnDragStart;
        // msp_stage_effect_icon.gameObject.GetComponent<UIEventListener>().onDragEnd = OnDragEnd;

        mbar.onChange.Add(new EventDelegate(OnChange));
        mbar_max.onChange.Add(new EventDelegate(OnChangeMax));
        
        maxAdvance = CSWingInfo.Instance.MaxAdvance;
        
        CSEffectPlayMgr.Instance.ShowUITexture(msp_stage_effect_bg, "wing_bg");
        CSEffectPlayMgr.Instance.ShowUIEffect(meffect_wing_idle_add, 17801);
    }

    void RefreshGameModel(uint id, object data)
    {
        SetGameModel();
    }

    void OnChange()
    {
        marrow.SetActive(mbar.value < 0.95);
    }

    void OnChangeMax()
    {
        marrow_max.SetActive(mbar_max.value < 0.95);
    }

    private const float DRAG_ANGLE_RATE = 0.2f;
    private const int MOVE_DISTANCE = 30;

    private Vector3 dragPos;
    private Vector3 mouseVector3;

    void OnDragStart(GameObject go)
    {
        mouseVector3 = Input.mousePosition;
    }

    void OnDragEnd(GameObject go)
    {
        UpdatePos(0, Input.mousePosition - mouseVector3);
    }

    void UpdatePos(uint id, object data)
    {
        dragPos = (Vector3) data;

        if (Mathf.Atan2(Mathf.Abs(dragPos.y), Mathf.Abs(dragPos.x)) <= DRAG_ANGLE_RATE)
        {
            if (dragPos.x > MOVE_DISTANCE)
                Right();
            else if (dragPos.x < -MOVE_DISTANCE)
                Left();
        }
    }


    public override void Show()
    {
        base.Show();
        if (CSWingInfo.Instance.MyWingData == null)
            Net.CSWingInfoMessage();
        else
        {
            mbar.value = 0;
            mbar_max.value = 0;
            isSelectTrunin10 = false;
            InitData();    
        }
    }


    void WingStarUp(uint id, object data)
    {
        wing.WingUpStarResponse msg = (wing.WingUpStarResponse) data;
        myWingData = CSWingInfo.Instance.MyWingData;
        if (WingTableManager.Instance.TryGetValue(myWingData.id, out wing))
        {
            int rank = (int) wing.rank;
            if (rank > curRank) //升阶
            {
                UIManager.Instance.CreatePanel<UIWingAdvanceSuccessPanel>();
                InitData();
            }
            else //升星
            {
                int starLevelChange = (int) wing.starID;
                if (starLevel < starLevelChange)
                {
                    starLevel = starLevelChange;
                    UtilityTips.ShowTips(563, 1.5f, ColorType.Green);
                    //升星特效
                    starEffect = mgrid_starts.controlList[starLevel - 1].transform.Find("sp_effect")
                        .GetComponent<UISpriteAnimation>();
                    if (starEffect != null && needStarEffect)
                    {
                        needStarEffect = false;
                        starEffect.gameObject.SetActive(true);
                        CSEffectPlayMgr.Instance.ShowUIEffect(starEffect.gameObject, "effect_star_add", 10, false);
                        starEffect.OnFinish = OnStarPlayFinish;
                    }

                    //属性特效
                    if (listAttrEffect != null && listNeedAttrEffect != null)
                    {
                        for (int i = 0; i < listAttrEffect.Count; i++)
                        {
                            if (listNeedAttrEffect[i])
                            {
                                listNeedAttrEffect[i] = false;
                                listAttrEffect[i].gameObject.SetActive(true);
                                CSEffectPlayMgr.Instance.ShowUIEffect(listAttrEffect[i].gameObject,
                                    "effect_dragon_levelup_add", 10, false, false);
                                if (i == listAttrEffect.Count - 1)
                                {
                                    listAttrEffect[i].OnFinish = OnAttrPlayFinish;
                                }
                            }
                        }
                    }
                    
                    ShowStarUpInfo();
                }
                else
                {
                    //经验
                    int curExp = myWingData.curExp; //当前经验
                    int upExp = (int) wing.starCostExp; //需要升级总经验
                    mslider_exp.value = (float) curExp / upExp;
                    CSStringBuilder.Clear();
                    mlb_exp.text = CSStringBuilder.Append(curExp, '/', upExp).ToString();
                }
            }
        }
    }

    protected void OnAttrPlayFinish()
    {
        for (int i = 0; i < listAttrEffect.Count; i++)
        {
            listAttrEffect[i].gameObject.SetActive(false);
            listNeedAttrEffect[i] = true;
        }
    }

    protected void OnStarPlayFinish()
    {
        starEffect.gameObject.SetActive(false);
        needStarEffect = true;
    }

    void RefreshData(uint id, object data)
    {
        InitData();
    }

    /// <summary>
    /// 道具和金币变化
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void RefreshDataItemChange(uint id, object data)
    {
        ShowEquips();
        SetTurnin(curShowType);
    }

    void InitData()
    {
        if (WingTableManager.Instance == null || WingTableManager.Instance.array.gItem.id2offset.Count <= 0)
        {
            // Debug.Log("翅膀系统配置表异常-----------@高飞");
            return;
        }

        myWingData = CSWingInfo.Instance.MyWingData;
        if (myWingData == null) return;
        if (!WingTableManager.Instance.TryGetValue(myWingData.id, out wing)) return;
        curRank = (int) wing.rank; //当前阶数
        curMaxStarLevel = 0;//当前最大星级赋值
        var arr = WingTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.WING;
            if (item.rank == curRank)
                curMaxStarLevel++;
        }
        curMaxStarLevel--;
        
        ShowWingInfo();
        ShowStarUpInfo();
    }


    private ILBetterList<GameObject> list_effect_handbook_card_adds;

    /// <summary>
    /// 显示当前阶数翅膀信息
    /// </summary>
    void ShowWingInfo()
    {
        //当前阶数翅膀信息
        selectWingIndex = curRank;
        string model = wing.model.ToString(); //模型id（玩家身上的）
        string icon = wing.pic; //图标
        mlb_title.text = CSString.Format(706, curRank.ToString());
        if (wing.id == 1000) //如果是1阶0星  特殊处理
        {
            if (WingTableManager.Instance.ContainsKey(1001))
            {
                CSEffectPlayMgr.Instance.ShowUIEffect(msp_stage_effect_icon.gameObject, (WingTableManager.Instance.array.gItem.id2offset[1001].Value as TABLE.WING).model.ToString(),
                    ResourceType.UIWing, 6);
            }
        }
        else
        {
            CSEffectPlayMgr.Instance.ShowUIEffect(msp_stage_effect_icon.gameObject, model, ResourceType.UIWing, 6);
        }

        // msp_stage_effect_icon.spriteName = icon;

        mobj_tips.SetActive(false);

        //所有阶数的信息
        listWingAdvance.Clear();
        WING wingItem;
        var arr = WingTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            wingItem = arr[k].Value as TABLE.WING;
            if (wingItem.starID == 0 && wingItem.rankType != 0)
            {
                listWingAdvance.Add(wingItem);
            }
        }

        if (list_effect_handbook_card_adds == null)
            list_effect_handbook_card_adds = new ILBetterList<GameObject>();


        mgrid_advance.MaxCount = listWingAdvance.Count;
        GameObject gp;
        ScriptBinder gpBinder;
        UILabel lb_name;
        UISprite sp_icon;
        // GameObject obj_select;
        GameObject effect_handbook_card_add;
        UISprite sp_quality;
        GameObject bg;
        for (int i = 0; i < mgrid_advance.MaxCount; i++)
        {
            gp = mgrid_advance.controlList[i];
            gpBinder = gp.transform.GetComponent<ScriptBinder>();
            lb_name = gpBinder.GetObject("lb_name") as UILabel;
            sp_icon = gpBinder.GetObject("sp_icon") as UISprite;
            // obj_select = gpBinder.GetObject("select") as GameObject;
            effect_handbook_card_add = gpBinder.GetObject("effect_handbook_card_add") as GameObject;
            sp_quality = gpBinder.GetObject("sp_quality") as UISprite;
            bg = gpBinder.GetObject("bg") as GameObject;

            if (list_effect_handbook_card_adds.Count <= i)
                list_effect_handbook_card_adds.Add(effect_handbook_card_add);
            CSEffectPlayMgr.Instance.ShowUIEffect(list_effect_handbook_card_adds[i], "effect_handbook_card_add");
            if (listWingAdvance[i].rank > wing.rank) //未激活的阶数
            {
                lb_name.text = CSString.Format(708, listWingAdvance[i].rank);
                sp_icon.spriteName = listWingAdvance[i].pic;
                sp_icon.color = Color.black; //置灰
                effect_handbook_card_add.SetActive(false);
                //品质暂时不明
                // sp_quality.spriteName
            }
            else //已激活的阶数
            {
                lb_name.text = CSString.Format(707);
                sp_icon.spriteName = listWingAdvance[i].pic;
                sp_icon.color = Color.white;
                effect_handbook_card_add.SetActive(true);
                //品质暂时不明
                // sp_quality.spriteName
            }

            UIEventListener.Get(bg, i).onClick = OnClickIcon;
        }
    }
    
    CSBetterLisHot<IntArray> attrInfo = new CSBetterLisHot<IntArray>();
    private RepeatedField<int> ids = new RepeatedField<int>();
    private RepeatedField<int> values = new RepeatedField<int>();
    CSBetterLisHot<IntArray> attrInfoNext = new CSBetterLisHot<IntArray>();
    private RepeatedField<int> idsNext = new RepeatedField<int>();
    private RepeatedField<int> valuesNext = new RepeatedField<int>();
    
    /// <summary>
    /// 显示当前升星信息
    /// </summary>
    void ShowStarUpInfo()
    {
        starLevel = (int) wing.starID;
        //星级
        mgrid_starts.MaxCount = curMaxStarLevel;
        GameObject gp;
        ScriptBinder gpBinder;
        GameObject sp_front;
        for (int i = 0; i < mgrid_starts.MaxCount; i++)
        {
            gp = mgrid_starts.controlList[i];
            gpBinder = gp.transform.GetComponent<ScriptBinder>();
            sp_front = gpBinder.GetObject("sp_front") as GameObject;
            sp_front.SetActive(starLevel > i);
        }

        if (wing.rank == maxAdvance && wing.starID == curMaxStarLevel) //满阶满星
        {
            curShowType = ShowType.MaxAdvance;
            ShowByShowType(curShowType);
            //属性只有单排
            SetAttrParaByCareer(attrInfo, CSMainPlayerInfo.Instance.Career);
            CSWingInfo.Instance.SetWingAddition(attrInfo, ids, values, CSMainPlayerInfo.Instance.Career);
            RepeatedField<CSAttributeInfo.KeyValue> attrItems = null;
            // if (attrInfo != null && attrInfo.Count == 2)
            // {
            //     attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, attrInfo[0], attrInfo[1]);
            // }
            if (ids.Count>0&&values.Count>0)
            {
                attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, ids, values);
            }

            UILabel lb_name;
            mgrid_effects_max.MaxCount = attrItems.Count;
            for (int i = 0; i < mgrid_effects_max.MaxCount; i++)
            {
                gp = mgrid_effects_max.controlList[i];
                gpBinder = gp.transform.GetComponent<ScriptBinder>();
                lb_name = gpBinder.GetObject("lb_name") as UILabel;
                //当前属性
                if (attrItems != null)
                {
                    CSStringBuilder.Clear();
                    lb_name.text = CSStringBuilder.Append("[cbb694]", attrItems[i].Key, CSString.Format(999),
                        "[-][DCD5B8]", attrItems[i].Value, "[-]").ToString();
                }
            }

            marrow_max.SetActive(mbar_max.value < 0.95);
            marrow.SetActive(false);
        }
        else //非满阶满星
        {
            curShowType = ShowType.Normal;
            ShowByShowType(curShowType);
            //属性加成（双排）
            SetAttrParaByCareer(attrInfo, CSMainPlayerInfo.Instance.Career);
            CSWingInfo.Instance.SetWingAddition(attrInfo, ids, values, CSMainPlayerInfo.Instance.Career);
            RepeatedField<CSAttributeInfo.KeyValue> attrItems = null;
            // if (attrInfo != null && attrInfo.Count == 2)
            // {
            //     attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, attrInfo[0], attrInfo[1]);
            // }
            if (ids.Count > 0 && values.Count > 0)
            {
                attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, ids, values);
            }

            //下一星级属性加成
            SetAttrParaByCareer(attrInfoNext, CSMainPlayerInfo.Instance.Career, true);
            CSWingInfo.Instance.SetWingAddition(attrInfoNext, idsNext, valuesNext, CSMainPlayerInfo.Instance.Career);
            RepeatedField<CSAttributeInfo.KeyValue> attrItemsNext = null;
            // if (attrInfoNext != null && attrInfoNext.Count == 2)
            // {
            //     attrItemsNext =
            //         CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, attrInfoNext[0], attrInfoNext[1]);
            // }
            if (idsNext.Count > 0 && valuesNext.Count > 0)
            {
                attrItemsNext = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, idsNext, valuesNext);
            }

            listAttrEffect = new ILBetterList<UISpriteAnimation>();
            listAttrEffect.Clear();
            UILabel lb_name;
            UILabel lb_nextName;
            UISpriteAnimation spriteAnimation;
            listNeedAttrEffect = new ILBetterList<bool>();
            listNeedAttrEffect.Clear();
            mgrid_effects.MaxCount = attrItems.Count;
            for (int i = 0; i < mgrid_effects.MaxCount; i++)
            {
                gp = mgrid_effects.controlList[i];
                gpBinder = gp.transform.GetComponent<ScriptBinder>();
                lb_name = gpBinder.GetObject("lb_name") as UILabel;
                lb_nextName = gpBinder.GetObject("lb_nextName") as UILabel;
                spriteAnimation = gpBinder.GetObject("lb_upgrade_effect") as UISpriteAnimation;
                listAttrEffect.Add(spriteAnimation);
                listNeedAttrEffect.Add(true);
                //当前属性
                if (attrItems != null)
                {
                    CSStringBuilder.Clear();
                    lb_name.text = CSStringBuilder.Append("[cbb694]", attrItems[i].Key,
                        CSString.Format(999), "[-][DCD5B8]", attrItems[i].Value, "[-]").ToString();
                }

                //下一星属性
                if (attrItemsNext != null)
                {
                    CSStringBuilder.Clear();
                    lb_nextName.text = CSStringBuilder.Append("[cbb694]", attrItemsNext[i].Key, CSString.Format(999),
                        "[-][00ff0c]", attrItemsNext[i].Value, "[-]").ToString();
                }
            }

            marrow.SetActive(mbar.value < 0.95);
            marrow_max.SetActive(false);

            //经验
            int curExp = myWingData.curExp; //当前经验
            int upExp = (int) wing.starCostExp; //需要升级总经验
            mslider_exp.value = (float) curExp / upExp;
            CSStringBuilder.Clear();
            mlb_exp.text = CSStringBuilder.Append(curExp, '/', upExp).ToString();
            // 需要的装备
            ShowEquips();
        }
    }

    
    ILBetterList<int> tempList = new ILBetterList<int>();
    /// <summary>
    /// 显示需要的装备或金币
    /// </summary>
    void ShowEquips()
    {
        equipInfo = wing.starCost;
        uiItemBarPrefabs[0].prefab.SetActive(false);
        uiItemBarPrefabs[1].prefab.SetActive(false);
        long curNum = 0;
        int maxNum = 0;
        listEquipStarUp.Clear();
        for (int i = 0; i < equipInfo.Count; i++)
        {
            tempList.Clear();
            ITEM cfg;
            if (ItemTableManager.Instance.TryGetValue(equipInfo[i].key(), out cfg))
            {
                uiItemBarPrefabs[i].prefab.SetActive(true);
                uiItemBarPrefabs[i].sp_icon.spriteName = $"tubiao{cfg.icon}";
                curNum = CSBagInfo.Instance.GetAllItemCount(cfg.id);
                maxNum = (int) equipInfo[i].value();
                tempList.Add(cfg.id);
                tempList.Add(curNum >= maxNum ? 1 : 0);
                listEquipStarUp.Add(tempList);
                string color;
                switch (cfg.type)
                {
                    case 1: //货币
                        color = curNum < maxNum ? "[ff0000]" : "[00ff0c]";
                        uiItemBarPrefabs[i].lb_value.text = $"{color}{UtilityMath.GetDecimalTenThousandValue(maxNum)}";
                        break;
                    default:
                        color = curNum < maxNum ? "[ff0000]" : "[00ff0c]";
                        uiItemBarPrefabs[i].lb_value.text = $"{color}{curNum}/{maxNum}";
                        break;
                }
            }

            UIEventListener.Get(uiItemBarPrefabs[i].btn_add.gameObject, equipInfo[i].key()).onClick = OnClickAdd;
            UIEventListener.Get(uiItemBarPrefabs[i].btn_sp.gameObject).onClick = o =>
            {
                UITipsManager.Instance.CreateTips(TipsOpenType.Normal, cfg.id);
            };
        }

        mgrid_equip.repositionNow = true;
        mgrid_equip.Reposition();
    }

    /// <summary>
    /// 根据类型显示/隐藏当前游戏物体
    /// </summary>
    /// <param name="type"></param>
    void ShowByShowType(ShowType type)
    {
        switch (type)
        {
            case ShowType.Normal:
                mfix.gameObject.SetActive(true);
                mfix.text = CSString.Format(709);
                mslider_exp.gameObject.SetActive(true);
                mgrid_equip.gameObject.SetActive(true);
                mlb_maxtips.gameObject.SetActive(false);
                mScrollView_effects.gameObject.SetActive(true);
                mScrollView_effects_max.gameObject.SetActive(false);
                break;
            case ShowType.MaxAdvance:
                mfix.gameObject.SetActive(false);
                mslider_exp.gameObject.SetActive(false);
                mgrid_equip.gameObject.SetActive(false);
                mlb_maxtips.gameObject.SetActive(true);
                mlb_maxtips.text = CSString.Format(711);
                mScrollView_effects.gameObject.SetActive(false);
                mScrollView_effects_max.gameObject.SetActive(true);
                break;
        }

        SetTurnin(type);
    }

    /// <summary>
    /// 上缴相关
    /// </summary>
    void SetTurnin(ShowType type)
    {
        switch (type)
        {
            case ShowType.Normal:
                //上缴红点
                mred_turnin.SetActive(CSWingInfo.Instance.IsUpStar());
                if (!IsEnough10())
                    isSelectTrunin10 = false;
                mbtn_turnin.gameObject.SetActive(!isSelectTrunin10);
                mbtn_turnin10.gameObject.SetActive(isSelectTrunin10);
                mbtn_check10.gameObject.SetActive(IsEnough10());
                mchoose.SetActive(isSelectTrunin10);
                SetGameModel();
                break;
            case ShowType.MaxAdvance:
                mbtn_turnin.gameObject.SetActive(false);
                mbtn_turnin10.gameObject.SetActive(false);
                mbtn_check10.gameObject.SetActive(false);
                mlb_gameModel.gameObject.SetActive(false);
                mchoose.SetActive(false);
                break;
        }
    }

    Map<int, long> mapItemCount = new Map<int, long>();

    /// <summary>
    /// 是否足够上缴10次
    /// </summary>
    /// <returns></returns>
    bool IsEnough10()
    {
        if (wing == null || myWingData == null) return false;
        TABLE.WING curWing = wing; //当前升到的翅膀的配置
        int curExp = myWingData.curExp; //当前经验
        int curstarCostExp = (int) curWing.starCostExp; //现在升级上限经验(叠加)
        mapItemCount.Clear();
        for (int i = 0; i < 10; ++i)
        {
            var cost = curWing.starCost;
            for (int j = 0; j < cost.Count; j++)
            {
                var kv = cost[j];
                TABLE.ITEM item;
                if (!ItemTableManager.Instance.TryGetValue(kv.key(), out item))
                    continue;

                if (!mapItemCount.ContainsKey(kv.key()))
                    mapItemCount.Add(kv.key(), kv.key().GetItemCount());

                long owned = mapItemCount[kv.key()];
                if (kv.value() > owned)
                {
                    return false;
                }
                else
                {
                    mapItemCount[kv.key()] -= kv.value();
                }

                if (item.type > 1) //非货币
                {
                    int exp = 0;
                    int addExp = 0;
                    if (Int32.TryParse(item.bufferParam, out exp))
                        addExp = exp * (int) kv.value();
                    
                    curExp += addExp;

                    //升阶或者升星或者溢出
                    if (curExp >= curstarCostExp)
                    {
                        TABLE.WING wingCfg;
                        if (WingTableManager.Instance.TryGetValue(curWing.id + 1, out wingCfg)) //是否有当前阶下一星
                        {
                            //升星
                            curWing = wingCfg;
                            curstarCostExp += (int) curWing.starCostExp;
                        }
                        else
                        {
                            if (curWing.rank == maxAdvance && curExp > curstarCostExp) //超出满阶满星
                            {
                                return false;
                            }

                            if (curWing.rank < maxAdvance)
                            {
                                if (WingTableManager.Instance.TryGetValue((int) ((curWing.rank + 1) * 1000), out wingCfg))
                                {
                                    //升阶
                                    curWing = wingCfg;
                                    curstarCostExp += (int) curWing.starCostExp;
                                }
                            }
                        }
                    }
                }
            }
        }

        return true;
    }

    /// <summary>
    /// 设置超链接显示隐藏
    /// </summary>
    void SetGameModel()
    {
        //优惠礼包里羽翼礼包有没有购买完
        mbtn_gameModel.gameObject.SetActive(CSDiscountGiftBagInfo.Instance.IsHasWingGiftBag());
        if (mbtn_gameModel.gameObject.activeSelf)
            mlb_gameModel.text = CSString.Format(curRank < 3 ? 1697 : 1698);
    }

    /// <summary>
    /// 根据职业获取相应属性id和数值
    /// </summary>
    /// <param name="career"></param>
    void SetAttrParaByCareer(CSBetterLisHot<IntArray> listAttrInfo, int career, bool isNext = false)
    {
        listAttrInfo.Clear();
        WING wingTable;
        switch (career)
        {
            case 1:
                if (isNext)
                {
                    wingTable = GetNextStarAttrPara(myWingData.id);
                    if (wingTable != null)
                    {
                        listAttrInfo.Add(wingTable.zsstarAttrPara);
                        listAttrInfo.Add(wingTable.starAttrNum);
                    }
                }
                else
                {
                    listAttrInfo.Add(wing.zsstarAttrPara);
                    listAttrInfo.Add(wing.starAttrNum);
                }

                break;
            case 2:
                if (isNext)
                {
                    wingTable = GetNextStarAttrPara(myWingData.id);
                    if (wingTable != null)
                    {
                        listAttrInfo.Add(wingTable.fsstarAttrPara);
                        listAttrInfo.Add(wingTable.starAttrNum);
                    }
                }
                else
                {
                    listAttrInfo.Add(wing.fsstarAttrPara);
                    listAttrInfo.Add(wing.starAttrNum);
                }

                break;
            case 3:
                if (isNext)
                {
                    wingTable = GetNextStarAttrPara(myWingData.id);
                    if (wingTable != null)
                    {
                        listAttrInfo.Add(wingTable.dsstarAttrPara);
                        listAttrInfo.Add(wingTable.starAttrNum);
                    }
                }
                else
                {
                    listAttrInfo.Add(wing.dsstarAttrPara);
                    listAttrInfo.Add(wing.starAttrNum);
                }
                break;
        }
    }

    /// <summary>
    /// 获取下一星级数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    TABLE.WING GetNextStarAttrPara(int id)
    {
        WING cmpWing = WingTableManager.Instance.array.gItem.id2offset[id].Value as TABLE.WING;
        WING wingItem;
        //下一星
        var arr = WingTableManager.Instance.array.gItem.handles;
        for (int k = 0,max = arr.Length;k < max;++k)
        {
            wingItem = arr[k].Value as TABLE.WING;
            if (wingItem.rank == cmpWing.rank && wingItem.starID == cmpWing.starID + 1)
            {
                return wingItem;
            }
        }

        //下一阶
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            wingItem = arr[k].Value as TABLE.WING;
            if (wingItem.rank == cmpWing.rank + 1 && wingItem.starID == 0)
            {
                return wingItem;
            }
        }

        return null;
    }

    /// <summary>
    /// 点击不同阶数翅膀小图标
    /// </summary>
    /// <param name="go"></param>
    void OnClickIcon(GameObject go)
    {
        if (go == null) return;
        int index = (int) UIEventListener.Get(go).parameter;
        if (listWingAdvance[index].rankType == 0) return;
        mlb_tips_title.text =
            CSString.Format(712, listWingAdvance[index].rank);
        msp_icon.spriteName = listWingAdvance[index].pic;
        if (listWingAdvance[index].rank > wing.rank)
        {
            mlb_isactive.text = CSString.Format(716);
        }
        else
        {
            mlb_isactive.text = CSString.Format(717);
            msp_icon.color = Color.white;
        }

        if (listWingAdvance[index].rankType == 1) //属性绝对值
        {
            string attrName =
                CSString.Format(
                    ClientAttributeTableManager.Instance.GetClientAttributeTipID((int) listWingAdvance[index]
                        .rankPara));

            int addition = CSWingInfo.Instance.GetWingTechniqueAddition();
            string attrNum = $"{listWingAdvance[index].rankNum * 1f / 100 *((addition+10000)*1f/10000)}%";
            CSStringBuilder.Clear();
            mlb_tips_content.text = CSStringBuilder.Append("[dcd5b8]", attrName, CSString.Format(999), "[-]",
                "[00ff0c]", attrNum, "[-]").ToString();
        }
        else if (listWingAdvance[index].rankType == 2) //暂时没有
        {
        }
        else if (listWingAdvance[index].rankType == 3) //技能
        {
            var mapSkill = SkillTableManager.Instance.array.gItem.id2offset;
            if (mapSkill != null)
            {
                var mapSkillItem = mapSkill[(int)listWingAdvance[index].rankNum].Value as TABLE.SKILL;
                string skillName = mapSkillItem.name;
                string skillDes = mapSkillItem.description;
                CSStringBuilder.Clear();
                mlb_tips_content.text = CSStringBuilder.Append(skillName, CSString.Format(999), skillDes).ToString();
            }
        }

        //自适应
        msp_tips.height = msp_icon.height + mlb_tips_content.height + 33;
        msp_tips.autoResizeBoxCollider = true;
        msp_tips.ResizeCollider();
        float x = 0;
        x = mgrid_advance.controlList[index].transform.position.x;
        Vector3 pos = Vector3.right * (x - mobj_tips.transform.position.x);
        mobj_tips.transform.position += pos;
        pos = Vector3.right * (-30);
        mobj_tips.transform.localPosition += pos;
        mobj_tips.SetActive(listWingAdvance[index].rankType != 2 && listWingAdvance[index].rankType != 0);
    }

    /// <summary>
    /// 点击关闭tips
    /// </summary>
    /// <param name="go"></param>
    void OnClickCloseTips(GameObject go)
    {
        if (mobj_tips.activeSelf)
        {
            mobj_tips.SetActive(false);
        }
    }

    /// <summary>
    /// 点击往左
    /// </summary>
    /// <param name="go"></param>
    void OnClickLeft(GameObject go)
    {
        if (go == null) return;
        Left();
    }

    void Left()
    {
        if (selectWingIndex > 1)
        {
            selectWingIndex--;
            mlb_title.text = CSString.Format(706, selectWingIndex);
            WING wingItem;
            var arr = WingTableManager.Instance.array.gItem.handles;
            for(int i = 0,max = arr.Length;i < max;++i)
            {
                wingItem = arr[i].Value as TABLE.WING;
                if (wingItem.rank == selectWingIndex)
                {
                    CSEffectPlayMgr.Instance.ShowUIEffect(msp_stage_effect_icon.gameObject, wingItem.model.ToString(),
                        ResourceType.UIWing, 6);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 点击往右
    /// </summary>
    /// <param name="go"></param>
    void OnClickRight(GameObject go)
    {
        if (go == null) return;
        Right();
    }

    void Right()
    {
        if (selectWingIndex < maxAdvance)
        {
            selectWingIndex++;
            mlb_title.text = CSString.Format(706, selectWingIndex);
            WING wingItem;
            var arr = WingTableManager.Instance.array.gItem.handles;
            for (int i = 0, max = arr.Length; i < max; ++i)
            {
                wingItem = arr[i].Value as TABLE.WING;
                if (wingItem.rank == selectWingIndex)
                {
                    CSEffectPlayMgr.Instance.ShowUIEffect(msp_stage_effect_icon.gameObject, wingItem.model.ToString(),
                        ResourceType.UIWing, 6);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 点击所需道具
    /// </summary>
    /// <param name="go"></param>
    void OnClickAdd(GameObject go)
    {
        if (go == null) return;
        int itemId = (int) UIEventListener.Get(go).parameter;
        //获取途径
        Utility.ShowGetWay(itemId);
    }

    /// <summary>
    /// 点击上缴
    /// </summary>
    /// <param name="go"></param>
    void OnClickTurnIn(GameObject go)
    {
        if (go == null) return;
        bool isItemsEnough = equipInfo.IsItemsEnough(showGetWay: true);
        //存在不满足条件的道具
        if (isItemsEnough)
        {
            // for (int i = 0; i < 100; i++)
            // {
            Net.CSWingUpStarMessage(1);
            // }
        }
    }

    /// <summary>
    /// 点击提升10次
    /// </summary>
    /// <param name="go"></param>
    void OnClickmTurnin10(GameObject go)
    {
        if (go == null) return;
        Net.CSWingUpStarMessage(10);
    }

    void OnClickmCheck10(GameObject go)
    {
        if (go == null) return;
        isSelectTrunin10 = !isSelectTrunin10;
        mchoose.SetActive(isSelectTrunin10);
        SetTurnin(ShowType.Normal);
    }

    void OnClickmGameModel(GameObject go)
    {
        if (go == null) return;
        UtilityPanel.JumpToPanel(12611);
    }

    public override void OnHide()
    {
        base.OnHide();
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(meffect_wing_idle_add);
        CSEffectPlayMgr.Instance.Recycle(msp_stage_effect_icon.gameObject);
        CSEffectPlayMgr.Instance.Recycle(msp_stage_effect_bg);
        if (list_effect_handbook_card_adds != null && list_effect_handbook_card_adds.Count > 0)
        {
            for (int i = 0; i < list_effect_handbook_card_adds.Count; i++)
            {
                CSEffectPlayMgr.Instance.Recycle(list_effect_handbook_card_adds[i]);
            }
        }

        base.OnDestroy();
    }
}

public class UIItemBarPrefab
{
    public UIItemBarPrefab(GameObject go, UISprite sp, UILabel lb, UIEventListener btn, UIEventListener btnSp)
    {
        prefab = go;
        sp_icon = sp;
        lb_value = lb;
        btn_add = btn;
        btn_sp = btnSp;
    }

    public GameObject prefab;
    public UISprite sp_icon;
    public UILabel lb_value;
    public UIEventListener btn_add;
    public UIEventListener btn_sp;
    public void Recycle()
    {
        prefab = null;
        sp_icon = null;
        lb_value = null;
        btn_add = null;
        btn_sp = null;
    }
}