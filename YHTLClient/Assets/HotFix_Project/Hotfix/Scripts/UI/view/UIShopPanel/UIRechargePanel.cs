using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIRechargePanel : UIBasePanel
{
   
    public override void Init()
    {
        base.Init();

        //mClientEvent.AddEvent(CEvent.MoneyChange, OnMoneyChange);目前该金币条ui去掉了
        mClientEvent.AddEvent(CEvent.RechargeInfoUpdate, OnRechargeInfoChange);

        CSEffectPlayMgr.Instance.ShowUITexture(mtexbg, "shop_bg");

        InitPanel();
    }

    public override void Show()
    {
        base.Show();
        CSRechargeInfo.Instance.MonthRechargePanelOpen();
    }

    public void InitPanel()
    {
        //RefreshMoneyUI();

        RefreshRechargeItems();
    }


    void RefreshRechargeItems()
    {
        List<CSRechargeData> list = CSRechargeInfo.Instance.CurRechargeList;

        mgrid.Bind<CSRechargeData, UIRechargeItem>(list, mPoolHandleManager);
    }


    void RefreshMoneyUI()
    {
        mlb_yuanbao.text = ((int) MoneyType.yuanbao).GetItemCount().ToString();
    }


    

    protected override void OnDestroy()
    {
        if (mtexbg != null)
            CSEffectPlayMgr.Instance.Recycle(mtexbg);

        mgrid.UnBind<UIRechargeItem>();

        base.OnDestroy();
    }


    void OnMoneyChange(uint id, object data)
    {
        RefreshMoneyUI();
    }

    void OnRechargeInfoChange(uint id, object data)
    {
        RefreshRechargeItems();
    }
}


public class UIRechargeItem : UIBinder
{
    private GameObject _tx_money;
    private UILabel _lb_moneyGold;
    private UILabel _lb_moneyCNY;
    UIGridContainer _grid_item;
    UISprite _sp_flag;

    private CSRechargeData mData;

    List<UIItemBase> itemList = new List<UIItemBase>();



    public override void Init(UIEventListener handle)
    {
        _tx_money = Get<GameObject>("sp_money");
        _lb_moneyGold = Get<UILabel>("lb_money1");
        _lb_moneyCNY = Get<UILabel>("lb_money2");
        _grid_item = Get<UIGridContainer>("grid_bag");
        _sp_flag = Get<UISprite>("sp_flag");

        //handle.onClick = OnClick;
        UIEventListener.Get(_tx_money.gameObject).onClick = OnClick;
    }

    public override void Bind(object data)
    {
        mData = data as CSRechargeData;
        if (mData == null || mData.Config == null) return;
        TABLE.RECHARGE cfg = mData.Config;
        if (!string.IsNullOrEmpty(cfg.imgSrc))
        {
            CSEffectPlayMgr.Instance.ShowUITexture(_tx_money, cfg.imgSrc);
        }

        int curGold = (int)cfg.gold;
        int curCNY = cfg.Money;

        _lb_moneyGold.text = $"{curGold}元宝";
        _lb_moneyCNY.text = $"￥{curCNY}";

        if (cfg.bonusBox > 0)
        {
            int rewardTimes = CSRechargeInfo.Instance.GetRechargeTimes(cfg.id);
            if (rewardTimes >= 1)//每月只能购买一次，策划要求定死
            {
                _grid_item.MaxCount = 0;
                _sp_flag.CustomActive(false);
                return;
            }

            if (mData.rewardList == null) return;

            if (itemList == null) itemList = new List<UIItemBase>();
            else itemList.Clear();

            var dic = mData.rewardList;
            Utility.GetItemByBoxid(mData.rewardList, _grid_item, ref itemList, itemSize.Size40);
            for (int i = 0; i < itemList.Count; i++)
            {
                var item = itemList[i];
                if (item == null || item.itemCfg == null || !dic.ContainsKey(item.itemCfg.id)) continue;
                int count = dic[item.itemCfg.id];
                if (count < 10000) continue;
                string numStr = UtilityMath.GetDecimalValue(count, "F1");
                item.SetCount(numStr, CSColor.white);
                //string num = utility
            }
            _sp_flag.CustomActive(true);
        }

    }

    public override void OnDestroy()
    {
        if (_tx_money != null) CSEffectPlayMgr.Instance.Recycle(_tx_money);
        UIItemManager.Instance.RecycleItemsFormMediator(itemList);
        _tx_money = null;
        _lb_moneyGold = null;
        _lb_moneyCNY = null;
        _grid_item = null;
        _sp_flag = null;
        mData = null;
    }


    void OnClick(GameObject obj)
    {
        if (mData == null || mData.Config == null) return;
        CSRechargeInfo.Instance.TryToRecharge(mData);
    }


}