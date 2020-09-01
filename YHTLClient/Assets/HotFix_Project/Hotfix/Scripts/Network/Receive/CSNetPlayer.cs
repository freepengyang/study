using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class CSNetPlayer : CSNetBase
{
    void ECM_ResRoleExpUpdatedMessage(NetInfo info)
    {
        player.RoleExpUpdated msg = Network.Deserialize<player.RoleExpUpdated>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forplayer.RoleExpUpdated");
            return;
        }
        CSMainPlayerInfo.Instance.Exp = msg.exp;
        //Debug.Log("获得经验   " + msg.exp);
    }
    void ECM_ResRoleUpgradeMessage(NetInfo info)
    {
        player.RoleUpgrade msg = Network.Deserialize<player.RoleUpgrade>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forplayer.RoleUpgrade");
            return;
        }
        CSMainPlayerInfo.Instance.GetLevelChangedMes(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.GetUpgrade, msg);
#if UNITY_ANDROID
        if (QuDaoConstant.GetPlatformData() != null && QuDaoConstant.GetPlatformData().submitData)
        {
            SDKUtility.SubmitGameData(4);//升级
        }
#else
        SDKUtility.SubmitGameData(4);
#endif
    }
    void ECM_ResRoleExtraValuesMessage(NetInfo info)
    {
        player.RoleExtraValues msg = Network.Deserialize<player.RoleExtraValues>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forplayer.RoleExtraValues");
            return;
        }
        CSMainPlayerInfo.Instance.PlayerRoleExtraValues(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.PlayerRoleExtraValues);
    }
    void ECM_ResDayPassedMessage(NetInfo info)
    {
        player.DayPassed msg = Network.Deserialize<player.DayPassed>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forplayer.DayPassed");
            return;
        }
        CSMainPlayerInfo.Instance.DayPassedRoleExtraValues(msg);
        CSMainPlayerInfo.Instance.ServerOpenDay = msg.roleExtraValues.openServerDays;
        HotManager.Instance.EventHandler.SendEvent(CEvent.ResDayPassedMessage);
    }
    /// <summary>
    /// 查看其他玩家信息响应
    /// </summary>
    /// <param name="info"></param>
    void ECM_ResOtherPlayerInfoMessage(NetInfo info)
    {
        user.OtherPlayerInfo msg = Network.Deserialize<user.OtherPlayerInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forplayer.OtherPlayerInfo");
            return;
        }
        CSOtherPlayerInfo.Instance.HandleSetOtherPlayerInfo(msg);
        // HotManager.Instance.EventHandler.SendEvent(CEvent.CheckPlayerInfo, msg);
        UIManager.Instance.CreatePanel<UICheckInfoCombinePanel>(p =>
        {
            (p as UICheckInfoCombinePanel).OpenChildPanel((int)UICheckInfoCombinePanel.ChildPanelType.CPT_Role);
        });
    }
    void ECM_ResCommonMessage(NetInfo info)
    {
        player.CommonInfo msg = Network.Deserialize<player.CommonInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forplayer.CommonInfo");
            return;
        }
    }
    void ECM_ResOtherPlayerCommonInfoMessage(NetInfo info)
    {
        user.OtherPlayerCommonInfo msg = Network.Deserialize<user.OtherPlayerCommonInfo>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forplayer.OtherPlayerCommonInfo");
            return;
        }
    }
    void ECM_RoleAttrNtfMessage(NetInfo info)
    {
        player.RoleAttrNtf msg = Network.Deserialize<player.RoleAttrNtf>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forplayer.RoleAttrNtf");
            return;
        }
    }
    void ECM_ResPlayerDieMessage(NetInfo info)
    {
        player.PlayerDie msg = Network.Deserialize<player.PlayerDie>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forplayer.PlayerDie");
            return;
        }
        CSReliveInfo.Instance.GetPlayerDieMessage(msg);
        CSMainPlayerInfo.Instance.HP = 0;
        UIManager.Instance.CreatePanel<UIDeadGrayPanel>();
        HotManager.Instance.EventHandler.SendEvent(CEvent.Death, msg);
        HotManager.Instance.MainEventHandler.SendEvent(MainEvent.CloseSelectionPanel, msg);
    }
    void ECM_ResPkValueUpdateMessage(NetInfo info)
    {
        player.PkValueUpdate msg = Network.Deserialize<player.PkValueUpdate>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forplayer.PkValueUpdate");
            return;
        }
        CSMainPlayerInfo.Instance.PkValue = msg.pkValue;
    }
    void ECM_SCRoleBriefMessage(NetInfo info)
    {
        player.ResRoleBrief msg = Network.Deserialize<player.ResRoleBrief>(info);
        if (null == msg)
        {
            UnityEngine.Debug.LogError("Deserialize Msg Failed Forplayer.ResRoleBrief");
            return;
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.Rename, msg.updateFlag);
    }
	void ECM_SCPkValueChangeMessage(NetInfo info)
	{
		player.PkValueChange msg = Network.Deserialize<player.PkValueChange>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forplayer.PkValueChange");
			return;
		}

        if (CSAvatarManager.Instance.GetAvatarInfo(msg.roleId) is CSPlayerInfo playerInfo)
        {
            FNDebug.LogFormat("<color=#53ddef>[人物灰名]:[roleId:{0}][pkValue:{1}]</color>", msg.roleId,msg.pkValue);
            playerInfo.PkValue = msg.pkValue;
        }
	}
	void ECM_SCPkGreyNameStateMessage(NetInfo info)
	{
		player.PkGreyNameState msg = Network.Deserialize<player.PkGreyNameState>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forplayer.PkGreyNameState");
			return;
		}

        if (CSAvatarManager.Instance.GetAvatarInfo(msg.roleId) is CSPlayerInfo playerInfo)
        {
            FNDebug.LogFormat("<color=#53ddef>[人物灰名]:[roleId:{0}][greyName:{1}]</color>", msg.roleId, msg.state);
            playerInfo.GreyName = msg.state == 1;
        }
    }
}