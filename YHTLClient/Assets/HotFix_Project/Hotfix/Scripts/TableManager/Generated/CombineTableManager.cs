//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_combine.proto
using System.Collections.Generic;
public partial class CombineTableManager : TableManager<TABLE.COMBINEARRAY, TABLE.COMBINE,int,CombineTableManager>
{
	public override bool TryGetValue(int key, out TABLE.COMBINE tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.COMBINE;
		}
		return null != tbl;
	}
	public int GetCombineId(int id,int defaultValue = default(int))
	{
		TABLE.COMBINE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetCombineGroupID(int id,int defaultValue = default(int))
	{
		TABLE.COMBINE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.groupID;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetCombineGroupName(int id,string defaultValue = default(string))
	{
		TABLE.COMBINE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.groupName;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetCombineSubType(int id,int defaultValue = default(int))
	{
		TABLE.COMBINE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.SubType;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetCombineGenerateItem(int id,int defaultValue = default(int))
	{
		TABLE.COMBINE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.generateItem;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetCombineSubTypeName(int id,string defaultValue = default(string))
	{
		TABLE.COMBINE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.SubTypeName;
		}
		else
		{
			return defaultValue;
		}
	}
	public IntArray GetCombineNeedItem(int id,IntArray defaultValue = default(IntArray))
	{
		TABLE.COMBINE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.needItem;
		}
		else
		{
			return defaultValue;
		}
	}
	public IntArray GetCombineNeedResource(int id,IntArray defaultValue = default(IntArray))
	{
		TABLE.COMBINE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.needResource;
		}
		else
		{
			return defaultValue;
		}
	}
	public IntArray GetCombineOpenTime(int id,IntArray defaultValue = default(IntArray))
	{
		TABLE.COMBINE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.openTime;
		}
		else
		{
			return defaultValue;
		}
	}
	public IntArray GetCombineCombineTime(int id,IntArray defaultValue = default(IntArray))
	{
		TABLE.COMBINE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.combineTime;
		}
		else
		{
			return defaultValue;
		}
	}
}