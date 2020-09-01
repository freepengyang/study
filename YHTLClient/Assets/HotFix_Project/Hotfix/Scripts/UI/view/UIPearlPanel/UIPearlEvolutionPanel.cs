using System.Collections;
using System.Collections.Generic;
using System.Linq;
using bag;
using baozhu;
using UnityEngine;

public partial class UIPearlEvolutionPanel : UIBasePanel
{
    public override bool ShowGaussianBlur { get => false; }

    private PearlData myPearlData;
    private List<bag.BagItemInfo> bagItemInfos;

    private int selectIndex = 0;
    private int lastSelectIndex = 0;
    
    List<UIItemBase> itemListBaoZhu;

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.GradeUpBaoZhu, RefreshData);
        mbtn_gotokill.onClick = OnClickGotoKill;
        mbtn_evolution.onClick = OnClickEvolution;
        mbtn_rule.onClick = OnClickRule;
        mlb_hint.onClick = OnClickRequire;
    }
    
    void OnClickRequire(GameObject go)
    {
        if (go == null) return;
        Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(573));
    }
    
    void RefreshData(uint id, object data)
    {
        myPearlData = CSPearlInfo.Instance.MyPearlData;
        if (myPearlData == null || BaoZhuJinHuaTableManager.Instance == null) return;
        selectIndex = 0;
        lastSelectIndex = 0;
        SortPearl();
        InitGrid();
        SetSelectItemInfo(selectIndex);
        SetRightInfo(selectIndex);
        UtilityTips.ShowTips(1109,1.5f, ColorType.Green);
    }
    
    /// <summary>
    //前往击杀
    /// </summary>
    /// <param name="go"></param>
    void OnClickGotoKill(GameObject go)
    {
        if (go == null) return;
        //打开Boss界面
        UIManager.Instance.CreatePanel<UIBossCombinePanel>(p => { (p as UIBossCombinePanel).SelectChildPanel(1); });
        UIManager.Instance.ClosePanel<UIPearlCombinedPanel>();
    }

    void OnClickEvolution(GameObject go)
    {
        if (go == null) return;
        //发送宝珠进化
        Net.CSGradeUpBaoZhuMessage(-12);
    }

    void OnClickRule(GameObject go)
    {
        if (go == null) return;
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.UpGradeBaoZhu);
    }

    public override void Show()
    {
        base.Show();
        CSEffectPlayMgr.Instance.ShowUITexture(mheadbg, "pearl_bg3");
        selectIndex = 0;
        lastSelectIndex = 0;
        InitData();
    }

    /// <summary>
    /// 打开宝珠进化界面
    /// </summary>
    /// <param name="id">宝珠唯一Id</param>
    public void ShowPearEvolution(long id)
    {
        myPearlData = CSPearlInfo.Instance.MyPearlData;
        if (myPearlData == null || BaoZhuJinHuaTableManager.Instance == null) return;
        SortPearl();
        if (bagItemInfos.Count <= 0)
        {
            memptyHint.SetActive(true);
            mobj_nonEmpty.SetActive(false);
        }
        else
        {
            for (int i = 0; i < bagItemInfos.Count; i++)
            {
                if (bagItemInfos[i].id == id)
                {
                    selectIndex = i;
                    lastSelectIndex = 0;
                    break;
                }
            }
            memptyHint.SetActive(false);
            mobj_nonEmpty.SetActive(true);
            InitGrid();
            SetSelectItemInfo(selectIndex);
            SetRightInfo(selectIndex);
        }
    }

    void InitData()
    {
        myPearlData = CSPearlInfo.Instance.MyPearlData;
        if (myPearlData == null || BaoZhuJinHuaTableManager.Instance == null) return;
        SortPearl();
        if (bagItemInfos.Count <= 0)
        {
            memptyHint.SetActive(true);
            mobj_nonEmpty.SetActive(false);
        }
        else
        {
            memptyHint.SetActive(false);
            mobj_nonEmpty.SetActive(true);
            InitGrid();
            SetSelectItemInfo(selectIndex);
            SetRightInfo(selectIndex);
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
    }

    /// <summary>
    /// 初始化宝珠列表
    /// </summary>
    void InitGrid()
    {
        if (itemListBaoZhu==null)
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
        GameObject redpoint;
        for (int i = 0; i < mgrid_preal.MaxCount; i++)
        {
            gp = mgrid_preal.controlList[i];
            gpBinder = gp.transform.GetComponent<ScriptBinder>();
            Item = gpBinder.GetObject("Item") as GameObject;
            lb_name = gpBinder.GetObject("lb_name") as UILabel;
            lb_lv = gpBinder.GetObject("lb_lv") as UILabel;
            sp_wear = gpBinder.GetObject("sp_wear") as UISprite;
            effect = gpBinder.GetObject("effect") as GameObject;
            redpoint = gpBinder.GetObject("redpoint") as GameObject;

            if (itemListBaoZhu.Count<i+1)
            {
                UIItemBase itemBase =  UIItemManager.Instance.GetItem(PropItemType.Normal, Item.transform, itemSize.Size66);
                itemListBaoZhu.Add(itemBase);
            }
      
            itemListBaoZhu[i].Refresh(bagItemInfos[i].configId,null,false);
            lb_name.text = $"{ItemTableManager.Instance.GetItemName(bagItemInfos[i].configId)}   Lv.{bagItemInfos[i].gemLevel}";
            lb_name.color = UtilityCsColor.Instance.GetColor(ItemTableManager.Instance.GetItemQuality(bagItemInfos[i].configId));
            lb_lv.gameObject.SetActive(false);
            sp_wear.gameObject.SetActive(CSBagInfo.Instance.IsEquip(bagItemInfos[i]));
            lb_count = itemListBaoZhu[i].obj.transform.Find("lb_count").gameObject.GetComponent<UILabel>();
            lb_count.text = BaoZhuJinHuaTableManager.Instance.GetBaoZhuJinHuaGradename(bagItemInfos[i].gemGrade);
            effect.SetActive(i == selectIndex);
            
            redpoint.SetActive(false);
            if (myPearlData.EquipPearl!=null)
            {
                redpoint.SetActive(bagItemInfos[i].id == myPearlData.EquipPearl.id
                                   && bagItemInfos[i].gemBossCounter >=
                                   BaoZhuJinHuaTableManager.Instance.GetBaoZhuJinHuaBossNum(bagItemInfos[i].gemGrade));
            }
            
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
        int index = (int) UIEventListener.Get(go).parameter;
        if (index == selectIndex) return;
        lastSelectIndex = selectIndex;
        selectIndex = index;
        SetSelectItemInfo(index);
        SetSelectItemInfo(lastSelectIndex);
        SetRightInfo(index);
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
        int curGrade = bagItemInfos[index].gemGrade;
        TABLE.BAOZHUJINHUA curBaozhujinhua = null;
        if (!BaoZhuJinHuaTableManager.Instance.TryGetValue(curGrade, out curBaozhujinhua))
        {
            // Debug.Log("----------------宝珠进化表中找不到该阶数的宝珠@高飞");
            return;
        }
        //公共部分ItemBase显示
        UIItemBase itemBase = new UIItemBase(mItemBase, PropItemType.Normal);
        itemBase.Refresh(bagItemInfos[index].configId);
        mlb_itemName.text = ItemTableManager.Instance.GetItemName(bagItemInfos[index].configId);
        mlb_itemName.color = UtilityCsColor.Instance.GetColor(ItemTableManager.Instance.GetItemQuality(bagItemInfos[index].configId));
        mlb_count.text = BaoZhuJinHuaTableManager.
            Instance.GetBaoZhuJinHuaGradename(bagItemInfos[index].gemGrade); //TODO:需要图标文字

        if (!CSPearlInfo.Instance.IsMaxGrade(curGrade)) //非满阶
        {
            mfullHint.SetActive(false);
            mnonfullHint.SetActive(true);
            mlb_title.text = CSString.Format(951, curGrade+1);
            //grid的总个数（至少有一条：等级上限）
            int gridCount = 1;
            //该技能槽是否已解锁
            bool isUnlock = true;
            //下一阶数宝珠可解锁的技能槽
            int skillSlotID =
                BaoZhuJinHuaTableManager.Instance.GetBaoZhuJinHuaSkillSlotID(bagItemInfos[index].gemGrade + 1);
            for (int i = 0; i < myPearlData.ListSkillSlotDatas.Count; i++)
            {
                if (myPearlData.ListSkillSlotDatas[i].SkillSlotId == skillSlotID)
                {
                    isUnlock = myPearlData.ListSkillSlotDatas[i].IsUnlock;
                    break;
                }
            }
            
            int nextAddRandomNum = BaoZhuJinHuaTableManager.Instance.GetBaoZhuJinHuaAddRandomAttr(bagItemInfos[index].gemGrade + 1);
            //下一阶随机条数不为0增加一条
            if (nextAddRandomNum>0)
            {
                gridCount++;
            }
            //下一阶未解锁最后加一条
            if (!isUnlock)
            {
                gridCount++;
            }

            // //随机属性
            // TABLE.ITEM cfg = null;
            // if (!ItemTableManager.Instance.TryGetValue(bagItemInfos[index].configId, out cfg)) return;
            // EquipRefineProDic ranattrList =
            //     StructTipData.Instance.GetEquipRandomDisjunctData(cfg, bagItemInfos[index].randAttrValues);
            // List<EquipRefineProperty> listEquipRefineProperty = ranattrList.ts;

            mgrid_effects.MaxCount = gridCount;
            GameObject gp;
            UILabel lb_name;
            for (int i = 0; i < mgrid_effects.MaxCount; i++)
            {
                gp = mgrid_effects.controlList[i];
                lb_name = gp.transform.Find("lb_name").gameObject.GetComponent<UILabel>();

                if (nextAddRandomNum>0&&i==0)
                {
                    lb_name.text = CSString.Format(952, CSString.Format(984, nextAddRandomNum));
                }
                else if(!isUnlock&&i==mgrid_effects.MaxCount-1)
                {
                    lb_name.text = CSString.Format(953, skillSlotID);   
                }
                else
                {
                    lb_name.text = CSString.Format(985, 
                        BaoZhuJinHuaTableManager.Instance.GetBaoZhuJinHuaMaxLevel(bagItemInfos[index].gemGrade + 1));
                }
            }

            mlb_condition.text =
                CSString.Format(CSString.Format(946), curBaozhujinhua.bossNum, curBaozhujinhua.bossLevel);
            int curNum = bagItemInfos[index].gemBossCounter;
            int maxNum = curBaozhujinhua.bossNum;
            if (curNum>maxNum)
            {
                curNum = maxNum;
            }
            mlb_exp.text = $"{curNum}/{maxNum}";
            mslider_exp.value = (float) curNum / maxNum;
            mbtn_gotokill.gameObject.SetActive(curNum < maxNum);
            mbtn_evolution.gameObject.SetActive(curNum >= maxNum);
        }
        else //满阶
        {
            mlb_title.text = CSString.Format(980);
            mgrid_effects.MaxCount = 2;
            GameObject gp;
            UILabel lb_name;
            for (int i = 0; i < mgrid_effects.MaxCount; i++)
            {
                gp = mgrid_effects.controlList[i];
                lb_name = gp.transform.Find("lb_name").gameObject.GetComponent<UILabel>();
                lb_name.text = CSString.Format(981);
            }

            mfullHint.SetActive(true);
            mnonfullHint.SetActive(false);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UIItemManager.Instance.RecycleItemsFormMediator(itemListBaoZhu);
    }
}