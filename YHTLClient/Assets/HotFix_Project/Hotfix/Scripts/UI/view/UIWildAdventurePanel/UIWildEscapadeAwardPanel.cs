using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UIWildEscapadeAwardPanel : UIBasePanel
{

    int mCurIndex = 1;
    const int MaxPage = 5;
    const int MaxCountPerPage = 20;

    List<UIItemBase> itemList;

    int syncTime;
    int time;
    int timeLimit;

    Schedule schRepeat;

    public override void Init()
	{
		base.Init();
        AddCollider();

        //msp_moneyIcon.spriteName = $"tubiao{ItemTableManager.Instance.GetItemCfg((int)MoneyType.gold).icon}";

        mClientEvent.Reg((uint)CEvent.WildAdventureInfoChange, AdventureInfoChange);
        mClientEvent.Reg((uint)CEvent.GetPetStateInfo, PetStateInfoChange);//Õ½³è×´Ì¬±ä¸ü

        mScrollView.onDragFinished = OnDragEnd;

        mbtn_draw.onClick = TakeOutRewardsClick;
    }
	
	public override void Show()
	{
		base.Show();

        InitItemBase();
        RefreshAllUI();

        Net.CSWildAdventrueMessage();
    }
	
	protected override void OnDestroy()
	{
        Timer.Instance.CancelInvoke(schRepeat);
        UIItemManager.Instance.RecycleItemsFormMediator(itemList);

		base.OnDestroy();
	}


    void RefreshAllUI()
    {
        RefreshTopUI();
        RefreshStoreHouse();
    }


    void RefreshTopUI()
    {
        mlb_exp.text = CSWildAdventureInfo.Instance.hasExp.ToString().BBCode(syncTime < timeLimit ? ColorType.Green : ColorType.Red);
        mlb_money.text = CSWildAdventureInfo.Instance.hasMoney.ToString().BBCode(syncTime < timeLimit ? ColorType.Green : ColorType.Red);

        RefreshTime(null);
    }


    void RefreshTime(Schedule sch)
    {
        if (time < timeLimit) time++;
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt((time - (hours*3600)) / 60);
        int seconds = time % 60;
        string sUnit = ClientTipsTableManager.Instance.GetClientTipsContext(423);
        string mUnit = ClientTipsTableManager.Instance.GetClientTipsContext(420);
        string hUnit = ClientTipsTableManager.Instance.GetClientTipsContext(419);

        mlb_time.text = $"{hours}{hUnit}{minutes}{mUnit}{seconds}{sUnit}".BBCode(ColorType.Green);
    }


    void InitItemBase()
    {
        UIItemManager.Instance.RecycleItemsFormMediator(itemList);
        itemList = UIItemManager.Instance.GetUIItems(MaxCountPerPage, PropItemType.Normal, mGird.transform);
        if (itemList == null) return;
        for (int i = 0; i < itemList.Count; i++)
        {
            UIEventListener.Get(itemList[i].obj).onDrag = OnDragItem;
        }

        mGird.Reposition();
    }


    void RefreshStoreHouse()
    {
        if (itemList == null || mCurIndex < 1 || mCurIndex > MaxPage) return;

        CSBetterLisHot<bag.BagItemInfo> rewards = CSWildAdventureInfo.Instance.GetRewardsList();
        int leftCount = (mCurIndex - 1) * MaxCountPerPage;
        for (int i = 0; i < itemList.Count; i++)
        {
            if ((i+leftCount) < rewards.Count)
            {
                itemList[i].Refresh(rewards[i + leftCount], ItemClick);
            }
            else
            {
                itemList[i].UnInit();
            }
        }

        mlb_page.text = $"< {mCurIndex}/{MaxPage} >";
    }



    void AdventureInfoChange(uint id, object data)
    {
        syncTime = CSWildAdventureInfo.Instance.time;
        timeLimit = CSWildAdventureInfo.Instance.timeLimit;
        time = syncTime;
        
        RefreshAllUI();

        if (!Timer.Instance.IsInvoking(schRepeat))
            schRepeat = Timer.Instance.InvokeRepeating(1, 1, RefreshTime);
    }
    

    void PetStateInfoChange(uint id, object data)
    {
        int lv = CSWoLongInfo.Instance.ReturnZhanHunSuitId();
        if (lv < 1) UIManager.Instance.ClosePanel<UIWildEscapadeAwardPanel>();
    }



    float temp = 0;
    private void OnDragItem(GameObject go, Vector2 delat)
    {
        temp += delat.x;
    }

    private void OnDragEnd()
    {
        if (temp > 50)
        {
            OnLeftClick();
        }
        else if (temp < -50)
        {
            OnRightClick();
        }

        temp = 0;
        mScrollView.ScrollImmidate(0);
        RefreshStoreHouse();
    }

    private void OnLeftClick(GameObject go = null)
    {
        if (mCurIndex == 1) return;
        mCurIndex--;
    }

    private void OnRightClick(GameObject go = null)
    {
        if (mCurIndex == MaxPage) return;
        mCurIndex++;
    }

    void ItemClick(UIItemBase item)
    {
        if (item != null)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.WildAdventureRewardReceive, item.itemCfg, item.infos);
        }
    }



    void TakeOutRewardsClick(GameObject go)
    {
        CSWildAdventureInfo.Instance.TryToTakeOutItem(-1);
    }

}
