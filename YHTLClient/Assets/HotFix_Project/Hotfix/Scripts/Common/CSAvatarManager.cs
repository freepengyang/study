
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fight;

// 模型管理
public class CSAvatarManager : CSGameMgrBase<CSAvatarManager>
{
    private int mAvatarTypeCount = 0;
    private int mAllAvatarNumInView = 0;
    private float mDistance = 0;

    const int INIT_CSAVATAR_COUNT = 32;

    /// <summary>
    /// <avatarID, CSAvatar>
    /// </summary>
    private Dictionary<long, CSAvatar> mAvatarDic = null;
    /// <summary>
    /// <AvatarType,<avatarID,true or false>>
    /// </summary>
    public static Dictionary<int, Dictionary<long, bool>> mAvatarInViewDic = null;
    /// <summary>
    /// <AvatarType, List<CSAvatar>>
    /// </summary>
    public Dictionary<int, List<CSAvatar>> mAvatarListDic = null;

    private ILBetterList<CSAvatar> mSelfPetList = null;

    private CSMisc.Dot2 mSelectTargetCoord = CSMisc.Dot2.Zero;
    private ILBetterList<CSAvatar> mCacheList = null;

    public static CSCharacter MainPlayer;
    public override void Awake()
    {
        base.Awake();
        Initialized(); 
        mSelectTargetCoord = CSMisc.Dot2.Zero;
    }

    private void Initialized()
    {
        mAvatarTypeCount = 21;
        mAvatarDic = new Dictionary<long, CSAvatar>(INIT_CSAVATAR_COUNT);   

        mAvatarInViewDic = new Dictionary<int, Dictionary<long, bool>>()
        {
            {EAvatarType.Player,new Dictionary<long,bool>()},
            {EAvatarType.Monster,new Dictionary<long,bool>()},
            {EAvatarType.NPC,new Dictionary<long,bool>()},
            {EAvatarType.Pet,new Dictionary<long,bool>()},
            {EAvatarType.ZhanHun,new Dictionary<long,bool>()},
        };

        mAvatarListDic = new Dictionary<int, List<CSAvatar>>(INIT_CSAVATAR_COUNT);
        mCacheList = new ILBetterList<CSAvatar>(8);
        mSelfPetList = new ILBetterList<CSAvatar>(4);
        mDistance = 0;

    }

    public void Update()
    {

        if (MainPlayer != null) MainPlayer.Update();

        for (int i = 0; i < mAvatarTypeCount; ++i)
        {
            if (i == EAvatarType.Item || i == EAvatarType.MainPlayer || i == EAvatarType.None) continue;

            List<CSAvatar> avatarList = null;

            if (mAvatarListDic.ContainsKey(i))
            {
                avatarList = mAvatarListDic[i];

                for (int j = 0; j < avatarList.Count; ++j)
                {
                    CSAvatar avatar = avatarList[j];

                    if (avatar != null)
                    {
                        avatar.Update();
                    }
                }
            }
        }
    }
    

    public CSAvatarInfo GetAvatarInfo(long id)
    {
        CSAvatar avatar = GetAvatar(id);
        if (avatar != null && avatar.BaseInfo != null)
        {
            return avatar.BaseInfo;
        }
        return null;
    }

    public CSAvatar GetAvatar(long id)
    {
        if(mAvatarDic.ContainsKey(id))
        {
            return mAvatarDic[id];
        }
        if (MainPlayer != null && MainPlayer.BaseInfo != null && MainPlayer.BaseInfo.ID == id)
        {
            return MainPlayer;
        }
        return null;
    }

    public CSAvatar GetAvatar(long configId, int avatarType)
    {
        List<CSAvatar> list = null;
        if (mAvatarListDic.ContainsKey(avatarType))
        {
            list = mAvatarListDic[avatarType];
            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i].BaseInfo.ConfigId == configId)
                {
                    return list[i];
                }
            }
        }
        return null;
    }

    public void AddAvatar(CSAvatar avatar)
    {
        if (!mAvatarDic.ContainsKey(avatar.ID))
        {
            mAvatarDic.Add(avatar.ID, avatar);
            AddAvatarListDic(avatar);
            AddAvatarInView(avatar);
            AddSelfPet(avatar);
        }
    }

    private void AddAvatarListDic(CSAvatar avatar)
    {
        int avatarType = avatar.AvatarType;
        if (!mAvatarListDic.ContainsKey(avatarType))
        {
            mAvatarListDic.Add(avatarType, new List<CSAvatar>());
        }
        mAvatarListDic[avatarType].Add(avatar);
    }

    public bool RemoveAvatar(long id)
    {
        CSAvatar avatar = GetAvatar(id);
        bool ret = RemoveAvatar(avatar);
        return ret;
    }

    public bool RemoveAvatar(CSAvatar avatar, bool isRemoveAll = false)
    {
        if (avatar != null)
        {
            if (!IsMainPlayer(avatar.ID))
            {
                avatar.Destroy();
                if(!isRemoveAll)
                {
                    mAvatarDic.Remove(avatar.ID);
                }
                RemoveAvatarInView(avatar);
                RemoveAvatarListDic(avatar);
                RemoveSelfPet(avatar);
                return true;
            }
        }
        return false;
    }

    private void Remove(CSAvatar avatar)
    {
        CancleSelectTarget(avatar.ID);
    }

    private void RemoveAvatarListDic(CSAvatar avatar)
    {
        int avatarType = avatar.AvatarType;
        if (mAvatarListDic.ContainsKey(avatarType))
        {
            mAvatarListDic[avatarType].Remove(avatar);
        }
    }

    private void AddSelfPet(CSAvatar avatar)
    {
        if (avatar.AvatarType == EAvatarType.Pet)
        {
            if (avatar.BaseInfo != null)
            {
                CSPetInfo petInfo = (CSPetInfo)avatar.BaseInfo;
                if (petInfo.MasterID == MainPlayer.ID)
                {
                    if(!mSelfPetList.Contains(avatar))
                    {
                        mSelfPetList.Add(avatar);
                    }
                }
            }
        }
    }

    private void RemoveSelfPet(CSAvatar avatar)
    {
        if(mSelfPetList.Contains(avatar))
        {
            mSelfPetList.Remove(avatar);
        }
    }

    public int GetPetCount()
    {
        return mSelfPetList.Count;
    }

    /// <summary>
    /// 由于加载的延迟，等待中或者加载完后如果服务器的数据已经删除了，
    /// </summary>
    /// <param name="id"></param>
    public void CheckRemoveAvater(long id)
    {
        CSAvatar avatar = GetAvatar(id);
        if (avatar != null)
        {
            RemoveAvatar(id);
        }
    }

    /// <summary>
    /// 检查移除已存在但无效的avatar
    /// 由于加载的延迟，等待中或者加载完后如果服务器的数据已经删除了
    /// </summary>
    /// <param name="avatar"></param>
    /// <returns>true:移除无效的avatar</returns>
    public bool IsCheckRemoveAvater(CSAvatar avatar)
    {
        if (avatar == null || avatar.BaseInfo == null) return false;
        if (GetAvatar(avatar.ID) == null)
        {
            RemoveAvatar(avatar);
            return true;
        }
        return false;
    }

    public bool IsMainPlayer(long id)
    {
        return (MainPlayer != null && MainPlayer.ID == id);
    }

    /// <summary>
    /// 添加进入视野的avatar
    /// </summary>
    /// <param name="avatar"></param>
    public void AddAvatarInView(CSAvatar avatar)
    {
        int type = avatar.AvatarType;

        if (mAvatarInViewDic.ContainsKey(type))
        {
            long avatarId = avatar.ID;

            if (!mAvatarInViewDic[type].ContainsKey(avatarId))
            {
                mAvatarInViewDic[type].Add(avatar.ID,true);

                avatar.isInView = true;

                mAllAvatarNumInView++;
                if (avatar.AvatarType == EAvatarType.NPC)
                {
                    CSNpc npc = avatar as CSNpc;
                    CSNpcTaskEffectMgr.Instance.AddNpc(npc);
                }
            }
        }
    }

    /// <summary>
    /// 移除离开视野的Avatar
    /// </summary>
    /// <param name="avatar"></param>
    public void RemoveAvatarInView(CSAvatar avatar)
    {
        RemoveViewAvatar(avatar);
        CancleSelectTarget(avatar.ID);
    }

    private void RemoveViewAvatar(CSAvatar avatar)
    {
        int type = avatar.GetAvatarTypeInt();
        if (mAvatarInViewDic.ContainsKey(type))
        {
            if (mAvatarInViewDic[type].ContainsKey(avatar.ID))
            {
                if (avatar.AvatarType == EAvatarType.NPC)
                {
                    CSNpc npc = avatar as CSNpc;
                    CSNpcTaskEffectMgr.Instance.RemoveNpc(npc);
                }
                mAvatarInViewDic[type].Remove(avatar.ID);
                avatar.isInView = false;
                mAllAvatarNumInView--;
            }
        }
    }


    public bool IsAvatarInViewDic(CSAvatar avatar)
    {
        int type = avatar.GetAvatarTypeInt();
        return mAvatarInViewDic.ContainsKey(type) && mAvatarInViewDic[type].ContainsKey(avatar.ID);
    }

    /// <summary>
    /// 是否达到玩家上限限制
    /// </summary>
    public bool IsPlayerMaxShowLimit
    {
        get
        {
            return (mAllAvatarNumInView >= CSResourceManager.Singleton.maxPlayerNum);
        }
    }

    public static int GetLowPriorityViewNum(CSAvatar avatar)
    {
        int num = 0;
        return num;
    }

    public int GetCount(int avatarType)
    {
        List<CSAvatar> list = GetAvatarList(avatarType);    
        if(list == null)
        {
            return 0;
        }
        return list.Count;
    }

    public bool IsHaveAvatar(int avatarType)
    {
        bool ret = GetCount(avatarType) > 0;
        return ret;
    }

    /// <summary>
    /// 是否需要删除视野内avatar
    /// 当达到玩家显示数量上限的时候需删除
    /// </summary>
    /// <param name="avatar"></param>
    /// <returns></returns>
    public static bool IsNeedRemoveViewNum(CSAvatar avatar)
    {
        int type = avatar.GetAvatarTypeInt();
        if (mAvatarInViewDic.ContainsKey(type))
        {
            return mAvatarInViewDic[type].Count > CSResourceManager.Singleton.maxPlayerNum;
        }
        return false;
    }

    public List<CSAvatar> GetAvatarList(int type)
    {
        if (mAvatarListDic.ContainsKey(type))
        {
            return mAvatarListDic[type];
        }
        return null;
    }

    public ILBetterList<CSAvatar> GetPetList(long masterId)
    {
        List<CSAvatar> list = GetAvatarList(EAvatarType.Pet);
        if(list == null)
        {
            return null;
        }
        mCacheList.Clear();
        for (int i = 0; i < list.Count; ++i)
        {
            CSPetInfo petInfo = list[i].BaseInfo as CSPetInfo;
            if(petInfo != null && petInfo.MasterID == masterId)
            {
                mCacheList.Add(list[i]);
            }
        }
        return mCacheList;
    }

    /// <summary>
    /// 获取非自动战斗最近的可攻击目标(包括玩家和宠物)
    /// </summary>
    /// <param name="monsterConfigID">大与0表示选中指定confgigId的avatar作为攻击目标</param>
    /// <param name="exclusionEnemyID">需过滤攻击目标的id</param>
    /// <param name="range">可攻击范围</param>
    /// <returns></returns>
    public CSAvatar GetNearestAttackTarget(int monsterConfigID = 0, long exclusionEnemyID = 0, int quality = 0,float range = float.MaxValue)
    {
        CSAvatar avatar = GetAttackTargetByType(EAvatarType.Monster, monsterConfigID, exclusionEnemyID, quality,range);
        if (avatar == null)
        {
            avatar = GetAttackTargetByType(EAvatarType.Player, monsterConfigID, exclusionEnemyID, quality, range);
            CSAvatar pet = GetAttackTargetByType(EAvatarType.Pet, monsterConfigID, exclusionEnemyID, quality, mDistance);
            mDistance = 0;
            if(pet != null)
            {
                return pet;
            }
        }
        return avatar;
    }

    /// <summary>
    /// 获取自动战斗最近的可攻击目标
    /// </summary>
    /// <param name="monsterConfigID">大与0表示选中指定confgigId的avatar作为攻击目标</param>
    /// <param name="exclusionEnemyID">需过滤攻击目标的id</param>
    /// <param name="range">可攻击范围</param>
    /// <returns></returns>
    public CSAvatar GetAutoFightNearestAttackTarget(int monsterConfigID = 0, long exclusionEnemyID = 0, int quality = 0, float range = float.MaxValue)
    {
        CSAvatar avatar = GetAttackTargetByType(EAvatarType.Monster, monsterConfigID, exclusionEnemyID, quality, range);
        return avatar;
    }

    /// <summary>
    /// 通过AvatarType获取最近的可攻击目标
    /// </summary>
    /// <param name="avatarType"></param>
    /// <param name="monsterConfigID">大与0表示选中指定confgigId的avatar作为攻击目标</param>
    /// <param name="exclusionEnemyID">需过滤攻击目标的id</param>
    /// <param name="quality">大与0表示选中目标的quality大与指定的quality</param>
    /// <param name="range">可攻击范围</param>
    /// <returns></returns>
    public CSAvatar GetAttackTargetByType(int avatarType, int monsterConfigID = 0, long exclusionEnemyID = 0, int quality = 0,float range = float.MaxValue)
    {
        List<CSAvatar> avatarList = GetAvatarList(avatarType);
        if (avatarList == null)
        {
            return null;
        }
        CSAvatar targetAvatar = null;
        float distance = range;
        CSMainPlayerInfo mainPlayerInfo = null;
        if(IsFriendType(avatarType))
        {
            mainPlayerInfo = CSMainPlayerInfo.Instance;
        }

        for (int i = 0; i < avatarList.Count; ++i)
        {
            CSAvatar avatar = avatarList[i];
            if (avatar != null)
            {
                CSAvatarInfo avatarInfo = avatar.BaseInfo;
                if (avatar.ID == exclusionEnemyID)
                {
                    continue;
                }
                if (IsFriendType(avatarType))
                {
                    if (!IsPlayerCanBeSelect(avatarInfo, mainPlayerInfo))
                    {
                        continue;
                    }
                }

                if (avatar.IsCanBeSelectAttack())
                {
                    float dis = Vector2.Distance(MainPlayer.ServerCell.LocalPosition2, avatar.ServerCell.LocalPosition2);
                    dis = Mathf.Abs(dis);
                    if ((dis < distance) && (monsterConfigID == 0 ||
                        avatarInfo.ConfigId == monsterConfigID) && (quality == 0 || avatarInfo.Quality >= quality ))
                    {
                        distance = dis;
                        mDistance = dis;
                        targetAvatar = avatar;
                    }
                }
            }
        }
        return targetAvatar;
    }

    /// <summary>
    /// 通过AvatarType获取最近的可攻击目标
    /// </summary>
    /// <param name="avatarType"></param>
    /// <param name="monsterConfigID">大与0表示选中指定confgigId的avatar作为攻击目标</param>
    /// <param name="exclusionEnemyID">需过滤攻击目标的id</param>
    /// <param name="quality">大与0表示选中目标的quality大与指定的quality</param>
    /// <param name="range">可攻击范围</param>
    /// <returns></returns>

    private void CancleSelectTarget(long id)
    {
        if(MainPlayer != null)
        {
            MainPlayer.TouchEvent.CancleSelectTarget(id);
        }
    }

    public CSAvatar GetAutoFightTarget(bool isAutofight,int monsterConfigId = 0,int quality = 0)
    {
        if (MainPlayer == null || MainPlayer.TouchEvent == null)
        {
            return null;
        }
        CSAvatar target = GetSelectTarget(monsterConfigId);
        if(target != null)
        {
            if(target.IsDead || target.IsServerDead || !target.IsLoad /*|| target.IsHiding*/)
            {
                target = null;
            }
        }
        if(target != null)
        {
            return target;
        }
        target = (isAutofight) ? GetAutoFightNearestAttackTarget(monsterConfigId) : GetNearestAttackTarget(monsterConfigId);

        if (target == null && monsterConfigId > 0 && isAutofight)
        {
            target = GetAutoFightNearestAttackTarget(0);
        }


        //MainPlayer.TouchEvent.SetTarget(target);
        ////TODO:ddn
        //if(MainPlayer.TouchEvent.Skill != null)
        //{
        //    MainPlayer.TouchEvent.Skill.Target = target;      
        //}


        return target;
    }

    public CSAvatar GetSelectTargetByType(bool isAutoFight,int skillTargetType,int monsterConfigId = 0, int quality = 0)
    {
        if (MainPlayer == null || MainPlayer.TouchEvent == null)
        {
            return null;
        }
        CSAvatar target = null;
        switch (skillTargetType)
        {
            case ESkillTargetType.Self:
                {
                    target = MainPlayer;
                }
                break;
            case ESkillTargetType.Friend:
                {
                    target = MainPlayer.TouchEvent.Target;
                    if((target == null) || (!IsFriendType(target.AvatarType)) || (!IsPlayerCanBeSelect(target.BaseInfo, CSMainPlayerInfo.Instance,true)))
                    {
                        target = MainPlayer;
                    }
                }
                break;
            case ESkillTargetType.Pet:
            case ESkillTargetType.SummonPet:
                {
                    target = MainPlayer.TouchEvent.Target;
                    if(target != null && target.BaseInfo != null && (target.AvatarType == EAvatarType.Pet || target.AvatarType == EAvatarType.ZhanHun))
                    {
                        CSPetInfo petInfo = target.BaseInfo as CSPetInfo;
                        if(petInfo != null && petInfo.MasterID == MainPlayer.ID)
                        {
                            return target;
                        }
                    }
                    target = MainPlayer;
                }
                break;
            default:
                target = GetAutoFightTarget(isAutoFight,monsterConfigId, quality);
                break;
        }
        return target;
    }

    public CSAvatar GetSelectTarget(int monsterConfigId = 0, int quality = 0)
    {
        if (MainPlayer == null || MainPlayer.TouchEvent == null)
        {
            return null;
        }
        CSAvatar target = GetTarget(monsterConfigId,quality);
        return target;
    }

    private CSAvatar GetTarget(int monsterConfigId , int quality)
    {
        CSAvatar target = MainPlayer.TouchEvent.Target;
        if (target != null && !target.IsServerDead && (target.AvatarType != EAvatarType.NPC))
        {
            if ((monsterConfigId == 0) || (target.BaseInfo != null
                && target.BaseInfo.ConfigId == monsterConfigId))
            {
                return target;
            }
        }
        return null;
    }

    /// <summary>
    /// 获取攻击范围内的可攻击目标
    /// </summary>
    /// <param name="attackCoord">攻击者当前坐标</param>
    /// <param name="tblSkill">技能表</param>
    /// <param name="monsterConfigID">大与0表示选中指定confgigId的avatar作为攻击目标</param>
    /// <param name="exclusionEnemyID">需过滤攻击目标的id</param>
    /// <returns></returns>
    public CSAvatar GetAttackTargetInRange(CSMisc.Dot2 attackCoord, TABLE.SKILL tblSkill, int monsterConfigID = 0, CSAvatar exclusionEnemy = null)
    {
        CSAvatar avatar = GetAttackTargetInRangeByType(EAvatarType.Monster, attackCoord,tblSkill, monsterConfigID, exclusionEnemy);
        if(avatar == null)
        {
            avatar = GetAttackTargetInRangeByType(EAvatarType.Player, attackCoord, tblSkill, monsterConfigID, exclusionEnemy);
        }
        if(avatar == null)
        {
            avatar = GetAttackTargetInRangeByType(EAvatarType.Pet, attackCoord, tblSkill, monsterConfigID, exclusionEnemy);
        }
        if(avatar == null)
        {
            avatar = GetAttackTargetInRangeByType(EAvatarType.ZhanHun, attackCoord, tblSkill, monsterConfigID, exclusionEnemy);
        }
        return avatar;
    }

    /// <summary>
    /// 获取攻击范围内的指定类型可攻击目标
    /// </summary>
    /// <param name="avatarType"></param>
    /// <param name="attackCoord"></param>
    /// <param name="tblSkill"></param>
    /// <param name="monsterConfigID"></param>
    /// <param name="exclusionEnemyID"></param>
    /// <returns></returns>
    public CSAvatar GetAttackTargetInRangeByType(int avatarType, CSMisc.Dot2 attackCoord, TABLE.SKILL tblSkill,int monsterConfigID = 0, CSAvatar exclusionEnemy = null)
    {
        List<CSAvatar> avatarList = GetAvatarList(avatarType);
        if (avatarList == null || tblSkill == null)
        {
            return null;
        }
        long exclusionEnemyID = 0;
        if(exclusionEnemy != null)
        {
            mSelectTargetCoord = exclusionEnemy.NewCell.Coord;
            exclusionEnemyID = exclusionEnemy.ID;
        }

        CSMainPlayerInfo mainPlayerInfo = null;

        if(IsFriendType(avatarType))
        {
            mainPlayerInfo = CSMainPlayerInfo.Instance;
        }

        for (int i = 0; i < avatarList.Count; ++i)
        {
            CSAvatar avatar = avatarList[i];
            if (avatar != null)
            {
                if (avatar.ID == exclusionEnemyID)
                {
                    continue;
                }

                if (IsFriendType(avatarType))
                {
                    if(!IsPlayerCanBeSelect(avatar.BaseInfo, mainPlayerInfo))
                    {
                        continue;
                    }
                }

                if (avatar.IsCanBeSelectAttack())
                {
                    if ((monsterConfigID == 0 || avatar.BaseInfo.ConfigId == monsterConfigID) &&
                        CSSkillTools.GetBestLaunchCoord(tblSkill.effectArea,tblSkill.clientRange,tblSkill.effectRange, attackCoord, avatar.NewCell.Coord))
                    {
                        if (exclusionEnemy != null)
                        {
                            if (CSSkillTools.IsCellDistance(mSelectTargetCoord, avatar.NewCell.Coord, tblSkill.effectRange))
                            {
                                return avatar;
                            }
                        }
                        else
                        {
                            return avatar;
                        }
                    }
                }
            }
        }
        return null;
    }

    public void RefreshAvatarBySet(int avatarType, bool isShow = true)
    {
        List<CSAvatar> avatarList = GetAvatarList(avatarType);
        if(avatarList == null)
        {
            return;
        }
        for(int i = 0; i < avatarList.Count; ++i)
        {
            CSAvatar avatar = avatarList[i];
            if (avatar != null && avatar.BaseInfo != null)
            {
                if(avatar.AvatarType == EAvatarType.Player)
                {
                    CSPlayerInfo info = avatar.BaseInfo as CSPlayerInfo;
                    if (info != null)
                    {
                        isShow = !CSConfigInfo.Instance.IsHidePlayer(info.GuildId);
                    }
                }
                avatar.Show(isShow);
                if(!isShow && avatar.head != null)
                {
                    avatar.head.isHideModel = !isShow;
                    avatar.head.Show();
                }
            }
        }
    }

    /// <summary>
    /// 显示隐藏场景中角色的名称
    /// </summary>
    /// <param name="avatarType"></param>
    public void RefreshNameBySet()
    {
        var avatarList = GetAvatarList(EAvatarType.Player);
        RefreshName(avatarList);
        avatarList = GetAvatarList(EAvatarType.Pet);
        RefreshName(avatarList);
        avatarList = GetAvatarList(EAvatarType.Monster);
        RefreshName(avatarList);
        avatarList = GetAvatarList(EAvatarType.ZhanHun);
        RefreshName(avatarList);
        // avatarList = GetAvatarList(EAvatarType.RoleMonster);
        // RefreshName(avatarList);
    }  

    private void RefreshName(List<CSAvatar> avatarList)
    {
        if(avatarList == null)
            return;
        bool isShow = true;
        for(int i = 0; i < avatarList.Count; ++i)
        {
            CSAvatar avatar = avatarList[i];
            if (avatar != null && avatar.BaseInfo != null)
            {
                isShow = !CSConfigInfo.Instance.GetBool(ConfigOption.HideAllName);
                avatar.head.SetHeadActive(isShow);
            }
            
        }
    }

    public void RefreshPlayerAppearance()
    {
        if(MainPlayer != null) MainPlayer.IsReplaceEquip = true;

        List<CSAvatar> avatarList = GetAvatarList(EAvatarType.Player);
        if (avatarList == null)
        {
            return;
        }
        for(int i = 0; i < avatarList.Count; ++i)
        {
            avatarList[i].IsReplaceEquip = true;
        }
    }

    public CSAvatarInfo GetZhanHunInfo()
    {
        CSPet pet = GetZhanHun();
        if(pet != null)
        {
            return pet.BaseInfo;
        }
        return null;
    }

    public CSPet GetZhanHun()
    {
        List<CSAvatar> petList = GetAvatarList(EAvatarType.ZhanHun);
        if (petList == null)
        {
            return null;
        }
        for (int i = 0; i < petList.Count; ++i)
        {
            CSPet pet = petList[i] as CSPet;
            if (pet != null && pet.IsSelfPet)
            {
                return pet;
            }
        }
        return null;
    }

    public bool IsInZhanHunAttackRange(int effectArea,int range, int effectRange, CSMisc.Dot2 targetCoord)
    {
        bool ret = true;
        CSPet zhanHun = GetZhanHun();
        if(zhanHun == null || zhanHun.OldCell == null)
        {
            return false;
        }
        ret = CSSkillTools.GetBestLaunchCoord(effectArea, range, effectRange, zhanHun.OldCell.Coord,targetCoord);
        return ret;
    }

    public bool IsPlayerCanBeSelect(CSAvatarInfo info,CSMainPlayerInfo mainPlayerInfo, bool isFriendSkill = false)
    {
        if(info == null || mainPlayerInfo == null)
        {
            return false;
        }

        if(info.AvatarType == EAvatarType.Pet || info.AvatarType == EAvatarType.ZhanHun)
        {
            CSPetInfo petInfo = info as CSPetInfo;
            if(petInfo == null)
            {
                return false;
            }
            if(petInfo.MasterID == mainPlayerInfo.ID)
            {
                return (isFriendSkill) ? true : false;
            }
            if(!isFriendSkill)
            {
                if(!petInfo.Awaked)
                {
                    return false;
                }
            }
            info = GetAvatarInfo(petInfo.MasterID);
        }

        CSPlayerInfo playerInfo = info as CSPlayerInfo;
        if(playerInfo == null)
        {
            return false;
        }
        if((playerInfo.AvatarType == (EAvatarType.Player) && (playerInfo.Level < CSConfigInfo.Instance.AttackPlayerLevel())))
        {
            return false;
        }
        int mode = (CSMainPlayerInfo.Instance.PkModeMap > 0) ? CSMainPlayerInfo.Instance.PkModeMap : CSMainPlayerInfo.Instance.PkMode;

        bool ret = isFriendSkill ? IsFriendSkillVaildByPKMode(playerInfo, mode, mainPlayerInfo.TeamId, mainPlayerInfo.GuildId) : 
            IsCanAttackByPKMode(playerInfo, mode, mainPlayerInfo.TeamId, mainPlayerInfo.GuildId);

        return ret;
    }

    /// <summary>
    /// 是否可攻击
    /// </summary>
    /// <param name="info">判定对象</param>
    /// <param name="pkMode">pk模式</param>
    /// <param name="teamID"></param>
    /// <param name="gongHuiID"></param>
    /// <param name="camp">阵营</param>
    /// <param name="friendUnionid"></param>
    /// <returns></returns>
    public bool IsCanAttackByPKMode(CSPlayerInfo info, int pkMode, long teamID, long gongHuiID)
    {
        if (pkMode == EPkMode.Peace) return false;
        if (pkMode == EPkMode.All) return true;
        if (pkMode == EPkMode.Team)
        {
            if (teamID != 0 && info.TeamId != 0) return teamID != info.TeamId;
            return true;
        }
        if (pkMode == EPkMode.Union)
        {
            if (gongHuiID != 0 && info.GuildId != 0)
            {
                if (gongHuiID == info.GuildId)
                {
                    return true;
                }
            }

            return true;
        }
        return false;
    }

    public bool IsFriendSkillVaildByPKMode(CSPlayerInfo info, int pkMode, long teamID, long gongHuiID)
    {
        if (pkMode == EPkMode.Peace) return true;
        if (pkMode == EPkMode.All) return true;
        if (pkMode == EPkMode.Red)
        {
            return (!UtilityFight.IsReadOrGreyName(info));
        }

        if (pkMode == EPkMode.Team)
        {
            if (teamID != 0 && info.TeamId != 0)
            {
                return (teamID == info.TeamId);
            }
            return false;
        }
        if (pkMode == EPkMode.Union)
        {
            if (gongHuiID != 0 && info.GuildId != 0)
            {
                return (gongHuiID == info.GuildId);
            }
            return false;
        }
        return false;
    }

    private bool IsFriendType(int avatarType)
    {
        return (avatarType == EAvatarType.Player || avatarType == EAvatarType.Pet || avatarType == EAvatarType.ZhanHun);
    }

    public void RemoveAllAvatar()
    {
        if (mAvatarDic != null)
        {
            var dic = mAvatarDic.GetEnumerator();
            while (dic.MoveNext())
            {
                CSAvatar a = dic.Current.Value;
                if (a != null)
                {
                    RemoveAvatar(a,true);
                }
            }
            mAvatarDic.Clear();
        }
        mAvatarInViewDic.Clear();
        mAvatarListDic.Clear();
        mDistance = 0;
    }

    public override void Destroy()
    {
        base.Destroy();
        RemoveAllAvatar();
    }
}