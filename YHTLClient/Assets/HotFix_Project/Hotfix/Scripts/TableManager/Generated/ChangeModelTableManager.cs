//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_changemodel.proto
using System.Collections.Generic;
public partial class ChangeModelTableManager : TableManager<TABLE.CHANGEMODELARRAY, TABLE.CHANGEMODEL,int,ChangeModelTableManager>
{
	public override bool TryGetValue(int key, out TABLE.CHANGEMODEL tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.CHANGEMODEL;
		}
		return null != tbl;
	}
	public int GetChangeModelId(int id,int defaultValue = default(int))
	{
		TABLE.CHANGEMODEL cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetChangeModelModel(int id,int defaultValue = default(int))
	{
		TABLE.CHANGEMODEL cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.model;
		}
		else
		{
			return defaultValue;
		}
	}
}