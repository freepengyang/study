//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_zhanchongxiliancost.proto
using System.Collections.Generic;
public partial class ZhanChongXiLianCostTableManager : TableManager<TABLE.ZHANCHONGXILIANCOSTARRAY, TABLE.ZHANCHONGXILIANCOST,int,ZhanChongXiLianCostTableManager>
{
	public override bool TryGetValue(int key, out TABLE.ZHANCHONGXILIANCOST tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.ZHANCHONGXILIANCOST;
		}
		return null != tbl;
	}
	public int GetZhanChongXiLianCostId(int id,int defaultValue = default(int))
	{
		TABLE.ZHANCHONGXILIANCOST cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetZhanChongXiLianCostLevClass(int id,int defaultValue = default(int))
	{
		TABLE.ZHANCHONGXILIANCOST cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.levClass;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetZhanChongXiLianCostHunlicost(int id,string defaultValue = default(string))
	{
		TABLE.ZHANCHONGXILIANCOST cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.hunlicost;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetZhanChongXiLianCostHunjicost(int id,string defaultValue = default(string))
	{
		TABLE.ZHANCHONGXILIANCOST cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.hunjicost;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetZhanChongXiLianCostHunzhicost(int id,string defaultValue = default(string))
	{
		TABLE.ZHANCHONGXILIANCOST cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.hunzhicost;
		}
		else
		{
			return defaultValue;
		}
	}
}