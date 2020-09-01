using System.Collections.Generic;

public class UtilityMainMath
{

    /// <summary>
    /// 字符串被某个个分隔符分成字符串数组
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="divide">分隔符</param>
    /// <returns></returns>
    public static string[] StrToStrArr(string str, char divide = '#')
    {
        if (string.IsNullOrEmpty(str)) return null;
        string[] content = null;
        content = str.Split(divide);
        return content;
    }

    //用特定字符切分string  返回list
    public static List<int> SplitStringToIntList(string str, char split = '#')
    {
        List<int> list = new List<int>();
        if (null != str)
        {
            string[] strList = str.Split(split);
            int temp;
            for (int i = 0; i < strList.Length; i++)
            {
                if (int.TryParse(strList[i], out temp)) list.Add(temp);
            }
        }

        return list;
    }
    public static List<long> SplitStringToLongList(string str, char split = '#')
    {
        List<long> list = new List<long>();
        if (null != str)
        {
            string[] strList = str.Split(split);
            long temp;
            for (int i = 0; i < strList.Length; i++)
            {
                if (long.TryParse(strList[i], out temp)) list.Add(temp);
            }
        }

        return list;
    }
    public static void SplitStringToIntList(List<int> _list, string str, char split = '#')
    {
        _list.Clear();
        if (null != str)
        {
            string[] strList = str.Split(split);
            int temp;
            for (int i = 0; i < strList.Length; i++)
            {
                if (int.TryParse(strList[i], out temp)) _list.Add(temp);
            }
        }
    }
    public static List<List<int>> SplitStringToIntLists(string str, char split1 = '&', char split2 = '#')
    {
        List<List<int>> list = new List<List<int>>();
        if (!string.IsNullOrEmpty(str))
        {
            string[] strList = str.Split(split1);
            for (int i = 0; i < strList.Length; i++)
            {
                list.Add(SplitStringToIntList(strList[i], split2));
            }
        }

        return list;
    }

    public static Dictionary<int, int> SplitStringToIntDic(string str, char split1 = '&', char split2 = '#')
    {
        Dictionary<int, int> dic = new Dictionary<int, int>();
        if (!string.IsNullOrEmpty(str))
        {
            string[] strList = str.Split(split1);
            for (int i = 0; i < strList.Length; i++)
            {
                if(!string.IsNullOrEmpty(strList[i]))
                {
                    string[] strs = strList[i].Split(split2);
                    if (strs.Length > 1)
                    {
                        int key = 0;
                        int value = 0;
                        if (int.TryParse(strs[0], out key) && int.TryParse(strs[1], out value))
                        {
                            if (!dic.ContainsKey(key))
                            {
                                dic.Add(key, value);
                            }
                        }
                    }
                }
            }
        }
        return dic;

    }

    /// <summary>
    ///   sundry表中double类型的数值  比如1.5d
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static float GetSundryFloatValue(string str)
    {
        string[] temp_result = str.Split('d');
        float d;
        if (float.TryParse(temp_result[0], out d))
        {
            return d;
        }
        return 0;
    }
}