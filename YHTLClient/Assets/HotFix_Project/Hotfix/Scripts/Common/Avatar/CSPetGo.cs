using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSPetGo : CSAvatarGo
{
    public override void Init(CSAvatar avatar)
    {
        if(avatar == null)
        {
            return;
        }
        base.Init(avatar);
        Transform root = avatar.CacheRootTransform;

        if (root != null)
        {
            CSModelModule module = avatar.ModelModule;

            if (module != null)
            {
                Owner.Model.InitPart(module);
            }

            if (Owner != null)
            {
                Owner.CacheTransform.localPosition = Owner.GetPosition();
                Owner.InitHead();
                if (Owner is CSPet pet)
                    pet.InitShieldEffect(true);
            }
        }
    }

    public override void OnHit(CSAvatar clicker)
    {
        if (Owner == null || Owner.IsDead || Owner.BaseInfo == null)
        {
            return;
        }

        //CSPetInfo petInfo = Owner.BaseInfo as CSPetInfo;
        //if(petInfo == null)
        //{
        //    return;
        //}
        //if(petInfo.MasterID == CSMainPlayerInfo.Instance.ID)
        //{
        //    return;
        //}

        base.OnHit(clicker);
        CSModel clickerModel = clicker.Model;
        if (clickerModel != null && clickerModel.Bottom.Go != null)
        {
            Owner.Model.AttachBottom(clickerModel.Bottom);
            clickerModel.ShowSelectAndHideOtherBottom(ModelStructure.Bottom);
            if (Owner.head != null)
            {
                Owner.head.Show();
            }
        }
       
        UIManager.Instance.CreatePanel<UIMonsterSelectionInfoPanel>(OpenSelectionInfoPanel);
    }

    private void OpenSelectionInfoPanel(UIBase uiBase)
    {
        if (uiBase == null)
        {
            return;
        }
        if(Owner == null || Owner.BaseInfo == null)
        {
            return;
        }
        UIMonsterSelectionInfoPanel uiPanel = uiBase as UIMonsterSelectionInfoPanel;
        if(uiPanel != null)
        {
            uiPanel.ShowSelectData(Owner.BaseInfo);
        }
    }

}
