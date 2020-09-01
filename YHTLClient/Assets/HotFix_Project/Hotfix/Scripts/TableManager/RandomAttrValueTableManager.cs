using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class RandomAttrValueTableManager : TableManager<TABLE.RANDOMATTRVALUEARRAY, TABLE.RANDOMATTRVALUE, int, RandomAttrValueTableManager>
{
    
    public int  GetItemCfg(int _level,int _type,int _param)
    {
        int maxValue = 0 ;
        TABLE.RANDOMATTRVALUE value;
        int key = (_level * 100) + _type + (_param * 100000);
        if (TryGetValue(key, out value))
        {
            return (int)value.MaxValue;
        }
        return maxValue;
    }

    public int GetIsShow(int _level, int _type, int _param)
    {
        int show = 0;
        TABLE.RANDOMATTRVALUE value;
        int key = (_level * 100) + _type + (_param * 100000);
        if (TryGetValue(key, out value))
        {
            return (int)value.show;
        }
        return show;
    }

}
