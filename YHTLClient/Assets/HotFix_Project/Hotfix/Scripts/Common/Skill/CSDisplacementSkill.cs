using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSDisplacementSkill 
{
    private CSAvatar mAvatar;
    private CSMisc.Dot2 mTargetDot;
    private CSMoveToTargetPos mMoveToTargetPos = new CSMoveToTargetPos();

    public void Update()
    {
        if(mMoveToTargetPos != null)
        {
            mMoveToTargetPos.Update();
        }
    }

    public virtual void SetQuickMove(int newX, int newY,CSAvatar avatar, int skillId = 0, long mainTargetId = 0)
    {
        //Debug.LogFormat("{0}   {1}  Cooord= ({2},{3})   TargetCooord = ({4}, {5})",
        //    avatar.GetName(), avatar.ID, newX, newY, avatar.OldCell.Coord.x, avatar.OldCell.Coord.y);

        //Debug.LogFormat("<color=#00ff00> ResSkillEffectMessage: {0}  ID={1}  OldCoord=({2},{3}) NewCooord = ({4}, {5})</color> ",
        //    avatar.GetName(), avatar.ID, avatar.OldCell.Coord.x, avatar.OldCell.Coord.y, newX, newY);

        if (newX == 0 || newY == 0)
        {
            return;
        }
        if(avatar.IsServerDead)
        {
            return;
        }
        mAvatar = avatar;
        mTargetDot = new CSMisc.Dot2(newX, newY);
        CSCell cell = CSMesh.Instance.getCell(mTargetDot.x, mTargetDot.y);
        if (cell == null)
        {
            return;
        }

        //mAvatar.MoveState = EMoveState.Controlled;
        mAvatar.actState.IsBeControl = true;
        mAvatar.ResetServerCell(mTargetDot.x, mTargetDot.y);
        if (!mAvatar.IsLoad)
        {
            mAvatar.ResetOldCell(mTargetDot.x, mTargetDot.y);
            //mAvatar.MoveState = EMoveState.initiative;
            mAvatar.actState.IsBeControl = false;
            return;
        }
        mAvatar.MoveTargetStop(true, false);
        mAvatar.Model.SetDirection(cell.LocalPosition2, mAvatar.OldCell);
        mAvatar.SetAction(CSMotion.Run);
        PlayYeManEffect(skillId, mainTargetId);
        BeginMoveTargetPos(cell.WorldPosition3);
    }

    private void PlayYeManEffect(int skillId, long targetId)
    {
        if (mAvatar.AvatarType == EAvatarType.Player && skillId > 0)
        {
            mAvatar.TouchEvent.Skill = mAvatar.SkillEngine.GetSkill(skillId);
            if (mAvatar.TouchEvent.Skill == null)
            {
                mAvatar.TouchEvent.Skill = mAvatar.SkillEngine.AddSkill(skillId);
            }
            CSAvatar avater = CSAvatarManager.Instance.GetAvatar(targetId);
            mAvatar.TouchEvent.SetTarget(avater);
            mAvatar.SkillEngine.LaunchYeMan(mAvatar.TouchEvent.Skill);
        }
    }

    public void StopYeMain()
    {
        if (mAvatar != null && mAvatar.IsBeControl)
        {
            //mAvatar.MoveState = EMoveState.initiative;
            mAvatar.actState.IsBeControl = false;
            mAvatar.MoveTargetStop(true, true);
            if (mMoveToTargetPos != null)
            {
                mMoveToTargetPos.Stop();
            }
            //mAvatar.Model.SetAction(CSMotion.Stand, true);
            //CSAutoFightManager.Instance.Stop();
        }
    }

    public void BeginMoveTargetPos(Vector3 targetPos)
    {
        mMoveToTargetPos.BeginMove(mAvatar, targetPos, OnYeManFinishCallBack);
    }

    private void OnYeManFinishCallBack()
    {
        //mAvatar.ResetOldCell(mTargetDot.x, mTargetDot.y);
        mAvatar.ResetPosition(mTargetDot);
        CSCell cell = CSMesh.Instance.getCell(mTargetDot.x, mTargetDot.y);
        if (cell != null)
        {
            mAvatar.Model.SetDirection(cell.LocalPosition2,mAvatar.OldCell);
        }
        //if (meffect != null) CSScene.Sington.ResSkillEffectMessageTargetImmi(meffect);
        StopYeMain();

        if(mAvatar.AvatarType == EAvatarType.MainPlayer)
        {
            HotManager.Instance.EventHandler.SendEvent(CEvent.UIJoystick_Reset);
            HotManager.Instance.EventHandler.SendEvent(CEvent.MainPlayer_StopTrigger);
            HotManager.Instance.EventHandler.SendEvent(CEvent.Scene_UpdateRoleMove, null);
            CSTerrain.Instance.refreshDisplayMeshCoord(mAvatar.OldCell);
        }
        //Debug.LogFormat("======> OnYeManFinishCallBack after: OldCell = ({0},{1})   NewCell=({2},{3})  " +
        //    "ServerCell = ({4},{5}) Name = {6}", mAvatar.OldCell.Coord.x,
        //    mAvatar.OldCell.Coord.y, mAvatar.NewCell.Coord.x, mAvatar.NewCell.Coord.y, mAvatar.ServerCell.Coord.x, mAvatar.ServerCell.Coord.y, mAvatar.GetName());
    }

    public void Destroy()
    {
        mAvatar = null;
        mMoveToTargetPos.Destroy();
    }
}
