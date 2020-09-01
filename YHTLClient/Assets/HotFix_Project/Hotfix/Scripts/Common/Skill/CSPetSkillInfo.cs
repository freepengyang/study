using fight;
using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using TABLE;
using UnityEngine;
using user;

public partial class CSSkillInfo : CSInfo<CSSkillInfo>
{
    /// <summary>
    /// 战宠技能 [职业] => 技能组ID => 技能信息
    /// </summary>
    protected Dictionary<int, FastArrayElementKeepHandle<PetSkillItemData>>[] mPetSkillGroup2List = new Dictionary<int, FastArrayElementKeepHandle<PetSkillItemData>>[4];
    /// <summary>
    /// 已经学习的战宠技能ID => skillInfo
    /// </summary>
    protected Dictionary<int, SkillInfo> mLearnedPetSkillDic = new Dictionary<int, SkillInfo>(16);
    /// <summary>
    /// 已经学习的战宠技能group => level
    /// </summary>
    protected Dictionary<int, int> mPetSkillGroup2LvDic = new Dictionary<int, int>(16);
    /// <summary>
    /// 战宠技能字典(本战宠能够学习的所有技能)
    /// </summary>
    protected Dictionary<int, PetSkillItemData> mPetJobSkillDic = new Dictionary<int, PetSkillItemData>(32);
    /// <summary>
    /// 过滤后的宠物技能(来自战宠技能字典)
    /// </summary>
    protected FastArrayElementKeepHandle<PetSkillItemData> mPetJobRelativedSkills = new FastArrayElementKeepHandle<PetSkillItemData>(64);

    void OnPetSkillDispose()
    {
        mPetJobRelativedSkills?.Clear();
        mPetJobRelativedSkills = null;
        mPetJobSkillDic?.Clear();
        mPetJobSkillDic = null;
        mPetSkillGroup2LvDic?.Clear();
        mPetSkillGroup2LvDic = null;
        mLearnedPetSkillDic?.Clear();
        mLearnedPetSkillDic = null;
        for(int i = 0; i < mPetSkillGroup2List.Length; ++i)
        {
            if(null != mPetSkillGroup2List[i])
            {
                mPetSkillGroup2List[i].Clear();
                mPetSkillGroup2List[i] = null;
            }
        }
        mPetSkillGroup2List = null;
    }

    bool mInitializePet = false;
    public void InitializePet(PlayerInfo playerInfo)
    {
        if (mInitializePet)
            return;
        mInitializePet = true;
        LoadBdjnItems();
    }

    public bool CheckExistCanUpgradePetSkill()
    {
        var datas = GetPetJobRelativeSkills();
        for (int i = 0; i < datas.Count; ++i)
            if (datas[i].learned && datas[i].canUpgrade)
                return true;
        return false;
    }

    bool TryAddPetSkill(int skillId)
    {
        TABLE.SKILL skillItem;
        if (!SkillTableManager.Instance.TryGetValue(skillId, out skillItem))
        {
            return false;
        }

        if (mLearnedPetSkillDic.Remove(skillId))
        {
            RemoveMergedPetSkill(skillId);
        }

        var skill = new fight.SkillInfo();
        skill.sid = skillId;
        skill.level = skillItem.level;
        skill.name = skillItem.name;
        mLearnedPetSkillDic.Add(skillId, skill);

        AddMergedPetSkill(skillId);

        return true;
    }

    void RemoveMergedPetSkill(int skillId)
    {
        int skillGroup = skillId / 1000;
        int skillLv = skillId % 1000;

        //吕天行说普攻不用检测
        if (skillGroup <= 0)
        {
            return;
        }

        if (mPetSkillGroup2LvDic.ContainsKey(skillGroup))
        {
            mPetSkillGroup2LvDic[skillGroup] -= skillLv;
#if UNITY_EDITOR
            if (mPetSkillGroup2LvDic[skillGroup] < 0)
            {
                FNDebug.LogError("[RemoveMergedSkill]:[技能校验错误]:删除后技能等级为负数");
            }
#endif
        }
        else
        {
            FNDebug.LogError("[RemoveMergedSkill]:[技能校验错误]:尝试删除不存在的技能");
        }
        //#if UNITY_EDITOR
        //        VerifyMergeSkill(skillId);
        //#endif
    }

    void AddMergedPetSkill(int skillId)
    {
        int skillGroup = skillId / 1000;
        int skillLv = skillId % 1000;

        //吕天行说普攻不用检测
        if (skillGroup <= 0)
        {
            return;
        }

        if (mPetSkillGroup2LvDic.ContainsKey(skillGroup))
        {
            mPetSkillGroup2LvDic[skillGroup] += skillLv;
        }
        else
        {
            mPetSkillGroup2LvDic.Add(skillGroup, skillLv);
        }
        //#if UNITY_EDITOR
        //        VerifyMergeSkill(skillId);
        //#endif
    }

    public FastArrayElementKeepHandle<PetSkillItemData> GetPetJobRelativeSkills()
    {
        return mPetJobRelativedSkills;
    }

    //计算本职业相关所有技能
    public FastArrayElementKeepHandle<PetSkillItemData> CalculatePetJobRelativeSkills()
    {
        mPetJobSkillDic.Clear();
        mPetJobRelativedSkills.Clear();

        int jobId = PetJob;
        if (!(jobId > 0 && jobId < 4))
        {
            return mPetJobRelativedSkills;
        }

        if (null == mPetSkillGroup2List[jobId])
        {
            return mPetJobRelativedSkills;
        }

        var it = mPetSkillGroup2List[jobId].GetEnumerator();
        while (it.MoveNext())
        {
            var skillList = it.Current.Value;
            if (null == skillList || skillList.Count <= 0)
                continue;

            InitNextSkill(skillList);

            mPetJobSkillDic.Add(it.Current.Key, skillList[0]);
        }

        ReplaceLearnedSkills(mPetJobSkillDic);

        var itValid = mPetJobSkillDic.GetEnumerator();
        while (itValid.MoveNext())
        {
            var itValue = itValid.Current.Value;
            if (itValue.item.show != 1)
                continue;

            //过滤被动技能
            if(!itValue.item.IsActivedSkill())
            {
                continue;
            }
                
            mPetJobRelativedSkills.Append(itValue);
        }
        
        mPetJobRelativedSkills.Sort(PetSkillCompare);
        return mPetJobRelativedSkills;
    }

    void ReplaceLearnedSkills(Dictionary<int, PetSkillItemData> job2Skills)
    {
        int jobId = PetJob;
        if (!(jobId > 0 && jobId < 4))
        {
            return;
        }

        var orgDic = mPetSkillGroup2List[jobId];
        if (null == orgDic)
        {
            return;
        }

        var it = mLearnedPetSkillDic.GetEnumerator();
        while (it.MoveNext())
        {
            var skillInfo = it.Current.Value;
            if (null == skillInfo)
            {
                continue;
            }

            TABLE.SKILL item = null;
            if (!SkillTableManager.Instance.TryGetValue(skillInfo.sid, out item))
            {
                continue;
            }

            if (!job2Skills.ContainsKey((int)item.skillGroup) || !orgDic.ContainsKey((int)item.skillGroup))
            {
                continue;
            }

            var skillArray = orgDic[(int)item.skillGroup];

            PetSkillItemData skillData = null;
            skillArray.Sort(PetSkillItemData.Compare);

            for (int j = 0; j < skillArray.Count; ++j)
            {
                var skill = skillArray[j];
                //设置命中
                if (skillArray[j].item.id == skillInfo.sid)
                {
                    skillData = skillArray[j];
                    break;
                }
            }

            if (null == skillData)
            {
                continue;
            }

            job2Skills[(int)item.skillGroup] = skillData;
        }
    }

    void PetSkillCompare(ref long sortValue, PetSkillItemData r)
    {
        sortValue = r.SortId;
    }

    public bool HasPetSkillLearned(int skillId)
    {
        return mPetSkillGroup2LvDic.ContainsKey(skillId / 1000);
    }

    void InitNextSkill(FastArrayElementKeepHandle<PetSkillItemData> skillArray)
    {
        for (int j = 0; j < skillArray.Count; ++j)
        {
            var skill = skillArray[j];
            //设置技能下一级标志
            if (j + 1 < skillArray.Count)
                skill.nextItem = skillArray[j + 1].item;
            else
                skill.nextItem = skill.item;
        }
    }

    FastArrayElementFromPool<PetSkillItemData> mPetBDJNItems;
    public FastArrayElementFromPool<PetSkillItemData> GetPetBDJNItems()
    {
        if (null == mPetBDJNItems)
        {
            mPetBDJNItems = mPoolHandle.CreateGeneratePool<PetSkillItemData>();
        }
        //if(mPetBDJNItems.Count <= 0)
        //{
        //    LoadBdjnItems();
        //}
        LoadBdjnItems(false);
        return mPetBDJNItems;
    }

    public void LoadBdjnItems(bool sendMsg = true)
    {
        if(null == mPetBDJNItems)
        {
            mPetBDJNItems = mPoolHandle.CreateGeneratePool<PetSkillItemData>();
        }
        for(int i = 0,max = mPetBDJNItems.Count;i < max;++i)
        {
            mPetBDJNItems[i].Reset();
        }
        mPetBDJNItems.Clear();
        //1 填充已经解锁得槽位
        var datas = CSWarPetRefineInfo.Instance.MyWarPetSkillDatas;
        for (int i = 0; i < datas.Count; ++i)
        {
            var itemData = mPetBDJNItems.Append();
            itemData.warPetSkillData = datas[i];
            itemData.Flag = 0;
            itemData.AddFlag(PetSkillItemData.SkillFlag.SF_DEACTIVED);
            if (datas[i].Special == 1)
            {
                itemData.AddFlag(PetSkillItemData.SkillFlag.SF_SPECIAL);
            }
            itemData.AddFlag(PetSkillItemData.SkillFlag.SF_UNLOCKED);
            itemData.item = datas[i].CfgSkill;
            itemData.XilanId = datas[i].XiLianId;
            if(datas[i].ID > 0)
            {
                itemData.AddFlag(PetSkillItemData.SkillFlag.SF_LEARNED);
            }
            if (datas[i].Special == 1)
            {
                if (null == itemData.item)
                {
                    SkillTableManager.Instance.TryGetValue(CSWarPetRefineInfo.Instance.SpecialSkillId, out TABLE.SKILL skillItem);
                    itemData.item = skillItem;
                }
            }
            itemData.Pos = datas[i].Pos;
            itemData.PetSkillAttrs = datas[i].PetSkillAttrs;
        }
        ////2 填充未解锁得槽位
        //for (int i = 0, max = GetPetBDJNMaxCount(); i < max; ++i)
        //{
        //    var itemData = mPetBDJNItems.Append();
        //    itemData.item = null;
        //    itemData.XilanId = 0;
        //    itemData.Flag = 0;
        //    itemData.AddFlag(PetSkillItemData.SkillFlag.SF_DEACTIVED);
        //    itemData.Pos = i;
        //    itemData.PetSkillAttrs = null;
        //}
        mPetBDJNItems.Sort(PetSkillItemData.BdjnCompare);
        if(sendMsg)
        HotManager.Instance.EventHandler.SendEvent(CEvent.PetBdjnChanged);
    }

    public void Link2PetRefinePanel()
    {
        //打开宠物天赋洗练界面
        UtilityPanel.JumpToPanel(26002);
    }

    /// <summary>
    /// 获得技能解锁需要的宠物天赋等级(李晨)
    /// </summary>
    /// <returns></returns>
    public int GetStudySkillNeedTalentLevel(int skillGroup)
    {
        int v = CSPetTalentInfo.Instance.GetSkillUnlockLv(skillGroup);
        return v;
    }

    /// <summary>
    /// 获得宠物天赋等级(李晨)
    /// </summary>
    /// <returns></returns>
    public int GetPetTalentLevel()
    {
        return CSPetTalentInfo.Instance.CurActivatedPoint;
    }

    /// <summary>
    /// 获得宠物等级(张魁)
    /// </summary>
    /// <returns></returns>
    public int GetPetLevel()
    {
        return CSPetBasePropInfo.Instance.GetPetLevel();
    }

    /// <summary>
    /// 被动技能格子数量(张魁)
    /// </summary>
    /// <returns></returns>
    public int GetPetBDJNMaxCount()
    {
        int zhanhunId = CSPetBasePropInfo.Instance.GetZhanHunSuitId();
        TABLE.ZHANHUNSUIT zhanhuiItem = null;
        if (!ZhanHunSuitTableManager.Instance.TryGetValue(zhanhunId, out zhanhuiItem))
        {
            return 0;
        }
        return zhanhuiItem.skillNum;
    }

    /// <summary>
    /// 加载学习的战宠技能
    /// </summary>
    /// <param name="skills"></param>
    public void LoadLearnedPetSkill(RepeatedField<int> skills)
    {
        mLearnedPetSkillDic.Clear();
        mPetSkillGroup2LvDic.Clear();
        if (null != skills)
        {
            for (int i = 0, max = skills.Count; i < max; ++i)
            {
                int skillId = skills[i];
                int sid = skillId;
                if (!mLearnedPetSkillDic.ContainsKey(sid))
                {
                    var skillInfo = new SkillInfo();
                    skillInfo.level = skillId % 1000;
                    skillInfo.sid = sid;
                    mLearnedPetSkillDic.Add(sid, skillInfo);
                }

                AddMergedPetSkill(sid);
            }
        }
        CalculatePetJobRelativeSkills();
    }

    CombinedSkillData mCombinedSkillData = new CombinedSkillData();
    /// <summary>
    /// 初始化战宠合体技能
    /// </summary>
    public void InitPetCombinedSkill(int hejiPoints,int hejiConfigId,int roleHejiConfigId)
    {
        HejiSkillTableManager.Instance.TryGetValue(hejiConfigId, out mCombinedSkillData.petConfig);
        HejiSkillTableManager.Instance.TryGetValue(roleHejiConfigId, out mCombinedSkillData.roleConfig);
        mCombinedSkillData.energyPoint = hejiPoints;
        if(null != mCombinedSkillData.roleConfig)
        {
            mCombinedSkillData.eachEnergyPoints = mCombinedSkillData.roleConfig.costPoint;
            mCombinedSkillData.fullEnergyPoints = mCombinedSkillData.roleConfig.maxPoint;
#if EnableEnergyLog
            FNDebug.Log($"<color=#00ff00>[合体技能]:每根能量值:{mCombinedSkillData.eachEnergyPoints}</color>");
            FNDebug.Log($"<color=#00ff00>[合体技能]:满值能量值:{mCombinedSkillData.fullEnergyPoints}</color>");
#endif
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.GetWarPetCombinedSkill);
    }

    /// <summary>
    /// 更新宠物能量点
    /// </summary>
    /// <param name="hejiPoints"></param>
    public void SetCombinedEnergyPoints(int hejiPoints)
    {
        mCombinedSkillData.energyPoint = hejiPoints;
#if EnableEnergyLog
        FNDebug.Log($"<color=#00ff00>[合体技能]:当前能量值:{hejiPoints}</color>");
        FNDebug.Log($"<color=#00ff00>[合体技能]:每根能量值:{mCombinedSkillData.eachEnergyPoints}</color>");
        FNDebug.Log($"<color=#00ff00>[合体技能]:满值能量值:{mCombinedSkillData.fullEnergyPoints}</color>");
#endif
        HotManager.Instance.EventHandler.SendEvent(CEvent.GetWarPetCombinedSkill);
    }

    public CombinedSkillData GetPetCombinedSkill()
    {
        return mCombinedSkillData;
    }

    /// <summary>
    /// 请求宠物技能升级接口 Net.Req...
    /// </summary>
    public void ReqPetSkillUpgrade(int skillId)
    {
        Net.CSPetSkillUpgradeMessage(skillId / 1000);
    }

    /// <summary>
    /// 需要对接网络数据
    /// </summary>
    /// <param name="oldId"></param>
    /// <param name="newId"></param>
    public void UpgradePetSkill(int oldId, int newId)
    {
        if (oldId == newId)
        {
            return;
        }

        if (!mLearnedPetSkillDic.Remove(oldId))
            return;

        RemoveMergedPetSkill(oldId);

        UtilityTips.ShowGreenTips(1529);

        TryAddPetSkill(newId);
        CalculatePetJobRelativeSkills();

        TABLE.SKILL oldItem = null;
        TABLE.SKILL newItem = null;
        if (oldId != 0 && !SkillTableManager.Instance.TryGetValue(oldId, out oldItem) || !SkillTableManager.Instance.TryGetValue(newId, out newItem))
        {
            return;
        }

        if (null != oldItem && oldItem.skillGroup != newItem.skillGroup)
            return;

        int jobId = PetJob;
        if (!(jobId > 0 && jobId < 4))
        {
            return;
        }

        var orgDic = mPetSkillGroup2List[jobId];
        if (null == orgDic || null != oldItem && !orgDic.ContainsKey(oldItem.skillGroup) || !orgDic.ContainsKey(newItem.skillGroup))
            return;

        //查找原来的数据
        for (int i = 0; i < mPetJobRelativedSkills.Count; ++i)
        {
            var skill = mPetJobRelativedSkills[i];
            if (skill.item.id == oldId || skill.item.id == newId)
            {
                bool replaced = false;
                var skillArray = orgDic[(int)skill.item.skillGroup];
                for (int j = 0; j < skillArray.Count; ++j)
                {
                    if (skillArray[j].item.id == newItem.id)
                    {
                        mPetJobRelativedSkills[i] = skillArray[j];
                        replaced = true;
                    }
                }

                if (replaced)
                {
                    mClientEvent.SendEvent(CEvent.PetSkillUpgradeSucceed, mJobRelativedSkills[i]);
                }
                break;
            }
        }
    }

#region 需要对接的内容
    /// <summary>
    /// 需要对接网络数据
    /// </summary>
    /// <param name="skillId"></param>
    public void AddNewPetSkill(int skillId)
    {
        if (!TryAddPetSkill(skillId))
        {
            return;
        }
        CalculatePetJobRelativeSkills();
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnPetSkillAdded, skillId);
    }
    /// <summary>
    /// 需要对接网络数据
    /// </summary>
    /// <param name="skillId"></param>
    public void RemovePetSkill(int skillId)
    {
        if (mLearnedPetSkillDic.Remove(skillId))
        {
            RemoveMergedPetSkill(skillId);
            CalculatePetJobRelativeSkills();
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnPetSkillRemoved, skillId);
        }
    }

    /// <summary>
    /// 当前宠物职业 这个后面可能需要修改
    /// </summary>
    public int PetJob
    {
        get
        {
            return petJob;
        }
        set
        {
            petJob = value;
            CalculatePetJobRelativeSkills();
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnPetJobChanged);
        }
    }
    int petJob = ECareer.Warrior;
#endregion
}