using System.Collections;
using System.Collections.Generic;
using System.Data;
using ultimate;
using UnityEngine;

public partial class UICompoundPanel : UIBasePanel
{
	public void SetGo(GameObject _go)
	{
		UIPrefab = _go;
	}

	/// <summary>
	/// 当前组索引
	/// </summary>
	private int curGroupIndex;
	/// <summary>
	/// 当前子页签索引
	/// </summary>
	private int curItemIndex;
	/// <summary>
	/// 是否跳转定位
	/// </summary>
	private bool isJump = false;
	
	//当前主页签是否打开
	private bool isOpenCurTab = true;

	/// <summary>
	/// 当前选中的组Id
	/// </summary>
	private int selectGroupId;
	/// <summary>
	/// 当前选中的子页签Id
	/// </summary>
	private int selectItemId;
	/// <summary>
	/// 合成组数据
	/// </summary>
	private FastArrayElementFromPool<CompoundGroupData> CompoundGroupDatas;
	/// <summary>
	/// 当前组信息
	/// </summary>
	private CompoundGroupData curCompoundGroupData;
	/// <summary>
	/// 所需额外货币Id
	/// </summary>
	private int moneyId;
	/// <summary>
	/// 当前选中的子页签Item
	/// </summary>
	private GenerateItemData curGenerateItemData;

	private UIItemBase itembase1;
	private UIItemBase itembase2;
	//private UIItemBase itemBaseNeed;

	public override void Init()
	{
		base.Init();
		mClientEvent.Reg((uint)CEvent.ItemListChange, RefreshDataItemChange);
		mClientEvent.Reg((uint)CEvent.CombineItem, RefreshData);

		mbtn_close.onClick = Close;
		mbtn_compound.onClick = OnClickCompound;
		mbtn_add.onClick = OnClickAdd;
		// mbtn_help.onClick = OnClickHelp;
	}

	void RefreshData(uint id, object data)
	{
		RefreshGrid();
		CSEffectPlayMgr.Instance.ShowUIEffect(mresidentEffect, 17087);
	}
	
	/// <summary>
	/// 道具和金币变化
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	void RefreshDataItemChange(uint id, object data)
	{
		RefreshGrid();
	}
	
	public override void Show()
	{
		base.Show();
		CSEffectPlayMgr.Instance.ShowUITexture(mcompound_bg1, "compound_bg1");
		CSEffectPlayMgr.Instance.ShowUITexture(mcompound_bg2, "compound_bg2");
	}

	/// <summary>
	/// 根据合成的道具Id打开合成界面
	/// </summary>
	/// <param name="itemId"></param>
	public void ShowCompoundPanel(int itemId = 0)
	{
		CompoundGroupDatas = CSCompoundInfo.Instance.MCachedCompoundGroupDatas;
		if (CompoundGroupDatas.Count <= 0) return;
		if (itemId <= 0) //正常处理
		{
			//默认第一个（没有任何可合成）
			selectGroupId = CompoundGroupDatas[0].GroupId;
			selectItemId = CompoundGroupDatas[0].GenerateItems[0].ItemId;
			curGroupIndex = 0;
			curItemIndex = 0;
			curCompoundGroupData = CompoundGroupDatas[0];
			curGenerateItemData = CompoundGroupDatas[0].GenerateItems[0];
			//寻找第一个能合成的
			GenerateItemData generateItemData;
			for (int i = 0; i < CompoundGroupDatas.Count; i++)
			{
				for (int j = 0; j < CompoundGroupDatas[i].GenerateItems.Count; j++)
				{
					generateItemData = CompoundGroupDatas[i].GenerateItems[j];
					if (generateItemData.NeedItemCount >= generateItemData.ListNeedItem[1]	
					    && (generateItemData.ListNeedResource.Count != 2
					        || (generateItemData.ListNeedResource.Count == 2
					            && generateItemData.NeedResourceCount >= generateItemData.ListNeedResource[1])))
					{
						selectGroupId = CompoundGroupDatas[i].GroupId;
						selectItemId = generateItemData.ItemId;
						curGroupIndex = i;
						curItemIndex = j;
						curCompoundGroupData = CompoundGroupDatas[i];
						curGenerateItemData = generateItemData;
						RefreshGrid();
						return;
					}
				}
			}
		}
		else
		{
			isJump = true;
			for (int i = 0; i < CompoundGroupDatas.Count; i++)
			{
				GenerateItemData generateItemData;
				for (int j = 0; j < CompoundGroupDatas[i].GenerateItems.Count; j++)
				{
					generateItemData = CompoundGroupDatas[i].GenerateItems[j];
					if (generateItemData.ListNeedItem[0] == itemId)
					{
						selectGroupId = CompoundGroupDatas[i].GroupId ;
						curGroupIndex = i;
						curCompoundGroupData = CompoundGroupDatas[i];
						selectItemId = generateItemData.ItemId;
						curItemIndex = j;
						curGenerateItemData = generateItemData;
						RefreshGrid();
						return;
					}
				}
			}
		}
		RefreshGrid();
		// SetSelectId(itemId);
	}

	/// <summary>
	/// 设置选中的大页签和小页签的值（1：不传值默认流程 2：只要传itemId,直接跳转到该子页签，groupId不生效 3：只传groupId,
	/// 跳转到该大页签，子页签按默认流程）
	/// </summary>
	/// <param name="itemId">子页签待合成装备Id</param>
	/// <param name="groupId">大页签组Id</param>
	void SetSelectId(int itemId=0, int groupId=0)
	{
		// CSEffectPlayMgr.Instance.Recycle(mresidentEffect);
		if (itemId<=0)//正常处理
		{
			if (groupId <= 0)
			{
				//默认第一个（没有任何可合成）
				selectGroupId = CompoundGroupDatas[0].GroupId;
				selectItemId = CompoundGroupDatas[0].GenerateItems[0].ItemId;
				curGroupIndex = 0;
				curItemIndex = 0;
				curCompoundGroupData = CompoundGroupDatas[0];
				curGenerateItemData = CompoundGroupDatas[0].GenerateItems[0];
				//寻找第一个能合成的
				GenerateItemData generateItemData;
				for (int i = 0; i < CompoundGroupDatas.Count; i++)
				{
					for (int j = 0; j < CompoundGroupDatas[i].GenerateItems.Count; j++)
					{
						generateItemData = CompoundGroupDatas[i].GenerateItems[j];
						if (generateItemData.NeedItemCount >= generateItemData.ListNeedItem[1]
						    && (generateItemData.ListNeedResource.Count != 2
						        || (generateItemData.ListNeedResource.Count == 2
						            && generateItemData.NeedResourceCount >= generateItemData.ListNeedResource[1])))
						{
							selectGroupId = CompoundGroupDatas[i].GroupId;
							selectItemId = generateItemData.ItemId;
							curGroupIndex = i;
							curItemIndex = j;
							curCompoundGroupData = CompoundGroupDatas[i];
							curGenerateItemData = generateItemData;
							RefreshGrid();
							return;
						}
					}
				}
			}
			else
			{
				for (int i = 0; i < CompoundGroupDatas.Count; i++)
				{
					if (CompoundGroupDatas[i].GroupId == groupId)
					{
						selectGroupId = groupId;
						curGroupIndex = i;
						curCompoundGroupData = CompoundGroupDatas[i];
						selectItemId = CompoundGroupDatas[i].GenerateItems[0].ItemId;
						curGenerateItemData = CompoundGroupDatas[i].GenerateItems[0];
						GenerateItemData generateItemData;
						for (int j = 0; j < CompoundGroupDatas[i].GenerateItems.Count; j++)
						{
							generateItemData = CompoundGroupDatas[i].GenerateItems[j];
							if (generateItemData.NeedItemCount >= generateItemData.ListNeedItem[1]
							    && (generateItemData.ListNeedResource.Count != 2
							        || (generateItemData.ListNeedResource.Count == 2
							            && generateItemData.NeedResourceCount >= generateItemData.ListNeedResource[1])))
							{
								selectItemId = generateItemData.ItemId;
								curItemIndex = j;
								curGenerateItemData = generateItemData;
								RefreshGrid();
								return;
							}
						}
						break;
					}
				}
			}
		}
		else//直接跳转到选中子页签
		{
			for (int i = 0; i < CompoundGroupDatas.Count; i++)
			{
				GenerateItemData generateItemData;
				for (int j = 0; j < CompoundGroupDatas[i].GenerateItems.Count; j++)
				{
					generateItemData = CompoundGroupDatas[i].GenerateItems[j];
					if (generateItemData.ItemId == itemId)
					{
						selectGroupId = CompoundGroupDatas[i].GroupId ;
						curGroupIndex = i;
						curCompoundGroupData = CompoundGroupDatas[i];
						selectItemId = generateItemData.ItemId;
						curItemIndex = j;
						curGenerateItemData = generateItemData;
						RefreshGrid();
						return;
					}
				}
			}
		}
		RefreshGrid();
	}

	void RefreshGrid()
	{
		// mgrid_table.UnBind<UICompoundBinder>();
		mgrid_table.MaxCount = CompoundGroupDatas.Count;
		GameObject gp;
		for (int i = 0; i < mgrid_table.MaxCount; i++)
		{
			gp = mgrid_table.controlList[i];
			var eventHandle = UIEventListener.Get(gp);
			UICompoundBinder compoundBinder;
			if(eventHandle.parameter == null)
			{
				compoundBinder = new UICompoundBinder();
				compoundBinder.Setup(eventHandle);
			}
			else
			{
				compoundBinder = eventHandle.parameter as UICompoundBinder;
			}
			compoundBinder.isSelect = CompoundGroupDatas[i].GroupId == selectGroupId;
			compoundBinder.selectItemId = selectItemId;
			compoundBinder.isOpen = compoundBinder.isSelect ? isOpenCurTab : false;
			compoundBinder.actionItem = OnClickGenerateItem;
			compoundBinder.actionGroup = OnClickGroup;
			compoundBinder.Bind(CompoundGroupDatas[i]);
		}
		
		mgrid_table.GetComponent<UITable>().repositionNow = true;
		mgrid_table.GetComponent<UITable>().Reposition();
		// ms.ResetPosition();
		// mScrollView_Info

		if (isJump)
		{
			float curheight = curGroupIndex*mgrid_table.CellHeight+curItemIndex*mgrid_sub.CellHeight;
			float scrowllviewheight = mScrollView_table.panel.height;
			float maxheight = CompoundGroupDatas.Count * mgrid_table.CellHeight +
			                  curCompoundGroupData.GenerateItems.Count * mgrid_sub.CellHeight;
			//差值
			float difference = maxheight-scrowllviewheight;
			CSGame.Sington.StartCoroutine(SetScrollValue(curheight / difference));
		}

		SetRightInfo();
	}

	IEnumerator SetScrollValue(float value)
	{
		yield return null;
		mScrollView_table.verticalScrollBar.value = value;
		isJump = false;
	}

	void OnClickGroup(int curGroupId, bool isOpen)
	{
		if (curGroupId == selectGroupId)
		{
			isOpenCurTab = !isOpen;
			RefreshGrid();
		}
		else
		{
			isOpenCurTab = true;
			isJump = false;
			SetSelectId(groupId:curGroupId);
		}
	}
	
	/// <summary>
	/// 点击同组中子页签更新选中ItemID
	/// </summary>
	/// <param name="itemId"></param>
	void OnClickGenerateItem(int itemId)
	{
		if (selectItemId == itemId) return;
		isJump = false;
		SetSelectId(itemId);
	}

	void SetRightInfo()
	{
		//设置所需装备信息
		if (curGenerateItemData.ListNeedItem.Count != 2)
		{
			// Debug.Log("--------------------合成功能所需道具配置有误@高飞");
			return;
		}
		else
		{
			if (itembase1==null)
				itembase1 = UIItemManager.Instance.GetItem(PropItemType.Normal, mItemBase1.transform);
			itembase1.Refresh(curGenerateItemData.ListNeedItem[0]);
			mlb_before.text = ItemTableManager.Instance.GetItemName(curGenerateItemData.ListNeedItem[0]);
			mlb_before.color =
				UtilityCsColor.Instance.GetColor(
					ItemTableManager.Instance.GetItemQuality(curGenerateItemData.ListNeedItem[0]));
			int curCount = curGenerateItemData.NeedItemCount;
			int maxCount = curGenerateItemData.ListNeedItem[1];
			itembase1.SetCount(curCount, maxCount);
		}
		//设置合成后装备信息
		if (itembase2==null)
			itembase2 = UIItemManager.Instance.GetItem(PropItemType.Normal, mItemBase2.transform);
		itembase2.Refresh(selectItemId);
		mlb_after.text = ItemTableManager.Instance.GetItemName(selectItemId);
		mlb_after.color =
			UtilityCsColor.Instance.GetColor(
				ItemTableManager.Instance.GetItemQuality(selectItemId));
		//设置合成额外消耗信息
		SetAdditionalNeed();
	}

	void SetAdditionalNeed()
	{
		mAdditionalNeed.SetActive(curGenerateItemData.ListNeedResource.Count==2&&curGenerateItemData.ListNeedResource[1]>0);
		mItemBaseNeed.SetActive(false);
		if (curGenerateItemData.ListNeedResource.Count==2&&curGenerateItemData.ListNeedResource[1]>0)
		{
			mUIItemBarPrefab.SetActive(!curGenerateItemData.IsMoney);
			// mItemBaseNeed.SetActive(!curGenerateItemData.IsMoney);

			long curCount = curGenerateItemData.NeedResourceCount;
			int maxCount = curGenerateItemData.ListNeedResource[1];
			string color = curCount >= maxCount
				? UtilityColor.GetColorString(ColorType.Green)
				: UtilityColor.GetColorString(ColorType.Red);
			
			moneyId = curGenerateItemData.ListNeedResource[0];
			msp_icon.spriteName = $"tubiao{ItemTableManager.Instance.GetItemIcon(moneyId)}";
			mlb_value.text = $"{color}{curCount}/{maxCount}";
			
			mbtn_sp.onClick = o =>
			{
				UITipsManager.Instance.CreateTips(TipsOpenType.Normal, moneyId);
			};
			
			// if (curGenerateItemData.IsMoney)
			// {
			// 	moneyId = curGenerateItemData.ListNeedResource[0];
			// 	msp_icon.spriteName = $"tubiao{ItemTableManager.Instance.GetItemIcon(curGenerateItemData.ListNeedResource[0])}";
			// 	mlb_value.text = $"{color}{curCount}/{maxCount}";
			// }
			// else
			// {
			// 	if (itemBaseNeed==null)
			// 		itemBaseNeed = UIItemManager.Instance.GetItem(PropItemType.Normal, mItemBaseNeed.transform);
			// 	itemBaseNeed.Refresh(curGenerateItemData.ListNeedResource[0]);
			// 	itemBaseNeed.SetCount((int)curCount, maxCount);
			// }
		}
	}

	/// <summary>
	/// 一键合成
	/// </summary>
	/// <param name="go"></param>
	void OnClickCompound(GameObject go)
	{
		if (curGenerateItemData == null) return;
		if (curGenerateItemData.ListNeedItem.Count != 2)
		{
			// Debug.Log("--------------------合成功能所需道具配置有误@高飞");
		}
		else
		{
			if (curGenerateItemData.NeedItemCount<curGenerateItemData.ListNeedItem[1])//道具不够
			{
				Utility.ShowGetWay(curGenerateItemData.ListNeedItem[0]);
			}
			else
			{
				if (curGenerateItemData.ListNeedResource.Count==2
				    &&curGenerateItemData.NeedResourceCount<curGenerateItemData.ListNeedResource[1])//额外消耗不够
				{
					Utility.ShowGetWay(curGenerateItemData.ListNeedResource[0]);
				}
				else
				{
					if (curGenerateItemData.NeedItemCount/curGenerateItemData.ListNeedItem[1]<3)//符合小于三次
					{
						//判断绑定情况
						// if (CSCompoundInfo.Instance.IsBinding(curGenerateItemData.ListNeedItem[0]))
						// {
						// 	UtilityTips.ShowPromptWordTips(70, () =>
						// 	{
						// 		Net.CSCombineItemMessage(curGenerateItemData.ItemId, 1);//合成一次
						// 	});
						// }
						// else
						// {
							Net.CSCombineItemMessage(curGenerateItemData.ItemId, 1);//合成一次	
						// }
					}
					else//能够合成3次以上
					{
						UIManager.Instance.CreatePanel<UICompoundPrompPanel>(f =>
						{
							(f as UICompoundPrompPanel).ShowCompoundPrompPanel(curGenerateItemData);
						});
					}
				}
			}
		}
	}

	void OnClickAdd(GameObject go)
	{
		if (moneyId>0)
		{
			Utility.ShowGetWay(moneyId);	
		}
	}

	/// <summary>
	/// 打开帮助界面(已弃用)
	/// </summary>
	// void OnClickHelp(GameObject go)
	// {
	// 	UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.EquipCompound);
	// }

	protected override void OnDestroy()
	{
		mgrid_table.UnBind<UICompoundBinder>();
		CompoundGroupDatas = null;
		CSEffectPlayMgr.Instance.Recycle(mresidentEffect);
		CSEffectPlayMgr.Instance.Recycle(mcompound_bg1);
		CSEffectPlayMgr.Instance.Recycle(mcompound_bg2);
		UIItemManager.Instance.RecycleSingleItem(itembase1);
		UIItemManager.Instance.RecycleSingleItem(itembase2);
		//UIItemManager.Instance.RecycleSingleItem(itemBaseNeed);
		base.OnDestroy();
	}
}
