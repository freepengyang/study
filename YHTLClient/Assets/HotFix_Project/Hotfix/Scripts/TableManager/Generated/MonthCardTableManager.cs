//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_monthcard.proto
using System.Collections.Generic;
public partial class MonthCardTableManager : TableManager<TABLE.MONTHCARDARRAY, TABLE.MONTHCARD,int,MonthCardTableManager>
{
	public override bool TryGetValue(int key, out TABLE.MONTHCARD tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.MONTHCARD;
		}
		return null != tbl;
	}
	public int GetMonthCardId(int id,int defaultValue = default(int))
	{
		TABLE.MONTHCARD cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetMonthCardName(int id,string defaultValue = default(string))
	{
		TABLE.MONTHCARD cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.name;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetMonthCardPrice(int id,int defaultValue = default(int))
	{
		TABLE.MONTHCARD cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.price;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetMonthCardGuaJiTimeAdd(int id,int defaultValue = default(int))
	{
		TABLE.MONTHCARD cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.guaJiTimeAdd;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetMonthCardPersonalBossAdd(int id,int defaultValue = default(int))
	{
		TABLE.MONTHCARD cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.personalBossAdd;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetMonthCardEquipPreventFalling(int id,int defaultValue = default(int))
	{
		TABLE.MONTHCARD cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.equipPreventFalling;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetMonthCardExclusiveMap(int id,int defaultValue = default(int))
	{
		TABLE.MONTHCARD cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.exclusiveMap;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetMonthCardRewardDay(int id,int defaultValue = default(int))
	{
		TABLE.MONTHCARD cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.rewardDay;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetMonthCardDuration(int id,int defaultValue = default(int))
	{
		TABLE.MONTHCARD cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.duration;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetMonthCardTip(int id,string defaultValue = default(string))
	{
		TABLE.MONTHCARD cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.tip;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetMonthCardBuffID(int id,string defaultValue = default(string))
	{
		TABLE.MONTHCARD cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.buffID;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetMonthCardShenfu(int id,int defaultValue = default(int))
	{
		TABLE.MONTHCARD cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.shenfu;
		}
		else
		{
			return defaultValue;
		}
	}
}