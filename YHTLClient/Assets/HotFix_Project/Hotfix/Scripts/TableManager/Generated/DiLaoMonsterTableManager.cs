//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_dilaomonster.proto
using System.Collections.Generic;
public partial class DiLaoMonsterTableManager : TableManager<TABLE.DILAOMONSTERARRAY, TABLE.DILAOMONSTER,int,DiLaoMonsterTableManager>
{
	public override bool TryGetValue(int key, out TABLE.DILAOMONSTER tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.DILAOMONSTER;
		}
		return null != tbl;
	}
	public int GetDiLaoMonsterId(int id,int defaultValue = default(int))
	{
		TABLE.DILAOMONSTER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetDiLaoMonsterWaves(int id,string defaultValue = default(string))
	{
		TABLE.DILAOMONSTER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.waves;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetDiLaoMonsterMid(int id,string defaultValue = default(string))
	{
		TABLE.DILAOMONSTER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.mid;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetDiLaoMonsterInterval(int id,string defaultValue = default(string))
	{
		TABLE.DILAOMONSTER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.interval;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetDiLaoMonsterMonsterBuff(int id,string defaultValue = default(string))
	{
		TABLE.DILAOMONSTER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.monsterBuff;
		}
		else
		{
			return defaultValue;
		}
	}
}