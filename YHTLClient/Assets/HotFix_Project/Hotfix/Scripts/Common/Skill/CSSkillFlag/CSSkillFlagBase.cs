using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSkillFlagBase
{
    protected CSSkillFlagItem attackFlagItem;
    protected CSSkillFlagItem targetFlagItem;
    public bool isHaveTarget;
    protected float maxAttackRangeRadius = 8.5f;
    protected float maxTargetRangeRadius = 2.5f;

    public bool IsCancle()
    {
        if(targetFlagItem != null)
        {
            return targetFlagItem.isCancle;
        }
        return false;
    }

    public virtual void Init()
    {

    }

    public virtual void Show(int skillId, int direction)
    {

    }

    public virtual void Hide()
    {
        if (attackFlagItem != null)
        {
            attackFlagItem.Hide();
        }
        if (targetFlagItem != null)
        {
            targetFlagItem.Hide();
        }
    }

    public virtual void OnDrag(Vector3 localPosition, int direction)
    {

    }

    public virtual void SetCancle(bool value)
    {
        if((attackFlagItem != null) && (attackFlagItem.isCancle != value))
        {
            attackFlagItem.SetCancle(value);
        }
        if ((targetFlagItem != null) && (targetFlagItem.isCancle != value))
        {
            targetFlagItem.SetCancle(value);
        }
    }

    public virtual void SetTarget(bool value)
    {
        if ((attackFlagItem != null) && (attackFlagItem.isHaveTarget != value))
        {
            attackFlagItem.SetTarget(value);
        }
        if ((targetFlagItem != null) && (targetFlagItem.isHaveTarget != value))
        {
            targetFlagItem.SetTarget(value);
        }
    }

    public virtual void SetValid(bool value)
    {
        if (attackFlagItem != null)
        {
            attackFlagItem.SetCancle(value);
        }
        if (targetFlagItem != null)
        {
            targetFlagItem.SetCancle(value);
        }
    }

    public virtual void Destroy()
    {
        if(attackFlagItem != null)
        {
            attackFlagItem.Destroy();
            attackFlagItem = null;
        }
        if(targetFlagItem != null)
        {
            targetFlagItem.Destroy();
            targetFlagItem = null;
        }
    }

}
