//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_eventtable.proto
using System.Collections.Generic;
public partial class EventTableTableManager : TableManager<TABLE.EVENTTABLEARRAY, TABLE.EVENTTABLE,int,EventTableTableManager>
{
	public override bool TryGetValue(int key, out TABLE.EVENTTABLE tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.EVENTTABLE;
		}
		return null != tbl;
	}
	public int GetEventTableId(int id,int defaultValue = default(int))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetEventTableIcon(int id,uint defaultValue = default(uint))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.icon;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetEventTableDeliverId(int id,string defaultValue = default(string))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.deliverId;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetEventTableStartTime(int id,uint defaultValue = default(uint))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.startTime;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetEventTableEndTime(int id,uint defaultValue = default(uint))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.endTime;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetEventTableDay(int id,string defaultValue = default(string))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.day;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetEventTableAwards(int id,string defaultValue = default(string))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.awards;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetEventTableTips(int id,string defaultValue = default(string))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.tips;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetEventTableNpcName(int id,string defaultValue = default(string))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.npcName;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetEventTableFirstTime(int id,uint defaultValue = default(uint))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.firstTime;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetEventTableOpenTime(int id,string defaultValue = default(string))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.openTime;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetEventTableLastTime(int id,uint defaultValue = default(uint))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.lastTime;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetEventTableOpenDay(int id,string defaultValue = default(string))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.openDay;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetEventTableCombineTime(int id,uint defaultValue = default(uint))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.combineTime;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetEventTableCombineDay(int id,string defaultValue = default(string))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.combineDay;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetEventTableMapId(int id,string defaultValue = default(string))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.mapId;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetEventTableTransportLimit(int id,uint defaultValue = default(uint))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.transportLimit;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetEventTablePushOpen(int id,string defaultValue = default(string))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.pushOpen;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetEventTableAttentionOpen(int id,uint defaultValue = default(uint))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.attentionOpen;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetEventTableMoren(int id,uint defaultValue = default(uint))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.moren;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetEventTableNeedDiYuan(int id,uint defaultValue = default(uint))
	{
		TABLE.EVENTTABLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.needDiYuan;
		}
		else
		{
			return defaultValue;
		}
	}
}