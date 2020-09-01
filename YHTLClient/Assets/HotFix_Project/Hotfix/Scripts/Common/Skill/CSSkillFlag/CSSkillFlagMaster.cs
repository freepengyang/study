using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSkillFlagMaster : CSSkillFlagBase
{
    public int offsetCoordX;
    public int offsetCoordY;
    private float attackRangeRadius;
    public override void Show(int skillId, int direction)
    {
        TABLE.SKILL tblSkill = null;
        if (SkillTableManager.Instance.TryGetValue(skillId, out tblSkill))
        {
            offsetCoordX = 0;
            offsetCoordY = 0;
            Vector3 initPos = GetInitPosition();
            maxAttackRangeRadius = 8.5f;
            maxTargetRangeRadius = 2.5f;
            if(attackFlagItem == null)
            {
                attackFlagItem = new CSSkillFlagItem();
            }
            if(targetFlagItem == null)
            {
                targetFlagItem = new CSSkillFlagItem();
            }

            attackRangeRadius = tblSkill.clientRange + 0.5f;
            attackFlagItem.SetTarget(true);
            attackFlagItem.Show(6060, (attackRangeRadius / maxAttackRangeRadius), Vector3.zero, CSSkillFlagItem.Z_ORDER);

            if (targetFlagItem == null)
            {
                targetFlagItem = new CSSkillFlagItem();
            }

            targetFlagItem.SetMoveRange(attackRangeRadius);
            targetFlagItem.SetTarget(true);

            float targetRangeRadius = (tblSkill.effectArea == (int)ESkillHurtEffectType.OneSide) ? tblSkill.effectRange * 0.5f : ((tblSkill.effectRange + 0.5f));
            targetFlagItem.Show(6061, (targetRangeRadius / maxTargetRangeRadius), initPos, CSSkillFlagItem.Z_ORDER);
            OnDrag(targetFlagItem.initPosition,direction);
        }
    }

    public override void OnDrag(Vector3 localPosition, int direction)
    {
        if (targetFlagItem != null)
        {
            targetFlagItem.SetTouchPos(localPosition);
            Vector3 pos = Vector3.zero;
            bool isInit = false;
            if (targetFlagItem.rangeEffect != null)
            {
                Transform root = targetFlagItem.rangeEffect.root;
                if (root != null)
                {
                    pos = root.localPosition;
                    offsetCoordX = (int)((pos.x) / CSCell.Size.x);
                    offsetCoordY = (int)((pos.y) / CSCell.Size.y);
                    isInit = true;
                }
            }
            if (!isInit)
            {
                pos = targetFlagItem.initPosition;
            }

            offsetCoordX = (int)((pos.x) / CSCell.Size.x);
            offsetCoordY = (int)((pos.y) / CSCell.Size.y);

            if (attackFlagItem != null)
            {
                attackFlagItem.SetCancle(targetFlagItem.isCancle);
            }
        }
        base.OnDrag(localPosition, direction);
    }

    public int GetCoordX(int targetCoordX, int attackCoordX)
    {
        return ((offsetCoordX == 0 && offsetCoordY == 0) ? targetCoordX : (offsetCoordX + attackCoordX));
    }

    public int GetCoordY(int targetCoordY, int attackCoordY)
    {
        return ((offsetCoordY == 0 && offsetCoordX == 0) ? targetCoordY : (attackCoordY - offsetCoordY));
    }

    public bool IsDrag()
    {
        return (offsetCoordX != 0 || offsetCoordY != 0);
    }

    public void Reset()
    {
        offsetCoordX = 0;
        offsetCoordY = 0;
    }

    private Vector3 GetInitPosition()
    {
        Vector3 pos = Vector3.zero;
        if(CSAvatarManager.Instance == null)
        {
            return pos;
        }

        CSAvatar target = CSAvatarManager.Instance.GetSelectTarget();
        if(target != null)
        {
            pos = GetOffsetPosition(target);
            if(pos == Vector3.zero)
            {
                target = null;
            }
        }
        if (target == null)
        {
            target = CSAvatarManager.Instance.GetNearestAttackTarget();
            pos = GetOffsetPosition(target);
        }
        return pos;
    }

    private Vector3 GetOffsetPosition(CSAvatar target)
    {
        Vector3 pos = Vector3.zero;
        if (target != null && target.OldCell != null)
        {
            CSMisc.Dot2 targetCoord = target.OldCell.Coord;
            CSMisc.Dot2 attackCoord =CSAvatarManager.MainPlayer.OldCell.Coord;
            pos.x = (targetCoord.x - attackCoord.x) * CSCell.Size.x;
            pos.y = -(targetCoord.y - attackCoord.y) * CSCell.Size.y;

            if(targetFlagItem != null)
            {
                if(Mathf.Abs(pos.x) > targetFlagItem.attackWidth || Mathf.Abs(pos.y) > targetFlagItem.attackHeight)
                {
                    return Vector3.zero;
                }
            }
        }
        return pos;
    }

    public bool IsInRange(CSAvatar target)
    {
        return (GetOffsetPosition(target) != Vector3.zero);
    }

    public override void Destroy()
    {
        offsetCoordX = 0;
        offsetCoordY = 0;
        base.Destroy();
    }
}
