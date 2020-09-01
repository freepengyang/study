using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class EnergyExchangeTableManager
{
    public TABLE.ENERGYEXCHANGE GetSingleCfgByTimes(int _times)
    {
        var arr = array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as TABLE.ENERGYEXCHANGE;
            if (item.times == _times)
            {
                return item;
            }
        }
        return null;
    }
}
