using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class PaoDianTableManager : TableManager<TABLE.PAODIANARRAY, TABLE.PAODIAN, int, PaoDianTableManager>
{
    public List<int> GetPaoDianInfoByType(int _type)
    {
        List<int> list = new List<int>();

        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.PAODIAN;
            if (item.type == _type)
            {
                list.Add(item.id);
            }
        }
        return list;
    }
}
