//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_summon.proto
using System.Collections.Generic;
public partial class SummonTableManager : TableManager<TABLE.SUMMONARRAY, TABLE.SUMMON,int,SummonTableManager>
{
	public override bool TryGetValue(int key, out TABLE.SUMMON tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.SUMMON;
		}
		return null != tbl;
	}
	public int GetSummonId(int id,int defaultValue = default(int))
	{
		TABLE.SUMMON cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
}