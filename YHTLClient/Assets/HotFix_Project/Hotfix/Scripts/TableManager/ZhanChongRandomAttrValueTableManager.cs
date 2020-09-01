using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ZhanChongRandomAttrValueTableManager : TableManager<TABLE.ZHANCHONGRANDOMATTRVALUEARRAY, TABLE.ZHANCHONGRANDOMATTRVALUE, int, ZhanChongRandomAttrValueTableManager>
{
    public int GetWoLongAttrMaxValue(int _levClass, int _type, int _param)
    {
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.ZHANCHONGRANDOMATTRVALUE;
            if (item.levClass == _levClass && item.type == _type && item.parameter == _param)
            {
                return item.attrValue;
            }
        }
        return 0;
    }
}
