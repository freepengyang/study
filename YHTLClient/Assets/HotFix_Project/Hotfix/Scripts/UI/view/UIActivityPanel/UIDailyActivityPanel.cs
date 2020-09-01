using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class UIDailyActivityPanel : UIBasePanel
{
    //日常活跃度面板
    
    protected UITexture msp_bg;
    //protected UITexture msp_bar;
    protected UISprite msp_bar;
    protected GameObject mobj_activity;
    protected UILabel mlb_activity;
    protected UIGridContainer mgrid_daily;
    protected GameObject mbtn_help;
    protected UITexture mtex_bar;
    int curActive = 0;
    private Dictionary<int, UIActivity> uiActivitiesDic;
    

    protected override void _InitScriptBinder()
    {
        msp_bg = ScriptBinder.GetObject("sp_bg") as UITexture;
        msp_bar = ScriptBinder.GetObject("sp_bar") as UISprite;
        //msp_bar = ScriptBinder.GetObject("sp_bar") as UITexture;
        
        
        mobj_activity = ScriptBinder.GetObject("obj_activity") as GameObject;
        mlb_activity = ScriptBinder.GetObject("lb_activity") as UILabel;
        mgrid_daily = ScriptBinder.GetObject("grid_daily") as UIGridContainer;
        mbtn_help = ScriptBinder.GetObject("btn_help") as GameObject;
        mtex_bar = ScriptBinder.GetObject("tex_bar") as UITexture;
    }

    private CSBetterList<UIActivityItem> leftUIItems = new CSBetterList<UIActivityItem>();
    private CSBetterList<ActivityDataDisplay> leftActivities = new CSBetterList<ActivityDataDisplay>();

    public override void Init()
    {
        _InitScriptBinder();
        mClientEvent.Reg((uint)CEvent.SCResActiveMessageRefresh, RefreshUI);
        mClientEvent.AddEvent(CEvent.DailyActiveTaskChange,RefreshRightListUI);
        //mClientEvent.Reg((uint)CEvent.DailyBtnRedPointCheck, OnCheckRedPoint);
        //mClientEvent.Reg((uint)CEvent.sc, ShopBuyTimesChange);
        UIEventListener.Get(mbtn_help).onClick = OnHelpClick;
        CSEffectPlayMgr.Instance.ShowUITexture(msp_bg.gameObject, "activitybg");
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_bar.gameObject, "activitybar");
        CSEffectPlayMgr.Instance.ShowUIEffect(msp_bar.gameObject, "effect_activity_bar_add");
    }

    public override void Show()
    {
        base.Show();
        RefreshLeftUI();
        RefreshRightListUI();
    }


    private void OnHelpClick(GameObject obj)
    {
        
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.DailyActive);
    }

    private void RefreshUI(uint uiEvtID, object data)
    {
        RefreshLeftUI();
    }

    private void RefreshRightListUI(uint uiEvtID, object data)
    {
        RefreshRightListUI();
    }

    //右侧活动列表ui
    void RefreshRightListUI()
    {
        var dic = CSActivityInfo.Instance.GetActiveDic(true);
        for (dic.Begin(); dic.Next();)
        {
            ActivityDataDisplay display = dic.Value;
            if (display == null || display.config == null || display.state == ActivityState.NotToday) continue;
            leftActivities.Add(display);
        }

        SortActivityCfgList(leftActivities);

        for (int i = 0; i < leftUIItems.Count; i++)
        {
            mPoolHandleManager.Recycle(leftUIItems[i]);
        }
        //FNDebug.Log($"当前缓存池长度{mPoolHandleManager.mPoolPairs.Count}");
        leftUIItems.Clear();

        mgrid_daily.MaxCount = leftActivities.Count;
        for (int i = 0; i < mgrid_daily.MaxCount; i++)
        {
            UIActivityItem item = mPoolHandleManager.GetCustomClass<UIActivityItem>();
            item.UIPrefab = mgrid_daily.controlList[i];
            item.InitItem(leftActivities[i],true);
            leftUIItems.Add(item);
        }

        //FNDebug.Log($"取出池子{mPoolHandleManager.mPoolPairs.Count}");
    }

    void RefreshLeftUI() {

        leftActivities.Clear();
        //leftUIItems.Clear();
        curActive = CSActivityInfo.Instance.Active;
        ShowUIActiveValue();
        mlb_activity.text = curActive.ToString();//当前活跃度的值
        
        int i = 0;
        var arr = ActiveRewardTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.ACTIVEREWARD;
            Transform trans = mobj_activity.transform.GetChild(i).transform;
            
            if (uiActivitiesDic == null)
            {
                uiActivitiesDic = mPoolHandleManager.GetSystemClass<Dictionary<int,UIActivity>>();
                uiActivitiesDic.Clear();
            }

            UIActivity uiActivity;
            if (uiActivitiesDic.ContainsKey(i))
            {
                uiActivity = uiActivitiesDic[i];
            }
            else
            {
                uiActivity = mPoolHandleManager.GetSystemClass<UIActivity>();
                uiActivity.init(trans);
                uiActivitiesDic.Add(i,uiActivity);
            }
            
            i++;
            
            UISprite msp_icon = uiActivity.msp_icon;
            Transform mobj_stamp = uiActivity.mobj_stamp;
            Transform mobj_select = uiActivity.mobj_select;
            UILabel mlb_num = uiActivity.mlb_num;
            GameObject mobj_repoint = uiActivity.mobj_repoint;
            UISprite msp_effect = uiActivity.msp_effect;

            if (msp_effect!= null&&msp_effect.atlas == null)
            {
                CSEffectPlayMgr.Instance.ShowUIEffect(msp_effect.gameObject, "effect_activity_select_add");
            }
            mlb_num.text = item.num;
            
            mobj_select.gameObject.SetActive(false);
            mobj_stamp.gameObject.SetActive(false);
            msp_effect.gameObject.SetActive(false);
            msp_icon.color = Color.white;
            var rewards = CSActivityInfo.Instance.Rewards;
            mobj_repoint.SetActive(false);
            
            
            if (rewards != null&&rewards.Contains(item.id))
            {
                UIEventListener.Get(msp_icon.gameObject).onClick = null;
                mobj_stamp.gameObject.SetActive(true);
                msp_icon.color = Color.black;
                //显示已领取
                UIEventListener.Get(msp_icon.gameObject, item).onClick = ShowRewardClick;
            }
            else {
                if (curActive >= int.Parse(item.num))
                {
                    //显示领取
                    mobj_repoint.SetActive(true);
                    mobj_select.gameObject.SetActive(true);
                    msp_effect.gameObject.SetActive(true);
                    
                    UIEventListener.Get(msp_icon.gameObject, item).onClick = ReceiveAwardClick;
                    
                }
                else
                {
                    UIEventListener.Get(msp_icon.gameObject, item).onClick = ShowRewardClick;
                }
            }
        }
        


    }
    void SortActivityCfgList(CSBetterList<ActivityDataDisplay> list)
    {
        if (list.Count < 2) return;
        list.Sort((a, b) =>
        {
            return a.state < b.state ? -1 : a.state > b.state ? 1 : (a.StartTimeHour * 60 + a.StartTimeMinute) - (b.StartTimeHour * 60 + b.StartTimeMinute);
        });
    }
    private void ShowRewardClick(GameObject obj)
    {
        
        TABLE.ACTIVEREWARD para = (TABLE.ACTIVEREWARD)UIEventListener.Get(obj).parameter;
        //Debug.Log("ShowRewardClick" + para.reward);
        UIManager.Instance.CreatePanel<UIDailyActivityRewardPanel>((f) =>
        {
            (f as UIDailyActivityRewardPanel).OpenPanel(para.reward);
        });

    }

    private void ReceiveAwardClick(GameObject obj)
    {
        

        TABLE.ACTIVEREWARD para = (TABLE.ACTIVEREWARD)UIEventListener.Get(obj).parameter;

        Dictionary<int, int> rewardDic = new Dictionary<int, int>();

        BoxTableManager.Instance.GetBoxAwardById(para.reward, rewardDic);
        //判断精力
        if (rewardDic.ContainsKey(12))
        {
            if (CSVigorInfo.Instance.IsVigorFull())
            {
                UtilityTips.ShowPromptWordTips(99,
                    () =>
                    {
                        UtilityPanel.JumpToPanel(12605);
                        UIManager.Instance.ClosePanel<UIActivityCombinedPanel>();
                    },
                    () =>
                    {
                        int lv = CSMainPlayerInfo.Instance.Level;
                        int deliverid = DeliverTableManager.Instance.GetSuggestDeliverId(lv);
                        UtilityPath.FindWithDeliverId(deliverid);
                        UIManager.Instance.ClosePanel<UIActivityCombinedPanel>();
                    });
                return;
            }
        }
        
        
        //Debug.Log("ReceiveAwardClick" +para.id);
        Net.ReqActiveRewardMessage(para.id);
    }

    /// <summary>
    /// 显示ui活跃度的值，
    /// </summary>
    private void ShowUIActiveValue() {
        float MaxactiveNum = float.Parse(SundryTableManager.Instance.GetSundryEffect(579));//活跃度当前值
        string[] ProgressFactors = SundryTableManager.Instance.GetSundryEffect(575).Split('#');
        int i = 0;
        float ProgressAddv = float.Parse(SundryTableManager.Instance.GetSundryEffect(580));
        msp_bar.fillAmount = 0;
        mtex_bar.fillAmount = 0;
        float ratio = -2f;
        float LastrewardNum = 0;
        //curActive = 180;
        TABLE.ACTIVEREWARD activerewardItem = null;
        var arr = ActiveRewardTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            activerewardItem = arr[k].Value as TABLE.ACTIVEREWARD;
            //Debug.Log(dic.Value.num + "||" + curActive);
            if (int.Parse(activerewardItem.num) > curActive)
            {
                //获取活跃度在该阶段的比值
                ratio = (curActive - LastrewardNum) / (float.Parse(activerewardItem.num) - LastrewardNum);
                //Debug.Log("ratio" + ratio + "|" + ProgressAddv + "|" + curActive + "|" + LastrewardNum + "|" + (float.Parse(dic.Value.num)));
                //Debug.Log("msp_bar" + (ProgressAddv + float.Parse(ProgressFactors[i]) * ratio));
                float value = (ProgressAddv + float.Parse(ProgressFactors[i]) * ratio) / 360;
                msp_bar.fillAmount = value;
                mtex_bar.fillAmount = value;
                return;
            }
            //最后一次循环判断activeNum 超过列表中的所有值
            if (i == max -1 && ratio < -1f)
            {
                ratio = (curActive - float.Parse(activerewardItem.num)) / (MaxactiveNum - float.Parse(activerewardItem.num));
                ProgressAddv += float.Parse(ProgressFactors[i]);
                float value = (ProgressAddv + float.Parse(ProgressFactors[i+1]) * ratio) / 360;
                msp_bar.fillAmount = value;
                mtex_bar.fillAmount = value;
            }
            
            ProgressAddv += float.Parse(ProgressFactors[i]);
            LastrewardNum = float.Parse(activerewardItem.num);
            i++;
        }

        //Debug.Log("msp_bar.fillAmount" + msp_bar.fillAmount);
    }

    public override void Dispose()
    {
        base.Dispose();
        msp_bg = null;
        mgrid_daily = null;
        msp_bar = null;
        mobj_activity = null;
        mlb_activity = null;
        leftUIItems = null;
        leftActivities = null;
        CSEffectPlayMgr.Instance.Recycle(msp_bar.gameObject);
    }

}

/// <summary>
/// 活跃度图标
/// </summary>
public class UIActivity
{
    //Transform trans = mobj_activity.transform.GetChild(i).transform;
    //i++;
    public UISprite msp_icon;
    public Transform mobj_stamp; 
    public Transform mobj_select;
    public UILabel mlb_num ;
    public GameObject mobj_repoint;
    public UISprite msp_effect;

    public void init(Transform trans)
    { 
        msp_icon = UtilityObj.Get<UISprite>(trans, "icon");
        mobj_stamp = UtilityObj.Get<Transform>(trans, "stamp");
        mobj_select = UtilityObj.Get<Transform>(trans, "select");
        mlb_num = UtilityObj.Get<UILabel>(trans, "lb_num");
        mobj_repoint = UtilityObj.Get<Transform>(trans, "redpoint").gameObject;
        msp_effect = UtilityObj.Get<UISprite>(trans,"selectEffect");
    }


}


