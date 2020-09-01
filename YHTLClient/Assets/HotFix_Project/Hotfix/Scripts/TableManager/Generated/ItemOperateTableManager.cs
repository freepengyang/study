//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_itemoperate.proto
using System.Collections.Generic;
public partial class ItemOperateTableManager : TableManager<TABLE.ITEMOPERATEARRAY, TABLE.ITEMOPERATE,int,ItemOperateTableManager>
{
	public override bool TryGetValue(int key, out TABLE.ITEMOPERATE tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.ITEMOPERATE;
		}
		return null != tbl;
	}
	public int GetItemOperateId(int id,int defaultValue = default(int))
	{
		TABLE.ITEMOPERATE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetItemOperateItemType(int id,uint defaultValue = default(uint))
	{
		TABLE.ITEMOPERATE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.itemType;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetItemOperateSecondType(int id,uint defaultValue = default(uint))
	{
		TABLE.ITEMOPERATE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.secondType;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetItemOperateOperationtype(int id,uint defaultValue = default(uint))
	{
		TABLE.ITEMOPERATE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.Operationtype;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetItemOperateLeftOperate(int id,string defaultValue = default(string))
	{
		TABLE.ITEMOPERATE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.leftOperate;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetItemOperateRightOperate(int id,string defaultValue = default(string))
	{
		TABLE.ITEMOPERATE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.rightOperate;
		}
		else
		{
			return defaultValue;
		}
	}
}