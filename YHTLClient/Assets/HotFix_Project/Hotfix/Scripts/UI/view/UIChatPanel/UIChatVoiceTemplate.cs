using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChatVoiceTemplate : UIChatVoiceCallPanel
{
    UILabel mOnLineNum;
    UILabel mDescribe;
    UISprite mVoiceIcon;
    UISprite mVoiceQua;
    UISprite mVoiceBg;
    protected override void _InitScriptBinder()
    {
        mOnLineNum = ScriptBinder.GetObject("OnLineNum") as UILabel;
        mDescribe = ScriptBinder.GetObject("Describe") as UILabel;
        mVoiceIcon = ScriptBinder.GetObject("VoiceIcon") as UISprite;
        mVoiceQua = ScriptBinder.GetObject("VoiceQua") as UISprite;
        mVoiceBg = ScriptBinder.GetObject("VoiceBg") as UISprite;
    }

    public override void Init()
    {
        base.Init();

        mBtnVoice.onClick = OnBtnVoiceClick;
        mClientEvent.AddEvent(CEvent.OnVoiceRoomNtfMessage, UpdateVoiceLoginNum);

        mClientEvent.AddEvent(CEvent.YvVoiceLoginType, UpdateLoginType);

        YvVoiceMgr.Instance.VoicePlayerUpdateRoom += GetLoginRoomNum;
        AddClick();
    }

    public void Show(ChatType chatType)
    {
        ShowDescribe(chatType);
    }

    public void ColseList()
    {
        if (mOption.transform.localScale.y == 1)
            mOption.PlayReverse();
    }

    private void ShowDescribe(ChatType channel)
    {
        curChannel = channel;
        switch (channel)
        {
            case ChatType.CT_GUILD:
                mDescribe.text = CSString.Format(380);
                isAlreadLogined = YvVoiceMgr.Instance.mLoginType == (int)VoiceLoginType.union;
                CurVoiceLoginType = VoiceLoginType.union;
                break;
            case ChatType.CT_TEAM:
                mDescribe.text = CSString.Format(381);
                isAlreadLogined = YvVoiceMgr.Instance.mLoginType == (int)VoiceLoginType.team;
                CurVoiceLoginType = VoiceLoginType.team;
                break;
        }
        if (isAlreadLogined)
        {
            getLoginNum();
            mVoiceLab.text = CSString.Format(383);
            mVoiceIcon.spriteName = "chat_bi1";
        }
        else
        {
            mOnLineNum.text = CSString.Format(378);
            mVoiceLab.text = CSString.Format(384);//进入
            mVoiceIcon.spriteName = "chat_bi3";
        }
        if (mOption.transform.localScale.y == 1)
            mOption.PlayReverse();
    }
    private void OnBtnVoiceClick(GameObject go)
    {
        if (isAlreadLogined)
        {
            if (mOption.transform.localScale.y == 0)
            {
                if (curChannel == ChatType.CT_TEAM)
                {
                    mBtnCall.gameObject.SetActive(false);
                    mBtnTableList.gameObject.SetActive(false);
                    mBtnPublish.gameObject.SetActive(false);

                    mVoiceBg.height = 60;
                }
                else
                {
                    mBtnCall.gameObject.SetActive(true);
                    mBtnTableList.gameObject. SetActive(true);
                    mBtnPublish.gameObject.SetActive(false);
                    mVoiceBg.height = 120;
                }
                if (!mOption.enabled)
                    mOption.enabled = true;
                if (YvVoiceMgr.Instance.isOpenVoiceSpeak)
                    mSpUpMic.spriteName = "chat_bi6";
                else
                    mSpUpMic.spriteName = "chat_bi8";
                mOption.PlayForward();
                mOptionTable.Reposition();
                mVoiceQua.transform.localRotation = new Quaternion(0f, 0f, -180f, 0f);
            }
            else
            {
                mOption.PlayReverse();
                mVoiceQua.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
            }
        }
        else
        {
            if (!VoiceChatManager.Instance.isAllowYvVoice(true))
                return;

            if(0 != YvVoiceMgr.Instance.mLoginType && YvVoiceMgr.Instance.mLoginType != (int)CurVoiceLoginType)
            {
                switch((VoiceLoginType)YvVoiceMgr.Instance.mLoginType)
                {
                    case VoiceLoginType.team:
                        UtilityTips.ShowRedTips(1845);
                        break;
                    case VoiceLoginType.union:
                        UtilityTips.ShowRedTips(1846);
                        break;
                }
                return;
            }

            VoiceChatManager.Instance.Login(CurVoiceLoginType, () =>
            {
                if (YvVoiceMgr.Instance.mLoginType == (int)CurVoiceLoginType)
                {
                    isAlreadLogined = true;
                    UtilityTips.ShowGreenTips(1847);
                    mVoiceLab.text = CSString.Format(383);
                }
                else
                    UtilityTips.ShowRedTips(334);
            });
        }
    }

    public void UpdateLoginType(uint id, object argv)
    {
        isAlreadLogined = false;
        if (CurVoiceLoginType != VoiceLoginType.None)
        {
            switch (CurVoiceLoginType)
            {
                case VoiceLoginType.union:
                    if (YvVoiceMgr.Instance.mLoginType == (int)VoiceLoginType.union)
                        isAlreadLogined = true;
                    break;
                case VoiceLoginType.team:
                    if (YvVoiceMgr.Instance.mLoginType == (int)VoiceLoginType.team)
                        isAlreadLogined = true;
                    break;
            }
        }
        if (isAlreadLogined)
        {
            mVoiceLab.text = CSString.Format(383);
        }
        else
        {
            mOnLineNum.text = CSString.Format(378);
            mVoiceLab.text = CSString.Format(384);
        }
        if (mOption.transform.localScale.y == 1)
        {
            mOption.PlayReverse();
        }
    }

    private void UpdateVoiceLoginNum(uint id, object argv)
    {
        var data = argv as chat.VoiceRoomNtf;
        if(null == data)
        {
            return;
        }

        if (YvVoiceMgr.Instance.mLoginType == (int)VoiceLoginType.None)
        {
            mOnLineNum.text = CSString.Format(378);
            return;
        }

        FNDebug.LogFormat("<color=#00ff00>[语音聊天]:更新在线人数:{0}</color>",data.num);
        mOnLineNum.text = CSString.Format(379, data.num);
    }

    private void GetLoginRoomNum()
    {
        FNDebug.LogFormat("<color=#00ff00>[语音聊天]:拉取语音在线人数</color>");
        //语音SDK的监听快于服务器收到消息，数量会少
        ScriptBinder.Invoke(1, getLoginNum);
    }

    private void getLoginNum()
    {
        Net.ChatVoiceRoomNumReqMessage(YvVoiceMgr.Instance.mLoginType);
    }

    protected override void OnDestroy()
    {
        YvVoiceMgr.Instance.VoicePlayerUpdateRoom -= GetLoginRoomNum;
        base.OnDestroy();
    }
}
