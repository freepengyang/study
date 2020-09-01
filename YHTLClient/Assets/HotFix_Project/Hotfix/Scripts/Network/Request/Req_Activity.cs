using System;
using Google.Protobuf.Collections;
public partial class Net
{
    public static void ReqSpecialActivityRewardMessage(Int32 activityId, Int32 goalId)
    {
        activity.ReqSpecialActivityReward req = CSProtoManager.Get<activity.ReqSpecialActivityReward>();
        req.activityId = activityId;
        req.goalId = goalId;
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqSpecialActivityRewardMessage, req);
    }
    public static void ReqActivityRewardMessage(Int32 activityId)
    {
        activity.ReqActivityReward req = CSProtoManager.Get<activity.ReqActivityReward>();
        req.activityId = activityId;
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqActivityRewardMessage, req);
    }
    // public static void CSSpecialActivityEquipCollectionMessage(Int32 activityId)
    // {
    //     activity.ReqSpecialActivityEquipCollection req = CSProtoManager.Get<activity.ReqSpecialActivityEquipCollection>();
    //     req.activityId = activityId;
    //     CSHotNetWork.Instance.SendMsg((int)ECM.CSSpecialActivityEquipCollectionMessage, req);
    // }
    public static void CSCollectRewardMessage(Int32 goal, Int32 activityId)
    {
        activity.ReqCollectReward req = CSProtoManager.Get<activity.ReqCollectReward>();
        req.goal = goal;
        req.activityId = activityId;
        CSHotNetWork.Instance.SendMsg((int)ECM.CSCollectRewardMessage, req);
    }
    public static void ReqActiveRewardMessage(Int32 configId)
    {
        activity.ReqActiveReward req = CSProtoManager.Get<activity.ReqActiveReward>();
        req.configId = configId;
        CSHotNetWork.Instance.SendMsg((int)ECM.ReqActiveRewardMessage, req);
    }
    // public static void CSBossFirstKillInfoMessage(Int32 activityId)
    // {
    //     activity.BossFirstKillInfoRequest req = CSProtoManager.Get<activity.BossFirstKillInfoRequest>();
    //     req.activityId = activityId;
    //     CSHotNetWork.Instance.SendMsg((int)ECM.CSBossFirstKillInfoMessage, req);
    // }
    // public static void CSBossFirstKillRewardMessage(Int32 goal, Int32 activityId, Int32 type)
    // {
    //     activity.BossFirstKillRewardRequest req = CSProtoManager.Get<activity.BossFirstKillRewardRequest>();
    //     req.goal = goal;
    //     req.activityId = activityId;
    //     req.type = type;
    //     CSHotNetWork.Instance.SendMsg((int)ECM.CSBossFirstKillRewardMessage, req);
    // }
    public static void CSSpecialActiveDataMessage(Int32 id)
    {
        activity.SpecialActivityDataRequest req = CSProtoManager.Get<activity.SpecialActivityDataRequest>();
        req.id = id;
        //Debug.Log(" ���󿪷������ " + id);
        CSHotNetWork.Instance.SendMsg((int)ECM.CSSpecialActiveDataMessage, req);
    }
	public static void CSBuyYuanbaoGiftMessage(Int32 id)
	{
		activity.SpecialActivityDataRequest req = CSProtoManager.Get<activity.SpecialActivityDataRequest>();
		req.id = id;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSBuyYuanbaoGiftMessage,req);
	}
	
	public static void CSSevenDayDataMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSSevenDayDataMessage,null);
	}
	public static void CSSevenDayRewardMessage(Int32 configId,Int32 scheduleId)
	{
		activity.ReqSevenDayReward req = CSProtoManager.Get<activity.ReqSevenDayReward>();
		req.configId = configId;
		req.scheduleId = scheduleId;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSSevenDayRewardMessage,req);
	}
	public static void CSEquipCompetitionDataMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSEquipCompetitionDataMessage,null);
	}
	public static void CSEquipCompetitionRewardMessage(int _cfgId,int _packId)
	{
		activity.ReqEquipCompetitionReward req = CSProtoManager.Get<activity.ReqEquipCompetitionReward>();
        req.configId = _cfgId;
        req.packId = _packId;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSEquipCompetitionRewardMessage,req);
	}
	// public static void CSEquipCompetitionBoxMessage()
	// {
	// 	CSHotNetWork.Instance.SendMsg((int)ECM.CSEquipCompetitionBoxMessage,null);
	// }
	public static void CSKuangHuanRewardMessage(Int32 activityId, Int32 goalId, int num)
	{
		activity.ReqKuangHuanReward req = CSProtoManager.Get<activity.ReqKuangHuanReward>();
		req.activityId = activityId;
		req.goalId = goalId;
		req.num = num;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSKuangHuanRewardMessage,req);
	}
}
