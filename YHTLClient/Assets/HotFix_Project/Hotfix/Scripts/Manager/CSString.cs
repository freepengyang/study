using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;

public class CSString
{
    public static String Format(string format, params object[] args)
    {
        return string.Format(format, args);
    }

    public static String Format(int tableID, params object[] args)
    {
        return string.Format(Format(tableID), args);
    }

    public static String Format(bool condition,int tableID1, int tableID2, params object[] args)
    {
        int id = condition ? tableID1 : tableID2;
        return string.Format(Format(id), args);
    }

    public static String Format(bool condition, int tableID1, int tableID2)
    {
        int id = condition ? tableID1 : tableID2;
        return ClientTipsTableManager.Instance.GetClientTipsContext(id);
    }

    public static String Format(int tableID)
    {
        return ClientTipsTableManager.Instance.GetClientTipsContext(tableID);
    }

    //获取提示框文字内容
    public static String Format2(int tableID, params object[] args)
    {
        return string.Format(Format2(tableID), args);
    }

    public static String Format2(int tableID)
    {
        TABLE.PROMPTWORD clientTips;
        return PromptWordTableManager.Instance.TryGetValue(tableID, out clientTips) ? clientTips.dec : "";
    }

    public static String CombineString(params string[] x_str)
    {
        if (x_str.Length <= 0)
            return "";
        CSStringBuilder.Clear();
        for (int i = 0; i < x_str.Length; i++)
        {
            CSStringBuilder.Append(x_str[i]);
        }
        return CSStringBuilder.ToString();
    }
}
