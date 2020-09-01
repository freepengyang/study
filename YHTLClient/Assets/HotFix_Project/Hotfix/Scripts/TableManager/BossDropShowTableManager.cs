using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class BossDropShowTableManager : TableManager<TABLE.BOSSDROPSHOWARRAY, TABLE.BOSSDROPSHOW, int, BossDropShowTableManager>
{
    public int GetDropIdByMonsterId(int _monsterId,int _sealLv)
    {
        var arr = array.gItem.handles;
        for(int i = 0,max = arr.Length;i < max;++i)
        {
            var item = arr[i].Value as TABLE.BOSSDROPSHOW;
            if (item.mid == _monsterId && (item.min <= _sealLv && _sealLv <= item.max))
            {
                return item.groupId;
            }
        }
        return 0;
    }
}
