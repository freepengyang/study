using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentLogicTrigger : IConditionTrigger
{
    public const int NORMAL_EQUIP_COUNT = 1;
    public const int EVENT_COUNT = 2;
    System.Func<bool>[] mEventFunc = new System.Func<bool>[EVENT_COUNT];
    public int Index { get; set; }

    public int LogicId { get; set; }
    GuideItemData guideItemData;

    public bool Auto()
    {
        if (null == guideItemData)
            return false;
        if (null == guideItemData.item)
        {
            return false;
        }
        return guideItemData.item.Auto == 1;
    }

    public void Create(GuideItemData guideItemData)
    {
        this.guideItemData = guideItemData;
        mEventFunc[NORMAL_EQUIP_COUNT] = HasNormalEquips;
        HotManager.Instance.EventHandler.AddEvent(CEvent.ItemListChange, OnItemListChange);
    }

    bool HasNormalEquips()
    {
        var count = CSItemCountManager.Instance.GetBagNormalEquipCount();
        if (count < guideItemData.triggerLevel)
            return false;

        var mission = CSMissionManager.Instance.GetMissionByTaskId(this.guideItemData.triggerTaskId);
        if (null == mission)
            return false;

        if ((int)mission.TaskState != this.guideItemData.triggerStatus)
            return false;

        return true;
    }

    public bool Condition(object argv)
    {
        if (null == guideItemData || null == guideItemData.item)
            return false;

        if (LogicId < 0 || LogicId >= mEventFunc.Length)
            return false;

        var eventFunc = mEventFunc[LogicId];
        if (null == eventFunc || !eventFunc())
            return false;

        if (CSGuideManager.Instance.IsOtherGroupGuiding(guideItemData.item.id))
        {
            return false;
        }

        if (guideItemData.item.beginLv > CSMainPlayerInfo.Instance.Level || guideItemData.item.endLv < CSMainPlayerInfo.Instance.Level)
            return false;

        if (CSGuideManager.Instance.IsGuideTriggered(guideItemData.item.id))
        {
            return false;
        }

        return true;
    }

    public void Trigger()
    {
        CSGuideManager.Instance.Trigger(guideItemData.item.id);
    }

    public string Description()
    {
        return @"常驻逻辑触发器";
    }

    protected void OnItemListChange(uint id,object argv)
    {
        if (LogicId < 0 || LogicId >= mEventFunc.Length)
            return;

        var eventFunc = mEventFunc[LogicId];
        if (null == eventFunc)
            return;

        if (CSGuideManager.Instance.CurrentGuideId != guideItemData.item.id)
            return;

        if(!eventFunc())
        {
            FNDebug.Log($"<color=#00ff00>condition unsatisfied stoped</color>");
            CSGuideManager.Instance.ResetGuideStep();
            CSGuideManager.Instance.CurrentGuideId = -1;
        }
    }

    public void Destroy()
    {
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.ItemListChange, OnItemListChange);
    }
}