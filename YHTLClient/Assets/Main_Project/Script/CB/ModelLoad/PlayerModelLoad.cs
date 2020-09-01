using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelLoad : ModelLoadBase, IModelLoad
{
    public override void Analyze()
    {
        base.BeginLoad();
        curMotion = this.Action.Motion;
        curDirection = this.Action.Direction;
        LoadBody();
        LoadWeapon();
        LoadWing();
        base.EndLoad(OnFinishAllCallBack);
    }

    public void LoadBody()
    {
        if (this.BodyId <= 0) return;
        
        if (lastBodyModel != this.BodyId)
        {
            if (base.IsCanLoad(ResourceType.PlayerAtlas, this.BodyId))
            {
                base.RemoveAtlasNum(ResourceType.PlayerAtlas, lastBodyModel);
                base.AddAtlasNum(ResourceType.PlayerAtlas, this.BodyId);
                lastBodyModel = this.BodyId;
            }
        }
        // 模型装备
        base.Load(this.BodyId, curMotion, curDirection, ModelStructure.Body, ResourceType.PlayerAtlas, ResourceAssistType.Player);
    }

    public void LoadWeapon()
    {
        if(curMotion == CSMotion.Dead) return;
       
        if(this.WeaponId <= 0) return;

        if (lastWeapon != this.WeaponId)
        {
            if (base.IsCanLoad(ResourceType.WeaponAtlas, this.WeaponId))
            {
                base.RemoveAtlasNum(ResourceType.WeaponAtlas, lastWeapon);
                base.AddAtlasNum(ResourceType.WeaponAtlas, this.WeaponId);
                lastWeapon = this.WeaponId;
                base.Load(this.WeaponId, curMotion, curDirection, ModelStructure.Weapon, ResourceType.WeaponAtlas, ResourceAssistType.Player);
            }
        }
        else
        {
            // 模型装备
            base.Load(this.WeaponId, curMotion, curDirection, ModelStructure.Weapon, ResourceType.WeaponAtlas, ResourceAssistType.Player);
        }
    }

    private void LoadWing()
    {
        if (curMotion == CSMotion.Dead) return;

        if (this.WingId <= 0) return;

        if (lastWing != this.WingId)
        {
            if (base.IsCanLoad(ResourceType.WingAtlas, this.WingId))
            {
                base.RemoveAtlasNum(ResourceType.WingAtlas, lastWing);
                base.AddAtlasNum(ResourceType.WingAtlas, this.WingId);
                lastWing = this.WingId;
                base.Load(this.WingId, curMotion, curDirection, ModelStructure.Wing, ResourceType.WingAtlas, ResourceAssistType.Player);
            }
        }
        else
        {
            // 模型装备
            base.Load(this.WingId, curMotion, curDirection, ModelStructure.Wing, ResourceType.WingAtlas, ResourceAssistType.Player);
        }
    }

    public override void OnFinishAllCallBack(ModelLoadBase b)
    {
        base.OnFinishAllCallBack(b);
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}
