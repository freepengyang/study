using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class UIColoursWorldPanel : UIBasePanel
{
    Bounds boundWorld;
    private bool _isCancel = false;
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
            mBtnMic.gameObject.SetActive(!value);
            mBtnVoiceCancel.gameObject.SetActive(value);
            mVoiceText.text = CSString.Format(value == false, 101634, 101635);
        }
    }
    public override UILayerType PanelLayerType
    {
        get
        {
            return UILayerType.Window;
        }
    }

    public override void Init()
    {
        base.Init();

        //TODO:被人杀了
        //mSocketEvent.Reg(ECM.ResKilledByMessage, ClosePanel);

        mBtnClose.onClick = OnClosePanelClick;
        mBtnSend.onClick = OnSendMsgClick;
        mBtnVoice.onPress = OnSendVoiceClick;
        mBtnVoice.onDrag = OnDrag;
        mInput.OnReachLimit = InputIsLimit;
    }


    public override void Show()
    {
        base.Show();
    }
    
    //文字达到上限
    private void InputIsLimit()
    {
        UtilityTips.ShowRedTips(388);
    }


    private void OnDrag(GameObject go, Vector2 delta)
    {
        if (boundWorld.Contains(Input.mousePosition))
        {
            isCancel = false;
        }
        else
        {
            isCancel = true;
        }
    }
    //发送语音消息
    private void OnSendVoiceClick(GameObject go, bool state)
    {
        if (state)
        {
            if(!CSChatManager.Instance.IsColoredWorldMsgCoolDown())
            {
                return;
            }
        }

        Vector3 posWorld = UICamera.currentCamera.WorldToScreenPoint(mBtnVoice.transform.position);
        boundWorld = new Bounds(posWorld, new Vector3(150, 150, 1));

        OnVoiceClick(state, new Vector3(300, 0, 0), 11);
    }



    //发送消息
    private void OnSendMsgClick(GameObject go)
    {
        if (!CSChatManager.Instance.IsColoredWorldMsgCoolDown())
        {
            return;
        }

        if (string.IsNullOrEmpty(mInput.value))
        {
            UtilityTips.ShowRedTips(387);
            return;
        }
        SendColoursWorldMsg();
    }

    private void SendColoursWorldMsg()
    {
        OnClosePanelClick(null);
        string value = mInput.value.Replace("\n", "");
        value = value.Replace("\\n", "");
        Net.ReqChatMessage((int)ChatType.CT_COLORED, value, 0);
    }
    private void OnCallbackFeather(PromptState state, bool b)
    {
        if (state == PromptState.leftBtn)
        {

        }
        else if (state == PromptState.rightBtn)
        {
            if (b)
            {
                Constant.isShowColoursWorld = !b;
            }
            //发送消息
            Net.ReqChatMessage((int)ChatType.CT_COLORED, mInput.value, 0);

            OnClosePanelClick(null);
        }
    }

    private void ClosePanel(uint uiEvtId, params object[] data)
    {
        OnClosePanelClick(null);
    }

    private void OnClosePanelClick(GameObject go)
    {
        UIManager.Instance.ClosePanel<UIColoursWorldPanel>();
    }

    public void OnVoiceClick(bool isPressed, Vector3 targetPosition, int channel)
    {
        CSPlayerInfo mPlayerInfo = CSMainPlayerInfo.Instance;

        long teamOrUnionId = 0;

        if (isPressed)
        {
            if (YvVoiceMgr.isRuningRecord)
            {
                UtilityTips.ShowTips(385);
                return;
            }

            isCancel = false;
            CSAudioMgr.Instance.EnableAudioMgr(false);//开始录音，停止背景音效跟特效音
            mMic.transform.localPosition = targetPosition;

            string textInfo = "0";
            if (CSMainPlayerInfo.Instance != null &&
                   //CSMainPlayerInfo.Instance.PrayInfo != null &&
                   CSMainPlayerInfo.Instance.RoleExtraValues != null &&
                  (CSMainPlayerInfo.Instance.RoleExtraValues.vipExp > 0 || CSMainPlayerInfo.Instance.VipLevel > 0))
            {
                textInfo = "1";
            }
            string Path = Application.persistentDataPath + "/";
            VoiceChatManager.Instance.StartRecord(Path, (teamOrUnionId.ToString() + "#" + textInfo + "#" + CSMainPlayerInfo.Instance.ID), channel, 0,
                (msg1) =>
                {
                    YvVoiceMgr.Instance.isCancelLuying = isCancel;
                });
        }
        else
        {
            CSAudioMgr.Instance.EnableAudioMgr(true);//手指抬起，继续背景音效跟特效音
            mMic.transform.localPosition = new Vector3(-10000f, 0f, 0f);
            VoiceChatManager.Instance.StopRecord();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
