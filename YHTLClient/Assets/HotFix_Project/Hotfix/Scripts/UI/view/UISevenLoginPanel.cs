using System.Collections.Generic;
using UnityEngine;

public partial class UISevenLoginPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

	Dictionary<int, UIItemBase> mId2ItemBase;
	Dictionary<int, UIItemBase> mId2ItemBaseChoiced;
	Dictionary<int, int> mCachedAwardDic = new Dictionary<int, int>(8);
	public override void Init()
	{
		base.Init();

		mbtn_close.onClick = this.Close;
		mClientEvent.AddEvent(CEvent.OnSevenDayLoginChanged, OnSevenDayLoginChanged);
		CSSevenLoginInfo.Instance.choicedId = -1;
		mId2ItemBase = new Dictionary<int, UIItemBase>(8);
		mId2ItemBaseChoiced = new Dictionary<int, UIItemBase>(8);
		mAwards = mPoolHandleManager.CreateItemPool(PropItemType.Normal, mgrid_award_parent,8,itemSize.Size64);
		mAwardsDic = new Dictionary<int, int>(8);

		CSEffectPlayMgr.Instance.ShowUITexture(mbanner26, "banner26");
	}

	#region events
	void OnSevenDayLoginChanged(uint id,object argv)
	{
		InitDayLabels();
	}
	#endregion

	#region day_labels
	void InitDayLabels()
	{
		var datas = CSSevenLoginInfo.Instance.LoginItems;
		mgrid_day_labels.MakeActivedChildCountNoTemplate(datas.Count);

		Vector3 pos = Vector3.zero;
		float offset = 0.0f;
        float dis = ScriptBinder.GetIntArgv(1);//4
		int bigHalfWidth = ScriptBinder.GetIntArgv(2);//70
		int smallHalfWidth = ScriptBinder.GetIntArgv(3);//63

        for (int i = 0,max = datas.Count; i < max;++i)
		{
			var data = datas[i];
			var child = mgrid_day_labels.GetChild(i);

			var choicedNode = child.Find("node_select");
			var generalNode = child.Find("node");

			choicedNode.CustomActive(data.choiced);
			generalNode.CustomActive(!data.choiced);

			UISprite bg = Get<UISprite>("bg", generalNode);
			if(null != bg)
			{
				UIEventListener.Get(bg.gameObject, data).onClick = OnLoginLabelChoiced;
				bg.spriteName = (data.status == 2 || data.status == 3) ? "seven_login_4" : "seven_login_2";
			}

            bg = Get<UISprite>("bg", choicedNode);
            if (null != bg)
            {
                UIEventListener.Get(bg.gameObject, data).onClick = OnLoginLabelChoiced;
				bg.spriteName = (data.status == 2 || data.status == 3) ? "seven_login_3" : "seven_login_1";
            }

            //acquired
            Transform goCheckMark = Get<Transform>("checkmark", generalNode);
			goCheckMark.CustomActive(data.status == 2);

            goCheckMark = Get<Transform>("checkmark", choicedNode);
            goCheckMark.CustomActive(data.status == 2);

            //overtime
            Transform goOverTime = Get<Transform>("overtime", generalNode);
			goOverTime.CustomActive(data.status == 3);

            goOverTime = Get<Transform>("overtime", choicedNode);
            goOverTime.CustomActive(data.status == 3);

            //un reached
            var unreached = Get<GameObject>("unreached", generalNode);
            unreached.CustomActive(data.status == 0);

            unreached = Get<GameObject>("unreached", choicedNode);
            unreached.CustomActive(data.status == 0);

            //can acquired
            var go_can_acquired = Get<GameObject>("go_can_acquired", generalNode);
			if(data.status == 1 && !data.choiced)
			{
				go_can_acquired.PlayEffect(ScriptBinder.GetIntArgv(0));
			}
			else
			{
				go_can_acquired.StopEffect();
			}

            go_can_acquired = Get<GameObject>("go_can_acquired", choicedNode);
            if (data.status == 1 && data.choiced)
            {
                go_can_acquired.PlayEffect(ScriptBinder.GetIntArgv(0));
            }
            else
            {
                go_can_acquired.StopEffect();
            }

            //item data
            Transform item_parent = Get<Transform>("item_parent", generalNode);
			if (!mId2ItemBase.TryGetValue(data.reward.id,out UIItemBase itemBase))
			{
				itemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, item_parent, itemSize.Size64);
				mId2ItemBase.Add(data.reward.id, itemBase);
				BoxTableManager.Instance.GetBoxAwardById(data.reward.goalId,mCachedAwardDic);
				for(var it = mCachedAwardDic.GetEnumerator();it.MoveNext();)
				{
					itemBase.Refresh(it.Current.Key);
					itemBase.SetCount(it.Current.Value);
					break;
				}
			}

            item_parent = Get<Transform>("item_parent", choicedNode);
            if (!mId2ItemBaseChoiced.TryGetValue(data.reward.id, out UIItemBase itemBaseChoiced))
            {
				itemBaseChoiced = UIItemManager.Instance.GetItem(PropItemType.Normal, item_parent, itemSize.Size68);
				mId2ItemBaseChoiced.Add(data.reward.id, itemBaseChoiced);
                BoxTableManager.Instance.GetBoxAwardById(data.reward.goalId, mCachedAwardDic);
                for (var it = mCachedAwardDic.GetEnumerator(); it.MoveNext();)
                {
					itemBaseChoiced.Refresh(it.Current.Key);
					itemBaseChoiced.SetCount(it.Current.Value);
                    break;
                }
            }

            //lb_theme
            UILabel label = Get<UILabel>("lb_theme", generalNode);
			label.text = data.reward.tips;

            label = Get<UILabel>("lb_theme", choicedNode);
            label.text = data.reward.tips;

            if (data.choiced)
			{
                if (i > 0)
                {
                    offset += bigHalfWidth;
                }

                //pos = child.localPosition;
				pos.x = offset;
				child.localPosition = pos;
                offset += bigHalfWidth + dis;

				InitChoicedData(data);
			}
			else
			{
                if (i > 0)
                {
                    offset += smallHalfWidth;
                }
                //pos = child.localPosition;
                pos.x = offset;
				child.localPosition = pos;
                offset += smallHalfWidth + dis;
            }
		}
	}

	protected void OnLoginLabelChoiced(GameObject go)
	{
		if(UIEventListener.Get(go).parameter is SevenLoginItem loginItem)
		{
            var datas = CSSevenLoginInfo.Instance.LoginItems;
            for (int i = 0, max = datas.Count; i < max; ++i)
            {
				datas[i].choiced = object.ReferenceEquals(datas[i], loginItem);
            }
			InitDayLabels();
		}
	}
	#endregion

	#region choiced_awards
	FastArrayElementFromPool<UIItemBase> mAwards;
	Dictionary<int, int> mAwardsDic;
	protected void InitAwards(int boxId)
	{
		mAwards.Clear();
		BoxTableManager.Instance.GetBoxAwardById(boxId, mAwardsDic);
		for(var it = mAwardsDic.GetEnumerator();it.MoveNext();)
		{
			if(ItemTableManager.Instance.TryGetValue(it.Current.Key,out TABLE.ITEM itemCfg))
			{
				var itemBase = mAwards.Append();
				itemBase.Refresh(itemCfg);
				itemBase.SetCount(it.Current.Value);
			}
		}

		float startX = 0.0f;
		float startY = 0.0f;
		float disX = ScriptBinder.GetIntArgv(4);
		float disY = ScriptBinder.GetIntArgv(5);
		int coloums = ScriptBinder.GetIntArgv(6);

		int rows = mAwards.Count / coloums;
		if (mAwards.Count % coloums != 0)
			rows += 1;
		float startYOffset = -(rows - 1) * 0.50f * disY;

		Vector3 pos = Vector3.zero;
		for(int i = 0,max = mAwards.Count; i < max;++i)
		{
			var itemBase = mAwards[i];
			if(i % coloums == 0)
			{
				int maxCnt = Mathf.Min(mAwards.Count - i, coloums);
				startX = -(maxCnt - 1) * 0.5f * disX;
				startY = startYOffset + (i / coloums) * disY;
			}
			pos.x = startX + i % coloums * disX;
			pos.y = startY;
			itemBase.obj_trans.localPosition = pos;
		}
	}
	#endregion

	#region choiced_awards button_status
	void InitBtnAwardsStatus(SevenLoginItem loginItem)
	{
		mbtn_extract.CustomActive(loginItem.status == 1);
		if (loginItem.status == 1)
			mgo_can_acquired.PlayEffect(ScriptBinder.GetIntArgv(7));
		else
			mgo_can_acquired.StopEffect();
		mlb_status.CustomActive(loginItem.status != 1);
		int tipId = 0;
		if(loginItem.status == 2)
		{
			tipId = 1944;
		}
		else if(loginItem.status == 3)
		{
			tipId = 1945;
		}
		else if(loginItem.status == 0)
		{
			tipId = 1946;
		}
		if(tipId > 0 && null != mlb_status)
		{
			mlb_status.text = CSString.Format(tipId, loginItem.reward.tips);
		}
		mbtn_extract.onClick = this.OnClickAcquired;
		mbtn_extract.parameter = loginItem;
	}

	void OnClickAcquired(GameObject go)
	{
		if(UIEventListener.Get(go).parameter is SevenLoginItem loginItem)
		{
			FNDebug.Log($"[ÇëÇó½±Àø]:activityId = {loginItem.reward.activityId} goalId = {loginItem.reward.goalId}");
			Net.ReqSpecialActivityRewardMessage(loginItem.reward.activityId, loginItem.reward.id);
		}
	}
    #endregion

    #region choiced_awards_links
	void InitLinkItems(TABLE.SPECIALACTIVEREWARD loginReward)
	{
        if (null != mlb_theme)
            mlb_theme.text = CSString.Format(1947, loginReward.tips);

		var tipItems = CSSevenLoginInfo.Instance.GetTipItems(loginReward.id % 100);
		var it = tipItems.GetEnumerator();
		mgrid_hints_transform.MakeActivedChildCountNoTemplate(tipItems.Count);
		for (int i = 0, max = mgrid_hints_transform.childCount; i < max; ++i)
        {
			var child = mgrid_hints_transform.GetChild(i);
			child.CustomActive(i < tipItems.Count);
			if(i < tipItems.Count)
			{
				it.MoveNext();
				OnTipItemVisible(child, it.Current.Value);
			}
		}
		mgrid_hints.Reposition();
    }

	void OnTipItemVisible(Transform child, List<TABLE.SPECIALACTIVITYTIP> tipItem)
	{
        UILabel lb_title = Get<UILabel>("lb_title", child);
        if (null != lb_title)
            lb_title.text = CSString.Format(1948, tipItem[0].desc);
        UIGrid grid_links = Get<UIGrid>("grid_links", child);
		var grid_links_transform = grid_links.transform;
		int cnt = tipItem.Count;
		grid_links_transform.MakeActivedChildCountNoTemplate(cnt);
		for(int i = 0,max = grid_links_transform.childCount;i < max;++i)
		{
            var c = grid_links_transform.GetChild(i);
            c.CustomActive(i < cnt);
			if(i < cnt)
			{
				UILabel lb_link = c.GetComponent<UILabel>();
				if(null != lb_link)
				{
					lb_link.text = $"[url=func:{(int)CSLinkFunction.CSLF_ACTIVITY_TIP_LINK}:tip:{tipItem[i].id}][u][007900]{tipItem[i].name}[-][/u][/url]";
					lb_link.SetupLink(null,UIManager.Instance.ClosePanelImmediately<UIServerActivityPanel>);
				}
			}
        }
		grid_links.Reposition();
	}
	#endregion

	protected void InitChoicedData(SevenLoginItem loginItem)
    {
		InitAwards(loginItem.reward.reward);
		InitBtnAwardsStatus(loginItem);
		InitLinkItems(loginItem.reward);
		mScrollView.ScrollImmidate(0);
	}

    public override void Show()
	{
		base.Show();

		InitDayLabels();
	}
	
	protected override void OnDestroy()
	{
		if(null != mId2ItemBase)
		{
			for(var it = mId2ItemBase.GetEnumerator();it.MoveNext();)
			{
				UIItemManager.Instance.RecycleSingleItem(it.Current.Value);
			}
			mId2ItemBase.Clear();
			mId2ItemBase = null;
		}
		if(null != mId2ItemBaseChoiced)
		{
            for (var it = mId2ItemBaseChoiced.GetEnumerator(); it.MoveNext();)
            {
                UIItemManager.Instance.RecycleSingleItem(it.Current.Value);
            }
			mId2ItemBaseChoiced.Clear();
			mId2ItemBaseChoiced = null;
        }
		mAwards?.Clear();
		mAwards = null;
		mAwardsDic?.Clear();
		mAwardsDic = null;
		if(null != mgo_can_acquired)
		{
			CSEffectPlayMgr.Instance.Recycle(mgo_can_acquired);
			mgo_can_acquired = null;
		}
		base.OnDestroy();
	}
}
