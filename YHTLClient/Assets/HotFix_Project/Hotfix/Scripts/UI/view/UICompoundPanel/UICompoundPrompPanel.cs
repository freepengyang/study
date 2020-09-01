using System.Collections.Generic;
using System.Data;
using UnityEngine;

public partial class UICompoundPrompPanel : UIBasePanel
{
    private GenerateItemData curGenerateItemData;

    /// <summary>
    /// 合成次数
    /// </summary>
    private int counts;

    /// <summary>
    /// 额外所需货币Id
    /// </summary>
    private int moneyId;

    private UIItemBase itemBase1;
    private UIItemBase itemBase2;
    private UIItemBase itemBaseNeed;

    public override void Init()
    {
        base.Init();
        mbtn_bg.onClick = Close;
        mbtn_close.onClick = Close;
        mbtn_add.onClick = OnClickAdd;
        mbtn_minus.onClick = OnClickMinus;
        mbtn_addMoney.onClick = OnClickAddMoney;
        mbtn_compound.onClick = OnClickCompound;
        EventDelegate.Add(mlb_inputvalue.onChange, OnChangeInput);
    }

    public override void Show()
    {
        base.Show();
    }

    public void ShowCompoundPrompPanel(GenerateItemData generateItemData)
    {
        if (generateItemData != null)
        {
            curGenerateItemData = generateItemData;
            InitData();
        }
    }

    void InitData()
    {
        //设置所需装备信息
        if (curGenerateItemData.ListNeedItem.Count != 2)
        {
            // Debug.Log("--------------------合成功能所需道具配置有误@高飞");
            return;
        }
        else
        {
            if (itemBase1==null)
                itemBase1 = UIItemManager.Instance.GetItem(PropItemType.Normal, mItemBase1.transform,itemSize.Size64);
            itemBase1.Refresh(curGenerateItemData.ListNeedItem[0]);
            int curCount = curGenerateItemData.NeedItemCount;
            int maxCount = curGenerateItemData.ListNeedItem[1];
            // string color = curCount >= maxCount
            //     ? UtilityColor.GetColorString(ColorType.Green)
            //     : UtilityColor.GetColorString(ColorType.Red);
            //
            // string countStr = $"{color}{curCount}/{maxCount}";
            itemBase1.SetCount(curCount, maxCount);
            // mlb_count.text = $"{color}{curCount}/{maxCount}";
        }

        //设置合成后装备信息
        if (itemBase2==null)
            itemBase2 = UIItemManager.Instance.GetItem(PropItemType.Normal, mItemBase2.transform,itemSize.Size64);
        itemBase2.Refresh(curGenerateItemData.ItemId);

        //设置合成次数

        int counts1 = Mathf.FloorToInt((float) curGenerateItemData.NeedItemCount / curGenerateItemData.ListNeedItem[1]);
		bool isNeed = curGenerateItemData.ListNeedResource.Count == 2;
		if (!curGenerateItemData.IsMoney && isNeed)
		{
			int counts2 = 0;
			GetItemCost(ref counts2);
			counts = counts1 > counts2 ? counts2 : counts1;
		}
		else
			counts = counts1;

		mlb_inputvalue.value = counts.ToString();

        //设置合成额外消耗信息
        mAdditionalNeed.SetActive(curGenerateItemData.ListNeedResource.Count == 2 &&
                                  curGenerateItemData.ListNeedResource[1] > 0);
        if (curGenerateItemData.ListNeedResource.Count == 2 && curGenerateItemData.ListNeedResource[1] > 0)
        {
			mUIItemBarPrefab.SetActive(!curGenerateItemData.IsMoney && isNeed);
			//mItemBaseNeed.SetActive(!curGenerateItemData.IsMoney);

			long curCount = curGenerateItemData.NeedResourceCount;
            int maxCount = curGenerateItemData.ListNeedResource[1];
            string color = curCount >= maxCount
                ? UtilityColor.GetColorString(ColorType.Green)
                : UtilityColor.GetColorString(ColorType.Red);

            if (curGenerateItemData.IsMoney)
            {
                moneyId = curGenerateItemData.ListNeedResource[0];
                msp_icon.spriteName = ItemTableManager.Instance.GetItemIcon(curGenerateItemData.ListNeedResource[0]);
                mlb_value.text = $"{color}{curCount}/{maxCount}";
            }
            else
            {
				//if (itemBaseNeed==null)
				//    itemBaseNeed = UIItemManager.Instance.GetItem(PropItemType.Normal, mItemBaseNeed.transform);
				//itemBaseNeed.Refresh(curGenerateItemData.ListNeedResource[0]);
				//itemBaseNeed.SetCount((int)curCount, maxCount);
				RefreshItemCost(curCount, maxCount * counts);
			}
		}
        else //无额外消耗
        {
            mbtn_compound.transform.localPosition += Vector3.up * 50;
            msp_bg.height -= 50;
        }
    }
	private void GetItemCost(ref int counts2)
	{
		int itemId = curGenerateItemData.ListNeedResource[0];
		long need = curGenerateItemData.ListNeedResource[1];
		long have = itemId.GetItemCount();
		counts2 = Mathf.FloorToInt((float)have / need);
	}
	private void RefreshItemCost(long curCount, int maxCount)
	{
		int itemId = curGenerateItemData.ListNeedResource[0];
		long have = curCount;
		long need = maxCount;
		string icon = ItemTableManager.Instance.GetItemIcon(itemId);
		string color = have >= maxCount
		? UtilityColor.GetColorString(ColorType.Green)
		: UtilityColor.GetColorString(ColorType.Red);
		msp_icon.spriteName = $"tubiao{icon}";
		mlb_value.text = $"{color}{curCount}/{maxCount}";

        UIEventListener.Get(mbtn_sp.gameObject).onClick = o =>
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, itemId);
        };
    }
    void OnClickAdd(GameObject go)
    {
        if (counts < Mathf.FloorToInt((float) curGenerateItemData.NeedItemCount / curGenerateItemData.ListNeedItem[1]))
        {
            counts++;
            mlb_inputvalue.value = counts.ToString();
        }
    }

    void OnClickMinus(GameObject go)
    {
        if (counts > 1)
        {
            counts--;
            mlb_inputvalue.value = counts.ToString();
        }
    }

    void OnChangeInput()
    {
        int num = int.Parse(mlb_inputvalue.value);
        int maxNum = Mathf.FloorToInt((float) curGenerateItemData.NeedItemCount / curGenerateItemData.ListNeedItem[1]);
		bool isNeed = curGenerateItemData.ListNeedResource.Count == 2;
		if (!curGenerateItemData.IsMoney && isNeed)
		{
			int num1 = 0;
			GetItemCost(ref num1);
			maxNum = maxNum > num1 ? num1 : maxNum;
		}

		if (num > 0 && num <= maxNum)
        {
            counts = num;
        }
        else if (num <= 0)
        {
            counts = 1;
            mlb_inputvalue.value = counts.ToString();
        }
        else if (num > maxNum)
        {
            counts = maxNum;
            mlb_inputvalue.value = counts.ToString();
        }

		if (!curGenerateItemData.IsMoney && isNeed)
		{
			int need = curGenerateItemData.ListNeedResource[1] * counts;
			long curCount = curGenerateItemData.NeedResourceCount;
			RefreshItemCost(curCount,need);
		}
	}

    void OnClickAddMoney(GameObject go)
    {
        if (curGenerateItemData.ListNeedResource.Count == 2)
        {
            Utility.ShowGetWay(curGenerateItemData.ListNeedResource[0]);
        }
    }

    void OnClickCompound(GameObject go)
    {
        if (counts > 0 && counts <=
            Mathf.FloorToInt((float) curGenerateItemData.NeedItemCount / curGenerateItemData.ListNeedItem[1]))
        {
            // //判断绑定情况
            // if (CSCompoundInfo.Instance.IsBinding(curGenerateItemData.ListNeedItem[0]))
            // {
            //     UtilityTips.ShowPromptWordTips(70,
            //         () =>
            //         {
            //             Net.CSCombineItemMessage(curGenerateItemData.ItemId, counts);
            //             Close();
            //         });
            // }
            // else
            // {
                Net.CSCombineItemMessage(curGenerateItemData.ItemId, counts);
                Close();
            // }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}