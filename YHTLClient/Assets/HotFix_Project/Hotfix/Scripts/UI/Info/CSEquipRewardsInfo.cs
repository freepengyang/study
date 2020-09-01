using activity;
using System.Collections.Generic;

public class CSEquipRewardsInfo : CSInfo<CSEquipRewardsInfo>
{
	ILBetterList<EquipRewardsData> dataList = new ILBetterList<EquipRewardsData>();//展示数据的列表
	Dictionary<int, EquipRewardsData> allDic = new Dictionary<int, EquipRewardsData>();//所有数据
	PoolHandleManager mPoolHandle = new PoolHandleManager();
	EquipRewardsData rewardData;//弹出奖励信息用
	EquipXuanShangData tempData = new EquipXuanShangData();//后端临时数据存储
	int activityType, type, activityId;
	int woLongId = 10105;//卧龙装备悬赏活动id
	int perId = 10124;//普通装备悬赏活动id
	bool isShowTipPanel = true;//是否显示奖励预览
	bool woLongRedPoint = false;
	bool perRedPoint = false;
	/// <summary>
	/// 后端数据处理
	/// </summary>
	/// <param name="msg"></param>
	public void SetEquipRewardsMessage(ResEquipXuanShang msg)
	{
		type = msg.type;
		activityId = msg.activityId;
		if (activityId == woLongId)
		{
			activityType = (int)OpenActivityType.EquipRewards;
			woLongRedPoint = false;
		}
		else if (activityId == perId)
		{
			activityType = (int)OpenActivityType.PerEquipRewards;
			perRedPoint = false;
		}
		for (int i = 0; i < msg.equipXuanShangData.Count; i++)
		{
			tempData = msg.equipXuanShangData[i];
			if (allDic.ContainsKey(tempData.id))
			{
				allDic[tempData.id].name = tempData.name;
				allDic[tempData.id].xuanShangType = tempData.xuanShangType;
				SetSortId(allDic[tempData.id]);
				CheckRedPoint(allDic[tempData.id], activityType);
			}
			else
			{
				EquipRewardsData data = mPoolHandle.GetCustomClass<EquipRewardsData>();
				data.id = tempData.id;
				data.activityType = activityType;
				data.activityId = activityId;
				data.isWoLong = activityId == woLongId;
				data.goalId = tempData.goalId;
				data.name = tempData.name;
				data.xuanShangType = tempData.xuanShangType;
				SetSortId(data);
				CheckRedPoint(data, activityType);
				allDic.Add(tempData.id, data);
			}
		}
		//通知客户端弹出小面板
		if (type == 2)
		{
			for (int i = 0; i < msg.equipXuanShangData.Count; i++)
			{
				tempData = msg.equipXuanShangData[i];
				if (tempData.goalId == 2)
				{
					if (rewardData == null)
						rewardData = mPoolHandle.GetCustomClass<EquipRewardsData>();
					rewardData.id = tempData.id;
					rewardData.isWoLong = activityId == woLongId;
					//打开小面板
					if (isShowTipPanel)
						UIManager.Instance.CreatePanel<UIEquipRewardShowPanel>();
					break;
				}
			}
		}
	}
	public EquipRewardsData GetRewardData()
	{
		return rewardData;
	}
	public void SetIsShowTipPanel(bool isShow)
	{
		isShowTipPanel = isShow;
	}
	/// <summary>
	/// 当前后端发的活动id
	/// </summary>
	/// <returns></returns>
	public int GetAcitvityId()
	{
		return activityId;
	}
	/// <summary>
	/// 获得卧龙装备悬赏活动id
	/// </summary>
	/// <returns></returns>
	public int GetWoLongId()
	{
		return woLongId;
	}
	/// <summary>
	/// 获得普通装备悬赏活动id
	/// </summary>
	/// <returns></returns>
	public int GetPerId()
	{
		return perId;
	}
	/// <summary>
	/// 根据活动类型，全服还是个人获得数组
	/// </summary>
	/// <param name="activityType"></param>
	/// <param name="goalId"></param>
	/// <returns></returns>
	public ILBetterList<EquipRewardsData> GetRewardsList(int activityType, int goalId)
	{
		if (allDic.Count > 0)
		{
			var dic = allDic.GetEnumerator();
			dataList.Clear();
			while (dic.MoveNext())
			{
				if (dic.Current.Value.activityType == activityType &&
					dic.Current.Value.goalId == goalId)
				{
					dataList.Add(dic.Current.Value);
				}
			}
		}
		if (goalId == 2)
			dataList.Sort((x, y) => { return x.sortId.CompareTo(y.sortId); });
		//dataList.Sort(Compaer);
		return dataList;
	}
	private void SetSortId(EquipRewardsData data)
	{
		if (data.xuanShangType == 3)
			data.sortId = 2;
		else if (data.xuanShangType == 2)
			data.sortId = 3;
		else
			data.sortId = 1;
	}
	private void CheckRedPoint(EquipRewardsData data,int type)
	{
		if (data.goalId == 2 && data.xuanShangType == 1)
		{
			if (type == (int)OpenActivityType.EquipRewards && !woLongRedPoint)
				woLongRedPoint = true;
			else if (type == (int)OpenActivityType.PerEquipRewards && !perRedPoint)
				perRedPoint = true;
		}
	}
	/// <summary>
	/// 个人装备排序
	/// </summary>
	/// <param name="l"></param>
	/// <param name="r"></param>
	/// <returns></returns>
	//private void Compaer(ref long sortValue, EquipRewardsData r)
	//{
	//	if (r.xuanShangType == 3)
	//		sortValue = r.id + 2000;
	//	if (r.xuanShangType == 2)
	//		sortValue = r.id + 3000;
	//	else
	//		sortValue = r.id+1000;
	//}
	/// <summary>
	/// 卧龙个人装备小红点检测
	/// </summary>
	/// <returns></returns>
	public bool WoLongRedPoint()
	{
		return woLongRedPoint;
	}
	/// <summary>
	/// 普通个人装备小红点检测
	/// </summary>
	/// <returns></returns>
	public bool PerRedPoint()
	{
		return perRedPoint;
	}
	public override void Dispose()
	{
		mPoolHandle?.OnDestroy();
		mPoolHandle = null;
		dataList = null;
		allDic = null;
		rewardData = null;
		tempData = null;

		activityType = 0;
		type = 0;
		woLongId = 0;
		perId = 0;
		activityId = 0;
		isShowTipPanel = false;
	}
}

public class EquipRewardsData : IDispose
{
	public int id { get; set; }//表id
	public int goalId { get; set; }//1全服，2个人
	public int xuanShangType { get; set; }//0:虚位以待 1:可领取 2:已领取 3:不可领
	public int sortId { get; set; }//排序id用
	public int activityType { get; set; }//跳转主页签用
	public int activityId { get; set; }//活动id
	public bool isWoLong { get; set; }//是否是卧龙装备悬赏活动
	public string name { get; set; }//玩家名
	public void Dispose()
	{
		id = 0;
		goalId = 0;
		xuanShangType = 0;
		sortId = 0;
		activityType = 0;
		activityId = 0;
		isWoLong = false;
		name = string.Empty;
	}
}