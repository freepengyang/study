using System.Collections.Generic;
using UnityEngine;

public partial class UIArmRacePromptPanel : UIBasePanel
{
    int state = 0;
    List<UIItemBase> itemList;
    public override void Init()
    {
        base.Init();
        UIEventListener.Get(mobj_bg).onClick = CloseClick;
        UIEventListener.Get(mbtn_get1).onClick = GetBtnClick;
        UIEventListener.Get(mbtn_recharge1).onClick = RechargeBtnClick;

    }

    public override void Show()
    {
        base.Show();
    }
    int tempId;
    /// <summary>
    /// 参数是groupID   需要一个转化
    /// </summary>
    /// <param name="_groupId"></param>
    public void SetType(int _groupId)
    {
        tempId = GiftBagTableManager.Instance.GetArmIdByGroupId(_groupId);
        mlb_des1.text = GiftBagTableManager.Instance.GetGiftBagDesc(tempId);
        dailypurchase.GiftBuyInfo info = CSArmRaceInfo.Instance.GetGiftInfoById(tempId);

        if (info.buyTimes == 0)
        {
            if (CSDayChargeInfo.Instance.GetDayCharge() >= GiftBagTableManager.Instance.GetGiftBagPara(tempId))
            {
                state = 2;
            }
            else
            {
                state = 1;
            }
        }
        else
        {
            state = 3;
        }
        //Debug.Log(state + "    " + tempId);
        mbtn_recharge1.SetActive(state == 1);
        mbtn_get1.SetActive(state == 2);
        mobj_complete1.SetActive(state == 3);
        mlb_num1.text = $"{CSDayChargeInfo.Instance.GetDayCharge()}/{GiftBagTableManager.Instance.GetGiftBagPara(tempId)}";
        mlb_num1.color = (CSDayChargeInfo.Instance.GetDayCharge() >= GiftBagTableManager.Instance.GetGiftBagPara(tempId) ? CSColor.green : CSColor.red);
        // 1充值   2领取   3已领取
        if (state == 2 || state == 3)
        {
            mlb_num1.color = CSColor.green;
        }
        else
        {
            mlb_num1.color = CSColor.red;
        }
        itemList = Utility.GetItemByBoxid(GiftBagTableManager.Instance.GetGiftBagRewards(tempId), mgrid_reward1, itemList, itemSize.Size60);
        //mgrid_reward1.Reposition();
    }
    protected override void OnDestroy()
    {
        if (itemList != null)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                UIItemManager.Instance.RecycleSingleItem(itemList[i]);
            }
        }
        base.OnDestroy();
    }

    void CloseClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIArmRacePromptPanel>();
    }
    void RechargeBtnClick(GameObject _go)
    {
        FNDebug.Log("跳转到充值界面");
        UIManager.Instance.ClosePanel<UIArmRacePromptPanel>();
        UtilityPanel.JumpToPanel(12305);
    }
    void GetBtnClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIArmRacePromptPanel>();
        Net.CSDailyPurchaseBuyMessage(tempId, 1);
    }
}
