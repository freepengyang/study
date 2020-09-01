using gem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public partial class UIGemLevelUpPanel : UIBasePanel
{
    GemInfo geminfo;
    /// <summary>
    /// 物品条数据
    /// </summary>
    FastArrayElementFromPool<ItemBarData> mItemBarDatas; 
    private int mFlag;
    private long costMoney = 0;
    List<UIItemBase> ItemBases = new List<UIItemBase>();
    private int differNum;
    
    public override void Init()
    {
        base.Init();
        AddCollider();
        UIEventListener.Get(mbtn_close).onClick = Close;
        UIEventListener.Get(mbtn_send).onClick = SendClick;
        //itemData数据
        mItemBarDatas = mPoolHandleManager.CreateGeneratePool<ItemBarData>(8);
        //mFlag = (int)ItemBarData.ItemBarType.IBT_SMALL_ICON | (int)ItemBarData.ItemBarType.IBT_ONLY_COST | (int)ItemBarData.ItemBarType.IBT_ADD;
        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_effect, "effect_arrow_levelup_add");
        //"effect_arrow_levelup_add"
    }


    private void SendClick(GameObject obj)
    {
        if (differNum > 0)
        {
            Utility.ShowGetWay(geminfo.gemId);
            return;
        }


        if (costMoney > 0)
        {
            if (!Utility.JudgeCharge<UIGemLevelUpPanel>((int)costMoney))
                return;
            UtilityTips.ShowPromptWordTips(72, () =>
                {
                    Net.CSUpgradePosGemMessage(geminfo.subType, geminfo.pos); 
                    Close();
                },
                costMoney);
            return;
        }

        Net.CSUpgradePosGemMessage(geminfo.subType, geminfo.pos);
        Close();
    }

    public void OpenPanel(GemInfo thisGem)
    {
        geminfo = thisGem;
        TABLE.GEM curgemTable;
        GemTableManager.Instance.TryGetValue(geminfo.gemId, out curgemTable);
        showGem(mobj_gem1.transform, curgemTable);
        CSBetterLisHot<string> titleList = mPoolHandleManager.GetSystemClass<CSBetterLisHot<string>>();
        titleList.Clear();
        var arr = GemTableManager.Instance.array.gItem.handles;
        TABLE.GEM nextGemtable = new TABLE.GEM();
        for(int i = 0,max = arr.Length;i < max;++i)
        {
            var item = arr[i].Value as TABLE.GEM;
            if (item.position == curgemTable.position && item.lv == curgemTable.lv + 1)
            {
                nextGemtable = item;
                break;
            }
        }

        showGem(mobj_gem2.transform, nextGemtable);

        int expendGemId = curgemTable.cost[0].key();
        long expendBagNum = CSBagInfo.Instance.GetItemCount(expendGemId);
        int expendnum = curgemTable.cost[0].value();
        mgrid_glist.MaxCount = expendnum;
        TABLE.GEM expend;

        if (GemTableManager.Instance.TryGetValue(expendGemId, out expend))
        {
            
            for (int i = 0; i < mgrid_glist.controlList.Count; i++)
            {
                showExpend(mgrid_glist.controlList[i].transform, expend, i < expendBagNum);
            }

            
            
            mItemBarDatas.Clear();
            
            //暂时注释 宝石补全功能
             differNum = expendnum - (int)expendBagNum;
            // long differNum = expendnum - expendBagNum;
            // string tempnumstr = CSString.Format(1042, expendBagNum, expendnum);
            // string numstr = expendBagNum >= expendnum
            //     ? tempnumstr.BBCode(ColorType.Green)
            //     : tempnumstr.BBCode(ColorType.Red);
            // mlb_bagnum.text = CSString.Format(1289, numstr);
            // CSStringBuilder.Clear();
            // string[] costTemp;
            // titleList.Clear();
            // if (differNum > 0)
            // {
            //     costTemp = ShopTableManager.Instance.GetPriceByItemID(curgemTable.id).Split('#');
            //     if (costTemp.Length >= 2)
            //     {
            //         costMoney = int.Parse(costTemp[1]) * differNum;
            //         //AddItemData(int.Parse(costTemp[0]), costMoney);
            //         ItemBarData itemData = mItemBarDatas.Append();
            //         int id = int.Parse(costTemp[0]);
            //         itemData.cfgId = id;
            //         itemData.needed = costMoney;
            //         itemData.owned = id.GetItemCount();
            //         mFlag = (int) ItemBarData.ItemBarType.IBT_COST_GENERAL_COMPARE_RED_GREEN_ICON; 
            //         itemData.flag = mFlag;
            //         titleList.Add(CSString.Format(1287));
            //     }
            // }

            if (curgemTable.itemId.Count != 0)
            {
                ItemBarData itemData = mItemBarDatas.Append();
                int id = curgemTable.itemId[0].key();
                itemData.cfgId = id;
                itemData.needed = curgemTable.itemId[0].value();
                itemData.owned = id.GetItemCount();
                 mFlag =  (int)ItemBarData.ItemBarType.IBT_GENERAL_COMPARE_SMALL_REDGREEN ;
                itemData.flag = mFlag;


                titleList.Add(CSString.Format(1288));
            }


            mgird_items.MaxCount = mItemBarDatas.Count;
            mgird_items.Bind<ItemBarData, UIItemBar>(mItemBarDatas, mPoolHandleManager);

            for (int i = 0; i < mgird_items.MaxCount; i++)
            {
                UILabel mlb_text = UtilityObj.Get<UILabel>(mgird_items.controlList[i].transform, "lb_text");
                mlb_text.text = titleList[i];
            }

            mPoolHandleManager.Recycle(titleList); 
        }
    }
    
    //显示宝石控件
    private void showGem(Transform trans, TABLE.GEM gemTable = null)
    {
        UILabel mlb_name = UtilityObj.Get<UILabel>(trans, "lb_name");
        //UISprite mspr_gemicon = UtilityObj.Get<UISprite>(trans, "gemicon");
        UILabel mlb_max = UtilityObj.Get<UILabel>(trans, "lb_max");
        //UISprite mquality = UtilityObj.Get<UISprite>(mspr_gemicon.transform, "quality");
        Transform item = UtilityObj.Get<Transform>(trans, "item");
        if (gemTable == null)
        {
            mlb_name.gameObject.SetActive(false);
            mlb_max.gameObject.SetActive(true);
            //mspr_gemicon.gameObject.SetActive(false);
        }
        else
        {

            ItemBases.Add(Utility.GetItemByInfo(gemTable.id,item,0,itemSize.Size60));  
            int quality = ItemTableManager.Instance.GetItemQuality(gemTable.id);
            mlb_name.text = UtilityColor.BBCode(gemTable.name, quality);
            //mquality.spriteName = $"quality{quality}";
            //mspr_gemicon.spriteName = ItemTableManager.Instance.GetItemIcon(gemTable.id);
        }
    }

    private void showExpend(Transform trans, TABLE.GEM gemTable, bool isGrade = true)
    {
        UILabel mlb_name = UtilityObj.Get<UILabel>(trans, "lb_name");
        //UISprite mspr_showicon = UtilityObj.Get<UISprite>(trans, "showicon");
        //UISprite mquality = UtilityObj.Get<UISprite>(mspr_showicon.transform, "quality");

        var itemBase = Utility.GetItemByInfo(gemTable.id, trans, 0, itemSize.Size60);
        ItemBases.Add(itemBase);
        
        int quality = ItemTableManager.Instance.GetItemQuality(gemTable.id);
        mlb_name.text = UtilityColor.BBCode(gemTable.name, quality);
        if (!isGrade)
        itemBase.IconGray();

        //mquality.spriteName = $"quality{quality}";
        //mspr_showicon.spriteName = ItemTableManager.Instance.GetItemIcon(gemTable.id);
        //if (!isGrade)
        //    mspr_showicon.color = Color.black;



    }

    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    protected override void OnDestroy()
    {
        if (ItemBases != null)
        {
            UIItemManager.Instance.RecycleItemsFormMediator(ItemBases);
            ItemBases.Clear();
            ItemBases = null;
        }
        CSEffectPlayMgr.Instance.Recycle(mobj_effect);
        mItemBarDatas.Clear();
        mItemBarDatas = null;
    }
}