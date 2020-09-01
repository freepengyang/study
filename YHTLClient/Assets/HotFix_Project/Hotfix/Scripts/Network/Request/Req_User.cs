using Google.Protobuf.Collections;
// 包结构集合点
// author: jiabao
// time：  2016.2.17
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Main_Project.Script.Update;
using UnityEngine;

/// <summary>
/// 所有请求服务器方法
/// </summary>
public partial class Net
{
    //请求登录
    public static void ReqLoginMessage(string name, int platformid, int serverid,string sign=null,long time=0, string phoneModel = "")
    {
        user.LoginRequest data = CSProtoManager.Get<user.LoginRequest>();
        data.loginName = name;
        data.forumId = platformid;
        data.serverId = serverid;

        if (!string.IsNullOrEmpty(sign))
            data.sign = sign;
        if (time != 0)
            data.time = time;
#if UNITY_ANDROID
        if (CSVersionManager.Instance != null)
            data.client = CSVersionManager.Instance.Version;
        else
            data.client = string.Empty;
#else
        data.client = string.Empty;
#endif
        data.phoneModel = phoneModel.ToLower();
        CSHotNetWork.Instance.SendMsg(ECM.ReqLoginMessage, data);
    }

	//重新请求登录
	public static void ReqLoginMessage()
	{
        user.LoginRequest data = CSProtoManager.Get<user.LoginRequest>();

        //user.LoginRequest data = new user.LoginRequest();
        data.loginName = CSConstant.loginName;
		data.forumId = Constant.platformid;
      
        data.serverId = CSConstant.mOnlyServerId;
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqLoginMessage, data);
    }

    //请求创建角色
    public static void ReqCreateRoleMessage(string name, int sex, int career,string phoneModel)
    {
        user.CreateRoleRequest data = CSProtoManager.Get<user.CreateRoleRequest>();// new user.CreateRoleRequest();
        data.name = name;
        data.sex = sex;
        data.career = career;
        data.phoneModel = phoneModel.ToLower();
        FNDebug.Log("请求创建角色 --> "+ name  + " 性别 "+sex + " 职业 "+ career+ " 机型 "+ phoneModel.ToLower());
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqCreateRoleMessage, data);
    }

    //请求随机名字   -- 0 女 1男
    public static void ReqRandomRoleName(int sex)
    {
        user.RandomRoleNameRequest data = CSProtoManager.Get<user.RandomRoleNameRequest>(); //new user.RandomRoleNameRequest();
        data.sex = sex;
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqRandomRoleNameMessage, data);
    }

    //请求选择角色
    public static void ReqChooseRole(long roleId)
    {
        user.RoleIdMsg data = CSProtoManager.Get<user.RoleIdMsg>(); //new user.RoleIdMsg();
        data.roleId = roleId;
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqChooseRoleMessage, data);
    }

    //请求删除角色
    public static void ReqRemoveRole(long roleId)
    {
        user.RoleIdMsg data = CSProtoManager.Get<user.RoleIdMsg>();// new user.RoleIdMsg();
        data.roleId = roleId;
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqDeleteRoleMessage, data);
    }

    public static void ReqLogoutMessage()
    {
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqLogoutMessage, null);
    }

    public static void ReqRechargeMessage(string msg)
    {
        user.RechargeingRequest data = CSProtoManager.Get<user.RechargeingRequest>();
        data.rechargeId = msg;
        CSHotNetWork.Instance.SendMsg((int) ECM.ReqRechargeMessage, data);
    }

    public static int NextCanMoveRequestTime;
    
    /*短线重连*/
    public static void ReqReconnectMessage(string strName)
    {
        user.ReconnectRequest req = CSProtoManager.Get<user.ReconnectRequest>(); //new user.ReconnectRequest();
        req.loginName = strName;
        req.forumId = Constant.platformid;
        req.roleId = Constant.mRoleId;
        req.serverId = CSConstant.mOnlyServerId;
        req.time = Constant.time;
        req.sign = Constant.sign;
        req.changeLine = Constant.IsChangeLine;
        CSHotNetWork.Instance.SendMsg(ECM.ReqReconnectMessage, req);
    }

    public static void ReqUserFeedbackMessage(string contect, string phoneType, string qq)
    {
        user.UserFeedbackRequest data = CSProtoManager.Get<user.UserFeedbackRequest>();
        data.content = contect;
        data.phoneType = phoneType;
        data.qq = qq;
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqUserFeedbackMessage, data);
    }
	public static void ReqBackToChooseRoleMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqBackToChooseRoleMessage,null);
	}
	public static void ReqBindPhoneNumberMessage(String phoneNumber)
	{
		user.BindPhoneNumberRequest req = CSProtoManager.Get<user.BindPhoneNumberRequest>();
		req.phoneNumber = phoneNumber;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqBindPhoneNumberMessage,req);
	}
	public static void ReqUserCheatSpeedMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqUserCheatSpeedMessage,null);
	}
	public static void ReqPushMessageMessage(Int32 type,RepeatedField<String> activityOpen,String cid,String packegeName)
	{
		user.PushMessageRequest req = CSProtoManager.Get<user.PushMessageRequest>();
		req.type = type;
		req.activityOpen.Clear();
		req.activityOpen.AddRange(activityOpen);
		activityOpen.Clear();
		CSNetRepeatedFieldPool.Put(activityOpen);
		req.cid = cid;
		req.packegeName = packegeName;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqPushMessageMessage,req);
	}
	public static void ReqUseInvitationCodeMessage(String invitationCode,String name,Int32 sex,Int32 career,Int32 photo)
	{
		user.UseInvitationCode req = CSProtoManager.Get<user.UseInvitationCode>();
		req.invitationCode = invitationCode;
		req.name = name;
		req.sex = sex;
		req.career = career;
		req.photo = photo;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqUseInvitationCodeMessage,req);
	}
	public static void CheckCreateRoleArgsVaildReqMessage(String name)
	{
		user.CheckCreateRoleArgsVaildReq req = CSProtoManager.Get<user.CheckCreateRoleArgsVaildReq>();
		req.name = name;
		CSHotNetWork.Instance.SendMsg((int)ECM.CheckCreateRoleArgsVaildReqMessage,req);
	}
}
