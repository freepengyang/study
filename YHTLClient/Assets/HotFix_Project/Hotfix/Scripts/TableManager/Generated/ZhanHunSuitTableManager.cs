//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_zhanhunsuit.proto
using System.Collections.Generic;
public partial class ZhanHunSuitTableManager : TableManager<TABLE.ZHANHUNSUITARRAY, TABLE.ZHANHUNSUIT,int,ZhanHunSuitTableManager>
{
	public override bool TryGetValue(int key, out TABLE.ZHANHUNSUIT tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.ZHANHUNSUIT;
		}
		return null != tbl;
	}
	public int GetZhanHunSuitId(int id,int defaultValue = default(int))
	{
		TABLE.ZHANHUNSUIT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetZhanHunSuitName(int id,string defaultValue = default(string))
	{
		TABLE.ZHANHUNSUIT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.name;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetZhanHunSuitSuitNum(int id,int defaultValue = default(int))
	{
		TABLE.ZHANHUNSUIT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.suitNum;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetZhanHunSuitLevClass(int id,int defaultValue = default(int))
	{
		TABLE.ZHANHUNSUIT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.levClass;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetZhanHunSuitQuality(int id,int defaultValue = default(int))
	{
		TABLE.ZHANHUNSUIT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.quality;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetZhanHunSuitSuitSummoned(int id,int defaultValue = default(int))
	{
		TABLE.ZHANHUNSUIT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.suitSummoned;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetZhanHunSuitWeaponmodel(int id,int defaultValue = default(int))
	{
		TABLE.ZHANHUNSUIT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.weaponmodel;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetZhanHunSuitClothesmodel(int id,int defaultValue = default(int))
	{
		TABLE.ZHANHUNSUIT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.clothesmodel;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetZhanHunSuitGetWay(int id,string defaultValue = default(string))
	{
		TABLE.ZHANHUNSUIT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.getWay;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetZhanHunSuitMaxLevel(int id,int defaultValue = default(int))
	{
		TABLE.ZHANHUNSUIT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.maxLevel;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetZhanHunSuitSkillNum(int id,int defaultValue = default(int))
	{
		TABLE.ZHANHUNSUIT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.skillNum;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetZhanHunSuitFactor(int id,string defaultValue = default(string))
	{
		TABLE.ZHANHUNSUIT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.factor;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetZhanHunSuitDescription(int id,string defaultValue = default(string))
	{
		TABLE.ZHANHUNSUIT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.description;
		}
		else
		{
			return defaultValue;
		}
	}
}