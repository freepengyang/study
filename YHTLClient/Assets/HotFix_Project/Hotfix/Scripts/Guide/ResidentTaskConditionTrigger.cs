using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentTaskConditionTrigger : IConditionTrigger
{
    public int Index { get; set; }
    GuideItemData guideItemData;

    public GuideItemData Value
    {
        get
        {
            return guideItemData;
        }
    }

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
    }

    public bool Condition(object argv)
    {
        if(!(argv is CSMissionBase mission))
        {
            return false;
        }

        if (null == guideItemData)
            return false;

        if (null == guideItemData.item)
        {
            return false;
        }

        if (CSGuideManager.Instance.IsOtherGroupGuiding(guideItemData.item.id))
        {
            return false;
        }

        if (guideItemData.triggerTaskType != mission.TasksTab.taskType)
        {
            return false;
        }

        //if((int)mission.TaskState != guideItemData.triggerStatus)
        //{
        //    return false;
        //}

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
        if(null != guideItemData)
            CSGuideManager.Instance.Trigger(guideItemData.item.id,guideItemData.triggerTaskId);
    }

    public string Description()
    {
        return @"等级触发器";
    }

    public void Destroy()
    {

    }
}