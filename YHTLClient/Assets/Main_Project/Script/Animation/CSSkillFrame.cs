
//author jiabao
//date 2016.5.11

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSSkillFrame : CSBaseFrame
{
    public override void Start()
    {
        mSprite = gameObject.GetComponent<CSSprite>();
        mMeshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    public override void Initialization()
    {
        mSprite = gameObject.GetComponent<CSSprite>();
    }

    public void Play()
    {
        mActive = true;
    }

    public override void Stop() 
    {
        mActive = false;
        curFrame = 0;
        if (onFinish != null)
        {
            onFinish();
        }
    }

    public override void Pause()
    {
        mActive = false; 
    }

    public override void setLoop(bool bl)
    {
        Loop = bl;
        mActive = true;
        curFrame = 0;
    }

    public override bool getLoop()
    {
        return Loop;
    }

    public override void setMeshRenderer(bool bl)
    {
        if (mMeshRenderer == null)
        {
            mMeshRenderer = gameObject.GetComponent<MeshRenderer>();
        }

        if (mMeshRenderer != null)
        {
            mMeshRenderer.enabled = bl;
        }
    }

    public override void setFirstName()
    {
        if (mCurrentNames.Count == 0)
            RebuildSpriteList();
        if (mCurrentNames.Count > 0)
            mSprite.SpriteName = mCurrentNames[0];
    }

    public override void RefreshNames(CSBetterList<string> list, bool isReset = false) 
    {
        if (list != null && list.Count > 0)
        {
            ResetToBeginning();
            mCurrentNames = list;
            curFrame = 0;
        }
    }

    public override void RefreshNames()
    {
        if (mSprite == null)
            mSprite = gameObject.GetComponent<CSSprite>();

        if (mSprite != null && mSprite.getAtlas != null)
        {
            ResetToBeginning();
            RebuildSpriteList();
            curFrame = 0;
        }
    }

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
            mCurrentNames.Sort(CompareSort);
        }
    }

    int CompareSort(string f, string s)
    {
        return string.Compare(f, s);
    }
    /// <summary>
    /// 重置
    /// </summary>
    public override void ResetToBeginning()
    {
        if (mSprite != null && mCurrentNames.Count > 0)
        {
            curFrame = 0;
        }
    }

    public override void OnDestroy()
    {
        mSprite = null;
        mCurrentNames.Clear();
    }

    public override void UpdateFrame()
    {
        if (mActive && mCurrentNames.Count > 1 && FPS > 0)
        {
            mDelta += RealTime.deltaTime;

            if (FPS <= 0) FPS = 0;

            if (mDelta > 1) mDelta = 0;

            float rate = 1f / FPS;

            if (rate < mDelta)
            {
                curFrame++;

                mDelta = (rate > 0f) ? mDelta - rate : 0f;

                if (curFrame >= mCurrentNames.Count)
                {
                    curFrame = 0;
                    mActive = Loop;

                    if (!Loop)
                    {
                        if (onFinish != null)
                        {
                            onFinish();
                        }
                        setMeshRenderer(false);
                    }
                }

                if (mActive)
                {
                    string name = mCurrentNames[curFrame];

                    if (string.IsNullOrEmpty(name))
                    {
                        setMeshRenderer(false);
                    }
                    else
                    {
                        mSprite.SpriteName = mCurrentNames[curFrame];
                    }
                }
            }
        }
    }
}
