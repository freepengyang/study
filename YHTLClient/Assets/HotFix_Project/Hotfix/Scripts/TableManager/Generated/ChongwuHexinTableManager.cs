//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_chongwuhexin.proto
using System.Collections.Generic;
public partial class ChongwuHexinTableManager : TableManager<TABLE.CHONGWUHEXINARRAY, TABLE.CHONGWUHEXIN,int,ChongwuHexinTableManager>
{
	public override bool TryGetValue(int key, out TABLE.CHONGWUHEXIN tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.CHONGWUHEXIN;
		}
		return null != tbl;
	}
	public int GetChongwuHexinId(int id,int defaultValue = default(int))
	{
		TABLE.CHONGWUHEXIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetChongwuHexinTalenttype(int id,int defaultValue = default(int))
	{
		TABLE.CHONGWUHEXIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.talenttype;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetChongwuHexinEquipmenttype(int id,int defaultValue = default(int))
	{
		TABLE.CHONGWUHEXIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.equipmenttype;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetChongwuHexinLevclass(int id,int defaultValue = default(int))
	{
		TABLE.CHONGWUHEXIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.levclass;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetChongwuHexinTip(int id,string defaultValue = default(string))
	{
		TABLE.CHONGWUHEXIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.tip;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetChongwuHexinPara1(int id,int defaultValue = default(int))
	{
		TABLE.CHONGWUHEXIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.para1;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetChongwuHexinPara2(int id,int defaultValue = default(int))
	{
		TABLE.CHONGWUHEXIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.para2;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetChongwuHexinPara3(int id,int defaultValue = default(int))
	{
		TABLE.CHONGWUHEXIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.para3;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetChongwuHexinPara4(int id,int defaultValue = default(int))
	{
		TABLE.CHONGWUHEXIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.para4;
		}
		else
		{
			return defaultValue;
		}
	}
}