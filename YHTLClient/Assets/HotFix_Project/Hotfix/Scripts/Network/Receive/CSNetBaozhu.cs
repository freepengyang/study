public partial class CSNetBaozhu : CSNetBase
{
	void ECM_SCLevelUpBaoZhuMessage(NetInfo info)
	{
		baozhu.ResLevelUpBaoZhu msg = Network.Deserialize<baozhu.ResLevelUpBaoZhu>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forbaozhu.ResLevelUpBaoZhu");
			return;
		}
		//CSPearlInfo.Instance.HandleLevelUpBaoZhu(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.LevelUpBaoZhu, msg);
	}
	void ECM_SCGradeUpBaoZhuMessage(NetInfo info)
	{
		baozhu.ResGradeUpBaoZhu msg = Network.Deserialize<baozhu.ResGradeUpBaoZhu>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forbaozhu.ResGradeUpBaoZhu");
			return;
		}
		//CSPearlInfo.Instance.HandleGradeUpBaoZhu(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.GradeUpBaoZhu, msg);
	}
	void ECM_SCBaoZhuSkillsMessage(NetInfo info)
	{
		baozhu.BaoZhuSkills msg = Network.Deserialize<baozhu.BaoZhuSkills>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forbaozhu.BaoZhuSkills");
			return;
		}
		CSPearlInfo.Instance.HandleRefreshSkill(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.RefreshBaoZhuSkills, msg);
	}
	void ECM_SCChoseBaoZhuSkillMessage(NetInfo info)
	{
		baozhu.BaoZhuSkills msg = Network.Deserialize<baozhu.BaoZhuSkills>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forbaozhu.BaoZhuSkills");
			return;
		}
		CSPearlInfo.Instance.HandBaoZhuSkills(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.ReplaceBaoZhuSkills, msg);
	}
	void ECM_SCBaoZhuBossCountChangeMessage(NetInfo info)
	{
		bag.EquipInfo msg = Network.Deserialize<bag.EquipInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forbaozhu.EquipInfo");
			return;
		}
		//CSPearlInfo.Instance.HandleBaoZhuBossCountChange(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.BaoZhuBossCountChange, msg);
	}
	void ECM_SCBaoZhuSlotSkillsMessage(NetInfo info)
	{
		baozhu.BaoZhuSkills msg = Network.Deserialize<baozhu.BaoZhuSkills>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forbaozhu.BaoZhuSkills");
			return;
		}
		CSPearlInfo.Instance.HandleBaoZhuSlotSkills(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.BaoZhuSlotSkills, msg);
	}
}
