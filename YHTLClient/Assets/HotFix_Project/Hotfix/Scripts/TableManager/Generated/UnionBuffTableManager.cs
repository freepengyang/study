//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_unionbuff.proto
using System.Collections.Generic;
public partial class UnionBuffTableManager : TableManager<TABLE.UNIONBUFFARRAY, TABLE.UNIONBUFF,int,UnionBuffTableManager>
{
	public override bool TryGetValue(int key, out TABLE.UNIONBUFF tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.UNIONBUFF;
		}
		return null != tbl;
	}
		public int make_id(int position,int level)
		{
			int ____id = 0;
			____id |= (int)(position << 0);
			____id |= (int)(level << 8);
			return ____id;
		}

	public int GetUnionBuffId(int id,int defaultValue = default(int))
	{
		TABLE.UNIONBUFF cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetUnionBuffLevel(int id,int defaultValue = default(int))
	{
		TABLE.UNIONBUFF cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.level;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetUnionBuffPosition(int id,int defaultValue = default(int))
	{
		TABLE.UNIONBUFF cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.position;
		}
		else
		{
			return defaultValue;
		}
	}
	public LongArray GetUnionBuffCost(int id,LongArray defaultValue = default(LongArray))
	{
		TABLE.UNIONBUFF cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.cost;
		}
		else
		{
			return defaultValue;
		}
	}
	public LongArray GetUnionBuffAttr(int id,LongArray defaultValue = default(LongArray))
	{
		TABLE.UNIONBUFF cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.attr;
		}
		else
		{
			return defaultValue;
		}
	}
}