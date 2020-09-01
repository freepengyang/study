//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_guidegroup.proto
using System.Collections.Generic;
public partial class GuideGroupTableManager : TableManager<TABLE.GUIDEGROUPARRAY, TABLE.GUIDEGROUP,int,GuideGroupTableManager>
{
	public override bool TryGetValue(int key, out TABLE.GUIDEGROUP tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.GUIDEGROUP;
		}
		return null != tbl;
	}
	public int GetGuideGroupId(int id,int defaultValue = default(int))
	{
		TABLE.GUIDEGROUP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGuideGroupGuildType(int id,int defaultValue = default(int))
	{
		TABLE.GUIDEGROUP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.GuildType;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetGuideGroupDepthPanel(int id,string defaultValue = default(string))
	{
		TABLE.GUIDEGROUP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.DepthPanel;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGuideGroupAuto(int id,int defaultValue = default(int))
	{
		TABLE.GUIDEGROUP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.Auto;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetGuideGroupFactor(int id,string defaultValue = default(string))
	{
		TABLE.GUIDEGROUP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.factor;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGuideGroupStep(int id,int defaultValue = default(int))
	{
		TABLE.GUIDEGROUP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.step;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGuideGroupTriggerType(int id,int defaultValue = default(int))
	{
		TABLE.GUIDEGROUP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.TriggerType;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetGuideGroupBeginParam(int id,string defaultValue = default(string))
	{
		TABLE.GUIDEGROUP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.beginParam;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetGuideGroupLinks(int id,string defaultValue = default(string))
	{
		TABLE.GUIDEGROUP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.Links;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGuideGroupBeginLv(int id,int defaultValue = default(int))
	{
		TABLE.GUIDEGROUP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.beginLv;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGuideGroupEndLv(int id,int defaultValue = default(int))
	{
		TABLE.GUIDEGROUP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.endLv;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGuideGroupAnchorX(int id,int defaultValue = default(int))
	{
		TABLE.GUIDEGROUP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.AnchorX;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGuideGroupAnchorY(int id,int defaultValue = default(int))
	{
		TABLE.GUIDEGROUP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.AnchorY;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGuideGroupAnchorAbsX(int id,int defaultValue = default(int))
	{
		TABLE.GUIDEGROUP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.AnchorAbsX;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGuideGroupAnchorAbsY(int id,int defaultValue = default(int))
	{
		TABLE.GUIDEGROUP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.AnchorAbsY;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGuideGroupTask(int id,int defaultValue = default(int))
	{
		TABLE.GUIDEGROUP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.task;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetGuideGroupTime(int id,int defaultValue = default(int))
	{
		TABLE.GUIDEGROUP cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.time;
		}
		else
		{
			return defaultValue;
		}
	}
}