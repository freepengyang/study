//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_worldclass.proto
using System.Collections.Generic;
public partial class WorldClassTableManager : TableManager<TABLE.WORLDCLASSARRAY, TABLE.WORLDCLASS,int,WorldClassTableManager>
{
	public override bool TryGetValue(int key, out TABLE.WORLDCLASS tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.WORLDCLASS;
		}
		return null != tbl;
	}
	public int GetWorldClassId(int id,int defaultValue = default(int))
	{
		TABLE.WORLDCLASS cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetWorldClassGuoQiDaHP(int id,int defaultValue = default(int))
	{
		TABLE.WORLDCLASS cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.guoQiDaHP;
		}
		else
		{
			return defaultValue;
		}
	}
}