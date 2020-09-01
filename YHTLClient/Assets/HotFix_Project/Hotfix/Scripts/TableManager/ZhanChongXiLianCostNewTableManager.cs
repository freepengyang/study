using System.Collections.Generic;
using Google.Protobuf.Collections;
public partial class ZhanChongXiLianCostNewTableManager : TableManager<TABLE.ZHANCHONGXILIANCOSTNEWARRAY, TABLE.ZHANCHONGXILIANCOSTNEW, int, ZhanChongXiLianCostNewTableManager>
{
    public IntArray GetCostByLevClass(int _levClass,int _type)
    {
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.ZHANCHONGXILIANCOSTNEW;
            if (item.levClass == _levClass)
            {
                if (_type == 1)
                {
                    return item.hunjicost;
                }
                else if (_type == 2)
                {
                    return item.hunjicost1;
                }
                else if (_type == 3)
                {
                    return item.hunlicost;
                }
                else if (_type == 4)
                {
                    return item.hunlicost1;
                }
            }
        }
        return IntArray.Default;
    }

    public TABLE.ZHANCHONGXILIANCOSTNEW GetCfg(int _levClass)
    {
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.ZHANCHONGXILIANCOSTNEW;
            if (item.levClass == _levClass)
            {
                return item;
            }
        }
        return null;
    }
}
