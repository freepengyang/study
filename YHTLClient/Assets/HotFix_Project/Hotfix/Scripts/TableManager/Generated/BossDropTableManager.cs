//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_bossdrop.proto
using System.Collections.Generic;
public partial class BossDropTableManager : TableManager<TABLE.BOSSDROPARRAY, TABLE.BOSSDROP,int,BossDropTableManager>
{
	public override bool TryGetValue(int key, out TABLE.BOSSDROP tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.BOSSDROP;
		}
		return null != tbl;
	}
	public int GetBossDropId(int id,int defaultValue = default(int))
	{
		TABLE.BOSSDROP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBossDropMid(int id,int defaultValue = default(int))
	{
		TABLE.BOSSDROP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.mid;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetBossDropName(int id,string defaultValue = default(string))
	{
		TABLE.BOSSDROP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.name;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBossDropDrop(int id,int defaultValue = default(int))
	{
		TABLE.BOSSDROP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.drop;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBossDropComDrop(int id,int defaultValue = default(int))
	{
		TABLE.BOSSDROP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.comDrop;
		}
		else
		{
			return defaultValue;
		}
	}
	public IntArray GetBossDropExtraDrop(int id,IntArray defaultValue = default(IntArray))
	{
		TABLE.BOSSDROP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.extraDrop;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBossDropFirstKillDrop(int id,int defaultValue = default(int))
	{
		TABLE.BOSSDROP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.firstKillDrop;
		}
		else
		{
			return defaultValue;
		}
	}
}