//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_talkpath.proto
using System.Collections.Generic;
public partial class TalkPathTableManager : TableManager<TABLE.TALKPATHARRAY, TABLE.TALKPATH,int,TalkPathTableManager>
{
	public override bool TryGetValue(int key, out TABLE.TALKPATH tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.TALKPATH;
		}
		return null != tbl;
	}
	public int GetTalkPathId(int id,int defaultValue = default(int))
	{
		TABLE.TALKPATH cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
}