using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChatVoiceCallPanel : UIBasePanel
{
    protected UIEventListener mBtnVoice;
    protected TweenScale mOption;
    protected UISprite mOptionBG;
    protected UITable mOptionTable;
    protected UILabel mVoiceLab;
    protected UIEventListener mBtnUpMic;
    protected UIEventListener mBtnCall;
    protected UIEventListener mBtnPublish;
    protected UIEventListener mBtnTableList;
    protected UIEventListener mBtnExit;
    protected UISprite mSpUpMic;
    //注意这里是故意这么写的
    protected void InitScriptBinder()
    {
        mBtnVoice = ScriptBinder.GetObject("BtnVoice") as UIEventListener;
        mOption = ScriptBinder.GetObject("Option") as TweenScale;
        mOptionBG = ScriptBinder.GetObject("OptionBG") as UISprite;
        mOptionTable = ScriptBinder.GetObject("OptionTable") as UITable;
        mVoiceLab = ScriptBinder.GetObject("VoiceLab") as UILabel;
        mBtnUpMic = ScriptBinder.GetObject("BtnUpMic") as UIEventListener;
        mBtnCall = ScriptBinder.GetObject("BtnCall") as UIEventListener;
        mBtnPublish = ScriptBinder.GetObject("BtnPublish") as UIEventListener;
        mBtnTableList = ScriptBinder.GetObject("BtnTableList") as UIEventListener;
        mBtnExit = ScriptBinder.GetObject("BtnExit") as UIEventListener;
        mSpUpMic = ScriptBinder.GetObject("SpUpMic") as UISprite;
    }

    public override void Init()
    {
        InitScriptBinder();
        base.Init();
    }

    protected bool isAlreadLogined;
    protected ChatType curChannel;
    protected VoiceLoginType CurVoiceLoginType; //该字段在主场景记录语音登录类型，在聊天面板记录聊天频道类型
    protected bool isMicrTeam;

    protected void AddClick()
    {
        //mClientEvent.AddEvent(CEvent.OnMainPlayerTeamIdChanged, ShowTeamInfo);
        mClientEvent.AddEvent(CEvent.YvVoiceSpeakState, YvVoiceSpeakState);
        mBtnUpMic.onClick = OnUpMicrClick;
        mBtnCall.onClick = OnCallClick;
        mBtnPublish.onClick = OnPublishClick;
        mBtnTableList.onClick = OnTableListClick;
        mBtnExit.onClick = OnExitClick;
    }
    private void OnUpMicrClick(GameObject go)
    {
        //高飞说 现在队伍不要限制
        /*if (CurVoiceLoginType == VoiceLoginType.team)
        {
            isMicrTeam = true;
            Net.ReqGetTeamInfoMessage();
            return;
        }*/
        if(CurVoiceLoginType == VoiceLoginType.union)
        {
            if (!CSMainPlayerInfo.Instance.CanSpeak && CSMainPlayerInfo.Instance.GuildPos != (int)GuildPos.President)
            {
                UtilityTips.ShowRedTips(1853);
                return;
            }
        }
        FNDebug.LogFormat("<color=#00ff00>[语音聊天]:上麦或下麦</color>");
        SwitchVoiceSpeakState();
    }
    private void OnCallClick(GameObject go)
    {
        string channelName = "";
        string name = "";
        switch (CurVoiceLoginType)
        {
            case VoiceLoginType.None:
            case VoiceLoginType.team:
                return;
            case VoiceLoginType.union:
                bool canspeak = CSMainPlayerInfo.Instance.CanSpeak || CSMainPlayerInfo.Instance.GuildPos == (int)GuildPos.President;
                if (!canspeak)
                {
                    UtilityTips.ShowRedTips(365);
                    return;
                }
                if (CSGuildInfo.Instance.IsCallGuildInCD(true))
                {
                    return;
                }
                channelName = CSString.Format(363);
                name = CSString.Format(364);
                break;
            default:
                break;
        }

        UtilityTips.ShowPromptWordTips(7, null, () =>
        {
            Net.ChatCallReleaseReqMessage((int)curChannel);
        }, name, channelName);
    }
    private void OnPublishClick(GameObject go)
    {
        if (YvVoiceMgr.Instance.isOpenVoiceSpeak)
        {
            int timestamp = PlayerPrefs.GetInt("VoicePushTime" + CSConstant.mOnlyServerId);
            long endTime = CSServerTime.Instance.TotalSeconds - timestamp;
            if (CSServerTime.Instance.TotalSeconds - timestamp > 60)
            {
                string channelName = "";
                if (CurVoiceLoginType == VoiceLoginType.union)
                    channelName = CSString.Format(362);// "行会";
                else if (CurVoiceLoginType == VoiceLoginType.team)
                    channelName = CSString.Format(361);// "队伍";
                                                       //CSStringBuilder.Clear();
                                                       //CSStringBuilder.Append("我正在", channelName, "频道语音直播，赶快一起", "[ffcc30][url=voice:", (int)YvVoiceMgr.Instance.mLoginType, "][u]加入收听[-][/u][/url]吧！");

                Net.ReqChatMessage((int)curChannel, CSString.Format(360, channelName, "[url=func:voice:", "[/url]"), 0, 2, "");

                PlayerPrefs.SetInt("VoicePushTime" + CSConstant.mOnlyServerId, (int)CSServerTime.Instance.TotalSeconds);
            }
            else
            {
                UtilityTips.ShowRedTips(339, 60 - endTime);
            }
        }
        else
        {
            UtilityTips.ShowRedTips(338);
        }
    }
    private void OnTableListClick(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIChatVoiceListPanel>(action: f =>
        {
            UIChatVoiceListPanel panel = f as UIChatVoiceListPanel;
            if (panel != null)
            {
                panel.ShowMsg(curChannel);
            }
        });
    }

    public virtual void OnExitClick(GameObject go)
    {
        VoiceChatManager.Instance.Logout(() =>
        {
            UtilityTips.ShowGreenTips(1848);
            mOption.PlayReverse();
            isAlreadLogined = false;
            mVoiceLab.text = CSString.Format(367);
        });
    }

    /*void ShowTeamInfo(uint id, object argv)
    {
        if (!isMicrTeam) return;
        isMicrTeam = false;
        var msg = argv as team.GetTeamInfoResponse;
        if(null == msg)
        {
            return;
        }

        if (msg.myTeam.leaderId == CSMainPlayerInfo.Instance.ID)
        {
            SwitchVoiceSpeakState();
        }
        else
        {
            UtilityTips.ShowRedTips(366);
        }
    }*/

    private void YvVoiceSpeakState(uint id, object argv)
    {
        if (YvVoiceMgr.Instance.isOpenVoiceSpeak)
        {
            mSpUpMic.spriteName = "chat_bi6";
        }
        else
            mSpUpMic.spriteName = "chat_bi8";
    }

    void SwitchVoiceSpeakState()
    {
        VoiceChatManager.Instance.SwitchVoiceSpeakState(() =>
        {
            if (YvVoiceMgr.Instance.isOpenVoiceSpeak)
            {
                UtilityTips.ShowTips(368);
                CSChatManager.Instance.AddVoiceLink((VoiceLoginType)YvVoiceMgr.Instance.mLoginType);
            }
            else
            {
                UtilityTips.ShowTips(369);
            }
        });
    }

    protected string GetVoiceName()
    {
        switch ((VoiceLoginType)YvVoiceMgr.Instance.mLoginType)
        {
            case VoiceLoginType.None:
                return CSString.Format(370);
            case VoiceLoginType.union:
                curChannel = ChatType.CT_GUILD;
                return CSString.Format(371);
            case VoiceLoginType.team:
                curChannel = ChatType.CT_TEAM;
                return CSString.Format(372);
        }
        return "";
    }
}