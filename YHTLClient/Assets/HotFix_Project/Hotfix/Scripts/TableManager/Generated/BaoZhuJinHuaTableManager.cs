//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_baozhujinhua.proto
using System.Collections.Generic;
public partial class BaoZhuJinHuaTableManager : TableManager<TABLE.BAOZHUJINHUAARRAY, TABLE.BAOZHUJINHUA,int,BaoZhuJinHuaTableManager>
{
	public override bool TryGetValue(int key, out TABLE.BAOZHUJINHUA tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.BAOZHUJINHUA;
		}
		return null != tbl;
	}
	public int GetBaoZhuJinHuaId(int id,int defaultValue = default(int))
	{
		TABLE.BAOZHUJINHUA cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBaoZhuJinHuaRank(int id,int defaultValue = default(int))
	{
		TABLE.BAOZHUJINHUA cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.rank;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBaoZhuJinHuaAddRandomAttr(int id,int defaultValue = default(int))
	{
		TABLE.BAOZHUJINHUA cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.addRandomAttr;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBaoZhuJinHuaMaxLevel(int id,int defaultValue = default(int))
	{
		TABLE.BAOZHUJINHUA cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.maxLevel;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBaoZhuJinHuaSkillSlotID(int id,int defaultValue = default(int))
	{
		TABLE.BAOZHUJINHUA cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.skillSlotID;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBaoZhuJinHuaBossLevel(int id,int defaultValue = default(int))
	{
		TABLE.BAOZHUJINHUA cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.bossLevel;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBaoZhuJinHuaBossNum(int id,int defaultValue = default(int))
	{
		TABLE.BAOZHUJINHUA cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.bossNum;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBaoZhuJinHuaQuality(int id,int defaultValue = default(int))
	{
		TABLE.BAOZHUJINHUA cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.quality;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBaoZhuJinHuaLevClass(int id,int defaultValue = default(int))
	{
		TABLE.BAOZHUJINHUA cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.levClass;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetBaoZhuJinHuaGradename(int id,string defaultValue = default(string))
	{
		TABLE.BAOZHUJINHUA cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.gradename;
		}
		else
		{
			return defaultValue;
		}
	}
}