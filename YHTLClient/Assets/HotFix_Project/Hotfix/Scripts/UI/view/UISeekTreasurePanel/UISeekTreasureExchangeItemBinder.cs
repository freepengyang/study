using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISeekTreasureExchangeItemBinderData
{
    public int pointsId;
    public TABLE.ITEM itemCfg;
    public string cost;
    public System.Action<int> action;
    public bool isEnough;
    public int count;
}

public class UISeekTreasureExchangeItemBinder : UIBinder
{
    private GameObject item;
    private UIItemBase ItemBase;
    private UILabel lb_name;
    private UILabel lb_cost;
    private UIEventListener btn_exchange;
    private UILabel lb_exchange; 
    private System.Action<int> action;
    private int pointsId;

    public override void Init(UIEventListener handle)
    {
        item = Get<GameObject>("item");
        lb_name = Get<UILabel>("lb_name");
        lb_cost = Get<UILabel>("lb_cost");
        btn_exchange = Get<UIEventListener>("btn_exchange");
        lb_exchange = Get<UILabel>("lb_exchange", btn_exchange.transform);
    }

    public override void Bind(object data)
    {
        UISeekTreasureExchangeItemBinderData gData = data as UISeekTreasureExchangeItemBinderData;
        if (null != gData)
        {
            btn_exchange.onClick = OnClick;
            action = gData.action;
            gData.action = null;
            pointsId = gData.pointsId;
            RefreshUI(gData);
        }
    }
    
    void OnClick(GameObject go)
    {
        action?.Invoke(pointsId);
    }

    void RefreshUI(UISeekTreasureExchangeItemBinderData gData)
    {
        if (ItemBase==null)
            ItemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, item.transform, itemSize.Size64);
        ItemBase.Refresh(gData.itemCfg);
        ItemBase.SetCount(gData.count, isGetDecimalValue:false);
        lb_name.text = gData.itemCfg.name;
        lb_name.color = UtilityCsColor.Instance.GetColor(gData.itemCfg.quality);
        lb_cost.text = gData.cost;
        btn_exchange.GetComponent<UISprite>().spriteName = gData.isEnough ? "btn_samll2" : "btn_samll4";
        lb_exchange.color =
            UtilityColor.GetColor(gData.isEnough ? ColorType.CommonButtonBrown : ColorType.CommonButtonGrey);
    }

    public override void OnDestroy()
    {
        UIItemManager.Instance.RecycleSingleItem(ItemBase);
        item = null;
        lb_name = null;
        lb_cost = null;
        btn_exchange = null;
        lb_exchange = null; 
        action = null;
    }
}
