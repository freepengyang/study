using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class XiLianCostTableManager : TableManager<TABLE.XILIANCOSTARRAY, TABLE.XILIANCOST, int, XiLianCostTableManager>
{
    //protected override void OnResourceLoaded(CSResourceWWW res)
    //{
    //    base.OnResourceLoaded(res);

    //    if (array != null)
    //    {
    //        for (int i = 0; i < array.rows.Count; i++)
    //        {
    //            AddTables(array.rows[i].level, array.rows[i]);
    //        }
    //    }
    //    base.OnDealOver();
    //}

    public TABLE.XILIANCOST GetCfg(int _level)
    {
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.XILIANCOST;
            if (item.level == _level)
            {
                return item;
            }
        }
        return null;
    }
}
