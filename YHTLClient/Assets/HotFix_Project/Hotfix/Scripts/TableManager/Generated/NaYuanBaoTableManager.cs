//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_nayuanbao.proto
using System.Collections.Generic;
public partial class NaYuanBaoTableManager : TableManager<TABLE.NAYUANBAOARRAY, TABLE.NAYUANBAO,int,NaYuanBaoTableManager>
{
	public override bool TryGetValue(int key, out TABLE.NAYUANBAO tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.NAYUANBAO;
		}
		return null != tbl;
	}
	public int GetNaYuanBaoId(int id,int defaultValue = default(int))
	{
		TABLE.NAYUANBAO cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetNaYuanBaoType(int id,int defaultValue = default(int))
	{
		TABLE.NAYUANBAO cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.type;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetNaYuanBaoParam(int id,int defaultValue = default(int))
	{
		TABLE.NAYUANBAO cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.param;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetNaYuanBaoName(int id,string defaultValue = default(string))
	{
		TABLE.NAYUANBAO cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.name;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetNaYuanBaoDesc(int id,string defaultValue = default(string))
	{
		TABLE.NAYUANBAO cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.desc;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetNaYuanBaoValue(int id,int defaultValue = default(int))
	{
		TABLE.NAYUANBAO cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.value;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetNaYuanBaoGamemodel(int id,int defaultValue = default(int))
	{
		TABLE.NAYUANBAO cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.gamemodel;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetNaYuanBaoDeliver(int id,int defaultValue = default(int))
	{
		TABLE.NAYUANBAO cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.deliver;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetNaYuanBaoOrder(int id,int defaultValue = default(int))
	{
		TABLE.NAYUANBAO cfg = null;
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