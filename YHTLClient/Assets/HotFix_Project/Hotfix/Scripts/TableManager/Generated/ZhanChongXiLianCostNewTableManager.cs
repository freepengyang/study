//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_zhanchongxiliancostnew.proto
using System.Collections.Generic;
public partial class ZhanChongXiLianCostNewTableManager : TableManager<TABLE.ZHANCHONGXILIANCOSTNEWARRAY, TABLE.ZHANCHONGXILIANCOSTNEW,int,ZhanChongXiLianCostNewTableManager>
{
	public override bool TryGetValue(int key, out TABLE.ZHANCHONGXILIANCOSTNEW tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.ZHANCHONGXILIANCOSTNEW;
		}
		return null != tbl;
	}
	public int GetZhanChongXiLianCostNewId(int id,int defaultValue = default(int))
	{
		TABLE.ZHANCHONGXILIANCOSTNEW cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetZhanChongXiLianCostNewLevClass(int id,int defaultValue = default(int))
	{
		TABLE.ZHANCHONGXILIANCOSTNEW cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.levClass;
		}
		else
		{
			return defaultValue;
		}
	}
	public IntArray GetZhanChongXiLianCostNewHunlicost(int id,IntArray defaultValue = default(IntArray))
	{
		TABLE.ZHANCHONGXILIANCOSTNEW cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.hunlicost;
		}
		else
		{
			return defaultValue;
		}
	}
	public IntArray GetZhanChongXiLianCostNewHunlicost1(int id,IntArray defaultValue = default(IntArray))
	{
		TABLE.ZHANCHONGXILIANCOSTNEW cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.hunlicost1;
		}
		else
		{
			return defaultValue;
		}
	}
	public IntArray GetZhanChongXiLianCostNewHunjicost(int id,IntArray defaultValue = default(IntArray))
	{
		TABLE.ZHANCHONGXILIANCOSTNEW cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.hunjicost;
		}
		else
		{
			return defaultValue;
		}
	}
	public IntArray GetZhanChongXiLianCostNewHunjicost1(int id,IntArray defaultValue = default(IntArray))
	{
		TABLE.ZHANCHONGXILIANCOSTNEW cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.hunjicost1;
		}
		else
		{
			return defaultValue;
		}
	}
}