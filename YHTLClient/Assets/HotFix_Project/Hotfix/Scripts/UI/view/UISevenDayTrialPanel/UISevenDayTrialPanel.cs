using System;
using UnityEngine;
using activity;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using TABLE;

public partial class UISevenDayTrialPanel : UIBasePanel
{
    SevenDayData sevenDayData;
    Map<int, CSBetterLisHot<GoalDatas>> SevenDayMap;
    //List<UIItemBase> itemBaseList;
    string[] tabTextList;
    int curTab;
    Vector3 oldPosition; //上次的位置
    CSBetterLisHot<GoalDatas> goalDataList;
    public override void Init()
	{
		base.Init();
        AddCollider();
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_7days.gameObject, "7days_bg");
        UIEventListener.Get(mbtn_close).onClick = Close;
        curTab = 0;
        mClientEvent.AddEvent(CEvent.SevenDayDataChange, OnSevenDayDataChange);
        tabTextList = SundryTableManager.Instance.GetSundryEffect(619).Split('#');
        
        mscrollview_task.verticalScrollBar.onChange.Add(new EventDelegate(() =>
        {
            Utility.OnChange(mscrollview_task.verticalScrollBar.value, mbtn_down);
        }));
        
        Utility.OnChange(mscrollview_task.verticalScrollBar.value,mbtn_down);
    }

    public override void Show()
    {
        //Debug.Log("Show");
        Net.CSSevenDayDataMessage();
        //请求数据
    }

    private void RefreshUI() {
        //Debug.Log("RefreshUI");
        sevenDayData = CSActivityInfo.Instance.SevenDayData;
        SevenDayMap = CSActivityInfo.Instance.GetSevenMapData();
        //显示左侧按钮
        mgrid_tab.MaxCount = SevenDayMap.Count;
        for (int i = 0; i < mgrid_tab.MaxCount; i++)
        {
            ScriptBinder binder = mgrid_tab.controlList[i];
            GameObject obj = mgrid_tab.controlList[i].gameObject;
            UIEventListener.Get(obj, i).onClick = OnTabClick;
            UISprite msp_lock = binder.GetObject("sp_lock") as UISprite;
            UISprite msp_complete = binder.GetObject("sp_complete") as UISprite;
            UILabel mlb_name = binder.GetObject("lb_name") as UILabel;
            GameObject redpoint = binder.GetObject("redpoint") as GameObject; 
            
            msp_lock.gameObject.SetActive(!CSActivityInfo.Instance.IsOpenTab(i));//判断该tab任务是否开启
            msp_complete.gameObject.SetActive(CSActivityInfo.Instance.IsFinishByType(i+1));//判断该分页任务是否已完成
            redpoint.SetActive(CSActivityInfo.Instance.IsHaveFinishByType(i+1));
            mlb_name.text = tabTextList[i];
        }
        OnTabClick();
        //显示奖励按钮
        var dic = NewbieActivityScheduleTableManager.Instance.array.gItem.id2offset;
        mgrid_reward.MaxCount = dic.Count;
        //Debug.Log("dic" + dic.Count);
        for (int i = 0; i < mgrid_reward.MaxCount; i++)
        {
            ScriptBinder binder = mgrid_reward.controlList[i];
            GameObject obj = mgrid_reward.controlList[i].gameObject;
            //显示状态
            UILabel mlb_point = binder.GetObject("lb_point") as UILabel;
            GameObject mtrans_check = binder.GetObject("check") as GameObject;
            GameObject mtrans_select = binder.GetObject("select") as GameObject; 
            //添加事件
            UIEventListener.Get(obj,i+1).onClick = OnRewardClick;
            bool isFinish = sevenDayData.score >= (dic[i + 1].Value as TABLE.NEWBIEACTIVITYSCHEDULE).requiresSore;
            bool isReceive = sevenDayData.scoreRewards.Contains(i + 1);
            mtrans_check.SetActive(isFinish && isReceive);
            mtrans_select.SetActive(isFinish && !isReceive);
            mlb_point.text = (dic[i + 1].Value as TABLE.NEWBIEACTIVITYSCHEDULE).requiresSore.ToString();
        }
        //显示奖励按钮
        mlb_point.text = sevenDayData.score.ToString();

    }
    
    //领取下方奖励
    private void OnRewardClick(GameObject obj)
    {
        
        int scheduleid = (int)UIEventListener.Get(obj).parameter;
        NEWBIEACTIVITYSCHEDULE NewBieData;
        if (NewbieActivityScheduleTableManager.Instance.TryGetValue(scheduleid, out NewBieData))
        {
            if (sevenDayData.score >= NewBieData.requiresSore && !sevenDayData.scoreRewards.Contains(scheduleid))
            {
                Net.CSSevenDayRewardMessage(0, scheduleid);
                return;
            }
            int boxid = int.Parse(NewBieData.rewards);
        

            UIManager.Instance.CreatePanel<UIUnsealRewardPanel>(f =>
            {
                (f as UIUnsealRewardPanel).Show(boxid);
            });
        }

    }

    private void OnSevenDayDataChange(uint uiEvtID, object data)
    {
        RefreshUI();
    }

    private void OnTabClick(GameObject obj = null)
    {
        mscrollview_task.ResetPosition();
        if (obj != null) {
            curTab = (int)UIEventListener.Get(obj).parameter;

            if (!CSActivityInfo.Instance.IsOpenTab(curTab)) {
                UtilityTips.ShowRedTips(ClientTipsTableManager.Instance.GetClientTipsContext(1128));
                return;
            }
            oldPosition = mobj_select.transform.localPosition;
            mobj_select.transform.localPosition = obj.transform.localPosition;
            mobj_select.GetComponentInChildren<UILabel>().text = tabTextList[curTab];
        }
        goalDataList = SevenDayMap[curTab + 1];
        mgrid_task.MaxCount = goalDataList.Count;
        mgrid_task.Bind<GoalDatas, UISevenDayTaskBar>(goalDataList.BetterLisToGoogleList(), mPoolHandleManager);
    }

    
    protected override void OnDestroy()
	{
		base.OnDestroy();
        mgrid_task.UnBind<UISevenDayTaskBar>();
        sevenDayData = null;
        SevenDayMap= null;
        //itemBaseList= null;
        tabTextList= null;
        curTab = 0;
        goalDataList = null;
	}

    // public override bool ShowGaussianBlur
    // {
    //     get { return false; }
    // }
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
}

public class UISevenDayTaskBar : UIBinder
{
    UILabel mlb_name;
    UIGridContainer mgrid_item;
    GameObject mbtn_draw;
    UISprite mbtn_go;
    GameObject msp_complete;
    GoalDatas _data;
    List<UIItemBase> UIItemBaseList;
    UITexture mtex_7daysslider;

    public override void Init(UIEventListener handle)
    {
        mlb_name = Get<UILabel>("lb_name");
        mgrid_item = Get<UIGridContainer>("Grid");
        msp_complete = Get<GameObject>("sp_complete");
        mbtn_draw = Get<GameObject>("btn_draw");
        mbtn_go = Get<UISprite>("btn_go");
        mtex_7daysslider = Get<UITexture>("7days_slider");
        
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_7daysslider.gameObject, "7days_slider");
    }

    public override void Bind(object data)
    {
        _data = (GoalDatas)data;
        int configId = _data.configId;
        NEWBIEACTIVITY newbie;
        if (NewbieActivityTableManager.Instance.TryGetValue(configId, out newbie))
        {
            //string [] paras = new string[]{newbie.requirePara1.ToString(),newbie.requirePara2.ToString()};//,paras )
            string nameInfo = newbie.desc2;

            int maxValue = newbie.count;//newbie.showType == 1 ? newbie.requirePara1 : 1;
            string processStr = CSString.Format(1042,_data.value,maxValue);
            string procress = _data.value >= maxValue ? processStr.BBCode(ColorType.Green) : processStr.BBCode(ColorType.Red);
            CSStringBuilder.Clear();
            mlb_name.text = CSStringBuilder.Append(nameInfo,procress).ToString();
            bool isDisable = newbie.deliver == 0 && newbie.uiModel == 0;
            mbtn_go.color = isDisable ? Color.black : Color.white;
            bool isFinish = _data.value >= newbie.count; //newbie.showType == 1 ?_data.value >= newbie.count : _data.value >= 1;
            bool isReceive = _data.reward == 1;

        
            mbtn_draw.gameObject.SetActive(isFinish && !isReceive);
            msp_complete.gameObject.SetActive(isFinish && isReceive);
            mbtn_go.gameObject.SetActive(!isFinish);

            Dictionary<int, int> BoxReward = NewbieActivityTableManager.Instance.GetitemMap(configId);
            Utility.GetItemByBoxid(BoxReward, mgrid_item, ref UIItemBaseList, itemSize.Size54);
            UIEventListener.Get(mbtn_draw.gameObject).onClick = OnDrawClick;
            UIEventListener.Get(mbtn_go.gameObject).onClick = OnGoClick;  
        }
        
    }
    
    private void OnGoClick(GameObject obj)
    {
        //int uimodel = NewbieActivityTableManager.Instance.GetNewbieActivityUiModel(_data.configId);
        NEWBIEACTIVITY newbie;
        if (NewbieActivityTableManager.Instance.TryGetValue(_data.configId,out newbie))
        {
            //UtilityPanel.JumpToPanel(uimodel);
        
            if (newbie.deliver != 0)
            {
                UtilityPath.FindWithDeliverId(newbie.deliver);
                UIManager.Instance.ClosePanel<UISevenDayTrialPanel>();
            }
            else if (newbie.uiModel != 0)
            {    
                UtilityPanel.JumpToPanel(newbie.uiModel);
                UIManager.Instance.ClosePanel<UISevenDayTrialPanel>();
            }
            
        }
        
    }

    private void OnDrawClick(GameObject obj)
    {
        //领取
        //Debug.Log("OnDrawClick" + _data.configId);
        Net.CSSevenDayRewardMessage(_data.configId, 0);
        
    }


    public override void OnDestroy()
    {
        if (UIItemBaseList != null)
        {
            UIItemManager.Instance.RecycleItemsFormMediator(UIItemBaseList);
            UIItemBaseList.Clear();
        }
        mlb_name = null;
        mgrid_item = null;
        mbtn_draw = null;
        mbtn_go = null;
        msp_complete = null;
        _data = null;
        UIItemBaseList = null;
        mtex_7daysslider = null;
    }
}




