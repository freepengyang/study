using bag;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TipDataItem : IDispose
{
    public void Dispose()
    {
        Title = "";
        message = "";
        for (int i = 0; i < Properties.Count; i++)
        {
            Properties[i].Dispose();
        }

        Properties.Clear();
    }


    public string Title { get; set; }
    public List<TipProperty> Properties { get; set; }

    public string message { get; set; }

    public TipDataItem()
    {
        Properties = new List<TipProperty>();
    }
}

public class EquipRefineProDic : IDispose
{
    public int Id = 0;
    public string name = "";
    public string des = "";
    public List<EquipRefineProperty> ts = new List<EquipRefineProperty>();

    public void Dispose()
    {
        var iter = ts.GetEnumerator();
        while (iter.MoveNext())
        {
            iter.Dispose();
        }

        ts.Clear();
    }
}

public class EquipRefineProperty : IDispose
{
    public void Dispose()
    {
        Id = 0;
        per = 0;
        name = "";
        des = "";
        value = 0;
        strValue = "";
        maxValue = 0;
        strMaxValue = "";
        data.Clear();
        attrIndexList.Clear();
        attrIndexList = null;
    }

    public int Id = 0;
    public int per = 0;
    public string name = "";
    public string des = "";
    public List<RandAttr> data = new List<RandAttr>();
    public List<int> attrIndexList = new List<int>();
    public int value = 0;
    public string strValue = "";
    public int levClass = 1;
    public int quality = 0;
    public int maxValue = 0;
    public string strMaxValue = "";
    public bool needPlus = false;
}

public class TipProperty : IDispose
{
    public void Dispose()
    {
        Name = "";
        Value = 0;
        MaxValueName = "";
        MaxValue = 0;
        exValue = "";
        exValue2 = "";
        quality = 0;
        id = 0;
        persent = 0;
    }

    public string Name { get; set; }
    public int Value { get; set; }
    public string ValueName { get; set; }
    public bool isValueAdd { get; set; }

    public string MaxValueName { get; set; }
    public int MaxValue { get; set; }
    public string exValue { get; set; }
    public string exValue2 { get; set; }
    public int quality { get; set; }
    public int id { get; set; }
    public int persent { get; set; }
    public Color Color { get; set; }

    public TipProperty()
    {
    }

    public TipProperty(string _name, string _ename, Color _color)
    {
        Name = _name;

        MaxValueName = _ename;

        Color = _color;
    }
}

public class TipsDataItemList
{
    public string TipsTitle { get; set; }
    public List<TipDataItem> TipItems { get; set; }
    public string Desc { get; set; }
    public string constellationDesc { get; set; }

    public TipsDataItemList()
    {
        TipItems = new List<TipDataItem>();
    }
}

public class EqiupTipData
{
    //默认用Right

    #region Right

    //根据配置 可以确定是否显示左边和中间的部分
    public TABLE.ITEM r_itemCfg;
    public BagItemInfo r_itemInfo;
    public TipDataItem r_basicInfo;
    public TipDataItem r_randomInfo;
    public TipDataItem r_equipSkillInfo;
    public TipProperty r_wolongSuit;
    public TipDataItem r_wolongAttr;
    public TipDataItem r_wolongSkill;

    #endregion

    #region Middle

    public TABLE.ITEM m_itemCfg;
    public BagItemInfo m_itemInfo;
    public TipDataItem m_basicInfo;
    public TipDataItem m_randomInfo;
    public TipDataItem m_equipSkillInfo;
    public TipProperty m_wolongSuit;
    public TipDataItem m_wolongAttr;
    public TipDataItem m_wolongSkill;

    #endregion

    #region Left

    public TABLE.ITEM l_itemCfg;
    public BagItemInfo l_itemInfo;
    public TipDataItem l_basicInfo;
    public TipDataItem l_randomInfo;
    public TipDataItem l_equipSkillInfo;
    public TipProperty l_wolongSuit;
    public TipDataItem l_wolongAttr;
    public TipDataItem l_wolongSkill;

    #endregion
}

public class TipsBtnData
{
    public string name;
    public TipBtnType type;
    public TABLE.ITEM cfg;
    public BagItemInfo info;
    public object ex_data;
    public TipsOpenType openType;

    public TipsBtnData(string _name, TipBtnType _type, TABLE.ITEM _cfg, BagItemInfo _info, TipsOpenType _openType,
        object _data = null)
    {
        name = _name;
        type = _type;
        cfg = _cfg;
        info = _info;
        ex_data = _data;
        openType = _openType;
    }
}


public class StructTipData : CSInfo<StructTipData>
{
    PoolHandleManager Pool = new PoolHandleManager();
    List<TipsBtnData> nameList = new List<TipsBtnData>();
    List<int> opeList = new List<int>();
    const string add = "  + ";
    const string splitStr = "~";
    const string noSpaceAdd = "+ ";
    string specialStr;//= [最大:{0}~{1}];
    string SpecialStr
    {
        get
        {
            if (specialStr == null) { specialStr = ClientTipsTableManager.Instance.GetClientTipsContext(1996); }
            return specialStr;
        }
    }
    string str;// = "[最大:{0}]";

    string Str
    {
        get
        {
            if (str == null) { str = ClientTipsTableManager.Instance.GetClientTipsContext(1997); }
            return str;
        }
    }
    string suitStr;// = "{0}[{1}/{2}]";

    string SuitStr
    {
        get
        {
            if (suitStr == null) { suitStr = ClientTipsTableManager.Instance.GetClientTipsContext(1998); }
            return suitStr;
        }
    }
    List<int> WLLonghunattr;
    string showFirstChargeAttr = "";
    string[] tipBtnNameList; // 洗练#重铸#穿戴#卸下#替换#使用#丢弃#替换（左）#替换（右）#拆分#放入#取出#捐献#兑换#祝福

    public override void Dispose()
    {
        opeList.Clear();
        opeList = null;
        nameList.Clear();
        nameList = null;
        Pool.RecycleAll();
    }
    public void RecycleSingle(object message)
    {
        Pool.Recycle(message);
    }

    #region 基础属性
    public TipDataItem GetEquipBasicData(TABLE.ITEM _itemCfg, bag.BagItemInfo info = null, float addAtribute = 0f)
    {
        ClientAttributeTableManager ins = ClientAttributeTableManager.Instance;
        ClientTipsTableManager tipsIns = ClientTipsTableManager.Instance;
        TipDataItem item = Pool.GetCustomClass<TipDataItem>();
        float addtionalAttr = addAtribute / 100;
        string addAttrPercent = addAtribute > 0 ? $"{noSpaceAdd}{(int)(addtionalAttr)}%".BBCode(ColorType.Green) : "";
        float rate = addAtribute * 0.0001f;
        string addValue = "";
        if (_itemCfg.phyAttMax != 0 || _itemCfg.phyAttMin != 0) //物理攻击
        {
            string needPlus = ins.GetClientAttributePlus(101) == 1 ? add : string.Empty;
            addValue = addAtribute > 0
                ? $"{add}{(int)(rate * _itemCfg.phyAttMin)}{splitStr}{(int)(rate * _itemCfg.phyAttMax)}".BBCode(
                    ColorType.Green)
                : "";
            item.Properties.Add(new TipProperty(
                $"{tipsIns.GetClientTipsContext(101)}{needPlus}{_itemCfg.phyAttMin}{splitStr}{_itemCfg.phyAttMax}",
                addValue, CSColor.beige));
        }

        if (_itemCfg.magicAttMax != 0 || _itemCfg.magicAttMin != 0) //魔法攻击
        {
            string needPlus = ins.GetClientAttributePlus(102) == 1 ? add : string.Empty;
            addValue = addAtribute > 0
                ? $"{add}{(int)(rate * _itemCfg.magicAttMin)}{splitStr}{(int)(rate * _itemCfg.magicAttMax)}".BBCode(
                    ColorType.Green)
                : "";
            item.Properties.Add(new TipProperty(
                $"{tipsIns.GetClientTipsContext(102)}{needPlus}{_itemCfg.magicAttMin}{splitStr}{_itemCfg.magicAttMax}",
                addValue, CSColor.beige));
        }

        if (_itemCfg.taoAttMax != 0 || _itemCfg.taoAttMin != 0) //道士攻击
        {
            string needPlus = ins.GetClientAttributePlus(103) == 1 ? add : string.Empty;
            addValue = addAtribute > 0
                ? $"{add}{(int)(rate * _itemCfg.taoAttMin)}{splitStr}{(int)(rate * _itemCfg.taoAttMax)}".BBCode(
                    ColorType.Green)
                : "";
            item.Properties.Add(new TipProperty(
                $"{tipsIns.GetClientTipsContext(103)}{needPlus}{_itemCfg.taoAttMin}{splitStr}{_itemCfg.taoAttMax}",
                addValue, CSColor.beige));
        }

        if (_itemCfg.phyDefMax != 0 || _itemCfg.phyDefMin != 0) //物理防御
        {
            string needPlus = ins.GetClientAttributePlus(104) == 1 ? add : string.Empty;
            addValue = addAtribute > 0
                ? $"{add}{(int)(rate * _itemCfg.phyDefMin)}{splitStr}{(int)(rate * _itemCfg.phyDefMax)}".BBCode(
                    ColorType.Green)
                : "";
            item.Properties.Add(new TipProperty(
                $"{tipsIns.GetClientTipsContext(104)}{needPlus}{_itemCfg.phyDefMin}{splitStr}{_itemCfg.phyDefMax}",
                addValue, CSColor.beige));
        }

        if (_itemCfg.magicDefMax != 0 || _itemCfg.magicDefMin != 0) //最大魔法防御
        {
            string needPlus = ins.GetClientAttributePlus(105) == 1 ? add : string.Empty;
            addValue = addAtribute > 0
                ? $"{add}{(int)(rate * _itemCfg.magicDefMin)}{splitStr}{(int)(rate * _itemCfg.magicDefMax)}".BBCode(
                    ColorType.Green)
                : "";
            item.Properties.Add(new TipProperty(
                $"{tipsIns.GetClientTipsContext(105)}{needPlus}{_itemCfg.magicDefMin}{splitStr}{_itemCfg.magicDefMax}",
                addValue, CSColor.beige));
        }

        if (_itemCfg.accurate != 0) //命中
        {
            var value = Math.Round(Convert.ToDecimal(_itemCfg.accurate * 0.01f), 1, MidpointRounding.AwayFromZero);
            addValue = addAtribute > 0 ? $"{add}{(int)(rate * _itemCfg.accurate)}".BBCode(ColorType.Green) : "";
            item.Properties.Add(new TipProperty(
                $"{tipsIns.GetClientTipsContext(ins.GetClientAttributeTipID(15))}{add}{value}", addValue, CSColor.beige));
        }

        if (_itemCfg.dodge != 0) //闪避
        {
            var value = Math.Round(Convert.ToDecimal(_itemCfg.dodge * 0.01f), 1, MidpointRounding.AwayFromZero);
            addValue = addAtribute > 0 ? $"{add}{(int)(rate * _itemCfg.dodge)}".BBCode(ColorType.Green) : "";
            item.Properties.Add(
                new TipProperty(
                    $"{tipsIns.GetClientTipsContext(ins.GetClientAttributeTipID(16))}{add}{value}", addValue, CSColor.beige));
        }

        if (_itemCfg.curse != 0) //诅咒值
        {
            addValue = addAtribute > 0 ? $"{add}{(int)(rate * _itemCfg.curse)}".BBCode(ColorType.Green) : "";
            item.Properties.Add(new TipProperty(
                $"{tipsIns.GetClientTipsContext(ins.GetClientAttributeTipID(27))}{add}{UtilityMath.GetRoundingInt(1 * _itemCfg.curse)}", addValue, CSColor.beige));
        }

        if (_itemCfg.luck != 0) //幸运值
        {
            addValue = addAtribute > 0 ? $"{add}{(int)(rate * _itemCfg.luck)}".BBCode(ColorType.Green) : "";
            item.Properties.Add(new TipProperty(
                $"{tipsIns.GetClientTipsContext(ins.GetClientAttributeTipID(26))}{add}{UtilityMath.GetRoundingInt(1 * _itemCfg.luck)}", addValue, CSColor.beige));
        }

        if (_itemCfg.criticalDamage != 0) //重击伤害
        {
            var value = Math.Round(Convert.ToDecimal(_itemCfg.criticalDamage * 0.01f), 1, MidpointRounding.AwayFromZero);
            addValue = addAtribute > 0 ? $"{add}{(int)(rate * _itemCfg.criticalDamage)}".BBCode(ColorType.Green) : "";
            item.Properties.Add(
                new TipProperty(
                    $"{tipsIns.GetClientTipsContext(ins.GetClientAttributeTipID(13))}{add}{value}", addValue, CSColor.beige));
        }

        if (_itemCfg.critical != 0) //重击概率
        {
            var value = Math.Round(Convert.ToDecimal(_itemCfg.critical * 0.01f), 1, MidpointRounding.AwayFromZero);
            addValue = addAtribute > 0 ? $"{add}{(int)(rate * _itemCfg.critical)}".BBCode(ColorType.Green) : "";
            item.Properties.Add(
                new TipProperty(
                    $"{tipsIns.GetClientTipsContext(ins.GetClientAttributeTipID(14))}{add}{value}", addValue, CSColor.beige));
        }

        if (_itemCfg.hp != 0) //血量
        {
            var value = Math.Round(Convert.ToDecimal(_itemCfg.hp), 1, MidpointRounding.AwayFromZero);
            addValue = addAtribute > 0 ? $"{add}{(int)(rate * _itemCfg.hp)}".BBCode(ColorType.Green) : "";
            item.Properties.Add(
                new TipProperty(
                    $"{tipsIns.GetClientTipsContext(ins.GetClientAttributeTipID(11))}{add}{value}", addValue, CSColor.beige));
        }

        if (_itemCfg.mp != 0) //蓝量
        {
            var value = Math.Round(Convert.ToDecimal(_itemCfg.mp), 1, MidpointRounding.AwayFromZero);
            addValue = addAtribute > 0 ? $"{add}{(int)(rate * _itemCfg.mp)}".BBCode(ColorType.Green) : "";
            item.Properties.Add(
                new TipProperty(
                    $"{tipsIns.GetClientTipsContext(ins.GetClientAttributeTipID(12))}{add}{value}", addValue, CSColor.beige));
        }

        if (info != null && info.weaponLuckLv > 0)
        {
            if (_itemCfg.type == 2 && _itemCfg.subType == 1)
            {
                item.Properties.Add(new TipProperty($"幸运{add}{UtilityMath.GetRoundingInt(1 * info.weaponLuckLv)}", "", CSColor.green));
            }
        }
        if (CSBagInfo.Instance.IsNormalEquip(_itemCfg))
        {
            if (_itemCfg.bufferParam != "")
            {
                List<int> yuanhun = UtilityMainMath.SplitStringToIntList(_itemCfg.bufferParam);
                if (yuanhun.Count > 1 && yuanhun[1] > 0)
                {
                    item.Properties.Add(new TipProperty($"{string.Format("战魂天赋等级 +{0}", yuanhun[1]) }", "", CSColor.purple));
                }
            }
        }
        return item;
    }
    #endregion

    #region 随机属性
    ILBetterList<RandAttr> attrList;
    ILBetterList<RandAttr> jobList;
    ILBetterList<RandAttr> jobAllList;
    public TipDataItem GetEquipRandomData(TABLE.ITEM _itemCfg, bag.BagItemInfo _info)
    {
        showFirstChargeAttr = "";
        if (_info == null)
        {
            return null;
        }
        TipDataItem itemslist = Pool.GetCustomClass<TipDataItem>();
        string temp_attrStr = "";
        if (attrList == null) { attrList = new ILBetterList<RandAttr>(); }
        if (jobList == null) { jobList = new ILBetterList<RandAttr>(); }
        if (jobAllList == null) { jobAllList = new ILBetterList<RandAttr>(); }
        attrList.Clear();
        jobList.Clear();
        jobAllList.Clear();
        //给首冲特殊装备生成属性字符串
        CSBetterLisHot<RandAttr> attrs = new CSBetterLisHot<RandAttr>();

        // 1.属性  2.装备技能  3.职业单技能   4.职业全技能
        for (int i = 0; i < _info.randAttrValues.Count; i++)
        {
            if (_info.randAttrValues[i].type == 1)
            {
                attrList.Add(_info.randAttrValues[i]);
            }
            else if (_info.randAttrValues[i].type == 3)
            {
                jobList.Add(_info.randAttrValues[i]);
            }
            else if (_info.randAttrValues[i].type == 4)
            {
                jobAllList.Add(_info.randAttrValues[i]);
            }
            attrs.Add(_info.randAttrValues[i]);
        }
        attrs.Sort((a, b) => { return a.param1 - b.param1; });
        for (int i = 0; i < attrs.Count; i++)
        {
            temp_attrStr = $"{attrs[i].type}#{attrs[i].param1}#{attrs[i].param2}#{attrs[i].value1}#{attrs[i].value2}#{attrs[i].configId1}#{attrs[i].configId2}";
            if ((i + 1) < attrs.Count)
            {
                temp_attrStr = $"{temp_attrStr}&";
            }
            showFirstChargeAttr = $"{showFirstChargeAttr}{temp_attrStr}";
        }
        Debug.Log(showFirstChargeAttr);
        for (int i = 0; i < attrList.Count; i++)
        {
            TipProperty pro = Pool.GetCustomClass<TipProperty>();
            int proId = attrList[i].param1;
            int proValue = attrList[i].value1;
            int MaxproId = attrList[i].param2;
            int MaxproValue = attrList[i].value2;
            int per = ClientAttributeTableManager.Instance.GetClientAttributePer(proId);
            string value = proValue.ToString();
            int temp_maxValue = RandomAttrValueTableManager.Instance.GetItemCfg(_itemCfg.levClass, 1, proId);
            string maxValue = temp_maxValue.ToString();
            string needPlus = ClientAttributeTableManager.Instance.GetClientAttributePlus(proId) == 1 ? add : " ";
#if UNITY_EDITOR
            if (proValue == 0 && MaxproValue == 0)
            {
                UtilityTips.ShowRedTips($"最大值最小值都==0  RandomAttrValu表填错了   ID为{proId}~{MaxproId}", 5f);
                UtilityTips.ShowRedTips($"最大值最小值都==0  RandomAttrValu表填错了   ID为{proId}~{MaxproId}", 5f);
                UtilityTips.ShowRedTips($"最大值最小值都==0  RandomAttrValu表填错了   ID为{proId}~{MaxproId}", 5f);
                continue;
            }
#endif
            if (MaxproValue != 0)
            {
                value = $"{proValue}~{MaxproValue}";
                int temp_maxValue2 = RandomAttrValueTableManager.Instance.GetItemCfg(_itemCfg.levClass, 1, MaxproId);
                string maxValue2 = temp_maxValue2.ToString();
                int clientTipsId = ClientAttributeTableManager.Instance.GetClientAttributeAttached(proId);
                if (per > 0)
                {
                    value =
                        $"{Math.Round(Convert.ToDecimal(proValue * 0.01f), 1, MidpointRounding.AwayFromZero)}%~{Math.Round(Convert.ToDecimal(MaxproValue * 0.01f), 1, MidpointRounding.AwayFromZero)}%";
                    maxValue =
                        $"{Math.Round(Convert.ToDecimal(temp_maxValue * 0.01f), 1, MidpointRounding.AwayFromZero)}%";
                    maxValue2 =
                        $"{Math.Round(Convert.ToDecimal(temp_maxValue2 * 0.01f), 1, MidpointRounding.AwayFromZero)}%";
                }
                pro.Name = $"{ClientTipsTableManager.Instance.GetClientTipsContext(clientTipsId)}{needPlus}{value}";
                pro.Value = proValue;
                pro.MaxValue = temp_maxValue;
                pro.MaxValueName = string.Format(SpecialStr, maxValue, maxValue2);
                int qua1 = attrList[i].configId1;
                int qua2 = attrList[i].configId2;
                pro.quality = (qua1 > qua2) ? qua1 : qua2;
                pro.persent = ClientAttributeTableManager.Instance.GetClientAttributePer(proId);
                itemslist.Properties.Add(pro);
            }
            else
            {
                if (per > 0)
                {
                    value = $"{Math.Round(Convert.ToDecimal(proValue * 0.01f), 1, MidpointRounding.AwayFromZero)}%";
                    maxValue =
                        $"{Math.Round(Convert.ToDecimal(temp_maxValue * 0.01f), 1, MidpointRounding.AwayFromZero)}%";
                }
                pro.Name =
                    $"{ClientTipsTableManager.Instance.GetClientTipsContext(ClientAttributeTableManager.Instance.GetClientAttributeTipID(proId))}{add}{value}";
                pro.Value = proValue;
                pro.ValueName = value;
                pro.MaxValue = temp_maxValue;
                pro.MaxValueName = string.Format(Str, maxValue);
                pro.quality = attrList[i].configId1;
                pro.persent = ClientAttributeTableManager.Instance.GetClientAttributePer(proId);
                itemslist.Properties.Add(pro);
            }
        }
        for (int i = 0; i < jobList.Count; i++)
        {
            int proId = jobList[i].param1;
            int proValue = jobList[i].value1;
            TipProperty pro = Pool.GetCustomClass<TipProperty>();
            pro.Name = $"{SkillTableManager.Instance.GetNameByGroupId(proId)}+{proValue}";
            pro.MaxValueName = string.Format(Str,
                RandomAttrValueTableManager.Instance.GetItemCfg(_itemCfg.levClass, 3, proId));
            pro.quality = jobList[i].configId1;
            itemslist.Properties.Add(pro);
        }
        for (int i = 0; i < jobAllList.Count; i++)
        {
            TipProperty pro = Pool.GetCustomClass<TipProperty>();
            pro.Name = $"{string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1623), Utility.GetJob(jobAllList[i].param1))}+{jobAllList[i].value1}";
            pro.MaxValueName = string.Format(Str,
                RandomAttrValueTableManager.Instance.GetItemCfg(_itemCfg.levClass, 4, jobAllList[i].param1));
            pro.quality = jobAllList[i].configId1;
            itemslist.Properties.Add(pro);
        }
        showFirstChargeAttr = $"{showFirstChargeAttr}{temp_attrStr}";
        return itemslist;
    }
    #endregion

    #region 卧龙套装
    public TipProperty GetWoLongSuitData(TABLE.ITEM _itemCfg)
    {
        TipProperty pro = Pool.GetCustomClass<TipProperty>();
        pro.Name = ZhanHunSuitTableManager.Instance.GetZhanHunSuitName((int)_itemCfg.zhanHunSuit);
        int hasNum = CSBagInfo.Instance.GetSelfWoLongNum(_itemCfg.levClass);
        int totalNum = ZhanHunSuitTableManager.Instance.GetZhanHunSuitSuitNum((int)_itemCfg.zhanHunSuit);
        if (hasNum < totalNum)
        {
            pro.exValue2 = string.Format(SuitStr, UtilityColor.Red, hasNum, totalNum);
        }
        else
        {
            pro.exValue2 = string.Format(SuitStr, UtilityColor.Green, hasNum, totalNum);
        }
        pro.MaxValueName = ZhanHunSuitTableManager.Instance.GetZhanHunSuitFactor(_itemCfg.zhanHunSuit);
        pro.exValue = ZhanHunSuitTableManager.Instance.GetZhanHunSuitDescription(_itemCfg.zhanHunSuit);
        return pro;
    }
    #endregion

    #region btns
    public List<TipsBtnData> StructBtnData(TipsOpenType _type, TABLE.ITEM _itemCfg, bag.BagItemInfo _info = null)
    {
        opeList.Clear();
        opeList = UtilityMainMath.SplitStringToIntList(
            ItemOperateTableManager.Instance.GetBtnsList(_itemCfg.type, _itemCfg.subType, _itemCfg.Operationtype), '#');
        if (tipBtnNameList == null)
        {
            tipBtnNameList = SundryTableManager.Instance.GetDes(2).Split('#');
        }
        nameList.Clear();
        for (int i = 0; i < opeList.Count; i++)
        {
            nameList.Add(new TipsBtnData(tipBtnNameList[opeList[i] - 1], (TipBtnType)opeList[i], _itemCfg, _info,
                _type));
        }
        return nameList;
    }

    public string GetTipBtnNameByType(int _type)
    {
        if (tipBtnNameList == null)
        {
            tipBtnNameList = SundryTableManager.Instance.GetDes(2).Split('#');
        }

        return tipBtnNameList[_type];
    }
    #endregion

    #region 重铸洗练属性展示
    public EquipRefineProDic GetEquipRandomDisjunctData(TABLE.ITEM _itemCfg, RepeatedField<RandAttr> _attrList)
    {
        EquipRefineProDic proDic = Pool.GetCustomClass<EquipRefineProDic>();
        for (int i = 0; i < _attrList.Count; i++)
        {
            EquipRefineProperty property = Pool.GetCustomClass<EquipRefineProperty>();
            property.Id = _attrList[i].param1;
            property.data.Add(_attrList[i]);
            property.attrIndexList.Add(i);
            //Debug.Log($"{_itemCfg.subType}   {_attrList[i].param1}  {_attrList[i].type}  ");
            if (_attrList[i].type == 1)
            {
                int attached = ClientAttributeTableManager.Instance.GetClientAttributeAttached(_attrList[i].param1);
                property.name =
                    ClientTipsTableManager.Instance.GetClientTipsContext(
                        ClientAttributeTableManager.Instance.GetClientAttributeTipID(attached));
                property.per = ClientAttributeTableManager.Instance.GetClientAttributePer(_attrList[i].param1);
                property.levClass = _itemCfg.levClass;
                int qua1 = _attrList[i].configId1;
                int qua2 = _attrList[i].configId2;
                property.quality = (qua1 > qua2) ? qua1 : qua2;
                property.maxValue =
                    RandomAttrValueTableManager.Instance.GetItemCfg(_itemCfg.levClass, 1, _attrList[i].param1);
#if UNITY_EDITOR
                if (_attrList[i].value1 == 0 && _attrList[i].value2 == 0)
                {
                    UtilityTips.ShowRedTips($"最大值最小值都==0  RandomAttrValu表填错了   ID为{_attrList[i].param1}~{_attrList[i].param2}", 5f);
                    UtilityTips.ShowRedTips($"最大值最小值都==0  RandomAttrValu表填错了   ID为{_attrList[i].param1}~{_attrList[i].param2}", 5f);
                    UtilityTips.ShowRedTips($"最大值最小值都==0  RandomAttrValu表填错了   ID为{_attrList[i].param1}~{_attrList[i].param2}", 5f);
                    continue;
                }
#endif
                if (_attrList[i].value2 != 0)
                {
                    int maxvalue2 =
                        RandomAttrValueTableManager.Instance.GetItemCfg(_itemCfg.levClass, 1, _attrList[i].param2);
                    if (property.per > 0)
                    {
                        string result =
                            $"+{Math.Round(Convert.ToDecimal(_attrList[i].value1 * 0.01f), 2, MidpointRounding.AwayFromZero)}%~{Math.Round(Convert.ToDecimal(_attrList[i].value2 * 0.01f), 2, MidpointRounding.AwayFromZero)}";
                        string Maxresult =
                            $"{Math.Round(Convert.ToDecimal(property.maxValue * 0.01f), 2, MidpointRounding.AwayFromZero)}%~{Math.Round(Convert.ToDecimal(maxvalue2 * 0.01f), 2, MidpointRounding.AwayFromZero)}";
                        property.strValue =
                            string.Format(ClientAttributeTableManager.Instance.GetClientAttributeAttributeFmt(attached),
                                result);
                        property.strMaxValue = string.Format(Str, Maxresult);
                    }
                    else
                    {
                        property.strValue = $"+{_attrList[i].value1}~{_attrList[i].value2}";
                        property.strMaxValue = string.Format(SpecialStr, property.maxValue, maxvalue2);
                    }
                }
                else
                {
                    if (property.per > 0)
                    {
                        string result =
                            $"+{Math.Round(Convert.ToDecimal(_attrList[i].value1 * 0.01f), 2, MidpointRounding.AwayFromZero)}%";
                        string Maxresult =
                            $"{Math.Round(Convert.ToDecimal(property.maxValue * 0.01f), 2, MidpointRounding.AwayFromZero)}%";
                        property.strValue =
                            string.Format(ClientAttributeTableManager.Instance.GetClientAttributeAttributeFmt(attached),
                                result);
                        property.strMaxValue = string.Format(Str, Maxresult);
                    }
                    else
                    {
                        //Debug.Log($"{_attrList[i].param1}   {_attrList[i].param1}   {attached}   {ClientAttributeTableManager.Instance.GetClientAttributeAttributeFmt(attached)}   {_attrList[i].value1}   {_attrList[i].value2}");
                        property.strValue =
                            string.Format(ClientAttributeTableManager.Instance.GetClientAttributeAttributeFmt(attached),
                                $"+{_attrList[i].value1}");
                        property.strMaxValue = string.Format(Str, property.maxValue);
                    }
                }
            }
            else if (_attrList[i].type == 2)
            {
                property.name = "装备技能:";
                property.strValue = $"[u]{SkillTableManager.Instance.GetNameByGroupId(_attrList[i].param1)}[-]";
                property.quality = _attrList[i].configId1;
            }
            else if (_attrList[i].type == 3)
            {
                property.name = SkillTableManager.Instance.GetNameByGroupId(_attrList[i].param1);
                property.strValue = $"+{_attrList[i].value1}";
                property.strMaxValue = string.Format(Str,
                    RandomAttrValueTableManager.Instance.GetItemCfg(_itemCfg.levClass, 3, _attrList[i].param1));
                property.quality = _attrList[i].configId1;
            }
            else if (_attrList[i].type == 4)
            {
                property.name = $"{Utility.GetJob(_attrList[i].param1)}职业技能";
                property.strValue = $"+{_attrList[i].value1}";
                property.strMaxValue = string.Format(Str,
                    RandomAttrValueTableManager.Instance.GetItemCfg(_itemCfg.levClass, 4, _attrList[i].param1));
                property.quality = _attrList[i].configId1;
            }
            proDic.ts.Add(property);
        }
        return proDic;
    }
    #endregion

    #region 部位类型信息
    string[] posNameList;
    public string GetEquipPosName(int _pos)
    {
        if (1 <= _pos && _pos <= 10)
        {
            if (posNameList == null)
            {
                posNameList = ClientTipsTableManager.Instance.GetClientTipsContext(213).Split('#');
                ;
            }
            return posNameList[_pos - 1];
        }
        else
        {
            return "";
        }
    }
    string[] itemTypeNameList;
    public string GetItemTypeName(int _type)
    {
        if (itemTypeNameList == null)
        {
            itemTypeNameList = SundryTableManager.Instance.GetSundryEffect(34).Split('#');
            ;
        }
        return itemTypeNameList[_type - 1];
    }
    #endregion


    #region 卧龙装备tips属性  技能展示

    public TipDataItem GetWolongRandomSkill(TABLE.ITEM _itemCfg, string _str)
    {
        TipDataItem itemslist = Pool.GetCustomClass<TipDataItem>();
        List<List<int>> attList = UtilityMainMath.SplitStringToIntLists(_str);
        WoLongRandomAttrTableManager ins = WoLongRandomAttrTableManager.Instance;
        for (int i = 0; i < attList.Count; i++)
        {
            TipProperty pro = Pool.GetCustomClass<TipProperty>();
            pro.Name = $"{SkillTableManager.Instance.GetNameByGroupId(ins.GetWoLongRandomAttrParameter(attList[i][0]))}+{attList[i][1]}";
            pro.quality = attList[i][2];
            itemslist.Properties.Add(pro);
        }
        return itemslist;
    }
    public TipDataItem GetWolongRandomSkill(TABLE.ITEM _itemCfg, bag.BagItemInfo _info)
    {
        if (_info == null)
        {
            return null;
        }
        //string attrStr = "";
        TipDataItem itemslist = Pool.GetCustomClass<TipDataItem>();
        for (int i = 0; i < _info.longJis.Count; i++)
        {
            int id = _info.longJis[i].id;
            int value = _info.longJis[i].effectValue;
            TipProperty pro = Pool.GetCustomClass<TipProperty>();
            WoLongRandomAttrTableManager ins = WoLongRandomAttrTableManager.Instance;
            pro.Name = $"{SkillTableManager.Instance.GetNameByGroupId(ins.GetWoLongRandomAttrParameter(id))}+{value}";
            pro.quality = _info.longJis[i].quality;
            itemslist.Properties.Add(pro);
            //string str = $"{id}#{value}#{_info.longJis[i].quality}";
            //if ((i + 1) < _info.longJis.Count)
            //{
            //    str = $"{str}&";
            //}
            //attrStr = $"{attrStr}{str}";
        }
        //Debug.Log($"龙技  {attrStr}");
        return itemslist;
    }
    public void GetWoLongLongliBaseAffix(RepeatedField<bag.WolongRandomEffect> _repeat, string _str)
    {
        List<List<int>> attList = UtilityMainMath.SplitStringToIntLists(_str);
        for (int i = 0; i < attList.Count; i++)
        {
            WolongRandomEffect eff = new WolongRandomEffect();
            eff.id = attList[i][0];
            eff.effectValue = attList[i][1];
            eff.quality = attList[i][2];
            _repeat.Add(eff);
        }
    }
    #endregion

    #region 首充普通装备展示
    ILBetterList<RandAttr> FCattrList = new ILBetterList<RandAttr>(10);
    ILBetterList<RandAttr> FCjobList = new ILBetterList<RandAttr>(10);
    ILBetterList<RandAttr> FCjobAllList = new ILBetterList<RandAttr>(10);
    public TipDataItem FirstChargeEquipShowAttr(TABLE.ITEM _itemCfg, string _attrs, PoolHandleManager _pool)
    {
        List<List<int>> attList = UtilityMainMath.SplitStringToIntLists(_attrs);
        TipDataItem itemslist = _pool.GetCustomClass<TipDataItem>();
        FCattrList.Clear();
        FCjobList.Clear();
        FCjobAllList.Clear();
        // 1.属性  2.装备技能  3.职业单技能   4.职业全技能
        for (int i = 0; i < attList.Count; i++)
        {
            RandAttr temp_att = _pool.GetSystemClass<RandAttr>();
            temp_att.type = attList[i][0];
            temp_att.param1 = attList[i][1];
            temp_att.param2 = attList[i][2];
            temp_att.value1 = attList[i][3];
            temp_att.value2 = attList[i][4];
            temp_att.configId1 = attList[i][5];
            temp_att.configId2 = attList[i][6];
            if (temp_att.type == 1)
            {
                FCattrList.Add(temp_att);
            }
            else if (temp_att.type == 3)
            {
                FCjobList.Add(temp_att);
            }
            else if (temp_att.type == 4)
            {
                FCjobAllList.Add(temp_att);
            }
        }
        //Debug.Log(showFirstChargeAttr);
        for (int i = 0; i < FCattrList.Count; i++)
        {
            TipProperty pro = _pool.GetCustomClass<TipProperty>();
            int proId = FCattrList[i].param1;
            int proValue = FCattrList[i].value1;
            int MaxproId = FCattrList[i].param2;
            int MaxproValue = FCattrList[i].value2;

            int per = ClientAttributeTableManager.Instance.GetClientAttributePer(proId);
            string value = proValue.ToString();
            int temp_maxValue = RandomAttrValueTableManager.Instance.GetItemCfg(_itemCfg.levClass, 1, proId);
            string maxValue = temp_maxValue.ToString();
            string needPlus = ClientAttributeTableManager.Instance.GetClientAttributePlus(proId) == 1 ? add : " ";
            //Debug.Log($"{_itemCfg.subType}   {proId}    ");
            if (MaxproValue != 0)
            {
                value = $"{proValue}~{MaxproValue}";
                int temp_maxValue2 = RandomAttrValueTableManager.Instance.GetItemCfg(_itemCfg.levClass, 1, MaxproId);
                string maxValue2 = temp_maxValue2.ToString();
                int clientTipsId = ClientAttributeTableManager.Instance.GetClientAttributeAttached(proId);
                if (per > 0)
                {
                    value =
                        $"{Math.Round(Convert.ToDecimal(proValue * 0.01f), 1, MidpointRounding.AwayFromZero)}%~{Math.Round(Convert.ToDecimal(MaxproValue * 0.01f), 1, MidpointRounding.AwayFromZero)}%";
                    maxValue =
                        $"{Math.Round(Convert.ToDecimal(temp_maxValue * 0.01f), 1, MidpointRounding.AwayFromZero)}%";
                    maxValue2 =
                        $"{Math.Round(Convert.ToDecimal(temp_maxValue2 * 0.01f), 1, MidpointRounding.AwayFromZero)}%";
                }

                pro.Name = $"{ClientTipsTableManager.Instance.GetClientTipsContext(clientTipsId)}{needPlus}{value}";
                pro.Value = proValue;
                pro.MaxValue = temp_maxValue;
                pro.MaxValueName = string.Format(SpecialStr, maxValue, maxValue2);
                int qua1 = FCattrList[i].configId1;
                int qua2 = FCattrList[i].configId2;
                pro.quality = (qua1 > qua2) ? qua1 : qua2;
                pro.persent = ClientAttributeTableManager.Instance.GetClientAttributePer(proId);
                itemslist.Properties.Add(pro);
            }
            else
            {
                if (per > 0)
                {
                    value = $"{Math.Round(Convert.ToDecimal(proValue * 0.01f), 1, MidpointRounding.AwayFromZero)}%";
                    maxValue =
                        $"{Math.Round(Convert.ToDecimal(temp_maxValue * 0.01f), 1, MidpointRounding.AwayFromZero)}%";
                }
                pro.Name =
                    $"{ClientTipsTableManager.Instance.GetClientTipsContext(ClientAttributeTableManager.Instance.GetClientAttributeTipID(proId))}{add}{value}";
                pro.Value = proValue;
                pro.ValueName = value;
                pro.MaxValue = temp_maxValue;
                pro.MaxValueName = string.Format(Str, maxValue);
                pro.quality = FCattrList[i].configId1;
                pro.persent = ClientAttributeTableManager.Instance.GetClientAttributePer(proId);
                itemslist.Properties.Add(pro);
            }
        }
        for (int i = 0; i < FCjobList.Count; i++)
        {
            int proId = FCjobList[i].param1;
            int proValue = FCjobList[i].value1;
            TipProperty pro = _pool.GetCustomClass<TipProperty>();
            pro.Name = $"{SkillTableManager.Instance.GetNameByGroupId(proId)}+{proValue}";
            pro.MaxValueName = string.Format(Str,
                RandomAttrValueTableManager.Instance.GetItemCfg(_itemCfg.levClass, 3, proId));
            pro.quality = FCjobList[i].configId1;
            itemslist.Properties.Add(pro);
        }
        for (int i = 0; i < FCjobAllList.Count; i++)
        {
            TipProperty pro = _pool.GetCustomClass<TipProperty>();
            pro.Name = $"{string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1623), Utility.GetJob(FCjobAllList[i].param1))}+{FCjobAllList[i].value1}";
            pro.MaxValueName = string.Format(Str,
                RandomAttrValueTableManager.Instance.GetItemCfg(_itemCfg.levClass, 4, FCjobAllList[i].param1));
            pro.quality = FCjobAllList[i].configId1;
            itemslist.Properties.Add(pro);
        }
        return itemslist;
    }
    #endregion
}