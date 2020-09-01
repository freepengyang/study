using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIActivityHalllTipsPanel : UIBasePanel
{
    private ActivityDataDisplay Display;
    private TABLE.ACTIVE ActivityCfg;

    private List<UIItemBase> rewardItemList = new List<UIItemBase>();

    public override void Init()
    {
        base.Init();

        UIEventListener.Get(mobj_mask).onClick = CloseClick;
        mBtn_go.onClick = GoBtnClick;
    }
    

    public void InitPanel(ActivityDataDisplay display)
    {
        if (display == null) return;
        Display = display;
        ActivityCfg = Display.config;

        RefreshUI();
    }


    public void InitPanel(TABLE.ACTIVE activityCfg)
    {
        if (activityCfg == null) return;
        ActivityCfg = activityCfg;
        Display = null;

        RefreshUI();
    }



    void RefreshUI()
    {
        mSp_icon.spriteName = ActivityCfg.icon;
        mlb_name.text = ActivityCfg.name;
        mlb_des.text = ActivityCfg.desc;
        mlb_level.text = ActivityCfg.level > 0 ? ActivityCfg.level.ToString() : "任意";

        if (ActivityCfg.recommend != 0)
        {
            mSp_flag.spriteName = ActivityCfg.recommend.ToString();
            mSp_flag.gameObject.SetActive(true);
        }
        else mSp_flag.gameObject.SetActive(false);

        if (Display != null)
        {
            int count = Display.completeTimes;
            int maxcount = ActivityCfg.count != 0 ? ActivityCfg.count : ActivityCfg.bonusCount;
            int leftCount = maxcount - count < 0 ? 0 : maxcount - count;
            mlb_count.text = $"{leftCount}/{ActivityCfg.count}";
            mlb_active.text = $"{count * ActivityCfg.bonus}/{ActivityCfg.count * ActivityCfg.bonus}";
        }
        mlb_count.gameObject.SetActive(Display != null && ActivityCfg.count > 0 && ActivityCfg.time == 0);
        //mlb_active.gameObject.SetActive(Display != null && ActivityCfg.bonus > 0);
        mlb_active.gameObject.SetActive(false);


        if (ActivityCfg.time == 0)
        {
            mlb_time.text = "全天";
        }
        else
        {
            string startStr = TimerTableManager.Instance.GetTimerStartTime(ActivityCfg.time);
            string endStr = TimerTableManager.Instance.GetTimerEndTime(ActivityCfg.time);
            if (!string.IsNullOrEmpty(startStr) && !string.IsNullOrEmpty(endStr))
            {
                int startTime = UtilityMath.CronTimeStringParseToHMS(startStr);
                int endTime = UtilityMath.CronTimeStringParseToHMS(endStr);
                string sStr = UtilityMath.TimeIntToString(startTime);
                string eStr = UtilityMath.TimeIntToString(endTime);
                mlb_time.text = $"{sStr}-{eStr}";
            }
            else mlb_time.text = "全天";
        }

        ResetPosition();

        SetRewardItems();
    }


    void SetRewardItems()
    {
        if (ActivityCfg == null) return;
        string[] itemsStr = ActivityCfg.awards.Split('&');
        rewardItemList = UIItemManager.Instance.GetUIItems(itemsStr.Length, PropItemType.Normal, mGrid_reward.transform);
        if (rewardItemList != null)
        {
            for (int i = 0; i < itemsStr.Length; i++)
            {
                string[] iStr = itemsStr[i].Split('#');
                if (iStr.Length > 1)
                {
                    int id = 0;
                    //int count = 0;
                    int.TryParse(iStr[0], out id);
                    //int.TryParse(iStr[1], out count);
                    rewardItemList[i].Refresh(id, ItemClick);
                    //rewardItemList[i].SetCount(count, CSColor.white);
                }
            }
            mGrid_reward.Reposition();
            mScroll_reward.ResetPosition();
        }
    }


    void ResetPosition()
    {
        int offset = mlb_des.height - 25 > 0 ? mlb_des.height - 25 : 0;
        mObj_fix.localPosition = new Vector2(-216, -21 - offset);
        mObj_scrollView.localPosition = new Vector2(-96, -51 - offset);

        if (Display != null)
        {
            mlb_name.transform.localPosition = new Vector2(-133, 156);
            mTrans_line.localPosition = new Vector2(0, -98 - offset);
            mTrans_line.gameObject.SetActive(Display.state == ActivityState.CanJoin);
            mBtn_go.transform.localPosition = new Vector2(0, -137 - offset);
            mBtn_go.gameObject.SetActive(Display.state == ActivityState.CanJoin);
            mSp_bg.height = (Display.state == ActivityState.CanJoin ? 364 : 300) + offset;
        }
        else
        {
            mlb_name.transform.localPosition = new Vector2(-133, 146);
            mTrans_line.gameObject.SetActive(false);
            mBtn_go.gameObject.SetActive(false);
            mSp_bg.height = 300 + offset;
        }

    }


    void ItemClick(UIItemBase item)
    {
        if (item != null)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, item.itemCfg, item.infos);
        }
    }


    void GoBtnClick(GameObject _go)
    {
        if (Display != null && ActivityCfg != null && Display.state == ActivityState.CanJoin)//跳转
        {
            if (ActivityCfg.deliver != 0)
            {
                UtilityPath.FindWithDeliverId(ActivityCfg.deliver);
            }
            else if (ActivityCfg.uiModel != 0)
            {
                UtilityPanel.JumpToPanel(ActivityCfg.uiModel);
            }
        }

        UIManager.Instance.ClosePanel<UIActivityHalllTipsPanel>();
        UIManager.Instance.ClosePanel<UIActivityCombinedPanel>();
    }

    void CloseClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIActivityHalllTipsPanel>();
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();

        UIItemManager.Instance.RecycleItemsFormMediator(rewardItemList);
    }
}
