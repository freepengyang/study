using System;
using System.Collections.Generic;
//using System.ComponentModel;
using UnityEngine;
using TABLE;
using activity;

public partial class UIServerActivityRankPanel : UIBasePanel
{
    List<TaskStruct> TaskStructList = new List<TaskStruct>();
    int sealAcId = 10101;
    SpecialActivityData rewardmes;
    rank.RankInfo rankinfo;
    //Schedule schedule;
    long leftTime;
    int interval = 20;

    private int curPersonReward; //个人奖励当前显示的index;
    
    public override void Init()
	{
		base.Init();
        //AddCollider();
        mClientEvent.AddEvent(CEvent.RankInfoChange, EquipRankChange);
        mClientEvent.AddEvent(CEvent.ResSpecialActivityDataMessage , ChangeTaskGrid);
        
        rewardmes = CSOpenServerACInfo.Instance.Rewards[sealAcId];

        CSEffectPlayMgr.Instance.ShowUITexture(mtex_banner9.gameObject, "banner9");

        BindUItaskBar(1,mgrid_task);
        BindUItaskBar(2,mgrid_person);
        //显示箭头
        mscrollview_person.SetDynamicArrowHorizontal(mbtn_right,mbtn_left);
        mscrollview_rank.SetDynamicArrowVertical(mbtn_down);
        mscrollview_person.ResetPosition();
        mscrollview_rank.ResetPosition();
        UIEventListener.Get(mbtn_right).onClick = OnClickRight;
        UIEventListener.Get(mbtn_left).onClick = OnClickLeft;
        //EventDelegate.Add(mprogressv.onChange, OnChangedCur);
        //mprogressv.onChange = OnValueChanged;
    }

    // private void OnChangedCur()
    // {
    //     int value = (int) (mprogressv.value * (mgrid_person.MaxCount - 1));
    //     Debug.Log("value :" + value);
    //     if (curPersonReward != value)
    //     {
    //         curPersonReward = value;
    //     }
    //     
    // }


    private void OnClickRight(GameObject obj)
    {
        curPersonReward = UtilityMath.GetRoundingInt(mprogressv.value * (mgrid_person.MaxCount - 1));
        int count = mgrid_person.MaxCount;
        curPersonReward = curPersonReward > count-1  ? count : curPersonReward + 1;
        TweenBar(curPersonReward);
    }

    private void OnClickLeft(GameObject obj)
    {
        curPersonReward = UtilityMath.GetRoundingInt(mprogressv.value * (mgrid_person.MaxCount - 1));
        curPersonReward = curPersonReward < 1  ? 0 : curPersonReward - 1;
        TweenBar(curPersonReward);
    }
    private void ChangeTaskGrid(uint uievtid = 0, object data = null)
    {
        if (data!= null)
        {
            if (data is SpecialActivityData specialActivityData)
            {
                if (specialActivityData.activityId != sealAcId)
                {
                    return;
                }
            }
        }
        //根据奖励设置个人奖励列表起始位置
        curPersonReward = CSOpenServerACInfo.Instance.GetFirstPos();
        TweenBar(curPersonReward);
        
        //设置时间
        SetTime();
        
    }

    void SetTime()
    {
        if (CSOpenServerACInfo.Instance.Rewards.ContainsKey(sealAcId))
        {
            leftTime = UIServerActivityPanel.GetEndTime(sealAcId);
            //schedule = Timer.Instance.InvokeRepeating(0f, 1f, CountDown);
            
            mCSInvoke?.InvokeRepeating(0f, 1f, CountDown);
        }
    }

    void TweenBar(int targetPos)
    {
        float tagetV = (float) targetPos / (float) (mgrid_person.MaxCount - 1);//减去1 是因为显示的item数量为 1
        TweenProgressBar.Begin(mprogressv, 1, mprogressv.value, tagetV);
    }

    private void CountDown()
    {
        if (leftTime>=0)
        {
            mlb_time.text = CSString.Format(1108, CSServerTime.Instance.FormatLongToTimeStr(leftTime, 1));
            leftTime--;
        }
        else
        {
            mCSInvoke?.StopInvokeRepeating();
            // if (Timer.Instance.IsInvoking(schedule))
            //     Timer.Instance.CancelInvoke(schedule);
        }
        
        //mlb_time.text = CSString.Format(1108, CSServerTime.Instance.FormatLongToTimeStr(UIServerActivityPanel.GetEndTime(sealAcId), 1));
        //leftTime--;
        interval--;
        if (interval< 0 )
        {
            //Debug.Log("111111");
            //请求排行榜数据
            Net.ReqRankInfoMessage((int)RankType.EQUIT_SCORE);
            interval = 20;
        }
    }

    private void EquipRankChange(uint uiEvtID, object data)
    {
        //Debug.Log("data" + data);
        rank.RankInfo mes = (rank.RankInfo)data;
        if (mes.type != (int)RankType.EQUIT_SCORE)
            return;
        
        RefreshUI();
    }

    public void RefreshUI() {
        
        rankinfo = CSRankInfo.Instance.GetRankByType((int)RankType.EQUIT_SCORE);
        //if (rankinfo == null)
        //    return;
        mgrid_rank.MaxCount = 20;
        
        for (int i = 0; i <mgrid_rank.MaxCount; i++)
        {
            ScriptBinder binder = mgrid_rank.controlList[i];
            UILabel mlb_level =  binder.GetObject("lb_level") as UILabel; 
            UISprite msp_level = binder.GetObject("sp_level") as UISprite;  
            
            UILabel mlb_name =  binder.GetObject("lb_name") as UILabel; 
            UILabel mlb_count =  binder.GetObject("lb_count") as UILabel;
            GameObject Sprite = binder.GetObject("Sprite") as GameObject;
            
            Sprite.SetActive(i%2 == 0);
            
            msp_level.gameObject.SetActive(i < 3);
            msp_level.spriteName = i < 3 ? $"rank{i + 1}" : "";
            mlb_level.text = i < 3 ? "" : (i+1).ToString();
            
            mlb_name.text = i< rankinfo?.ranks.Count ? rankinfo?.ranks[i].name : CSString.Format(1135);
            mlb_count.text = i< rankinfo?.ranks.Count ? rankinfo?.ranks[i].value.ToString() :CSString.Format(1136);
            

        }
        mlb_rank.text = CSString.Format(1140);
        var iter = rankinfo.ranks.GetEnumerator();
        int index = 1;
        while (iter.MoveNext()) {
            if (iter.Current.rid == CSMainPlayerInfo.Instance.ID)
            {
                mlb_rank.text  += index.ToString();
                break;
            }
            index++;
        }
        if (mlb_rank.text == CSString.Format(1140))
            mlb_rank.text += CSString.Format(1115);

        mlb_point.text = CSString.Format(1141) + rewardmes.maxScore; 
    }

    public void BindUItaskBar(int rewardType,UIGridContainer gridContainer) {
        var taskData = SpecialActiveRewardTableManager.Instance.GetDataByIdAndType(10101, rewardType);
        gridContainer.MaxCount = taskData.Count;
        TaskStruct _taskStruct;
        TaskStructList.Clear();
        for (int i = 0; i < taskData.Count; i++)
        {

            _taskStruct.activityReward = taskData[i];
            _taskStruct.rewardmes = rewardmes;
            TaskStructList.Add(_taskStruct);
        }
        gridContainer.Bind<TaskStruct, UItaskBar>(TaskStructList, mPoolHandleManager);
        
    }


    public override void Show()
    {
        base.Show();
        
        //显示排行榜数据
        //RefreshUI();
        Net.ReqRankInfoMessage((int)RankType.EQUIT_SCORE);
    }

    public override void OnHide()
    {
        //Debug.Log("OnHide");
        mCSInvoke?.StopInvokeRepeating();
         // if (Timer.Instance.IsInvoking(schedule))
         //     Timer.Instance.CancelInvoke(schedule);
    }


    protected override void OnDestroy()
	{
        //Debug.Log("OnDestroy");
        
        mCSInvoke?.StopInvokeRepeating();
        
        // if (Timer.Instance.IsInvoking(schedule))
        //     Timer.Instance.CancelInvoke(schedule);
        //schedule = null;
        mgrid_task.UnBind<UItaskBar>();
        mgrid_person.UnBind<UItaskBar>();
        
        base.OnDestroy();
        
        
    }
}

public struct TaskStruct
{
    public SPECIALACTIVEREWARD activityReward;
    public SpecialActivityData rewardmes;
    //public int Rank;
}

public class UItaskBar : UIBinder
{
    UILabel mlb_name;
    UIGridContainer mgrid_item;
    GameObject mobj_complete;
    GameObject mbtn_draw;
    GameObject mbtn_up;
    TaskStruct _data;
    List<UIItemBase> UIItemBaseList = new List<UIItemBase>();
    int sealAcId = 10101;
    EventHanlderManager mClientEvent = new EventHanlderManager(EventHanlderManager.DispatchType.Event);
    public override void Init(UIEventListener handle)
    {
        mlb_name = Get<UILabel>("lb_name");
        mgrid_item = Get<UIGridContainer>("grid_item");
        mobj_complete = Get<GameObject>("sp_complete");
        mbtn_draw = Get<GameObject>("btn_draw");
        mbtn_up = Get<GameObject>("btn_up");
        mClientEvent.AddEvent(CEvent.ResSpecialActivityDataMessage, SpecialActivityChange);
    }

    public override void Bind(object data)
    {
        _data = (TaskStruct)data;
        var activityReward = _data.activityReward;

        string[] termNum = activityReward.termNum.Split('#');
        
        if (activityReward.rewardType == 1)
        {
            int ranklv = int.Parse(termNum[0]);
            int scorelv = int.Parse(termNum[1]);
            mlb_name.text = ranklv != -1 ?
                CSString.Format(1105, termNum[0], termNum[1]) : CSString.Format(1106, termNum[1]);
        }

        if (_data.activityReward.rewardType == 2)
        {
            int maxValue = int.Parse(activityReward.termNum);
            int goalid = _data.rewardmes.maxScore >= maxValue ? maxValue : _data.rewardmes.maxScore;
            mlb_name.text = CSString.Format(1107,activityReward.goalId, goalid, 
                activityReward.termNum);
        }

        
            
        
        Utility.GetItemByBoxid(activityReward.reward, mgrid_item, ref UIItemBaseList, itemSize.Size50);
        UIEventListener.Get(mbtn_draw).onClick = OnDrawClick;
        UIEventListener.Get(mbtn_up).onClick = OnUpClick;
        //RefreshUI();
    }

    private void RefreshUI() {
        int id = _data.activityReward.id;
        bool isReceive = _data.rewardmes.finishGoals.Contains(id);  //是否可以领取
        bool isRecieved = _data.rewardmes.rewardGoals.Contains(id);  //是否已领取
        //Debug.Log(isReceive +"||" + isRecieved);
        mbtn_draw?.SetActive(isReceive);
        mbtn_up?.SetActive(!isReceive && !isRecieved); //显示装备提升
        mobj_complete?.SetActive(isRecieved);
    }

    private void SpecialActivityChange(uint uiEvtID, object data)
    {
        _data.rewardmes = CSOpenServerACInfo.Instance.Rewards[sealAcId];
        RefreshUI();
    }

    private void OnUpClick(GameObject obj)
    {
        Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(617));
    }

    private void OnDrawClick(GameObject obj)
    {
        Net.ReqSpecialActivityRewardMessage(10101, _data.activityReward.id);
    }

    public override void OnDestroy()
    {
        //Debug.Log("OnDestroy 111");
        mlb_name = null;
        mgrid_item = null;
        mobj_complete = null;
        mbtn_draw = null;
        mbtn_up = null;
        if (UIItemBaseList != null)
        {
            UIItemManager.Instance.RecycleItemsFormMediator(UIItemBaseList);
            UIItemBaseList.Clear();
        }
        mClientEvent.RemoveEvent(CEvent.ResSpecialActivityDataMessage,SpecialActivityChange);
    }
}
