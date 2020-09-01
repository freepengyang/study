using UnityEngine;
using System.Collections;

public class CSAudioMono : CSMonoBase
{
    public AudioSource audioSource;
    public bool isLoop = false;
    public CSLoopAudio loopAudio = null;
    public void Play(bool isLoop,bool isBg,float volume)
    {
        this.isLoop = isLoop;
        if (isLoop&&isBg)
        {
            audioSource.loop = false;
            if (loopAudio == null)
            {
                loopAudio = gameObject.AddComponent<CSLoopAudio>();
            }
            else
            {
                loopAudio.enabled = true;
            }
            loopAudio.Begin(audioSource.clip,volume);
        }
        else
        {
            audioSource.loop = isLoop;
            audioSource.Play();
        }
       
    }

    public void SetLoopVolume(float v)
    {
        if (loopAudio != null)
            loopAudio.SetVolume(v);
    }

    public override void CSUpdate()
    {
        base.CSUpdate();
    }

    public override void CSOnDestroy()
    {
        base.CSOnDestroy();
        if (loopAudio != null)
            loopAudio.enabled = false;
    }
}
