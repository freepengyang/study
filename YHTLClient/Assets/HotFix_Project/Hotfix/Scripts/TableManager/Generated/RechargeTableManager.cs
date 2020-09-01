//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_recharge.proto
using System.Collections.Generic;
public partial class RechargeTableManager : TableManager<TABLE.RECHARGEARRAY, TABLE.RECHARGE,int,RechargeTableManager>
{
	public override bool TryGetValue(int key, out TABLE.RECHARGE tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.RECHARGE;
		}
		return null != tbl;
	}
		public int make_id7_money20(int money,int id)
		{
			int ____id = 0;
			____id |= (int)(money << 0);
			____id |= (int)(id << 20);
			return ____id;
		}

	public int GetRechargeId(int id,int defaultValue = default(int))
	{
		TABLE.RECHARGE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetRechargeGold(int id,uint defaultValue = default(uint))
	{
		TABLE.RECHARGE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.gold;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetRechargeMoney(int id,int defaultValue = default(int))
	{
		TABLE.RECHARGE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.money;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetRechargeBonusBox(int id,int defaultValue = default(int))
	{
		TABLE.RECHARGE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.bonusBox;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetRechargeImgSrc(int id,string defaultValue = default(string))
	{
		TABLE.RECHARGE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.imgSrc;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetRechargeId7_money20(int id,uint defaultValue = default(uint))
	{
		TABLE.RECHARGE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id7_money20;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetRechargeChannelControl(int id,int defaultValue = default(int))
	{
		TABLE.RECHARGE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.channelControl;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetRechargeTimes(int id,string defaultValue = default(string))
	{
		TABLE.RECHARGE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.times;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetRechargeVip(int id,string defaultValue = default(string))
	{
		TABLE.RECHARGE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.vip;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetRechargeOrder(int id,int defaultValue = default(int))
	{
		TABLE.RECHARGE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.order;
		}
		else
		{
			return defaultValue;
		}
	}
}