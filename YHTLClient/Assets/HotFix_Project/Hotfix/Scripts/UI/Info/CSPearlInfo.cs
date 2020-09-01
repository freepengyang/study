using System;
using System.Collections;
using System.Collections.Generic;
using bag;
using TABLE;
using UnityEngine;
using wing;

/// <summary>
/// 进化宝珠
/// </summary>
public class CSPearlInfo : CSInfo<CSPearlInfo>
{
    public CSPearlInfo()
    {
    }

    public override void Dispose()
    {
        Pool.RecycleAll();
       
    }

    private PearlData myPearlData = new PearlData();

    /// <summary>
    /// 进化宝珠信息
    /// </summary>
    public PearlData MyPearlData
    {
        get { return myPearlData; }
    }

    /// <summary>
    /// 获取当前阶数宝珠升阶需要击杀的Boss等级和数量条件(如果是最高阶则返回null)
    /// </summary>
    /// <param name="grade">宝珠阶数</param>
    /// <returns></returns>
    public List<int> GetPearlBossCondition(int grade)
    {
        List<int> listLevelAndNum = new List<int>();
        listLevelAndNum.Clear();
        var arr = BaoZhuJinHuaTableManager.Instance.array.gItem.handles;
        bool isMaxGrade = true;
        for(int i = 0,max = arr.Length;i < max;++i)
        {
            var item = arr[i].Value as TABLE.BAOZHUJINHUA;
            if (item.rank > grade)
            {
                isMaxGrade = false;
                break;
            }
        }

        var dic = BaoZhuJinHuaTableManager.Instance.array.gItem.id2offset;

        if (!isMaxGrade && dic.ContainsKey(grade))
        {
            var handle = dic[grade].Value as TABLE.BAOZHUJINHUA;
            listLevelAndNum.Add(handle.bossLevel);
            listLevelAndNum.Add(handle.bossNum);
            return listLevelAndNum;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 宝珠在当前阶数是否最高等级
    /// </summary>
    ///  /// <param name="grade">当前宝珠阶数</param>
    /// <param name="level">当前宝珠等级</param>
    /// <returns></returns>
    public bool IsMaxLevel(int grade, int level)
    {
        bool isMaxLevel = false;
        var mapJinHua = BaoZhuJinHuaTableManager.Instance.array.gItem.id2offset;
        if (mapJinHua == null) 
            return isMaxLevel;

        if(!mapJinHua.ContainsKey(grade))
        {
            return false;
        }

        if ((mapJinHua[grade].Value as TABLE.BAOZHUJINHUA).maxLevel == level)
        {
            isMaxLevel = true;
        }

        return isMaxLevel;
    }

    /// <summary>
    /// 宝珠是否最高阶
    /// </summary>
    /// <param name="grade">宝珠当前阶数</param>
    /// <returns></returns>
    public bool IsMaxGrade(int grade)
    {
        bool isMaxGrade = true;
        var arr = BaoZhuJinHuaTableManager.Instance.array.gItem.handles;
        for(int i = 0,max = arr.Length;i < max;++i)
        {
            var item = arr[i].Value as TABLE.BAOZHUJINHUA;
            if (item.rank > grade)
            {
                isMaxGrade = false;
                break;
            }
        }

        return isMaxGrade;
    }

    /// <summary>
    /// 获取当前已有技能技能槽
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, int> GetSkillSlot()
    {
        if (myPearlData != null && myPearlData.SkillSlot != null)
        {
            return myPearlData.SkillSlot;
        }

        return null;
    }

    /// <summary>
    /// 获取所有技能槽信息
    /// </summary>
    /// <returns></returns>
    public List<SkillSlotData> GetListSkillSlotData()
    {
        return myPearlData.ListSkillSlotDatas;
    }

    /// <summary>
    /// 技能槽排序，按技能槽id从小到大
    /// </summary>
    public List<SkillSlotData> GetSortSkillSolt()
    {
        List<SkillSlotData> list = new List<SkillSlotData>();
        var arr = BaoZhuSkillLibHighTableManager.Instance.array.gItem.handles;
        if (arr == null) return list;
        Dictionary<int, List<int>> dicSkillSlot = new Dictionary<int, List<int>>();
        dicSkillSlot.Clear();
        if (myPearlData.SkillSlot != null)
        {
            var iter = myPearlData.SkillSlot.GetEnumerator();
            while (iter.MoveNext())
            {
                List<int> templist = new List<int>();
                templist.Add(iter.Current.Value);
                dicSkillSlot.Add(iter.Current.Key, templist);
            }
        }

        TABLE.BAOZHUSKILLLIBHIGH skillItem = null;
        for(int i = 0,max = arr.Length;i < max;++i)
        {
            skillItem = arr[i].Value as TABLE.BAOZHUSKILLLIBHIGH;
            if (skillItem.sign != 0)
            {
                List<int> templist = new List<int>();
                if (!dicSkillSlot.ContainsKey(skillItem.slot))
                {
                    templist.Add(0);
                    templist.Add(skillItem.library);
                    dicSkillSlot.Add(skillItem.slot, templist);
                }
                else
                {
                    dicSkillSlot[skillItem.slot].Add(skillItem.library);
                }
            }
        }

        //这里把字典转成一个链表，因为字典是没有办法直接进行排序的。
        List<KeyValuePair<int, List<int>>> LineListDic = new List<KeyValuePair<int, List<int>>>(dicSkillSlot);
        CSBetterLisHot<KeyValuePair<int, List<int>>> Linelist = new CSBetterLisHot<KeyValuePair<int, List<int>>>();
        Linelist.Clear();
        for (int i = 0; i < LineListDic.Count; i++)
        {
            Linelist.Add(LineListDic[i]);
        }

        //利用链表的Sort方法进行排序
        Linelist.Sort(
            delegate(KeyValuePair<int, List<int>> s1, KeyValuePair<int, List<int>> s2)
            {
                return s1.Key.CompareTo(s2.Key);
            });
        dicSkillSlot.Clear();
        //背包中宝珠最大阶数
        int maxBaoZhuGrade = 0;
        for (int i = 0; i < myPearlData.ListPearl.Count; i++)
        {
            if (myPearlData.ListPearl[i].gemGrade > maxBaoZhuGrade)
            {
                maxBaoZhuGrade = myPearlData.ListPearl[i].gemGrade;
            }
        }

        //最大解锁的技能槽位Id
        int maxUnlockId = BaoZhuJinHuaTableManager.Instance.GetBaoZhuJinHuaSkillSlotID(maxBaoZhuGrade);

        list.Clear();
        for (int i = 0; i < Linelist.Count; i++)
        {
            SkillSlotData skillSlotData = new SkillSlotData();
            skillSlotData.SkillSlotId = Linelist[i].Key;
            skillSlotData.SkillId = Linelist[i].Value[0];
            skillSlotData.LibID = Linelist[i].Value[1];
            skillSlotData.IsUnlock = (skillSlotData.SkillSlotId <= maxUnlockId);
            var arrJinHua = BaoZhuJinHuaTableManager.Instance.array.gItem.handles;
            if (arrJinHua != null)
            {
                for(int k = 0,max = arrJinHua.Length;k < max;++k)
                {
                    var item = arrJinHua[k].Value as TABLE.BAOZHUJINHUA;
                    if (item.skillSlotID == skillSlotData.SkillSlotId)
                    {
                        skillSlotData.BaoZhuGradeMinUnlock = item.id;
                        break;
                    }
                }
            }

            list.Add(skillSlotData);
        }

        return list;
    }


    /// <summary>
    /// 是否有可进化的宝珠
    /// </summary>
    /// <returns></returns>
    public bool HasEvolution()
    {
        bag.BagItemInfo equipPearl = myPearlData.EquipPearl;
        if (equipPearl == null) return false;
        int curBossCount = equipPearl.gemBossCounter;
        int maxBossCount = BaoZhuJinHuaTableManager.Instance.GetBaoZhuJinHuaBossNum(equipPearl.gemGrade);
        return curBossCount >= maxBossCount;
    }

    /// <summary>
    /// 是否有不存在技能的技能槽
    /// </summary>
    /// <returns></returns>
    public bool HasNonSkillSlot()
    {
        for (int i = 0; i < myPearlData.ListSkillSlotDatas.Count; i++)
        {
            if (myPearlData.ListSkillSlotDatas[i].IsUnlock && myPearlData.ListSkillSlotDatas[i].SkillId == 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 获取宝珠当前阶数属性加成万分比整数(例如取到100, 表示1%)
    /// </summary>
    /// <param name="quality">宝珠品质</param>
    /// <param name="level">宝珠等级</param>
    /// <returns></returns>
    public int GetUpGradeProportion(int quality, int level)
    {
        //倍数
        float multiple = 1f;
        var arr = BaoZhuTableManager.Instance.array.gItem.handles;
        TABLE.BAOZHU baozhuItem = null;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            baozhuItem = arr[k].Value as TABLE.BAOZHU;
            if (baozhuItem.quality == quality && baozhuItem.grade < level)
            {
                multiple *= (baozhuItem.addAtribute*1f / 10000 + 1);
            }
        }

        int proportion = Mathf.RoundToInt(multiple*10000 - 10000);
        return proportion;
    }


    PoolHandleManager Pool = new PoolHandleManager();

    /// <summary>
    /// 获取宝珠属性列表
    /// </summary>
    /// <returns></returns>
    public PearAttrData GetPearAttrData(TABLE.ITEM itemCfg,  int addAtribute = 0)
    {
        ClientAttributeTableManager ins = ClientAttributeTableManager.Instance;
        PearAttrData pearAttrData = Pool.GetCustomClass<PearAttrData>();
        float rate = addAtribute*1f / 10000 + 1;
        string attrName = "";
        int minValue = 0;
        int maxValue = 0;

        if (itemCfg.phyAttMax != 0 || itemCfg.phyAttMin != 0) //物理攻击
        {
            attrName = CSString.Format(101);
            minValue = Mathf.RoundToInt(rate * itemCfg.phyAttMin);
            maxValue = Mathf.RoundToInt(rate * itemCfg.phyAttMax);
            pearAttrData.PearAttrItems.Add(new PearAttrItem(attrName, minValue, maxValue));
        }

        if (itemCfg.magicAttMax != 0 || itemCfg.magicAttMin != 0) //魔法攻击
        {
            attrName = CSString.Format(102);
            minValue = Mathf.RoundToInt(rate * itemCfg.magicAttMin);
            maxValue = Mathf.RoundToInt(rate * itemCfg.magicAttMax);
            pearAttrData.PearAttrItems.Add(new PearAttrItem(attrName, minValue, maxValue));
        }

        if (itemCfg.taoAttMax != 0 || itemCfg.taoAttMin != 0) //道士攻击
        {
            attrName = CSString.Format(103);
            minValue = Mathf.RoundToInt(rate * itemCfg.taoAttMin);
            maxValue = Mathf.RoundToInt(rate * itemCfg.taoAttMax);
            pearAttrData.PearAttrItems.Add(new PearAttrItem(attrName, minValue, maxValue));
        }

        if (itemCfg.phyDefMax != 0 || itemCfg.phyDefMin != 0) //物理防御
        {
            attrName = CSString.Format(104);
            minValue = Mathf.RoundToInt(rate * itemCfg.phyDefMin);
            maxValue = Mathf.RoundToInt(rate * itemCfg.phyDefMax);
            pearAttrData.PearAttrItems.Add(new PearAttrItem(attrName, minValue, maxValue));
        }

        if (itemCfg.magicDefMax != 0 || itemCfg.magicDefMin != 0) //最大魔法防御
        {
            attrName = CSString.Format(105);
            minValue = Mathf.RoundToInt(rate * itemCfg.magicDefMin);
            maxValue = Mathf.RoundToInt(rate * itemCfg.magicDefMax);
            pearAttrData.PearAttrItems.Add(new PearAttrItem(attrName, minValue, maxValue));
        }

        if (itemCfg.accurate != 0) //命中
        {
            attrName = CSString.Format(25);
            minValue = Mathf.RoundToInt(rate * itemCfg.accurate);
            pearAttrData.PearAttrItems.Add(new PearAttrItem(attrName, minValue));
        }

        if (itemCfg.dodge != 0) //闪避
        {
            attrName = CSString.Format(26);
            minValue = Mathf.RoundToInt(rate * itemCfg.dodge);
            pearAttrData.PearAttrItems.Add(new PearAttrItem(attrName, minValue));
        }

        if (itemCfg.curse != 0) //诅咒值
        {
            attrName = CSString.Format(37);
            minValue = Mathf.RoundToInt(rate * itemCfg.curse);
            pearAttrData.PearAttrItems.Add(new PearAttrItem(attrName, minValue));
        }

        if (itemCfg.luck != 0) //幸运值
        {
            attrName = CSString.Format(36);
            minValue = Mathf.RoundToInt(rate * itemCfg.luck);
            pearAttrData.PearAttrItems.Add(new PearAttrItem(attrName, minValue));
        }

        if (itemCfg.criticalDamage != 0) //重击伤害
        {
            attrName = CSString.Format(23);
            minValue = Mathf.RoundToInt(rate * itemCfg.criticalDamage);
            pearAttrData.PearAttrItems.Add(new PearAttrItem(attrName, minValue));
        }

        if (itemCfg.critical != 0) //重击概率
        {
            attrName = CSString.Format(24);
            minValue = Mathf.RoundToInt(rate * itemCfg.critical);
            pearAttrData.PearAttrItems.Add(new PearAttrItem(attrName, minValue));
        }

        return pearAttrData;
    }

    #region 网络响应处理函数

    /// <summary>
    /// 上线获取技能槽信息
    /// </summary>
    /// <param name="msg"></param>
    public void HandleBaoZhuSlotSkills(baozhu.BaoZhuSkills msg)
    {
        if (msg == null) return;
        if (myPearlData.SkillSlot == null)
        {
            myPearlData.SkillSlot = new Dictionary<int, int>();
        }

        for (int i = 0; i < msg.baoZhuSkills.Count; i++)
        {
            if (myPearlData.SkillSlot.ContainsKey(msg.baoZhuSkills[i].slot))
            {
                myPearlData.SkillSlot[msg.baoZhuSkills[i].slot] = msg.baoZhuSkills[i].skill;
            }
            else
            {
                myPearlData.SkillSlot.Add(msg.baoZhuSkills[i].slot, msg.baoZhuSkills[i].skill);
            }
        }
    }

    /// <summary>
    /// 处理已装备宝珠打怪数量变化
    /// </summary>
    public void HandleBaoZhuBossCountChange(bag.EquipInfo msg)
    {
        if (msg == null) return;
        // if (msg.position==12)
        // {
        //     msg.position = -12;
        // }
        CSBagInfo.Instance.GetEuqipRecastRes(msg);
    }

    /// <summary>
    /// 处理宝珠升级
    /// </summary>
    public void HandleLevelUpBaoZhu(baozhu.ResLevelUpBaoZhu msg)
    {
        if (msg == null) return;
        CSBagInfo.Instance.GetEuqipRecastRes(msg.equip);
    }

    /// <summary>
    /// 处理宝珠升阶
    /// </summary>
    /// <param name="msg"></param>
    public void HandleGradeUpBaoZhu(baozhu.ResGradeUpBaoZhu msg)
    {
        if (msg == null) return;
        CSBagInfo.Instance.GetEuqipRecastRes(msg.equip);
    }

    /// <summary>
    /// 处理获取最近的一次技能槽刷新出的技能
    /// </summary>
    /// <param name="msg"></param>
    public void HandleRefreshSkill(baozhu.BaoZhuSkills msg)
    {
        if (msg == null) return;
        if (myPearlData.dicRefreshSkill == null)
        {
            myPearlData.dicRefreshSkill = new Dictionary<int, int>();
        }

        for (int i = 0; i < msg.baoZhuSkills.Count; i++)
        {
            if (myPearlData.dicRefreshSkill.ContainsKey(msg.baoZhuSkills[i].slot))
            {
                myPearlData.dicRefreshSkill[msg.baoZhuSkills[i].slot] = msg.baoZhuSkills[i].skill;
            }
            else
            {
                myPearlData.dicRefreshSkill.Add(msg.baoZhuSkills[i].slot, msg.baoZhuSkills[i].skill);
            }
        }
    }

    /// <summary>
    /// 处理技能槽增加技能
    /// </summary>
    /// <param name="msg"></param>
    public void HandBaoZhuSkills(baozhu.BaoZhuSkills msg)
    {
        if (msg == null) return;
        if (myPearlData.SkillSlot == null)
        {
            myPearlData.SkillSlot = new Dictionary<int, int>();
        }

        for (int i = 0; i < msg.baoZhuSkills.Count; i++)
        {
            if (myPearlData.SkillSlot.ContainsKey(msg.baoZhuSkills[i].slot))
            {
                myPearlData.SkillSlot[msg.baoZhuSkills[i].slot] = msg.baoZhuSkills[i].skill;
            }
            else
            {
                myPearlData.SkillSlot.Add(msg.baoZhuSkills[i].slot, msg.baoZhuSkills[i].skill);
            }
        }
    }

    #endregion
}

/// <summary>
/// 宝珠升级属性列表
/// </summary>
public class PearAttrData : IDispose
{
    public PearAttrData()
    {
        PearAttrItems = new List<PearAttrItem>();
    }

    public List<PearAttrItem> PearAttrItems { get; set; }

    public void Dispose()
    {
        if (PearAttrItems != null)
        {
            PearAttrItems.Clear();
            PearAttrItems = null;
        }
    }
}

/// <summary>
/// 宝珠升级单个属性
/// </summary>
public class PearAttrItem : IDispose
{
    public PearAttrItem(string attrName, int minValue, int maxValue = 0)
    {
        AttrName = attrName;
        MinValue = minValue;
        MaxValue = maxValue;
        StrValue = maxValue == 0 ? $"{MinValue}" : $"{MinValue}~{MaxValue}";
    }

    public string AttrName { get; set; }
    public int MinValue { get; set; }
    public int MaxValue { get; set; }

    public string StrValue { get; set; }

    public void Dispose()
    {
        AttrName = String.Empty;
        MinValue = 0;
        MaxValue = 0;
    }
}

/// <summary>
/// 进化宝珠数据类
/// </summary>
public class PearlData
{
    /// <summary>
    /// 所有宝珠信息
    /// </summary>
    public ILBetterList<bag.BagItemInfo> ListPearl
    {
        get { return /*CSBagInfo.Instance.GetBaoZhuItems()*/new ILBetterList<BagItemInfo>(); }
    }

    /// <summary>
    /// 已装备宝珠信息(第12个格子)
    /// </summary>
    public BagItemInfo EquipPearl
    {
        get { return CSBagInfo.Instance.GetSelfEquipByGridPos(12); }
    }

    /// <summary>
    /// 已有技能技能槽（技能槽Id，技能Id）
    /// </summary>
    public Dictionary<int, int> SkillSlot { get; set; }

    /// <summary>
    /// 所有技能槽数据
    /// </summary>
    public List<SkillSlotData> ListSkillSlotDatas
    {
        get { return CSPearlInfo.Instance.GetSortSkillSolt(); }
    }

    /// <summary>
    /// 各个技能槽最近一次刷新的技能
    /// </summary>
    public Dictionary<int, int> dicRefreshSkill { get; set; }
}

/// <summary>
/// 技能槽信息
/// </summary>
public class SkillSlotData
{
    /// <summary>
    /// 技能槽Id
    /// </summary>
    public int SkillSlotId { get; set; }

    /// <summary>
    /// 当前拥有的技能Id
    /// </summary>
    public int SkillId { get; set; }

    /// <summary>
    /// 是否解锁
    /// </summary>
    public bool IsUnlock { get; set; }

    /// <summary>
    /// 解锁需要的宝珠最小等级
    /// </summary>
    public int BaoZhuGradeMinUnlock { get; set; }

    /// <summary>
    /// 技能槽技能库Id
    /// </summary>
    public int LibID { get; set; }
}