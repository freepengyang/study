using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIChatPanel : UIBasePanel
{
    FastArray<ChannelData> mChannels = new FastArray<ChannelData>();
    private Vector3 ChatPanelClose = new Vector3(0, 0, 0);
    private Vector3 ChatPanelOpen = new Vector3(722, 0, 0);
    private Vector3 ChatPanelItem = new Vector3(722, 200, 0);
    protected UIChatListViewBinder mChatListViewComponent = new UIChatListViewBinder();
    protected UIChatListViewData mChatListViewData = new UIChatListViewData
    {
        eMode = UIChatBinder.ChatVisibleMode.CVM_CHAT_PANEL,
    };
    protected UIChatVoiceTemplate mUIChatVoicePanel = null;

    protected bool IsPanelOpen
    {
        get
        {
            return mPanelTween.value.x != ChatPanelClose.x;
        }
    }

    protected enum ChatChildPanel
    {
        CCP_CHAT_VOICE = 1,
        CCP_CHAT_SETTING = 2,
    }

    private void PlayChatPanelPosition(Vector3 targetPos, float duration = 0.5f)
    {
        mPanelTween.from = mPanelTween.value;
        mPanelTween.to = targetPos;
        mPanelTween.duration = duration;

        mPanelTween.ResetToBeginning();
        mPanelTween.PlayForward();
    }

    public override void Init()
    {
        base.Init();

        UIPrefab.transform.localPosition = new Vector3(455, 0, 0);
        Vector3 pos = Vector3.zero;
        if(null != UICamera.currentCamera)
            UICamera.currentCamera.WorldToScreenPoint(mBtnVoice.transform.position);
        bound = new Bounds(new Vector3(520 + pos.x, pos.y, pos.z), new Vector3(80, 80, 1));
        RegChildPanel<UIChatVoiceTemplate>((int)ChatChildPanel.CCP_CHAT_VOICE, mVoicePanelHandle, null, null);
        RegChildPanel<UIChatSettingPanel>((int)ChatChildPanel.CCP_CHAT_SETTING, mChatSettingPanel.gameObject,null,null);

        InitChannels();
        InitChatListView();

//#if UNITY_EDITOR
//        ScriptBinder.InvokeRepeating(0.0f, 0.01f, Update);
//#endif
        mIsBottomMessageVisible = false;
        mBtnMaskLayer.gameObject.SetActive(false);
        mBtnClose.onClick = OnBtnCloseClicked;
        mBtnSend.onClick = OnSendMsgClick;
        mBtnAdd.onClick = OnBtnAddClicked;
        mBtnMaskLayer.onClick = OnBtnMaskClicked;
        mBtnSetting.onClick = OnSettingClick;
        mClientEvent.AddEvent(CEvent.ShowChatPanel, OnEventShowChatPanel);
        mClientEvent.AddEvent(CEvent.HideChatPanel, OnEventHideChatPanel);
        mClientEvent.AddEvent(CEvent.OnRecievedNewChatMessage, OnReceiveChatMessage);
        mClientEvent.AddEvent(CEvent.OnMainPlayerTeamIdChanged, OnTeamChanged);
        mClientEvent.AddEvent(CEvent.OnMainPlayerGuildIdChanged, OnGuildChanged);
        mClientEvent.AddEvent(CEvent.OnChatChannelMessageCleared, OnChatChannelMessageCleared);
        mClientEvent.AddEvent(CEvent.Scene_Change,OnSceneChanged);
        mClientEvent.AddEvent(CEvent.Scene_ChangeMap,OnSceneChanged);
        mClientEvent.AddEvent(CEvent.OnChatSettingChanged, OnChatSettingChanged);
        mClientEvent.AddEvent(CEvent.OnLeaderCallMessage, OnLeaderCallEvent);

        mBtnVoice.onPress = OnVoiceClick;
        mBtnVoice.onDrag = OnDrag;
        UIEventListener.Get(mUnReadTips).onClick = OnClickNoRead;

        mScrollViewChat.onDragFinished += ChatScrollViewDragFinished;
        mChatSpring.onMovement += ChatSpringOnMovement;
        mChatSpring.onFinished += ChatSpringFinish;
        mScrollViewChat.CallBackShouldMove = CallBackShouldMove;

        mInput.onValidate = OnValidateInput;
        UIEventListener.Get(mInput.gameObject).onSelect = OnSelectInput;

        InitVoiceSetting();

        mScrollViewChat.ResetPosition();
    }

    /// <summary>
    /// 外部调用打开面板
    public void Show(ChatType channel, bool popupPanel = false, bool isClosePanel = false)
    {
        if (popupPanel && mPanelTween.tweenFactor == 0)
        {
            if (IsPanelOpen)
                PlayChatPanelPosition(ChatPanelClose);
            else
                PlayChatPanelPosition(ChatPanelOpen);
        }

        if (isClosePanel && IsPanelOpen)
        {
            PlayChatPanelPosition(ChatPanelClose);
            return;
        }

        mClientEvent.SendEvent(CEvent.OnChatChannelChanged, (int)channel);
    }

    private char OnValidateInput(string text, int charIndex, char addedChar)
    {
        if (addedChar == '[')
            return '【';
        if (addedChar == ']')
            return '】';
        return addedChar;
    }

    private void OnSelectInput(GameObject go, bool state)
    {
        if (state && mBtnMaskLayer.gameObject.activeSelf)
        {
            CloseChatAddPanel();
        }
    }


    #region voice member

    private Bounds bound;
    private bool _isCancel = false;
    public bool isCancel
    {
        get
        {
            return _isCancel;
        }
        set
        {
            if (_isCancel != value)
            {
                _isCancel = value;
                mMic.SetActive(!value);
                mMicUndo.SetActive(value);
                mVoiceText.text = CSString.Format(value == false, 324, 325);
            }
        }
    }
    #endregion

    #region voice function
    /// <summary>
    /// 点击语音
    /// </summary>
    /// <param name="gp"></param>
    /// <param name="isPressed"></param>
    public void OnVoiceClick(GameObject gp, bool isPressed)
    {
        ///0、综
        ///1、世
        ///2、行
        ///3、队
        ///4、附
        ///5、系
        
        if(!VoiceChatManager.Instance.isAllowYvVoice(isPressed))
        {
            return;
        }

        CSPlayerInfo mPlayerInfo = CSMainPlayerInfo.Instance;

        long teamOrUnionId = 0;
        if (mChannelData.eChatType == ChatType.CT_TEAM)
        {
            if(!Utility.HasTeam())
            {
                UtilityTips.ShowTips(321);
                return;
            }

            teamOrUnionId = CSMainPlayerInfo.Instance.TeamId;
        }
        else if (mChannelData.eChatType == ChatType.CT_GUILD)
        {
            if(!Utility.HasGuild())
            {
                UtilityTips.ShowTips(322);
                return;
            }
            teamOrUnionId = CSMainPlayerInfo.Instance.GuildId;
        }

        if (isPressed)
        {
            if (YvVoiceMgr.isRuningRecord)
            {
                UtilityTips.ShowTips(323);
                return;
            }

            isCancel = false;
            CSAudioMgr.Instance.EnableAudioMgr(false);//开始录音，停止播放背景音效跟特效音
            mMicHandle.transform.localPosition = Vector3.zero;

            string textInfo = "0";
            
            if (CSMainPlayerInfo.Instance.RoleExtraValues != null &&
                  (CSMainPlayerInfo.Instance.RoleExtraValues.vipExp > 0 || CSMainPlayerInfo.Instance.VipLevel > 0))
            {
                textInfo = "1";
            }
            string Path = Application.persistentDataPath + "/";
            VoiceChatManager.Instance.StartRecord(Path, (teamOrUnionId.ToString() + "#" + textInfo + "#" + CSMainPlayerInfo.Instance.ID), (int)mChannelData.eChatType, 0,
                (msg1) =>
                {
                    YvVoiceMgr.Instance.isCancelLuying = isCancel;
                });
        }
        else
        {
            CSAudioMgr.Instance.EnableAudioMgr(true);//手指抬起，开始播放背景音效跟特效音
            mMicHandle.transform.localPosition = new Vector3(-10000.0f, 0, 0);
            VoiceChatManager.Instance.StopRecord();
        }
    }

    public void OnVoiceClick(bool isPressed, Vector3 targetPosition, ChatType channel)
    {
        if (!VoiceChatManager.Instance.isAllowYvVoice(isPressed))
        {
            return;
        }

        CSPlayerInfo mPlayerInfo = CSMainPlayerInfo.Instance;

        long teamOrUnionId = 0;

        if (channel == ChatType.CT_TEAM)
        {
            if (!Utility.HasTeam())
            {
                if(isPressed)
                    UtilityTips.ShowTips(321);
                return;
            }

            teamOrUnionId = CSMainPlayerInfo.Instance.TeamId;
        }
        else if (channel == ChatType.CT_GUILD)
        {
            if (!Utility.HasGuild())
            {
                if (isPressed)
                    UtilityTips.ShowTips(322);
                return;
            }
            teamOrUnionId = CSMainPlayerInfo.Instance.GuildId;
        }

        if (isPressed)
        {
            if (YvVoiceMgr.isRuningRecord)
            {
                UtilityTips.ShowTips(323);
                return;
            }

            isCancel = false;
            CSAudioMgr.Instance.EnableAudioMgr(false);//开始录音，停止背景音效跟特效音
            mMicHandle.transform.localPosition = targetPosition;

            string textInfo = "0";
            if (CSMainPlayerInfo.Instance.RoleExtraValues != null &&
                  (CSMainPlayerInfo.Instance.RoleExtraValues.vipExp > 0 || CSMainPlayerInfo.Instance.VipLevel > 0)
                  )
            {
                textInfo = "1";
            }
            string Path = Application.persistentDataPath + "/";
            VoiceChatManager.Instance.StartRecord(Path, (teamOrUnionId.ToString() + "#" + textInfo + "#" + CSMainPlayerInfo.Instance.ID),(int)channel, 0,
                (msg1) =>
                {
                    YvVoiceMgr.Instance.isCancelLuying = isCancel;
                });
        }
        else
        {
            CSAudioMgr.Instance.EnableAudioMgr(true);//手指抬起，继续背景音效跟特效音
            mMicHandle.transform.localPosition = new Vector3(-10000f, 0f, 0f);
            VoiceChatManager.Instance.StopRecord();
        }
    }

    /// <summary>
    /// 拖拽
    /// </summary>
    /// <param name="gp"></param>
    /// <param name="delta"></param>
    public void OnDrag(GameObject gp, Vector2 delta)
    {
        if (bound.Contains(Input.mousePosition))
        {
            isCancel = false;
        }
        else
        {
            isCancel = true;
        }
    }
    #endregion

    private void InitVoiceSetting()
    {
        CSConfigInfo mConfigInfo = CSConfigInfo.Instance;
        Constant.autoPlay_Guild = mConfigInfo.GetBool(ConfigOption.AutoPlayGuildAudio);
        Constant.autoPlay_Team = mConfigInfo.GetBool(ConfigOption.AutoPlayTeamAudio);
        Constant.autoPlay_World = mConfigInfo.GetBool(ConfigOption.AutoPlayWorldAudio);
        Constant.autoPlay_Private = mConfigInfo.GetBool(ConfigOption.AutoPlayPrivateAudio);
        Constant.autoPlay_FuJin = mConfigInfo.GetBool(ConfigOption.AutoPlayFuJinAudio);
        Constant.autoPlay_ColorWorld = mConfigInfo.GetBool(ConfigOption.AutoPlayColorWorldAudio);

        Constant.showWorldMsg = mConfigInfo.GetBool(ConfigOption.AutoPlayWorldText);
        Constant.showTeamMsg = mConfigInfo.GetBool(ConfigOption.AutoPlayTeamText);
        Constant.showGuildMsg = mConfigInfo.GetBool(ConfigOption.AutoPlayGuildText);
        Constant.showNearMsg = mConfigInfo.GetBool(ConfigOption.AutoPlayNearbyText);
        Constant.showPrivateMsg = mConfigInfo.GetBool(ConfigOption.AutoPlayPrivateText);
    }

    private void OnClickNoRead(GameObject go)
    {
        if (go.activeSelf)
        {
            go.SetActive(false);
            mIsBottomMessageVisible = false;
            mScrollViewChat.SetDragAmount(mChatScrollBar.value, 1);
        }
    }

    private void OnSendMsgClick(GameObject go)
    {
        CloseChatAddPanel();
        CSChatManager.Instance.SendChatMsg(mInput, (int)mChannelData.eChatType);
    }

    protected void SendChatMsg()
    {
        CSChatManager.Instance.SendChatMsg(mInput, (int)mChannelData.eChatType);
    }

#if UNITY_EDITOR
    void Update()
    {
        if ((Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter)) && IsPanelOpen)
        {
            if(!string.IsNullOrEmpty(mInput.value))
            {
                SendChatMsg();
            }
        }
    }
#endif

    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    protected override void OnDestroy()
    {
        mChatListViewComponent.Destroy();
        mChatListViewComponent = null;
        mChannelData = null;
        mChannels.Dispose();
        mChannels = null;

        base.OnDestroy();
    }

    protected void InitChannels()
    {
        mChannels.Clear();
        int cnt = (int)ChatType.CT_COUNT;
        for (int i = 0; i < cnt; ++i)
        {
            var eChatType = (ChatType)i;
            bool opened = CSChatManager.Instance.IsChannelOpened(eChatType);
            if(opened)
            {
                var data = mChannels.PushNewElementToTail();
                data.eChatType = eChatType;
                data.onChannelChanged = OnChannelChanged;
                data.isOn = false;
                data.mask = eChatType == ChatType.CT_COMPREHENSIVE ? (-1) : (1 << (int)eChatType);
            }
        }
        mChatGroup.Bind<ChannelBinder,ChannelData>(mChannels,mClientEvent);
    }

    protected void InitChatListView()
    {
        mLChatTemplate.gameObject.SetActive(false);
        mRChatTemplate.gameObject.SetActive(false);
        var listener = UIEventListener.Get(mChatListView);
        mChatListViewComponent.Setup(listener);
    }

    protected void OnEventShowChatPanel(uint uiEvtID, object data)
    {
        if(!IsPanelOpen)
        {
            if(null != mChannelData)
            {
                RefreshCurrentChannelMessage();
            }
            PlayChatPanelPosition(ChatPanelOpen);
        }
    }

    protected void OnEventHideChatPanel(uint uiEvtID, object data)
    {
        if(IsPanelOpen)
        {
            PlayChatPanelPosition(ChatPanelClose);
        }
    }

    protected void OnChatChannelMessageCleared(uint uiEvtID, object data)
    {
        ChatType eChatType = (ChatType)data;
        if (IsPanelOpen && null != mChannelData && eChatType == mChannelData.eChatType)
        {
            RefreshCurrentChannelMessage();
        }
    }

    protected void OnSceneChanged(uint uiEvtID, object data)
    {
        RefreshChannelInfo();
    }

    protected void OnChatSettingChanged(uint uiEvtID, object data)
    {
        InitVoiceSetting();
    }

    protected void OnLeaderCallEvent(uint uiEventId, object argv)
    {
        var reqData = argv as chat.ReleaseNtf;
        if(null == reqData)
        {
            return;
        }

        VoiceLoginType voiceLoginType = VoiceLoginType.union;
        string channelName = CSString.Format(362);// "行会";
        if (reqData.rid == (int)ChatType.CT_GUILD)
        {
            channelName = CSString.Format(362);//"行会";
            voiceLoginType = VoiceLoginType.union;
        }

        //创建行会召集令界面
        if (YvVoiceMgr.Instance.mLoginType == (int)voiceLoginType)
        {
            return;
        }

        UIManager.Instance.CreatePanel("UISummonPanel", UIManager.Instance.GetRoot(), (f) =>
        {
            CsUseCallItem.Instance.AddPanel(reqData.rid, f as UISummonPanel);
            //CSStringBuilder.Clear();
            //CSStringBuilder.Append("[eee5c3]", reqData.position, "[ffcc30]", reqData.name, "[-]邀请你进入", channelName, "语音频道");
            (f as UISummonPanel).RefreshUI(CSString.Format(1852, reqData.position, reqData.name, channelName), (s, d) =>
            {
                if (s == (int)MsgBoxType.MBT_OK)
                {
                    VoiceChatManager.Instance.Login(voiceLoginType,()=>
                    {
                        if (YvVoiceMgr.Instance.mLoginType == (int)voiceLoginType)
                            UtilityTips.ShowGreenTips(1847);
                        else
                            UtilityTips.ShowRedTips(334);
                    });
                    CsUseCallItem.Instance.RemovePanel(reqData.rid);
                }
                else if (s == (int)MsgBoxType.MBT_CANCEL)
                {
                    CsUseCallItem.Instance.RemovePanel(reqData.rid);
                }
            }, 60, reqData.rid);
        });
    }

    protected void RefreshCurrentChannelMessage()
    {
        if(null != mChannelData)
        {
            if (mIsBottomMessageVisible)
            {
                mUnReadTips.gameObject.SetActive(true);
            }

            var chatDatas = CSChatManager.Instance.GetChannelDatas(mChannelData.eChatType);
            mChatListViewData.oldChatDatas = mChatListViewData.chatDatas;
            mChatListViewData.chatDatas = chatDatas;
            if (mChatListViewData.oldChatDatas == null)
            {
                mChatListViewData.oldChatDatas = chatDatas;
            }
            mChatListViewData.eChatType = mChannelData.eChatType;
            mChatListViewComponent.Bind(mChatListViewData);
            mChatListViewComponent.TitleHeight = mVoicePanelHandle.gameObject.activeSelf ? 30 : 0;

            if (!mIsBottomMessageVisible && mChatScrollBar.alpha == 1)
            {
                mScrollViewChat.SetDragAmount(mChatScrollBar.value, 1);
            }
        }
    }

    protected void OnReceiveChatMessage(uint uiEvtID, object data)
    {
        if((IsPanelOpen && data is ChatData chatData))
        {
            if(null != mChannelData && chatData.IsLinkedChannedl(mChannelData.eChatType))
            {
                RefreshCurrentChannelMessage();
            }
        }
    }

    protected void OnTeamChanged(uint uiEvtID, object data)
    {
        RefreshChannelInfo();
    }

    protected void OnGuildChanged(uint uiEvtID, object data)
    {
        RefreshChannelInfo();
    }

    protected void RefreshChannelInfo()
    {
        if(null != mChannelData)
        {
            bool isShowVoice = false;
            //根据当前人物是否加入队伍，是否加入家族判断是否显示聊天框
            switch (mChannelData.eChatType)
            {
                case ChatType.CT_COMPREHENSIVE:
                    HideInput(true, CSString.Format(316));//综合频道隐藏输入框
                    break;
                case ChatType.CT_SYSTEM:
                    HideInput(true, CSString.Format(316));//系统频道隐藏输入框
                    break;
                case ChatType.CT_GUILD:
                    isShowVoice = Utility.HasGuild();
                    HideInput(!isShowVoice, CSString.Format(317));
                    break;
                case ChatType.CT_TEAM:
                    isShowVoice = Utility.HasTeam();
                    HideInput(!isShowVoice, CSString.Format(318));
                    break;
                default:
                    HideInput(false,string.Empty);
                    break;
            }

            if (Utility.IsCrossServerMap(CSScene.GetMapID()) && mChannelData.eChatType != ChatType.CT_TEAM &&
                mChannelData.eChatType != ChatType.CT_SYSTEM && mChannelData.eChatType != ChatType.CT_COMPREHENSIVE)
            {
                HideInput(true, CSString.Format(314));
                isShowVoice = false;
            }

            HideVoicePanel(isShowVoice);
        }
    }

    protected void HideVoicePanel(bool visible)
    {
        if (!visible && null == mUIChatVoicePanel)
        {
            if(null != mVoicePanelHandle)
            {
                mVoicePanelHandle.CustomActive(false);
                mReplaceHandle.CustomActive(visible);
                //mScrollViewChat.TitleHeight = 0;
            }
            return;
        }

        if (null == mUIChatVoicePanel)
        {
            mUIChatVoicePanel = OpenChildPanel((int)ChatChildPanel.CCP_CHAT_VOICE) as UIChatVoiceTemplate;
        }

        //if (visible)
        //{
        //    mChatViewPanel.baseClipRegion = new Vector4(27, -13, 418, 522);
        //    mChatListView.transform.localPosition = new Vector3(-140, 235, 0);
        //}
        //else
        //{
        //    mChatViewPanel.baseClipRegion = new Vector4(27, 6, 418, 560);
        //    mChatListView.transform.localPosition = new Vector3(-140, 273, 0);
        //}
        mUIChatVoicePanel.Show(mChannelData != null ? mChannelData.eChatType : ChatType.CT_COMPREHENSIVE);
        mUIChatVoicePanel.SetVisible(visible);
        mReplaceHandle.CustomActive(visible);
    }

    protected void OnBtnCloseClicked(GameObject go)
    {
        HidePanel();
    }

    protected void OnBtnAddClicked(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIChatAddPanel>(action: (f) =>
        {
            UIChatAddPanel panel = f as UIChatAddPanel;
            if (panel != null)
            {
                UIChatAddPanelData data = new UIChatAddPanelData();
                data.channelType = ChatChannelType.ChatPanel;
                data.onLinkSelected = this.OnLinkSelected;
                data.channel = mChannelData.eChatType;
                panel.Show(data);
            }
        });

        PlayChatPanelPosition(ChatPanelItem, 0.15f);
        mBtnMaskLayer.gameObject.SetActive(true);
    }

    protected void OnLinkSelected(string value, LinkInsertMode insertMode)
    {
        if (insertMode == LinkInsertMode.LM_INSERT_END)
        {
            mInput.SetValue = mInput.value.Length;
        }
        else if(insertMode == LinkInsertMode.LM_INSERT_BEGIN)
        {
            mInput.SetValue = 0;
        }
        mInput?.InsertText(value);
    }

    protected void OnBtnMaskClicked(GameObject go)
    {
        CloseChatAddPanel();
    }

    //点击设置按钮打开设置窗口，关闭按钮界面
    private void OnSettingClick(GameObject go)
    {
        OpenChildPanel((int)ChatChildPanel.CCP_CHAT_SETTING);
    }

    protected void CloseChatAddPanel()
    {
        mClientEvent.SendEvent(CEvent.CloseChatAddPanel);
        PlayChatPanelPosition(ChatPanelOpen, 0.15f);
        mBtnMaskLayer.gameObject.SetActive(false);
    }

    protected void HidePanel()
    {
        PlayChatPanelPosition(ChatPanelClose);
        if(null != mUIChatVoicePanel)
        {
            mUIChatVoicePanel.ColseList();
        }
    }

    private void HideInput(bool isHide, string showInfo)
    {
        mInput.gameObject.SetActive(!isHide);
        if (QuDaoConstant.OpenVoice)
            mBtnVoice.gameObject.SetActive(!isHide);
        else
            mBtnVoice.gameObject.SetActive(false);
        mBtnSend.gameObject.SetActive(!isHide);
        mBtnAdd.gameObject.SetActive(!isHide);
        mNoChat.gameObject.SetActive(isHide);
        if(isHide)
        {
            mNoChat.text = showInfo;
        }
    }

    protected ChannelData mChannelData;

    protected void OnChannelChanged(ChannelData channelData)
    {
        mChannelData = channelData;
        mIsBottomMessageVisible = false;
        mUnReadTips.gameObject.SetActive(false);
        mChatViewPanel.transform.localPosition = new Vector3(-435, 22,0);
        mChatViewPanel.clipOffset = new Vector2(20, 0);
        mScrollViewChat.ResetPosition();
        RefreshChannelInfo();
        //Debug.LogFormat("OnChannelSelected eChatType = {0}", channelData.eChatType);
        var chatDatas = CSChatManager.Instance.GetChannelDatas(channelData.eChatType);
        mChatListViewData.eChatType = channelData.eChatType;
        mChatListViewData.oldChatDatas = mChatListViewData.chatDatas;
        mChatListViewData.chatDatas = chatDatas;
        if (mChatListViewData.oldChatDatas == null)
        {
            mChatListViewData.oldChatDatas = chatDatas;
        }
        mChatListViewComponent.Bind(mChatListViewData);
        mReplaceHandle.gameObject.SetActive(mVoicePanelHandle.gameObject.activeSelf);
        mChatListViewComponent.TitleHeight = mVoicePanelHandle.gameObject.activeSelf ? 30 : 0;
        var bounds = this.bound;
        mScrollViewChat.SetDragAmount(mChatScrollBar.value, 1);
    }

    protected bool mIsBottomMessageVisible;
    private void ChatScrollViewDragFinished()
    {
        GameObject topMessage = mChatSpring.topMessage.gameObject;
        GameObject bottomMessage = mChatSpring.bottomMesssage.gameObject;
        if (mChatScrollBar.value == 1)
        {
            if (!topMessage.activeSelf)
                topMessage.SetActive(true);
            if (bottomMessage.activeSelf)
                bottomMessage.SetActive(false);
            UtilityTips.ShowTips(319, 1.5f, ColorType.Red);
            if (mUnReadTips.activeSelf)
            {
                mUnReadTips.SetActive(false);
            }
            mIsBottomMessageVisible = false;
        }
        else if (mChatScrollBar.value == 0)
        {
            if (topMessage.activeSelf)
                topMessage.SetActive(false);
            if (!bottomMessage.activeSelf)
                bottomMessage.SetActive(true);

            UtilityTips.ShowTips(320, 1.5f, ColorType.Red);

            mIsBottomMessageVisible = true;
        }
        else
        {
            mIsBottomMessageVisible = true;
        }
    }
    private void ChatSpringOnMovement(Vector3 f)
    {
        mChatSpring.UpdateSprite(f.y);
    }

    private void ChatSpringFinish()
    {
        if (mChatSpring.topMessage.gameObject.activeSelf)
        {
            mChatSpring.topMessage.gameObject.SetActive(false);
        }

        if (mChatSpring.bottomMesssage.gameObject.activeSelf)
        {
            mChatSpring.bottomMesssage.gameObject.SetActive(false);
        }
    }
    //Scroll View 从不可拖拽到可拖拽回调
    private void CallBackShouldMove()
    {
        //if (!mIsBottomMessageVisible)
        //{
        //    if(mScrollViewChat.shouldMove)
        //    {
        //        mScrollViewChat.SetDragAmount(mChatScrollBar.value, 1);
        //    }
        //    else
        //    {
        //        mScrollViewChat.SetDragAmount(mChatScrollBar.value, 0);
        //    }
        //}
    }
}