using System;
using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有请求服务器方法
/// </summary>
public partial class Net
{
    /// <summary>
    /// 使用技能请求
    /// </summary>
    /// <param name="skillId"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="targetId"></param>
    /// <param name="time"></param>
    public static void ReqUseSkillMessage(int skillId, long targetId, int x, int y, long time)
    {
        fight.UseSkillRequest req = CSProtoManager.Get<fight.UseSkillRequest>();
        req.skillId = skillId;
        req.x = x;
        req.y = y;
        req.targetId = targetId;
        req.time = time;

        TABLE.SKILL skillItem = null;
        if(SkillTableManager.Instance.TryGetValue(skillId,out skillItem))
        {
            CSSkillInfo.Instance.StartPublicCdTime = Time.time;
            var skillCd = CSSkillInfo.Instance.Get();
            skillCd.skillId = skillId;
            skillCd.startTime = (long)(Time.realtimeSinceStartup * 1000);
            skillCd.cdTime = skillItem.cdTime;
            skillCd.endTime = skillCd.startTime + skillCd.cdTime;
            skillCd.skillItem = skillItem;
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnSkillEnterCD, skillCd);
            //CSSkillInfo.Instance.Put(skillCd);

            if (UtilityFight.IsFlagSkill(skillItem.skillGroup))
            {
                CSMisc.Dot2 dot2 =CSAvatarManager.MainPlayer.OldCell.Coord;
                //req.x = CSSkillFlag.Instance.GetCoordX(x, dot2.x);
                //req.y = CSSkillFlag.Instance.GetCoordY(y, dot2.y);
                req.x = CSSkillFlagManager.Instance.GetCoordX(x, dot2.x);
                req.y = CSSkillFlagManager.Instance.GetCoordY(y, dot2.y);
                req.targetId = 0;
                CSSkillFlagManager.Instance.Reset();
                //CSSkillFlag.Instance.Reset();
               CSAvatarManager.MainPlayer.TouchEvent.SetSkillTargetCoord(req.x, req.y);
                //if (req.x != x || req.y != y)
                //{
                //    req.targetId = 0;
                //   CSAvatarManager.MainPlayer.TouchEvent.SkillTargetCoord = new CSMisc.Dot2(req.x, req.y);
                //}
            }
            else
            {
                if(UtilityFight.IsSkillWorkOnTarget(skillItem.effectArea))
                {
                   CSAvatarManager.MainPlayer.TouchEvent.SkillTargetCoord = CSMisc.Dot2.Zero;
                }
                else
                {
                   CSAvatarManager.MainPlayer.TouchEvent.SetSkillTargetCoord(req.x, req.y);
                }
            }

            //if (skillItem.skillGroup == (int)(ESkillGroup.YeMan))
            {
                //Debug.LogFormat(" <color=#00ffcc>ReqUseSkillMessage: skillId = {0},Time = {1}, targetId={2} , targetCoord=({3},{4}), " +
                //    "MainPlayerID = {5},MainPlayerCoord = ({6},{7}) {8}   x = {9}  y = {10}  reqTargetId = {11}</color>", skillId, time, targetId, req.x, req.y, CSMainPlayerInfo.Instance.ID,
                //   CSAvatarManager.MainPlayer.OldCell.Coord.x,CSAvatarManager.MainPlayer.OldCell.Coord.y, skillItem.name, x, y, req.targetId);
            }
        }
        CSAutoFightManager.Instance.ResetLaunchSkill(skillId);
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqUseSkillMessage, req);
        CSProtoManager.Recycle(req);
    }

    /// <summary>
    /// 技能升级请求
    /// </summary>
    /// <param name="skillId"></param>
    public static void ReqUpgradeSkill(int skillId)
    {
        fight.SkillIdInfo req = CSProtoManager.Get<fight.SkillIdInfo>();
        req.skillId = skillId;
        CSHotNetWork.Instance.SendMsg((int)ECM.CSUpgradeSkillMessage, req);
        CSProtoManager.Recycle(req);
    }

    /// <summary>
    /// 技能快捷键保存请求
    /// </summary>
    /// <param name="cuts"></param>
    public static void ReqSaveSkillShortCutMessage(params long[] cuts)
    {
        var req = CSProtoManager.Get<fight.SaveSkillShortCutRequest>();
        req.skillShortCut.Clear();
        for (int i = 0; i < cuts.Length; ++i)
            req.skillShortCut.Add(cuts[i]);
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqSaveSkillShortCutMessage, req);
        CSProtoManager.Recycle(req);
    }

    /// <summary>
    /// 设置技能自动释放请求
    /// </summary>
    /// <param name="skillId"></param>
    /// <param name="bAuto"></param>
    /// <param name="param">用来判断是否显示技能开关提示的参数。1显示0不显示</param>
    public static void ReqSetSkillAutoStateMessage(int skillId,bool bAuto, int param = 1)
    {
        var req = CSProtoManager.Get<fight.SkillAutoState>();
        req.auto = bAuto ? 2 : 1;
        req.skillId = skillId;
        req.param = param;
        CSHotNetWork.Instance.SendMsg((int)ECM.CSSetSkillAutoStateMessage, req);
        CSProtoManager.Recycle(req);
    }

    public static void ReqSetPkModeMessage(int _mode)
    {
        var req = CSProtoManager.Get<fight.SetPkModeRequest>();
        req.pkMode = _mode;
        FNDebug.Log("请求pk模式改动   " + _mode);
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqSetPkModeMessage, req);
        CSProtoManager.Recycle(req);
    }
}