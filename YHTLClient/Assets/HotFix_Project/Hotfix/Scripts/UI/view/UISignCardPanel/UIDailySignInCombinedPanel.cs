using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class UIDailySignInCombinedPanel : UIBasePanel
{

    CSSignCardInfo signInfo;
    CSBetterLisHot<SignCardData> playerCards;

    EndLessKeepHandleList<UISignCardTipsCollectionItem, SignCardCollectionData> endLessList;

    /// <summary>  显示万能卡数量的文本控件缓存，key为万能卡品质  </summary>
    Map<int, UILabel> universalCountLabels;

    /// <summary>  是否可以翻牌  </summary>
    bool canDrawCard;

    /// <summary>动画卡牌缓存列表</summary>
    CSBetterLisHot<UIAnimSignCard> animCards = new CSBetterLisHot<UIAnimSignCard>();

    /// <summary> 左侧组合类缓存列表 </summary>
    CSBetterLisHot<UISignCardTipsCollectionItem> leftItems = new CSBetterLisHot<UISignCardTipsCollectionItem>();

    /// <summary> 背包卡牌类缓存列表 </summary>
    CSBetterLisHot<UISignCardItem> commonCards = new CSBetterLisHot<UISignCardItem>();

    TABLE.SIGNCARD curGetCard;
    CSBetterLisHot<TABLE.SIGNCARD> notGetCards;

    /// <summary>  抽牌界面是否已出现  </summary>
    bool cardPoolIsShow;


    //public CSBetterLisHot<TABLE.SIGNCARD> tempPool = new CSBetterLisHot<TABLE.SIGNCARD>();
    ///// <summary> 下一次抽中的卡) </summary>
    //public TABLE.SIGNCARD tempDrawCard;

    bool exchangeLock;


    // Schedule drawCardBeginSch;
    // Schedule animCardsHideSch;
    // Schedule PoolOrBagFadeInAndOutSch;

    string universalHintStr;



    public override void Init()
    {
        base.Init();

        signInfo = CSSignCardInfo.Instance;
        playerCards = signInfo.playerCards;
        
        CSEffectPlayMgr.Instance.ShowUITexture(mobj_emptyTexture, "daily_hint");

        mClientEvent.Reg((int)CEvent.CardPoolUpdate, CardPoolUpdate);
        mClientEvent.Reg((int)CEvent.PlayerCardChange, PlayerCardChange);
        mClientEvent.Reg((int)CEvent.PiecesCountChange, PiecesCountChange);
        mClientEvent.Reg((int)CEvent.CollectionReachedInfoChange, CollectionReachedInfoChange);
        mClientEvent.Reg((int)CEvent.UltHonorReceive, UltHonorReceive);
        mClientEvent.Reg((uint)CEvent.CollectionLockInfoChange, CollectionReachedInfoChange);

        mbtn_showPreview.onClick = AwardPreviewBtnClick;
        mbtn_exchangePieces.onClick = ExchangePiecesBtnClick;
        UIEventListener.Get(mbtn_help).onClick = HelpBtnClick;
        mbtn_hint.onClick = HintDiClick;

        if (signInfo != null && signInfo.allCollections != null && signInfo.allCollections.Count > 0)
        {
            int length = mwrap.itemSize * signInfo.allCollections.Count;
            mscroll_left.SetDynamicArrowVerticalWithWrap(length, mbtn_scrollLeft);
        }
        else mscroll_left.SetDynamicArrowVertical(mbtn_scrollLeft);

        mscroll_bag.SetDynamicArrowVertical(mbtn_scrollBag);

        universalHintStr = ClientTipsTableManager.Instance.GetClientTipsContext(1682);

        mfx_fly.SetOnFinished(() => { mfx_fly.CustomActive(false); });
        mfx_fly.CustomActive(false);

        RegisterRed(mbtn_showPreview.transform.Find("redpoint").gameObject, RedPointType.SignInAchivement);

        InitUniversalCardsUI();
    }

    public override void Show()
    {
        base.Show();

        InitPoolAndBagAnim();

        RefreshRightBagUI();
        RefreshLeftUI();
        RefreshBagCardsUI();

        RefreshUniversalCardsCount();

        //Net.ReqSignInfoMessage();
        CardPoolUpdate(0, null);

        mobj_hint.CustomActive(false);

    }

    public override void OnHide()
    {
        CancelDelayInvoke();
    }

    void RefreshLeftUI()
    {
        if (signInfo == null) return;
        int universalCount = signInfo.GetUniversalCardsCount();
        mobj_empty.CustomActive(playerCards.Count <= 0 && universalCount <= 0);
        mobj_notEmpty.CustomActive(playerCards.Count > 0 || universalCount > 0);

        if (playerCards.Count > 0 || universalCount > 0)
        {
            CheckAllCollectionsList();

            if (signInfo.allCollections == null) return;

            int length = signInfo.allCollections.Count > 7 ? 7 : signInfo.allCollections.Count;

            if (endLessList == null)
            {
                endLessList = new EndLessKeepHandleList<UISignCardTipsCollectionItem, SignCardCollectionData>(SortType.Vertical, mwrap, mPoolHandleManager, length, ScriptBinder);
            }
            endLessList.Clear();

            for (int i = 0; i < signInfo.allCollections.Count; i++)
            {
                var data = signInfo.allCollections[i];
                endLessList.Append(data);
            }

            endLessList.Bind();

            //if (leftItems == null) leftItems = new CSBetterLisHot<UISignCardTipsCollectionItem>();
            //else RecycleList(leftItems);

            //mGrid_left.MaxCount = signInfo.allCollections.Count;
            //for (int i = 0; i < mGrid_left.MaxCount; i++)
            //{
            //    UISignCardTipsCollectionItem item = mPoolHandleManager.GetCustomClass<UISignCardTipsCollectionItem>();
            //    item.UIPrefab = mGrid_left.controlList[i];
            //    item.RefreshUI(signInfo.allCollections[i]);
            //    leftItems.Add(item);
            //    UIEventListener.Get(item.UIPrefab, signInfo.allCollections[i]).onClick = CollectionClick;
            //}
        }
    }

    /// <summary>
    /// 卡牌数量和碎片数量的ui刷新。不包含背包卡牌刷新
    /// </summary>
    void RefreshRightBagUI()
    {
        if (signInfo == null) return;
        int curPieces = signInfo.playerPiecesCount;
        int needPieces = signInfo.piecesExchangeNeed;
        mlb_pieces.text = $"{curPieces}/{needPieces}".BBCode(curPieces >= needPieces ? ColorType.Green : ColorType.MainText);

        string mainColor = UtilityColor.GetColorString(ColorType.MainText);
        string redColor = UtilityColor.GetColorString(ColorType.Red);
        if (playerCards == null) return;
        int curCards = playerCards.Count;
        int cardsLimit = signInfo.playerCardLimit;
        mlb_cards.text = $"{mainColor}卡牌栏{(curCards >= cardsLimit - 1 ? redColor : mainColor)}({curCards}/{cardsLimit})";

        msp_exchangePieces.color = curPieces < needPieces ? Color.black : CSColor.white;

        mobj_fullHint.SetActive(curCards >= cardsLimit);
    }


    void RefreshBagCardsUI()
    {
        if (playerCards == null) return;
        playerCards.Sort((a, b) =>
        {
            return b.config.quality - a.config.quality;
        });

        if (commonCards == null) commonCards = new CSBetterLisHot<UISignCardItem>();
        else RecycleList(commonCards);

        mGrid_commonCard.MaxCount = playerCards.Count;
        for (int i = 0; i < mGrid_commonCard.MaxCount; i++)
        {
            UISignCardItem item = mPoolHandleManager.GetCustomClass<UISignCardItem>();
            item.UIPrefab = mGrid_commonCard.controlList[i];
            item.RefreshUI(playerCards[i], i);
            commonCards.Add(item);

            UIEventListener.Get(item.UIPrefab, playerCards[i]).onClick = BagCardClick;
        }

        //mbtn_scrollBag.SetActive(playerCards.Count > 20);
    }


    /// <summary>
    /// 初始化万能卡ui
    /// </summary>
    void InitUniversalCardsUI()
    {
        if (signInfo == null || signInfo.playerUniversalCards == null) return;

        if (universalCountLabels == null) universalCountLabels = new Map<int, UILabel>();
        else universalCountLabels.Clear();

        mGrid_universalCard.MaxCount = signInfo.playerUniversalCards.Count;
        int i = 0;
        UISprite card;
        UISprite frame;
        UILabel num;
        for (signInfo.playerUniversalCards.Begin(); signInfo.playerUniversalCards.Next();)
        {
            var item = mGrid_universalCard.controlList[i];
            int quality = signInfo.playerUniversalCards.Key;
            card = item.GetComponent<UISprite>();
            card.spriteName = signInfo.GetUniversalMiniSp(quality);
            frame = item.transform.GetChild(0).GetComponent<UISprite>();
            frame.spriteName = signInfo.GetMiniCardsFrameSp(quality);
            num = item.transform.GetChild(1).GetComponent<UILabel>();
            num.text = $"X{signInfo.playerUniversalCards.Value.Count}";
            universalCountLabels.Add(quality, num);

            UIEventListener.Get(item, quality).onClick = ShowHintClick;

            i++;
        }
    }

    /// <summary>
    /// 刷新万能卡数量
    /// </summary>
    void RefreshUniversalCardsCount()
    {
        if (signInfo == null || universalCountLabels == null || signInfo.playerUniversalCards == null || universalCountLabels.Count != signInfo.playerUniversalCards.Count) return;

        for (signInfo.playerUniversalCards.Begin(); signInfo.playerUniversalCards.Next();)
        {
            universalCountLabels[signInfo.playerUniversalCards.Key].text = $"X{signInfo.playerUniversalCards.Value.Count}";
        }
    }



    void InitPoolAndBagAnim()
    {
        cardPoolIsShow = false;
        mobj_cardPool.SetActive(false);
        mobj_bag.SetActive(true);
        mtweenAlphaBag.ResetToFrom();//不能依靠该方式重置alpha值。因为tween在正序和倒序播放动画后该重置方法重置的值是相反的
        mtweenAlphaBag.value = 1;
        mtweenAlphaBag.SetOnFinished(BagPanelTweenAlphaFinished);

        mtweenAlphaPool.ResetToFrom();
        mtweenAlphaPool.value = 0;
        mtweenAlphaPool.SetOnFinished(PoolPanelTweenAlphaFinished);
    }


    void ShowCardPool()
    {
        //InitPoolAndBagAnim();

        mtweenAlphaBag.PlayForward();
        ScriptBinder.Invoke(mtweenAlphaBag.duration, () => 
        {
            mtweenAlphaPool.PlayForward(); }
        );
        //PoolOrBagFadeInAndOutSch = Timer.Instance.Invoke(mtweenAlphaBag.duration, sch =>
        //{
        //    mtweenAlphaPool.PlayForward();
        //});

        cardPoolIsShow = true;
        canDrawCard = true;
        RefreshCardPool();
    }

    void RefreshCardPool()
    {
        if (animCards != null)
        {
            RecycleList(animCards);
        }
        else animCards = new CSBetterLisHot<UIAnimSignCard>();

        // 注，协议中写的未获得的卡片是5张，实际是6张，后端将抽中的也放了进来，可直接展示
        //if (mGrid_cardPool.MaxCount != notGetCards.Count)
        //{
        //    mGrid_cardPool.MaxCount = notGetCards.Count;
        //}
        mGrid_cardPool.MaxCount = notGetCards.Count;

        float x = 0;
        float y = 0;

        for (int i = 0; i < mGrid_cardPool.MaxCount; i++)
        {
            if (i < 3) y = 0;
            else y = -mGrid_cardPool.CellHeight;

            x = (i % 3) * mGrid_cardPool.CellWidth;

            mGrid_cardPool.controlList[i].transform.localPosition = new Vector2(x, y);

            UIAnimSignCard card = mPoolHandleManager.GetCustomClass<UIAnimSignCard>();
            card.InitItem(mGrid_cardPool.controlList[i], PoolCardClick);
            card.RefreshUI(notGetCards[i]);

            animCards.Add(card);
        }

        ScriptBinder.Invoke2(2f, () =>
        {
            if (animCards == null) return;
            for (int i = 0; i < animCards.Count; i++)
            {
                animCards[i]?.BeginShuffle();
            }
        });
        //drawCardBeginSch = Timer.Instance.Invoke(2f, sch=> 
        //{
        //    if (animCards == null) return;
        //    for (int i = 0; i < animCards.Count; i++)
        //    {
        //        animCards[i]?.BeginShuffle();
        //    }
        //});
    }


    void CloseCardPool(Vector2 selectCardPos)
    {
        //mobj_cardPool.SetActive(false);
        //mobj_bag.SetActive(true);

        mtweenAlphaPool.PlayReverse();

        bool showFly = curGetCard.perfert == 1;
        if (showFly)
        {
            Vector2 from = selectCardPos + new Vector2(-33, 80);
            Vector2 to = new Vector2(-108 + 70 * (curGetCard.quality - 1), -275);
            float duration = from.y > -42 ? 0.7f : 0.35f;
            PlayFlyEffect(from, to, duration);
        }        

        ScriptBinder.Invoke(mtweenAlphaPool.duration, () =>
        {
            mtweenAlphaBag.PlayReverse();
            cardPoolIsShow = false;
            exchangeLock = false;
            
        });
        //PoolOrBagFadeInAndOutSch = Timer.Instance.Invoke(mtweenAlphaPool.duration, sch =>
        //{
        //    mtweenAlphaBag.PlayReverse();
        //    cardPoolIsShow = false;
        //    exchangeLock = false;
        //});

        if (animCards != null)
        {
            RecycleList(animCards);
        }
        
        curGetCard = null;
        notGetCards = null;
    }


    void PlayFlyEffect(Vector3 fromPos, Vector3 toPos, float duration)
    {
        CSEffectPlayMgr.Instance.ShowParticleEffect(mfx_fly.gameObject, 17101, 0, true, 1, false, /*new Vector3(-148, -33)*/Vector2.zero);
        mfx_fly.CustomActive(true);
        mfx_fly.from = fromPos;
        mfx_fly.to = toPos;
        mfx_fly.duration = duration;
        mfx_fly.ResetToFrom();
        mfx_fly.PlayForward();
    }

    
    void CheckAllCollectionsList()
    {
        if (signInfo == null || signInfo.allCollections == null) return;

        signInfo.allCollections.Sort((a, b) =>
        {
            if (a.hasReached == b.hasReached)
            {
                int countA = a.notActiveList.Count;
                int countB = b.notActiveList.Count;
                return countA != countB ? countA - countB : a.id - b.id;
            }
            else
            {
                return a.hasReached ? 1 : -1;
            }
        });
    }


    #region btnClicks
    void AwardPreviewBtnClick(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIDailySignInPreviewPanel>();
    }


    void ExchangePiecesBtnClick(GameObject go)
    {
        if (exchangeLock) return;
        if(playerCards.Count >= signInfo.playerCardLimit)
        {
            UtilityTips.ShowRedTips(1713);//栏位已满
            return;
        }
        int universalCount = signInfo.GetUniversalCardsCount();
        if (playerCards.Count + universalCount >= CSSignCardInfo.Instance.allCardLimit)
        {
            UtilityTips.ShowRedTips(2017);//数量达到上限
            return;
        }

        if (signInfo.playerPiecesCount < signInfo.piecesExchangeNeed)
        {
            UtilityTips.ShowRedTips(1631);//碎片不足
            return;
        }
        if (signInfo.todayExchangePiecesTimes >= signInfo.piecesExchangeLimit)
        {
            UtilityTips.ShowRedTips(1070);//本日次数达到上限
            return;
        }
        UtilityTips.ShowPromptWordTips(65, ConfirmExchangePieces, CSSignCardInfo.Instance.piecesExchangeNeed);
    }


    void ConfirmExchangePieces()
    {
        exchangeLock = true;
        Net.ReqExchangeCardMessage();
    }


    void CollectionClick(GameObject go)
    {
        SignCardCollectionData data = (SignCardCollectionData)UIEventListener.Get(go).parameter;
        if (data != null)
        {
            UIManager.Instance.CreatePanel<UIDailySignInTipsPanel>((f) =>
            {
                (f as UIDailySignInTipsPanel).OpenPanel(data);
            });
        }
    }


    void BagCardClick(GameObject go)
    {
        SignCardData data = (SignCardData)UIEventListener.Get(go).parameter;
        if (data != null)
        {
            UIManager.Instance.CreatePanel<UIDailySignInCardTipsPanel>((f) =>
            {
                (f as UIDailySignInCardTipsPanel).OpenPanel(data);
            });
        }
    }


    void PoolCardClick(UIAnimSignCard card)
    {
        if (!canDrawCard || curGetCard == null || card == null) return;
        canDrawCard = false;

        CSSignCardInfo.Instance.TryToDrawACard();

        card.RefreshUI(curGetCard);
        card.BackToFrontAnim();

        ScriptBinder.Invoke3(1f, () =>
        {
            if (animCards != null)
            {
                for (int i = 0; i < animCards.Count; i++)
                {
                    if (animCards[i] == card) continue;
                    animCards[i].DispearAnim();
                }
            }
        });
        //drawCardEndSch = Timer.Instance.Invoke(1f, sch =>
        //{
        //    if (animCards != null)
        //    {
        //        for (int i = 0; i < animCards.Count; i++)
        //        {
        //            if (animCards[i] == card) continue;
        //            animCards[i].DispearAnim();
        //        }
        //    }
        //});

        Vector2 cardPos = card.UIPrefabTrans ? card.UIPrefabTrans.localPosition : new Vector3(-148, -33);
        ScriptBinder.Invoke4(2f, ()=> { CloseCardPool(cardPos); });
        //animCardsHideSch = Timer.Instance.Invoke(2f, sch => { CloseCardPool(); });
    }

    void HelpBtnClick(GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.SignIn);
    }


    void HintDiClick(GameObject go)
    {
        mobj_hint.CustomActive(false);
    }


    void ShowHintClick(GameObject go)
    {
        int quality = System.Convert.ToInt32(UIEventListener.Get(go).parameter);
        string name = CSSignCardInfo.Instance.GetUniversalCardsNameStr(quality);
        string hint = $"{name}：{universalHintStr}";
        mobj_hint.CustomActive(true);
        mlb_hint.text = hint;
        mtrans_hintView.localPosition = new Vector2(0 + 70 * (quality - 1), 0);
    }


    #endregion


    #region Events
    void CardPoolUpdate(uint id, object data)
    {
        //EventData eData = data as EventData;

        //curGetCard = (TABLE.SIGNCARD)eData.arg1;
        //notGetCards = (CSBetterLisHot<TABLE.SIGNCARD>)eData.arg2;

        if (cardPoolIsShow)
        {
            return;
        }

        curGetCard = CSSignCardInfo.Instance.curDrawCard;
        notGetCards = CSSignCardInfo.Instance.cardPool;

        if (curGetCard != null && notGetCards != null && notGetCards.Count > 0)
        {
            if (playerCards.Count < signInfo.playerCardLimit)//有空余卡牌栏才弹出抽卡
            {
                exchangeLock = true;
                ShowCardPool();
            }
        }
        else
        {
            exchangeLock = false;
            Net.ReqSignInfoMessage();
        }
    }


    void PlayerCardChange(uint id, object data)
    {
        RefreshRightBagUI();
        RefreshLeftUI();
        RefreshBagCardsUI();
        RefreshUniversalCardsCount();
    }

    void PiecesCountChange(uint id, object data)
    {
        RefreshRightBagUI();
    }

    void CollectionReachedInfoChange(uint id, object data)
    {
        RefreshLeftUI();
    }

    void UltHonorReceive(uint id, object data)
    {
        PlayerCardChange(id, data);
    }
    
    #endregion


    void CancelDelayInvoke()
    {
        ScriptBinder.StopInvoke();
        ScriptBinder.StopInvoke2();
        ScriptBinder.StopInvoke3();
        ScriptBinder.StopInvoke4();
        // Timer.Instance.CancelInvoke(drawCardBeginSch);
        // Timer.Instance.CancelInvoke(animCardsHideSch);
        // Timer.Instance.CancelInvoke(PoolOrBagFadeInAndOutSch);
    }


    void PoolPanelTweenAlphaFinished()
    {
        mobj_cardPool.SetActive(mtweenAlphaPool.value > 0.5f);
    }

    void BagPanelTweenAlphaFinished()
    {
        mobj_bag.SetActive(mtweenAlphaBag.value > 0.5f);
    }


    void RecycleList<T>(CSBetterLisHot<T> list)
    {
        if (list != null && mPoolHandleManager != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                mPoolHandleManager.Recycle(list[i]);
            }
            list.Clear();
        }
    }



    protected override void OnDestroy()
    {
        CancelDelayInvoke();

        CSEffectPlayMgr.Instance.Recycle(mobj_emptyTexture);
        //signInfo = null;
        //playerCards?.Clear();
        //playerCards = null;
        endLessList?.Destroy();
        endLessList = null;
        universalCountLabels?.Clear();
        universalCountLabels = null;
        animCards?.Clear();
        animCards = null;
        
        base.OnDestroy();
    }
}



public class UISignCardItem : UIBase, IDispose
{
    UILabel _lb_name;
    UILabel lb_name { get { return _lb_name ?? (_lb_name = Get<UILabel>("lb_name")); } }

    UISprite _sp_card;
    UISprite sp_card { get { return _sp_card ?? (_sp_card = Get<UISprite>("sp_card")); } }

    UISprite _sp_flag;
    UISprite sp_flag { get { return _sp_flag ?? (_sp_flag = Get<UISprite>("sp_flag")); } }

    UISprite _sp_frame;
    UISprite sp_frame { get { return _sp_frame ?? (_sp_frame = Get<UISprite>("sp_frame")); } }


    SignCardData mData;

    int cardDepth = 4;
    int flagDepth = 5;
    int frameDepth = 6;
    int nameDepth = 7;
    

    public override void Dispose()
    {
        UnBindObj();
        base.Dispose();
    }

    protected override void OnDestroy()
    {
        UnBindObj();
        base.OnDestroy();
    }

    void UnBindObj()
    {
        mData = null;
        _lb_name = null;
        _sp_card = null;
        _sp_flag = null;
        _sp_frame = null;
        UIPrefab = null;
        UIPrefabTrans = null;
    }

    public void RefreshUI(SignCardData data, int index)
    {
        if (data == null || data.config == null) return;
        mData = data;

        //var color = UtilityColor.GetColorTypeByQuality(data.config.quality);
        UIPrefab.CustomActive(false);
        lb_name.text = $"[dcd5b8]{data.config.name}[-]";
        sp_card.spriteName = data.config.pic.ToString();
        sp_flag.spriteName = CSSignCardInfo.Instance.GetCardNameBgSp(data.config.quality);
        sp_frame.spriteName = CSSignCardInfo.Instance.GetCardBigFrameSp(data.config.quality);
        UIPrefab.name = data.config.name;
        sp_card.depth = cardDepth + index * 5;
        sp_flag.depth = flagDepth + index * 5;
        sp_frame.depth = frameDepth + index * 5;
        lb_name.depth = nameDepth + index * 5;

        UISprite sp_parent = UIPrefab.GetComponent<UISprite>();
        if (sp_parent != null)
        {
            sp_parent.depth = 2 + index * 5;
        }
        UIPrefab.CustomActive(true);
    }

}


public class UIAnimSignCard : UIBase, IDispose
{
    UILabel _lb_name;
    UILabel lb_name { get { return _lb_name ?? (_lb_name = Get<UILabel>("front/lb_name")); } }

    UISprite _sp_card;
    UISprite sp_card { get { return _sp_card ?? (_sp_card = Get<UISprite>("front/sp_card")); } }

    UISprite _sp_flag;
    UISprite sp_flag { get { return _sp_flag ?? (_sp_flag = Get<UISprite>("front/sp_flag")); } }

    UISprite _sp_frame;
    UISprite sp_frame { get { return _sp_frame ?? (_sp_frame = Get<UISprite>("front/sp_frame")); } }

    GameObject _obj_front;
    GameObject obj_front { get { return _obj_front ?? (_obj_front = Get<GameObject>("front")); } }

    GameObject _obj_back;
    GameObject obj_back { get { return _obj_back ?? (_obj_back = Get<GameObject>("back")); } }

    
    TweenScale tweenScale;
    float tweenScaleDuration;

    TweenPosition tweenPos;
    float tweenPosDuration;

    TweenAlpha tweenAlpha;

    Action<UIAnimSignCard> clickAction;

    bool canClick;

    bool isPlayTween;
    

    const float moveToCenterWaitTime = 0.5f;

    bool isUniversal;


    protected override void OnDestroy()
    {
        UnBindObj();

        base.OnDestroy();
    }

    public override void Dispose()
    {
        UnBindObj();

        base.Dispose();
    }

    public void RefreshUI(TABLE.SIGNCARD cfg)
    {         
        if (cfg == null) return;

        var color = UtilityColor.GetColorTypeByQuality(cfg.quality);
        lb_name.text = $"[dcd5b8]{cfg.name}[-]";
        sp_card.spriteName = cfg.pic.ToString();
        sp_flag.spriteName = CSSignCardInfo.Instance.GetCardNameBgSp(cfg.quality);
        sp_frame.spriteName = CSSignCardInfo.Instance.GetCardBigFrameSp(cfg.quality);

        isUniversal = cfg.perfert == 1;
    }


    void UnBindObj()
    {
        //tweenPos?.PlayForward();
        CancelDelayInvoke();
        clickAction = null;
        //tweenPos?.ResetToBeginning();

        _lb_name = null;
        _sp_card = null;
        _sp_flag = null;
        _sp_frame = null;
        _obj_front = null;
        _obj_back = null;
        UIPrefabTrans = null;

    }


    public void InitItem(GameObject go, Action<UIAnimSignCard> _action)
    {

        UIPrefab = go;
        UIPrefabTrans = UIPrefab.transform;
        UIEventListener.Get(go).onClick = CardClick;

        clickAction = _action;
        tweenScale = UIPrefab?.GetComponent<TweenScale>();
        tweenPos = UIPrefab?.GetComponent<TweenPosition>();
        tweenAlpha = UIPrefab?.GetComponent<TweenAlpha>();
        if (tweenScale == null || tweenPos == null || tweenAlpha == null) return;
        onHide();

        tweenScale.ResetToFrom();
        tweenScale.from = new Vector3(1, 1, 1);
        tweenScale.to = new Vector3(0, 1, 1);
        tweenScale.value = new Vector3(1, 1, 1);

        tweenPos.from = UIPrefabTrans.localPosition;
        tweenPos.ResetToFrom();
        //tweenPos.to = new Vector3(180, -110, 0);
        //tweenPos.SetStartToCurrentValue();

        tweenScale.SetOnFinished(OnTweenFinished);
        tweenPos.SetOnFinished(OnTweenFinished);

        tweenScaleDuration = tweenScale.duration;
        tweenPosDuration = tweenPos.duration;

        tweenAlpha.ResetToBeginning();
        //tweenAlpha.value = 1;



        obj_front?.SetActive(true);
        obj_back?.SetActive(false);

        isPlayTween = false;
        canClick = false;
    }
    
    public void onHide()
    {
        tweenScale.enabled = false;
        tweenPos.enabled = false;
        //tweenAlpha.enabled = false;
    }


    public void BeginShuffle()
    {
        FrontToBackAnim();
        ScriptBinder.Invoke(tweenScaleDuration * 2, ShuffleCardAnim);
        //schA = Timer.Instance.Invoke(tweenScaleDuration * 2, sch =>
        //{
        //    ShuffleCardAnim();
        //});
    }


    public void FrontToBackAnim()
    {
        if (tweenScale == null || isPlayTween) return;
        isPlayTween = true;
        obj_front?.SetActive(true);
        obj_back?.SetActive(false);
        tweenScale.PlayForward();
        ScriptBinder.Invoke2(tweenScaleDuration, () =>
        {
            obj_front?.SetActive(false);
            obj_back?.SetActive(true);
            tweenScale.PlayReverse();
        });
        //schB = Timer.Instance.Invoke(tweenScaleDuration, sch =>
        //{
        //    obj_front?.SetActive(false);
        //    obj_back?.SetActive(true);
        //    tweenScale.PlayReverse();
        //});
    }

    public void BackToFrontAnim()
    {
        if (tweenScale == null || isPlayTween) return;
        isPlayTween = true;
        obj_front?.SetActive(false);
        obj_back?.SetActive(true);
        tweenScale.PlayForward();
        ScriptBinder.Invoke3(tweenScaleDuration, () =>
        {
            obj_front?.SetActive(true);
            obj_back?.SetActive(false);
            tweenScale.PlayReverse();
        });
        //schC = Timer.Instance.Invoke(tweenScaleDuration, sch =>
        //{
        //    obj_front?.SetActive(true);
        //    obj_back?.SetActive(false);
        //    tweenScale.PlayReverse();
        //});
    }


    public void ShuffleCardAnim()
    {
        if (tweenPos == null || isPlayTween) return;
        isPlayTween = true;
        tweenPos.PlayForward();
        ScriptBinder.Invoke4(tweenPosDuration + moveToCenterWaitTime, () =>
        {
            tweenPos.AddOnFinished(OnShuffleFinished);
            tweenPos.PlayReverse();
        });
        //schD = Timer.Instance.Invoke(tweenPosDuration + moveToCenterWaitTime, sch =>
        //{
        //    tweenPos.AddOnFinished(OnShuffleFinished);
        //    tweenPos.PlayReverse();
        //});
    }

    public void DispearAnim()
    {
        if (tweenAlpha == null || isPlayTween) return;
        isPlayTween = true;
        tweenAlpha.PlayForward();
    }


    void OnTweenFinished()
    {
        isPlayTween = false;
    }


    void OnShuffleFinished()
    {
        canClick = true;
    }


    void CardClick(GameObject go)
    {
        if (!canClick || isPlayTween) return;
        clickAction?.Invoke(this);
    }

    void CancelDelayInvoke()
    {
        ScriptBinder.StopInvoke();
        ScriptBinder.StopInvoke2();
        ScriptBinder.StopInvoke3();
        ScriptBinder.StopInvoke4();
    }
}