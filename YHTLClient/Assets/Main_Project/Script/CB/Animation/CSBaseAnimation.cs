
//author LiZongFu
//date 2016.5.11

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShadowData
{
    public Transform ShadowUvGoTrans;
    public CSSpriteBase ShadowSprite;
}

public class CSBaseAnimation : MonoBehaviour
{
    public CSSpriteBase Sprite;

    public GameObject Go;

    private Transform mCahcheTrans;

    public UnityEngine.Transform CahcheTrans
    {
        get
        {
            if (mCahcheTrans == null && Go != null)
                mCahcheTrans = Go.transform;
            return mCahcheTrans;
        }
    }

    public Vector2 getSpriteUV()
    {
        return Sprite == null ? Vector2.zero : Sprite.getUV;
    }

    public void SetShaodwShader(ISFSprite sprite, EShareMatType t)
    {
        EShareMatType type = t;
        CSMisc.blackColor.a = type == EShareMatType.Transparent ? 0.3f : 0.5f;
        CSMisc.greyColor.r = CSMisc.greyColor.g = CSMisc.greyColor.b = CSMisc.greyColor.a = 1;
        EShareMatType blackType = type == EShareMatType.Transparent ? EShareMatType.Balck_Transparent : EShareMatType.Balck;
        sprite.SetShader(CSShaderManager.GetShareMaterial(sprite.getAtlas, blackType), CSMisc.blackColor, CSMisc.greyColor);
    }

}