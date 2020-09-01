using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class WoLongLevelTableManager
{
    public int GetMaxId()
    {
        return array.gItem.id2offset.Count - 1;
    }

    public int GetCostByWoLongLevel(int _lv)
    {
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.WOLONGLEVEL;
            if (item.woLongLevel == _lv)
            {
                return item.woLongSoul;
            }
        }
        return 0;
    }
}