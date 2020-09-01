using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void GMCommand(string strt)
	{
		user.GMCommand req = CSProtoManager.Get<user.GMCommand>(); //new user.GMCommand();
		req.command = strt;
		UnityEngine.Debug.LogError(strt);
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqGMCommandMessage, req);
		UnityEngine.Debug.LogError(strt);
	}

	/// <summary> �ͻ�������ͨ����Ϣ </summary>
	public static void ReqBackServerMessage(int type, long data64)
	{
		player.CommonInfo req = CSProtoManager.Get<player.CommonInfo>();// new player.CommonInfo();
		req.type = type;
		req.data64 = data64;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqBackServerMessage, req);
	}
	public static void ReqGetRoleExtraValuesMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqGetRoleExtraValuesMessage,null);
	}
	public static void ReqSaveRoleSettingsMessage(RepeatedField<Int64> roleSettings)
	{
		player.SaveRoleSettingsMsg req = CSProtoManager.Get<player.SaveRoleSettingsMsg>();
		req.roleSettings.Clear();
		req.roleSettings.AddRange(roleSettings);
		roleSettings.Clear();
		CSNetRepeatedFieldPool.Put(roleSettings);
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqSaveRoleSettingsMessage,req);
	}
	/// <summary>
	/// 请求其他玩家信息
	/// </summary>
	/// <param name="roleId"></param>
	public static void ReqGetOtherPlayerInfoMessage(long roleId)
	{
		user.RoleIdMsg req = CSProtoManager.Get<user.RoleIdMsg>();
		req.roleId = roleId;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqGetOtherPlayerInfoMessage,req);
	}
	public static void ReqSaveNewbieGuideMessage(Int32 groupId,Int32 step)
	{
		player.SaveNewbieGuideRequest req = CSProtoManager.Get<player.SaveNewbieGuideRequest>();
		req.groupId = groupId;
		req.step = step;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqSaveNewbieGuideMessage,req);
	}
	public static void ReqCommonMessage(Int32 type,Int32 data,String str,Int64 data64)
	{
		player.CommonInfo req = CSProtoManager.Get<player.CommonInfo>();
		req.type = type;
		req.data = data;
		req.str = str;
		req.data64 = data64;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqCommonMessage,req);
	}
	public static void ReqBackServerMessage(Int32 type,Int32 data,String str,Int64 data64)
	{
		player.CommonInfo req = CSProtoManager.Get<player.CommonInfo>();
		req.type = type;
		req.data = data;
		req.str = str;
		req.data64 = data64;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqBackServerMessage,req);
	}
	public static void CSUpdateNameMessage(String newName)
	{
		player.ReqUpdateName req = CSProtoManager.Get<player.ReqUpdateName>();
		req.newName = newName;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSUpdateNameMessage,req);
	}
	public static void ReqGetDownloadRewardMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqGetDownloadRewardMessage,null);
	}
}
