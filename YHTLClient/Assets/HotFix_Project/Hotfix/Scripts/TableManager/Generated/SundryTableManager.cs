//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_sundry.proto
using System.Collections.Generic;
public partial class SundryTableManager : TableManager<TABLE.SUNDRYARRAY, TABLE.SUNDRY,int,SundryTableManager>
{
	public override bool TryGetValue(int key, out TABLE.SUNDRY tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.SUNDRY;
		}
		return null != tbl;
	}
	public int GetSundryId(int id,int defaultValue = default(int))
	{
		TABLE.SUNDRY cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetSundryEffect(int id,string defaultValue = default(string))
	{
		TABLE.SUNDRY cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.effect;
		}
		else
		{
			return defaultValue;
		}
	}
}