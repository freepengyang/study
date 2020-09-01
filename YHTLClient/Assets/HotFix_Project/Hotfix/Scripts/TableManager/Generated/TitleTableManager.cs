//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_title.proto
using System.Collections.Generic;
public partial class TitleTableManager : TableManager<TABLE.TITLEARRAY, TABLE.TITLE,int,TitleTableManager>
{
	public override bool TryGetValue(int key, out TABLE.TITLE tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.TITLE;
		}
		return null != tbl;
	}
		public int make_atkMin10_atkMax10_phyDefMin10(uint phyDefMin,uint atkMax,uint atkMin)
		{
			int ____id = 0;
			____id |= (int)(phyDefMin << 0);
			____id |= (int)(atkMax << 10);
			____id |= (int)(atkMin << 20);
			return ____id;
		}

		public int make_phyDefMax10_magicDefMin10_magicDefMax10(uint magicDefMax,uint magicDefMin,uint phyDefMax)
		{
			int ____id = 0;
			____id |= (int)(magicDefMax << 0);
			____id |= (int)(magicDefMin << 10);
			____id |= (int)(phyDefMax << 20);
			return ____id;
		}

		public int make_hp17_pkDmg14(uint pkDmg,uint hp)
		{
			int ____id = 0;
			____id |= (int)(pkDmg << 0);
			____id |= (int)(hp << 14);
			return ____id;
		}

		public int make_critRate14_critDamage17(uint critDamage,uint critRate)
		{
			int ____id = 0;
			____id |= (int)(critDamage << 0);
			____id |= (int)(critRate << 17);
			return ____id;
		}

	public int GetTitleId(int id,int defaultValue = default(int))
	{
		TABLE.TITLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetTitleTitleName(int id,string defaultValue = default(string))
	{
		TABLE.TITLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.titleName;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetTitleType(int id,uint defaultValue = default(uint))
	{
		TABLE.TITLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.type;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetTitleSex(int id,uint defaultValue = default(uint))
	{
		TABLE.TITLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.sex;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetTitleLastTime(int id,uint defaultValue = default(uint))
	{
		TABLE.TITLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.lastTime;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetTitleImgRes(int id,string defaultValue = default(string))
	{
		TABLE.TITLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.imgRes;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetTitleDes(int id,string defaultValue = default(string))
	{
		TABLE.TITLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.des;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetTitleGetWay(int id,string defaultValue = default(string))
	{
		TABLE.TITLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.getWay;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetTitleCannal(int id,uint defaultValue = default(uint))
	{
		TABLE.TITLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.cannal;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetTitleAtkMin10_atkMax10_phyDefMin10(int id,uint defaultValue = default(uint))
	{
		TABLE.TITLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.atkMin10_atkMax10_phyDefMin10;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetTitlePhyDefMax10_magicDefMin10_magicDefMax10(int id,uint defaultValue = default(uint))
	{
		TABLE.TITLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.phyDefMax10_magicDefMin10_magicDefMax10;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetTitleHp17_pkDmg14(int id,uint defaultValue = default(uint))
	{
		TABLE.TITLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.hp17_pkDmg14;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetTitleCritRate14_critDamage17(int id,uint defaultValue = default(uint))
	{
		TABLE.TITLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.critRate14_critDamage17;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetTitleShow(int id,uint defaultValue = default(uint))
	{
		TABLE.TITLE cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.show;
		}
		else
		{
			return defaultValue;
		}
	}
}