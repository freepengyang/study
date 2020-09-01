using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UIExchangeShopPanel : UIBasePanel
{

    FastArrayElementFromPool<UIShopTopTabBinder> tabList;

    EndLessKeepHandleList<UIExchangeShopItemBinder, CSExchangeItemData> endLessList;

    int curShowPage;//从1开始

    /// <summary> 当前显示的数据类型。对应BiQiShop表的主类型，1为开服类，2为日常类 </summary>
    int curDataType;

    TABLE.ITEM curTicket;


    public override void Init()
	{
		base.Init();

        mClientEvent.AddEvent(CEvent.ExchangeShopDataChange, DataChangeEvent);
        mClientEvent.AddEvent(CEvent.ResDayPassedMessage, ExchangeShopRefresh);
        mClientEvent.AddEvent(CEvent.ItemListChange, ItemListChangeEvent);

        mbtn_ticketAdd.onClick = AddTicketBtnClick;
        UIEventListener.Get(msp_ticketIcon.gameObject).onClick = TicketSpClick;

        curDataType = CSExchangeShopInfo.Instance.CurShowDataType;
        InitTopTabs();
    }
	
	public override void Show()
	{
		base.Show();

        CSExchangeShopInfo.Instance.OpenExchangeShop();
        RefreshUI();
    }
	
	protected override void OnDestroy()
	{
        if (tabList != null)
        {
            for (int i = 0; i < tabList.Count; i++)
            {
                tabList[i]?.Destroy();
            }
            tabList.Clear();
            tabList = null;
        }

        endLessList?.Destroy();
        endLessList = null;

        curTicket = null;

        base.OnDestroy();
	}


    void InitTopTabs()
    {
        int tabCount = 0;
        if (curDataType == 1)
            tabCount = CSExchangeShopInfo.Instance.serverOpenExchangeDays;
        else tabCount = 6;//日常循环策划定死为周一到周六6天

        if (tabList == null) tabList = mPoolHandleManager.CreateGeneratePool<UIShopTopTabBinder>();
        else
        {
            for (int i = 0; i < tabList.Count; i++)
            {
                tabList[i]?.Destroy();
            }
            tabList.Clear();
        }

        mgrid_toggles.MaxCount = tabCount;
        for (int i = 0; i < mgrid_toggles.controlList.Count; i++)
        {
            UIShopTopTabBinder binder = tabList.Append();
            var handle = UIEventListener.Get(mgrid_toggles.controlList[i]);
            binder.Setup(handle);
            binder.Bind(i + 1);//从1开始。和biqiShop表subType对应
            binder.SetClick(OnTabClick);
        }
    }


    void RefreshUI()
    {
        if (curDataType == 1)
        {
            OnTabClick((int)CSExchangeShopInfo.Instance.serverOpenDay);
        }
        else if (curDataType == 2)
        {
            int day = (int)CSExchangeShopInfo.Instance.dayOfWeek;
            OnTabClick(day == 0 ? 1 : day);
        }
    }


    void RefreshContents()
    {
        ILBetterList<CSExchangeItemData> list = CSExchangeShopInfo.Instance.GetOneDayDatasList(curShowPage);
        if (list == null || list.Count < 1) return;

        if (endLessList == null)
        {
            endLessList = new EndLessKeepHandleList<UIExchangeShopItemBinder, CSExchangeItemData>(SortType.Horizen, mwrap, mPoolHandleManager, 5, ScriptBinder);
        }
        endLessList.Clear();

        for (int i = 0; i < list.Count; i++)
        {
            endLessList.Append(list[i]);
        }

        endLessList.Bind();

        mScrollView.ScrollImmidate(0);
        mScrollView.SetDynamicArrowHorizontalWithWrap(mwrap.itemSize * list.Count, mobj_arrowR, mobj_arrowL);

        mobj_hint.CustomActive(curDataType == 2);
        RefreshTicket();
    }


    void RefreshTicket()
    {        
        if (curDataType == 1)
        {
            var list = CSExchangeShopInfo.Instance.serverOpenTicketList;
            if (list != null)
            {
                list.TryGetValue(curShowPage, out curTicket);
            }
        }
        else
        {
            var list = CSExchangeShopInfo.Instance.dailyTicketList;
            if (list != null)
            {
                list.TryGetValue(curShowPage, out curTicket);
            }           
        }

        if (curTicket == null) return;

        msp_ticketIcon.spriteName = $"tubiao{curTicket.id}";
        mlb_ticketNum.text = curTicket.id.GetItemCount().ToString();
    }


    void OnTabClick(int idx)
    {        
        ChangeTab(idx);
        RefreshContents();
    }


    void ChangeTab(int selectIdx)
    {
        if (tabList == null) return;

        /*if (curDataType == 1 && selectIdx < CSExchangeShopInfo.Instance.serverOpenDay) return;*/
        curShowPage = selectIdx;

        for (int i = 0; i < tabList.Count; i++)
        {
            var tabBinder = tabList[i];
            if (tabBinder.idx == curShowPage) tabBinder.SetState(1);
            else
            {
                if (curDataType == 2)
                    tabBinder.SetState(0);
                else
                {
                    tabBinder.SetState(tabBinder.idx < CSExchangeShopInfo.Instance.serverOpenDay ? 2 : 0);
                }
            }
        }
    }



    void DataChangeEvent(uint id, object param)
    {
        curDataType = CSExchangeShopInfo.Instance.CurShowDataType;
        InitTopTabs();

        RefreshUI();
    }


    void ExchangeShopRefresh(uint id, object param)
    {
        RefreshUI();
    }


    void ItemListChangeEvent(uint id, object param)
    {
        RefreshTicket();
    }



    void AddTicketBtnClick(GameObject go)
    {
        if (curTicket == null) return;
        TABLE.ITEM universal = CSExchangeShopInfo.Instance.universalTicket;
        if (universal == null) return;
        if (curTicket.id == universal.id)
        {
            Utility.ShowGetWay(curTicket.id);
        }
        else
        {
            UIManager.Instance.CreatePanel<UIFastExchangePanel>((f) =>
            {
                (f as UIFastExchangePanel).UniversalTicketExchange(universal, curTicket);
            });
        }
    }


    void TicketSpClick(GameObject go)
    {
        if (curTicket == null) return;
        UITipsManager.Instance.CreateTips(TipsOpenType.Normal, curTicket.id);
    }

}



public class UIExchangeShopItemBinder : UIBinder
{
    UISprite sp_flag;
    UILabel lb_name;
    UIGridContainer grid_rewards;
    UILabel lb_exchangeTimes;
    GameObject btn_exchange;
    GameObject obj_soldOut;
    GameObject obj_notToday;
    UILabel lb_yuanbao;
    UILabel lb_tickets;
    UISprite sp_ticket;
    UISprite sp_yuanbao;
    GameObject tex_bg;


    CSExchangeItemData mData;

    List<UIItemBase> itemList;


    public override void Init(UIEventListener handle)
    {
        sp_flag = Get<UISprite>("sp_flag");
        lb_name = Get<UILabel>("lb_name");
        grid_rewards = Get<UIGridContainer>("grid");
        lb_exchangeTimes = Get<UILabel>("lb_exchange");
        btn_exchange = Get<GameObject>("btn_exchange");
        obj_soldOut = Get<GameObject>("sp_outofdate");
        obj_notToday = Get<GameObject>("lb_hint");
        lb_yuanbao = Get<UILabel>("Money/lb_money1");
        lb_tickets = Get<UILabel>("Money/lb_money2");
        sp_ticket = Get<UISprite>("Money/lb_money2/Sprite");
        sp_yuanbao = Get<UISprite>("Money/lb_money1/Sprite");
        tex_bg = Get<GameObject>("bg");

        if (sp_ticket != null)
        {
            UIEventListener.Get(sp_ticket.gameObject).onClick = TicketIconClick;
        }
        if (sp_yuanbao != null)
        {
            UIEventListener.Get(sp_yuanbao.gameObject).onClick = YuanBaoIconClick;
        }

        HotManager.Instance.EventHandler.AddEvent(CEvent.ExchangeShopSingleChange, InfoChange);
    }

    public override void OnDestroy()
    {
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.ExchangeShopSingleChange, InfoChange);
        UIItemManager.Instance.RecycleItemsFormMediator(itemList);
        if (tex_bg != null)
        {
            CSEffectPlayMgr.Instance.Recycle(tex_bg);
        }
        sp_flag = null;
        lb_name = null;
        grid_rewards = null;
        lb_exchangeTimes = null;
        btn_exchange = null;
        obj_soldOut = null;
        obj_notToday = null;
        lb_yuanbao = null;
        lb_tickets = null;
        sp_ticket = null;
        sp_yuanbao = null;
        tex_bg = null;

        mData = null;
    }


    public override void Bind(object data)
    {
        mData = data as CSExchangeItemData;
        if (mData == null || mData.config == null) return;
        TABLE.BIQISHOP cfg = mData.config;

        if (tex_bg != null)
        {
            CSEffectPlayMgr.Instance.ShowUITexture(tex_bg, "special_shop_bg");
        }

        if (string.IsNullOrEmpty(cfg.Recommend)) sp_flag.CustomActive(false);
        else
        {
            sp_flag.CustomActive(true);
            sp_flag.spriteName = cfg.Recommend;
        }

        lb_name.text = cfg.name.ToString();

        if (cfg.frequency > 0)
        {
            lb_exchangeTimes.CustomActive(true);
            int curTimes = CSExchangeShopInfo.Instance.GetBuyTimes(cfg.id);
            lb_exchangeTimes.text = $"{curTimes}/{cfg.frequency}".BBCode(curTimes >= cfg.frequency ? ColorType.Red : ColorType.Green);
            obj_soldOut.CustomActive(curTimes >= cfg.frequency);
        }
        else
        {
            lb_exchangeTimes.CustomActive(false);
            obj_soldOut.CustomActive(false);
        }


        lb_yuanbao.text = $"{cfg.value} +".BBCode(ColorType.MainText);
        lb_tickets.text = $"{cfg.value2}".BBCode(ColorType.MainText);

        if (mData.ticket != null)
        {
            sp_ticket.spriteName = $"tubiao{mData.ticket.id}";
        }
        

        bool isToday = TodayCanExchange();
        btn_exchange.CustomActive(isToday);
        obj_notToday.CustomActive(!isToday);

        if (itemList == null)
        {
            itemList = PoolHandle.GetSystemClass<List<UIItemBase>>();
            itemList.Clear();
        }

        Utility.GetItemByBoxid(mData.RewardDic, grid_rewards, ref itemList, itemSize.Default);
        if (itemList != null)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                var item = itemList[i];
                if (item == null || item.itemCfg == null || !mData.RewardDic.ContainsKey(item.itemCfg.id)) continue;
                int count = mData.RewardDic[item.itemCfg.id];
                if (count <= 1) continue;
                item.SetCount(count.ToString(), CSColor.white);
            }
        }
        

        if (isToday)
        {
            UIEventListener.Get(btn_exchange).onClick = ExchangeBtnClick;
        }
    }


    bool TodayCanExchange()
    {
        if (mData == null || mData.config == null) return false;
        int mainType = mData.config.type;
        int subType = mData.config.subType;
        if (mainType == 1)
        {
            return CSExchangeShopInfo.Instance.serverOpenDay == subType;
        }
        else if(mainType == 2)
        {
            int day = CSExchangeShopInfo.Instance.dayOfWeek;
            return day == 0 || day == subType;
        }

        return false;
    }
    

    void ExchangeBtnClick(GameObject go)
    {
        if (mData == null || mData.config == null || mData.ticket == null) return;
        bool isToday = TodayCanExchange();
        if (!isToday) return;

        TABLE.BIQISHOP cfg = mData.config;
        TABLE.ITEM ticket = mData.ticket;

        bool moneyEnough = ((int)MoneyType.yuanbao).GetItemCount() >= cfg.value; 
        bool ticketEnough = ticket.id.GetItemCount() >= cfg.value2;

        if (moneyEnough && ticketEnough)
        {
            //判断精力
            if (mData.RewardDic.ContainsKey(12))
            {
                if (CSVigorInfo.Instance.IsVigorFull())
                {
                    UtilityTips.ShowPromptWordTips(99,
                    () =>
                    {
                        UtilityPanel.JumpToPanel(12605);
                        UIManager.Instance.ClosePanel<UISpecialShopCombinePanel>();
                    },
                    () =>
                    {
                        UtilityPath.FindWithDeliverId(DeliverTableManager.Instance.GetSuggestDeliverId(CSMainPlayerInfo.Instance.Level));
                        UIManager.Instance.ClosePanel<UISpecialShopCombinePanel>();
                    });
                    return;
                }
            }

            Net.CSDuiHuanItemMessage(cfg.id);
        }
        else
        {
            if (!moneyEnough)
            {
                //YuanBaoNotEnough();
                Utility.ShowGetWay(3);
            }
            else if (!ticketEnough)
            {
                TABLE.ITEM universal = CSExchangeShopInfo.Instance.universalTicket;
                if (universal == null) return;
                if (ticket.id == universal.id)
                {
                    Utility.ShowGetWay(universal.id);
                }
                else
                {
                    UIManager.Instance.CreatePanel<UIFastExchangePanel>((f) =>
                    {
                        (f as UIFastExchangePanel).UniversalTicketExchange(universal, ticket);
                    });
                }
            }
        }
    }


    //元宝不足处理
    void YuanBaoNotEnough()
    {
        if (!CSVipInfo.Instance.IsFirstRecharge())
        {
            UtilityTips.ShowPromptWordTips(5, () =>
            {
                UIManager.Instance.CreatePanel<UIRechargeFirstPanel>();
                UIManager.Instance.ClosePanel<UISpecialShopCombinePanel>();
            });
            return;
        }

        UtilityTips.ShowPromptWordTips(6, () =>
        {
            UtilityPanel.JumpToPanel(12305);
            UIManager.Instance.ClosePanel<UISpecialShopCombinePanel>();
        });
    }


    void TicketIconClick(GameObject go)
    {
        if (mData != null && mData.ticket != null)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, mData.ticket.id);
        }
    }

    void YuanBaoIconClick(GameObject go)
    {
        UITipsManager.Instance.CreateTips(TipsOpenType.Normal, 3);
    }


    void InfoChange(uint id, object data)
    {
        if (mData == null || mData.config == null) return;
        var cfg = mData.config;
        if (cfg.frequency > 0 && lb_exchangeTimes != null)
        {
            lb_exchangeTimes.CustomActive(true);
            int curTimes = CSExchangeShopInfo.Instance.GetBuyTimes(cfg.id);
            lb_exchangeTimes.text = $"{curTimes}/{cfg.frequency}".BBCode(curTimes >= cfg.frequency ? ColorType.Red : ColorType.Green);
            obj_soldOut?.CustomActive(curTimes >= cfg.frequency);
        }
    }
}



public class UIShopTopTabBinder : UIBinder
{
    UILabel lb_textA;
    UILabel lb_textB;
    GameObject obj_check;
    UISprite sp_back;

    int curState; //0未选中，1选中，2不可点击

    public int idx;

    Action<int> clickAction;


    public override void Init(UIEventListener handle)
    {
        lb_textA = Get<UILabel>("Background/Label");
        lb_textB = Get<UILabel>("Checkmark/Label");
        obj_check = Get<GameObject>("Checkmark");
        sp_back = Get<UISprite>("Background");
    }

    public override void OnDestroy()
    {
        clickAction = null;
        lb_textA = null;
        lb_textB = null;
        obj_check = null;
        sp_back = null;
    }


    public override void Bind(object data)
    {
        idx = Convert.ToInt32(data);

        int mainType = CSExchangeShopInfo.Instance.CurShowDataType;

        string textStr = "";
        if (mainType == 1)
        {
            string str = CSExchangeShopInfo.Instance.GetServerOpenDayStr();
            textStr = CSString.Format(str, idx);
        }
        else if (mainType == 2)
        {
            textStr = CSExchangeShopInfo.Instance.GetDayOfWeekStr(idx);
        }

        lb_textA.text = textStr;
        lb_textB.text = textStr;

        clickAction = null;
        if (Handle != null)
        {
            Handle.onClick = OnTabClick;
        }
    }


    public void SetClick(Action<int> _action)
    {
        if (_action != null)
            clickAction = _action;
    }


    public void SetState(int state)
    {
        obj_check.CustomActive(state == 1);
        //sp_back.color = state == 2 ? Color.black : CSColor.white;
        sp_back.spriteName = state == 2 ? "btn_special_shop" : "tab_big2";

        curState = state;
    }


    void OnTabClick(GameObject go)
    {  
        if (curState == 0 || curState == 2)
        {
            if (clickAction == null) return;
            clickAction(idx);
        }
        //else if (curState == 2)
        //{
        //    UtilityTips.ShowRedTips(1836);
        //}
    }

}