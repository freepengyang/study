using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using user;

public partial class CSNetUser : CSNetBase
{
    public override void HandByNetCallback(ECM _type, NetInfo obj)
    {
        switch (_type)
        {
            case ECM.Connect:
                HotManager.Instance.EventHandler.SendEvent(CEvent.Connect, obj);
                OnConnectSucceed(0, null);
                break;
            case ECM.ConnectFail:
                HotManager.Instance.EventHandler.SendEvent(CEvent.ConnectFail, obj);
                break;
            case ECM.Disconnect:
                HotManager.Instance.EventHandler.SendEvent(CEvent.Disconnect, obj);
                OnConnectFailure(0, null);
                break;
        }
    }

    private void OnConnectSucceed(uint uiEvtID, params object[] data)
    {
        if (!Constant.isAccountException && CSGame.Sington.mCurState == GameState.MainScene)
        {
            switch (Constant.mCurServer)
            {
                case ServerType.GameServer:
                    CSHotNetWork.Instance.ReqReconnect();
                    break;
            }

            UIManager.Instance.ClosePanel<UIWaiting>();
            Constant.IsChangeLine = false;
        }
    }

    private void OnConnectFailure(uint uiEvtID, params object[] data)
    {
        if (!Constant.isAccountException && CSGame.Sington.mCurState == GameState.MainScene)
        {
            CSHotNetWork.Instance.RequestServerState();
        }
    }

    /// <summary>
    /// 断线响应
    /// </summary>
    /// <param name="obj"></param>
    void ECM_ResDisconnectMessage(NetInfo obj)
    {
        user.DisconnectResponse rsp = Network.Deserialize<user.DisconnectResponse>(obj);
        Constant.isAccountException = true;
        switch ((GoingDownReason)rsp.reason)
        {
            case GoingDownReason.Maintain:
                if (Platform.mPlatformType == PlatformType.EDITOR)
                {
                    UtilityTips.ShowPromptForceWordTip(42, CSHotNetWork.Instance.OnReturnToCheckVersion);
                }
                else
                {
                    CSHotNetWork.Instance.RequestServerState();
                }

                break;
            case GoingDownReason.Block:
                UtilityTips.ShowPromptForceWordTip(43, CSHotNetWork.Instance.OnReturn);
                break;
            case GoingDownReason.AnotherSession:
                UtilityTips.ShowPromptForceWordTip(44, CSHotNetWork.Instance.OnReturn);
                break;
        }
    }

    void ECM_ResPlayerInfoMessage(NetInfo obj)
    {
        try
        {
            user.PlayerInfo roleInfo = Network.Deserialize<user.PlayerInfo>(obj);
            CSMainPlayerInfo.CreateInstance(roleInfo);
        }
        catch (System.Exception ex)
        {
            if (UIManager.Instance != null)
            {
                UILoginRolePanel panel = UIManager.Instance.GetPanel<UILoginRolePanel>();
                if (panel != null)
                {
                    if (panel.CurRoleinfo != null)
                    {
                        UnityEngine.Debug.LogError(panel.CurRoleinfo.roleName + " 获得数据为 ECM_ResPlayerInfoMessage = " +
                                                   ex.ToString());
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("CurRoleInfo == null_0 ECM_ResPlayerInfoMessage");
                    }
                }
                else
                {
                    UnityEngine.Debug.LogError("登入信息异常1 = " + ex.ToString());
                }
            }
            else
            {
                UnityEngine.Debug.LogError("登入信息异常2 = " + ex.ToString());
            }
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.ResPlayerInfoMessage, obj);
    }

    int[] powers = new int[2];
    void ECM_ResPlayAttributeChangedMessage(NetInfo obj)
    {
        if(!CSScene.IsLanuchMainPlayer) return;
        user.PlayerAttribute rsp = Network.Deserialize<user.PlayerAttribute>(obj);
        //Dictionary<int, TupleProperty> oldData =
        //    new Dictionary<int, TupleProperty>(CSMainPlayerInfo.Instance.GetMyAttr());
        CSMainPlayerInfo.Instance.ResPlayAttributeChangedMessage(rsp);

        if (rsp.nbValue > CSMainPlayerInfo.Instance.OldFightPower)
            FNDebug.Log($"<color=#00ff00>[战斗力:{rsp.nbValue} => {CSMainPlayerInfo.Instance.OldFightPower}]</color>");
        else
            FNDebug.Log($"<color=#ff0000>[战斗力:{rsp.nbValue} => {CSMainPlayerInfo.Instance.OldFightPower}]</color>");

        if (rsp.nbValue > CSMainPlayerInfo.Instance.OldFightPower)
        {
            int from = CSMainPlayerInfo.Instance.OldFightPower;
            int to = rsp.nbValue;
            powers[0] = from;
            powers[1] = to;
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnPlayFightPowerChanged,powers);
            UIManager.Instance.CreatePanel<UIAttributeChangePanel>(f =>
            {
                (f as UIAttributeChangePanel).Show(from, to);
            });
        }
    }
    void ECM_ResLoginMessage(NetInfo info)
    {
        user.LoginResponse msg = Network.Deserialize<user.LoginResponse>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Foruser.LoginResponse");
            return;
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.ResLoginMessage, info);
    }
    void ECM_ResRandomRoleNameMessage(NetInfo info)
    {
        user.RandomRoleNameResponse msg = Network.Deserialize<user.RandomRoleNameResponse>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Foruser.RandomRoleNameResponse");
            return;
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.ResRandomRoleNameMessage, msg);
    }
    void ECM_ResDeleteRoleMessage(NetInfo info)
    {
        user.RoleIdMsg msg = Network.Deserialize<user.RoleIdMsg>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Foruser.RoleIdMsg");
            return;
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.ResDeleteRoleMessage, info);
    }
    void ECM_ResPlayerEquipChangedMessage(NetInfo info)
    {
        user.RoleBrief msg = Network.Deserialize<user.RoleBrief>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Foruser.RoleBrief");
            return;
        }
        if(CSAvatarManager.Instance == null)
        {
            FNDebug.LogError("CSAvatarManager Instance is null");
            return;
        }
        CSAvatar avatar = CSAvatarManager.Instance.GetAvatar(msg.roleId);
        if (avatar != null && avatar.BaseInfo != null)
        {
            CSPlayerInfo playerInfo = avatar.BaseInfo as CSPlayerInfo;
            if (playerInfo != null)
            {
                playerInfo.UpdateRoleBrief(msg);
            }
        }
    }
    void ECM_ResPushMessageMessage(NetInfo info)
    {
        user.PushMessageResponse msg = Network.Deserialize<user.PushMessageResponse>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Foruser.PushMessageResponse");
            return;
        }
    }
    void ECM_ResExtractInvitationCodeMessage(NetInfo info)
    {
        user.ExtractInvitationCodeResponse msg = Network.Deserialize<user.ExtractInvitationCodeResponse>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Foruser.ExtractInvitationCodeResponse");
            return;
        }
    }
    void ECM_CheckCreateRoleArgsVaildAckMessage(NetInfo info)
    {
        user.CheckCreateRoleArgsVaildAck msg = Network.Deserialize<user.CheckCreateRoleArgsVaildAck>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Foruser.CheckCreateRoleArgsVaildAck");
            return;
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.CheckCreateRoleArgsVaildAckMessage, info);
    }
    void ECM_ServerTimeNtfMessage(NetInfo info)
    {
        user.ServerTimeNtf msg = Network.Deserialize<user.ServerTimeNtf>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Foruser.ServerTimeNtf");
            return;
        }
        CSServerTime.Instance.refreshTime(msg.serverTime);
        CSServerTime.Instance.ServerMergeCount = msg.mergeCount;
    }
    
    void ECM_ServerLoadNtfMessage(NetInfo info)
    {
        user.ServerLoadNtf msg = Network.Deserialize<user.ServerLoadNtf>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Foruser.ServerLoadNtf");
            return;
        }
        FNDebug.Log("系统负载通知: count : " + msg.count);
    }
    void ECM_ServerBusyNtfMessage(NetInfo info)
    {
        user.ServerBusyNtf msg = Network.Deserialize<user.ServerBusyNtf>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Foruser.ServerBusyNtf");
            return;
        }
        FNDebug.Log("系统繁忙通知: count : " + msg.count);
    }
    void ECM_CreateRoleNtfMessage(NetInfo info)
    {
        user.CreateRoleNtf msg = Network.Deserialize<user.CreateRoleNtf>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Foruser.CreateRoleNtf");
            return;
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.CreateRoleNtfMessage, info);
    }
    void ECM_ResLoginAnotherSessionMessage(NetInfo info)
    {

    }
    void ECM_ResLoginSignTimeoutMessage(NetInfo info)
    {

    }
    void ECM_SCPlayerMoveSpeedMessage(NetInfo info)
    {
        user.PlayerMoveSpeed msg = Network.Deserialize<user.PlayerMoveSpeed>(info);
        if (null == msg)
        {
            FNDebug.LogError("Deserialize Msg Failed Foruser.PlayerMoveSpeed");
            return;
        }
        CSAvatar avatar = CSAvatarManager.Instance.GetAvatar(msg.roleId);

        avatar?.SetStepTime(msg.speed);
    }
	void ECM_SCRoleListMessage(NetInfo info)
	{
		user.LoginResponse msg = Network.Deserialize<user.LoginResponse>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Foruser.LoginResponse");
			return;
		}
        UserManager.Instance.UpdateRoleList(msg);
    }
}