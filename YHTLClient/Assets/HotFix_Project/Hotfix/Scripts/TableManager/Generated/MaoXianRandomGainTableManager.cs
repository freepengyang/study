//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_maoxianrandomgain.proto
using System.Collections.Generic;
public partial class MaoXianRandomGainTableManager : TableManager<TABLE.MAOXIANRANDOMGAINARRAY, TABLE.MAOXIANRANDOMGAIN,int,MaoXianRandomGainTableManager>
{
	public override bool TryGetValue(int key, out TABLE.MAOXIANRANDOMGAIN tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.MAOXIANRANDOMGAIN;
		}
		return null != tbl;
	}
	public int GetMaoXianRandomGainId(int id,int defaultValue = default(int))
	{
		TABLE.MAOXIANRANDOMGAIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetMaoXianRandomGainType(int id,int defaultValue = default(int))
	{
		TABLE.MAOXIANRANDOMGAIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.type;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetMaoXianRandomGainParameter(int id,int defaultValue = default(int))
	{
		TABLE.MAOXIANRANDOMGAIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.parameter;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetMaoXianRandomGainIcon(int id,string defaultValue = default(string))
	{
		TABLE.MAOXIANRANDOMGAIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.Icon;
		}
		else
		{
			return defaultValue;
		}
	}
}