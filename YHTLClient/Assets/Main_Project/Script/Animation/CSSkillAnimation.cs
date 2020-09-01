
//-------------------------------------------------------------------------
//Author jiabao
//Time 2015.12.15
//-------------------------------------------------------------------------

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSSkillAnimation : CSBaseAnimation
{
    public IBaseFrame FrameAni;
    private float mPlayBeginTime;
    private Transform mChild;
    private bool mIsDataSplit;

    public bool Run
    {
        get
        {
            return FrameAni != null && FrameAni.isPlaying;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="go"></param>
    /// <param name="avater"></param>
    /// <param name="parent">是否有父类</param>
    public void InItData(GameObject go, bool isDataSplit)
    {
        mIsDataSplit = isDataSplit;
        Go = go;

        if (Go != null && mChild == null)
        {
            mChild = new GameObject("Ani").transform;
            mChild.parent = Go.transform;
            mChild.localPosition = Vector3.zero;
            mChild.localScale = Vector3.one;

            Sprite = mChild.gameObject.AddComponent<CSSprite>();
            if (mIsDataSplit)
            {
                FrameAni = mChild.gameObject.AddComponent<CSSkillFrame2>();
            }
            else
            {
                FrameAni = mChild.gameObject.AddComponent<CSSkillFrame>();
            }
        }
        else
        {
            Sprite = mChild.GetComponent<CSSprite>();
            if (mIsDataSplit)
            {
                FrameAni = mChild.GetComponent<CSSkillFrame2>();
            }
            else
            {
                FrameAni = mChild.GetComponent<CSSkillFrame>();
            }
        }

        if (FrameAni != null)
        {
            FrameAni.Initialization();
        }
    }

    void Update()
    {
        if (FrameAni == null) return;

        if (Run)
        {
            if (Time.time >= mPlayBeginTime)
            {
                BaseFrame b = FrameAni as BaseFrame;
                if (b.onStart != null)
                {
                    b.onStart();
                }
                else
                {
                    if (FrameAni != null)
                    {
                        FrameAni.UpdateFrame();
                    }
                }
            }
        }
    }
    // 延迟播放
    public void Play(float time)
    {
        if (FrameAni != null)
        {
            mPlayBeginTime = Time.time + time;
            FrameAni.Play();
        }
    }

    public void SetCurrentNames(int frame)
    {
        if (FrameAni != null)
        {
            if (mIsDataSplit)
            {
                CSBaseFrame2 b = FrameAni as CSBaseFrame2;
                b.CurrentNameCount = (int)frame;
            }
        }
    }

    public void ClearCurrentNames()
    {
        if (FrameAni != null)
        {
            if (mIsDataSplit)
            {
                CSBaseFrame2 b = FrameAni as CSBaseFrame2;
                b.Clear();
            }
        }
    }

    public void ClearAtlas()
    {
        if (FrameAni != null)
        {
            if (mIsDataSplit)
            {
                CSBaseFrame2 b = FrameAni as CSBaseFrame2;
                b.ClearAtlas();
            }
        }
    }

    public void Stop()
    {
        if (FrameAni != null)
        {
            FrameAni.Stop();
        }
    }

    public void setFPS(int fps)
    {
        if (FrameAni != null)
        {
            if (FrameAni.isChange)
                FrameAni.FPS = fps;
        }
    }
    public void setLoop(bool bl)
    {
        if (FrameAni != null)
        {
            FrameAni.setLoop(bl);
        }
    }

    public void setAtlas(UIAtlas atlas)
    {
        if (Sprite != null && FrameAni != null)
        {
            Sprite.getAtlas = atlas;
            UpdateAnimationsNames();
            CSSkillFrame sf = FrameAni as CSSkillFrame;
            if (sf != null)
            {
                sf.setFirstName();
                sf.setMeshRenderer(false);
            }
            FrameAni.SetAtlasPostProc();
        }
    }

    public void UpdateAnimationsNames(int curDirection = 8)
    {
        if (FrameAni != null)
        {
            FrameAni.RefreshNames();
        }
    }
}
