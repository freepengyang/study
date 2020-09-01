using System;
using System.Collections.Generic;
using UnityEngine;

public class UIWelfareDirectPurchaseBinder : UIBinder
{
    private DirectPurchaseData directPurchaseData;
    private UILabel lb_name;
    private UILabel lb_count;
    private UIEventListener btn_buy;
    private UILabel lb_buy;
    private UIGridContainer grid_itemBase;
    private GameObject sp_soldout;
    private GameObject redpoint;

    private GameObject gp;

    //private CSRechargeData rechargeData;
    TABLE.RECHARGE recharge;

    PoolHandleManager mPoolHandle = new PoolHandleManager();
    
    Dictionary<int, int> dicBoxItem = new Dictionary<int, int>();

    public override void Init(UIEventListener handle)
    {
        gp = handle.gameObject;
        lb_name = Get<UILabel>("lb_name");
        lb_count = Get<UILabel>("lb_count");
        btn_buy = Get<UIEventListener>("btn_buy");
        lb_buy = Get<UILabel>("lb_buy", btn_buy.transform);
        grid_itemBase = Get<UIGridContainer>("grid_itemBase");
        sp_soldout = Get<GameObject>("sp_soldout");
        redpoint = Get<GameObject>("redpoint", btn_buy.transform);

        UIEventListener.Get(btn_buy.gameObject).onClick = OnClickBuy;
        
        CSEffectPlayMgr.Instance.ShowUITexture(gp, "direct_purchase_bg");
    }

    public override void Bind(object data)
    {
        if (data == null) return;
        directPurchaseData = (DirectPurchaseData) data;
        if (directPurchaseData.Giftbag.gainType==3)
        {
            RechargeTableManager.Instance.TryGetValue(directPurchaseData.Giftbag.para, out recharge);
            //if (RechargeTableManager.Instance.TryGetValue(directPurchaseData.Giftbag.para, out recharge))
            //    rechargeData.Init(recharge, RechargeType.DirectPurchase);
        }
        RefreshUI();
    }

    private List<UIItemBase> listItemBase;
    //private ILBetterList<ILBetterList<int>> listBoxItem;
    //private ILBetterList<int> listTmp;

    void RefreshUI()
    {
        if (listItemBase == null)
            listItemBase = new List<UIItemBase>();
        lb_name.text = directPurchaseData.Giftbag.name;
        BoxTableManager.Instance.GetBoxAwardById(directPurchaseData.Giftbag.rewards, dicBoxItem);
        grid_itemBase.MaxCount = Mathf.Min(4, dicBoxItem.Count);
        int index = 0;
        GameObject gp;
        for (var it = dicBoxItem.GetEnumerator(); it.MoveNext();)
        {
            if (index<4)
            {
                gp = grid_itemBase.controlList[index];
                if (listItemBase.Count <= index)
                    listItemBase.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, gp.transform));
                UIItemBase itemBase = listItemBase[index];
                itemBase.Refresh(it.Current.Key);
                itemBase.SetCount(it.Current.Value, false,false);
                index++;
            }
        }

        lb_count.text = $"{directPurchaseData.BuyTimes}/{directPurchaseData.Giftbag.limitTime}";
        lb_count.gameObject.SetActive(directPurchaseData.SoldOut == 0);
        btn_buy.gameObject.SetActive(directPurchaseData.SoldOut == 0);
        sp_soldout.gameObject.SetActive(directPurchaseData.SoldOut == 1);
        switch (directPurchaseData.Giftbag.gainType)
        {
            case 1: //元宝
                lb_buy.text = CSString.Format(1750, directPurchaseData.Giftbag.para);
                redpoint.SetActive(CSItemCountManager.Instance.GetItemCount((int)MoneyType.yuanbao) >=
                                   directPurchaseData.Giftbag.para);
                break;
            case 3: //人民币
                lb_buy.text = CSString.Format(1751,
                    RechargeTableManager.Instance.GetRechargeMoney(directPurchaseData.Giftbag.para));
                redpoint.SetActive(false);
                break;
        }
    }

    void OnClickBuy(GameObject go)
    {
        switch (directPurchaseData.Giftbag.gainType)
        {
            case 1: //元宝
                if (CSItemCountManager.Instance.GetItemCount((int)MoneyType.yuanbao) >= directPurchaseData.Giftbag.para)
                {
                    Net.CSDailyPurchaseBuyMessage(directPurchaseData.Giftbag.id, 1);
                }
                else
                {
                    // if (CSVipInfo.Instance.IsFirstRecharge()) //充值
                    // {
                    //     UtilityTips.ShowPromptWordTips(6, () => { UtilityPanel.JumpToPanel(12305); });
                    // }
                    // else //活动
                    // {
                    //     UtilityTips.ShowPromptWordTips(5,
                    //         () => { UIManager.Instance.CreatePanel<UIRechargeFirstPanel>(); });
                    // }
                    Utility.ShowGetWay(3);
                }

                break;
            case 3: //人民币
                CSRechargeInfo.Instance.TryToRecharge(recharge);
                break;
        }
    }

    public override void OnDestroy()
    {
        directPurchaseData = null;
        lb_name = null;
        lb_count = null;
        btn_buy = null;
        lb_buy = null;
        grid_itemBase = null;
        sp_soldout = null;
        //listBoxItem = null;
        //listTmp = null;
        redpoint = null;
        recharge = null;
        mPoolHandle = null;
        gp = null;
        dicBoxItem = null;
        CSEffectPlayMgr.Instance.Recycle(gp);
        UIItemManager.Instance.RecycleItemsFormMediator(listItemBase);
    }
}