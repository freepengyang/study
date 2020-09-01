using System.Collections.Generic;
using UnityEngine;
public partial class UIServerActivityBossPanel : UIBasePanel
{
	int activityId = 10120;
	long leftTime = 0;
	string costicon = "";
	CSBetterLisHot<BossKuangHuanData> productDataList;
	List<UIItemBase> itemList = new List<UIItemBase>();
	public override void Init()
	{
		base.Init();
		mBtnMove.onClick = OnBtnMove;
		mBtnHelp.onClick = OnBtnHelp;
		costicon = CSBossKuangHuanInfo.Instance.GetCostIcon();
		mClientEvent.AddEvent(CEvent.GetBossKuangHuanUpdateInfo, GetBossKuangHuanUpdateInfo);
		mClientEvent.AddEvent(CEvent.ItemListChange, RefreshIntegralTicketNum);
	}
	public override void Show()
	{
		base.Show();
		RefreshTime();
		RefreshIntegralTicketNum(0,null);
		CSEffectPlayMgr.Instance.ShowUITexture(mBG, "banner11");
	}
	private void GetBossKuangHuanUpdateInfo(uint id, object data)
	{
		RefreshUI();
	}
	private void RefreshUI()
	{
		productDataList = CSBossKuangHuanInfo.Instance.GetBossKuangHuanList();
		UISprite spIcon = mUIItemBarPrefab.transform.GetChild(0).GetComponent<UISprite>();
		spIcon.spriteName = costicon;
		mGrid.MaxCount = productDataList.Count;
		for (int i=0;i<mGrid.MaxCount;i++)
		{
			BossKuangHuanItem item = mPoolHandleManager.GetCustomClass<BossKuangHuanItem>();
			item.SetData(mGrid.controlList[i], productDataList[i],costicon,i);
			item.Refresh();
		}
		mScrollBar.onChange.Add(new EventDelegate(OnChange));
	}
	private void OnChange()
	{
		mDownIcon.SetActive(mScrollBar.value <= 0.95);
	}
	private void RefreshIntegralTicketNum(uint id, object data)
	{
		long costHaveNum = CSBossKuangHuanInfo.Instance.GetBossKuangHuanIntegralTicketNum();
		UILabel lbValue = mUIItemBarPrefab.transform.GetChild(1).GetComponent<UILabel>();
		lbValue.text = costHaveNum.ToString();
	}
	private void OnBtnMove(GameObject _go)
	{
		UtilityPanel.JumpToPanel(10420);
		UIManager.Instance.ClosePanel<UIServerActivityPanel>();
	}
	private void OnBtnHelp(GameObject _go)
	{
		UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.Help_BossKuangHuan);
	}
	private void RefreshTime()
	{
		leftTime = UIServerActivityPanel.GetEndTime(activityId);
		mCSInvoke?.InvokeRepeating(0f, 1f, CountDown);
	}
	private void CountDown()
	{
		if(leftTime <= 0)
		{
			mLbTime.text = CSString.Format(1108, CSServerTime.Instance.FormatLongToTimeStr(0, 1));
			mCSInvoke?.StopInvokeRepeating();
		}
		else
		{
			mLbTime.text = CSString.Format(1108, CSServerTime.Instance.FormatLongToTimeStr(leftTime, 1));
			leftTime--;
		}
		
	}
	public override void OnHide()
	{
		base.OnHide();
	}
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if(mClientEvent != null)
		{
			mClientEvent.RemoveEvent(CEvent.GetBossKuangHuanUpdateInfo, GetBossKuangHuanUpdateInfo);
			mClientEvent.RemoveEvent(CEvent.ItemListChange, RefreshIntegralTicketNum);
		}
		CSEffectPlayMgr.Instance.Recycle(mBG);
		itemList.Clear();
		mCSInvoke?.StopInvokeRepeating();
		itemList = null;

		activityId = 0;
		leftTime = 0;

		costicon = "";
	}
}

public class BossKuangHuanItem : IDispose
{
	GameObject go,flag;
	UILabel lb_name, lb_inventory, lb_count;
	Transform itemParentTrs;
	UIEventListener btn_exchange;
	UISprite sp_cost;
	BossKuangHuanData productData;
	UIItemBase itemBase;
	string costicon;
	int productListIdx = 1;
	public void SetData(GameObject _go, BossKuangHuanData _productData,string _costicon,int _productListIdx)
	{
		go = _go;
		productData = _productData;
		costicon = _costicon;
		productListIdx = _productListIdx;
		InitComponent();
	}
	private void InitComponent()
	{
		lb_name = go.transform.Find("lb_name").GetComponent<UILabel>();
		itemParentTrs = go.transform.Find("Item");
		btn_exchange = go.transform.Find("btn_exchange").GetComponent<UIEventListener>();
		flag = go.transform.Find("sp_complete").gameObject;
		lb_inventory = go.transform.Find("lb_inventory").GetComponent<UILabel>();
		lb_count = go.transform.Find("lb_count").GetComponent<UILabel>();
		sp_cost = go.transform.Find("lb_count/Sprite").GetComponent<UISprite>();
	}
	public void Refresh()
	{
		int tableId = productData.tableId;
		int stockNum = productData.stockNum;
		int itemId = BossCarnivalRewardsTableManager.Instance.GetBossCarnivalRewardsRewardId(tableId);
		int itemNum = BossCarnivalRewardsTableManager.Instance.GetBossCarnivalRewardsPointNum(tableId);
		int quality = ItemTableManager.Instance.GetItemQuality(itemId);
		int costNum = BossCarnivalRewardsTableManager.Instance.GetBossCarnivalRewardsNum(tableId);
		int stockNumMax = BossCarnivalRewardsTableManager.Instance.GetBossCarnivalRewardsStockNum(tableId);
		string itemName = ItemTableManager.Instance.GetItemName(itemId);
		string numColor;
		btn_exchange.gameObject.SetActive(stockNum > 0);
		flag.SetActive(stockNum <= 0);
		lb_name.text = itemName;
		lb_name.color = UtilityCsColor.Instance.GetColor(quality);
		lb_count.text = $"[3d1400]{costNum}";
		sp_cost.spriteName = costicon;
		numColor = stockNum > 0 ? "[007900]" : UtilityColor.GetColorString(ColorType.Red);
		lb_inventory.text = $"{numColor}{stockNum}/{stockNumMax}";
		if (itemBase == null)
			itemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, itemParentTrs, itemSize.Size60);
		RefreshItem(itemBase, itemId, itemNum);

		btn_exchange.onClick = OnBtnExchange;
	}
	private void RefreshItem(UIItemBase item,int id,int num)
	{
		item.Refresh(id);
		item.SetCount(num, Color.white);
	}
	private void OnBtnExchange(GameObject _go)
	{
		int stockNum = productData.stockNum;
		if (stockNum > 0)
		{
			CSBossKuangHuanInfo.Instance.SetBossKuangHuanDataListIdx(productListIdx);
			UIManager.Instance.CreatePanel<UIBuyBossKuangHuanPanel>();
		}
	}
	public void Dispose()
	{
		if (itemBase != null) { UIItemManager.Instance.RecycleSingleItem(itemBase); }
		itemBase = null;
		go = null;
		lb_name = null;
		itemParentTrs = null;
		btn_exchange = null;
		flag = null;
		lb_inventory = null;
		lb_count = null;
		sp_cost = null;
		productData = null;
		costicon = null;
		productListIdx = 1;
	}
}