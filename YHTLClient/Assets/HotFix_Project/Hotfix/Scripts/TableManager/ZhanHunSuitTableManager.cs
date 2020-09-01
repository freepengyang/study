public partial class ZhanHunSuitTableManager : TableManager<TABLE.ZHANHUNSUITARRAY, TABLE.ZHANHUNSUIT, int, ZhanHunSuitTableManager>
{
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

    public string GetItemName(int suitId)
    {
        TABLE.ZHANHUNSUIT suitTb;
        if (TryGetValue(suitId, out suitTb))
        {
            if (suitTb != null)
            {
                return suitTb.name;
            }
        }
        return "";
    }

    public TABLE.ZHANHUNSUIT GetSuitCfg(int suitId)
    {
        TABLE.ZHANHUNSUIT suitTb;
        if (TryGetValue(suitId, out suitTb))
        {
            return suitTb;
        }
        return null;
    }
}
