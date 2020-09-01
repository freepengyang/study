using System;
using System.Collections;
using System.Collections.Generic;
using bag;
using baozhu;
using Google.Protobuf.Collections;
using TABLE;
using UnityEngine;

public partial class UIPearlUpgradePanel : UIBasePanel
{
    /// <summary>
    /// 可吞噬装备选中类型
    /// </summary>
    enum TypeSelect
    {
        None, //无操作
        Select, //强制选中
        NonSelect, //强制不选中
    }

    public override bool ShowGaussianBlur
    {
        get => false;
    }

    private PearlData myPearlData;
    private List<bag.BagItemInfo> bagItemInfos;

    private int selectIndex = 0;
    private int lastSelectIndex = 0;

    private BagItemInfo curSelectPear;

    /// <summary>
    /// 可吞噬装备
    /// </summary>
    private List<BagItemInfo> devourEquips;

    /// <summary>
    /// 已选择的可吞噬装备bagIndex列表
    /// </summary>
    private List<int> selectEquips;

    /// <summary>
    /// 宝珠缓存池
    /// </summary>
    List<UIItemBase> itemListBaoZhu;

    /// <summary>
    /// 是否一键选择
    /// </summary>
    private bool isSelectComplete = false;

    private string green = "[00ff0c]";
    private string main = "[cbb694]";


    #region 循环滚动相关变量

    /// <summary>
    /// 每页最多能显示的物品数量
    /// </summary>
    private const int EquipMaxCount = 16;

    /// <summary>
    /// 上一个wrap中的真实索引（所有整数）
    /// </summary>
    int lastRealIndex = 0;

    /// <summary>
    /// 当前页数
    /// </summary>
    private int curPage = 1;

    /// <summary>
    /// 总页数
    /// </summary>
    private int maxPage = 1;

    /// <summary>
    /// Grid0当前页数
    /// </summary>
    private int gridPage0 = 1;

    /// <summary>
    /// Grid1当前页数
    /// </summary>
    private int gridPage1 = 1;

    /// <summary>
    /// 当前在page中间位置的Grid，0为Grid0，1为Grid1
    /// </summary>
    //private int curCenterGrid = 0;

    /// <summary>
    /// Grid0缓存池
    /// </summary>
    private List<UIItemBase> itemBaseList0 = new List<UIItemBase>();

    /// <summary>
    /// Grid1缓存池
    /// </summary>
    private List<UIItemBase> itemBaseList1 = new List<UIItemBase>();

    private UIGridContainer curGrid;
    private List<UIItemBase> curItemBaseList;

    #endregion

    private string risk = "";

    private float curSliderValue = 0f;

    /// <summary>
    /// 当前宝珠升了多少级
    /// </summary>
    private int upLevelCount = -1;


    public override void Init()
    {
        base.Init();
        risk = CSString.Format(999);
        mClientEvent.Reg((uint) CEvent.LevelUpBaoZhu, RefreshData);
        mClientEvent.Reg((uint) CEvent.ItemListChange, RefreshData);
        mClientEvent.Reg((uint) CEvent.MoneyChange, RefreshData);
        mbtn_rule.onClick = OnClickRule;
        mbtn_select.onClick = OnClickSelect;
        mbtn_upgrade.onClick = OnClickUpGrade;
        mlb_hint.onClick = OnClickRequire;
        mlb_hintMax.onClick = OnClickHintMax;
        mlb_hintNoDevours.onClick = OnClickHintNoDevours;

        //无限滚动
        mwrap_page.onInitializeItem = WrapUpdate;
        mwrap_page.GetComponent<UICenterOnChild>().onCenter = OnCenter;
    }

    /// <summary>
    /// 点击跳转获取吞噬物品
    /// </summary>
    /// <param name="go"></param>
    void OnClickHintNoDevours(GameObject go)
    {
        Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(661));
    }

    /// <summary>
    /// 点击跳转到宝珠进化
    /// </summary>
    /// <param name="go"></param>
    void OnClickHintMax(GameObject go)
    {
        BagItemInfo bagItemInfo = null;
        if (curSelectPear != null)
        {
            bagItemInfo = curSelectPear;
        }

        UIManager.Instance.CreatePanel<UIPearlCombinedPanel>(f =>
            {
                UIPearlEvolutionPanel uiPearlEvolutionPanel =
                    (f as UIPearlCombinedPanel).OpenChildPanel((int) UIPearlCombinedPanel.ChildPanelType.CPT_Evolution)
                    as UIPearlEvolutionPanel;
                if (uiPearlEvolutionPanel != null && bagItemInfo != null)
                {
                    uiPearlEvolutionPanel.ShowPearEvolution(bagItemInfo.id);
                }
            }
        );
    }

    void OnClickRequire(GameObject go)
    {
        if (go == null) return;
        Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(573));
    }

    void RefreshData(uint id, object data)
    {
        baozhu.ResLevelUpBaoZhu msg = (baozhu.ResLevelUpBaoZhu) data;
        isSelectComplete = false;
        myPearlData = CSPearlInfo.Instance.MyPearlData;
        if (myPearlData == null) return;

        for (int i = 0; i < bagItemInfos.Count; i++)
        {
            if (bagItemInfos[i].id == msg.equip.equip.id)
            {
                upLevelCount = msg.equip.equip.gemLevel - bagItemInfos[i].gemLevel;
                break;
            }
        }

        SortPearl();
        lastSelectIndex = selectIndex;
        bool isExist = false;
        for (int i = 0; i < bagItemInfos.Count; i++)
        {
            if (bagItemInfos[i].id == msg.equip.equip.id)
            {
                selectIndex = i;
                isExist = true;
            }
        }

        if (!isExist)
        {
            selectIndex = 0;
            lastSelectIndex = 0;
        }

        SortDevourEquips();
        InitGrid();
        SetSelectItemInfo(selectIndex);
        SetRightInfo(selectIndex);

        UtilityTips.ShowTips(563, 1.5f, ColorType.Green);
    }

    /// <summary>
    /// 打开说明面板
    /// </summary>
    /// <param name="go"></param>
    void OnClickRule(GameObject go)
    {
        if (go == null) return;
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.UpLevelBaoZhu);
    }

    /// <summary>
    /// 一键选择
    /// </summary>
    /// <param name="go"></param>
    void OnClickSelect(GameObject go)
    {
        if (go == null) return;

        if (BaoZhuJinHuaTableManager.Instance.GetBaoZhuJinHuaMaxLevel(bagItemInfos[selectIndex].gemGrade) ==
            bagItemInfos[selectIndex].gemLevel)
        {
            //等级达到上限，需要进化才能继续升级
            UtilityTips.ShowTips(925, 1.5f, ColorType.Red);
            return;
        }

        isSelectComplete = !isSelectComplete;
        if (devourEquips != null)
        {
            if (isSelectComplete)
            {
                int myLevel = CSMainPlayerInfo.Instance.Level;
                for (int i = 0; i < devourEquips.Count; i++)
                {
                    if (myLevel - ItemTableManager.Instance.GetItemLevel(devourEquips[i].configId) >= 20)
                    {
                        RefeshEquipItem(i, TypeSelect.Select);
                    }
                    else
                    {
                        RefeshEquipItem(i, TypeSelect.NonSelect);
                    }
                }

                if (selectEquips.Count <= 0)
                {
                    UtilityTips.ShowTips(1112, 1.5f, ColorType.Red);
                    isSelectComplete = false;
                }
            }
            else
            {
                selectEquips.Clear();
                for (int i = 0; i < devourEquips.Count; i++)
                {
                    RefeshEquipItem(i, TypeSelect.NonSelect);
                }
            }
        }
    }

    /// <summary>
    /// 点击宝珠升级
    /// </summary>
    /// <param name="go"></param>
    void OnClickUpGrade(GameObject go)
    {
        if (go == null) return;
        if (BaoZhuJinHuaTableManager.Instance.GetBaoZhuJinHuaMaxLevel(bagItemInfos[selectIndex].gemGrade) ==
            bagItemInfos[selectIndex].gemLevel)
        {
            //等级达到上限，需要进化才能继续升级
            UtilityTips.ShowTips(925, 1.5f, ColorType.Red);
            return;
        }

        if (selectEquips.Count > 0)
        {
            //当前加上的总经验
            int addExp = 0;
            for (int i = 0; i < selectEquips.Count; i++)
            {
                for (int j = 0; j < devourEquips.Count; j++)
                {
                    if (devourEquips[j].bagIndex == selectEquips[i])
                    {
                        // addExp += ItemTableManager.Instance.GetItemDevouredExp(devourEquips[j].configId);
                        break;
                    }
                }
            }

            //当前等级上限
            int levelLimit =
                BaoZhuJinHuaTableManager.Instance.GetBaoZhuJinHuaMaxLevel(bagItemInfos[selectIndex].gemGrade);
            //宝珠表索引
            int baozhuId = bagItemInfos[selectIndex].quality * 1000 + bagItemInfos[selectIndex].gemLevel;
            //当前等级升级所需经验
            int expUpdate = BaoZhuTableManager.Instance.GetBaoZhuExp(baozhuId) - bagItemInfos[selectIndex].gemExp;
            //当前升到等级上限所需经验
            int expUpdateLevelLimit = expUpdate;
            var arr = BaoZhuTableManager.Instance.array.gItem.handles;
            for(int k = 0,max = arr.Length;k < max;++k)
            {
                var value = arr[k].Value as TABLE.BAOZHU;
                if (value.quality == bagItemInfos[selectIndex].quality &&
                    value.grade > bagItemInfos[selectIndex].gemLevel &&
                    value.grade <= levelLimit)
                {
                    expUpdateLevelLimit += value.exp;
                }
            }


            RepeatedField<int> repeatedEquips = new RepeatedField<int>();
            for (int i = 0; i < selectEquips.Count; i++)
            {
                repeatedEquips.Add(selectEquips[i]);
            }

            //如果超过等级限制上限
            if (addExp > expUpdateLevelLimit)
            {
                //弹提示框
                string tittle = PromptWordTableManager.Instance.GetPromptWordTitle(59);
                System.Action right = CSLevelUpBaoZhuMessage;
                UtilityTips.ShowPromptWordTips(59, null, right, tittle);
            }
            else
            {
                if (CSBagInfo.Instance.IsEquip(bagItemInfos[selectIndex]))
                {
                    Net.CSLevelUpBaoZhuMessage(-12, repeatedEquips);
                }
                else
                {
                    Net.CSLevelUpBaoZhuMessage(bagItemInfos[selectIndex].bagIndex, repeatedEquips);
                }
            }
        }
    }

    void CSLevelUpBaoZhuMessage()
    {
        RepeatedField<int> repeatedEquips = new RepeatedField<int>();
        for (int i = 0; i < selectEquips.Count; i++)
        {
            repeatedEquips.Add(selectEquips[i]);
        }

        if (CSBagInfo.Instance.IsEquip(bagItemInfos[selectIndex]))
        {
            Net.CSLevelUpBaoZhuMessage(-12, repeatedEquips);
        }
        else
        {
            Net.CSLevelUpBaoZhuMessage(bagItemInfos[selectIndex].bagIndex, repeatedEquips);
        }
    }

    public override void Show()
    {
        base.Show();
        CSEffectPlayMgr.Instance.ShowUITexture(mheadbg, "pearl_bg1");
        selectIndex = 0;
        lastSelectIndex = 0;
        InitData();
    }

    void InitData()
    {
        isSelectComplete = false;
        myPearlData = CSPearlInfo.Instance.MyPearlData;
        if (myPearlData == null) return;
        SortPearl();
        SortDevourEquips();
        if (bagItemInfos.Count <= 0)
        {
            mobj_nonEmpty.SetActive(false);
            memptyHint.SetActive(true);
        }
        else
        {
            mobj_nonEmpty.SetActive(true);
            memptyHint.SetActive(false);
            InitGrid();
            SetSelectItemInfo(selectIndex);
            SetRightInfo(selectIndex);
        }
    }

    /// <summary>
    /// 排序可吞噬装备
    /// </summary>
    void SortDevourEquips()
    {
        List<BagItemInfo> listDevourEquips = mPoolHandleManager.GetSystemClass<List<BagItemInfo>>();
        CSBagInfo.Instance.GetdevourItems(listDevourEquips);
        CSBetterLisHot<bag.BagItemInfo> betterLisHot = new CSBetterLisHot<BagItemInfo>();
        betterLisHot.Clear();
        for (int i = 0; i < listDevourEquips.Count; i++)
        {
            betterLisHot.Add(listDevourEquips[i]);
        }

        betterLisHot.Sort((a, b) =>
        {
            int lva = ItemTableManager.Instance.GetItemLevel(a.configId);
            int lvb = ItemTableManager.Instance.GetItemLevel(b.configId);
            return lva <= lvb ? -1 : 1;
        });

        devourEquips = new List<BagItemInfo>();
        devourEquips.Clear();

        for (int i = 0; i < betterLisHot.Count; i++)
        {
            devourEquips.Add(betterLisHot[i]);
        }
    }

    /// <summary>
    /// 排序宝珠
    /// </summary>
    void SortPearl()
    {
        bagItemInfos = new List<BagItemInfo>();
        bagItemInfos.Clear();
        for (int i = 0; i < myPearlData.ListPearl.Count; i++)
        {
            bagItemInfos.Add(myPearlData.ListPearl[i]);
        }

        //排序
        bagItemInfos.Sort((a, b) =>
        {
            if (myPearlData.EquipPearl == null ||
                (myPearlData.EquipPearl != null &&
                 a.id != myPearlData.EquipPearl.id && b.id != myPearlData.EquipPearl.id))
            {
                if (a.quality == b.quality)
                {
                    if (a.gemLevel == b.gemLevel)
                    {
                        return -1;
                    }
                    else //等级高的优先
                    {
                        return a.gemLevel > b.gemLevel ? -1 : 1;
                    }
                }
                else //品质高的优先
                {
                    return a.quality > b.quality ? -1 : 1;
                }
            }
            else //已穿戴优先
            {
                return a.id == myPearlData.EquipPearl.id ? -1 : 1;
            }
        });

        if (bagItemInfos.Count > 0)
        {
            curSelectPear = bagItemInfos[0];
        }
    }

    /// <summary>
    /// 初始化宝珠列表
    /// </summary>
    void InitGrid()
    {
        if (itemListBaoZhu == null)
        {
            itemListBaoZhu = new List<UIItemBase>();
        }

        // itemListBaoZhu.Clear();
        mgrid_preal.MaxCount = bagItemInfos.Count;
        GameObject gp;
        ScriptBinder gpBinder;
        GameObject Item;
        UILabel lb_name;
        UILabel lb_lv;
        UISprite sp_wear;
        UILabel lb_count;
        GameObject effect;
        for (int i = 0; i < mgrid_preal.MaxCount; i++)
        {
            gp = mgrid_preal.controlList[i];
            gpBinder = gp.transform.GetComponent<ScriptBinder>();
            Item = gpBinder.GetObject("Item") as GameObject;
            lb_name = gpBinder.GetObject("lb_name") as UILabel;
            lb_lv = gpBinder.GetObject("lb_lv") as UILabel;
            sp_wear = gpBinder.GetObject("sp_wear") as UISprite;
            effect = gpBinder.GetObject("effect") as GameObject;

            if (itemListBaoZhu.Count < i + 1)
            {
                UIItemBase itemBase =
                    UIItemManager.Instance.GetItem(PropItemType.Normal, Item.transform, itemSize.Size66);
                itemListBaoZhu.Add(itemBase);
            }

            itemListBaoZhu[i].Refresh(bagItemInfos[i].configId, null, false);
            lb_name.text =
                $"{ItemTableManager.Instance.GetItemName(bagItemInfos[i].configId)}   Lv.{bagItemInfos[i].gemLevel}";
            lb_name.color =
                UtilityCsColor.Instance.GetColor(ItemTableManager.Instance.GetItemQuality(bagItemInfos[i].configId));
            lb_lv.gameObject.SetActive(false);
            sp_wear.gameObject.SetActive(CSBagInfo.Instance.IsEquip(bagItemInfos[i]));
            lb_count = itemListBaoZhu[i].obj.transform.Find("lb_count").gameObject.GetComponent<UILabel>();
            lb_count.text = BaoZhuJinHuaTableManager.Instance.GetBaoZhuJinHuaGradename(bagItemInfos[i].gemGrade);
            effect.SetActive(i == selectIndex);
            UIEventListener.Get(gp, i).onClick = OnClickItem;
        }
    }

    /// <summary>
    /// 点击单个Item响应
    /// </summary>
    /// <param name="go"></param>
    void OnClickItem(GameObject go)
    {
        if (go == null) return;
        int i = (int) UIEventListener.Get(go).parameter;
        if (i == selectIndex) return;
        upLevelCount = -1;
        lastSelectIndex = selectIndex;
        selectIndex = i;
        SetSelectItemInfo(i);
        SetSelectItemInfo(lastSelectIndex);
        SetRightInfo(i);
        curSelectPear = bagItemInfos[i];
    }

    /// <summary>
    /// 设置单个Item点击后的变化
    /// </summary>
    /// <param name="index"></param>
    void SetSelectItemInfo(int index)
    {
        GameObject gp;
        GameObject effect;
        gp = mgrid_preal.controlList[index];
        effect = gp.transform.Find("effect").gameObject;
        effect.SetActive(index == selectIndex);
    }

    /// <summary>
    /// 设置右边的信息
    /// </summary>
    /// <param name="index"></param>
    void SetRightInfo(int index)
    {
        UIItemBase itemBase = new UIItemBase(mItemBase, PropItemType.Normal);
        itemBase.Refresh(bagItemInfos[index].configId, null, false);
        mlb_itemName.text = ItemTableManager.Instance.GetItemName(bagItemInfos[index].configId);
        mlb_itemName.color =
            UtilityCsColor.Instance.GetColor(ItemTableManager.Instance.GetItemQuality(bagItemInfos[index].configId));
        mlb_count.text =
            BaoZhuJinHuaTableManager.Instance.GetBaoZhuJinHuaGradename(bagItemInfos[index].gemGrade); //TODO:需要图标文字
        int baozhuConfigId = bagItemInfos[index].quality * 1000 + bagItemInfos[index].gemLevel;
        if (!BaoZhuTableManager.Instance.ContainsKey(baozhuConfigId))
        {
            // Debug.Log("-----------------------------配置表不存在该等级宝珠@高飞");
            return;
        }

        int maxExp = BaoZhuTableManager.Instance.GetBaoZhuExp(baozhuConfigId);

        //当前等级属性通用
        mlb_curLevel.text = CSString.Format(949, bagItemInfos[index].gemLevel);
        TABLE.ITEM itemCfg;
        PearAttrData pearAttrData = null;
        PearAttrData pearAttrData1 = null;
        if (ItemTableManager.Instance.TryGetValue(bagItemInfos[index].configId, out itemCfg))
        {
            pearAttrData = CSPearlInfo.Instance.GetPearAttrData(itemCfg,
                CSPearlInfo.Instance.GetUpGradeProportion(bagItemInfos[index].quality, bagItemInfos[index].gemLevel));
            if (maxExp != 0) //非满级则有下一级加成
            {
                pearAttrData1 = CSPearlInfo.Instance.GetPearAttrData(itemCfg,
                    CSPearlInfo.Instance.GetUpGradeProportion(bagItemInfos[index].quality,
                        bagItemInfos[index].gemLevel + 1));
            }
        }

        if (pearAttrData != null)
        {
            mgrid_effects.MaxCount = pearAttrData.PearAttrItems.Count;
            GameObject gp;
            ScriptBinder gpBinder;
            UILabel lb_name;
            UILabel lb_nextName;

            for (int i = 0; i < mgrid_effects.MaxCount; i++)
            {
                PearAttrItem pearAttrItem = pearAttrData.PearAttrItems[i];
                gp = mgrid_effects.controlList[i];
                gpBinder = gp.transform.GetComponent<ScriptBinder>();
                lb_name = gpBinder.GetObject("lb_name") as UILabel;
                lb_nextName = gpBinder.GetObject("lb_nextName") as UILabel;

                lb_name.text =
                    $"{main}{pearAttrItem.AttrName}{risk}{pearAttrItem.StrValue}[-]";
                if (maxExp != 0 && pearAttrData1 != null) //未满级
                {
                    PearAttrItem pearAttrItem1 = pearAttrData1.PearAttrItems[i];
                    lb_nextName.text =
                        $"{main}{pearAttrItem1.AttrName}{risk}[-]{green}{pearAttrItem1.StrValue}[-]";
                }
                else
                {
                    lb_nextName.text =
                        $"{main}{pearAttrItem.AttrName}{risk}[-]{CSString.Format(954)}";
                }
            }
        }

        if (maxExp == 0) //满级
        {
            mfullHint.SetActive(true);
            mnonfullHint.SetActive(false);
            mlb_hintMax.gameObject.SetActive(false);
            mgrid_dot.gameObject.SetActive(false);
            mScrollView_ConsumableEquip.gameObject.SetActive(false);
            mlb_hintNoDevours.gameObject.SetActive(false);
            mlb_nextLevel.text = CSString.Format(971);
            mlb_exp.text = CSString.Format(1260, 0, 200);
            mslider_exp.value = 0;
            mslider_exp_add.value = 0;
        }
        else //非满级(分为达到当前等级上限和未达到当前等级上限)
        {
            int levelLimit = BaoZhuJinHuaTableManager.Instance.GetBaoZhuJinHuaMaxLevel(bagItemInfos[index].gemGrade);
            mlb_nextLevel.text = CSString.Format(950, bagItemInfos[index].gemLevel + 1);
            int curExp = bagItemInfos[index].gemExp;
            mlb_exp.text = CSString.Format(1260, curExp, maxExp);

            // 升级先到1再到指定value  非升级直接到value
            float value = (float) curExp / maxExp;
            if (upLevelCount < 0)
            {
                mslider_exp.value = value;
            }
            else
            {
                if (upLevelCount == 0)
                {
                    TweenProgressBar.Begin(mslider_exp, 0.5f * (value - curSliderValue), curSliderValue, value);
                }
                else
                {
                    CSGame.Sington.StartCoroutine(SetProgressBarValue(upLevelCount, curSliderValue, value));
                }
            }

            curSliderValue = value;
            mslider_exp_add.value = 0;

            if (bagItemInfos[index].gemLevel >= levelLimit) //达到等级限制
            {
                mfullHint.SetActive(false);
                mnonfullHint.SetActive(false);
                mlb_hintMax.gameObject.SetActive(true);
                mgrid_dot.gameObject.SetActive(false);
                mScrollView_ConsumableEquip.gameObject.SetActive(false);
                mlb_hintNoDevours.gameObject.SetActive(false);
            }
            else //未达到等级限制
            {
                // mlb_nextLevel.text = CSString.Format(950, bagItemInfos[index].gemLevel + 1);
                // int curExp = bagItemInfos[index].gemExp;
                // mlb_exp.text = CSString.Format(1260, curExp, maxExp);
                //
                // // 升级先到1再到指定value  非升级直接到value
                // float value = (float) curExp / maxExp;
                // if (upLevelCount < 0)
                // {
                //     mslider_exp.value = value;
                // }
                // else
                // {
                //     if (upLevelCount == 0)
                //     {
                //         TweenProgressBar.Begin(mslider_exp, 0.5f * (value - curSliderValue), curSliderValue, value);
                //     }
                //     else
                //     {
                //         CSGame.Sington.StartCoroutine(SetProgressBarValue(upLevelCount, curSliderValue, value));
                //     }
                // }
                //
                // curSliderValue = value;
                // mslider_exp_add.value = 0;

                if (devourEquips.Count <= 0) //若没有可吞噬装备
                {
                    mfullHint.SetActive(false);
                    mnonfullHint.SetActive(false);
                    mlb_hintMax.gameObject.SetActive(false);
                    mgrid_dot.gameObject.SetActive(false);
                    mScrollView_ConsumableEquip.gameObject.SetActive(false);
                    mlb_hintNoDevours.gameObject.SetActive(true);
                }
                else
                {
                    mfullHint.SetActive(false);
                    mnonfullHint.SetActive(true);
                    mlb_hintMax.gameObject.SetActive(false);
                    mgrid_dot.gameObject.SetActive(true);
                    mScrollView_ConsumableEquip.gameObject.SetActive(true);
                    mlb_hintNoDevours.gameObject.SetActive(false);
                    //升级按钮默认置灰
                    mbtn_upgrade.GetComponent<UISprite>().color = Color.black;
                    mbtn_upgrade.GetComponent<BoxCollider>().enabled = false;

                    //重置可吞噬装备列表
                    isSelectComplete = false; //重置一键选择
                    selectEquips = new List<int>();
                    selectEquips.Clear();
                    mScrollView_ConsumableEquip.ResetPosition(); //重置ScrollView
                    InitGridWrap();
                }
            }
        }

        upLevelCount = -1;
    }

    IEnumerator SetProgressBarValue(int count, float start, float end)
    {
        for (int i = 0; i <= count; i++)
        {
            if (i == 0)
            {
                TweenProgressBar.Begin(mslider_exp, 0.5f * (1 - start), start, 1);
                yield return new WaitForSeconds(0.5f);
            }
            else if (i == count)
            {
                TweenProgressBar.Begin(mslider_exp, 0.5f * end, 0, end);
            }
            else
            {
                TweenProgressBar.Begin(mslider_exp, 0.5f, 0, 1);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    #region 循环滚动相关函数

    /// <summary>
    /// 初始化无限滚动组件
    /// </summary>
    void InitGridWrap()
    {
        RefreshMaxPag();
        curPage = 1;
        gridPage0 = 1;
        mwrap_page.enabled = false;
        if (maxPage < 2)
        {
            mwrap_page.minIndex = 0;
            mwrap_page.maxIndex = 1;
            RefreshOnePagesItems(0, gridPage0);
        }
        else
        {
            mwrap_page.minIndex = 0;
            mwrap_page.maxIndex = maxPage - 1;
            gridPage1 = gridPage0 + 1;
            RefreshOnePagesItems(0, gridPage0);
            RefreshOnePagesItems(1, gridPage1);
        }

        mwrap_page.enabled = true;
        RefreshPageDotUI();
    }

    /// <summary>
    /// 刷新最大页数
    /// </summary>
    void RefreshMaxPag()
    {
        if (devourEquips == null) return;
        int equipCount = devourEquips.Count;
        maxPage = equipCount == 0 ? 1 : Mathf.CeilToInt((float) equipCount / EquipMaxCount);
        mScrollView_ConsumableEquip.enabled = maxPage > 1;
    }

    /// <summary>
    /// 刷新页签点
    /// </summary>
    void RefreshPageDotUI()
    {
        if (maxPage < 2)
        {
            mgrid_dot.MaxCount = 0;
            return;
        }

        mgrid_dot.MaxCount = maxPage;
        for (int i = 0; i < mgrid_dot.MaxCount; i++)
        {
            var selected = mgrid_dot.controlList[i].transform.GetChild(1);
            selected.gameObject.SetActive(i + 1 == curPage);
        }
    }

    /// <summary>
    /// 无限滚动UI组件回调
    /// </summary>
    /// <param name="go"></param>
    /// <param name="wrapIndex">待显示Grid索引（0、1）</param>
    /// <param name="realIndex">真实页数索引（起始为0,范围为所有整数）</param>
    void WrapUpdate(GameObject go, int wrapIndex, int realIndex)
    {
        int page = lastRealIndex > realIndex ? curPage - 1 : curPage + 1;
        // page = page <= 0 ? MaxShopPage : page > MaxShopPage ? 1 : page;//循环用
        page = page <= 0 ? 1 : page > maxPage ? maxPage : page; //非循环用
        lastRealIndex = realIndex;
        if (wrapIndex == 0) gridPage0 = page;
        else gridPage1 = page;
        RefreshOnePagesItems(wrapIndex, page);
    }

    /// <summary>
    /// 当前中间的Grid发生变化完成回调
    /// </summary>
    /// <param name="go">中间Grid的GameObject</param>
    void OnCenter(GameObject go)
    {
        if (go.name == "Grid0")
        {
            //curCenterGrid = 0;
            curPage = gridPage0;
            curItemBaseList = itemBaseList0;
            curGrid = mGrid0;
        }
        else
        {
            //curCenterGrid = 1;
            curPage = gridPage1;
            curItemBaseList = itemBaseList1;
            curGrid = mGrid1;
        }

        RefreshPageDotUI();
    }

    /// <summary>
    /// 刷新某一页grid的物品
    /// </summary>
    /// <param name="wrapIndex">0对应grid1,1对应grid2</param>
    /// <param name="page">刷新对应新生成的页数，起始页为1</param>
    void RefreshOnePagesItems(int wrapIndex, int page)
    {
        int passCount = (page - 1) * EquipMaxCount; //当前index在此基础上往上加
        List<UIItemBase> itemBaseListCur = wrapIndex == 0 ? itemBaseList0 : itemBaseList1;
        UIGridContainer gridCur = wrapIndex == 0 ? mGrid0 : mGrid1;
        gridCur.MaxCount = EquipMaxCount;
        GameObject gp;
        GameObject node;
        GameObject select;
        for (int i = 0; i < gridCur.MaxCount; i++)
        {
            gp = gridCur.controlList[i];
            node = gp.transform.Find("node").gameObject;
            if (itemBaseListCur.Count < EquipMaxCount && itemBaseListCur.Count == i)
            {
                UIItemBase itemBase =
                    UIItemManager.Instance.GetItem(PropItemType.Normal, node.transform, itemSize.Size66);
                itemBaseListCur.Add(itemBase);
            }

            if (i + passCount < devourEquips.Count && i + passCount >= 0) //如果该格子有装备
            {
                itemBaseListCur[i].Refresh(devourEquips[i + passCount].configId, null, false);
                select = itemBaseListCur[i].obj.transform.Find("select").gameObject;
                select.SetActive(selectEquips.Contains(devourEquips[i + passCount].bagIndex));
                itemBaseListCur[i].obj.transform.Find("sp_itemicon").gameObject.SetActive(true);
                itemBaseListCur[i].obj.transform.Find("sp_quality").gameObject.SetActive(true);

                UIEventListener.Get(node).ClickIntervalTime = 0f;
                UIEventListener.Get(node, i + passCount).onClick = OnClickEquipItem;
            }
            else
            {
                select = itemBaseListCur[i].obj.transform.Find("select").gameObject;
                select.SetActive(false);
                itemBaseListCur[i].obj.transform.Find("sp_itemicon").gameObject.SetActive(false);
                itemBaseListCur[i].obj.transform.Find("sp_quality").gameObject.SetActive(false);
                UIEventListener.Get(node, i + passCount).onClick = null;
            }
        }
    }

    #endregion


    void OnClickEquipItem(GameObject go)
    {
        if (go == null) return;
        //index为真实索引
        int index = (int) UIEventListener.Get(go).parameter;
        //处理按钮置灰点击弹tips(被阶数限制)
        if (BaoZhuJinHuaTableManager.Instance.GetBaoZhuJinHuaMaxLevel(bagItemInfos[selectIndex].gemGrade) ==
            bagItemInfos[selectIndex].gemLevel)
        {
            //等级达到上限，需要进化才能继续升级
            UtilityTips.ShowTips(925, 1.5f, ColorType.Red);
            return;
        }

        RefeshEquipItem(index);
    }

    void RefeshEquipItem(int index, TypeSelect type = TypeSelect.None)
    {
        if (selectEquips != null)
        {
            switch (type)
            {
                case TypeSelect.None:
                    if (selectEquips.Contains(devourEquips[index].bagIndex))
                    {
                        selectEquips.Remove(devourEquips[index].bagIndex);
                    }
                    else
                    {
                        selectEquips.Add(devourEquips[index].bagIndex);
                    }

                    break;
                case TypeSelect.Select:
                    selectEquips.Add(devourEquips[index].bagIndex);
                    break;
                case TypeSelect.NonSelect:
                    if (selectEquips.Contains(devourEquips[index].bagIndex))
                    {
                        selectEquips.Remove(devourEquips[index].bagIndex);
                    }

                    break;
            }
        }

        //打勾更新
        RefreshOnePagesItems(0, gridPage0);
        RefreshOnePagesItems(1, gridPage1);

        //经验条变化
        if (selectEquips.Count <= 0)
        {
            int baozhuConfigId = bagItemInfos[selectIndex].quality * 1000 + bagItemInfos[selectIndex].gemLevel;
            int curExp = bagItemInfos[selectIndex].gemExp;
            int maxExp = BaoZhuTableManager.Instance.GetBaoZhuExp(baozhuConfigId);
            mlb_exp.text = CSString.Format(1260, curExp, maxExp);
            mslider_exp.value = (float) curExp / maxExp;
            mslider_exp_add.value = 0;

            mlb_nextLevel.text = CSString.Format(950, bagItemInfos[selectIndex].gemLevel + 1);
            TABLE.ITEM itemCfg;
            PearAttrData pearAttrData1;
            if (ItemTableManager.Instance.TryGetValue(bagItemInfos[selectIndex].configId, out itemCfg))
            {
                pearAttrData1 = CSPearlInfo.Instance.GetPearAttrData(itemCfg,
                    CSPearlInfo.Instance.GetUpGradeProportion(bagItemInfos[selectIndex].quality,
                        bagItemInfos[selectIndex].gemLevel + 1));
                GameObject gp;
                UILabel lb_nextName;
                for (int i = 0; i < mgrid_effects.MaxCount; i++)
                {
                    PearAttrItem pearAttrItem1 = pearAttrData1.PearAttrItems[i];
                    gp = mgrid_effects.controlList[i];
                    lb_nextName = gp.transform.Find("lb_nextName").gameObject.GetComponent<UILabel>();
                    lb_nextName.text =
                        $"{main}{pearAttrItem1.AttrName}{risk}[-]{green}{pearAttrItem1.StrValue}[-]";
                }
            }

            //升级按钮置灰
            mbtn_upgrade.GetComponent<UISprite>().color = Color.black;
            mbtn_upgrade.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            int addExp = 0;
            for (int i = 0; i < selectEquips.Count; i++)
            {
                for (int j = 0; j < devourEquips.Count; j++)
                {
                    if (devourEquips[j].bagIndex == selectEquips[i])
                    {
                        // addExp += ItemTableManager.Instance.GetItemDevouredExp(devourEquips[j].configId);
                        break;
                    }
                }
            }

            int baozhuConfigId = bagItemInfos[selectIndex].quality * 1000 + bagItemInfos[selectIndex].gemLevel;
            int curExp = bagItemInfos[selectIndex].gemExp;
            int maxExp = BaoZhuTableManager.Instance.GetBaoZhuExp(baozhuConfigId);

            mlb_exp.text = CSString.Format(1153, curExp, addExp, maxExp);
            if (curExp + addExp >= maxExp)
            {
                mslider_exp_add.value = 1f;
                /*达到升级显示预估等级*/
                //当前等级上限
                int levelLimit =
                    BaoZhuJinHuaTableManager.Instance.GetBaoZhuJinHuaMaxLevel(bagItemInfos[selectIndex].gemGrade);
                //宝珠表索引
                int baozhuId = bagItemInfos[selectIndex].quality * 1000 + bagItemInfos[selectIndex].gemLevel;
                //当前等级升级所需经验
                int expUpdate = BaoZhuTableManager.Instance.GetBaoZhuExp(baozhuId) - bagItemInfos[selectIndex].gemExp;
                //当前升到等级上限所需经验
                int expUpdateLevelLimit = expUpdate;
                //当前能够升级的等级
                int uplevel = 0;
                var arr = BaoZhuTableManager.Instance.array.gItem.handles;
                TABLE.BAOZHU baozhuItem = null;
                for (int i = 0, max = arr.Length; i < max; ++i)
                {
                    baozhuItem = arr[i].Value as TABLE.BAOZHU;
                    if (baozhuItem.quality == bagItemInfos[selectIndex].quality &&
                        baozhuItem.grade > bagItemInfos[selectIndex].gemLevel &&
                        baozhuItem.grade <= levelLimit &&
                        curExp + addExp >= curExp + expUpdateLevelLimit)
                    {
                        if (uplevel < baozhuItem.grade)
                        {
                            uplevel = baozhuItem.grade;
                            expUpdateLevelLimit += baozhuItem.exp;
                        }
                    }
                }

                mlb_nextLevel.text = CSString.Format(950, uplevel);
                TABLE.ITEM itemCfg;
                PearAttrData pearAttrData1;
                if (ItemTableManager.Instance.TryGetValue(bagItemInfos[selectIndex].configId, out itemCfg))
                {
                    pearAttrData1 = CSPearlInfo.Instance.GetPearAttrData(itemCfg,
                        CSPearlInfo.Instance.GetUpGradeProportion(bagItemInfos[selectIndex].quality, uplevel));
                    GameObject gp;
                    UILabel lb_nextName;
                    for (int i = 0; i < mgrid_effects.MaxCount; i++)
                    {
                        PearAttrItem pearAttrItem1 = pearAttrData1.PearAttrItems[i];
                        gp = mgrid_effects.controlList[i];
                        lb_nextName = gp.transform.Find("lb_nextName").gameObject.GetComponent<UILabel>();
                        lb_nextName.text =
                            $"{main}{pearAttrItem1.AttrName}{risk}[-]{green}{pearAttrItem1.StrValue}[-]";
                    }
                }
            }
            else
            {
                mslider_exp_add.value = (float) (curExp + addExp) / maxExp;
                /*未达到升级显示下一级*/
                mlb_nextLevel.text = CSString.Format(950, bagItemInfos[selectIndex].gemLevel + 1);
                TABLE.ITEM itemCfg;
                PearAttrData pearAttrData1;
                if (ItemTableManager.Instance.TryGetValue(bagItemInfos[selectIndex].configId, out itemCfg))
                {
                    pearAttrData1 = CSPearlInfo.Instance.GetPearAttrData(itemCfg,
                        CSPearlInfo.Instance.GetUpGradeProportion(bagItemInfos[selectIndex].quality,
                            bagItemInfos[selectIndex].gemLevel + 1));
                    GameObject gp;
                    UILabel lb_nextName;
                    for (int i = 0; i < mgrid_effects.MaxCount; i++)
                    {
                        PearAttrItem pearAttrItem1 = pearAttrData1.PearAttrItems[i];
                        gp = mgrid_effects.controlList[i];
                        lb_nextName = gp.transform.Find("lb_nextName").gameObject.GetComponent<UILabel>();
                        lb_nextName.text =
                            $"{main}{pearAttrItem1.AttrName}{risk}[-]{green}{pearAttrItem1.StrValue}[-]";
                    }
                }
            }

            mbtn_upgrade.GetComponent<UISprite>().color = Color.white;
            mbtn_upgrade.GetComponent<BoxCollider>().enabled = true;
        }
    }

    public override void OnHide()
    {
        base.OnHide();
        upLevelCount = -1;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UIItemManager.Instance.RecycleItemsFormMediator(itemListBaoZhu);
        UIItemManager.Instance.RecycleItemsFormMediator(itemBaseList0);
        UIItemManager.Instance.RecycleItemsFormMediator(itemBaseList1);
        upLevelCount = -1;
    }
}