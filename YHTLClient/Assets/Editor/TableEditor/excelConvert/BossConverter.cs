using bag;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TABLE;
using UnityEngine;
namespace Smart.Editor
{
    public enum OptionType
    {
        OptionRepeated = 1,
        OptionRequired = 2,
    }
    public enum VarOptionType
    {
        VarTypeSint32 = 1,
        VarTypeString = 2,
    }

    public interface IConverter
    {
        void Run();
    }

    //public abstract class Converter<A> : IConverter<A> where A : ILFastMode, new()
    //{
    //    protected string sheetName;
    //    protected List<List<string>> mContents;

    //    public void Run(string sheetName)
    //    {
    //        this.sheetName = sheetName;
    //        ExcelCreater.Load<A>(a =>
    //        {
    //            mContents = new List<List<string>>(a.gItem.handles.Length);
    //            for (int i = 0, max = a.gItem.handles.Length; i < max; ++i)
    //            {
    //                var item = a.gItem.handles[i].Value as TABLE.ITEM;
    //                int coloum = 0;
    //                string contentValue = string.Empty;
    //                while (ConvertRowContent(ref coloum, ref contentValue, item))
    //                {
    //                    mContents[i].Add(contentValue);
    //                }
    //            }
    //        });
    //    }

    //    / <summary>
    //    / 这里实现表头定义
    //    / </summary>
    //    / <param name = "sheet" ></ param >
    //    public abstract void WriteHead(ISheet sheet);
    //    public abstract void WriteContent(ISheet sheet);

    //    public void Save()
    //    {
    //        var path = ExcelCreater.GetTableExcelPath(sheetName);
    //        if (System.IO.File.Exists(path))
    //            System.IO.File.Delete(path);

    //        IWorkbook workbook = new HSSFWorkbook();
    //        ISheet sheet = workbook.CreateSheet(sheetName);
    //        workbook.SetActiveSheet(0);
    //        if (null != sheet)
    //        {
    //            WriteHead(sheet);
    //            WriteContent(sheet);
    //        }

    //        using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
    //        {
    //            workbook.Write(fs);
    //        }
    //    }
    //}

    public class BossConverter : IConverter
    {
        TABLE.ITEMARRAY items;
        TABLE.MDROPITEMSARRAY dropItems;
        TABLE.MBOXARRAY boxItems;
        TABLE.BOSSDROPARRAY bossDropItems;
        TABLE.MONSTERINFOARRAY monsterItems;

        public void Run()
        {
            if(!ExcelCreater.Load<TABLE.ITEMARRAY, TABLE.MDROPITEMSARRAY, TABLE.MBOXARRAY, TABLE.BOSSDROPARRAY, TABLE.MONSTERINFOARRAY>((a, b, c, d,e) =>
             {
                 items = a;
                 dropItems = b;
                 boxItems = c;
                 bossDropItems = d;
                 monsterItems = e;
             }))
            {
                return;
            }
            UnityEngine.Debug.LogFormat("<color=#00ff00>[加载表格成功]</color>");
            BuildBigGroupIdMap();
            UnityEngine.Debug.LogFormat("<color=#00ff00>[构建MDROPITEMS字典成功]</color>");
            BuildGroupBoxMap();
            UnityEngine.Debug.LogFormat("<color=#00ff00>[构建MBOX字典成功]</color>");
            CollectValidDatas();
            UnityEngine.Debug.LogFormat("<color=#00ff00>[收集组数据成功]</color>");
            ExpressValidDatas();
            UnityEngine.Debug.LogFormat("<color=#00ff00>[组建组数据成功]</color>");
            SaveValidDatas();
            UnityEngine.Debug.LogFormat("<color=#00ff00>[导出表格数据成功]</color>");
            UnityEngine.Debug.LogFormat("<color=#ffff00>[友情提示]:[请记得先转依赖表]:ITEM|MDROPITEM|MBOX|BOSSDROP|MONSTERINFO</color>");
            UnityEngine.Debug.LogFormat("<color=#ffff00>[友情提示]:[请记得先转依赖表]:ITEM|MDROPITEM|MBOX|BOSSDROP|MONSTERINFO</color>");
            UnityEngine.Debug.LogFormat("<color=#ffff00>[友情提示]:[请记得先转依赖表]:ITEM|MDROPITEM|MBOX|BOSSDROP|MONSTERINFO</color>");
        }

        Dictionary<int, List<TABLE.MDROPITEMS>> mBigGroup2Items;
        void BuildBigGroupIdMap()
        {
            mBigGroup2Items = new Dictionary<int, List<TABLE.MDROPITEMS>>(dropItems.gItem.handles.Length);
            var handles = dropItems.gItem.handles;
            for (int i = 0,max = handles.Length; i < max; ++i)
            {
                var item = handles[i].Value as TABLE.MDROPITEMS;
                if (item.rate == 0 || item.count == 0)
                    continue;

                List<TABLE.MDROPITEMS> dropItems = null;
                if(!mBigGroup2Items.TryGetValue(item.bigGroupId,out dropItems))
                {
                    dropItems = new List<TABLE.MDROPITEMS>(16);
                    mBigGroup2Items.Add(item.bigGroupId,dropItems);
                }
                dropItems.Add(item);
            }
        }

        Dictionary<int, List<TABLE.MBOX>> mGroup2BoxItems;
        void BuildGroupBoxMap()
        {
            mGroup2BoxItems = new Dictionary<int, List<TABLE.MBOX>>(boxItems.gItem.handles.Length);
            var handles = boxItems.gItem.handles;
            for (int i = 0, max = handles.Length; i < max; ++i)
            {
                var item = handles[i].Value as TABLE.MBOX;
                List<TABLE.MBOX> boxItems = null;
                if (!mGroup2BoxItems.TryGetValue(item.group, out boxItems))
                {
                    boxItems = new List<TABLE.MBOX>(16);
                    mGroup2BoxItems.Add(item.group, boxItems);
                }
                boxItems.Add(item);
            }
        }

        class FinalExpressItem
        {
            public int id;
            public int levelType;
            public int minLv;
            public int maxLv;
            public TABLE.MONSTERINFO monsterItem;
            public List<TABLE.ITEM> items;
            static List<TABLE.ITEM> filterItems = new List<TABLE.ITEM>(256);
            static StringBuilder builder = new StringBuilder(1024);
            enum ExpressItemType
            {
                EIT_SPECIAL = 1,//特殊物品
                EIT_WOLONG_EQUIP = 2,//卧龙装备
                EIT_NORMAL_EQUIP = 3,//常规装备
                EIT_OTHER = 4,//其他装备
                EIT_NONE,
            }
            const int maxItemCount = 16;
            public void Filter(int career, int sex,System.Comparison<TABLE.ITEM> comparison)
            {
                filterItems.Clear();
                for (int i = 0; i < items.Count; ++i)
                {
                    if ((items[i].career == 0 || items[i].career == career) && (items[i].sex == sex || items[i].sex == 2))
                    {
                        filterItems.Add(items[i]);
                    }
                }
                filterItems.Sort(comparison);
            }

            bool IsSpecial(TABLE.ITEM itemCfg)
            {
                return null != itemCfg && itemCfg.exValue > 0;
            }

            bool IsEquip(TABLE.ITEM itemCfg)
            {
                return null != itemCfg && itemCfg.type == 2;
            }

            public string MakeIntArrayString()
            {
                builder.Clear();
                if (null != filterItems)
                {
                    int validCnt = 0;
                    int stageCount = 4;
                    ExpressItemType stage = ExpressItemType.EIT_SPECIAL;
                    for (int i = 0,max = filterItems.Count; i < max && validCnt < maxItemCount; ++i)
                    {
                        bool Append(TABLE.ITEM itemCfg,ref int count,System.Func<TABLE.ITEM,bool> verifier)
                        {
                            if(null != verifier && !verifier(itemCfg) || count == 0)
                            {
                                count += 4;
                                if(stage != ExpressItemType.EIT_NONE)
                                    stage = stage + 1;
                                return true;
                            }
                            --count;
                            if (builder.Length > 0)
                                builder.Append("#");
                            builder.Append(itemCfg.id);
                            ++validCnt;
                            return false;
                        }

                        var cfg = filterItems[i];
                        bool interrupt = false;
                        switch(stage)
                        {
                            case ExpressItemType.EIT_SPECIAL:
                                interrupt = Append(cfg, ref stageCount, IsSpecial);
                                if (interrupt && stageCount == 4)
                                {
                                    while (i < max && IsSpecial(filterItems[i]))
                                        ++i;
                                }
                                break;
                            case ExpressItemType.EIT_WOLONG_EQUIP:
                                interrupt = Append(cfg, ref stageCount, IsWoLongEquip);
                                if (interrupt)
                                    while (i < max && IsWoLongEquip(filterItems[i]))
                                        ++i;
                                break;
                            case ExpressItemType.EIT_NORMAL_EQUIP:
                                interrupt = Append(cfg, ref stageCount, IsEquip);
                                if (interrupt)
                                    while (i < max && IsEquip(filterItems[i]))
                                        ++i;
                                break;
                            case ExpressItemType.EIT_OTHER:
                                interrupt = Append(cfg, ref stageCount, null);
                                break;
                        }

                        if (stage == ExpressItemType.EIT_NONE)
                            break;

                        if (interrupt)
                            --i;
                    }
                }
                return builder.ToString();
            }
        }

        class BossShowItem
        {
            //public int id;
            //public int minLv;
            //public int maxLv;
            public MONSTERINFO monsterItem;
            public List<int>[] items = new List<int>[16];
            public bool isValid
            {
                get
                {
                    for (int i = 0; i < items.Length; ++i)
                        if (null != items[i] && items[i].Count > 0)
                            return true;
                    return false;
                }
            }

            public static int[] monsterLvClass = new int[]
            {
                10,30,50,70,90,110,120,130,140,150,160,170,180,190,200
            };
            static int MonsterLvClass(int monsterLv)
            {
                int findIndex = -1;
                for (int idx = 0; idx < monsterLvClass.Length; ++idx)
                {
                    if (monsterLv <= monsterLvClass[idx])
                    {
                        findIndex = idx;
                        break;
                    }
                }
                return findIndex;
            }

            public void AddItem(List<int> levelFits,int itemId,ITEMARRAY itemArray,int[] levelMax, MONSTERINFO monsterItem, List<int> levelExtras)
            {
                bool error = false;
                for(int i = 0,max = levelExtras.Count; i < max;++i)
                {
                    var itemInfo = itemArray.gItem.id2offset[itemId].Value as TABLE.ITEM;
                    if (itemInfo.type == 2 && itemInfo.subType < 100)
                    {
                        if (monsterItem.PropertiesSuit == 1 || monsterItem.PropertiesSuit == 3)
                        {
                            if (itemInfo.levClass != levelExtras[i] && levelExtras[i] >= 3 && monsterItem.id != 2122015)
                            {
                                FNDebug.LogErrorFormat("[表格错误]:物品:[{0}]:怪物等级:[{1}=>{2}]:[levClass:{3}]:怪物ID = [{4}]:[{5}]", itemInfo.name, monsterItem.level, levelExtras[i], itemInfo.levClass
                                    , monsterItem.id, monsterItem.name);
                                error = true;
                            }
                        }
                    }
                }
                if (error)
                    return;

                for(int i = 0,max = levelFits.Count; i < max; ++i)
                {
                    int lvIdx = levelFits[i];
                    if (null == items[lvIdx])
                        items[lvIdx] = new List<int>(8);

                    if (!items[lvIdx].Contains(itemId))
                    {
                        var itemInfo = itemArray.gItem.id2offset[itemId].Value as TABLE.ITEM;
                        if (itemInfo.type == 2 && itemInfo.subType < 100)
                        {
                            if(monsterItem.PropertiesSuit == 1 || monsterItem.PropertiesSuit == 3)
                            {
                                //if(itemInfo.levClass != lvIdx)
                                //{
                                //    FNDebug.LogErrorFormat("[表格错误]:物品:[{0}]:怪物等级:[{1}=>{2}]:[levClass:{3}]", itemInfo.name, monsterItem.level, lvIdx, itemInfo.levClass);
                                //}
                            }
                            else
                            {
                                if(itemInfo.levClass != MonsterLvClass((int)monsterItem.level) && monsterItem.id != 2140945)
                                {
                                    FNDebug.LogErrorFormat("[表格错误]:物品:[{0}]:怪物等级:[{1}=>{2}]:[levClass:{3}]:怪物ID = [{4}]:[{5}]", itemInfo.name, monsterItem.level, MonsterLvClass((int)monsterItem.level), itemInfo.levClass
                                        , monsterItem.id, monsterItem.name);
                                }
                            }
                            
                        }
                        items[lvIdx].Add(itemId);
                    }
                }
            }
        }


        int[] levelMax = new int[]
        {
            50,70,90,100,110,120,130,140,150,160,170,180,190,200
        };
        static List<int> levelFits = new List<int>();
        static List<int> levelFitsExtra = new List<int>();
        void Level2Index(string level,ref List<int> levels, ref List<int> levelFitsExtra)
        {
            levels.Clear();
            levelFitsExtra.Clear();
            if (string.IsNullOrEmpty(level))
            {
                for (int i = 0; i < levelMax.Length; ++i)
                {
                    levels.Add(i);
                }

                for(int i = 0; i < BossShowItem.monsterLvClass.Length;++i)
                {
                    levelFitsExtra.Add(i);
                }
                return;
            }

            var tokens = level.Split('#');
            for(int i = 0,max = tokens.Length;i < max;++i)
            {
                int fitValue = -1;
                if (int.TryParse(tokens[i], out fitValue) && fitValue > 0)
                {
                    int findIndex = -1;
                    for (int idx = 0; idx < levelMax.Length; ++idx)
                    {
                        if (fitValue <= levelMax[idx])
                        {
                            findIndex = idx;
                            break;
                        }
                    }
                    if (-1 != findIndex)
                    {
                        if (!levels.Contains(findIndex))
                            levels.Add(findIndex);
                    }
                    findIndex = -1;
                    for (int idx = 0; idx < BossShowItem.monsterLvClass.Length; ++idx)
                    {
                        if (fitValue <= BossShowItem.monsterLvClass[idx])
                        {
                            findIndex = idx;
                            break;
                        }
                    }
                    if (-1 != findIndex)
                    {
                        if (!levelFitsExtra.Contains(findIndex))
                            levelFitsExtra.Add(findIndex);
                    }
                }
            }
        }

        List<BossShowItem> bossShowItems;
        List<FinalExpressItem> generatedItems;
        List<int> temps = new List<int>(16);

        void CollectValidDatas()
        {
            var handles = bossDropItems.gItem.handles;
            temps.Clear();
            bossShowItems = new List<BossShowItem>(handles.Length);
            BossShowItem bossShowItem = null;

            var boxItemDic = boxItems.gItem.id2offset;
            for (int i = 0,max = handles.Length;i < max;++i)
            {
                var item = handles[i].Value as TABLE.BOSSDROP;
                if (!monsterItems.gItem.id2offset.ContainsKey(item.mid))
                {
                    UnityEngine.Debug.LogErrorFormat("[怪物ID:{0}]无法在怪物表中被找到", item.mid);
                    continue;
                }

                var monsterItem = monsterItems.gItem.id2offset[item.mid].Value as TABLE.MONSTERINFO;
                if (null == monsterItem)
                    continue;

                if (monsterItem.type == 1 && monsterItem.quality <= 2)
                    continue;

                //if(monsterItem.id != 5001255)
                //{
                //    continue;
                //}

                temps.Clear();

                if (item.drop != 0)
                {
                    if (!temps.Contains(item.drop))
                        temps.Add(item.drop);
                }
                if (item.comDrop != 0)
                {
                    if(!temps.Contains(item.comDrop))
                        temps.Add(item.comDrop);
                }
                for(int k = 0,mv = item.extraDrop.Count; k < mv;++k)
                {
                    int v = item.extraDrop[k];
                    if (v != 0 && !temps.Contains(v))
                        temps.Add(v);
                }

                if (temps.Count <= 0)
                    continue;

                if (null == bossShowItem)
                    bossShowItem = new BossShowItem();
                bossShowItem.monsterItem = monsterItems.gItem.id2offset[item.mid].Value as TABLE.MONSTERINFO;

                for (int j = 0; j < temps.Count;++j)
                {
                    int group = temps[j];

                    if(!mBigGroup2Items.ContainsKey(group))
                    {
                        UnityEngine.Debug.LogErrorFormat("[掉落组ID无效:{0}]无法在MDROPITEMS表中被找到,BOSSDROPID = {1}", group, item.id);
                        continue;
                    }

                    foreach(var mdropItem in mBigGroup2Items[group])
                    {
                        if (monsterItem.PropertiesSuit == 1 || monsterItem.PropertiesSuit == 3)
                            Level2Index(mdropItem.level, ref levelFits,ref levelFitsExtra);
                        else
                        {
                            levelFitsExtra.Clear();
                            levelFits.Clear();
                            levelFits.Add(0);
                        }

                        if (mGroup2BoxItems.ContainsKey(mdropItem.itemId))
                        {
                            foreach (var boxItem in mGroup2BoxItems[mdropItem.itemId])
                            {
                                if (!items.gItem.id2offset.ContainsKey(boxItem.itemId))
                                {
                                    UnityEngine.Debug.LogErrorFormat("[物品ID:{0}]无法在物品表中被找到",boxItem.itemId);
                                    continue;
                                }
                                bossShowItem.AddItem(levelFits, boxItem.itemId, items, levelMax, monsterItem, levelFitsExtra);
                            }
                        }
                        else
                        {
                            if (!items.gItem.id2offset.ContainsKey(mdropItem.itemId))
                            {
                                UnityEngine.Debug.LogErrorFormat("[物品ID:{0}]无法在物品表中被找到", mdropItem.itemId);
                                continue;
                            }

                            bossShowItem.AddItem(levelFits, mdropItem.itemId, items, levelMax, monsterItem, levelFitsExtra);
                        }
                    }
                }

                if(bossShowItem.isValid)
                {
                    bossShowItems.Add(bossShowItem);
                    bossShowItem = null;
                }
            }
        }

        public static bool IsWoLongEquip(TABLE.ITEM _cfg)
        {
            if (_cfg == null) { return false; }
            if (_cfg.type == 2 && 101 <= _cfg.subType && _cfg.subType <= 110)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int ItemComparer(TABLE.ITEM a_cfg, TABLE.ITEM b_cfg)
        {
            bool a_special = a_cfg.exValue > 0;
            bool b_special = b_cfg.exValue > 0;
            if (a_special != b_special)
                return a_special ? -1 : 1;

            if(a_special)
            {
                if (a_cfg.exValue != b_cfg.exValue)
                    return b_cfg.exValue - a_cfg.exValue;
                return a_cfg.id - b_cfg.id;
            }

            bool a_equip = a_cfg.type == 2;
            bool b_equip = b_cfg.type == 2;
            if (a_equip != b_equip)
            {
                return a_equip ? -1 : 1;
            }

            if (a_equip)
            {
                bool a_wolong = IsWoLongEquip(a_cfg);
                bool b_wolong = IsWoLongEquip(b_cfg);
                if (a_wolong != b_wolong)
                {
                    return a_wolong ? -1 : 1;
                }

                if(a_wolong)
                {
                    if (a_cfg.wolongLv != b_cfg.wolongLv)
                        return b_cfg.wolongLv - a_cfg.wolongLv;
                    if (a_cfg.subType != b_cfg.subType)
                        return a_cfg.subType - b_cfg.subType;
                    return a_cfg.id - b_cfg.id;
                }

                if (a_cfg.quality != b_cfg.quality)
                {
                    return b_cfg.quality - a_cfg.quality;
                }

                if (a_cfg.level != b_cfg.level)
                    return b_cfg.level - a_cfg.level;

                return a_cfg.id - b_cfg.id;
            }

            if (a_cfg.quality != b_cfg.quality)
            {
                return b_cfg.quality - a_cfg.quality;
            }

            if (a_cfg.level != b_cfg.level)
                return b_cfg.level - a_cfg.level;

            return a_cfg.id - b_cfg.id;
        }

        void ExpressValidDatas()
        {
            generatedItems = new List<FinalExpressItem>(4096);
            int totalCnt = 0;
            for(int i = 0; i < bossShowItems.Count;++i)
            {
                var bossShowItem = bossShowItems[i];
                if (null == bossShowItem)
                    continue;

                for (int k = 0,max = bossShowItem.items.Length;k < max; ++k)
                {
                    var kv = bossShowItem.items[k];
                    if (null == kv || kv.Count <= 0)
                        continue;
                    int minLevel = k > 0 ? levelMax[k - 1] : 0;
                    int maxLevel = levelMax[k];

                    var monsterItem = bossShowItem.monsterItem;
                    if (monsterItem.id > 0xFFFFFFF)
                    {
                        UnityEngine.Debug.LogErrorFormat("怪物ID超过数字上限[0,0xFFFFFF] NOW={0}", monsterItem.id);
                        continue;
                    }

                    if (k + 1 > 0xF)
                    {
                        UnityEngine.Debug.LogErrorFormat("怪我掉落级别超过数字上限[0,0xF] NOW={0}", k + 1);
                        continue;
                    }

                    if (!(monsterItem.PropertiesSuit == 1 || monsterItem.PropertiesSuit == 3))
                    {
                        maxLevel = levelMax[levelMax.Length - 1];
                    }

                    int id = ((k + 1) << 28) | bossShowItem.monsterItem.id;
                    ++totalCnt;

                    FinalExpressItem finalItems = new FinalExpressItem
                    {
                        id = id,
                        minLv = minLevel,
                        maxLv = maxLevel,
                        levelType = k + 1,
                        monsterItem = bossShowItem.monsterItem,
                    };

                    finalItems.items = new List<TABLE.ITEM>(kv.Count);
                    for (int j = 0, mj = kv.Count; j < mj; ++j)
                        finalItems.items.Add(items.gItem.id2offset[kv[j]].Value as TABLE.ITEM);
                    finalItems.items.Sort(ItemComparer);

                    generatedItems.Add(finalItems);
                    //if(!(bossShowItem.monsterItem.PropertiesSuit == 1 || bossShowItem.monsterItem.PropertiesSuit == 3))
                    //{
                    //    Debug.LogFormat("[id:{4}]|[monsterid:{0}]|[level:{1}]|[{2},{3}]|[count:{5}]", bossShowItem.monsterItem.id, k + 1, minLevel, maxLevel, id, kv.Count);
                    //}
                }
            }
            UnityEngine.Debug.LogFormat("[ExpressValidDatas]:Count:{0} totalCnt:{1}", bossShowItems.Count, totalCnt);
        }

        void WriteHead(ISheet sheet)
        {
            sheet.WriteHead(0, "id", VarOptionType.VarTypeSint32, OptionType.OptionRequired,true);
            sheet.WriteHead(1, "monsterid", VarOptionType.VarTypeSint32, OptionType.OptionRequired, false);
            sheet.WriteHead(2, "levelType", VarOptionType.VarTypeSint32, OptionType.OptionRequired, false);
            sheet.WriteHead(3, "levelMin", VarOptionType.VarTypeSint32, OptionType.OptionRequired, false);
            sheet.WriteHead(4, "levelMax", VarOptionType.VarTypeSint32, OptionType.OptionRequired, false);
            sheet.WriteHead(5, "itemId0", VarOptionType.VarTypeSint32, OptionType.OptionRepeated, true);
            sheet.WriteHead(6, "itemId1", VarOptionType.VarTypeSint32, OptionType.OptionRepeated, true);
            sheet.WriteHead(7, "itemId2", VarOptionType.VarTypeSint32, OptionType.OptionRepeated, true);
            sheet.WriteHead(8, "itemId3", VarOptionType.VarTypeSint32, OptionType.OptionRepeated, true);
            sheet.WriteHead(9, "itemId4", VarOptionType.VarTypeSint32, OptionType.OptionRepeated, true);
            sheet.WriteHead(10, "itemId5", VarOptionType.VarTypeSint32, OptionType.OptionRepeated, true);

            sheet.SetStringValue(5, 0, "id");
            sheet.SetStringValue(5, 1, "怪物id");
            sheet.SetStringValue(5, 2, "等级段类型");
            sheet.SetStringValue(5, 3, "等级");
            sheet.SetStringValue(5, 4, "品质");
            sheet.SetStringValue(5, 5, "角色装备1");
            sheet.SetStringValue(5, 6, "角色装备2");
            sheet.SetStringValue(5, 7, "角色装备3");
            sheet.SetStringValue(5, 8, "角色装备4");
            sheet.SetStringValue(5, 9, "角色装备5");
            sheet.SetStringValue(5, 10, "角色装备6");
        }

        void WriteContent(ISheet sheet)
        {
            int row = 9;
            for(int i = 0,max = generatedItems.Count;i < max;++i)
            {
                var item = generatedItems[i];
                sheet.SetIntValue(row, 0, item.id);
                sheet.SetIntValue(row, 1, item.monsterItem.id);
                sheet.SetIntValue(row, 2, item.levelType);
                sheet.SetIntValue(row, 3, item.minLv);
                sheet.SetIntValue(row, 4, item.maxLv);
                
                item.Filter(1, 0, ItemComparer);
                sheet.SetStringValue(row, 5, item.MakeIntArrayString());

                item.Filter(2, 0, ItemComparer);
                sheet.SetStringValue(row, 6, item.MakeIntArrayString());

                item.Filter(3, 0, ItemComparer);
                sheet.SetStringValue(row, 7, item.MakeIntArrayString());

                item.Filter(1, 1, ItemComparer);
                sheet.SetStringValue(row, 8, item.MakeIntArrayString());

                item.Filter(2, 1, ItemComparer);
                sheet.SetStringValue(row, 9, item.MakeIntArrayString());

                item.Filter(3, 1, ItemComparer);
                sheet.SetStringValue(row, 10, item.MakeIntArrayString());

                ++row;
            }
        }

        void SaveValidDatas()
        {
            var sheetName = "DropShow";
            var path = ExcelCreater.GetTableExcelPath(sheetName);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(sheetName);
            workbook.SetActiveSheet(0);
            if (null != sheet)
            {
                WriteHead(sheet);
                WriteContent(sheet);
            }

            using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                workbook.Write(fs);
            }

            generatedItems?.Clear();
            generatedItems = null;

            bossShowItems?.Clear();
            bossShowItems = null;
        }
    }
}
