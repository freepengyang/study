
//author LiZongFu
//date 2016.5.11

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public interface IBaseFrame
{
    void Play(bool isReset = true);
    void Initialization();
    bool isPlaying { get; }
    void UpdateFrame();
    float EstimateTakeTime { get; }
    bool isChange { get; set; }
    int FPS { get; set; }
    bool Loop { get; set; }
    void setLoop(bool bl);
    bool getLoop();
    void Clear();
    void RefreshNames();
    void SetAtlasPostProc();
    void Stop();
    SFAtlas atlas { get; }
    int curFrame { get; set; }
    bool IsPlayingOnLastOneFrame();
    void OnNoLoopPlayFinish();
    void OnChangeOneFrame(int frame);
}

public abstract class BaseFrame : MonoBehaviour
{
    public System.Action onStart;
    public System.Action onFinish;

    private int mStopFrame = EActionStopFrameType.First;
    public int StopFrame
    {
        get { return mStopFrame; }
        set { mStopFrame = value; }
    }
}

public class SFBaseFrame : BaseFrame, IBaseFrame
{
    public virtual void SetAtlasPostProc()
    {

    }
    private int mFPS = 10;
    public int FPS
    {
        get { return mFPS; }
        set { mFPS = value; }
    }
    private bool mLoop = false;
    public bool Loop
    {
        get { return mLoop; }
        set { mLoop = value; }
    }
    private bool mIsChange = true;
    public bool isChange
    {
        get { return mIsChange; }
        set { mIsChange = value; }
    }

    public SFAtlas atlas
    {
        get
        {
            if (mSprite != null) return mSprite.getAtlas;
            return null;
        }
    }

    protected bool mActive = false;
    protected int mIndex = 0;
    protected float mDelta = 0f;
    
    protected CSSpriteBase mSprite;

    protected CSBetterList<string> mCurrentNames = new CSBetterList<string>();

    public int mCurrentNameCount = 8;//默认10个贴图

    protected MeshRenderer mMeshRenderer;

    public int curFrame
    {
        get 
        {
            return mIndex; 
        }
        set
        {
            mIndex = value;
        }
    }

    public bool IsPlayingOnLastOneFrame()
    {
        return mIndex == mCurrentNames.Count - 1;
    }

    public int frames
    {
        get { return mCurrentNames.Count; }
    }

    public float EstimateTakeTime
    {
        get
        {
            float t = ((1000f / (float)FPS) * (float)mCurrentNames.Count) * 0.001f;
            return t;
        }
    }

    public bool isPlaying
    {
        get { return mActive; }
    }

    public virtual void Start()
    {
        mMeshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    public virtual void Stop()
    {

    }

    public virtual void OnNoLoopPlayFinish()
    {

    }

    public virtual void Initialization()
    {
        mSprite = gameObject.GetComponent<CSSpriteBase>();
    }

    public void Play(bool isReset = true)
    {
        mActive = true;
        curFrame = 0;
    }

    public virtual void Pause()
    {
        mActive = false;
    }

    public virtual void setLoop(bool bl)
    {
        Loop = bl;
        mActive = true;
    }

    public virtual bool getLoop()
    {
        return Loop;
    }

    public virtual void setMeshRenderer(bool bl)
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

    public virtual void setFirstName()
    {
        if (mCurrentNames.Count > 0)
        {
            mSprite.SpriteName = mCurrentNames[0];
        } 
    }

    public virtual void RefreshNames(CSBetterList<string> list, bool isReset = false)
    {
        if (list != null && list.Count > 0)
        {
            if (isReset) ResetToBeginning();
            mCurrentNames = list;
        }
    }

    public void Clear()
    {
        mCurrentNames.Clear();
    }

    public virtual void RefreshNames()
    {
        if (mSprite == null)
            mSprite = gameObject.GetComponent<CSSpriteBase>();

        if (mSprite != null && mSprite.getAtlas != null)
        {
            RebuildSpriteList();
        }
    }

    public virtual void RebuildSpriteList()
    {
    }
  
    /// <summary>
    /// 重置
    /// </summary>
    public virtual void ResetToBeginning()
    {
        if (mSprite != null && mCurrentNames.Count > 0)
        {
            curFrame = 0;
        }
    }

    public virtual void OnDestroy()
    {
        mSprite = null;
        mCurrentNames.Clear();
    }

    public virtual void UpdateFrame()
    {
    }

    public virtual void OnChangeOneFrame(int frame) { }
}
