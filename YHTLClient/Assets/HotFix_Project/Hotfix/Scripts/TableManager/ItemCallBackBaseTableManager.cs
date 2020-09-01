using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ItemCallBackBaseTableManager : TableManager<TABLE.ITEMCALLBACKBASEARRAY, TABLE.ITEMCALLBACKBASE, int, ItemCallBackBaseTableManager>
{
    /// <summary>
    /// type 1普通  2卧龙
    /// </summary>
    /// <param name="_type"></param>
    public void GetItemList(int _type, List<int> _itemIdList)
    {
        _itemIdList.Clear();
        var arr = array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var value = arr[i].Value as TABLE.ITEMCALLBACKBASE;
            if (value.type == _type && value.unlock == 0)
            {
                _itemIdList.Add(value.item);
            }
        }
    }
}
