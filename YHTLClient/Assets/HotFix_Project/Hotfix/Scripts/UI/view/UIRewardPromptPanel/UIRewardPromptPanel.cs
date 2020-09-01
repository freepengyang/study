using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RewardPromptType {
    BackBtn, //有返回按钮
    Countdown, // 有倒计时
}
public partial class UIRewardPromptPanel : UIBase
{
    private List<StringData> listData;//item信息字符串
    System.Action clickAct;


    public override void Init()
    {
        base.Init();
        AddCollider();
        UIEventListener.Get(mbtn_back).onClick = OnBackClick;
        mClientEvent.AddEvent(CEvent.LeaveInstance, GetLeaveInstance);
        ShowEffect();
    }

    private void GetLeaveInstance(uint uiEvtID, object data)
    {
        Close();
    }
    

    public void ShowUI(int _instanceId, System.Action act,RewardPromptType type = RewardPromptType.BackBtn)
    {
        List<StringData> temp_list = mPoolHandleManager.GetSystemClass<List<StringData>>();
        temp_list.Clear();
        List<List<int>> reward_list = mPoolHandleManager.GetSystemClass<List<List<int>>>();
        reward_list.Clear();
        reward_list = UtilityMainMath.SplitStringToIntLists(InstanceTableManager.Instance.GetInstanceRewards(_instanceId));
        for (int i = 0; i < reward_list.Count; i++)
        {
            temp_list.Add(new StringData(reward_list[i][0], reward_list[i][1]));
        }
        listData = temp_list;
        clickAct = act;
        RefreshUI(listData,type);
    }

    public void ShowUI(List<StringData> listData, System.Action act,RewardPromptType type = RewardPromptType.BackBtn)
    {
        if (listData == null) return;
        this.listData = listData;
        clickAct = act;
        RefreshUI(listData,type);
    }

    public void RefreshUI(List<StringData> listData,RewardPromptType type = RewardPromptType.BackBtn)
    {
        Utility.SetRewardItems(listData, mGrid_reward.transform);
        mGrid_reward.Reposition();
        mScroll_reward.ResetPosition();
        if (type == RewardPromptType.BackBtn)
        {
            mbtn_back.SetActive(true);
            mlb_time.gameObject.SetActive(false);
        }
        else
        {
            mbtn_back.SetActive(false);
            mlb_time.gameObject.SetActive(true);
            time = int.Parse(SundryTableManager.Instance.GetSundryEffect(658));
            ScriptBinder.InvokeRepeating(0f, 1f, CountDown);
        }

    }

    private int time;
    private void OnBackClick(GameObject go)
    {
        if (clickAct != null) clickAct();
        UIManager.Instance.ClosePanel<UIRewardPromptPanel>();
        
        
    }

    private void CountDown()
    {
        if (time>0)
        {
            mlb_time.text = time.ToString();
            time--;
        }
        else
        {
            // if (Timer.Instance.IsInvoking(schedule))
            //     Timer.Instance.CancelInvoke(schedule);
            Close();
        }
    }

    //显示特效
    private void ShowEffect()
    {
        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_effect,"effect_instance_success_add");
    }
    
    void ItemClick(UIItemBase item)
    {
        if (item != null)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, item.itemCfg, item.infos);
        }

        
        
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        listData = null;
        clickAct = null;
        CSEffectPlayMgr.Instance.Recycle(mobj_effect);
    }
}
