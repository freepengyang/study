using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChatHelper
{
    public static void SteupChannel(this UISprite sprite,ChatData chatData)
    {
        if(null != sprite && null != chatData)
        {
            sprite.spriteName = $"chattype{(int)chatData.Channel}";
        }
    }

    public static void SetupLink(this UILabel label,System.Action actionFinished = null, System.Action actionStart = null)
    {
        if (null != label)
        {
            var boxColider = label.gameObject.GetComponent<BoxCollider>();
            if(null == boxColider)
            {
                boxColider = label.gameObject.AddComponent<BoxCollider>();
            }
            if (null != boxColider)
            {
                boxColider.size = label.localSize;
            }
            UIEventListener.Get(label.gameObject).onClick = f =>
            {
                var text = label.GetUrlAtPosition(UICamera.lastWorldPosition);
                if (text != null)
                {
                    actionStart?.Invoke();
                    CSSuperLink.Instance.Link(text);
                    actionFinished?.Invoke();
                }
               
            };
        }
    }

    public static string GetDuty(this ChatData chatData)
    {
        if(null == chatData || null == chatData.msg)
        {
            return string.Empty;
        }

        return chatData.msg.unionPostionName;
    }

    public static string GetTeamDuty(this ChatData chatData)
    {
        return chatData.msg.isBaby ? CSString.Format(332) : string.Empty;
    }

    public static string GetName(this ChatData chatData)
    {
        if(null == chatData || null == chatData.msg || chatData.msg.channel == (int)ChatType.CT_SYSTEM || chatData.msg.sender == 0 || string.IsNullOrEmpty(chatData.msg.name))
        {
            return string.Empty;
        }

        if (chatData.msg.sender == CSMainPlayerInfo.Instance.ID)
        {
            return CSString.Format(328);
        }

        var channel = chatData.Channel;
        CSStringBuilder.Clear();
        string positionStr = string.Empty;
        if (channel == ChatType.CT_GUILD)
        {
            string position = GetDuty(chatData);
            position = NGUIText.StripSymbols(position);
            if(position.Equals(CSString.Format(1729)))
            {
                position = string.Empty;
            }
            if (!string.IsNullOrEmpty(position))
                positionStr = CSString.Format(396, position);
        }
        else if (channel == ChatType.CT_TEAM)
        {
            string position = GetTeamDuty(chatData);
            if (!string.IsNullOrEmpty(position))
                positionStr = CSString.Format(397, position);
        }

        var relation = CSFriendInfo.Instance.GetRelation(chatData.msg.sender);
        //([仇])|([帮会职业]|[队伍职业])|玩家名称
        string playerName = string.Empty;
        if(relation == FriendType.FT_ENEMY)
        {
            playerName = CSString.Format(635, positionStr, chatData.msg.name);
        }
        else
        {
            playerName = CSString.Format(395, positionStr, chatData.msg.name);
        }
        return $"[url=func:{(int)CSLinkFunction.CSLF_MAIN_ROLE_LINK}:role:{chatData.msg.sender}:{chatData.msg.name}:{0}:{0}:{chatData.msg.sex}:{0}:{0}:{chatData.msg.level}:{chatData.msg.nationId}:{chatData.msg.career}]{playerName}[/url]";
    }

    public static bool IsVoiceMessage(this ChatData chatData)
    {
        if(null != chatData && null != chatData.msg && chatData.msg.type == 1)
        {
            return true;
        }
        return false;
    }

    public static bool IsSystemMessage(this ChatData chatData)
    {
        if(null != chatData && null != chatData.msg)
        {
            if(chatData.Channel == ChatType.CT_SYSTEM || chatData.msg.sender == 0 || string.IsNullOrEmpty(chatData.msg.name))
            {
                return true;
            }
        }

        return false;
    }
}