using System;
using UnityEngine;

public enum VoiceLoginType
{
    None = 0,
    team = 1,
    union = 2,
}

public class VoiceChatManager : CSInfo<VoiceChatManager>
{
    private string openId = ""; //登录ID
    public int openServerBanYvVoice = 3; //开服天数限制
    public int openServerVipLevelYvVoice = 3; //开服 vip限制
    public int guildLevelLimit = 15;//行会语音限制

    //public GameObject GuildeVoiceIcon;
    //public GameObject TeamVoiceIcon;
    private void SpeakMemberChanged()
    {
        FNDebug.LogFormat($"<color=#00ff00>[语音聊天]:发送事件 YvVoiceUpmicPlayerChanged</color>");
        HotManager.Instance.EventHandler.SendEvent(CEvent.YvVoiceUpmicPlayerChanged);
    }

    private void VoiceSpeakChange(bool isOpen)
    {
        if (!isOpen)
            CSAudioMgr.Instance.EnableAudioMgr(true);
        HotManager.Instance.EventHandler.SendEvent(CEvent.YvVoiceSpeakState);
    }

    private void LoginTypeChange()
    {
        HotManager.Instance.EventHandler.SendEvent(CEvent.YvVoiceLoginType);
    }

    private void ShowTips(int id, string paramsStr)
    {
        if (id == 1604)
        {
            UtilityTips.ShowRedTips(id, GetRoomName((VoiceLoginType)YvVoiceMgr.Instance.mLoginType));
            return;
        }
        
        if (string.IsNullOrEmpty(paramsStr))
            UtilityTips.ShowRedTips(id);
        else
            UtilityTips.ShowRedTips(id, paramsStr);
    }

    private void LoginRoom(int loginType)
    {
        Net.ChatJoinVoiceRoomReqMessage(loginType);
        RefushBtnState();
    }

    public void LogoutRoom(int loginType)
    {
        Net.ChatLeaveVoiceRoomReqMessage(loginType);
        RefushBtnState();
    }

    public VoiceChatManager()
    {
        //注释语音
        //现在没有特殊场景需要关闭实时语音，，所以先注释，以后有需求时，再打开
        //mClientEvent.Reg((uint) CEvent.Scene_Change, OnChangeScene);
        mClientEvent.AddEvent(CEvent.MainPlayerCanSpeak, MainPlayerCanSpeak);
        mClientEvent.AddEvent(CEvent.OnMainPlayerGuildIdChanged, OnMainPlayerGuildIdChanged);
        mClientEvent.AddEvent(CEvent.OnMainPlayerTeamIdChanged, OnMainPlayerTeamIdChanged);
    }

    public void Init()
    {
        if (!isAllowYvVoice(false))
            return;
        openId = CSMainPlayerInfo.Instance.ID.ToString();
        if (string.IsNullOrEmpty(openId))
            openId = CSServerTime.Instance.ServerNowsMilli.ToString();
        AddCallBack();
        YvVoiceMgr.Instance.Init(openId);
    }

    private void InitValue()
    {
        int.TryParse(SundryTableManager.Instance.GetSundryEffect(672), out openServerBanYvVoice);
        int.TryParse(SundryTableManager.Instance.GetSundryEffect(673), out openServerVipLevelYvVoice);
        int.TryParse(SundryTableManager.Instance.GetSundryEffect(674), out guildLevelLimit);
    }

    private void AddCallBack()
    {
        YvVoiceMgr.Instance.SpeakingMemberChangedCB = SpeakMemberChanged;
        YvVoiceMgr.Instance.VoiceSpeakChangeCallBack = VoiceSpeakChange;
        YvVoiceMgr.Instance.LoginRoomCallBack = LoginRoom;
        YvVoiceMgr.Instance.LogoutRoomCallBack = LogoutRoom;
        YvVoiceMgr.Instance.ShowTipsCallBack = ShowTips;
        YvVoiceMgr.Instance.LoginTypeChangeCallBack = LoginTypeChange;
    }

    public void Login(VoiceLoginType type, System.Action response = null)
    {
        try
        {
            if (YvVoiceMgr.Instance.mLoginType == (int) type)
            {
                //发现这东西不置空，会导致发送事件错误
                response = null;
                return;
            }

            if (YvVoiceMgr.Instance.mLoginType != 0)
            {
                YvVoiceMgr.Instance.Logout(() => { Login(type, response); });
                return;
            }

            string roomID = "";
            switch (type)
            {
                case VoiceLoginType.union:
                    if (CSMainPlayerInfo.Instance.GuildId == 0)
                    {
                        UtilityTips.ShowRedTips(1605);
                        response = null;
                        return;
                    }

                    if (CSMainPlayerInfo.Instance.GuildLevel < guildLevelLimit)
                    {
                        UtilityTips.ShowRedTips(1606);
                        response = null;
                        return;
                    }

                    roomID = CSMainPlayerInfo.Instance.GuildId.ToString();
                    break;
                case VoiceLoginType.team:
                    if (CSMainPlayerInfo.Instance.TeamId == 0)
                    {
                        UtilityTips.ShowRedTips(1607);
                        response = null;
                        return;
                    }

                    roomID = CSMainPlayerInfo.Instance.TeamId.ToString();
                    break;
            }

            YvVoiceMgr.isRuningRecord = false;
            YvVoiceMgr.Instance.Login((int) type, CSMainPlayerInfo.Instance.Name, CSConstant.loginName,
                CSMainPlayerInfo.Instance.Level, roomID,
                response);
        }
        catch
        {
        }
    }

    public void Logout(System.Action response = null, bool isLogoutGame = false)
    {
        YvVoiceMgr.Instance.Logout(response, isLogoutGame);
    }

    public bool PlayAudio(string url, System.Action response = null)
    {
        try
        {
            if (!isAllowYvVoice(false))
            {
                response = null;
                return false;
            }

            if (isBanAllVoice) //特殊场景,不允许播放语音
            {
                response = null;
                UtilityTips.ShowRedTips(1602);
                return false;
            }

            return YvVoiceMgr.Instance.PlayAudio(url, response);
        }
        catch
        {
        }

        return false;
    }
    
    public void StopRecord()
    {
        YvVoiceMgr.Instance.StopRecord();
    }

    #region 按钮状态的处理

    public void RefushBtnState()
    {
        //if (GuildeVoiceIcon != null)
        //{
        //    GuildeVoiceIcon.SetActive(YvVoiceMgr.Instance.mLoginType == (int) VoiceLoginType.union);
        //}

        //if (TeamVoiceIcon != null)
        //    TeamVoiceIcon.SetActive(YvVoiceMgr.Instance.mLoginType == (int) VoiceLoginType.team);
        FNDebug.LogFormat("<color=#00ff00>[刷新语音按钮状态]</color>");
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnVoiceStateChanged);
    }

    #endregion


    public void StartRecord(string savePath, string extend, int _channel, long sendToName,
        System.Action<string> luYingResponse = null, System.Action<string> UploadResponse = null)
    {
        try
        {
            if (!isAllowYvVoice())
            {
                luYingResponse = null;
                UploadResponse = null;
                return;
            }

            if (isBanAllVoice) //特殊场景,不允许播放语音
            {
                UtilityTips.ShowRedTips(1602);
                luYingResponse = null;
                UploadResponse = null;
                return;
            }

            if (YvVoiceMgr.isRuningRecord)
            {
                UtilityTips.ShowTips(1608);
                luYingResponse = null;
                UploadResponse = null;
                return;
            }

            YvVoiceMgr.isRuningRecord = true;


            string name = CSMainPlayerInfo.Instance.Name;
            long id = CSMainPlayerInfo.Instance.ID;
            YvVoiceMgr.Instance.StartRecord(luYingResponse, UploadResponse,
                (convertCode, convertFileId, convertResult, fileid, voiceduration) =>
                {
                    if (convertCode == 0)
                    {
                        //先进行AES加密,然后从加密后的字段中截取部分作为翻译文章的加密key
                        extend += "#" + convertResult;
                        string AESKey =
                            UtilityEncrypt.GeneraKey(
                                UtilityEncrypt.AESEncrypt(fileid),
                                CSMainPlayerInfo.Instance.ID.ToString());
                        extend = UtilityEncrypt.AESEncrypt(extend, AESKey);
                        //服务器要求，第五位一定是翻译内容，发送给后台
                        string message = $"{name}${id}${fileid}${voiceduration}${convertResult}${extend}";
                        Net.ReqChatMessage(_channel, message, sendToName, 1);
                    }
                    else
                    {
                        //先进行AES加密,然后从加密后的字段中截取部分作为翻译文章的加密key
                        extend += "#" + convertResult;
                        string AESKey =
                            UtilityEncrypt.GeneraKey(
                                UtilityEncrypt.AESEncrypt(fileid),
                                CSMainPlayerInfo.Instance.ID.ToString());
                        extend = UtilityEncrypt.AESEncrypt(extend, AESKey);
                        string message = $"{name}${id}${fileid}${voiceduration}${convertResult}${extend}";
                        Net.ReqChatMessage(_channel, message, sendToName, 1);
                    }
                });
        }
        catch
        {
            YvVoiceMgr.isRuningRecord = false;
        }
    }


#region 场景切换后,判断新场景的语音开放状态

    bool notesLister;
    bool notesSpeak;
    bool isBanAllVoice = false;
    bool isBanRealTimeVoice = false;

    private void OnChangeScene(uint uiEvtID, object data)
    {
        if (data == null) return;
        int mapId;
        int.TryParse(data.ToString(), out mapId);
        SpecialScene(mapId);
    }

    public void SpecialScene(int sceneID)
    {
        IsBanAllVoiceScene(sceneID);
        IsBanRealTimeVoiceScene(sceneID);
    }

    void IsBanAllVoiceScene(int sceneID)
    {
        if (IntIsInIntlist(sceneID, BanAllVoiceScene))
        {
            if (!isBanAllVoice)
            {
                UtilityTips.ShowRedTips(1614);
                //记录当前状态
                notesSpeak = YvVoiceMgr.Instance.isOpenVoiceSpeak;
                notesLister = YvVoiceMgr.Instance.isOpenVoiceLister;
                if (YvVoiceMgr.Instance.isOpenVoiceLister)
                    SwitchVoiceListerState();
                if (YvVoiceMgr.Instance.isOpenVoiceSpeak)
                    SwitchVoiceSpeakState();
                isBanAllVoice = true;
            }

            return;
        }

        if (isBanAllVoice) //之前切换过了特殊场景
        {
            isBanAllVoice = false;

            if (notesLister)
                SwitchVoiceListerState();
            if (notesSpeak)
                SwitchVoiceSpeakState();
        }
    }

    void IsBanRealTimeVoiceScene(int sceneID)
    {
        if (IntIsInIntlist(sceneID, BanRealTimeVoice))
        {
            if (!isBanRealTimeVoice)
            {
                UtilityTips.ShowRedTips(1615);
                //记录当前状态
                notesSpeak = YvVoiceMgr.Instance.isOpenVoiceSpeak;
                notesLister = YvVoiceMgr.Instance.isOpenVoiceLister;
                if (YvVoiceMgr.Instance.isOpenVoiceLister)
                    SwitchVoiceListerState();
                if (YvVoiceMgr.Instance.isOpenVoiceSpeak)
                    SwitchVoiceSpeakState();
                isBanRealTimeVoice = true;
            }

            return;
        }

        if (isBanRealTimeVoice) //之前切换过了特殊场景
        {
            isBanRealTimeVoice = false;

            if (notesLister)
                SwitchVoiceListerState();
            if (notesSpeak)
                SwitchVoiceSpeakState();
        }
    }

    public bool IntIsInIntlist(int num, int[] nums)
    {
        for (int i = 0; i < nums.Length; i++)
        {
            if (nums[i] == num) return true;
        }

        return false;
    }

    public int[] BanAllVoiceScene = new int[] {};
    public int[] BanRealTimeVoice = new int[] {};

     #endregion

    private void MainPlayerCanSpeak(uint uiEvtID, object argv)
    {
        if (CSMainPlayerInfo.Instance == null) return;
        if (YvVoiceMgr.Instance.mLoginType == (int) VoiceLoginType.union && !CSMainPlayerInfo.Instance.CanSpeak &&
            YvVoiceMgr.Instance.isOpenVoiceSpeak)
        {
            SwitchVoiceSpeakState();
        }
    }

    private void OnMainPlayerGuildIdChanged(uint uiEvtID, object argv)
    {
        if(YvVoiceMgr.Instance.mLoginType == (int)VoiceLoginType.union)
        {
            if(CSMainPlayerInfo.Instance.GuildId == 0)
            {
                FNDebug.LogFormat("<color=#00ff00>[语音聊天]:退出行会,关闭语音</color>");
                Logout(()=>
                {
                    UtilityTips.ShowGreenTips(1848);
                }, false);
                //YvVoiceMgr.Instance.isOpenVoiceSpeak = false;
            }
        }
    }

    private void OnMainPlayerTeamIdChanged(uint uiEvtID, object argv)
    {
        if (YvVoiceMgr.Instance.mLoginType == (int)VoiceLoginType.team)
        {
            if (CSMainPlayerInfo.Instance.TeamId == 0)
            {
                FNDebug.LogFormat("<color=#00ff00>[语音聊天]:退出队伍,关闭语音</color>");
                Logout(()=>
                {
                    UtilityTips.ShowGreenTips(1848);
                }, false);
                //YvVoiceMgr.Instance.isOpenVoiceSpeak = false;
            }
        }
    }


    public string GetRoomName(VoiceLoginType mVoiceLoginType)
    {
        switch (mVoiceLoginType)
        {
            case VoiceLoginType.union:
                return CSString.Format(404);
            case VoiceLoginType.team:
                return CSString.Format(405);
        }

        return "";
    }

    public void SwitchVoiceSpeakState(System.Action response = null, bool ForceClose = false, bool isShowTips = true)
    {
        try
        {
            if (YvVoiceMgr.Instance.mLoginType == (int) VoiceLoginType.union) //公会模式下需要检查语音权限
            {
                if (CSMainPlayerInfo.Instance.GuildPos != (int)GuildPos.President && !CSMainPlayerInfo.Instance.CanSpeak && !YvVoiceMgr.Instance.isOpenVoiceSpeak)
                {
                    if (isShowTips)
                        UtilityTips.ShowRedTips(373);
                    response = null;
                    return;
                }
            }

            if (isBanAllVoice) //特殊场景,不允许播放语音
            {
                UtilityTips.ShowRedTips(374);
                response = null;
                return;
            }

            if (isBanRealTimeVoice)
            {
                UtilityTips.ShowRedTips(375);
                response = null;
                return;
            }

            if (ForceClose) //强制关闭语音
            {
                YvVoiceMgr.Instance.StopVoice();
                CSAudioMgr.Instance.EnableAudioMgr(true); //开启背景音效跟特效音
                return;
            }

            if (!YvVoiceMgr.Instance.isLogin)
            {
                UtilityTips.ShowRedTips(376);
            }
            else
            {
                bool result = YvVoiceMgr.Instance.OpenVoice();
                if (result)
                {
                    CSAudioMgr.Instance.EnableAudioMgr(!YvVoiceMgr.Instance.isOpenVoiceSpeak); //实时语音开始时候,背景音关闭
                    if (response != null)
                        response();

                    RefushBtnState();
                }
                else
                {
                    UtilityTips.ShowRedTips(377);
                }
            }
        }
        catch
        {
        }
    }

    public void SwitchVoiceListerState()
    {
        try
        {
            if (!isAllowYvVoice(false))
            {
                return;
            }

            if (isBanAllVoice) //特殊场景,不允许播放语音
            {
                UtilityTips.ShowRedTips(1602);
                return;
            }

            if (isBanRealTimeVoice)
            {
                UtilityTips.ShowRedTips(1603);
                return;
            }

            if (YvVoiceMgr.Instance.isLogin)
            {
                YvVoiceMgr.Instance.EnableSpeaker();
                RefushBtnState();
            }
        }
        catch
        {
        }
    }

    public bool isAllowYvVoice(bool isShowTips = true)
    {
#if UNITY_ANDROID
        if (!QuDaoConstant.OpenVoice)
        {
            if (isShowTips)
                UtilityTips.ShowTips(1600);

            FNDebug.LogFormat("<color=#ff0000>[语音聊天]:渠道关闭了语音</color>");
            return false;
        }

        CSMainPlayerInfo playerInfo = CSMainPlayerInfo.Instance;
        if (playerInfo != null && (playerInfo.RoleExtraValues != null
                                   && playerInfo.RoleExtraValues.openServerDays < openServerBanYvVoice))
        {
            if (playerInfo.VipLevel < openServerVipLevelYvVoice)
            {
                if (isShowTips)
                {
                    UtilityTips.ShowRedTips(1601, openServerVipLevelYvVoice, openServerBanYvVoice);
                }
                FNDebug.LogFormat("<color=#ff0000>[语音聊天]:语音VIP等级不足[{0}]级 或者开服天数不做[{1}]天</color>",openServerVipLevelYvVoice,openServerBanYvVoice);
                return false;
            }
            else
                return true;
        }
        else
        {
            return true;
        }
#else
        if (!QuDaoConstant.OpenVoice) 
        {
            Debug.LogFormat("<color=#ff0000>[语音聊天]:渠道关闭了语音</color>");
            return false;
        }
        return true;
#endif
    }
    
    public bool PlayChatVoice(string url, System.Action finish)
    {
        if (!QuDaoConstant.OpenVoice)
        {
            UtilityTips.ShowRedTips(1741);
            return false;
        }

        YvVoiceMgr.Instance.StopAudio();

        CSAudioMgr.Instance.EnableAudioMgr(false);//下载成功之后自动播放，并停止背景音效

        YvVoiceMgr.Instance.PlayAudio(url, () => {
            if(finish != null)
            {
                finish();
            }
            CSAudioMgr.Instance.EnableAudioMgr(true);//语音播放完成之后，继续背景音效
        });

        return true;
    }
    
    public void StopChatVoice()
    {
        CSAudioMgr.Instance.EnableAudioMgr(true);//语音播放完成之后，继续背景音效
    }

    public override void Dispose()
    {
    }
}