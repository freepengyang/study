using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIDailyActivityTipsPanel : UIBasePanel
{
    private ActivityDataDisplay Display;
    private TABLE.ACTIVE ActivityCfg;

    private List<UIItemBase> rewardItemList = new List<UIItemBase>();

    public override void Init()
    {
        base.Init();

        UIEventListener.Get(mobj_mask).onClick = CloseClick;
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
        mlb_name.text = ActivityCfg.name;
        mlb_des.text = ActivityCfg.desc;
        mlb_level.text = ActivityCfg.level > 0 ? ActivityCfg.level.ToString() : "任意";

        if (Display != null)
        {
            int count = Display.activityCount;
            int maxcount = ActivityCfg.count != 0 ? ActivityCfg.count : ActivityCfg.bonusCount;
            mlb_count.text = $"{count}/{maxcount}";
            mlb_active.text = $"{count * ActivityCfg.bonus}/{ActivityCfg.bonusCount * ActivityCfg.bonus}";
            mlb_count.gameObject.SetActive(maxcount > 0);
        }
        else mlb_count.gameObject.SetActive(false);
        //mlb_count.gameObject.SetActive(Display != null && ActivityCfg.count > 0);
        mlb_active.gameObject.SetActive(Display != null && ActivityCfg.bonus > 0);


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
        mSp_bg.height = 288 + offset;
        mObj_fix.localPosition = new Vector2(-215, -7 - offset);
        mObj_scrollView.localPosition = new Vector2(-91, -36 - offset);

        if (Display != null) mlb_name.transform.localPosition = new Vector2(-213, 158);
        else mlb_name.transform.localPosition = new Vector2(-213, 148);
    }


    void ItemClick(UIItemBase item)
    {
        if (item != null)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, item.itemCfg, item.infos);
        }
    }


    void CloseClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIDailyActivityTipsPanel>();
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();

        UIItemManager.Instance.RecycleItemsFormMediator(rewardItemList);
    }
}
