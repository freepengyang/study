using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class RandomAttrTableManager : TableManager<TABLE.RANDOMATTRARRAY, TABLE.RANDOMATTR, int, RandomAttrTableManager>
{
    //public int GetRandomAttrEntryQuality(int id, int defaultValue = default(int))
    //{
    //    TABLE.RANDOMATTR cfg = null;
    //    if (TryGetValue(id, out cfg))
    //    {
    //        return cfg.Type;
    //    }
    //    else
    //    {
    //        return defaultValue;
    //    }
    //}
    //protected override void OnResourceLoaded(CSResourceWWW res)
    //{
    //    base.OnResourceLoaded(res);

    //    if (array != null)
    //    {
    //        for (int i = 0; i < array.rows.Count; i++)
    //        {
    //            AddTables(array.rows[i].id, array.rows[i]);
    //        }
    //    }
    //    base.OnDealOver();
    //}

    //public Dictionary<int, TABLE.RANDOMATTR> GetItemCfgListByQuality(int _quality)
    //{
    //    Dictionary<int, TABLE.RANDOMATTR> list = new Dictionary<int, TABLE.RANDOMATTR>();
    //    TABLE.RANDOMATTR value;
    //    int i = 0;
    //    var iter = dic.GetEnumerator();
    //    while (iter.MoveNext())
    //    {
    //        if (iter.Current.Value.Quality == _quality && !list.ContainsKey((int)iter.Current.Value.parameter))
    //        {
    //            list.Add((int)iter.Current.Value.parameter, iter.Current.Value);
    //        }
    //        i++;
    //    }

    //    return list;
    //}

    //public int GetItemCfgListByQuality(int _subtype, int _type, int _para)
    //{
    //    var iter = dic.GetEnumerator();
    //    while (iter.MoveNext())
    //    {
    //        if (iter.Current.Value.Quality == _subtype && iter.Current.Value.type == _type && iter.Current.Value.parameter == _para)
    //        {
    //            return iter.Current.Value.entryQuality;
    //        }
    //    }
    //    return 0;
    //}
}
