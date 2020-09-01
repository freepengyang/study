//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_buffer.proto
using System.Collections.Generic;
public partial class BufferTableManager : TableManager<TABLE.BUFFERARRAY, TABLE.BUFFER,int,BufferTableManager>
{
	public override bool TryGetValue(int key, out TABLE.BUFFER tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.BUFFER;
		}
		return null != tbl;
	}
	public int GetBufferId(int id,int defaultValue = default(int))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetBufferName(int id,string defaultValue = default(string))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.name;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetBufferIcon(int id,string defaultValue = default(string))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.icon;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetBufferType(int id,uint defaultValue = default(uint))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.type;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetBufferExeType(int id,uint defaultValue = default(uint))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.exeType;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetBufferExeParam(int id,uint defaultValue = default(uint))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.exeParam;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetBufferDispelType(int id,uint defaultValue = default(uint))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.dispelType;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetBufferDispelParam(int id,uint defaultValue = default(uint))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.dispelParam;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBufferEffectId(int id,int defaultValue = default(int))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.effectId;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetBufferTips(int id,string defaultValue = default(string))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.tips;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBufferTipParam(int id,int defaultValue = default(int))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.tipParam;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetBufferShowDelay(int id,uint defaultValue = default(uint))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.showDelay;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetBufferAttBuff(int id,string defaultValue = default(string))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.attBuff;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetBufferHurtBuff(int id,string defaultValue = default(string))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.hurtBuff;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBufferExpBuff(int id,int defaultValue = default(int))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.expBuff;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetBufferReplyBuff(int id,string defaultValue = default(string))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.replyBuff;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetBufferReboundBuff(int id,string defaultValue = default(string))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.reboundBuff;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetBufferParameterBuff(int id,string defaultValue = default(string))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.parameterBuff;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetBufferShowOrHide(int id,uint defaultValue = default(uint))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.showOrHide;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetBufferHeight(int id,string defaultValue = default(string))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.height;
		}
		else
		{
			return defaultValue;
		}
	}
	public uint GetBufferLayer(int id,uint defaultValue = default(uint))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.layer;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetBufferJunpWord(int id,string defaultValue = default(string))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.junpWord;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetBufferTriggerTime(int id,string defaultValue = default(string))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.triggerTime;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBufferExclusionGroup(int id,int defaultValue = default(int))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.exclusionGroup;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBufferExclusionOrder(int id,int defaultValue = default(int))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.exclusionOrder;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetBufferSuperposition(int id,string defaultValue = default(string))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.superposition;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBufferLayers(int id,int defaultValue = default(int))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.layers;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetBufferFormula(int id,string defaultValue = default(string))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.formula;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetBufferMapId(int id,string defaultValue = default(string))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.mapId;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetBufferLevel(int id,int defaultValue = default(int))
	{
		TABLE.BUFFER cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.level;
		}
		else
		{
			return defaultValue;
		}
	}
}