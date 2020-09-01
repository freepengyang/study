
//-------------------------------------------------------------------------
//动画播放
//Author jiabao
//Time 2015.12.15
//-------------------------------------------------------------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CSEffectAnimation : MonoBehaviour
{
    public int  FPS = 10;
    public bool Loop = false;

    private bool mActive = true;
    private int mIndex = 0;
    private float mDelta = 0f;

    private CSSprite mSprite;

    private List<string> mCurrentNames = new List<string>();

    private MeshRenderer mMeshRenderer;

    public delegate void OnFinish();
    public OnFinish onFinish;

    private int mStopFrameType;
    public int StopFrameType
    {
        get { return mStopFrameType; }
        set { mStopFrameType = value; }
    }

    public void Start() 
    {
        mSprite = gameObject.GetComponent<CSSprite>();
        mMeshRenderer = gameObject.GetComponent<MeshRenderer>();
        RebuildSpriteList();
    }

    public void Play()
    {
        mActive = true;
        mIndex = 0;
    }

    public void setLoop(bool bl) 
    {
        Loop = bl;
        mActive = true;
        mIndex = 0;
    }

    public void RefreshNames()
    {
        if (mSprite == null)
            mSprite = gameObject.GetComponent<CSSprite>();

        if (mSprite != null && mSprite.Atlas != null) 
        {
            ResetToBeginning();
            RebuildSpriteList();
            mIndex = 0;
        }
    }

    public void RebuildSpriteList()
    {
        mCurrentNames.Clear();

        if (mSprite != null && mSprite.Atlas != null)
        {
            List<UISpriteData> sprites = mSprite.Atlas.spriteList;

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
    /// <summary>
    /// 重置
    /// </summary>
    public void ResetToBeginning()
    {
        if (mSprite != null && mCurrentNames.Count > 0)
        {
            mIndex = 0;
        }
    }

    public void OnDestroy()
    {
        mSprite = null;
        mStopFrameType = EActionStopFrameType.None;
        mCurrentNames.Clear();
    }

    public void Update()
    {
        if (mActive && mCurrentNames.Count >=1 && FPS > 0)
        {
            mDelta += RealTime.deltaTime;

            if (FPS <= 0) FPS = 0;

            if (mDelta > 1) mDelta = 0;

            float rate = 1f / FPS;

            if (rate < mDelta)
            {
                mDelta = (rate > 0f) ? mDelta - rate : 0f;

                if (mIndex >= mCurrentNames.Count)
                {
                    mIndex = 0;
                    mActive = Loop;
                    
                    mSprite.SpriteName = mCurrentNames[mIndex];
                    if (!Loop)
                    {
                        if (onFinish != null)
                        {
                            onFinish();
                        }
                        if (mStopFrameType == EActionStopFrameType.End)
                        {
                            mIndex = mCurrentNames.Count - 1;
                            mSprite.SpriteName = mCurrentNames[mIndex];
                        }
                    }
                    if (mActive)
                    {
                        mIndex++;
                        return;
                    }
                }

                if (mActive)
                {
                    mSprite.SpriteName = mCurrentNames[mIndex];
                    
                    mIndex++;
                }
            }
        }
    }
}
