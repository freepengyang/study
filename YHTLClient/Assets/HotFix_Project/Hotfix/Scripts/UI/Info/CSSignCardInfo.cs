using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSignCardInfo : CSInfo<CSSignCardInfo>
{
    /// <summary> 组合中需要的卡牌上限数 </summary>
    //const int MaxCardCountInCollection = 5;

    /// <summary> 初始卡牌栏位数量配置在sundry表中的id </summary>
    const int InitCardLimitSundryTableId = 511;
    /// <summary> 碎片兑换配置在sundry表中的id </summary>
    const int PiecesExchangeSundryTableId = 513;
    /// <summary> 普通卡加万能卡数量上限配置在sundry表中的id </summary>
    const int AllCardCountLimitSundryTableId = 1123;

    #region stringCache
    readonly string[] MiniCardsFrameSp = { "frame_mini_1", "frame_mini_4", "frame_mini_3", "frame_mini_5", "frame_mini_2" };
    readonly string[] CardFrameSp = { "bg_auto_1", "bg_auto_4", "bg_auto_3", "bg_auto_5", "bg_auto_2" };
    readonly string[] CardNameBgSp = { "name_bg_1", "name_bg_4", "name_bg_3", "name_bg_5", "name_bg_2" };
    readonly string[] CardBigFrameSp = { "frame_1", "frame_2", "frame_3", "frame_4", "frame_5" };
    readonly string[] UniversalMiniSp = { "160", "161", "162", "163", "164" };
    readonly string[] QuestionMarkSp = { "1001", "1002", "1003", "1004", "1005" };


    string anyCardStr;
    public string AnyCardStr {
        get
        {
            if (string.IsNullOrEmpty(anyCardStr))
             anyCardStr = ClientTipsTableManager.Instance.GetClientTipsContext(1754);
            return anyCardStr;
        }
    }

    string anyWhiteCardStr;
    public string AnyWhiteCardStr
    {
        get
        {
            if (string.IsNullOrEmpty(anyWhiteCardStr))
                anyWhiteCardStr = ClientTipsTableManager.Instance.GetClientTipsContext(1755);
            return anyWhiteCardStr;
        }
    }

    string anyGreenCardStr;
    public string AnyGreenCardStr
    {
        get
        {
            if (string.IsNullOrEmpty(anyGreenCardStr))
                anyGreenCardStr = ClientTipsTableManager.Instance.GetClientTipsContext(1756);
            return anyGreenCardStr;
        }
    }

    string anyBlueCardStr;
    public string AnyBlueCardStr
    {
        get
        {
            if (string.IsNullOrEmpty(anyBlueCardStr))
                anyBlueCardStr = ClientTipsTableManager.Instance.GetClientTipsContext(1757);
            return anyBlueCardStr;
        }
    }

    string anyPurpleCardStr;
    public string AnyPurpleCardStr
    {
        get
        {
            if (string.IsNullOrEmpty(anyPurpleCardStr))
                anyPurpleCardStr = ClientTipsTableManager.Instance.GetClientTipsContext(1758);
            return anyPurpleCardStr;
        }
    }

    string anyOrangeCardStr;
    public string AnyOrangeCardStr
    {
        get
        {
            if (string.IsNullOrEmpty(anyOrangeCardStr))
                anyOrangeCardStr = ClientTipsTableManager.Instance.GetClientTipsContext(1759);
            return anyOrangeCardStr;
        }
    }

    #endregion

    ILBetterList<string> universalCardsName = new ILBetterList<string>();

    /// <summary> 玩家持有的碎片数量 </summary>
    public int playerPiecesCount;
    /// <summary> 碎片兑换每次消耗的碎片数 </summary>
    public int piecesExchangeNeed;

    /// <summary> 玩家今日已兑换次数 </summary>
    public int todayExchangePiecesTimes;
    /// <summary> 碎片兑换每天次数上限 </summary>
    public int piecesExchangeLimit;

    /// <summary> 玩家当前卡牌上限数(卡牌栏位) </summary>
    public int playerCardLimit;

    /// <summary> 所有卡牌上限数(普通+万能) </summary>
    public int allCardLimit;


    /// <summary> 玩家当前可抽卡次数 </summary>
    public int playerDrawCardTimes;


    PoolHandleManager mPoolHandle = new PoolHandleManager();

    /// <summary> 玩家持有卡牌信息 </summary>
    public CSBetterLisHot<SignCardData> playerCards = new CSBetterLisHot<SignCardData>();

    /// <summary> 玩家持有万能卡,key为万能卡品质 </summary>
    public Map<int, CSBetterLisHot<SignCardData>> playerUniversalCards = new Map<int, CSBetterLisHot<SignCardData>>();

    int universalCardsCount;

    /// <summary> 本次抽卡卡池 </summary>
    public CSBetterLisHot<TABLE.SIGNCARD> cardPool = new CSBetterLisHot<TABLE.SIGNCARD>();
    /// <summary> 本次抽中的卡 </summary>
    public TABLE.SIGNCARD curDrawCard;

    /// <summary> 玩家普通成就信息 </summary>
    public CSBetterLisHot<CommonAchievementData> playerAchievement = new CSBetterLisHot<CommonAchievementData>();

    /// <summary> 玩家终极就信息 </summary>
    public UltimateAchievementData ultAchievementData;

    /// <summary> 所有组合信息(目前是以所有成就包含的组合+安慰组合来算，并没有直接遍历配置表中的所有组合) </summary>
    public CSBetterLisHot<SignCardCollectionData> allCollections = new CSBetterLisHot<SignCardCollectionData>();


    bool signFuncOpen;
    bool SignFuncOpen
    {
        get
        {
            if (!signFuncOpen)
            {
                signFuncOpen = UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_signIn);
            }
            return signFuncOpen;
        }
    }



    public override void Dispose()
    {
        mPoolHandle?.OnDestroy();
        mPoolHandle = null;
        playerCards?.Clear();
        playerCards = null;
        playerUniversalCards?.Clear();
        playerUniversalCards = null;
        cardPool?.Clear();
        cardPool = null;


    }


    public void Initialize()
    {
        InitExchangePiecesCountsAndLimits();
        InitUniversalCardsMap();
        InitCommonAchievements();
        InitUltimateAchievement();

        InitUniversalCardsName();

        Net.ReqSignInfoMessage();
    }

    #region SCEvents
    public void SC_SignInfo(sign.SignInfo msg)
    {
        playerCardLimit = msg.capacity;
        playerPiecesCount = msg.fragment;
        todayExchangePiecesTimes = msg.todayExchangeCount;
        
        //组合锁定和达成信息
        if (allCollections != null)
        {
            for (int i = 0; i < allCollections.Count; i++)
            {
                allCollections[i].ServerTryToSetLockInfo(false);
                allCollections[i].hasReached = false;
            }
        }
        else InitCommonAchievements();
        for (int i = 0; i < msg.lockIds.Count; i++)
        {
            SignCardCollectionData data = allCollections.FirstOrNull(x => { return x.id == msg.lockIds[i]; });
            if (data != null) data.ServerTryToSetLockInfo(true);
        }
        for (int i = 0; i < msg.collection.Count; i++)
        {
            SignCardCollectionData data = allCollections.FirstOrNull(x => { return x.id == msg.collection[i]; });
            if (data != null) data.hasReached = true;
        }

        //卡牌部分
        ClearPlayerAllCommonCards();
        for (int i = 0; i < msg.cards.Count; i++)
        {
            PlayerGetNewCommonCard(msg.cards[i]);
        }
        //万能卡部分
        InitUniversalCardsMap();
        for (int i = 0; i < msg.masterCards.Count; i++)
        {
            TABLE.SIGNCARD cfg;
            if (!SignCardTableManager.Instance.TryGetValue(msg.masterCards[i].cardId, out cfg)) continue;
            UpdateUniversalCards(msg.masterCards[i], cfg);
        }

        //成就部分
        for (int i = 0; i < playerAchievement.Count; i++)
        {
            CommonAchievementData data = playerAchievement[i];
            data.receivedReward = false;
        }
        for (int i = 0; i < msg.honors.Count; i++)
        {
            CommonAchievementData data = playerAchievement.FirstOrNull(x => { return x.id == msg.honors[i].honorId; });
            if (data != null)
            {
                data.receivedReward = msg.honors[i].reward == 1;
            }
        }


        mClientEvent.SendEvent(CEvent.PlayerCardChange);
    }

    /// <summary>
    /// 注，协议中写的未获得的卡片是5张，实际是6张，后端将抽中的也放了进来，可直接展示
    /// </summary>
    /// <param name="msg"></param>
    public void SC_CardPoolInfo(sign.CardInfo msg)
    {
        SignCardTableManager.Instance.TryGetValue(msg.card, out curDrawCard);
        if (cardPool == null) cardPool = new CSBetterLisHot<TABLE.SIGNCARD>();
        else cardPool.Clear();
        
        for (int i = 0; i < msg.cardNotGet.Count; i++)
        {
            TABLE.SIGNCARD card;
            if (!SignCardTableManager.Instance.TryGetValue(msg.cardNotGet[i], out card)) continue;
            cardPool.Add(card);
        }

        mClientEvent.SendEvent(CEvent.CardPoolUpdate);
        //if (curDrawCard != null && cardPool.Count > 0)
        //{
        //    EventData eData = CSEventObjectManager.Instance.SetValue(curDrawCard, cardPool);
        //    mClientEvent.SendEvent(CEvent.CardPoolUpdate, eData);
        //    CSEventObjectManager.Instance.Recycle(eData);
        //}

    }


    /// <summary>
    /// 卡牌变化。注：proto中SignCard的lid为唯一id， cardId为配置id， 普通卡牌时count为1代表新增，0代表移除。万能卡时count为当前此类万能卡的数量，直接更新到此数量即可。
    /// </summary>
    /// <param name="msg"></param>
    public void SC_CardChange(sign.CardChange msg)
    {
        RepeatedField<sign.SignCard> list = msg.cards;
        for (int i = 0; i < list.Count; i++)
        {
            TABLE.SIGNCARD cfg;
            if (!SignCardTableManager.Instance.TryGetValue(list[i].cardId, out cfg)) continue;
            if (cfg.perfert == 1)//万能卡
            {
                UpdateUniversalCards(list[i], cfg);
            }
            else
            {
                if (list[i].count == 1) PlayerGetNewCommonCard(list[i]);
                else PlayerLostCommonCard(list[i]);
            }
        }

        mClientEvent.SendEvent(CEvent.PlayerCardChange);
    }

    /// <summary>
    /// 组合锁定信息。后端传来的实际为锁定的组合id
    /// </summary>
    /// <param name="msg"></param>
    public void SC_CollectionLockChange(sign.LockCard msg)
    {
        if (allCollections == null) return;
        for (int i = 0; i < allCollections.Count; i++)
        {
            allCollections[i].ServerTryToSetLockInfo(false);
        }

        for (int i = 0; i < msg.cardGroups.Count; i++)
        {
            SignCardCollectionData data = allCollections.FirstOrNull(x => { return x.id == msg.cardGroups[i]; });
            if (data != null) data.ServerTryToSetLockInfo(true);
        }

        mClientEvent.SendEvent(CEvent.CollectionLockInfoChange);
    }

    public void SC_PiecesChange(sign.FragmentChange msg)
    {
        playerPiecesCount = msg.fragment;
        todayExchangePiecesTimes = msg.todayExchangeCount;
        mClientEvent.SendEvent(CEvent.PiecesCountChange);
    }


    public void SC_CollectionChange(sign.CollectionChange msg)
    {
        if (allCollections == null) InitCommonAchievements();
        SignCardCollectionData data = allCollections.FirstOrNull(x => { return x.id == msg.collection; });
        if (data != null) data.hasReached = true;

        mClientEvent.SendEvent(CEvent.CollectionReachedInfoChange);
    }


    /// <summary>
    /// 成就变化，仅普通成就
    /// </summary>
    /// <param name="msg"></param>
    public void SC_HonorChange(sign.HonorChange msg)
    {
        if (playerAchievement == null) InitCommonAchievements();
        CommonAchievementData data = playerAchievement.FirstOrNull(x => { return x.id == msg.honor.honorId; });
        if (data != null)
        {
            data.receivedReward = msg.honor.reward == 1;
        }
        mClientEvent.SendEvent(CEvent.HonorChange);
    }


    public void SC_UltAchievementReached(sign.ResFinalSignReward msg)
    {
        if (ultAchievementData == null) InitUltimateAchievement();
        ultAchievementData.spendDays = msg.day;
        ultAchievementData.exchangePiecesCount = msg.fragmentCount;
        ultAchievementData.reachedCount = msg.count;

        //ClearPlayerAllCommonCards();
        ////InitExchangePiecesCountsAndLimits();
        //InitUniversalCardsMap();
        //InitCommonAchievements();

        mClientEvent.SendEvent(CEvent.UltHonorReceive);
        UIManager.Instance.CreatePanel<UIDailySignInPromptPanel>();
    }

    #endregion


    #region Init
    void InitExchangePiecesCountsAndLimits()
    {
        string str = SundryTableManager.Instance.GetSundryEffect(PiecesExchangeSundryTableId);
        string[] str1 = str.Split('#');
        if (str1.Length > 0) int.TryParse(str1[0], out piecesExchangeNeed);
        if (str1.Length > 1) int.TryParse(str1[1], out piecesExchangeLimit);

        string strLimit = SundryTableManager.Instance.GetSundryEffect(InitCardLimitSundryTableId);
        int.TryParse(strLimit, out playerCardLimit);

        strLimit = SundryTableManager.Instance.GetSundryEffect(AllCardCountLimitSundryTableId);
        int.TryParse(strLimit, out allCardLimit);
    }

    void InitUniversalCardsMap()
    {
        if (playerUniversalCards == null) playerUniversalCards = new Map<int, CSBetterLisHot<SignCardData>>();

        var arr = SignCardTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var element = arr[k].Value as TABLE.SIGNCARD;
            if (element.perfert == 1)
            {
                CSBetterLisHot<SignCardData> list = null;
                playerUniversalCards.TryGetValue(element.quality, out list);
                if (list == null)
                {
                    list =new CSBetterLisHot<SignCardData>();
                    playerUniversalCards[element.quality] = list;
                }
                else list.Clear();
            }
        }
    }

    void InitCommonAchievements()
    {
        if (playerAchievement == null) playerAchievement = new CSBetterLisHot<CommonAchievementData>();
        else RecycleOneListData(playerAchievement);

        if (allCollections == null) allCollections = new CSBetterLisHot<SignCardCollectionData>();
        else RecycleOneListData(allCollections);

        var arr = SignCardHonorTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            TABLE.SIGNCARDHONOR cfg = arr[k].Value as TABLE.SIGNCARDHONOR;
            if (cfg.isLast == 1) continue;//终极成就不做处理。也不包含组合信息

            List<int> collectionIds = UtilityMainMath.SplitStringToIntList(cfg.honorRequire);
            if (cfg.isComfort != 1)//安慰礼包不在成就里显示，但是会显示在主界面的所有组合中
            {
                CommonAchievementData data = NewAchievementData(cfg);
                BindCollectionsToList(collectionIds, data.collections, allCollections);
                playerAchievement.Add(data);
            }
            else BindCollectionsToList(collectionIds, allCollections);
        }

        playerAchievement.Sort((a, b) => { return b.id - a.id; });
    }
    
    void InitUltimateAchievement()
    {
        if (ultAchievementData == null) ultAchievementData = mPoolHandle.GetCustomClass<UltimateAchievementData>();
        TABLE.SIGNCARDHONOR cfg = null;
        var arr = SignCardHonorTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var x = arr[k].Value as TABLE.SIGNCARDHONOR;
            if (x.isLast == 1)
            {
                cfg = x;
                break;
            }
        }
        if (cfg == null) return;
        ultAchievementData.Init(cfg);
    }

    #endregion
    

    #region otherFunc
    CommonAchievementData NewAchievementData(TABLE.SIGNCARDHONOR cfg)
    {
        CommonAchievementData data = mPoolHandle.GetCustomClass<CommonAchievementData>();
        data.Init(cfg);
        return data;
    }


    /// <summary>
    /// 根据id将组合配置信息存入到指定列表,非数据信息
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="lists"></param>
    void BindCollectionsToList(List<int> ids, params CSBetterLisHot<SignCardCollectionData>[] lists)
    {
        if (lists.Length < 1) return;
        for (int i = 0; i < ids.Count; i++)
        {
            TABLE.SIGNCARDCOLLECTION cfg;
            if (!SignCardCollectionTableManager.Instance.TryGetValue(ids[i], out cfg)) continue;
            SignCardCollectionData data = NewCollectionData(cfg);
            for (int j = 0; j < lists.Length; j++)
            {
                lists[j].Add(data);
            }
        }
    }


    SignCardCollectionData NewCollectionData(TABLE.SIGNCARDCOLLECTION cfg)
    {
        SignCardCollectionData data = mPoolHandle.GetCustomClass<SignCardCollectionData>();
        data.Init(cfg);
        return data;
    }



    SignCardData NewCardData(TABLE.SIGNCARD cfg, long entityId)
    {
        SignCardData data = mPoolHandle.GetCustomClass<SignCardData>();
        data.Init(cfg, entityId);
        return data;
    }


    /// <summary>
    /// 绑定与某张卡牌相关的所有组合信息，在新增卡牌数据时调用。万能卡不绑定
    /// </summary>
    /// <param name="cardData"></param>
    void BindCardAndCollections(SignCardData cardData)
    {
        if (allCollections == null || allCollections.Count < 1 || cardData.config == null) return;

        for (int i = 0; i < allCollections.Count; i++)
        {
            TABLE.SIGNCARDCOLLECTION cfg = allCollections[i].config;
            if (cfg == null || allCollections[i].needCardsInfo == null) continue;
            bool isRelated = false;
            switch (cfg.require)
            {
                case 1:
                    isRelated = allCollections[i].needCardsInfo.ContainsKey(cardData.id);
                    break;
                case 2:
                    isRelated = allCollections[i].needCardsInfo.ContainsKey(cardData.config.quality);
                    break;
                case 3:
                    isRelated = true;
                    break;
            }
            if (isRelated)
            {
                cardData.SetCollection(allCollections[i]);
                allCollections[i].AddSatisfiedCard(cardData);
            }
        }
    }

    /// <summary>
    /// 将某张卡牌与所有组合解绑，在失去这张卡牌时调用
    /// </summary>
    /// <param name="cardData"></param>
    void UnBindCardAndCollections(SignCardData cardData)
    {
        if (allCollections == null || allCollections.Count < 1 || cardData.config == null) return;

        CSBetterLisHot<SignCardCollectionData> list = cardData.relatedCollection;
        if (list == null || list.Count < 1) return;
        for (int i = 0; i < list.Count; i++)
        {
            list[i].RemoveSatisfiedCard(cardData);
        }
    }


    void UpdateUniversalCards(sign.SignCard data, TABLE.SIGNCARD cfg)
    {
        if (cfg == null || playerUniversalCards == null) return;     

        CSBetterLisHot<SignCardData> list;
        if (playerUniversalCards.TryGetValue(cfg.quality, out list))
        {
            RecycleOneListData(list);
            for (int i = 0; i < data.count; i++)
            {
                SignCardData card = NewCardData(cfg, data.lid);
                list.Add(card);
            }
        }
    }


    void PlayerGetNewCommonCard(sign.SignCard info)
    {
        if (playerCards == null) playerCards = new CSBetterLisHot<SignCardData>();

        if (playerCards.Any(x => { return x.id == info.cardId && x.entityId == info.lid; })) return;//检查是否有重复卡牌

        TABLE.SIGNCARD cfg;
        if (!SignCardTableManager.Instance.TryGetValue(info.cardId, out cfg)) return;
                

        SignCardData data = NewCardData(cfg, info.lid);
        BindCardAndCollections(data);
        playerCards.Add(data);
    }


    void PlayerLostCommonCard(sign.SignCard info)
    {
        if (playerCards == null || playerCards.Count < 1) return;

        SignCardData data = playerCards.FirstOrNull(x => { return x.id == info.cardId && x.entityId == info.lid; });
        if (data != null)
        {
            UnBindCardAndCollections(data);
            playerCards.Remove(data);
            mPoolHandle.Recycle(data);
        }
    }


    void ClearPlayerAllCommonCards()
    {
        if (playerCards == null || playerCards.Count < 1) return;
        for (int i = 0; i < playerCards.Count; i++)
        {
            UnBindCardAndCollections(playerCards[i]);
        }
        RecycleOneListData(playerCards);
    }


    void RecycleOneListData<T>(CSBetterLisHot<T> list)
    {
        if (list == null || mPoolHandle == null) return;
        for (int i = 0; i < list.Count; i++)
        {
            mPoolHandle.Recycle(list[i]);
        }
        list.Clear();
    }


    /// <summary>
    /// 对组合信息排序
    /// </summary>
    void SortCollections()
    {
        if (allCollections == null || allCollections.Count < 2) return;
        allCollections.Sort((a, b) =>
        {
            if (a.hasReached != b.hasReached)
            {
                return a.hasReached ? -1 : 1;
            }
            else
            {
                return b.ReachedCount() - a.ReachedCount();
            }
        });
    }

    void InitUniversalCardsName()
    {
        if (universalCardsName == null) universalCardsName = new ILBetterList<string>();
        else universalCardsName.Clear();
        var arr = SignCardTableManager.Instance.array.gItem.handles;
        for(int k = 0,max = arr.Length;k < max;++k)
        {
            var item = arr[k].Value as TABLE.SIGNCARD;
            if (item.perfert == 1)
            {
                string color = UtilityColor.GetItemNameValue(item.quality);
                universalCardsName.Add($"{color}{item.name}");
            }
        }
    }


    #endregion


    #region publicFunc

    /// <summary>  获取当前终极成就信息  </summary> <returns></returns>
    public UltimateAchievementData GetUltimateData()
    {
        if (ultAchievementData == null)
        {
            InitUltimateAchievement();
        }

        return ultAchievementData;
    }


    Dictionary<int, int> temp = new Dictionary<int, int>();
    /// <summary>  获取某个组合的卡牌满足数量（不算重复），普通卡和万能卡都要算上  </summary> <returns></returns>
    public int CollectionActivedCardCount(SignCardCollectionData data)
    {
        if (data == null || data.notActiveList == null) return 0;

        var miniCards = data.GetMiniCardPreview();
        int count = 5 - data.notActiveList.Count;
        if (count < 5)
        {
            if (data.config != null && data.config.require == 3)
            {
                for (var it = playerUniversalCards.GetEnumerator(); it.MoveNext();)
                {
                    var list = it.Current.Value;
                    count += Mathf.Min(list.Count, 5 - count);
                    if (count >= 5) break;
                }
            }
            else
            {
                temp.Clear();
                for (int i = 0; i < miniCards.Count; i++)
                {
                    if (miniCards[i].isActive) continue;
                    var k = miniCards[i].quality;
                    if (!temp.ContainsKey(k)) temp.Add(k, 1);
                    else temp[k] = temp[k] + 1;
                }

                CSBetterLisHot<SignCardData> list;
                for (var it = temp.GetEnumerator(); it.MoveNext();)
                {
                    var key = it.Current.Key;
                    var value = it.Current.Value;
                    if (playerUniversalCards.TryGetValue(key, out list))
                    {
                        count += Mathf.Min(list.Count, value);
                        if (count >= 5) break;
                    }
                }
            }
            
        }

        return count;
    }


    public void TryToDrawACard()
    {
        if (curDrawCard == null || cardPool == null || cardPool.Count < 6) return;
        
        curDrawCard = null;
        cardPool.Clear();
        mClientEvent.SendEvent(CEvent.CardPoolUpdate);

        Net.ReqChoseCardMessage();
    }


    public bool CanSignIn()
    {
        if (!SignFuncOpen)
        {
            return false;
        }
        return curDrawCard != null && cardPool != null && cardPool.Count > 0;
    }


    public bool AnyAchievementCanAccept()
    {
        if (!SignFuncOpen)
        {
            return false;
        }
        if (playerAchievement == null || ultAchievementData == null) return false;

        int commonReceivedCount = 0;
        for (int i = 0; i < playerAchievement.Count; i++)
        {
            var data = playerAchievement[i];
            if (data.receivedReward)
            {
                commonReceivedCount++;
                continue;
            }
            var hasReachedCount = data.GetReachedCollectionsCount();
            if (hasReachedCount >= data.collections.Count) return true;
        }

        return commonReceivedCount >= playerAchievement.Count;
    }



    public int GetUniversalCardsCount()
    {
        universalCardsCount = 0;
        if (playerUniversalCards == null) return 0;
        for (var it = playerUniversalCards.GetEnumerator(); it.MoveNext();)
        {
            var list = it.Current.Value;
            if (list != null) universalCardsCount += list.Count;
        }

        return universalCardsCount;
    }


    public string GetMiniCardsFrameSp(int quality)
    {
        if (quality < 1 || quality > 5) return "";
        return MiniCardsFrameSp[quality - 1];
    }

    public string GetCardFrameSp(int quality)
    {
        if (quality < 1 || quality > 5) return "";
        return CardFrameSp[quality - 1];
    }

    public string GetCardNameBgSp(int quality)
    {
        if (quality < 1 || quality > 5) return "";
        return CardNameBgSp[quality - 1];
    }

    public string GetCardBigFrameSp(int quality)
    {
        if (quality < 1 || quality > 5) return "";
        return CardBigFrameSp[quality - 1];
    }


    public string GetUniversalMiniSp(int quality)
    {
        if (quality < 1 || quality > 5) return "";
        return UniversalMiniSp[quality - 1];
    }


    public string GetQuestionCardSp(int quality)
    {
        if (quality < 1 || quality > 5) return "";
        return QuestionMarkSp[quality - 1];
    }


    public string GetUniversalCardsNameStr(int quality)
    {
        if (universalCardsName == null) return "";
        if (quality < 1 || quality > universalCardsName.Count) return "";
        return universalCardsName[quality - 1];
    }

    #endregion
}


public class SignCardData : IDispose
{
    public int id { get { return _config == null ? 0 : _config.id; } }

    long _entityId;
    /// <summary>  实体id(唯一id)  </summary>
    public long entityId { get { return _entityId; } }

    TABLE.SIGNCARD _config;
    public TABLE.SIGNCARD config { get { return _config; } }

    /// <summary>  该卡牌可兑换的所有组合  </summary>
    public CSBetterLisHot<SignCardCollectionData> relatedCollection;

    bool _isUniversal;
    /// <summary> 是否万能牌 </summary>
    public bool isUniversal { get { return _isUniversal; } }

    public void Dispose()
    {
        _config = null;
        relatedCollection?.Clear();
        relatedCollection = null;
    }


    public void Init(TABLE.SIGNCARD cfg, long entityId)
    {
        if (cfg == null) return;
        _config = cfg;
        _entityId = entityId;
        _isUniversal = cfg.perfert == 1;

        if (relatedCollection == null) relatedCollection = new CSBetterLisHot<SignCardCollectionData>();
        else relatedCollection.Clear();
    }

    public void SetCollection(SignCardCollectionData data)
    {
        if (data == null || _isUniversal) return;//万能牌不做此处理
        if (relatedCollection == null) relatedCollection = new CSBetterLisHot<SignCardCollectionData>();

        relatedCollection.Add(data);
    }

    /// <summary>  尝试分解该卡牌  </summary>
    public void TryToBreak()
    {
        if (_isUniversal)
        {
            UtilityTips.ShowRedTips("万能卡无法分解");
            return;
        }
        if (relatedCollection != null && relatedCollection.Any((x) => { return x.isLocked; }))
        {
            UtilityTips.ShowRedTips(1147);
        }
        else UtilityTips.ShowPromptWordTips(66, ConfirmBreak, config.name, config.recycleNum);
    }

    void ConfirmBreak()
    {
        Net.ReqSignFragmentMessage(entityId.ToGoogleList());
    }

}


/// <summary> 组合信息  </summary>
public class SignCardCollectionData : IDispose
{
    public int id { get { return _config == null ? 0 : _config.id; } }

    TABLE.SIGNCARDCOLLECTION _config;
    public TABLE.SIGNCARDCOLLECTION config { get { return _config; } }


    TABLE.SIGNCARDHONOR _honorCfg;
    public TABLE.SIGNCARDHONOR honorCfg { get { return _honorCfg; } }
    
    
    /// <summary> 本次是否兑换过 </summary>
    public bool hasReached;

    bool _isLocked;
    /// <summary> 是否锁定::组合锁定状态下满足该组合的所有卡牌均被锁定，锁定后后续获得的卡牌也会被锁。目前只有指定id类型的组合可锁 </summary>
    public bool isLocked { get { return _isLocked; } }



    /// <summary>  该组合的道具奖励信息  </summary>
    public Map<int, int> rewardDic = new Map<int, int>();

    /// <summary>  该组合需要的卡牌信息::如果config.require为1，则key为卡牌指定id，为2时则key为quality。value为对应需要的数量  </summary>
    /// <summary>  该集合在初始化之后不再变动  </summary>
    public Map<int, int> needCardsInfo = new Map<int, int>();

    /// <summary>  该组合可用的万能卡类型信息.key为万能卡品质,value为数量(数量其实只在指定id类型的情况下有用)  </summary>
    public Map<int, int> needPerfectCardsInfo = new Map<int, int>();

    /// <summary>  未激活的卡牌信息(显示用),随玩家卡牌变动。value为id或品质  </summary>
    public CSBetterLisHot<int> notActiveList = new CSBetterLisHot<int>();

    /// <summary>  满足条件的玩家卡牌,随玩家卡牌变动  </summary>
    public CSBetterLisHot<SignCardData> satisfiedCards = new CSBetterLisHot<SignCardData>();

    /// <summary>  显示用的迷你卡牌预览 </summary>
    public List<MiniCardSlot> miniCards = new List<MiniCardSlot>();


    PoolHandleManager mPoolHandle = new PoolHandleManager();


    public void Dispose()
    {
        rewardDic?.Clear();
        rewardDic = null;
        needCardsInfo?.Clear();
        needCardsInfo = null;
        needPerfectCardsInfo?.Clear();
        needPerfectCardsInfo = null;
        notActiveList?.Clear();
        notActiveList = null;
        satisfiedCards?.Clear();
        satisfiedCards = null;
        miniCards?.Clear();
        miniCards = null;

        mPoolHandle?.OnDestroy();
        mPoolHandle = null;
    }


    public void Init(TABLE.SIGNCARDCOLLECTION cfg)
    {
        if (cfg == null) return;
        _config = cfg;

        mPoolHandle.RecycleAll();

        if (SignCardHonorTableManager.Instance.TryGetValue(cfg.reward, out _honorCfg))
        {
            //奖励信息
            string rewardsStr = _honorCfg.item;
            var rewardList = UtilityMainMath.SplitStringToIntLists(rewardsStr);
            if (rewardDic == null) rewardDic = new Map<int, int>();
            else rewardDic.Clear();
            for (int i = 0; i < rewardList.Count; i++)
            {
                if (rewardList[i].Count < 2) continue;
                rewardDic.Add(rewardList[i][0], rewardList[i][1]);
            }
        }

        

        //需要的卡牌信息
        if (needCardsInfo == null) needCardsInfo = new Map<int, int>();
        else needCardsInfo.Clear();
        if (needPerfectCardsInfo == null) needPerfectCardsInfo = new Map<int, int>();
        else needPerfectCardsInfo.Clear();

        if (notActiveList == null) notActiveList = new CSBetterLisHot<int>();
        else notActiveList.Clear();
        if (miniCards == null) miniCards = new List<MiniCardSlot>();
        else miniCards.Clear();

        switch (cfg.require)
        {
            case 1:
                List<int> cardIds = UtilityMainMath.SplitStringToIntList(cfg.param);
                TABLE.SIGNCARD card = null;
                for (int i = 0; i < cardIds.Count; i++)
                {
                    if (!SignCardTableManager.Instance.TryGetValue(cardIds[i], out card)) continue;
                    needCardsInfo.Add(cardIds[i], 1);//策划表示指定id类型时必然是五个不同的id。
                    notActiveList.Add(cardIds[i]);

                    int qua = card.quality;
                    if (!needPerfectCardsInfo.ContainsKey(qua)) needPerfectCardsInfo.Add(qua, 1);
                    else needPerfectCardsInfo[qua] = needPerfectCardsInfo[qua] + 1;

                    MiniCardSlot mini = mPoolHandle.GetCustomClass<MiniCardSlot>();
                    string icon = card.icon;
                    mini.Init(qua, icon);
                    mini.id = cardIds[i];
                    miniCards.Add(mini);
                }
                break;
            case 2:
                List<List<int>> qualities = UtilityMainMath.SplitStringToIntLists(cfg.param);
                for (int i = 0; i < qualities.Count; i++)
                {
                    if (qualities[i].Count < 1) continue;
                    int num = qualities[i].Count < 2 ? 1 : qualities[i][1];
                    needCardsInfo.Add(qualities[i][0], num);
                    for (int j = 0; j < num; j++)
                    {
                        notActiveList.Add(qualities[i][0]);
                        MiniCardSlot mini = mPoolHandle.GetCustomClass<MiniCardSlot>();
                        //string icon = CSSignCardInfo.Instance.GetQuestionCardSp(qualities[i][0]);
                        mini.Init(qualities[i][0]);
                        miniCards.Add(mini);
                    }
                    needPerfectCardsInfo[qualities[i][0]] = num;

                }
                break;
            case 3://安慰礼包为五张任意卡
                //for (var it = SignCardTableManager.Instance.dic.GetEnumerator(); it.MoveNext();)
                //{
                //    var v = it.Current.Value;
                //    if (v.perfert == 1)
                //    {
                //        needPerfectCardsInfo[v.quality] = 5;//这个数量在安慰礼包的情况下其实没用
                //    }
                //}

                //string miniIcon = CSSignCardInfo.Instance.GetQuestionCardSp(1);
                for (int i = 0; i < 5; i++)
                {
                    notActiveList.Add(1);
                    MiniCardSlot mini = mPoolHandle.GetCustomClass<MiniCardSlot>();
                    mini.Init(1, "1001");
                    miniCards.Add(mini);

                    needPerfectCardsInfo[i + 1] = 5;
                }
                break;
        }

        if (satisfiedCards == null) satisfiedCards = new CSBetterLisHot<SignCardData>();
        else satisfiedCards.Clear();

    }


    /// <summary>
    /// 加入满足该组合条件的卡牌
    /// </summary>
    /// <param name="data"></param>
    public void AddSatisfiedCard(SignCardData data)
    {
        if (data == null || data.config == null) return;

        if (satisfiedCards == null) satisfiedCards = new CSBetterLisHot<SignCardData>();
        
        if (satisfiedCards.Any((x) => { return x.entityId == data.entityId; }))
        {
            FNDebug.LogError("正在向组合信息中添加具有和已有元素相同实体id的卡牌，请检查流程");
            return;
        }

        satisfiedCards.Add(data);

        if (notActiveList == null || config == null || miniCards == null) return;
        MiniCardSlot mini = null;
        switch (config.require)
        {
            case 1:
                if (notActiveList.Contains(data.id)) notActiveList.Remove(data.id);
                mini = miniCards.FirstOrNull((x) => { return x.id == data.id && x.quality == data.config.quality && x.icon == data.config.icon.ToString() && !x.isActive; });
                break;
            case 2:
                if (notActiveList.Contains(data.config.quality)) notActiveList.Remove(data.config.quality);
                mini = miniCards.FirstOrNull((x) => { return x.quality == data.config.quality && !x.isActive; });
                break;
            case 3:
                if (notActiveList.Count > 0) notActiveList.RemoveAt(notActiveList.Count - 1);
                mini = miniCards.FirstOrNull((x) => { return !x.isActive; });
                break;
        }
        if (mini != null) mini.isActive = true;
    }


    /// <summary>
    /// 移除满足该组合条件的卡牌
    /// </summary>
    /// <param name="data"></param>
    public void RemoveSatisfiedCard(SignCardData data)
    {
        if (satisfiedCards == null || data == null || data.config == null) return;

        if (satisfiedCards.Contains(data))
        {
            satisfiedCards.Remove(data);
            if (notActiveList == null || config == null || miniCards == null) return;
            MiniCardSlot mini = null;
            switch (config.require)
            {
                case 1:
                    if (/*!notActiveList.Contains(data.id) &&*/ !satisfiedCards.Any(x => { return x.id == data.id; }))
                    {
                        notActiveList.Add(data.id);
                        mini = miniCards.FirstOrNull((x) => { return x.id == data.id && x.quality == data.config.quality && x.icon == data.config.icon.ToString() && x.isActive; });
                    }                    
                    break;
                case 2:
                    int needCount = 0;
                    needCardsInfo.TryGetValue(data.config.quality, out needCount);
                    if (satisfiedCards.WhereCount(x=> { return x.config.quality == data.config.quality; }) < needCount)
                    {
                        notActiveList.Add(data.config.quality);
                        mini = miniCards.FirstOrNull((x) => { return x.quality == data.config.quality && x.isActive; });
                    }
                    break;
                case 3:
                    if(satisfiedCards.Count < 5)
                    {
                        notActiveList.Add(1);
                        mini = miniCards.FirstOrNull((x) => { return x.isActive; });
                    }
                    break;
            }
            if (mini != null) mini.isActive = false;
        }
    }

    /// <summary>
    /// 是否可达成该组合
    /// </summary>
    /// <returns></returns>
    public bool CanReached()
    {
        if (notActiveList == null) return false;
        if (notActiveList.Count > 0) return false;
        return true;
    }

    /// <summary>
    /// 达成的卡牌数量(不算重复)
    /// </summary>
    /// <returns></returns>
    public int ReachedCount()
    {
        if (notActiveList == null) return 0;
        int leftNum = 5;
        leftNum -= notActiveList.Count;
        return leftNum < 0 ? 0 : leftNum;
    }


    public List<MiniCardSlot> GetMiniCardPreview()
    {
        if (miniCards == null) return null;
        miniCards.Sort((a, b) =>
        {
            if (a.isActive == b.isActive)
            {
                return b.quality - a.quality;
            }
            else
            {
                return a.isActive ? -1 : 1;
            }
        });
        return miniCards;
    }


    /// <summary> 尝试锁定或解锁该组合，目前只有指定id类型的组合可锁 </summary>
    public void TryToLockOrUnLock()
    {
        if (config == null || config.require != 1) return;
        if (!isLocked)
        {
            Net.ReqLockCardMessage(config.id.ToGoogleList());
        }
        else Net.ReqUnLockCardMessage(config.id.ToGoogleList());

        int tipsId = _isLocked ? 1634 : 1633;
        UtilityTips.ShowTips(tipsId);
    }

    public void ServerTryToSetLockInfo(bool setlock)
    {
        if (config == null) return;
        if (config.require != 1 && setlock)
        {
            FNDebug.LogError("后端异常，正在尝试锁定非指定id类型的组合！");
            return;
        }

        _isLocked = setlock;
    }
}



/// <summary> 普通成就  </summary>
public class CommonAchievementData : IDispose
{
    public int id { get { return _config == null ? 0 : _config.id; } }

    TABLE.SIGNCARDHONOR _config;
    public TABLE.SIGNCARDHONOR config { get { return _config; } }

    /// <summary> 该成就包含的所有组合 </summary>
    public CSBetterLisHot<SignCardCollectionData> collections;

    /// <summary>  成就奖励信息(对应honorItem字段)  </summary>
    public Map<int, int> honorRewardDic;

    /// <summary>  成就下的组合奖励信息(对应item字段)  </summary>
    public Map<int, int> collectionRewardDic;

    /// <summary>  该成就奖励的额外卡牌栏位  </summary>
    public int rewardSlots;

    /// <summary>  该成就奖励本次是否领取  </summary>
    public bool receivedReward;

    public void Dispose()
    {
        collections?.Clear();
        collections = null;
        honorRewardDic?.Clear();
        honorRewardDic = null;
        collectionRewardDic?.Clear();
        collectionRewardDic = null;
    }

    public void Init(TABLE.SIGNCARDHONOR cfg)
    {
        if (cfg == null) return;
        _config = cfg;

        if (collections == null) collections = new CSBetterLisHot<SignCardCollectionData>();
        else collections.Clear();

        //奖励信息
        if (honorRewardDic == null) honorRewardDic = new Map<int, int>();
        else honorRewardDic.Clear();
        string rewardsStr = SignCardHonorTableManager.Instance.GetSignCardHonorHonorItem(cfg.id);
        var list = UtilityMainMath.SplitStringToIntLists(rewardsStr);
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Count < 2) continue;
            honorRewardDic.Add(list[i][0], list[i][1]);
        }

        if (collectionRewardDic == null) collectionRewardDic = new Map<int, int>();
        else collectionRewardDic.Clear();
        string rewardsStr2 = SignCardHonorTableManager.Instance.GetSignCardHonorItem(cfg.id);
        var list2 = UtilityMainMath.SplitStringToIntLists(rewardsStr2);
        for (int i = 0; i < list2.Count; i++)
        {
            if (list2[i].Count < 2) continue;
            collectionRewardDic.Add(list2[i][0], list2[i][1]);
        }
    }
    

    /// <summary> 获取完成的组合数量 </summary>
    public int GetReachedCollectionsCount()
    {
        if (collections == null) return 0;
        int count = collections.WhereCount((x) => { return x.hasReached; });
        return count;
    }

}

/// <summary> 终极成就  </summary>
public class UltimateAchievementData : IDispose
{
    TABLE.SIGNCARDHONOR _config;
    public TABLE.SIGNCARDHONOR config { get { return _config; } }

    /// <summary> 达成天数  </summary>
    public int spendDays;

    /// <summary> 总达成次数  </summary>
    public int reachedCount;

    /// <summary> 碎片兑换次数  </summary>
    public int exchangePiecesCount;

    /// <summary>  成就奖励信息(对应honorItem字段)  </summary>
    public Map<int, int> honorRewardDic;

    /// <summary>  成就下的组合奖励信息(对应item字段)  </summary>
    public Map<int, int> collectionRewardDic;

    public void Init(TABLE.SIGNCARDHONOR cfg)
    {
        if (cfg == null) return;
        _config = cfg;
        //奖励信息
        if (honorRewardDic == null) honorRewardDic = new Map<int, int>();
        else honorRewardDic.Clear();
        string rewardsStr = SignCardHonorTableManager.Instance.GetSignCardHonorHonorItem(cfg.id);
        var list = UtilityMainMath.SplitStringToIntLists(rewardsStr);
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Count < 2) continue;
            honorRewardDic.Add(list[i][0], list[i][1]);
        }

        if (collectionRewardDic == null) collectionRewardDic = new Map<int, int>();
        else collectionRewardDic.Clear();
        string rewardsStr2 = SignCardHonorTableManager.Instance.GetSignCardHonorItem(cfg.id);
        var list2 = UtilityMainMath.SplitStringToIntLists(rewardsStr2);
        for (int i = 0; i < list2.Count; i++)
        {
            if (list2[i].Count < 2) continue;
            collectionRewardDic.Add(list2[i][0], list2[i][1]);
        }
    }

    public void Dispose()
    {
        honorRewardDic?.Clear();
        honorRewardDic = null;
        collectionRewardDic?.Clear();
        collectionRewardDic = null;
    }


}



/// <summary> 组合信息中用来预览的迷你卡牌  </summary>
public class MiniCardSlot : IDispose
{
    public void Dispose()
    {

    }

    public int id;//只有指定id类型的组合才会用到
    public int quality;
    public bool isActive;
    public string icon;

    string[] QuestionMarkSp = { "1001", "1002", "1003", "1004", "1005" };

    public void Init(int _quality, string _icon = "", bool _isActive = false)
    {
        quality = _quality;
        isActive = _isActive;
        
        if (string.IsNullOrEmpty(_icon))
        {
            if (quality > 0  && quality < 6)
            {
                icon = QuestionMarkSp[quality - 1];
            }
        }
        else icon = _icon;
    }
}