using System.Collections.Generic;
using UnityEngine;

public partial class UIHonorResultPromptPanel : UIBasePanel
{
    enum ShowType
    {
        success = 1,
        fail = 2,
    }
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    #region
    ShowType currentPanelType;
    Vector3 failPos = new Vector3(0, -74, 0);
    Vector3 successPos = new Vector3(-90, -74, 0);
    List<UIItemBase> itembaseList;
    int countDownTime = 5;
    Schedule schedule;
    #endregion

    public override void Init()
    {
        base.Init();
        UIEventListener.Get(mbtn_back).onClick = BackBtnClick;
        UIEventListener.Get(mbtn_next).onClick = NextBtnClick;
    }

    public override void Show()
    {
        base.Show();
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mobj_effect);
        Timer.Instance.CancelInvoke(schedule);
        if (itembaseList != null)
        {
            for (int i = 0; i < itembaseList.Count; i++)
            {
                UIItemManager.Instance.RecycleSingleItem(itembaseList[i]);
            }
        }
        base.OnDestroy();
    }

    public void SetShowType(int _type, TABLE.INSTANCE _instanceInfo = null)
    {
        currentPanelType = (ShowType)_type;
        if (currentPanelType == ShowType.success)
        {
            CSEffectPlayMgr.Instance.ShowUIEffect(mobj_effect, 17750);
        }
        mobj_fail.SetActive(currentPanelType == ShowType.success ? false : true);
        mobj_failDes.SetActive(currentPanelType == ShowType.success ? false : true);
        mobj_success.SetActive(currentPanelType == ShowType.success ? true : false);
        mbtn_next.SetActive(currentPanelType == ShowType.success ? true : false);
        mbtn_back.transform.localPosition = (currentPanelType == ShowType.success ? successPos : failPos);
        if (currentPanelType == ShowType.success)
        {
            if (_instanceInfo.level == InstanceTableManager.Instance.GetHonorChanllengeMaxLevel())
            {
                mbtn_next.SetActive(false);
                mbtn_back.transform.localPosition = new Vector3(0, -74, 0);
            }
            else
            {
                mbtn_back.transform.localPosition = new Vector3(-90, -74, 0);
                mbtn_next.SetActive(true);
                if (!Timer.Instance.IsInvoking(schedule))
                {
                    mlb_next.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1908), countDownTime);
                    schedule = Timer.Instance.InvokeRepeating(1f, 1f, CountDown);
                }
            }
            List<List<int>> rewards = UtilityMainMath.SplitStringToIntLists(_instanceInfo.rewards);
            itembaseList = UIItemManager.Instance.GetUIItems(rewards.Count, PropItemType.Normal, mgrid_reward.transform);
            for (int i = 0; i < itembaseList.Count; i++)
            {
                itembaseList[i].Refresh(rewards[i][0]);
                itembaseList[i].SetCount(rewards[i][1]);
            }
        }
        mgrid_reward.Reposition();
    }
    void CountDown(Schedule _schedule)
    {
        countDownTime--;
        if (countDownTime <= 0)
        {
            Timer.Instance.CancelInvoke(schedule);
            countDownTime = 5;
            Net.ReqEnterNextInstanceMessage();
            UIManager.Instance.ClosePanel<UIHonorResultPromptPanel>();
        }
        mlb_next.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1908), countDownTime);
    }
    void BackBtnClick(GameObject _go)
    {
        Net.ReqLeaveInstanceMessage(true);
        UIManager.Instance.ClosePanel<UIHonorResultPromptPanel>();
    }
    void NextBtnClick(GameObject _go)
    {
        Net.ReqEnterNextInstanceMessage();
        UIManager.Instance.ClosePanel<UIHonorResultPromptPanel>();
    }
}
