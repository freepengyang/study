using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CSConfigInfo : CSInfo<CSConfigInfo>
{
    public Dictionary<ConfigOption, int> mDefaultDic = new Dictionary<ConfigOption, int>();   //默认设置
    public Dictionary<ConfigOption, int> mDataDic = new Dictionary<ConfigOption, int>();  //当前设置
    public Dictionary<ConfigOption, CEvent> mEventLinkDic = new Dictionary<ConfigOption, CEvent>();
    //private Dictionary<ConfigOption, int> mOldDataDic = new Dictionary<ConfigOption, int>();   //上一次保存后的设置
    CSBetterLisHot<ConfigAutoReleaseSkill> skillList = new CSBetterLisHot<ConfigAutoReleaseSkill>();
    public CSBetterLisHot<ConfigAutoReleaseSkill> SkillList { get { return skillList; } }

    PoolHandleManager mPoolHandle = new PoolHandleManager();


    //用来读取和存储自动释放技能设置的key，对应三个职业，后面连接技能组id
    const string warriorSkillPrefsKey = "AutoReleaseSkillWarrior_";
    const string masterSkillPrefsKey = "AutoReleaseSkillMaster_";
    const string taoistSkillPrefsKey = "AutoReleaseSkillTaoist_";

    //const int SmartHalfMoonSkill = 5;//半月弯刀技能组id

    /// <summary>
    /// 需要在本地存储的自动释放技能组id
    /// </summary>
    readonly Dictionary<int, ConfigOption> AutoSkillSaveOnLocal = new Dictionary<int, ConfigOption>{
        { 5, ConfigOption.SmartHalfMoonMachetes},
        { 12, ConfigOption.AutoReleasMagicShield},
        { 23, ConfigOption.AutoReleasTaoistPet},
        { 24, ConfigOption.AutoReleasInfinityQi},
    };

    List<int> byPickUpLvList = new List<int>();
    List<int> wlPickUpLvList = new List<int>();
    List<int> attackLvList = new List<int>();
    List<int> returnTimeList = new List<int>();
    List<int> transferTimeList = new List<int>();

    /// <summary>
    /// 单体技能组id列表(随当前职业变动，目前只有法师)
    /// </summary>
    public List<int> skillSingleIds = new List<int>();
    public List<string> skillSingleNames = new List<string>();
    /// <summary>
    /// 群体技能组id列表(随当前职业变动，目前只有法师)
    /// </summary>
    public List<int> skillGroupIds = new List<int>();
    public List<string> skillGroupNames = new List<string>();



    //特殊需求变量
    bool hideAllPlayers = false;
    bool hideMyGuildPlayers = false;


    public CSConfigInfo()
    {
        RegEvent(ConfigOption.AutoPlayGuildAudio, 1, CEvent.OnChatSettingChanged);
        RegEvent(ConfigOption.AutoPlayTeamAudio, 1, CEvent.OnChatSettingChanged);
        RegEvent(ConfigOption.AutoPlayPrivateAudio, 1, CEvent.OnChatSettingChanged);
        RegEvent(ConfigOption.AutoPlayFuJinAudio, 1, CEvent.OnChatSettingChanged);
        RegEvent(ConfigOption.AutoPlayColorWorldAudio, 1, CEvent.OnChatSettingChanged);
        RegEvent(ConfigOption.AutoPlayWorldAudio, 0, CEvent.OnChatSettingChanged);
        RegEvent(ConfigOption.AutoPlayGuildText, 1, CEvent.OnChatSettingChanged);
        RegEvent(ConfigOption.AutoPlayPrivateText, 1, CEvent.OnChatSettingChanged);
        RegEvent(ConfigOption.AutoPlayTeamText, 1, CEvent.OnChatSettingChanged);
        RegEvent(ConfigOption.AutoPlayNearbyText, 1, CEvent.OnChatSettingChanged);
        RegEvent(ConfigOption.AutoPlayWorldText, 1, CEvent.OnChatSettingChanged);
        RegEvent(ConfigOption.MakeVipLevelVisible, 1, CEvent.OnChatSettingChanged);
        //声音
        RegEvent(ConfigOption.BgMusic, 1, CEvent.Setting_BgmSwitchChange);
        RegEvent(ConfigOption.BgMusicSlider, 50, CEvent.Setting_BgmValueChange);
        RegEvent(ConfigOption.EffectSound, 1, CEvent.Setting_SoundEffectSwitchChange);
        RegEvent(ConfigOption.EffectSoundSlider, 50, CEvent.Setting_SoundEffectValueChange);
        RegEvent(ConfigOption.VoiceSound, 1, CEvent.Setting_VoiceSwitchChange);
        RegEvent(ConfigOption.VoiceSoundSlider, 50, CEvent.Setting_VoiceValueChange);
        //固定摇杆
        RegEvent(ConfigOption.FixJoystick, 0, CEvent.Setting_FixJoystickChange);
        //消息
        RegEvent(ConfigOption.PushActivity, 1, CEvent.Setting_PushActivityChange);
        RegEvent(ConfigOption.ForbidGuild, 0, CEvent.Setting_ForbidGuildChange);
        RegEvent(ConfigOption.ForbidFriend, 0, CEvent.Setting_ForbidFriendChange);
        RegEvent(ConfigOption.ForbidStranger, 0, CEvent.Setting_ForbidStrangerChange);
        //装备拾取
        RegEvent(ConfigOption.AllEquipPickUp, 1, CEvent.Setting_AllEquipPickUpChange);
        //三个小开关
        RegEvent(ConfigOption.BYEquipPickUpLvSwitch, 1, CEvent.Setting_BYEquipPickUpLvSwitchChange);
        RegEvent(ConfigOption.BYEquipPickUpQualitySwitch, 1, CEvent.Setting_BYEquipPickUpQualitySwitchChange);
        RegEvent(ConfigOption.WLEquipPickUpLvSwitch, 1, CEvent.Setting_WLEquipPickUpLvSwitchChange);

        RegEvent(ConfigOption.BYEquipPickUpLv, 0, CEvent.Setting_BYEquipPickUpLvChange);//值为下拉框子物体下标
        RegEvent(ConfigOption.BYEquipPickUpQuality, 0, CEvent.Setting_BYEquipPickUpQualityChange);//值为下拉框子物体下标
        RegEvent(ConfigOption.WLEquipPickUpLv, 0, CEvent.Setting_WLEquipPickUpLvChange);//值为下拉框子物体下标
        //道具拾取
        RegEvent(ConfigOption.AllItemPickUp, 1, CEvent.Setting_AllItemPickUpChange);
            //道具拾取详细勾选规则已改，在CSConfigInfoExtends上单独处理
            //RegEvent(ConfigOption.MoneyPickUp, 1, CEvent.Setting_MoneyPickUpChange);
            //RegEvent(ConfigOption.ForgingMaterialsPickUp, 1, CEvent.Setting_ForgingMaterialsPickUpChange);
            //RegEvent(ConfigOption.FashionPiecesPickUp, 1, CEvent.Setting_FashionPiecesPickUpChange);
            //RegEvent(ConfigOption.DrugsPickUp, 1, CEvent.Setting_DrugPickUpChange);
            //RegEvent(ConfigOption.OthersPickUp, 1, CEvent.Setting_OtherPickUpChange);
        //自动吃药
        RegEvent(ConfigOption.AutoTakeDrugSwitch, 1, CEvent.Setting_AutoTakeDrugSwitchChange);
        int defaultDrugId = /*30042023*/-1;
        //var ids = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(654));
        //if (ids != null && ids.Length > 0)
        //{
        //    int.TryParse(ids[0], out defaultDrugId);
        //}
        RegEvent(ConfigOption.AutoTakeDrug, defaultDrugId, CEvent.Setting_AutoTakeDrugChange);//药品id
        RegEvent(ConfigOption.AutoTakeDrugHp, 80, CEvent.Setting_AutoTakeDrugHpChange);
        //蓝药
        RegEvent(ConfigOption.AutoTakeMpDrugSwitch, 1, CEvent.Setting_AutoTakeMpDrugSwitchChange);
        int defaultMpDrugId = /*30042023*/-1;
        //ids = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(655));
        //if (ids != null && ids.Length > 0)
        //{
        //    int.TryParse(ids[0], out defaultMpDrugId);
        //}
        RegEvent(ConfigOption.AutoTakeMpDrug, defaultMpDrugId, CEvent.Setting_AutoTakeMpDrugChange);//药品id
        RegEvent(ConfigOption.AutoTakeDrugMp, 80, CEvent.Setting_AutoTakeDrugMpChange);
        //瞬回药
        RegEvent(ConfigOption.AutoInstantDrugSwitch, 1, CEvent.Setting_AutoTakeInstantDrugSwitchChange);
        int defaultInstantDrugId = /*30042023*/-1;
        //var ids = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(1142));
        //if (ids != null && ids.Length > 0)
        //{
        //    int.TryParse(ids[0], out defaultDrugId);
        //}
        RegEvent(ConfigOption.AutoInstantDrug, defaultInstantDrugId, CEvent.Setting_AutoTakeInstantDrugChange);//药品id
        RegEvent(ConfigOption.AutoInstantDrugHp, 60, CEvent.Setting_AutoTakeInstantDrugHpChange);

        //随机传送石
        RegEvent(ConfigOption.AutoRandomDeliverySwitch, 0, CEvent.Setting_AutoRandomDeliverySwitch);
        RegEvent(ConfigOption.AutoRandomDeliveryHp, 40, CEvent.Setting_AutoRandomDeliveryHpChange);
        RegEvent(ConfigOption.AutoRandomDeliveryTime, 0, CEvent.Setting_AutoRandomDeliveryTimeChange);//值为索引

        //回城传送石
        RegEvent(ConfigOption.AutoGoBackSwitch, 0, CEvent.Setting_AutoGoBackSwitch);
        RegEvent(ConfigOption.AutoGoBackHp, 20, CEvent.Setting_AutoGoBackHpChange);
        RegEvent(ConfigOption.AutoGoBackTime, 0, CEvent.Setting_AutoGoBackTimeChange);//值为索引

        //攻击设置
        RegEvent(ConfigOption.AutoAttackPlayerLv, 0, CEvent.Setting_AutoAttackPlayerLvChange);
        //单体技能
        RegEvent(ConfigOption.SkillSingle, 10, CEvent.Setting_SkillSingleChange);
        //群体技能
        RegEvent(ConfigOption.SkillGroup, 13, CEvent.Setting_SkillGroupChange);
        //画面
        RegEvent(ConfigOption.PopGraphicsModeTips, 1, CEvent.Setting_PopGraphicsModeTipsChange);
        RegEvent(ConfigOption.GraphicsMode, 0, CEvent.Setting_GraphicsModeChange);//0默认模式，1流畅，2极速
        //隐藏
        RegEvent(ConfigOption.HideAllPlayers, 0, CEvent.Setting_HideAllPlayersChange);
        hideAllPlayers = false;
        RegEvent(ConfigOption.HideMyGuildPlayers, 0, CEvent.Setting_HideMyGuildPlayersChange);
        hideMyGuildPlayers = false;
        RegEvent(ConfigOption.HideMonsters, 0, CEvent.Setting_HideMonstersChange);
        RegEvent(ConfigOption.HideSkillEffect, 0, CEvent.Setting_HideSkillEffectChange);
        RegEvent(ConfigOption.HideTaoistMonster, 0, CEvent.Setting_HideTaoistMonsterChange);//隐藏道士召唤兽
        RegEvent(ConfigOption.HideWarPet, 0, CEvent.Setting_HideWarPetChange);//隐藏战魂
        RegEvent(ConfigOption.HideAllName, 0, CEvent.Setting_HideAllNameChange);//隐藏全部名称

        //战斗，技能释放
        RegEvent(ConfigOption.SmartHalfMoonMachetes, 1, CEvent.Setting_AutoReleaseSkillChange);//智能半月弯刀
        RegEvent(ConfigOption.AutoReleasMagicShield, 1, CEvent.Setting_AutoReleaseSkillChange);//自动魔法盾
        RegEvent(ConfigOption.AutoReleasTaoistPet, 1, CEvent.Setting_AutoReleaseSkillChange);//自动释放道士宠物
        RegEvent(ConfigOption.AutoReleasInfinityQi, 1, CEvent.Setting_AutoReleaseSkillChange);//自动释放无极真气
        InitAutoReleasSkillConfigs();

        InitPropPickUpSettings();

        InitSundryList();

        //mClientEvent.AddEvent(CEvent.ResPlayerInfoMessage, ResPlayerInfoEvent);
    }


    public void Init()
    {
        InitSingleAndGroupSkillsConfig();
        LoadConfigOnLocal();

        //请求好友消息设置及陌生人消息设置
        Net.CSSettingMessage();

        CSAutoUseItemMgr.Instance.Init();
    }


    void InitSundryList()
    {
        byPickUpLvList = UtilityMainMath.SplitStringToIntList(SundryTableManager.Instance.GetSundryEffect(461));
        wlPickUpLvList = UtilityMainMath.SplitStringToIntList(SundryTableManager.Instance.GetSundryEffect(462));
        attackLvList = UtilityMainMath.SplitStringToIntList(SundryTableManager.Instance.GetSundryEffect(463));
        returnTimeList = UtilityMainMath.SplitStringToIntList(SundryTableManager.Instance.GetSundryEffect(1147));
        transferTimeList = UtilityMainMath.SplitStringToIntList(SundryTableManager.Instance.GetSundryEffect(1146));
    }


    protected void RegEvent(ConfigOption configOption,int defaultValue,CEvent linkedEvent)
    {
        mDefaultDic.Add(configOption, defaultValue);
        mEventLinkDic.Add(configOption, linkedEvent);

        mDataDic.Add(configOption, defaultValue);
    }

    /// <summary>恢复默认设置</summary>
    public void RestoreDefault()
    {
        if (mDefaultDic == null || mDefaultDic.Count < 1) return;

        mDataDic.Clear();
        var cur = mDefaultDic.GetEnumerator();
        while (cur.MoveNext())
        {
            mDataDic.Add(cur.Current.Key, cur.Current.Value);
        }

        //自动释放技能部分
        InitAutoReleasSkillConfigs();
    }

    public bool ContainOption(ConfigOption configOption)
    {
        return mDataDic.ContainsKey(configOption);
    }

    /// <summary>获取int型设置的值</summary>
    public int GetInt(ConfigOption configOption)
    {
        return ContainOption(configOption) ? mDataDic[configOption] : 0;
    }

    /// <summary>获取bool型设置的值</summary>
    public bool GetBool(ConfigOption configOption)
    {
        return GetInt(configOption) == 1;
    }
    
    /// <summary>获取滑动条的值</summary>
    public float GetFloat(ConfigOption configOption)
    {
        float v = mDataDic[configOption]/* / 100f*/;
        return v;
    }

    /// <summary>设置如勾选框一类的bool型值</summary>
    public void SetBool(ConfigOption configOption, bool value)
    {
        if (!ContainOption(configOption))
        {
            return;
        }
        
        var oldValue = mDataDic[configOption];
        var currentValue = value ? 1 : 0;
        if (oldValue != currentValue)
        {
            mDataDic[configOption] = currentValue;
            //保存
            SaveSingleConfigOnLocal(configOption, currentValue);

            if (configOption == ConfigOption.HideAllPlayers) hideAllPlayers = value;
            if (configOption == ConfigOption.HideMyGuildPlayers) hideMyGuildPlayers = value;
            switch (configOption)
            {
                case ConfigOption.ForbidFriend:
                    {
                        Net.CSSettingSocialMessage(value ? 2 : 1);
                    }
                    break;
                case ConfigOption.ForbidStranger:
                    {
                        Net.CSSettingStrangerInfoMessage(value ? 2 : 1);
                    }
                    break;
                case ConfigOption.ForbidGuild:
                    {
                        Net.CSSettingGuildMessage(value ? 2 : 1);
                    }
                    break;
                case ConfigOption.HideAllPlayers:
                case ConfigOption.HideMyGuildPlayers:
                    {
                        CSAvatarManager.Instance.RefreshAvatarBySet(EAvatarType.Player);
                    }
                    break;
                case ConfigOption.HideMonsters:
                    {
                        bool isShow = !CSConfigInfo.Instance.GetBool(ConfigOption.HideMonsters);
                        CSAvatarManager.Instance.RefreshAvatarBySet(EAvatarType.Monster, isShow);
                    }
                    break;
                case ConfigOption.HideTaoistMonster:
                    {
                        bool isShow = !CSConfigInfo.Instance.GetBool(ConfigOption.HideTaoistMonster);
                        CSAvatarManager.Instance.RefreshAvatarBySet(EAvatarType.Pet, isShow);
                    }
                    break;
                case ConfigOption.HideWarPet:
                    {
                        bool isShow = !CSConfigInfo.Instance.GetBool(ConfigOption.HideWarPet);
                        CSAvatarManager.Instance.RefreshAvatarBySet(EAvatarType.ZhanHun, isShow);
                    }
                    break;
                case ConfigOption.PopGraphicsModeTips:
                    {
                        if(GetGraphicsMode() != (int)EShowOptionType.Normal)
                        {
                            CSAvatarManager.Instance.RefreshPlayerAppearance();
                        }
                    }
                    break;
                case ConfigOption.HideAllName:
                    {
                        CSAvatarManager.Instance.RefreshNameBySet();
                    }
                    break;
                
            }
            CEvent eventId = mEventLinkDic[configOption];

            for (var it = AutoSkillSaveOnLocal.GetEnumerator(); it.MoveNext();)//存在本地的自动释放技能发事件单独处理
            {
                if (it.Current.Value == configOption)
                {
                    HotManager.Instance.EventHandler.SendEvent(eventId, it.Current.Key);
                    return;
                }
            }
            HotManager.Instance.EventHandler.SendEvent(eventId,value);
        }
    }

    /// <summary>设置如音量一类的int型值</summary>
    public void SetInt(ConfigOption configOption, int value)
    {
        if (!ContainOption(configOption)) return;

        int oldValue = mDataDic[configOption];
        if (value != oldValue)
        {
            mDataDic[configOption] = value;
            //保存
            SaveSingleConfigOnLocal(configOption, value);
            switch (configOption)
            {
                case ConfigOption.GraphicsMode:
                    {
                        CSAvatarManager.Instance.RefreshPlayerAppearance();
                    }
                    break;
                case ConfigOption.SkillSingle:
                case ConfigOption.SkillGroup:
                    {
                        CSSkillPriorityInfo.Instance.RemoveAutoFightByGroup(oldValue);
                    }
                    break;
                default:
                    break;
            }

            CEvent eventId = mEventLinkDic[configOption];
            HotManager.Instance.EventHandler.SendEvent(eventId, value);
        }
    }

    
    public void SetFloat(ConfigOption configOption, float value, bool sendEvnet = true)
    {
        if (!ContainOption(configOption)) return;
        var currentValue = Mathf.RoundToInt(value * 100);
        int oldValue = mDataDic[configOption];
        if (currentValue != oldValue)
        {
            mDataDic[configOption] = currentValue;
            //保存
            SaveSingleConfigOnLocal(configOption, currentValue);
            if (sendEvnet)
            {
                CEvent eventId = mEventLinkDic[configOption];
                HotManager.Instance.EventHandler.SendEvent(eventId, currentValue);
            }
        }            
    }


    /// <summary>
    /// 获取本元装备自动拾取等级
    /// </summary>
    /// <returns></returns>
    public int GetBenYuanEquipPickUpLevel()
    {
        int index = GetInt(ConfigOption.BYEquipPickUpLv);
        if (index < 0 || byPickUpLvList == null || index >= byPickUpLvList.Count) return 0;
        return byPickUpLvList[index];
    }

    /// <summary>
    /// 获取本元装备自动拾取品质
    /// </summary>
    /// <returns></returns>
    public int GetBenYuanEquipPickUpQuality()
    {
        //int[] datas = { (int)ColorType.White, (int)ColorType.Green, (int)ColorType.Blue, (int)ColorType.Purple, (int)ColorType.Orange };
        int index = GetInt(ConfigOption.BYEquipPickUpQuality);
        return index + 1;
        //if (datas.Length > index)
        //{
        //    return datas[index];
        //}

        ///return datas[0];
    }

    /// <summary>
    /// 获取卧龙装备自动拾取等级
    /// </summary>
    /// <returns></returns>
    public int GetWoLongEquipPickUpLevel()
    {
        int index = GetInt(ConfigOption.WLEquipPickUpLv);
        if (index < 0 ||wlPickUpLvList == null || index >= wlPickUpLvList.Count) return 0;
        return wlPickUpLvList[index];
    }

    /// <summary>
    /// 不选择低于多少等级的玩家为攻击目标
    /// </summary>
    /// <returns></returns>
    public int AttackPlayerLevel()
    {
        int index = GetInt(ConfigOption.AutoAttackPlayerLv);
        if (index < 0 || attackLvList == null || index >= attackLvList.Count) return 0;
        return attackLvList[index];
    }

    /// <summary>
    /// 获取画面模式
    /// </summary>
    /// <returns></returns>
    public int GetGraphicsMode()
    {
        return GetInt(ConfigOption.GraphicsMode);
    }


    /// <summary>
    /// 设置技能是否自动释放。由设置界面勾选时直接调用
    /// </summary>
    /// <param name="info"></param>
    /// <param name="value"></param>
    public void SetAutoReleaseSkill(ConfigAutoReleaseSkill info, bool value)
    {
        if (skillList == null || skillList.Count < 1) return;
        ConfigAutoReleaseSkill data = skillList.FirstOrNull(x => { return x.Career == info.Career && x.SkillGroup == info.SkillGroup; });
        if (data != null)
        {
            bool oldValue = data.AutoRelease;
            data.SetState(value);
            int group = data.SkillGroup;
            if (AutoSkillSaveOnLocal.ContainsKey(group))
            {                
                SetAutoSkillBool(group, value);
            }
            else
            {
                int lv = CSSkillInfo.Instance.GetLearnedSkillLvBySkillGroup(group);
                lv = lv < 1 ? 1 : lv;
                Net.ReqSetSkillAutoStateMessage(group * 1000 + lv, value, 0);
            }
            //if (oldValue != value)
            //{
            //    HotManager.Instance.EventHandler.SendEvent(CEvent.Setting_AutoReleaseSkillChange);
            //}
        }
    }


    public void SetAutoSkillBool(int group, bool value)
    {
        if (!AutoSkillSaveOnLocal.ContainsKey(group)) return;
        SetBool(AutoSkillSaveOnLocal[group], value);
    }


    public bool GetAutoSkillBool(int group)
    {
        if (!AutoSkillSaveOnLocal.ContainsKey(group)) return CSSkillInfo.Instance.GetSkillAutoPlayByGroup(group);

        return GetBool(AutoSkillSaveOnLocal[group]);
    }

    public bool TryGetAutoSkill(int group, out bool ret)
    {
        ret = false;
        ConfigOption configOption;
        if (AutoSkillSaveOnLocal.TryGetValue(group, out configOption))
        {
            ret = GetBool(configOption);
            return true;
        }
        if(skillSingleIds.Contains(group))
        {
            int skillGroup = CSConfigInfo.Instance.GetInt(ConfigOption.SkillSingle);
            ret = (skillGroup == group);
            Debug.LogFormat("group = {0}   skillGroup = {1}  ret = {2}", group,skillGroup, ret);
            return true;
        }
        if(skillGroupIds.Contains(group))
        {
            int skillGroup = CSConfigInfo.Instance.GetInt(ConfigOption.SkillGroup);
            ret = (skillGroup == group);
            return true;
        }
        return false;
    }

    public bool IsHidePlayer(long guild)
    {
        if (!CSScene.IsLanuchMainPlayer)
        {
            return true;
        }
        if (hideAllPlayers)
        {
            return true;
        }
        if(hideMyGuildPlayers)
        {
            return (guild > 0 && CSMainPlayerInfo.Instance.GuildId == guild);
        }
        return false;
    }


    #region OtherFunc
    void InitAutoReleasSkillConfigs()
    {
        int warriorSundryId = 630;
        int masterSundryId = 631;
        int taoistSundryId = 632;

        if (skillList == null) skillList = new CSBetterLisHot<ConfigAutoReleaseSkill>();
        else
        {
            mPoolHandle.RecycleAll();
            skillList.Clear();
        }

        //ECareer career = CSMainPlayerInfo.Instance.CareerType;
        ////Debug.LogError("玩家职业：" + career);
        //int sundrtId = career == ECareer.Master ? masterSundryId : career == ECareer.Taoist ? taoistSundryId : warriorSundryId;
        ParseSkillConfigInSundry(warriorSundryId, ECareer.Warrior);
        ParseSkillConfigInSundry(masterSundryId, ECareer.Master);
        ParseSkillConfigInSundry(taoistSundryId, ECareer.Taoist);

        //if (mEventLinkDic == null) mEventLinkDic = new Dictionary<ConfigOption, CEvent>();
        //mEventLinkDic.Add(ConfigOption.WarriorAutoReleaseSkill, CEvent.Setting_AutoReleaseSkillChange);
        //mEventLinkDic.Add(ConfigOption.MasterAutoReleaseSkill, CEvent.Setting_AutoReleaseSkillChange);
        //mEventLinkDic.Add(ConfigOption.TaoistAutoReleaseSkill, CEvent.Setting_AutoReleaseSkillChange);
    }


    void ParseSkillConfigInSundry(int _sundryId, int career)
    {
        var list = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(_sundryId), '&');
        if (list == null) return;
        for (int i = 0; i < list.Length; i++)
        {
            ConfigAutoReleaseSkill data = mPoolHandle.GetCustomClass<ConfigAutoReleaseSkill>();
            string[] str = list[i].Split('#');
            int id = 0;
            string name = "";
            if (str.Length > 0) int.TryParse(str[0], out id);
            if (str.Length > 1) name = str[1];
            data.Init(career, id, name);
            //data.SetBool(true);//默认勾选
            if (AutoSkillSaveOnLocal.ContainsKey(id))
            {
                data.SetState(GetAutoSkillBool(id));
                data.saveToLocal = true;
            }
            else
            {
                data.SetState(CSSkillInfo.Instance.GetSkillAutoPlayByGroup(id));
                data.saveToLocal = false;
            }
            
            skillList.Add(data);
        }
    }


    void InitSingleAndGroupSkillsConfig()
    {
        if (skillSingleIds == null) skillSingleIds = new List<int>();
        else skillSingleIds.Clear();
        if (skillSingleNames == null) skillSingleNames = new List<string>();
        else skillSingleNames.Clear();

        if (skillGroupIds == null) skillGroupIds = new List<int>();
        else skillGroupIds.Clear();
        if (skillGroupNames == null) skillGroupNames = new List<string>();
        else skillGroupNames.Clear();

        if (CSMainPlayerInfo.Instance.Career == ECareer.Master)
        {
            string single = SundryTableManager.Instance.GetSundryEffect(720);
            var datas = single.Split('&');
            for (int i = 0; i < datas.Length; i++)
            {
                var str = datas[i].Split('#');
                if (str.Length < 2) continue;
                int id = 0;
                if (int.TryParse(str[0], out id))
                {
                    skillSingleIds.Add(id);
                    skillSingleNames.Add(str[1]);
                }
            }

            string group = SundryTableManager.Instance.GetSundryEffect(721);
            datas = group.Split('&');
            for (int i = 0; i < datas.Length; i++)
            {
                var str = datas[i].Split('#');
                if (str.Length < 2) continue;
                int id = 0;
                if (int.TryParse(str[0], out id))
                {
                    skillGroupIds.Add(id);
                    skillGroupNames.Add(str[1]);
                }
            }
        }
    }


    #endregion


    #region Save&Load

    void SaveSingleConfigOnLocal(ConfigOption configOption, int value)
    {
        var id = CSMainPlayerInfo.Instance.ID;
        string key = $"{id}{ configOption.ToString() }";
        PlayerPrefs.SetInt(key, value);
    }


    /// <summary>
    /// 存储设置到本地
    /// </summary>
    void SaveAllConfigOnLocal()
    {
        if (mDataDic == null || mDataDic.Count < 1) return;
        var id = CSMainPlayerInfo.Instance.ID;
        var cur = mDataDic.GetEnumerator();
        while (cur.MoveNext())
        {
            string key = $"{id}{ cur.Current.Key.ToString() }";
            PlayerPrefs.SetInt(key, cur.Current.Value);
        }

        //技能部分
        //if (skillList == null || skillList.Count < 1) return;

        //for (int i = 0; i < skillList.Count; i++)
        //{
        //    ConfigAutoReleaseSkill data = skillList[i];
        //    string key = "";
        //    switch (data.Career)
        //    {
        //        case ECareer.Warrior:
        //            key = $"{id}{warriorSkillPrefsKey}{data.SkillGroup}";
        //            break;
        //        case ECareer.Master:
        //            key = $"{id}{masterSkillPrefsKey}{data.SkillGroup}";
        //            break;
        //        case ECareer.Taoist:
        //            key = $"{id}{taoistSkillPrefsKey}{data.SkillGroup}";
        //            break;
        //    }
        //    if (!string.IsNullOrEmpty(key))
        //    {
        //        PlayerPrefs.SetInt(key, data.AutoRelease ? 1 : 0);
        //    }
        //}
    }

    /// <summary>
    /// 从本地读取设置
    /// </summary>
    void LoadConfigOnLocal()
    {
        if (mDefaultDic == null || mDefaultDic.Count < 1) return;
        if (mDataDic == null) mDataDic = new Dictionary<ConfigOption, int>();

        //mDataDic.Clear();
        var id = CSMainPlayerInfo.Instance.ID;
        //Debug.LogError("@@@玩家id:" + id);
        var cur = mDefaultDic.GetEnumerator();
        while (cur.MoveNext())
        {
            string key = $"{id}{ cur.Current.Key.ToString() }";
            int value = PlayerPrefs.GetInt(key, cur.Current.Value);
            //mDataDic.Add(cur.Current.Key, value);
            mDataDic[cur.Current.Key] = value;
        }

        //拾取部分
        LoadPropPickUpSettingFromLocal();

        //技能部分
        if (skillList == null || skillList.Count < 1) InitAutoReleasSkillConfigs();
        else
        {
            for (int i = 0; i < skillList.Count; i++)
            {
                ConfigAutoReleaseSkill data = skillList[i];
                if (AutoSkillSaveOnLocal.ContainsKey(data.SkillGroup))
                {
                    int value = mDataDic[AutoSkillSaveOnLocal[data.SkillGroup]];
                    data.SetState(value == 1);
                    continue;
                }
                data.SetState(CSSkillInfo.Instance.GetSkillAutoPlayByGroup(data.SkillGroup));
            }
        }
        //for (int i = 0; i < skillList.Count; i++)
        //{
        //    ConfigAutoReleaseSkill data = skillList[i];
        //    string key = "";
        //    switch (data.Career)
        //    {
        //        case ECareer.Warrior:
        //            key = $"{id}{warriorSkillPrefsKey}{data.SkillGroup}";
        //            break;
        //        case ECareer.Master:
        //            key = $"{id}{masterSkillPrefsKey}{data.SkillGroup}";
        //            break;
        //        case ECareer.Taoist:
        //            key = $"{id}{taoistSkillPrefsKey}{data.SkillGroup}";
        //            break;
        //    }
        //    if (!string.IsNullOrEmpty(key))
        //    {
        //        int value = PlayerPrefs.GetInt(key, data.AutoRelease ? 1 : 0);
        //        data.SetBool(value != 0);
        //    }
        //}
    }


    public void SaveConfig()
    {
        SaveAllConfigOnLocal();
    }
    #endregion



    #region Events

    void ResPlayerInfoEvent(uint id, object data)
    {
        Init();
    }


    #endregion



    public override void Dispose()
    {
        //SaveConfig();

        //CSMainPlayerInfo.Instance.EventHandler.RemoveEvent(CEvent.HP_Change, HpDrugEvent);
        //CSMainPlayerInfo.Instance.EventHandler.RemoveEvent(CEvent.MP_Change, MpDrugEvent);

        mPoolHandle?.OnDestroy();
        mPoolHandle = null;

        mDefaultDic.Clear();
        mDataDic.Clear();
        mEventLinkDic.Clear();

        skillList?.Clear();
        skillList = null;

        byPickUpLvList?.Clear();
        wlPickUpLvList?.Clear();
        attackLvList?.Clear();
        returnTimeList?.Clear();
        transferTimeList?.Clear();
    }
}


public class ConfigAutoReleaseSkill : IDispose
{
    int career;
    /// <summary> 对应职业 </summary>
    public int Career { get { return career; } }

    int skillGroup;
    /// <summary> 技能组id </summary>
    public int SkillGroup { get { return skillGroup; } }

    bool autoReleas;
    /// <summary> 是否自动释放 </summary>
    public bool AutoRelease
    {
        get
        {
            if (saveToLocal) return autoReleas;
            else return CSSkillInfo.Instance.GetSkillAutoPlayByGroup(SkillGroup);
        }
    }

    /// <summary> 用来在设置界面中显示的文字 </summary>
    public string showName;

    /// <summary> 是否单独处理，保存到本地。暂时只有战士的只能圆月弯刀是这样的特殊处理，其他开关均和技能面板一致 </summary>
    public bool saveToLocal;


    public void Dispose() { }


    public void Init(int _career, int _skillGroup, string _showName)
    {
        career = _career;
        skillGroup = _skillGroup;
        showName = _showName;
        autoReleas = false;
    }


    public void SetState(bool value)
    {
        autoReleas = value;
    }

}