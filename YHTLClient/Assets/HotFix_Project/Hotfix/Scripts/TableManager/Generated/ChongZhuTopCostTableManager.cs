//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_chongzhutopcost.proto
using System.Collections.Generic;
public partial class ChongZhuTopCostTableManager : TableManager<TABLE.CHONGZHUTOPCOSTARRAY, TABLE.CHONGZHUTOPCOST,int,ChongZhuTopCostTableManager>
{
	public override bool TryGetValue(int key, out TABLE.CHONGZHUTOPCOST tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.CHONGZHUTOPCOST;
		}
		return null != tbl;
	}
	public int GetChongZhuTopCostId(int id,int defaultValue = default(int))
	{
		TABLE.CHONGZHUTOPCOST cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetChongZhuTopCostLevclass(int id,int defaultValue = default(int))
	{
		TABLE.CHONGZHUTOPCOST cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.levclass;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetChongZhuTopCostQuality(int id,int defaultValue = default(int))
	{
		TABLE.CHONGZHUTOPCOST cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.quality;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetChongZhuTopCostPayType(int id,int defaultValue = default(int))
	{
		TABLE.CHONGZHUTOPCOST cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.payType;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetChongZhuTopCostPrice(int id,int defaultValue = default(int))
	{
		TABLE.CHONGZHUTOPCOST cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.price;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetChongZhuTopCostCostItemID(int id,int defaultValue = default(int))
	{
		TABLE.CHONGZHUTOPCOST cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.costItemID;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetChongZhuTopCostNum(int id,int defaultValue = default(int))
	{
		TABLE.CHONGZHUTOPCOST cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.num;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetChongZhuTopCostCostItemID2(int id,int defaultValue = default(int))
	{
		TABLE.CHONGZHUTOPCOST cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.costItemID2;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetChongZhuTopCostNum2(int id,int defaultValue = default(int))
	{
		TABLE.CHONGZHUTOPCOST cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.num2;
		}
		else
		{
			return defaultValue;
		}
	}
}