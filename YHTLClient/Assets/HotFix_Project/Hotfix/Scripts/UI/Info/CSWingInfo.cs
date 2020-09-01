using System;
using System.Collections.Generic;
using bag;
using Google.Protobuf.Collections;
using TABLE;
using UnityEngine;
using wing;

public class CSWingInfo : CSInfo<CSWingInfo>
{
    public CSWingInfo()
    {
        var arrWing = WingTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arrWing.Length; k < max; ++k)
        {
            var item = arrWing[k].Value as TABLE.WING;
            if (item.starID == 0)
                maxAdvance++;
        }

        for (int k = 0, max = arrWing.Length; k < max; ++k)
        {
            var item = arrWing[k].Value as TABLE.WING;
            if (item.rank == maxAdvance)
                maxStarLevel++;
        }

        //0星开始的
        maxStarLevel--;


        wingSpiritData = mPoolHandle.GetCustomClass<WingSpiritData>();
        List<List<int>> list = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(1053));
        if (list.Count == 3)
        {
            wingUnLockLevel = list[0][1];
            wingColorUnLockLevel = list[1][1];
            wingTechniqueUnLockLevel = list[2][1];
        }

        if (listAttrAddition == null)
            listAttrAddition = new ILBetterList<ILBetterList<int>>();
        var arr = YuLingLevelTableManager.Instance.array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var value = arr[i].Value as YULINGLEVEL;
            if (!string.IsNullOrEmpty(value.exattr))
            {
                ILBetterList<int> listInt = new ILBetterList<int>();
                listInt.Add(value.id);
                listInt.Add(Int32.Parse(value.exattr));
                listAttrAddition.Add(listInt);
            }
        }
    }

    public override void Dispose()
    {
        mPoolHandle?.OnDestroy();
        mPoolHandle = null;
        wingSpiritData.Dispose();
    }

    PoolHandleManager mPoolHandle = new PoolHandleManager();

    int maxAdvance = 0;
    public int MaxAdvance => maxAdvance;
    int maxStarLevel = 0;
    public int MaxStarLevel => maxStarLevel;

    /// <summary>
    /// 羽翼之魂解锁羽灵等级
    /// </summary>
    private int wingUnLockLevel = 0;

    public int WingUnLockLevel => wingUnLockLevel;

    /// <summary>
    /// 羽技之魂解锁羽灵等级
    /// </summary>
    private int wingTechniqueUnLockLevel = 0;

    public int WingTechniqueUnLockLevel => wingTechniqueUnLockLevel;

    /// <summary>
    /// 幻彩之魂解锁羽灵等级
    /// </summary>
    private int wingColorUnLockLevel = 0;

    public int WingColorUnLockLevel => wingColorUnLockLevel;

    /// <summary>
    /// 我的翅膀信息
    /// </summary>
    WingData myWingData;

    public WingData MyWingData => myWingData;

    /// <summary>
    /// 我的羽灵信息
    /// </summary>
    private WingSpiritData wingSpiritData;

    public WingSpiritData WingSpiritData => wingSpiritData;

    /// <summary>
    /// 羽灵加成信息
    /// </summary>
    private ILBetterList<ILBetterList<int>> listAttrAddition = new ILBetterList<ILBetterList<int>>();

    public ILBetterList<ILBetterList<int>> ListAttrAddition => listAttrAddition;

    /// <summary>
    /// 槽位信息
    /// </summary>
    private Dictionary<int, WingSoulData> dicSlotData = new Dictionary<int, WingSoulData>();

    public Dictionary<int, WingSoulData> DicSlotData => dicSlotData;

    /// <summary>
    /// 槽位下一级羽灵之魂Id
    /// </summary>
    private Dictionary<int, int> dicSlotNextId = new Dictionary<int, int>();

    public Dictionary<int, int> DicSlotNextId => dicSlotNextId;


    /// <summary>
    /// 获取没有羽灵之魂时基础属性
    /// </summary>
    /// <param name="career"></param>
    /// <returns></returns>
    public List<List<int>> GetListNoWingSoulAttrs(int career)
    {
        List<List<int>> list = null;
        switch (career)
        {
            case 1:
                list = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(1072));
                break;
            case 2:
                list = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(1073));
                break;
            case 3:
                list = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(1074));
                break;
        }

        return list;
    }

    /// <summary>
    /// 设置羽翼属性加成
    /// </summary>
    /// <param name="arrInfo"></param>
    public void SetWingAddition(CSBetterLisHot<IntArray> arrInfo, RepeatedField<int> ids, RepeatedField<int> values,
        int career)
    {
        if (arrInfo == null || ids == null || values == null) return;
        ids.Clear();
        values.Clear();
        for (int i = 0, max = arrInfo.Count; i < max; i++)
        {
            IntArray intArray = arrInfo[i];
            if (i == 0)
            {
                for (int j = 0, max1 = intArray.Count; j < max1; j++)
                {
                    ids.Add(intArray[j]);
                }
            }
            else if (i == 1)
            {
                for (int j = 0, max1 = intArray.Count; j < max1; j++)
                {
                    values.Add(intArray[j]);
                }
            }
        }

        if (dicSlotData.ContainsKey(1) && dicSlotData[1].YulingsoulCfg != null)
        {
            LongArray array = new LongArray();
            switch (career)
            {
                case 1:
                    array = dicSlotData[1].YulingsoulCfg.zsexattr;
                    break;
                case 2:
                    array = dicSlotData[1].YulingsoulCfg.fsexattr;
                    break;
                case 3:
                    array = dicSlotData[1].YulingsoulCfg.dsexattr;
                    break;
            }

            for (int i = 0, max = array.Count; i < max; i++)
            {
                int key = array[i].key();
                int value = array[i].value();

                for (int j = 0, max1 = ids.Count; j < max1; j++)
                {
                    if (ids[j] == key)
                    {
                        float temp = (float) values[i];
                        temp *= ((value + 10000) * 1f / 10000);
                        values[i] = (int) temp;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 设置幻彩属性加成
    /// </summary>
    /// <param name="arrInfo"></param>
    public void SetWingColorAddition(CSBetterLisHot<IntArray> arrInfo, RepeatedField<int> ids,
        RepeatedField<int> values, int career)
    {
        if (arrInfo == null || ids == null || values == null) return;
        ids.Clear();
        values.Clear();
        for (int i = 0, max = arrInfo.Count; i < max; i++)
        {
            IntArray intArray = arrInfo[i];
            if (i == 0)
            {
                for (int j = 0, max1 = intArray.Count; j < max1; j++)
                {
                    ids.Add(intArray[j]);
                }
            }
            else if (i == 1)
            {
                for (int j = 0, max1 = intArray.Count; j < max1; j++)
                {
                    values.Add(intArray[j]);
                }
            }
        }

        if (dicSlotData.ContainsKey(2) && dicSlotData[2].YulingsoulCfg != null)
        {
            LongArray array = new LongArray();
            switch (career)
            {
                case 1:
                    array = dicSlotData[2].YulingsoulCfg.zsexattr;
                    break;
                case 2:
                    array = dicSlotData[2].YulingsoulCfg.fsexattr;
                    break;
                case 3:
                    array = dicSlotData[2].YulingsoulCfg.dsexattr;
                    break;
            }

            for (int i = 0, max = array.Count; i < max; i++)
            {
                int key = array[i].key();
                int value = array[i].value();

                for (int j = 0, max1 = ids.Count; j < max1; j++)
                {
                    if (ids[j] == key)
                    {
                        float temp = (float) values[i];
                        temp *= ((value + 10000) * 1f / 10000);
                        values[i] = (int) temp;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 获取羽技之魂加成
    /// </summary>
    /// <returns></returns>
    public int GetWingTechniqueAddition()
    {
        int wingSoulAddition = wingSpiritData.Addition;

        int addition = 0;
        if (dicSlotData.ContainsKey(3) && dicSlotData[3].YulingsoulCfg != null)
            addition = dicSlotData[3].YulingsoulCfg.exattr;

        addition = (int) (addition * (10000 + wingSoulAddition) * 1f / 10000);
        return addition;
    }

    /// <summary>
    /// 根据职业获取羽灵相应属性id和数值
    /// </summary>
    /// <param name="career"></param>
    public LongArray GetAttrParaByCareer(int career, YULINGLEVEL data)
    {
        switch (career)
        {
            case 1:
                return data.zsattr;
            case 2:
                return data.fsattr;
            case 3:
                return data.dsattr;
        }

        return data.zsattr;
    }

    /// <summary>
    /// 根据职业获取羽灵之魂相应相应基础属性id和数值
    /// </summary>
    /// <param name="career"></param>
    public LongArray GetBaseAttrParaByCareer(int career, YULINGSOUL data)
    {
        switch (career)
        {
            case 1:
                return data.zsattr;
            case 2:
                return data.fsattr;
            case 3:
                return data.dsattr;
        }

        return data.zsattr;
    }

    /// <summary>
    /// 根据职业获取羽灵之魂相应相应特殊属性id和数值
    /// </summary>
    /// <param name="career"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public LongArray GetSpecialAttrParaByCareer(int career, YULINGSOUL data)
    {
        switch (career)
        {
            case 1:
                return data.zsexattr;
            case 2:
                return data.fsexattr;
            case 3:
                return data.dsexattr;
        }

        return data.zsexattr;
    }

    private ILBetterList<BagItemInfo> listWingSoulList = new ILBetterList<BagItemInfo>();

    /// <summary>
    /// 获取已排序好的羽翼之魂列表
    /// </summary>
    /// <returns></returns>
    public ILBetterList<BagItemInfo> GetWingSoulList()
    {
        CSBagInfo.Instance.SetItemsByTypeAndSubType(listWingSoulList, 9, 6);
        listWingSoulList.Sort(SortWingSoulList);
        return listWingSoulList;
    }

    private ILBetterList<BagItemInfo> listWingColorSoulList = new ILBetterList<BagItemInfo>();

    /// <summary>
    /// 获取已排序好的幻彩之魂列表
    /// </summary>
    /// <returns></returns>
    public ILBetterList<BagItemInfo> GetWingColorSoulList()
    {
        CSBagInfo.Instance.SetItemsByTypeAndSubType(listWingColorSoulList, 9, 7);
        listWingColorSoulList.Sort(SortWingSoulList);
        return listWingColorSoulList;
    }

    private ILBetterList<BagItemInfo> listWingTechniqueList = new ILBetterList<BagItemInfo>();

    /// <summary>
    /// 获取已排序好的羽技之魂列表
    /// </summary>
    /// <returns></returns>
    public ILBetterList<BagItemInfo> GetWingTechniqueSoulList()
    {
        CSBagInfo.Instance.SetItemsByTypeAndSubType(listWingTechniqueList, 9, 8);
        listWingTechniqueList.Sort(SortWingSoulList);
        return listWingTechniqueList;
    }


    int SortWingSoulList(BagItemInfo a, BagItemInfo b)
    {
        return -(YuLingSoulTableManager.Instance.GetYuLingSoulLevel(a.configId)
            .CompareTo(YuLingSoulTableManager.Instance.GetYuLingSoulLevel(b.configId)));
    }

    /// <summary>
    /// 是否显示羽翼入口红点
    /// </summary>
    /// <returns></returns>
    public bool IsActiveRedPointFunction()
    {
        return IsUpStar() || HasActive() || IsHasBetterYulingSoul() || IsEnoughPromote();
    }

    /// <summary>
    /// 是否显示羽灵页签红点
    /// </summary>
    /// <returns></returns>
    public bool IsActiveRedPointYuLing()
    {
        return IsHasBetterYulingSoul() || IsEnoughPromote();
    }

    /// <summary>
    /// 是否有更好羽灵之魂
    /// </summary>
    /// <returns></returns>
    public bool IsHasBetterYulingSoul()
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_WingSoul)) return false;
        return IsHasBetterWing() || IsHasBetterWingColor() || IsHasBetterWingTechnique();
    }

    /// <summary>
    /// 羽灵是否满足升级条件
    /// </summary>
    /// <returns></returns>
    public bool IsEnoughPromote()
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_WingSoul)) return false;
        if (!wingSpiritData.isMax && wingSpiritData.YulinglevelCfg != null)
        {
            if (wingSpiritData.YulinglevelCfg.levelUp.Count > 0 &&
                wingSpiritData.YulinglevelCfg.levelUp.IsItemsEnough())
                return true;
        }

        return false;
    }

    private ILBetterList<bag.BagItemInfo> wingBagItemInfos = new ILBetterList<BagItemInfo>();

    /// <summary>
    /// 是否拥有更好的羽翼之魂
    /// </summary>
    /// <returns></returns>
    public bool IsHasBetterWing()
    {
        if (dicSlotData.Count <= 0 || !dicSlotData.ContainsKey(1) || dicSlotData[1].IsSoulMax) return false;
        if (wingSpiritData.YuLingLevelId < wingUnLockLevel) return false;
        CSBagInfo.Instance.SetItemsByTypeAndSubType(wingBagItemInfos, 9, 6);
        if (wingBagItemInfos.Count > 0)
        {
            if (dicSlotData[1].yuLingSoulId <= 0)
                return true;

            for (int i = 0, max = wingBagItemInfos.Count; i < max; i++)
            {
                BagItemInfo bagItemInfo = wingBagItemInfos[i];
                if (YuLingSoulTableManager.Instance.GetYuLingSoulLevel(bagItemInfo.configId) >
                    YuLingSoulTableManager.Instance.GetYuLingSoulLevel(dicSlotData[1].yuLingSoulId))
                    return true;
            }
        }

        return false;
    }


    private ILBetterList<bag.BagItemInfo> wingColorBagItemInfos = new ILBetterList<BagItemInfo>();

    /// <summary>
    /// 是否拥有更好的幻彩之魂
    /// </summary>
    /// <returns></returns>
    public bool IsHasBetterWingColor()
    {
        if (dicSlotData.Count <= 0 || !dicSlotData.ContainsKey(2) || dicSlotData[2].IsSoulMax) return false;
        if (wingSpiritData.YuLingLevelId < wingColorUnLockLevel) return false;
        CSBagInfo.Instance.SetItemsByTypeAndSubType(wingColorBagItemInfos, 9, 7);
        if (wingColorBagItemInfos.Count > 0)
        {
            if (dicSlotData[2].yuLingSoulId <= 0)
                return true;

            for (int i = 0, max = wingColorBagItemInfos.Count; i < max; i++)
            {
                BagItemInfo bagItemInfo = wingColorBagItemInfos[i];
                if (YuLingSoulTableManager.Instance.GetYuLingSoulLevel(bagItemInfo.configId) >
                    YuLingSoulTableManager.Instance.GetYuLingSoulLevel(dicSlotData[2].yuLingSoulId))
                    return true;
            }
        }

        return false;
    }

    private ILBetterList<bag.BagItemInfo> wingTechniqueBagItemInfos = new ILBetterList<BagItemInfo>();

    /// <summary>
    /// 是否拥有更好的羽技之魂
    /// </summary>
    /// <returns></returns>
    public bool IsHasBetterWingTechnique()
    {
        if (dicSlotData.Count <= 0 || !dicSlotData.ContainsKey(3) || dicSlotData[3].IsSoulMax) return false;
        if (wingSpiritData.YuLingLevelId < wingTechniqueUnLockLevel) return false;
        CSBagInfo.Instance.SetItemsByTypeAndSubType(wingTechniqueBagItemInfos, 9, 8);
        if (wingTechniqueBagItemInfos.Count > 0)
        {
            if (dicSlotData[3].yuLingSoulId <= 0)
                return true;

            for (int i = 0, max = wingTechniqueBagItemInfos.Count; i < max; i++)
            {
                BagItemInfo bagItemInfo = wingTechniqueBagItemInfos[i];
                if (YuLingSoulTableManager.Instance.GetYuLingSoulLevel(bagItemInfo.configId) >
                    YuLingSoulTableManager.Instance.GetYuLingSoulLevel(dicSlotData[3].yuLingSoulId))
                    return true;
            }
        }

        return false;
    }


    /// <summary>
    /// 判断传入的id和槽位中的哪个更高级
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool judgebodyWingSoulByBag(int id, int position)
    {
        if (dicSlotData != null && dicSlotData.ContainsKey(position) && dicSlotData[position] != null)
        {
            if (dicSlotData[position].lockFlag == 0)
                return false;
            else if (dicSlotData[position].lockFlag == 1)
                return true;
            else if (dicSlotData[position].lockFlag == 2)
            {
                TABLE.YULINGSOUL yulingsoul;
                if (YuLingSoulTableManager.Instance.TryGetValue(id, out yulingsoul))
                    return yulingsoul.level >
                           YuLingSoulTableManager.Instance.GetYuLingSoulLevel(dicSlotData[position].yuLingSoulId);
            }
        }

        return false;
    }

    /// <summary>
    /// 是否有足够材料可以升星翅膀
    /// </summary>
    /// <returns></returns>
    public bool IsUpStar()
    {
        if (myWingData == null) return false;
        TABLE.WING wing;
        if (!WingTableManager.Instance.TryGetValue(myWingData.id, out wing)) return false;

        //满阶满星:
        if (wing.rank == maxAdvance && wing.starID == maxStarLevel)
            return false;

        //非满阶满星:
        int curExp = myWingData.curExp;
        int maxExp = (int) wing.starCostExp;
        //当前升星需要的经验
        int difference = maxExp - curExp;
        //每次上缴的装备和数量
        var equips = wing.starCost;

        //当前可上缴最多次数
        int maxTurnInCount = 0;
        for (int i = 0; i < equips.Count; i++)
        {
            long curCount = CSBagInfo.Instance.GetAllItemCount(equips[i].key());
            int turnInCount = Mathf.FloorToInt((float) curCount / equips[i].value());
            if (i == 0)
            {
                maxTurnInCount = turnInCount;
            }
            else
            {
                if (turnInCount < maxTurnInCount)
                {
                    maxTurnInCount = turnInCount;
                }
            }
        }

        //上缴最大可加经验
        int addExp = 0;
        for (int i = 0; i < equips.Count; i++)
        {
            if (ItemTableManager.Instance.TryGetValue(equips[i].key(), out TABLE.ITEM Cfg))
            {
                if (Cfg.type != 1 /*不是货币*/ && Cfg.bufferParam != null)
                {
                    List<int> bufferParam =
                        UtilityMainMath.SplitStringToIntList(Cfg.bufferParam);
                    if (bufferParam.Count > 0)
                    {
                        addExp += maxTurnInCount * equips[i].value() * bufferParam[0];
                    }
                }
            }
        }

        return addExp >= difference;
    }

    /// <summary>
    /// 是否有可以激活的幻彩
    /// </summary>
    /// <returns></returns>
    public bool HasActive()
    {
        if (myWingData == null) return false;
        if (myWingData.GetWingColorDatas().Count == 0) return false;
        for (int i = 0, max = myWingData.GetWingColorDatas().Count; i < max; i++)
        {
            WingColorData wingColorData = myWingData.GetWingColorDatas()[i];
            if (wingColorData.endTime == 0 && wingColorData.count > 0)
                return true;
        }

        return false;
    }

    #region 网络响应处理函数

    /// <summary>
    /// 获取翅膀信息处理
    /// </summary>
    /// <param name="data"></param>
    public void GetWingInfo(wing.WingInfoResponse data)
    {
        if (data == null) return;
        if (myWingData == null)
        {
            myWingData = new WingData();
        }

        myWingData.id = data.wingInfo.wingId;
        myWingData.curExp = data.wingInfo.curExp;
        myWingData.wingColorId = data.wingInfo.wingColorId;

        myWingData.wingColorInfos.Clear();
        for (int i = 0; i < data.wingColorInfos.Count; i++)
        {
            myWingData.wingColorInfos.Add(data.wingColorInfos[i]);
        }
    }

    /// <summary>
    /// 翅膀升星处理
    /// </summary>
    /// <param name="data"></param>
    public void HandleWingUpStar(wing.WingUpStarResponse data)
    {
        if (data == null || myWingData == null) return;
        myWingData.id = data.wingInfo.wingId;
        myWingData.curExp = data.wingInfo.curExp;
        myWingData.wingColorId = data.wingInfo.wingColorId;
    }

    /// <summary>
    /// 穿戴/脱下幻彩道具处理
    /// </summary>
    /// <param name="data"></param>
    public void HandleDressColorWing(wing.DressColorWingResponse data)
    {
        if (data == null || myWingData == null) return;
        myWingData.wingColorId = data.type == 1 ? data.itemId : 0;
    }

    /// <summary>
    /// 翅膀升阶处理
    /// </summary>
    /// <param name="data"></param>
    public void HandleWingAdvance(wing.WingAdvanceResponse data)
    {
        if (data == null || myWingData == null) return;
        myWingData.id = data.wingInfo.wingId;
        myWingData.curExp = data.wingInfo.curExp;
        myWingData.wingColorId = data.wingInfo.wingColorId;
        // UIManager.Instance.CreatePanel<UIWingAdvanceSuccessPanel>(
        //     f => { (f as UIWingAdvanceSuccessPanel).OpenWingAdvanceSuccess(data.wingInfo); });
    }

    /// <summary>
    /// 翅膀经验丹使用处理
    /// </summary>
    /// <param name="data"></param>
    public void HandleWingExpItemUse(wing.WingExpItemUseResponse data)
    {
        if (data == null) return;
        TABLE.WING wingTable1;
        TABLE.WING wingTable2;
        if (!WingTableManager.Instance.TryGetValue(myWingData.id, out wingTable1)) return;
        if (!WingTableManager.Instance.TryGetValue(data.wingInfo.wingId, out wingTable2)) return;
        if (wingTable2.rank > wingTable1.rank)
        {
            //升阶
            UIManager.Instance.CreatePanel<UIWingAdvanceSuccessPanel>();
        }

        myWingData.id = data.wingInfo.wingId;
        myWingData.curExp = data.wingInfo.curExp;
        myWingData.wingColorId = data.wingInfo.wingColorId;
    }

    /// <summary>
    /// 处理幻彩道具变更
    /// </summary>
    /// <param name="msg"></param>
    public void HandleColorWingChange(wing.WingColorChange msg)
    {
        if (msg == null) return;
        for (int i = 0; i < msg.wingColorInfos.Count; i++)
        {
            WingColorInfo msgInfo = msg.wingColorInfos[i];
            bool isExist = false;
            for (int j = 0; j < myWingData.wingColorInfos.Count; j++)
            {
                WingColorInfo myWingColorInfo = myWingData.wingColorInfos[j];
                if (msgInfo.id == myWingColorInfo.id)
                {
                    if (msgInfo.endTime == 0)
                    {
                        myWingData.wingColorInfos.RemoveAt(j);
                    }
                    else
                    {
                        myWingData.wingColorInfos[j] = msgInfo;
                    }

                    isExist = true;
                }
            }

            if (!isExist && msgInfo.endTime != 0)
            {
                myWingData.wingColorInfos.Add(msgInfo);
            }
        }
    }

    /// <summary>
    /// 处理羽灵信息
    /// </summary>
    /// <param name="msg"></param>
    public void HandleYuLingInfo(wing.ResYuLingInfo msg)
    {
        if (msg == null) return;
        dicSlotData.Clear();
        dicSlotData.Add(1, null);
        dicSlotData.Add(2, null);
        dicSlotData.Add(3, null);
        wingSpiritData.YuLingLevelId = msg.id;
        wingSpiritData.isMax = msg.isMax;
        wingSpiritData.Addition = msg.addition;
        if (YuLingLevelTableManager.Instance.TryGetValue(msg.id, out TABLE.YULINGLEVEL CfgLevel))
            wingSpiritData.YulinglevelCfg = CfgLevel;

        if (!msg.isMax)
        {
            if (YuLingLevelTableManager.Instance.TryGetValue(msg.id + 1, out TABLE.YULINGLEVEL NextCfgLevel))
                wingSpiritData.NextYulinglevelCfg = NextCfgLevel;
        }

        wingSpiritData.WingSoulDatas.Clear();
        for (int i = 0, max = msg.yuLingInfo.Count; i < max; i++)
        {
            var Item = msg.yuLingInfo[i];
            WingSoulData wingSoulData = mPoolHandle.GetCustomClass<WingSoulData>();
            wingSoulData.Position = Item.position;
            wingSoulData.yuLingSoulId = Item.yuLingSoulId;
            wingSoulData.lockFlag = Item.lockFlag;
            wingSoulData.IsSoulMax = Item.isSoulMax;
            if (YuLingSoulTableManager.Instance.TryGetValue(Item.yuLingSoulId, out TABLE.YULINGSOUL CfgSoul))
                wingSoulData.YulingsoulCfg = CfgSoul;

            wingSpiritData.WingSoulDatas.Add(wingSoulData);

            if (!dicSlotData.ContainsKey(Item.position))
                dicSlotData.Add(Item.position, wingSoulData);
            else
                dicSlotData[Item.position] = wingSoulData;
        }


        dicSlotNextId.Clear();
        var arr = YuLingSoulTableManager.Instance.array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var value = arr[i].Value as YULINGSOUL;
            if (dicSlotData.ContainsKey(1) && value.position == 1 && dicSlotData[1].YulingsoulCfg != null)
            {
                if (dicSlotData[1].IsSoulMax && dicSlotData[1].YulingsoulCfg.id == value.id)
                    dicSlotNextId.Add(1, 0);
                else
                {
                    if (value.level == dicSlotData[1].YulingsoulCfg.level + 1)
                        dicSlotNextId.Add(1, value.id);
                }
            }

            if (dicSlotData.ContainsKey(2) && value.position == 2 && dicSlotData[2].YulingsoulCfg != null)
            {
                if (dicSlotData[2].IsSoulMax && dicSlotData[2].YulingsoulCfg.id == value.id)
                    dicSlotNextId.Add(2, 0);
                else
                {
                    if (value.level == dicSlotData[2].YulingsoulCfg.level + 1)
                        dicSlotNextId.Add(2, value.id);
                }
            }

            if (dicSlotData.ContainsKey(3) && value.position == 3 && dicSlotData[3].YulingsoulCfg != null)
            {
                if (dicSlotData[3].IsSoulMax && dicSlotData[3].YulingsoulCfg.id == value.id)
                    dicSlotNextId.Add(3, 0);
                else
                {
                    if (value.level == dicSlotData[3].YulingsoulCfg.level + 1)
                        dicSlotNextId.Add(3, value.id);
                }
            }
        }
    }

    #endregion
}

/// <summary>
/// 羽灵信息
/// </summary>
public class WingSpiritData : IDispose
{
    /// <summary>
    /// 羽灵等级表Id
    /// </summary>
    public int YuLingLevelId { get; set; }

    /// <summary>
    /// 是否满级
    /// </summary>
    public bool isMax { get; set; }

    /// <summary>
    /// 加成万分比
    /// </summary>
    public int Addition { get; set; }

    /// <summary>
    /// 当前等级羽灵配置信息
    /// </summary>
    private TABLE.YULINGLEVEL yulinglevelCfg;

    public YULINGLEVEL YulinglevelCfg
    {
        get => yulinglevelCfg;
        set => yulinglevelCfg = value;
    }

    /// <summary>
    /// 下一级羽灵配置信息
    /// </summary>
    private TABLE.YULINGLEVEL nextYulinglevelCfg;

    public YULINGLEVEL NextYulinglevelCfg
    {
        get => nextYulinglevelCfg;
        set => nextYulinglevelCfg = value;
    }

    ILBetterList<WingSoulData> wingSoulDatas = new ILBetterList<WingSoulData>();

    public ILBetterList<WingSoulData> WingSoulDatas
    {
        get => wingSoulDatas;
        set => wingSoulDatas = value;
    }

    public void Dispose()
    {
        yulinglevelCfg = null;
        nextYulinglevelCfg = null;
        wingSoulDatas?.Clear();
        wingSoulDatas = null;
    }
}


/// <summary>
/// 羽灵之魂信息
/// </summary>
public class WingSoulData : IDispose
{
    /// <summary>
    /// 槽位索引
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// 镶嵌的羽灵之魂Id
    /// </summary>
    public int yuLingSoulId { get; set; }

    /// <summary>
    /// 0为未解锁，1未镶嵌，2已镶嵌
    /// </summary>
    public int lockFlag { get; set; }

    /// <summary>
    /// 是否满级
    /// </summary>
    public bool IsSoulMax { get; set; }

    /// <summary>
    /// 羽灵之魂配置
    /// </summary>
    private TABLE.YULINGSOUL yulingsoulCfg;

    public YULINGSOUL YulingsoulCfg
    {
        get => yulingsoulCfg;
        set => yulingsoulCfg = value;
    }

    public void Dispose()
    {
        yulingsoulCfg = null;
    }
}


/// <summary>
/// 翅膀信息
/// </summary>
public class WingData
{
    /// <summary>
    /// 已装备翅膀唯一Id
    /// </summary>
    public int id = 0;

    /// <summary>
    /// 当前经验
    /// </summary>
    public int curExp = 0;

    /// <summary>
    /// 已装备幻彩Id
    /// </summary>
    public int wingColorId = 0;

    /// <summary>
    /// 幻彩列表(服务器给的已激活的)
    /// </summary>
    public ILBetterList<WingColorInfo> wingColorInfos = new ILBetterList<WingColorInfo>();

    private ILBetterList<WingColorData> tempList = new ILBetterList<WingColorData>();
    private ILBetterList<bag.BagItemInfo> wingColorsForBag = new ILBetterList<BagItemInfo>();
    private ILBetterList<WingColorData> list = new ILBetterList<WingColorData>();
    private ILBetterList<WingColorData> listWingColorDataActive = new ILBetterList<WingColorData>();
    private ILBetterList<WingColorData> listWingColorDataNoActive = new ILBetterList<WingColorData>();

    /// <summary>
    /// 我的幻彩列表(自己整理的,包括背包的和服务器给的)
    /// </summary>
    public ILBetterList<WingColorData> GetWingColorDatas()
    {
        tempList.Clear();
        //添加已激活的
        for (int i = 0; i < wingColorInfos.Count; i++)
        {
            WingColorInfo temp = wingColorInfos[i];
            if (listWingColorDataActive.Count <= i)
                listWingColorDataActive.Add(new WingColorData());

            WingColorData wingColorData = listWingColorDataActive[i];
            wingColorData.configId = temp.id;
            wingColorData.endTime = temp.endTime; //已激活的只有-1(永久)和>0(限时已激活)两种情况
            wingColorData.count = 0; //用于显示数量(已激活的不算进数量)
            tempList.Add(wingColorData);
        }
        
        //添加背包中未激活的
        CSBagInfo.Instance.SetItemsByTypeAndSubType(wingColorsForBag, 5, 9);
        BagItemInfo bagItemInfo;
        for (int i = 0; i < wingColorsForBag.Count; i++)
        {
            bagItemInfo = wingColorsForBag[i];
            if (listWingColorDataNoActive.Count<=i)
                listWingColorDataNoActive.Add(new WingColorData());
            WingColorData wingColorData = listWingColorDataNoActive[i];
            wingColorData.configId = bagItemInfo.configId;
            wingColorData.endTime = 0; //未激活的只有0(表示已获得且未激活)
            wingColorData.count = bagItemInfo.count; //用于显示数量
            wingColorData.id = bagItemInfo.id;
            wingColorData.bagIndex = bagItemInfo.bagIndex;
            tempList.Add(wingColorData);
        }

        //合并数量
        list.Clear();
        WingColorData colorData;
        for (int i = 0; i < tempList.Count; i++)
        {
            colorData = tempList[i];
            bool isExist = false;
            WingColorData temp1;
            for (int j = 0; j < list.Count; j++)
            {
                temp1 = list[j];
                if (temp1.configId == colorData.configId)
                {
                    if (colorData.endTime != 0)
                        temp1.endTime = colorData.endTime;

                    temp1.count += colorData.count;
                    isExist = true;
                }
            }

            if (!isExist)
                list.Add(colorData);
        }

        return list;
    }
}

/// <summary>
/// 单个幻彩结构
/// </summary>
public class WingColorData
{
    public int configId;
    public long endTime;
    public int count;
    public long id;
    public int bagIndex;
}