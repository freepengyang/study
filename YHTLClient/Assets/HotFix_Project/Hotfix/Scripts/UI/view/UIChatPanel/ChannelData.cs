using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnChannelChanged(ChannelData channelData);
public class ChannelData : IndexedItem
{
    public int Index { get; set; }
    public bool isOn;
    public int mask;
    public ChatType eChatType;
    /// <summary>
    /// 只要有人上麦了就是TRUE
    /// </summary>
    public bool enableVoice
    {
        get
        {
            if (eChatType == ChatType.CT_TEAM)
            {
                return !(CSMainPlayerInfo.Instance.TeamId == 0 || YvVoiceMgr.Instance.mLoginType != (int)VoiceLoginType.team || (!YvVoiceMgr.Instance.isOpenVoiceLister && !YvVoiceMgr.Instance.isOpenVoiceSpeak));
            }
            if(eChatType == ChatType.CT_GUILD)
            {
                if (CSMainPlayerInfo.Instance.GuildId == 0)
                    return false;
                if (YvVoiceMgr.Instance.mLoginType != (int)VoiceLoginType.union)
                    return false;
                //如果别人上麦了或者自己上麦了
                if (!YvVoiceMgr.Instance.isOpenVoiceLister && !YvVoiceMgr.Instance.isOpenVoiceSpeak)
                    return false;
                return true;
            }
            return false;
        }
    }
    public OnChannelChanged onChannelChanged;
};