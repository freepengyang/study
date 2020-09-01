using System;
using System.Collections.Generic;
using baozhu;
using UnityEngine;
using Google.Protobuf.Collections;

//常用工具类
public partial class Utility
{
    private static List<CareerData> mCareerDataList = null;

    public static int GetBodyModel(int bodyModelID, int fashionId, int sex = 0, int career = 0)
    {
        if (IsInMap(ESpecialMap.DiXiaXunBao))
        {
            if (sex == ESex.Man)
            {
                return 210211160;
            }

            return 210201160;
        }

        if (CSConfigInfo.Instance.GetBool(ConfigOption.PopGraphicsModeTips))
        {
            EShowOptionType showOptionType = (EShowOptionType) CSConfigInfo.Instance.GetGraphicsMode();
            switch (showOptionType)
            {
                case EShowOptionType.TopSpeed:
                case EShowOptionType.Fluency:
                {
                    int topSpeedModel = GetTopSpeedBodyModel(sex, career);
                    if (topSpeedModel > 0)
                    {
                        return topSpeedModel;
                    }
                }
                    break;
            }
        }

        if (fashionId > 0)
        {
            TABLE.FASHION tblFashion = null;
            if (FashionTableManager.Instance.TryGetValue(fashionId, out tblFashion) && tblFashion.clothesModel != 0)
            {
                return tblFashion.clothesModel;
            }
        }

        if (bodyModelID > 0 && bodyModelID != 625000 && bodyModelID != 615000)
        {
            TABLE.ITEM tblItem = null;
            if (ItemTableManager.Instance.TryGetValue(bodyModelID, out tblItem))
            {
                return tblItem.model;
            }
        }

        return bodyModelID;
    }

    public static int GetWeaponModel(int weaponId, int fashionId, int sex = 0, int career = 0,
        int avatarType = EAvatarType.Player)
    {
        if (IsInMap(ESpecialMap.DiXiaXunBao))
        {
            return 210121150;
        }

        if (CSConfigInfo.Instance.GetBool(ConfigOption.PopGraphicsModeTips))
        {
            EShowOptionType showOptionType = (EShowOptionType) CSConfigInfo.Instance.GetGraphicsMode();
            if ((showOptionType == EShowOptionType.TopSpeed) && (weaponId != 0 || fashionId != 0))
            {
                int topSpeedModel = GetTopSpeedWeaponModel(sex, career);
                if (topSpeedModel > 0)
                {
                    return topSpeedModel;
                }
            }
        }

        if (fashionId > 0)
        {
            TABLE.FASHION tblFashion = null;
            if (FashionTableManager.Instance.TryGetValue(fashionId, out tblFashion) && tblFashion.weaponryModel != 0)
            {
                return tblFashion.weaponryModel;
            }
        }

        TABLE.ITEM tblItem = null;
        if (ItemTableManager.Instance.TryGetValue(weaponId, out tblItem))
        {
            return tblItem.model;
        }

        return 0;
    }

    public static int GetWingModelId(long id)
    {
        if (IsInMap(ESpecialMap.DiXiaXunBao))
        {
            return 0;
        }

        int huancaiId = (int) (id >> 32);
        int wingId = (int) (id & 0xffffffff);
        TABLE.HUANCAI tblHuanCai = null;
        if (HuanCaiTableManager.Instance.TryGetValue(huancaiId, out tblHuanCai))
        {
            return tblHuanCai.model;
        }

        TABLE.WING tblWing = null;
        if (WingTableManager.Instance.TryGetValue(wingId, out tblWing))
        {
            return tblWing.model;
        }

        return 0;
    }

    public static TABLE.ZHANHUNSUIT GetZhanHunSuit(int monsterId)
    {
        TABLE.ZHANHUNSUIT tblZhanHunSuit = null;
        var arr = ZhanHunSuitTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            tblZhanHunSuit = arr[k].Value as TABLE.ZHANHUNSUIT;
            if (tblZhanHunSuit.suitSummoned == monsterId)
            {
                return tblZhanHunSuit;
            }
        }

        return null;
    }

    public static int GetTopSpeedBodyModel(int sex, int career)
    {
        List<CareerData> list = GetDefaultCarrerData();
        for (int i = 0; i < list.Count; ++i)
        {
            CareerData careerData = list[i];
            if (careerData.sex == sex && careerData.career == career)
            {
                return careerData.model;
            }
        }

        return 0;
    }

    public static int GetTopSpeedWeaponModel(int sex, int career)
    {
        List<CareerData> list = GetDefaultCarrerData();
        for (int i = 0; i < list.Count; ++i)
        {
            CareerData careerData = list[i];
            if (careerData.sex == sex && careerData.career == career)
            {
                return careerData.weapon;
            }
        }

        return 0;
    }

    public static List<CareerData> GetDefaultCarrerData()
    {
        if (mCareerDataList == null)
        {
            mCareerDataList = new List<CareerData>();
            TABLE.SUNDRY tblSundry;
            if (SundryTableManager.Instance.TryGetValue(707, out tblSundry))
            {
                List<List<int>> list = UtilityMainMath.SplitStringToIntLists(tblSundry.effect);
                for (int i = 0; i < list.Count; ++i)
                {
                    List<int> tempList = list[i];
                    if (tempList.Count < 4)
                    {
                        continue;
                    }

                    for (int s_i = 0; s_i < tempList.Count; ++s_i)
                    {
                        CareerData carrerData = new CareerData(tempList[0], tempList[1], tempList[2], tempList[3]);
                        mCareerDataList.Add(carrerData);
                    }
                }
            }
        }

        return mCareerDataList;
    }

    static string warrior;
    static string mage;
    static string taoist;
    static string normal;

    public static string GetJob(int _job)
    {
        if (_job == 0)
        {
            return normal ?? (normal = ClientTipsTableManager.Instance.GetClientTipsContext(210));
        }
        else if (_job == 1)
        {
            return warrior ?? (warrior = ClientTipsTableManager.Instance.GetClientTipsContext(207));
        }
        else if (_job == 2)
        {
            return mage ?? (mage = ClientTipsTableManager.Instance.GetClientTipsContext(208));
        }
        else
        {
            return taoist ?? (taoist = ClientTipsTableManager.Instance.GetClientTipsContext(209));
        }
    }

    static string man;
    static string woman;

    public static string GetSex(int _sex)
    {
        return (_sex == 1)
            ? (man ?? (man = ClientTipsTableManager.Instance.GetClientTipsContext(211)))
            : (woman ?? (woman = ClientTipsTableManager.Instance.GetClientTipsContext(212)));
    }

    public static bool IsCrossServerMap(int mapId)
    {
        return false;
    }

    public static bool HasTeam()
    {
        return CSMainPlayerInfo.Instance.TeamId != 0;
    }

    public static bool HasGuild()
    {
        return CSMainPlayerInfo.Instance.GuildId != 0;
    }

    public static int GetGuildLevel()
    {
        if (!HasGuild())
            return 0;
        return 5;
    }

    public static string GetCareerName(int career, bool isShortNaming = false)
    {
        switch (career)
        {
            case ECareer.Warrior:
                return isShortNaming ? CSString.Format(1174) /*战*/ : CSString.Format(207); //"战士";
            case ECareer.Master:
                return isShortNaming ? CSString.Format(1175) /*法*/ : CSString.Format(208); //"法师";
            case ECareer.Taoist:
                return isShortNaming ? CSString.Format(1176) /*道*/ : CSString.Format(209); //"道士";
        }

        return isShortNaming ? CSString.Format(1174) /*战*/ : CSString.Format(207); //"战士";
    }

    protected static byte[] msgBuffer = new byte[4096];

    //可以将数据转换为string传递服务器，对应得到数据 在 OpenURLOnClick
    public static string GetStructString(System.Object msg)
    {
        if (msg is Google.Protobuf.IMessage imessage)
        {
            int size = imessage.CalculateSize();
            if (size <= msgBuffer.Length)
            {
                using (Google.Protobuf.CodedOutputStream stream = new Google.Protobuf.CodedOutputStream(msgBuffer))
                {
                    imessage.WriteTo(stream);
                    return Convert.ToBase64String(msgBuffer, 0, size);
                }
            }
        }

        return string.Empty;
    }

    public static bool IsInPrison()
    {
        //TODO:
        fight.BufferInfo redNameBuff = null; // CSMainPlayerInfo.Instance.BuffInfo.GetBuff(940400);

        return redNameBuff != null && redNameBuff.bufferValue == 1;
    }

    /// <summary>
    /// 判断当前点距离玩家是否在一定范围内  同地图判断
    /// </summary>
    /// <returns></returns>
    public static bool IsNearPlayerInMap(int goalX, int goalY, int distance = 5)
    {
        if (!CSScene.IsLanuchMainPlayer) return false;
        if (UtilityMath.Compare(goalX,CSAvatarManager.MainPlayer.NewCell.mCell_x, distance) &&
            UtilityMath.Compare(goalY,CSAvatarManager.MainPlayer.NewCell.mCell_y, distance))
            return true;
        return false;
    }

    /// <summary>
    /// 判断当前点距离玩家是否在一定范围内  加上地图判断
    /// </summary>
    /// <returns></returns>
    public static bool IsNearPlayerInMap(int mapId, int goalX, int goalY, int distance = 5)
    {
        if (!CSScene.IsLanuchMainPlayer) return false;
        if (CSScene.GetMapID() != mapId) return false;
        if (UtilityMath.Compare(goalX,CSAvatarManager.MainPlayer.NewCell.mCell_x, distance) &&
            UtilityMath.Compare(goalY,CSAvatarManager.MainPlayer.NewCell.mCell_y, distance))
            return true;
        return false;
    }

    //获得一个点到玩家的距离
    public static int DistanceFromPlayer(int x, int y)
    {
        return Mathf.Abs(CSAvatarManager.MainPlayer.NewCell.Coord.x - x) + Mathf.Abs(CSAvatarManager.MainPlayer.NewCell.Coord.y - y);
    }

    /// <summary>
    /// 能否传送
    /// </summary>
    /// <returns></returns>
    public static bool IsCanTransfer()
    {
        if (CSResourceManager.Instance.IsChangingScene) return false;


        return true;
    }

    /// <summary>
    /// 设置玩家头像
    /// </summary>
    /// <param name="sex"></param>
    /// <param name="career"></param>
    /// <returns></returns>
    public static string GetPlayerIcon(int sex, int career)
    {
        string spriteName = "";
        switch (sex)
        {
            case 0: //女
                switch (career)
                {
                    case 1:
                        spriteName = "10";
                        break;
                    case 2:
                        spriteName = "20";
                        break;
                    case 3:
                        spriteName = "30";
                        break;
                    default:
                        break;
                }

                break;
            case 1: //男
                switch (career)
                {
                    case 1:
                        spriteName = "11";
                        break;
                    case 2:
                        spriteName = "21";
                        break;
                    case 3:
                        spriteName = "31";
                        break;
                    default:
                        break;
                }

                break;
            case 2: //通用
                break;
            default:
                break;
        }

        return spriteName;
    }

    public static bool isDoMissionOrAutoFighting
    {
        get
        {
            if (CSPathFinderManager.IsAutoFinPath || CSAutoFightManager.Instance.IsAutoFight) return true;
            else return false;
        }
    }

    /// <summary>
    /// 仅热更工程调用
    /// </summary>
    /// <param name="poolNameShow"></param>
    /// <param name="poolName"></param>
    /// <param name="go"></param>
    /// <param name="type"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static CSObjectPoolItem GetAndAddPoolItem_Class(string poolNameShow, string poolName, GameObject go,
        Type type, params object[] args)
    {
        CSObjectPoolItem poolItem =
            CSObjectPoolMgr.Instance.GetAndAddPoolItem_Class_Hotfix(poolNameShow, poolName, go, type, args);
        if (poolItem.objParam == null && type != null)
        {
            poolItem.objParam = Activator.CreateInstance(type);
        }

        return poolItem;
    }


    /// <summary>
    /// 给定itemId打开获取途径面板
    /// </summary>
    /// <param name="itemId">物品Id</param>
    public static void ShowGetWay(int itemId)
    {
        bool IsNewGet = false;
        int wayId = 0;
        string tempStr = ItemTableManager.Instance.GetItemGetWay(itemId);
        List<int> tempList = UtilityMainMath.SplitStringToIntList(tempStr);
        for (int i = 0; i < tempList.Count; i++)
        {
            int type = GetWayTableManager.Instance.GetGetWayType(tempList[i]);
            if (type == 3)
            {
                wayId = tempList[i];
                IsNewGet = true;
                break;
            }
        }

        if (!IsNewGet)
            UIManager.Instance.CreatePanel<UIFastAccessPanel>((f) => { (f as UIFastAccessPanel).RefreshUI(itemId); });
        else
            UIManager.Instance.CreatePanel<UIFastAccessTwoPanel>((f) =>
            {
                (f as UIFastAccessTwoPanel).RefreshUI(wayId);
            });
    }

    /// <summary>
    /// 给定GetWay的Id字符串打开获取途径面板
    /// </summary>
    /// <param name="getWayStr">GetWay的Id字符串，以#连接</param>
    public static void ShowGetWay(string getWayStr)
    {
        UIManager.Instance.CreatePanel<UIFastAccessPanel>((f) => { (f as UIFastAccessPanel).RefreshUI(getWayStr); });
    }

    /// <summary>
    /// 给定GetWay的列表打开获取途径面板
    /// </summary>
    /// <param name="listGetWay">GetWay列表</param>
    public static void ShowGetWay(List<int> listGetWay)
    {
        UIManager.Instance.CreatePanel<UIFastAccessPanel>((f) => { (f as UIFastAccessPanel).RefreshUI(listGetWay); });
    }
	/// <summary>
	/// 卧龙等级不足时候，打开获取途径面板
	/// </summary>
	/// <param name="playLv"></param>
	/// <param name="needLv"></param>
	public static void ShowLimitWoLongLvWay(int playLv, int needLv)
	{
		UIManager.Instance.CreatePanel<UIFastAccessWoLongPanel>((f) =>
		{
			(f as UIFastAccessWoLongPanel).RefreshUI(
				playLv, needLv, (int)UIFastAccessWoLongPanel.ShowState.Wolong);
		});
	}
	/// <summary>
	/// 等级不足时候，打开获取途径面板
	/// </summary>
	/// <param name="playLv"></param>
	/// <param name="needLv"></param>
	public static void ShowLimitLvWay(int playLv, int needLv)
	{
		UIManager.Instance.CreatePanel<UIFastAccessWoLongPanel>((f) => {
			(f as UIFastAccessWoLongPanel).RefreshUI(
				playLv, needLv, (int)UIFastAccessWoLongPanel.ShowState.Exp);
		});
	}

    /// <summary>
    /// 倒计时离开场景页面
    /// </summary>
    /// <param name="second">剩余时间（秒）</param>
    /// <param name="endAction">结束后回调</param>
    /// <param name="cycleAction">每秒回调</param>
    public static void ShowCountDownLeavePanel(int second, System.Action endAction = null,
        System.Action cycleAction = null)
    {
        if (second <= 0) return;
        UIManager.Instance.CreatePanel<UICountDownLeavePanel>((f) =>
        {
            (f as UICountDownLeavePanel).InitData(second, endAction, cycleAction);
        });
    }


    /// <summary>
    /// 打开显示总属性加成面板
    /// </summary>
    /// <param name="titleId">clienttips表Id，用于填写标题</param>
    /// <param name="listAllAttribute">属性信息总和列表</param>
    public static void ShowAllAttributePanel(int titleId, List<List<int>> listAllAttribute)
    {
        if (listAllAttribute == null) return;
        UIManager.Instance.CreatePanel<UIAllAttributeTipsPanel>((f) =>
        {
            (f as UIAllAttributeTipsPanel).ShowAllAttributeData(titleId, listAllAttribute);
        });
    }


    /// <summary>
    /// 打开显示总属性加成面板
    /// </summary>
    /// <param name="titleId">clienttips表Id，用于填写标题</param>
    /// <param name="listReAllAttribute">属性信息总和列表</param>
    public static void ShowAllAttributePanel(int titleId, List<RepeatedField<int>> listReAllAttribute)
    {
        if (listReAllAttribute == null) return;
        List<List<int>> listInfo = new List<List<int>>();
        List<int> listInt = new List<int>();
        listInfo.Clear();
        for (int i = 0; i < listReAllAttribute.Count; i++)
        {
            listInt.Clear();
            for (int j = 0; j < listReAllAttribute[i].Count; j++)
            {
                listInt.Add(listReAllAttribute[i][j]);
            }

            listInfo.Add(listInt);
        }

        UIManager.Instance.CreatePanel<UIAllAttributeTipsPanel>((f) =>
        {
            (f as UIAllAttributeTipsPanel).ShowAllAttributeData(titleId, listInfo);
        });
    }


    /// <summary>
    /// 打开显示总属性加成面板
    /// </summary>    
    /// <param name="titleId">clienttips表Id，用于填写标题</param>
    /// <param name="mapAllAttribute">属性信息总和列表</param>
    public static void ShowAllAttributePanel(int titleId, Map<int, int> mapAllAttribute)
    {
        if (mapAllAttribute == null) return;
        List<List<int>> listInfo = new List<List<int>>();
        listInfo.Clear();
        for (mapAllAttribute.Begin(); mapAllAttribute.Next();)
        {
            List<int> listInt = new List<int>();
            listInt.Add(mapAllAttribute.Key);
            listInt.Add(mapAllAttribute.Value);
            listInfo.Add(listInt);
        }

        UIManager.Instance.CreatePanel<UIAllAttributeTipsPanel>((f) =>
        {
            (f as UIAllAttributeTipsPanel).ShowAllAttributeData(titleId, listInfo);
        });
    }

    //设置item列表
    public static void SetRewardItems(List<StringData> listData, Transform trans)
    {
        //if (ActivityCfg == null) return;
        //string[] itemsStr = items.Split('&');
        List<UIItemBase> rewardItemList = UIItemManager.Instance.GetUIItems(listData.Count, PropItemType.Normal, trans);
        if (rewardItemList != null)
        {
            for (int i = 0; i < listData.Count; i++)
            {
                rewardItemList[i].Refresh(listData[i].id, ItemClick);
                rewardItemList[i].SetCount(listData[i].count, CSColor.white);
            }
        }
    }


    public static void ItemClick(UIItemBase item)
    {
        if (item != null)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, item.itemCfg, item.infos);
        }
    }

    public static void ShowRewardPanel(List<StringData> listData, System.Action act,
        RewardPromptType type = RewardPromptType.BackBtn)
    {
        UIManager.Instance.CreatePanel<UIRewardPromptPanel>((t) =>
        {
            UIRewardPromptPanel panel = t as UIRewardPromptPanel;
            if (panel != null)
            {
                panel.ShowUI(listData, act, type);
            }
        });
    }

    public static void ShowRewardPanel(int _instanceId, System.Action act,
        RewardPromptType type = RewardPromptType.BackBtn)
    {
        UIManager.Instance.CreatePanel<UIRewardPromptPanel>((t) =>
        {
            UIRewardPromptPanel panel = t as UIRewardPromptPanel;
            if (panel != null)
            {
                panel.ShowUI(_instanceId, act, type);
            }
        });
    }

    public static void OpenSkillTipPanel(int skillid)
    {
        UIManager.Instance.CreatePanel<UIDungeonSkillTipsPanel>((f) =>
        {
            (f as UIDungeonSkillTipsPanel).OpenPanel(skillid);
        });
    }

    //static List<UIItemBase> itemList = new List<UIItemBase>();

    /// <summary>
    /// 再指定节点上设置item,如果首次已经设置那么会刷新item表
    /// </summary>
    /// <param name="boxid"> box id</param>
    /// <param name="mGrid"> 节点</param>
    /// <param name="uiItemlists"> 如果不是首次，传入此参数</param>
    /// <param name="size"> item的尺寸</param>
    /// <returns></returns>
    public static List<UIItemBase> GetItemByBoxid(int boxid, UIGrid mGrid, List<UIItemBase> uiItemlists = null,
        itemSize size = itemSize.Default, bool isEffect = false)
    {
        if (mGrid == null)
        {
            FNDebug.LogError("mGrid is null");
            return null;
        }

        Dictionary<int, int> BoxReward = new Dictionary<int, int>();

        List<UIItemBase> uiItemBaseList = uiItemlists;

        BoxTableManager.Instance.GetBoxAwardById(boxid, BoxReward);
        if (BoxReward.Count > 0)
        {
            if (uiItemBaseList == null || uiItemBaseList.Count < BoxReward.Count)
            {
                UIItemManager.Instance.RecycleItemsFormMediator(uiItemBaseList);
                uiItemBaseList?.Clear();
                uiItemBaseList =
                    UIItemManager.Instance.GetUIItems(BoxReward.Count, PropItemType.Normal, mGrid.transform, size);
            }

            //把超过的item隐藏
            for (int j = 0; j < mGrid.transform.childCount; j++)
            {
                if (j >= BoxReward.Count)
                    mGrid.transform.GetChild(j).gameObject.SetActive(false);
            }

            //刷新item数据
            int i = 0;
            for (var it = BoxReward.GetEnumerator(); it.MoveNext();)
            {
                mGrid.transform.GetChild(i).gameObject.SetActive(true);
                if (ItemTableManager.Instance.TryGetValue(it.Current.Key, out TABLE.ITEM boxItem))
                {
                    uiItemBaseList[i].Refresh(boxItem, ItemClick, true, isEffect);
                    uiItemBaseList[i].SetCount(it.Current.Value);
                }

                i++;
            }
        }

        mGrid.Reposition();
        return uiItemBaseList;
    }

    /// <summary>
    /// 再指定节点上设置item,如果首次已经设置那么会刷新item表
    /// </summary>
    /// <param name="boxid"></param>
    /// <param name="grid"></param>
    /// <param name="uiItemlists"></param>
    /// <param name="size"></param>
    public static void GetItemByBoxid(int boxid, UIGridContainer grid, ref List<UIItemBase> uiItemlists,
        itemSize size = itemSize.Default, bool isEffect = false)
    {
        Dictionary<int, int> BoxReward = new Dictionary<int, int>();
        BoxTableManager.Instance.GetBoxAwardById(boxid, BoxReward);
        GetItemByBoxid(BoxReward, grid, ref uiItemlists, size, isEffect);
    }

    /// <summary>
    /// 再指定节点上设置item,如果首次已经设置那么会刷新item表
    /// </summary>
    /// <param name="BoxReward">box信息</param>
    /// <param name="grid">挂载位置</param>
    /// <param name="uiItemlists">返回item引用</param>
    /// <param name="size">格子尺寸</param>
    /// <param name="isEffect">是否显示特效</param>
    public static void GetItemByBoxid(Dictionary<int, int> BoxReward, UIGridContainer grid,
        ref List<UIItemBase> uiItemlists,
        itemSize size = itemSize.Default, bool isEffect = false)
    {
        //Map<int, int> BoxReward = new Map<int, int>();
        //BoxTableManager.Instance.GetBoxAwardById(boxid, BoxReward);
        grid.MaxCount = BoxReward.Count;
        int i = 0;
        if (uiItemlists == null)
            uiItemlists = new List<UIItemBase>();

        for (var it = BoxReward.GetEnumerator(); it.MoveNext();)
        {
            if (uiItemlists != null && uiItemlists.Count <= i)
            {
                uiItemlists.Add(UIItemManager.Instance.GetItem(PropItemType.Normal,
                    grid.controlList[i].transform, size));
            }

            if (ItemTableManager.Instance.TryGetValue(it.Current.Key, out TABLE.ITEM boxItem))
            {
                uiItemlists[i].Refresh(boxItem, null, true, isEffect);
                uiItemlists[i].SetCount(it.Current.Value);
            }

            i++;
        }

        //return uiItemBaseList;
    }

    public static UIItemBase GetItemByInfo(int id, Transform trans, int num = 1, itemSize size = itemSize.Default,
        bool isEffect = false)
    {
        var itemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, trans, size);
        if (ItemTableManager.Instance.TryGetValue(id, out TABLE.ITEM boxItem))
        {
            itemBase.Refresh(boxItem, null, true, isEffect);
            itemBase.SetCount(num);
        }

        return itemBase;
        //itemBase.Refresh();
    }


    /// <summary>
    /// 滑动组件箭头隐藏显示
    /// </summary>
    /// <param name="value"></param>
    /// <param name="down"></param>
    /// <param name="up"></param>
    public static void OnChange(float value, GameObject down = null, GameObject up = null)
    {
        if (value >= 0.95)
        {
            up?.SetActive(true);
            down?.SetActive(false);
        }
        else if (value <= 0.05)
        {
            down?.SetActive(true);
            up?.SetActive(false);
        }
        else
        {
            up?.SetActive(false);
            down?.SetActive(false);
        }
    }

    // public static void OnChageHorizontal(float value, GameObject down = null, GameObject up = null)
    // {
    //     if (value >= 0.95)
    //     {
    //         up?.SetActive(true);
    //         down?.SetActive(false);
    //     }
    //     else if (value <= 0.05)
    //     {
    //         down?.SetActive(true);
    //         up?.SetActive(false);
    //     }
    //     else
    //     {
    //         up?.SetActive(true);
    //         down?.SetActive(true);
    //     }
    // }


    public static void OpenGiftPrompt(Dictionary<int, int> BoxReward)
    {
        UIManager.Instance.CreatePanel<UIGiftPromptPanel>((f) => { (f as UIGiftPromptPanel).OpenPanel(BoxReward); });
    }

    //public static void OpenGiftPrompt(int boxid)
    //{
    //    Dictionary<int, int> BoxReward = BoxTableManager.Instance.GetBoxAwardById(boxid);
    //    UIManager.Instance.CreatePanel<UIGiftPromptPanel>((f) => { (f as UIGiftPromptPanel).OpenPanel(BoxReward); });
    //}

    public static int GetBoxEffect(int boxid)
    {
        return BoxTableManager.Instance.GetEffect(boxid, CSMainPlayerInfo.Instance.Sex,
            CSMainPlayerInfo.Instance.Career);
    }

    public static T Get<T>(Transform parent, string path) where T : Component
    {
        if (parent == null)
        {
            return null;
        }

        Transform transform = parent.Find(path);

        if (transform != null)
        {
            GameObject go = transform.gameObject;
            if (go)
            {
                T obj = go.GetComponent<T>();
                if (obj)
                {
                    return obj as T;
                }
            }
        }

        return null;
    }

    //物品飞到背包里
    public static void ShowFlyToBagEffect(List<List<int>> rewardLis, Vector3 startPos, float time = 0.4f,
        bool IsClosePanel = true)
    {
        UIManager.Instance.CreatePanel<UIRewardFlyEffectPanel>((f) =>
        {
            UIRewardFlyEffectPanel effectPanel = f as UIRewardFlyEffectPanel;
            if (effectPanel != null)
            {
                effectPanel.SetAnimationDuration(time);
                effectPanel.ShowEffect(rewardLis, startPos, IsClosePanel);
            }
        });
    }

    //物品飞到人物身上
    public static void ShowFlyToPlayerEffect(List<int> rewardLis, List<Vector3> startList, float time = 0.4f,
        bool IsClosePanel = true)
    {
        UIManager.Instance.CreatePanel<UIRewardFlyEffectPanel>((f) =>
        {
            UIRewardFlyEffectPanel effectPanel = f as UIRewardFlyEffectPanel;
            if (effectPanel != null)
            {
                effectPanel.SetAnimationDuration(time);
                effectPanel.ShowFlyItemToPlayer(rewardLis, startList, IsClosePanel);
            }
        });
    }


    /// <summary>
    /// 显示隐藏主界面活动栏
    /// </summary>
    /// <param name="isShow">是否显示</param>
    public static void ShowOrHideUIActivitiesPanel(bool isShow)
    {
        UIMainSceneManager mainSceneManager = UIManager.Instance.GetPanel<UIMainSceneManager>();
        if (null != mainSceneManager)
        {
            UIBase uiBase;
            if (mainSceneManager._RegisterPanel.TryGetValue(typeof(UIActivitiesPanel), out uiBase))
            {
                UIActivitiesPanel uiActivitiesPanel = uiBase as UIActivitiesPanel;
                uiActivitiesPanel.ShowActivities(isShow, true);
            }
        }
    }

    private static Dictionary<int, List<List<int>>> DicSundryInfos; //杂项表中金币和绑元的信息

    public static Dictionary<int, List<List<int>>> GetGoldSundryInfo()
    {
        if (DicSundryInfos != null)
        {
            return DicSundryInfos;
        }
        else
        {
            DicSundryInfos = new Dictionary<int, List<List<int>>>();
        }

        //从sundry取出金币与绑元的icon信息
        var goldInfo = SundryTableManager.Instance.GetSundryEffect(614);
        var goldInfos = UtilityMainMath.SplitStringToIntLists(goldInfo);
        DicSundryInfos.Add(1, goldInfos);
        var bindGoldInfo = SundryTableManager.Instance.GetSundryEffect(615);
        var bindGoldInfos = UtilityMainMath.SplitStringToIntLists(bindGoldInfo);
        DicSundryInfos.Add(2, bindGoldInfos);

        return DicSundryInfos;
    }


    /// <summary>
    /// 打开宠物技能tips
    /// </summary>
    /// <param name="id">宠物技能Id</param>
    /// <param name="type">tips显示类型</param>
    public static void OpenUIWarPetSkillTipsPanel(int id,
        SkillTipsAdaptiveType type = SkillTipsAdaptiveType.BottomRight)
    {
        UIManager.Instance.CreatePanel<UIWarPetSkillTipsPanel>(
            P => { (P as UIWarPetSkillTipsPanel).OpenWarPetSkillTipsPanel(id, type); }
        );
    }


    /// <summary>
    /// 判断充值  先判断是否首充,再判断钱是否够 如果为false 说明金币条件不满足
    /// </summary>
    /// <returns></returns>
    public static bool JudgeCharge<T>(int cost) where T : UIBasePanel
    {
        // if (!CSVipInfo.Instance.IsFirstRecharge())
        // {
        //         UtilityTips.ShowPromptWordTips(5, () =>
        //         {
        //             UIManager.Instance.CreatePanel<UIRechargeFirstPanel>();
        //             UIManager.Instance.ClosePanel<T>();
        //         });
        //     return false;
        // }

        //如果元宝不足，提示不足
        if (((int) MoneyType.yuanbao).GetItemCount() < cost)
        {
            ShowGetWay((int) MoneyType.yuanbao);
            // UtilityTips.ShowPromptWordTips(6, () =>
            // {
            //     UtilityPanel.JumpToPanel(12305);
            //     UIManager.Instance.ClosePanel<T>();
            // });
            // return false;
        }

        return true;
    }

    /// <summary>
    /// 处理getway信息，如果首充已充的话，不显示首充按钮
    /// </summary>
    /// <param name="getWayList"></param>
    /// <returns></returns>
    public static List<int> DealWithFirstRecharge(List<int> getWayList)
    {
        if (getWayList == null) return null;
        bool isFirstRecharge = CSVipInfo.Instance.IsFirstRecharge();
        for (int i = 0; i < getWayList.Count; i++)
        {
            if (isFirstRecharge && getWayList[i] == 660)
            {
                getWayList.RemoveAt(i);
                break;
            }
        }

        return getWayList;
    }

    public static bool IsInMap(int mapId)
    {
        return (CSMainPlayerInfo.Instance.MapID == mapId);
    }


    /// <summary>
    /// 获取宠物技能属性相对应的值
    /// </summary>
    /// <param name="attrType">ChongwuXilianShujuku配置表type</param>
    /// <param name="param">ChongwuXilianShujuku配置表相应parameter值</param>
    /// <param name="value">属性数值</param>
    /// <param name="color">属性颜色</param>
    /// <returns></returns>
    public static string GetPetSkillAttr(int attrType, int param, int value, int color = -1, int qualityHandle = 1)
    {
        string str = String.Empty;
        switch (attrType)
        {
            case 1:
                TABLE.CLIENTATTRIBUTE clientattribute;
                if (ClientAttributeTableManager.Instance.TryGetValue(param,
                    out clientattribute))
                {
                    if (color >= 0)
                    {
                        str = clientattribute.per <= 0
                            ? $"{value}".BBCode(color)
                            : $"{(value * 1f / 100).ToString("f1")}%".BBCode(color);
                    }
                    else
                    {
                        str = clientattribute.per <= 0
                            ? $"{value}"
                            : $"{(value * 1f / 100).ToString("f1")}%";
                    }
                }

                break;
            case 2:
                TABLE.CHONGWUSHUXING chongwushuxing;
                if (ChongwuShuxingTableManager.Instance.TryGetValue(param,
                    out chongwushuxing))
                {
                    if (param == 2) //转化为品质文字
                    {
                        if (qualityHandle == 1)
                            str = UtilityColor.GetQualityText(value);
                        else
                            str = value.QualityTextWithOutColor();
                    }
                    else
                    {
                        switch (chongwushuxing.num)
                        {
                            case 1:
                                str = color >= 0 ? $"{value}".BBCode(color) : $"{value}";
                                break;
                            case 2:
                                string f = param == 13 ? "f2" : "f1";

                                str = color >= 0
                                    ? $"{(value * 1f / 100).ToString(f)}%".BBCode(color)
                                    : $"{(value * 1f / 100).ToString(f)}%";
                                break;
                            case 3:
                                str = color >= 0
                                    ? $"{(value * 1f / 1000).ToString("f2")}".BBCode(color)
                                    : $"{(value * 1f / 1000).ToString("f2")}";
                                break;
                        }
                    }
                }

                break;
        }

        return str;
    }

    /// <summary>
    /// buff属性加成显示规则
    /// </summary>
    /// <param name="listBuffEffect">属性加成列表</param>
    /// <param name="bufferId">当前bufferId</param>
    public static void SetAttributeBuff(ILBetterList<string> listBuffEffect, int bufferId)
    {
        if (listBuffEffect == null) return;
        listBuffEffect.Clear();
        TABLE.BUFFER tableBuffer;
        if (!BufferTableManager.Instance.TryGetValue(bufferId, out tableBuffer)) return;
        string[] arrContent;
        float buffvalue = 0f; //公式计算属性值
        switch ((BuffType) tableBuffer.type)
        {
            case BuffType.Attribute:
                if (tableBuffer.tipParam == 1)//单独tips
                {
                    listBuffEffect.Add(tableBuffer.tips);
                }
                else if(tableBuffer.tipParam == 0)//一条属性一行
                {
                    string attrName = "";
                    List<List<int>> listInt = UtilityMainMath.SplitStringToIntLists(tableBuffer.attBuff);
                    for (int i = 0; i < listInt.Count; i++)
                    {
                        List<int> list = listInt[i];
                        if (list.Count == 2)
                        {
                            string buffvalueStr = string.Empty;
                            TABLE.CLIENTATTRIBUTE tableClientAttribute;
                            if (ClientAttributeTableManager.Instance.TryGetValue(list[0], out tableClientAttribute))
                            {
                                attrName = CSString.Format(tableClientAttribute.tipID);
                                buffvalue = UtilityMath.GetBufferValue(bufferId);
                                buffvalueStr = buffvalue.ToString();
                                if (tableClientAttribute.per == 10000)
                                    buffvalueStr = $"{(buffvalue * 1f / 100).ToString($"f{tableClientAttribute.point}")}%";
                            }

                            listBuffEffect.Add(CSString.Format(tableBuffer.tips, attrName, buffvalueStr));
                        }
                    }
                }
                break;
            case BuffType.Damage:
                arrContent = tableBuffer.hurtBuff.Split('#');
                if (arrContent.Length != 2) return;
                switch ((BuffEffectOpportunityType) tableBuffer.exeType)
                {
                    case BuffEffectOpportunityType.Period: //每{0}秒造成{1}点伤害
                        listBuffEffect.Add(CSString.Format(tableBuffer.tips, arrContent[1], arrContent[0]));
                        break;
                    case BuffEffectOpportunityType.Delay: //{0}秒之后造成{1}点伤害
                        listBuffEffect.Add(CSString.Format(tableBuffer.tips, tableBuffer.exeParam, arrContent[0]));
                        break;
                    default:
                        break;
                }

                break;
            case BuffType.Experience: //经验 + {0}%
                listBuffEffect.Add(CSString.Format(tableBuffer.tips, tableBuffer.expBuff * 1f / 100));
                break;
            case BuffType.SlowDown: //移速 - {0}%
                listBuffEffect.Add(CSString.Format(tableBuffer.tips, tableBuffer.parameterBuff));
                break;
            case BuffType.ShieldValue: //携带护盾，护盾值 ：{0}
                listBuffEffect.Add(CSString.Format(tableBuffer.tips, tableBuffer.parameterBuff));
                break;
            case BuffType.HPRecovery:
                arrContent = tableBuffer.replyBuff.Split('#');
                if (arrContent.Length != 2) return;
                switch ((BuffEffectOpportunityType) tableBuffer.exeType)
                {
                    case BuffEffectOpportunityType.Immediately: //恢复{0}点血量
                        if (arrContent[0] != "0")
                            listBuffEffect.Add(CSString.Format(tableBuffer.tips, arrContent[0]));
                        else
                            listBuffEffect.Add(tableBuffer.tips);
                        break;
                    case BuffEffectOpportunityType.Period: //每{0}秒恢复{1}点血量
                        if (arrContent[0] != "0")
                            listBuffEffect.Add(CSString.Format(tableBuffer.tips, arrContent[1], arrContent[0]));
                        else
                            listBuffEffect.Add(tableBuffer.tips);
                        break;
                    case BuffEffectOpportunityType.Delay: //{0}秒后恢复{1}点血量
                        if (arrContent[0] != "0")
                            listBuffEffect.Add(CSString.Format(tableBuffer.tips, tableBuffer.exeParam, arrContent[0]));
                        else
                            listBuffEffect.Add(tableBuffer.tips);
                        break;
                    default:
                        break;
                }

                break;
            case BuffType.MPRecovery:
                arrContent = tableBuffer.replyBuff.Split('#');
                if (arrContent.Length != 2) return;
                switch ((BuffEffectOpportunityType) tableBuffer.exeType)
                {
                    case BuffEffectOpportunityType.Immediately: //恢复{0}点蓝量
                        if (arrContent[0] != "0")
                            listBuffEffect.Add(CSString.Format(tableBuffer.tips, arrContent[0]));
                        else
                            listBuffEffect.Add(tableBuffer.tips);
                        break;
                    case BuffEffectOpportunityType.Period: //每{0}秒恢复{1}点蓝量
                        if (arrContent[0] != "0")
                            listBuffEffect.Add(CSString.Format(tableBuffer.tips, arrContent[1], arrContent[0]));
                        else
                            listBuffEffect.Add(tableBuffer.tips);
                        break;
                    case BuffEffectOpportunityType.Delay: //{0}秒后恢复{1}点蓝量
                        if (arrContent[0] != "0")
                            listBuffEffect.Add(CSString.Format(tableBuffer.tips, tableBuffer.exeParam, arrContent[0]));
                        else
                            listBuffEffect.Add(tableBuffer.tips);
                        break;
                    default:
                        break;
                }

                break;
            case BuffType.ShieldRecovery:
                arrContent = tableBuffer.replyBuff.Split('#');
                if (arrContent.Length != 2) return;
                switch ((BuffEffectOpportunityType) tableBuffer.exeType)
                {
                    case BuffEffectOpportunityType.Immediately: //恢复{0}点护盾值
                        if (arrContent[0] != "0")
                            listBuffEffect.Add(CSString.Format(tableBuffer.tips, arrContent[0]));
                        else
                            listBuffEffect.Add(tableBuffer.tips);
                        break;
                    case BuffEffectOpportunityType.Period: //每{0}秒恢复{1}点护盾值
                        if (arrContent[0] != "0")
                            listBuffEffect.Add(CSString.Format(tableBuffer.tips, arrContent[1], arrContent[0]));
                        else
                            listBuffEffect.Add(tableBuffer.tips);
                        break;
                    case BuffEffectOpportunityType.Delay: //{0}秒后恢复{1}点护盾值
                        if (arrContent[0] != "0")
                            listBuffEffect.Add(CSString.Format(tableBuffer.tips, tableBuffer.exeParam, arrContent[0]));
                        else
                            listBuffEffect.Add(tableBuffer.tips);
                        break;
                    default:
                        break;
                }

                break;
            case BuffType.ReboundDamage: //反弹{0}%的伤害
                listBuffEffect.Add(CSString.Format(tableBuffer.tips, tableBuffer.reboundBuff));
                break;
            case BuffType.BlueShield: //法力值抵挡伤害，可抵挡{0}点
                listBuffEffect.Add(tableBuffer.tips);
                break;
            case BuffType.SealExperience: //封印经验，获得经验 + {0}%
                listBuffEffect.Add(CSString.Format(tableBuffer.tips, tableBuffer.expBuff));
                break;
            default: //只读tips类型
                listBuffEffect.Add(tableBuffer.tips);
                break;
        }
    }


    /// <summary>
    /// 处理getway表的function
    /// </summary>
    /// <param name="getWayId"></param>
    public static void DoGetWayFunc(int getWayId)
    {
        TABLE.GETWAY getWay = null;
        if (GetWayTableManager.Instance.TryGetValue(getWayId, out getWay))
        {
            if (getWay.function == 0) return;
            int funcId = getWay.function;
            if (getWay.type == 1)
            {
                if (funcId == 27101)
                {
                    CSPetLevelUpInfo.Instance.JudgeOpenPetLevelUpPanel();
                    return;
                }
                UtilityPanel.JumpToPanel(funcId);
            }
            else if(getWay.type == 2)
            {
                UtilityPath.FindWithDeliverId(funcId);
            }
        }
    }
}

public struct StringData
{
    public int id;

    public int count;

    //public uint strength;
    public StringData(int _id, int _count /*, uint _strength = 0*/)
    {
        this.id = _id;
        this.count = _count;
        //this.strength = _strength;
    }
}

//ILRuntime
public class CareerData
{
    public int sex;
    public int career;
    public int model;
    public int weapon;

    public CareerData(int _sex, int _career, int _model, int _weapon)
    {
        this.sex = _sex;
        this.career = _career;
        this.model = _model;
        this.weapon = _weapon;
    }
}