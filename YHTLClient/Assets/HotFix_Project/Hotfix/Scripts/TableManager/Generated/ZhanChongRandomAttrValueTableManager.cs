//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_zhanchongrandomattrvalue.proto
using System.Collections.Generic;
public partial class ZhanChongRandomAttrValueTableManager : TableManager<TABLE.ZHANCHONGRANDOMATTRVALUEARRAY, TABLE.ZHANCHONGRANDOMATTRVALUE,int,ZhanChongRandomAttrValueTableManager>
{
	public override bool TryGetValue(int key, out TABLE.ZHANCHONGRANDOMATTRVALUE tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.ZHANCHONGRANDOMATTRVALUE;
		}
		return null != tbl;
	}
	public int GetZhanChongRandomAttrValueId(int id,int defaultValue = default(int))
	{
		TABLE.ZHANCHONGRANDOMATTRVALUE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetZhanChongRandomAttrValueLevClass(int id,int defaultValue = default(int))
	{
		TABLE.ZHANCHONGRANDOMATTRVALUE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.levClass;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetZhanChongRandomAttrValueType(int id,int defaultValue = default(int))
	{
		TABLE.ZHANCHONGRANDOMATTRVALUE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.type;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetZhanChongRandomAttrValueParameter(int id,int defaultValue = default(int))
	{
		TABLE.ZHANCHONGRANDOMATTRVALUE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.parameter;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetZhanChongRandomAttrValueAttrValue(int id,int defaultValue = default(int))
	{
		TABLE.ZHANCHONGRANDOMATTRVALUE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.attrValue;
		}
		else
		{
			return defaultValue;
		}
	}
}