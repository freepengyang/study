using Google.Protobuf.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public partial class UIDailySignInTipsPanel : UIBasePanel
{

    /// <summary> 该组合中依赖的玩家卡牌的所有锁定的依赖组合 </summary>
    List<SignCardCollectionData> allRelatedList = new List<SignCardCollectionData>();

    /// <summary> 锁定的非万能卡(用来做右侧提示) </summary>
    CSBetterLisHot<SignCardData> lockedCards = new CSBetterLisHot<SignCardData>();

    /// <summary> 当前组合可以用的所有玩家持有的万能卡 </summary>
    CSBetterLisHot<SignCardData> relatedPerfectCards = new CSBetterLisHot<SignCardData>();

    /// <summary> 当前勾选的卡牌 </summary>
    CSBetterLisHot<UISignCardMidItem> selectedCards = new CSBetterLisHot<UISignCardMidItem>();


    CSBetterLisHot<TABLE.SIGNCARD> notGetList = new CSBetterLisHot<TABLE.SIGNCARD>();


    SignCardCollectionData data;

    EndLessKeepHandleList<UISignCardTipsCollectionItem, SignCardCollectionData> endLessList;

    public override void Init()
    {
        base.Init();
        AddCollider();

        mClientEvent.Reg((uint)CEvent.CollectionLockInfoChange, CollectionLockInfoChange);

        mbtn_award.onClick = GetRewardBtnClick;
        mbtn_lock.onClick = LockOrUnlockBtnClick;
        UIEventListener.Get(msp_icon.gameObject).onClick = ShowRewardClick;
        
        mscroll_left.SetDynamicArrowVertical(mbtn_scroll);
    }

    public void OpenPanel(SignCardCollectionData collectionData)
    {
        if (collectionData == null || collectionData.config == null) return;
        data = collectionData;

        if (selectedCards == null) selectedCards = new CSBetterLisHot<UISignCardMidItem>();
        else selectedCards.Clear();
        
        mlb_desc.text = data.config.desc;
        mlb_name.text = data.config.name;
        mlb_lock.text = data.isLocked ? "[b0bbcf]解锁" : "[cfbfb0]锁定";
        msp_lockBtn.spriteName = data.isLocked ? "btn_nbig1" : "btn_nbig2";
        mbtn_lock.gameObject.SetActive(data.config.require == 1);
        mobj_curlock.SetActive(data.isLocked);
        //icon
        msp_icon.spriteName = data.honorCfg.pic.ToString();
        msp_quality.spriteName = $"quality{data.honorCfg.quality}";

        GetLockedCards();

        if (relatedPerfectCards == null) relatedPerfectCards = new CSBetterLisHot<SignCardData>();
        else relatedPerfectCards.Clear();

        if (CSSignCardInfo.Instance.playerUniversalCards != null)
        {
            var dic = CSSignCardInfo.Instance.playerUniversalCards;
            for (dic.Begin(); dic.Next();)
            {
                if (!data.needPerfectCardsInfo.ContainsKey(dic.Key)) continue;
                var list = dic.Value;
                for (int i = 0; i < list.Count; i++)
                {
                    relatedPerfectCards.Add(list[i]);
                }
            }
        }

        mPoolHandleManager.RecycleAll();
        //左页面卡牌部分
        int groupA = data.satisfiedCards.Count;//持有的符合条件的普通卡
        int groupB = relatedPerfectCards.Count;//持有的符合品质的万能卡
        int groupC = data.notActiveList.Count;//灰色未满足的虚拟卡

        if (data.config.require == 1)
        {
            notGetList.Clear();
            for (int i = 0; i < groupC; i++)
            {
                int id = data.notActiveList[i];
                TABLE.SIGNCARD cfg;
                if (SignCardTableManager.Instance.TryGetValue(id, out cfg))
                {
                    notGetList.Add(cfg);
                }
            }
            notGetList.Sort((a, b) => { return b.quality - a.quality; });
        }
        else data.notActiveList.Sort((a, b) => { return b - a; });

        data.satisfiedCards.Sort((a, b) =>
        {
            return b.config.quality - a.config.quality;
        });
        relatedPerfectCards.Sort((a, b) =>
        {
            return b.config.quality - a.config.quality;
        });

        int MAXCOUNT = groupA + groupB + groupC;

        mGrid_leftCards.MaxCount = MAXCOUNT;
        for (int i = 0; i < mGrid_leftCards.MaxCount; i++)
        {
            UISignCardMidItem item = mPoolHandleManager.GetCustomClass<UISignCardMidItem>();
            item.UIPrefab = mGrid_leftCards.controlList[i];
            
            if (i < groupA)
            {
                bool islocked = lockedCards.Any(x => { return x.id == data.satisfiedCards[i].id; });
                item.InitItem(data.satisfiedCards[i], islocked, TryToCheckOrUnCheckCard);
            }
            else if (i < groupA + groupB)
            {
                int index = i - groupA;
                item.InitItem(relatedPerfectCards[index], false, TryToCheckOrUnCheckCard);
            }
            else
            {
                int index = i - groupA - groupB;
                switch (data.config.require)
                {
                    case 1:
                        if (notGetList != null && notGetList.Count > index)
                        {
                            TABLE.SIGNCARD cfg = notGetList[index];
                            item.InitItem(cfg);
                        }
                        break;
                    case 2:
                        item.InitItem(data.notActiveList[index]);
                        break;
                    case 3:
                        item.InitItem(-1);
                        break;
                };
            }
            
        }

        int offset = MAXCOUNT > 5 ? 0 : (int)mGrid_leftCards.CellHeight;
        msp_leftBg.height = 411 - offset;
        mobj_line2.transform.localPosition = new Vector2(-189, -160 + offset);
        mbtn_scroll.transform.localPosition = new Vector2(-195, -161 + offset);
        msp_awardBtn.transform.localPosition = new Vector2(-195, -193 + offset);


        RefreshRightUI();
        RefreshGetAwardBtnUI();
    }


    void RefreshRightUI()
    {
        //GetLockedCards();

        mobj_view2.SetActive(lockedCards.Count > 0);

        if (lockedCards.Count < 1) return;

        string hint = ClientTipsTableManager.Instance.GetClientTipsContext(1069);
        CSStringBuilder.Clear();
        StringBuilder names = null;
        for (int i = 0; i < lockedCards.Count; i++)
        {
            names = CSStringBuilder.Append(lockedCards[i].config.name);
            if (i < lockedCards.Count - 1)
            {
                names = CSStringBuilder.Append("、");
            }
        }

        mlb_hintRight.text = CSString.Format(hint, names.ToString());

        //整合锁定组合
        if (allRelatedList == null) allRelatedList = new List<SignCardCollectionData>();
        else allRelatedList.Clear();
        for (int i = 0; i < data.satisfiedCards.Count; i++)
        {
            data.satisfiedCards[i].relatedCollection.WhereToList((x) => { return x.id != data.id && x.isLocked && !allRelatedList.Contains(x); }, allRelatedList, false);
        }

        //int length = allRelatedList.Count > 5 ? 5 : allRelatedList.Count;
        if (endLessList == null)
        {
            endLessList = new EndLessKeepHandleList<UISignCardTipsCollectionItem, SignCardCollectionData>(SortType.Vertical, mwrap_right, mPoolHandleManager, 5, ScriptBinder);
        }
        endLessList.Clear();

        for (int i = 0; i < allRelatedList.Count; i++)
        {
            var item = allRelatedList[i];
            endLessList.Append(item);
        }

        endLessList.Bind();

        //mGrid_right.Bind<SignCardCollectionData, UISignCardTipsCollectionItem>(allRelatedList, mPoolHandleManager);

        int lbOffset = mlb_hintRight.height - 56 > 0 ? mlb_hintRight.height - 56 : 0;
        int gridOffset = endLessList.Count > 2 ? 0 : endLessList.Count == 2 ? 34 : 137;
        
        mtrans_rightScroll.localPosition = new Vector2(183, 67 - lbOffset);
        msp_rightBg.height = 324 + lbOffset - gridOffset;
    }
    

    void RefreshGetAwardBtnUI()
    {
        //msp_awardBtn.color = selectedCards == null || selectedCards.Count < 5 ? Color.black : CSColor.white;//现在不用这种方式置灰
        msp_awardBtn.spriteName = selectedCards == null || selectedCards.Count < 5 ? "btn_samll4" : "btn_samll1";
        mlb_awardBtn.color = UtilityColor.HexToColor(selectedCards == null || selectedCards.Count < 5 ? "#c0c0c0" : "#b0bbcf");
    }


    void GetLockedCards()
    {
        if (lockedCards == null) lockedCards = new CSBetterLisHot<SignCardData>();
        data.satisfiedCards.WhereToList((x) => { return x.relatedCollection.Any((y) => { return y.id != data.id && y.isLocked; }); }, lockedCards);
    }




    public void CollectionLockInfoChange(uint id, object eData)
    {
        if (data != null) OpenPanel(data);
    }


    void GetRewardBtnClick(GameObject go)
    {
        if (selectedCards == null || selectedCards.Count < 5)
        {
            UtilityTips.ShowRedTips(1151);
        }
        else UtilityTips.ShowPromptWordTips(67, ConfirmGetReward, data.config.name);
    }


    void ConfirmGetReward()
    {
        if (data == null || selectedCards == null || selectedCards.Count < 5) return;

        RepeatedField<long> lids = new RepeatedField<long>();
        for (int i = 0; i < selectedCards.Count; i++)
        {
            lids.Add(selectedCards[i].data.entityId);
        }
        Net.ReqSignComposeMessage(data.id, lids);

        UIManager.Instance.ClosePanel<UIDailySignInTipsPanel>();
    }


    void LockOrUnlockBtnClick(GameObject go)
    {
        if (data == null) return;
        data.TryToLockOrUnLock();
    }


    void TryToCheckOrUnCheckCard(UISignCardMidItem card)
    {
        if (card.data == null) return;

        //Debug.LogError("@@@@@Name:" + SignCardTableManager.Instance.GetSignCardName(card.data.id));

        if (card.isSelect)
        {
            selectedCards.Remove(card);
            card.CheckOrUnCheck(false);
        }
        else
        {
            bool canSelect = SelectCardAndCancelASelectedCard(card.data);
            if (canSelect)
            {
                selectedCards.Add(card);
                card.CheckOrUnCheck(true);
            }
            //else
            //{
            //    UtilityTips.ShowRedTips(1076);
            //}
        }

        RefreshGetAwardBtnUI();
    }


    /// <summary>
    /// 检测某张卡是否可以选择。取消选择不做此检测***需求改了，现在不用了
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    /// 指定id类型的组合检测逻辑(require==1)：
    /// 当前检测的卡如果是万能卡，那么直接查找组合中可用的万能卡信息，判断勾选数量是否小于可用数量即可。
    /// 如果非万能卡，先检测是否已勾选过相同id的卡。
    /// 若未勾选过，则判断该组合可用的同品质万能卡数量和勾选的同品质万能卡数量。
    bool CanSelectSomeCard(SignCardData card)
    {
        if (data == null || data.config == null || data.needCardsInfo == null || card == null || card.config == null) return false;
        
        if (card.relatedCollection.Any(x => { return x.id != data.id && x.isLocked; }))
        {
            UtilityTips.ShowRedTips(1269);
            return false;//锁定卡牌无法勾选
        }

        if (selectedCards == null)
        {
            selectedCards = new CSBetterLisHot<UISignCardMidItem>();
            return true;
        }
        if (selectedCards.Count >= 5)
        {
            UtilityTips.ShowRedTips(1268);
            return false;
        }

        bool res = false;
        switch (data.config.require)
        {
            case 1:
                if (data.needPerfectCardsInfo == null) return false;
                int needPerfectCount = 0;
                data.needPerfectCardsInfo.TryGetValue(card.config.quality, out needPerfectCount);
                int selectPerfectCount = selectedCards.WhereCount(x => { return x.data.isUniversal && x.data.config.quality == card.config.quality; });
                //Debug.LogError("@@@@@万能卡:" + card.isUniversal);
                if (card.isUniversal)
                {
                    res = needPerfectCount > selectPerfectCount;
                    if (!res) UtilityTips.ShowRedTips("已选择过同类型万能卡");
                    return res;
                }
                else
                {
                    if (selectPerfectCount < 1)
                    {
                        res = !selectedCards.Any(x => { return x.data.config.id == card.config.id; });
                        if (!res) UtilityTips.ShowRedTips(1076);
                        return res;
                    }
                    res = needPerfectCount > selectPerfectCount;
                    if (!res) UtilityTips.ShowRedTips("已选择过同类型万能卡");
                    return res;
                }
            case 2:
                int needCount = 0;
                if (!data.needCardsInfo.TryGetValue(card.config.quality, out needCount))
                {
                    FNDebug.LogError("组合信息界面流程异常::组合require==2::当前尝试勾选的卡牌品质不符合该组合需要的品质");
                    return false;
                }
                int selectCount = selectedCards.WhereCount(x => { return x.data.config.quality == card.config.quality; });
                res = needCount > selectCount;
                if (!res) UtilityTips.ShowRedTips("已选择过同类型卡牌");
                return res;
            case 3:
                return true;
        }

        FNDebug.LogError("检测卡牌勾选时异常，有未判断到的情况出现");
        return false;
    }

    
    /// <summary>
    /// 新需求，勾选新卡的同时取消勾选一张同类型旧卡
    /// </summary>
    /// <param name="card"></param>
    bool SelectCardAndCancelASelectedCard(SignCardData card)
    {
        if (data == null || data.config == null || data.needCardsInfo == null || card == null || card.config == null) return false;


        if (card.relatedCollection.Any(x => { return x.id != data.id && x.isLocked; }))
        {
            UtilityTips.ShowRedTips(1269);
            return false;//锁定卡牌无法勾选
        }

        if (selectedCards == null) selectedCards = new CSBetterLisHot<UISignCardMidItem>();
        if (selectedCards.Count < 1) return true;


        bool res = true;
        UISignCardMidItem cancelCard = null;
        //判断已勾选的卡数量是否达到需要的数量，满足时，再取消勾选掉其中一张卡
        int needCount = 0;
        int selectCount = 0;
        switch (data.config.require)
        {
            case 1:
                if (card.isUniversal)
                {
                    needCount = data.miniCards.WhereCount(x => { return x.quality == card.config.quality; });
                    selectCount = selectedCards.WhereCount(x => { return x.data.config.quality == card.config.quality; });
                    if (selectCount >= needCount)
                    {
                        cancelCard = selectedCards.FirstOrNull(x => { return x.data.config.quality == card.config.quality; });
                    }
                }
                else
                {
                    cancelCard = selectedCards.FirstOrNull(x => { return x.data.id == card.id; });//先判断有没有已选的同id卡，再判断万能卡
                    if (cancelCard == null)
                    {
                        selectCount = selectedCards.WhereCount(x => { return x.data.config.quality == card.config.quality; });//选择的万能卡和非万能卡要全算在内
                        int needPerfectCount = 0;
                        if (data.needPerfectCardsInfo.TryGetValue(card.config.quality, out needPerfectCount) && selectCount >= needPerfectCount)
                        {
                            cancelCard = selectedCards.FirstOrNull(x => { return x.data.isUniversal && x.data.config.quality == card.config.quality; });//只看万能卡
                        }
                    }
                }
                break;
            case 2:
                data.needCardsInfo.TryGetValue(card.config.quality, out needCount);
                selectCount = selectedCards.WhereCount(x => { return x.data.config.quality == card.config.quality; });
                if (selectCount >= needCount)
                {
                    cancelCard = selectedCards.FirstOrNull(x => { return x.data.config.quality == card.config.quality; });
                }                
                break;
            case 3:
                if (selectedCards.Count >= 5)
                {
                    cancelCard = selectedCards[0];
                }
                break;
        }

        if (cancelCard != null)
        {
            cancelCard.CheckOrUnCheck(false);
            selectedCards.Remove(cancelCard);
        }

        return res;
    }



    void ShowRewardClick(GameObject go)
    {
        if (data == null) return;
        string rewardName = data.honorCfg.name;
        UIManager.Instance.CreatePanel<UIDailySignInAwardPanel>((f) =>
        {
            (f as UIDailySignInAwardPanel).OpenPanel(rewardName, data.rewardDic);
        });
    }


    protected override void OnDestroy()
    {
        data = null;
        allRelatedList?.Clear();
        allRelatedList = null;
        lockedCards?.Clear();
        lockedCards = null;
        relatedPerfectCards?.Clear();
        relatedPerfectCards = null;
        selectedCards?.Clear();
        selectedCards = null;
        endLessList?.Destroy();
        endLessList = null;

        //mGrid_right.UnBind<UISignCardTipsCollectionItem>();

        base.OnDestroy();
    }
}


public class UISignCardMidItem : UIBase, IDispose
{
    //UISprite _sp_bg;
    //UISprite sp_bg { get { return _sp_bg ?? (_sp_bg = Get<UISprite>("sp_bg")); } }

    UISprite _sp_frame;
    UISprite sp_frame { get { return _sp_frame ?? (_sp_frame = Get<UISprite>("sp_frame")); } }

    UISprite _sp_card;
    UISprite sp_card { get { return _sp_card ?? (_sp_card = Get<UISprite>("sp_card")); } }

    GameObject _obj_select;
    GameObject obj_select { get { return _obj_select ?? (_obj_select = Get<GameObject>("select")); } }

    GameObject _obj_lock;
    GameObject obj_lock { get { return _obj_lock ?? (_obj_lock = Get<GameObject>("lock")); } }

    GameObject _obj_mask;
    GameObject obj_mask { get { return _obj_mask ?? (_obj_mask = Get<GameObject>("sp_mask")); } }

    UILabel _lb_name;
    UILabel lb_name { get { return _lb_name ?? (_lb_name = Get<UILabel>("lb_hint")); } }


    Action<UISignCardMidItem> clickAction;

    public SignCardData data;

    public bool isSelect;

    string cardName;

    public override void Dispose()
    {
        data = null;
        _sp_frame = null;
        _sp_card = null;
        _obj_select = null;
        _obj_lock = null;
        _obj_mask = null;
        _lb_name = null;
        clickAction = null;
        base.Dispose();
    }

    /// <summary> 可以操作的实体卡(达成的卡) </summary>
    public void InitItem(SignCardData data, bool isLocked, Action<UISignCardMidItem> action)
    {
        if(data != null && data.config != null) this.data = data;
        clickAction = action;

        isSelect = false;
        RefreshUI(data.config.quality, isLocked);
        //sp_card.color = CSColor.white;
        obj_mask.SetActive(false);
        sp_card.spriteName = data.config.pic;
        cardName = data.config.name;
        lb_name.text = cardName.BBCode(data.config.quality);
        UIEventListener.Get(UIPrefab).onClick = OnClick;
    }

    /// <summary> 指定id但未达成的卡走这条方法 </summary>
    public void InitItem(TABLE.SIGNCARD cfg)
    {
        isSelect = false;
        RefreshUI(cfg.quality);
        sp_card.spriteName = cfg.icon;
        //sp_card.color = Color.black;
        cardName = cfg.name;
        lb_name.text = cardName.BBCode(cfg.quality);
        obj_mask.SetActive(true);
        UIEventListener.Get(UIPrefab).onClick = NotGetClick;
    }

    /// <summary> 未指定id且未达成的卡才会走这条方法 </summary>
    public void InitItem(int quality)
    {
        int qua = quality;
        if (qua < 1)
        {
            qua = 1;
            cardName = CSSignCardInfo.Instance.AnyCardStr;
            lb_name.text = cardName.BBCode(ColorType.MainText);
        }
        else
        {
            switch (quality)
            {
                case 1:
                    cardName = CSSignCardInfo.Instance.AnyWhiteCardStr;
                    lb_name.text = cardName.BBCode(ColorType.White);
                    break;
                case 2:
                    cardName = CSSignCardInfo.Instance.AnyGreenCardStr;
                    lb_name.text = cardName.BBCode(ColorType.Green);
                    break;
                case 3:
                    cardName = CSSignCardInfo.Instance.AnyBlueCardStr;
                    lb_name.text = cardName.BBCode(ColorType.Blue);
                    break;
                case 4:
                    cardName = CSSignCardInfo.Instance.AnyPurpleCardStr;
                    lb_name.text = cardName.BBCode(ColorType.Purple);
                    break;
                case 5:
                    cardName = CSSignCardInfo.Instance.AnyOrangeCardStr;
                    lb_name.text = cardName.BBCode(ColorType.Orange);
                    break;
            }
        }
        isSelect = false;
        RefreshUI(qua);
        if (sp_card != null)
            sp_card.spriteName = CSSignCardInfo.Instance.GetQuestionCardSp(qua);
        //sp_card.color = Color.black;
        obj_mask?.CustomActive(true);
        UIEventListener.Get(UIPrefab).onClick = NotGetClick;
    }



    void RefreshUI(int quality, bool isLocked = false)
    {
        obj_select?.CustomActive(false);
        obj_lock?.CustomActive(isLocked);
        if (sp_frame != null)
            sp_frame.spriteName = CSSignCardInfo.Instance.GetMiniCardsFrameSp(quality);
    }

    
    

    void OnClick(GameObject go)
    {
        clickAction?.Invoke(this);
    }


    void NotGetClick(GameObject go)
    {
        UtilityTips.ShowRedTips(1915, cardName);
    }



    public void CheckOrUnCheck(bool check)
    {
        //if (check) sp_card.color = CSColor.gray;
        //else sp_card.color = CSColor.white;

        obj_select?.CustomActive(check);
        obj_mask?.CustomActive(check);

        isSelect = check;
    }
}