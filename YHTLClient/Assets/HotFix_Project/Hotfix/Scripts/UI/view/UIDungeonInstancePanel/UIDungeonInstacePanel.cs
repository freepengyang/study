using System.Collections;
using System.Collections.Generic;
using instance;
using UnityEngine;

public partial class UIDungeonInstacePanel
{
    public override UILayerType PanelLayerType => UILayerType.Resident;

    public override bool ShowGaussianBlur => false;


    private InstanceInfo _InstanceInfo;
    TABLE.INSTANCE instance;
    private List<int> conditionList;
    List<List<int>> awardList;
    List<UIItemBase> itemList;

    //Schedule schedule;

    public override void Init()
    {
        base.Init();
        awardList = new List<List<int>>();
        itemList = new List<UIItemBase>();
        for (int i = 0; i < 3; i++)
        {
            itemList.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, mGrid.transform, itemSize.Size50));
        }
        _InstanceInfo = CSInstanceInfo.Instance.GetInstanceInfo();
        mClientEvent.AddEvent(CEvent.GetEnterInstanceInfo, GetEnterInstanceInfo);
        mClientEvent.AddEvent(CEvent.ResInstanceInfo, ResInstanceInfo);
        mClientEvent.AddEvent(CEvent.ECM_SCInstanceFinishMessage, SCInstanceFinishMessage);
    }

    public override void Show()
    {
        base.Show();
        if (_InstanceInfo != null)
            Refresh();
    }

    int gap = 0;

    private void Refresh()
    {
        if (!InstanceTableManager.Instance.TryGetValue(_InstanceInfo.instanceId, out instance)) return;
        mlb_title.text = instance.tips;
        //Timer.Instance.CancelInvoke(schedule);

        if (string.IsNullOrEmpty(instance.rewards))
        {
            maward.SetActive(false);
        }
        else
        {
            if (cor != null)
            {
                ScriptBinder.StopCoroutine(cor);
            }
            cor = ScriptBinder.StartCoroutine(ShowReward());
        }
        RefreshMissionTarget();
    }
    Coroutine cor;
    IEnumerator ShowReward()
    {
        yield return null;
        awardList = UtilityMainMath.SplitStringToIntLists(instance.rewards);
        if (awardList.Count > itemList.Count)
        {
            gap = awardList.Count - itemList.Count;
            for (int i = 0; i < gap; i++)
            {
                itemList.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, mGrid.transform, itemSize.Size50));
            }
        }
        for (var i = 0; i < itemList.Count; i++)
        {
            yield return null;
            if (i >= awardList.Count)
            {
                itemList[i].UnInit();
                itemList[i].obj.SetActive(false);
            }
            else
            {
                itemList[i].obj.SetActive(false);
                itemList[i].Refresh(awardList[i][0]);
                itemList[i].SetCount(awardList[i][1]);
                itemList[i].obj.SetActive(true);
            }
        }
        mGrid.Reposition();
    }
    private void RefreshMissionTarget()
    {
        if (string.IsNullOrEmpty(instance.conditionSuccess) || instance.conditionSuccess == "0") return;
        conditionList = UtilityMainMath.SplitStringToIntList(instance.conditionSuccess);
        if (conditionList.Count < 2) return;

        switch (conditionList[0])
        {
            case 1:

                if (MonsterInfoTableManager.Instance.TryGetValue(conditionList[1], out TABLE.MONSTERINFO monsterinfo))
                {
                    if (_InstanceInfo.success)
                    {
                        mlb_killMonster.text =
                            $"{UtilityColor.GetColorStr(monsterinfo.quality)}{monsterinfo.name}{UtilityColor.Green}1/1";
                    }
                    else
                    {
                        mlb_killMonster.text =
                            $"{UtilityColor.GetColorStr(monsterinfo.quality)}{monsterinfo.name}{UtilityColor.Red}0/1";
                    }
                }

                break;
            case 2:
                mlb_killMonster.text = ClientTipsTableManager.Instance.GetClientTipsContext(1620);
                break;
            case 6:
                mlb_killMonster.text = $"{_InstanceInfo.killedMonsters + _InstanceInfo.killedBoss}/{conditionList[1]}";
                mlb_killMonster.text = _InstanceInfo.killedMonsters + _InstanceInfo.killedBoss < conditionList[1]
                    ? mlb_killMonster.text.BBCode(ColorType.Red)
                    : mlb_killMonster.text.BBCode(ColorType.Green);
                break;
        }
    }

    private void GetEnterInstanceInfo(uint id, object data)
    {
        _InstanceInfo = CSInstanceInfo.Instance.GetInstanceInfo();
        Refresh();
        RefreshMissionTarget();
        ShowSuccessOrFail();
    }

    private void ResInstanceInfo(uint id, object data)
    {
        _InstanceInfo = CSInstanceInfo.Instance.GetInstanceInfo();
        Refresh();
        RefreshMissionTarget();
        ShowSuccessOrFail();
        //Debug.Log($"{instance.type}  {_InstanceInfo.success}  ");
        if (instance.type == 20 && _InstanceInfo.state == 3) //荣耀挑战
        {
            int HonorType = (_InstanceInfo.success) ? 1 : 2;
            UIManager.Instance.CreatePanel<UIHonorResultPromptPanel>(p =>
            {
                (p as UIHonorResultPromptPanel).SetShowType(HonorType, instance);
            });
        }
        //if (instance.type == 13 && _InstanceInfo.state == 3 && _InstanceInfo.success) //个人boss
        //{
        //    Utility.ShowRewardPanel(instance.id, () =>
        //    {
        //        Net.ReqLeaveInstanceMessage(true);
        //    }, RewardPromptType.Countdown);
        //}
    }

    void SCInstanceFinishMessage(uint id, object data)
    {
    }

    private void ShowSuccessOrFail()
    {
        //后续有显示隐藏 副本通关成功失败的效果时，可以用此方法
        /*if (_InstanceInfo.state != 3 || isShowEffect) return;

        int instanceType = InstanceTableManager.Instance.GetInstanceType(_InstanceInfo.instanceId);

        isShowEffect = true;
        mChallengeResult.SetActive(true);
        if (_InstanceInfo.success)
        {
            CSEffectPlayMgr.Instance.ShowUITexture(mbgtitle, "extremity_challenge_success");
        }
        else
        {
            CSEffectPlayMgr.Instance.ShowUITexture(mbgtitle, "extremity_challenge_failure");
        }

        Timer.Instance.Invoke(2, schedule => { mChallengeResult.SetActive(false); });*/
    }

    protected override void OnDestroy()
    {
        if (itemList != null)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                UIItemManager.Instance.RecycleSingleItem(itemList[i]);
            }
        }
        //Timer.Instance.CancelInvoke(schedule);
        _InstanceInfo = null;
        instance = null;
        conditionList?.Clear();
        conditionList = null;
        CSEffectPlayMgr.Instance.Recycle(mbgtitle);
        base.OnDestroy();
    }
}