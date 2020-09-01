using System;
using System.Collections.Generic;
using System.Diagnostics;
using bag;
using pet;
using TABLE;
using UnityEngine;
using Google.Protobuf.Collections;

public class CSPetLevelUpInfo : CSInfo<CSPetLevelUpInfo>
{
    
    protected PoolHandleManager mEquipPoolManager;
    public CSPetLevelUpInfo()
    {
        Init();
    }

    //背包页数
    public int PageNum = 8;

    public int PrompLevel = 40;//弹promp所要用的等级
    
    public void Init()
    {
        string playId = CSMainPlayerInfo.Instance.ID.ToString();
        
        mEquipPoolManager = new PoolHandleManager();
        
        // for (int i = 0; i < SetStrs.Length; i++)
        // {
        //     SetStrs[i] = $"{SetStrs[i]}{playId}";
        //     
        //     mDefaultDic.Add(SetStrs[i],0/*SetStrsValue[i]*/);
        // }
        //
        // for (int i = 0; i < SetPopStrs.Length; i++)
        // {
        //     SetPopStrs[i] = $"{SetPopStrs[i]}{playId}";
        // }
        
        Career = CSMainPlayerInfo.Instance.Career;
        sex = CSMainPlayerInfo.Instance.Sex;
        
        //OnChangeSet();
        //mClientEvent.AddEvent(CEvent.ConfigCallback,OnChangeSet);
        mClientEvent.AddEvent(CEvent.ItemListChange,OnItemChange);
        mClientEvent.AddEvent(CEvent.WearEquip, OnItemChange);
        mClientEvent.AddEvent(CEvent.UnWeatEquip, OnItemChange);
        mClientEvent.AddEvent(CEvent.BagSort, OnItemChange); //背包整理
        mClientEvent.AddEvent(CEvent.BagInit, OnInit); 
        
        string levelstr = SundryTableManager.Instance.GetSundryEffect(1082);
        int.TryParse(levelstr,out AutoRecycleVipLevel);
        //InitBagData();
    }

    
    
    public RepeatedField<int> callBackSetList = new RepeatedField<int>();
    
    public void OnChangeSet(CallBackSetting callBackSetting)
    {
        callBackSetList = callBackSetting.callBackSettingList;
        if (callBackSetList.Count > 0)
        {
            SetBoolList.Clear();
            var v = callBackSetList[0];
            SetBoolList.Add((v >> 4 & 1) == 1);
            SetBoolList.Add((v >> 3 & 1) == 1);
            SetBoolList.Add((v >> 2 & 1) == 1);
            SetBoolList.Add((v >> 1 & 1) == 1);
            SetBoolList.Add((v & 1) == 1);
        }
        mClientEvent.SendEvent(CEvent.ConfigCallback);
    }

    public void SendSetting()
    {
        int v = 0;

        int num = SetBoolList.Count - 1;
        
        for (int i = num; i >=0 ; i--)
        {
            v = v | (SetBoolList[num-i] ? 1 : 0) << i;
        }
        if (callBackSetList.Count > 0)
        {
            callBackSetList[0] = v;
            Net.CSSetCallBackSettingMessage(callBackSetList);
        }
        // v = v | (SetBoolList[4] ? 1 : 0);
        // v = v | (SetBoolList[3] ? 1 : 0) << 1;
        // v = v | (SetBoolList[2] ? 1 : 0) << 2;
        // v = v | (SetBoolList[1] ? 1 : 0) << 3;
        // v = v | (SetBoolList[0] ? 1 : 0) << 4;
    }

    // private void OnChangeSet(uint uievtid = 0, object data = null)
    // {
    //     // SetBoolList[0] = PlayerPrefs.GetInt(SetStrs[0],mDefaultDic[SetStrs[0]]) == 1;
    //     // SetBoolList[1] = PlayerPrefs.GetInt(SetStrs[1],mDefaultDic[SetStrs[1]]) == 1;
    //     // SetBoolList[2] = PlayerPrefs.GetInt(SetStrs[2],mDefaultDic[SetStrs[2]]) == 1;
    //     // SetBoolList[3] = PlayerPrefs.GetInt(SetStrs[3],mDefaultDic[SetStrs[3]]) == 1;
    //     // SetBoolList[4] = PlayerPrefs.GetInt(SetStrs[4],mDefaultDic[SetStrs[4]]) == 1;
    //     // Career = CSMainPlayerInfo.Instance.Career;
    //     // sex = CSMainPlayerInfo.Instance.Sex;
    // }

    #region 服务端返回信息
    /// <summary>
    /// 战宠信息
    /// </summary>
    private ResPetInfo _resPetInfo;

    /// <summary>
    /// 升级返回信息
    /// </summary>
    private ResItemCallBackInfo _resItemCallBackInfo;

      
    public ResPetInfo resPetInfo
    {
        get => _resPetInfo;
        set
        {
            _resPetInfo = value;
            mClientEvent.SendEvent(CEvent.CSPetInfoMessage);
        }
    
    }

    public ResItemCallBackInfo resItemCallBackInfo
    {
        get => _resItemCallBackInfo;
        set
        {
            _resItemCallBackInfo = value;
        }
    }

    #endregion

    #region 设置信息
    
    //public string[] SetStrs = new[] {"ItemNormal1", "ItemNormal2", "ItemWolong1", "ItemWolong2" , "ItemVip"};
    //public int[] SetStrsValue = new[] {1, 0, 1, 0, 0};
    //public string[] SetPopStrs = new[] {"ItemQualityFilterType","ItemLevelLimit","ItemGradeLimit"};

    public int AutoRecycleVipLevel = 0;
    
    public Dictionary<string, int> mDefaultDic = new Dictionary<string, int>();   //默认设置
    private Dictionary<int, List<int>> _dicOption;
    public List<int> GetOptionsValue(int id)
    {
        if (_dicOption == null)
            _dicOption = new Dictionary<int, List<int>>();
        if (_dicOption.ContainsKey(id))
        {
            return _dicOption[id];
        }
        else
        {
            var list = new List<int>();
            string[] temp = SundryTableManager.Instance.GetSundryEffect(id).Split('&');
            if (temp == null)
                return null;
            for (int i = 0; i < temp.Length; i++)
            {
                string[] strList = temp[i].Split('#');
                list.Add(int.Parse(strList[0]));
            }
            _dicOption.Add(id,list);
            return list;
        }
    }
    
    #endregion

    #region 主面板使用信息
    RecycleMode _mode = RecycleMode.M_NORMAL;
    public RecycleMode Mode 
    {
        get { return _mode; }
        set
        {
            _mode = value;
        }
    }
    
    public ILBetterList<BagClass> normalEquipList= new ILBetterList<BagClass>(64);//普通装备列表
    public ILBetterList<BagClass> wolongEquipList= new ILBetterList<BagClass>(64);//卧龙装备列表
    
    private Dictionary<long, BagClass> SelectNormalList = new Dictionary<long, BagClass>();
    private Dictionary<long, BagClass> SelectWolongList = new Dictionary<long, BagClass>();
    
    // public bool SetBoolList[0];
    // public bool SetBoolList[1];
    // public bool SetBoolList[2];
    // public bool SetBoolList[3];
    // public bool SetBoolList[4];
    
    public ILBetterList<bool> SetBoolList = new ILBetterList<bool>(5);
    
    int sex;
    int Career;
    private bool isRecycleSuccess = false;
    
    public void SetResItemCallBackInfo(ResItemCallBackInfo info)
    {
        resItemCallBackInfo = info;
        isRecycleSuccess = true;
        UIManager.Instance.CreatePanel<UIPetLevelUpPromptPanel>();
    }


    private int GetSortId(ITEM item)
    {
        int add = item.subType == 71 ? 1000 : 0;
        return add +(100 - item.levClass) * 10 + (10 - item.quality);
    }

    public BagClass GetBagClass(BagItemInfo info , ITEM item)
    {
        var equip = mEquipPoolManager.GetSystemClass<BagClass>();
        equip.bagItemInfo = info;
        equip.sortId = GetSortId(item);
        equip.subType = item.subType;
        equip.item = item;
        return equip;
    }

    #region 性能优化减少排序数量级

    protected void OnInit(uint id, object argv)
    {
        InitBagData();
    }
    
    protected void OnItemChange(uint id, object argv)
    {
        
        Timer.Instance.CancelInvoke(schedule);
        if (!Timer.Instance.IsInvoking(schedule))
            schedule = Timer.Instance.Invoke(0.04f, DelayClick);
        //CSGame.Sington.StartCoroutine();
    }
    
    
    Schedule schedule;
    void DelayClick(Schedule _schedule)
    {
        InitBagData();
        mClientEvent.SendEvent(CEvent.OnRecycleItemChange);
    }
    
    /// <summary>
    /// 刷新装备列表
    /// </summary>
    /// <param name="uievtid"></param>
    /// <param name="data"></param>
    public void InitBagData(bool isSort = true)
    {
        //bool isPanel = UIManager.Instance.IsPanel<UIPetLevelUpPanel>();
        if (normalEquipList == null) normalEquipList = new ILBetterList<BagClass>(64);
        if (wolongEquipList == null) wolongEquipList = new ILBetterList<BagClass>(64);

        //回收所有引用
        mEquipPoolManager.RecycleAll();
        
        normalEquipList.Clear();
        wolongEquipList.Clear();
        
        var iteminfo =  CSBagInfo.Instance.GetBagItemData();    
        var iter = iteminfo.GetEnumerator();
        ITEM item;
        while (iter.MoveNext())
        {
            if (ItemTableManager.Instance.TryGetValue(iter.Current.Value.configId,out item))
            {
                var bagiteminfo = iter.Current.Value;
                
                if (item.subType == 71 && item.type == 5)
                {
                    var bagClass = GetBagClass(bagiteminfo, item);
                    normalEquipList.Add(bagClass);
                    wolongEquipList.Add(bagClass);
                }
                
                if (item.type == 2)
                {
                    if (item.subType > 0 && item.subType <= 10)
                    {
                        if (item.callback.Count == 0)
                            continue;
                        normalEquipList.Add(GetBagClass(bagiteminfo,item));
                    }

                    if (item.subType > 100 && item.subType <= 110)
                    {
                        if (item.callback.Count == 0)
                            continue;
                        wolongEquipList.Add(GetBagClass(bagiteminfo,item));
                    }
                }
            }
        }

        if (isRecycleSuccess)
        {
             RefreshAllSelectData();
             //ShowSelectPromp(null,isPanel);
             isRecycleSuccess = false;
             
        }
        
        //自动回收
        bagindexs.Clear();

        if (SetBoolList.Count <=4)
            return;
        
        if (SetBoolList[4] && CSBagInfo.Instance.IsNullBagNum() < 10)
        {
            RefreshAllSelectData();
            //ShowSelectPromp(() => { },isPanel);
            for (var it = SelectNormalList.GetEnumerator();it.MoveNext();)
            {
                bagindexs.Add(it.Current.Value.bagItemInfo.bagIndex);
            } 
            
            for (var it = SelectWolongList.GetEnumerator();it.MoveNext();)
            {
                bagindexs.Add(it.Current.Value.bagItemInfo.bagIndex);
            }
                
            if (bagindexs.Count > 0)
            {
                Net.CSPetUpgradeMessage(1,bagindexs);
                return;
            }
            //return;
        }

        if (isSort)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            SortListComparer(normalEquipList);
            SortListComparer(wolongEquipList);

            //normalEquipList.Sort(SortList);
            //wolongEquipList.Sort(SortList);
            sw.Stop();
            //FNDebug.Log($"[InitBagData:sort] : cost {sw.ElapsedMilliseconds} ||{normalEquipList.Count} ms");
        }
    }

    /// <summary>
    /// 判断是否需要取消选中比身上等级高的装备
    /// </summary>
    public void ShowSelectPromp(Action action = null,bool isPanel = false)
    {
        var list = RefreshSelectData(Mode);
        if (list.Count>0)
        {
            UtilityTips.ShowPromptWordTips(100, 
                () =>
            {
                UnSelect(list);
                mClientEvent.SendEvent(CEvent.OnRecycleItemChange);
            }, () =>
                {
                    if (action != null) action();
                }
            );
        }
        else
            if (action != null) action();
    }


    private void UnSelect(ILBetterList<BagClass> unselectList)
    {
        for (int i = 0; i < unselectList.Count; i++)
        {
            long id = unselectList[i].bagItemInfo.id;
            SelectNormalList.Remove(id);
            SelectWolongList.Remove(id);
        }
        
        
    }


    private RepeatedField<int> bagindexs = new RepeatedField<int>();
    protected void SortListComparer(ILBetterList<BagClass> datas)
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
            datas[i] = handles[i].handle as BagClass;
        }

        handles.OnRecycle();
        datasList.OnRecycle();
    }
    
    
    // private void AddOrRemoveEquip(BagItemInfo bagiteminfo,ItemChangeType type)
    // {
    //     ITEM item;
    //     if (ItemTableManager.Instance.TryGetValue(bagiteminfo.configId,out item))
    //     {
    //         //var bagiteminfo = iter.Current.Value;
    //         var bagclass = GetBagClass(bagiteminfo, item);
    //         if (item.subType == 71 && item.type == 5)
    //         {
    //             InsertEquipData(bagclass,normalEquipList,type);
    //             InsertEquipData(bagclass,wolongEquipList,type);
    //         }
    //             
    //         if (item.type == 2)
    //         {
    //             if (item.subType > 0 && item.subType <= 10)
    //             {
    //                 bool isHaveValue = ItemCallBackTableManager.Instance.IsHaveByItems
    //                     (bagiteminfo.quality, item.levClass, 1);
    //                 if (isHaveValue)
    //                 {
    //                     InsertEquipData(bagclass,normalEquipList,type);
    //                 }
    //             }
    //
    //             if (item.subType > 100 && item.subType <= 110)
    //             {
    //                 bool isHaveValue = ItemCallBackTableManager.Instance.IsHaveByItems
    //                     (item.quality, item.levClass, 2);
    //                 if (isHaveValue)
    //                 {
    //                     InsertEquipData(bagclass,wolongEquipList,type);
    //                 } 
    //             }
    //         }
    //     }
    // }
    
    // private void InsertEquipData(BagClass bagClass,ILBetterList<BagClass> list,ItemChangeType type)
    // {
    //     if (type == ItemChangeType.Add)
    //     {
    //         for (int i = 0; i < list.Count; i++)
    //         {
    //             if (list[i].sortId < bagClass.sortId )
    //             {
    //                 list.Insert(i,bagClass);
    //                 break;
    //             }
    //         }
    //     }
    //
    //     if (type == ItemChangeType.Remove)
    //     {
    //         list.Remove(bagClass);
    //     }
    //
    //     if (type == ItemChangeType.NumAdd||type == ItemChangeType.NumReduce)
    //     {
    //         for (int i = 0; i < list.Count; i++)
    //         {
    //             if (list[i].bagItemInfo.id == bagClass.bagItemInfo.id)
    //             {
    //                 list[i] = bagClass;
    //                 break;
    //             }
    //         }
    //     }
    //
    // }
    
    #endregion
    
    public ILBetterList<BagClass> GetBagInfo()
    {
        if (Mode == RecycleMode.M_NORMAL)
        {
            return normalEquipList;
        }
        else
        {
            return wolongEquipList;
        }

    }

    public void Sort(ILBetterList<BagClass> list)
    {
        list.Sort(SortList);
    }

    private int SortList(BagClass a, BagClass b)
    {
        return b.sortId - a.sortId;
    }


    public Dictionary<long, BagClass> GetCurSelectList()
    {
        if (Mode == RecycleMode.M_NORMAL)
        {
            return SelectNormalList;
        }
        else
        {
            return SelectWolongList;
        }
    }

    public void ClearSelect()
    {
        SelectNormalList.Clear();
        SelectWolongList.Clear();
    }

    // public void SetSelect(BagItemInfo bagItemInfo)
    // {
    //     var iter = GetCurSelectList().GetEnumerator();
    //     while (iter.MoveNext())
    //     {
    //         if (iter.Current.Key == bagItemInfo.id)
    //         {
    //             iter.Current.Value.
    //         }
    //     }
    // }

    // public void RefreshEquipSelectData()
    // {
    //     ClearSelect();
    //     var temList = GetBagInfo();
    //     for (int i = 0; i < temList.Count; i++)
    //     {
    //         RefreshSelectList(temList[i].bagItemInfo.id,temList[i],IsSelect(temList[i].item,Mode),Mode);
    //     }
    // }
    
    ILBetterList<BagClass> UnSelectList = new ILBetterList<BagClass>(); //不选中列表
    Dictionary<int,BagClass> dicEquipTop = new Dictionary<int, BagClass>(); //同部位,同等级 最高评分,最高品质列表

    /// <summary>
    /// 判断是否是最高级别的装备,传入的装备比之前的更高 ,返回之前的BagClass 否则返回自己
    /// </summary>
    private BagClass OnTopEquip(BagClass bag)
    {
        int tempKey = bag.item.subType + bag.item.level * 100;
        if (dicEquipTop.ContainsKey(tempKey))
        {
            var oldbag = dicEquipTop[tempKey];
                
            if (oldbag.item.quality == bag.item.quality)
            {
                var oldid = oldbag.bagItemInfo.id;
                var oldfightCount = CSItemCountManager.Instance.GetFightScore(oldid);
                var id = bag.bagItemInfo.id;
                var fightCount = CSItemCountManager.Instance.GetFightScore(id);
                if (fightCount > oldfightCount)
                {
                    dicEquipTop[tempKey] = bag;
                    return oldbag ;
                }
                else
                    return bag;
            }

            if (bag.item.quality > oldbag.item.quality)
            {
                dicEquipTop[tempKey] = bag;
                return oldbag;    
            }
            else
                return bag;
        }
        else
        {
            dicEquipTop[tempKey] = bag;
            return null;
        }


    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isSelect">判断选中的装备是否有身上是否有更高的等级 </param>
    public void RefreshAllSelectData(bool isPanel = false)
    {
        UnSelectList.Clear();
        ClearSelect();
        dicEquipTop.Clear();
        for (int i = 0; i < normalEquipList.Count; i++)
        {
            var bag = normalEquipList[i];
            bool isSelect = OnSelectEquip(bag.item, RecycleMode.M_NORMAL);

            if (SetBoolList[1])
            {
                if (!isSelect)
                {
                    var tempbag = OnTopEquip(bag);
                    if (tempbag != null)
                    {
                        bag = tempbag;
                        RefreshSelectList(bag.bagItemInfo.id , bag,true,RecycleMode.M_NORMAL);
                    }
                }
                else
                    RefreshSelectList(bag.bagItemInfo.id , bag,isSelect,RecycleMode.M_NORMAL);
            }
            else
                  RefreshSelectList(bag.bagItemInfo.id , bag,isSelect,RecycleMode.M_NORMAL);


            if (isSelect && isPanel)
            {
                if (CSBagInfo.Instance.GetEquipLevelByPos(bag.item)&& bag.item.level >= PrompLevel)
                    UnSelectList.Add(bag);
            }
        }
        
        for (int i = 0; i < wolongEquipList.Count; i++)
        {
            var bag = wolongEquipList[i];
            bool isSelect = OnSelectEquip(bag.item,RecycleMode.M_WOLONG);
            RefreshSelectList(bag.bagItemInfo.id,bag,isSelect,RecycleMode.M_WOLONG);
            if (isSelect && isPanel )
            {
                if (CSBagInfo.Instance.GetEquipLevelByPos(bag.item))
                    UnSelectList.Add(bag);
            }
        }
        //return UnSelectList;
    }
    
    public ILBetterList<BagClass> RefreshSelectData(RecycleMode Mode)
    {
        UnSelectList.Clear();
        //ClearSelect();
        dicEquipTop.Clear();
        if (Mode == RecycleMode.M_NORMAL)
        {
            SelectNormalList.Clear();
            if (normalEquipList != null)
            {
                for (int i = 0; i < normalEquipList.Count; i++)
                {
                    var bag = normalEquipList[i];
                    bool isSelect = OnSelectEquip(bag.item, RecycleMode.M_NORMAL);
                    if (SetBoolList[1])
                    {
                        if (!isSelect)
                        {
                            var tempbag = OnTopEquip(bag);
                            if (tempbag != null)
                            {
                                bag = tempbag;
                                RefreshSelectList(bag.bagItemInfo.id , bag,true,RecycleMode.M_NORMAL);
                            }
                        }
                        else
                            RefreshSelectList(bag.bagItemInfo.id , bag,isSelect,RecycleMode.M_NORMAL);
                    }
                    else
                        RefreshSelectList(bag.bagItemInfo.id , bag,isSelect,RecycleMode.M_NORMAL);
                
                    if (isSelect)
                    {
                        if (CSBagInfo.Instance.GetEquipLevelByPos(bag.item) && bag.item.level >= PrompLevel)
                            UnSelectList.Add(bag);
                    }
                }
            }
        }

        if (Mode == RecycleMode.M_WOLONG)
        {
            SelectWolongList.Clear();
            for (int i = 0; i < wolongEquipList.Count; i++)
            {
                var bag = wolongEquipList[i];
                bool isSelect = OnSelectEquip(bag.item,RecycleMode.M_WOLONG);
                RefreshSelectList(bag.bagItemInfo.id,bag,isSelect,RecycleMode.M_WOLONG);
                if (isSelect )
                {
                    if (CSBagInfo.Instance.GetEquipLevelByPos(bag.item))
                        UnSelectList.Add(bag);
                }
            }
        }
        
        return UnSelectList;
    }
    
    /// <summary>
    /// 刷新选择列表,key为背包得唯一id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="item"></param>
    /// <param name="isSelect"></param>
    public void RefreshSelectList(long id,BagClass item,bool isSelect,RecycleMode mode)
    {
        if (mode == RecycleMode.M_NORMAL)
        {
            if (isSelect)
            {
                SelectNormalList[id] = item;
            }
            else
            {
                if (SelectNormalList.ContainsKey(id))
                {
                    SelectNormalList.Remove(id);
                }
                
            }
        }
        else
        {
            if (isSelect)
            {
                SelectWolongList[id] = item;
            }
            else
            {
                if (SelectWolongList.ContainsKey(id))
                {
                    SelectWolongList.Remove(id);
                }
                
            }
        }
    }
    
    public bool OnSelectEquip(ITEM item,RecycleMode curMode)
    {
        if (item == null)
            return false;
        //var popSetSets = SetPopStrs;
        if (curMode == RecycleMode.M_NORMAL)
        {
            bool boolNormal2 = true; //判断第一个选项是否满足
            bool boolNormal1 = true; //判断第二个选项是否满足
            //添加设置条件

            // if (SetBoolList[1])
            // {
            //     if ((item.career != Career && item.career != 0) || (item.sex != sex && item.sex != 2))
            //         boolNormal2 = true;
            //     else
            //         boolNormal2 = false;
            // }
           
            //return true;
            if (SetBoolList[0])
            {
                if (callBackSetList.Count != 4)
                    return false;
                var qualityList = GetOptionsValue(685);
                var lvList = GetOptionsValue(686);
                int qualityIndex = callBackSetList[1];
                int lvIndex = callBackSetList[2];
                
                //防错处理
                if (lvList.Count <= lvIndex || qualityList.Count <= qualityIndex)
                    return false;
                //判断如果装备品质和等级小于选中限定,则跳过
                if (lvList[lvIndex] >= item.levClass && qualityList[qualityIndex] >= item.quality)
                    boolNormal1 = true;
                else
                    boolNormal1 = false;
                
                return boolNormal1;
            }
            //需求修改 现在两个条件是互斥的 
            if (SetBoolList[1])
            {
                //四个条件
                if (item.quality > 4)
                    return false;
                
                if ((item.career != Career && item.career != 0) || (item.sex != sex && item.sex != 2))
                    return true;
                
                
                var list =  CSBagInfo.Instance.GetPosByItem(item);
                if (list.Count == 0)
                    return false;
                //获得装备的列表 最大循环2次 
                for (int i = 0; i < list.Count; i++)
                {
                    ITEM itemcfg;
                    if (ItemTableManager.Instance.TryGetValue(list[i].configId, out itemcfg))
                    {
                        if (itemcfg.level < item.level || itemcfg.quality < item.quality)
                            return false;
                    }
                    
                    
                    //int level = ItemTableManager.Instance.GetItemLevel();
                    //return false;
                }
                
                return true;
            }
            //return boolNormal1 && boolNormal2;
        }
        if (curMode == RecycleMode.M_WOLONG)
        {
            bool boolWolong2 = true; //判断第一个选项是否满足
            bool boolWolong1 = true; //判断第二个选项是否满足
            if (SetBoolList[3])
            {
                if ((item.career != Career && item.career != 0) || (item.sex != sex && item.sex != 2))
                    boolWolong2 = true;
                else
                    boolWolong2 = false;
            }
            if (SetBoolList[2])
            {
                if (callBackSetList.Count != 4)
                    return false;
                var lvList = GetOptionsValue(687);
                int lvIndex = callBackSetList[3];
                if (lvList.Count <= lvIndex)
                    return false;
                //判断如果装备品质和等级小于选中限定,则跳过
                if (lvList[lvIndex] >= item.levClass)
                     boolWolong1 = true;
                 else
                     boolWolong1 = false;
                //return boolWolong1;
            }
            
             if (SetBoolList[3] == false&& SetBoolList[2] == false)
                 return false;
             return boolWolong1 && boolWolong2;
        }
        return false;
    }
    
    
    
    
    
    
    
    #endregion

    #region 回收成功面板使用数据

    public int oldCurExp;
    public int oldCurLv;

    private Dictionary<int, int> _dicTimes;

    /// <summary>
    /// 根据道具数量控制播放时间 第一个参数 为 数量 ,第二个参数为时间
    /// </summary>
    public Dictionary<int, int> DicTimes 
    {
        get
        {
            if (_dicTimes == null)
            {
                _dicTimes = new Dictionary<int, int>();
                string[] temp = SundryTableManager.Instance.GetSundryEffect(722).Split('&');
                for (int i = 0; i < temp.Length; i++)
                {
                    string[] v = temp[i].Split('#');
                    _dicTimes[int.Parse(v[1])] = int.Parse(v[0]);
                }
            }
            
            return _dicTimes;
        } 
        set => _dicTimes = value;
    }

    #endregion

    #region 红点

    public bool PetLevelUpRedPoint()
    {
        // if (normalEquipList == null || wolongEquipList == null)
        //     return false;
        
        if (SelectNormalList?.Count >= 10 || SelectWolongList?.Count >= 10 )
        {
            return true;
        }
        return false;
    }

    
    public bool PetLevelUpRedPointByType(RecycleMode Type)
    {
        // if (normalEquipList == null || wolongEquipList == null)
        //     return false;
        
        if (Type == RecycleMode.M_NORMAL)
        {
            return SelectNormalList?.Count >= 10;
        }
        else
        {
            return SelectWolongList?.Count >= 10;
        }
    }
        
    #endregion

    #region 处理界面的开启

    private System.Action _action;
    public bool JudgeOpenPetLevelUpPanel(System.Action action = null)
    {
        _action = action;
        
        int funcId = 37;
        TABLE.FUNCOPEN funcopenItem;
        if (!FuncOpenTableManager.Instance.TryGetValue(funcId, out funcopenItem))
        {
            return false;
        }
        if (CSMainPlayerInfo.Instance.Level < funcopenItem.needLevel)
        {
            UtilityTips.ShowRedTips(106, funcopenItem.needLevel, funcopenItem.functionName);
            return false;
        }
        if (CSMainPlayerInfo.Instance.VipLevel < 1)
        {
            UtilityTips.ShowPromptWordTips(75, TransMitToOpenRecyclePanel, ToVipPanel);
            return false;
        }
        return  UtilityPanel.JumpToPanel(27101);
    }

    
    void TransMitToOpenRecyclePanel()
    {
        UtilityPath.FindWithDeliverId(1047);
        if (_action != null)
            _action();
    }

    void ToVipPanel()
    {
        UIManager.Instance.CreatePanel<UIVIPPanel>();
        if (_action != null)
            _action();
    }

    #endregion
    
    
    public override void Dispose()
    {
        _resPetInfo = null;
        _resItemCallBackInfo = null;
        //mClientEvent.RemoveEvent(CEvent.ConfigCallback,OnChangeSet);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnItemChange);
        mClientEvent.RemoveEvent(CEvent.WearEquip, OnItemChange);
        mClientEvent.RemoveEvent(CEvent.UnWeatEquip, OnItemChange);
        mClientEvent.RemoveEvent(CEvent.BagSort,OnItemChange);
    }
}

/// <summary>
/// 保存的背包数据用于简化排序
/// </summary>
public class BagClass
{
    public BagItemInfo bagItemInfo;
    public int sortId;
    public int subType;
    public ITEM item;
}


public enum RecycleMode
{
    M_NORMAL = 1,    
    M_WOLONG = 2,
}