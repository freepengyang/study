
using System.Collections.Generic;

public partial class SundryTableManager : TableManager<TABLE.SUNDRYARRAY, TABLE.SUNDRY, int, SundryTableManager>
{
    //protected override void OnResourceLoaded(CSResourceWWW res)
    //{
    //    base.OnResourceLoaded(res);
    //    if (array != null)
    //        for (int i = 0; i < array.rows.Count; i++)
    //            AddTables(array.rows[i].id, array.rows[i]);
    //    base.OnDealOver();
    //    IntiData();
    //}

    List<int> MapList = new List<int>();

    public void IntiData()
    {
        TABLE.SUNDRY tbl_sundry = SundryTableManager.Instance[248];

        if (tbl_sundry == null) return;

        string[] strs = tbl_sundry.effect.Split('#');
        int num = 0;
        for (int i = 0; i < strs.Length; i++)
        {
            if (int.TryParse(strs[i], out num))
            {
                if (!MapList.Contains(num))
                {
                    MapList.Add(num);
                }
            }
        }
    }

    public bool GetMapId(int mapid)
    {
        return MapList.Contains(mapid);
    }

    public static int GetValue(int id)
    {
        TABLE.SUNDRY table;
        int result = 0;
        if (Instance.TryGetValue(id, out table))
        {
            int.TryParse(table.effect, out result);
        }

        return result;
    }
    public string GetDes(int id)
    {
        TABLE.SUNDRY table;
        if (Instance.TryGetValue(id, out table))
        {
            return table.effect;
        }
        return "";
    }

    public static int[] GetArrayValue(int id)
    {
        TABLE.SUNDRY table;
        if (Instance.TryGetValue(id, out table))
        {
            string[] splits = table.effect.Split('#');
            int[] result = new int[splits.Length];
            for (int i = 0; i < splits.Length; i++)
            {
                int value;
                if (int.TryParse(splits[i], out value))
                {
                    result[i] = value;
                }
            }
            return result;
        }
        return null;
    }
}
