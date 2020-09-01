using System;

public class HotFix_InvokeThird
{
    //UIPlaySoundExtend 
    public static Action<int> UIPlaySoundInCSAudioAction;

    public static void UIPlaySoundInCSAudio(int type)
    {
        if (UIPlaySoundInCSAudioAction != null)
        {
            UIPlaySoundInCSAudioAction(type);
        }
    }

    //语音播放
    public static Func<string, Action, bool> UIPlayChatVoiceAction;

    public static bool UIPlayChatVoice(string url, Action finish)
    {
        if (UIPlayChatVoiceAction != null)
        {
            return UIPlayChatVoiceAction(url, finish);
        }

        return false;
    }
    
    //停止语音播放
    public static Action UIStopChatVoiceAction;

    public static void UIStopChatVoice()
    {
        if (UIStopChatVoiceAction != null)
        {
            UIStopChatVoiceAction();
        }
    }
}