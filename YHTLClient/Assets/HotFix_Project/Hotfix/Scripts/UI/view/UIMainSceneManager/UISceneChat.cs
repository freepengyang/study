using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UISceneChat : UIBasePanel
{
    UnityEngine.GameObject mGrid;
    UnityEngine.GameObject mTemplate;
    UIEventListener mBtnChat;
    TweenAlpha mTweenAlpha;
    protected override void _InitScriptBinder()
    {
        mGrid = ScriptBinder.GetObject("Grid") as UnityEngine.GameObject;
        mTemplate = ScriptBinder.GetObject("Template") as UnityEngine.GameObject;
        mBtnChat = ScriptBinder.GetObject("btn_chat") as UIEventListener;
        mTweenAlpha = ScriptBinder.GetObject("TweenAlpha") as TweenAlpha;
    }

    UIChatBaseListViewBinder mChatListViewComponent = new UIChatBaseListViewBinder();
    protected FastArrayElementKeepHandle<ChatData> mComprehensiveDatas = new FastArrayElementKeepHandle<ChatData>(128);

    void OnOpenUIChatPanel(GameObject go)
    {
        HotManager.Instance.EventHandler.SendEvent(CEvent.ShowChatPanel);
    }

    bool _open = false;
    bool MakeChatOpen
    {
        get
        {
            return _open;
        }
        set
        {
            _open = value;
            PlayAnimation(value);
        }
    }

    void OnTweenFinish()
    {
        //mBtnChat.CustomActive(!_open);
    }

    void PlayAnimation(bool open)
    {
        EventDelegate.Add(mTweenAlpha.onFinished,OnTweenFinish);
        if (open)
            mTweenAlpha.PlayForward();
        else
            mTweenAlpha.PlayReverse();
    }

    void FadeOutChatPanel()
    {
        MakeChatOpen = false;
    }

    int chatFadeOutTime = 8;

    public override void Init()
    {
        base.Init();

        int sundryId = 675;
        TABLE.SUNDRY sundryItem = null;
        if(SundryTableManager.Instance.TryGetValue(sundryId,out sundryItem))
        {
            int.TryParse(sundryItem.effect, out chatFadeOutTime);
        }
        FNDebug.LogFormat("<color=#00ff00>[聊天界面事件]:{0}</color>",chatFadeOutTime);
        mBtnChat.onClick = OnOpenUIChatPanel;

        var listener = UIEventListener.Get(mGrid);
        mChatListViewComponent.Setup(listener);

        mClientEvent.AddEvent(CEvent.OnRecievedNewChatMessage, OnReceiveChatMessage);
        mClientEvent.AddEvent(CEvent.OnChatSettingChanged, OnChatSettingChanged);
    }

    UIChatBaseListViewData mChatListViewData = new UIChatBaseListViewData
    {
        eChatType = ChatType.CT_COMPREHENSIVE,
    };

    protected void OnReceiveChatMessage(uint id,object argv)
    {
        if(argv is ChatData chatData && chatData.msg.type == 1)
        {
            TryAutoPlatVoice(chatData);
        }
        RefreshCurrentChannelMessage();
    }

    protected void OnChatSettingChanged(uint id, object argv)
    {
        RefreshCurrentChannelMessage();
    }

    protected void TryAutoPlatVoice(ChatData chatData)
    {
        if(null == chatData || null == chatData.msg || chatData.msg.type != 1)
        {
            return;
        }
        var chatMsg = chatData.msg;

        //����¼��ʱ����������
        if (YvVoiceMgr.isRuningRecord)
        {
            return;
        }
        //�������Լ�˵������
        if (chatMsg.sender == CSMainPlayerInfo.Instance.ID)
            return;

        if (chatData.Channel == ChatType.CT_WORLD && !Constant.autoPlay_World)
            return;

        if (chatData.Channel == ChatType.CT_NEIGHBOURING && !Constant.autoPlay_FuJin)
            return;

        if (chatData.Channel == ChatType.CT_GUILD && !Constant.autoPlay_Guild)
            return;

        if (chatData.Channel == ChatType.CT_PRIVATE && !Constant.autoPlay_Private)
            return;

        if (chatData.Channel == ChatType.CT_TEAM && !Constant.autoPlay_Team)
            return;

        if (chatData.coloredMsg && !Constant.autoPlay_ColorWorld)
            return;

        string[] msgs = chatMsg.message.Split('$');

        if (msgs.Length >= 3)
        {
            YvVoiceMgr.AutoPlayAudioList.Add(msgs[2]);
        }
    }

    protected void RefreshCurrentChannelMessage()
    {
        var chatDatas = CSChatManager.Instance.GetChannelDatas((int)ChatType.CT_COMPREHENSIVE);
        mComprehensiveDatas.Clear();
        for(int i = 0; i < chatDatas.Count; ++i)
        {
            var data = chatDatas[i];
            if(null == data)
            {
                continue;
            }

            //if(data.msg.type == 1)
            //{
            //    //������Ϣ����
            //    TryAutoPlatVoice(data);
            //}

            //��ҳ�治��ʾ������Ϣ
            if (data.coloredMsg)
            {
                continue;
            }

            if(data.Channel == (ChatType.CT_WORLD) && !CSConfigInfo.Instance.GetBool(ConfigOption.AutoPlayWorldText))
            {
                continue;
            }

            if (data.Channel == (ChatType.CT_GUILD) && !CSConfigInfo.Instance.GetBool(ConfigOption.AutoPlayGuildText))
            {
                continue;
            }

            if (data.Channel == (ChatType.CT_TEAM) && !CSConfigInfo.Instance.GetBool(ConfigOption.AutoPlayTeamText))
            {
                continue;
            }

            if (data.Channel == (ChatType.CT_PRIVATE) && !CSConfigInfo.Instance.GetBool(ConfigOption.AutoPlayPrivateText))
            {
                continue;
            }

            if (data.Channel == (ChatType.CT_NEIGHBOURING) && !CSConfigInfo.Instance.GetBool(ConfigOption.AutoPlayNearbyText))
            {
                continue;
            }
            mComprehensiveDatas.Append(data);
        }
        mChatListViewData.chatDatas = mComprehensiveDatas;
        mChatListViewComponent.Bind(mChatListViewData);
        mChatListViewComponent.TitleHeight = 0;

        if (mComprehensiveDatas.Count <= 0)
        {
            ScriptBinder.StopInvoke();
            MakeChatOpen = false;
        }
        else
        {
            MakeChatOpen = true;
            ScriptBinder.StopInvoke();
            ScriptBinder.Invoke(chatFadeOutTime,FadeOutChatPanel);
        }
    }

    public override void Show()
    {
        base.Show();

        RefreshCurrentChannelMessage();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        mComprehensiveDatas.Clear();
    }

}