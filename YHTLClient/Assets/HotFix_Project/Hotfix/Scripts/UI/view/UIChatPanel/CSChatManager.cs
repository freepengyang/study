using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSChatManager : CSInfo<CSChatManager>
{
    protected Dictionary<long,FixedCircleArray<ChatData>> mPrivateChats = new Dictionary<long,FixedCircleArray<ChatData>>(32);
    protected FixedCircleArray<ChatData>[] mChannelDatas = new FixedCircleArray<ChatData>[(int)ChatType.CT_COUNT];
    protected Stack<ChatData> mPoolDatas = new Stack<ChatData>(1024);
    protected float mLastColoredTime = 0;
    public float ColoredTime
    {
        get
        {
            return mLastClickTime;
        }
        set
        {
            mLastClickTime = value;
        }
    }
    protected float mLastClickTime = 0;
    protected const float CONST_CHAT_INTERVAL = 5.0F;
    protected const int CONST_CHAT_MAX_WORDS_LENGTH = 100;
    protected int mChatGuid = 0;
    protected const int CONST_MAX_CACHED = 256;
    protected const int CONST_MAX_GENERAL_CACHED = 50;

    protected Dictionary<ChatType, bool> mLastUrlDic = new Dictionary<ChatType, bool>((int)ChatType.CT_ALL_COUNT);
    protected Dictionary<ChatType, List<LinkBase>> mRelinkDics = new Dictionary<ChatType, List<LinkBase>>((int)ChatType.CT_ALL_COUNT);
    public LocationLink AddLocation(ChatType chatType, TABLE.MAPINFO mapInfo, int x,int y)
    {
        LocationLink linkItem = new LocationLink
        {
            MapInfo = mapInfo,
            CoordX = x,
            CoordY = y,
        };
        List<LinkBase> pool = null;
        if (!mRelinkDics.ContainsKey(chatType))
        {
            pool = new List<LinkBase>(8);
            mRelinkDics.Add(chatType, pool);
        }
        else
        {
            pool = mRelinkDics[chatType];
        }
        pool.Add(linkItem);
        return linkItem;
    }

    public void SetLastUrl(ChatType channel,bool value)
    {
        if(mLastUrlDic.ContainsKey(channel))
        {
            mLastUrlDic[channel] = value;
        }
        else
        {
            mLastUrlDic.Add(channel, value);
        }
    }

    public bool GetLastUrl(ChatType channel)
    {
        return mLastUrlDic.ContainsKey(channel) && mLastUrlDic[channel];
    }

    public bool TryOpenColoursWorldPanel()
    {
        TABLE.SUNDRY sundry;
        if (SundryTableManager.Instance.TryGetValue(299, out sundry))
        {
            int limitLv = 0;
            int.TryParse(sundry.effect, out limitLv);
            if (CSMainPlayerInfo.Instance.VipLevel >= limitLv)
            {
                UIManager.Instance.CreatePanel<UIColoursWorldPanel>();
                return true;
            }
            else
            {
                string str = null;
                str = CSString.Format(391, sundry.effect);
                UtilityTips.ShowRedTips(str);
                return false;
            }
        }
        return true;
    }

    public bool IsColoredWorldMsgCoolDown(bool needMsg = true)
    {
        TABLE.SUNDRY sundry;
        if (SundryTableManager.Instance.TryGetValue(298, out sundry))
        {
            int deltime;
            if (int.TryParse(sundry.effect, out deltime))
            {
                if (CSServerTime.Instance.TotalSeconds - ColoredTime < deltime * 0.001)
                {
                    if(needMsg)
                        UtilityTips.ShowRedTips(386);
                    return false;
                }
            }
        }
        return false;
    }

    public EmotionLink AddEmotion(ChatType chatType,string emotion)
    {
        EmotionLink linkItem = new EmotionLink
        {
            Emotion = emotion
        };
        List<LinkBase> pool = null;
        if (!mRelinkDics.ContainsKey(chatType))
        {
            pool = new List<LinkBase>(8);
            mRelinkDics.Add(chatType, pool);
        }
        else
        {
            pool = mRelinkDics[chatType];
        }
        pool.Add(linkItem);
        return linkItem;
    }

    public ItemLink AddItem(ChatType chatType,TABLE.ITEM item,bag.BagItemInfo itemInfo)
    {
        bool normalEquip = CSBagInfo.Instance.IsNormalEquip(item);
        int quality = normalEquip ? itemInfo.quality : item.quality;
        ItemLink linkItem = new ItemLink
        {
            Item = item,
            quality = quality,
            bytes = Utility.GetStructString(itemInfo),
        };
        List<LinkBase> pool = null;
        if (!mRelinkDics.ContainsKey(chatType))
        {
            pool = new List<LinkBase>(8);
            mRelinkDics.Add(chatType, pool);
        }
        else
        {
            pool = mRelinkDics[chatType];
        }
        pool.Add(linkItem);
        return linkItem;
    }

    public CSChatManager()
    {
        for(int i = (int)ChatType.CT_COMPREHENSIVE; i < (int)ChatType.CT_COUNT; ++i)
        {
            if(i == (int)ChatType.CT_COMPREHENSIVE)
            {
                mChannelDatas[i] = new FixedCircleArray<ChatData>(CONST_MAX_CACHED);
            }
            else
            {
                mChannelDatas[i] = new FixedCircleArray<ChatData>(CONST_MAX_GENERAL_CACHED);
            }
        }
        HotManager.Instance.EventHandler.AddEvent(CEvent.OnMainPlayerTeamIdChanged, OnTeamIdChanged);
        HotManager.Instance.EventHandler.AddEvent(CEvent.OnMainPlayerGuildIdChanged, OnGuildIdChanged);
        HotManager.Instance.EventHandler.AddEvent(CEvent.Scene_ChangeMap, OnSceneChanged);
        HotManager.Instance.EventHandler.AddEvent(CEvent.Scene_Change, OnSceneChanged);
        Initialize();
    }

    protected void OnTeamIdChanged(uint uiEvtID, object data)
    {
        if(!Utility.HasTeam())
        {
            ClearChannelInfo(ChatType.CT_TEAM);
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnChatChannelMessageCleared, ChatType.CT_TEAM);
        }
    }

    protected void OnGuildIdChanged(uint uiEvtID, object data)
    {
        if (!Utility.HasGuild())
        {
            ClearChannelInfo(ChatType.CT_GUILD);
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnChatChannelMessageCleared, ChatType.CT_GUILD);
        }
    }

    protected void OnSceneChanged(uint uiEvtID, object data)
    {
        ClearChannelInfo(ChatType.CT_NEIGHBOURING);
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnChatChannelMessageCleared, ChatType.CT_NEIGHBOURING);
    }

    public FixedCircleArray<ChatData> GetChannelDatas(ChatType eChatType)
    {
        int idx = (int)eChatType;
        idx = Mathf.Clamp(idx, 0, mChannelDatas.Length - 1);
        return mChannelDatas[idx];
    }

    public string GetChanelName(ChatType chatType)
    {
        int id = (int)chatType + 201;
        var content = ClientTipsTableManager.Instance.GetClientTipsContext(id);
        return content;
    }

    public bool IsChannelOpened(ChatType chatType)
    {
        return true;
    }

    public void AddPrivateChatMasg(chat.ChatMessage chatMsg)
    {
        bool IsSelf = CSMainPlayerInfo.Instance.ID == chatMsg.sender ? true : false;
        long guid = IsSelf ? chatMsg.sendToID : chatMsg.sender;
        string name = IsSelf ? chatMsg.sendTo : chatMsg.name;
        var nationId = IsSelf ? chatMsg.sendToNation : chatMsg.nationId;

        //陌生人聊天屏蔽
        //1 开启了不接受陌生人消息
        //2 不是自己的消息
        //3 私聊列表中没有这个玩家
        var chatFriend = CSFriendInfo.Instance.GetPrivateChatFriend(chatMsg.sender);
        if(!CSFriendInfo.Instance.AcceptStrangerMsg && !IsSelf && null == chatFriend)
        {
            FNDebug.LogFormat("<color=#00ff00>[陌生人消息已经被屏蔽]:{0}</color>", chatMsg.name);
            return;
        }

        //加入聊天信息
        FixedCircleArray<ChatData> channelBuf = null;
        if(!mPrivateChats.ContainsKey(guid))
        {
            channelBuf = new FixedCircleArray<ChatData>(128);
            mPrivateChats.Add(guid, channelBuf);
        }
        else
        {
            channelBuf = mPrivateChats[guid];
        }

        if (channelBuf.IsFull)
        {
            var headData = channelBuf[0];
            if (mPoolDatas.Count < CONST_MAX_CACHED)
            {
                headData.Reset();
                mPoolDatas.Push(headData);
            }
            //将头部元素置空
            channelBuf.SetElementAt(0, null);
        }
        var chatData = mPoolDatas.Count > 0 ? mPoolDatas.Pop() : new ChatData();
        chatData.coloredMsg = false;
        chatData.guid = ++mChatGuid;
        chatData.msg = chatMsg;
        chatData.LinkChannel(ChatType.CT_PRIVATE);
        channelBuf.Append(chatData);
        CSFriendInfo.Instance.AddPrivateChat(guid, name,chatMsg.career,chatMsg.level,chatMsg.sex,true,IsSelf);
        HotManager.Instance.EventHandler.SendEvent(CEvent.PrivateChatMessage, chatData);
    }

    public void RemovePrivateChatDatas(long roleId)
    {
        mPrivateChats.Remove(roleId);
    }

    FixedCircleArray<ChatData> mShadowDatas = new FixedCircleArray<ChatData>(8);
    public FixedCircleArray<ChatData> GetPrivateChatDatas(long roleId)
    {
        FixedCircleArray<ChatData> ret = null;
        if(mPrivateChats.ContainsKey(roleId))
        {
            ret = mPrivateChats[roleId];
        }
        if (null == ret)
            return mShadowDatas;
        return ret;
    }

    protected void ClearChannelInfo(ChatType chatType)
    {
        //综合频道聊天信息不可被清楚
        int channel = (int)chatType;
        if(channel <= 0 || channel >= mChannelDatas.Length || null == mChannelDatas[channel])
        {
            return;
        }
        var channelBuf = mChannelDatas[channel];
        for(int i = 0; i < channelBuf.Count; ++i)
        {
            var chatData = channelBuf[i];
            chatData.UnLinkChannel(chatType);
            if (!chatData.used && mPoolDatas.Count < CONST_MAX_CACHED)
            {
                mPoolDatas.Push(chatData);
            }
        }
        channelBuf.Clear();
    }

    //显示主页面彩世信息
    private void ShowColoursWorldMsg(chat.ChatMessage s_data)
    {
        string msg = "";
        if (s_data.type == 1)
        {
            string[] msgs = s_data.message.Split('$');
            if (msgs.Length >= 5)
            {
                msg = msgs[4];
            }
        }
        else
        {
            msg = s_data.message;
        }
        CSStringBuilder.Clear();
        CSStringBuilder.Append(s_data.name, "#", msg);

        //TODO:CHAT 主界面显示主页面彩世信息
        //Tip.BulletinResponse message = new Tip.BulletinResponse();
        //message.count = 1;
        //message.msg = CSStringBuilder.ToString();
        //message.display = 10;
        //CSNoticeManager.Instance.NoticeEnqueue((NoticeType)message.display, message);
    }

    public void ForbidChat(long roleId)
    {
        for (int i = 0; i < (int)ChatType.CT_COUNT; ++i)
        {
            var channel = (ChatType)i;
            var channelDatas = mChannelDatas[i];
            int k = 0;//valid idx
            for (int j = 0; j < channelDatas.Count; ++j)
            {
                var chatData = channelDatas[j];
                if (null != chatData && chatData.msg.sender == roleId)
                {
                    chatData.UnLinkChannel(channel);
                    if (!chatData.used && mPoolDatas.Count < CONST_MAX_CACHED)
                    {
                        mPoolDatas.Push(chatData);
                    }
                    channelDatas.SetElementAt(j, null);
                }
                else
                {
                    channelDatas.SetElementAt(k,chatData);
                    ++k;
                }
            }
            channelDatas.RemoveElementFromTail(channelDatas.Count - k);
        }
    }

    public void AddChatMsg(chat.ChatMessage chatMsg)
    {
        if(null == chatMsg || CSFriendInfo.Instance.IsPlayerInBlackList(chatMsg.sender))
        {
            return;
        }

        int channel = chatMsg.channel;

        if(channel == (int)ChatType.CT_COMPREHENSIVE)
        {
            FNDebug.LogErrorFormat("can not add msg which channel is CT_COMPREHENSIVE");
            return;
        }

        if(channel == (int)ChatType.CT_PRIVATE)
        {
            AddPrivateChatMasg(chatMsg);
            return;
        }

        bool coloredMsg = channel == (int)ChatType.CT_COLORED;
        if(coloredMsg)
        {
            //在主界面弹出彩世界信息
            ShowColoursWorldMsg(chatMsg);
            //修改频道入世界频道
            channel = (int)ChatType.CT_WORLD;
            //修改消息层结构体数据
            chatMsg.channel = channel;
        }

        if (!(channel >= 0 && channel < mChannelDatas.Length))
        {
            FNDebug.LogErrorFormat("channel error = {0}", channel);
            return;
        }

        var comprehensiveChannelBuf = mChannelDatas[(int)ChatType.CT_COMPREHENSIVE];
        if (comprehensiveChannelBuf.IsFull)
        {
            var headData = comprehensiveChannelBuf[0];
            headData.UnLinkChannel(ChatType.CT_COMPREHENSIVE);
            if(!headData.used && mPoolDatas.Count < CONST_MAX_CACHED)
            {
                headData.Reset();
                //回收这块内存
                mPoolDatas.Push(headData);
            }
            //将头部元素置空
            comprehensiveChannelBuf.SetElementAt(0, null);
        }

        //从池中获取一块内存
        var chatData = comprehensiveChannelBuf.Append(mPoolDatas.Count > 0 ? mPoolDatas.Pop() : new ChatData());
        chatData.LinkChannel(ChatType.CT_COMPREHENSIVE);
        chatData.coloredMsg = coloredMsg;
        chatData.guid = ++mChatGuid;
        chatData.msg = chatMsg;
        //chatData.msg.sendTo = chatMsg.sendTo;
        //chatData.msg.career = chatMsg.career;
        //chatData.msg.channel = chatMsg.channel;
        //chatData.msg.message = chatMsg.message;
        //chatData.msg.name = chatMsg.name;
        //chatData.msg.vip = chatMsg.vip;
        //chatData.msg.sex = chatMsg.sex;
        //chatData.msg.type = chatMsg.type;
        //chatData.msg.unionPostionName = chatMsg.unionPostionName;
        //chatData.msg.showType = chatMsg.showType;
        //chatData.msg.isBaby = chatMsg.isBaby;
        //chatData.msg.sender = chatMsg.sender;

        var channelBuf = mChannelDatas[channel];
        if(channelBuf.IsFull)
        {
            var headData = channelBuf[0];
            headData.UnLinkChannel((ChatType)channel);
            if (!headData.used && mPoolDatas.Count < CONST_MAX_CACHED)
            {
                mPoolDatas.Push(headData);
            }
            //将头部元素置空
            channelBuf.SetElementAt(0, null);
        }
        channelBuf.Append(chatData);
        chatData.LinkChannel((ChatType)channel);

        HotManager.Instance.EventHandler.SendEvent(CEvent.OnRecievedNewChatMessage, chatData);
    }

    //public bool DebugChatMsg(UIInput input, int channel, ChatChannelType channelType = ChatChannelType.ChatPanel, string sendToName = "", long sendToId = 0, string content = "")
    //{
    //    var text = input.value;

    //    if (true)
    //    {
    //        List<LinkBase> gLink;
    //        if (mRelinkDics.ContainsKey((ChatType)channel))
    //        {
    //            gLink = mRelinkDics[(ChatType)channel];
    //            for (int i = 0; i < gLink.Count; ++i)
    //            {
    //                text = gLink[i].Replace(text);
    //            }
    //            gLink.Clear();
    //        }
    //    }

    //    chat.ChatMessage msg = new chat.ChatMessage();
    //    msg.channel = (int)ChatType.CT_PRIVATE;
    //    msg.isMonthCard = false;
    //    msg.level = 70;
    //    msg.message = input.value;
    //    msg.sender = CSMainPlayerInfo.Instance.ID + 1465646;
    //    msg.name = CSMainPlayerInfo.Instance.Name + "_Other";
    //    msg.sex = 1;
    //    msg.type = 0;
    //    msg.vip = (int)Random.Range(0, 1);
    //    msg.isBaby = true;
    //    msg.nationPostionName = "队长";
    //    msg.unionPostionName = "成员";
    //    CSChatManager.Instance.AddChatMsg(msg);

    //    msg = new chat.ChatMessage();
    //    msg.channel = (int)ChatType.CT_PRIVATE;
    //    msg.isMonthCard = false;
    //    msg.level = 70;
    //    msg.message = input.value;
    //    msg.sender = CSMainPlayerInfo.Instance.ID;
    //    msg.name = CSMainPlayerInfo.Instance.Name + "_ME";
    //    msg.sex = 1;
    //    msg.type = 0;
    //    msg.vip = (int)Random.Range(0, 1);
    //    msg.isBaby = true;
    //    msg.nationPostionName = "队长";
    //    msg.unionPostionName = "成员";
    //    CSChatManager.Instance.AddChatMsg(msg);
    //    //CSChatManager.Instance.

    //    //input.value = string.Empty;
    //    SetLastUrl((ChatType)channel, false);

    //    return false;
    //}

    System.Text.RegularExpressions.Regex regexCommand = new System.Text.RegularExpressions.Regex("@(\\w+) (\\d+)");
    bool GMCommand(string value)
    {
        
        if (regexCommand.IsMatch(value))
        {
            var match = regexCommand.Match(value);
            switch (match.Groups[1].Value)
            {
                case "guide":
                    int guideId = 0;
                    TABLE.GUIDEGROUP guideItem;
                    if (int.TryParse(match.Groups[2].Value, out guideId) && GuideGroupTableManager.Instance.TryGetValue(guideId, out guideItem))
                    {
                        CSGuideManager.Instance.Trigger(guideId);
                    }
                    return true;
                case "funcopen":
                    int functionId = 0;
                    TABLE.FUNCOPEN funcItem;
                    if (int.TryParse(match.Groups[2].Value, out functionId) && FuncOpenTableManager.Instance.TryGetValue(functionId, out funcItem))
                    {
                        CSNewFunctionUnlockManager.Instance.TestTrigger(functionId);
                    }
                    return true;
            }
        }
        return false;
    }

    private bool IsGMPanel(string text)
    {
        if (text.Equals("@NBGM true"))
        {
            if(QuDaoConstant.isEditorMode() || HttpRequest.Instance.IsAdminUser(Constant.mWhiteListIp))
            {
                UIManager.Instance.CreatePanel<UIGMMenuPanel>();
            }
            return true;
        }
        if(text.Equals("@HideUIScene"))
        {
            HotManager.Instance.EventHandler.SendEvent(CEvent.MoveUIMainScenePanel, true);
            return true;
        }

        return false;
    }

    public void AddVoiceLink(VoiceLoginType voiceLoginType)
    {
        if(voiceLoginType == VoiceLoginType.union)
        {
            string content = CSString.Format(1856, (int)voiceLoginType);
            CSChatManager.Instance.SendChatMsg(input: null, channel: (int)ChatType.CT_GUILD, content: content);
        }
        else if(voiceLoginType == VoiceLoginType.team)
        {
            string content = CSString.Format(1856, (int)voiceLoginType);
            CSChatManager.Instance.SendChatMsg(input: null, channel: (int)ChatType.CT_TEAM, content: content);
        }
    }
    
    public void SendChatMsg(UIInput input, int channel, ChatChannelType channelType = ChatChannelType.ChatPanel, string sendToName = "", long sendToId = 0,string content = "")
    {
#if UNITY_EDITOR
        if(null != input && GMCommand(input.value))
        {
            input.value = string.Empty;
            return;
        }
#endif
        if(null != input && IsGMPanel(input.value))
        {
            input.value = string.Empty;
            return;
        }
        //var g = $"[url=func:5:team:{CSTeamInfo.Instance.TeamId}]{arrText[0]}[u]{arrText[1]}[/u][/url]";
        //DebugChatMsg(input, channel, ChatChannelType.ChatPanel, sendToName, 0);
        //DebugChatMsg(input, channel, ChatChannelType.ChatPanel, sendToName, 0);
        //DebugChatMsg(input, channel, ChatChannelType.ChatPanel, sendToName, 0);
        //DebugChatMsg(input, channel, ChatChannelType.ChatPanel, sendToName, 0);
        //DebugChatMsg(input, channel, ChatChannelType.ChatPanel, sendToName, 0);
        //DebugChatMsg(input, channel, ChatChannelType.ChatPanel, sendToName, 0);
        //DebugChatMsg(input, channel, ChatChannelType.ChatPanel, sendToName, 0);
        //DebugChatMsg(input, channel, ChatChannelType.ChatPanel, sendToName, 0);
        //DebugChatMsg(input, channel, ChatChannelType.ChatPanel, sendToName, 0);
        //DebugChatMsg(input, channel, ChatChannelType.ChatPanel, sendToName, 0);
        //return;
        if (channel == (int)ChatType.CT_COMPREHENSIVE)
        {
            UtilityTips.ShowTips(326, 1.5f, ColorType.Red);
            return;
        }

        if (channel == (int)ChatType.CT_SYSTEM)
        {
            UtilityTips.ShowTips(327, 1.5f, ColorType.Red);
            return;
        }

        if(channel == (int)ChatType.CT_TEAM)
        {
            if(!Utility.HasTeam())
            {
                UtilityTips.ShowRedTips(318);
                return;
            }
        }

        if (channel == (int)ChatType.CT_GUILD)
        {
            if (!Utility.HasGuild())
            {
                UtilityTips.ShowRedTips(317);
                return;
            }
        }

        int levelLmt = GetChatLimit(channel);
        if(levelLmt < 0)
        {
            UtilityTips.ShowTips(316, 1.5f, ColorType.Red);
            return;
        }

        //如果等级不足
        if(levelLmt > 0 && (CSMainPlayerInfo.Instance.Level < levelLmt))
        {
            int vipLimit = GetChatVipLevelLimit();
            if(vipLimit < 0)//如果系统没有开放vip特权
            {
                UtilityTips.ShowTips(CSString.Format(330,levelLmt), 1.5f, ColorType.Red);//聊天等级不足
                return;
            }
            //如果系统开放了vip特权而且自身VIP等级未达到要求
            if (CSMainPlayerInfo.Instance.VipLevel < vipLimit)
            {
                UtilityTips.ShowTips(CSString.Format(331, levelLmt,vipLimit), 1.5f, ColorType.Red);//聊天等级不足或者vip等级不足
                return;
            }
        }

        if (string.IsNullOrEmpty(content) && (null == input || string.IsNullOrEmpty(input.value)))
        {
            UtilityTips.ShowTips(301, 1.5f, ColorType.Red);
            return;
        }

        if(string.IsNullOrEmpty(content) && null != input)
            content = input.value;

        if (channel == (int)ChatType.CT_PRIVATE && !string.IsNullOrEmpty(sendToName))
        {
            if (CSFriendInfo.Instance.IsPlayerInBlackList(sendToId))
            {
                UtilityTips.ShowRedTips(633);
                return;
            }
        }

        if (Time.time - mLastClickTime <= CONST_CHAT_INTERVAL)
        {
            float passedTime = (Time.time - mLastClickTime);
            passedTime = Mathf.Max(0, passedTime);
            int leftTime = (int)Mathf.Max(1, CONST_CHAT_INTERVAL - passedTime);
            UtilityTips.ShowTips(CSString.Format(303, leftTime), 1.5f, ColorType.Red);
            return;
        }
        mLastClickTime = Time.time;

        if (NGUIText.StripSymbols(content).Length > CONST_CHAT_MAX_WORDS_LENGTH)
        {
            UtilityTips.ShowRedTips(304, 1.5f, ColorType.Red);
            return;
        }

        List<LinkBase> links;
        if (mRelinkDics.ContainsKey((ChatType)channel))
        {
            links = mRelinkDics[(ChatType)channel];
            for (int i = 0; i < links.Count; ++i)
            {
                content = links[i].Replace(content);
            }
            links.Clear();
        }

        if(null != input)
            input.value = string.Empty;
        SetLastUrl((ChatType)channel, false);

        Net.ReqChatMessage(channel, content, sendToId, 2, sendToName);
    }

    public void Initialize()
    {
        mLastColoredTime = 0;
        mLastClickTime = 0;
        LoadChatLevelLimit();
    }

    protected int[] mChatLevelLimits = new int[((int)ChatType.CT_ALL_COUNT)];
    protected void LoadChatLevelLimit()
    {
        for(int i = 0; i < mChatLevelLimits.Length; ++i)
        {
            mChatLevelLimits[i] = 0;
        }

        int sundryId = 295;
        TABLE.SUNDRY sundry = null;
        if(SundryTableManager.Instance.TryGetValue(sundryId,out sundry))
        {
            var tokens = sundry.effect.Split('&');
            for(int i = 0; i < tokens.Length; ++i)
            {
                var channelAndLmt = tokens[i].Split('#');
                int channel = 0;
                int level = 0;
                if(channelAndLmt.Length == 2 && int.TryParse(channelAndLmt[0],out channel) && int.TryParse(channelAndLmt[1],out level))
                {
                    if(channel >= 0 && channel < mChatLevelLimits.Length)
                    {
                        mChatLevelLimits[channel] = level;
                    }
                }
            }
        }
    }

    public int GetGuildVoiceGuildLevelLimit()
    {
        int suntryId = 297;
        TABLE.SUNDRY table;
        int level = 0;
        if (SundryTableManager.Instance.TryGetValue(suntryId, out table) && int.TryParse(table.effect, out level))
        {
            return level;
        }
        return 0;
    }

    public int GetChatLimit(int channel)
    {
        if(channel < 0 || channel >= (int)ChatType.CT_ALL_COUNT)
        {
            UtilityTips.ShowRedTips(316);
            return -1;
        }

        return mChatLevelLimits[channel];
    }

    public int GetVoiceChatChannel(VoiceLoginType voiceLoginType)
    {
        int channelId = -1;
        switch (voiceLoginType)
        {
            case VoiceLoginType.union:
                channelId = (int)ChatType.CT_GUILD;
                break;
            case VoiceLoginType.team:
                channelId = (int)ChatType.CT_TEAM;
                break;
            default:
                break;
        }
        return channelId;
    }

    public int GetChatVipLevelLimit()
    {
        int sundryId = 296;
        TABLE.SUNDRY sundry = null;
        int vipLevel = 0;
        if (!SundryTableManager.Instance.TryGetValue(sundryId, out sundry) || !int.TryParse(sundry.effect,out vipLevel))
        {
            //无VIP等级特权
            return -1;
        }

        return vipLevel;
    }

    public bool IsVoiceOpen(ChatType chatType)
    {
        return chatType == ChatType.CT_TEAM || chatType == ChatType.CT_GUILD;
    }
    
    public override void Dispose()
    {
        mLastUrlDic.Clear();
        mRelinkDics.Clear();
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.OnMainPlayerTeamIdChanged,OnTeamIdChanged);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.OnMainPlayerGuildIdChanged,OnGuildIdChanged);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.Scene_ChangeMap, OnSceneChanged);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.Scene_Change, OnSceneChanged);
    }
}

public class ChatData
{
    public enum ChatDataType
    {
        CT_NORMAL = 0,
        CT_COLORED = 1,//彩世
    }
    public bool used
    {
        get
        {
            return channelFlag != 0;
        }
    }
    private int channelFlag;
    public int guid;
    public chat.ChatMessage msg;// = new chat.ChatMessage();
    //public bool left;
    public bool coloredMsg { get; set; }

    public ChatType Channel
    {
        get
        {
            if (null != msg)
                return (ChatType)msg.channel;
            return ChatType.CT_COMPREHENSIVE;
        }
    }

    public ChatData()
    {
        Reset();
    }

    public void Reset()
    {
        coloredMsg = false;
        //left = true;
        channelFlag = 0;
        guid = 0;
        msg = null;
    }

    public void LinkChannel(ChatType eChannel)
    {
        int flag = (1 << (int)eChannel);
        if(flag == (channelFlag & flag))
        {
            FNDebug.LogError("channel has linked, there is something wrong with your logic ....");
            return;
        }
        channelFlag |= flag;
    }

    public void UnLinkChannel(ChatType eChannel)
    {
        int flag = (1 << (int)eChannel);
        if(0 == (channelFlag & flag))
        {
            FNDebug.LogError("unlinked a unused channel, there is something wrong with your logic ....");
            return;
        }
        channelFlag &= ~flag;
    }

    public bool IsLinkedChannedl(ChatType eChannel)
    {
        int flag = (1 << (int)eChannel);
        return (channelFlag & flag) > 0;
    }
}

//public enum ChatChannelType
//{
//    None,
//    ChatPanel,
//    Friend,
//}