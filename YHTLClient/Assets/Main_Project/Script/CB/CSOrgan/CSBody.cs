
//-------------------------------------------------------------------------
//CSBody
//Author jiabao
//Time 2015.1.18
//-------------------------------------------------------------------------


using UnityEngine;
using System.Collections;

public class CSBody : CSOrgan
{
    public CSBody(GameObject go, CSOrganData _organDatar) : base(go, _organDatar)
    {
        Structure = ModelStructure.Body;
        Type = ModelBearing.Body;
    }

    public override void Initialization()
    {
        if (this == null) return;
        base.Initialization();
        bool b = IsHasShoadow;
        if (b != IsHasShoadow)
        {
            if (Animation != null && Animation.ShadowData != null && Animation.ShadowData.ShadowSprite != null)
            {
                if (Animation.ShadowData.ShadowSprite.gameObject.activeSelf != IsHasShoadow)
                    Animation.ShadowData.ShadowSprite.gameObject.SetActive(IsHasShoadow);
            }
        }
    }

    public void UpdateShoadow(bool isShow)
    {
        switch (organData.avatarType)
        {
            case EAvatarType.Player:
            case EAvatarType.MainPlayer:
            case EAvatarType.Monster:
                if (Animation != null && Animation.ShadowData != null)
                {
                    CSSpriteBase ShadowSprite = Animation.ShadowData.ShadowSprite;

                    if (ShadowSprite != null && ShadowSprite.gameObject.activeSelf != isShow)
                    {
                        ShadowSprite.gameObject.SetActive(isShow);
                    }
                }
                break;
        }
    }
}