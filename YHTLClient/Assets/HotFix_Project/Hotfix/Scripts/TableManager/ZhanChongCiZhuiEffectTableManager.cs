

public partial class ZhanChongCiZhuiEffectTableManager : TableManager<TABLE.ZHANCHONGCIZHUIEFFECTARRAY, TABLE.ZHANCHONGCIZHUIEFFECT, int, ZhanChongCiZhuiEffectTableManager>
{
    public TABLE.ZHANCHONGCIZHUIEFFECT GetDataByType(int _id)
    {
        TABLE.ZHANCHONGCIZHUIEFFECT eff;
        if (TryGetValue(_id, out eff))
        {
            return eff;
        }
        else
        {
            return null;
        }
    }
}
