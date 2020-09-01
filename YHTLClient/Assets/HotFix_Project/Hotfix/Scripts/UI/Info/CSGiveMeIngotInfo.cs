using System.Collections.Generic;
using Google.Protobuf.Collections;
public class CSGiveMeIngotInfo : CSInfo<CSGiveMeIngotInfo>
{
	PoolHandleManager mPoolHandle = new PoolHandleManager();
	Dictionary<int, GiveMeIngotData> giveIngotDic;
	Dictionary<int, int> netGiveIngotDic;
	ILBetterList<GetWayData> costWayList;
	ILBetterList<GiveMeIngotData> giveIngotList;
	int yieldId = 10000;//特殊处理，野外公共地图
	int ingotId = 3;
	int allIngot = 0;
	int allLimitIngot = 0;
	/// <summary>
	/// 设置元宝信息
	/// </summary>
	/// <param name="msg"></param>
	public void SetIngotInfo(yuanbao.YuanBaoInfo msg)
	{
		if (netGiveIngotDic == null) netGiveIngotDic = new Dictionary<int, int>();
		int netKey, netValue;
		for(int i=0;i< msg.instances.Count;i++)
		{
			netKey = msg.instances[i].instanceType;
			netValue = msg.instances[i].count;
			if (!netGiveIngotDic.ContainsKey(netKey))
				netGiveIngotDic.Add(netKey, netValue);
			else
				netGiveIngotDic[netKey] = netValue;
		}
		//野外boss特殊处理
		if(!netGiveIngotDic.ContainsKey(yieldId))
			netGiveIngotDic.Add(yieldId,msg.yewaiGet);
		else
			netGiveIngotDic[yieldId] = msg.yewaiGet;
		//第一次创建字典
		TABLE.NAYUANBAO ingot = null;
		if (giveIngotDic == null)
		{
			giveIngotDic = new Dictionary<int, GiveMeIngotData>();
			TableHandle[] giveIngotArr = NaYuanBaoTableManager.Instance.array.gItem.handles;
			for (int k = 0, max = giveIngotArr.Length; k < max; ++k)
			{
				ingot = giveIngotArr[k].Value as TABLE.NAYUANBAO;
				if (!giveIngotDic.ContainsKey(ingot.id))
				{
					GiveMeIngotData data = mPoolHandle.GetCustomClass<GiveMeIngotData>();
					data.id = ingot.id;
					data.type = ingot.type;
					data.param = ingot.param;
					data.maxValue = ingot.value;
					data.gameModelId = ingot.gamemodel;
					data.deliverId = ingot.deliver;
					data.order = ingot.order;
					data.name = ingot.name;
					data.desc = ingot.desc;
					if (netGiveIngotDic.ContainsKey(ingot.id))
						data.value = netGiveIngotDic[ingot.id];
					else
						data.value = 0;
					giveIngotDic.Add(ingot.id, data);
				}
			}
		}
		//已经存储字典后，对应key赋值
		else
		{
			var dic = netGiveIngotDic.GetEnumerator();
			while (dic.MoveNext())
			{
				if(giveIngotDic.ContainsKey(dic.Current.Key))
					giveIngotDic[dic.Current.Key].value = dic.Current.Value;
				
			}
		}
		SetAllIngotInt();
		SetCostWayList();
		SetDicLimit();
	}
	/// <summary>
	/// 设置字典条件相关
	/// </summary>
	public void SetDicLimit()
	{
		int playLv = CSMainPlayerInfo.Instance.Level;//玩家等级
		long serverOpenDay = CSMainPlayerInfo.Instance.ServerOpenDay;//服务器开服天数
		int activeLv = CSPetTalentInfo.Instance.CurActivatedPoint;  //战魂天赋等级
		int funcOpenId,needLv,openDay;
		var dic = giveIngotDic.GetEnumerator();
		GiveMeIngotData data;
		while (dic.MoveNext())
		{
			data = dic.Current.Value;
			if(data.type == 1)
			{
				funcOpenId = data.param;
				needLv = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(funcOpenId);
				openDay = FuncOpenTableManager.Instance.GetFuncOpenOpenday(funcOpenId);
				data.isShowBtn = playLv >= needLv && serverOpenDay >= openDay;
				if (playLv < needLv)
					data.limitDesc = CSString.Format(1953, needLv);
				else if (serverOpenDay < openDay)
					data.limitDesc = CSString.Format(1954, openDay);
			}
			else if(data.type == 2)
			{
				data.isShowBtn = activeLv >= data.param ;
				if (activeLv < data.param)
					data.limitDesc = CSString.Format(1955, data.param);
			}
		}
		SortGiveMeIngotList();
	}
	/// <summary>
	/// 字典排序
	/// 已解锁的>未解锁的，
	/// 已解锁的里面，进度条未满的>进度条已满
	/// 进度条未满的，按照表格里的orderID排序
	/// </summary>
	private void SortGiveMeIngotList()
	{
		if (giveIngotList == null)
			giveIngotList = mPoolHandle.GetSystemClass<ILBetterList<GiveMeIngotData>>();
		else
			giveIngotList.Clear();
		var dic = giveIngotDic.GetEnumerator();
		while(dic.MoveNext())
		{
			giveIngotList.Add(dic.Current.Value);
		}
		giveIngotList.Sort(CmpGiveIngotList);
	}
	private int CmpGiveIngotList(GiveMeIngotData l, GiveMeIngotData r)
	{
		if (l.isShowBtn == r.isShowBtn && l.isShowBtn)
			return l.order - r.order;
		else if (l.isShowBtn != r.isShowBtn)
		{
			if (l.isShowBtn)
				return -1;
			else
				return 1;
		}
		else
			return l.order - r.order;
	}
	/// <summary>
	/// 获取列表数据
	/// </summary>
	/// <returns></returns>
	public ILBetterList<GiveMeIngotData> GetGiveIngotList()
	{
		return giveIngotList;
	}
	/// <summary>
	/// 设置元宝总和
	/// </summary>
	private void SetAllIngotInt()
	{
		var dic = giveIngotDic.GetEnumerator();
		allIngot = 0;
		allLimitIngot = 0;
		while (dic.MoveNext())
		{
			allIngot += dic.Current.Value.value;
			allLimitIngot += dic.Current.Value.maxValue;
		}
	}
	/// <summary>
	/// 所得元宝总和
	/// </summary>
	/// <returns></returns>
	public int GetAllIngot()
	{
		return allIngot;
	}
	/// <summary>
	/// 元宝上限总和
	/// </summary>
	/// <returns></returns>
	public int GetAllLimitIngot()
	{
		return allLimitIngot;
	}
	/// <summary>
	/// 元宝id
	/// </summary>
	/// <returns></returns>
	public int GetIngotId()
	{
		return ingotId;
	}
	/// <summary>
	/// 设置消耗途径
	/// </summary>
	private void SetCostWayList()
	{
		if(costWayList == null)
		{
			string costWayStr = SundryTableManager.Instance.GetSundryEffect(1061);
			List<int> wayList = UtilityMainMath.SplitStringToIntList(costWayStr);
			CSGetWayInfo.Instance.GetGetWays(wayList, ref costWayList);
		}
	}
	/// <summary>
	/// 消耗途径
	/// </summary>
	/// <returns></returns>
	public ILBetterList<GetWayData> GetCostWayList()
	{
		return costWayList;
	}
	public void ShowIngot()
	{
		mClientEvent.SendEvent(CEvent.FirstShowIngotHead);
	}
	public override void Dispose()
	{
		mPoolHandle?.OnDestroy();
		ingotId = 0;
		yieldId = 0;
		allIngot = 0;
		allLimitIngot = 0;

		giveIngotDic = null;
		netGiveIngotDic = null;
		mPoolHandle = null;
		costWayList = null;
		giveIngotList = null; 
	}
}
public class GiveMeIngotData:IDispose
{
	public int id { get; set;  }
	public int type { get; set; }
	public int param { get; set; }
	public int value { get; set; }
	public int maxValue { get; set; }
	public int gameModelId { get; set; }
	public int deliverId { get; set; }
	public int order { get; set; }
	public string name { get; set; }
	public string desc { get; set; }
	public string limitDesc { get; set; }
	public bool isShowBtn { get; set; }
	public void Dispose()
	{
		id = 0;
		type = 0;
		param = 0;
		value = 0;
		maxValue = 0;
		gameModelId = 0;
		deliverId = 0;
		order = 0;
		limitDesc = "";
		name = "";
		desc = "";
		isShowBtn = false;
	}
}