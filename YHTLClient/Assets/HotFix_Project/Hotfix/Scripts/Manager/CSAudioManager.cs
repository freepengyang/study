using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSAudioManager : CSInfo<CSAudioManager>
{
    private AudioData audioData;

    public CSAudioManager()
    {
        audioData = new AudioData();
    }

    public void Play(bool isMainPlayer, int audioTblID, int action = 0, int delayTime = 0,bool isLoop = false)
    {
        PlayAudio(isMainPlayer, audioTblID, action, delayTime, isLoop);
    }

    public CSAudio PlayAudio(bool isMainPlayer, int audioTblID, int action = 0, int delayTime = 0, bool isLoop = false)
    {
        if (CSAudioMgr.Instance == null) return null;

        int key = action * 10000000 + audioTblID;
        CSAudio audio = null;
        if (AudioTableManager.Instance.TryGetValue(key, out TABLE.AUDIO tbl))
        {
            if (tbl == null) return null;

            audioData.resourcePath = tbl.resourcePath;
            audioData.Type = tbl.Type;
            audioData.BgMusic = CSConfigInfo.Instance.GetBool(ConfigOption.BgMusic);
            audioData.BgMusicSlider = CSConfigInfo.Instance.GetFloat(ConfigOption.BgMusicSlider);
            audioData.EffectSound = CSConfigInfo.Instance.GetBool(ConfigOption.EffectSound);
            audioData.EffectSoundSlider = CSConfigInfo.Instance.GetFloat(ConfigOption.EffectSoundSlider);
            audioData.Volume = tbl.Volume;
            audio = CSAudioMgr.Instance.Play(isMainPlayer, audioTblID, audioData, action, delayTime, isLoop);
        }
        return audio;
    }



    /// <summary>
    /// UIPlaySoundExtend  挂脚本，用事件传递
    /// </summary>
    /// <param name="type"></param>
    public void PlayUIInThird(int type)
    {
        Play(true, type);
    }

    public override void Dispose()
    {
    }

}
