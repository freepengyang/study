using UnityEngine;

public partial class UIChatVoiceMenuPanel : UIBasePanel
{
    UIChatPanel _uiChatPanel;
    protected UIChatPanel ChatPanel
    {
        get
        {
            if(null == _uiChatPanel)
            {
                _uiChatPanel = UIManager.Instance.GetPanel<UIChatPanel>();
            }
            return _uiChatPanel;
        }
    }

    public override void Init()
    {
        base.Init();

        mBtnVoice.onClick = OnVoiceClick;
        mBtnVoiceWorld.onClick = OnVoiceWorldClick;
        mBtnVoiceGuild.onClick = OnVoiceGangClick;
        mbtn_voice_Neighbouring.onClick = OnVoiceNeighbouringClick;
        mbtn_voice_Team.onClick = OnVoiceTeamClick;
        mBtnVoice.onPress = OnVoicePress;
        mBtnVoice.onDrag = OnDrag;
    }

    protected override void OnDestroy()
    {
        _uiChatPanel = null;
        base.OnDestroy();
    }

    private void OnVoiceClick(GameObject g)
    {
        SetOptionTween();
    }

    private void SetOptionTween()
    {
        if (mOption.transform.localScale.y == 0)
        {
            if (!mOption.enabled)
                mOption.enabled = true;
            mOption.PlayForward();
            mOptionTable.Reposition();
            mArrow.transform.Rotate(new Vector3(0, 0, 180));
        }
        else
        {
            mOption.PlayReverse();
            mArrow.transform.Rotate(new Vector3(0, 0, -180));
        }
    }

    private ChatType channel = ChatType.CT_WORLD;

    private void OnVoiceWorldClick(GameObject go)
    {
        if (go == null) return;
        channel = ChatType.CT_WORLD;
        mVoiceSprite.spriteName = go.GetComponent<UISprite>().spriteName;
        go.SetActive(false);

        //mBtnVoiceWorld.CustomActive(true);
        mbtn_voice_Neighbouring.CustomActive(true);
        mbtn_voice_Team.CustomActive(true);
        mBtnVoiceGuild.CustomActive(true);

        mOptionTable.Reposition();
        SetOptionTween();
    }
    private void OnVoiceGangClick(GameObject go)
    {
        if (go == null) return;
        channel = ChatType.CT_GUILD;
        mVoiceSprite.spriteName = go.GetComponent<UISprite>().spriteName;
        go.SetActive(false);

        mBtnVoiceWorld.CustomActive(true);
        mbtn_voice_Neighbouring.CustomActive(true);
        mbtn_voice_Team.CustomActive(true);
        //mBtnVoiceGuild.CustomActive(true);

        mOptionTable.Reposition();
        SetOptionTween();
    }

    private void OnVoiceTeamClick(GameObject go)
    {
        if (go == null) return;
        channel = ChatType.CT_TEAM;
        mVoiceSprite.spriteName = go.GetComponent<UISprite>().spriteName;
        go.SetActive(false);

        mBtnVoiceWorld.CustomActive(true);
        mbtn_voice_Neighbouring.CustomActive(true);
        //mbtn_voice_Team.CustomActive(true);
        mBtnVoiceGuild.CustomActive(true);

        mOptionTable.Reposition();
        SetOptionTween();
    }

    private void OnVoiceNeighbouringClick(GameObject go)
    {
        if (go == null) return;
        channel = ChatType.CT_NEIGHBOURING;
        mVoiceSprite.spriteName = go.GetComponent<UISprite>().spriteName;
        go.SetActive(false);

        mBtnVoiceWorld.CustomActive(true);
        //mbtn_voice_Neighbouring.CustomActive(true);
        mbtn_voice_Team.CustomActive(true);
        mBtnVoiceGuild.CustomActive(true);

        mOptionTable.Reposition();
        SetOptionTween();
    }

    private float longClickDuraction = 0.6f;
    private void OnVoicePress(GameObject go, bool pressed)
    {
        if (ChatPanel != null)
        {
            if (pressed)
            {
                ScriptBinder.StopInvoke();
                ScriptBinder.Invoke(longClickDuraction, OnVoicePressSchedule);
            }
            else
            {
                ChatPanel.OnVoiceClick(false, Vector3.zero, channel);
                ScriptBinder.StopInvoke();
            }
        }
    }

    protected void OnVoicePressSchedule()
    {
        if(null != ChatPanel)
        {
            ChatPanel.OnVoiceClick(true, new Vector3(478, 0, 0), channel);
        }
    }

    Bounds boundGang;
    Bounds boundWorld;
    Bounds boundNation;

    private void OnDrag(GameObject go, Vector2 delta)
    {
        if (boundGang.Contains(Input.mousePosition) || boundWorld.Contains(Input.mousePosition) || boundNation.Contains(Input.mousePosition))
        {
            if (ChatPanel != null)
            {
                ChatPanel.isCancel = false;
            }
        }
        else
        {
            if (ChatPanel != null)
            {
                ChatPanel.isCancel = true;
            }
        }
    }
}