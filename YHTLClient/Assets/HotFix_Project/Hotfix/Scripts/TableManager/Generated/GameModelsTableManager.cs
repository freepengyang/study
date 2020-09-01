//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_gamemodels.proto
using System.Collections.Generic;
public partial class GameModelsTableManager : TableManager<TABLE.GAMEMODELSARRAY, TABLE.GAMEMODELS,int,GameModelsTableManager>
{
	public override bool TryGetValue(int key, out TABLE.GAMEMODELS tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.GAMEMODELS;
		}
		return null != tbl;
	}
	public int GetGameModelsId(int id,int defaultValue = default(int))
	{
		TABLE.GAMEMODELS cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetGameModelsModel(int id,string defaultValue = default(string))
	{
		TABLE.GAMEMODELS cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.model;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetGameModelsModelName(int id,string defaultValue = default(string))
	{
		TABLE.GAMEMODELS cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.modelName;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGameModelsLayer(int id,int defaultValue = default(int))
	{
		TABLE.GAMEMODELS cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.layer;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGameModelsSubLayer(int id,int defaultValue = default(int))
	{
		TABLE.GAMEMODELS cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.subLayer;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGameModelsFunctionId(int id,int defaultValue = default(int))
	{
		TABLE.GAMEMODELS cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.functionId;
		}
		else
		{
			return defaultValue;
		}
	}
}