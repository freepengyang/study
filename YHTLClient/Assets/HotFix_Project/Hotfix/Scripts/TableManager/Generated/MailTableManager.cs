//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_mail.proto
using System.Collections.Generic;
public partial class MailTableManager : TableManager<TABLE.MAILARRAY, TABLE.MAIL,int,MailTableManager>
{
	public override bool TryGetValue(int key, out TABLE.MAIL tbl)
	{
		tbl = null;
		if (array == null || array.gItem == null)
			return false;
		TableHandle handle = null;
		if (array.gItem.id2offset.TryGetValue(key, out handle))
		{
			tbl = handle.Value as TABLE.MAIL;
		}
		return null != tbl;
	}
	public int GetMailId(int id,int defaultValue = default(int))
	{
		TABLE.MAIL cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetMailItems(int id,string defaultValue = default(string))
	{
		TABLE.MAIL cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.items;
		}
		else
		{
			return defaultValue;
		}
	}
}