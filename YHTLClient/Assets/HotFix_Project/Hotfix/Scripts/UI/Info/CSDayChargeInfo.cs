using daycharge;
using System.Collections.Generic;

public class CSDayChargeInfo : CSInfo<CSDayChargeInfo>
{
	PoolHandleManager mPoolHandle = new PoolHandleManager();
	DayChargeData dayChargeData = new DayChargeData();
	int rewardsMax = 0;     //奖励最大值
	int tableId = 1;        //默认第一个
	bool isShowMapRed = true;
	int mapItemId = 0;
	int mapItemCost = 0;
	public void SetDayChargeInfoMessage(DayChargeResponse msg)
	{
		//var arr = DayChargeTableManager.Instance.array.gItem.handles;
		//long openServerDays = CSMainPlayerInfo.Instance.ServerOpenDay;
		//int maxId = DayChargeTableManager.Instance.GetDayChargeId(arr.Length);
		//long maxDay = DayChargeTableManager.Instance.GetDayChargeOpenDay(maxId);
		//if (openServerDays >= maxDay)
		//{
		//	tableId = maxId;
		//}
		//else
		//{
		//	TABLE.DAYCHARGE dayChargeItem = null;
		//	for (int i = 0, max = arr.Length; i < max; ++i)
		//	{
		//		dayChargeItem = arr[i].Value as TABLE.DAYCHARGE;
		//		if (dayChargeItem.openDay > openServerDays && dayChargeItem.id > 1)
		//		{
		//			tableId = dayChargeItem.id - 1;
		//			break;
		//		}
		//	}
		//}
		string[] rewardsMes = UtilityMainMath.StrToStrArr(DayChargeTableManager.Instance.GetDayChargeRewards(tableId));
		if (rewardsMes == null) return;
		dayChargeData = mPoolHandle.GetCustomClass<DayChargeData>();
		rewardsMax = rewardsMes.Length;
		dayChargeData.dayCharge = msg.dayCharge;
		dayChargeData.todayEnterNum = msg.todayEnterDayChargeMapTimes;
		dayChargeData.btnShow = msg.dones.Count < msg.cans.Count ? 1 : 0;

	}
	public void ResetDayChargeInfoMessage(GetRewardResponse msg)
	{
		//0:条件不满足；1：领取成功;2:已经领取过
		dayChargeData.state = msg.state;
		if (msg.state == 1)
		{
			if (dayChargeData.curId < rewardsMax - 1)
			{
				dayChargeData.curId += 1;
				dayChargeData.leftDayCharge = GetLeftCharge();
				dayChargeData.btnShow = dayChargeData.leftDayCharge <= 0 ? 1 : 0;
				SetBoxId();
			}
			else
			{
				dayChargeData.curId = rewardsMax - 1;
				dayChargeData.btnShow = 2;
			}
		}
	}
	private void SetBoxId()
	{
		string[] boxsMes;
		boxsMes = UtilityMainMath.StrToStrArr(DayChargeTableManager.Instance.GetDayChargeRewards(tableId));
		if (boxsMes == null) return;
		int.TryParse(boxsMes[dayChargeData.curId], out dayChargeData.boxId);
	}
	private int GetLeftCharge()
	{
		int leftCharge = 0;
		int costCharge = 0;
		string[] mes = UtilityMainMath.StrToStrArr(DayChargeTableManager.Instance.GetDayChargeCost(tableId));
		if (mes == null) return leftCharge;
		int.TryParse(mes[dayChargeData.curId], out costCharge);
		if (costCharge >= dayChargeData.dayCharge)
		{
			leftCharge = costCharge - dayChargeData.dayCharge;
		}
		return leftCharge;
	}
	//控制福利大厅面板显示/隐藏页签
	public bool isReceived()
	{
		bool isShow = true;
		if (dayChargeData.curId < rewardsMax - 1)
		{
			isShow = true;
		}
		else
		{
			isShow = dayChargeData.btnShow != 2;
		}
		return isShow;
	}
	//是否显示小红点
	public bool HasNotify
	{
		get { return isRedPointShow(); }
	}
	private bool isRedPointShow()
	{
		bool isShow = false;
		if (isReceived())
		{
			isShow = dayChargeData.btnShow == 1;
		}
		return isShow;
	}
	public DayChargeData GetChargeData()
	{
		return dayChargeData;
	}
	public int GetDayChargeMapId()
	{
		int mapId = 0;
		var arr = InstanceTableManager.Instance.array.gItem.handles;
		for (int i = 0, max = arr.Length; i < max; ++i)
		{
			var item = arr[i].Value as TABLE.INSTANCE;
			if (item.type == 19)
			{
				mapId = item.id;
			}
		}
		return mapId;
	}
	public void SetIsShowMapRed()
	{
		isShowMapRed = false;
	}
	public bool GetIsShowMapRed()
	{
		return isShowMapRed;
	}
	public bool IsShowMapRedPoint()
	{
		int num = CSMainPlayerInfo.Instance.RoleExtraValues.todayTimes;
		if (mapItemId <= 0)
		{
			int mapId = GetDayChargeMapId();
			string costStr = InstanceTableManager.Instance.GetInstanceRequireItems(mapId);
			List<int> costList = UtilityMainMath.SplitStringToIntList(costStr);
			mapItemId = costList[0];
			mapItemCost = costList[1];
		}
		long have = mapItemId.GetItemCount();
		if (num > 0 && !isShowMapRed)
		{
			if (have < mapItemCost)
				return false;
			else
				return true;
		}
		else
		{
			return isShowMapRed;
		}
	}
	public bool IsSendNet()
	{
		if (dayChargeData.dayCharge > 0 && dayChargeData.todayEnterNum <= 0) { return true; }
		return false;
	}
	public int GetDayCharge()
	{
		return dayChargeData.dayCharge;
	}
	public override void Dispose()
	{
		mPoolHandle = null;
		dayChargeData = null;
		rewardsMax = 0;
		tableId = 1;
		isShowMapRed = true;
		mapItemId = 0;
		mapItemCost = 0;
		mPoolHandle?.OnDestroy();
	}
}
public class DayChargeData : IDispose
{
	public DayChargeData() { }
	public DayChargeData(int _dayCharge, int _leftDayCharge, int _curId, int _boxId,
		int _btnShow, int _state, int _todayEnterNum)
	{
		dayCharge = _dayCharge;
		leftDayCharge = _leftDayCharge;
		curId = _curId;
		boxId = _boxId;
		btnShow = _btnShow;
		state = _state;
		todayEnterNum = _todayEnterNum;
	}
	public int dayCharge = 0;                       //当前充值金额
	public int leftDayCharge = 0;                   //下一级奖励所剩额度
	public int curId = 0;                           //当前奖励的索引
	public int boxId = 0;                           //box表id
	public int btnShow = 0;                         //0:充值页面，1领取页面,2奖励已全部领完
	public int state = 0;                           //0:条件不满足；1：领取成功;2:已经领取过
	public int todayEnterNum = 0;                   //当天进入每日充值地图次数
	public void Dispose() { }
}