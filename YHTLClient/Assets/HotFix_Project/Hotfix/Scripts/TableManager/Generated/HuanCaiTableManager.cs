//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_huancai.proto
using System.Collections.Generic;
public partial class HuanCaiTableManager : TableManager<TABLE.HUANCAIARRAY, TABLE.HUANCAI,int,HuanCaiTableManager>
{
	public override bool TryGetValue(int key, out TABLE.HUANCAI tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.HUANCAI;
		}
		return null != tbl;
	}
	public int GetHuanCaiId(int id,int defaultValue = default(int))
	{
		TABLE.HUANCAI cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetHuanCaiItemId(int id,int defaultValue = default(int))
	{
		TABLE.HUANCAI cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.itemId;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetHuanCaiName(int id,string defaultValue = default(string))
	{
		TABLE.HUANCAI cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.name;
		}
		else
		{
			return defaultValue;
		}
	}
	public IntArray GetHuanCaiZsattrPara(int id,IntArray defaultValue = default(IntArray))
	{
		TABLE.HUANCAI cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.zsattrPara;
		}
		else
		{
			return defaultValue;
		}
	}
	public IntArray GetHuanCaiFsattrPara(int id,IntArray defaultValue = default(IntArray))
	{
		TABLE.HUANCAI cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.fsattrPara;
		}
		else
		{
			return defaultValue;
		}
	}
	public IntArray GetHuanCaiDsattrPara(int id,IntArray defaultValue = default(IntArray))
	{
		TABLE.HUANCAI cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.dsattrPara;
		}
		else
		{
			return defaultValue;
		}
	}
	public IntArray GetHuanCaiAttrNum(int id,IntArray defaultValue = default(IntArray))
	{
		TABLE.HUANCAI cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.attrNum;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetHuanCaiModel(int id,int defaultValue = default(int))
	{
		TABLE.HUANCAI cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.model;
		}
		else
		{
			return defaultValue;
		}
	}
}