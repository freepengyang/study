//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_specialequip.proto
using System.Collections.Generic;
public partial class SpecialEquipTableManager : TableManager<TABLE.SPECIALEQUIPARRAY, TABLE.SPECIALEQUIP,int,SpecialEquipTableManager>
{
	public override bool TryGetValue(int key, out TABLE.SPECIALEQUIP tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.SPECIALEQUIP;
		}
		return null != tbl;
	}
	public int GetSpecialEquipId(int id,int defaultValue = default(int))
	{
		TABLE.SPECIALEQUIP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSpecialEquipType(int id,int defaultValue = default(int))
	{
		TABLE.SPECIALEQUIP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.type;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetSpecialEquipAttr(int id,string defaultValue = default(string))
	{
		TABLE.SPECIALEQUIP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.attr;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetSpecialEquipLongji(int id,string defaultValue = default(string))
	{
		TABLE.SPECIALEQUIP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.longji;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetSpecialEquipLongli1(int id,string defaultValue = default(string))
	{
		TABLE.SPECIALEQUIP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.longli1;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetSpecialEquipLongli2(int id,string defaultValue = default(string))
	{
		TABLE.SPECIALEQUIP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.longli2;
		}
		else
		{
			return defaultValue;
		}
	}
}