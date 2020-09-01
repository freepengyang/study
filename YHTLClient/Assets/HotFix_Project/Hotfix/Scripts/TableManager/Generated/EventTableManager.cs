//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_event.proto
using System.Collections.Generic;
public partial class EventTableManager : TableManager<TABLE.EVENTARRAY, TABLE.EVENT,int,EventTableManager>
{
	public override bool TryGetValue(int key, out TABLE.EVENT tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.EVENT;
		}
		return null != tbl;
	}
	public int GetEventId(int id,int defaultValue = default(int))
	{
		TABLE.EVENT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetEventMapId(int id,int defaultValue = default(int))
	{
		TABLE.EVENT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.mapId;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetEventType(int id,int defaultValue = default(int))
	{
		TABLE.EVENT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.type;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetEventX(int id,int defaultValue = default(int))
	{
		TABLE.EVENT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.x;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetEventY(int id,int defaultValue = default(int))
	{
		TABLE.EVENT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.y;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetEventParam(int id,string defaultValue = default(string))
	{
		TABLE.EVENT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.param;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetEventOpenCombineTime(int id,uint defaultValue = default(uint))
	{
		TABLE.EVENT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.openCombineTime;
		}
		else
		{
			return defaultValue;
		}
	}
}