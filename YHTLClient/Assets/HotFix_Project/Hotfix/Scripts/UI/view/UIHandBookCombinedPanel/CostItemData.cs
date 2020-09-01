using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CostItemMoneyFlag
{
    CIMF_COMPARE = (1 << 0),
    CIMF_RED_GREEN = (1 << 1),
    CIMF_SHORT_EXPRESS = (1 << 2),
    CIMF_SHORT_EXPRESS_ONE_POINT = (1 << 3),
    CIMF_SHORT_EXPRESS_TWO_POINT = (1 << 4),
}

public class CostItemData
{
    public int cfgId;
    public long count;
    public TABLE.ITEM item;
    public int moneyFlag;
    public bool HasMoneyFlag(int flag)
    {
        return (flag & moneyFlag) == flag;
    }
}