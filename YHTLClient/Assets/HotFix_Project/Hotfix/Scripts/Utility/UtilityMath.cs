using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using UnityEngine;

public class UtilityMath
{
    public const long wan = 10000;
    public const long shiwan = 100000;
    public const long baiwan = 1000000;
    public const long qianwan = 10000000;
    public const long yi = 100000000;
    public const long shiyi = 1000000000;
    public const long baiyi = 10000000000;
    public const long qianyi = 100000000000;
    public const long wanyi = 1000000000000;
    /// <summary>
    /// 获取服务器端发送64位长整型低32位的值
    /// </summary>
    /// <param name="Value">64位长整型</param>
    /// <returns></returns>
    public static int GetProPertyValue(long Value)
    {
        return (int)(Value & 0xffffffff);
    }

    public static Int64 SetBytes64(long num)
    {
        return num << 32;
    }

    public static Int64 GetRefine(long num, long value)
    {
        long Num = num >> 32 & 0xff;
        return (SetBytes64(Num) | value);
    }

    /// <summary>
    /// 获取服务器端发送64位长整型高32位的值
    /// </summary>
    /// <param name="Value">64位长整型</param>
    /// <returns></returns>
    public static int GetProPerty(long Value)
    {
        return (int)(Value >> 32 & 0xff);

    }


    /// <summary>
    /// decimal保留指定位数小数
    /// </summary>
    /// <param name="num">原始数量</param>
    /// <param name="scale">保留小数位数</param>
    /// <returns>截取指定小数位数后的数量字符串</returns>
    public static string DecimalToString(decimal num, int scale)
    {
        string numToString = num.ToString();

        int index = numToString.IndexOf(".");
        int length = numToString.Length;

        if (index != -1)
        {
            return string.Format("{0}.{1}",
                numToString.Substring(0, index),
                numToString.Substring(index + 1, Math.Min(length - index - 1, scale)));
        }
        else
        {
            return num.ToString();
        }
    }


    public static bool Compare(int x, int y, int distance)
    {
        return Mathf.Abs(x - y) < distance;
    }
    public static string GetEquipRecastDecimalValue(long _num)
    {
        if (_num < 10000)
        {
            return _num.ToString();
        }
        else
        {
            //$"{Convert.ToDecimal(_num * 0.0001f).ToString("F1")}万";
            return $"{DecimalToString(Convert.ToDecimal(_num * 0.0001f), 1)}万";
        }
    }
    public static string GetDecimalValue(double _num, string _deil)
    {
        float result;
        string str;
        if (_num < 10000)
        {
            return Math.Ceiling(_num).ToString();
        }
        else if (10000 <= _num && _num < 100000000)
        {
            result = float.Parse((_num / 10000f).ToString(_deil));
            str = string.Concat(result, "万");
        }
        else
        {
            result = float.Parse((_num / 100000000f).ToString(_deil));
            str = string.Concat(result, "亿");
        }

        return str;
    }
    public static string GetDecimalValue(long _num)
    {
        string str;
        if (_num < 10000)
        {
            return _num.ToString();
        }
        else if (10000 <= _num && _num < 100000000)
        {
            str = $"{Math.Floor((_num / 10000f))}万";
        }
        else
        {
            str = $"{Math.Floor((_num / 100000000f))}亿";
        }

        return str;
    }
    public static string GetItemBaseCount(long _num)
    {
        //在没有特殊要求得情况下
        //默认万位以下显示具体数值
        //到达十万显示 10.5万,一位小数
        //到达百万以上显示102.5万 一位小数 依次

        string str;
        if (_num < 10000)
        {
            return _num.ToString();
        }
        else if (10000 <= _num && _num < 100000)
        {
            str = $"{Math.Round(Convert.ToDecimal(_num * 0.0001f), 1, MidpointRounding.AwayFromZero)}万";
        }
        else if (100000 <= _num && _num < 1000000)
        {
            str = $"{Math.Round(Convert.ToDecimal(_num * 0.0001f), 1, MidpointRounding.AwayFromZero)}万";
        }
        else if (1000000 <= _num && _num < 10000000)
        {
            str = $"{Math.Round(Convert.ToDecimal(_num * 0.0001f), 1, MidpointRounding.AwayFromZero)}万";
        }
        else if (10000000 <= _num && _num < 100000000)
        {
            str = $"{Math.Round(Convert.ToDecimal(_num * 0.0001f), 1, MidpointRounding.AwayFromZero)}万";
        }
        else
        {
            str = $"{Math.Round(Convert.ToDecimal(_num * 0.00000001f), 1, MidpointRounding.AwayFromZero)}亿";
        }
        return str;
    }
    //十万,十亿判断,转化用
    public static string GetDecimalTenThousandValue(long _num, string _deil)
    {
        float result;
        string str;
        if (_num < 100000)
        {
            return _num.ToString();
        }
        else if (100000 <= _num && _num < 1000000000)
        {
            result = float.Parse((_num / 10000f).ToString(_deil));
            str = string.Concat(result, "万");
        }
        else
        {
            result = float.Parse((_num / 100000000f).ToString(_deil));
            str = string.Concat(result, "亿");
        }

        return str;
    }
    public static string GetDecimalTenThousandValue(long _num)
    {
        string str;
        if (_num < 100000)
        {
            return _num.ToString();
        }
        else if (100000 <= _num && _num < 1000000000)
        {
            str = $"{Math.Floor((_num / 10000f))}万";
        }
        else
        {
            str = $"{Math.Floor((_num / 100000000f))}亿";
        }

        return str;
    }



    #region CronTime
    /// <summary>
    /// 将cron字符串转成时分秒的int(如：201500即20：15：00)
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int CronTimeStringParseToHMS(string str)
    {
        if (string.IsNullOrEmpty(str)) return 0;

        string[] timeArr = str.Split(' ');
        int s = 0;
        if (timeArr.Length > 0) int.TryParse(timeArr[0], out s);
        int m = 0;
        if (timeArr.Length > 1) int.TryParse(timeArr[1], out m);
        int h = 0;
        if (timeArr.Length > 2) int.TryParse(timeArr[2], out h);


        return h * 10000 + m * 100 + s;
    }

    /// <summary>
    /// 将cron字符串解析出时分秒的int值
    /// </summary>
    /// <param name="str"></param>
    /// <param name="hour"></param>
    /// <param name="minute"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static void CronTimeStringParseToHMS(string str, out int hour, out int minute, out int second)
    {
        hour = 0;
        minute = 0;
        second = 0;
        if (string.IsNullOrEmpty(str)) return;
        string[] timeArr = str.Split(' ');
        if (timeArr.Length > 0) int.TryParse(timeArr[0], out second);
        if (timeArr.Length > 1) int.TryParse(timeArr[1], out minute);
        if (timeArr.Length > 2) int.TryParse(timeArr[2], out hour);
    }

    /// <summary>
    /// 将cron字符串转成时分格式的int
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int CronTimeStringParseToHM(string str)
    {
        if (string.IsNullOrEmpty(str)) return 0;

        string[] timeArr = str.Split(' ');
        int m = 0;
        if (timeArr.Length > 1) int.TryParse(timeArr[1], out m);
        int h = 0;
        if (timeArr.Length > 2) int.TryParse(timeArr[2], out h);


        return h * 100 + m;
    }

    /// <summary>
    /// 将cron字符串转成时分字符串，如20:00
    /// </summary>
    /// <param name="str"></param>
    public static string CronTimeStringParseToHMString(string str)
    {
        if (string.IsNullOrEmpty(str)) return "00:00";

        string[] timeArr = str.Split(' ');
        string hour = timeArr.Length > 2 ? timeArr[2] : "00";
        string minute = timeArr.Length > 1 ? timeArr[1] : "00";

        hour = int.Parse(hour) < 10 ? $"0{hour}" : hour;
        minute = int.Parse(minute) < 10 ? $"0{minute}" : minute;

        return $"{hour}:{minute}";
    }

    /// <summary>
    /// 将int格式的时分秒转成字符串。如161555转成16:15:55
    /// </summary>
    /// <param name="hmsInt">时分秒格式，即5位或6位的int</param>
    /// <param name="needSecond">是否需要输出秒位字符</param>
    /// <returns></returns>
    public static string TimeIntToString(int hmsInt, bool needSecond = false)
    {
        string str = "";
        int h = Mathf.FloorToInt(hmsInt / 10000);
        int m = Mathf.FloorToInt((hmsInt - h * 10000) / 100);
        int s = Mathf.FloorToInt(hmsInt - h * 10000 - m * 100);
        string hStr = h < 10 ? $"0{h}" : h.ToString();
        string mStr = m < 10 ? $"0{m}" : m.ToString();
        string sStr = s < 10 ? $"0{s}" : s.ToString();
        str = string.Format("{0}:{1}{2}", hStr, mStr, needSecond ? $":{sStr}" : "");

        return str;
    }

    /// <summary>
    /// 将cron字符串转成时时间戳
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static long CronTimeStringParseToTamp(string str)
    {
        if (string.IsNullOrEmpty(str)) return 0;

        string[] timeArr = str.Split(' ');
        int s = 0;
        if (timeArr.Length > 0) int.TryParse(timeArr[0], out s);
        int m = 0;
        if (timeArr.Length > 1) int.TryParse(timeArr[1], out m);
        int h = 0;
        if (timeArr.Length > 2) int.TryParse(timeArr[2], out h);

        DateTime dateTime = CSServerTime.Instance.ServerNows;
        DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, h, m, s);

        return CSServerTime.DateTimeToStamp(dateTime2);
    }
    #endregion

    /// <summary>
    /// 通过buffer表配置的公式获取buffer属性值(仅用于属性buff)
    /// </summary>
    /// <param name="bufferId"></param>
    /// <param name="level">角色等级对应公式中的X</param>
    /// <param name="targetAttribute">目标属性对应公式中的Y</param>
    /// <returns></returns>
    public static float GetBufferValue(int bufferId)
    {
        TABLE.BUFFER tblBuff = BufferTableManager.Instance[bufferId];
        if (tblBuff == null)
        {
            return 0;
        }
        List<int> list = UtilityMainMath.SplitStringToIntList(tblBuff.attBuff);
        if (list.Count < 2)
        {
            return 0;
        }
        if (list[1] != 0)
        {
            return list[1];
        }
        if (string.IsNullOrEmpty(tblBuff.formula))
        {
            return 0;
        }
        string formula = tblBuff.formula;
        int targetAttribute = CSMainPlayerInfo.Instance.GetMyAttrById(list[0]);
        Regex regex = new Regex("X", RegexOptions.IgnoreCase);
        formula = regex.Replace(formula, CSMainPlayerInfo.Instance.Level.ToString());
        regex = new Regex("Y", RegexOptions.IgnoreCase);
        formula = regex.Replace(formula, targetAttribute.ToString());
        var result = new DataTable().Compute(formula, "");
        return Convert.ToSingle(result);
    }


    /// <summary>
    /// 小数四舍五入
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int GetRoundingInt(float value)
    {
        int floor = Mathf.FloorToInt(value);
        if (value - floor < 0.5f)
        {
            return floor;
        }
        else
        {
            return floor + 1;
        }
    }


    public static int GetTenMultiple(int _value)
    {
        int remainder = _value % 10;
        int mul = _value / 10;
        if (remainder == 0)
        {
            return (mul - 1) <= 0 ? 1 : (mul - 1) * 10;
        }
        return mul * 10;
    }
}
