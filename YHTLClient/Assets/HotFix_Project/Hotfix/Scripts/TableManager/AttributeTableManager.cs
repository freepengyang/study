

public partial class AttributeTableManager : TableManager<TABLE.ATTRIBUTEARRAY, TABLE.ATTRIBUTE, int, AttributeTableManager>
{
    public static uint GetTipId(uint id)
    {
        TABLE.ATTRIBUTE value;
        return Instance.TryGetValue((int)id, out value) ? value.tipID : 0;
    }

    public static int GetPensentValue(uint id)
    {
        TABLE.ATTRIBUTE value;
        return Instance.TryGetValue((int)id, out value) ? value.per : 0;
    }
}
