using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class LevelTableManager : TableManager<TABLE.LEVELARRAY, TABLE.LEVEL, int, LevelTableManager>
{
    //protected override void OnResourceLoaded(CSResourceWWW res)
    //{
    //    base.OnResourceLoaded(res);

    //    if (array != null)
    //    {
    //        for (int i = 0; i < array.rows.Count; i++)
    //        {
    //            AddTables(array.rows[i].id, array.rows[i]);
    //        }
    //    }
    //    base.OnDealOver();
    //}

    public long GetExpByLevel(int _level)
    {
        TABLE.LEVEL itemTb;
        long result = 0;
        if (TryGetValue(_level, out itemTb))
        {
            if (itemTb != null)
            {
                long.TryParse(itemTb.upgrade, out result);
            }
        }
        return result;
    }
}
