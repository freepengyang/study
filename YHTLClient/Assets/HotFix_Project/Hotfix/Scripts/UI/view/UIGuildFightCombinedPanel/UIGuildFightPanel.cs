using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIGuildFightPanel : UIBasePanel
{
	Stack<GuildFightAwardItemBinder> mCachedBinders = new Stack<GuildFightAwardItemBinder>(4);
	FastArrayElementFromPool<GuildFightAwardItemBinder> mSmallFightBinders;
	GuildFightAwardItemBinder mMainBinder;
	public override void Init()
	{
		base.Init();

		mbtn_help.onClick = OnClickHelp;
		mbtn_go_fight.onClick = OnClickGoFight;
		mMainBinder = mbigItem.GetOrAddBinder<GuildFightAwardItemBinder>(mPoolHandleManager);

		InitFixedDesc();

		ScriptBinder.InvokeRepeating(0, 1.0f, UpdateStageDesc);

		mClientEvent.AddEvent(CEvent.OnGuildFightDataChanged, OnGuildFightDataChanged);

		CSEffectPlayMgr.Instance.ShowUITexture(mtex_bg, "guildfight_bg");

		mSmallFightBinders = new FastArrayElementFromPool<GuildFightAwardItemBinder>(4, () =>
		{
			if(mCachedBinders.Count > 0)
			{
				mCachedBinders.Peek().Handle.CustomActive(true);
				return mCachedBinders.Pop();
			}
			GameObject clonedObject = Object.Instantiate(mminItem, mminItem.transform.parent) as GameObject;
			clonedObject.transform.localPosition = Vector3.zero;
			clonedObject.transform.localScale = Vector3.one;
			clonedObject.transform.localRotation = Quaternion.identity;
			var binder = clonedObject.GetOrAddBinder<GuildFightAwardItemBinder>(mPoolHandleManager);
			binder.Handle.CustomActive(true);
			return binder;
		},
		(GuildFightAwardItemBinder binder) =>
		{
			binder.Handle.CustomActive(false);
			mCachedBinders.Push(binder);
		});
	}

	public override void Show()
	{
		base.Show();

		RefreshAwards();
		CSGuildFightManager.Instance.QueryFightInfo();
	}

	protected void UpdateStageDesc()
	{
        if (null != mlb_stage_desc)
        {
            mlb_stage_desc.text = CSGuildFightManager.Instance.GetStageDesc();
        }

		if(null != mbigItem && mbigItem.activeInHierarchy)
		{
			mMainBinder.TryRefreshTime();
		}
    }

	protected void InitFixedDesc()
	{
		if(null != mlb_fixed_desc)
		{
			mlb_fixed_desc.text = CSString.Format(1029, CSGuildFightManager.Instance.ExtraPercentValue);
		}
	}

	protected void OnClickHelp(GameObject go)
	{
		UIManager.Instance.CreatePanel<UIGuildFightRulePanel>();
		//UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.GuildFightHelp);
	}

	protected void RefreshAwards()
	{
		mSmallFightBinders.Clear();
		var fightDatas = CSGuildFightManager.Instance.GetFightItemDatas();
		Vector3 pos = Vector3.zero;
		float offset = 0.0f;
		float dis = 4;
		mbigItem.CustomActive(false);
		for (int i = 0; i < fightDatas.Count; ++i)
		{
			if(fightDatas[i].IsMain)
			{
				if(i > 0)
				{
					offset += 70;
				}
				var binder = mbigItem.GetOrAddBinder<GuildFightAwardItemBinder>(mPoolHandleManager);
				mbigItem.CustomActive(true);
				binder.Bind(fightDatas[i]);
				pos = mbigItem.transform.localPosition;
				pos.x = offset;
				mbigItem.transform.localPosition = pos;
				offset += 70 + dis;
			}
			else
			{
                if (i > 0)
                {
                    offset += 63;
                }
                var binder = mSmallFightBinders.Append();
				binder.Bind(fightDatas[i]);
                pos = binder.Handle.transform.localPosition;
                pos.x = offset;
                binder.Handle.transform.localPosition = pos;
                offset += 63 + dis;
            }
		}
	}

    protected void OnClickGoFight(GameObject go)
    {
		UIManager.Instance.ClosePanel<UIGuildFightCombinedPanel>();
		CSGuildFightManager.Instance.DeliverToSabacFightZone();
	}

	protected void OnGuildFightDataChanged(uint id,object argv)
	{
		RefreshAwards();
	}

	protected override void OnDestroy()
	{
		if(null != mSmallFightBinders)
		{
            for (int i = 0; i < mSmallFightBinders.Count; ++i)
            {
                mSmallFightBinders[i].Destroy();
            }
			mSmallFightBinders.Clear();
			mSmallFightBinders = null;
		}
		mCachedBinders?.Clear();
		mCachedBinders = null;
		CSEffectPlayMgr.Instance.Recycle(mtex_bg);
		mClientEvent.RemoveEvent(CEvent.OnGuildFightDataChanged, OnGuildFightDataChanged);
        base.OnDestroy();
	}
}
