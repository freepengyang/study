using activity;

public class CSBossKuangHuanInfo : CSInfo<CSBossKuangHuanInfo>
{
	int productListIdx = 1;
	int costId = 0;
	int activityId = 10120;
	CSBetterLisHot<BossKuangHuanData> productDataList = new CSBetterLisHot<BossKuangHuanData>();
	CSBetterLisHot<BossKuangHuanData> productOnList = new CSBetterLisHot<BossKuangHuanData>();
	CSBetterLisHot<BossKuangHuanData> productOffList = new CSBetterLisHot<BossKuangHuanData>();
	PoolHandleManager mPoolHandle = new PoolHandleManager();
	public void SetBossKuangHuanMessage(ResBossKuangHuan msg)
	{
		productDataList.Clear();
		productOnList.Clear();
		productOffList.Clear();
		for (int i = 0; i < msg.bossKuangHuanData.Count; i++)
		{
			BossKuangHuanData productData = mPoolHandle.GetCustomClass<BossKuangHuanData>();
			productData.tableId = msg.bossKuangHuanData[i].goalId;
			productData.stockNum = msg.bossKuangHuanData[i].stockNum;
			if (productData.stockNum <= 0)
			{
				productOffList.Add(productData);
			}
			else
			{
				productOnList.Add(productData);
			}
		}
		for(int i=0; i < productOnList.Count; i++)
		{
			productDataList.Add(productOnList[i]);
		}
		for (int i = 0; i < productOffList.Count; i++)
		{
			productDataList.Add(productOffList[i]);
		}

		var itemExcel = ItemTableManager.Instance.array.gItem.handles;
		TABLE.ITEM item = null;
		for (int i = 0,max = itemExcel.Length;i < max;++i)
		{
			item = itemExcel[i].Value as TABLE.ITEM;
			if (item.type == 5 && item.subType == 67)
			{
				costId = item.id;
			}
		}
	}
	private int GetCostId()
	{		
		return costId;
	}
	public string GetCostIcon()
	{
		string costIcon = "";
		int costId = GetCostId();
		costIcon = $"tubiao{ItemTableManager.Instance.GetItemIcon(costId)}";
		return costIcon;
	}
	public CSBetterLisHot<BossKuangHuanData> GetBossKuangHuanList()
	{
		return productDataList;
	}
	public long GetBossKuangHuanIntegralTicketNum()
	{
		return costId.GetItemCount(); 
	}
	public void SetBossKuangHuanDataListIdx(int idx)
	{
		productListIdx = idx;
	}
	public BossKuangHuanData GetBossKuangHuanData()
	{
		return productDataList[productListIdx];
	}
	public bool IsShowRedPoint()
	{
		int tableId, stockNum,costNum;
		bool isRed = false;
		long itemNum = GetBossKuangHuanIntegralTicketNum();
		for(int i = 0;i< productDataList.Count;i++)
		{
			tableId = productDataList[i].tableId;
			stockNum = productDataList[i].stockNum;
			costNum = BossCarnivalRewardsTableManager.Instance.GetBossCarnivalRewardsNum(tableId);
			if (stockNum > 0 && itemNum >= costNum)
			{
				isRed = true;
				break;
			}
		}
		if(isRed)
		{
			long leftTime = UIServerActivityPanel.GetEndTime(activityId);
			if (leftTime <= 0)
				isRed = false;
		}

		return isRed;
	}
	public override void Dispose()
	{
		mPoolHandle?.OnDestroy();
		mPoolHandle = null;

		productDataList.Clear();
		productOnList.Clear();
		productOffList.Clear();
		
		productDataList = null;
		productOnList = null;
		productOffList = null;
		
		productListIdx = 0;
		costId = 0;
		activityId = 0;
	}
}
public class BossKuangHuanData : IDispose
{
	public BossKuangHuanData() { }
	public BossKuangHuanData(int _tableId,int _stockNum)
	{
		tableId = _tableId;
		stockNum = _stockNum;
	}
	public int tableId = 0;
	public int stockNum = 0;
	public void Dispose() { }
}