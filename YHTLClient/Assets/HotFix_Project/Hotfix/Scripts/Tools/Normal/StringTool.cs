using UnityEngine;
using System.Collections;

public class StringTool
{
    /// <summary>
    /// 通过传进来的中文KEY 去数据表里面读对应替换的多语言文字
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string GetText(string text)
    {
        return text;
    }

    /// <summary>
    /// 通过传进来的中文KEY 去数据表里面读对应替换的多语言文字
    /// </summary>
    /// <param name="text"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string GetText(string text, params object[] args)
    {
        return string.Format(text, args);
    }
    /// <summary>
    /// 通过传进来的中文KEY 去数据表里面读对应替换的多语言文字
    /// </summary>
    /// <param name="args">多参</param>
    /// <returns></returns>
    public static string GetText(params string[] args)
    {
        CSStringBuilder.Clear();
        CSStringBuilder.Append(args);
        return CSStringBuilder.ToString();
    }
}
