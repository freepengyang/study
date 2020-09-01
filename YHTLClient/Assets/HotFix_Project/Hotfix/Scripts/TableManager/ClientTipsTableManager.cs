using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ClientTipsTableManager : TableManager<TABLE.CLIENTTIPSARRAY, TABLE.CLIENTTIPS, int, ClientTipsTableManager>
{
    //protected override void OnResourceLoaded(CSResourceWWW res)
    //{
    //    base.OnResourceLoaded(res);
    //    if (array != null)
    //        for (int i = 0; i < array.rows.Count; i++)
    //            AddTables((int)array.rows[i].id, array.rows[i]);
    //    base.OnDealOver();
    //}
    public static string GetContext(uint id)
    {
        TABLE.CLIENTTIPS tb;
        return Instance.TryGetValue((int)id, out tb) ? tb.context : "";
    }
}
