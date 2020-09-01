using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class JingjiHuodongjiangliTableManager : TableManager<TABLE.JINGJIHUODONGJIANGLIARRAY, TABLE.JINGJIHUODONGJIANGLI, int, JingjiHuodongjiangliTableManager>
{
    public void GetJingJiTasks(int _type, ILBetterList<TABLE.JINGJIHUODONGJIANGLI> _list)
    {
        if (_list == null) { return; }
        _list.Clear();
        var arr = array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as TABLE.JINGJIHUODONGJIANGLI;
            if (item.jingjiHuodongId == _type)
            {
                _list.Add(item);
            }
        }
    }
}
