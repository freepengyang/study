using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSSkillEndEffectMulti : CSSkillEffectMultiBase<CSSkillEndEffect>
{
    public override void Init(AvatarUnit avatar,AvatarUnit target,CSSkillEffectData effectData, Transform parent)
    {
        if(effectData != null && effectData.Num > 0)
        {
            mListPoint.Clear();
            //AnalysisString(skill.effect.mType, ref mType, ref mNum);
            AnalysisString(effectData.Point);
            mNum = effectData.Num;
            mList.Clear();
            mAvatar = avatar;
            for (int i = 0; i < mNum; i++)
            {
                CSSkillEndEffect t = new CSSkillEndEffect();
                t.Init(avatar,target,effectData,true,parent);
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
            CSCell attackPosition = null;

            for (int i = 0; i < mList.Count; i++)
            {
                CSSkillEndEffect t = mList[i];
                if (i >= mListPoint.Count) continue;
                f = mListPoint[i].time * 0.01f;
                x = d.x + mListPoint[i].x;
                y = d.y + mListPoint[i].y;
                CSCell cell = CSMesh.Instance.getCell(x, y);
                attackPosition = cell;
                t.curDirection = 8;
                t.attackPosition = attackPosition;
                t.AttackTarget = null;
                if(t.Info != null)
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
   
}
