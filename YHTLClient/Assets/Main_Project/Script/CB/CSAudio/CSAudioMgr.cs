using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum EAudioType
{
    None = 0,
    BGM = 1,
    UI = 2,
    Player = 3,
    Monster = 4,
    NPC = 5,
    SkillEffect = 6,
}

public class AudioData
{
    public int Type { get; set; }

    public int Volume { get; set; }

    public string resourcePath { get; set; }

    public bool BgMusic { get; set; }

    public float BgMusicSlider { get; set; }

    public bool EffectSound { get; set; }

    public float EffectSoundSlider { get; set; }
}

public class CSAudioMgr : CSGameMgrBase2<CSAudioMgr>
{
    public CSAudioMgr()
    {
        mIsEnable = !YvVoiceMgr.Instance.isOpenVoiceSpeak;
    }

    private CSBetterList<CSAudio> mAudioList;

    public CSAudio mCurBgm = null;

    private bool mIsEnable = true;

    public Type curNPCWinType;

    public Action<CSAudio> onRemoveAudio;

    public static List<uint> hasPlayNPCAudioList;

    const float mNpcAudioScaleOther = 0.1f;

    public override bool IsDonotDestroy
    {
        get { return true; }
    }

    public override void Awake()
    {
        mAudioList = new CSBetterList<CSAudio>();
        hasPlayNPCAudioList = new List<uint>();
    }

    public void ClearOnReturn()
    {
        hasPlayNPCAudioList?.Clear();
        RemoveAllAudio();
    }

    public void RegRemoveAudioCallBack(System.Action<CSAudio> callBack)
    {
        onRemoveAudio -= callBack;
        onRemoveAudio += callBack;
    }

    public void UnRegRemoveAudioCallBack(System.Action<CSAudio> callBack)
    {
        onRemoveAudio -= callBack;
    }

    public void Update()
    {
        if (mAudioList == null) return;

        for (int i = mAudioList.Count - 1; i >= 0; i--)
        {
            CSAudio audio = mAudioList[i];
            audio.CSUpdate();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path">相对于StreamingAssets文件夹,例如Audio/UI/xxxx.assetbundle,</param>
    /// <param name="name"></param>
    /// <param name="isLoop"></param>
    /// <param name="volume">背景和音效的声音音量都在这里控制</param>
    /// <param name="isRePlay">是否重新播放</param>
    /// <param name="isLoop">是否需要循环</param>
    /// <param name="poolNum">=0时,播放完毕后就扔给PoolManger自动删除,如果非0,下载前先从PoolManger拿，如果能拿到，直接用，反之Load，创建，播放完毕之后，将其扔给PoolManger</param>
    private CSAudio Play(EAudioType type, bool isMainPlayer, string name, float fadeInTime = 0, float volume = 1,
        bool isLoop = false, bool isClone = true, int delayBeing = 0, bool isBg = false)
    {
        if (!mIsEnable) return null;
        int num = GetAudioNum(name);
        if (num >= 5 && !isMainPlayer) return null;
        CSAudio audio = null;
        CSObjectPoolItem poolItem = GetAndAddPoolItem_Class("CSAudio", "CSAudio", null, typeof(CSAudio), null);
        if(poolItem == null)
        {
            return null;
        }
        audio = poolItem.objParam as CSAudio;
        audio.Init(this);
        audio.PoolItem = poolItem;
        audio.Play(type, name, fadeInTime, volume, isLoop, isClone, 0, isBg);
        if (null != mAudioList) mAudioList.Add(audio);
        return audio;
    }

    public int GetAudioNum(string name)
    {
        if (mAudioList == null) return 0;
        int num = 0;
        for (int i = 0; i < mAudioList.Count; i++)
        {
            CSAudio a = mAudioList[i];
            if (a == null) continue;
            if (a.ResName == name) num++;
        }

        return num;
    }

    public CSAudio Play(bool isMainPlayer, int audioTblID, AudioData audioData, int action = 0, int delayTime = 0,
        bool isLoop = false)
    {
        if (!mIsEnable) return null;

        CSAudio a = null;

        switch ((EAudioType) audioData.Type)
        {
            case EAudioType.BGM:
            {
                float volume = audioData.Volume * 0.01f * (!audioData.BgMusic ? 0 : audioData.BgMusicSlider);
                if (volume < 0.01f) volume = 0;
                if (mCurBgm != null)
                {
                    mCurBgm.CSOnDestroy();
                }
                else
                {
                    mCurBgm = new CSAudio();
                }

                mCurBgm.Init(this);
                mCurBgm.Play((EAudioType) audioData.Type, audioData.resourcePath, 0, volume, true, false, delayTime,
                    true);
                if (!mAudioList.Contains(mCurBgm))
                    mAudioList.Add(mCurBgm);
                a = mCurBgm;
            }
                break;
            case EAudioType.NPC:
            {
                //PlayNPCAudio(typeof(UIMissionAwardPanel), tbl.id);
            }
                break;
            case EAudioType.UI:
            case EAudioType.Player:
            case EAudioType.Monster:
            case EAudioType.SkillEffect:
            {
                float volume = audioData.Volume * 0.01f * (!audioData.EffectSound ? 0 : audioData.EffectSoundSlider);
                if (CSAudioOut.curNPCAudio != null) volume *= mNpcAudioScaleOther;
                if (volume < 0.01f) volume = 0;
                a = Play((EAudioType) audioData.Type, isMainPlayer, audioData.resourcePath, 0, volume, isLoop, true,
                    delayTime);
            }
                break;
        }

        if (a != null) a.tblID = audioTblID;

        return a;
    }

    //开启游戏，表格未加载
    public void PlayLoginBGM()
    {
        float volume = 0.5f;
        if (volume < 0.01f) volume = 0;
        if (mCurBgm != null)
        {
            mCurBgm.CSOnDestroy();
        }
        else
        {
            mCurBgm = new CSAudio();
        }

        mCurBgm.Init(this);
        mCurBgm.Play(EAudioType.BGM, "BGM/login", 0, volume, true, false, 0, true);
        if (!mAudioList.Contains(mCurBgm))
            mAudioList.Add(mCurBgm);
    }

    public void SetBgVolume(EAudioType type, float f, float fadeTime = 0)
    {
        float volume = 1;
        if (volume < 0.01f) volume = 0;
        if (volume == 0) return;
        if (type == EAudioType.NPC)
        {
            if (mCurBgm != null)
            {
                if (fadeTime != 0)
                {
                    mCurBgm.SetFadeInTime(volume * f, fadeTime);
                }
                else
                {
                    mCurBgm.SetLoopVolume(volume * f);
                }
            }
        }
    }

    public void SetEffectVolume(EAudioType type, float f, float fadeTime = 0) //type 音效播放的时候，自动将技能音量减小到f（百分比）
    {
        float volume = 1;
        if (volume < 0.01f) volume = 0;
        if (volume == 0) return;
        if (type == EAudioType.NPC)
        {
            for (int i = mAudioList.Count - 1; i >= 0; i--)
            {
                CSAudio audio = mAudioList[i];
                if (audio.AudioType != EAudioType.NPC && !audio.IsBg)
                {
                    if (fadeTime != 0)
                    {
                        audio.SetFadeInTime(volume * f, fadeTime);
                    }
                    else
                    {
                        audio.SetValue(volume * f);
                    }
                }
            }
        }
    }

    public void EnableAudioMgr(bool b)
    {
        if (this == null) return;
        //开启实时语音时，暂停其他声音
        if (b && YvVoiceMgr.Instance.isOpenVoiceSpeak)
            return;
        if (gameObject.activeSelf != b)
        {
            gameObject.SetActive(b);
        }

        CSAudioOut.EnableAudioOut(b);
        mIsEnable = b;
    }

    public void RemoveAudio(CSAudio audio)
    {
        if (audio != null)
        {
            if (onRemoveAudio != null)
                onRemoveAudio(audio);
            audio.CSOnDestroy();
            mAudioList.Remove(audio);
        }
    }

    public override void Destroy()
    {
        RemoveAllAudio();
        mAudioList.Clear();
        mCurBgm = null;
        base.Destroy();
    }

    public override void OnDestroy()
    {
        RemoveAllAudio();
        mAudioList.Clear();
        mAudioList = null;
        mCurBgm = null;
        base.OnDestroy();
    }

    public void RemoveAllAudio()
    {
        if (mAudioList != null)
        {
            for (int i = mAudioList.Count - 1; i >= 0; i--)
            {
                RemoveAudio(mAudioList[i]);
            }

            mAudioList.Clear();
            RemoveAudio(mCurBgm);
        }

        mCurBgm = null;
    }

    public void OnNPcAudioPlayOver()
    {
        SetBgVolume(EAudioType.NPC, 1, 1);
        SetEffectVolume(EAudioType.NPC, 1, 1);
    }

    public static void PlayFirstGuideAudio(int id)
    {
        //if(Instance)
        //{
        //    if (!CSScene.MainPlayerInfo.SpeechRecord[id])
        //    {
        //        TABLE.AUDIOVOICE table;
        //        if (AudioVoiceTableManager.Instance.TryGetValue((uint)id, out table))
        //        {
        //            Instance.Play(true, table.audioId);
        //            CSScene.MainPlayerInfo.SpeechRecord[id] = true;
        //            Net.ReqSaveRoleSpeechMessage();
        //        }
        //    }
        //}
    }

    private CSObjectPoolItem GetAndAddPoolItem_Class(string poolNameShow, string poolName, GameObject go, Type type,
        params object[] args)
    {
        if (CSObjectPoolMgr.Instance == null) return null;

        CSObjectPoolItem poolItem =
            CSObjectPoolMgr.Instance.GetAndAddPoolItem_Class(poolNameShow, poolName, go, type, args);

        if (poolItem.objParam == null && type != null)
        {
            poolItem.objParam = Activator.CreateInstance(type);
        }

        return poolItem;
    }
}