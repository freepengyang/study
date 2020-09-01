using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelBinder : UIBinder
{
    UILabel lb_channel_name;
    UIEventListener btn_voice;
    UISprite sp_check_mark;
    UILabel lb_check_mark_name;
    UIToggle toggle;

    public override void Init(UIEventListener handle)
    {
        lb_channel_name = Get<UILabel>("lb_channel_name");
        btn_voice = UIEventListener.Get(handle.transform.Find("btn_voice").gameObject);
        sp_check_mark = Get<UISprite>("sp_check_mark");
        lb_check_mark_name = Get<UILabel>("sp_check_mark/lb_check_mark_name");
        toggle = Get<UIToggle>(string.Empty);
        EventHandle.AddEvent(CEvent.OnChatChannelChanged, OnChatChannelChanged);
        EventHandle.AddEvent(CEvent.OnVoiceStateChanged, OnVoiceStateChanged);
        EventHandle.AddEvent(CEvent.OnMainPlayerTeamIdChanged, OnVoiceStateChanged);
        EventHandle.AddEvent(CEvent.OnMainPlayerGuildIdChanged, OnVoiceStateChanged);
    }

    protected void OnVoiceStateChanged(uint id, object argv)
    {
        btn_voice.CustomActive(null != mChannelData && mChannelData.enableVoice);
    }

    protected void OnChatChannelChanged(uint id,object argv)
    {
        if(null != mChannelData)
        {
            int channel = (int)argv;
            if (channel == (int)mChannelData.eChatType)
            {
                toggle.value = true;
            }
        }
    }

    protected ChannelData mChannelData;
    public override void Bind(object data)
    {
        mChannelData = data as ChannelData;
        if (null != mChannelData)
        {
            //if(mChannelData.eChatType ==  ChatType.CT_GUILD)
            //{
            //    VoiceChatManager.Instance.GuildeVoiceIcon = btn_voice.gameObject;
            //}
            //else if(mChannelData.eChatType == ChatType.CT_TEAM)
            //{
            //    VoiceChatManager.Instance.TeamVoiceIcon = btn_voice.gameObject;
            //}
            lb_channel_name.text = CSChatManager.Instance.GetChanelName(mChannelData.eChatType);
            lb_check_mark_name.text = lb_channel_name.text;
            EventDelegate.Add(toggle.onChange, OnChannelSelected);
            btn_voice.CustomActive(null != mChannelData && mChannelData.enableVoice);
            if (mChannelData.isOn)
            {
                toggle.value = mChannelData.isOn;
            }
        }
    }

    protected void OnChannelSelected()
    {
        if(null != mChannelData && toggle.value)
        {
            mChannelData.onChannelChanged?.Invoke(mChannelData);
        }
    }

    public override void OnDestroy()
    {
        EventHandle.RemoveEvent(CEvent.OnChatChannelChanged, OnChatChannelChanged);
        EventHandle.RemoveEvent(CEvent.OnVoiceStateChanged, OnVoiceStateChanged);
        EventHandle.RemoveEvent(CEvent.OnMainPlayerTeamIdChanged, OnVoiceStateChanged);
        EventHandle.RemoveEvent(CEvent.OnMainPlayerGuildIdChanged, OnVoiceStateChanged);
    }
}