using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActivityBasePanel : UIBase
{
	public override PrefabTweenType PanelTweenType
	{
		get => PrefabTweenType.NpcDialog;
	}
	public override bool ShowGaussianBlur
	{
		get { return false; }
	}
	private enum RandomThingState
	{
		/// <summary> 未开启</summary>
		NoOpen,
		/// <summary> 开启</summary>
		Open,
		/// <summary> 已结束</summary>
		Finish,
	}
	
    protected UILabel mlb_level;
    protected GameObject mbtn_enter;
	protected UIGrid mGrid;
	protected List<UIItemBase> tipList= new List<UIItemBase>();
	protected GameObject mlb_activity;
	RandomThingState state;
	UILabel contentTips;
	string str1 = "";
	string str2 = "";
	
	int countDown = 0;
	
	Schedule scheduleRepeat;
	protected override void _InitScriptBinder()
    {

        mlb_level = (ScriptBinder.GetObject("lb_level") as UnityEngine.GameObject)?.GetComponent<UILabel>();
        mbtn_enter = ScriptBinder.GetObject("btn_enter") as UnityEngine.GameObject;
		mGrid = ScriptBinder.GetObject("Grid") as UIGrid;
		mlb_activity = ScriptBinder.GetObject("lb_activity") as GameObject;
	}

	public override void Init()
    {
        base.Init();
        AddCollider();
        if (mlb_level == null|| mbtn_enter == null)
        {
            FNDebug.LogError("mlb_level 或 mbtn_enter 不能为空");
            return;
        }
		int lv = int.Parse(mlb_level.text);
        ColorType type = lv > CSMainPlayerInfo.Instance.Level ? ColorType.Red : ColorType.NPCImportantText;
        mlb_level.text = string.Format(mlb_level.FormatStr, lv).BBCode(type);
        UIEventListener.Get(mbtn_enter).onClick = EnterClick;
    }

    public virtual void EnterClick(GameObject obj)
    {

    }
    
    protected void RefreshUI(int type)
    {
	    state = RandomThingState.NoOpen;
	    var arr = TimerTableManager.Instance.array.gItem.handles;
	    List<int> startTimeList = new List<int>();
	    List<int> endTimeList = new List<int>();
	    string startStr = "";
	    string endStr = "";
	    int curTime = CSServerTime.Now.Hour * 10000 + CSServerTime.Now.Minute * 100 + CSServerTime.Now.Second;
	    contentTips = mlb_activity.GetComponent<UILabel>();
		TABLE.TIMER timerItem = null;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
			timerItem = arr[k].Value as TABLE.TIMER;

			if (timerItem.type == type)
		    {
			    startStr = TimerTableManager.Instance.GetTimerStartTime(timerItem.id);
			    endStr = TimerTableManager.Instance.GetTimerEndTime(timerItem.id);
			    startTimeList.Add(UtilityMath.CronTimeStringParseToHMS(startStr));
			    endTimeList.Add(UtilityMath.CronTimeStringParseToHMS(endStr));
		    }
	    }
	    for (int i=0;i < startTimeList.Count;i++)
	    {
		    if (curTime >= startTimeList[i] && curTime <= endTimeList[i])
		    {
			    state = RandomThingState.Open;
			    break;
		    }
	    }
	    if(curTime > GetEndTimeMax(endTimeList))
	    {
		    state = RandomThingState.Finish;
	    }
	    if(state == RandomThingState.NoOpen)
	    {
		    for (int i = 0; i < startTimeList.Count; i++)
		    {
				if (curTime < startTimeList[i])
			    {
					countDown = GetSeconds(startTimeList[i]) - GetSeconds(curTime);
					break;
			    }
		    }
		}
	    SetBtnUI(state);
    }
	private int GetSeconds(int time)
	{
		int h, m, s;
		int tempTime = 0;
		h = Mathf.FloorToInt(time / 10000);
		m = Mathf.FloorToInt((time - h * 10000) / 100);
		s = Mathf.FloorToInt(time - h * 10000 - m * 100);
		tempTime = h * 3600 + m * 60 + s;
		return tempTime;
	}
    private int GetEndTimeMax(List<int> endTimeList)
    {
	    int temp = 0;
	    for (int i=0;i< endTimeList.Count;i++)
	    {
		    if (endTimeList[i] > temp)
		    {
			    temp = endTimeList[i];
			    continue;
		    }
	    }
	    return temp;
    }
    
	private void SetBtnUI(RandomThingState state)
	{
		string colorRed = UtilityColor.GetColorString(ColorType.Red);
		switch (state)
		{
			case RandomThingState.NoOpen:
				mlb_activity.SetActive(true);
				mbtn_enter.SetActive(false);
				CancelDelayInvoke();
				str1 = ClientTipsTableManager.Instance.GetClientTipsContext(1113);
				//scheduleRepeat = Timer.Instance.InvokeRepeating(0f, 1f, ScheduleReapeat);
				ScriptBinder.InvokeRepeating(0f, 1f, ScheduleReapeat);
				break;
			case RandomThingState.Open:
				mlb_activity.SetActive(false);
				mbtn_enter.SetActive(true);
				break;
			case RandomThingState.Finish:
				mlb_activity.SetActive(true);
				mbtn_enter.SetActive(false);
				contentTips.text = ClientTipsTableManager.Instance.GetClientTipsContext(1114);
				break;
		}
	}
    
	void ScheduleReapeat()
	{
		if (countDown > 0)
		{
			str2 = CSServerTime.Instance.FormatLongToTimeStr(countDown,3);
			contentTips.text = CSString.Format(str1, str2);
			countDown -= 1;
		}
		else
		{
			ScriptBinder.StopInvokeRepeating();
			//CancelDelayInvoke();
			contentTips.text = "";
			mbtn_enter.gameObject.SetActive(true);
		}
	}
    
    protected override void Close()
    {
        UIManager.Instance.ClosePanel(GetType());
    }
	public void RefreshItem(string show)
	{
		if (mGrid)
		{
			string[] itemList = UtilityMainMath.StrToStrArr(show,'&');
			if (itemList == null) return;
			string[] mes;
			if (tipList != null)
			{
				UIItemManager.Instance.RecycleItemsFormMediator(tipList);
				tipList.Clear();
			}
			for (int i = 0; i < itemList.Length; i++)
			{
				mes = UtilityMainMath.StrToStrArr(itemList[i]);
				if (mes == null) return;
				tipList.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, mGrid.transform,itemSize.Size66));
				tipList[i].Refresh(ItemTableManager.Instance.GetItemCfg(int.Parse(mes[0])));
				//tipList[i].SetCount(int.Parse(mes[1]), CSColor.white);
			}
		}
		mGrid.Reposition();
	}
	
	private void CancelDelayInvoke()
	{
		if (Timer.Instance.IsInvoking(scheduleRepeat))
		{
			Timer.Instance.CancelInvoke(scheduleRepeat);
		}
	}
	
	// public override bool ShowGaussianBlur
 //    {
 //        get { return false; }
 //    }
	protected override void OnDestroy()
	{
		if (mGrid)
		{
			UIItemManager.Instance.RecycleItemsFormMediator(tipList);
			tipList.Clear();
			tipList = null;
		}

		//CancelDelayInvoke();
		state = 0;
		countDown = 0;

		str1 = "";
		str2 = "";
		contentTips = null;
		scheduleRepeat = null;
	}
}
