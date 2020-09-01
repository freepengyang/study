using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class UIUpcomingActivitiesPanel : UIBasePanel
{
    ILBetterList<UpComingACItem> items = new ILBetterList<UpComingACItem>();
    int MAXCOUNT = 2;
    ILBetterList<ActivityDataDisplay> dataList;
    private bool isFoldOut = true;//是否折叠
    Vector3 notFold = new Vector3(0, 180, 0);
    Vector3 fold = Vector3.zero;

    /// <summary>
    /// 超过多少个数才折叠
    /// </summary>
    const int minCountOfFolds = 1;

    public override void Init()
    {
        base.Init();
        UIEventListener.Get(mbtn_arrow).onClick = FoldBtnClick;
        mClientEvent.AddEvent(CEvent.ActivityBubbleChange, GetAcsChange);
        mClientEvent.AddEvent(CEvent.Setting_PushActivityChange, PushActivitySettingEvent);

        RefreshAcs();
        PushActivitySettingEvent(0, null);
    }

    public override void Show()
    {
        base.Show();
    }
    protected override void OnDestroy()
    {
        dataList = null;
        base.OnDestroy();

    }
    void GetAcsChange(uint id, object data)
    {
        RefreshAcs();
    }
    void RefreshAcs()
    {
        dataList = CSActivityRemindInfo.Instance.GetShowBubbleList();
        mgrid_Acs.MaxCount = MAXCOUNT = dataList.Count;
        int gap = MAXCOUNT - items.Count;
        for (int i = 0; i < gap; i++)
        {
            items.Add(new UpComingACItem());
        }
        for (int i = 0; i < MAXCOUNT; i++)
        {
            items[i].Init(mgrid_Acs.controlList[i]);
            items[i].Refresh(dataList[i]);
            if (isFoldOut && i > minCountOfFolds - 1)
            {
                items[i].go.SetActive(false);
            }
            else
            {
                items[i].go.SetActive(true);
            }
        }
        StartCor();
        RefershArrow();
    }
    void RefershArrow()
    {
        mobj_arrow.localEulerAngles = isFoldOut == true ? fold : notFold;
        mbtn_arrow.CustomActive(MAXCOUNT > minCountOfFolds);
        mobj_redpoint.CustomActive(MAXCOUNT > minCountOfFolds && isFoldOut);
        if (MAXCOUNT > minCountOfFolds)
        {
            var cellWidth = Mathf.Abs(mgrid_Acs.CellWidth);
            if (isFoldOut)
            {
                mobj_arrow.localPosition = new Vector3(-183, -186, 0);
            }
            else
            {
                mobj_arrow.localPosition = new Vector3(-183 - (cellWidth * (MAXCOUNT - 1)), -186, 0);
            }
            mlb_count.text = MAXCOUNT.ToString();
        }
    }
    
    void FoldBtnClick(GameObject _go)
    {
        isFoldOut = !isFoldOut;
        RefreshAcs();
    }
    void StartCor()
    {
        if (MAXCOUNT > 0)
        {
            ScriptBinder.InvokeRepeating(1, 1, ScheduleReapeat);
        }
        else
        {
            ScriptBinder.StopInvokeRepeating();
        }
    }
    void ScheduleReapeat()
    {
        for (int i = 0; i < MAXCOUNT; i++)
        {
            items[i].RunTimer();
        }
    }

    void PushActivitySettingEvent(uint id, object param)
    {
        if (CSConfigInfo.Instance.GetBool(ConfigOption.PushActivity))
        {
            CSActivityRemindInfo.Instance.CheckActivitis();
            ScriptBinder.InvokeRepeating2(1, 1, CheckShowActivities);
        }
        else
        {
            ScriptBinder.StopInvokeRepeating2();
        }
    }

    void CheckShowActivities()
    {
        if (CSServerTime.Now.Second != 0) return;//非整分钟的时候不检测，减少开销
        CSActivityRemindInfo.Instance.CheckActivitis();
    }


    class UpComingACItem
    {
        public GameObject go;
        UISprite icon;
        UILabel tips;
        UILabel name;
        Action<UpComingACItem> action;
        ActivityDataDisplay data;

        long timerStamp;
        string exTimerStr;

        public UpComingACItem()
        {

        }
        public void Init(GameObject _go)
        {
            go = _go;
            icon = go.transform.Find("sp_icon").GetComponent<UISprite>();
            tips = go.transform.Find("lb_tips").GetComponent<UILabel>();
            name = go.transform.Find("Label").GetComponent<UILabel>();
            UIEventListener.Get(go).onClick = OnClick;
        }
        public void Refresh(ActivityDataDisplay _data)
        {
            data = _data;
            if (data != null)
            {
                icon.spriteName = data.config.icon;
                name.text = data.config.name;
                timerStamp = data.PreviewType == 1 ? data.StartTimeFakeStamp : data.FinishTimeFakeStamp;
                exTimerStr = data.PreviewType == 1 ? ClientTipsTableManager.Instance.GetClientTipsContext(724) : ClientTipsTableManager.Instance.GetClientTipsContext(725);
                RunTimer();
            }
        }
        public void RunTimer()
        {
            if (data != null)
            {
                long timer = timerStamp - CSServerTime.Instance.TotalSeconds;
                if (timer < 0)
                {
                    go.CustomActive(false);
                    return;
                }
                var timerStr = /*timer > 3600 ? CSServerTime.Instance.FormatLongToTimeStrHour(timer) : */CSServerTime.Instance.FormatLongToTimeStrMin(timer);
                tips.text = timer >= 0 ? CSString.Format(exTimerStr, timerStr) : "";
            }
            else go.CustomActive(false);

        }

        public void OnClick(GameObject _item)
        {

            if (data == null || data.config == null) return;
            if (data.state == ActivityState.CanJoin)
                UtilityTips.ShowPromptWordTips(13, JoinActivity, data.config.name.BBCode(ColorType.Green));
            else
                UtilityTips.ShowTips(CSString.Format(2011, data.config.name));
        }

        void JoinActivity()
        {
            if (data == null || data.config == null) return;

            if (data.config.deliver != 0)
            {
                UtilityPath.FindWithDeliverId(data.config.deliver);
            }
            else if (data.config.uiModel != 0)
            {
                UtilityPanel.JumpToPanel(data.config.uiModel);
            }
        }

    }
}

