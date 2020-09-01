using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UtilityFightPower
{
    static UtilityFightPower _instance;
    public static UtilityFightPower Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UtilityFightPower();
            }
            return _instance;
        }
        set { _instance = value; }
    }
    public UtilityFightPower()
    {
        SundryTableManager ins = SundryTableManager.Instance;
        minGongFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(351)) * 0.0001f;
        maxGongFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(352)) * 0.0001f;
        minFangFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(353)) * 0.0001f;
        maxFangFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(354)) * 0.0001f;
        hpFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(355)) * 0.0001f;
        mpFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(356)) * 0.0001f;
        shanbiFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(357)) * 0.0001f;
        mingzhongFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(358)) * 0.0001f;
        baolvFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(359)) * 0.0001f;
        baoshangFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(360)) * 0.0001f;
        perGongFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(361)) * 0.0001f;
        perFangFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(362)) * 0.0001f;
        perHpFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(363)) * 0.0001f;
        perMpFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(364)) * 0.0001f;
        paodianFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(365)) * 0.0001f;
        perPaoDianFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(366)) * 0.0001f;
        zhuangbeibaolvFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(367)) * 0.0001f;
        zhuangbeifangbaoFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(368)) * 0.0001f;
        zengshangFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(641)) * 0.0001f;
        jianshangFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(642)) * 0.0001f;
        attrFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(643)) * 0.0001f;
        zhanChongFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(645)) * 0.0001f;
        yuanhunFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(821)) * 0.0001f;
        yuanhunLvFactor = int.Parse(SundryTableManager.Instance.GetSundryEffect(822));
    }
    #region 系数定义
    float minGongFactor = 0;
    float maxGongFactor = 0;
    float perGongFactor = 0;
    float minFangFactor = 0;
    float maxFangFactor = 0;
    float perFangFactor = 0;
    float hpFactor = 0;
    float mpFactor = 0;
    float shanbiFactor = 0;
    float mingzhongFactor = 0;
    float baolvFactor = 0;
    float baoshangFactor = 0;
    float perHpFactor = 0;
    float perMpFactor = 0;
    float paodianFactor = 0;
    float perPaoDianFactor = 0;
    float zhuangbeibaolvFactor = 0;
    float zhuangbeifangbaoFactor = 0;
    float zengshangFactor = 0;
    float jianshangFactor = 0;

    //普通装备元魂属性加成
    float yuanhunFactor = 0;
    float yuanhunLvFactor = 0;

    float attrFactor = 0;
    float zhanChongFactor = 0;

    #endregion
    #region  属性定义
    //最小三攻
    float minWugong = 0;
    float minMogong = 0;
    float minDaogone = 0;
    //最大三攻
    float maxWugong = 0;
    float maxMogong = 0;
    float maxDaogong = 0;
    //百分比三攻
    float totalWugong = 0;
    float totalMogong = 0;
    float totalDaogong = 0;
    //最小双防
    float minWufang = 0;
    float minMofang = 0;
    //最大双防
    float maxWufang = 0;
    float maxMofang = 0;
    //百分比双防
    float totalWufang = 0;
    float totalMofang = 0;
    //生命
    float hp = 0;
    //法力
    float mp = 0;
    //闪避
    float shanbi = 0;
    //命中
    float mingzhong = 0;
    //暴击
    float baojilv = 0;
    //暴击率
    float baoshang = 0;
    //百分比生命
    float perHp = 0;
    //百分比法力
    float perMp = 0;
    //泡点固定值
    float paodian = 0;
    //泡点百分比
    float perPaodian = 0;
    //装备爆率
    float zhuangbeiBaolv = 0;
    //装备防爆
    float zhuangbeifangbao = 0;
    //增伤
    float zengshang = 0;
    //减伤
    float jianshang = 0;
    //普通装备元魂加成
    float yuanhunAttr = 0;
    //普通装备战魂等级加成
    float yuanhunLv = 0;

    #endregion

    List<List<int>> WLLonghunattr;
    float reault = 0;
    int Calculate(bag.BagItemInfo _info, TABLE.ITEM _cfg)
    {
        #region
        minWugong = 0;
        minMogong = 0;
        minDaogone = 0;
        maxWugong = 0;
        maxMogong = 0;
        maxDaogong = 0;
        totalWugong = 0;
        totalMogong = 0;
        totalDaogong = 0;
        minWufang = 0;
        minMofang = 0;
        maxWufang = 0;
        maxMofang = 0;
        totalWufang = 0;
        totalMofang = 0;
        hp = 0;
        mp = 0;
        shanbi = 0;
        mingzhong = 0;
        baojilv = 0;
        baoshang = 0;
        perHp = 0;
        perMp = 0;
        paodian = 0;
        perPaodian = 0;
        zhuangbeiBaolv = 0;
        zhuangbeifangbao = 0;
        zengshang = 0;
        jianshang = 0;
        yuanhunAttr = 0;
        yuanhunLv = 0;

        //基础属性读item
        minWugong = _cfg.phyAttMin;
        minMogong = _cfg.magicAttMin;
        minDaogone = _cfg.taoAttMin;
        maxWugong = _cfg.phyAttMax;
        maxMogong = _cfg.magicAttMax;
        maxDaogong = _cfg.taoAttMax;
        minWufang = _cfg.phyDefMin;
        minMofang = _cfg.magicDefMin;
        maxWufang = _cfg.phyDefMax;
        maxMofang = _cfg.magicDefMax;
        mingzhong = _cfg.accurate;
        shanbi = _cfg.dodge;
        baojilv = _cfg.critical;
        baoshang = _cfg.criticalDamage;
        hp = _cfg.hp;
        mp = _cfg.mp;

        #endregion

        if (_cfg.type == 10)//怀旧装备
        {
            float att = (float)Math.Pow(_cfg.levClass, attrFactor);
            float baseAttr = minWugong * minGongFactor + minMogong * minGongFactor + minDaogone * minGongFactor
                    + maxWugong * maxGongFactor + maxMogong * maxGongFactor + maxDaogong * maxGongFactor
                    + minWufang * minFangFactor + minMofang * minFangFactor + maxWufang * maxFangFactor + maxMofang * maxFangFactor
                    + hp * hpFactor + mp * mpFactor + paodian * paodianFactor
                    + (shanbi * shanbiFactor + mingzhong * mingzhongFactor + baojilv * baolvFactor + baoshang * baoshangFactor +
                    totalWugong * perGongFactor + totalMogong * perGongFactor + totalDaogong * perGongFactor + totalWufang * perFangFactor + totalMofang * perFangFactor
                    + perHp * perHpFactor + perMp * perMpFactor + perPaodian * perPaoDianFactor
                    + zhuangbeiBaolv * zhuangbeibaolvFactor + zhuangbeifangbao * zhuangbeifangbaoFactor + zengshang * zengshangFactor + jianshang * jianshangFactor) * att;
            reault = (int)baseAttr;
        }
        else
        {
            if (CSBagInfo.Instance.IsWoLongEquip(_cfg))
            {
                WolongScoreTableManager ins = WolongScoreTableManager.Instance;
                float power1 = 0;
                float power2 = 0;
                float power3 = 0;
                int valueType = 0;
                int value = 0;
                //龙魂
                if (WLLonghunattr == null)
                {
                    WLLonghunattr = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(41));
                }
                int mul = 0;
                if ((_cfg.subType % 100) == 1 || (_cfg.subType % 100) == 2)
                {
                    mul = WLLonghunattr[0][_cfg.levClass];
                }
                else
                {
                    mul = WLLonghunattr[1][_cfg.levClass];
                }
                valueType = ins.GetWolongScoreValueType(_cfg.levClass * 10000 + 1 * 100);
                value = ins.GetWolongScoreValue(_cfg.levClass * 10000 + 1 * 100);
                float valueF = (valueType == 1) ? (value) : (value / 10000f);
                power1 = (mul * valueF) / 10000f;
                //龙技
                float value2 = ins.GetWolongScoreValue(_cfg.levClass * 10000 + 2 * 100);
                int valueType2 = ins.GetWolongScoreValueType(_cfg.levClass * 10000 + 2 * 100);
                for (int i = 0; i < _info.longJis.Count; i++)
                {
                    power2 = power2 + _info.longJis[i].effectValue * ((valueType2 == 1) ? value2 : value2 / 10000f);
                }
                //强化词缀
                for (int i = 0; i < _info.intensifyAffixs.Count; i++)
                {
                    bag.WolongRandomEffect _mes = _info.intensifyAffixs[i];
                    int effectId = WoLongRandomAttrTableManager.Instance.GetWoLongRandomAttrParameter(_mes.id);
                    TABLE.WOLONGSCORE tbl;
                    ins.TryGetValue(_cfg.levClass * 10000 + 3 * 100 + effectId, out tbl);
                    if (tbl != null)
                    {
                        float re = (_mes.effectValue * 1f) / (Math.Abs(tbl.valueMax - tbl.valueMin)) * tbl.value;
                        //FNDebug.Log($"{re}     {tbl.id}     {_mes.effectValue}      {tbl.valueMax}      {tbl.valueMin}      {tbl.value}");
                        power3 = power3 + re;
                    }
                }
                float att = (float)Math.Pow(_cfg.levClass, attrFactor);
                float baseAttr = minWugong * minGongFactor + minMogong * minGongFactor + minDaogone * minGongFactor
                    + maxWugong * maxGongFactor + maxMogong * maxGongFactor + maxDaogong * maxGongFactor
                    + minWufang * minFangFactor + minMofang * minFangFactor + maxWufang * maxFangFactor + maxMofang * maxFangFactor
                    + hp * hpFactor + mp * mpFactor + paodian * paodianFactor
                    + (shanbi * shanbiFactor + mingzhong * mingzhongFactor + baojilv * baolvFactor + baoshang * baoshangFactor +
                    totalWugong * perGongFactor + totalMogong * perGongFactor + totalDaogong * perGongFactor + totalWufang * perFangFactor + totalMofang * perFangFactor
                    + perHp * perHpFactor + perMp * perMpFactor + perPaodian * perPaoDianFactor
                    + zhuangbeiBaolv * zhuangbeibaolvFactor + zhuangbeifangbao * zhuangbeifangbaoFactor + zengshang * zengshangFactor + jianshang * jianshangFactor) * att + yuanhunAttr + yuanhunLv;
                reault = (int)(power1 + power2 + power3 + baseAttr);
            }
            else if (CSBagInfo.Instance.IsNormalEquip(_cfg))
            {
                for (int i = 0; i < _info.randAttrValues.Count; i++)
                {
                    bag.RandAttr attr = _info.randAttrValues[i];
                    if (attr.type == 1)
                    {
                        if (attr.param1 == 2)
                        {
                            minWugong = (minWugong + attr.value1);
                            maxWugong = (maxWugong + attr.value2);
                        }
                        else if (attr.param1 == 4)
                        {
                            minMogong = (minMogong + attr.value1);
                            maxMogong = (maxMogong + attr.value2);
                        }
                        else if (attr.param1 == 6)
                        {
                            minDaogone = (minDaogone + attr.value1);
                            maxDaogong = (maxDaogong + attr.value2);
                        }
                        else if (attr.param1 == 8)
                        {
                            minWufang = (minWufang + attr.value1);
                            maxWufang = (maxWufang + attr.value2);
                        }
                        else if (attr.param1 == 10)
                        {
                            minMofang = (minMofang + attr.value1);
                            maxMofang = (maxMofang + attr.value2);
                        }
                        else if (attr.param1 == 11)
                        {
                            hp = (hp + attr.value1);
                        }
                        else if (attr.param1 == 12)
                        {
                            mp = (mp + attr.value1);
                        }
                        else if (attr.param1 == 15)
                        {
                            mingzhong = (mingzhong + attr.value1);
                        }
                        else if (attr.param1 == 16)
                        {
                            shanbi = (shanbi + attr.value1);
                        }
                        else if (attr.param1 == 14)
                        {
                            baojilv = (baojilv + attr.value1);
                        }
                        else if (attr.param1 == 13)
                        {
                            baoshang = (baoshang + attr.value1);
                        }
                        else if (attr.param1 == 19)
                        {
                            totalWugong = (totalWugong + attr.value1);
                        }
                        else if (attr.param1 == 20)
                        {
                            totalMogong = (totalMogong + attr.value1);
                        }
                        else if (attr.param1 == 21)
                        {
                            totalDaogong = (totalDaogong + attr.value1);
                        }
                        else if (attr.param1 == 17)
                        {
                            totalWufang = (totalWufang + attr.value1);
                        }
                        else if (attr.param1 == 18)
                        {
                            totalMofang = (totalMofang + attr.value1);
                        }
                        else if (attr.param1 == 22)
                        {
                            perHp = (perHp + attr.value1);
                        }
                        else if (attr.param1 == 23)
                        {
                            perMp = (perMp + attr.value1);
                        }
                        else if (attr.param1 == 28)
                        {
                            paodian = (paodian + attr.value1);
                        }
                        else if (attr.param1 == 39)
                        {
                            perPaodian = (perPaodian + attr.value1);
                        }
                        else if (attr.param1 == 33)
                        {
                            zhuangbeifangbao = (zhuangbeifangbao + attr.value1);
                        }
                        else if (attr.param1 == 47)
                        {
                            zhuangbeiBaolv = (zhuangbeiBaolv + attr.value1);

                        }
                    }
                }
                float att = (float)Math.Pow(_cfg.levClass, attrFactor);
                List<int> yuanhunList = UtilityMainMath.SplitStringToIntList(_cfg.bufferParam);
                if (yuanhunList.Count > 1)
                {
                    yuanhunAttr = yuanhunFactor * yuanhunList[0] * _cfg.levClass;
                    yuanhunLv = yuanhunLvFactor * yuanhunList[1];
                }
                reault = minWugong * minGongFactor + minMogong * minGongFactor + minDaogone * minGongFactor
                    + maxWugong * maxGongFactor + maxMogong * maxGongFactor + maxDaogong * maxGongFactor
                    + minWufang * minFangFactor + minMofang * minFangFactor + maxWufang * maxFangFactor + maxMofang * maxFangFactor
                    + hp * hpFactor + mp * mpFactor + paodian * paodianFactor
                    + (shanbi * shanbiFactor + mingzhong * mingzhongFactor + baojilv * baolvFactor + baoshang * baoshangFactor +
                    totalWugong * perGongFactor + totalMogong * perGongFactor + totalDaogong * perGongFactor + totalWufang * perFangFactor + totalMofang * perFangFactor
                    + perHp * perHpFactor + perMp * perMpFactor + perPaodian * perPaoDianFactor
                    + zhuangbeiBaolv * zhuangbeibaolvFactor + zhuangbeifangbao * zhuangbeifangbaoFactor + zengshang * zengshangFactor + jianshang * jianshangFactor) * att + yuanhunAttr + yuanhunLv;
            }
        }

        return (int)reault;
    }


    public static int GetFightPower(bag.BagItemInfo _info, TABLE.ITEM _cfg)
    {
        if (_cfg.type != (int)ItemType.Equip && _cfg.type != (int)ItemType.NostalgicEquip)
        {
            return 0;
        }
        return Instance.Calculate(_info, _cfg);
    }
}
