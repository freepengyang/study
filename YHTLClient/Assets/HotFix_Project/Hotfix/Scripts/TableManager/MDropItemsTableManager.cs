using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TABLE;

public partial class MDropItemsTableManager : TableManager<TABLE.MDROPITEMSARRAY, TABLE.MDROPITEMS, int, MDropItemsTableManager>
{
    List<List<int>> dropLvType;
    IntArray[] dropItems = new IntArray[6];
    public void GetDropItemsByMonsterId(int _monsterId, CSBetterLisHot<int> _list, int getCount = 0)
    {
        TABLE.MONSTERINFO monsterInfo;
        if (MonsterInfoTableManager.Instance.TryGetValue(_monsterId, out monsterInfo))
        {
            int lvType = 0;
            int lv = (int)monsterInfo.level;
            if (monsterInfo.PropertiesSuit == 1)
            {
                lv = CSSealGradeInfo.Instance.MySealLevel;
                lvType = GetLvType(lv) + 1;
            }
            else if (monsterInfo.PropertiesSuit == 3)
            {
                lv = CSMainPlayerInfo.Instance.Level;
                lvType = GetLvType(lv) + 1;
            }
            else
            {
                lvType = 1;
            }
            int id = _monsterId + (lvType << 28);
            TABLE.DROPSHOW drop;
            if (DropShowTableManager.Instance.TryGetValue(id, out drop))
            {
                int sex = CSMainPlayerInfo.Instance.Sex;
                int career = CSMainPlayerInfo.Instance.Career;
                int idx = sex * 3 + career - 1;
                dropItems[0] = drop.itemId0;
                dropItems[1] = drop.itemId1;
                dropItems[2] = drop.itemId2;
                dropItems[3] = drop.itemId3;
                dropItems[4] = drop.itemId4;
                dropItems[5] = drop.itemId5;
                var dropItemsArray = dropItems[idx];
                for (int i = 0,max = dropItemsArray.Count; i < max && (getCount <= 0 || i < getCount); i++)
                {
                    _list.Add(dropItemsArray[i]);
                }
                //_list.Sort((a, b) =>
                //{
                //    TABLE.ITEM a_cfg = ItemTableManager.Instance.GetItemCfg(a);
                //    TABLE.ITEM b_cfg = ItemTableManager.Instance.GetItemCfg(b);

                //    bool a_equip = a_cfg.type == 2;
                //    bool b_equip = b_cfg.type == 2;
                //    if (a_equip != b_equip)
                //    {
                //        return a_equip ? -1 : 1;
                //    }

                //    if (a_equip)
                //    {
                //        bool a_wolong = CSBagInfo.Instance.IsWoLongEquip(a_cfg);
                //        bool b_wolong = CSBagInfo.Instance.IsWoLongEquip(b_cfg);
                //        if (a_wolong != b_wolong)
                //        {
                //            return a_wolong ? -1 : 1;
                //        }
                //    }

                //    if (a_cfg.quality != b_cfg.quality)
                //    {
                //        return b_cfg.quality - a_cfg.quality;
                //    }

                //    if (a_equip)
                //    {
                //        bool wolong = CSBagInfo.Instance.IsWoLongEquip(a_cfg);
                //        if (wolong)
                //        {
                //            if (a_cfg.wolongLv != b_cfg.wolongLv)
                //                return b_cfg.wolongLv - a_cfg.wolongLv;
                //        }

                //        if (a_cfg.level != b_cfg.level)
                //            return b_cfg.level - a_cfg.level;
                //    }

                //    return a_cfg.id < b_cfg.id ? -1 : (a_cfg.id == b_cfg.id ? 0 : 1);
                //});

                //if (_list.Count > getCount && getCount > 0)
                //{
                //    _list.RemoveRange(getCount);
                //}
            }
        }
    }
    int GetLvType(int _lv)
    {
        if (dropLvType == null)
        {
            dropLvType = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(1012));
        }
        for (int i = 0; i < dropLvType.Count; i++)
        {
            if (dropLvType[i][0] <= _lv && _lv <= dropLvType[i][1])
            {
                return i;
            }
        }
        return 0;
    }
}

