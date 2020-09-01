using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using TABLE;
using UnityEngine;

public partial class CSNetFight : CSNetBase
{
    public void ECM_ResSkillEffectMessage(NetInfo obj)
    {
        fight.SkillEffect rsp = Network.Deserialize<fight.SkillEffect>(obj);
        TABLE.SKILL tblSkill = null;
        if (!SkillTableManager.Instance.TryGetValue(rsp.skillId, out tblSkill))
        {
            return;
        }

        if (tblSkill.skillGroup == (int)ESkillGroup.ZhanHunLieHuo)
        {
            return;
        }

        //CSAvatar attackerTT = CSAvatarManager.Instance.GetAvatar(rsp.performer);
        //if (attackerTT == null)
        //{
        //    return;
        //}
        //if (attackerTT.AvatarType == EAvatarType.MainPlayer)
        //{
        //    //if (rsp.doubleHit > 0)
        //    {
        ////        Debug.LogFormat("ResSkillEffectMessage: skillId ={0}  mainTargetId = {1}   performer={2} targetCount = {3}  missedIdsCount = {4} {5}   isCriticalIds = {6}  Time.time = {7}  {8}  doubleHit = {9}"
        ////, rsp.skillId, rsp.mainTargetId, rsp.performer, rsp.targets.Count, rsp.missedIds.Count, tblSkill.name, rsp.isCriticalIds.Count, Time.time, tblSkill.name, rsp.doubleHit);

        //    }

        //}
        if(CSAvatarManager.Instance == null) return;

        CSAvatar attacker = CSAvatarManager.Instance.GetAvatar(rsp.performer);

        for (int i = 0; i < rsp.targets.Count; ++i)
        {
            fight.EffectedTarget effect = rsp.targets[i];
            CSAvatar avatar = CSAvatarManager.Instance.GetAvatar(effect.id);
            if(avatar == null)
            {
                continue;
            }
            if (avatar != null && avatar.BaseInfo != null /*&& avatar.AvatarType != EAvatarType.MainPlayer*/)
            {
                //DeltaHP 在 hp之前
                avatar.BaseInfo.DeltaHP = effect.deltaHP;
                if(effect.hp != null)
                {
                    avatar.BaseInfo.RealHP = effect.hp.value;
                    avatar.IsServerDead = (effect.hp.value <= 0);
                }
                if ((effect.newX > 0 && effect.newY > 0) && 
                    (avatar.NewCell.Coord.x != effect.newX || avatar.NewCell.Coord.y != effect.newY))
                {
                    avatar.DisplacementSkill.SetQuickMove(effect.newX, effect.newY, avatar);
                }
                if((tblSkill.skillGroup == (int)ESkillGroup.LingHunHuoFu) && (tblSkill.level >= 20))
                {
                    if (attacker != null && attacker.SkillEngine != null)
                    {
                        attacker.SkillEngine.DoubleHit(rsp.skillId, avatar);
                    }
                }
                if(avatar.AvatarType == EAvatarType.MainPlayer && attacker.AvatarType == EAvatarType.Player && effect.deltaHP < 0)
                {
                    CSDamageInfo.Instance.AddDamage(attacker.ID, -effect.deltaHP);
                }
            }
        }

        if (attacker == null)
        {
            return;
        }
        CSAvatar target = CSAvatarManager.Instance.GetAvatar(rsp.mainTargetId);

        if (attacker.AvatarType == EAvatarType.MainPlayer)
        {
            if (target != null && (target.BaseInfo.RealHP == 0 || target.BaseInfo.HP == 0))
            {
                attacker.TouchEvent.Clear();
            }
            if(attacker.BaseInfo != null)
            {
                attacker.BaseInfo.MP = rsp.mp;
            }
            //CSAutoFightManager.Instance.CheckStop();
        }
        if(tblSkill.skillGroup != (int)ESkillGroup.LieHuo)
        {
            CSHurtManager.Instance.Play(rsp, attacker, target);
        }

        if ((tblSkill.skillGroup) == ESkillGroup.YeMan || tblSkill.clientTargetType == ESkillTargetType.Displacement)
        {
            attacker.DisplacementSkill.SetQuickMove(rsp.newX, rsp.newY, attacker,rsp.skillId, rsp.mainTargetId);
        }
        else
        {
            if (attacker.AvatarType != EAvatarType.MainPlayer)
            {
                CSSkill skill = attacker.TouchEvent.Skill;
                if (skill != null)
                {
                    attacker.TouchEvent.SetTarget(target);
                    skill.Target = target;

                    attacker.TouchEvent.SkillTargetCoord = (UtilityFight.IsSkillWorkOnTarget(tblSkill.effectArea)) ? CSMisc.Dot2.Zero : new CSMisc.Dot2(rsp.x, rsp.y);

                    //if(tblSkill.skillGroup == (int)ESkillGroup.LingHunHuoFu)
                    //{
                    //    attacker.TouchEvent.SkillTargetCoord = new CSMisc.Dot2(rsp.x, rsp.y);
                    //}
                    if(UtilityFight.IsFlagSkill(tblSkill.skillGroup))
                    {
                        CSCell cell = attacker.OldCell;
                        if (cell != null)
                        {
                            attacker.TouchEvent.SkillTargetCoord = new CSMisc.Dot2(rsp.x, rsp.y);
                            attacker.TowardsTargetAttack(attacker.OldCell.Coord);
                        }
                    }
                    else if (target != null && target.ServerCell != null)
                    {
                        attacker.TowardsTargetAttack(attacker.ServerCell.Coord);
                    }
                }
            }
        }

        if (rsp.doubleHit > 0)
        {
            if (attacker.SkillEngine != null)
            {
                attacker.SkillEngine.DoubleHit(rsp.skillId, target);
            }
        }
    }

    public void ECM_SCUpgradeSkillMessage(NetInfo obj)
    {
        fight.SCUpgradeSkillInfo rsp = Network.Deserialize<fight.SCUpgradeSkillInfo>(obj);
        if (rsp.state != 1)
            return;
        CSSkillInfo.Instance.UpgradeSkill(rsp.oldSkillId, rsp.newSkillId);
        UtilityFight.LaunchUpgradeSkill(rsp.newSkillId);
    }

    public void ECM_SCSkillShortCutMessage(NetInfo obj)
    {
        //返回登录时，还在下发，会报错
        if(!CSScene.IsLanuchMainPlayer) return;
        var msg = Network.Deserialize<fight.SaveSkillShortCutRequest>(obj);
        CSMainPlayerInfo.Instance.GetMyInfo().skillShortCut.Clear();
        CSMainPlayerInfo.Instance.GetMyInfo().skillShortCut.AddRange(msg.skillShortCut);
        CSSkillInfo.Instance.SetSkillShortCuts(msg.skillShortCut);
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnSkillSlotChanged);
    }

    public void ECM_ClearSkillCDNtfMessage(NetInfo obj)
    {
        fight.ClearSkillCDNtf msg = Network.Deserialize<fight.ClearSkillCDNtf>(obj);
        TABLE.SKILL skillItem = null;
        if (SkillTableManager.Instance.TryGetValue(msg.skillId, out skillItem))
        {
            var skillCd = CSSkillInfo.Instance.Get();
            skillCd.skillId = msg.skillId;
            //skillCd.endTime = msg.lastUseTime;
            //long ticks = CSServerTime.Instance.ServerNowsMilli.Ticks;
            //if (ticks < msg.lastUseTime)
            //{
            //    skillCd.endTime = (long)(Time.realtimeSinceStartup * 1000) + (msg.lastUseTime - ticks)/ 10000;
            //}
            //else
            //{
            //    skillCd.endTime = (long)(Time.realtimeSinceStartup * 1000);
            //}
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnSkillCoolDown, skillCd);
            CSSkillInfo.Instance.Put(skillCd);
        }
    }

    public void ECM_SCSetSkillAutoStateMessage(NetInfo obj)
    {
        fight.SkillAutoState msg = Network.Deserialize<fight.SkillAutoState>(obj);
        bool autoPlay = msg.auto == 2;
        if(autoPlay)
            CSSkillInfo.Instance.RemoveSkillFromForbiddenList(msg.skillId,msg.param == 1);
        else
            CSSkillInfo.Instance.AddSkillToForbiddenList(msg.skillId,msg.param == 1);
    }

    public void ECM_ResAddBufferMessage(NetInfo obj)
    {
        fight.BufferChanged rsp = Network.Deserialize<fight.BufferChanged>(obj);
        CSBuffManager.Instance.AddBuff(rsp);
    }
    public void ECM_PkModeChangedNtfMessage(NetInfo obj)
    {
        fight.PkModeChangedNtf rsp = Network.Deserialize<fight.PkModeChangedNtf>(obj);
        CSMainPlayerInfo.Instance.PkMode = rsp.pkMode;
        HotManager.Instance.EventHandler.SendEvent(CEvent.PkModeChangedNtf);
        HotManager.Instance.EventHandler.SendEvent(CEvent.PkModeTips);
    }
	void ECM_ResRemoveBufferMessage(NetInfo info)
	{
		fight.BufferChanged msg = Network.Deserialize<fight.BufferChanged>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forfight.BufferChanged");
			return;
		}
        CSBuffManager.Instance.RemoveBuff(msg);
	}
	void ECM_ResBufferDeltaHPMessage(NetInfo info)
	{
		fight.BufferDeltaHP msg = Network.Deserialize<fight.BufferDeltaHP>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forfight.BufferDeltaHP");
			return;
		}
        CSHurtManager.Instance.ShowHp(msg);
	}
	void ECM_SCAddSkillMessage(NetInfo info)
	{
		fight.SkillIdInfo msg = Network.Deserialize<fight.SkillIdInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forfight.SkillIdInfo");
			return;
		}

        CSSkillInfo.Instance.AddNewSkill(msg.skillId);
        TryExpressNewSkill(msg.skillId);
    }

    void TryExpressNewSkill(int skillId)
    {
        int skillGroup = skillId / 1000;
        int skillLv = skillId % 1000;
        //1 由于升级的时候 服务器也会发送这个消息 因此要判断 是不是等级为1级 如果是1级则是首次加入
        if (skillLv != 1)
        {
            return;
        }

        //2 判断技能组是否需要展示
        if (!CSNewFunctionUnlockManager.Instance.GetSkillGroupNeedExpress(skillGroup))
        {
            return;
        }

        //3判断此技能是否存在
        TABLE.SKILL skillItem = null;
        if (!SkillTableManager.Instance.TryGetValue(skillId, out skillItem))
        {
            return;
        }

        //4判断技能是否符合职业
        if(skillItem.career != 4 && skillItem.career != CSMainPlayerInfo.Instance.Career)
        {
            return;
        }

        int slotKey = CSSkillInfo.Instance.GetSlotKeyBySkill(skillId);
        CSNewFunctionUnlockManager.Instance.TriggerNewSkillGet(skillId, slotKey + 1);
    }

    void ECM_SCRemoveSkillMessage(NetInfo info)
	{
		fight.SkillIdInfo msg = Network.Deserialize<fight.SkillIdInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forfight.SkillIdInfo");
			return;
		}

        CSSkillInfo.Instance.RemoveSkill(msg.skillId);
    }
	void ECM_ResBufferInfoMessage(NetInfo info)
	{
		fight.BufferChanged msg = Network.Deserialize<fight.BufferChanged>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forfight.BufferChanged");
			return;
		}
	}
	void ECM_ResRemoveSkillMessage(NetInfo info)
	{
		fight.RemoveSkillMsg msg = Network.Deserialize<fight.RemoveSkillMsg>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forfight.RemoveSkillMsg");
			return;
		}


	}
	void ECM_BufferRemoveRemindNtfMessage(NetInfo info)
	{
		fight.BufferRemoveRemindNtf msg = Network.Deserialize<fight.BufferRemoveRemindNtf>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forfight.BufferRemoveRemindNtf");
			return;
		}
	}

	void ECM_SCUpdateSkillRefixMessage(NetInfo info)
	{
        user.SkillRefixInfo msg = Network.Deserialize<user.SkillRefixInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forfight.SkillRefixInfo");
			return;
		}

        CSSkillInfo.Instance.ModifyAttachedSkill(msg);
	}
	void ECM_SCPlayerFsmStateMessage(NetInfo info)
	{
		fight.PlayerFsmState msg = Network.Deserialize<fight.PlayerFsmState>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forfight.PlayerFsmState");
			return;
		}
        CSMainPlayerInfo.Instance.PlayerFsmState = (msg.state);
    }
}