using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ChongZhuCostTableManager : TableManager<TABLE.CHONGZHUCOSTARRAY, TABLE.CHONGZHUCOST, int, ChongZhuCostTableManager>
{
    public TABLE.CHONGZHUCOST GetCfg(int id, int _levClass)
    {
        var arr = array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as TABLE.CHONGZHUCOST;
            if (item.id == id && item.levclass == _levClass)
            {
                return item;
            }
        }
        return null;
    }
}
