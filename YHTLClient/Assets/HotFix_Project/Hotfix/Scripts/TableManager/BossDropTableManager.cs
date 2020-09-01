public partial class BossDropTableManager : TableManager<TABLE.BOSSDROPARRAY, TABLE.BOSSDROP, int, BossDropTableManager>
{
    public TABLE.BOSSDROP GetBossDropCfg(int _id)
    {
        var arr = array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as TABLE.BOSSDROP;
            if (item.mid == _id)
            {
                return item;
            }
        }
        return null;
    }
}
