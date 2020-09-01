using System;
using System.Collections.Generic;
using Google.Protobuf;

public class TableUtility : Singleton2<TableUtility>
{
    public Dictionary<Type, MessageParser> TableLoadDicHot = new Dictionary<Type, MessageParser>
    {
        {typeof(TABLE.SKILLDIRECTION), TABLE.SKILLDIRECTIONARRAY.Parser},
        {typeof(TABLE.SKILLEFFECT), TABLE.SKILLEFFECTARRAY.Parser},
        //{typeof(TABLE.DROPSHOW), TABLE.DROPSHOWARRAY.Parser},
    };

    public Google.Protobuf.MessageParser GetMsgHotType(Type type)
    {
        if (TableLoadDicHot.ContainsKey(type))
        {
            return TableLoadDicHot[type];
        }

        return null;
    }
}