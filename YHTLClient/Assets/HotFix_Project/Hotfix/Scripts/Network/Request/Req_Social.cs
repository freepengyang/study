using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void ReqGetSocialInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqGetSocialInfoMessage,null);
	}
	public static void ReqAddRelationMessage(RepeatedField<Int64> roleId,Int32 relation)
	{
		social.AddRelationRequest req = CSProtoManager.Get<social.AddRelationRequest>();
		req.roleId.Clear();
		req.roleId.AddRange(roleId);
		roleId.Clear();
		CSNetRepeatedFieldPool.Put(roleId);
		req.relation = relation;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqAddRelationMessage,req);
	}
	public static void ReqDeleteRelationMessage(RepeatedField<Int64> roleId)
	{
		social.DeleteRelationRequest req = CSProtoManager.Get<social.DeleteRelationRequest>();
		req.roleId.Clear();
		req.roleId.AddRange(roleId);
		roleId.Clear();
		CSNetRepeatedFieldPool.Put(roleId);
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqDeleteRelationMessage,req);
	}
	public static void ReqFindPlayerByNameMessage(String name,Int32 type)
	{
		social.FindPlayerByNameRequest req = CSProtoManager.Get<social.FindPlayerByNameRequest>();
		req.name = name;
		//req.type = type;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqFindPlayerByNameMessage,req);
	}
	public static void ReqAddFriendByNameMessage(String name)
	{
		social.AddFriendByNameRequest req = CSProtoManager.Get<social.AddFriendByNameRequest>();
		req.name = name;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqAddFriendByNameMessage,req);
	}
	public static void ReqRejectRelationsMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqRejectRelationsMessage,null);
	}
	public static void ReqGetAllSocialInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqGetAllSocialInfoMessage,null);
	}
	public static void RejectSingleReqMessage(Int64 roleId)
	{
		social.RejectSingleReq req = CSProtoManager.Get<social.RejectSingleReq>();
		req.roleId = roleId;
		CSHotNetWork.Instance.SendMsg((int)ECM.RejectSingleReqMessage,req);
	}
	public static void QueryLatelyTouchReqMessage(RepeatedField<Int64> roleId)
	{
		social.QueryLatelyTouchReq req = CSProtoManager.Get<social.QueryLatelyTouchReq>();
		req.roleId.Clear();
		req.roleId.AddRange(roleId);
		roleId.Clear();
		CSNetRepeatedFieldPool.Put(roleId);
		CSHotNetWork.Instance.SendMsg((int)ECM.QueryLatelyTouchReqMessage,req);
	}
    public static void CSSettingSocialMessage(int value)
    {
        social.ReqSettingSocial req = CSProtoManager.Get<social.ReqSettingSocial>();
        req.socialFlag = value;
        CSHotNetWork.Instance.SendMsg((int)ECM.CSSettingSocialMessage, req);
    }
    public static void CSSettingStrangerInfoMessage(int value)
    {
        social.ReqSettingStrangerInfo req = CSProtoManager.Get<social.ReqSettingStrangerInfo>();
        req.strangerFlag = value;
        //Debug.LogError("@@@@@@@req.strangerFlag:" + req.strangerFlag);
        CSHotNetWork.Instance.SendMsg((int)ECM.CSSettingStrangerInfoMessage, req);
    }
    public static void CSSettingMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSSettingMessage,null);
	}
	public static void CSSettingGuildMessage(int value)
	{
		social.ReqSettingGuild req = CSProtoManager.Get<social.ReqSettingGuild>();
        req.guildFlag = value;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSSettingGuildMessage,req);
	}
}
