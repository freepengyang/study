using UnityEngine;
using System;
using System.Collections;

public class NHyperLink_Voice : MonoBehaviour
{
    private GameObject _change;

    private GameObject change
    {
        get { return _change ?? (_change = transform.Find("change").gameObject); }
    }


    private UISpriteAnimation _changeAni;

    private UISpriteAnimation changeAni
    {
        get { return _changeAni ?? (_changeAni = change.GetComponent<UISpriteAnimation>()); }
    }

    private UISprite _changeSpr;

    private UISprite changeSpr
    {
        get { return _changeSpr ?? (_changeSpr = change.GetComponent<UISprite>()); }
    }

    public string mUrl { get; set; }
    public int mDuration { get; set; }
    public string ext { get; set; }

    private UILabel lb_text = null;

    private const float delayTime = 3; //3秒之内只能点击一次
    private bool canPlayVoick = true;

    void Awake()
    {
        lb_text = this.transform.Find("lb_text").GetComponent<UILabel>();

        PlayVoiceChangeBG(false);
    }

    void OnClick()
    {
        PlayVoice();
    }

    public void StopChange()
    {
        PlayVoiceChangeBG(false);

        HotFix_InvokeThird.UIStopChatVoice();
    }

    private void PlayVoice()
    {
        if (string.IsNullOrEmpty(mUrl)) return;

        if (!canPlayVoick)
        {
            canPlayVoick = false;
            return;
        }

        bool isPlay = HotFix_InvokeThird.UIPlayChatVoice(mUrl, PlayFinish);
        if (isPlay)
        {
            Invoke("CanDownLoad", delayTime);
            PlayVoiceChangeBG(true);
            Invoke("StopChange", mDuration);
        }
    }

    private void PlayFinish()
    {
        PlayVoiceChangeBG(false);
    }

    void CanDownLoad()
    {
        canPlayVoick = true;
    }

    private void PlayVoiceChangeBG(bool isPlay)
    {
        changeAni.enabled = isPlay;
        if (!isPlay)
        {
            changeSpr.spriteName = "chat_yy3";
        }
    }
}