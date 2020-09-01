using fight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAuto
{
    public int id;
    public bool isOpen;
    public bool isCanRelease;
    public TABLE.SKILL tblSkill;
    public void Init(int id)
    {
        this.id = id;
        isOpen = true;
        isCanRelease = true;
    }

    public void InitSkillTable(TABLE.SKILL _tblSkill)
    {
        tblSkill = _tblSkill;
    }
    public int group
    {
        get
        {
            return (tblSkill != null ? tblSkill.skillGroup : 0);
        }
    }
    public int range
    {
        get
        {
            return (tblSkill != null ? tblSkill.clientRange : 0);
        }
    }
    public int mpCost
    {
        get
        {
            return (tblSkill != null ? tblSkill.mpCost : 0);
        }
    }
    public int skillId
    {
        get
        {
            return (tblSkill!= null ? tblSkill.id : id);
        }
    }
    public int automatically
    {
        get
        {
            return (tblSkill != null ? tblSkill.automatically : 0);
        }
    }

    public int targetType
    {
        get
        {
            return (tblSkill != null ? tblSkill.clientTargetType : 0);
        }
    }

    public int buffId
    {
        get
        {
            return (tblSkill != null ? tblSkill.buff : 0);
        }
    }

    public int LaunchType
    {
        get
        {
            return (tblSkill != null ? tblSkill.launchType : 0); 
        }
    }
}

public class SkillPrioriry
{
    private ILBetterList<SkillAuto> mAutoReleaseList = new ILBetterList<SkillAuto>();
    protected PoolHandleManager mSkillAutoPool = new PoolHandleManager();

    public ILBetterList<SkillAuto> AutoReleaseList
    {
        get
        {
            return mAutoReleaseList;
        }
    }

    public SkillAuto Get()
    {
        SkillAuto skillAuto = mSkillAutoPool.GetSystemClass<SkillAuto>();
        return skillAuto;
    }

    public void Add(int id)
    {
        TABLE.SKILL tblSkill = CSSkillInfo.Instance.GetValidSkillItem(id, false);
        if (tblSkill == null)
        {
            tblSkill = SkillTableManager.Instance[id];
        }
        if (tblSkill != null)
        {
            SkillAuto skillAuto = GetSkillAutoByGroup(tblSkill.skillGroup);
            if (skillAuto != null)
            {
                skillAuto.Init(id);
                skillAuto.InitSkillTable(tblSkill);
                return;
            }
            skillAuto = Get();
            skillAuto.Init(id);
            skillAuto.InitSkillTable(tblSkill);
            if (skillAuto.tblSkill != null)
            {
                int index = GetInsertIndex(skillAuto.tblSkill.order);
                //Debug.LogFormat("======> SkillAuto: id = {0}  group = {1}  isOpen = {2}  isCanRelease = {2}", 
                //    skillAuto.id, skillAuto.group, skillAuto.isOpen, skillAuto.isCanRelease);
                mAutoReleaseList.Insert(index, skillAuto);
            }
            else
            {
                FNDebug.LogFormat("======> SkillPrioriry->Add: skillId = {0}  table is null");
            }
        }
    }


    public void Remove(int id)
    {
        for (int i = 0; i < mAutoReleaseList.Count; ++i)
        {
            SkillAuto skillAuto = mAutoReleaseList[i];
            if (skillAuto.id == id)
            {
                skillAuto.isOpen = false;
                skillAuto.isCanRelease = false;
            }
        }
    }

    public void RemoveByGroup(int group)
    {
        for (int i = 0; i < mAutoReleaseList.Count; ++i)
        {
            SkillAuto skillAuto = mAutoReleaseList[i];
            if (skillAuto.group == group)
            {
                skillAuto.isOpen = false;
                skillAuto.isCanRelease = false;
            }
        }
    }

    public void Update(int oldId, int newId)
    {
        if(IsNew(oldId, newId))
        {
            Add(newId);
        }
    }

    private bool IsNew(int oldId, int newId)
    {
        for (int i = 0; i < mAutoReleaseList.Count; ++i)
        {
            SkillAuto skillAuto = mAutoReleaseList[i];
            if (skillAuto.id == oldId)
            {
                skillAuto.id = newId;
                TABLE.SKILL tblSkill = SkillTableManager.Instance[newId];
                skillAuto.InitSkillTable(tblSkill);                
                return false;
            }
        }
        return true;
    }

    public SkillAuto GetSkillAutoByGroup(int group)
    {
        for(int i = 0; i < mAutoReleaseList.Count; ++i)
        {
            SkillAuto skillAuto = mAutoReleaseList[i];
            if(skillAuto.group == group)
            {
                return skillAuto;
            }
        }
        return null;
    }

    public SkillAuto GetSkillAutoById(int id)
    {
        for (int i = 0; i < mAutoReleaseList.Count; ++i)
        {
            SkillAuto skillAuto = mAutoReleaseList[i];
            if (skillAuto.id == id)
            {
                return skillAuto;
            }
        }
        return null;
    }

    private int GetInsertIndex(int order)
    {
        int count = mAutoReleaseList.Count;
        for (int i = 0; i < mAutoReleaseList.Count; ++i)
        {
            TABLE.SKILL tblSkill = mAutoReleaseList[i].tblSkill;
            if (tblSkill != null)
            {
                if (order > tblSkill.order)
                {
                    return i;
                }
            }
        }
        return count;
    }

    private void InitSkillTable(SkillAuto skillAuto)
    {
        if(skillAuto != null)
        {
            TABLE.SKILL tblSkill = CSSkillInfo.Instance.GetValidSkillItem(skillAuto.id,false);
            if(tblSkill == null)
            {
                tblSkill = SkillTableManager.Instance[skillAuto.id];
            }
            skillAuto.InitSkillTable(tblSkill);
        }
    }

    private TABLE.SKILL GetSkill(SkillAuto skillAuto)
    {
        if(skillAuto == null)
        {
            return null;
        }
        TABLE.SKILL tblSkill = SkillTableManager.Instance[skillAuto.id];
        skillAuto.InitSkillTable(tblSkill);
        return tblSkill;
    }

    public void Dispose()
    {
        mAutoReleaseList.Clear();
        mAutoReleaseList = null;
        mSkillAutoPool = null;
    }
}

public class CSSkillPriorityInfo : CSInfo<CSSkillPriorityInfo>
{
    /// <summary>
    /// <career, skillId>
    /// </summary>
    private Dictionary<int, int> mDefaultReleaseSkillIdDic = new Dictionary<int, int>()
    {
        { ECareer.Warrior, 1},
        { ECareer.Master, 2},
        { ECareer.Taoist, 3},
    };

    /// <summary>
    /// <career, List<SkillGroup>>
    /// </summary>
    private Dictionary<int, List<int>> mDefaultAutoSkillDic = new Dictionary<int, List<int>>()
    {
        { ECareer.Warrior,new List<int>(){ 97, 3, 4, 5, 7, 8, 9 } },
        { ECareer.Master,new List<int>(){ 12} },
        { ECareer.Taoist,new List<int>(){ 20,19,22,23,24} },
    };

    public static List<int> skillAutoReleaseNonCombat = new List<int>() {
        (int)ESkillGroup.LieHuo, (int)ESkillGroup.MagicShield, (int)ESkillGroup.WuJiZhenQi };


    /// <summary>
    /// <skillGroup, flag>
    /// </summary>
    private Dictionary<int, bool> mCareerAutoReleaseDic = new Dictionary<int, bool>();

    private List<int> mCareerAutoReleaseList = new List<int>();

    private SkillPrioriry mCommonFight = new SkillPrioriry();
    private SkillPrioriry mAutoFight = new SkillPrioriry();

    private SkillAuto mTargetSkillAuto = null;

    public SkillAuto TargetSkillAyuto
    {
        get
        {
            if(mTargetSkillAuto == null)
            {
                mTargetSkillAuto = new SkillAuto();
            }
            return mTargetSkillAuto;
        }
    } 

    public ILBetterList<SkillAuto> CommonFightSkillList
    {
        get
        {
            return mCommonFight.AutoReleaseList;
        }
    }

    public ILBetterList<SkillAuto> AutoReleaseList
    {
        get
        {
            return mAutoFight.AutoReleaseList;
        }
    }

    public void Initialize()
    {
        InitAutoFightSkillList();
        int career = CSMainPlayerInfo.Instance.Career;
        int defaultSkillId = 0;
        if (mDefaultReleaseSkillIdDic.TryGetValue(career, out defaultSkillId))
        {
            mCommonFight.Add(defaultSkillId);
            //if(career == ECareer.Warrior)
            {
                mAutoFight.Add(defaultSkillId);
            }
        }
        if(mCareerAutoReleaseList.Count <= 0)
        {
            mDefaultAutoSkillDic.TryGetValue(career, out mCareerAutoReleaseList);
        }


        //bool isBanYueOpen = CSConfigInfo.Instance.GetBool(ConfigOption.SmartHalfMoonMachetes);
        var dic = CSSkillInfo.Instance.GetLearnedSkills().GetEnumerator();
        while (dic.MoveNext())
        {
            SkillInfo skillInfo = dic.Current.Value;
            InitAutoFight(skillInfo.sid,career);
        }

        mClientEvent.AddEvent(CEvent.OnSkillAdded, OnSkillAdded);
        mClientEvent.AddEvent(CEvent.OnSkillRemoved, OnSkillRemoved);
        mClientEvent.AddEvent(CEvent.AttachedSkillModified, OnRefreshAttachedSkillModified);
        mClientEvent.AddEvent(CEvent.Setting_AutoReleaseSkillChange, OnSettingLayerSkillChange);
        mClientEvent.AddEvent(CEvent.Setting_SkillSingleChange, OnSettingSkillChange);
        mClientEvent.AddEvent(CEvent.Setting_SkillGroupChange, OnSettingSkillChange);
    }

    private void InitAutoFight(int sid, int career)
    {
        TABLE.SKILL tblSkill = null;
        if (SkillTableManager.Instance.TryGetValue(sid, out tblSkill))
        {
            if (career == ECareer.Warrior)
            {
                if (CSSkillInfo.Instance.GetSkillAutoPlay(sid))
                {
                    AddCommonFight(sid);
                    AddAutoFight(sid);
                }
                else if (tblSkill.skillGroup == (int)ESkillGroup.BanYue)
                {
                    AddAutoFight(sid);
                }
            }
            else if (mCareerAutoReleaseList.Contains(tblSkill.skillGroup))
            {
                AddAutoFight(sid);
            }
        }
    }

    private void InitAutoFightSkillList()
    {
        int career = CSMainPlayerInfo.Instance.Career;
        TABLE.SUNDRY tblSundry = null;
        if (SundryTableManager.Instance.TryGetValue(664, out tblSundry))
        {
            string[] strs = UtilityMainMath.StrToStrArr(tblSundry.effect, '&');
            int careerMax = ECareer.Taoist;
            if (strs.Length >= careerMax)
            {
                mCareerAutoReleaseList = UtilityMainMath.SplitStringToIntList(strs[(int)career - 1]);
            }
        }

        if (mCareerAutoReleaseList.Count <= 0)
        {
            mCareerAutoReleaseList = mDefaultAutoSkillDic[career];
        }

        if (career == ECareer.Master)
        {
            List<int> list = null;
            if (mDefaultAutoSkillDic.TryGetValue(career, out list))
            {
               if(list != null)
                {
                    int skillGroup = CSConfigInfo.Instance.GetInt(ConfigOption.SkillSingle);
                    if(!list.Contains(skillGroup))
                    {
                        list.Add(skillGroup);
                    }
                    skillGroup = CSConfigInfo.Instance.GetInt(ConfigOption.SkillGroup);
                    if (!list.Contains(skillGroup))
                    {
                        list.Add(skillGroup);
                    }
                    mCareerAutoReleaseList = list;
                }
            }
        }
    }


#region 普通攻击
    public void AddCommonFight(int id)
    {
        mCommonFight.Add(id);
    }

    public void RemoveCommonFight(int id)
    {
        mCommonFight.Remove(id);
    }
    #endregion

    #region 自动攻击
    public void AddAutoFight(int id)
    {
        TABLE.SKILL tblSkill = null;
        if (SkillTableManager.Instance.TryGetValue(id, out tblSkill))
        {
            if (mCareerAutoReleaseList.Contains(tblSkill.skillGroup))
            {
                mAutoFight.Add(id);
            }
            //RemoveAutoDefaultSkill(tblSkill.skillGroup);
            bool ret = true;
            if (CSConfigInfo.Instance.TryGetAutoSkill(tblSkill.skillGroup, out ret))
            {
                if (!ret)
                {
                    RemoveAutoFight(id);
                }
            }
        }
    }
    public void RemoveAutoFight(int id)
    {
        mAutoFight.Remove(id);
    }

    public void RemoveAutoFightByGroup(int group)
    {
        mAutoFight.RemoveByGroup(group);
    }

    #endregion

    public void Update(TABLE.SKILL tblOldSkill, TABLE.SKILL tblNewSkill)
    {
        if(tblOldSkill != null && tblNewSkill != null)
        {
            //if (tblOldSkill.automatically > 0 && tblNewSkill.automatically > 0)
            SkillAuto skillAuto = GetAutoFightSkillAutoByGroup(tblOldSkill.skillGroup);
            if(skillAuto != null)
            {
                mCommonFight.Update(tblOldSkill.id, tblNewSkill.id);
                mAutoFight.Update(tblOldSkill.id, tblNewSkill.id);
            }
        }
    }

    public SkillAuto GetSkillAuto(int skillId)
    {
        ILBetterList<SkillAuto> skillAutoList = mCommonFight.AutoReleaseList;
        for (int i = 0; i < skillAutoList.Count; ++i)
        {
            SkillAuto skillAuto = skillAutoList[i];
            if(skillAuto != null && skillAuto.skillId == skillId)
            {
                return skillAuto;
            }
        }
        return null;
    }

    private SkillAuto GetAutoFightSkillAutoByGroup(int group)
    {
        ILBetterList<SkillAuto> skillAutoList = mAutoFight.AutoReleaseList;
        for (int i = 0; i < skillAutoList.Count; ++i)
        {
            SkillAuto skillAuto = skillAutoList[i];
            if (skillAuto != null && skillAuto.group == group)
            {
                return skillAuto;
            }
        }
        return null;
    }

    public SkillAuto GetTargetSkillAuto(int skillId)
    {
        if(TargetSkillAyuto.skillId != skillId)
        {
            TargetSkillAyuto.Init(skillId);
            TargetSkillAyuto.InitSkillTable(SkillTableManager.Instance[skillId]);
        }
        return TargetSkillAyuto;
    }

    public bool IsCanAutoReleaseByMp(int mp)
    {
        ILBetterList<SkillAuto> skillAutoList = mAutoFight.AutoReleaseList;
        int max = skillAutoList.Count;
        if (max <= 0)
        {
            return false;
        }
        for (int i = 0; i < max; ++i)
        {
            SkillAuto skillAuto = skillAutoList[i];
            if (skillAuto != null && skillAuto.isCanRelease)
            {
                if (mp >= skillAuto.mpCost || !CSSkillInfo.Instance.GetSkillNeedCostMp(skillAuto.group))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void RefreshDefaultReleaseSkill(bool value)
    {
        int career = CSMainPlayerInfo.Instance.Career;
        if (mDefaultReleaseSkillIdDic.ContainsKey(career))
        {
            int skillId = mDefaultReleaseSkillIdDic[career];
            SkillAuto skillAuto = mAutoFight.GetSkillAutoById(skillId);
            if(skillAuto != null)
            {
                skillAuto.isCanRelease = value;
            }
        }
    }

    private void OnSkillAdded(uint evtId, object obj)
    {
        if(obj != null)
        {
            int skillId = (int)obj;
            InitAutoFight(skillId, CSMainPlayerInfo.Instance.Career);
        }
    }

    private void OnSkillRemoved(uint evtId, object obj)
    {
        if(obj != null)
        {
            int skillId = (int)obj;
            int career = CSMainPlayerInfo.Instance.Career;

            TABLE.SKILL tblSkill = null;
            if (SkillTableManager.Instance.TryGetValue(skillId, out tblSkill))
            {
                if (career == ECareer.Warrior)
                {
                    if (CSSkillInfo.Instance.GetSkillAutoPlay(skillId))
                    {
                        RemoveCommonFight(skillId);
                        RemoveAutoFight(skillId);
                    }
                }
                else if (mCareerAutoReleaseList.Contains(tblSkill.skillGroup))
                {
                    RemoveCommonFight(skillId);
                    RemoveAutoFight(skillId);
                }
            }
        }
    }

    private void OnSettingLayerSkillChange(uint evtId, object obj)
    {
        if(obj == null)
        {
            return;
        }
        int group = (int)obj;
        SkillAuto skillAuto = GetAutoFightSkillAutoByGroup(group);
        if(skillAuto != null)
        {
            bool ret = CSConfigInfo.Instance.GetAutoSkillBool(group);
            if(ret)
            {
                AddAutoFight(skillAuto.id);
            }
            else
            {
                RemoveAutoFight(skillAuto.id);
            }
        }
    }

    private void OnSettingSkillChange(uint evtId, object obj)
    {
        if (obj == null)
        {
            return;
        }
        int skillGroup = (int)obj;
        AddAutoFightBySkillGroup(skillGroup);
    }

    private void OnRefreshAttachedSkillModified(uint evtId, object obj)
    {
        if(obj == null)
        {
            return;
        }
        int skillGroup = (int)obj;

        SkillAuto skillAuto = GetAutoFightSkillAutoByGroup(skillGroup);
        if (skillAuto != null)
        {
            AddAutoFight(skillAuto.id);
        }
    }

    private void AddAutoFightBySkillGroup(int skillGroup)
    {
        SkillInfo skillInfo = CSSkillInfo.Instance.GetLearnedSkillByGroup(skillGroup);
        if (skillInfo != null)
        {
            if(!mCareerAutoReleaseList.Contains(skillGroup))
            {
                mCareerAutoReleaseList.Add(skillGroup);
            }
            AddAutoFight(skillInfo.sid);
        }
    }

    private void RemoveAutoDefaultSkill(int skillGroup)
    {
        switch (skillGroup)
        {
            case ESkillGroup.LeiDianShu:
                {
                    RemoveAutoFight(mDefaultReleaseSkillIdDic[ECareer.Master]);
                }
                break;
            case ESkillGroup.LingHunHuoFu:
                {
                    RemoveAutoFight(mDefaultReleaseSkillIdDic[ECareer.Taoist]);

                }
                break;
        }
    }


    public override void Dispose()
    {
        mCommonFight.Dispose();
        mAutoFight.Dispose();
        mCommonFight = null;
        mAutoFight = null;
        mTargetSkillAuto = null;
    }

}

