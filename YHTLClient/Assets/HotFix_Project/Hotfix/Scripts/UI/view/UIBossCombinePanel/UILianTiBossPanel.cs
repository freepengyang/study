
using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UILianTiBossPanel : UIBasePanel
{
	#region variable

	GameObject curCheckMark;
	List<lianTiBossItem> defeatList = new List<lianTiBossItem>();
	List<lianTiBossItem> previewList = new List<lianTiBossItem>();
	List<UIItemBase> rewardList = new List<UIItemBase>();
	Dictionary<int, int> dic;
	Dictionary<int, long> timeDic;
	lianTiBossItem curBossitem;
	UILabel mapName, lb_time;
	UISprite sp_enter;
	string btnName = string.Empty;
	string mapBtnName = string.Empty;//倒计时结束，显示按钮名
	long countDown = 0;
	#endregion

	public override void Init()
	{
		base.Init();
		CSEffectPlayMgr.Instance.ShowUITexture(mtex_bg, "challengeboss_bg");
		CSEffectPlayMgr.Instance.ShowUITexture(mtex_noBossbg, "pattern");
		UIEventListener.Get(mbtn_defeat, 1).onClick = ShowTypeChange;
		UIEventListener.Get(mbtn_preview, 2).onClick = ShowTypeChange;
	}
	public override void Show()
	{
		base.Show();
		Net.CSBossInfoMessage();
		Net.CSLianTiFieldMessage();
		mClientEvent.Reg((uint)CEvent.ECM_SCBossInfoMessage, GetDefeatData);
	}
	public override void OnShow(int typeId = 0)
	{
		base.OnShow(typeId);
	}
	public override void OnHide()
	{
		base.OnHide();
		defeatList.Clear();
		previewList.Clear();
		if (rewardList.Count > 0)
		{
			UIItemManager.Instance.RecycleItemsFormMediator(rewardList);
			rewardList.Clear();
		}
		mClientEvent.RemoveEvent(CEvent.ECM_SCBossInfoMessage, GetDefeatData);
	}
	void GetDefeatData(uint id, object data)
	{
		boss.ChallengeBossInfoResponse msg = (boss.ChallengeBossInfoResponse)data;
		RefreshDefeat(msg);
		RefreshPreview();
		ShowTypeChange(mbtn_defeat);
	}
	private void ShowTypeChange(GameObject _go)
	{
		int type = (int)UIEventListener.Get(_go).parameter;
		if (curCheckMark != null)
		{
			curCheckMark.SetActive(false);
		}

		curCheckMark = _go.transform.Find("Checkmark").gameObject;
		curCheckMark.SetActive(true);
		RefreshBossList(type);
	}
	private void RefreshBossList(int _type)
	{
		mscr_defeat.gameObject.SetActive(_type == 1);
		mscr_preview.gameObject.SetActive(_type == 2);
		if (_type == 1)
			SetShowAndClick(mgrid_defeat);
		else
			SetShowAndClick(mgrid_preview);
	}
	private void SetShowAndClick(UIGridContainer grid)
	{
		if (grid.controlList.Count > 0)
		{
			ShowNoBoss(false);
			ItemClick(grid.controlList[0]);
		}
		else
			ShowNoBoss(true);
	}
	private void ItemClick(GameObject _go)
	{
		if (curBossitem != null)
		{
			curBossitem.ChangeSelectState(false);
		}

		curBossitem = (lianTiBossItem)UIEventListener.Get(_go).parameter;
		curBossitem.ChangeSelectState(true);
		ShowModel();
		ShowMapBtn(curBossitem.MapIds());
		ShowReward();
	}
	void ShowModel()
	{
		mlb_bossName.text = MonsterInfoTableManager.Instance.GetMonsterInfoName(curBossitem.monsterid);
		mlb_bossName.color =
			UtilityCsColor.Instance.GetColor(
				MonsterInfoTableManager.Instance.GetMonsterInfoQuality(curBossitem.monsterid));

		CSEffectPlayMgr.Instance.ShowUIEffect(mlb_bossSprite.gameObject,
			$"{BossTableManager.Instance.GetBossShow(curBossitem.Id)}",
			ResourceType.UIMonster, 5);
	}
	void ShowReward()
	{
		mlb_bossLv.text = $"{MonsterInfoTableManager.Instance.GetMonsterInfoLevel(curBossitem.monsterid)}";
		mlb_refreshTime.text = BossTableManager.Instance.GetBossTime(curBossitem.Id);
		int pro = MonsterInfoTableManager.Instance.GetMonsterInfoPropertiesSuit(curBossitem.monsterid);
		CSBetterLisHot<int> info = new CSBetterLisHot<int>();
		MDropItemsTableManager.Instance.GetDropItemsByMonsterId(curBossitem.monsterid, info, 9);
		if (info.Count > rewardList.Count)
		{
			int gap = info.Count - rewardList.Count;
			for (int i = 0; i < gap; i++)
			{
				rewardList.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, mgrid_rewards.transform,
					itemSize.Size60));
			}
		}

		for (int i = 0; i < rewardList.Count; i++)
		{
			if (i >= info.Count)
			{
				rewardList[i].obj.SetActive(false);
				//rewardList[i].UnInit();
			}
			else
			{
				rewardList[i].obj.SetActive(true);
				rewardList[i].Refresh(info[i]);
			}
		}

		mgrid_rewards.Reposition();
		mscr_rewardScr.ResetPosition();
	}
	List<int> deliverIds = new List<int>();
	void ShowMapBtn(List<int> _mapIds)
	{
		long time, curTime;
		List<int> mapIds;
		if (_mapIds != null)
			mapIds = _mapIds;
		else
			mapIds = BossTableManager.Instance.GetMapIdsByMonsterId(curBossitem.monsterid);
		deliverIds.Clear();
		BossTableManager.Instance.GetDeliverIdsByMonsterId(curBossitem.monsterid, deliverIds);
		mgrid_mapbtns.MaxCount = mapIds.Count;
		curTime = CSServerTime.Instance.TotalMillisecond;
		for (int i = 0; i < mgrid_mapbtns.controlList.Count; i++)
		{
			int level = InstanceTableManager.Instance.GetInstanceLevel(mapIds[i]);
			sp_enter = mgrid_mapbtns.controlList[i].GetComponent<UISprite>();
			mapName = mgrid_mapbtns.controlList[i].transform.Find("Label").GetComponent<UILabel>();
			lb_time = mgrid_mapbtns.controlList[i].transform.Find("lb_time").GetComponent<UILabel>();
			mapBtnName = MapInfoTableManager.Instance.GetMapInfoName(mapIds[i]).BBCode(ColorType.TabCheck);
			if (curBossitem.Id > 0 && timeDic.ContainsKey(curBossitem.Id))
			{
				time = timeDic[curBossitem.Id];
				countDown = (time - curTime) / 1000;
				if (time > 0)
				{
					lb_time.text = CSString.Format(2030, CSServerTime.Instance.FormatLongToTimeStr(countDown, 19));
					mCSInvoke?.StopInvokeRepeating();
					mCSInvoke?.InvokeRepeating(0f, 1f, CountDown);
					sp_enter.spriteName = "tab_chat1";
					mapBtnName = MapInfoTableManager.Instance.GetMapInfoName(mapIds[i]).BBCode(ColorType.TabBackground);
				}
				else
					SetBtnON();
			}
			else
				SetBtnON();
			mapName.text = mapBtnName;
			UIEventListener.Get(mgrid_mapbtns.controlList[i], level).onClick = MapBtnClick;
		}
		mscr_mapbtn.ResetPosition();
	}
	private void CountDown()
	{
		if (countDown <= 0)
		{
			SetBtnON();
			mCSInvoke?.StopInvokeRepeating();
		}
		else
		{
			lb_time.text = CSString.Format(2030, CSServerTime.Instance.FormatLongToTimeStr(countDown, 19));
			countDown--;
		}
	}
	private void SetBtnON()
	{
		lb_time.text = "";
		sp_enter.spriteName = "btn_number3";
	}
	void MapBtnClick(GameObject _go)
	{
		int level = (int)UIEventListener.Get(_go).parameter;
		CSLianTiInfo.Instance.SetLianTiLandTind(level);
		UIManager.Instance.CreatePanel<UILianTiLandPanel>();
		UIManager.Instance.ClosePanel<UIBossCombinePanel>();
		HotManager.Instance.EventHandler.SendEvent(CEvent.GetLianTiLandInfo);
	}
	private void ShowNoBoss(bool _state)
	{
		mobj_noBoss.SetActive(_state);
		mobj_right.SetActive(!_state);
	}
	private void RefreshDefeat(boss.ChallengeBossInfoResponse msg)
	{
		if (dic == null) dic = new Dictionary<int, int>();
		if (timeDic == null) timeDic = new Dictionary<int, long>();
		for (int i = 0; i < msg.challengeBossInfo.Count; i++)
		{
			int id = msg.challengeBossInfo[i].id;
			long refreshTime = msg.challengeBossInfo[i].refreshTime;
			int monsId = BossTableManager.Instance.GetBossMonsterid(id);
			int bossType = BossTableManager.Instance.GetBossBossType(id);
			if (timeDic.ContainsKey(id))
				timeDic[id] = refreshTime;
			else
				timeDic.Add(id, refreshTime);
			if (bossType == 2)
			{
				if (!dic.ContainsKey(monsId))
				{
					dic.Add(monsId, id);
				}
			}
		}

		mgrid_defeat.MaxCount = dic.Count;
		int k = 0;
		var iter = dic.GetEnumerator();
		while (iter.MoveNext())
		{
			defeatList.Add(new lianTiBossItem(mgrid_defeat.controlList[k], ItemClick));
			defeatList[k].RefreshByMonsterId(iter.Current.Key, iter.Current.Value);
			k++;
		}
	}
	private void RefreshPreview()
	{
		Dictionary<int, int> monsterDic = BossTableManager.Instance.GetPreviewBossMes(2);
		mgrid_preview.MaxCount = monsterDic.Count;
		var iter = monsterDic.GetEnumerator();
		int i = 0;
		while (iter.MoveNext())
		{
			previewList.Add(new lianTiBossItem(mgrid_preview.controlList[i], ItemClick));
			previewList[i].RefreshByMonsterId(iter.Current.Key, iter.Current.Value);
			i++;
		}
	}
	protected override void OnDestroy()
	{
		CSEffectPlayMgr.Instance.Recycle(mtex_bg);
		CSEffectPlayMgr.Instance.Recycle(mtex_noBossbg);
		CSEffectPlayMgr.Instance.Recycle(mlb_bossSprite.gameObject);
		if (rewardList.Count > 0)
		{
			UIItemManager.Instance.RecycleItemsFormMediator(rewardList);
			rewardList.Clear();
		}
		mCSInvoke?.StopInvokeRepeating();
		mapName = null;
		lb_time = null;
		timeDic = null;
		dic = null;
		btnName = string.Empty;
		mapBtnName = string.Empty;
		countDown = 0;
		base.OnDestroy();
	}
	public class lianTiBossItem : IDispose
	{
		GameObject go;
		GameObject select;
		UISprite icon;
		UILabel lv;
		UILabel name;
		Action<GameObject> action;
		public int monsterid;
		public int Id = 0;
		public List<int> mapIds = new List<int>(20);

		public lianTiBossItem(GameObject _go, Action<GameObject> _dele)
		{
			go = _go;
			action = _dele;
			select = go.transform.Find("select").gameObject;
			icon = go.transform.Find("headitem").GetComponent<UISprite>();
			name = go.transform.Find("lb_name").GetComponent<UILabel>();
			lv = go.transform.Find("lb_lv").GetComponent<UILabel>();
			UIEventListener.Get(go, this).onClick = action;
		}
		public void RefreshByMonsterId(int _bossId, int _id = 0)
		{
			monsterid = _bossId;
			if (_id > 0)
			{
				Id = _id;
				//BossTableManager.Instance.GetMapsByMonsterId(_bossId, mapIds);
			}
			icon.spriteName = MonsterInfoTableManager.Instance.GetMonsterInfoHead(monsterid).ToString();
			lv.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1166),
				MonsterInfoTableManager.Instance.GetMonsterInfoLevel(monsterid));
			name.text = MonsterInfoTableManager.Instance.GetMonsterInfoName(monsterid);
			name.color = UtilityCsColor.Instance.GetColor(MonsterInfoTableManager.Instance.GetMonsterInfoQuality(monsterid));
		}
		public void ChangeSelectState(bool _state)
		{
			select.SetActive(_state);
		}
		public List<int> MapIds()
		{
			mapIds.Clear();
			BossTableManager.Instance.GetMapsByMonsterId(monsterid, mapIds);
			return mapIds;
		}
		public void Dispose()
		{
			go = null;
			select = null;
			icon = null;
			lv = null;
			name = null;
			action = null;
			monsterid = 0;
			Id = 0;
			mapIds.Clear();
		}
	}
}