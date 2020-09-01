using UnityEngine;
using System.Collections;

public class CSSkillMoveEffectMulti : CSSkillEffectMultiBase<CSSkillMoveEffect>
{
    public override void Init(AvatarUnit avatar, AvatarUnit target, CSSkillEffectData effectData, Transform parent)
    {
        if (effectData != null && effectData.Num > 0)
        {
            mListPoint.Clear();
            AnalysisString(effectData.Point);
            //AnalysisString(skill.Effect.mType, ref mType, ref mNum);
            mNum = effectData.Num;
            mList.Clear();
            mAvatar = avatar;
            for (int i = 0; i < mNum; i++)
            {
                CSSkillMoveEffect t = new CSSkillMoveEffect();
                t.Init(avatar, target,effectData, true, parent);
                t.speed = 2;
                // t.rotationBeginDir = i;
                mList.Add(t);
            }
        }
        else
        {
            base.Init(avatar,target,effectData,parent);
        }
    }


    public override void Play(float time)
    {
        if (mNum > 0)
        {
            if (this == null || mAvatar == null || CSPreLoadingBase.Instance == null)
            {
                return;
            }
            CSCell oldCell = mAvatar.OldCell;
            if (oldCell == null)
            {
                return;
            }

            CSMisc.Dot2 d = oldCell.Coord;
            float f = 0.0f;
            int x = 0;
            int y = 0;
            int l;
            CSCell attackPosition = null;

            for (int i = 0; i < mList.Count; i++)
            {
                CSSkillMoveEffect t = mList[i];
                if (i >= mListPoint.Count) continue;

                x = d.x + mListPoint[i].x;
                y = d.y + mListPoint[i].y;
                l = x + mListPoint[i].mlength;
                f = mListPoint[i].time * 0.01f;

                CSCell cell = CSMesh.Instance.getCell(x, y);
                if(cell == null)
                {
                    continue;
                }
                attackPosition = cell;
                t.curDirection = 0;
                t.AttackTarget = null;
                t.startPosition = null;
                t.SetRotationDirction(0);
                if (mType == 3)
                {
                    t.startPosition = attackPosition;
                    cell = CSMesh.Instance.getCell(l, y);
                    t.attackPosition = cell;
                }
                else
                {
                    t.attackPosition = attackPosition;
                }
                if (t.Info != null)
                {
                    t.Info.targetCoordX = x;
                    t.Info.targetCoordY = y;
                }
                t.Play(f * 0.02f);
            }
        }
        else
        {
            base.Play(time);
        }
    }

    public void UpdateNextEffect(CSSkillEndEffectMulti endEffect, CSSkillEndFootEffectMulti endFootEffect)
    {
        for (int i = 0; i < mList.Count; ++i)
        {
            CSSkillMoveEffect moveEffect = mList[i];
            moveEffect.endEffect = endEffect;
            moveEffect.endFootEffect = endFootEffect;
        }
    }
}
