using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIBuyVigorPanel : UIBasePanel
{
    public override UILayerType PanelLayerType { get { return UILayerType.Tips; } }
    #region variable
    UIItemBase item;
    fight.BufferInfo buffinfo;
    int vigorDanId = 610;
    int vigorBuffId = 0;
    int vigorMultiples = 0;
    TABLE.ITEM vigorDanCfg;
    Coroutine buffCountCor;
    long remainingTime;
    WaitForSeconds wait;
    int buyNum = 1;
    int shopId = 125;

    int costType = 0;
    int costValue = 0;
    #endregion
    public override void Init()
    {
        base.Init();
        AddCollider();
        item = UIItemManager.Instance.GetItem(PropItemType.Normal, mtran_itemPar);
        vigorMultiples = int.Parse(SundryTableManager.Instance.GetSundryEffect(95));
        vigorDanCfg = ItemTableManager.Instance.GetItemCfg(vigorDanId);
        List<int> idList = UtilityMainMath.SplitStringToIntList(vigorDanCfg.bufferParam);
        if (idList.Count > 0)
        {
            vigorBuffId = idList[0];
        }
        wait = new WaitForSeconds(1f);
        UIEventListener.Get(mbtn_close).onClick = Click;
        UIEventListener.Get(mbtn_buy).onClick = BuyBtnClick;
        UIEventListener.Get(mbtn_add).onClick = NumAdd;
        UIEventListener.Get(mbtn_reduce).onClick = NumReduce;
        UIEventListener.Get(mbtn_costAdd).onClick = CostAdd;
        UIEventListener.Get(msp_costIcon.gameObject).onClick = ShowTips;
        costType = ShopTableManager.Instance.GetShopPayType(shopId);
        costValue = ShopTableManager.Instance.GetShopValue(shopId);
        msp_costIcon.spriteName = $"tubiao{costType}";
        minput_num.value = buyNum.ToString();
        mlb_costNum.text = (costValue * buyNum).ToString();
        item.Refresh(vigorDanId);
        mlb_name.text = ItemTableManager.Instance.GetItemName(vigorDanId);
        mlb_name.color = UtilityCsColor.Instance.GetColor(ItemTableManager.Instance.GetItemQuality(vigorDanId));
        minput_num.onValidate = OnValidateInput;
        minput_num.onChange.Add(new EventDelegate(OnSellPriceChange));
        if (CSItemCountManager.Instance.GetItemCount(costType) < (costValue * buyNum))
        {
            mlb_costNum.color = CSColor.red;
        }
        else
        {
            mlb_costNum.color = CSColor.green;
        }
    }

    public override void Show()
    {
        base.Show();
        RefreshVigorBuff();
    }
    void RefreshVigorBuff()
    {
        buffinfo = CSMainPlayerInfo.Instance.BuffInfo.GetBuff(vigorBuffId);
        mlb_des2.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(538), vigorMultiples * (1 + (vigorDanCfg.data * 0.0001)));
        if (buffinfo != null)
        {
            buffCountCor = ScriptBinder.StartCoroutine(BuffCountDown());
            remainingTime = (buffinfo.totalTime + buffinfo.addTime - CSServerTime.Instance.TotalMillisecond) / 1000;
            mlb_time.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1902), CSServerTime.Instance.FormatLongToTimeStr(remainingTime));
        }
        else
        {
            //mlb_des2.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(538), vigorMultiples);
            mlb_time.text = ClientTipsTableManager.Instance.GetClientTipsContext(1901);
            if (buffCountCor != null)
            {
                ScriptBinder.StopCoroutine(buffCountCor);
            }
        }
    }
    IEnumerator BuffCountDown()
    {
        yield return wait;
        RefreshVigorBuff();
    }
    #region ����¼�
    char OnValidateInput(string text, int charIndex, char addedChar)
    {
        int num = 0;
        if (int.TryParse(addedChar.ToString(), out num))
        {
            return addedChar;
        }
        else
        {
            return (char)0;
        }
    }
    void OnSellPriceChange()
    {
        if (string.IsNullOrEmpty(minput_num.value))
        {
            return;
        }
        if (CSItemCountManager.Instance.GetItemCount(costType) < (costValue * buyNum))
        {
            mlb_costNum.color = CSColor.red;
        }
        else
        {
            mlb_costNum.color = CSColor.green;
        }
    }
    void Click(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIBuyVigorPanel>();
    }
    void BuyBtnClick(GameObject _go)
    {
        if (CSItemCountManager.Instance.GetItemCount(costType) < (costValue * buyNum))
        {
            Utility.ShowGetWay(costType);
            UIManager.Instance.ClosePanel<UIBuyVigorPanel>();
        }
        else
        {
            Net.CSShopBuyItemMessage(125, buyNum, false, true);
            UIManager.Instance.ClosePanel<UIBuyVigorPanel>();
            UtilityTips.ShowTips(1790, 1.5f, ColorType.Green, vigorMultiples * (1 + (vigorDanCfg.data * 0.0001)));
        }
    }
    void NumAdd(GameObject _go)
    {
        buyNum++;
        buyNum = (buyNum > 99) ? 99 : buyNum;
        minput_num.value = buyNum.ToString();
        mlb_costNum.text = (costValue * buyNum).ToString();
    }
    void NumReduce(GameObject _go)
    {
        buyNum--;
        buyNum = (buyNum <= 0) ? 1 : buyNum;
        minput_num.value = buyNum.ToString();
        mlb_costNum.text = (costValue * buyNum).ToString();
    }
    void CostAdd(GameObject _go)
    {
        Utility.ShowGetWay(3);
        UIManager.Instance.ClosePanel<UIBuyVigorPanel>();
    }
    void ShowTips(GameObject _go)
    {
        UITipsManager.Instance.CreateTips(TipsOpenType.Normal, 3);
    }
    #endregion
    protected override void OnDestroy()
    {
        if (item != null)
        {
            UIItemManager.Instance.RecycleSingleItem(item);
        }
        base.OnDestroy();
    }
}
