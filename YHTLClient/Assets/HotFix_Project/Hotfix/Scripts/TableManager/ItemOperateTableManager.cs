using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ItemOperateTableManager : TableManager<TABLE.ITEMOPERATEARRAY, TABLE.ITEMOPERATE, int, ItemOperateTableManager>
{


    public string GetBtnsList(int _itemType, int _subType, int _Operationtype)
    {
        TABLE.ITEMOPERATE itemTb;
        if (TryGetValue((_itemType * 100 + _subType * 10000 + _Operationtype), out itemTb))
        {
            if (itemTb != null)
            {
                return itemTb.leftOperate;
            }
        }
        return "";
    }

    public string GetBtnsColorList(int _itemType, int _subType, int _Operationtype)
    {
        TABLE.ITEMOPERATE itemTb;
        if (TryGetValue((_itemType * 100 + _subType * 10000 + _Operationtype), out itemTb))
        {
            if (itemTb != null)
            {
                return itemTb.rightOperate;
            }
        }
        return "";
    }
    public int GetOperaTypeForBatchUse(int _itemType, int _subType, int _Operationtype)
    {
        TABLE.ITEMOPERATE itemTb;
        if (TryGetValue((_itemType * 100 + _subType * 10000 + _Operationtype), out itemTb))
        {
            if (itemTb != null)
            {
                return (int)itemTb.Operationtype;
            }
        }
        return 0;
    }
}
