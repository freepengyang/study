using System.Collections.Generic;
using UnityEngine;

public partial class UIMaFaPromptPanel : UIBasePanel
{
	PoolHandleManager mPoolHandle = new PoolHandleManager();
	List<UIItemBase> itemList = new List<UIItemBase>();
	MaFaActiveData activeData;
	public enum State
	{
		rewards = 0,    //����˷��͵Ľ���
		preview = 1,    //��������Ԥ��
		advanced = 2,   //���׸߼�����Ԥ��
	}
	public override void Init()
	{
		base.Init();
		AddCollider();
		UIEventListener.Get(mBtnBuy.gameObject).onClick = OnBtnBuy;
	}
	public override void Show()
	{
		base.Show();
	}
	public void RefreshRewardsUI(int idx)
	{
		itemList.Clear();
		//�������佱��
		if (idx == (int)State.rewards)
		{
			mafa.MafaBoxReward maFaRewards = CSMaFaInfo.Instance.GetMafaBoxRewards();
			if (maFaRewards != null)
			{
				mGrid.MaxCount = maFaRewards.rewards.Count;
				for (int i = 0; i < mGrid.MaxCount; i++)
				{
					SetItemInfo(i, maFaRewards.rewards[i].itemId, maFaRewards.rewards[i].count);
				}
			}
			mTitle.spriteName = "title6";
			mLbTitle.text = "";
		}
		//����Ԥ��
		else if (idx == (int)State.preview)
		{
			int sundryId = 0;
			int num = CSMaFaInfo.Instance.GetLeftNum();
			sundryId = num == 1 ? 1011 : 1010;
			string itemStr = SundryTableManager.Instance.GetSundryEffect(sundryId);
			List<int> items = UtilityMainMath.SplitStringToIntList(itemStr);
			mGrid.MaxCount = items.Count;
			for (int i = 0; i < mGrid.MaxCount; i++)
			{
				SetItemInfo(i, items[i]);
			}
			mTitle.spriteName = "title26";
			mLbTitle.text = CSString.Format(1879);
		}
		//��Ҫ������ʾ
		else if (idx == (int)State.advanced)
		{
			activeData = CSMaFaInfo.Instance.GetAcitveData();
			int id = activeData.id;
			string itemStr = MafaActivityTableManager.Instance.GetMafaActivityShow(id);
			List<List<int>> items = UtilityMainMath.SplitStringToIntLists(itemStr);
			mGrid.MaxCount = items.Count;
			for (int i = 0; i < mGrid.MaxCount; i++)
			{
				SetItemInfo(i, items[i][0], items[i][1]);
			}
			mTitle.spriteName = "title27";
			mLbBtnBuy.text = CSString.Format(1870);
			mBtnBuy.gameObject.SetActive(true);
			mLbTitle.text = CSString.Format(1880);
		}

		//��������
		UIWidget wid = mTexBG.GetComponent<UIWidget>();
		int index = mGrid.MaxCount / mGrid.MaxPerLine;
		int distanceY = 0;
		int btnDistanceY = 20;		//���ư�ť����ǿ���ʾ
		if (index>1)
		{
			index--;
			distanceY = (int)(index * mGrid.CellHeight);
			wid.height += distanceY;
			mHintTrs.localPosition += Vector3.down * distanceY;
		}
		if (idx == (int)State.advanced)
		{
			mBtnBuy.localPosition += Vector3.down * distanceY;
			wid.height += distanceY + btnDistanceY;
			mHintTrs.localPosition += Vector3.down * (distanceY+ btnDistanceY) ;
		}
	}
	private void SetItemInfo(int idx, int id, int num = 0)
	{
		Transform parentTrs = mGrid.controlList[idx].transform;
		UIItemBase item = UIItemManager.Instance.GetItem(PropItemType.Normal, parentTrs, itemSize.Size66);
		item.Refresh(id);
		if (num > 0)
			item.SetCount(num,false,false);
		itemList.Add(item);
	}
	private void OnBtnBuy(GameObject _go)
	{
		TABLE.RECHARGE recharge = null;
		int rechargeId = MafaActivityTableManager.Instance.GetMafaActivityBuy(activeData.id);
		if (RechargeTableManager.Instance.TryGetValue(rechargeId, out recharge))
		{
			CSRechargeInfo.Instance.TryToRecharge(recharge);
			Close();
		}
	}
	protected override void OnDestroy()
	{
		mPoolHandle?.OnDestroy();
		if (itemList != null) {
			UIItemManager.Instance.RecycleItemsFormMediator(itemList);
			itemList.Clear();
			itemList = null;
		}
		activeData = null;
		base.OnDestroy();
	}
}