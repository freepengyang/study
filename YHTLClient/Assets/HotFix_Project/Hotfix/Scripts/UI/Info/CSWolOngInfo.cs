using System.Collections.Generic;
using wolong;

public class CSWoLongInfo : CSInfo<CSWoLongInfo>
{
    WoLongInfo mInfo;
    //WoLongPetInfo petActiveInfo;
    //WoLongPetInfo petCancelInfo;
    //WoLongPetState petState;

    int petAwakeEquipId = 0;        //装备配置表id
    long petAwakeOnlyEquipId = 0;   //装备唯一id,同种木剑也不同id，给服务端用
    int petAwakeTableId = 1;
    bool isActivite = false;        //战魂觉醒是否激活
    bool isSucess = false;          //战魂觉醒是否成功
    CSBetterLisHot<PetAwakeData> petAwakeList = new CSBetterLisHot<PetAwakeData>();
    //CSBetterLisHot<PetSkillData> petSkillList = new CSBetterLisHot<PetSkillData>();
    PoolHandleManager mPoolHandle = new PoolHandleManager();
    public void GetWoLongInfo(WoLongInfo _mInfo)
    {
        //Debug.Log("收到卧龙信息");
        mInfo = _mInfo;
    }
    public WoLongInfo ReturnWoLongInfo()
    {
        return mInfo;
    }
    public int GetWoLongLevel()
    {
        if (mInfo != null)
        {
            return mInfo.wolongLevel;
        }
        return 0;
    }
    public bool GetWolongUpGradeRedPointState()
    {
        //功能未开放时    返回false
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcp_wolong))
        {
            return false;
        }
        //已达到最大等级  返回false
        if (GetWoLongLevel() == WoLongLevelTableManager.Instance.GetMaxId())
        {
            return false;
        }
        long mNum = CSItemCountManager.Instance.GetItemCount((int)MoneyType.wolongxiuwei);
        int cfgNum = WoLongLevelTableManager.Instance.GetCostByWoLongLevel(GetWoLongLevel() + 1);
        //Debug.Log($" {GetWoLongLevel()}    {mNum}   {cfgNum } ");
        return (mNum >= cfgNum) ? true : false;
    }
    List<ActiveSkillGroupInfo> activeSkill = new List<ActiveSkillGroupInfo>();
    public void GetWoLongEnabledSkill(SkillGroupInfoResponse _res)
    {
        //UnityEngine.Debug.Log("卧龙装备技能激活  " + _res.activeSkillGroupInfos.Count);
        int count = activeSkill.Count;
        activeSkill.Clear();
        for (int i = 0; i < _res.activeSkillGroupInfos.Count; i++)
        {
            if (_res.activeSkillGroupInfos[i].level > 0)
            {
                if (!activeSkill.Contains(_res.activeSkillGroupInfos[i]))
                {
                    activeSkill.Add(_res.activeSkillGroupInfos[i]);

                }
            }
        }
        if (_res.login != 1)
        {
            if (activeSkill.Count > count && activeSkill.Count != 0)
            {
                UIManager.Instance.CreatePanel<UIWoLongPromptPanel>(p =>
                {
                    (p as UIWoLongPromptPanel).SetData(activeSkill);
                });
            }
        }
    }
    public int WoLongEnabledSkillLevel(int _groupId)
    {
        for (int i = 0; i < activeSkill.Count; i++)
        {
            ActiveSkillGroupInfo info = activeSkill[i];
            //UnityEngine.Debug.Log("激活的卧龙技能  " + info.skillGroup + "   " + info.level);
            if (_groupId == info.skillGroup)
            {
                return info.level;
            }
        }
        return 1;
    }




    /*-------------------------战宠激活、出战、合体、死亡用----------------------*/
    //public void GetPetActiveInfo(WoLongPetInfo _petActiveInfo)
    //{
    //    petActiveInfo = _petActiveInfo;
    //    petCancelInfo = null;
    //    GetZhanChongJueXingTableId();//检测，是否战宠觉醒激活
    //}
    //public WoLongPetInfo ReturnPetActiveInfo()
    //{
    //    return petActiveInfo;
    //}
    //public void GetPetCancelInfo(WoLongPetInfo _petCancelInfo)
    //{
    //    petCancelInfo = _petCancelInfo;
    //    petActiveInfo = null;
    //    petState = null;
    //    isActivite = false;     //战宠觉醒未激活
    //}
    //public WoLongPetInfo ReturnPetCancelInfo()
    //{
    //    return petCancelInfo;
    //}
    //public void GetPetState(WoLongPetState _petStateMsg)
    //{
    //    petState = _petStateMsg;
    //}
    //public WoLongPetState ReturnPetState()
    //{
    //    return petState;
    //}
    private CSBetterLisHot<PetAwakeData> ReturnPetAwakeDataList()
    {
        return petAwakeList;
    }
    public void SetPetAwakeEquipId(int _id)
    {
        petAwakeEquipId = _id;
    }
    public int ReturnPetAwakeEquipId()
    {
        return petAwakeEquipId;
    }
    public void SetPetAwakeOnlyEquipId(long _id)
    {
        petAwakeOnlyEquipId = _id;
    }
    public long ReturnPetAwakeOnlyEquipId()
    {
        return petAwakeOnlyEquipId;
    }
    public int ReturnPetId()
    {
        int petId = 0;
        ////角色登录获得是否激活战宠状态
        //int wolongPetid = CSMainPlayerInfo.Instance.WolongPetId;
        ////激活战宠后获取战魂suitID				
        //if (ReturnPetCancelInfo() == null)
        //{
        //    if (ReturnPetActiveInfo() != null)
        //    {
        //        petId = ReturnPetActiveInfo().id;
        //    }
        //    else
        //    {
        //        if (wolongPetid > 0)
        //        {
        //            petId = wolongPetid;
        //        }
        //    }
        //}
        return petId;
    }

    //private void SetSkillListInfo(List<PetSkillData> tempPetSkillList, bool isGet)
    //{
    //    for (int i = 0; i < tempPetSkillList.Count; i++)
    //    {
    //        if (tempPetSkillList[i].isGet == isGet)
    //        {
    //            petSkillList.Add(tempPetSkillList[i]);
    //        }
    //    }
    //}
    //public CSBetterLisHot<PetSkillData> ReturnPetSkillInfo()
    //{
    //    return petSkillList;
    //}
    /*-------------------------战宠觉醒信息处理----------------------*/
    public void GetPetAwakeDataList(SoldierSoulInfoResponse msg)
    {
        petAwakeList.Clear();
        for (int i = 0; i < msg.soldierSoulInfos.Count; i++)
        {
            PetAwakeData petAwakeData = mPoolHandle.GetCustomClass<PetAwakeData>();
            petAwakeData.zhanhunId = msg.soldierSoulInfos[i].id;
            petAwakeData.stage = msg.soldierSoulInfos[i].stage;
            petAwakeList.Add(petAwakeData);

        }
        GetZhanChongJueXingTableId();
    }
    public void GetPetAwakeDataListNew(SoldierSoulInfo msg)
    {
        GetIsSucess(msg);
        petAwakeList.Clear();
        petAwakeEquipId = 0;
        petAwakeOnlyEquipId = 0;
        PetAwakeData petAwakeData = mPoolHandle.GetCustomClass<PetAwakeData>();
        petAwakeData.zhanhunId = msg.id;
        petAwakeData.stage = msg.stage;
        petAwakeList.Add(petAwakeData);
        GetZhanChongJueXingTableId();
    }
    private void GetIsSucess(SoldierSoulInfo msg)
    {
        isSucess = msg.stage > ZhanChongJueXingTableManager.Instance.GetZhanChongJueXingStage(petAwakeTableId);
    }
    public bool ReturnIsSucess()
    {
        return isSucess;
    }
    private void GetZhanChongJueXingTableId()
    {
        int id = 1;
        int zhanhunSuitId = 0;
        int stage = 0;
        int petId = ReturnPetId();
        petAwakeList = ReturnPetAwakeDataList();
        //激活战宠后获取战魂suitID														
        if (petId > 0)
        {
            zhanhunSuitId = GetZhanHunSuitId(petId);
        }
        if (zhanhunSuitId > 0)
        {
            for (int i = 0; i < petAwakeList.Count; i++)
            {
                if (petAwakeList[i].zhanhunId == zhanhunSuitId)
                {
                    stage = petAwakeList[i].stage;
                }
            }
            TABLE.ZHANCHONGJUEXING juexingItem = null;
            var arr = ZhanChongJueXingTableManager.Instance.array.gItem.handles;
            for (int k = 0, max = arr.Length; k < max; ++k)
            {
                juexingItem = arr[k].Value as TABLE.ZHANCHONGJUEXING;
                if (juexingItem.zhanhun == zhanhunSuitId && juexingItem.stage == stage)
                {
                    id = juexingItem.id;
                    isActivite = true;
                }
            }
        }
        petAwakeTableId = id;
    }
    //根据战宠id，获得战宠套装id == 战魂suitID
    private int GetZhanHunSuitId(int petId)
    {
        int zhanhunSuitId = 0;
        TABLE.ZHANHUNSUIT zhanhunsuitItem = null;
        var arr = ZhanHunSuitTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            zhanhunsuitItem = arr[k].Value as TABLE.ZHANHUNSUIT;
            if (zhanhunsuitItem.suitSummoned == petId)
            {
                zhanhunSuitId = zhanhunsuitItem.id;
            }
        }
        return zhanhunSuitId;
    }
    public int ReturnZhanHunSuitId()
    {
        int zhanhunSuitId = 0;
        int petId = ReturnPetId();
        if (petId > 0)
        {
            zhanhunSuitId = GetZhanHunSuitId(petId);
        }
        return zhanhunSuitId;
    }
    //获取1武器model,2衣服model
    public string ReturnZhanHunModel(int idx)
    {
        string model = "";
        int zhanhunSuitId = ReturnZhanHunSuitId();
        //if (idx == 1)
        //    model = ZhanHunSuitTableManager.Instance.GetZhanHunSuitWeaponmodel(zhanhunSuitId);
        //else if (idx == 2)
        //    model = ZhanHunSuitTableManager.Instance.GetZhanHunSuitClothesmodel(zhanhunSuitId);
        return model;
    }
    //根据zhanhunSuitId，获取1武器model,2衣服model
    public string ReturnZhanHunModel(int idx, int zhanhunSuitId)
    {
        string model = "";
        //if (idx == 1)
        //    model = ZhanHunSuitTableManager.Instance.GetZhanHunSuitWeaponmodel(zhanhunSuitId);
        //else if (idx == 2)
        //    model = ZhanHunSuitTableManager.Instance.GetZhanHunSuitClothesmodel(zhanhunSuitId);
        return model;
    }
    public int ReturnZhanHunBodyEffect()
    {
        int zhanhunSuitId = ReturnZhanHunSuitId();
        zhanhunSuitId = zhanhunSuitId < 1 ? 1 : zhanhunSuitId;
        return ZhanChongJueXingTableManager.Instance.GetZhanChongJueXingBodyeffect(zhanhunSuitId);
    }

    public bool isPetAwakeListPanel()
    {
        Dictionary<int, bag.BagItemInfo> bagItemList = new Dictionary<int, bag.BagItemInfo>();
        CSBagInfo.Instance.GetBagWoLongEquipData(bagItemList);
        if (bagItemList.Count > 0)
        {
            int id = ReturnPetAwakeTableId();
            int itemId, itemIdNew;
            string[] consume = UtilityMainMath.StrToStrArr(ZhanChongJueXingTableManager.Instance.GetZhanChongJueXingConsume(id));
            if (consume == null)
            {
                return false;
            }
            if (bagItemList.Count > 0)
            {
                var iter = bagItemList.GetEnumerator();
                while (iter.MoveNext())
                {
                    itemId = iter.Current.Value.configId;
                    for (int i = 0; i < consume.Length; i++)
                    {
                        int.TryParse(consume[i], out itemIdNew);
                        if (itemIdNew == itemId)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    public int ReturnPetAwakeTableId()
    {
        return petAwakeTableId;
    }
    public bool ReturnPetAwakeIsActivite()
    {
        return isActivite;
    }
    //检测战宠觉醒小红点用
    public bool isShowRedPointPetAwake()
    {
        int id = petAwakeTableId;
        int stage = ZhanChongJueXingTableManager.Instance.GetZhanChongJueXingStage(id);
        int nextStage = ZhanChongJueXingTableManager.Instance.GetZhanChongJueXingStage(id + 1);
        if (isPetAwakeListPanel())
        {
            return nextStage > stage && isActivite;
        }
        return false;
    }
    public override void Dispose()
    {
        mPoolHandle?.OnDestroy();
        mPoolHandle = null;

        petAwakeList.Clear();
        //petSkillList.Clear();

        //petActiveInfo = null;
        //petCancelInfo = null;
        //petState = null;
        petAwakeList = null;

        petAwakeEquipId = 0;
        petAwakeOnlyEquipId = 0;
        petAwakeTableId = 1;
        isActivite = false;
        isSucess = false;
    }
}

//战魂觉醒数据
public class PetAwakeData : IDispose
{
    public PetAwakeData() { }
    public PetAwakeData(int _zhanhunId, int _stage)
    {
        zhanhunId = _zhanhunId;
        stage = _stage;
    }
    public int zhanhunId = 0;
    public int stage = 0;
    public void Dispose() { }
}