//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_biqishop.proto
using System.Collections.Generic;
public partial class BiQiShopTableManager : TableManager<TABLE.BIQISHOPARRAY, TABLE.BIQISHOP,int,BiQiShopTableManager>
{
	public override bool TryGetValue(int key, out TABLE.BIQISHOP tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.BIQISHOP;
		}
		return null != tbl;
	}
	public int GetBiQiShopId(int id,int defaultValue = default(int))
	{
		TABLE.BIQISHOP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBiQiShopType(int id,int defaultValue = default(int))
	{
		TABLE.BIQISHOP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.type;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBiQiShopSubType(int id,int defaultValue = default(int))
	{
		TABLE.BIQISHOP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.subType;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBiQiShopBoxId(int id,int defaultValue = default(int))
	{
		TABLE.BIQISHOP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.boxId;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetBiQiShopName(int id,string defaultValue = default(string))
	{
		TABLE.BIQISHOP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.name;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBiQiShopValue(int id,int defaultValue = default(int))
	{
		TABLE.BIQISHOP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.value;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBiQiShopValue2(int id,int defaultValue = default(int))
	{
		TABLE.BIQISHOP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.value2;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBiQiShopFrequency(int id,int defaultValue = default(int))
	{
		TABLE.BIQISHOP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.frequency;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBiQiShopShowMoney(int id,int defaultValue = default(int))
	{
		TABLE.BIQISHOP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.showMoney;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetBiQiShopRecommend(int id,string defaultValue = default(string))
	{
		TABLE.BIQISHOP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.Recommend;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBiQiShopOrder(int id,int defaultValue = default(int))
	{
		TABLE.BIQISHOP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.order;
		}
		else
		{
			return defaultValue;
		}
	}
}