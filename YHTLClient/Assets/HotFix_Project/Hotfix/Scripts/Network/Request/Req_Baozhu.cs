using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSLevelUpBaoZhuMessage(Int32 bagIndex,RepeatedField<Int32> eatIndex)
	{
		baozhu.LevelUpBaoZhu req = CSProtoManager.Get<baozhu.LevelUpBaoZhu>();
		req.bagIndex = bagIndex;
		req.eatIndex.Clear();
		req.eatIndex.AddRange(eatIndex);
		eatIndex.Clear();
		CSNetRepeatedFieldPool.Put(eatIndex);
		CSHotNetWork.Instance.SendMsg((int)ECM.CSLevelUpBaoZhuMessage,req);
	}
	public static void CSGradeUpBaoZhuMessage(Int32 bagIndex)
	{
		baozhu.GradeUpBaoZhu req = CSProtoManager.Get<baozhu.GradeUpBaoZhu>();
		req.bagIndex = bagIndex;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSGradeUpBaoZhuMessage,req);
	}
	public static void CSRandBaoZhuSkillMessage(Int32 slot)
	{
		baozhu.RandBaoZhuSkill req = CSProtoManager.Get<baozhu.RandBaoZhuSkill>();
		req.slot = slot;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSRandBaoZhuSkillMessage,req);
	}
	public static void CSChoseBaoZhuSkillMessage(Int32 slot)
	{
		baozhu.RandBaoZhuSkill req = CSProtoManager.Get<baozhu.RandBaoZhuSkill>();
		req.slot = slot;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSChoseBaoZhuSkillMessage,req);
	}
}
