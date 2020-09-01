public partial class SpecialEquipTableManager : TableManager<TABLE.SPECIALEQUIPARRAY, TABLE.SPECIALEQUIP, int, SpecialEquipTableManager>
{
    public bool IsSpecialEquip(int _id)
    {
        return array.gItem.id2offset.ContainsKey(_id);
    }
}