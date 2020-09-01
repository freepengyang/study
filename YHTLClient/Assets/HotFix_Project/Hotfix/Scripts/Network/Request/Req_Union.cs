using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSGetUnionInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSGetUnionInfoMessage,null);
	}
	public static void CSCreateUnionMessage(String name)
	{
		union.CreateUnionRequest req = CSProtoManager.Get<union.CreateUnionRequest>();
		req.name = name;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSCreateUnionMessage,req);
	}
	public static void CSApplyUnionMessage(Int64 unionId)
	{
		union.ApplyUnionRequest req = CSProtoManager.Get<union.ApplyUnionRequest>();
		req.unionId = unionId;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSApplyUnionMessage,req);
	}
	public static void CSInviteUnionMessage(Int64 roleId)
	{
		union.InviteUnionRequest req = CSProtoManager.Get<union.InviteUnionRequest>();
		req.roleId = roleId;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSInviteUnionMessage,req);
	}
	public static void CSChangeUnionPositionMessage(Int64 roleId,Int32 position)
	{
		union.ChangePositionMsg req = CSProtoManager.Get<union.ChangePositionMsg>();
		req.roleId = roleId;
		req.position = position;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSChangeUnionPositionMessage,req);
	}
	public static void CSLeaveUnionMessage(Int64 roleId)
	{
		union.LeaveUnionRequest req = CSProtoManager.Get<union.LeaveUnionRequest>();
		req.roleId = roleId;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSLeaveUnionMessage,req);
	}
	public static void CSUnionDonateGoldMessage(Int32 count)
	{
		union.UnionDonateGoldRequest req = CSProtoManager.Get<union.UnionDonateGoldRequest>();
		req.count = count;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSUnionDonateGoldMessage,req);
	}
	public static void CSUnionDonateEquipMessage(Int32 bagIndex,Int32 count)
	{
		union.UnionDonateEquipRequest req = CSProtoManager.Get<union.UnionDonateEquipRequest>();
		req.bagIndex = bagIndex;
		req.count = count;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSUnionDonateEquipMessage,req);
	}
	public static void CSUnionExchangeEquipMessage(Int64 itemId,Int32 count)
	{
		union.UnionExchangeEquipRequest req = CSProtoManager.Get<union.UnionExchangeEquipRequest>();
		req.itemId = itemId;
		req.count = count;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSUnionExchangeEquipMessage,req);
	}
	public static void CSUnionChangePresidentMessage(Int64 roleId)
	{
		union.UnionChangePresidentRequest req = CSProtoManager.Get<union.UnionChangePresidentRequest>();
		req.roleId = roleId;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSUnionChangePresidentMessage,req);
	}
	public static void CSDisposeUnionMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSDisposeUnionMessage,null);
	}
	public static void CSConfirmUnionApplyMessage(RepeatedField<Int64> roleIds,Boolean confirm)
	{
		union.ConfirmAppliesRequest req = CSProtoManager.Get<union.ConfirmAppliesRequest>();
		req.roleIds.Clear();
		req.roleIds.AddRange(roleIds);
		roleIds.Clear();
		CSNetRepeatedFieldPool.Put(roleIds);
		req.confirm = confirm;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSConfirmUnionApplyMessage,req);
	}
	public static void CSUnionDestroyItemMessage(RepeatedField<Int64> itemId)
	{
		union.UnionDestroyItemRequest req = CSProtoManager.Get<union.UnionDestroyItemRequest>();
		req.itemId.Clear();
		req.itemId.AddRange(itemId);
		itemId.Clear();
		CSNetRepeatedFieldPool.Put(itemId);
		CSHotNetWork.Instance.SendMsg((int)ECM.CSUnionDestroyItemMessage,req);
	}
	public static void CSUnionDeclareWarMessage(Int64 unionId)
	{
		union.DeclareWarRequest req = CSProtoManager.Get<union.DeclareWarRequest>();
		req.unionId = unionId;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSUnionDeclareWarMessage,req);
	}
	public static void CSGetUnionTabMessage(Int32 tab)
	{
		union.GetUnionTabRequest req = CSProtoManager.Get<union.GetUnionTabRequest>();
		req.tab = tab;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSGetUnionTabMessage,req);
	}
	public static void CSSendSouvenirWealthMessage(Int32 totalWealth,Int32 totalNumber)
	{
		union.SendSouvenirWealthRequest req = CSProtoManager.Get<union.SendSouvenirWealthRequest>();
		req.totalWealth = totalWealth;
		req.totalNumber = totalNumber;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSSendSouvenirWealthMessage,req);
	}
	public static void CSGetSouvenirWealthMessage(Int64 id)
	{
		union.GetSouvenirWealthRequest req = CSProtoManager.Get<union.GetSouvenirWealthRequest>();
		req.id = id;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSGetSouvenirWealthMessage,req);
	}
	public static void CSUnionBulletinMessage(String bulletin)
	{
		union.BulletinRequest req = CSProtoManager.Get<union.BulletinRequest>();
		req.bulletin = bulletin;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSUnionBulletinMessage,req);
	}
	public static void CSUnionJoinInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSUnionJoinInfoMessage,null);
	}
	public static void CSImpeachmentMessage(Int32 type,Int32 data)
	{
		union.ImpeachmentRequest req = CSProtoManager.Get<union.ImpeachmentRequest>();
		req.type = type;
		req.data = data;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSImpeachmentMessage,req);
	}
	public static void CSSetAutoJoinLevelMessage(Int32 level)
	{
		union.SetAutoJionLevelRequest req = CSProtoManager.Get<union.SetAutoJionLevelRequest>();
		req.level = level;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSSetAutoJoinLevelMessage,req);
	}
	public static void CSSetApplyLeaderMessage(Boolean allow)
	{
		union.SetApplyLeader req = CSProtoManager.Get<union.SetApplyLeader>();
		req.allow = allow;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSSetApplyLeaderMessage,req);
	}
	public static void CSCombineUnionMessage(Int32 operationType,Int32 combineType,Int64 unionId,String unionName,Int64 unionId1,String unionName1,Boolean agree)
	{
		union.combineUnion req = CSProtoManager.Get<union.combineUnion>();
		req.operationType = operationType;
		req.combineType = combineType;
		req.unionId = unionId;
		req.unionName = unionName;
		req.unionId1 = unionId1;
		req.unionName1 = unionName1;
		req.agree = agree;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSCombineUnionMessage,req);
	}
	public static void CSUnionExchangeEquipLimitMessage(Int32 reincarnation)
	{
		union.ExchangeEquipLimit req = CSProtoManager.Get<union.ExchangeEquipLimit>();
		req.reincarnation = reincarnation;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSUnionExchangeEquipLimitMessage,req);
	}
	public static void CSUnionRenameMessage(String name)
	{
		union.RenameRequest req = CSProtoManager.Get<union.RenameRequest>();
		req.name = name;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSUnionRenameMessage,req);
	}
	public static void CSPushUnionMessageMessage(String message)
	{
		union.pushMessage req = CSProtoManager.Get<union.pushMessage>();
		req.message = message;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSPushUnionMessageMessage,req);
	}
	public static void CSImproveInfosMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSImproveInfosMessage,null);
	}
	public static void CSImproveMessage(Int32 position)
	{
		union.ImproveRequest req = CSProtoManager.Get<union.ImproveRequest>();
		req.position = position;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSImproveMessage,req);
	}
	public static void CSRemoveApplyUnionMessage(Int64 unionId)
	{
		union.RemoveApplyUnion req = CSProtoManager.Get<union.RemoveApplyUnion>();
		req.unionId = unionId;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSRemoveApplyUnionMessage,req);
	}
	public static void CSSplitYuanbaoMessage(RepeatedField<union.YuanbaoItem> items)
	{
		union.SplitYuanbaoRequest req = CSProtoManager.Get<union.SplitYuanbaoRequest>();
		req.items.Clear();
		req.items.AddRange(items);
		items.Clear();
		CSNetRepeatedFieldPool.Put(items);
		CSHotNetWork.Instance.SendMsg((int)ECM.CSSplitYuanbaoMessage,req);
	}
	public static void ReqUpdateSpeakLimitsMessage(RepeatedField<Int64> roleIds,Boolean canSpeak)
	{
		union.UpdateCanSpeakMsg req = CSProtoManager.Get<union.UpdateCanSpeakMsg>();
		req.roleIds.Clear();
		req.roleIds.AddRange(roleIds);
		roleIds.Clear();
		CSNetRepeatedFieldPool.Put(roleIds);
		req.canSpeak = canSpeak;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqUpdateSpeakLimitsMessage,req);
	}
	public static void ReqUnionCallInfoMessage(String playerName,Int32 mapId,Int32 posx,Int32 posy)
	{
		union.UnionCallInfo req = CSProtoManager.Get<union.UnionCallInfo>();
		req.playerName = playerName;
		req.mapId = mapId;
		req.posx = posx;
		req.posy = posy;
		CSHotNetWork.Instance.SendMsg((int)ECM.ReqUnionCallInfoMessage,req);
	}
}
