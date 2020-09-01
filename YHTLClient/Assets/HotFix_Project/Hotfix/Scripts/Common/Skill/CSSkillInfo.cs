using fight;
using FlyBirds.Model;
using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using TABLE;
using UnityEngine;
using user;

public static class SkillExtend
{
    static string[] quaSttr;
    public static string QualityTextWithOutColor(this int quality)
    {
        quality = Mathf.Clamp(quality, 1, 5);
        if (quaSttr == null)
            quaSttr = SundryTableManager.Instance.GetSundryEffect(1007).Split('#');
        return quaSttr[quality - 1];
    }

    public static string BdjnPartDesc(this pet.PetSkillAttr skillAttr, int color = 1)
    {
        if (null == skillAttr)
            return string.Empty;

        return Utility.GetPetSkillAttr(skillAttr.attrType, skillAttr.param, skillAttr.value, color, 2);
    }

    public static string BdjnDesc(this TABLE.SKILL item, int xilianId, string[] argvs)
    {
        if (null != item && null != argvs)
        {
            string description = string.Empty;
            if (argvs.Length == 1)
            {
                description = string.Format(item.clientDescription, argvs[0]);
            }
            else if (argvs.Length == 2)
            {
                description = string.Format(item.clientDescription, argvs[0], argvs[1]);
            }
            else if (argvs.Length == 3)
            {
                description = string.Format(item.clientDescription, argvs[0], argvs[1], argvs[2]);
            }
            else if (argvs.Length == 4)
            {
                description = string.Format(item.clientDescription, argvs[0], argvs[1], argvs[2], argvs[3]);
            }
            else if (argvs.Length == 5)
            {
                description = string.Format(item.clientDescription, argvs[0], argvs[1], argvs[2], argvs[3], argvs[4]);
            }
            else
            {
                description = string.Format(item.clientDescription, argvs[0], argvs[1], argvs[2], argvs[3], argvs[4], argvs[5]);
            }

            if (item.cdTime > 0 || item.mpCost > 0)
            {
                if (item.cdTime <= 0)
                {
                    return $"{CSString.Format(550, description)}\n{CSString.Format(1505, item.mpCost)}";
                }
                else if (item.mpCost <= 0)
                {
                    return $"{CSString.Format(550, description)}\n{CSString.Format(1504, item.cdTime * 0.001f)}";
                }
                else
                {
                    return $"{CSString.Format(550, description)}\n{CSString.Format(1504, item.cdTime * 0.001f)}    {CSString.Format(1505, item.mpCost)}";
                }
            }
            else
                return CSString.Format(550, description);
        }
        return string.Empty;
    }

    public static string Desc(this TABLE.SKILL item)
    {
        if (null != item)
        {
            if (item.cdTime > 0 || item.mpCost > 0)
            {
                if (item.cdTime <= 0)
                {
                    return $"{CSString.Format(550, item.description)}\n{CSString.Format(1505, item.mpCost)}";
                }
                else if (item.mpCost <= 0)
                {
                    return $"{CSString.Format(550, item.description)}\n{CSString.Format(1504, item.cdTime * 0.001f)}";
                }
                else
                {
                    return $"{CSString.Format(550, item.description)}\n{CSString.Format(1504, item.cdTime * 0.001f)}    {CSString.Format(1505, item.mpCost)}";
                }
            }
            else
                return CSString.Format(550, item.description);
        }
        return string.Empty;
    }

    public static string NextDesc(this TABLE.SKILL item, TABLE.SKILL current)
    {
        if (null != item && null != current)
        {
            if (item.cdTime > 0 || item.mpCost > 0)
            {
                int timeId = item.cdTime != current.cdTime ? 1502 : 1502;
                int mpcostId = item.mpCost != current.mpCost ? 1503 : 1503;
                if (item.cdTime <= 0)
                {
                    return $"{CSString.Format(1299, item.clientDescription)}\n{CSString.Format(mpcostId, item.mpCost)}";
                }
                else if (item.mpCost <= 0)
                {
                    return $"{CSString.Format(1299, item.clientDescription)}\n{CSString.Format(timeId, item.cdTime * 0.001f)}";
                }
                else
                {
                    return $"{CSString.Format(1299, item.clientDescription)}\n{CSString.Format(timeId, item.cdTime * 0.001f)}    {CSString.Format(mpcostId, item.mpCost)}";
                }
            }
            else
                return CSString.Format(1299, item.clientDescription);
        }
        return string.Empty;
    }

    public static bool IsActivedSkill(this TABLE.SKILL item)
    {
        if (null != item)
        {
            if (item.type != 6 && item.type != 7)
                return true;
        }
        return false;
    }
}

public class SkillInfoData : IndexedItem
{
    public int SkillGroup
    {
        get
        {
            return null == item ? 0 : item.skillGroup;
        }
    }
    public static void Compare(ref long sortValue, SkillInfoData r)
    {
        sortValue = r.item.id;
    }
    public int Index { get; set; }
    public TABLE.SKILL item;

    public TABLE.SKILL currentValidItem
    {
        get
        {
            if (learned)
                return CSSkillInfo.Instance.GetValidSkillItem(null == item ? 0 : item.id, false);
            return item;
        }
    }

    public TABLE.SKILL nextValidItem
    {
        get
        {
            if (learned)
                return CSSkillInfo.Instance.GetValidSkillItem(null == item ? 0 : item.id, true);
            return nextItem;
        }
    }

    public bool QualityFly
    {
        get
        {
            TABLE.SKILL nvi = nextValidItem;
            return null != nvi && !string.IsNullOrEmpty(nvi.Speciallv) && currentValidItem != nvi;
        }
    }

    protected TABLE.SKILL _nextItem;
    public TABLE.SKILL nextItem
    {
        get
        {
            return learned ? _nextItem : item;
        }
        set
        {
            _nextItem = value;
        }
    }
    public bool learned
    {
        get
        {
            return null != item && CSSkillInfo.Instance.HasSkillLearned(item.id);
        }
    }

    public bool canBeDraged
    {
        get
        {
            return null != item && item.type != 6 && item.type != 7 && learned;
        }
    }

    public void Reset()
    {
        _nextItem = null;
    }

    public bool isLevelFull
    {
        get
        {
            return null != item && learned && item == nextItem;
        }
    }

    public bool canUpgrade
    {
        get
        {
            if (null == nextItem || null == item)
                return false;

            if (isLevelFull)
                return false;

            if (!nextItem.studyLevel.IsMainPlayerLevelEnough())
                return false;

            if (!nextItem.cost.IsItemsEnough())
                return false;

            return true;
        }
    }

    public bool callUpgrade
    {
        get
        {
            if (null == nextItem || null == item)
                return false;

            if (isLevelFull)
            {
                UtilityTips.ShowRedTips(544);
                return false;
            }

            if (!nextItem.studyLevel.IsMainPlayerLevelEnough(true))
                return false;
            if (!nextItem.cost.IsItemsEnough(543, true))
                return false;
            return true;
        }
    }

    public bool hasSpecialEffect
    {
        get
        {
            return nextHighEffectDistance > 0;
        }
    }
    public int nextHighEffectDistance
    {
        get
        {
            var civ = currentValidItem;
            if (null == civ)
                return -1;

            int dis = CSSkillInfo.Instance.GetNextSpecialDistance(civ.id);
            return learned ? dis : dis + 1;
        }
    }
    public System.Action<SkillInfoData> onSkillClicked;
}

public class SkillCD
{
    public int skillId;
    public System.Int64 startTime;
    public System.Int64 endTime;
    public System.Int64 cdTime;
    public TABLE.SKILL skillItem;
    protected float process;

    public void Reset()
    {
        startTime = 0;
        skillId = 0;
        endTime = 0;
        cdTime = 0;
        process = 0.0f;
        skillItem = null;
    }

    public SkillCD Copy(SkillCD r)
    {
        this.skillId = r.skillId;
        this.startTime = r.startTime;
        this.endTime = r.endTime;
        this.cdTime = r.cdTime;
        this.skillItem = r.skillItem;
        this.process = r.process;
        return this;
    }
    public bool IsCDEnd
    {
        get
        {
            return (long)(Time.realtimeSinceStartup * 1000) >= endTime;
        }
    }
    public void CalculateCD()
    {
        var delta = (long)(Time.realtimeSinceStartup * 1000) - startTime;
        float v = 1.0f - delta * 1.0f / cdTime;
        v = Mathf.Clamp01(v);
        process = v;
    }

    public float Process
    {
        get
        {
            return process;
        }
    }
}

public partial class CSSkillInfo : CSInfo<CSSkillInfo>
{
    //[职业] => 技能组ID => 技能信息
    protected Dictionary<int, FastArrayElementKeepHandle<SkillInfoData>>[] mSkillGroup2List = new Dictionary<int, FastArrayElementKeepHandle<SkillInfoData>>[4];
    protected Dictionary<int, SkillInfoData> mJob2SkillDic = new Dictionary<int, SkillInfoData>(32);
    protected FastArrayElementKeepHandle<SkillInfoData> mJobRelativedSkills = new FastArrayElementKeepHandle<SkillInfoData>(64);
    protected long[] mSkillShortCuts = new long[Constant.CONST_SKILL_SHORTCUT_LENGTH];
    protected Dictionary<int, SkillCD> mSkillCDDic = new Dictionary<int, SkillCD>(32);
    protected FastArrayElementKeepHandle<SkillCD> mTraveledCDItems = new FastArrayElementKeepHandle<SkillCD>(8);
    protected HashSet<int> mSkillAutoPlayForbiddenDic = new HashSet<int>();
    protected HashSet<int> mSkillGroupAutoPlayForbiddenDic = new HashSet<int>();
    protected PoolHandleManager mPoolHandle = new PoolHandleManager();
    protected bool mCDDirty = false;
    //protected Schedule mSchedule;
    protected TimerEventHandle mSchedule;
    private float mDefaultPublicCd = 0.66f;
    /// <summary>
    /// 已经学习的技能ID => skillInfo
    /// </summary>
    protected Dictionary<int, SkillInfo> mLearnedSkillDic = new Dictionary<int, SkillInfo>(16);
    /// <summary>
    /// 已经学习的技能 group => level
    /// </summary>
    protected Dictionary<int, int> mSkillGroup2LvDic = new Dictionary<int, int>(16);
    /// <summary>
    /// 装备附加技能 group => level
    /// </summary>
    protected Dictionary<int, int> mSkillModifiedDic = new Dictionary<int, int>(16);
    /// <summary>
    /// 技能最大等级字典
    /// </summary>
    protected Dictionary<int, int> mSkillGroup2MaxLv = new Dictionary<int, int>(16);
    /// <summary>
    /// 技能特殊信息字典 [key] =>[距离特殊效果等级]
    /// </summary>
    protected Dictionary<int, int> mNextDistanceDic = new Dictionary<int, int>(16);

    public int GetNextSpecialDistance(int skillId)
    {
        int skillGroup = skillId / 1000;
        int level = skillId % 1000;
        return GetNextSpecialDistance(skillGroup, level);
    }

    public int GetNextSpecialDistance(int skillGroup, int level)
    {
        int maxLv = mSkillGroup2MaxLv.ContainsKey(skillGroup) ? mSkillGroup2MaxLv[skillGroup] : 0;
        if (maxLv < 1)
            return -1;

        level = Mathf.Min(level, maxLv);

        int skillId = skillGroup * 1000 + level;
        if (mNextDistanceDic.ContainsKey(skillId))
            return mNextDistanceDic[skillId];

        TABLE.SKILL skillItem = null;
        if (SkillTableManager.Instance.TryGetValue(skillId, out skillItem))
        {
            int nextLv = -1;
            for (int i = maxLv; i >= 1; --i)
            {
                int makedId = skillGroup * 1000 + i;
                TABLE.SKILL makedItem = null;
                mNextDistanceDic.Remove(makedId);
                if (SkillTableManager.Instance.TryGetValue(makedId, out makedItem))
                {
                    ++nextLv;
                    mNextDistanceDic.Add(makedId, nextLv);
                    if (!string.IsNullOrEmpty(makedItem.Speciallv))
                    {
                        nextLv = 0;
                    }
                }
                else
                {
                    mNextDistanceDic.Add(makedId, -1);
                }
            }
        }

        if (mNextDistanceDic.ContainsKey(skillId))
            return mNextDistanceDic[skillId];

        mNextDistanceDic.Add(skillId, -1);
        return -1;
    }

    public bool HasSkillLearned(int skillId)
    {
        return mSkillGroup2LvDic.ContainsKey(skillId / 1000) && mSkillGroup2LvDic[skillId / 1000] > 0;
    }

    /// <summary>
    /// 获取人物已经学习的主动技能
    /// </summary>
    /// <returns></returns>
    public int GetPlayerLearnedActivedSkill()
    {
        int cnt = 0;
        for (var it = mSkillGroup2LvDic.Keys.GetEnumerator(); it.MoveNext();)
        {
            TABLE.SKILL skillItem = null;
            int id = it.Current * 1000 + 1;
            if (!SkillTableManager.Instance.TryGetValue(id, out skillItem))
            {
                continue;
            }
            if (!IsActivedSkill(skillItem))
            {
                continue;
            }
            ++cnt;
        }
        return cnt;
    }

    public int GetLearnedSkillLvBySkillGroup(int group)
    {
        return mSkillGroup2LvDic.ContainsKey(group) ? mSkillGroup2LvDic[group] : 0;
    }

    public int GetLearnedSkillIdByGroup(int group)
    {
        if (mSkillGroup2LvDic.TryGetValue(group, out int skillLv))
        {
            return group * 1000 + skillLv;
        }
        return 0;
    }

    public SkillInfo GetLearnedSkillByGroup(int group)
    {
        if (mSkillGroup2LvDic.TryGetValue(group, out int skillLv))
        {
            if (mLearnedSkillDic.TryGetValue(group * 1000 + skillLv, out SkillInfo skillInfo))
                return skillInfo;
        }
        return null;
    }

    public Dictionary<int, SkillInfo> GetLearnedSkills()
    {
        return mLearnedSkillDic;
    }

    public bool GetSkillNeedCostMp(int skillGroup)
    {
        if (CSInstanceInfo.Instance.isGuwu(skillGroup))
        {
            //FNDebug.LogFormat("<color=#cd4e8f>[不需要耗蓝]:{0}</color>", skillGroup);
            return false;
        }
        return true;
    }

    public float StartPublicCdTime { get; set; }

    public SkillCD Get()
    {
        var skillCD = mPoolHandle.GetSystemClass<SkillCD>();
        return skillCD;
    }

    public void Put(SkillCD skillCD)
    {
        if (null != skillCD)
            mPoolHandle.Recycle(skillCD);
    }

    PoolHandleManager mPoolHandleManager = new PoolHandleManager();
    bool mInitialized = false;
    public void Initialize(PlayerInfo playerInfo)
    {
        if (mInitialized)
        {
            return;
        }
        mInitialized = true;
        LoadSkillMaxLv();
        LoadLearnedSkill(playerInfo.skills);
        ModifyAttachedSkill(playerInfo.skillRefixData);
        LoadTableSkills();
        InitSkillShortCuts();
        LoadAutoPlaySkills();
        CalculateJobRelativeSkills();
        mSkillCDDic.Clear();
        mClientEvent.AddEvent(CEvent.OnSkillEnterCD, OnSkillEnterCD);
        mClientEvent.AddEvent(CEvent.OnSkillCoolDown, OnSkillCoolDown);
        mCDDirty = true;
        mSchedule = CSTimer.Instance.InvokeRepeating(0.0f, 0.001f, OnSchedule,-1,8888);
        mQualityFlyList = mPoolHandleManager.CreateGeneratePool<SkillEffectItemData>(8);

        CSSkillPriorityInfo.Instance.Initialize();
    }

    public int GetMaxSkillLv(int skillId)
    {
        int skillGroup = skillId / 1000;
        return mSkillGroup2MaxLv.ContainsKey(skillGroup) ? mSkillGroup2MaxLv[skillGroup] : 0;
    }

    public int GetAttachedSkillLv(int skillId)
    {
        int skillGroup = skillId / 1000;
        if (skillGroup <= 0)
            return 0;
        int attachedLv = mSkillModifiedDic.ContainsKey(skillGroup) ? mSkillModifiedDic[skillGroup] : 0;
        attachedLv += AllAddSkillLv;
        return attachedLv;
    }

    public TABLE.SKILL GetValidSkillItem(int skillId, bool next)
    {
        int skillGroupId = skillId / 1000;
        if (!mSkillGroup2LvDic.ContainsKey(skillGroupId))
        {
            return null;
        }
        TABLE.SKILL item = null;
        int maxLv = mSkillGroup2MaxLv.ContainsKey(skillGroupId) ? mSkillGroup2MaxLv[skillGroupId] : 0;
        if (maxLv < 1)
            return null;
        int learnedLv = mSkillGroup2LvDic[skillGroupId] + GetAttachedSkillLv(skillId) + (next ? 1 : 0);
        learnedLv = Mathf.Min(learnedLv, maxLv);
        int newSkillId = skillGroupId * 1000 + learnedLv;
        SkillTableManager.Instance.TryGetValue(newSkillId, out item);
        return item;
    }

    protected void LoadAutoPlaySkills()
    {
        mSkillAutoPlayForbiddenDic.Clear();
        mSkillGroupAutoPlayForbiddenDic.Clear();
        var myinfo = CSMainPlayerInfo.Instance.GetMyInfo();
        if (null == myinfo || null == myinfo.autoSkills)
        {
            mClientEvent.SendEvent(CEvent.OnSkillAutoPlayChanged);
            return;
        }

        //在列表中的是被关闭的自动施放列表
        var autoPlaySkill = myinfo.autoSkills;
        for (int i = 0; i < autoPlaySkill.skillIds.Count; ++i)
        {
            mSkillAutoPlayForbiddenDic.Add(autoPlaySkill.skillIds[i]);
            mSkillGroupAutoPlayForbiddenDic.Add(autoPlaySkill.skillIds[i] / 1000);
        }
        mClientEvent.SendEvent(CEvent.OnSkillAutoPlayChanged);

    }

    public bool CheckHasCanUpgradeSkill()
    {
        var datas = GetJobRelativeSkills();
        for (int i = 0; i < datas.Count; ++i)
            if (datas[i].canUpgrade)
                return true;
        return false;
    }

    public bool GetSkillAutoPlay(int skillId)
    {
        TABLE.SKILL skill = null;
        if (!SkillTableManager.Instance.TryGetValue(skillId, out skill))
            return false;

        if (skill.automatically == 0)
            return false;

        return !mSkillAutoPlayForbiddenDic.Contains(skillId);
    }

    public bool GetSkillAutoPlayByGroup(int skillGroup)
    {
        return !mSkillGroupAutoPlayForbiddenDic.Contains(skillGroup);
    }

    public bool IsSwitchTypeSkill(int skillId)
    {
        TABLE.SKILL skill = null;
        if (!SkillTableManager.Instance.TryGetValue(skillId, out skill))
            return false;

        if (skill.automatically != 2)
            return false;

        return true;
    }

    public void AddSkillToForbiddenList(int skillId, bool needHint)
    {
        if (mSkillAutoPlayForbiddenDic.Add(skillId))
        {
            mSkillGroupAutoPlayForbiddenDic.Add(skillId / 1000);
            TABLE.SKILL skillItem;
            if (SkillTableManager.Instance.TryGetValue(skillId, out skillItem))
            {
                if (needHint)
                {
                    UtilityTips.LeftDownTips(string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(880), skillItem.name).BBCode(ColorType.Red), true);
                    //UtilityTips.ShowRedTips(880, skillItem.name);
                }
            }
            mClientEvent.SendEvent(CEvent.OnSkillAutoPlayChanged);
            CSSkillPriorityInfo.Instance.RemoveCommonFight(skillId);
            CSSkillPriorityInfo.Instance.RemoveAutoFight(skillId);
        }
    }

    public void RemoveSkillFromForbiddenList(int skillId, bool needHint)
    {
        if (mSkillAutoPlayForbiddenDic.Remove(skillId))
        {
            mSkillGroupAutoPlayForbiddenDic.Remove(skillId / 1000);
            TABLE.SKILL skillItem;
            if (SkillTableManager.Instance.TryGetValue(skillId, out skillItem))
            {
                if (needHint)
                {
                    UtilityTips.LeftDownTips(string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(879), skillItem.name).BBCode(ColorType.Green), true);
                    //UtilityTips.ShowGreenTips(879, skillItem.name);
                }
            }
            CSSkillPriorityInfo.Instance.AddCommonFight(skillId);
            CSSkillPriorityInfo.Instance.AddAutoFight(skillId);
            mClientEvent.SendEvent(CEvent.OnSkillAutoPlayChanged);
        }
    }

    protected FastArrayMeta<int> mCDList = new FastArrayMeta<int>(8);

    protected void OnSchedule()
    {
        mCDList.Clear();

        for(int i = 0,max = mTraveledCDItems.Count;i < max;++i)
        {
            var cdItem = mTraveledCDItems[i];
            if(cdItem.IsCDEnd)
            {
                mCDList.Add(cdItem.skillId / 1000);
            }
            else
            {
                cdItem.CalculateCD();
            }
        }

        if (mCDList.Count > 0)
            mCDDirty = true;

        for (int i = 0,max = mCDList.Count; i < max; ++i)
        {
            var cd = mCDList[i];
            var cdItem = mSkillCDDic[cd];
            mTraveledCDItems.SwapRemove(cdItem);
            mSkillCDDic.Remove(mCDList[i]);
            Put(cdItem);
        }

        if (mCDDirty)
        {
            mCDDirty = false;
            mClientEvent.SendEvent(CEvent.OnSkillCDChanged);
        }
    }

    protected void OnSkillEnterCD(uint id, object argv)
    {
        if (!(argv is SkillCD skillCD))
            return;

        int skillGroup = skillCD.skillId / 1000;

        if (!mSkillCDDic.ContainsKey(skillGroup))
        {
            mSkillCDDic.Add(skillGroup, skillCD);
            mTraveledCDItems.Append(skillCD);
        }
        else
        {
            SkillCD stored = mSkillCDDic[skillGroup];
            if (!object.ReferenceEquals(mSkillCDDic[skillGroup],skillCD))
            {
                mTraveledCDItems.SwapRemove(stored);
                Put(stored);
                mSkillCDDic[skillGroup] = skillCD;
                mTraveledCDItems.Append(skillCD);
            }
        }
        mCDDirty = true;
    }

    protected void OnSkillCoolDown(uint id, object argv)
    {
        if (!(argv is SkillCD skillCD))
            return;

        int skillGroup = skillCD.skillId / 1000;
        if (mSkillCDDic.ContainsKey(skillGroup))
        {
            var cdItem = mSkillCDDic[skillGroup];
            mTraveledCDItems.SwapRemove(cdItem);
            mSkillCDDic.Remove(skillGroup);
            Put(cdItem);
            mCDDirty = true;
        }
    }

    public bool IsSkillInCD(int skillId)
    {
        int skillGroup = skillId / 1000;
        return mSkillCDDic.ContainsKey(skillGroup);
    }

    public float GetSkillCDAmount(int skillId)
    {
        int skillGroup = skillId / 1000;
        SkillCD skillCD = null;
        if (mSkillCDDic.ContainsKey(skillGroup))
            skillCD = mSkillCDDic[skillGroup];

        return null == skillCD ? 0.0f : skillCD.Process;
    }

    protected void InitSkillShortCuts()
    {
        for (int i = 0; i < mSkillShortCuts.Length; ++i)
        {
            mSkillShortCuts[i] = 0;
            mSkillShortCuts[i] = mSkillShortCuts[i].SetKeyCode(i);
        }
        SetSkillShortCuts(CSMainPlayerInfo.Instance.GetMyInfo().skillShortCut);
    }

    public void SetSkillShortCuts(Google.Protobuf.Collections.RepeatedField<long> shortCuts)
    {
        for (int i = 0; i < shortCuts.Count; ++i)
        {
            int keyCode = shortCuts[i].KeyCode();
            int keyValue = shortCuts[i].KeyValue();
            if (keyCode >= 0 && keyCode < mSkillShortCuts.Length)
            {
                mSkillShortCuts[keyCode] = mSkillShortCuts[keyCode].SetKeyValue(keyValue);
            }
        }
    }

    public void SetSlotValue(int keyCode, int value, bool sendMsg)
    {
        if (!(keyCode >= 0 && keyCode < mSkillShortCuts.Length))
        {
            return;
        }
        mSkillShortCuts[keyCode] = mSkillShortCuts[keyCode].SetKeyValue(value);
        if (sendMsg)
            Net.ReqSaveSkillShortCutMessage(mSkillShortCuts);
    }

    public int GetSlotKeyBySkill(int skillId)
    {
        for (int i = 0, max = mSkillShortCuts.Length; i < max; ++i)
        {
            if (mSkillShortCuts[i].KeyValue() == skillId)
                return mSkillShortCuts[i].KeyCode();
        }
        return -1;
    }

    public void ResetSlotValues()
    {
        for (int i = 0; i < mSkillShortCuts.Length; ++i)
        {
            mSkillShortCuts[i] = mSkillShortCuts[i].SetKeyValue(0);
        }
        Net.ReqSaveSkillShortCutMessage(mSkillShortCuts);
    }

    public int GetSlotValue(int keyCode)
    {
        if (keyCode < 0 || keyCode >= mSkillShortCuts.Length)
            return 0;
        return mSkillShortCuts[keyCode].KeyValue();
    }

    FastArrayElementFromPool<SkillEffectItemData> mQualityFlyList;
    public FastArrayElementFromPool<SkillEffectItemData> GetQualityFlyListByGroupId(int skillGroupId)
    {
        mQualityFlyList.Clear();
        int jobId = CSMainPlayerInfo.Instance.Career;
        if (!(jobId > 0 && jobId < 4))
        {
            return mQualityFlyList;
        }

        int maxLv = mSkillGroup2MaxLv.ContainsKey(skillGroupId) ? mSkillGroup2MaxLv[skillGroupId] : 0;
        if (maxLv < 1)
            return mQualityFlyList;

        for (int i = 1; i <= maxLv; ++i)
        {
            int id = i + skillGroupId * 1000;
            TABLE.SKILL skillItem = null;
            if (!SkillTableManager.Instance.TryGetValue(id, out skillItem))
            {
                continue;
            }
            if (!string.IsNullOrEmpty(skillItem.Speciallv))
            {
                var data = mQualityFlyList.Append();
                data.learned = HasSkillLearned(id);
                data.skillItem = skillItem;
            }
        }
        return mQualityFlyList;
    }

    public FastArrayElementKeepHandle<SkillInfoData> GetJobRelativeSkills()
    {
        return mJobRelativedSkills;
    }

    public bool GetSkillInstalled(int skillId)
    {
        for (int i = 0; i < mSkillShortCuts.Length; ++i)
            if (mSkillShortCuts[i].KeyValue() == skillId)
                return true;
        return false;
    }

    bool IsActivedSkill(TABLE.SKILL skill)
    {
        if (null != skill && skill.type != 6 && skill.type != 7)
            return true;
        return false;
    }

    bool TryAddSkill(int skillId)
    {
        TABLE.SKILL skillItem;
        if (!SkillTableManager.Instance.TryGetValue(skillId, out skillItem))
        {
            return false;
        }

        if (mLearnedSkillDic.Remove(skillId))
        {
            RemoveMergedSkill(skillId);
        }

        var skill = new fight.SkillInfo();
        skill.sid = skillId;
        skill.level = skillItem.level;
        skill.name = skillItem.name;
        mLearnedSkillDic.Add(skillId, skill);

        AddMergedSkill(skillId);

        return true;
    }

    void RemoveMergedSkill(int skillId)
    {
        int skillGroup = skillId / 1000;
        int skillLv = skillId % 1000;

        //吕天行说普攻不用检测
        if (skillGroup <= 0)
        {
            return;
        }

        if (mSkillGroup2LvDic.ContainsKey(skillGroup))
        {
            mSkillGroup2LvDic[skillGroup] -= skillLv;
#if UNITY_EDITOR
            if (mSkillGroup2LvDic[skillGroup] < 0)
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

    void AddMergedSkill(int skillId)
    {
        int skillGroup = skillId / 1000;
        int skillLv = skillId % 1000;

        //吕天行说普攻不用检测
        if (skillGroup <= 0)
        {
            return;
        }

        if (mSkillGroup2LvDic.ContainsKey(skillGroup))
        {
            mSkillGroup2LvDic[skillGroup] += skillLv;
        }
        else
        {
            mSkillGroup2LvDic.Add(skillGroup, skillLv);
        }
        //#if UNITY_EDITOR
        //        VerifyMergeSkill(skillId);
        //#endif
    }

    void VerifyMergeSkill(int skillId)
    {
        int skillGroup = skillId / 1000;
        int skillLv = skillId % 1000;

        //吕天行说普攻不用检测
        if (skillGroup <= 0)
        {
            return;
        }

        TABLE.SKILL skillItem;
        if (!SkillTableManager.Instance.TryGetValue(skillId, out skillItem))
        {
            FNDebug.LogErrorFormat("[技能校验错误]: skillId = {0} 通知吕天行", skillId);
            FNDebug.LogErrorFormat("[技能校验错误]: skillId = {0} 通知吕天行", skillId);
            FNDebug.LogErrorFormat("[技能校验错误]: skillId = {0} 通知吕天行", skillId);
            FNDebug.LogErrorFormat("[技能校验错误]: skillId = {0} 通知吕天行", skillId);
            return;
        }
        if (skillItem.skillGroup != skillGroup || skillItem.level != skillLv)
        {
            FNDebug.LogErrorFormat("[技能校验错误]: skillId = {0} 通知吕天行 技能ID规则不满足 公式 skillGroup * 1000 + level = skillId", skillId);
            FNDebug.LogErrorFormat("[技能校验错误]: skillId = {0} 通知吕天行 技能ID规则不满足 公式 skillGroup * 1000 + level = skillId", skillId);
            FNDebug.LogErrorFormat("[技能校验错误]: skillId = {0} 通知吕天行 技能ID规则不满足 公式 skillGroup * 1000 + level = skillId", skillId);
            FNDebug.LogErrorFormat("[技能校验错误]: skillId = {0} 通知吕天行 技能ID规则不满足 公式 skillGroup * 1000 + level = skillId", skillId);
            return;
        }

        FNDebug.LogWarningFormat("<color=#00ff00>[技能]:[{0}][Lv.{1}]</color>", skillItem.name, mSkillGroup2LvDic[skillGroup]);
    }

    public void AddNewSkill(int skillId, bool dirty = true)
    {
        if (!TryAddSkill(skillId))
        {
            return;
        }
        CalculateJobRelativeSkills();
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnSkillAdded, skillId);
    }

    public void RemoveSkill(int skillId)
    {
        if (mLearnedSkillDic.Remove(skillId))
        {
            RemoveMergedSkill(skillId);
            CalculateJobRelativeSkills();
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnSkillRemoved, skillId);
        }
    }

    public void UpgradeSkill(int oldId, int newId)
    {
        if (oldId == newId)
        {
            return;
        }

        if (!mLearnedSkillDic.Remove(oldId))
            return;

        RemoveMergedSkill(oldId);

        UtilityTips.ShowGreenTips(553);

        TryAddSkill(newId);
        CalculateJobRelativeSkills();

        TABLE.SKILL oldItem = null;
        TABLE.SKILL newItem = null;
        if (oldId != 0 && !SkillTableManager.Instance.TryGetValue(oldId, out oldItem) || !SkillTableManager.Instance.TryGetValue(newId, out newItem))
        {
            return;
        }

        if (null != oldItem && oldItem.skillGroup != newItem.skillGroup)
            return;

        int jobId = CSMainPlayerInfo.Instance.Career;
        if (!(jobId > 0 && jobId < 4))
        {
            return;
        }

        var orgDic = mSkillGroup2List[jobId];
        if (null == orgDic || null != oldItem && !orgDic.ContainsKey(oldItem.skillGroup) || !orgDic.ContainsKey(newItem.skillGroup))
            return;

        //查找原来的数据
        for (int i = 0; i < mJobRelativedSkills.Count; ++i)
        {
            var skill = mJobRelativedSkills[i];
            if (skill.item.id == oldId || skill.item.id == newId)
            {
                bool replaced = false;
                var skillArray = orgDic[(int)skill.item.skillGroup];
                for (int j = 0; j < skillArray.Count; ++j)
                {
                    if (skillArray[j].item.id == newItem.id)
                    {
                        mJobRelativedSkills[i] = skillArray[j];
                        replaced = true;
                    }
                }

                if (replaced)
                {
                    //执行槽位技能替换
                    TryReSaveSkillShortCuts(oldItem, newItem);
                    //执行技能快捷替换
                    TryReSetAutoPlaySkill(oldItem, newItem);
                    mClientEvent.SendEvent(CEvent.SkillUpgradeSucceed, mJobRelativedSkills[i]);
                }
                break;
            }
        }
        CSSkillPriorityInfo.Instance.Update(oldItem, newItem);
    }

    protected void TryReSaveSkillShortCuts(TABLE.SKILL oldItem, TABLE.SKILL newItem)
    {
        if (!(null != oldItem && null != newItem && oldItem.skillGroup == newItem.skillGroup))
        {
            return;
        }

        for (int i = 0; i < mSkillShortCuts.Length; ++i)
        {
            int skillId = mSkillShortCuts[i].KeyValue();
            if (skillId == oldItem.id)
            {
                mSkillShortCuts[i] = mSkillShortCuts[i].SetKeyValue(newItem.id);
                Net.ReqSaveSkillShortCutMessage(mSkillShortCuts);
                return;
            }
        }
    }

    protected void TryReSetAutoPlaySkill(TABLE.SKILL oldItem, TABLE.SKILL newItem)
    {
        if (!(null != oldItem && null != newItem && oldItem.skillGroup == newItem.skillGroup))
        {
            return;
        }

        //在列表里的表示关闭自动释放的
        if (mSkillAutoPlayForbiddenDic.Contains(oldItem.id))
        {
            mSkillAutoPlayForbiddenDic.Remove(oldItem.id);
            mSkillGroupAutoPlayForbiddenDic.Remove(oldItem.id / 1000);
            mSkillAutoPlayForbiddenDic.Add(newItem.id);
            mSkillGroupAutoPlayForbiddenDic.Add(newItem.id / 1000);
            Net.ReqSetSkillAutoStateMessage(oldItem.id, true);
            Net.ReqSetSkillAutoStateMessage(newItem.id, false);
        }
    }

    //计算本职业相关所有技能
    FastArrayElementKeepHandle<SkillInfoData> CalculateJobRelativeSkills()
    {
        mJob2SkillDic.Clear();
        mJobRelativedSkills.Clear();

        int jobId = CSMainPlayerInfo.Instance.Career;
        if (!(jobId > 0 && jobId < 4))
        {
            return mJobRelativedSkills;
        }

        if (null == mSkillGroup2List[jobId])
        {
            return mJobRelativedSkills;
        }

        var it = mSkillGroup2List[jobId].GetEnumerator();
        while (it.MoveNext())
        {
            var skillList = it.Current.Value;
            if (null == skillList || skillList.Count <= 0)
                continue;

            InitNextSkill(skillList);

            mJob2SkillDic.Add(it.Current.Key, skillList[0]);
        }

        ReplaceLearnedSkills(mJob2SkillDic);

        var itValid = mJob2SkillDic.GetEnumerator();
        while (itValid.MoveNext())
        {
            if (itValid.Current.Value.item.show != 1)
                continue;
            mJobRelativedSkills.Append(itValid.Current.Value);
        }

        mJobRelativedSkills.Sort(SkillCompare);
        return mJobRelativedSkills;
    }

    protected void SkillCompare(ref long sortValue, SkillInfoData r)
    {
        sortValue = r.item.showorder;
    }

    void InitNextSkill(FastArrayElementKeepHandle<SkillInfoData> skillArray)
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

    void ReplaceLearnedSkills(Dictionary<int, SkillInfoData> job2Skills)
    {
        int jobId = CSMainPlayerInfo.Instance.Career;
        if (!(jobId > 0 && jobId < 4))
        {
            return;
        }

        var orgDic = mSkillGroup2List[jobId];
        if (null == orgDic)
        {
            return;
        }

        var it = mLearnedSkillDic.GetEnumerator();
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

            SkillInfoData skillData = null;
            skillArray.Sort(SkillInfoData.Compare);

            for (int j = 0; j < skillArray.Count; ++j)
            {
                var skill = skillArray[j];
                //设置命中
                if (skillArray[j].item.id == skillInfo.sid)
                {
                    skillData = skillArray[j];
                }
            }

            if (null == skillData)
            {
                continue;
            }

            job2Skills[(int)item.skillGroup] = skillData;
        }
    }

    HashSet<int> modifyiedList = new HashSet<int>();
    public void ModifyAttachedSkill(SkillRefixInfo skillRefixInfo)
    {
        AllAddSkillLv = skillRefixInfo.allSkillRefixLevel;
        modifyiedList.Clear();
        var it = mSkillModifiedDic.GetEnumerator();
        while (it.MoveNext())
        {
            modifyiedList.Add(it.Current.Key);
        }
        mSkillModifiedDic.Clear();

        for (int i = 0, max = skillRefixInfo.levelRefixList.Count; i < max; ++i)
        {
            var kv = skillRefixInfo.levelRefixList[i];
            modifyiedList.Add(kv.type);

            if (!mSkillModifiedDic.ContainsKey(kv.type))
            {
                mSkillModifiedDic.Add(kv.type, kv.value);
            }
            else
            {
                if (mSkillModifiedDic[kv.type] != kv.value)
                {
                    mSkillModifiedDic[kv.type] = kv.value;
                }
            }
        }

        var itHash = modifyiedList.GetEnumerator();
        while (itHash.MoveNext())
        {
            int skillGroup = itHash.Current;
            int realLv = mSkillModifiedDic.ContainsKey(skillGroup) ? mSkillModifiedDic[skillGroup] : 0;
            int lv = realLv > 0 ? realLv : 1;
            int skillId = skillGroup * 1000 + lv;
            TABLE.SKILL skillItem = null;
            if (!SkillTableManager.Instance.TryGetValue(skillId, out skillItem))
            {
                FNDebug.LogErrorFormat("[技能修正]:group={0} lv={1} 技能ID={2}无法在表中找到", skillGroup, lv, skillId);
                return;
            }

            //if(realLv == 0)
            //{
            //    Debug.LogWarningFormat("<color=#ff00ff>[技能修正]:{0} 已经被移除</color>", skillItem.name, lv);
            //}
            //else
            //{
            //    Debug.LogWarningFormat("<color=#ff00ff>[技能修正]:{0}+{1}</color>", skillItem.name, lv);
            //}
            UtilityFight.LaunchUpgradeSkill(skillId);
            HotManager.Instance.EventHandler.SendEvent(CEvent.AttachedSkillModified, skillGroup);
        }
        modifyiedList.Clear();
    }
    /// <summary>
    /// 计算技能最大等级
    /// </summary>
    protected void LoadSkillMaxLv()
    {
        mSkillGroup2MaxLv.Clear();
        var arr = SkillTableManager.Instance.array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            TABLE.SKILL item = arr[i].Value as TABLE.SKILL;
            int skillGroup = item.skillGroup;
            int skillLv = item.level;
            //这里计算最大技能等级
            if (!mSkillGroup2MaxLv.ContainsKey(skillGroup))
            {
                mSkillGroup2MaxLv.Add(skillGroup, skillLv);
            }
            else
            {
                mSkillGroup2MaxLv[skillGroup] = Mathf.Max(mSkillGroup2MaxLv[skillGroup], skillLv);
            }
        }
    }
    protected void LoadLearnedSkill(RepeatedField<SkillGroupInfo> skills)
    {
        mLearnedSkillDic.Clear();
        mSkillGroup2LvDic.Clear();
        if (null != skills)
        {
            for (int i = 0, max = skills.Count; i < max; ++i)
            {
                var skill = skills[i];
                int sid = skill.sid;
                if (!mLearnedSkillDic.ContainsKey(sid))
                {
                    var skillInfo = new SkillInfo();
                    skillInfo.level = skill.level;
                    skillInfo.sid = sid;
                    mLearnedSkillDic.Add(sid, skillInfo);
                }

                AddMergedSkill(sid);
            }
        }
    }

    public int AllAddSkillLv { get; set; }
    void ModifyAttachedSkill(int skillGroup, int lv)
    {
        if (!mSkillModifiedDic.ContainsKey(skillGroup))
        {
            mSkillModifiedDic.Add(skillGroup, lv);
        }
        else
        {
            if (mSkillModifiedDic[skillGroup] != lv)
            {
                mSkillModifiedDic[skillGroup] = lv;
            }
        }
    }

    public void LoadTableSkills()
    {
        var arr = SkillTableManager.Instance.array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            TABLE.SKILL value = arr[i].Value as TABLE.SKILL;
            int gid = (int)value.skillGroup;
            int career = value.career;
            int userType = value.usertype;

            int idx = -1;
            if (userType == 1)
            {
                if (career == ECareer.Warrior)
                {
                    idx = 1;
                }
                else if (career == ECareer.Master)
                {
                    idx = 2;
                }
                else if (career == ECareer.Taoist)
                {
                    idx = 3;
                }
                else if (career == 4)
                {
                    idx = 0;
                }

                if (idx < 0 || idx >= mSkillGroup2List.Length)
                {
                    continue;
                }

                if (null == mSkillGroup2List[idx])
                {
                    mSkillGroup2List[idx] = new Dictionary<int, FastArrayElementKeepHandle<SkillInfoData>>(32);
                }
                var skillGroup = mSkillGroup2List[idx];

                FastArrayElementKeepHandle<SkillInfoData> gList = null;
                if (!skillGroup.ContainsKey(gid))
                {
                    gList = new FastArrayElementKeepHandle<SkillInfoData>(4);
                    skillGroup.Add(gid, gList);
                }
                else
                {
                    gList = skillGroup[gid];
                }

                var skillData = new SkillInfoData
                {
                    item = value,
                };
                gList.Append(skillData);
            }
            else if (userType == 2)
            {
                if (career == ECareer.Warrior)
                {
                    idx = 1;
                }
                else if (career == ECareer.Master)
                {
                    idx = 2;
                }
                else if (career == ECareer.Taoist)
                {
                    idx = 3;
                }
                else if (career == 4)
                {
                    idx = 0;
                }

                if (idx < 0 || idx >= mPetSkillGroup2List.Length)
                {
                    continue;
                }

                if (null == mPetSkillGroup2List[idx])
                {
                    mPetSkillGroup2List[idx] = new Dictionary<int, FastArrayElementKeepHandle<PetSkillItemData>>(32);
                }
                var skillGroup = mPetSkillGroup2List[idx];
                FastArrayElementKeepHandle<PetSkillItemData> gList = null;
                if (!skillGroup.ContainsKey(gid))
                {
                    gList = new FastArrayElementKeepHandle<PetSkillItemData>(4);
                    skillGroup.Add(gid, gList);
                }
                else
                {
                    gList = skillGroup[gid];
                }
                var skillData = new PetSkillItemData();
                skillData.item = value;
                gList.Append(skillData);
            }
        }

        //添加通用技能至其他职业
        if (null != mSkillGroup2List[0])
        {
            var commonOccur = mSkillGroup2List[0];
            for (int i = 1, max = mSkillGroup2List.Length; i < max; ++i)
            {
                var current = mSkillGroup2List[i];
                if (null != current)
                {
                    var git = commonOccur.GetEnumerator();
                    while (git.MoveNext())
                    {
                        if (!current.ContainsKey(git.Current.Key))
                        {
                            current.Add(git.Current.Key, git.Current.Value);
                        }
                        else
                        {
                            FNDebug.LogError($"[技能表配置错误]:Id Repeated Id = {git.Current.Key}");
                        }
                    }
                }
            }
        }
    }

    public bool IsSkillInPublicCD()
    {
        float attackSpeed = CSMainPlayerInfo.Instance.GetMyAttrById(34) * 0.001f;
        //Debug.Log("=======> attackSpeed = " + attackSpeed);
        attackSpeed = (attackSpeed > 0.001f) ? (attackSpeed + 0.1f) : mDefaultPublicCd;
        return (attackSpeed > (Time.time - StartPublicCdTime));
    }

    public bool IsCanLaunch(int skillId)
    {
        if (!IsSkillInCD(skillId))
        {
            return !IsSkillInPublicCD();
        }
        return false;
    }

    public override void Dispose()
    {
        OnPetSkillDispose();
        if (null != mSchedule)
        {
            CSTimer.Instance.remove_timer(mSchedule);
        }
        mSchedule = null;
        modifyiedList?.Clear();
        modifyiedList = null;
        mLearnedSkillDic.Clear();
        mLearnedSkillDic = null;
        mSkillGroup2LvDic.Clear();
        mSkillGroup2LvDic = null;
        mTraveledCDItems.Clear();
        mTraveledCDItems = null;
        mSkillCDDic.Clear();
        mSkillCDDic = null;
        mPoolHandle = null;
        mCDList.Clear();
        mCDList = null;
        mClientEvent.RemoveEvent(CEvent.OnSkillEnterCD, OnSkillEnterCD);
        mClientEvent.RemoveEvent(CEvent.OnSkillCoolDown, OnSkillCoolDown);
        for (int i = 0; i < mSkillGroup2List.Length; ++i)
        {
            mSkillGroup2List[i]?.Clear();
            mSkillGroup2List[i] = null;
        }
        mSkillGroup2List = null;
        mJob2SkillDic.Clear();
        mJob2SkillDic = null;
        mJobRelativedSkills.Clear();
        mJobRelativedSkills = null;
        mSkillAutoPlayForbiddenDic?.Clear();
        mSkillAutoPlayForbiddenDic = null;
        mSkillGroupAutoPlayForbiddenDic?.Clear();
        mSkillGroupAutoPlayForbiddenDic = null;
        StartPublicCdTime = 0;
    }
}