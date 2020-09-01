using UnityEngine;
using System.Collections;

/// <summary>
/// 只是用这个功能同时来测试Mono和数据分离的功能，实际并不需要写成这样
/// </summary>
public class CSAudio : CSDataBase
{
    public float volume;

    public bool isLoop = false;

    public float liveTime = 0;

    private float mBeginPlayTime = 0;

    public float fadeInTime = 0;

    private float fadeInBeginVolume = 0;

    public bool isClone = false;

    public string name;

    string resPath;

    private CSAudioMono audioMono;

    string mResName = "";//资源的名字
    public string ResName
    {
        get { return mResName; }
        set { mResName = value; }
    }
    CSObjectPoolItem mPoolItem;
    public CSObjectPoolItem PoolItem
    {
        get { return mPoolItem; }
        set { mPoolItem = value; }
    }

    CSObjectPoolItem mResPoolItem;

    private CSAudioMgr mMgr;

    public int tblID;

    public bool isPlaying = false;

    public bool isLoading = false;

    private int mDelayPlay = 0;
    private bool mIsBg = false;
    public bool IsBg
    {
        get { return mIsBg; }
        set { mIsBg = value; }
    }
    private EAudioType mAudioType = EAudioType.BGM;
    public EAudioType AudioType
    {
        get { return mAudioType; }
        set { mAudioType = value; }
    }

    public void Init(CSAudioMgr mgr)
    {
        mMgr = mgr;
    }

    public void Play(EAudioType type,string name, float fadeInTime = 0, float volume = 1, bool isLoop = true, bool isClone = true, int delayPlay = 0,bool isBg = false)//delayPlay 百分比
    {
        this.volume = volume;
        this.isLoop = isLoop;
        this.fadeInTime = fadeInTime;
        this.isClone = isClone;
        this.mIsBg = isBg;
        this.AudioType = type;
        mResName = name;
        isLoading = true;
        mDelayPlay = delayPlay;
        LoadAudio(name);
    }
    
    void LoadAudio(string name)
    {
        this.name = name;
        LoadResource(name, ResourceType.Audio, ResourceAssistType.ForceLoad, OnLoadAudio);
    }

    void OnLoadAudio(CSDataBase data)
    {
        isLoading = false;
        resPath = data.Res.Path;

        if (Res.MirrorObj == null)
        {
            CSOnDestroy();
            return;
        }

        audioMono = data.MonoInfo as CSAudioMono;
        if (audioMono == null)
        {
            GameObject go = new GameObject("Audio");
            go.transform.parent = CSAudioMgr.CahcheTrans;
            data.MonoInfo = go.AddComponent<CSAudioMono>();
            audioMono = data.MonoInfo as CSAudioMono;
            audioMono.audioSource = go.AddComponent<AudioSource>();
            audioMono = data.MonoInfo as CSAudioMono;
        }
        else
        {
            audioMono.gameObject.SetActive(true);
        }
        audioMono.audioSource.clip = Res.MirrorObj as AudioClip;
        liveTime = audioMono.audioSource.clip.length;
        if (fadeInTime == 0)
        {
            audioMono.audioSource.volume = volume;
        }
        if (mDelayPlay == 0) audioMono.Play(isLoop, mIsBg,volume);
        mBeginPlayTime = Time.time;
        if (CSObjectPoolMgr.Instance != null)
        {
            CSObjectPoolMgr.Instance.RemovePoolItem(mResPoolItem);
            mResPoolItem = CSObjectPoolMgr.Instance.GetAndAddPoolItem_Resource(audioMono.audioSource.clip.name, resPath, null);
        }
    }

    public void SetValue(float volume)
    {
        if (fadeInTime != 0)
        {
            fadeInBeginVolume = volume;
        }
        else
        {
            this.volume = volume;
            if (audioMono != null && audioMono.audioSource != null)
                audioMono.audioSource.volume = volume;
        }
    }

    public void SetFadeInTime(float volume,float time)
    {
        fadeInTime = time;
        fadeInBeginVolume = volume;
    }

    public void SetLoopVolume(float v)
    {
        if (fadeInTime != 0)
        {
            fadeInBeginVolume = v;
        }
        else
        {
            this.volume = v;
            if (audioMono != null)
                audioMono.SetLoopVolume(v);
        }
    }

    public override void CSUpdate()
    {
        base.CSUpdate();

        if (audioMono != null && audioMono.audioSource != null)
            isPlaying = audioMono.audioSource.isPlaying;

        if (fadeInTime != 0)
        {
            if (audioMono != null)
            {
                volume = Mathf.Lerp(volume, fadeInBeginVolume, Time.deltaTime * fadeInTime);
                if (IsBg)
                {
                    audioMono.SetLoopVolume(volume);
                }
                else
                {
                    audioMono.audioSource.volume = volume;
                }
                if (Mathf.Abs(volume - fadeInBeginVolume) < 0.01f)
                {
                    fadeInTime = 0;
                }
            }
        }

        if (mDelayPlay != 0 && audioMono != null)
        {
            if (Time.time - mBeginPlayTime > mDelayPlay * 0.01f)
            {
                mDelayPlay = 0;
                audioMono.Play(isLoop, mIsBg, volume);
            }
        }
        if (!isLoop)
        {
            if (mDelayPlay == 0 && audioMono != null && audioMono.audioSource != null && !isPlaying && !isLoading)
            {
                mMgr.RemoveAudio(this);
            }
        }
    }

    public override void CSOnDestroy()
    {
        if (MonoInfo != null)
        {
            MonoInfo.gameObject.SetActive(false);
        }
        isLoading = false;
        RemovePoolItem();
    }

    void RemovePoolItem()
    {
        if (CSObjectPoolMgr.Instance == null) return;
        CSObjectPoolMgr.Instance.RemovePoolItem(mResPoolItem);
        mResPoolItem = null;
        CSObjectPoolMgr.Instance.RemovePoolItem(mPoolItem);
        mPoolItem = null;
    }
}
