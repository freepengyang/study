using System.Collections.Generic;
using Google.Protobuf.Collections;
using lianti;

public class CSLianTiInfo : CSInfo<CSLianTiInfo>
{
	private int lianTiID = 0;
	private string cost = "";
	private long owned = 0;
	private int t_ind = 0;
	private bool isTurn = true;//关闭返回炼体boss界面
	List<long> listCost = new List<long>();
	CSBetterLisHot<LianTiLandData> lianTiLandList = new CSBetterLisHot<LianTiLandData>();
	PoolHandleManager mPoolHandle = new PoolHandleManager();
	public int LianTiID
	{
		get { return lianTiID; }
	}

	public bool HasNotify
	{
		get { return CanUpgrade(); }
	}

	public int GetLianTiLandTind()
	{
		return t_ind;
	}
	public void SetLianTiLandTind(int _t_ind)
	{
		t_ind = _t_ind;
	}
	/// <summary>
	/// 能否升级
	/// </summary>
	private bool CanUpgrade()
	{
		TABLE.LIANTI item = null;
		if (!LianTiTableManager.Instance.TryGetValue(lianTiID, out item))
		{
			return false;
		}

		if (NextID() == 0)
		{
			return false;
		}
		if (!IsNotLvReach())
		{
			return false;
		}
		if (!IsEnoughItem())
		{
			return false;
		}
		return true;
	}
	public bool GetIsTurn()
	{
		return isTurn;
	}
	public void SetIsTurn(bool _isTurn)
	{
		isTurn = _isTurn;
	}
	public bool IsNotLvReach()
	{
		TABLE.LIANTI item = null;
		if (!LianTiTableManager.Instance.TryGetValue(lianTiID, out item))
		{
			return false;
		}
		if (item.level > CSMainPlayerInfo.Instance.Level)
		{
			return false;
		}
		return true;
	}
	public bool IsEnoughItem()
	{
		cost = LianTiTableManager.Instance.GetLianTiCost(lianTiID);
		listCost = UtilityMainMath.SplitStringToLongList(cost);
		int itemId = (int)listCost[0];
		owned = itemId.GetItemCount();
		if (owned < listCost[1])
		{
			return false;
		}
		return true;
	}
	public int NextID()
	{
		TABLE.LIANTI item = null;
		if (LianTiTableManager.Instance.TryGetValue(lianTiID + 1, out item))
		{
			return lianTiID + 1;
		}
		return 0;
	}
	public string GetLianTiName()
	{
		return LianTiTableManager.Instance.GetLianTiName(LianTiID);
	}
	//获取炼体等级
	public int GetLianTiLv()
	{
		int lianTiLv = 1;
		if (lianTiID > 0)
		{
			var show = LianTiTableManager.Instance.GetLianTiShow(lianTiID);
			lianTiLv = show[0];
		}
		return lianTiLv;
	}
	//获取炼体阶级
	public int GetLianTiClass()
	{
		int lianTiClass = 0;
		var show = LianTiTableManager.Instance.GetLianTiShow(lianTiID);
		lianTiClass = show[1];
		return lianTiClass;
	}
	/// <summary>
	/// 处理炼体数据
	/// </summary>
	/// <param name="msg"></param>
	public void GetLianTiInfoMessage(LianTiInfoResponse msg)
	{
		if (msg == null) return;
		lianTiID = msg.lianTiInfo.lianTiId;
	}
	/// <summary>
	/// 处理炼体之地数据
	/// </summary>
	/// <param name="msg"></param>
	public void GetLianTiLandInfoMessage(LianTiFieldResponse msg)
	{
		int openLevel, level;
		string lianTiName, tempName;
		if (lianTiLandList == null)
			lianTiLandList = mPoolHandle.GetSystemClass<CSBetterLisHot<LianTiLandData>>();
		else
			lianTiLandList.Clear();

		tempName = ClientTipsTableManager.Instance.GetClientTipsContext(878);
		for (int i = 0; i < msg.lianTiFields.Count; i++)
		{
			LianTiLandData tempData = mPoolHandle.GetCustomClass<LianTiLandData>();
			tempData.mapId = msg.lianTiFields[i].instanceId;
			tempData.bossNum = msg.lianTiFields[i].bossNum;
			tempData.surBossNum = msg.lianTiFields[i].surviveBossNum;
			tempData.isFirstKill = msg.lianTiFields[i].hasFirstKillReward;
			openLevel = InstanceTableManager.Instance.GetInstanceOpenLianTiLevel(tempData.mapId);
			lianTiName = LianTiTableManager.Instance.GetLianTiName(openLevel);
			tempData.tip = CSString.Format(1303, lianTiName);
			level = InstanceTableManager.Instance.GetInstanceLevel(tempData.mapId);
			tempData.mapName = CSString.Format(tempName, level);
			lianTiLandList.Add(tempData);
		}
	}
	public CSBetterLisHot<LianTiLandData> GetLianTiLandList()
	{
		return lianTiLandList;
	}
	public override void Dispose()
	{
		t_ind = 0;
		lianTiID = 0;
		cost = "";
		owned = 0;
		lianTiLandList.Clear();
		mPoolHandle?.OnDestroy();

		isTurn = true;
		listCost = null;

		lianTiLandList = null;
		mPoolHandle = null;
	}
}

public class LianTiLandData : IDispose
{
	public LianTiLandData() { }
	public LianTiLandData(int id, int num1, int num2, bool firstKill, string _tip, string _mapName)
	{
		mapId = id;                 //副本id
		surBossNum = num1;          //boss存活数
		bossNum = num2;             //boss数
		isFirstKill = firstKill;    //是否首杀
		tip = _tip;                 //条件不足的提示
		mapName = _mapName;         //地图名
	}
	public int mapId = 0;
	public int surBossNum = 0;
	public int bossNum = 0;
	public bool isFirstKill = false;
	public string tip = "";
	public string mapName = "";
	public void Dispose()
	{
		mapId = 0;
		surBossNum = 0;
		bossNum = 0;
		isFirstKill = false;
		tip = "";
		mapName = "";
	}
}