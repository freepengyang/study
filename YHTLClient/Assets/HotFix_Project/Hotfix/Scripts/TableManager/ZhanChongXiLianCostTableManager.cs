using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ZhanChongXiLianCostTableManager : TableManager<TABLE.ZHANCHONGXILIANCOSTARRAY, TABLE.ZHANCHONGXILIANCOST, int, ZhanChongXiLianCostTableManager>
{
    public string GetCostByLevel(int _levClass, int _type)
    {
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.ZHANCHONGXILIANCOST;
            if (item.levClass == _levClass)
            {
                if (_type == 1)
                {
                    return item.hunlicost;
                }
                else if (_type == 2)
                {
                    return item.hunjicost;
                }
                else if (_type == 3)
                {
                    return item.hunzhicost;
                }
            }
        }
        return "";
    }
}
