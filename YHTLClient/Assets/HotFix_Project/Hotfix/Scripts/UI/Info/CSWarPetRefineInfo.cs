using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using TABLE;
using UnityEngine;

public class CSWarPetRefineInfo : CSInfo<CSWarPetRefineInfo>
{
    public CSWarPetRefineInfo()
    {
        Init();
    }

    public override void Dispose()
    {

    }

    void Init()
    {
        //特殊技能gruopId
        int.TryParse(SundryTableManager.Instance.GetSundryEffect(704), out specialSkillGroupId);
        //技能预览表获取
        var arr = SkillTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.SKILL;
            if (item.usertype == 2 &&
                (item.type == 6 || item.type == 7))
            {
                if (item.skillGroup != specialSkillGroupId)
                    allPreviewSkills.Add(item);
            }
        }
    }

    /// <summary>
    /// 特殊技能当前ID(护体技能)
    /// </summary>
    public int SpecialSkillId
    {
        get
        {
            int id = 0;
            if (specialSkillGroupId > 0)
                id = specialSkillGroupId * 1000 + Mathf.Max(1, CSPetBasePropInfo.Instance.GetZhanHunSuitId());
            return id;
        }
    }

    /// <summary>
    /// 特殊技能组Id
    /// </summary>
    private int specialSkillGroupId = 0;

    public int SpecialSkillGroupId => specialSkillGroupId;

    /// <summary>
    /// 我的战宠主页面被动技能列表
    /// </summary>
    private ILBetterList<WarPetSkillData> myWarPetSkillDatas = new ILBetterList<WarPetSkillData>();

    public ILBetterList<WarPetSkillData> MyWarPetSkillDatas => myWarPetSkillDatas;

    /// <summary>
    /// 被动技能列表展示界面用
    /// </summary>
    ILBetterList<TABLE.SKILL> allPreviewSkills = new ILBetterList<TABLE.SKILL>();

    public ILBetterList<TABLE.SKILL> AllPreviewSkills => allPreviewSkills;

    /// <summary>
    /// 获取当前选中被动技能的消耗列表
    /// </summary>
    /// <param name="special">0:普通被动技能, 1:特殊技能(护体)</param>
    /// <returns></returns>
    public LongArray GetCurCost(int special)
    {
        TABLE.CHONGWUXILIANCOST chongwuxiliancost;
        if (ChongwuXilianCostTableManager.Instance.TryGetValue(CSPetBasePropInfo.Instance.GetZhanHunSuitId(),
            out chongwuxiliancost))
        {
            switch (special)
            {
                case 0:
                    return chongwuxiliancost.normalCost;
                case 1:
                    return chongwuxiliancost.specialCost;
            }
        }

        return default(LongArray);
    }

    /// <summary>
    /// 是否有足够消耗注入(只有注入使用)
    /// </summary>
    /// <returns></returns>
    public bool IsHasEnoughCost()
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_WarPetRefine)) return false;
        var kv = GetCurCost(0);
        for (int i = 0; i < myWarPetSkillDatas.Count; i++)
        {
            WarPetSkillData warPetSkillData = myWarPetSkillDatas[i];
            if (warPetSkillData.Special == 0 && warPetSkillData.ID == 0 && kv.IsItemsEnough())
                return true;
        }

        return false;
    }

    #region 网络响应处理

    List<List<string>> listAttrDisplays = new List<List<string>>();
    List<List<string>> listTmpAttrDisplays = new List<List<string>>();

    /// <summary>
    /// 处理宠物天赋被动技能信息
    /// </summary>
    /// <param name="msg"></param>
    public void HandlePetTianFuPassiveSkill(pet.PetTianFuPassiveSkillList msg)
    {
        if (msg == null) return;
        //被动技能槽所有信息
        for (int i = 0; i < msg.skillList.Count; i++)
        {
            pet.PetTianFuPassiveSkill skillItem = msg.skillList[i];
            if (myWarPetSkillDatas.Count <= i)
                myWarPetSkillDatas.Add(new WarPetSkillData());
            WarPetSkillData warPetSkillData = myWarPetSkillDatas[i];


            if (listAttrDisplays.Count <= i)
                listAttrDisplays.Add(new List<string>());
            List<string> str = listAttrDisplays[i];
            str.Clear();

            if (listTmpAttrDisplays.Count <= i)
                listTmpAttrDisplays.Add(new List<string>());
            List<string> strTmp = listTmpAttrDisplays[i];
            strTmp.Clear();

            RefeshData(warPetSkillData, skillItem, i);
        }

        myWarPetSkillDatas.Sort(SortWarPetSkillDatas);
    }

    /// <summary>
    /// 处理返回注入,洗炼,继续洗炼
    /// </summary>
    /// /// <param name="msg"></param>
    public void HandlePetTianFuRandomPassiveSkill(pet.PetTianFuPassiveSkill msg)
    {
        if (msg == null) return;
        for (int i = 0; i < myWarPetSkillDatas.Count; i++)
        {
            WarPetSkillData warPetSkillData = myWarPetSkillDatas[i];
            if (warPetSkillData.Pos == msg.pos)
            {
                if (listAttrDisplays.Count <= i)
                    listAttrDisplays.Add(new List<string>());
                List<string> str = listAttrDisplays[i];
                str.Clear();

                if (listTmpAttrDisplays.Count <= i)
                    listTmpAttrDisplays.Add(new List<string>());
                List<string> strTmp = listTmpAttrDisplays[i];
                strTmp.Clear();

                RefeshData(warPetSkillData, msg, i);
                break;
            }
        }

        myWarPetSkillDatas.Sort(SortWarPetSkillDatas);
    }

    /// <summary>
    /// 处理返回替换
    /// </summary>
    /// <param name="msg"></param>
    public void HandlePetTianFuChosePassiveSkill(pet.PetTianFuPassiveSkill msg)
    {
        if (msg == null) return;
        for (int i = 0; i < myWarPetSkillDatas.Count; i++)
        {
            WarPetSkillData warPetSkillData = myWarPetSkillDatas[i];
            if (warPetSkillData.Pos == msg.pos)
            {
                if (listAttrDisplays.Count <= i)
                    listAttrDisplays.Add(new List<string>());
                List<string> str = listAttrDisplays[i];
                str.Clear();

                if (listTmpAttrDisplays.Count <= i)
                    listTmpAttrDisplays.Add(new List<string>());
                List<string> strTmp = listTmpAttrDisplays[i];
                strTmp.Clear();

                RefeshData(warPetSkillData, msg, i);
                break;
            }
        }

        myWarPetSkillDatas.Sort(SortWarPetSkillDatas);
    }

    void RefeshData(WarPetSkillData warPetSkillData, pet.PetTianFuPassiveSkill skillItem, int index)
    {
        warPetSkillData.Special = skillItem.special;
        warPetSkillData.Pos = skillItem.pos;
        warPetSkillData.XiLianId = skillItem.xilianId;
        warPetSkillData.PetSkillAttrs = skillItem.attrs;
        warPetSkillData.TmpPetSkillAttrs = skillItem.tmpAttrs;
        warPetSkillData.CfgSkill = null;
        warPetSkillData.TmpCfgSkill = null;

        warPetSkillData.ID = warPetSkillData.Special == 0
            ? (skillItem.skillGroup == 0 ? 0 : skillItem.skillGroup * 1000 + 1)
            : (skillItem.skillGroup == 0
                ? 0
                : skillItem.skillGroup * 1000 + CSPetBasePropInfo.Instance.GetZhanHunSuitId());
        TABLE.SKILL tableSkill1;
        if (SkillTableManager.Instance.TryGetValue(warPetSkillData.ID, out tableSkill1))
            warPetSkillData.CfgSkill = tableSkill1;

        warPetSkillData.TmpID = warPetSkillData.Special == 0
            ? (skillItem.tmpSkillGroup == 0 ? 0 : skillItem.tmpSkillGroup * 1000 + 1)
            : (skillItem.tmpSkillGroup == 0
                ? 0
                : skillItem.tmpSkillGroup * 1000 + CSPetBasePropInfo.Instance.GetZhanHunSuitId());
        TABLE.SKILL tableSkill2;
        if (SkillTableManager.Instance.TryGetValue(warPetSkillData.TmpID, out tableSkill2))
            warPetSkillData.TmpCfgSkill = tableSkill2;

        //特殊技能配置赋值
        TABLE.SKILL tableSkill3;
        if (warPetSkillData.Special == 1 && SkillTableManager.Instance.TryGetValue(SpecialSkillId, out tableSkill3))
        {
            warPetSkillData.CfgSkill = tableSkill3;
            warPetSkillData.TmpCfgSkill = tableSkill3;
        }

        //属性值列表
        warPetSkillData.AttrDisplays = GetArrStrings(warPetSkillData.PetSkillAttrs, index, 1);
        warPetSkillData.TmpAttrDisplays = GetArrStrings(warPetSkillData.TmpPetSkillAttrs, index, 2);
    }

    string[] GetArrStrings(RepeatedField<pet.PetSkillAttr> listPetSkillAttrs, int index, int type)
    {
        List<string> attrDisplays = type == 1 ? listAttrDisplays[index] : listTmpAttrDisplays[index];
        if (listPetSkillAttrs != null && listPetSkillAttrs.Count > 0)
        {
            for (int i = 0; i < listPetSkillAttrs.Count; i++)
            {
                pet.PetSkillAttr petSkillAttr = listPetSkillAttrs[i];
                string str = Utility.GetPetSkillAttr(petSkillAttr.attrType, petSkillAttr.param, petSkillAttr.value,
                    petSkillAttr.color);
                // switch (petSkillAttr.attrType)
                // {
                //     case 1:
                //         TABLE.CLIENTATTRIBUTE clientattribute;
                //         if (ClientAttributeTableManager.Instance.TryGetValue(petSkillAttr.param,
                //             out clientattribute))
                //         {
                //             str = clientattribute.per <= 0
                //                 ? $"{petSkillAttr.value}".BBCode(petSkillAttr.color)
                //                 : $"{(petSkillAttr.value * 1f / 100).ToString("f1")}%".BBCode(petSkillAttr.color);
                //         }
                //
                //         break;
                //     case 2:
                //         TABLE.CHONGWUSHUXING chongwushuxing;
                //         if (ChongwuShuxingTableManager.Instance.TryGetValue(petSkillAttr.param,
                //             out chongwushuxing))
                //         {
                //             if (petSkillAttr.param == 2) //转化为品质文字
                //             {
                //                 str = UtilityColor.GetQualityText(petSkillAttr.value);
                //             }
                //             else
                //             {
                //                 str = chongwushuxing.num != 3
                //                     ? $"{petSkillAttr.value}".BBCode(petSkillAttr.color)
                //                     : $"{(petSkillAttr.value * 1f / 1000).ToString("f2")}".BBCode(petSkillAttr.color);
                //             }
                //         }
                //
                //         break;
                // }

                attrDisplays.Add(str);
            }
        }

        return attrDisplays.ToArray();
    }

    int SortWarPetSkillDatas(WarPetSkillData a, WarPetSkillData b)
    {
        return a.Pos.CompareTo(b.Pos);
    }

    #endregion
}


/// <summary>
/// 战宠被动技能数据
/// </summary>
public class WarPetSkillData
{
    /// <summary>
    /// 技能位置(0开始)
    /// </summary>
    public int Pos { get; set; }

    /// <summary>
    /// 是否特殊技能(0普通被动技能  1特殊护体技能)
    /// </summary>
    public int Special { get; set; }

    /// <summary>
    /// 洗炼ID
    /// </summary>
    public int XiLianId { get; set; }

    /// <summary>
    /// 技能ID
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// 当前洗炼出来的技能ID
    /// </summary>
    public int TmpID { get; set; }

    /// <summary>
    /// 当前技能配置
    /// </summary>
    public TABLE.SKILL CfgSkill { get; set; }

    /// <summary>
    /// 当前洗炼技能配置
    /// </summary>
    public TABLE.SKILL TmpCfgSkill { get; set; }

    /// <summary>
    /// 当前技能属性值数组
    /// </summary>
    public RepeatedField<pet.PetSkillAttr> PetSkillAttrs { get; set; }

    /// <summary>
    /// 洗炼出来的技能属性值数组
    /// </summary>
    public RepeatedField<pet.PetSkillAttr> TmpPetSkillAttrs { get; set; }

    /// <summary>
    /// 当前技能属性显示(带颜色)
    /// </summary>
    public string[] AttrDisplays { get; set; }

    /// <summary>
    /// 洗炼出来的技能属性显示(带颜色)
    /// </summary>
    public string[] TmpAttrDisplays { get; set; }

    public string Description
    {
        get
        {
            if (null != CfgSkill)
                return string.Format(CfgSkill.clientDescription, AttrDisplays);
            return string.Empty;
        }
    }
}