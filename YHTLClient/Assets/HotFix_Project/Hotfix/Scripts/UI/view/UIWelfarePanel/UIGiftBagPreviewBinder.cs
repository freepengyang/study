using System.Collections.Generic;
using Google.Protobuf.Collections;
using UnityEngine;

public class UIGiftBagPreviewBinder : UIBinder
{
    private UILabel lb_name;
    private UIGridContainer grid_itemBase;
    private UILabel lb_count;
    private UILabel lb_count_name;
    private UILabel lb_buy;
    private GameObject btn_buy;
    private GameObject redpoint;
    private GameObject sp_soldout;
    private GameObject gp;

    Dictionary<int, int> dicBoxItem = new Dictionary<int, int>();

    //private CSRechargeData rechargeData;
    TABLE.RECHARGE recharge;
    PoolHandleManager mPoolHandle = new PoolHandleManager();
    private DirectPurchaseData directPurchaseData;
    public RepeatedField<int> viewedIds;

    public override void Init(UIEventListener handle)
    {
        gp = handle.gameObject;
        lb_name = Get<UILabel>("lb_name");
        grid_itemBase = Get<UIGridContainer>("grid_itemBase");
        lb_count = Get<UILabel>("lb_count");
        lb_count_name = Get<UILabel>("Label", lb_count.transform);
        btn_buy = Get<GameObject>("btn_buy");
        lb_buy = Get<UILabel>("lb_buy", btn_buy.transform);
        redpoint = Get<GameObject>("redpoint", btn_buy.transform);
        sp_soldout = Get<GameObject>("sp_soldout");
        CSEffectPlayMgr.Instance.ShowUITexture(gp, "gift_bag_frame");
        UIEventListener.Get(btn_buy.gameObject).onClick = OnClickBtnBuy;
        //暂时不用红点
        redpoint.SetActive(false);
    }

    public override void Bind(object data)
    {
        if (data == null) return;
        directPurchaseData = (DirectPurchaseData) data;
        if (directPurchaseData.Giftbag.gainType == 3)
        {
            if (!RechargeTableManager.Instance.TryGetValue(directPurchaseData.Giftbag.para,
                out recharge))
            {
                //Debug
            }
        }

        RefreshUI();
    }

    private List<UIItemBase> listItemBase;
    private ILBetterList<ILBetterList<int>> listBoxItem;
    private ILBetterList<int> listTmp;

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
            if (index < 4)
            {
                gp = grid_itemBase.controlList[index];
                if (listItemBase.Count <= index)
                    listItemBase.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, gp.transform,
                        itemSize.Size60));
                UIItemBase itemBase = listItemBase[index];
                itemBase.Refresh(it.Current.Key);
                itemBase.SetCount(it.Current.Value, false, false);
                index++;
            }
        }

        int limitType = directPurchaseData.Giftbag.limitType;
        switch (limitType)
        {
            case 0:
                lb_count.gameObject.SetActive(false);
                sp_soldout.SetActive(false);
                break;
            case 1:
            case 2:
                lb_count.gameObject.SetActive(true);
                int buytimes = directPurchaseData.BuyTimes;
                int limitTime = directPurchaseData.Giftbag.limitTime;
                lb_count.text =
                    $"{buytimes}/{limitTime}".BBCode(buytimes >= limitTime
                        ? ColorType.ToolTipUnDone
                        : ColorType.ToolTipDone);

                sp_soldout.SetActive(buytimes >= limitTime);

                if (limitType == 1)
                    lb_count_name.text = CSString.Format(1812);
                else if (limitType == 2)
                    lb_count_name.text = CSString.Format(1811);
                break;
        }

        // lb_count_name.text = 
        // lb_count.text =
        //     $"{buyTimes}/{limitTime}".BBCode(buyTimes >= limitTime ? ColorType.ToolTipUnDone : ColorType.ToolTipDone);
        // lb_count.gameObject.SetActive(discountGiftBagGroupData.SoldOut == 0);
        btn_buy.gameObject.SetActive(directPurchaseData.SoldOut == 0);
        sp_soldout.gameObject.SetActive(directPurchaseData.SoldOut == 1);
        switch (directPurchaseData.Giftbag.gainType)
        {
            case 1: //元宝
                lb_buy.text = CSString.Format(1843, directPurchaseData.Giftbag.para);
                // redpoint.SetActive(CSBagInfo.Instance.GetMoneyByType(MoneyType.yuanbao) >=
                //                    directPurchaseData.Giftbag.para);
                break;
            case 3: //人民币
                lb_buy.text = CSString.Format(1844,
                    RechargeTableManager.Instance.GetRechargeMoney(directPurchaseData.Giftbag.para));
                // redpoint.SetActive(false);
                break;
        }
    }

    void OnClickBtnBuy(GameObject go)
    {
        switch (directPurchaseData.Giftbag.gainType)
        {
            case 1: //元宝
                if (CSItemCountManager.Instance.GetItemCount((int) MoneyType.yuanbao) >=
                    directPurchaseData.Giftbag.para)
                {
                    Net.CSDailyPurchaseBuyMessage(directPurchaseData.Giftbag.id, 1);
                    if (viewedIds != null && viewedIds.Count > 0)
                        Net.CSLookGiftMessage(viewedIds);
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
                if (viewedIds != null && viewedIds.Count > 0)
                    Net.CSLookGiftMessage(viewedIds);
                break;
        }
    }

    public override void OnDestroy()
    {
        grid_itemBase = null;
        lb_name = null;
        lb_count = null;
        lb_count_name = null;
        sp_soldout = null;
        directPurchaseData = null;
        dicBoxItem = null;
        recharge = null;
        mPoolHandle = null;
        dicBoxItem = null;
        btn_buy = null;
        redpoint = null;
        directPurchaseData = null;
        viewedIds = null;
        CSEffectPlayMgr.Instance.Recycle(gp);
        UIItemManager.Instance.RecycleItemsFormMediator(listItemBase);
    }
}