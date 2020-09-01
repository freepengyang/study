using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using Main_Project.Script.Update;
using FlyBirds.Model;


public class CSHotNetWork
{
    private static CSHotNetWork mInstance;

    public static CSHotNetWork Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new CSHotNetWork();
            }

            return mInstance;
        }
        set { mInstance = value; }
    }

    public void SendMsg(ECM id, IMessage msg)
    {
        CSNetwork.Instance.SendMsg((int) id, msg);
        CSProtoManager.Recycle(msg);
    }

    public void SendMsg(int id, IMessage msg)
    {
        CSNetwork.Instance.SendMsg(id, msg);
        CSProtoManager.Recycle(msg);
    }

    /// <summary>
    /// 请求重新登录
    /// </summary>
    public void ReqReconnect()
    {
        string userName = string.Empty;
        if(CSScene.Sington != null)
        {
            CSScene.Sington.Reconnect();
        }
        if (QuDaoConstant.GetPlatformData() != null && !string.IsNullOrEmpty(CSConstant.loginName))
            userName = CSConstant.loginName;
        Net.ReqReconnectMessage(userName);
        Constant.IsChangeLine = false;
    }

    public void ReqBackGame()
    {
        int port = 0;

        string host = string.Empty;

        ServerListData S_data = HttpRequest.Instance.CurGameService(CSConstant.mSeverId);

        QuDaoConfig mCurPlatform = QuDaoConstant.GetPlatformData();

        if (S_data != null && mCurPlatform != null)
        {
            host = S_data.S_DomainID;
            port = S_data.S_Port + Constant.AddValue;
            Net.NextCanMoveRequestTime = 0;
            CSNetwork.Instance.Close();
            CSNetwork.Instance.ReqConnect(host, port);
        }
    }

    /// <summary>
    /// 返回登录
    /// </summary>
    public void OnReturn()
    {
        //游戏返回选角之后重新登录出现问题，先调用登出  
        VoiceChatManager.Instance.Logout(isLogoutGame: true);
        switch (Platform.mPlatformType)
        {
            case PlatformType.EDITOR:
                LogoutGameAccout();
                break;
            case PlatformType.ANDROID:
                QuDaoConfig mConfig = QuDaoConstant.GetPlatformData();
                if (mConfig != null) //退出账号的时候,上传角色数据给qudao
                {
                    if (mConfig.submitData) SDKUtility.SubmitGameData(5); //退出游戏
                    if (mConfig.exitAccount)
                        QuDaoInterface.Instance.Logout();
                    else //sdk无相关调用
                        LogoutGameAccout(); //根据sdk的需求,调用不同的退出账号接口
                }

                break;
            case PlatformType.IOS:
                SDKUtility.SubmitGameData(5); //注销
                QuDaoInterface.Instance.Logout();
                break;
        }
    }

    public void LogoutGameAccout()
    {
        CSGame.Sington.StopAllCoroutines();
        CSNewFunctionUnlockManager.Instance.ClearCB();
        UIManager.Instance.CloseAllPanel(new List<Type>()
        {
            typeof(UILogin)
        }, true);
        CSConstant.mSeverId = 0;
        CSConstant.mOnlyServerId = 0;
        CSGame.Sington.ChangeStateBackFromGame();
        HotManager.Instance.ChangeStateBackFromGame();
        if (CSNetwork.Instance.Client != null && CSNetwork.Instance.Client.Connected)
        {
            Net.ReqLogoutMessage();
        }

        CSNetwork.Instance.Close();
        DisposeGame();
    }

    public void OnReturnChooseRole()
    {
        CSGame.Sington.StopAllCoroutines();
        CSNewFunctionUnlockManager.Instance.ClearCB();
        UIManager.Instance.CloseAllPanel(null, true);
        CSAudioMgr.Instance.ClearOnReturn();
        CSGame.Sington.ChangeStateBackFromGame(true);
        HotManager.Instance.ChangeStateBackFromGame(true);
        if (CSNetwork.Instance.Client != null && CSNetwork.Instance.Client.Connected)
        {
            Net.ReqBackToChooseRoleMessage();
        }

        DisposeGame();
    }

    private void DisposeGame()
    {
        //游戏返回选角之后语音重新登录出现问题，先调用登出  
        CSTimer.Instance.Dispose();
        VoiceChatManager.Instance.Logout(isLogoutGame: true);
        CSAudioOut.Clear();
        CSAudioMgr.Instance.ClearOnReturn();
        HotManager.Instance.UnRegEventHandle();
        CSScene.ForceDestroy();
        CSMainPlayerInfo.Instance.Release();
        CSNetRepeatedFieldPool.Dispose();
        CSEventObjectManager.Instance.Dispose();
        CSConstant.MainPlayerName = "主角尚未加载";
        CSBaseManager.Instance.Reset();
        CSPoolManager.Dispose();
        CSProtoManager.Dispose();
        CSGameManager.Instance.Clear();
        FNDebug.Log("资源清理成功");
    }

    /// <summary>
    /// 重启游戏 需要销毁部分资源
    /// </summary>
    public void RestartGame()
    {
        if(CSAudioMgr.Instance != null) CSAudioMgr.Instance.ClearOnReturn();
        CoroutineManager.StopAllCoroutine();
        if(CSResUpdateManager.Instance != null) CSResUpdateManager.Instance.OnDispose();
        if(CSCheckResourceManager.Instance != null) CSCheckResourceManager.Instance.OnDispose();
        if(ReporterManager.instance != null) ReporterManager.instance.OnDestroy();
    }

    public void OnReturnToCheckVersion()
    {
        QuDaoInterface.Instance.FinishGame();
    }

    public void RequestServerState()
    {
        Constant.isAccountException = false;
        OnConnectTips();
    }

    private void OnConnectTips()
    {
        if (!Constant.isAccountException && CSGame.Sington.mCurState == GameState.MainScene)
        {
            switch (Constant.mCurServer)
            {
                case ServerType.GameServer:
                    UIManager.Instance.OpenPanel<UIWaiting>().Show(true);
                    break;
            }
        }
    }
}