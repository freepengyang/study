using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSYuanBaoInfo : CSInfo<CSYuanBaoInfo>
{
    /// <summary>
    /// 所有元宝id。可交易和不可交易
    /// </summary>
    List<int> allYuanBaoId = new List<int>();
    /// <summary>
    /// 可交易元宝id
    /// </summary>
    List<int> canTradeList = new List<int>();

    /// <summary>
    /// 不可交易元宝id
    /// </summary>
    List<int> canNotTradeList = new List<int>();


    string allGetWayStr;
    string canTradeGetWayStr;
    string canNotTradeGetWayStr;

    TABLE.ITEM yuanBaoCfg;

    public override void Dispose()
    {
        allYuanBaoId?.Clear();
        allYuanBaoId = null;
        canTradeList?.Clear();
        canTradeList = null;
        canNotTradeList?.Clear();
        canNotTradeList = null;
    }


    public CSYuanBaoInfo()
    {
        Init();
    }


    void Init()
    {
        string idStr = MoneyTypeTableManager.Instance.GetMoneyTypeContent(2);
        UtilityMainMath.SplitStringToIntList(canTradeList, idStr);

        idStr = MoneyTypeTableManager.Instance.GetMoneyTypeContent(3);
        UtilityMainMath.SplitStringToIntList(canNotTradeList, idStr);

        allYuanBaoId?.Clear();
        allYuanBaoId.AddRange(canTradeList);
        allYuanBaoId.AddRange(canNotTradeList);

        allGetWayStr = MoneyTypeTableManager.Instance.GetMoneyTypeGetway(1);
        canTradeGetWayStr = MoneyTypeTableManager.Instance.GetMoneyTypeGetway(2);
        canNotTradeGetWayStr = MoneyTypeTableManager.Instance.GetMoneyTypeGetway(3);

        ItemTableManager.Instance.TryGetValue(3, out yuanBaoCfg);
    }



    public long GetYuanBao(int _type)
    {
        long count = 0;
        count = GetAllYuanBaoCount();
        return count;
    }


    /// <summary>
    /// 所有元宝数量。可交易和不可交易
    /// </summary>
    /// <returns></returns>
    public long GetAllYuanBaoCount()
    {
        long count = 0;
        if (allYuanBaoId == null) return 0;
        for (int i = 0; i < allYuanBaoId.Count; i++)
        {
            count += (CSBagInfo.Instance.GetMoneyCount(allYuanBaoId[i]));
        }
        return count;
    }
    /// <summary>
    /// 可交易元宝数量
    /// </summary>
    /// <returns></returns>
    public long GetCanTradeYuanBaoCount()
    {
        long count = 0;
        if (canTradeList == null) return 0;
        for (int i = 0; i < canTradeList.Count; i++)
        {
            count += (CSBagInfo.Instance.GetMoneyCount(canTradeList[i]));
        }
        return count;
    }
    /// <summary>
    /// 不可交易元宝数量
    /// </summary>
    /// <returns></returns>
    public long GetCanNotTradeYuanBaoCount()
    {
        long count = 0;
        if (canNotTradeList == null) return 0;
        for (int i = 0; i < canNotTradeList.Count; i++)
        {
            count += (CSBagInfo.Instance.GetMoneyCount(canNotTradeList[i]));
        }
        return count;
    }


    public TABLE.ITEM GetYuanBaoConfig()
    {
        return yuanBaoCfg;
    }


    public void ShowGetWay(int _type)
    {
        int num = 0;
        switch (_type)
        {
            case 1:
            case 2:
            case 3:
                num = 3;
                break;

        }
        Utility.ShowGetWay(num);
    }
}