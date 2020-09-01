using stonetreasure;
using System;

public class CSStonetreasureInfo : CSInfo<CSStonetreasureInfo>
{
	int _floorId, _floor, _stoneTypeId;
	bool _isSurpriseAnim;
	int _freeNum = 0;
	//稀有奖励、特殊奖励、惊喜奖励、查看奖励（有可能有惊喜奖励）,石块们信息，普通奖励的石阵们信息
	//点击单独石块信息，石块传送阵信息
	CSBetterLisHot<shizhenRewardsData> rareRewards = new CSBetterLisHot<shizhenRewardsData>();
	CSBetterLisHot<shizhenRewardsData> specialRewards = new CSBetterLisHot<shizhenRewardsData>();
	CSBetterLisHot<shizhenRewardsData> surpriseRewards = new CSBetterLisHot<shizhenRewardsData>();
	CSBetterLisHot<shizhenRewardsData> checkAllRewards = new CSBetterLisHot<shizhenRewardsData>();
	CSBetterLisHot<shizhenRewardsData> surpriseTwoRewards = new CSBetterLisHot<shizhenRewardsData>();
	CSBetterLisHot<shizhenTypeData> shizhenTypeList = new CSBetterLisHot<shizhenTypeData>();
	CSBetterLisHot<shizhenTypeData> shizhenNormalList = new CSBetterLisHot<shizhenTypeData>();
	shizhenTypeData szTypeSingleData = new shizhenTypeData();
	shizhenTypeData szTypeDoorData = new shizhenTypeData();
	PoolHandleManager mPoolHandle = new PoolHandleManager();
	CSBetterLisHot<shizhenRewardsData> surprise = new CSBetterLisHot<shizhenRewardsData>();
	CSBetterLisHot<shizhenRewardsData> normal = new CSBetterLisHot<shizhenRewardsData>();
	string[] costMes;
	public void SetFloorInfoMessage(FloorInfoResponse msg)
	{
		_floorId = msg.stoneId;
		_floor = msg.floor;
		_stoneTypeId = msg.stoneTypeId;
		_isSurpriseAnim = msg.jingxiflag;
		_freeNum = msg.jingxiCostNum;

		SetRewards(msg);
		SetCheckAllRewards(msg);
		SetTombTypeList(msg);
	}
	private void SetRewards(FloorInfoResponse msg)
	{
		rareRewards.Clear();
		specialRewards.Clear();
		surpriseRewards.Clear();
		for (int i = 0; i < msg.stoneReward.Count; i++)
		{
			//稀有
			if (IsOtherRewards(msg.stoneReward[i].itemId, 2))
			{
				SetRewards(msg.stoneReward[i], rareRewards);
			}
			//特殊
			if (IsOtherRewards(msg.stoneReward[i].itemId, 3))
			{
				SetRewards(msg.stoneReward[i], specialRewards);
			}
			//惊喜
			if (IsOtherRewards(msg.stoneReward[i].itemId, 4))
			{
				SetRewards(msg.stoneReward[i], surpriseRewards);
			}
		}
		//设置惊喜+特殊层
		SetSurpriseTwoRewards();
	}
	private void SetSurpriseTwoRewards()
	{
		surpriseTwoRewards.Clear();
		for (int i = 0; i < surpriseRewards.Count; i++)
		{
			surpriseRewards[i].isShow = true;
			surpriseTwoRewards.Add(surpriseRewards[i]);
		}
		for (int i = 0; i < specialRewards.Count; i++)
		{
			surpriseTwoRewards.Add(specialRewards[i]);
		}
	}
	//惊喜+普通，查看全部用
	private void SetCheckAllRewards(FloorInfoResponse msg)
	{
		surprise.Clear();
		normal.Clear();
		checkAllRewards.Clear();
		for (int i = 0; i < msg.stoneReward.Count; i++)
		{
			if (IsOtherRewards(msg.stoneReward[i].itemId, 4))
			{
				SetRewards(msg.stoneReward[i], surprise);
			}
			else
			{
				SetRewards(msg.stoneReward[i], normal);
			}
		}
		for (int i = 0; i < surprise.Count; i++)
		{
			checkAllRewards.Add(surprise[i]);
		}
		for (int i = 0; i < normal.Count; i++)
		{
			checkAllRewards.Add(normal[i]);
		}
	}
	private void SetTombTypeList(FloorInfoResponse msg)
	{
		shizhenTypeList.Clear();
		int id;
		string[] mes;
		for (int i=0;i<msg.stateInfo.Count;i++)
		{
			shizhenTypeData szTypeData = mPoolHandle.GetCustomClass <shizhenTypeData>();
			id = msg.stateInfo[i].itemId;
			mes = UtilityMainMath.StrToStrArr(msg.stateInfo[i].location);
			if (mes == null) return;
			int.TryParse(mes[0], out szTypeData.x);
			int.TryParse(mes[1], out szTypeData.y);
			szTypeData.id = id;
			//有道具处理
			if (id > 0)
			{
				szTypeData.itemNum = msg.stateInfo[i].itemNum;
				szTypeData.itemId = ShiZhenRewardTableManager.Instance.GetShiZhenRewardItemId(id);
				szTypeData.color = Math.Abs(msg.stateInfo[i].color);
				szTypeData.iconId = ItemTableManager.Instance.GetItemIcon(szTypeData.itemId);
			}
			//无道具处理
			else if (id > -3 && id < 0)
				szTypeData.color = Math.Abs(msg.stateInfo[i].itemId);
			//进入下一层
			else if (id == -3)
			{
				szTypeData.nextFloor = Math.Abs(msg.stateInfo[i].itemId);
				szTypeData.color = Math.Abs(msg.stateInfo[i].color);
			}
			if (id > -3 && id <1)
				szTypeData.roundNum = msg.stateInfo[i].colorByRound;
			shizhenTypeList.Add(szTypeData);
		}
	}
	//击碎石块，响应
	public void SetStoneLocationMessage(StoneLocationResponse msg)
	{
		szTypeSingleData = mPoolHandle.GetCustomClass<shizhenTypeData>();
		string[] mesList = UtilityMainMath.StrToStrArr(msg.location);
		if (mesList == null) return;
		int.TryParse(mesList[0], out szTypeSingleData.x);
		int.TryParse(mesList[1], out szTypeSingleData.y);
		int id;
		if (IsStoneChangeInfo(szTypeSingleData))
		{
			id = msg.itemId;
			szTypeSingleData.color = Math.Abs(msg.color);
			if (id > 0)
			{
				ResetAllRewards(id);
				szTypeSingleData.id = id;
				szTypeSingleData.itemNum = msg.itemNum;
				szTypeSingleData.itemId = ShiZhenRewardTableManager.Instance.GetShiZhenRewardItemId(id);
				szTypeSingleData.iconId = ItemTableManager.Instance.GetItemIcon(szTypeSingleData.itemId);
			}
			else if (id <= -3)
			{
				szTypeSingleData.nextFloor = Math.Abs(id);
			}
			else if(id == 0)
			{
				szTypeSingleData.roundNum = msg.colorByRound;
			}
			shizhenTypeList.Add(szTypeSingleData);
		}
	}
	//进行道具扣除处理
	private void ResetAllRewards(int id)
	{
		ResetOtherRewards(rareRewards, id);
		ResetOtherRewards(specialRewards, id);
		ResetOtherRewards(surpriseRewards, id);
		ResetOtherRewards(checkAllRewards, id);
		SetSurpriseTwoRewards();
		//ResetOtherRewards(surpriseTwoRewards, id);
	}
	private void ResetOtherRewards(CSBetterLisHot<shizhenRewardsData> rewards,int id)
	{
		for (int i = 0; i < rewards.Count; i++)
		{
			if (rewards[i].id == id)
			{
				rewards[i].getNum += 1;
				if (rewards[i].getNum >= rewards[i].sum)
				{
					rewards[i].getNum = rewards[i].sum;
				}
			}
		}
	}
	public void SetDownLocationMessage(DownLocationResponse msg)
	{
		szTypeDoorData = mPoolHandle.GetCustomClass<shizhenTypeData>();
		string[] mesList = UtilityMainMath.StrToStrArr(msg.location);
		if (mesList == null) return;
		int.TryParse(mesList[0], out szTypeDoorData.x);
		int.TryParse(mesList[1], out szTypeDoorData.y);
		szTypeDoorData.color = Math.Abs(msg.color);
		szTypeDoorData.nextFloor = 3;
		if (!IsHaveDoorInfo(szTypeDoorData))
		{
			shizhenTypeList.Add(szTypeDoorData);
		}
	}
	public void ClearDownLocationMessage()
	{
		szTypeDoorData = mPoolHandle.GetCustomClass<shizhenTypeData>();
	}
	public void SetNormalAndDownMessage(GetNormalAndDownResponse msg)
	{
		shizhenNormalList.Clear();
		int id;
		string[] mes;
		for (int i = 0; i < msg.stateInfo.Count; i++)
		{
			shizhenTypeData szTypeData = mPoolHandle.GetCustomClass<shizhenTypeData>();
			id = msg.stateInfo[i].itemId;
			mes = UtilityMainMath.StrToStrArr(msg.stateInfo[i].location);
			if (mes == null) return;
			int.TryParse(mes[0], out szTypeData.x);
			int.TryParse(mes[1], out szTypeData.y);
			szTypeData.id = id;
			//有道具处理
			if (id > 0)
			{
				szTypeData.itemNum = ShiZhenRewardTableManager.Instance.GetShiZhenRewardNum(id);
				szTypeData.itemId = ShiZhenRewardTableManager.Instance.GetShiZhenRewardItemId(id);
				szTypeData.color = Math.Abs(msg.stateInfo[i].color);
				szTypeData.iconId = ItemTableManager.Instance.GetItemIcon(szTypeData.itemId);
			}
			else if(id == 0)
			{
				szTypeData.color = Math.Abs(msg.stateInfo[i].color);
				szTypeData.roundNum = msg.stateInfo[i].colorByRound;
			}
			else if(id == -3)
				szTypeData.color = Math.Abs(msg.stateInfo[i].color);
			shizhenNormalList.Add(szTypeData);
		}
	}
	private bool IsHaveDoorInfo(shizhenTypeData szTypeDoorData)
	{
		for (int i = 0; i < shizhenTypeList.Count; i++)
		{
			if(shizhenTypeList[i].x == szTypeDoorData.x && shizhenTypeList[i].y == szTypeDoorData.y)
			{
				return true;
			}
		}
		return false;
	}
	public shizhenTypeData GetStoneLocationMessage()
	{
		return szTypeSingleData;
	}
	public shizhenTypeData GetDownLocationMessage()
	{
		return szTypeDoorData;
	}
	private bool IsStoneChangeInfo(shizhenTypeData szTypeData)
	{
		for(int i=0;i< shizhenTypeList.Count;i++)
		{
			if(shizhenTypeList[i].x == szTypeData.x && shizhenTypeList[i].y == szTypeData.y)
			{
				return false;
			}
		}
		return true;
	}
	//刷新面板，设置奖励列表
	private void SetRewards(StoneReward stoneReward, CSBetterLisHot<shizhenRewardsData> rewardsDataList)
	{
		shizhenRewardsData rewardsData = mPoolHandle.GetCustomClass<shizhenRewardsData>();
		rewardsData.getNum = stoneReward.getNum;
		rewardsData.id = stoneReward.itemId;
		rewardsData.sum = stoneReward.sum;
		rewardsData.itemId = ShiZhenRewardTableManager.Instance.GetShiZhenRewardItemId(rewardsData.id);
		rewardsDataList.Add(rewardsData);
	}
	//判断是否是稀有、特殊、惊喜道具
	private bool IsOtherRewards(int _itemId, int _type)
	{
		int type = ShiZhenRewardTableManager.Instance.GetShiZhenRewardType(_itemId);
		return type == _type;
	}
	public void SetFreeNum()
	{
		_freeNum = _freeNum > 0 ? _freeNum -1:0; 
	}
	//检测稀有以上的装备，是否都领取完
	public bool IsMoveTips()
	{
		return CheckSpecial(rareRewards) || CheckSpecial(specialRewards) || CheckSpecial(surpriseRewards);
	}
	private bool CheckSpecial(CSBetterLisHot<shizhenRewardsData> rewardsNew)
	{
		for (int i = 0; i < rewardsNew.Count; i++)
		{
			if (rewardsNew[i].getNum < rewardsNew[i].sum)
			{
				return true;
			}
		}
		return false;
	}
	//检测小红点
	public bool HasNotify
	{
		get { return CanCost(); }
	}
	public bool CanCost()
	{
		int costId, costNeed;
		long have;
		if(costMes == null)
		{
			string tempStr = SundryTableManager.Instance.GetSundryEffect(465);
			costMes = UtilityMainMath.StrToStrArr(tempStr);
		}
		if (costMes == null) return false;
		int.TryParse(costMes[0],out costId);
		int.TryParse(costMes[1], out costNeed);
		if (costId <= 0) return false;
		have = costId.GetItemCount();
		return have >= costNeed;
	}
	public int GetFloor()
	{
		return _floor;
	}
	public int GetFloorId()
	{
		return _floorId;
	}
	public int GetStoneTypeId()
	{
		return _stoneTypeId;
	}
	public int GetFreeNum()
	{
		return _freeNum;
	}
	public bool GetIsSurpriseAnim()
	{
		return _isSurpriseAnim;
	}
	public CSBetterLisHot<shizhenRewardsData> GetRareRewards()
	{
		return rareRewards;
	}
	public CSBetterLisHot<shizhenRewardsData> GetSpecialRewards()
	{
		return specialRewards;
	}
	public CSBetterLisHot<shizhenRewardsData> GetSurpriseRewards()
	{
		return surpriseRewards;
	}
	public CSBetterLisHot<shizhenRewardsData> GetCheckAllRewards()
	{
		return checkAllRewards;
	}

	//特殊+惊喜层显示用
	public CSBetterLisHot<shizhenRewardsData> GetSurpriseTwoRewards()
	{
		return surpriseTwoRewards;
	}
	public CSBetterLisHot<shizhenTypeData> GetShiZhenTypeList()
	{
		return shizhenTypeList;
	}
	public CSBetterLisHot<shizhenTypeData> GetShiZhenNormalList()
	{
		return shizhenNormalList;
	}
	public override void Dispose()
	{
		mPoolHandle?.OnDestroy();
		mPoolHandle = null;

		rareRewards.Clear();
		specialRewards.Clear();
		surpriseRewards.Clear();
		checkAllRewards.Clear();
		surpriseTwoRewards.Clear();
		shizhenTypeList.Clear();
		shizhenNormalList.Clear();
		surprise.Clear();
		normal.Clear();

		rareRewards = null;
		specialRewards = null;
		surpriseRewards = null;
		checkAllRewards = null;
		surpriseTwoRewards = null;
		shizhenTypeList = null;
		shizhenNormalList = null;
		szTypeSingleData = null;
		szTypeDoorData = null;
		surprise = null;
		normal = null;
		costMes = null;

		_floorId = 0;
		_floor = 0;
		_freeNum = 0;
	}
}

//石阵奖励显示
public class shizhenRewardsData: IDispose
{
	public shizhenRewardsData() { }
	public shizhenRewardsData(int _getNum, int _id, int _sum,int _itemId,bool _isShow)
	{
		getNum = _getNum;        //拥有数
		id = _id;				//表id
		sum = _sum;             //最大数
		itemId = _itemId;       //道具id
		isShow = _isShow;		//是否显示惊喜角标
	}
	public int getNum = 0;
	public int id = 0;
	public int sum = 0;
	public int itemId = 0;
	public bool isShow = false;
	public void Dispose() { }
}
//石阵石块显示用
public class shizhenTypeData : IDispose
{
	public shizhenTypeData() { }
	public shizhenTypeData(int _x,int _y, int _id,string _iconId, 
		int _itemNum, int _color,int _nextFloor,int _itemId)
	{
		x = _x;						 //坐标x
		y = _y;						//坐标y
		id = _id;					//表id，分为>0道具、0空白、-1深红色、-2浅红色、-3下一层
		iconId = _iconId;			//道具Icon
		itemNum = _itemNum;			//道具数量
		color = _color;             //表id>0的时候才会用到 -1深红色、-2浅红色
		nextFloor = _nextFloor;		//传送门
		itemId = _itemId;			//道具id	
	}
	public string location = "";
	public int x = -99;
	public int y = -99;
	public int id = 0;
	public int color = -99;
	public int itemNum = 0;
	public string iconId = "";
	public int nextFloor = 0;
	public int roundNum = -99;
	public int itemId = 0;
	public void Dispose()
	{
		location = "";
		x = 0;
		y = 0;
		id = 0;
		color = 0;
		itemNum = 0;
		iconId = "";
		nextFloor = 0;
		roundNum = 0;
		itemId = 0;
	}
}