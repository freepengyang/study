using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSAudioOut : MonoBehaviour {
    public static CSAudioOut mcurNPCAudio = null;
    public static CSAudioOut curNPCAudio
    {
        get { return mcurNPCAudio; }
        set { mcurNPCAudio = value; }
    }
    private static List<CSAudioOut> mAudioOutList = new List<CSAudioOut>();//层级不在CSAudioMgr下面，夸场景不删除，删除逻辑在自己的Update中。
    public static List<CSAudioOut> AudioOutList
    {
        get { return mAudioOutList; }
    }
    private AudioSource mAudioSource;
    public UnityEngine.AudioSource AudioSource
    {
        get { return mAudioSource; }
        set { mAudioSource = value; }
    }

    private string mResPath = string.Empty;
    public string ResPath
    {
        get { return mResPath; }
        set { mResPath = value; }
    }

    private float mVolume;
    public uint tblID;
    public bool isLoading;

    public bool isPlaying
    {
        get
        {
            if (mAudioSource != null && mAudioSource.clip != null) return mAudioSource.isPlaying;
            return false;
        }
    }


    public static CSAudioOut Create(uint tblID, string resName, float volume)
    {
#if UNITY_EDITOR
        GameObject go = new GameObject(resName);
#else
        GameObject go = new GameObject("AudioOut");
#endif
        CSAudioOut a = go.AddComponent<CSAudioOut>();
        mAudioOutList.Add(a);
        a.Init(tblID, resName, volume);
        return a;
    }
    void Init(uint tblID, string resName, float volume)
    {
        DontDestroyOnLoad(gameObject);
        isLoading = true;
        mVolume = volume;
        mAudioSource = gameObject.AddComponent<AudioSource>();
        CSResource res = CSResourceManager.Singleton.AddQueue(resName, ResourceType.Audio, OnLoaded, ResourceAssistType.UI);
        mResPath = res.Path;
        res.IsCanBeDelete = false;
    }

    void OnLoaded(CSResource res)
    {
        if (this == null) return;
        isLoading = false;
        if (res.MirrorObj == null) return;
        mAudioSource.clip = res.MirrorObj as AudioClip;
        mAudioSource.volume = mVolume;
        mAudioSource.Play();
    }

    public void SetVolume(float f)
    {
        mVolume = f;
        if (mAudioSource != null)
        {
            mAudioSource.volume = f;
        }
    }

    void Update()
    {
        if (mAudioSource == null || mAudioSource.clip == null) return;
        if (!isPlaying)
        {
            Destroy();
        }
    }

    public static void EnableAudioOut(bool b)
    {
        for (int i = mAudioOutList.Count - 1; i >= 0; i--)
        {
            CSAudioOut a = mAudioOutList[i];
            if (a == null)
            {
                mAudioOutList.RemoveAt(i);
                continue;
            }
            if (a.gameObject.activeSelf != b)
            {
                a.gameObject.SetActive(b);
            }
        }
    }

    public void Destroy()
    {
        if (mcurNPCAudio == this)
        {
            if (CSAudioMgr.Instance != null) CSAudioMgr.Instance.OnNPcAudioPlayOver();
        }
        mAudioOutList.Remove(this);
        CSResource res = CSResourceManager.Instance.GetRes(mResPath);
        if (res != null)
        {
            res.IsCanBeDelete = true;
        }
        mAudioSource = null;
        if (gameObject != null)
        {
            GameObject.Destroy(gameObject);
        }
    }

    public static void Clear()
    {
        for (int i = 0; i < mAudioOutList.Count; i++)
        {
            CSAudioOut a = mAudioOutList[i];
            if (a != null)
            {
                a.Destroy();
            }
        }
        mAudioOutList.Clear();
    }
}
