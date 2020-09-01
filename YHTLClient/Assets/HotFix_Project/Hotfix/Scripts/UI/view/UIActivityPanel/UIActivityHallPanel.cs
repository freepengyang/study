using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIActivityHallPanel : UIBasePanel
{
    private UIGridContainer _grid_left;
    private UIGridContainer grid_left { get { return _grid_left ?? (_grid_left = Get<UIGridContainer>("center/view/left/Scroll View/Grid")); } }

    private UIGridContainer _grid_right;
    private UIGridContainer grid_right { get { return _grid_right ?? (_grid_right = Get<UIGridContainer>("center/view/right/Scroll View/Grid")); } }


    private ILBetterList<ActivityDataDisplay> leftActivities = new ILBetterList<ActivityDataDisplay>();
    private ILBetterList<ActivityDataDisplay> rightActivities = new ILBetterList<ActivityDataDisplay>();

    private ILBetterList<UIActivityItem> leftUIItems = new ILBetterList<UIActivityItem>();
    private ILBetterList<UIActivityItem> rightUIItems = new ILBetterList<UIActivityItem>();




    public override void Init()
    {
        base.Init();

        InitActivitiesConfig();
        
        mClientEvent.AddEvent(CEvent.DailyActiveTaskChange, DisplayChange);

    }


    public override void Show()
    {
        base.Show();


        DisplayChange(0, null);
    }



    void RefreshLeftListUI()
    {
        for (int i = 0; i < leftUIItems.Count; i++)
        {
            mPoolHandleManager.Recycle(leftUIItems[i]);
        }
        leftUIItems.Clear();

        SortLeftList(leftActivities);

        grid_left.MaxCount = leftActivities.Count;
        for (int i = 0; i < grid_left.MaxCount; i++)
        {
            UIActivityItem item = mPoolHandleManager.GetCustomClass<UIActivityItem>();
            item.UIPrefab = grid_left.controlList[i];
            item.InitItem(leftActivities[i]);
            leftUIItems.Add(item);
        }
    }

    void RefreshRightListUI()
    {
        for (int i = 0; i < rightUIItems.Count; i++)
        {
            mPoolHandleManager.Recycle(rightUIItems[i]);
        }
        rightUIItems.Clear();

        SortRightList(rightActivities);

        grid_right.MaxCount = rightActivities.Count;
        for (int i = 0; i < grid_right.MaxCount; i++)
        {
            UIActivityItem item = mPoolHandleManager.GetCustomClass<UIActivityItem>();
            item.UIPrefab = grid_right.controlList[i];
            item.InitItem(rightActivities[i]);
            rightUIItems.Add(item);
        }
    }


    void InitActivitiesConfig()
    {
        leftActivities.Clear();
        rightActivities.Clear();
        var dic = CSActivityInfo.Instance.GetCommonDic(true);
        for (dic.Begin(); dic.Next();)
        {
            ActivityDataDisplay display = dic.Value;
            if (display == null || display.config == null || display.state == ActivityState.NotToday) continue;
            leftActivities.Add(display);
        }

        dic = CSActivityInfo.Instance.GetTimeLimitDic(true);
        for (dic.Begin(); dic.Next();)
        {
            ActivityDataDisplay display = dic.Value;
            if (display == null || display.config == null || display.state == ActivityState.NotToday) continue;
            rightActivities.Add(display);
        }

        dic = CSActivityInfo.Instance.GetSpecialDic(true);
        for (dic.Begin(); dic.Next();)
        {
            ActivityDataDisplay display = dic.Value;
            if (display == null || display.config == null || display.state == ActivityState.NotToday) continue;
            rightActivities.Add(display);
        }

    }


    void SortLeftList(ILBetterList<ActivityDataDisplay> list)
    {
        if (list.Count < 2) return;
        list.Sort((a, b) =>
        {
            return a.state != b.state ? a.state - b.state : a.config.ordering != b.config.ordering ? a.config.ordering - b.config.ordering : a.id - b.id;
        });
    }


    void SortRightList(ILBetterList<ActivityDataDisplay> list)
    {
        if (list.Count < 2) return;
        list.Sort((a, b) =>
        {
            int startTimeA = a.StartTimeHour * 100 + a.StartTimeMinute;
            int startTimeB = b.StartTimeHour * 100 + b.StartTimeMinute;
            return a.state != b.state ? a.state - b.state : startTimeA != startTimeB ? startTimeA - startTimeB : a.id - b.id;
        });
    }


    void ScheduleReapeat(Schedule schedule)
    {
        for (int i = 0; i < rightActivities.Count; i++)
        {
            CSActivityInfo.Instance.CheckActivityState(rightActivities[i]);
        }
        SortRightList(rightActivities);
        RefreshRightListUI();
    }


    void DisplayChange(uint id, object data)
    {
        RefreshLeftListUI();
        RefreshRightListUI();
    }


    protected override void OnDestroy()
    {
        //CancelDelayInvoke();
        //mPoolHandleManager.RecycleAll();
        base.OnDestroy();
    }

    //public override void OnHide()
    //{
    //    CancelDelayInvoke();
    //    base.OnHide();
    //}
}


public class UIActivityItem : UIBase, IDispose
{
    #region forms
private GameObject _btn_bg;
    private GameObject btn_bg { get { return _btn_bg ?? (_btn_bg = Get<GameObject>("bg")); } }

    private GameObject _btn_get;
    private GameObject btn_get { get { return _btn_get ?? (_btn_get = Get<GameObject>("btn_upgarde")); } }

    private UISprite _sp_btnGet;
    private UISprite sp_btnGet { get { return _sp_btnGet ?? (_sp_btnGet = Get<UISprite>("btn_upgarde")); } }

    private UILabel _lb_btnGet;
    private UILabel lb_btnGet { get { return _lb_btnGet ?? (_lb_btnGet = Get<UILabel>("btn_upgarde/lb_desc")); } }

    private UILabel _name;
    private UILabel mName { get { return _name ?? (_name = Get<UILabel>("lb_name")); } }

    private UILabel _lb_count;
    private UILabel lb_count { get { return _lb_count ?? (_lb_count = Get<UILabel>("lb_count")); } }

    private UILabel _lb_opentime;
    private UILabel lb_opentime { get { return _lb_opentime ?? (_lb_opentime = Get<UILabel>("lb_opentime")); } }

    private UILabel _lb_active;
    private UILabel lb_active { get { return _lb_active ?? (_lb_active = Get<UILabel>("lb_active")); } }

    private UILabel _lb_condition;
    private UILabel lb_condition { get { return _lb_condition ?? (_lb_condition = Get<UILabel>("lb_hint")); } }

    private GameObject _obj_complete;
    private GameObject obj_complete { get { return _obj_complete ?? (_obj_complete = Get<GameObject>("sp_complete")); } }

    private UISprite _sp_icon;
    private UISprite sp_icon { get { return _sp_icon ?? (_sp_icon = Get<UISprite>("ItemBase/sp_itemicon")); } }

    private UISprite _sp_flag;
    private UISprite sp_flag { get { return _sp_flag ?? (_sp_flag = Get<UISprite>("ItemBase/flag")); } }
#endregion

    private ActivityDataDisplay Display;
    private TABLE.ACTIVE ActiveCfg;
    private bool isActivity = false; //是否是活跃度
    public void InitItem(ActivityDataDisplay display,bool isActivity = false)
    {
        if (display == null || display.config == null)
        {
            FNDebug.LogError("@@@@@display空:" + display.id);
            return;
        }
        Display = display;
        ActiveCfg = Display.config;
        this.isActivity = isActivity;
        UIEventListener.Get(btn_bg).onClick = ItemOnClick;
        UIEventListener.Get(btn_get).onClick = GetBtnOnClick;

        UIPrefab.name = $"Activity_{ActiveCfg.id}";
        
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (ActiveCfg == null || Display == null) return;

        mName.text = ActiveCfg.name;
        sp_icon.spriteName = ActiveCfg.icon;
        

        if (ActiveCfg.recommend != 0)
        {
            sp_flag.spriteName = ActiveCfg.recommend.ToString();
            sp_flag.gameObject.SetActive(true);
        }
        else sp_flag.gameObject.SetActive(false);

        
        int count =  isActivity ?  Display.activityCount : Display.completeTimes;

        lb_opentime.gameObject.SetActive(false);
        lb_count.gameObject.SetActive(false);
        if (ActiveCfg.time != 0 && !isActivity)//限时
        {
            string sStr = string.Format("{0}:{1}", Display.StartTimeHour.ToString("D2"), Display.StartTimeMinute.ToString("D2"));
            string eStr = string.Format("{0}:{1}", Display.EndTimeHour.ToString("D2"), Display.EndTimeMinute.ToString("D2"));
            lb_opentime.text = $"{sStr}-{eStr}";
            //lb_opentime.transform.localPosition = new Vector2(132, -43);
            lb_opentime.gameObject.SetActive(!isActivity);

        }
        else
        {
            if (ActiveCfg.count > 0 || ActiveCfg.bonusCount >0)
            {
                lb_count.gameObject.SetActive(true);
                int maxcount = ActiveCfg.count != 0 ? ActiveCfg.count : ActiveCfg.bonusCount;
                int leftCount = 0;
                if (!isActivity)
                {
                    leftCount = maxcount - count < 0 ? 0 : maxcount - count;
                    lb_count.color = leftCount < 1 ? CSColor.red : CSColor.green;
                }
                else
                {
                    leftCount = count;
                    lb_count.color = CSColor.green;
                }

                lb_count.text = $"{leftCount}/{maxcount}";
                
            }

            if (ActiveCfg.bonus > 0)
            {
                lb_active?.gameObject.SetActive(true);
                if (lb_active != null)
                lb_active.text = $"{count * ActiveCfg.bonus}/{ActiveCfg.bonusCount * ActiveCfg.bonus}";
            }
            else lb_active?.gameObject.SetActive(false);
        }

        

        obj_complete.SetActive(Display.state == ActivityState.Completed);

        lb_condition.gameObject.SetActive(false);
        switch (Display.state)
        {
            case ActivityState.MismatchCondition:
                lb_condition.text = CSString.Format(2010, ActiveCfg.level).BBCode(ColorType.Red);
                lb_condition.gameObject.SetActive(true);
                break;
            case ActivityState.OpenSoon:
                lb_condition.text = CSActivityInfo.Instance.NotOpenText;
                lb_condition.gameObject.SetActive(true);
                break;
            case ActivityState.Missed:
                lb_condition.text = CSActivityInfo.Instance.IsFinishText;
                lb_condition.gameObject.SetActive(true);
                break;
        }
        //if (Display.state == ActivityState.MismatchCondition)
        //{
        //    lb_condition.text = CSString.Format(2010, ActiveCfg.level).BBCode(ColorType.Red);
        //}

        btn_get.SetActive(Display.state == ActivityState.CanJoin);
    }


    void ItemOnClick(GameObject _go)
    {
        
        if (Display == null) return;

        if (isActivity)
        {
            UIManager.Instance.CreatePanel<UIDailyActivityTipsPanel>((f) =>
            {
                (f as UIDailyActivityTipsPanel).InitPanel(Display);
            });
        }
        else
        {
            
            UIManager.Instance.CreatePanel<UIActivityHalllTipsPanel>((f) =>
            {
                (f as UIActivityHalllTipsPanel).InitPanel(Display);
            });
        }
        
        
    }

    void GetBtnOnClick(GameObject _go)
    {
        if (Display.state == ActivityState.CanJoin)//跳转
        {
            UIManager.Instance.ClosePanel<UIActivityCombinedPanel>();
            if (ActiveCfg.deliver != 0)
            {
                UtilityPath.FindWithDeliverId(ActiveCfg.deliver);
            }
            else if (ActiveCfg.uiModel != 0)
            {
                UtilityPanel.JumpToPanel(ActiveCfg.uiModel);
            }
        }
        else ItemOnClick(_go);
    }

    public override void Dispose()
    {
        _btn_bg = null;
        _btn_get = null;
        _sp_btnGet = null;
        _lb_btnGet = null;
        _name = null;
        _lb_count = null;
        _lb_opentime = null;
        _lb_active = null;
        _lb_condition = null;
        _obj_complete = null;
        _sp_icon = null;
        _sp_flag = null;

        Display = null;
        ActiveCfg = null;
        base.Dispose();
    }
}
