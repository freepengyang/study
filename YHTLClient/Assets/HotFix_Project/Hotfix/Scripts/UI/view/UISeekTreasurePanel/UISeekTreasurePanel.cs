using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Text.RegularExpressions;
using UnityEngine;

public partial class UISeekTreasurePanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get => false;
    }

    private const int MaxCount = 8;
    private int seekConfigId = 0;
    private long minSeekCount = 0;

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.SeekTreasureBox, SeekTreasureBox);
        mClientEvent.Reg((uint) CEvent.SeekTreasureHistory, SeekTreasureHistory);
        mClientEvent.Reg((uint) CEvent.ItemListChange, ItemListChange);

        mbtn_seek1.onClick = OnClickSeek1;
        mbtn_seek10.onClick = OnClickSeek10;
        mbtn_seek20.onClick = OnClickSeek20;
        // mbtn_recharge.onClick = OnClickRecharge;
        mbtn_add_cost.onClick = OnClickCost;
        mbtn_me.onClick = OnClickMe;
        mbtn_all.onClick = OnClickAll;
        mbtn_gift.onClick = OnClickGift;
        mbtn_propsIcon.onClick = OnClickPropsIcon;
        mbtn_close_bubble.onClick = OnClickCloseBubble;
        mbubble.SetActive(CSSeekTreasureInfo.Instance.IsOpenBubble);
        seekConfigId = CSSeekTreasureInfo.Instance.seekConfigId;
        minSeekCount = CSSeekTreasureInfo.Instance.seekCost;

    }
    
    void OnClickCloseBubble(GameObject go)
    {
        mbubble.SetActive(false);
        CSSeekTreasureInfo.Instance.IsOpenBubble = false;
    }

    void SeekTreasureBox(uint id, object data)
    {
        SetCurrency();
        InitGridAwardList();
        SetButtonsAndStarTime();
    }

    void SeekTreasureHistory(uint id, object data)
    {
        RefreshRecordData();
    }
    
    /// <summary>
    /// 寻宝券变化
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void ItemListChange(uint id, object data)
    {
        mlb_props.text = CSBagInfo.Instance.GetAllItemCount(seekConfigId).ToString();
    }

    public override void Show()
    {
        base.Show();
        CSEffectPlayMgr.Instance.ShowUITexture(mtreasure_bg2, "treasure_bg2");
        CSEffectPlayMgr.Instance.ShowUITexture(mtreasure_line, "treasure_line");
        CSEffectPlayMgr.Instance.ShowUIEffect(meffect_seek_treasure_idle_add, "effect_seek_treasure_idle_add");
        Net.ReqTreasureIdMessage();
        Net.ReqGetTreasureHuntMessage();
    }

    /// <summary>
    /// 刷新记录信息
    /// </summary>
    void RefreshRecordData()
    {
        SetCurrency();
        SetGridMyrecord();
        SetGridAllrecord();
    }


    private long starTime = 0;
    /// <summary>
    /// 设置寻宝按钮和下次开始时间
    /// </summary>
    void SetButtonsAndStarTime()
    {
        if (CSSeekTreasureInfo.Instance.StarTime>0)
        {
            mGift.SetActive(false);
            mButtons.SetActive(false);
            mHints.SetActive(true);
            starTime = CSSeekTreasureInfo.Instance.StarTime/1000 - CSServerTime.Instance.TotalSeconds;
            ScriptBinder.InvokeRepeating(0f, 1f, StarTime);
        }
        else
        {
            mGift.SetActive(true);
            mButtons.SetActive(true);
            mHints.SetActive(false);
            ScriptBinder.StopInvoke();
        }
    }

    void StarTime()
    {
        if (starTime>=0)
        {
            mlb_starTime.text = CSServerTime.Instance.FormatLongToTimeStr(starTime, 3);
            starTime--;
        }
    }

    /// <summary>
    /// 设置寻宝券和积分
    /// </summary>
    void SetCurrency()
    {
        msp_propsIcon.spriteName = $"tubiao{ItemTableManager.Instance.GetItemIcon(seekConfigId)}";
        mlb_props.text = CSBagInfo.Instance.GetAllItemCount(seekConfigId).ToString();
        mlb_integral.text = CSItemCountManager.Instance.GetItemCount((int)MoneyType.xunbaojifen).ToString();
    }

    List<UIItemBase> listItemBases = new List<UIItemBase>();

    /// <summary>
    /// 初始化奖励池
    /// </summary>
    void InitGridAwardList()
    {
        mgrid_awardList1.MaxCount = MaxCount;
        mgrid_awardList2.MaxCount = MaxCount;
        List<int> treasureChests = CSSeekTreasureInfo.Instance.GetTreasureChests();
        if (treasureChests.Count != 17)
        {
            // Debug.Log("------------------宝箱数量配置错误@吕惠铭");
            return;
        }

        GameObject gp;
        UIItemBase itemBase;
        for (int i = 0; i < MaxCount; i++)
        {
            gp = mgrid_awardList1.controlList[i];
            if (listItemBases.Count <= i)
                listItemBases.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, gp.transform));
            itemBase = listItemBases[i];
            TABLE.ITEM cfg;
            if (ItemTableManager.Instance.TryGetValue(treasureChests[i], out cfg))
            {
                itemBase.Refresh(cfg);
            }
        }

        for (int i = 0; i < MaxCount; i++)
        {
            gp = mgrid_awardList2.controlList[i];
            if (listItemBases.Count <= i + 8)
                listItemBases.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, gp.transform));
            itemBase = listItemBases[i + 8];
            TABLE.ITEM cfg;
            if (ItemTableManager.Instance.TryGetValue(treasureChests[i + 8], out cfg))
            {
                itemBase.Refresh(cfg);
            }
        }

        if (listItemBases.Count<=16)
            listItemBases.Add(new UIItemBase(mawardMiddle, PropItemType.Normal));
        
        itemBase = listItemBases[16];
        TABLE.ITEM cfgMiddle;
        if (ItemTableManager.Instance.TryGetValue(treasureChests[16], out cfgMiddle))
        {
            itemBase.Refresh(cfgMiddle);
        }
    }

    /// <summary>
    /// 设置我的记录列表
    /// </summary>
    void SetGridMyrecord()
    {
        List<string> myHistory = CSSeekTreasureInfo.Instance.MySeekWreasureHistory;
        //添加数据
        MatchCollection matchs = null;
        // string pattern = "\\[item:(\\d+)\\]";
        string pattern = "item:\\d+";
        mgrid_myrecord.MaxCount = myHistory.Count;
        UILabel lb_content;
        for (int i = 0; i < mgrid_myrecord.MaxCount; i++)
        {
            lb_content = mgrid_myrecord.controlList[i].GetComponent<UILabel>();

            matchs = Regex.Matches(myHistory[i], pattern);
            int itemid = 0;
            TABLE.ITEM item = null;
            if (matchs.Count > 0)
            {
                int star = 5;
                int lenght = matchs[0].Value.Length - 5;
                itemid = int.Parse(matchs[0].Value.Substring(star, lenght));

                if (ItemTableManager.Instance.TryGetValue(itemid, out item))
                {
                    CSStringBuilder.Clear();
                    lb_content.text = CSStringBuilder.Append(UtilityColor.GetColorString(ColorType.MainText),
                        myHistory[i].Replace(matchs[0].Value, item.name)).ToString();
                }
            }

            UIEventListener.Get(lb_content.gameObject, item).onClick = OnClickLookItemInfo;
        }

        //UI表现
        CSGame.Sington.StartCoroutine(SetScrollValue(1));
    }

    /// <summary>
    /// 设置全部记录列表
    /// </summary>
    void SetGridAllrecord()
    {
        List<string> allHistory = CSSeekTreasureInfo.Instance.AllSeekWreasureHistory;
        //添加数据
        MatchCollection matchs = null;
        // string pattern = "\\[item:(\\d+)\\]";
        string pattern = "item:\\d+";
        mgrid_allrecord.MaxCount = allHistory.Count;
        UILabel lb_content;
        for (int i = 0; i < mgrid_allrecord.MaxCount; i++)
        {
            lb_content = mgrid_allrecord.controlList[i].GetComponent<UILabel>();

            matchs = Regex.Matches(allHistory[i], pattern);
            int itemid = 0;
            TABLE.ITEM item = null;
            if (matchs.Count > 0)
            {
                int star = 5;
                int lenght = matchs[0].Value.Length - 5;
                itemid = int.Parse(matchs[0].Value.Substring(star, lenght));

                if (ItemTableManager.Instance.TryGetValue(itemid, out item))
                {
                    CSStringBuilder.Clear();
                    lb_content.text = CSStringBuilder.Append(UtilityColor.GetColorString(ColorType.MainText),
                        allHistory[i].Replace(matchs[0].Value, item.name)).ToString();
                }
            }

            UIEventListener.Get(lb_content.gameObject, item).onClick = OnClickLookItemInfo;
        }

        //UI表现
        CSGame.Sington.StartCoroutine(SetScrollValue(1));
    }

    IEnumerator SetScrollValue(float value)
    {
        yield return null;
        mScrollViewMyrecord.ResetPosition();
        mScrollViewAllrecord.ResetPosition();

        if (mScrollViewMyrecord.GetComponent<UIPanel>().height >=
            mgrid_myrecord.CellHeight * mgrid_myrecord.controlList.Count)
        {
            mScrollViewMyrecord.verticalScrollBar.value = 0;
        }
        else
        {
            mScrollViewMyrecord.verticalScrollBar.value = value;
        }

        if (mScrollViewAllrecord.GetComponent<UIPanel>().height >=
            mgrid_allrecord.CellHeight * mgrid_allrecord.controlList.Count)
        {
            mScrollViewAllrecord.verticalScrollBar.value = 0;
        }
        else
        {
            mScrollViewAllrecord.verticalScrollBar.value = value;
        }
    }

    private void OnClickLookItemInfo(GameObject go)
    {
        TABLE.ITEM item = go.GetComponent<UIEventListener>().parameter as TABLE.ITEM;
        if (item == null) return;
        CSSeekTreasureInfo.Instance.ShowItemTip(item.id);
    }

    /// <summary>
    /// 不处理
    /// </summary>
    /// <param name="go"></param>
    void OnClickMe(GameObject go)
    {
        if (go == null) return;
    }

    /// <summary>
    /// 不处理
    /// </summary>
    /// <param name="go"></param>
    void OnClickAll(GameObject go)
    {
        if (go == null) return;
    }


    void OnClickGift(GameObject go)
    {
        if (go == null) return;
        UtilityPanel.JumpToPanel(12612);
        // UIManager.Instance.ClosePanel<UISeekTreasureCombinedPanel>();
    }

    // /// <summary>
    // /// 点击充值
    // /// </summary>
    // /// <param name="go"></param>
    // void OnClickRecharge(GameObject go)
    // {
    //     if (go == null) return;
    //     // UIManager.Instance.CreatePanel<UIShopPanel>();
    //     UtilityPanel.JumpToPanel(12305);
    //     UIManager.Instance.ClosePanel<UISeekTreasureCombinedPanel>();
    // }

    /// <summary>
    /// 消耗Getway
    /// </summary>
    /// <param name="go"></param>
    void OnClickCost(GameObject go)
    {
        if (go == null) return;
        Utility.ShowGetWay(seekConfigId);
    }

    /// <summary>
    /// 寻宝1次
    /// </summary>
    /// <param name="go"></param>
    void OnClickSeek1(GameObject go)
    {
        if (go == null) return;
        ReqSeek(1);
    }

    /// <summary>
    /// 寻宝10次
    /// </summary>
    /// <param name="go"></param>
    void OnClickSeek10(GameObject go)
    {
        if (go == null) return;
        ReqSeek(2);
    }

    /// <summary>
    /// 寻宝20次
    /// </summary>
    /// <param name="go"></param>
    void OnClickSeek20(GameObject go)
    {
        if (go == null) return;
        ReqSeek(3);
    }

    void ReqSeek(int type)
    {
        if (IsEnoughYuanBao(type))
        {
            Net.ReqTreasureHuntMessage(type);
            CSEffectPlayMgr.Instance.ShowUIEffect(meffect_seek_treasure_point_add, "effect_seek_treasure_point_add",
                _loop: false);
        }
        else
        {
            // if (CSVipInfo.Instance.IsFirstRecharge()) //充值
            // {
            //     UtilityTips.ShowPromptWordTips(6, () =>
            //     {
            //         UtilityPanel.JumpToPanel(12305);
            //         UIManager.Instance.ClosePanel<UISeekTreasureCombinedPanel>();
            //     });
            // }
            // else //活动
            // {
            //     UtilityTips.ShowPromptWordTips(5, OpenUIRechargeFirstPanel);
            // }
            
            UtilityTips.ShowPromptWordTips(91, () =>
            {
                UtilityPanel.JumpToPanel(12612);
                // UIManager.Instance.ClosePanel<UISeekTreasureCombinedPanel>();
            });
        }
    }

    // void OpenUIRechargeFirstPanel()
    // {
    //     UIManager.Instance.CreatePanel<UIRechargeFirstPanel>();
    //     UIManager.Instance.ClosePanel<UISeekTreasureCombinedPanel>();
    // }

    bool IsEnoughYuanBao(int type)
    {
        long curCount = CSBagInfo.Instance.GetAllItemCount(seekConfigId);
        switch (type)
        {
            case 1:
                return curCount >= minSeekCount;
            case 2:
                return curCount >= minSeekCount*10;
            case 3:
                return curCount >= minSeekCount*20;
        }

        return false;
    }
    
    
    void OnClickPropsIcon(GameObject go)
    {
        UITipsManager.Instance.CreateTips(TipsOpenType.Normal, seekConfigId);
    }

    public override void OnHide()
    {
        ScriptBinder.StopInvoke();
        base.OnHide();
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mtreasure_bg2);
        CSEffectPlayMgr.Instance.Recycle(mtreasure_line);
        CSEffectPlayMgr.Instance.Recycle(meffect_seek_treasure_idle_add);
        CSEffectPlayMgr.Instance.Recycle(meffect_seek_treasure_point_add);
        UIItemManager.Instance.RecycleItemsFormMediator(listItemBases);
        base.OnDestroy();
    }
}