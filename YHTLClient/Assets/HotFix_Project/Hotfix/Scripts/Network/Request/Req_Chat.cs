using System;
using Google.Protobuf.Collections;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public partial class Net
{
    /// <summary>
    /// 发送消息
    /// </summary>
    public static void ReqChatMessage(int channel, string _message, long _sendTo, int type = 2, string playerName = "", int _quickchatid = 0)
    {
        if (channel == (int)ChatType.CT_COLORED)
        {
            CSChatManager.Instance.ColoredTime = CSServerTime.Instance.TotalSeconds;
        }
        chat.ChatRequest msg = CSProtoManager.Get<chat.ChatRequest>();
        msg.channel = channel;
        msg.message = _message;
        msg.sendToName = channel == (int)ChatType.CT_PRIVATE ? playerName : "";
        msg.sendTo = channel == (int)ChatType.CT_PRIVATE ? _sendTo.ToString() : "0";
        msg.type = type;
        msg.quickChatId = _quickchatid;
        msg.showVipLevel = CSConfigInfo.Instance.GetBool(ConfigOption.MakeVipLevelVisible) ? 0 : 1;
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqChatMessage, msg);
    }

    //语音召集请求
    public static void ChatCallReleaseReqMessage(int type)
    {
        FNDebug.LogFormat("<color=#00ff00>[语音聊天]:发送请求行会召集令</color>");
        chat.ReleaseReq msg = CSProtoManager.Get<chat.ReleaseReq>();
        msg.type = type;
        CSHotNetWork.Instance.SendMsg((int)ECM.ReleaseReqMessage, msg);
    }

    //语音房间登录请求
    public static void ChatJoinVoiceRoomReqMessage(int loginType)
    {
        chat.JoinOrLeaveVoiceRoomReq msg = CSProtoManager.Get<chat.JoinOrLeaveVoiceRoomReq>();
        int channelId = CSChatManager.Instance.GetVoiceChatChannel((VoiceLoginType)loginType);
        if (channelId != -1)
        {
            msg.rid = channelId;
            FNDebug.LogFormat("<color=#00ff00>[语音聊天]:发送加入语音房间请求:[channelId:{0}]</color>",channelId);
            CSHotNetWork.Instance.SendMsg((int)ECM.JoinVoiceRoomReqMessage, msg);
        }
    }

    //离开房间
    public static void ChatLeaveVoiceRoomReqMessage(int loginType)
    {
        chat.JoinOrLeaveVoiceRoomReq msg = CSProtoManager.Get<chat.JoinOrLeaveVoiceRoomReq>();
        int channelId = CSChatManager.Instance.GetVoiceChatChannel((VoiceLoginType)loginType);
        if (channelId != -1)
        {
            msg.rid = channelId;
            FNDebug.LogFormat("<color=#00ff00>[语音聊天]:发送离开语音房间请求:[channelId:{0}]</color>", channelId);
            CSHotNetWork.Instance.SendMsg((int)ECM.LeaveVoiceRoomReqMessage, msg);
        }
    }

    //请求语音频道人数
    public static void ChatVoiceRoomNumReqMessage(int loginType)
    {
        chat.JoinOrLeaveVoiceRoomReq msg = CSProtoManager.Get<chat.JoinOrLeaveVoiceRoomReq>();
        int channelId = CSChatManager.Instance.GetVoiceChatChannel((VoiceLoginType)loginType);
        if (channelId != -1)
        {
            msg.rid = channelId;
            FNDebug.LogFormat("<color=#00ff00>[语音聊天]:拉取语音在线人数 VoiceRoomNumReqMessage</color>");
            CSHotNetWork.Instance.SendMsg((int)ECM.VoiceRoomNumReqMessage, msg);
        }
    }

    //请求上麦人员信息
    public static void ChatGetUpMicrPlayer(List<long> roleId)
    {
        chat.RoleDetailReq msg = CSProtoManager.Get<chat.RoleDetailReq>();
        for (int i = 0; i < roleId.Count; i++)
        {
            msg.roleId.Add(roleId[i]);
        }
        if (msg.roleId.Count > 0)
        {
            CSHotNetWork.Instance.SendMsg((int)ECM.RoleDetailReqMessage, msg);
        }
    }
	public static void InformReqMessage(Int64 roleId)
	{
		chat.InformReq req = CSProtoManager.Get<chat.InformReq>();
		req.roleId = roleId;
		CSHotNetWork.Instance.SendMsg((int)ECM.InformReqMessage,req);
	}
	public static void BigExpressionReqMessage(String content)
	{
		chat.BigExpressionReq req = CSProtoManager.Get<chat.BigExpressionReq>();
		req.content = content;
		CSHotNetWork.Instance.SendMsg((int)ECM.BigExpressionReqMessage,req);
	}
}