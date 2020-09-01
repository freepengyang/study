using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class XiLianListShowTableManager : TableManager<TABLE.XILIANLISTSHOWARRAY, TABLE.XILIANLISTSHOW, int, XiLianListShowTableManager>
{
    public List<TABLE.XILIANLISTSHOW> GetCurrentData(int _pos, int _lvClass,int _type)
    {
        List<TABLE.XILIANLISTSHOW> list = new List<TABLE.XILIANLISTSHOW>();
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.XILIANLISTSHOW;
            if (item.subType == _pos && item.levClass == _lvClass && item.type == _type)
            {
                list.Add(item);
            }
        }
        return list;
    }
}
