using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSundryData 
{
    private static Dictionary<int, int> mCanCrossServerDayDic = null;
    public static bool IsCanCrossServerDay(int mapId, int serverOpenDay)
    {
        if (mCanCrossServerDayDic == null)
        {
            mCanCrossServerDayDic = SplitToIntDic(201);
        }
        if (mCanCrossServerDayDic.ContainsKey(mapId))
        {
            return mCanCrossServerDayDic[mapId] <= serverOpenDay;
        }
        return true;
    }

    public static Dictionary<int, int> SplitToIntDic(int sundryId)
    {
        Dictionary<int, int> dic = new Dictionary<int, int>();
        TABLE.SUNDRY tblSundry = null;
        if (SundryTableManager.Instance.TryGetValue(sundryId, out tblSundry))
        {
            string[] strs = tblSundry.effect.Split('#');
            for (int i = 0, iMax = strs.Length; i < strs.Length; i += 2)
            {
                if (i + 1 < iMax)
                {
                    if (int.TryParse(strs[i], out int key) && int.TryParse(strs[i + 1], out int val))
                    {
                        if (!dic.ContainsKey(key))
                        {
                            dic.Add(key, val);
                        }
                    }
                }
            }
        }
        return dic;
    }

    public static List<int> SplitIntToList(int sundryId)
    {
        List<int> list = null;
        TABLE.SUNDRY tblSundry = SundryTableManager.Instance[sundryId];
        if(tblSundry == null)
        {
            list = new List<int>();
            return list;
        }
        list = UtilityMainMath.SplitStringToIntList(tblSundry.effect);
        return list;
    }


}
