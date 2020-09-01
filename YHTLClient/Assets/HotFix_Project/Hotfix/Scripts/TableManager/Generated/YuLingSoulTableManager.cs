//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_yulingsoul.proto
using System.Collections.Generic;
public partial class YuLingSoulTableManager : TableManager<TABLE.YULINGSOULARRAY, TABLE.YULINGSOUL,int,YuLingSoulTableManager>
{
	public override bool TryGetValue(int key, out TABLE.YULINGSOUL tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.YULINGSOUL;
		}
		return null != tbl;
	}
	public int GetYuLingSoulId(int id,int defaultValue = default(int))
	{
		TABLE.YULINGSOUL cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetYuLingSoulName(int id,string defaultValue = default(string))
	{
		TABLE.YULINGSOUL cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.name;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetYuLingSoulLevel(int id,int defaultValue = default(int))
	{
		TABLE.YULINGSOUL cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.level;
		}
		else
		{
			return defaultValue;
		}
	}
	public LongArray GetYuLingSoulCost(int id,LongArray defaultValue = default(LongArray))
	{
		TABLE.YULINGSOUL cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.cost;
		}
		else
		{
			return defaultValue;
		}
	}
	public LongArray GetYuLingSoulZsattr(int id,LongArray defaultValue = default(LongArray))
	{
		TABLE.YULINGSOUL cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.zsattr;
		}
		else
		{
			return defaultValue;
		}
	}
	public LongArray GetYuLingSoulFsattr(int id,LongArray defaultValue = default(LongArray))
	{
		TABLE.YULINGSOUL cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.fsattr;
		}
		else
		{
			return defaultValue;
		}
	}
	public LongArray GetYuLingSoulDsattr(int id,LongArray defaultValue = default(LongArray))
	{
		TABLE.YULINGSOUL cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.dsattr;
		}
		else
		{
			return defaultValue;
		}
	}
	public LongArray GetYuLingSoulZsexattr(int id,LongArray defaultValue = default(LongArray))
	{
		TABLE.YULINGSOUL cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.zsexattr;
		}
		else
		{
			return defaultValue;
		}
	}
	public LongArray GetYuLingSoulFsexattr(int id,LongArray defaultValue = default(LongArray))
	{
		TABLE.YULINGSOUL cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.fsexattr;
		}
		else
		{
			return defaultValue;
		}
	}
	public LongArray GetYuLingSoulDsexattr(int id,LongArray defaultValue = default(LongArray))
	{
		TABLE.YULINGSOUL cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.dsexattr;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetYuLingSoulExattr(int id,int defaultValue = default(int))
	{
		TABLE.YULINGSOUL cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.exattr;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetYuLingSoulPosition(int id,int defaultValue = default(int))
	{
		TABLE.YULINGSOUL cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.position;
		}
		else
		{
			return defaultValue;
		}
	}
}