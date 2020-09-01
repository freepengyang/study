using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using TencentMobileGaming;


public class YvVoiceMgr
{
    public bool isLogin = false; //是否进入了房间
    public bool _isOpenVoiceSpeak = false;
    public Action<bool> VoiceSpeakChangeCallBack;
    public Action LoginTypeChangeCallBack;
    public Action<int> LoginRoomCallBack;
    public Action<int> LogoutRoomCallBack;
    public Action<int, string> ShowTipsCallBack;

    public bool isOpenVoiceSpeak
    {
        get { return _isOpenVoiceSpeak; }
        set
        {
            _isOpenVoiceSpeak = value;
            //实时语音关闭时，开启背景音乐
            if (VoiceSpeakChangeCallBack != null)
                VoiceSpeakChangeCallBack(_isOpenVoiceSpeak);
        }
    }

    public bool _isOpenVoiceLister = true;

    public bool isOpenVoiceLister
    {
        get { return _isOpenVoiceLister; }
        set { _isOpenVoiceLister = value; }
    }

    public bool isCancelLuying = false;

    public bool isLoginRes = false; //避免在登入界面反复点击造成重复登入语音房间导致出现异常

    private string filePath = string.Empty;
    private string filePathUrl = string.Empty;
    private string voiceUrl = string.Empty;
    private int uploadResult = -1;
    private int duration = 0;
    private string openId = ""; //登录ID
    private int readyLoginType = 0; //准备登录的类型
    private System.Action VoiceResponse = null; //语音传入的回调
    public event Action VoicePlayerUpdateRoom; //玩家进入登出房间通知
    private bool voiceIsInit = false; //控制Poll事件调用

    //上麦人员
    private AudioStreamNumber roomAudioNumber = new AudioStreamNumber();


    private static YvVoiceMgr mInstance;

    public static YvVoiceMgr Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new YvVoiceMgr();
            return mInstance;
        }
        set { mInstance = value; }
    }

    public int _mLoginType = 0; //当前登入的类型

    public int mLoginType
    {
        get { return _mLoginType; }
        set
        {
            if (_mLoginType == value) return;
            _mLoginType = value;
            if (LoginTypeChangeCallBack != null)
                LoginTypeChangeCallBack();
        }
    }

    private void AddITMGClick()
    {
        FNDebug.LogFormat("<color=#ff0000>[注册语音回调]</color>");
        ITMGContext.GetInstance().OnEnterRoomCompleteEvent -= OnEnterRoomComplete;
        ITMGContext.GetInstance().OnExitRoomCompleteEvent -= OnExitRoomComplete;
        ITMGContext.GetInstance().OnRoomDisconnectEvent -= OnRoomDisconnect;
        ITMGContext.GetInstance().OnEndpointsUpdateInfoEvent -= OnEndpointsUpdateInfo;
        ITMGContext.GetInstance().onEventCallBack -= OnEventCallBack;
        ITMGContext.GetInstance().OnEnterRoomCompleteEvent += OnEnterRoomComplete;
        ITMGContext.GetInstance().OnExitRoomCompleteEvent += OnExitRoomComplete;
        ITMGContext.GetInstance().OnRoomDisconnectEvent += OnRoomDisconnect;
        ITMGContext.GetInstance().OnEndpointsUpdateInfoEvent += OnEndpointsUpdateInfo;
        ITMGContext.GetInstance().onEventCallBack += OnEventCallBack;
    }


    #region Update 语音相关任务队列处理

    public void Update()
    {
        UpdateAutoPlayAudio(); //处理语音自动播放队列
        //通过在 update 里面周期的调用 Poll 可以触发事件回调
        if (voiceIsInit)
            ITMGContext.GetInstance().Poll();
    }

    public static List<string> AutoPlayAudioList = new List<string>();
    bool isAutoPlayingAudio = false;

    void UpdateAutoPlayAudio()
    {
        if (AutoPlayAudioList.Count > 0 && !isAutoPlayingAudio)
        {
            isAutoPlayingAudio = true;
            //腾讯人员：我们服务器有个优化  但是会导致  传入的fileID和 Onevent 里面的不一样，如果 你们有比对的话 ，就给你们加个配置 不换FILEID。
            FileDownloadDelegateImpl filedownloadDelegate = new FileDownloadDelegateImpl(
                delegate(int downCode, string downFilePath, string downFileid)
                {
                    if (downCode == 0)
                    {
                        AudioPlayDelegateImpl audioPlayDelegate = new AudioPlayDelegateImpl(
                            delegate(int code, string filepath)
                            {
                                if (code == 0 && AutoPlayAudioList.Count > 0)
                                {
                                    AutoPlayAudioList.RemoveAt(0);
                                }
                                else
                                {
                                    AutoPlayAudioList.Clear();
                                }

                                isAutoPlayingAudio = false;
                            });
                        audioPlayDelegate.start(downFilePath);
                    }
                    else
                    {
                        isRuningRecord = false;
                        isAutoPlayingAudio = false;
                        if (AutoPlayAudioList.Count > 0)
                        {
                            AutoPlayAudioList.RemoveAt(0);
                        }
                    }
                });
            filePath = Application.persistentDataPath + string.Format("/{0}.silk", sUid++) + "_down";
            filedownloadDelegate.start(AutoPlayAudioList[0], filePath);
        }
    }

    #endregion

    #region 语音接口的调用

    public void Init(string voiceOpenId)
    {
        try
        {
            AddITMGClick();
            openId = voiceOpenId;
            ITMGContext.GetInstance().SetLogLevel(ITMGContext.LOG_LEVEL_INFO, ITMGContext.LOG_LEVEL_NONE);

            int ret = ITMGContext.GetInstance().Init(CSConstant.YvTMGSDKAppId.ToString(), openId);
            if (ret != QAVError.OK)
            {
                FNDebug.Log("语音初始化失败， QAVError： == " + ret);
            }
            else
            {
                voiceIsInit = true;
                byte[] authBuffer =
                    QAVAuthBuffer.GenAuthBuffer(CSConstant.YvTMGSDKAppId, null, openId, CSConstant.YvTMGSDKKey);
                ITMGContext.GetInstance().GetPttCtrl().ApplyPTTAuthbuffer(authBuffer);
            }
        }
        catch
        {
        }
    }

    public void Login(int loginType, string name, string uid, int level, string roomID, System.Action response = null)
    {
        try
        {
            AutoPlayAudioList.Clear();
            isAutoPlayingAudio = false;

            readyLoginType = loginType;
            VoiceResponse = response;
            byte[] authBuffer =
                QAVAuthBuffer.GenAuthBuffer(CSConstant.YvTMGSDKAppId, roomID, openId, CSConstant.YvTMGSDKKey);
            int loginResult = ITMGContext.GetInstance()
                .EnterRoom(roomID, ITMGRoomType.ITMG_ROOM_TYPE_FLUENCY, authBuffer);
            if (loginResult != QAVError.OK)
            {
                Logout();
                //isLogin = false;
                //isOpenVoiceSpeak = false;
                //isOpenVoiceLister = false;
                if (response != null)
                    response();
                //readyLoginType = 0;
                VoiceResponse = null;
            }
        }
        catch
        {
        }
    }

    public void Logout(System.Action response = null, bool isLogoutGame = false)
    {
        try
        {
            isLoginRes = false;
            if (!isLogin) 
                return;
            FNDebug.LogFormat($"<color=#00ff00>[语音]:登出</color>");
            isLogin = false;
            isOpenVoiceSpeak = false;
            isOpenVoiceLister = false;
            readyLoginType = 0;

            AutoPlayAudioList.Clear();
            isAutoPlayingAudio = false;
            VoiceResponse = response;
            mSpeakingMembers.Clear();
            if (voiceIsInit && ITMGContext.GetInstance().IsRoomEntered())
                ITMGContext.GetInstance().ExitRoom();
            //切换账号时反初始化
            if (isLogoutGame)
            {
                if (voiceIsInit)
                {
                    ITMGContext.GetInstance().Uninit();
                    UnInit();
                }
                voiceIsInit = false;
            }
        }
        catch
        {
        }
    }

    public void StopVoice()
    {
        isOpenVoiceSpeak = false;
        ITMGContext.GetInstance().GetAudioCtrl().EnableMic(false);
    }

    public bool OpenVoice()
    {
        int resultState = ITMGContext.GetInstance().GetAudioCtrl().EnableMic(!isOpenVoiceSpeak);
        if (resultState == QAVError.OK)
        {
            isOpenVoiceSpeak = !isOpenVoiceSpeak;
            return true;
        }

        return false;
    }

    public void EnableSpeaker()
    {
        isOpenVoiceLister = !isOpenVoiceLister;
        ITMGContext.GetInstance().GetAudioCtrl().EnableSpeaker(isOpenVoiceLister);
    }


    public static bool isRuningRecord = false;

    public void StartRecord(Action<string> luYingResponse = null, Action<string> UploadResponse = null,
        Action<int, string, string, string, int> action = null)
    {
        AudioRecordDelegateImpl audioRecordDelegate = new AudioRecordDelegateImpl(
            delegate(int code, string filePath)
            {
                if (code == 0)
                {
                    if (luYingResponse != null)
                        luYingResponse("");
                    if (isCancelLuying)
                    {
                        isRuningRecord = false;
                        return;
                    }

                    FileUploadDelegateImpl fileuploadDelegate = new FileUploadDelegateImpl(
                        delegate(int uploadcode, string filepath, string fileid)
                        {
                            if (uploadcode == 0)
                            {
                                uploadResult = 0;
                                FileDownloadDelegateImpl filedownloadDelegate = new FileDownloadDelegateImpl(
                                    delegate(int downCode, string downFilePath, string downFileid)
                                    {
                                        if (downCode == 0)
                                        {
                                            int voiceduration = ITMGContext.GetInstance().GetPttCtrl()
                                                .GetVoiceFileDuration(downFilePath);

                                            ConvertTextDelegateImpl convertTextDelegate =
                                                new ConvertTextDelegateImpl(delegate(int convertCode,
                                                    string convertFileId, string convertResult)
                                                {
                                                    if (action != null)
                                                    {
                                                        action(convertCode, convertFileId, convertResult, fileid,
                                                            voiceduration);
                                                    }

                                                    isRuningRecord = false;
                                                });
                                            convertTextDelegate.start(downFileid);
                                        }
                                        else
                                        {
                                            isRuningRecord = false;
                                            ShowTipsCallBack(1609, "");
                                        }
                                    });
                                filedownloadDelegate.start(fileid, filepath);
                            }
                            else
                            {
                                isRuningRecord = false;
                                uploadResult = -1;
                                ShowTipsCallBack(1610, "");
                            }
                        });
                    fileuploadDelegate.start(filePath);
                }
                else
                {
                    if (ITMGContext.GetInstance().IsRoomEntered())
                    {
                        //ShowTipsCallBack(105565, GetRoomName());
                        ShowTipsCallBack(1604, "");
                    }
                    else if (code == 4103)
                    {
                        ShowTipsCallBack(1611, "");
                    }
                    else
                    {
                        ShowTipsCallBack(1612, Convert.ToString(code));
                    }

                    isRuningRecord = false;
                }
            });
        audioRecordDelegate.start();
    }


    public void StopRecord()
    {
        ITMGContext.GetInstance().GetPttCtrl().StopRecording();
    }

    private int sUid = 0;

    public bool PlayAudio(string url, System.Action response = null)
    {
        try
        {
            FileDownloadDelegateImpl filedownloadDelegate = new FileDownloadDelegateImpl(
                delegate(int downCode, string downFilePath, string downFileid)
                {
                    if (downCode == 0)
                    {
                        isAutoPlayingAudio = true;
                        AudioPlayDelegateImpl audioPlayDelegate = new AudioPlayDelegateImpl(
                            delegate(int code, string filepath)
                            {
                                try
                                {
                                    isAutoPlayingAudio = false;
                                    if (response != null)
                                        response();
                                }
                                catch
                                {
                                }
                            });
                        audioPlayDelegate.start(downFilePath);
                    }
                    else
                    {
                        isRuningRecord = false;
                        ShowTipsCallBack(1609, "");
                    }
                });
            filePath = Application.persistentDataPath + string.Format("/{0}.silk", sUid++) + "_down";
            filedownloadDelegate.start(url, filePath);
            return true;
        }
        catch
        {
        }

        return false;
    }

    public void StopAudio()
    {
        try
        {
            if (AutoPlayAudioList != null) AutoPlayAudioList.Clear();
            isAutoPlayingAudio = false;

            ITMGContext.GetInstance().GetPttCtrl().StopPlayFile();
        }
        catch
        {
        }
    }

    #endregion

    #region 语音回调事件

    public List<long> mSpeakingMembers = new List<long>();
    public Action SpeakingMemberChangedCB;
    public void OnEnterRoomComplete(int result, string error_info)
    {
        if (result == 0)
        {
            FNDebug.LogFormat($"<color=#00ff00>[语音]:进入房间成功</color>");
            mLoginType = readyLoginType;
            isLogin = true;
            isOpenVoiceSpeak = false;
            isOpenVoiceLister = true;
            int speakerResult = ITMGContext.GetInstance().GetAudioCtrl().EnableSpeaker(isOpenVoiceLister);
            if (LoginRoomCallBack != null)
                LoginRoomCallBack(readyLoginType);
            if (VoiceResponse != null)
            {
                VoiceResponse();
                VoiceResponse = null;
            }
        }
        else
        {
            FNDebug.LogFormat($"<color=#ff0000>[语音]:进入房间失败</color>");
            Logout();
            //isLogin = false;
            //isOpenVoiceSpeak = false;
            //isOpenVoiceLister = false;
            VoiceResponse = null;
            //readyLoginType = 0;
        }
    }

    public void OnExitRoomComplete()
    {
        isLogin = false;
        isOpenVoiceSpeak = false;
        if (LogoutRoomCallBack != null)
        {
            LogoutRoomCallBack(mLoginType);
        }

        mLoginType = 0;
        try
        {
            if (VoiceResponse != null)
            {
                VoiceResponse();
                VoiceResponse = null;
            }
        }
        catch
        {
        }
    }

    public void OnRoomDisconnect(int result, string error_info)
    {
    }

    public void OnEndpointsUpdateInfo(int eventID, int count, string[] openIdList)
    {
//只能用if。。。。
        if (eventID == ITMGContext.EVENT_ID_ENDPOINT_ENTER || eventID == ITMGContext.EVENT_ID_ENDPOINT_EXIT)
        {
            if (VoicePlayerUpdateRoom != null)
                VoicePlayerUpdateRoom();
        }
        else if (eventID == ITMGContext.EVENT_ID_ENDPOINT_HAS_AUDIO)
        {
            long id;
            foreach (string openId in openIdList)
            {
                if (long.TryParse(openId, out id))
                {
                    if (!mSpeakingMembers.Contains(id))
                        mSpeakingMembers.Add(id);
                    FNDebug.LogFormat("<color=#00ff00>[语音聊天]:[加入上麦ID:{0}]:[当前上麦人数:{1}]</color>", id, mSpeakingMembers.Count);
                }
            }
            if (SpeakingMemberChangedCB != null)
                SpeakingMemberChangedCB();
        }
        else if (eventID == ITMGContext.EVENT_ID_ENDPOINT_NO_AUDIO)
        {
            long id;
            foreach (string openId in openIdList)
            {
                if (long.TryParse(openId, out id))
                {
                    mSpeakingMembers.Remove(id);
                    FNDebug.LogFormat("<color=#00ff00>[语音聊天]:[移除上麦ID:{0}]:[当前上麦人数:{1}]</color>", id, mSpeakingMembers.Count);
                }
            }
            if (SpeakingMemberChangedCB != null)
                SpeakingMemberChangedCB();
        }
    }

    public void OnEventCallBack(int type, int subType, string data)
    {
        switch (type)
        {
            //房间总人数
            case (int) ITMG_MAIN_EVENT_TYPE.ITMG_MAIN_EVENT_TYPE_NUMBER_OF_USERS_UPDATE:
                //roomUserNum = JsonUtility.FromJson<UserClass>(data);
                break;
            //上麦总人数
            case (int) ITMG_MAIN_EVENT_TYPE.ITMG_MAIN_EVENT_TYPE_NUMBER_OF_AUDIOSTREAMS_UPDATE:
                //roomAudioNumber = JsonUtility.FromJson<AudioStreamNumber>(data);
                break;
        }
    }

    #endregion
    
    private void UnInit()
    {
        FNDebug.LogFormat("<color=#ff0000>[清除语音回调]</color>");
        ITMGContext.GetInstance().OnEnterRoomCompleteEvent -= OnEnterRoomComplete;
        ITMGContext.GetInstance().OnExitRoomCompleteEvent -= OnExitRoomComplete;
        ITMGContext.GetInstance().OnRoomDisconnectEvent -= OnRoomDisconnect;
        ITMGContext.GetInstance().OnEndpointsUpdateInfoEvent -= OnEndpointsUpdateInfo;
        ITMGContext.GetInstance().onEventCallBack -= OnEventCallBack;
        VoiceSpeakChangeCallBack = null;
        LoginTypeChangeCallBack = null;
        LoginRoomCallBack = null;
        LogoutRoomCallBack = null;
        ShowTipsCallBack = null;
        SpeakingMemberChangedCB = null;
    }
}

#region 离线语音处理

class AudioRecordDelegateImpl
{
    static int sUid = 0;
    QAVRecordFileCompleteCallback mHandler = null;
    QAVRecordFileCompleteCallback mInnerHandler = null;
    public AudioRecordDelegateImpl(QAVRecordFileCompleteCallback handler)
    {
        mHandler = handler;
        mInnerHandler = new QAVRecordFileCompleteCallback(delegate (int code, string filePath)
        {
            innerHandlerImpl(code, filePath);
        });
        ITMGContext.GetInstance().GetPttCtrl().OnRecordFileComplete += mInnerHandler;
    }

    public void start()
    {
        string recordPath = Application.persistentDataPath + string.Format("/{0}.silk", sUid++);
        int ret = ITMGContext.GetInstance().GetPttCtrl().StartRecording(recordPath);
        if (ret != 0)
        {
            innerHandlerImpl(-1, "");
        }
    }

    private void innerHandlerImpl(int code, string filePath)
    {
        if (null != mHandler)
        {
            mHandler(code, filePath);
        }
        ITMGContext.GetInstance().GetPttCtrl().OnRecordFileComplete -= mInnerHandler;
    }
}


class AudioStreamingRecordDelegateImpl
{
    static int sUid = 10000;
    QAVStreamingRecognitionCallback mHandler = null;
    QAVStreamingRecognitionCallback mInnerHandler = null;
    public AudioStreamingRecordDelegateImpl(QAVStreamingRecognitionCallback handler)
    {
        mHandler = handler;
        mInnerHandler = new QAVStreamingRecognitionCallback(delegate (int code, string fileid, string filepath, string result)
        {
            innerHandlerImpl(code, fileid, filepath, result);
        });
        ITMGContext.GetInstance().GetPttCtrl().OnStreamingSpeechComplete += mInnerHandler;
    }

    public void start(string language)
    {
        string recordPath = Application.persistentDataPath + string.Format("/{0}.silk", sUid++);
        //int ret = ITMGContext.GetInstance().GetPttCtrl().StartRecordingWithStreamingRecognition(recordPath, "cmn-Hans-CN");
        int ret = ITMGContext.GetInstance().GetPttCtrl().StartRecordingWithStreamingRecognition(recordPath, language);
        if (ret != 0)
        {
            innerHandlerImpl(-1, "", "", "");
        }
    }

    private void innerHandlerImpl(int code, string fileid, string filepath, string result)
    {
        if (null != mHandler)
        {
            mHandler(code, fileid, filepath, result);
        }
        ITMGContext.GetInstance().GetPttCtrl().OnStreamingSpeechComplete -= mInnerHandler;
    }
}


class FileUploadDelegateImpl
{
    QAVUploadFileCompleteCallback mHandler = null;
    QAVUploadFileCompleteCallback mInnerHandler = null;
    public FileUploadDelegateImpl(QAVUploadFileCompleteCallback handler)
    {
        mHandler = handler;
        mInnerHandler = new QAVUploadFileCompleteCallback(delegate (int code, string filepath, string fileid)
        {
            innerHandlerImpl(code, filepath, fileid);
        });
        ITMGContext.GetInstance().GetPttCtrl().OnUploadFileComplete += mInnerHandler;
    }

    public void start(string filePath)
    {
        ITMGContext.GetInstance().GetPttCtrl().UploadRecordedFile(filePath);
    }

    private void innerHandlerImpl(int code, string filePath, string fileId)
    {
        if (null != mHandler)
        {
            mHandler(code, filePath, fileId);
        }
        ITMGContext.GetInstance().GetPttCtrl().OnUploadFileComplete -= mInnerHandler;
    }
}


class FileDownloadDelegateImpl
{
    QAVDownloadFileCompleteCallback mHandler = null;
    QAVDownloadFileCompleteCallback mInnerHandler = null;
    public FileDownloadDelegateImpl(QAVDownloadFileCompleteCallback handler)
    {
        mHandler = handler;
        mInnerHandler = new QAVDownloadFileCompleteCallback(delegate (int code, string filepath, string fileid)
        {
            innerHandlerImpl(code, filepath, fileid);
        });

        ITMGContext.GetInstance().GetPttCtrl().OnDownloadFileComplete += mInnerHandler;
    }

    public void start(string fileId, string filePath)
    {

        ITMGContext.GetInstance().GetPttCtrl().DownloadRecordedFile(fileId, filePath);
    }

    private void innerHandlerImpl(int code, string filePath, string fileId)
    {
        if (null != mHandler)
        {
            mHandler(code, filePath, fileId);
        }

        ITMGContext.GetInstance().GetPttCtrl().OnDownloadFileComplete -= mInnerHandler;
    }
}

class AudioPlayDelegateImpl
{
    QAVPlayFileCompleteCallback mHandler = null;
    QAVPlayFileCompleteCallback mInnerHandler = null;
    public AudioPlayDelegateImpl(QAVPlayFileCompleteCallback handler)
    {
        mHandler = handler;
        mInnerHandler = new QAVPlayFileCompleteCallback(delegate (int code, string filepath)
        {
            innerHandlerImpl(code, filepath);
        });

        ITMGContext.GetInstance().GetPttCtrl().OnPlayFileComplete += mInnerHandler;
    }

    public void start(string filePath)
    {
        ITMGContext.GetInstance().GetPttCtrl().PlayRecordedFile(filePath);
    }

    private void innerHandlerImpl(int code, string filePath)
    {
        if (null != mHandler)
        {
            mHandler(code, filePath);
        }

        ITMGContext.GetInstance().GetPttCtrl().OnPlayFileComplete -= mInnerHandler;
    }
}

class ConvertTextDelegateImpl
{
    QAVSpeechToTextCallback mHandler = null;
    QAVSpeechToTextCallback mInnerHandler = null;
    public ConvertTextDelegateImpl(QAVSpeechToTextCallback handler)
    {
        mHandler = handler;
        mInnerHandler = new QAVSpeechToTextCallback(delegate (int code, string fileid, string result)
        {
            innerHandlerImpl(code, fileid, result);
        });

        ITMGContext.GetInstance().GetPttCtrl().OnSpeechToTextComplete += mInnerHandler;
    }

    public void start(string fileId)
    {
        ITMGContext.GetInstance().GetPttCtrl().SpeechToText(fileId);
    }

    private void innerHandlerImpl(int code, string fileid, string result)
    {
        if (null != mHandler)
        {
            mHandler(code, fileid, result);
        }

        ITMGContext.GetInstance().GetPttCtrl().OnSpeechToTextComplete -= mInnerHandler;
    }
}

public class UserClass
{
    public int AllUser;
    public int AccUser;
    public int ProxyUser;

    public UserClass()
    {
        ProxyUser = 0;
        AccUser = 0;
        AllUser = 0;
    }
}

public class AudioStreamNumber
{
    public int AudioStreams;

    public AudioStreamNumber()
    {
        AudioStreams = 0;
    }
}

#endregion