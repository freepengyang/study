using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pet;


public class CSPetTalentInfo : CSInfo<CSPetTalentInfo>
{
    /// <summary>
    /// 超过多少等级才会获得等级天赋点
    /// </summary>
    int getTalentPointLowestLv;

    /// <summary> 宠物等级 </summary>
    public int PetLv { get { return CSPetBasePropInfo.Instance.GetPetLevel();} }

    /// <summary> 来自等级的天赋点(注：并非宠物多少级就有多少点。) </summary>
    public int PointFromLv
    {
        get
        {
            int dot = PetLv - getTalentPointLowestLv;
            dot = dot < 0 ? 0 : dot;
            return dot;
        }
    }

    int pointFromEquip;
    /// <summary> 来自装备的天赋点 </summary>
    public int PointFromEquip { get { return pointFromEquip > MaxLevel ? MaxLevel : pointFromEquip; } }


    /// <summary> 回退点数。即实际生效的等级减去各种途径的点数。该值如小于0，则该值代表玩家可以手动点亮的数量。大于0则是回退数量。等于0则既没有可手动点亮的天赋也没有回退的天赋 </summary>
    public int BackspacePoint { get { return CurActivatedPoint - (pointFromEquip + PointFromLv); } }


    int activatedPage;
    /// <summary> 当前生效的天赋在第几页。后端发来 </summary>
    public int ActivatedPage { get { return activatedPage; } }

    int activatedLvInCurPage;
    /// <summary> 当前生效的天赋在当前页的等级。后端发来 </summary>
    public int ActivatedLvInCurPage { get { return activatedLvInCurPage; } }


    /// <summary> 当前实际生效的天赋等级，由后端传来 </summary>
    public int CurActivatedPoint
    {
        get
        {
            if (activatedPage <= 0) return 0;
            return (activatedPage - 1) * starPerPage + activatedLvInCurPage;
        }
    }

    /// <summary> 天赋总页数 </summary>
    public int MaxPage { get; set; }

    /// <summary> 天赋最大等级 </summary>
    public int MaxLevel { get; set; }


    /// <summary> 每页多少个天赋点 </summary>
    readonly int starPerPage = 9;
    /// <summary> 初始时从第几页开始（和后端保持一致） </summary>
    readonly int initPage = 1;
    /// <summary> 初始时从第几页的第几星开始（和后端保持一致） </summary>
    readonly int initStar = 0;


    /// <summary> 所有的天赋数据，key为第几页 </summary>
    Dictionary<int, FastArrayElementFromPool<CSTalentData>> allTalentData;

    /// <summary> 所有的核心天赋数据，不只包含大天赋，小天赋也有可能是核心天赋。通过配置表中的type字段判断 </summary>
    FastArrayElementKeepHandle<CSTalentData> coreTalentData;
    public FastArrayElementKeepHandle<CSTalentData> CoreTalentData { get { return coreTalentData; } }

    /// <summary> 所有核心数据 </summary>
    ILBetterList<CSTalentCore> allCoreList;


    /// <summary> 普通天赋描述文字缓存，key为clienttips对应id </summary>
    Dictionary<int, string> normalTalentDescCache = new Dictionary<int, string>();
    /// <summary> 核心天赋描述文字缓存，key为核心表对应id </summary>
    Dictionary<int, string> coreTalentDescCache = new Dictionary<int, string>();


    /// <summary>
    /// 玩家身上普通装备
    /// </summary>
    Dictionary<int, bag.BagItemInfo> playerEquips = new Dictionary<int, bag.BagItemInfo>();


    /// <summary> 天赋所有属性数据 </summary>
    Dictionary<int, CSTalentAttrData> allAttrDic = new Dictionary<int, CSTalentAttrData>();
    public Dictionary<int, CSTalentAttrData> AllAttrDic { get { return allAttrDic; } }


    PoolHandleManager mPoolHandle = new PoolHandleManager();


    //缓存
    /// <summary>
    /// 被动技能槽解锁等级
    /// </summary>
    int passiveSkillSlotUnlockLv;
    /// <summary>
    /// 合体技能解锁等级
    /// </summary>
    int combinedSkillUnlockLv;

    Dictionary<int, int> skillUnlockLv = new Dictionary<int, int>();


    #region UIString
    /// <summary> 天赋{0}级解锁 </summary>
    public string unlockClientStr;
    /// <summary> 总天赋等级：{0}级 </summary>
    public string curMaxLvClientStr;
    /// <summary> (装备额外+{0}级) </summary>
    public string equipLvClientStr;
    /// <summary> 天赋{0}级 </summary>
    public string talentLvStr;

    #endregion


    int lastFakeLvInPanel;
    /// <summary> 上次界面关闭时特效播放到的等级 </summary>
    public int LastFakeLvInPanel
    {
        get { return lastFakeLvInPanel; }
        set
        {
            lastFakeLvInPanel = value;
            mClientEvent.SendEvent(CEvent.PetTalentCheckRedpoint);
        }
    }


    public override void Dispose()
    {
        //mClientEvent.RemoveEvent(CEvent.WearEquip, EquipChangeEvent);
        //mClientEvent.RemoveEvent(CEvent.UnWeatEquip, EquipChangeEvent);
        mPoolHandle?.OnDestroy();
        mPoolHandle = null;

        allTalentData?.Clear();
        allTalentData = null;
        coreTalentData?.Clear();
        coreTalentData = null;

        allCoreList?.Clear();
        allCoreList = null;

        normalTalentDescCache?.Clear();
        normalTalentDescCache = null;

        coreTalentDescCache?.Clear();
        coreTalentDescCache = null;

        playerEquips?.Clear();
        playerEquips = null;

        allAttrDic?.Clear();
        allAttrDic = null;

        skillUnlockLv?.Clear();
        skillUnlockLv = null;
    }


    public CSPetTalentInfo()
    {
        Init();
        //mClientEvent.AddEvent(CEvent.WearEquip, EquipChangeEvent);
        //mClientEvent.AddEvent(CEvent.UnWeatEquip, EquipChangeEvent);
    }


    void Init()
    {
        string sundryLv = SundryTableManager.Instance.GetSundryEffect(700);
        int.TryParse(sundryLv, out getTalentPointLowestLv);
        
        activatedPage = initPage;
        activatedLvInCurPage = initStar;

        int cfgCount = ChongwuTianfuTableManager.Instance.array.gItem.id2offset.Count;
        MaxPage = Mathf.CeilToInt(cfgCount / starPerPage);
        MaxLevel = cfgCount;
        if (cfgCount < 1)
        {
            FNDebug.LogError("宠物天赋表无数据");
            return;
        }

        if (cfgCount % starPerPage != 0)
        {
            FNDebug.LogErrorFormat("宠物天赋表配置有误，有一页天赋不是{0}个！！！策划检查表格！！！", starPerPage);
        }

        if (allTalentData != null && allTalentData.Count > 0) return;

        if (allTalentData == null) allTalentData = new Dictionary<int, FastArrayElementFromPool<CSTalentData>>(256);
        if (coreTalentData == null) coreTalentData = new FastArrayElementKeepHandle<CSTalentData>(128);

        if (allCoreList == null) allCoreList = new ILBetterList<CSTalentCore>(128);
        else allCoreList.Clear();

        if (allAttrDic == null) allAttrDic = new Dictionary<int, CSTalentAttrData>(64);
        else allAttrDic.Clear();

        var arr = ChongwuTianfuTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            TABLE.CHONGWUTIANFU cfg = arr[k].Value as TABLE.CHONGWUTIANFU;
            if (cfg == null) continue;

            FastArrayElementFromPool<CSTalentData> list = null;      
            if (!allTalentData.TryGetValue(cfg.paging, out list))
            {
                list = mPoolHandle.CreateGeneratePool<CSTalentData>();
                allTalentData.Add(cfg.paging, list);
            }
            if (list != null)
            {
                CSTalentData data = list.Append();
                data.Init(cfg);
                if (cfg.type == 1)
                {
                    int fakeAttrId = cfg.tip;
                    if (cfg.tip == 1658 || cfg.tip == 1660 || cfg.tip == 1662)
                    {
                        fakeAttrId -= 1;
                    }
                    if (allAttrDic.ContainsKey(fakeAttrId)) continue;
                    CSTalentAttrData attr = InitAttrData(fakeAttrId);
                    if (attr != null) allAttrDic.Add(fakeAttrId, attr);
                }
                else if (cfg.type == 2)
                {
                    coreTalentData.Append(data);//所有核心天赋数据
                    TABLE.CHONGWUHEXIN coreCfg = null;
                    if (ChongwuHexinTableManager.Instance.TryGetValue(cfg.talentid, out coreCfg))
                    {
                        CSTalentCore core = mPoolHandle.GetCustomClass<CSTalentCore>();
                        core.Init(coreCfg, cfg.unlockinlevel);
                        allCoreList.Add(core);
                        data.LinkCore(core);
                    }
                    //else Debug.LogErrorFormat("宠物天赋表{0}的talentid为{1}，核心表中无此条数据，策划检查表格!!!", cfg.id, cfg.talentid);
                }
            }
        }

        for (var it = allTalentData.GetEnumerator(); it.MoveNext();)
        {
            var list = it.Current.Value;
            list.Sort(CoreTalentSorter);
        }

        coreTalentData.Sort(CoreTalentSorter);


        InitClientTipsStr();
    }

    protected void CoreTalentSorter(ref long sortValue,CSTalentData r)
    {
        sortValue = r.Id;
    }



    /// <summary>
    /// 天赋信息
    /// </summary>
    /// <param name="msg"></param>
    public void SC_TalentInfo(PetTianFuInfo msg)
    {
        SC_TalentUpdate(msg, false);
        LastFakeLvInPanel = CurActivatedPoint;
    }


    /// <summary>
    /// 天赋变化
    /// </summary>
    /// <param name="msg"></param>
    public void SC_TalentUpdate(PetTianFuInfo msg, bool needCoreTips)
    {
        int oldPage = activatedPage;
        int oldStar = activatedLvInCurPage;
        activatedPage = msg.paging < initPage ? initPage : msg.paging;
        activatedLvInCurPage = msg.starrating;
        pointFromEquip = msg.equipAddLevel;

        if (oldPage != activatedPage || oldStar != activatedLvInCurPage)
        {
            mClientEvent.SendEvent(CEvent.PetTalentLvChange);
        }

        int oldLv = (oldPage - 1) * starPerPage + oldStar;
        CheckTalentUnlock(oldLv, needCoreTips);     
    }


    void CheckTalentUnlock(int oldLv, bool needCoreUnlockTip = false)
    {
        var arr = ChongwuTianfuTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            TABLE.CHONGWUTIANFU item = arr[k].Value as TABLE.CHONGWUTIANFU;
            if (item.id <= oldLv || item.id > CurActivatedPoint) continue;
            if (item.type == 1)
            {
                int fakeAttrId = item.tip;
                if (item.tip == 1658 || item.tip == 1660 || item.tip == 1662)
                {
                    fakeAttrId -= 1;
                }
                if (allAttrDic == null || !allAttrDic.ContainsKey(fakeAttrId)) continue;
                allAttrDic[fakeAttrId].AddValue(item.tip, item.value);
            }
            else
            {
                if (!needCoreUnlockTip || item.talentid == 0) continue;
                //核心天赋tips
                string tips = CSPetTalentInfo.Instance.GetTalentDesc(item.talentid, 2);
                if (string.IsNullOrEmpty(tips)) continue;
                string newStr = tips.Replace("[u=skill]", "");
                UtilityTips.ShowGreenTips(newStr, 2.5f);
            }            
        }
    }

    

    /// <summary>
    /// 装备变化
    /// </summary>
    /// <param name="id"></param>
    /// <param name="param"></param>
    void EquipChangeEvent(uint id, object param)
    {
        int oldPoint = pointFromEquip;
        CSBagInfo.Instance.GetNormalEquip(playerEquips);

        int newPoint = 0;
        for (var it = playerEquips.GetEnumerator(); it.MoveNext();)
        {
            var bagInfo = it.Current.Value;
            if (bagInfo == null) continue;
            int cfgId = bagInfo.configId;
            string str = ItemTableManager.Instance.GetItemBufferParam(cfgId);
            if (!str.Contains("#")) continue;
            var strParam = str.Split('#');
            if (strParam.Length > 1)
            {
                int num = 0;
                int.TryParse(strParam[1], out num);
                newPoint += num;
            }            
        }

        pointFromEquip = newPoint;

        //if (oldPoint < pointFromEquip)//来自装备的点变少时再处理
        //    mClientEvent.SendEvent(CEvent.PetTalentEquipPointChange);目前界面打开时不考虑这种情况
    }


    void InitClientTipsStr()
    {
        unlockClientStr = ClientTipsTableManager.Instance.GetClientTipsContext(1691);
        curMaxLvClientStr = ClientTipsTableManager.Instance.GetClientTipsContext(1708);
        equipLvClientStr = ClientTipsTableManager.Instance.GetClientTipsContext(1709);
        talentLvStr = ClientTipsTableManager.Instance.GetClientTipsContext(1717);
    }


    CSTalentAttrData InitAttrData(int _id)
    {
        int id = _id;
        if (_id == 1657 || _id == 1659 || _id == 1661)
        {
            id = _id + 1;
        }
        CSTalentAttrData attr = mPoolHandle.GetCustomClass<CSTalentAttrData>();
        attr.Init(_id, id);
        return attr;
    }


    #region PublicFunctions
    /// <summary> 获取某一页的天赋信息 </summary>
    public FastArrayElementFromPool<CSTalentData> GetOnePageList(int page)
    {
        if (page < initPage || page > MaxPage || allTalentData == null) return null;

        FastArrayElementFromPool<CSTalentData> list = null;
        allTalentData.TryGetValue(page, out list);

        return list;
    }


    /// <summary>
    /// 获取天赋描述文字
    /// </summary>
    /// <param name="id"></param>
    /// <param name="talentType">对应配置表中的type，1为普通走clientTips，2为核心走ChongwuHexin</param>
    /// <returns></returns>
    public string GetTalentDesc(int id, int talentType = 1)
    {
        string desc = "";
        if (talentType == 1)
        {
            if (normalTalentDescCache == null) normalTalentDescCache = new Dictionary<int, string>();
            if (!normalTalentDescCache.ContainsKey(id))
            {
                desc = ClientTipsTableManager.Instance.GetClientTipsContext(id);
                normalTalentDescCache.Add(id, desc);
            }
            else desc = normalTalentDescCache[id];
        }

        if (talentType == 2)
        {
            if (coreTalentDescCache == null) coreTalentDescCache = new Dictionary<int, string>();
            if (!coreTalentDescCache.ContainsKey(id))
            {
                desc = ChongwuHexinTableManager.Instance.GetChongwuHexinTip(id);
                coreTalentDescCache.Add(id, desc);
            }
            else desc = coreTalentDescCache[id];
        }

        return desc;
    }



    /// <summary>
    /// 获取解锁宠物技能需要的天赋等级
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public int GetSkillUnlockLv(int groupId)
    {
        if (skillUnlockLv == null || allCoreList == null) return 0;
        if (skillUnlockLv.ContainsKey(groupId))
        {
            return skillUnlockLv[groupId];
        }

        CSTalentCore core = allCoreList.FirstOrNull(x => 
        {
            if (x.config == null) return false;
            var cfg = x.config;
            return x.coreType == 3 &&(cfg.para1 == groupId || cfg.para2 == groupId
            || cfg.para3 == groupId || cfg.para4 == groupId);
        });

        if (core != null)
        {
            skillUnlockLv.Add(groupId, core.unlockLv);
            return core.unlockLv;
        }
        return 0;
    }


    public int GetCoreUnlockLv(int coreId)
    {
        if (allCoreList == null) return 0;
        CSTalentCore core = allCoreList.FirstOrNull(x =>
        {
            if (x.config == null) return false;
            return x.Id == coreId;
        });

        if (core != null) return core.unlockLv;

        return 0;
    }


    public int GetPassiveSkillUnlockLv()
    {
        if (passiveSkillSlotUnlockLv > 0) return passiveSkillSlotUnlockLv;
        if (allCoreList != null)
        {
            for (int i = 0; i < allCoreList.Count; i++)
            {
                var core = allCoreList[i];
                if (core.config == null) continue;
                if (core.config.talenttype == 5)
                {
                    passiveSkillSlotUnlockLv = core.unlockLv;
                    break;
                }
            }
        }
        
        return passiveSkillSlotUnlockLv;
    }


    public int GetCombinedSkillUnlockLv()
    {
        if (combinedSkillUnlockLv > 0) return combinedSkillUnlockLv;
        if (allCoreList != null)
        {
            for (int i = 0; i < allCoreList.Count; i++)
            {
                var core = allCoreList[i];
                if (core.config == null) continue;
                if (core.config.talenttype == 6)
                {
                    combinedSkillUnlockLv = core.unlockLv;
                    break;
                }
            }
        }
        
        return combinedSkillUnlockLv;
    }
    

    public bool CheckRedPoint()
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_petHead))
        {
            return false;
        }
        return LastFakeLvInPanel < CurActivatedPoint;
    }


    #endregion

}


public class CSTalentData : IDispose
{
    int _id;
    /// <summary> 天赋表id，同时也是解锁等级  </summary>
    public int Id { get { return _id; } }

    public TABLE.CHONGWUTIANFU config;

    public string desc;

    /// <summary> 0小天赋，1大天赋。（不是普通或核心）。并非由config.type来决定。 </summary>
    public int talentType;

    /// <summary> 对应的核心数据 </summary>
    public CSTalentCore linkedCore;

    public void Dispose()
    {
        config = null;
        linkedCore = null;
    }

    public void Init(TABLE.CHONGWUTIANFU cfg)
    {
        if (cfg == null) return;
        config = cfg;
        _id = config.id;
        talentType = config.starrating % 9 == 0 ? 1 : 0;        
    }


    public void LinkCore(CSTalentCore core)
    {
        if (core != null && core.config != null)
        {
            linkedCore = core;
        }
    }


    public void SetDesc()
    {
        if (config == null) return;
        int descId = 0;
        if (config.type == 1)//描述文字读clienttips..
        {
            //int attrId = config.parameter;
            //descId = (int)ClientAttributeTableManager.Instance.GetClientAttributeTipID(attrId);
            descId = config.tip;
        }
        else if (config.type == 2)//读核心表
        {
            descId = config.talentid;
        }
        desc = CSPetTalentInfo.Instance.GetTalentDesc(descId, config.type);
    }
}


public class CSTalentCore : IDispose
{
    int _id;
    /// <summary> 核心表id  </summary>
    public int Id { get { return _id; } }

    public TABLE.CHONGWUHEXIN config;

    /// <summary> 核心类型，对应核心配置表中的talenttype </summary>
    public int coreType;

    /// <summary> 解锁需要的天赋等级  </summary>
    public int unlockLv;

    public void Dispose()
    {
        config = null;
    }

    public void Init(TABLE.CHONGWUHEXIN cfg, int _unlockLv)
    {
        if (cfg == null) return;
        config = cfg;
        _id = cfg.id;
        coreType = cfg.talenttype;
        unlockLv = _unlockLv;
    }


}



public class CSTalentAttrData : IDispose
{
    public int tipId;
    public int tipId2;//不合并的属性两个id相同

    public int value;
    public int value2;

    public string tipStr;

    public void Dispose() { }

    public void Init(int _id, int _id2)
    {        
        tipId = _id;
        tipId2 = _id2;
        value = 0;
        value2 = 0;
        if (tipId2 == tipId)
        {
            tipStr = ClientTipsTableManager.Instance.GetClientTipsContext(_id);
        }
        else
        {
            if (_id == 1657)
            {
                tipStr = ClientTipsTableManager.Instance.GetClientTipsContext(1732);
            }
            if (_id == 1659)
            {
                tipStr = ClientTipsTableManager.Instance.GetClientTipsContext(1733);
            }
            if (_id == 1661)
            {
                tipStr = ClientTipsTableManager.Instance.GetClientTipsContext(1734);
            }
        }
    }

    public void AddValue(int _id, int _value)
    {
        if (_id == tipId) value += _value;
        else if (_id == tipId2) value2 += _value;
    }
}

