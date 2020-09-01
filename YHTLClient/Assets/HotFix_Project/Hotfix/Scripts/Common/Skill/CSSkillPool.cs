using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSkillPool : Singleton<CSSkillPool>
{
    //character skill is not here
    private Dictionary<int, List<CSSkill>> mSkillDic = new Dictionary<int, List<CSSkill>>();
    public CSSkill PopSkill(int id,CSAvatar avatar)
    {
        if (avatar == null)
        {
            return null;
        }
        TABLE.SKILL tbl;
        if (SkillTableManager.Instance.TryGetValue(id, out tbl))
        {
            int effectId = tbl.effectId;
            if (mSkillDic.ContainsKey(effectId) && mSkillDic[effectId].Count > 0)
            {
                int index = mSkillDic[effectId].Count-1;
                CSSkill skill = mSkillDic[effectId][index];
                mSkillDic[effectId].RemoveAt(index);
                return skill;
            }
            else
            {
                CSSkill skill = new CSSkill(id, avatar);
                return skill;
            }
        }
        return null;
    }

    public void PushSkill(CSSkill skill)
    {
        if (skill == null || skill.SkillInfo == null) return;

        int effectId = skill.SkillInfo.effectId;

        if (!mSkillDic.ContainsKey(effectId))
        {
            mSkillDic.Add(effectId, new List<CSSkill>());
        }
        if (!mSkillDic[effectId].Contains(skill))
        {
            mSkillDic[effectId].Add(skill);
        }
    }

    public void Clear()
    {
        mSkillDic.Clear();
    }
}