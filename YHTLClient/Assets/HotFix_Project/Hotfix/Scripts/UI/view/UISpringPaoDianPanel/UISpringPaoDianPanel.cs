using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UISpringPaoDianPanel : UIBasePanel
{
    private enum RandomThingState
    {
        /// <summary> 未开启</summary>
        NoOpen,
        /// <summary> 开启</summary>
        Open,
        /// <summary> 已结束</summary>
        Finish,
    }
    GameObject mlb_activity;
    RandomThingState state;


    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.NpcDialog;
    }
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }
    int instanceId = 0;
    FastArrayElementFromPool<UIItemBase> items;
    List<List<int>> rewardMes = new List<List<int>>();
    List<int> startTimeList = new List<int>();
    List<int> endTimeList = new List<int>();
    string startStr = "";
    string endStr = "";
    int countDown = 0;

    public override void Init()
    {
        base.Init();
        AddCollider();
        instanceId = InstanceTableManager.Instance.GetSpringPaoDianOpenId();
        items = mPoolHandleManager.CreateItemPool(PropItemType.Normal, mgrid_rewards.transform);
        UIEventListener.Get(mobj_bg).onClick = CloseClick;
        UIEventListener.Get(mbtn_enter).onClick = EnterBtnClick;
        rewardMes = UtilityMainMath.SplitStringToIntLists(InstanceTableManager.Instance.GetInstanceShow(instanceId));
        items.Clear();
        for (int i = 0; i < rewardMes.Count; i++)
        {
            var item = items.Append();
            item.Refresh(rewardMes[i][0]);
            item.SetCount(rewardMes[i][1]);
        }
        mgrid_rewards.Reposition();


        int curTime = CSServerTime.Now.Hour * 10000 + CSServerTime.Now.Minute * 100 + CSServerTime.Now.Second;
        TABLE.TIMER timerItem = null;
        var arr = TimerTableManager.Instance.array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            timerItem = arr[i].Value as TABLE.TIMER;

            if (timerItem.type == 4)
            {
                startStr = TimerTableManager.Instance.GetTimerStartTime(timerItem.id);
                endStr = TimerTableManager.Instance.GetTimerEndTime(timerItem.id);
                startTimeList.Add(UtilityMath.CronTimeStringParseToHMS(startStr));
                endTimeList.Add(UtilityMath.CronTimeStringParseToHMS(endStr));
            }
        }
        for (int i = 0; i < startTimeList.Count; i++)
        {
            if (curTime >= startTimeList[i] && curTime <= endTimeList[i])
            {
                state = RandomThingState.Open;
                mbtn_enter.SetActive(true);
                mlb_Acstate.text = "";
                break;
            }
        }
        if (curTime > GetEndTimeMax(endTimeList))
        {
            state = RandomThingState.Finish;
            mbtn_enter.SetActive(false);
            mlb_Acstate.text = ClientTipsTableManager.Instance.GetClientTipsContext(1114);
        }
        if (state == RandomThingState.NoOpen)
        {
            mbtn_enter.SetActive(false);
            for (int i = 0; i < startTimeList.Count; i++)
            {
                if (curTime < startTimeList[i])
                {
                    countDown = GetSeconds(startTimeList[i]) - GetSeconds(curTime);
                    break;
                }
            }
            ScriptBinder.InvokeRepeating(0f, 1f, ScheduleReapeat);
        }
    }
    void ScheduleReapeat()
    {
        if (countDown > 0)
        {
            string str2 = CSServerTime.Instance.FormatLongToTimeStr(countDown, 3);
            mlb_Acstate.text = CSString.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1113), str2);
            countDown -= 1;
        }
        else
        {
            ScriptBinder.StopInvokeRepeating();
            //CancelDelayInvoke();
            mlb_Acstate.text = "";
            mbtn_enter.gameObject.SetActive(true);
        }
    }
    private int GetEndTimeMax(List<int> endTimeList)
    {
        int temp = 0;
        for (int i = 0; i < endTimeList.Count; i++)
        {
            if (endTimeList[i] > temp)
            {
                temp = endTimeList[i];
                continue;
            }
        }
        return temp;
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

    public override void Show()
    {
        base.Show();
    }
    void CloseClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UISpringPaoDianPanel>();
    }
    void EnterBtnClick(GameObject _go)
    {
        //Debug.Log(InstanceTableManager.Instance.GetSpringPaoDianOpenId());
        UIManager.Instance.ClosePanel<UISpringPaoDianPanel>();
        Net.ReqEnterInstanceMessage(instanceId);
    }

    protected override void OnDestroy()
    {
        rewardMes.Clear();
        rewardMes = null;
        items?.Clear();
        items = null;
        base.OnDestroy();
    }
}
