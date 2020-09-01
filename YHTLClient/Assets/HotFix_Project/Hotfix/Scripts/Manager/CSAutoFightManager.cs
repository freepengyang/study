using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSAutoFightManager:Singleton<CSAutoFightManager>
{
    private bool mIsAutoFigth = false;
    private bool mIsCommonFight = false;
    private bool mIsAutoPickItem = false;
    private int mQuality = 0;
    private int mMonsterConfigId = 0;
    private long mPickingItemID = 0;
    private float mDetectTime = 0f; //杀怪间隔
    private float mLastDetectTime = 0f;
    private int mLaunchSkillId = 0;
    private bool mIsReleaseSpecialAutoSkill;
    private bool mIsCheckAutoItemAfter = false;
    private bool mIsTowardTransmit = false;

    public bool IsAutoFight
    {
        get
        {
            return mIsAutoFigth;
        }
        set
        {
            SetAutoFight(value,true);
        }
    }

    public void SetAutoFight(bool value,bool isResetMission)
    {
        mPickingItemID = 0;
        if (mIsAutoFigth != value)
        {
            mIsAutoFigth = value;

            //UnityEngine.Debug.LogFormat("<color=#ff0000> ==========> mIsAutoFight = {0}  Time.time = {1}</color>",mIsAutoFigth,Time.time);

            if (mIsAutoFigth)
            {
                CSPathFinderManager.Instance.ReSetPath(value, false, isResetMission);
            }
            CSPlayerAutoActionInfo.Instance.SetAutoAction();
            HotManager.Instance.EventHandler.SendEvent(CEvent.AutoFight_Change);
        }
        if (!mIsAutoFigth)
        {
            Reset();
        }
    }

    public bool IsCommonFight
    {
        get
        {
            return mIsCommonFight;
        }
        set
        {
            if(mIsCommonFight != value)
            {
                mIsCommonFight = value;
            }
            if(mIsCommonFight)
            {
                ResetLaunchSkill();
            }
        }
    }

    public bool IsAutoPickItem
    {
        get
        {
            return mIsAutoPickItem;
        }
        set
        {
            if(mIsAutoPickItem != value)
            {
                mIsAutoPickItem = value;
            }
        }
    }

    public bool IsLaunchTargetSkill
    {
        get
        {
            return (mLaunchSkillId > 0);
        }
    }

    public int LaunchSkillId
    {
        get
        {
            return mLaunchSkillId;
        }
        set
        {
            mLaunchSkillId = value;
        }
    }

    private bool IsCommonOrClickAttack()
    {
        return (mLaunchSkillId > 0) || (mIsCommonFight);
    }

    public void Update()
    {
        if (CSResourceManager.Instance == null || CSResourceManager.Instance.IsChangingScene)
        {
            return;
        }
        if (!CSScene.IsLanuchMainPlayer)
        {
            mLastDetectTime = Time.time;
            return;
        }
        //if(!CSConstant.IsLanuchMainPlayer)
        //{
        //    mLastDetectTime = Time.time;
        //    return;
        //}
        if (Time.time < CSScene.loadMainPlayerTime + 1) 
        {
            return;
        }
        if (!IsCanRelease())
        {
            return;
        }

        if (!IsPlayerCanMove())
        {
            return;
        }

        mIsReleaseSpecialAutoSkill = DetectSpecialAutoReleaseSkill();


        if (mIsAutoFigth || mIsCommonFight || IsLaunchTargetSkill)
        {
            CSAvatar target = CSAvatarManager.Instance.GetSelectTarget();
            bool isAutoPickItem = false;
            if((target == null) && mIsReleaseSpecialAutoSkill)
            {
                mIsCheckAutoItemAfter = true;
            }

            if((target == null) && (!mIsReleaseSpecialAutoSkill || mIsCheckAutoItemAfter))
            {
                if (Time.time < (mLastDetectTime + mDetectTime) )
                {
                    return;
                }
                if((!mIsCommonFight) && (!IsLaunchTargetSkill))
                {
                    isAutoPickItem = AutoPickItem();
                }
            }
            if(!isAutoPickItem)
            {
                mIsCheckAutoItemAfter = false;
                bool ret = DetectAutoFight();
                if(!ret && mIsReleaseSpecialAutoSkill)
                {
                   CSAvatarManager.MainPlayer.UnSaveTowardTarget();
                }
                if(!ret && !mIsTowardTransmit && CSInstanceInfo.Instance.IsUnionInstanceFinish())
                {
                    mIsTowardTransmit = true;
                    TABLE.SCENETRIGGER tblSceneTrigger = null;
                    if (SceneTriggerTableManager.Instance.TryGetValue(CSMainPlayerInfo.Instance.MapID, out tblSceneTrigger))
                    {
                        CSAvatarManager.MainPlayer.TowardsTarget(new CSMisc.Dot2(tblSceneTrigger.x,tblSceneTrigger.y));
                    }
                }
            }
        }
        else if(mIsAutoPickItem)
        {
            if(!AutoPickItem())
            {
                IsAutoPickItem = false;
            }
            if(!mIsAutoPickItem)
            {
                FNDebug.LogFormat("<color=#00ff00>[PickItem]:发送事件:AutoPickItem_Over</color>");
                HotManager.Instance.EventHandler.SendEvent(CEvent.AutoPickItem_Over);
            }
        }
        else
        {
            CSDropSystem.Instance.DetectPickItemStand();
        }
    }

    private bool DetectAutoFight()
    {
        CSCharacter character =CSAvatarManager.MainPlayer;
        if(IsLaunchTargetSkill)
        {
            SkillAuto skillAuto = CSSkillPriorityInfo.Instance.GetTargetSkillAuto(LaunchSkillId);
            CSAvatar target = null;
            if (skillAuto.isCanRelease)
            {
                target = CSAvatarManager.Instance.GetSelectTargetByType(false,skillAuto.targetType, mMonsterConfigId, mQuality);
                if (target != null || skillAuto.LaunchType == ESkillLaunchType.InSitu)
                {
                    CSSkill skill = character.SkillEngine.AddSkill(skillAuto.skillId);
                    if (skill.IsCanRelease)
                    {
                        TouchData touchData =CSAvatarManager.MainPlayer.TouchEvent;
                        if (skillAuto.LaunchType == ESkillLaunchType.InSitu || target.AvatarType == EAvatarType.MainPlayer)
                        {
                            touchData.Skill = skill;
                            bool targetIsCharacter = true;
                            if(target != null && UtilityFight.IsFlagSkill(skillAuto.group))
                            {
                                if(CSSkillFlagManager.Instance.IsTargetInRange(target))
                                {
                                    targetIsCharacter = false;
                                }
                                else
                                {
                                   target = CSAvatarManager.Instance.GetNearestAttackTarget();
                                    if (CSSkillFlagManager.Instance.IsTargetInRange(target))
                                    {
                                        targetIsCharacter = false;
                                    }
                                }
                            }
                            if(targetIsCharacter)
                            {
                                SetSkillTarget(skill, character);
                                character.ForceAttack(character.OldCell.Coord);
                            }
                            else
                            {
                                SetSkillTarget(skill, target);
                                touchData.SetTarget(target);
                                character.ForceAttack(target.OldCell.Coord);
                            }
                        }
                        else if(target != null)
                        {
                            touchData.Skill = skill;
                            touchData.SetTarget(target);
                            SetSkillTarget(skill, target);
                            character.TowardsTargetAttack(target.OldCell.Coord);
                        }
                        return true;
                    }
                }
            }
            if (target == null)
            {
                LaunchSkillId = 0;
            }
            return false;
        }

        CSSkillInfo skillInfo = CSSkillInfo.Instance;
        ILBetterList<SkillAuto> autoSkills = SkillAutoList;
        if (autoSkills == null)
        {
            return false;
        }
        CSBuffInfo buffInfo = character.BaseInfo.BuffInfo;
        bool isBanYueOpen = CSConfigInfo.Instance.GetBool(ConfigOption.SmartHalfMoonMachetes);
        bool isCommonOrClickAttack = IsCommonOrClickAttack();
        for (int i = 0; i < autoSkills.Count; i++)
        {
            SkillAuto skillAuto = autoSkills[i];
            if (skillAuto == null)
            {
                continue;
            }
            if(!skillAuto.isCanRelease)
            {
                bool isContinute = true;
                if(skillAuto.group == ESkillGroup.BanYue)
                {
                    isContinute = (!isBanYueOpen) && !CSSkillInfo.Instance.GetSkillAutoPlay(skillAuto.id);
                }
                if(isContinute)
                {
                    continue;
                }
            }
            if(skillAuto.targetType == ESkillTargetType.SummonPet)
            {
                int petCount = CSAvatarManager.Instance.GetPetCount();
                if ((skillAuto.skillId < ESpecialSkillID.ShenShou10 && petCount > 0) ||
                    (skillAuto.skillId >= ESpecialSkillID.ShenShou10 && petCount > 1))
                {
                    continue;
                }
            }
            if(skillAuto.group == ESkillGroup.LieHuo)
            {
                continue;
            }
            CSAvatar target = CSAvatarManager.Instance.GetSelectTargetByType(!isCommonOrClickAttack,skillAuto.targetType, mMonsterConfigId, mQuality);
            if(target == null)
            {
                continue;
            }

            if(IsNeedCheckTargetHaveBuff(skillAuto.group))
            {
                if (target.AvatarType == EAvatarType.MainPlayer && buffInfo.IsHasBuff(skillAuto.buffId))
                {
                    continue;
                }
                if (target.BaseInfo != null)
                {
                    CSBuffInfo targetbuffInfo = target.BaseInfo.BuffInfo;
                    if (targetbuffInfo != null && targetbuffInfo.IsHasBuff(skillAuto.buffId))
                    {
                        continue;
                    }
                }
            }

            if(skillAuto.group == ESkillGroup.BanYue)
            {
                bool isVaild = false;
                CSAvatar secondTarget = CSAvatarManager.Instance.GetAttackTargetInRange(character.NewCell.Coord, skillAuto.tblSkill, 0, target);
                isVaild = (secondTarget != null);

                //if (isVaild && (target.AvatarType == EAvatarType.Player || target.AvatarType == EAvatarType.Pet))
                //{
                //    if (!CSAvatarManager.Instance.IsPlayerCanBeSelect(target.BaseInfo, CSMainPlayerInfo.Instance))
                //    {
                //        CSAvatar thirdTarget = CSAvatarManager.Instance.GetAttackTargetInRange(character.NewCell.Coord, skillAuto.tblSkill, 0, secondTarget);
                //        isVaild = isVaild && (thirdTarget != null);
                //    }
                //}
                bool isAutoReleaseOpen = CSSkillInfo.Instance.GetSkillAutoPlay(skillAuto.id);
                if (!isVaild)
                {
                    if (isAutoReleaseOpen)
                    {
                        Net.ReqSetSkillAutoStateMessage(skillAuto.id, false);
                    }
                    continue;
                }
                else if(!isAutoReleaseOpen)
                {
                    Net.ReqSetSkillAutoStateMessage(skillAuto.id, true);
                }
            }
            else if((skillAuto.group == ESkillGroup.BingPaoXiao) || (skillAuto.group == ESkillGroup.LiuXingHuoYu))
            {
                CSAvatar secondTarget = CSAvatarManager.Instance.GetAttackTargetInRange(character.NewCell.Coord, skillAuto.tblSkill, 0, target);
                if (secondTarget == null)
                {
                    continue;
                }
            }

            CSSkill skill = character.SkillEngine.AddSkill(skillAuto.skillId);
            if (skill.IsCanRelease)
            {
                TouchData touchData =CSAvatarManager.MainPlayer.TouchEvent;
                if (target.AvatarType == EAvatarType.MainPlayer || skillAuto.LaunchType == ESkillLaunchType.InSitu)
                {
                    touchData.Skill = skill;
                    if (UtilityFight.IsFlagSkill(skillAuto.group))
                    {
                        SetSkillTarget(skill, target);
                        touchData.SetTarget(target);
                        character.ForceAttack(target.OldCell.Coord);
                    }
                    else
                    {
                        SetSkillTarget(skill, character);
                        character.ForceAttack(character.OldCell.Coord);
                    }
                }
                else
                {
                    touchData.Skill = skill;
                    touchData.SetTarget(target);
                    SetSkillTarget(skill, target);
                    character.TowardsTargetAttack(target.OldCell.Coord);
                }
                return true;
            }
        }

        return false;
    }

    private bool IsPlayerCanMove()
    {
        CSCharacter character =CSAvatarManager.MainPlayer;
        if (character.IsBeControl || character.IsDead || character.IsServerDead)
        {
            return false;
        }
        if (character.IsAttackPlaying())
        {
            return false ;
        }
        return true;
    }

    bool AutoPickItem()
    {
        CSItem item = null;
        if (mPickingItemID != 0)
        {
            item = CSDropManager.Instance.GetItem(mPickingItemID);
            if(item == null)
            {
                mPickingItemID = 0;
                return true;
            }
            CSCharacter chr =CSAvatarManager.MainPlayer;
            bool isCanCrossScene = CSScene.IsCanCrossScene;
            if (CSBagInfo.Instance.IsBagFilled() && 
                (item.itemTbl != null && item.itemTbl.type != BagItemType.Property))
            {
                mPickingItemID = 0;
            }
            else if (!item.OldCell.node.isCanCrossNpc)
            {
                mPickingItemID = 0;
            }
            else if (!isCanCrossScene && !item.OldCell.node.isProtect && item.OldCell.node.avatarNum > 0)
            {
                mPickingItemID = 0;
            }
            else if (!chr.IsMoving)
            {
                if (!chr.OldCell.Coord.Equal(item.OldCell.Coord))
                {
                    CSTouchEvent.Sington.OnHitTerrain(item.OldCell.Coord, false);
                }
                else
                {
                    CSDropSystem.Instance.DetectPickItem(item.OldCell.Coord);
                }
            }
            return true;
        }

        item = CSDropSystem.Instance.GetPriorityItem();
        if (item != null)
        {
            if (!CSDropSystem.Instance.PickUpTypeProrityDic.ContainsKey(item.pickType))
            {
                if (CSAvatarManager.Instance.IsHaveAvatar(EAvatarType.Monster))
                {
                    return false;
                }
            }
            if (item.isLoad)
            {
                mPickingItemID = item.BaseInfo.itemId;
                CSTouchEvent.Sington.OnHitTerrain(item.OldCell.Coord, false);
                mLastDetectTime = Time.time;
                mDetectTime = CSDropSystem.Instance.PickItemIntervalTime;//每次拾取
            }
            else
            {
                mLastDetectTime = Time.time;
                mDetectTime = 0.5f;//增大检测时间
            }
            return true;
        }

        return false;
    }

    private bool DetectSpecialAutoReleaseSkill()
    {
        ILBetterList<SkillAuto> autoSkills = CSSkillPriorityInfo.Instance.AutoReleaseList;
        if (autoSkills == null)
        {
            return false;
        }
        CSSkillInfo skillInfo = CSSkillInfo.Instance;
        int selfMp = CSMainPlayerInfo.Instance.MP;
        CSBuffInfo buffInfo = CSMainPlayerInfo.Instance.BuffInfo;
        
        for (int i = 0; i < autoSkills.Count; ++i)
        {
            SkillAuto skillAuto = autoSkills[i];
            if(skillAuto == null)
            {
                continue;
            }
            if (CSSkillPriorityInfo.skillAutoReleaseNonCombat.Contains(skillAuto.group))
            {
                if (IsCanLaunchTargetSkill(skillAuto, selfMp))
                {
                    if (!buffInfo.IsHasBuff(skillAuto.buffId))
                    {
                        //CSSkillLaunchSystem.Instance.LaunchSkillWithoutAction(skillAuto.skillId);
                        //CSSkillLaunchSystem.Instance.LaunchSkillAuto(skillAuto.tblSkill);

                        switch(skillAuto.group)
                        {
                            case ESkillGroup.LieHuo:
                                {
                                    if (CSMainPlayerInfo.Instance.PlayerFsmState != EPlayerFsmState.Fight)
                                    {
                                        return false;
                                    }
                                    if (UtilityFight.InAttackRange(1))
                                    {
                                        CSSkillLaunchSystem.Instance.LaunchSkillAuto(skillAuto.tblSkill);
                                        return true;
                                    }
                                    return false;
                                }
                            case ESkillGroup.WuJiZhenQi:
                            case ESkillGroup.MagicShield:
                                {
                                    if(CSAvatarManager.MainPlayer.IsMoving)
                                    {
                                        return false;
                                    }
                                }
                                break;
                        }
         
                        if(CSPathFinderManager.Instance.PathGuideState == PathGuideState.AutoMission)
                        {
                            return false;
                        }
     
                        if(mLaunchSkillId != skillAuto.id)
                        {
                            LaunchSkillId = skillAuto.id;
                           CSAvatarManager.MainPlayer.SaveTowardsTarget();
                        }
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void SetSkillTarget(CSSkill skill, CSAvatar target)
    {
        if (skill.Target == null || skill.Target != target)
        {
            skill.Target = target;
        }
    }

    private bool IsCanRelease()
    {
        if (CSSkillInfo.Instance.IsSkillInPublicCD())
        {
            return false;
        }
        return true;
    }

    private bool IsCanLaunchTargetSkill(SkillAuto skillAuto, int selfMp)
    {
        if(skillAuto == null)
        {
            return false;
        }
        if (CSSkillInfo.Instance.IsSkillInCD(skillAuto.id))
        {
            return false;
        }
        if (selfMp < skillAuto.mpCost && CSSkillInfo.Instance.GetSkillNeedCostMp(skillAuto.group))
        {
            return false;
        }
        if (!skillAuto.isCanRelease)
        {
            return false;
        }
        return true;
    }

    public bool IsNeedCheckTargetHaveBuff(int skillGroup)
    {
        switch(skillGroup)
        {
            case ESkillGroup.ShiDuShu:
                return true;
            default:
                return false;
        }
    }

    public void BeginFight(int monsterConfigId,bool resetMission = true)
    {
        mMonsterConfigId = monsterConfigId;
        SetAutoFight(true, resetMission);
    }

    public void BeginFightByQuality(int quality)
    {
        mQuality = quality;
        IsAutoFight = true;
    }

    private ILBetterList<SkillAuto> SkillAutoList
    {
        get
        {
            return (mIsAutoFigth) ? (CSSkillPriorityInfo.Instance.AutoReleaseList) : (CSSkillPriorityInfo.Instance.CommonFightSkillList);
        }
    }

    public void ResetLaunchSkill(int skillId = 0)
    {
        if(skillId == 0 || skillId == mLaunchSkillId)
        {
            mLaunchSkillId = 0;
        }
    }

    public void Stop()
    {
        //TODO:ddn CommonFight情况下 摇杆移动无效会寻路到攻击目标
        if (mIsCommonFight || mIsAutoFigth || IsLaunchTargetSkill)
        {
            if (CSAvatarManager.MainPlayer.TouchEvent.Type == ETouchType.Attack)
            {
               CSAvatarManager.MainPlayer.TouchEvent.Type = ETouchType.Normal;
            }
        }
        IsAutoFight = false;
        IsCommonFight = false;
        mIsAutoPickItem = false;
        mIsCheckAutoItemAfter = false;
        LaunchSkillId = 0;
    }

    public void StopAutoFight()
    {
        if(mIsAutoFigth)
        {
            IsAutoFight = false;
        }
    }

    public void Reset()
    {
        mMonsterConfigId = 0;
        mQuality = 0;
        LaunchSkillId = 0;
        mIsTowardTransmit = false;
    }

    public void Destroy()
    {

    }

}
