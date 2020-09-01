public partial class CSNetFashion : CSNetBase
{
	/// <summary>
	/// 时装数据
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCAllFashionInfoMessage(NetInfo info)
	{
		fashion.AllFashionInfo msg = Network.Deserialize<fashion.AllFashionInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forfashion.AllFashionInfo");
			return;
		}
		CSFashionInfo.Instance.GetAllFashionInfo(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.AllFashionInfo, msg);
	}
	/// <summary>
	/// 穿时装回应
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCEquipFashionMessage(NetInfo info)
	{
		fashion.FashionIdList msg = Network.Deserialize<fashion.FashionIdList>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forfashion.FashionInfo");
			return;
		}
		CSFashionInfo.Instance.HandleEquipFashion(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.EquipFashion, msg);
	}
	/// <summary>
	/// 升星回应
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCFashionStarLevelUpMessage(NetInfo info)
	{
		fashion.FashionInfo msg = Network.Deserialize<fashion.FashionInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forfashion.FashionInfo");
			return;
		}
		CSFashionInfo.Instance.HandleFashionStarLevelUp(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.FashionStarLevelUp, msg);
	}
	/// <summary>
	/// 添加时装
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCAddFashionMessage(NetInfo info)
	{
		fashion.FashionInfo msg = Network.Deserialize<fashion.FashionInfo>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forfashion.FashionInfo");
			return;
		}
		CSFashionInfo.Instance.AddFashion(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.AddFashion, msg);
	}
	/// <summary>
	/// 删除时装
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCRemoveFashionMessage(NetInfo info)
	{
		fashion.FashionIdList msg = Network.Deserialize<fashion.FashionIdList>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forfashion.FashionIdList");
			return;
		}
		CSFashionInfo.Instance.RemoveFashion(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.RemoveFashion, msg);
	}
	/// <summary>
	/// 卸下时装
	/// </summary>
	/// <param name="info"></param>
	void ECM_SCUnEquipFashionMessage(NetInfo info)
	{
		fashion.FashionId msg = Network.Deserialize<fashion.FashionId>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forfashion.FashionId");
			return;
		}
		CSFashionInfo.Instance.UnEquipFashion(msg);
		HotManager.Instance.EventHandler.SendEvent(CEvent.UnEquipFashion, msg);
	}
}
