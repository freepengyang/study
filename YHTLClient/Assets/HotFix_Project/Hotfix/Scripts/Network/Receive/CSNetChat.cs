using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class CSNetChat : CSNetBase
{
    void ECM_ResChatMessage(NetInfo info)
    {
        chat.ChatMessage msg = Network.Deserialize<chat.ChatMessage>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forchat.ChatMessage");
            return;
        }

        CSChatManager.Instance.AddChatMsg(msg);
    }

    void ECM_LeftCornerTipNtfMessage(NetInfo info)
    {
        chat.LeftCornerTipNtf msg = Network.Deserialize<chat.LeftCornerTipNtf>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forchat.LeftCornerTipNtf");
            return;
        }
    }

    /// <summary>
    /// 行会语音召集令
    /// </summary>
    /// <param name="info"></param>
    void ECM_ReleaseNtfMessage(NetInfo info)
    {
        chat.ReleaseNtf msg = Network.Deserialize<chat.ReleaseNtf>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forchat.ReleaseNtf");
            return;
        }

        FNDebug.LogFormat("<color=#00ff00>[语音聊天]:收到行会召集令</color>");
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnLeaderCallMessage, msg);
    }

    void ECM_VoiceRoomNtfMessage(NetInfo info)
    {
        chat.VoiceRoomNtf msg = Network.Deserialize<chat.VoiceRoomNtf>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forchat.VoiceRoomNtf");
            return;
        }

        FNDebug.LogFormat("<color=#00ff00>[语音聊天]:收到服务器OnVoiceRoomNtfMessage:[人数:{0}]</color>", msg.num);
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnVoiceRoomNtfMessage, msg);
    }

    void ECM_RoleDetailNtfMessage(NetInfo info)
    {
        chat.RoleDetailNtf msg = Network.Deserialize<chat.RoleDetailNtf>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forchat.RoleDetailNtf");
            return;
        }

        CSGuildInfo.Instance.RefreshUpMicPlayers(msg.infos);
    }

    void ECM_ForbidChatNtfMessage(NetInfo info)
    {
        chat.ForbidChatNtf msg = Network.Deserialize<chat.ForbidChatNtf>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forchat.ForbidChatNtf");
            return;
        }

        CSChatManager.Instance.ForbidChat(msg.roleId);
    }

    void ECM_BigExpressionNtfMessage(NetInfo info)
    {
        chat.BigExpressionNtf msg = Network.Deserialize<chat.BigExpressionNtf>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forchat.BigExpressionNtf");
            return;
        }
    }
}