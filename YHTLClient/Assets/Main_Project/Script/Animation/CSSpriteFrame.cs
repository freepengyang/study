﻿
//author jiabao
//date 2016.5.11

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSSpriteFrame : SFSpriteFrame
{
    public override void RebuildSpriteList()
    {
        mCurrentNames.Clear();

        if (mSprite != null && mSprite.getAtlas != null)
        {
            CSSprite s = mSprite as CSSprite;
            List<UISpriteData> sprites = s.Atlas.spriteList;

            for (int i = 0, imax = sprites.Count; i < imax; ++i)
            {
                UISpriteData sprite = sprites[i];
                if (sprite != null)
                {
                    mCurrentNames.Add(sprite.name);
                }
            }
            //mCurrentNames.Sort(CompareSort);
        }
    }
}
