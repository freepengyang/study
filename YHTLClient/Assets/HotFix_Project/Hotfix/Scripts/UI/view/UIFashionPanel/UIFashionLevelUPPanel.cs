using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIFashionLevelUPPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get => false;
    }

    private FashionItemData myFashionItemData;
    private int myCareer = CSMainPlayerInfo.Instance.Career;

    UISpriteAnimation starEffect;
    private bool needStarEffect = true;

    List<UISpriteAnimation> listAttrEffect;
    List<bool> listNeedAttrEffect;

    private UIItemBase itemBase;

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.FashionStarLevelUp, RefreshData);
        // mClientEvent.Reg((uint) CEvent.ItemChange, RefreshDataItemChange);
        // mClientEvent.Reg((uint) CEvent.MoneyChange, RefreshDataItemChange);
        mbtn_close.onClick = OnClickClose;
        // mbtn_add.onClick = OnClickAdd;
        mbtn_upgrade.onClick = OnClickUpGrade;
        mItemBarDatas = mPoolHandleManager.CreateGeneratePool<ItemBarData>(8);
    }

    void RefreshData(uint id, object data)
    {
        if (data == null) return;
        fashion.FashionInfo msg = (fashion.FashionInfo) data;
        if (myFashionItemData.FashionId == msg.fashionId)
        {
            int onlyId = msg.fashionId + 100 * msg.star;
            if (CSFashionInfo.Instance.DicFashionWarehouses.TryGetValue(onlyId, out myFashionItemData))
            {
                SetStarUpData(myFashionItemData);
                //升级特效
                //星星
                int index = myFashionItemData.Star <= 7 ? myFashionItemData.Star - 1 : myFashionItemData.Star - 8;
                starEffect = mgrid_starts.controlList[index].transform.Find("sp_effect").GetComponent<UISpriteAnimation>();
                if (starEffect != null && needStarEffect)
                {
                    needStarEffect = false;
                    starEffect.gameObject.SetActive(true);
                    CSEffectPlayMgr.Instance.ShowUIEffect(starEffect.gameObject, "effect_star_add", 10, false);
                    starEffect.OnFinish = OnStarPlayFinish;
                }

                //属性
                if (listAttrEffect != null && listNeedAttrEffect != null)
                {
                    for (int i = 0; i < listAttrEffect.Count; i++)
                    {
                        if (listNeedAttrEffect[i])
                        {
                            listNeedAttrEffect[i] = false;
                            listAttrEffect[i].gameObject.SetActive(true);
                            CSEffectPlayMgr.Instance.ShowUIEffect(listAttrEffect[i].gameObject, "effect_dragon_levelup_add",
                                10, false, false);
                            if (i == listAttrEffect.Count - 1)
                            {
                                listAttrEffect[i].OnFinish = OnAttrPlayFinish;
                            }
                        }
                    }
                }
            }
        }
    }

    protected void OnStarPlayFinish()
    {
        starEffect.gameObject.SetActive(false);
        needStarEffect = true;
    }

    protected void OnAttrPlayFinish()
    {
        for (int i = 0; i < listAttrEffect.Count; i++)
        {
            listAttrEffect[i].gameObject.SetActive(false);
            listNeedAttrEffect[i] = true;
        }
    }


    // void OnClickAdd(GameObject go)
    // {
    //     Utility.ShowGetWay(UtilityMainMath.SplitStringToIntList(
    //         FashionTableManager.Instance.GetFashionGetWay(myFashionItemData.id)
    //     ));
    // }

    void OnClickUpGrade(GameObject go)
    {
        if (myFashionItemData == null) return;
        FashionStarUpData fashionStarUpData = GetFashionStarUpData(myFashionItemData.Id);
        if (myFashionItemData.Star == 14) //满星级
        {
            UtilityTips.ShowTips(731, 1.5f, ColorType.Red);
            return;
        }

        if (fashionStarUpData.isStarUp)
        {
            Net.CSFashionStarLevelUpMessage(myFashionItemData.FashionId);
        }
        else
        {
            UtilityTips.ShowTips(681, 1.5f, ColorType.Red);
        }
    }

    void OnClickClose(GameObject go)
    {
        Close();
    }


    public override void Show()
    {
        base.Show();
        CSEffectPlayMgr.Instance.ShowUITexture(mbg2, "bag_bg");
    }


    //属性
    List<List<int>> tempList = new List<List<int>>();
    List<List<int>> templistNext = new List<List<int>>();

    public void SetStarUpData(FashionItemData fashionItemData)
    {
        if (fashionItemData == null) return;
        myFashionItemData = fashionItemData;
        //道具icon
        if (itemBase == null)
            itemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, mItemBase.transform);
        itemBase.ShowIconFashion(fashionItemData.Id);
        //道具名字
        TABLE.FASHION fashion;
        if (!FashionTableManager.Instance.TryGetValue(fashionItemData.Id, out fashion))
        {
            return;
        }
        mlb_equipname.text = fashion.name;
        mlb_equipname.color = UtilityCsColor.Instance.GetColor(FashionTableManager.Instance.GetFashionQuality(fashion.id));

        mgrid_starts.MaxCount = 7;
        GameObject gp;
        GameObject star;
        int starNum = 0;
        if (fashionItemData.Star == 14) //满星级
        {
            mScrollView_1.SetActive(false);
            mScrollView_2.SetActive(true);
            mgrid_UIItemBar.gameObject.SetActive(false);
            mbtn_upgrade.gameObject.SetActive(true);
            for (int i = 0; i < mgrid_starts.MaxCount; i++)
            {
                gp = mgrid_starts.controlList[i];
                star = gp.transform.Find("sp_front").gameObject;
                star.SetActive(true);
            }

            tempList.Clear();
            switch (myCareer)
            {
                case 1: //战
                    tempList = UtilityMainMath.SplitStringToIntLists(fashion.phy);
                    break;
                case 2: //法
                    tempList = UtilityMainMath.SplitStringToIntLists(fashion.magic);
                    break;
                case 3: //道
                    tempList = UtilityMainMath.SplitStringToIntLists(fashion.tao);
                    break;
            }

            mgrid_attributes2.MaxCount = tempList.Count;
            GameObject gp2;
            UILabel lb_content;
            for (int i = 0; i < mgrid_attributes2.MaxCount; i++)
            {
                gp2 = mgrid_attributes2.controlList[i];
                lb_content = gp2.transform.Find("lb_name").gameObject.GetComponent<UILabel>();
                lb_content.text =
                    $"{CSString.Format(ClientAttributeTableManager.Instance.GetClientAttributeTipID(tempList[i][0]))}" +
                    $"{CSString.Format(999)}{tempList[i][1]}";
            }
        }
        else //未满星级
        {
            mScrollView_1.SetActive(true);
            mScrollView_2.SetActive(false);
            mgrid_UIItemBar.gameObject.SetActive(true);
            mbtn_upgrade.gameObject.SetActive(true);
            starNum = fashionItemData.Star <= 7? fashionItemData.Star:fashionItemData.Star - 7;
            
            for (int i = 0; i < mgrid_starts.MaxCount; i++)
            {
                gp = mgrid_starts.controlList[i];
                star = gp.transform.Find("sp_front").gameObject;
                star.SetActive(i < starNum);
            }

            tempList.Clear();
            templistNext.Clear();
            switch (myCareer)
            {
                case 1: //战
                    tempList = UtilityMainMath.SplitStringToIntLists(fashion.phy);
                    templistNext = UtilityMainMath.SplitStringToIntLists((FashionTableManager.Instance.array.gItem.id2offset[fashion.id+100].Value as TABLE.FASHION).phy);
                    break;
                case 2: //法
                    tempList = UtilityMainMath.SplitStringToIntLists(fashion.magic);
                    templistNext = UtilityMainMath.SplitStringToIntLists((FashionTableManager.Instance.array.gItem.id2offset[fashion.id + 100].Value as TABLE.FASHION).magic);
                    break;
                case 3: //道
                    tempList = UtilityMainMath.SplitStringToIntLists(fashion.tao);
                    templistNext = UtilityMainMath.SplitStringToIntLists((FashionTableManager.Instance.array.gItem.id2offset[fashion.id + 100].Value as TABLE.FASHION).tao);
                    break;
            }

            mgrid_attributes1.MaxCount = tempList.Count;
            listAttrEffect = new List<UISpriteAnimation>();
            listAttrEffect.Clear();
            listNeedAttrEffect = new List<bool>();
            listNeedAttrEffect.Clear();
            GameObject gp1;
            UILabel lb_content;
            UILabel lb_contentNext;
            UISpriteAnimation spriteAnimation;
            for (int i = 0; i < mgrid_attributes1.MaxCount; i++)
            {
                gp1 = mgrid_attributes1.controlList[i];
                lb_content = gp1.transform.Find("lb_name").gameObject.GetComponent<UILabel>();
                lb_contentNext = gp1.transform.Find("lb_nextName").gameObject.GetComponent<UILabel>();
                spriteAnimation = gp1.transform.Find("lb_upgrade_effect").gameObject.GetComponent<UISpriteAnimation>();
                listAttrEffect.Add(spriteAnimation);
                listNeedAttrEffect.Add(true);
                lb_content.text =
                    $"{CSString.Format(ClientAttributeTableManager.Instance.GetClientAttributeTipID(tempList[i][0]))}" +
                    $"{CSString.Format(999)}{tempList[i][1]}";
                lb_contentNext.text = 
                    $"{CSString.Format(ClientAttributeTableManager.Instance.GetClientAttributeTipID(templistNext[i][0]))}" +
                    $"{CSString.Format(999)}[00ff0c]{templistNext[i][1]}[-]";
            }

            FashionStarUpData fashionStarUpData = GetFashionStarUpData(fashionItemData.Id);
            // msp_icon.spriteName = $"tubiao{ItemTableManager.Instance.GetItemIcon(fashionStarUpData.starUpEquipId)}";
            //
            // int curCount = fashionStarUpData.ownedStarUpNum;
            // int maxCount = fashionStarUpData.starUpEquipNum;
            // string color = curCount >= maxCount
            //     ? UtilityColor.GetColorString(ColorType.Green)
            //     : UtilityColor.GetColorString(ColorType.Red);
            // mlb_value.text = $"{color}{curCount}/{maxCount}";
            mItemBarDatas.Clear();
            ItemBarData itemData = mItemBarDatas.Append();
            itemData.cfgId = fashionStarUpData.starUpEquipId;
            itemData.needed = fashionStarUpData.starUpEquipNum;
            itemData.owned = fashionStarUpData.ownedStarUpNum;
            itemData.flag = (int)ItemBarData.ItemBarType.IBT_GENERAL_COMPARE_SMALL_REDGREEN;
            mgrid_UIItemBar.MaxCount = mItemBarDatas.Count;
            mgrid_UIItemBar.Bind<ItemBarData, UIItemBar>(mItemBarDatas, mPoolHandleManager);
        }
    }
    
    FastArrayElementFromPool<ItemBarData> mItemBarDatas;

    FashionStarUpData fashionStarUpData = new FashionStarUpData();
    FashionStarUpData GetFashionStarUpData(int id)
    {
        fashionStarUpData.id = id;
        List<int> listInfo = UtilityMainMath.SplitStringToIntList((FashionTableManager.Instance.array.gItem.id2offset[id].Value as TABLE.FASHION).cost);
        var handles = FashionTableManager.Instance.array.gItem.handles;
        for (int i = 0,max = handles.Length;i < max;++i)
        {
            if (handles[i].key == id && listInfo != null && listInfo.Count == 2)
            {
                fashionStarUpData.starUpEquipId = listInfo[0];
                fashionStarUpData.starUpEquipNum = listInfo[1];
                fashionStarUpData.ownedStarUpNum =
                    (int) CSItemCountManager.Instance.GetItemCount(fashionStarUpData.starUpEquipId);
                fashionStarUpData.isStarUp =
                    fashionStarUpData.ownedStarUpNum >= fashionStarUpData.starUpEquipNum;
            }
        }

        return fashionStarUpData;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (listAttrEffect != null)
        {
            for (int i = 0; i < listAttrEffect.Count; i++)
            {
                CSEffectPlayMgr.Instance.Recycle(listAttrEffect[i].gameObject);
                listAttrEffect[i].OnFinish = null;
                listAttrEffect[i] = null;
            }

            listAttrEffect = null;
        }
    }
}


/// <summary>
/// 升星信息
/// </summary>
public class FashionStarUpData
{
    /// <summary>
    /// 时装唯一Id
    /// </summary>
    public int id = 0;

    /// <summary>
    /// 升星需要的装备Id
    /// </summary>
    public int starUpEquipId = 0;

    /// <summary>
    /// 升星需要的装备总数量
    /// </summary>
    public int starUpEquipNum = 0;

    /// <summary>
    /// 已拥有的升星装备数量
    /// </summary>
    public int ownedStarUpNum = 0;

    /// <summary>
    /// 是否可升星
    /// </summary>
    public bool isStarUp = false;
}