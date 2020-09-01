using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBarData
{
    public int bgDepth = 5;
    public int cfgId;
    public long owned;
    public long needed;
    public int flag = ((int)ItemBarType.IBT_ADD | (int)ItemBarType.IBT_COMPARE);
    public int desId;
    public int bgWidth = -1;
    public System.Action onAddAction;
    public EventHanlderManager eventHandle;

    public enum ItemBarMode
    {
        IBM_GENERAL = 0,
        IBM_MONEY = 1,
        IBM_SABAC = 2,
    }
    public ItemBarMode eMode = ItemBarMode.IBM_GENERAL;
    public enum ItemBarType
    {
        IBT_ADD = (1 << 0),
        IBT_COMPARE = (1 << 1),
        IBT_SMALL_ICON = (1 << 2),
        IBT_RED_GREEN = (1 << 3),
        IBT_ONLY_COST = (1 << 4),
        IBT_ONLY_OWNED = (1 << 5),
        IBT_SHORT_EXPRESS = (1 << 6),//10000 => 1w etc
        IBT_SHORT_EXPRESS_WITH_ONE_POINT = (1 << 7),//1234 => 1.2w etc
        IBT_SHORT_EXPRESS_WITH_TWO_POINT = (1 << 8),//1234 => 1.23w etc

        IBT_GENERAL = IBT_ADD,
        IBT_GENERAL_COMPARE = IBT_COMPARE | IBT_ADD,
        IBT_GENERAL_SMALL = IBT_GENERAL | IBT_SMALL_ICON,
        IBT_GENERAL_COMPARE_SMALL = IBT_GENERAL_COMPARE | IBT_SMALL_ICON,
        IBT_GENERAL_COMPARE_SMALL_REDGREEN = IBT_GENERAL_COMPARE_SMALL|IBT_RED_GREEN,
        IBT_COST_GENERAL_COMPARE_RED_GREEN_ICON = IBT_GENERAL_COMPARE | IBT_ONLY_COST | IBT_RED_GREEN | IBT_SMALL_ICON,

        //IBT_MONEYPANEL = IBT_SMALL_ICON | IBT_ADD,
        IBT_MONEYPANEL = (1 << 10),
        //IBT_
    }
    public bool HasFlag(ItemBarType eFlag)
    {
        return (flag & (int)eFlag) == (int)eFlag;
    }

    public void OnDestroy()
    {
        onAddAction = null;
        eventHandle = null;
        flag = 0;
    }
    public void ResetOwned()
    {
        if (flag == (int)ItemBarData.ItemBarType.IBT_MONEYPANEL)
        {
            owned = cfgId.GetItemCountByMoneyType();
        }
    }
    public void GMCmdAddItem(int cnt = 1)
    {
        Net.GMCommand($"@i {cfgId} {cnt} 0");
    }
}

public class UIItemBarManager : IPooledGridContainerManager<UIItemBar, ItemBarData, UIItemBarManager>
{
    const string CONST_ITEM_BAR_PREFAB = "UIItemBarPrefab";
    protected override string GetPrefabName()
    {
        return CONST_ITEM_BAR_PREFAB;
    }
    public override ItemBarData Get()
    {
        ItemBarData ret = base.Get();
        ret.bgWidth = 0;
        return ret;
    }
}

public class UISabacItemManager : IPooledGridContainerManager<UIItemBar, ItemBarData, UISabacItemManager>
{
    const string CONST_ITEM_BAR_PREFAB = "UISabacItemPrefab";
    protected override string GetPrefabName()
    {
        return CONST_ITEM_BAR_PREFAB;
    }
    public override ItemBarData Get()
    {
        ItemBarData ret = base.Get();
        ret.bgWidth = 0;
        return ret;
    }
}

public class UIItemMoneyManager : IPooledGridContainerManager<UIItemBar, ItemBarData, UIItemMoneyManager>
{
    const string CONST_ITEM_BAR_PREFAB = "UIItemMoneyPrefab";
    protected override string GetPrefabName()
    {
        return CONST_ITEM_BAR_PREFAB;
    }
    public override ItemBarData Get()
    {
        ItemBarData ret = base.Get();
        ret.bgWidth = 0;
        return ret;
    }
}

public class UIDialogBarManager : IPooledGridContainerManager<UIItemBar, ItemBarData, UIDialogBarManager>
{
    const string CONST_ITEM_BAR_PREFAB = "UIDialogBarPrefab";
    protected override string GetPrefabName()
    {
        return CONST_ITEM_BAR_PREFAB;
    }
    public override ItemBarData Get()
    {
        ItemBarData ret = base.Get();
        ret.bgWidth = 0;
        return ret;
    }
}