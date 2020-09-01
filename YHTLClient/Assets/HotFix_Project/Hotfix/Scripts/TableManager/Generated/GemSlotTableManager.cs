//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_gemslot.proto
using System.Collections.Generic;
public partial class GemSlotTableManager : TableManager<TABLE.GEMSLOTARRAY, TABLE.GEMSLOT,int,GemSlotTableManager>
{
	public override bool TryGetValue(int key, out TABLE.GEMSLOT tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.GEMSLOT;
		}
		return null != tbl;
	}
	public int GetGemSlotId(int id,int defaultValue = default(int))
	{
		TABLE.GEMSLOT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGemSlotKillBoss(int id,int defaultValue = default(int))
	{
		TABLE.GEMSLOT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.killBoss;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGemSlotNum(int id,int defaultValue = default(int))
	{
		TABLE.GEMSLOT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.num;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGemSlotCostnum(int id,int defaultValue = default(int))
	{
		TABLE.GEMSLOT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.costnum;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetGemSlotName(int id,string defaultValue = default(string))
	{
		TABLE.GEMSLOT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.name;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetGemSlotPic(int id,string defaultValue = default(string))
	{
		TABLE.GEMSLOT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.pic;
		}
		else
		{
			return defaultValue;
		}
	}
}