using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ArmsRaceTaskTableManager : TableManager<TABLE.ARMSRACETASKARRAY, TABLE.ARMSRACETASK, int, ArmsRaceTaskTableManager>
{
    public void GetTasksByGroup(int _group, List<TABLE.ARMSRACETASK> list)
    {
        list.Clear();
        var arr = array.gItem.handles;
        for(int i = 0,max = arr.Length;i < max;++i)
        {
            var item = arr[i].Value as TABLE.ARMSRACETASK;
            if (item.group == _group)
            {
                list.Add(item);
            }
        }
    }
}
