using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

public partial class UIPrivateChatPanel : UIBasePanel
{
    private bool _isCancel;
    public bool isCancel
    {
        get
        {
            return _isCancel;
        }
        set
        {
            if (_isCancel == value) return;
            _isCancel = value;
            mbtn_voice_huatong.SetActive(!value);
            mbtn_voice_quxiao.SetActive(value);
            mbtn_voice_text.text = CSString.Format(value == false,627,628);
        }
    }

    protected UIChatListViewBinder mChatListViewComponent = new UIChatListViewBinder();
    protected UIChatListViewData mChatListViewData = new UIChatListViewData
    {
        eMode = UIChatBinder.ChatVisibleMode.CVM_CHAT_PANEL,
    };

    private Vector3 ChatPanelItem = new Vector3(0, 200, 0);
    private Vector3 FriendPanelRest = new Vector3(0, 0, 0);
    private social.FriendInfo PrivatePlayer;
    private Bounds bound;
    private bool isShowNoReadTips = false;
    public FriendType eFriendType;
    public override void Init()
    {
        base.Init();
        mClientEvent.AddEvent(CEvent.PrivateChatMessage, OnReceiveChatMessage);
        mClientEvent.AddEvent(CEvent.SocialInfoUpdate, OnReceiveAddFriend);
        mBtnAdd.onClick = OnAddWindowClick;
        mBtnSendMessage.onClick = SendChatMessage;
        mBtnVoice.onPress = OnVoiceClick;
        mBtnVoice.onDrag = OnDrag;
        UIEventListener.Get(mContainer).onClick = CloseChatExpressionPanel;
        mAddFriendBtn.onClick = OnClickAddFriendBtn;
        mDeleteFriendBtn.onClick = OnClockDeleteFriendBtn;
        UIEventListener.Get(mNoReadTips).onClick = OnClickNoRead;

        mchatScrollView.onDragFinished = ChatScrollViewDragFinished;
        mchatSpring.onMovement = ChatSpringOnMovement;
        mchatSpring.onFinished = ChatSpringFinish;
        mchatScrollView.CallBackShouldMove = CallBackShouldMove;

        InitChatListView();
    }

    protected void InitChatListView()
    {
        mLChatTemplate.gameObject.SetActive(false);
        mRChatTemplate.gameObject.SetActive(false);
        var listener = UIEventListener.Get(mChatListView);
        mChatListViewComponent.Setup(listener);
    }

    public void RefreshUI(FriendType type)
    {
        if (CSFriendInfo.Instance.ChooseFriend == null) return;
        mInput.value = "";
        eFriendType = type;
        mNoReadTips.gameObject.SetActive(false);
        isShowNoReadTips = false;
        this.PrivatePlayer = CSFriendInfo.Instance.ChooseFriend;
        ShowFriendBtn();
        RefreshChatMessage();
        var playerName = eFriendType == FriendType.FT_ENEMY ? CSString.Format(636, PrivatePlayer.name,string.Empty) : PrivatePlayer.name;
        mchatName.text = $"{UtilityColor.GetColorString(ColorType.SubTitleColor)}{CSString.Format(631, playerName.BBCode(ColorType.TitleColor))}";
    }

    /// <summary>
    /// 刷新聊天信息
    /// </summary>
    private void RefreshChatMessage()
    {
        var datas = CSChatManager.Instance.GetPrivateChatDatas(PrivatePlayer.roleId);
        mChatListViewData.oldChatDatas = mChatListViewData.chatDatas;
        mChatListViewData.chatDatas = datas;
        mChatListViewData.eChatType = ChatType.CT_PRIVATE;
        mChatListViewComponent.Bind(mChatListViewData);
        mChatListViewComponent.TitleHeight = 0;// mVoicePanelHandle.gameObject.activeSelf ? 30 : 0;

        //if (!mIsBottomMessageVisible && mchatScrollBar.alpha == 1)
        //{
        //    mchatScrollView.SetDragAmount(mchatScrollBar.value, 1);
        //}
    }

    //收到聊天信息
    private void OnReceiveChatMessage(uint id, object argv)
    {
        if(!(argv is ChatData chatData))
            return;

        if (!UIPrefab.gameObject.activeSelf)
            return;

        if (null == PrivatePlayer || PrivatePlayer.roleId != chatData.msg.sender && PrivatePlayer.roleId != chatData.msg.sendToID)
            return;

        if (chatData.Channel != ChatType.CT_PRIVATE)
            return;

        if (CSFriendInfo.Instance.IsPlayerInBlackList(chatData.msg.sender))
            return;

        RefreshChatMessage();

        if (isShowNoReadTips)
        {
            mNoReadTips.gameObject.SetActive(true);
        }
        else if (mchatScrollBar.alpha == 1)
        {
            FNDebug.LogFormat("<color=#00ff00>SetDragAmount 1</color>");
            mchatScrollView.SetDragAmount(0, 1);
        }
        //策划说不需要跳转
        //if (PrivatePlayer.roleId == chatData.msg.sendToID)
        //{
        //    mClientEvent.SendEvent(CEvent.PrivateChatToMessage, PrivatePlayer);
        //}
    }

    private void OnReceiveAddFriend(uint id, object argv)
    {
        if (PrivatePlayer != null)
        {
            if (eFriendType == FriendType.FT_NONE)
            {
                PrivatePlayer = CSFriendInfo.Instance.GetFriendInfoFromTouchList(PrivatePlayer.roleId);
            }
            else
            {
                PrivatePlayer = CSFriendInfo.Instance.GetFriendInfoByGuid(PrivatePlayer.roleId);
            }
        }
        ShowFriendBtn();
    }


    /// <summary>
    /// 关闭聊天表情包面板
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    private void CloseChatExpressionPanel(GameObject go = null)
    {
        mContainer.gameObject.SetActive(false);
        mClientEvent.SendEvent(CEvent.CloseChatAddPanel);
        PlayChatPanelPosition(FriendPanelRest, 0.15f);
    }

    /// <summary>
    /// 打开聊天表情包面板
    /// </summary>
    /// <param name="gp"></param>
    private void OnAddWindowClick(GameObject gp)
    {
        mContainer.gameObject.SetActive(true);
        UIManager.Instance.CreatePanel<UIChatAddPanel>(action: (f) =>
        {
            UIChatAddPanel panel = f as UIChatAddPanel;
            if (panel != null)
            {
                UIChatAddPanelData data = new UIChatAddPanelData();
                data.channelType = ChatChannelType.Friend;
                data.onLinkSelected = this.OnLinkSelected;
                data.channel = ChatType.CT_PRIVATE;
                panel.Show(data);
            }
        });
        PlayChatPanelPosition(ChatPanelItem, 0.15f);
    }

    protected void OnLinkSelected(string value, LinkInsertMode insertMode)
    {
        if (insertMode == LinkInsertMode.LM_INSERT_END)
        {
            mInput.SetValue = mInput.value.Length;
        }
        else if (insertMode == LinkInsertMode.LM_INSERT_BEGIN)
        {
            mInput.SetValue = 0;
        }
        mInput?.InsertText(value);
    }

    private void SendChatMessage(GameObject go)
    {
        if(null != PrivatePlayer)
        {
            CloseChatExpressionPanel();
            CSChatManager.Instance.SendChatMsg(mInput, (int)ChatType.CT_PRIVATE, ChatChannelType.Friend, PrivatePlayer.name, PrivatePlayer.roleId);
        }
    }

    /// <summary>
    /// 点击语音
    /// </summary>
    /// <param name="gp"></param>
    /// <param name="isPressed"></param>
    public void OnVoiceClick(GameObject gp, bool isPressed)
    {
        if (isPressed)
        {
            int levelLmt = CSChatManager.Instance.GetChatLimit((int)ChatType.CT_PRIVATE);
            if (levelLmt < 0)
            {
                UtilityTips.ShowTips(316, 1.5f, ColorType.Red);
                return;
            }

            //如果等级不足
            if (levelLmt > 0 && (CSMainPlayerInfo.Instance.Level < levelLmt))
            {
                int vipLimit = CSChatManager.Instance.GetChatVipLevelLimit();
                if (vipLimit < 0)//如果系统没有开放vip特权
                {
                    UtilityTips.ShowTips(CSString.Format(330, levelLmt), 1.5f, ColorType.Red);//聊天等级不足
                    return;
                }
                //如果系统开放了vip特权而且自身VIP等级未达到要求
                if (CSMainPlayerInfo.Instance.VipLevel < vipLimit)
                {
                    UtilityTips.ShowTips(CSString.Format(331, levelLmt, vipLimit), 1.5f, ColorType.Red);//聊天等级不足或者vip等级不足
                    return;
                }
            }
        }//私聊

        if (isPressed)
        {
            if (YvVoiceMgr.isRuningRecord)
            {
                UtilityTips.ShowTips(628);
                return;
            }
            CSPlayerInfo mPlayerInfo = CSMainPlayerInfo.Instance;
            long teamOrUnionId = 0;
            isCancel = false;
            CSAudioMgr.Instance.EnableAudioMgr(false);//开始录音，停止播放背景音效跟特效音
            // Utility.Removedistance(gp_huatong.transform, false);
            mGpHuatong.transform.localPosition = new Vector3(-540, -41, 0);
            string textInfo = "0";
            if (//CSAvatarManager.MainPlayerInfo.PrayInfo != null &&
                   CSMainPlayerInfo.Instance.RoleExtraValues != null &&
                  (CSMainPlayerInfo.Instance.RoleExtraValues.vipExp > 0 || CSMainPlayerInfo.Instance.VipLevel > 0))
            {
                textInfo = "1";
            }
            string Path = Application.persistentDataPath + "/";
            VoiceChatManager.Instance.StartRecord(Path, (teamOrUnionId.ToString() + "#" + textInfo + "#" + CSMainPlayerInfo.Instance.ID), 5, PrivatePlayer.roleId,
                (msg1) =>
                {
                    YvVoiceMgr.Instance.isCancelLuying = isCancel;
                });
        }
        else
        {
            CSAudioMgr.Instance.EnableAudioMgr(true);//手指抬起，开始播放背景音效跟特效音
                                                     //    Utility.Removedistance(gp_huatong.transform, true);
            mGpHuatong.transform.localPosition = new Vector3(-1300, -41, 0);
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

    private void ChatScrollViewDragFinished()
    {
        GameObject topMessage = mchatSpring.topMessage.gameObject;
        GameObject bottomMessage = mchatSpring.bottomMesssage.gameObject;
        if (mchatScrollBar.value == 1)
        {
            if (!topMessage.activeSelf)
                topMessage.SetActive(true);
            if (bottomMessage.activeSelf)
                bottomMessage.SetActive(false);
            UtilityTips.ShowRedTips(629);
            if (mNoReadTips.activeSelf)
            {
                mNoReadTips.SetActive(false);
            }
            isShowNoReadTips = false;
        }
        else if (mchatScrollBar.value == 0)
        {
            if (topMessage.activeSelf)
                topMessage.SetActive(false);
            if (!bottomMessage.activeSelf)
                bottomMessage.SetActive(true);

            UtilityTips.ShowRedTips(630);

            isShowNoReadTips = true;
        }
        else
        {
            isShowNoReadTips = true;
        }
    }
    private void OnClickNoRead(GameObject go)
    {
        if (go.activeSelf)
        {
            go.SetActive(false);
            isShowNoReadTips = false;
            FNDebug.LogFormat("<color=#00ff00>SetDragAmount 1</color>");
            mchatScrollView.SetDragAmount(0, 1);
        }
    }

    private void OnClickAddFriendBtn(GameObject go)
    {
        Net.ReqAddRelationMessage(PrivatePlayer.roleId.ToGoogleList(), (int)FriendType.FT_FRIEND);
    }

    private void OnClockDeleteFriendBtn(GameObject go)
    {
        CSFriendInfo.Instance.RemovePlayerFromTouchList(PrivatePlayer.roleId);
        mClientEvent.SendEvent(CEvent.OnRecvNewPrivateChatMsg);

        CSChatManager.Instance.RemovePrivateChatDatas(PrivatePlayer.roleId);
        mClientEvent.SendEvent(CEvent.RemoveTouchPlayer);
        RefreshChatMessage();
        FNDebug.LogFormat("<color=#00ff00>SetDragAmount 0</color>");
        mchatScrollView.SetDragAmount(0, 0);
        mchatScrollBar.value = 0;
        mchatScrollView.ResetPosition();
    }

    private void ChatSpringOnMovement(Vector3 f)
    {
        mchatSpring.UpdateSprite(f.y);
    }

    private void ChatSpringFinish()
    {
        if (mchatSpring.topMessage.gameObject.activeSelf)
        {
            mchatSpring.topMessage.gameObject.SetActive(false);
        }

        if (mchatSpring.bottomMesssage.gameObject.activeSelf)
        {
            mchatSpring.bottomMesssage.gameObject.SetActive(false);
        }
    }
    private void CallBackShouldMove()
    {
        if (!isShowNoReadTips)
        {
            if(mchatScrollView.shouldMoveVertically)
            {
                FNDebug.LogFormat("<color=#00ff00>SetDragAmount 1</color>");
                mchatScrollView.SetDragAmount(0, 1);
            }
            else
            {
                FNDebug.LogFormat("<color=#00ff00>SetDragAmount 0</color>");
                mchatScrollView.SetDragAmount(0, 0);
            }
        }
    }

    private void PlayChatPanelPosition(Vector3 targetPos, float duration = 0.5f)
    {
        mClientEvent.SendEvent(CEvent.OnPrivateChatTween, new TweenPosData
        {
             targetPos = targetPos,
             duration = duration,
        });
    }

    private void ShowFriendBtn()
    {
        if (eFriendType == FriendType.FT_NONE && PrivatePlayer != null)
        {
            mAddFriendBtn.gameObject.SetActive(PrivatePlayer.relation == 0 && false);
            mDeleteFriendBtn.gameObject.SetActive(true);
        }
        else
        {
            mAddFriendBtn.gameObject.SetActive(false);
            mDeleteFriendBtn.gameObject.SetActive(false);
        }
    }

    protected override void OnDestroy()
    {
        mClientEvent.SendEvent(CEvent.CloseChatAddPanel);
        mChatListViewComponent.Destroy();
        mChatListViewComponent = null;
        mChatListViewData?.Clear();
        mChatListViewData = null;
        base.OnDestroy();
    }
}




