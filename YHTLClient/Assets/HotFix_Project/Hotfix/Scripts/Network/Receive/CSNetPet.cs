public partial class CSNetPet : CSNetBase
{
    void ECM_SCWoLongPetActiveMessage(NetInfo info)
    {
        pet.WoLongPetInfo msg = Network.Deserialize<pet.WoLongPetInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forpet.WoLongPetInfo");
            return;
        }

        //int suitId = CSPetBasePropInfo.Instance.GetZhanHunSuitId();
        CSPetBasePropInfo.Instance.SetPetInfo(msg);
        CSSkillInfo.Instance.LoadLearnedPetSkill(msg.skills);
        FNDebug.LogFormat("<color=#00ff00>[战魂能量点]:{0} ECM_SCWoLongPetActiveMessage</color>", msg.hejiPoint);
        CSSkillInfo.Instance.InitPetCombinedSkill(msg.hejiPoint, msg.hejiConfigId, msg.roleHejiConfigId);

        //if (suitId == 0 && suitId != msg.id)
        UIManager.Instance.CreatePanel<UIPetActivePanel>();
        HotManager.Instance.EventHandler.SendEvent(CEvent.GetWarPetBaseActive);
    }

    void ECM_SCNotifyWoLongPetStateMessage(NetInfo info)
    {
        pet.WoLongPetState msg = Network.Deserialize<pet.WoLongPetState>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forpet.WoLongPetState");
            return;
        }

        CSPetBasePropInfo.Instance.SetPetStateInfo(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.GetPetStateInfo);
    }

    void ECM_SCWoLongPetInfoMessage(NetInfo info)
    {
        pet.WoLongPetInfo msg = Network.Deserialize<pet.WoLongPetInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forpet.WoLongPetInfo");
            return;
        }

        CSPetBasePropInfo.Instance.SetPetInfo(msg);
        CSSkillInfo.Instance.Initialize(CSMainPlayerInfo.Instance.GetMyInfo());
        CSSkillInfo.Instance.InitializePet(CSMainPlayerInfo.Instance.GetMyInfo());
        CSSkillInfo.Instance.LoadLearnedPetSkill(msg.skills);

        FNDebug.LogFormat("<color=#00ff00>[战魂能量点]:{0} ECM_SCWoLongPetInfoMessage</color>", msg.hejiPoint);
        CSSkillInfo.Instance.InitPetCombinedSkill(msg.hejiPoint, msg.hejiConfigId, msg.roleHejiConfigId);

        HotManager.Instance.EventHandler.SendEvent(CEvent.GetWarPetBaseInfo);
    }

    void ECM_SCPlayerWoLongViewInfoMessage(NetInfo info)
    {
        //FNDebug.LogFormat("<color=#00ff00>收到消息:[人物护盾条]</color>");

        pet.PlayerPetViewInfo msg = Network.Deserialize<pet.PlayerPetViewInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forwolong.PlayerPetViewInfo");
            return;
        }

        if (msg.roleId == CSMainPlayerInfo.Instance.ID)
        {
            CSMainPlayerInfo.Instance.Shield = msg.shieldAttr;
            CSMainPlayerInfo.Instance.MaxShield = msg.maxShield;
            CSMainPlayerInfo.Instance.HasShield = true;
        }

        if(CSAvatarManager.Instance == null)
        {
            return;
        }
        var avatar = CSAvatarManager.Instance.GetAvatar(msg.roleId);
        if (null == avatar)
            return;

        if (!(avatar.head is CSHeadPlayer headPlayer))
        {
            return;
        }

        if (avatar ==CSAvatarManager.MainPlayer)
        {
            CSAvatarInfo avatarInfo = CSAvatarManager.Instance.GetZhanHunInfo();
            if (avatarInfo != null)
            {
                if (avatarInfo is CSPetInfo petInfo)
                {
                    petInfo.HasShield = true;
                    petInfo.Shield = msg.shieldAttr;
                }
            }
        }
        else
        {
            CSAvatarInfo avatarInfoOther = CSAvatarManager.Instance.GetAvatarInfo(msg.roleId);
            if (avatarInfoOther!=null)
            {
                if (avatarInfoOther is CSPlayerInfo playerInfo)
                {
                    playerInfo.HasShield = true;
                    playerInfo.Shield = msg.shieldAttr;
                    playerInfo.MaxShield = msg.maxShield;
                }
            }
        }

        float amount = 1.0f;
        if (msg.maxShield > 0)
            amount = UnityEngine.Mathf.Clamp01(msg.shieldAttr * 1.0f / msg.maxShield);

        //FNDebug.LogFormat("<color=#00ff00>[人物护盾条]: [visible:{0}] [amount:{1}] [shieldAttr:{2}] [maxShield:{3}]</color>",
        //    msg.wolongPetState == 2, amount, msg.shieldAttr, msg.maxShield);
        headPlayer.SetShieldAttr(/*msg.wolongPetState == 2*/true, amount);
    }

    void ECM_SCPetInfoMessage(NetInfo info)
    {
        pet.ResPetInfo msg = Network.Deserialize<pet.ResPetInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forpet.ResPetInfo");
            return;
        }

        CSPetLevelUpInfo.Instance.resPetInfo = msg;
    }

    void ECM_SCItemCallBackInfoMessage(NetInfo info)
    {
        pet.ResItemCallBackInfo msg = Network.Deserialize<pet.ResItemCallBackInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forpet.ResItemCallBackInfo");
            return;
        }
        CSPetLevelUpInfo.Instance.SetResItemCallBackInfo(msg);
    }

    void ECM_SCPetTianFuInfoMessage(NetInfo info)
    {
        pet.PetTianFuInfo msg = Network.Deserialize<pet.PetTianFuInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forpet.UnlockPetTianFu");
            return;
        }

        CSPetTalentInfo.Instance.SC_TalentInfo(msg);
    }

    void ECM_SCPetSkillUpgradeMessage(NetInfo info)
    {
        pet.ResPetSkillUpgrade msg = Network.Deserialize<pet.ResPetSkillUpgrade>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forpet.ResPetSkillUpgrade");
            return;
        }

        CSSkillInfo.Instance.UpgradePetSkill(msg.oldSkillId, msg.newSkillId);
    }

    void ECM_SCPetHpMessage(NetInfo info)
    {
        pet.PetHpInfo msg = Network.Deserialize<pet.PetHpInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forpet.PetHpInfo");
            return;
        }

        CSPetBasePropInfo.Instance.SetPetHpAndMaxHp(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.GetWarPetBaseHpInfo);
    }

    /// <summary>
    /// 宠物天赋被动技能信息(宠物升阶的时候也会推)
    /// </summary>
    /// <param name="info"></param>
    void ECM_SCPetTianFuPassiveSkillMessage(NetInfo info)
    {
        pet.PetTianFuPassiveSkillList msg = Network.Deserialize<pet.PetTianFuPassiveSkillList>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forpet.PetTianFuPassiveSkillList");
            return;
        }

        CSWarPetRefineInfo.Instance.HandlePetTianFuPassiveSkill(msg);
        CSSkillInfo.Instance.LoadBdjnItems();
        HotManager.Instance.EventHandler.SendEvent(CEvent.PetTianFuPassiveSkillMessage);
    }

    /// <summary>
    /// 生成新技能
    /// </summary>
    /// <param name="info"></param>
    void ECM_SCPetTianFuRandomPassiveSkillMessage(NetInfo info)
    {
        pet.PetTianFuPassiveSkill msg = Network.Deserialize<pet.PetTianFuPassiveSkill>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forpet.PetTianFuPassiveSkill");
            return;
        }

        CSWarPetRefineInfo.Instance.HandlePetTianFuRandomPassiveSkill(msg);
        CSSkillInfo.Instance.LoadBdjnItems();
        HotManager.Instance.EventHandler.SendEvent(CEvent.PetTianFuRandomPassiveSkill);
    }

    /// <summary>
    /// 替换新技能成功
    /// </summary>
    /// <param name="info"></param>
    void ECM_SCPetTianFuChosePassiveSkillMessage(NetInfo info)
    {
        pet.PetTianFuPassiveSkill msg = Network.Deserialize<pet.PetTianFuPassiveSkill>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forpet.PetTianFuPassiveSkill");
            return;
        }

        CSWarPetRefineInfo.Instance.HandlePetTianFuChosePassiveSkill(msg);
        CSSkillInfo.Instance.LoadBdjnItems();
        HotManager.Instance.EventHandler.SendEvent(CEvent.PetTianFuChosePassiveSkill);
    }

    void ECM_SCPetTianFuChangeMessage(NetInfo info)
    {
        pet.PetTianFuChange msg = Network.Deserialize<pet.PetTianFuChange>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forpet.PetTianFuChange");
            return;
        }

        CSPetTalentInfo.Instance.SC_TalentUpdate(msg.tianfuInfo, true);
        for (int i = 0, max = msg.sid.Count; i < max; ++i)
        {
            FNDebug.LogFormat("<color=#00ff00>[PetTianFuChange]:[sid:{0}]</color>", msg.sid[i]);
            CSSkillInfo.Instance.AddNewPetSkill(msg.sid[i]);
        }
    }
	void ECM_SCPetHejiPointMessage(NetInfo info)
	{
		pet.PetHejiPointChange msg = Network.Deserialize<pet.PetHejiPointChange>(info);
		if(null == msg)
		{
			FNDebug.LogError("Deserialize Msg Failed Forpet.PetHejiPointChange");
			return;
		}

        FNDebug.LogFormat("<color=#00ff00>[战魂能量点]:{0} ECM_SCPetHejiPointMessage</color>", msg.point);
        CSSkillInfo.Instance.SetCombinedEnergyPoints(msg.point);
    }
	void ECM_SCPetActivePvpMessage(NetInfo info)
	{
		pet.ActivePvpInfo msg = Network.Deserialize<pet.ActivePvpInfo>(info);
		if(null == msg)
		{
			FNDebug.LogError("Deserialize Msg Failed Forpet.ActivePvpInfo");
			return;
		}

        if (CSAvatarManager.Instance == null)
        {
            return;
        }

        if(CSAvatarManager.Instance.GetAvatarInfo(msg.petId) is CSPetInfo petInfo)
        {
            petInfo.Awaked = msg.activePvp;
        }
    }
	void ECM_SCCallBackSettingMessage(NetInfo info)
	{
        //FNDebug.Log("1111111111111111111111");
		pet.CallBackSetting msg = Network.Deserialize<pet.CallBackSetting>(info);
		if(null == msg)
		{
			FNDebug.LogError("Deserialize Msg Failed Forpet.CallBackSetting");
			return;
		}

        CSPetLevelUpInfo.Instance.OnChangeSet(msg);

    }
}