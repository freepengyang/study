using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDirectPurchaseRewardBinder : UIBinder
{
    private DirectPurchaseRewardsData directPurchaseRewardsData;
    private UILabel lb_des;
    private GameObject sp_check;
    private UIEventListener btn_get;
    private UIEventListener btn_go;
    private UILabel lb_get;
    private UIGridContainer grid_itemBase;
    private GameObject redpoint;
    public UIScrollView scrollView;

    Dictionary<int, int> dicBoxItem = new Dictionary<int, int>();

    public override void Init(UIEventListener handle)
    {
        lb_des = Get<UILabel>("lb_des");
        sp_check = Get<GameObject>("sp_check");
        btn_go = Get<UIEventListener>("btn_go");
        btn_get = Get<UIEventListener>("btn_get");
        lb_get = Get<UILabel>("Label", btn_get.transform);
        grid_itemBase = Get<UIGridContainer>("ScrollView/grid_itemBase");
        redpoint = Get<GameObject>("redpoint", btn_get.transform);
        UIEventListener.Get(btn_get.gameObject).onClick = OnClickReceive;
        UIEventListener.Get(btn_go.gameObject).onClick = OnClickGo;
    }

    public override void Bind(object data)
    {
        if (data == null) return;
        directPurchaseRewardsData = (DirectPurchaseRewardsData) data;
        RefreshUI();
    }

    private List<UIItemBase> listItemBase;

    void RefreshUI()
    {
        if (listItemBase == null)
            listItemBase = new List<UIItemBase>();
        int curnum = CSDirectPurchaseInfo.Instance.Money;
        int maxnum = directPurchaseRewardsData.Rechargereward.need;
        
        string numStr =
            $"({curnum}/{maxnum})".BBCode(
                curnum >= maxnum
                    ? ColorType.Green
                    : ColorType.Red);
        lb_des.text =
            $"{CSString.Format(directPurchaseRewardsData.Rechargereward.desc, maxnum)}{numStr}";
        //Item列表
        if (listItemBase == null)
            listItemBase = new List<UIItemBase>();
        dicBoxItem?.Clear();
        BoxTableManager.Instance.GetBoxAwardById(directPurchaseRewardsData.Rechargereward.box, dicBoxItem);
        grid_itemBase.MaxCount = dicBoxItem.Count;
        int index = 0;
        GameObject gp;
        for (var it = dicBoxItem.GetEnumerator(); it.MoveNext();)
        {
            gp = grid_itemBase.controlList[index];
            if (listItemBase.Count <= index)
                listItemBase.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, gp.transform, itemSize.Size58));
            UIItemBase itemBase = listItemBase[index];
            itemBase.obj.GetComponent<UIDragScrollView>().scrollView = scrollView;
            itemBase.Refresh(it.Current.Key);
            itemBase.SetCount(it.Current.Value, false,false);
            index++;
        }

        switch (directPurchaseRewardsData.ReceiveState)
        {
            case 0://可领取
                // btn_get.GetComponent<UISprite>().spriteName = "btn_samll1";
                // lb_get.color = UtilityColor.GetColor(ColorType.CommonButtonGreen);
                sp_check.SetActive(false);
                redpoint.SetActive(true);
                btn_get.gameObject.SetActive(true);
                btn_go.gameObject.SetActive(false);
                break;
            case 1://未达成
                // btn_get.GetComponent<UISprite>().spriteName = "btn_samll4";
                // lb_get.color = UtilityColor.GetColor(ColorType.CommonButtonGrey);
                sp_check.SetActive(false);
                redpoint.SetActive(false);
                btn_get.gameObject.SetActive(false);
                btn_go.gameObject.SetActive(true);
                break;
            case 2://已领取
                sp_check.SetActive(true);
                redpoint.SetActive(false);
                btn_get.gameObject.SetActive(false);
                btn_go.gameObject.SetActive(false);
                break;
        }
    }

    void OnClickReceive(GameObject go)
    {
        switch (directPurchaseRewardsData.ReceiveState)
        {
            case 0:
                Net.CSReceiveAccumulatedRechargeRewardMessage(directPurchaseRewardsData.Rechargereward.id);
                break;
            case 1:
                UtilityTips.ShowRedTips(1802);
                break;
        }
    }

    void OnClickGo(GameObject go)
    {
        if (!UIManager.Instance.IsPanel<UIWelfareDirectPurchasePanel>())
            UtilityPanel.JumpToPanel(12602);

        UIManager.Instance.ClosePanel<UIDirectPurchaseRewardPanel>();
    }

    public override void OnDestroy()
    {
        directPurchaseRewardsData = null;
        lb_des = null;
        sp_check = null;
        btn_get = null;
        grid_itemBase = null;
        scrollView = null;
        lb_get = null;
        btn_go = null;
        UIItemManager.Instance.RecycleItemsFormMediator(listItemBase);
    }
}