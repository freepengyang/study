//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_getway.proto
using System.Collections.Generic;
public partial class GetWayTableManager : TableManager<TABLE.GETWAYARRAY, TABLE.GETWAY,int,GetWayTableManager>
{
	public override bool TryGetValue(int key, out TABLE.GETWAY tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.GETWAY;
		}
		return null != tbl;
	}
	public int GetGetWayId(int id,int defaultValue = default(int))
	{
		TABLE.GETWAY cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetGetWayName(int id,string defaultValue = default(string))
	{
		TABLE.GETWAY cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.name;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGetWayType(int id,int defaultValue = default(int))
	{
		TABLE.GETWAY cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.type;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGetWayFunction(int id,int defaultValue = default(int))
	{
		TABLE.GETWAY cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.function;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGetWayRecommend(int id,int defaultValue = default(int))
	{
		TABLE.GETWAY cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.recommend;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetGetWayTips(int id,string defaultValue = default(string))
	{
		TABLE.GETWAY cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.Tips;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGetWayShowLevel(int id,int defaultValue = default(int))
	{
		TABLE.GETWAY cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.showLevel;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGetWayServer(int id,int defaultValue = default(int))
	{
		TABLE.GETWAY cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.server;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGetWayUpTime(int id,int defaultValue = default(int))
	{
		TABLE.GETWAY cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.upTime;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGetWayDownTime(int id,int defaultValue = default(int))
	{
		TABLE.GETWAY cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.downTime;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGetWayOrder(int id,int defaultValue = default(int))
	{
		TABLE.GETWAY cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.order;
		}
		else
		{
			return defaultValue;
		}
	}
}