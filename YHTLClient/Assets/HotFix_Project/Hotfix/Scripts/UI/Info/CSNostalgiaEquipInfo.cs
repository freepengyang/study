using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using bag;
using Google.Protobuf.Collections;
using memory;
using rank;
using TABLE;
using UnityEngine;

public enum NostalgiaSelectType
{
    EQUIP,
    BAG
}
public class CSNostalgiaEquipInfo : CSInfo<CSNostalgiaEquipInfo>
{
    //背包数据
    public Dictionary<long,NostalgiaBagClass> BagList = new Dictionary<long, NostalgiaBagClass>(32);
    //装备数据
    public Dictionary<long,NostalgiaBagClass> EquipList = new Dictionary<long,NostalgiaBagClass>(64);
    
    //背包堆叠列表,用于判断是否可以升阶
    public Dictionary<int,List<NostalgiaBagClass>> bagStackList = new Dictionary<int, List<NostalgiaBagClass>>();
    
    //身上装备格子信息
    private ILBetterList<HUAIJIUSLOT> slots = new ILBetterList<HUAIJIUSLOT>(32);
    
    //身上的套装
    public Dictionary<int,NostalgiaSuit> suitInfos = new Dictionary<int,NostalgiaSuit>(4);

    //旧套装缓存
    public Dictionary<int,OldSuit> oldSuit = new Dictionary<int,OldSuit>(4);
    
    //最低级套装列表
    public Dictionary<int,HUAIJIUSUIT> lowSuits = new Dictionary<int, HUAIJIUSUIT>(4);
    
    //最高级套装列表
    //public Dictionary<int,HUAIJIUSUIT> TopSuits = new Dictionary<int, HUAIJIUSUIT>(4);
    
    //ILBetterList<long> idbaglist = new ILBetterList<long>(); //背包的唯一id列表
    
    
    //public Dictionary<int,List<HUAIJIUSUIT>> TopSuitsubTypeData = new Dictionary<int, List<HUAIJIUSUIT>>();
    //套装的堆叠数据 key 套装类型  + subtype value为背包数据的数据 
    public Dictionary<int,List<NostalgiaBagClass>> TopsuitsubTypeData = new Dictionary<int, List<NostalgiaBagClass>>();
    
    /// <summary>
    /// 背包的排序列表,在需要的时候主动调用排序后获取值 
    /// </summary>
    public ILBetterList<NostalgiaBagClass> bagSortList = new ILBetterList<NostalgiaBagClass>();

    
    private PoolHandleManager mEquipPoolManager;

    //装备升级所需要得材料数量
    public int NostalgialevelUpNum = 2; 
    
    public int getziIndex;

    public long curSelectid = 0;
    
    //升级界面当前选中的道具列表
    public ILBetterList<NostalgiaBase> curSelectlist = new ILBetterList<NostalgiaBase>();
    public int curClickIndex = 0;
    public long nextSummonTime;
    public int GetziIndex
    {
        get => getziIndex;
        set
        {
            getziIndex = value;
        }
    }

    public ILBetterList<NostalgiaBagClass> GetSortList(NostalgiaSelectType type,bool isSelect = false)
    {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        bagSortList.Clear();
       
        //UnityEngine.Profiling.Profiler.BeginSample("testEnumeratorTimer");
        if (type == NostalgiaSelectType.BAG)
        {
            for (var it = BagList.GetEnumerator(); it.MoveNext();)
            {
                var v = it.Current.Value;
                if (isSelect)
                {
                    if (v.item.saleType != 3)
                        bagSortList.Add(v);   
                }
                else
                    bagSortList.Add(v);
            }
            
            // for (int i = 0, max = idbaglist.Count; i <max ; i++)
            // {
            //     NostalgiaBagClass data; 
            //     if (BagList.TryGetValue(idbaglist[i] ,out data))
            //     {
            //         if (isSelect)
            //         {
            //             if (data.item.saleType != 3)
            //                 bagSortList.Add(data);   
            //         }
            //         else
            //         bagSortList.Add(data);   
            //     }
            // }
        }
        //UnityEngine.Profiling.Profiler.EndSample();
        if (type == NostalgiaSelectType.EQUIP)
        {
            for (var it = EquipList.GetEnumerator(); it.MoveNext();)
            {
                var v = it.Current.Value;
                bagSortList.Add(v);   
            }
        }
        //sw.Stop();
        //FNDebug.Log($"[InitBagData:sort] : cost {sw.ElapsedMilliseconds} ||{bagSortList.Count} ms");
        
        SortListComparer(bagSortList);
        
        return bagSortList;

    }

    protected void SortListComparer(ILBetterList<NostalgiaBagClass> datas)
    {
        var datasList = SortHelper.GetComparersList(32);
        datasList.AddCompare(SortHelper.IntLess, 0);

        var handles = SortHelper.GetSortHandle(datas.Count);

        for(int i = 0,max = datas.Count;i < max;++i)
        {
            handles[i].handle = datas[i];
            handles[i].intValue[0] = datas[i].sortId;
        }

        for (int i = 0, max = datas.Count; i < max; ++i)
        {
            datas[i] = handles[i].handle as NostalgiaBagClass;
        }

        handles.OnRecycle();
        datasList.OnRecycle();
    }
    
    

    public List<int> suitSubTypes = new List<int>();
    public CSNostalgiaEquipInfo()
    {
        //初始话格子和套装信息,都是读表
        InitSlots();
        mEquipPoolManager = new PoolHandleManager();
        InitSuits();
        string str = SundryTableManager.Instance.GetSundryEffect(1064);
        suitSubTypes = UtilityMainMath.SplitStringToIntList(str);
        int maxitemnum;
        int.TryParse(SundryTableManager.Instance.GetSundryEffect(1122), out maxitemnum);
        PageNum = maxitemnum / 25;
    }
    
    //背包单页最大数量
    public int PageMaxNum = 25;
    //背包页数
    public int PageNum = 0;

    /// <summary>
    /// 判断背包格子是否已满
    /// </summary>
    /// <returns></returns>
    public bool isEquipBagFull()
    {
       return EquipList.Count >= GetziIndex;
    }

    public void OnClickFriend(GameObject obj)
    {
        //记忆
        //var Info = CSNostalgiaEquipInfo.Instance;
        if (suitInfos.ContainsKey(2))
        {
            //判断是否有装备
            var suitInfo = suitInfos[2];
            var para = suitInfo.Huaijiusuit.param2.Split('#');
            if (!suitInfo.isActive)
            {
                UtilityTips.ShowRedTips(1991);
                return;
            }
            
            //判断是否是队长
            if (!CSTeamInfo.Instance.IsTeamLeader())
            {
                UtilityTips.ShowRedTips(1989);
                return;
            }

            //判断cd是否达到
            if (nextSummonTime > CSServerTime.Instance.TotalMillisecond)
            {
                long remainTime = (nextSummonTime - CSServerTime.Instance.TotalMillisecond) / 1000;
                UtilityTips.ShowRedTips(1992,CSServerTime.Instance.FormatLongToTimeStr(remainTime,3));
                return;
            }
            
            bool ismap = false;
            //判断地图
            int mapid = CSMainPlayerInfo.Instance.MapID;
            for (int i = 0; i < para.Length; i++)
            {
                if (int.Parse(para[i]) == mapid)
                    ismap = true;
            }
            
            if (!ismap)
            {
                UtilityTips.ShowRedTips(1990);
                return;
            }

            UtilityTips.ShowPromptWordTips(97, () =>
            {
                Net.ReqMemorySummonTeamMessage();
                //UtilityTips.ShowRedTips(2013);
            });
            
        }
    }    
    private void InitSlots()
    {
        
        var arr = HuaiJiuSlotTableManager.Instance.array.gItem.handles;
        if (arr != null)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                var huaijiuslot = arr[i].Value as HUAIJIUSLOT;
                if (null != huaijiuslot)
                    slots.Add(huaijiuslot);
            }   
        }
    }
    private void InitSuits()
    {
        var arr = HuaiJiuSuitTableManager.Instance.array.gItem.handles;
        if (arr != null)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                var suit = arr[i].Value as HUAIJIUSUIT;
                int type = suit.type;
                if (lowSuits.ContainsKey(type))
                {
                    if (lowSuits[type].id > suit.id)
                        lowSuits[type] = suit;
                }
                else
                    lowSuits[type] = suit;

                // if (!TopSuitsubTypeData.ContainsKey(type))
                // {
                //     TopSuitsubTypeData[type] = new List<HUAIJIUSUIT>();
                //     TopSuitsubTypeData[type].Add(suit);
                // }
                // else
                //     TopSuitsubTypeData[type].Add(suit);
                
                // if (TopSuits.ContainsKey(type))
                // {
                //     if (TopSuits[type].id < suit.id)
                //         TopSuits[type] = suit;
                // }
                // else
                //     TopSuits[type] = suit;
            }   
        }
    }

    #region 列表get set
    
    public void AddBagList(RepeatedField<BagItemInfo> itemInfos,bool isInit = false)
    {
        if (isInit)
        {
            BagList.Clear();
            //idbaglist.Clear();
        }
        for (int i = 0; i < itemInfos.Count; i++)
        {
            var bagClass =  mEquipPoolManager.GetSystemClass<NostalgiaBagClass>();
            if (ItemTableManager.Instance.TryGetValue(itemInfos[i].configId,out bagClass.item))
            {
                bagClass.bagiteminfo = itemInfos[i];
                var item = bagClass.item;
                bagClass.sortId = GetSortId(item);
                HuaiJiuSuitTableManager.Instance.TryGetValue(bagClass.item.huaiJiuSuit, out bagClass.Huaijiusuit);
                if (bagClass.Huaijiusuit == null)
                {
                    FNDebug.LogError($"配表错误@高飞 id {bagClass.item.huaiJiuSuit}");
                    return;
                }
                bagClass.type = NostalgiaSelectType.BAG;
                //增加堆叠列表
                FNDebug.Log($"堆叠列表增加{item.id}");
                if (bagStackList.ContainsKey(item.id))
                    bagStackList[item.id].Add(bagClass);
                else
                {
                    bagStackList[item.id] = new List<NostalgiaBagClass>(16);
                    bagStackList[item.id].Add(bagClass);
                }

                //增加top列表
                int huaijiuindex = GetsuitsubTypeid(bagClass.Huaijiusuit.type,item.subType);
                if (TopsuitsubTypeData.ContainsKey(huaijiuindex) && TopsuitsubTypeData[huaijiuindex].Count >0)
                {
                    //if (TopsuitsubTypeData[huaijiuindex][0].Huaijiusuit.rank < bagClass.Huaijiusuit.rank)
                    TopsuitsubTypeData[huaijiuindex].Add(bagClass);
                }
                else
                {
                    TopsuitsubTypeData[huaijiuindex] = new List<NostalgiaBagClass>();
                    TopsuitsubTypeData[huaijiuindex].Add(bagClass);
                }

            }
            
            //增加背包列表
            if (BagList.ContainsKey(itemInfos[i].id))
            {
                long id = itemInfos[i].id;
                int itemid = BagList[id].item.id;
                FNDebug.Log($"列表替换{id}");
                if (bagStackList.ContainsKey(itemid))
                {
                    bagStackList[itemid].Remove(BagList[id]);
                    FNDebug.Log($"堆叠列表移除 {id}剩余 {bagStackList[itemid].Count} ");
                }
                FNDebug.Log($"列表替换{itemInfos[i].id}");
                BagList[id] = bagClass;
            }
            else
            {
                FNDebug.Log($"列表增加{itemInfos[i].id}");
                BagList.Add(itemInfos[i].id,bagClass);
                //idbaglist.Add(itemInfos[i].id);
            }
        }
    }

    /// <summary>
    /// 获取装备dic id 根据 suit Type 和 item subType
    /// </summary>
    /// <param name="type">suit 中的type </param>
    /// <param name="subType">item表中的subType</param>
    /// <returns></returns>
    public int GetsuitsubTypeid(int type , int subType)
    {
        return subType * 100 + type;
    }

    public void RemoveBagList(RepeatedField<long> itemInfos)
    {
        for (int i = 0; i < itemInfos.Count; i++)
        {
            long id = itemInfos[i];
            if (BagList.ContainsKey(id))
            {
                //移除堆叠列表
                int itemid = BagList[id].item.id;
                if (bagStackList.ContainsKey(itemid))
                {
                    bagStackList[itemid].Remove(BagList[id]);
                    FNDebug.Log($"堆叠列表移除 {id}剩余 {bagStackList[itemid].Count} ");
                }
                //移除top列表
                var v = BagList[id];
                int huaijiuindex = GetsuitsubTypeid(v.Huaijiusuit.type,v.item.subType);
                if (TopsuitsubTypeData.ContainsKey(huaijiuindex))
                    TopsuitsubTypeData[huaijiuindex].Remove(BagList[id]);
                
            }
            FNDebug.Log($"列表移除 {id}");
            BagList.Remove(id);
            //idbaglist.Remove(id);
        }
    }

    //移除物品(丢弃)
    public void RemoveItem(long id)
    {
        if (BagList.ContainsKey(id))
        {
            var info = BagList[id];
            if (bagStackList.ContainsKey(info.item.id))
            {
                bagStackList[info.item.id].Remove(info);
                BagList.Remove(id);
                //idbaglist.Remove(id);

            }
        }

        if (EquipList.ContainsKey(id))
        {
            EquipList.Remove(id);
        }
        
    }
    
    public void AddEquipList(RepeatedField<BagItemInfo> equipInfos,bool isInit = false)
    {
        // if (isInit)
        // {
        //     EquipList.Clear();
        // }
        
        for (int i = 0; i < equipInfos.Count; i++)
        {
            var equipClass = mEquipPoolManager.GetSystemClass<NostalgiaBagClass>();
            if (ItemTableManager.Instance.TryGetValue(equipInfos[i].configId,out equipClass.item))
            {
                //equipClass.equipInfo = equipInfos[i];
                var item = equipClass.item;
                equipClass.sortId = GetEquipsortId(item);
                equipClass.bagiteminfo = equipInfos[i];
                //HuaiJiuSuitTableManager.Instance.TryGetValue(item.huaiJiuSuit ,out equipClass.Huaijiusuit);
                HuaiJiuSuitTableManager.Instance.TryGetValue(item.huaiJiuSuit, out equipClass.Huaijiusuit);
                if (equipClass.Huaijiusuit == null)
                {
                    FNDebug.LogError($"配表错误@高飞 id {item.huaiJiuSuit}");
                    //return;
                }
                equipClass.type = NostalgiaSelectType.EQUIP;
                
            }

            long id = equipInfos[i].id;
            if (EquipList.ContainsKey(id))
            {
                FNDebug.Log($"装备列表替换{id}");
                EquipList[id] = equipClass;
            }
            else
            {
                FNDebug.Log($"装备列表增加{id}");
                EquipList.Add(equipInfos[i].id,equipClass);
            }
        }
        
    }

    public void RemoveEquipList(BagItemInfo equipInfo)
    {
        FNDebug.Log($"装备列表移除{equipInfo.id}");
        EquipList.Remove(equipInfo.id);    
    }

    public int GetSortId(ITEM item)
    {
        if (item == null)
            return 0;
        return item.subType + 1000 * item.huaiJiuSuit + 100000 * item.levClass;
    }

    private int GetEquipsortId(ITEM item)
    {
        if (item == null)
            return 0;
        return item.subType + 1000 * item.levClass + 100000 * item.huaiJiuSuit;
    }

    public ILBetterList<HUAIJIUSLOT> GetSlots()
    {
        return slots;
    }

    #endregion

    /// <summary>
    /// 获取套装第一个没有的类型
    /// </summary>
    public int GetTopRemoveSubType(NostalgiaSuit suit)
    {
        for (int i = 0; i < suitSubTypes.Count; i++)
        {
            if (!suit.equips.ContainsKey(suitSubTypes[i]))
            {
                return suitSubTypes[i];
            }
        }

        return 0;
    }

    public void BagListChange(MemoryAdd memoryAdd)
    {
        if (memoryAdd == null)
            return;

        var changeList = memoryAdd.changeList;
        AddBagList(changeList);
    }

    public void EquipListChange(MemoryEquipChange info)
    {
        if (info == null)
            return;
        
        var equips = info.equips;
        if (equips == null)
            return;
        
        if (!EquipList.ContainsKey(equips.id))
        {
            RepeatedField<BagItemInfo> equipInfos = mEquipPoolManager.GetSystemClass<RepeatedField<BagItemInfo>>();
            equipInfos.Clear();
            equipInfos.Add(equips);
            AddEquipList(equipInfos);
            mEquipPoolManager.Recycle(equipInfos);
        }
        else
        {
            RemoveEquipList(equips);
        }

        SuitNumChange();

    }

    public int SortList(NostalgiaBase a, NostalgiaBase b)
    {
        return b.sortId - a.sortId;
    }

    public long IsRepleace(int huaijiuid,int subType)
    {
        //var list = CSNostalgiaEquipInfo.Instance.EquipList;
        long oldid = 0;
        for (var it = EquipList.GetEnumerator(); it.MoveNext();)
        {
            HUAIJIUSUIT huaijiusuit;
                    
            if (HuaiJiuSuitTableManager.Instance.TryGetValue(huaijiuid,out huaijiusuit))
            {
                var suit = it.Current.Value.Huaijiusuit;
                if (suit.type == huaijiusuit.type && suit.rank < huaijiusuit.rank && subType ==  it.Current.Value.item.subType)
                {
                    oldid = it.Current.Value.bagiteminfo.id;
                    break;
                    
                } 
            }
        }

        return oldid;
    }

    public Dictionary<int,NostalgiaBagClass> GetSuitNum(HUAIJIUSUIT huaijiusuit)
    {
        //FNDebug.Log($"类型是{huaijiusuit.type}||{huaijiusuit.id}");
        Dictionary<int ,NostalgiaBagClass> temp = mEquipPoolManager.GetSystemClass<Dictionary<int ,NostalgiaBagClass>>();
        temp.Clear();
        for (var it = EquipList.GetEnumerator(); it.MoveNext();)
        {
            var suit = it.Current.Value.Huaijiusuit;
            if (suit.type == huaijiusuit.type && suit.rank >= huaijiusuit.rank)
            {
                int subType = it.Current.Value.item.subType;
                if (!temp.ContainsKey(subType))
                {
                    temp.Add(subType,it.Current.Value);
                }
                else
                {
                    if (it.Current.Value.sortId > temp[subType].sortId)
                        temp[subType] = it.Current.Value;
                }
            }
        }
        //FNDebug.Log($"输出id数量{temp.Count}");
        return temp;
    }

    /// <summary>
    /// 获取套装信息
    /// <param name="msg"></param>
    public void SuitChange(MemoryEquipSuit msg)
    {
        //总共循环3次
        suitInfos.Clear();
        var suits = msg.suits;
        //var count = msg.count;
        
        for (int i = 0; i < suits.Count; i++)
        {
            HUAIJIUSUIT tempsuit;
            //suitclass.position = suits[i].position;
            if (HuaiJiuSuitTableManager.Instance.TryGetValue(suits[i] , out tempsuit))
            {
                NostalgiaSuit  suitclass;
                int type = tempsuit.type;
                if (!suitInfos.ContainsKey(type))
                    suitclass = mEquipPoolManager.GetSystemClass<NostalgiaSuit>();
                else
                    suitclass = suitInfos[type];
               
                suitclass.Huaijiusuit = tempsuit;
                suitclass.equips = GetSuitNum(suitclass.Huaijiusuit);
                int count = suitclass.equips.Count;
                suitclass.Count = count;
                suitclass.isActive = count == suitSubTypes.Count;
                suitclass.isMax = suitclass.Huaijiusuit.nextID == 0;
                suitclass.maxNum = suitSubTypes.Count;
                //suitclass.AddEquip = "";

                if (oldSuit.ContainsKey(type))
                {
                    bool isActive = oldSuit[type].isActive;
                    var huaijiusuit = oldSuit[type].Huaijiusuit;
                    int tempsuitid = suitclass.Huaijiusuit.id;
                    if ((!isActive && suitclass.isActive)||(isActive && huaijiusuit.id != tempsuitid))
                    {
                        //显示激活
                        CSGame.Sington.StartCoroutine(ShowTipPanel(suitclass));
                    }
                }
                
                if (oldSuit.Count > i)
                {
                    //bool isActive = (oldSuit[i] & 1) == 1;
                    //int suitId = oldSuit[i] >> 1;
                   
                }
                if(!suitInfos.ContainsKey(type))
                    suitInfos.Add(type,suitclass);
            }
        }

        for (int i = 1; i <= 3; i++)
        {
            if (!suitInfos.ContainsKey(i))
            {
                var suitclass = mEquipPoolManager.GetSystemClass<NostalgiaSuit>();
                if (lowSuits.ContainsKey(i))
                {
                    suitclass.Huaijiusuit = lowSuits[i];
                    suitclass.equips = GetSuitNum(suitclass.Huaijiusuit);
                    suitclass.isMax = false;
                    suitclass.isActive = false;
                    suitclass.maxNum = suitSubTypes.Count;
                    suitclass.Count = suitclass.equips.Count;
                    suitInfos.Add(i,suitclass);
                } 
            }
        }
        
        
        oldSuit.Clear();
        for (var it = suitInfos.GetEnumerator();it.MoveNext();)
        {
            var v = it.Current.Value;
            int type = v.Huaijiusuit.type;
            //int boolindex = v.isActive ? 1 : 0;
            //int res =  it.Current.Value.Huaijiusuit.id << 1 | boolindex;
            if (oldSuit.ContainsKey(type))
            {
                oldSuit[type].isActive = v.isActive;
                oldSuit[type].Huaijiusuit = v.Huaijiusuit;
            }
            else
            {
                var oldsuit = new OldSuit();
                oldsuit.isActive = v.isActive;
                oldsuit.Huaijiusuit = v.Huaijiusuit;
                oldSuit.Add(type,oldsuit);
            }

        }
    }

    public void SuitNumChange()
    {
        //oldSuit.Clear();
        for (var it = suitInfos.GetEnumerator();it.MoveNext();)
        {
            var v = it.Current.Value;
            var suit = v.Huaijiusuit;
            v.equips = GetSuitNum(suit);
            v.Count = v.equips.Count;
            v.isActive = v.Count == v.maxNum;
            //v.isMax = v.Huaijiusuit.nextID == 0;
            //oldSuit.Add(it.Current.Value.isActive);
        }
        
    }


    IEnumerator ShowTipPanel(NostalgiaSuit suit)
    {
        yield return null;
        //显示面板
        UIManager.Instance.CreatePanel<UINostalgiaPromptPanel>(f =>
        { 
            (f as UINostalgiaPromptPanel).OpenPanel(suit);
        }
        );
        
    }

    /// <summary>
    /// 判断背包内的物品是否比装备高级
    /// </summary>
    /// <returns></returns>
    public bool IsArrow(ITEM item)
    {
        HUAIJIUSUIT suit;
        if (HuaiJiuSuitTableManager.Instance.TryGetValue(item.huaiJiuSuit, out suit))
        {
            for (var it = EquipList.GetEnumerator(); it.MoveNext();)
            {
                var cursuit = it.Current.Value.Huaijiusuit;
                if (cursuit.type == suit.type && cursuit.rank < suit.rank)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool BagRedPointCheck()
    {
        if (GetziIndex > EquipList.Count && BagList.Count > 0)
        {
            return true;
        }

        for (var it = EquipList.GetEnumerator(); it.MoveNext();)
        {
            var itemid = it.Current.Value.item.id;
                
            if (bagStackList.ContainsKey(itemid))
            {
                if (bagStackList[itemid].Count >= NostalgialevelUpNum)
                {
                    return true;
                } 
            }
        }
        return false;
    }


    public override void Dispose()
    {
        
    }

}

public class NostalgiaBagClass :NostalgiaBase
{
   
}

public class NostalgiaEquipClass :NostalgiaBase
{
    public EquipInfo equipInfo;
}

public class NostalgiaBase
{
    public BagItemInfo bagiteminfo;
    public ITEM item;
    public int sortId;
    public HUAIJIUSUIT Huaijiusuit;
    public NostalgiaSelectType type;
}

public class NostalgiaSuit
{
    public HUAIJIUSUIT Huaijiusuit;
    public bool isMax; //是否是最大套装
    public bool isActive; //是否激活
    public int maxNum;
    public int Count;
    public Dictionary<int, NostalgiaBagClass> equips;

    //public int RemoveSubType; //缺少的subType
    
    //public NostalgiaBagClass AddEquip; //选取加入的装备信息
    //public int num;
}

public class OldSuit
{
    public HUAIJIUSUIT Huaijiusuit;
    public bool isActive;
}


