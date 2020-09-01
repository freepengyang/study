using UnityEngine;

public partial class UIGuildTreasureCabinetPanel : UIBasePanel
{
	public override void Init()
	{
		base.Init();
		mItemDatas = new FastArrayElementKeepHandle<ItemBarData>(4);
		mbtn_go.onClick = OnClickEnter;
		mClientEvent.AddEvent(CEvent.ItemListChange, OnItemCounterChanged);
		mClientEvent.AddEvent(CEvent.MoneyChange, OnItemCounterChanged);
		mClientEvent.AddEvent(CEvent.GetEnterInstanceInfo, OnEnterInstance);

		CSEffectPlayMgr.Instance.ShowUITexture(msand_bg, "guildfight_sand_bg");
		CSEffectPlayMgr.Instance.ShowUITexture(msand, "treasure_cabinet_00");
		CSEffectPlayMgr.Instance.ShowUITexture(msand_line, "guildfight_sand_line");
	}

	int sundryId = 609;//副本表
	int instanceId = 301401;//副本表
	TABLE.SUNDRY mSundryItem;
	FastArrayElementKeepHandle<ItemBarData> mItemDatas;
	int mFlag = (int)ItemBarData.ItemBarType.IBT_SMALL_ICON | (int)ItemBarData.ItemBarType.IBT_COMPARE | (int)ItemBarData.ItemBarType.IBT_RED_GREEN | (int)ItemBarData.ItemBarType.IBT_ADD;

	protected void Refresh()
	{
		bool isSabakeMember = CSGuildFightManager.Instance.IsSabakeMember;
		menterHint.CustomActive(isSabakeMember);
		mgrid_costs.CustomActive(!isSabakeMember);
		mItemDatas.Clear();
		if(SundryTableManager.Instance.TryGetValue(sundryId, out mSundryItem))
		{
			UIItemBarManager.Instance.Split(mItemDatas,mSundryItem.effect,mFlag);
		}
		UISabacItemManager.Instance.Bind(mgrid_costs, mItemDatas);
	}
	
	public override void Show()
	{
		base.Show();

		Refresh();
	}

	void OnClickEnter(GameObject go)
	{
        if (null == mSundryItem)
            return;

        if (!CSGuildFightManager.Instance.IsSabakeMember)
        {
            if (!mPoolHandleManager.IsItemEnough(mSundryItem.effect, 0, true))
            {
                return;
            }
        }

		Enter();
	}

	void Enter()
	{
		Net.ReqEnterInstanceMessage(instanceId, true);
	}

	protected void OnItemCounterChanged(uint id,object argv)
	{
		Refresh();
	}

	protected void OnEnterInstance(uint id,object argv)
	{
		if(argv is instance.InstanceInfo msg)
		{
			UIManager.Instance.ClosePanel<UIGuildFightCombinedPanel>();
		}
	}

	protected override void OnDestroy()
	{
        CSEffectPlayMgr.Instance.Recycle(msand_bg);
        CSEffectPlayMgr.Instance.Recycle(msand);
        CSEffectPlayMgr.Instance.Recycle(msand_line);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnItemCounterChanged);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnItemCounterChanged);
		mClientEvent.RemoveEvent(CEvent.GetEnterInstanceInfo, OnEnterInstance);
		if (null != mgrid_costs)
		{
			UISabacItemManager.Instance.UnBind(mgrid_costs);
			mgrid_costs = null;
		}
		base.OnDestroy();
	}
}
