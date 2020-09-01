using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class GiftBagTableManager : TableManager<TABLE.GIFTBAGARRAY, TABLE.GIFTBAG, int, GiftBagTableManager>
{
    public int GetArmIdByGroupId(int _groupId)
    {
        List<int> list = new List<int>();
        var arr = array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as TABLE.GIFTBAG;
            if (item.type == 1)
            {
                list.Add(item.id);
            }
        }
        return list[_groupId];
    }
}
