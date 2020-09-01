using System.Collections.Generic;
using fashion;
using Google.Protobuf.Collections;
using Unity.Collections;

public class CSFashionInfo : CSInfo<CSFashionInfo>
{
    public CSFashionInfo()
    {
        Init();
    }

    public override void Dispose()
    {
        mPoolHandle?.OnDestroy();
        mPoolHandle = null;
    }

    PoolHandleManager mPoolHandle = new PoolHandleManager();

    void Init()
    {
        fashionWarehouses.Clear();
        dicFashionWarehouses.Clear();
        var arr = FashionTableManager.Instance.array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            TABLE.FASHION fashionItem = arr[i].Value as TABLE.FASHION;
            if (fashionItem.sex == 2 || fashionItem.sex == CSMainPlayerInfo.Instance.Sex) //性别限制
            {
                FashionItemData fashionItemData = mPoolHandle.GetCustomClass<FashionItemData>();
                fashionItemData.InitData(fashionItem.fashionid, UtilityMainMath.SplitStringToIntList(fashionItem.cost),
                    fashionItem.combineItem, fashionItem.combineNum, fashionItem.type, fashionItem.star,
                    fashionItem.sex);
                fashionWarehouses.Add(fashionItemData);
                dicFashionWarehouses.Add(fashionItemData.Id, fashionItemData);
            }
        }
    }

    /// <summary>
    /// 我的已拥有时装信息
    /// </summary>
    AllFashionInfo myAllFashionInfo = new AllFashionInfo();

    public AllFashionInfo MyAllFashionInfo => myAllFashionInfo;

    /// <summary>
    /// 时装仓库列表
    /// </summary>
    ILBetterList<FashionItemData> fashionWarehouses = new ILBetterList<FashionItemData>();
    
    /// <summary>
    /// 时装仓库字典
    /// </summary>
    Dictionary<int, FashionItemData> dicFashionWarehouses = new Dictionary<int, FashionItemData>();
    
    public Dictionary<int, FashionItemData> DicFashionWarehouses => dicFashionWarehouses;


    /// <summary>
    /// 排序后的时装(衣服和套装)信息(包括未获得的)
    /// </summary>
    ILBetterList<FashionItemData> sortedFashions = new ILBetterList<FashionItemData>();

    public ILBetterList<FashionItemData> SortedFashions => sortedFashions;

    /// <summary>
    /// 排序后的所有武器信息（包括未获得的）
    /// </summary>
    ILBetterList<FashionItemData> sortedWeapons = new ILBetterList<FashionItemData>();

    public ILBetterList<FashionItemData> SortedWeapons => sortedWeapons;

    /// <summary>
    /// 排序后的所有称号信息（包括未获得的）
    /// </summary>
    ILBetterList<FashionItemData> sortedTitles = new ILBetterList<FashionItemData>();

    public ILBetterList<FashionItemData> SortedTitles => sortedTitles;


    Map<TypeFashion, Map<TypeFashion, int>> map = new Map<TypeFashion, Map<TypeFashion, int>>();

    /// <summary>
    /// 玩家身上穿着的时装模型字典
    /// </summary>
    public Map<TypeFashion, Map<TypeFashion, int>> GetMapFashionModel()
    {
        map.Clear();
        if (myAllFashionInfo.fashionIds.Count > 0)
        {
            int id = 0;
            int type = 0;
            int configId = 0;
            FashionInfo fashionInfo;
            for (int i = 0; i < myAllFashionInfo.fashionIds.Count; i++)
            {
                configId = myAllFashionInfo.fashionIds[i];
                for (int j = 0; j < myAllFashionInfo.fashions.Count; j++)
                {
                    fashionInfo = myAllFashionInfo.fashions[j];
                    if (configId == fashionInfo.fashionId)
                    {
                        id = fashionInfo.fashionId + fashionInfo.star * 100;
                        type = FashionTableManager.Instance.GetFashionType(id);
                        Map<TypeFashion, int> temp = new Map<TypeFashion, int>();
                        int model = 0;
                        switch ((TypeFashion) type)
                        {
                            case TypeFashion.Clothes:
                                model = FashionTableManager.Instance.GetFashionClothesModel(id);
                                temp.Add(TypeFashion.Clothes, model);
                                map.Add(TypeFashion.Clothes, temp);
                                break;
                            case TypeFashion.Weapon:
                                model = FashionTableManager.Instance.GetFashionWeaponryModel(id);
                                temp.Add(TypeFashion.Weapon, model);
                                map.Add(TypeFashion.Weapon, temp);
                                break;
                            case TypeFashion.Title:
                                model = FashionTableManager.Instance.GetFashionTitleModel(id);
                                temp.Add(TypeFashion.Title, model);
                                map.Add(TypeFashion.Title, temp);
                                break;
                            case TypeFashion.FashionSet:
                                model = FashionTableManager.Instance.GetFashionClothesModel(id);
                                if (model > 0)
                                    temp.Add(TypeFashion.Clothes, model);

                                model = FashionTableManager.Instance.GetFashionWeaponryModel(id);
                                if (model > 0)
                                    temp.Add(TypeFashion.Weapon, model);

                                model = FashionTableManager.Instance.GetFashionTitleModel(id);
                                if (model > 0)
                                    temp.Add(TypeFashion.Title, model);

                                map.Add(TypeFashion.FashionSet, temp);
                                break;
                        }
                    }
                }
            }
        }

        return map;
    }


    /// <summary>
    /// 存放已激活的时装的fahionId列表
    /// </summary>
    ILBetterList<int> activeFashions = new ILBetterList<int>();

    /// <summary>
    /// 刷新各时装列表数据
    /// </summary>
    void RefreshFashionData()
    {
        sortedFashions.Clear();
        sortedWeapons.Clear();
        sortedTitles.Clear();
        activeFashions.Clear();
        for (int i = 0, max = myAllFashionInfo.fashions.Count; i < max; i++)
        {
            FashionInfo fashionInfo = myAllFashionInfo.fashions[i];
            activeFashions.Add(fashionInfo.fashionId);
        }

        for (int i = 0, max = fashionWarehouses.Count; i < max; i++)
        {
            FashionItemData fashionItem = fashionWarehouses[i];
            bool isActive = false;
            for (int j = 0, max1 = myAllFashionInfo.fashions.Count; j < max1; j++)
            {
                FashionInfo fashionInfo = myAllFashionInfo.fashions[j];
                int onlyId = fashionInfo.fashionId + fashionInfo.star * 100;
                bool isEquip = myAllFashionInfo.fashionIds.Contains(fashionInfo.fashionId);
                long timeLimit = fashionInfo.timeLimit;
                if (onlyId == fashionItem.Id)
                {
                    isActive = true;
                    fashionItem.RefreshData(isEquip, true, timeLimit);
                    switch ((TypeFashion) fashionItem.Type)
                    {
                        case TypeFashion.Clothes:
                        case TypeFashion.FashionSet:
                            sortedFashions.Add(fashionItem);
                            break;
                        case TypeFashion.Weapon:
                            sortedWeapons.Add(fashionItem);
                            break;
                        case TypeFashion.Title:
                            sortedTitles.Add(fashionItem);
                            break;
                    }
                }
            }

            if (!isActive)
            {
                fashionItem.RefreshData(); //所有未激活的刷新基础数据
                if (fashionItem.Star == 1 && !activeFashions.Contains(fashionItem.FashionId)) //未激活只加入1星基础等级
                {
                    switch ((TypeFashion) fashionItem.Type)
                    {
                        case TypeFashion.Clothes:
                        case TypeFashion.FashionSet:
                            sortedFashions.Add(fashionItem);
                            break;
                        case TypeFashion.Weapon:
                            sortedWeapons.Add(fashionItem);
                            break;
                        case TypeFashion.Title:
                            sortedTitles.Add(fashionItem);
                            break;
                    }
                }
            }
        }

        SortFashions();
        SortFashionForRedPoint();
    }


    /// <summary>
    /// 排序所有时装列表
    /// </summary>
    void SortFashions()
    {
        //临时存放已装备衣服或者套装
        FashionItemData equipItemClothesAndSets = null;
        //衣服和套装排序
        sortedFashions.Sort(SortClothesAndSets);
        /*把已装备的放最前面*/
        for (int i = 0, max = sortedFashions.Count; i < max; i++)
        {
            if (sortedFashions[i].IsEquip)
            {
                equipItemClothesAndSets = sortedFashions[i];
                sortedFashions.RemoveAt(i);
                break;
            }
        }

        if (equipItemClothesAndSets != null)
            sortedFashions.Insert(0, equipItemClothesAndSets);


        //临时存放已装备武器
        FashionItemData equipItemWeapons = null;
        //武器排序
        sortedWeapons.Sort(SortOtherFashons);
        //把已装备的放最前面
        for (int i = 0; i < sortedWeapons.Count; i++)
        {
            if (sortedWeapons[i].IsEquip)
            {
                equipItemWeapons = sortedWeapons[i];
                sortedWeapons.RemoveAt(i);
                break;
            }
        }

        if (equipItemWeapons != null)
            sortedWeapons.Insert(0, equipItemWeapons);


        //临时存放已装备称号
        FashionItemData equipItemTitles = null;
        //称号排序
        sortedTitles.Sort(SortOtherFashons);
        //把已装备的放最前面
        for (int i = 0; i < sortedTitles.Count; i++)
        {
            if (sortedTitles[i].IsEquip)
            {
                equipItemTitles = sortedTitles[i];
                sortedTitles.RemoveAt(i);
                break;
            }
        }

        if (equipItemTitles != null)
            sortedTitles.Insert(0, equipItemTitles);
    }

    /// <summary>
    /// 排序套装和衣服
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    int SortClothesAndSets(FashionItemData a, FashionItemData b)
    {
        if (a.Type != b.Type)
            return -(a.Type.CompareTo(b.Type));
        else
        {
            if (a.IsActive != b.IsActive)
                return a.IsActive ? -1 : 1;
            else
                return a.Id.CompareTo(b.Id);
        }
    }

    /// <summary>
    /// 排序武器和称号
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    int SortOtherFashons(FashionItemData a, FashionItemData b)
    {
        if (a.IsActive != b.IsActive)
            return a.IsActive ? -1 : 1;
        else
            return a.Id.CompareTo(b.Id);
    }


    /// <summary>
    /// 排序未激活时装中红点的优先
    /// </summary>
    public void SortFashionForRedPoint()
    {
        sortedFashions.Sort(SortFashionCanActive);
        sortedWeapons.Sort(SortFashionCanActive);
        sortedTitles.Sort(SortFashionCanActive);
    }

    /// <summary>
    /// 仅排序可激活的优先(红点优先)
    /// </summary>
    /// <returns></returns>
    int SortFashionCanActive(FashionItemData a, FashionItemData b)
    {
        if (!a.IsActive && !b.IsActive)
        {
            if ((a.IsCanBeActivated && b.IsCanBeActivated) || (!a.IsCanBeActivated && !b.IsCanBeActivated))
                return -1;
            else
                return a.IsCanBeActivated ? -1 : 1;
        }

        return -1;
    }


    /// <summary>
    /// 是否拥有可激活或者可升星的时装
    /// </summary>
    /// <returns></returns>
    public bool HasActiveAndUpStar()
    {
        //时装
        for (int i = 0; i < sortedFashions.Count; i++)
        {
            FashionItemData fashionItemData = sortedFashions[i];

            if (!fashionItemData.IsActive && fashionItemData.IsCanBeActivated || //未激活且可激活
                fashionItemData.IsActive && fashionItemData.IsUpStar) //已激活且可升星
                return true;
        }

        //武器
        for (int i = 0; i < sortedWeapons.Count; i++)
        {
            FashionItemData fashionItemData = sortedWeapons[i];

            if (!fashionItemData.IsActive && fashionItemData.IsCanBeActivated || //未激活且可激活
                fashionItemData.IsActive && fashionItemData.IsUpStar) //已激活且可升星
                return true;
        }

        //称号
        for (int i = 0; i < sortedTitles.Count; i++)
        {
            FashionItemData fashionItemData = sortedTitles[i];

            if (!fashionItemData.IsActive && fashionItemData.IsCanBeActivated || //未激活且可激活
                fashionItemData.IsActive && fashionItemData.IsUpStar) //已激活且可升星
                return true;
        }

        return false;
    }

    /// <summary>
    /// 是否有可激活
    /// </summary>
    /// <returns></returns>
    public bool HasActive()
    {
        // SortFashions();
        //时装
        for (int i = 0; i < sortedFashions.Count; i++)
        {
            FashionItemData fashionItemData = sortedFashions[i];
            if (!fashionItemData.IsActive && fashionItemData.IsCanBeActivated)
                return true;
        }

        //武器
        for (int i = 0; i < sortedWeapons.Count; i++)
        {
            FashionItemData fashionItemData = sortedWeapons[i];
            if (!fashionItemData.IsActive && fashionItemData.IsCanBeActivated)
                return true;
        }

        //称号
        for (int i = 0; i < sortedTitles.Count; i++)
        {
            FashionItemData fashionItemData = sortedTitles[i];
            if (!fashionItemData.IsActive && fashionItemData.IsCanBeActivated)
                return true;
        }

        return false;
    }

    /// <summary>
    /// 是否有可升星
    /// </summary>
    /// <returns></returns>
    public bool HasUpSatr()
    {
        //时装
        for (int i = 0; i < sortedFashions.Count; i++)
        {
            FashionItemData fashionItemData = sortedFashions[i];
            if (fashionItemData.IsActive && fashionItemData.IsUpStar)
                return true;
        }

        //武器
        for (int i = 0; i < sortedWeapons.Count; i++)
        {
            FashionItemData fashionItemData = sortedWeapons[i];
            if (fashionItemData.IsActive && fashionItemData.IsUpStar)
                return true;
        }

        //称号
        for (int i = 0; i < sortedTitles.Count; i++)
        {
            FashionItemData fashionItemData = sortedTitles[i];
            if (fashionItemData.IsActive && fashionItemData.IsUpStar)
                return true;
        }

        return false;
    }

    /// <summary>
    /// 时装是否有红点
    /// </summary>
    /// <returns></returns>
    public bool HasActiveAndUpStarForFashion()
    {
        for (int i = 0; i < sortedFashions.Count; i++)
        {
            FashionItemData fashionItemData = sortedFashions[i];

            if (!fashionItemData.IsActive && fashionItemData.IsCanBeActivated || //未激活且可激活
                fashionItemData.IsActive && fashionItemData.IsUpStar) //已激活且可升星
                return true;
        }

        return false;
    }

    /// <summary>
    /// 幻武是否有红点
    /// </summary>
    /// <returns></returns>
    public bool HasActiveAndUpStarForWeapon()
    {
        for (int i = 0; i < sortedWeapons.Count; i++)
        {
            FashionItemData fashionItemData = sortedWeapons[i];

            if (!fashionItemData.IsActive && fashionItemData.IsCanBeActivated || //未激活且可激活
                fashionItemData.IsActive && fashionItemData.IsUpStar) //已激活且可升星
                return true;
        }

        return false;
    }

    /// <summary>
    /// 称号是否有红点
    /// </summary>
    /// <returns></returns>
    public bool HasActiveAndUpStarForTitle()
    {
        for (int i = 0; i < sortedTitles.Count; i++)
        {
            FashionItemData fashionItemData = sortedTitles[i];

            if (!fashionItemData.IsActive && fashionItemData.IsCanBeActivated || //未激活且可激活
                fashionItemData.IsActive && fashionItemData.IsUpStar) //已激活且可升星
                return true;
        }

        return false;
    }

    #region 网络响应处理函数

    /// <summary>
    /// 获取时装信息
    /// </summary>
    /// <param name="msg"></param>
    public void GetAllFashionInfo(AllFashionInfo msg)
    {
        if (msg == null) return;
        myAllFashionInfo = msg;
        RefreshFashionData();
    }

    /// <summary>
    /// 处理时装穿戴
    /// </summary>
    /// <param name="msg"></param>
    public void HandleEquipFashion(FashionIdList msg)
    {
        if (msg == null) return;
        myAllFashionInfo.fashionIds.Clear();
        for (int i = 0; i < msg.ids.Count; i++)
        {
            myAllFashionInfo.fashionIds.Add(msg.ids[i]);
        }

        RefreshFashionData();
    }

    /// <summary>
    /// 处理升星
    /// </summary>
    /// <param name="msg"></param>
    public void HandleFashionStarLevelUp(FashionInfo msg)
    {
        if (msg == null) return;
        for (int i = 0; i < myAllFashionInfo.fashions.Count; i++)
        {
            if (myAllFashionInfo.fashions[i].fashionId == msg.fashionId)
            {
                myAllFashionInfo.fashions[i] = msg;
                break;
            }
        }

        RefreshFashionData();
    }

    /// <summary>
    /// 添加时装处理
    /// </summary>
    /// <param name="msg"></param>
    public void AddFashion(FashionInfo msg)
    {
        if (msg == null) return;
        myAllFashionInfo.fashions.Add(msg);
        RefreshFashionData();
        int id = msg.fashionId + 100 * msg.star;
        UtilityTips.ShowGreenTips(665, FashionTableManager.Instance.GetFashionName(id));
    }

    /// <summary>
    /// 删除时装处理
    /// </summary>
    /// <param name="msg"></param>
    public void RemoveFashion(FashionIdList msg)
    {
        if (msg == null) return;
        for (int i = 0; i < myAllFashionInfo.fashions.Count; i++)
        {
            for (int j = 0; j < msg.ids.Count; j++)
            {
                if (msg.ids[j] == myAllFashionInfo.fashions[i].fashionId)
                {
                    myAllFashionInfo.fashions.RemoveAt(i);
                }
            }
        }

        RefreshFashionData();
    }

    /// <summary>
    /// 卸下时装处理
    /// </summary>
    /// <param name="msg"></param>
    public void UnEquipFashion(FashionId msg)
    {
        if (msg == null) return;
        for (int i = 0; i < myAllFashionInfo.fashionIds.Count; i++)
        {
            if (myAllFashionInfo.fashionIds[i] == msg.id)
            {
                myAllFashionInfo.fashionIds.RemoveAt(i);
                break;
            }
        }

        RefreshFashionData();
    }

    #endregion
}


/// <summary>
/// 单个时装信息（包括所有的，用于展示）
/// </summary>
public class FashionItemData : IDispose
{
    public void Dispose()
    {
        listCost?.Clear();
        listCost = null;
    }

    public void InitData(int _fashionId, List<int> _listCost, int _combineEquipId, int _combineNum, int _type, int _star,
        int _sex)
    {
        fashionId = _fashionId;
        listCost = _listCost;
        combineEquipId = _combineEquipId;
        combineNum = _combineNum;
        type = _type;
        star = _star;
        sex = _sex;
        id = FashionId + 100 * Star;
        RefreshData();
    }

    public void RefreshData(bool _isEquip = false, bool _isActive = false, long _timeLimit = 0)
    {
        isEquip = _isEquip;
        isActive = _isActive;
        timeLimit = _timeLimit;
    }

    private int id;
    /// <summary>
    /// 时装唯一Id
    /// </summary>
    public int Id => id;


    private int fashionId;
    /// <summary>
    /// 时装Id
    /// </summary>
    public int FashionId => fashionId;


    private int type;
    /// <summary>
    /// 时装类型
    /// </summary>
    public int Type => type;


    private int star;
    /// <summary>
    /// 星级
    /// </summary>
    public int Star => star;


    private int sex;
    /// <summary>
    /// 性别限制(0女 1男 2通用)
    /// </summary>
    public int Sex => sex;

    
    private long timeLimit;
    /// <summary>
    /// 结束时间
    /// </summary>
    public long TimeLimit => timeLimit;


    private bool isEquip;
    /// <summary>
    /// 是否装备
    /// </summary>
    public bool IsEquip => isEquip;


    private bool isActive;
    /// <summary>
    /// 是否激活（获得）
    /// </summary>
    public bool IsActive => isActive;

    
    private List<int> listCost = new List<int>();
    /// <summary>
    /// 升星消耗
    /// </summary>
    public List<int> ListCost => listCost;

    /// <summary>
    /// 是否可升星
    /// </summary>
    public bool IsUpStar
    {
        get
        {
            if (ListCost == null) return false;
            bool isUpStar = false;
            if (Star < 14) //14星满星,未满星才考虑是否可升星
            {
                if (ListCost.Count != 2)
                {
                    // Debug.Log("--------------------------时装升星消耗配置格式错误@吕惠铭");
                }
                else
                {
                    long count = CSBagInfo.Instance.GetAllItemCount(ListCost[0]);
                    isUpStar = count >= ListCost[1];
                }
            }

            return isUpStar;
        }
    }

    private int combineEquipId;
    /// <summary>
    /// 合成碎片id
    /// </summary>
    public int CombineEquipId => combineEquipId;


    private int combineNum;
    /// <summary>
    /// 合成消耗碎片总数量
    /// </summary>
    public int CombineNum => combineNum;

    
    /// <summary>
    /// 已拥有碎片数量
    /// </summary>
    public int OwnedCombineNum => (int) CSItemCountManager.Instance.GetItemCount(CombineEquipId);

    
    /// <summary>
    /// 是否可激活
    /// </summary>
    public bool IsCanBeActivated => OwnedCombineNum >= CombineNum;
}