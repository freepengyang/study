using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIRealTimeSpriteAni : MonoBehaviour
{
    private int FPS = 30;
    [HideInInspector]
    [SerializeField]
    protected string mPrefix = "";

    [SerializeField]
    protected bool Loop = true;

    [SerializeField]
    protected bool Snap = true;

    private float perFrameDelta=0;

    protected UISprite mSprite;
    //添加AnimSprite属性
    public UISprite AnimSprite { get { return mSprite ?? (mSprite = GetComponent<UISprite>()); } }
    protected float mDelta = 0f;
    protected int mIndex = 0;
    protected bool mActive = true;
    protected List<string> mSpriteNames = new List<string>();

    /// <summary>
    /// Number of frames in the animation.
    /// </summary>

    public int frames { get { return mSpriteNames.Count; } }

    /// <summary>
    /// Animation framerate.
    /// </summary>

    public int framesPerSecond { get { return FPS; } set { FPS = value; } }

    /// <summary>
    /// Set the name prefix used to filter sprites from the atlas.
    /// </summary>

    public string namePrefix { get { return mPrefix; } set { if (mPrefix != value) { mPrefix = value; RebuildSpriteList(); } } }

    /// <summary>
    /// Set the animation to be looping or not
    /// </summary>

    public bool loop { get { return Loop; } set { Loop = value; } }

    /// <summary>
    /// Returns is the animation is still playing or not
    /// </summary>

    public bool isPlaying { get { return mActive; } }

    /// <summary>
    /// Rebuild the sprite list first thing.
    /// </summary>

    protected virtual void Start() { RebuildSpriteList(); }

    /// <summary>
    /// Advance the sprite animation process.
    /// </summary>
    void FixedUpdate()
    {
        if (mActive && mSpriteNames.Count > 1 && Application.isPlaying && FPS > 0)
        {
            float curMillionSec = Time.realtimeSinceStartup - (int)Time.realtimeSinceStartup;

            mIndex = (int)(curMillionSec/ perFrameDelta);


            if (mIndex >= mSpriteNames.Count)
            {
                mIndex = 0;
                mActive = Loop;
                //添加完成事件
                if (!Loop && OnFinish != null) OnFinish();
            }

            if (mActive)
            {
                mSprite.spriteName = mSpriteNames[mIndex];
                if (Snap) mSprite.MakePixelPerfect();
            }
        }
    }

    /// <summary>
    /// Rebuild the sprite list after changing the sprite name.
    /// </summary>

    public void RebuildSpriteList()
    {
        if (mSprite == null) mSprite = GetComponent<UISprite>();
        mSpriteNames.Clear();

        perFrameDelta = 1.0f / (float)FPS;//一帧需要的时间

        if (mSprite != null && mSprite.atlas != null)
        {
            List<UISpriteData> sprites = mSprite.atlas.spriteList;

            for (int i = 0, imax = sprites.Count; i < imax; ++i)
            {
                UISpriteData sprite = sprites[i];

                if (string.IsNullOrEmpty(mPrefix) || sprite.name.StartsWith(mPrefix))
                {
                    mSpriteNames.Add(sprite.name);
                }
            }
            mSpriteNames.Sort(delegate(string x, string y) { return int.Parse(x).CompareTo(int.Parse(y)); });
        }

        FPS = mSpriteNames.Count;
    }

    /// <summary>
    /// Reset the animation to the beginning.
    /// </summary>

    public void Play() { mActive = true; }

    /// <summary>
    /// Pause the animation.
    /// </summary>

    public void Pause() { mActive = false; }

    /// <summary>
    /// Reset the animation to frame 0 and activate it.
    /// </summary>

    public void ResetToBeginning()
    {
        mActive = true;
        mIndex = 0;

        if (mSprite != null && mSpriteNames.Count > 0)
        {
            mSprite.spriteName = mSpriteNames[mIndex];
            if (Snap) mSprite.MakePixelPerfect();
        }
    }

    /// <summary>
    /// 设置动画信息(在动态添加脚本的时候调用来初始化游戏数据)
    /// </summary>
    /// <param name="_Loop">是否循环</param>
    /// <param name="_Snap">是否snap图片</param>
    public void SetAnimationInfo(bool _Loop,bool _Snap)
    {
        this.Loop = _Loop;
        this.Snap = _Snap;
        RebuildSpriteList();
    }

    ////////////////////////////////////////////////
    public System.Action OnFinish;

}
