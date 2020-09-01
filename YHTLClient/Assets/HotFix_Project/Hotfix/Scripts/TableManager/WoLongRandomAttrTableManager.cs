using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class WoLongRandomAttrTableManager : TableManager<TABLE.WOLONGRANDOMATTRARRAY, TABLE.WOLONGRANDOMATTR, int, WoLongRandomAttrTableManager>
{
    public TABLE.WOLONGRANDOMATTR GetCfgById(int _id)
    {
        TABLE.WOLONGRANDOMATTR cfg = null;
        if (TryGetValue(_id, out cfg))
        {
            return cfg;
        }
        else
        {
            return null;
        }
    }
}
