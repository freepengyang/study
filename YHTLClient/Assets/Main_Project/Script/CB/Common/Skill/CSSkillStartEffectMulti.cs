using UnityEngine;
using System.Collections;

public class CSSkillStartEffectMulti : CSSkillEffectMultiBase<CSSkillStartEffect>
{
    public bool IsBeginDelay
    {
        get
        {
            if (mList.Count > 0) return mList[0].IsBeginDelay;
            return false;
        }
    }

    public void UpdateNextEffect(CSSkillMoveEffectMulti moveEffect, CSSkillEndEffectMulti endEffect, CSSkillEndFootEffectMulti endFootEffect)
    {
        for(int i = 0; i < mList.Count; ++i)
        {
            CSSkillStartEffect startEffect = mList[i];
            startEffect.moveEffect = moveEffect;
            startEffect.endEffect = endEffect;
            startEffect.endFootEffect = endFootEffect;
        }
    }
}
