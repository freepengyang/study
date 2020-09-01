using UnityEngine;
using System.Collections;

public class UIChatVoiceMainTemplate : UIChatVoiceCallPanel
{
    UIEventListener mBtnHide;
    UIEventListener mBtnTeam;
    UIEventListener mBtnGuild;
    UISprite mVoiceIcon;
    UISprite mVoiceBG;

    protected override void _InitScriptBinder()
    {
        mBtnHide = ScriptBinder.GetObject("BtnHide") as UIEventListener;
        mBtnTeam = ScriptBinder.GetObject("BtnTeam") as UIEventListener;
        mBtnGuild = ScriptBinder.GetObject("BtnGuild") as UIEventListener;
        mVoiceIcon = ScriptBinder.GetObject("VoiceIcon") as UISprite;
        mVoiceBG = ScriptBinder.GetObject("VoiceBG") as UISprite;
    }


    public override void Init()
    {
        base.Init();
        AddClick();
        mClientEvent.AddEvent(CEvent.YvVoiceLoginType, UpdateLoginMainType);
        mBtnVoice.onClick = OnBtnVoiceClick;
        mBtnHide.onClick = OnBtnVoiceClick;
        mBtnTeam.parameter = VoiceLoginType.team;
        mBtnTeam.onClick = OnChooseVoiceClick;
        mBtnGuild.parameter = VoiceLoginType.union;
        mBtnGuild.onClick = OnChooseVoiceClick;

        UpdateLoginMainType(0, null);
    }

    private void OnChooseVoiceClick(GameObject go)
    {
        VoiceLoginType voiceLogin = (VoiceLoginType)UIEventListener.Get(go).parameter;

        if(!TryLoginVoice(voiceLogin))
        {
            return;
        }

        VoiceChatManager.Instance.Login(voiceLogin, () =>
        {
            if (YvVoiceMgr.Instance.mLoginType !=  (int)VoiceLoginType.None)
            {
                UtilityTips.ShowGreenTips(1847);
                mVoiceLab.text = GetVoiceName();
                mOption.PlayReverse();
                ScriptBinder.Invoke(mOption.duration + 0.2f, OpenVoicePanel);
            }
            else
                UtilityTips.ShowRedTips(334);
        });
    }

    protected bool TryLoginVoice(VoiceLoginType voiceLoginType)
    {
        if (voiceLoginType == VoiceLoginType.None)
            return true;

        if(voiceLoginType == VoiceLoginType.union)
        {
            if (!Utility.HasGuild())
            {
                UtilityTips.ShowRedTips(335);
                return false;
            }

            int curValue = Utility.GetGuildLevel();
            int limitValue = CSChatManager.Instance.GetGuildVoiceGuildLevelLimit();
            if(limitValue > curValue)
            {
                UtilityTips.ShowRedTips(CSString.Format(336,limitValue));
                return false;
            }

            return true;
        }

        if(voiceLoginType == VoiceLoginType.team)
        {
            if (!Utility.HasTeam())
            {
                UtilityTips.ShowRedTips(337);
                return false;
            }

            return true;
        }

        return true;
    }

    private void OpenVoicePanel()
    {
        OnBtnVoiceClick(null);
    }

    private void OnBtnVoiceClick(GameObject go)
    {
        int num = 4;
        CurVoiceLoginType = VoiceLoginType.team;
        if (CurVoiceLoginType != VoiceLoginType.None)
        {
            if (mOption.transform.localScale.y == 0)
            {
                if (mBtnGuild.gameObject.activeSelf)
                    mBtnGuild.gameObject.SetActive(false);
                if (mBtnTeam.gameObject.activeSelf)
                    mBtnTeam.gameObject.SetActive(false);

                if (!mBtnExit.gameObject.activeSelf)
                    mBtnExit.gameObject.SetActive(true);
                if (!mBtnPublish.gameObject.activeSelf)
                    mBtnPublish.gameObject.SetActive(true);
                if (!mBtnUpMic.gameObject.activeSelf)
                    mBtnUpMic.gameObject.SetActive(true);
                if (CurVoiceLoginType == VoiceLoginType.team)
                {
                    mBtnCall.gameObject.SetActive(false);
                    mBtnTableList.gameObject.SetActive(false);
                    num = 3;
                }
                else
                {
                    mBtnCall.gameObject.SetActive(true);
                    mBtnTableList.gameObject.SetActive(true);
                    num = 5;
                }
                if (!mOption.enabled)
                    mOption.enabled = true;
                if (YvVoiceMgr.Instance.isOpenVoiceSpeak)
                    mSpUpMic.spriteName = "chat_bi6";
                else
                    mSpUpMic.spriteName = "chat_bi8";

                mOptionBG.height = num * 32 + 14;
                mOption.PlayForward();
                mOptionTable.Reposition();
            }
            else
            {
                mOption.PlayReverse();
            }
        }
        else
        {
            num = 2;
            if (mOption.transform.localScale.y == 0)
            {
                if (!mBtnGuild.gameObject.activeSelf)
                    mBtnGuild.gameObject.SetActive(true);
                if (!mBtnTeam.gameObject.activeSelf)
                    mBtnTeam.gameObject.SetActive(true);

                if (mBtnExit.gameObject.activeSelf)
                    mBtnExit.gameObject.SetActive(false);
                if (mBtnPublish.gameObject.activeSelf)
                    mBtnPublish.gameObject.SetActive(false);
                if (mBtnUpMic.gameObject.activeSelf)
                    mBtnUpMic.gameObject.SetActive(false);
                if (mBtnCall.gameObject.activeSelf)
                    mBtnCall.gameObject.SetActive(false);
                if (mBtnTableList.gameObject.activeSelf)
                    mBtnTableList.gameObject.SetActive(false);
              
                if (!mOption.enabled)
                    mOption.enabled = true;
                mOptionBG.height = num * 32 + 14;

                mOption.PlayForward();
                mOptionTable.Reposition();
            }
            else
            {
                mOption.PlayReverse();
            }
        }
       
    }

    public override void OnExitClick(GameObject go)
    {
        VoiceChatManager.Instance.Logout(() =>
        {
            UtilityTips.ShowGreenTips(1848);
            mOption.PlayReverse();
            CurVoiceLoginType = VoiceLoginType.None;
            mVoiceLab.text = CSString.Format(101548);
        });
    }

    protected void UpdateLoginMainType(uint id, object argv)
    {
        CurVoiceLoginType = (VoiceLoginType)YvVoiceMgr.Instance.mLoginType;
        if (mOption.transform.localScale.y == 1)
        {
            mOption.PlayReverse();
        }
        if (CurVoiceLoginType != VoiceLoginType.None)
            mVoiceIcon.spriteName = "chat_bi3";
        else
            mVoiceIcon.spriteName = "chat_bi5";

        mVoiceLab.text = GetVoiceName();
        if (CurVoiceLoginType != VoiceLoginType.None)
        {
            UtilityTips.ShowTips(100109, 1.5f, ColorType.Yellow, GetVoiceName());
        }
    }
}
