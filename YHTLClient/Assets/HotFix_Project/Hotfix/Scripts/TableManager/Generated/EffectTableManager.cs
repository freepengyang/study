//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_effect.proto
using System.Collections.Generic;
public partial class EffectTableManager : TableManager<TABLE.EFFECTARRAY, TABLE.EFFECT,int,EffectTableManager>
{
	public override bool TryGetValue(int key, out TABLE.EFFECT tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.EFFECT;
		}
		return null != tbl;
	}
	public int GetEffectId(int id,int defaultValue = default(int))
	{
		TABLE.EFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetEffectName(int id,string defaultValue = default(string))
	{
		TABLE.EFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.name;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetEffectResType(int id,int defaultValue = default(int))
	{
		TABLE.EFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.resType;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetEffectDestroyTime(int id,int defaultValue = default(int))
	{
		TABLE.EFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.destroyTime;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetEffectEffecttype(int id,int defaultValue = default(int))
	{
		TABLE.EFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.effecttype;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetEffectOffsetX(int id,int defaultValue = default(int))
	{
		TABLE.EFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.offsetX;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetEffectOffsetY(int id,int defaultValue = default(int))
	{
		TABLE.EFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.offsetY;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetEffectOffsetZ(int id,int defaultValue = default(int))
	{
		TABLE.EFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.offsetZ;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetEffectPlayType(int id,int defaultValue = default(int))
	{
		TABLE.EFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.playType;
		}
		else
		{
			return defaultValue;
		}
	}
}