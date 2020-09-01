using System.Collections.Generic;
using Google.Protobuf.Collections;
public class CSMaFaInfo : CSInfo<CSMaFaInfo>
{
	PoolHandleManager mPoolHandle = new PoolHandleManager();
	ILBetterList<MaFaData> itemDataList;
	MaFaActiveData activeData;
	Dictionary<int, int> normalDic = new Dictionary<int, int>();
	Dictionary<int, int> advancedDic = new Dictionary<int, int>();
	int minLayer = 0;
	int boxNeedCount = 0;
	int keyId = 26;
	int keyNeed = 0;
	string layerStr = "";
	mafa.MafaBoxReward maFaRewards;
	/// <summary>
	/// 分析服务端数据，并保存活动数据
	/// </summary>
	/// <param name="msg"></param>
	public void SetActiveInfo(mafa.MafaInfo msg)
	{
		layerStr = CSString.Format(1810).BBCode(ColorType.MainText);
		if (activeData == null)
			activeData = mPoolHandle.GetCustomClass<MaFaActiveData>();
		if (boxNeedCount <= 0)
		{
			string boxStr = SundryTableManager.Instance.GetSundryEffect(738);
			int.TryParse(boxStr, out boxNeedCount);
		}
		if (keyNeed <= 0)
		{
			string keyStr = SundryTableManager.Instance.GetSundryEffect(739);
			int.TryParse(keyStr, out keyNeed);
		}
		activeData.id = msg.season;
		activeData.exp = msg.exp;
		activeData.superUnlock = msg.superUnlock;
		activeData.boxCount = msg.boxCount;
		activeData.layer = msg.layer;
		activeData.boxLevel = msg.boxLevel;
		activeData.seasonEndDay = msg.seasonEndDay;
		SetItemDataList(msg.layers);
	}
	/// <summary>
	/// 获取活动倒计时时间
	/// </summary>
	/// <returns></returns>
	public long GetActiveTime()
	{
		long durartion = 0;
		long openServerDays = CSMainPlayerInfo.Instance.ServerOpenDay;
		long tim = CSServerTime.GetZeroClockGapSeconds();
		durartion = (activeData.seasonEndDay - openServerDays) * 24 * 60 * 60;
		durartion += tim;
		return durartion;
	}
	/// <summary>
	/// 获取活动名
	/// </summary>
	/// <returns></returns>
	public string GetAcitveName()
	{
		return MafaActivityTableManager.Instance.GetMafaActivitySmallName(activeData.id);
	}
	/// <summary>
	/// 获取活动数据
	/// </summary>
	/// <returns></returns>
	public MaFaActiveData GetAcitveData()
	{
		return activeData;
	}
	/// <summary>
	/// 设置奖励列表状态
	/// </summary>
	/// <param name="layers"></param>
	public void SetItemDataList(RepeatedField<mafa.MafaLayer> layers)
	{
		for (int i = 0; i < layers.Count; i++)
		{
			if (layers[i].isSuper == 0)
				if (normalDic.ContainsKey(layers[i].layer))
					normalDic[layers[i].layer] = (int)UIMaFaPanel.ItemState.Get;
				else
					normalDic.Add(layers[i].layer, (int)UIMaFaPanel.ItemState.Get);
			else if (layers[i].isSuper == 1)
			{
				if (advancedDic.ContainsKey(layers[i].layer))
					advancedDic[layers[i].layer] = (int)UIMaFaPanel.ItemState.Get;
				else
					advancedDic.Add(layers[i].layer, (int)UIMaFaPanel.ItemState.Get);
			}
		}
		if (itemDataList == null)
			itemDataList = mPoolHandle.GetSystemClass<ILBetterList<MaFaData>>();
		else
			itemDataList.Clear();

		var arr = MafaActivityRewardTableManager.Instance.array.gItem.handles;
		int rewardMin = 0;
		int rewardMax = 0;
		SetRewardsMinAndMax(ref rewardMin, ref rewardMax);
		for (int k = rewardMin, max = rewardMax; k < max; ++k)
		{
			if (arr[k] == null) continue;
			var item = arr[k].Value as TABLE.MAFAACTIVITYREWARD;
			if (item.descid == activeData.id)
			{
				MaFaData data = mPoolHandle.GetCustomClass<MaFaData>();
				data.layer = item.layer;
				SetListState(normalDic, false, item.layer, ref data.state1);
				SetListState(advancedDic, true, item.layer, ref data.state2);
				SetListItem(item.reward1, ref data.id1, ref data.num1);
				SetListItem(item.reward2, ref data.id2, ref data.num2);
				data.layerStr = layerStr;
				itemDataList.Add(data);
			}
		}
		SetMinLayer();
	}
	/// <summary>
	/// 获取赛季奖励的最小序号和最大序号
	/// </summary>
	/// <param name="rewardMin"></param>
	/// <param name="rewardMax"></param>
	private void SetRewardsMinAndMax(ref int rewardMin,ref int rewardMax)
	{
		IntArray nums = MafaActivityTableManager.Instance.GetMafaActivityRewardNum(activeData.id);
		rewardMin = nums[0] - 1;
		rewardMax = nums[1] + 1;
	}
	/// <summary>
	/// 设置跳转位置用，取连续获得的最小值
	/// </summary>
	private void SetMinLayer()
	{
		int nMinLayer = 0;
		int aMinLayer = 0;
		SetMinLayer(1, ref nMinLayer);
		if (activeData.superUnlock == 1)
		{
			SetMinLayer(2, ref aMinLayer);
			minLayer = nMinLayer < aMinLayer ? minLayer : aMinLayer;
		}
		else
			minLayer = nMinLayer;
	}
	private void SetMinLayer(int idx,ref int minLayer)
	{
		int state = 0;
		for (int i = 0; i < itemDataList.Count; i++)
		{
			if (idx == 1)
				state = itemDataList[i].state1;
			else if (idx == 2)
				state = itemDataList[i].state2;

			if (state == (int)UIMaFaPanel.ItemState.Get)
				minLayer++;
			else
				break;
		}
	}
	/// <summary>
	/// 获得跳转位置最小值
	/// </summary>
	/// <returns></returns>
	public int GetMinLayer()
	{
		return minLayer;
	}

	public bool GetBoxRedPoint()
	{
		bool isRed = false;
		long keyHave = keyId.GetItemCount();
		
		if (keyHave >= keyNeed)
			isRed = true;
		return isRed;
	}
	public int GetKeyNeed()
	{
		return keyNeed;
	}
	public int GetKeyId()
	{
		return keyId;
	}
	public int GetBoxNeedCount()
	{
		return boxNeedCount;
	}
	/// <summary>
	/// 检测奖励里有没有可领取，红点检测
	/// </summary>
	/// <returns></returns>
	public bool GetRewardsRedPoint()
	{
		bool isRed = false;
		GetRewardsRedPoint(1,ref isRed);
		if (activeData == null) return isRed;
		if(activeData.superUnlock == 1 && !isRed)
			GetRewardsRedPoint(2, ref isRed);
		return isRed;
	}
	private void GetRewardsRedPoint(int idx, ref bool _isRed)
	{
		int state = 0;
		if (itemDataList == null) return;
		for (int i = 0; i < itemDataList.Count; i++)
		{
			state = idx == 1 ? itemDataList[i].state1 : itemDataList[i].state2;
			if (state == (int)UIMaFaPanel.ItemState.CanGet)
			{
				_isRed = true;
				break;
			}
		}
	}
	/// <summary>
	/// 获取剩余次数可打开大宝箱，1为可获取
	/// </summary>
	/// <returns></returns>
	public int GetLeftNum()
	{
		int num = 0;
		int boxCount = activeData.boxCount;
		if (boxCount >= boxNeedCount)
		{
			num = boxCount % boxNeedCount;
			num = boxNeedCount - num;
		}
		else
			num = boxNeedCount - boxCount;
		return num;
	}
	/// <summary>
	/// 设置玛法奖励数据列表中的状态
	/// </summary>
	/// <param name="dic"></param>
	/// <param name="isAdvanced"></param>
	/// <param name="layer"></param>
	/// <param name="state"></param>
	private void SetListState(Dictionary<int, int> dic,bool isAdvanced ,int layer,ref int state)
	{
		if(!isAdvanced)
			SetListState(dic, layer, ref state);
		else
		{
			if(activeData.superUnlock == 1)
				SetListState(dic, layer,ref state);
			else
				state = (int)UIMaFaPanel.ItemState.Locked;
		}
	}
	/// <summary>
	/// 设置已领取、未领取、未达成
	/// </summary>
	/// <param name="dic"></param>
	/// <param name="layer"></param>
	/// <param name="state"></param>
	private void SetListState(Dictionary<int, int> dic, int layer, ref int state)
	{
		if (dic.ContainsKey(layer))
			state = dic[layer];
		else if (layer <= activeData.layer)
			state = (int)UIMaFaPanel.ItemState.CanGet;
		else
			state = (int)UIMaFaPanel.ItemState.NotReach;
	}
	/// <summary>
	/// 设置道具id，数量
	/// </summary>
	/// <param name="rewards"></param>
	/// <param name="id"></param>
	/// <param name="num"></param>
	private void SetListItem(IntArray rewards,ref int id, ref int num)
	{
		id = rewards[0];
		num = rewards[1];
	}
	/// <summary>
	/// 领取层数奖励变动
	/// </summary>
	/// <param name="msg"></param>
	public void SetMafaLayerChange(mafa.MafaLayerList msg)
	{
		int keyId = 0;
		for (int i = 0; i < msg.layers.Count; i++)
		{
			keyId = msg.layers[i].layer;
			if (msg.layers[i].isSuper != 1)
			{
				if (normalDic.ContainsKey(keyId))
					normalDic[keyId] = (int)UIMaFaPanel.ItemState.Get;
				else
					normalDic.Add(keyId, (int)UIMaFaPanel.ItemState.Get);
			}
			else
			{
				if (advancedDic.ContainsKey(keyId))
					advancedDic[keyId] = (int)UIMaFaPanel.ItemState.Get;
				else
					advancedDic.Add(keyId, (int)UIMaFaPanel.ItemState.Get);
			}
		}
		ResetChangeLayerInfo(normalDic,true);
		ResetChangeLayerInfo(advancedDic, false);
		SetMinLayer();
	}
	/// <summary>
	/// 根据字典，设置层数奖励状态
	/// </summary>
	/// <param name="dic"></param>
	/// <param name="isNormal"></param>
	private void ResetChangeLayerInfo(Dictionary<int, int> dic,bool isNormal)
	{
		var arr = dic.GetEnumerator();
		int key, value;
		while (arr.MoveNext())
		{
			key = arr.Current.Key - 1;
			value = arr.Current.Value;
			if (isNormal)
				itemDataList[key].state1 = value;
			else
				itemDataList[key].state2 = value;
		}
	}
	/// <summary>
	/// 获得层数奖励状态
	/// </summary>
	/// <returns></returns>
	public ILBetterList<MaFaData> GetItemDataList()
	{
		return itemDataList;
	}
	/// <summary>
	/// 战令经验变动设置
	/// </summary>
	/// <param name="msg"></param>
	public void MafaExpChange(mafa.MafaExpChange msg)
	{
		activeData.exp = msg.exp;
		activeData.layer = msg.layer;
		ReSetItemDataList();
	}
	/// <summary>
	/// 战令升级，刷新可领取数据
	/// </summary>
	private void ReSetItemDataList()
	{
		for (int i = 0; i < itemDataList.Count; i++)
		{
			MaFaData data = itemDataList[i];
			if (data.layer <= activeData.layer)
			{
				if (data.state1 == (int)UIMaFaPanel.ItemState.NotReach)
					data.state1 = (int)UIMaFaPanel.ItemState.CanGet;

				if (activeData.superUnlock == 1)
				{
					if (data.state2 == (int)UIMaFaPanel.ItemState.NotReach ||
						data.state2 == (int)UIMaFaPanel.ItemState.Locked)
						data.state2 = (int)UIMaFaPanel.ItemState.CanGet;
				}
			}
			else
			{
				if (activeData.superUnlock == 1)
				{
					if (data.state2 == (int)UIMaFaPanel.ItemState.Locked)
						data.state2 = (int)UIMaFaPanel.ItemState.NotReach;
				}
			}
		}
	}
	/// <summary>
	/// 进阶高级，刷新数据
	/// </summary>
	/// <param name="msg"></param>
	public void MafaSuperLayerUnlock(mafa.MafaSuperLayerUnlock msg)
	{
		activeData.boxLevel = msg.boxLevel;
		activeData.superUnlock = 1;
		ReSetItemDataList();
		SetMinLayer();
	}
	/// <summary>
	/// 领取宝箱奖励，数据刷新
	/// </summary>
	/// <param name="msg"></param>
	public void MafaBoxRewardMessage(mafa.MafaBoxReward msg)
	{
		maFaRewards = msg;
		activeData.boxCount++;
	}
	/// <summary>
	/// 获得奖励面板（二级面板），处理用数据
	/// </summary>
	/// <returns></returns>
	public mafa.MafaBoxReward GetMafaBoxRewards()
	{
		return maFaRewards;
	}
	/// <summary>
	/// 获得当前战令等级的经验上限
	/// </summary>
	/// <returns></returns>
	public int GetMaxExp()
	{
		int maxExp = 0;
		var arr = MafaActivityRewardTableManager.Instance.array.gItem.handles;
		int rewardMin = 0;
		int rewardMax = 0;
		SetRewardsMinAndMax(ref rewardMin,ref rewardMax);
		for (int k = rewardMin, max = rewardMax; k < max; ++k)
		{
			if (arr[k] == null) continue;
			var item = arr[k].Value as TABLE.MAFAACTIVITYREWARD;
			if (item.descid == activeData.id &&
				item.layer == activeData.layer + 1)
			{
				maxExp = item.cost[0];
				break;
			}
		}
		return maxExp;
	}
	public override void Dispose()
    {
        mPoolHandle?.OnDestroy();
        itemDataList?.Clear();
		normalDic = null;
		advancedDic = null;
		mPoolHandle = null;
        itemDataList = null;
        activeData = null;
        maFaRewards = null;
        minLayer = 0;
        boxNeedCount = 0;
        keyId = 0;
        keyNeed = 0;
		layerStr = "";
	}
}
public class MaFaActiveData: IDispose
{
	public MaFaActiveData() { }
	public MaFaActiveData(int _id,int _exp,int _superUnlock,int _boxCount,int _layer,int _seasonEndDay,
		int _boxLevel)
	{
		id = _id;
		exp = _exp;
		superUnlock = _superUnlock;
		boxCount = _boxCount;
		layer = _layer;
		seasonEndDay = _seasonEndDay;
		boxLevel = _boxLevel;
	}
	public int id = 0;
	public int exp = 0;
	public int superUnlock = 0;
	public int boxCount = 0;
	public int layer = 0;
	public int seasonEndDay = 0;
	public int boxLevel = 0;
	public void Dispose()
	{
		id = 0;
		exp = 0;
		superUnlock = 0;
		boxCount = 0;
		layer = 0;
		seasonEndDay = 0;
		boxLevel = 0;
	}
}
public class MaFaData : IDispose
{
	public MaFaData() { }
	public MaFaData(int _id1, int _num1, int _state1, int _id2, int _num2, int _state2, int _layer,string _layerStr)
	{
		id1 = _id1;
		num1 = _num1;
		state1 = _state1;
		id2 = _id2;
		num2 = _num2;
		state2 = _state2;
		layer = _layer;
		layerStr = _layerStr;
	}
	public int id1 = 0;
	public int num1 = 0;
	public int state1 = 0;
	public int id2 = 0;
	public int num2 = 0;
	public int state2 = 0;
	public int layer = 0;
	public string layerStr = "";

	public void Dispose()
	{
		id1 = 0;
		num1 = 0;
		state1 = 0;
		id2 = 0;
		num2 = 0;
		state2 = 0;
		layer = 0;
		layerStr = "";
	}
}