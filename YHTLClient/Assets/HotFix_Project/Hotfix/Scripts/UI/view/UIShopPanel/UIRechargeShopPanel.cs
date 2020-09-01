using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public partial class UIRechargeShopPanel : UIBasePanel
{
    
    private Dictionary<int, FastArrayElementFromPool<FastArrayElementKeepHandle<CSShopCommodityData>>> dicPages;//分页数据
    private ILBetterList<btnClass> btnClasses;
    float oldScrollValue;
    int maxShopPage;
    int curPage;
    int curSubType;
    string[] btninfo;
    private int dailyRmb;
    
    float FlipPageNeededValue { get { return 0.05f; } }


    public override void Init()
	{
		base.Init();
        mClientEvent.AddEvent(CEvent.ShopBuyTimesChange, OnBuyTimesChange);
        mClientEvent.AddEvent(CEvent.ShopInfoChange, OnBuyTimesChange);
        mClientEvent.AddEvent(CEvent.DailyRmbChange,RefreshRechargeNum);
        mClientEvent.AddEvent(CEvent.ItemListChange,RefreshMoney);
        mClientEvent.AddEvent(CEvent.PrompClose,OnClose);
        InitLists();
        mscroll.onDragFinished += OnDragFinished;
        curSubType = 1;     
        UIEventListener.Get(mlb_hint).onClick = OnClickHint;
        

    }
    public override void Show()
	{
    	base.Show();
        Net.CSDailyRmbInfoMessage();

        CSShopInfo.Instance.RechargeRed = false;
        
        //刷新下方item
        int index = (int) MoneyType.yuanbao;
        msp_icon.spriteName = $"tubiao{index}";
        mlb_value.text = index.GetItemCount().ToString();
        UIEventListener.Get(mbtn_add).onClick = (a) =>
        {
            Utility.ShowGetWay(index);
        };
    }

    public override void SelectChildPanel(int type = 1)
    {
        RefreshPages(type,true);
    }

    private void OnClickHint(GameObject obj)
    {
        UtilityPanel.JumpToPanel(12305);
        //Utility
    }

    //刷新玩家身上元宝
    private void RefreshMoney(uint id = 0, object data = null)
    {
        int index = (int) MoneyType.yuanbao;
        mlb_value.text = index.GetItemCount().ToString();
    }

    //刷新今日充值数量
    private void RefreshRechargeNum(uint id = 0, object data = null)
    {
        dailyRmb = CSShopInfo.Instance.dailyRmb;
        mlb_DayNum.text = dailyRmb.ToString();
        RefreshPages(curSubType, true);
    }

    protected override void OnDestroy()
	{
        mgrid_Item.UnBind<UIRechargeShopPage>();
        mPoolHandleManager.Recycle(dicPages);
        mPoolHandleManager.Recycle(btnClasses);
        mClientEvent.RemoveEvent(CEvent.PrompClose);
		base.OnDestroy();
	}

    void InitLists()
    {
        btninfo = SundryTableManager.Instance.GetSundryEffect(736).Split('#');
        mToggle.MaxCount = btninfo.Length;
        btnClasses = mPoolHandleManager.GetSystemClass<ILBetterList<btnClass>>();
        btnClasses.Clear();
        if (dicPages == null)
        {
            dicPages = mPoolHandleManager.GetSystemClass<Dictionary<int, 
                FastArrayElementFromPool<FastArrayElementKeepHandle<CSShopCommodityData>>>>();

            dicPages.Clear();
        }

        for (int i = 0; i <  mToggle.MaxCount; i++)
        {
            GameObject obj = mToggle.controlList[i];
            //显示按钮名称
            var btnclass = mPoolHandleManager.GetSystemClass<btnClass>();
            
            btnclass.Init(obj.transform,btninfo[i]);
            btnClasses.Add(btnclass);
            //注册事件
            UIEventListener.Get(obj,i+1).onClick = OnClickBtn;
            dicPages[i+1] = new FastArrayElementFromPool<FastArrayElementKeepHandle<CSShopCommodityData>>(16,Get, Put);
            AddShopElementsToArray(i+1);
        }   
    }

    private void OnClickBtn(GameObject obj)
    {
        int index = (int)UIEventListener.Get(obj).parameter;

        int curmoney;

        int.TryParse(btninfo[index - 1], out curmoney);
        mlb_hint.SetActive(CSShopInfo.Instance.dailyRmb < curmoney);
     
        RefreshPages(index,true);
    }


    void AddShopElementsToArray(int dicKey)
    {
        var dic = CSShopInfo.Instance.RechargeShopInfo;
        if (dic == null) return;
        if (dic.ContainsKey(dicKey))
        {
            var list = dic[dicKey];
            if (list == null || list.Count < 1) return;

            int pageIndex = 0;
            var array = dicPages[dicKey];
            
            for (int i = 0; i < list.Count; i++)
            {
                FastArrayElementKeepHandle<CSShopCommodityData> pageList = null;
                if (array.Count <= pageIndex)
                {
                    pageList = array.Append();
                }
                else pageList = array[pageIndex];
                if (pageList == null) continue;
                if (pageList.Count >= 12)
                {
                    pageIndex++;
                    i--;
                    continue;
                }
                pageList.Append(list[i]);
            }
        }
    }

    void RefreshPages(int index, bool needReset = false)
    {
        curSubType = index;
        maxShopPage = 0;

        for (int i = 0; i < btnClasses.Count; i++)
        {
            btnClasses[i].SelectType(index == i+1,dailyRmb);
        }
        
        if (dicPages.ContainsKey(index))
        {
            mgrid_Item.Bind<FastArrayElementKeepHandle<CSShopCommodityData>, UIRechargeShopPage>(dicPages[index],mPoolHandleManager);
            maxShopPage = dicPages[index].Count;
        }

        if (maxShopPage < 1) return;

        if (needReset)
        {
            curPage = 1;
            mscroll.ScrollImmidate(0);
            oldScrollValue = /*mscroll.horizontalScrollBar.value*/0;
        }

        RefreshPageDots(curPage);
    }

    void RefreshPageDots(int _curPage)
    {
        if (maxShopPage < 2)
        {
            mgrid_pageDot.MaxCount = 0;
            return;
        }

        curPage = _curPage;

        mgrid_pageDot.MaxCount = maxShopPage;
        for (int i = 0; i < mgrid_pageDot.MaxCount; i++)
        {
            var selected = mgrid_pageDot.controlList[i].transform.GetChild(1);
            selected.gameObject.SetActive(i + 1 == curPage);
        }
    }

    void OnDragFinished()
    {
        float value = mscrollBar.value;
        if (maxShopPage < 1 || Mathf.Abs(value - oldScrollValue) < FlipPageNeededValue)
        {
            ScrollDoReset();
            return;
        }
        bool toLeft = value < oldScrollValue;
        if (toLeft && curPage > 1)
        {
            curPage--;
            ScrollDoCenter();
        }
        else if (!toLeft && curPage < maxShopPage)
        {
            curPage++;
            ScrollDoCenter();
        }
        else
        {
            ScrollDoReset();
            return;
        }

        RefreshPageDots(curPage);
    }

    void ScrollDoCenter()
    {
        float newValue = (float)(curPage - 1) / (float)(maxShopPage - 1);
        oldScrollValue = newValue;
        TweenProgressBar.Begin(mscrollBar, 0.2f, mscrollBar.value, newValue);
    }

    void ScrollDoReset()
    {
        TweenProgressBar.Begin(mscrollBar, 0.2f, mscrollBar.value, oldScrollValue);
    }

    public FastArrayElementKeepHandle<CSShopCommodityData> Get()
    {
        var element = mPoolHandleManager.GetSystemClass<FastArrayElementKeepHandle<CSShopCommodityData>>();
        element.Clear();
        return element;
    }
    public void Put(FastArrayElementKeepHandle<CSShopCommodityData> element)
    {
        if(null != element)
        {
            element.Clear();
            mPoolHandleManager.Recycle(element);
        }
    }

    private void OnClose(uint uievtid, object data)
    {
        UIManager.Instance.ClosePanel<UISpecialShopCombinePanel>();
    }
    
    
    #region CEvents
    void OnBuyTimesChange(uint id, object data)
    {
        RefreshPages(curSubType);
    }

    #endregion
}

// public enum RechargeShopBtnType
// {
//     SELECT = 0, 
//     UNSELECT,
//     UNReach, //未达成
// }
public class UIRechargeShopPage : UIBinder
{

    UIGridContainer grid;

    public override void Init(UIEventListener handle)
    {
        grid = handle.GetComponent<UIGridContainer>();
    }
    public override void Bind(object data)
    {
        FastArrayElementKeepHandle<CSShopCommodityData> list = data as FastArrayElementKeepHandle<CSShopCommodityData>;
        if (list == null || list.Count < 1 || list.Count > 12)
        {
            FNDebug.LogError("@@@@商品页列表错误");
            return;
        }

        if (grid != null)
        {
            grid.Bind<CSShopCommodityData, RechargeShopItem>(list, PoolHandle);
        }
    }

    public override void OnDestroy()
    {
        if (grid != null)
        {
            grid.UnBind<RechargeShopItem>();
        }
        grid = null;
    }
}




public class RechargeShopItem : UIShopItem
{
    
    public override void QuickBuyBtnClick(GameObject _go)
    {
        if (itemBase.itemCfg == null || mData == null || mData.config == null) return;
        //var info = SundryTableManager.Instance.GetSundryEffect(736).Split('#');
        int curMoney = 0;
        int subType = mData.config.subType;

        curMoney = CSShopInfo.Instance.GetShopMoney(subType - 1);
        
        // if (info.Length > subType)
        // {
        //     int.TryParse(info[subType-1], out curMoney);
        // }

        if (CSShopInfo.Instance.dailyRmb < curMoney)
        {
            
            UtilityTips.ShowPromptWordTips(92, () =>
            {
                UtilityPanel.JumpToPanel(12305);
            });
            return;
        }
        base.QuickBuyBtnClick(_go);
    }

   
}




public class btnClass
{
    public UILabel bgName;
    public UILabel maskName;
    public UISprite bg;
    public Transform checkmark;
    public UIButton uiButton;
    private int Money;
    public Transform sp_unreach;
    
    
    

    public void Init(Transform trans,string money)
    {
        int temp;
        int.TryParse(money, out temp);
        Money = temp;
        checkmark = UtilityObj.Get<Transform>(trans, "Checkmark");
        bg = UtilityObj.Get<UISprite>(trans, "Background");
        bgName = UtilityObj.Get<UILabel>(bg.transform, "Label");
        maskName = UtilityObj.Get<UILabel>(checkmark, "Label");
        sp_unreach = UtilityObj.Get<Transform>(trans, "sp_unreach");
        uiButton = trans.GetComponent<UIButton>();
        string str = CSString.Format(1780, money);
        bgName.text = str;
        maskName.text = str;
    }

    public void SelectType(bool isSelect,int curMoney)
    {
        checkmark.gameObject.SetActive(isSelect);
        //uiButton.defaultColor = curMoney < Money ? Color.black : Color.white;
        sp_unreach.gameObject.SetActive(curMoney < Money);
        
        //bg.color = curMoney < Money ? Color.black : Color.white;
    }


}
