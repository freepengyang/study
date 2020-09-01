
//author jiabao
//date 2016.5.11

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CSSkillFrame2 : CSBaseFrame2
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
        }
    }

    public override void Start()
    {
        mSprite = gameObject.GetComponent<CSSprite>();
        mMeshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    public override void Stop() 
    {
        mIsPlaying = false;
        curFrame = 0;
        if (onFinish != null)
        {
            onFinish();
        }
    }

    public override void UpdateFrame()
    {
        if (mIsPlaying && mCurrentNameCount > 1 && FPS > 0)
        {
            mDelta += RealTime.deltaTime;

            if (FPS <= 0) FPS = 0;

            if (mDelta > 1) mDelta = 0;

            float rate = 1f / FPS;

            if (rate < mDelta)
            {
                curFrame++;

                mDelta = (rate > 0f) ? mDelta - rate : 0f;

                if (curFrame >= mCurrentNameCount)
                {
                    curFrame = 0;
                    mIsPlaying = Loop;
                    if (curFrame < mCurrentNames.Count)
                    {
                        mSprite.SpriteName = mCurrentNames[curFrame];
                    }
                    if (!Loop)
                    {
                        if (onFinish != null)
                        {
                            onFinish();
                        }
                    }
                }

                if (mIsPlaying)
                {
                    if (curFrame < mCurrentNames.Count)
                    {
                        mSprite.SpriteName = mCurrentNames[curFrame];
                    }
                }
            }
        }
    }
}
