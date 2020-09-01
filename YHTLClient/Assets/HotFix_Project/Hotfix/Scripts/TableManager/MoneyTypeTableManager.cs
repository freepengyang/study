using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class MoneyTypeTableManager : TableManager<TABLE.MONEYTYPEARRAY, TABLE.MONEYTYPE, int, MoneyTypeTableManager>
{
    public TABLE.MONEYTYPE GetCfg(int id)
    {
        TABLE.MONEYTYPE cfg = null;
        if (TryGetValue(id, out cfg))
        {
            return cfg;
        }
        else
        {
            return null;
        }
    }
}
