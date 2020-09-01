using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleConditionTrigger : IConditionTrigger
{
    public int Index { get; set; }
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
    }

    public bool Condition(object argv)
    {
        if(!(argv is int bubbleId))
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

        if (guideItemData.triggerTaskId != bubbleId)
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
        return @"等级触发器";
    }

    public void Destroy()
    {

    }
}