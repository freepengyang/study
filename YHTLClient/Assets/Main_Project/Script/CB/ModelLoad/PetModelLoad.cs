using System;
using System.Collections;
using Random = UnityEngine.Random;

public class PetModelLoad : ModelLoadBase, IModelLoad
{
    public override void Analyze()
    {
        base.BeginLoad();
        curMotion = this.Action.Motion;
        curDirection = this.Action.Direction;
        LoadBody();
        LoadWeapon();
        base.EndLoad(OnFinishAllCallBack);
    }

    private void LoadBody()
    {
        if(this.BodyId == 0) return;
     
        if(eAvatarType == EAvatarType.ZhanHun)
        {
            base.Load(this.BodyId, curMotion, curDirection, ModelStructure.Body, ResourceType.PlayerAtlas, ResourceAssistType.Player);
        }
        else
        {
            base.Load(this.BodyId, curMotion, curDirection, ModelStructure.Body, ResourceType.MonsterAtlas, ResourceAssistType.Monster);
        }
    }

    private void LoadWeapon()
    {
        if (curMotion == CSMotion.Dead) return;
 
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

    public override void OnFinishAllCallBack(ModelLoadBase b)
    {
        base.OnFinishAllCallBack(b);
    }
}