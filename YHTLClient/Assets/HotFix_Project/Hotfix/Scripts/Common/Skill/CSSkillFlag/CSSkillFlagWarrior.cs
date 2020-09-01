using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSkillFlagWarrior : CSSkillFlagBase
{
    public override void Show(int skillId, int direction)
    {
        TABLE.SKILL tblSkill = null;
        if(SkillTableManager.Instance.TryGetValue(skillId, out tblSkill))
        {
            maxAttackRangeRadius = 6.0f;
            maxTargetRangeRadius = 6.0f;

            //if (attackFlagItem == null)
            //{
            //    attackFlagItem = new CSSkillFlagItem();
            //}
            //float rangeRadius = tblSkill.range + 0.5f;
            //attackFlagItem.Show(6063, 1, Vector3.zero);

            float rangeRadius = tblSkill.clientRange + 0.5f;
            if (targetFlagItem == null)
            {
                targetFlagItem = new CSSkillFlagItem();
            }
            targetFlagItem.SetLastDirection(CSDirection.None);
            targetFlagItem.SetDirection(direction);
            targetFlagItem.SetMoveRange(rangeRadius);
            targetFlagItem.SetTarget(true);
            targetFlagItem.Show(6062, 1.0f, Vector3.zero, -CSSkillFlagItem.Z_ORDER);
        }

        base.Show(skillId, direction);
    }

    public override void OnDrag(Vector3 localPosition, int direction)
    {
        if(targetFlagItem != null)
        {
            targetFlagItem.SetRotation(direction);
            bool isCancle = targetFlagItem.GetCancle(localPosition);
            targetFlagItem.SetCancle(isCancle);
            if(attackFlagItem != null)
            {
                attackFlagItem.SetCancle(targetFlagItem.isCancle);
            }
        }
        base.OnDrag(localPosition, direction);
    }
}
