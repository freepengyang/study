using fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSBuffManager : Singleton<CSBuffManager>
{
    public const int EFFECT_COLOR_MAX = 10;

    public void AddBuff(BufferChanged info)
    {
        if (info == null)
        {
            FNDebug.Log("======> AddBuff: info is null");
            return;
        }
        if (CSAvatarManager.Instance == null)
        {
            FNDebug.LogError("======> AddBuff: CSAvatarManager is null");
            return;
        }
        //Debug.LogErrorFormat("======> AddBuff: {0}", info.id);
        CSAvatar avatar = CSAvatarManager.Instance.GetAvatar(info.id);
        if (avatar != null && avatar.BaseInfo != null)
        {
            //TABLE.BUFFER tblBuff = null;
            //if (BufferTableManager.Instance.TryGetValue(info.buffer.bufferId, out tblBuff))
            //{
            //    FNDebug.LogFormat("<color=#00ff00>AddBuff: {0}  {1}  {2}  {3} Time.time = {4}  {5}</color>", avatar.BaseInfo.Name, avatar.BaseInfo.ID, info.buffer.bufferId, tblBuff.name, Time.time, avatar.AvatarType.ToString());
            //}
            avatar.BaseInfo.BuffInfo.UpdateBuff(info.buffer);
            AddBuff(avatar, info.id, info.buffer.bufferId, info);
        }
    }

    public void AddBuff(CSAvatar avatar, long id, int buffConfigId, BufferChanged info = null)
    {
        if (IsRefreshBuffEffectState(avatar))
        {
            avatar.BaseInfo.BuffInfo.UpdateState(avatar.actState);
        }
        RefreshHidingBuff(avatar);
        if (avatar.AvatarType == EAvatarType.MainPlayer && info != null)
        {
            HotManager.Instance.EventHandler.SendEvent(CEvent.Buff_Add, info);
        }

        if (CSConfigInfo.Instance.GetBool(ConfigOption.HideSkillEffect))
        {
            return;
        }

        TABLE.BUFFER tblBuff = null;
        if (BufferTableManager.Instance.TryGetValue(buffConfigId, out tblBuff))
        {
            if (tblBuff.effectId > EFFECT_COLOR_MAX)
            {
                avatar.AddBuffEffect(tblBuff.effectId);
            }
        }
    }

    public void RemoveBuff(BufferChanged info)
    {
        if(info == null)
        {
            FNDebug.Log("======> RemoveBuff: info is null");
            return;
        }
        if(CSAvatarManager.Instance == null)
        {
            FNDebug.LogError("======> RemoveBuff: CSAvatarManager is null");
            return;
        }
        CSAvatar avatar = CSAvatarManager.Instance.GetAvatar(info.id);
        if (avatar != null && avatar.BaseInfo != null)
        {
            int buffId = info.buffer.bufferId;
            //TABLE.BUFFER tblBuff = null;
            //if (BufferTableManager.Instance.TryGetValue(info.buffer.bufferId, out tblBuff))
            //{
            //    FNDebug.LogFormat("<color=#ff0000>RemoveBuff: {0}  {1}  {2}  {3}  {4}</color>", avatar.BaseInfo.Name, avatar.BaseInfo.ID, info.buffer.bufferId, tblBuff.name, Time.time);
            //}
            avatar.BaseInfo.BuffInfo.RemoveBuff(buffId);
            if(IsRefreshBuffEffectState(avatar))
            {
                avatar.BaseInfo.BuffInfo.UpdateState(avatar.actState);
            }
            RefreshHidingBuff(avatar);

            if (avatar.AvatarType == EAvatarType.MainPlayer)
            {
                HotManager.Instance.EventHandler.SendEvent(CEvent.Buff_Remove,info);
            }

            TABLE.BUFFER tblBuff = null;
            if(BufferTableManager.Instance.TryGetValue(info.buffer.bufferId, out tblBuff))
            {
                avatar.RemoveBuffEffect(tblBuff.effectId);
            }
        }
    }

    private bool IsRefreshBuffEffectState(CSAvatar avatar)
    {
        if (avatar.AvatarType == EAvatarType.Monster)
        {
            CSMonster monster = avatar as CSMonster;
            if (monster != null)
            {
                return (!monster.IsWall());
            }
        }
        return true;
    }

    private void RefreshHidingBuff(CSAvatar avatar)
    {
        if (CSBuffInfo.avatarState.IsHiding != avatar.actState.IsHiding)
        {
            //if (avatar.AvatarType != EAvatarType.MainPlayer)
            //{
            //    avatar.SetActive(!avatar.actState.IsHiding);
            //}
            avatar.SetShaderName();
        }

        if(CSBuffInfo.avatarState.ColorType != avatar.actState.ColorType)
        {
            avatar.SetShaderName();
        }
    }

    public void Destroy()
    {
    }
}
