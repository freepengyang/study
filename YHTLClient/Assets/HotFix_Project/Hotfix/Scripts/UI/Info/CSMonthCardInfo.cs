using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSMonthCardInfo : CSInfo<CSMonthCardInfo>
{
    /// <summary>
    /// 勇者月卡充值表id
    /// </summary>
    public int rechargeCardAId;
    /// <summary>
    /// 王者月卡充值表id
    /// </summary>
    public int rechargeCardBId;


    public TABLE.RECHARGE cardRechargeA;
    public TABLE.RECHARGE cardRechargeB;


    Map<int, monthcard.MonthCardInfo> monthCardInfo;

    /// <summary>
    /// 勇者之地杂项id
    /// </summary>
    const int sundryAId = 994;
    /// <summary>
    /// 王者之地杂项id
    /// </summary>
    const int sundryBId = 995;


    public int ticketAId;
    public int ticketBId;
     
    public int needACount;
    public int needBCount;


    bool hasOpenCardMapPanel;

    public override void Dispose()
    {
        monthCardInfo?.Clear();
        monthCardInfo = null;

        cardRechargeA = null;
        cardRechargeB = null;

    }


    public CSMonthCardInfo()
    {
        string str = SundryTableManager.Instance.GetSundryEffect(sundryAId);
        string[] strArr;
        if (!string.IsNullOrEmpty(str))
        {
            strArr = str.Split('#');
            if (strArr.Length >= 2)
            {
                int.TryParse(strArr[0], out ticketAId);
                int.TryParse(strArr[1], out needACount);
            }
        }

        str = SundryTableManager.Instance.GetSundryEffect(sundryBId);
        if (!string.IsNullOrEmpty(str))
        {
            strArr = str.Split('#');
            if (strArr.Length >= 2)
            {
                int.TryParse(strArr[0], out ticketBId);
                int.TryParse(strArr[1], out needBCount);
            }
        }

        rechargeCardAId = MonthCardTableManager.Instance.GetMonthCardPrice(1);
        rechargeCardBId = MonthCardTableManager.Instance.GetMonthCardPrice(2);

        RechargeTableManager.Instance.TryGetValue(rechargeCardAId, out cardRechargeA);
        RechargeTableManager.Instance.TryGetValue(rechargeCardBId, out cardRechargeB);
    }


    public void SC_MonthCardInfo(monthcard.MonthCardInfoResponse msg)
    {
        if (monthCardInfo == null) monthCardInfo = new Map<int, monthcard.MonthCardInfo>();
        for (int i = 0; i < msg.monthCardInfo.Count; i++)
        {
            SC_OneCardInfoChange(msg.monthCardInfo[i]);
        }
    }


    public void SC_BuyCardRes(monthcard.MonthCardInfo info)
    {
        monthcard.MonthCardInfo old = GetOneCardInfo(info.id);
        long oldTime = old.endTime;
        if (info.endTime > oldTime)
        {
            UtilityTips.ShowRedTips(1126, MonthCardTableManager.Instance.GetMonthCardName(info.id));
        }

        SC_OneCardInfoChange(info);
    }


    public void SC_ReceiveRewardsRes(monthcard.MonthCardInfo info)
    {
        monthcard.MonthCardInfo old = GetOneCardInfo(info.id);
        bool oldRes = old.isReceive;
        if (!oldRes && info.isReceive)
        {
            UIManager.Instance.CreatePanel<UIMonthCardGiftPanel>(f =>
            {
                (f as UIMonthCardGiftPanel).OpenPanel(old);
            });
        }

        SC_OneCardInfoChange(info);
    }


    void SC_OneCardInfoChange(monthcard.MonthCardInfo info)
    {
        if (monthCardInfo == null) monthCardInfo = new Map<int, monthcard.MonthCardInfo>();
        //Debug.LogError("@@@@@ID:" + info.id);
        int id = info.id;
        if (!monthCardInfo.ContainsKey(id)) monthCardInfo.Add(id, info);
        else monthCardInfo[id] = info;
        
        mClientEvent.SendEvent(CEvent.MonthCardInfoChange);
    }


    public monthcard.MonthCardInfo GetOneCardInfo(int id)
    {
        if (monthCardInfo == null) monthCardInfo = new Map<int, monthcard.MonthCardInfo>();

        if (!monthCardInfo.ContainsKey(id) || monthCardInfo[id] == null)
        {
            monthcard.MonthCardInfo card = new monthcard.MonthCardInfo();
            card.id = id;
            card.isReceive = false;
            card.endTime = 0;
            card.keepRewardDay = 0;
            monthCardInfo.Add(id, card);
        }

        return monthCardInfo[id];
    }


    public bool HasMonthCard(int id)
    {
        if (monthCardInfo == null || !monthCardInfo.ContainsKey(id) || monthCardInfo[id] == null) return false;
        long curTimeStamp = CSServerTime.Instance.TotalMillisecond;
        return monthCardInfo[id].endTime > curTimeStamp;
    }


    public void TryToBuyCard(int id)
    {
        //int moneyID = (int)MoneyType.yuanbao;
        //if (moneyID.GetItemCount() < MonthCardTableManager.Instance.GetMonthCardPrice(id))
        //{
        //    //UtilityTips.ShowRedTips(CSString.Format(965, ItemTableManager.Instance.GetItemName(moneyID)));
        //    UtilityPanel.JumpToPanel(12305);
        //    UIManager.Instance.ClosePanel<UIWelfareActivityPanel>();
        //    return;
        //}

        //Net.CSBuyMonthCardMessage(id);  旧规则，元宝购买

        if (id == 1 && cardRechargeA != null)
        {
            CSRechargeInfo.Instance.TryToRecharge(cardRechargeA);
        }
        else if (id == 2 && cardRechargeB != null)
        {
            CSRechargeInfo.Instance.TryToRecharge(cardRechargeB);
        }
    }


    public void TryToReceiveRewards(int id)
    {
        Net.CSReceiveMonthCardRewardMessage(id);
    }


    public bool MonthCardRedPoint()
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_monthCard))
        {
            return false;
        }
        if (monthCardInfo == null) return false;
        long curTimeStamp = CSServerTime.Instance.TotalMillisecond;
        for (monthCardInfo.Begin(); monthCardInfo.Next();)
        {
            monthcard.MonthCardInfo card = monthCardInfo.Value;
            if (card == null) continue;
            if (card.endTime > curTimeStamp && !card.isReceive)
            {
                return true;
            }
        }

        return false;
    }


    public int GetShenFuAddTimes()
    {
        int t = 0;
        if (monthCardInfo == null) 
            return 0;

        long curTimeStamp = CSServerTime.Instance.TotalMillisecond;
        for (monthCardInfo.Begin(); monthCardInfo.Next();)
        {
            monthcard.MonthCardInfo card = monthCardInfo.Value;
            if (card == null) continue;
            if (card.endTime > curTimeStamp)
            {
                t += 1;
            }
        }

        return t;
    }

    public int GetShenFuTime()
    {
        int t = 0;
        if (monthCardInfo == null) return 0;

        long curTimeStamp = CSServerTime.Instance.TotalMillisecond;
        for (monthCardInfo.Begin(); monthCardInfo.Next();)
        {
            monthcard.MonthCardInfo card = monthCardInfo.Value;
            if (card == null) continue;
            if (card.endTime > curTimeStamp)
            {
                t += MonthCardTableManager.Instance.GetMonthCardShenfu(card.id);
            }
        }
        
        return t;
    }


    public bool MonthCardMapShowRedPoint()
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_monthCard))
        {
            return false;
        }
        //if (hasOpenCardMapPanel) return false;
        if (HasMonthCard(1))
        {
            if (ticketAId.GetItemCount() >= needACount) return true;
        }

        if (HasMonthCard(2))
        {
            if (ticketBId.GetItemCount() >= needBCount) return true;
        }

        return false;
    }


    public void OpenCardMapPanel()
    {
        if (!hasOpenCardMapPanel)
        {
            hasOpenCardMapPanel = true;
            mClientEvent.SendEvent(CEvent.CardMapPanelOpened);
        }
    }
}
