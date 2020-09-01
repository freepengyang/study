using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSHeadPet : CSHead
{
    private CSLabel _lb_master_name;
    public CSLabel lb_master_name { get { return _lb_master_name ?? (_lb_master_name = UtilityObj.GetOrAdd<CSLabel>(transform, "root/lb_master_name")); } }
    private CSSprite _sp_awake_flag;
    public CSSprite sp_awake_flag { get { return _sp_awake_flag ?? (_sp_awake_flag = UtilityObj.GetOrAdd<CSSprite>(transform, "root/sp_awake_flag")); } }

    public override void Init(CSAvatar avatar, bool isVisible = true, bool isHideModel = false, bool isHideAllName = false)
    {
        base.Init(avatar, isVisible,isHideModel, isHideAllName);

        if (null != lb_master_name)
        {
            lb_master_name.fontSize = 16;
        }

        CheckActivedPvp();

        var eventHandler = avatar.BaseInfo.EventHandler;
        eventHandler.AddEvent(CEvent.pet_awaked, OnPetAwaked);
        //SetMasterName();
    }

    public override void Destroy()
    {
        var eventHandler = mAvatar.BaseInfo.EventHandler;
        eventHandler.RemoveEvent(CEvent.pet_awaked, OnPetAwaked);
        base.Destroy();
    }

    protected void OnPetAwaked(uint id,object argv)
    {
        CheckActivedPvp();
    }

    protected void CheckActivedPvp()
    {
        if (mAvatar.BaseInfo is CSPetInfo petInfo)
        {
            if (petInfo.Awaked)
                FNDebug.Log("<color=#00ff00>[战魂觉醒]:已经觉醒</color>");
            else
                FNDebug.Log("<color=#00ff00>[战魂觉醒]:未觉醒</color>");
            sp_awake_flag.CustomActive(petInfo.Awaked);
        }
    }

    protected virtual void SetMasterName()
    {
        if (null != lb_master_name)
        {
            if (mAvatar.BaseInfo is CSPetInfo petInfo)
            {
                if (CSAvatarManager.Instance.GetAvatarInfo(petInfo.MasterID) is CSPlayerInfo playerInfo)
                {
                    lb_master_name.text = playerInfo.Name;
                    return;
                }
            }
            lb_master_name.text = string.Empty;
        }
    }

    protected override void SetName()
    {
        if (null == lb_actorName)
            return;
        
        
        if (Utility.IsInMap(ESpecialMap.DiXiaXunBao))
        {
            lb_actorName.text = ClientTipsTableManager.Instance.GetClientTipsContext(1712);
            return;
        }
        
        if (mAvatar.AvatarType == EAvatarType.Pet || mAvatar.AvatarType == EAvatarType.ZhanHun)
        {
            string playName = "";
            
            if (mAvatar.BaseInfo is CSPetInfo petInfo)
            {
                if (CSAvatarManager.Instance.GetAvatarInfo(petInfo.MasterID) is CSPlayerInfo playerInfo)
                {
                    playName = playerInfo.Name;
                }
            }

            if (playName != string.Empty)
            {
                lb_actorName.text = $"{mAvatar.GetName()}({playName})";
            }
            else
            {
                lb_actorName.text = mAvatar.GetName();
            }

        }
        else
        {
            lb_actorName.text = mAvatar.GetName();
        }
    }

}
