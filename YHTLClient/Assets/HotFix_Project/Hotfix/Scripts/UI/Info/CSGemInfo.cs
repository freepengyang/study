using System.Collections.Generic;
using gem;
using bag;
using Google.Protobuf.Collections;
using TABLE;

public class CSGemInfo : CSInfo<CSGemInfo>
{
    public Map<int, PosGemSuit> suitList = new Map<int, PosGemSuit>(); //套装客户端数据

    public List<GemData> gemDatas = new List<GemData>(); //玩家身上的宝石列表客户端数据

    public Dictionary<int,int> GemTabRedDic = new Dictionary<int, int>();//宝石红点列表
    
    private PosGemInfo _info { get; set; } //服务端发过来的数据
    private GemSuit suit { get; set; } //玩家达成的套装id服务器数据
    PoolHandleManager mPoolHandleManager = new PoolHandleManager();

    /// <summary>
    /// 背包中最大宝石的缓存列表 如果列表中有对应的宝石那么就不用在遍历
    /// </summary>
    private Map<int, int> MaxBagGemIds = new Map<int, int>(); 

    /// <summary>
    /// 每个分页签第一个可以升级的红点
    /// </summary>
    public Dictionary<int,GemInfo> LevelUpGems = new Dictionary<int,GemInfo>(); 

    
    public void SetData(PosGemInfo info)
    {
        _info = info;
        gemDatas.Clear();
        var arr = GemSlotTableManager.Instance.array.gItem.handles;

        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            GemData gemData = new GemData();
            gemData.subType = arr[k].key;
            gemData.bossCounter = _info.bossCounter;
            gemData.unlockingPosition = _info.unlockingPosition;
            //Debug.Log("11" + _info.unlockingPosition);
            if (_info != null)
            {
                gemData.gemslot = arr[k].Value as TABLE.GEMSLOT;
                    // GemSlotTableManager.Instance.dic[iter.Current.Key];
                gemData.SetData(_info);
                //Debug.Log(gemData.GemInfos[1].pos);
            }

            gemDatas.Add(gemData);
        }
    }

    public void SetSuit(GemSuit data)
    {
        suit = data;
        suitList.Clear();
        //这里是13条数据与slot数据对应 0代表所有

        for (int i = 0,max = GemSlotTableManager.Instance.array.gItem.id2offset.Count; i <= max; i++)
        {
            PosGemSuit posGemSuit = null;
            for (int j = 0; j < data.sutis.Count; j++)
            {
                if (data.sutis[j].pos == i)
                {
                    posGemSuit = data.sutis[j];
                }
            }

            if (posGemSuit == null)
            {
                posGemSuit = new PosGemSuit();
                posGemSuit.configId = 0;
                posGemSuit.pos = i;
            }

            suitList.Add(i, posGemSuit);
        }

        mClientEvent.SendEvent(CEvent.GemSuitInfoChange);
    }

    /// <summary>
    /// 从背包里获取可以镶嵌的宝石列表
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public List<BagItemInfo> GetGeminlayList(int location)
    {
        //宝石位置列表
        List<BagItemInfo> listBagItemInfo = new List<BagItemInfo>();
        var geminfos = gemDatas[location - 1].GemInfos;
        for (geminfos.Begin();geminfos.Next(); )
        {
            //BagItemInfo bagiteminfo = null;
            BagItemInfo bagiteminfo = CSBagInfo.Instance.GethighGemInfoByPos(geminfos.Value.pos);
            if ( bagiteminfo != null && geminfos.Value.gemId < bagiteminfo.configId )
            {
                listBagItemInfo.Add(bagiteminfo);
            }
            
            //if (geminfos.Value.gemId == 0)
            //    bagiteminfo = CSBagInfo.Instance.GethighGemInfoByPos(geminfos.Value.pos);
            // if (bagiteminfo != null)
            //     listBagItemInfo.Add(bagiteminfo);
        }
        
        return listBagItemInfo;
    }

    public void SetUnlockData(UnlockGemPosition Unlockinfo)
    {
        //_info.unlockingPosition = Unlockinfo.unlockingPosition;
        for (int i = 0; i < gemDatas.Count; i++)
        {
            gemDatas[i].unlockingPosition = Unlockinfo.unlockingPosition;
        }

        //Debug.Log("SetUnlockData" + Unlockinfo.unlockingPosition);
        //SetData(_info);
        mClientEvent.SendEvent(CEvent.CSUnlockGemPositionMessage);
    }

    public void SetBossCount(int count)
    {
        if (gemDatas.Count > 0)
        {
            for (int i = 0; i < gemDatas.Count; i++)
            {
                gemDatas[i].bossCounter = count;
            }
        }
        mClientEvent.SendEvent(CEvent.GemBossKillCount,count);
    }

    public void SetPosGemChangeData(PosGemChange PosGemInfo)
    {
        //Debug.Log(PosGemInfo.subType + "|" + PosGemInfo.pos);
        GemInfo info = new GemInfo();
        info.pos = PosGemInfo.pos;
        info.subType = PosGemInfo.subType;
        info.gemId = PosGemInfo.gemId;
        gemDatas[PosGemInfo.subType - 1].SetGemInfos(info);
        //Debug.Log(gemDatas[0].GemInfos[1].gemId);
        mClientEvent.SendEvent(CEvent.CSGemRefresh, info);
    }

    /// <summary>
    /// 根据宝石等级获取身上大于当前宝石等级的宝石数量
    /// </summary>
    /// <param name="lv">等级 如果等级为0那么返回 表中最小等级</param>
    /// <param name="gemPosition">宝石部位，如果为0则获取全身宝石</param>
    public int GetGemNum(int gemPosition = 0, int lv = 0)
    {
        int num = 0;
        if (lv == 0)
        {
            var arr = GemSuitTableManager.Instance.array.gItem.handles;
            for(int i = 0,max = arr.Length;i < max;++i)
            {
                var item = arr[i].Value as TABLE.GEMSUIT;
                if (item.gamPosition == gemPosition)
                {
                    lv = item.gamLevel;
                    break;
                }
            }
        }

        for (int i = 0; i < gemDatas.Count; i++)
        {
            GemData gemdata = gemDatas[i];
            var geminfos = gemDatas[i].GemInfos;
            for (geminfos.Begin();geminfos.Next();)
            {
                int gemid = geminfos.Value.gemId;
                if (gemPosition != 0)
                {
                    //Debug.Log(GemTableManager.Instance.GetGemLv(gemid) + "||" + lv + "||" + gemPosition + "||" + GemTableManager.Instance.GetGemPosition(gemid));


                    if (GemTableManager.Instance.GetGemLv(gemid) >= lv && gemdata.subType == gemPosition)
                    {
                        num++;
                    }
                }
                else
                {
                    if (GemTableManager.Instance.GetGemLv(gemid) >= lv)
                        num++;
                }
            }
        }
        
        return num;
    }

    /// <summary>
    /// 获取当前套装的下一等级套装，如果套装为最高级，那么返回最高级
    /// </summary>
    /// <param name="suitData"></param>
    /// <returns></returns>
    public GEMSUIT GetNextSuit(int id, int subType)
    {
        if (id == 0)
            return GetGemSuitLow(subType);
        GEMSUIT suitData;
        GemSuitTableManager.Instance.TryGetValue(id, out suitData);
        GEMSUIT maxgemsuit = null;
        var arr = GemSuitTableManager.Instance.array.gItem.handles;
        TABLE.GEMSUIT gemSuitItem = null;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            gemSuitItem = arr[i].Value as TABLE.GEMSUIT;
            if (gemSuitItem.gamPosition == suitData.gamPosition)
            {
                maxgemsuit = gemSuitItem;
                if (gemSuitItem.level > suitData.level)
                    return gemSuitItem;
            }
        }

        return maxgemsuit;
    }

    public bool IsMaxLV(int id)
    {
        if (id == 0)
        {
            return false;
        }

        var arr = GemTableManager.Instance.array.gItem.handles;
        GEM gem;
        if (GemTableManager.Instance.TryGetValue(id, out gem))
        {
            for (int i = 0, max = arr.Length; i < max; ++i)
            {
                var item = arr[i].Value as GEM;
                if (item.position == gem.position&& item.lv > gem.lv)
                {
                    return false;
                }
            }
        }

        return true;

    }



    /// <summary>
    /// 根据位置获取最低套装
    /// </summary>
    /// <param name="gamPosition"></param>
    public GEMSUIT GetGemSuitLow(int gamsubType)
    {
        if (gamsubType > GemSlotTableManager.Instance.array.gItem.id2offset.Count)
        {
            FNDebug.LogError("gamPosition is error");
            return null;
        }

        var arr = GemSuitTableManager.Instance.array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as TABLE.GEMSUIT;
            if (item.gamPosition == gamsubType)
            {
                return item;
            }
        }

        return null;
    }

    public RepeatedField<CSAttributeInfo.KeyValue> GetallAttr()
    {
        if (gemDatas.Count <= 0)
        {
            return null;
        }
        
        var attrs = mPoolHandleManager.GetSystemClass<RepeatedField<KEYVALUE>>();
        attrs.Clear();

        //现在如果玩家无属性加成,直接显示无属性加成 
        for (int i = 1; i <= 5; i++)
        {
            int id = GetGemTableLow(i);
            GEM Data;
            GemTableManager.Instance.TryGetValue(id, out Data);
            var AttrInfo =
                CSGemInfo.Instance.GetAttrParaByCareer(CSMainPlayerInfo.Instance.Career, Data);
        
            RepeatedField<KEYVALUE> copyAttrInfo = new RepeatedField<KEYVALUE>();
            for (int j = 0; j < AttrInfo.Count; j++)
            {
                KEYVALUE keyvalue = new KEYVALUE();
                keyvalue.key = AttrInfo[j].key();
                keyvalue.value = AttrInfo[j].value();
                copyAttrInfo.Add(keyvalue);
                copyAttrInfo[j].value = 0;
            }
        
            attrs.AddRange(copyAttrInfo);
        }

        bool IsAttr = false; //是否有属性加成
        for (int i = 0; i < gemDatas.Count; i++)
        {
            var geminfos = gemDatas[i].GemInfos;
            for (geminfos.Begin();geminfos.Next();)
            {
                if (geminfos.Value.gemId != 0)
                {
                    GEM gemTableData;
                    GemTableManager.Instance.TryGetValue(geminfos.Value.gemId, out gemTableData);
                    var listAttrInfo =
                        CSGemInfo.Instance.GetAttrParaByCareer(CSMainPlayerInfo.Instance.Career, gemTableData);
                    //Debug.Log("listAttrInfo[0].value" + listAttrInfo[0].value); 
                    for(int k = 0; k < listAttrInfo.Count;++k)
                    {
                        KEYVALUE keyvalue = new KEYVALUE();
                        keyvalue.key = listAttrInfo[k].key();
                        keyvalue.value = listAttrInfo[k].value();
                        attrs.Add(keyvalue);
                    }
                    //attrs.AddRange(listAttrInfo);
                    IsAttr = true;
                }
            }
        }

        if (IsAttr == false)
        {
            return null;
        }
        
        //Debug.Log("attrs.Count" + attrs.Count);
        if (attrs.Count > 0)
        {
            return CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, attrs);
        }

        return null;
    }

    /// <summary>
    /// 根据职业获取相应属性id和数值
    /// </summary>
    /// <param name="career"></param>
    public LongArray GetAttrParaByCareer(int career, GEM data)
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
    /// 返回表中最低级的宝石id
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public int GetGemTableLow(int pos)
    {
        if (pos == 0)
            return 0;
        var arr = GemTableManager.Instance.array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as GEM;
            if (item.position == pos)
            {
                return item.id;
            }
        }

        return 0;
    }

    /// <summary>
    /// 判断背包里的宝石和比身上的宝石哪个更高 如果身上没有宝石返回false 用来做宝石提升箭头的判断
    /// </summary>
    public bool judgeBagGemByBody(int subtype, int pos)
    {
        int gemid = 0;
        for (int i = 0; i < gemDatas.Count; i++)
        {
            if (gemDatas[i].subType == subtype)
                gemid = gemDatas[i].GemInfos[pos].gemId;
        }

        BagItemInfo bagiteminfo = CSBagInfo.Instance.GethighGemInfoByPos(pos);
        if (bagiteminfo == null || gemid == 0)
            return false;

        //Debug.Log("FDFD" + bagiteminfo.configId+ "||" + gemid);
        if (GemTableManager.Instance.GetGemLv(bagiteminfo.configId) > GemTableManager.Instance.GetGemLv(gemid))
            return true;
        else
            return false;
    }


    public void SetMaxBagGem(GemInfo info)
    {
        if (info.gemId != 0)
        {
            MaxBagGemIds.Remove(info.pos);
        }
            
    }


    /// <summary>
    /// 根据类型判断背包里的宝石和比身上的宝石哪个更高 如果如果身上没有宝石 宝石等级为0
    /// </summary>
    /// <param name="subtype"></param>
    /// <returns></returns>
    public bool judgeBagGemByBody(int subtype)
    {
        GemData gemdatatemp = null;
        for (int i = 0; i < gemDatas.Count; i++)
        {
            if (gemDatas[i].subType == subtype)
            {
                gemdatatemp = gemDatas[i];
                break;
            }
        }

        if (gemdatatemp == null)
        {
            FNDebug.LogError("数据错误 subtype" + subtype);
            return false;
        }

        var geminfos = gemdatatemp.GemInfos;
        
        for (geminfos.Begin();geminfos.Next();)
        {
            int gemid = geminfos.Value.gemId;

            int configid;
            if (!MaxBagGemIds.ContainsKey(geminfos.Value.pos))
            {
                BagItemInfo bagiteminfo = CSBagInfo.Instance.GethighGemInfoByPos(geminfos.Value.pos);
                if (bagiteminfo == null)
                    continue;
                MaxBagGemIds.Add(geminfos.Value.pos,bagiteminfo.configId);

                configid = bagiteminfo.configId;
            }
            else
            {
                configid = MaxBagGemIds[geminfos.Value.pos];
            }

            if (GemTableManager.Instance.GetGemLv(configid) > GemTableManager.Instance.GetGemLv(gemid))
                return true;
        }

        return false;
    }

    public bool IsGemLevelUp(int subtype)
    {
        GemData gemdatatemp = null;
        LevelUpGems.Remove(subtype);
        for (int i = 0; i < gemDatas.Count; i++)
        {
            if (gemDatas[i].subType == subtype)
            {
                gemdatatemp = gemDatas[i];
                break;
            }
        }

        if (gemdatatemp == null)
        {
            FNDebug.LogError("数据错误 subtype" + subtype);
            return false;
        }

        var geminfos = gemdatatemp.GemInfos;
        
        
        for (geminfos.Begin();geminfos.Next();)
        {
            int gemid = geminfos.Value.gemId;
            GEM gemtable;
            if (GemTableManager.Instance.TryGetValue(gemid ,out gemtable))
            {
                BagItemInfo bagiteminfo = CSBagInfo.Instance.GetBagItemByConfigId(gemid);
                if (bagiteminfo == null)
                    continue;
                //var cost = gemtable.cost.Split('#');
                
                if (gemtable.cost.Length>0 && gemtable.cost[0].key() == gemid)
                {
                    if (bagiteminfo.count >= gemtable.cost[0].value())
                    {
                        if (gemtable.itemId.Length > 0)
                        {
                            var item = gemtable.itemId[0];
                            if (item.key().GetItemCount() >= item.value())
                            {
                                LevelUpGems[subtype] = geminfos.Value;
                                return true;
                            }
                        }
                        else
                        {
                            var item = gemtable.itemId[0];
                            return true;
                        }


                    }
                }
            }
        }

        return false;
    }

    
    
    public bool IsGemLevelUp(int subtype, int pos)
    {
        int gemid = 0;
        for (int i = 0; i < gemDatas.Count; i++)
        {
            if (gemDatas[i].subType == subtype)
                gemid = gemDatas[i].GemInfos[pos].gemId;
        }

        BagItemInfo bagiteminfo = CSBagInfo.Instance.GetBagItemByConfigId(gemid);
        if (bagiteminfo == null || gemid == 0)
            return false;

        GEM gemtable;
        if (GemTableManager.Instance.TryGetValue(gemid, out gemtable))
        {
            //var cost = gemtable.cost.Split('#');
            if (gemtable.cost.Length > 0&& gemtable.cost[0].key() == gemid)
            {
				
                if (gemtable.itemId.Length > 0)
                {
                    var item = gemtable.itemId[0];
                    if (item.key().GetItemCount() >= item.value())
                    {
                        return bagiteminfo.count >= gemtable.cost[0].value();
                    }
                    
                }
                else
                {
                    return bagiteminfo.count >= gemtable.cost[0].value();
                }
            }
        }
        
        return false;
    }

    /// <summary>
    /// 判断背包里的宝石和比身上的宝石哪个更高 传入背包中的id值
    /// </summary>
    /// <param name="gemid"></param>
    /// <returns></returns>
    public bool judgebodyGemByBag(int gemid)
    {
        //Debug.Log("gemid" + gemid);
        if (gemid == 0)
            return false;
        int lv = GemTableManager.Instance.GetGemLv(gemid);
        int pos = GemTableManager.Instance.GetGemPosition(gemid);
        for (int i = 0; i < gemDatas.Count; i++)
        {
            GemData gemData = gemDatas[i];
            var geminfos = gemDatas[i].GemInfos;
            for (geminfos.Begin();geminfos.Next();)
            {
                bool islock = gemData.unlockingPosition == -1 || gemData.unlockingPosition > gemData.subType; //判断是否解锁

                if (pos == geminfos.Value.pos && islock)
                {
                    if (geminfos.Value.gemId == 0)
                        return true;
                    if (lv > GemTableManager.Instance.GetGemLv(geminfos.Value.gemId))
                        return true;
                }
            }
        }

        return false;
    }

    public override void Dispose()
    {

        suitList = null;
        gemDatas = null;
        _info = null;
        suit = null;
        MaxBagGemIds = null;
        mPoolHandleManager = null;

    }

   

    public List<GemData> GetgemDatas()
    {
        return gemDatas;
    }

    public bool GemLevelUpCheck()
    {
        return LevelUpGems.Count > 0;
    }

    public bool GemRedCheck(EventData eventData = null)
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_baoshi))
        {
            return false;
        }
        
        if (gemDatas == null||gemDatas.Count == 0)
        {
            return false;
        }
        //根据返回的背包类型来添加数据,避免再一次遍历背包
        if (eventData != null &&eventData.arg1 is BagItemInfo bagitemInfo &&eventData.arg2 is ItemChangeType itemChangeType)
        {
            if (itemChangeType == ItemChangeType.NumReduce || itemChangeType == ItemChangeType.Remove)
            {
                //ITEM cfg = ItemTableManager.Instance.GetItemCfg(bagitemInfo.configId);
                GEM gem;
                if (bagitemInfo != null &&GemTableManager.Instance.TryGetValue(bagitemInfo.configId,out gem))
                {
                    if (MaxBagGemIds.ContainsKey(gem.position) && MaxBagGemIds[gem.position] == gem.id )
                    {
                        MaxBagGemIds.Remove(gem.position);
                    }  
                }
            }

            if (itemChangeType == ItemChangeType.Add || itemChangeType == ItemChangeType.NumAdd)
            {
                GEM gem;
                if (bagitemInfo != null &&GemTableManager.Instance.TryGetValue(bagitemInfo.configId,out gem))
                {
                    if (MaxBagGemIds.ContainsKey(gem.position) )
                    {
                        int maxlevel = GemTableManager.Instance.GetGemLv(MaxBagGemIds[gem.position]);
                        if (gem.lv >maxlevel)
                        {
                            MaxBagGemIds[gem.position] = gem.id;
                        }
                    }
                    else
                    {
                        MaxBagGemIds.Add(gem.position,gem.id);
                    }
                }
            }
        }

        int redNum = 0;
        var arr = GemSlotTableManager.Instance.array.gItem.handles;
        //遍历身上部位
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as GEMSLOT;
            if (item.id >= gemDatas[0].unlockingPosition && gemDatas[0].unlockingPosition != -1)
            {
                break;
            }
            int subtype = item.id;
            bool isBagLvMoreBody = judgeBagGemByBody(subtype);
            bool isLevelUp = IsGemLevelUp(subtype);
            bool isRed = isBagLvMoreBody || isLevelUp;
            int redInt = (isBagLvMoreBody ? 1 : 0) | ((isLevelUp ? 1 : 0 )<< 1); 
            
            if (isRed)
                redNum++;
            
            
            if (GemTabRedDic.ContainsKey(subtype))
                GemTabRedDic[subtype] = redInt;
            else
                GemTabRedDic.Add(subtype,redInt);
        }
        mClientEvent.SendEvent(CEvent.GemTabRedChange);
        
        return redNum > 0;
        
    }
    
    
    
}

public class GemData : IDispose
{
    //public Dictionary<int, List<GemInfo>> GemPartInfos = new Dictionary<int, List<GemInfo>>();//每一个部件身上需要的信息 int值表示部件id
    public Map<int, GemInfo> GemInfos = new Map<int, GemInfo>();

    public TABLE.GEMSLOT gemslot { get; set; }

    public int bossCounter { get; set; }

    public int unlockingPosition { get; set; }

    public int subType { get; set; } //部位值


    public void SetGemInfos(GemInfo info)
    {
        //Debug.Log("info.pos" +info.pos + "||" + info.gemId);
        GemInfos[info.pos] = info;
    }

    public void SetData(PosGemInfo infos)
    {
        for (int i = 1; i <= 5; i++)
        {
            for (int j = 0; j < infos.gemInfo.Count; j++)
            {
                GemInfo info = infos.gemInfo[j];
                if (info.pos == i && info.subType == subType)
                {
                    GemInfos.Add(i, info);
                }
            }

            if (!GemInfos.ContainsKey(i))
            {
                GemInfo gemInfo = new GemInfo();
                gemInfo.subType = subType;
                gemInfo.pos = i;
                gemInfo.gemId = 0;
                GemInfos.Add(i, gemInfo);
            }
        }
    }


    //返回该宝石在背包内的同类宝石列表
    public GemInfo GetGemBySubType(int pos)
    {
        for (int i = 0; i < GemInfos.Count; i++)
        {
            if (GemTableManager.Instance.GetGemPosition(GemInfos[i].gemId) == pos)
            {
                return GemInfos[i];
            }
        }
        
        return null;
    }

    public void Dispose()
    {
        
    }
}