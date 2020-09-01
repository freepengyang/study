using UnityEngine;
using System.Collections;

public class CSLoopAudio : MonoBehaviour {

	// Use this for initialization
    public AudioSource a1;
    public AudioSource a2;
    private AudioSource cur;
    int count = 0;
    bool isBegin = false;
    float volume = 1;
    public void Begin(AudioClip clip,float volume)
    {
        this.volume = volume;
        if (a1 == null)
        {
            a1 = gameObject.AddComponent<AudioSource>();
            a1.playOnAwake = false;
        }
        if (a2 == null)
        {
            a2 = gameObject.AddComponent<AudioSource>();
            a2.playOnAwake = false;
        }
        a1.clip = clip;
        a2.clip = clip;
        a1.volume = volume;
        a2.volume = volume;
        a1.Stop();
        a2.Stop();
        cur = GetCurAudio();
        cur.PlayScheduled(0.1f);
        count++;
        isBegin = true;
    }

    public void SetVolume(float v)
    {
        volume = v;
        if (a1 != null) a1.volume = v;
        if (a2 != null) a2.volume = v;
    }

    AudioSource GetCurAudio()
    {
        if (count % 2 == 0) return a1;
        return a2;
    }

    public void Update()
    {
        if (!isBegin || cur.clip == null) return;
        if (cur.time+0.1f>=cur.clip.length)
        {
            //cur.Stop();
            cur = GetCurAudio();
            cur.PlayScheduled(0.1f);
            count++;
        }
    }
}
