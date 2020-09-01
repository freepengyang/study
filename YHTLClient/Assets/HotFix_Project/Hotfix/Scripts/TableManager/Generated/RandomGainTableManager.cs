//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_randomgain.proto
using System.Collections.Generic;
using Google.Protobuf.Collections;
public partial class RandomGainTableManager : TableManager<TABLE.RANDOMGAINARRAY, TABLE.RANDOMGAIN,int,RandomGainTableManager>
{
	protected override void OnResourceLoaded(CSResourceWWW res)
	{
		base.OnResourceLoaded(res);
		if (array != null)
		{
			for (int i = 0; i < array.rows.Count; i++)
			{
				int id = array.rows[i].id;
				AddTables(id, array.rows[i]);
			}
		}
		base.OnDealOver();
	}
	public int GetRandomGainId(int id,int defaultValue = default(int))
	{
		TABLE.RANDOMGAIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetRandomGainGainID(int id,int defaultValue = default(int))
	{
		TABLE.RANDOMGAIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.gainID;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetRandomGainType(int id,int defaultValue = default(int))
	{
		TABLE.RANDOMGAIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.Type;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetRandomGainTypeRate(int id,int defaultValue = default(int))
	{
		TABLE.RANDOMGAIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.typeRate;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetRandomGainParameter(int id,int defaultValue = default(int))
	{
		TABLE.RANDOMGAIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.parameter;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetRandomGainParaRate(int id,int defaultValue = default(int))
	{
		TABLE.RANDOMGAIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.paraRate;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetRandomGainMax(int id,int defaultValue = default(int))
	{
		TABLE.RANDOMGAIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.max;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetRandomGainIcon(int id,string defaultValue = default(string))
	{
		TABLE.RANDOMGAIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.Icon;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetRandomGainFactor(int id,int defaultValue = default(int))
	{
		TABLE.RANDOMGAIN cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.factor;
		}
		else
		{
			return defaultValue;
		}
	}
}