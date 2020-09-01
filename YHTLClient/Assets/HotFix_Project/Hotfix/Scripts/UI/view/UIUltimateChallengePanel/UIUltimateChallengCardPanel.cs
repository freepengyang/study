using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using ultimate;
using UnityEngine;
using Random = System.Random;

public partial class UIUltimateChallengCardPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }

    private RepeatedField<ThreeTuple> _SelectAdditionEffect;
    //private RepeatedField<ThreeTuple> _NewAdditionEffect = new RepeatedField<ThreeTuple>();

    private UltimateCard[] _UltimateCardList;

    private const int CARDHIDETIME = 2;

    private List<int> _OpenConsume;
    private int _refreshConsume;
    private UltimateCard _CurSelectCard;
    //private UltimateCard _CurClickCard;
    //private bool isRefresh;
    private int freeCount;

    bool isMaxRefreshCount;

    public override void Init()
    {
        base.Init();

        //mClientEvent.AddEvent(CEvent.UltimateOpenCardInfoMessage, UltimateOpenCardInfoMessage);
        mClientEvent.AddEvent(CEvent.UltimateSelectCardIndexMessage, UltimateSelectCardIndexMessage);
        mClientEvent.AddEvent(CEvent.Scene_ChangeMap, UpdateMap);
        mClientEvent.AddEvent(CEvent.MoneyChange, RefreshConsume);

        UIEventListener.Get(mbtn_config).onClick = OnConfirmClick;
        mbtn_openadd.onClick = OnGetMoneyClick;
        mbtn_refreshadd.onClick = OnGetMoneyClick;
        mbtn_upgarde.onClick = OnRefreshClick;

        _OpenConsume = UtilityMainMath.SplitStringToIntList(SundryTableManager.Instance.GetSundryEffect(400));
        if (_OpenConsume != null)
        {
            for (var i = 0; i < _OpenConsume.Count; i++)
            {
                if (_OpenConsume[i] == 0)
                    freeCount++;
            }
        }
        int.TryParse(SundryTableManager.Instance.GetSundryEffect(402), out _refreshConsume);

        //isRefresh = true;
        
        CSEffectPlayMgr.Instance.ShowUITexture(mchallenge_bg, "challenge_bg");
        CSEffectPlayMgr.Instance.ShowUITexture(mchallenge_card3, "challenge_card3");

        mlb_hintBeforeSelect.text = ClientTipsTableManager.Instance.GetClientTipsContext(1648);
    }


    public override void Show()
    {
        base.Show();
        Reset();
        mdrawAgain.SetActive(true);
        mbtn_config.SetActive(false);
        mhint1.gameObject.SetActive(false);
        mhint2.SetActive(false);

        mlb_hintBeforeSelect.CustomActive(true);

        _SelectAdditionEffect = CSUltimateInfo.Instance._SelectAdditionEffect.additionAttrs;
        int curTimes = CSUltimateInfo.Instance._SelectAdditionEffect.resetTimes;
        int maxTimes = CSUltimateInfo.Instance._SelectAdditionEffect.maxResetTimes;
        isMaxRefreshCount = curTimes >= maxTimes;
        UtilityTips.ShowGreenTips(1904, curTimes, maxTimes);

        if (_UltimateCardList == null)
            _UltimateCardList = new UltimateCard[_SelectAdditionEffect.Count];
        //else
        //   _UltimateCardList.Clear();

        RefreshUI(true, _SelectAdditionEffect);
        RefreshConsume(0, null);
    }

    private void Reset()
    {
        //_CurClickCard = null;
        _CurSelectCard = null;
        //_NewAdditionEffect?.Clear();
        if (_UltimateCardList != null)
        {
            for (var i = 0; i < _UltimateCardList.Length; i++)
            {
                _UltimateCardList[i]?.UnInit();
            }
        }
    }

    private void RefreshConsume(uint id, object data)
    {
        mlb_refreshvalue.text = CSItemCountManager.Instance.GetItemCount((int)MoneyType.yuanbao) >= _refreshConsume
            ? _refreshConsume.ToString().BBCode(ColorType.Green)
            : _refreshConsume.ToString().BBCode(ColorType.Red);

        
        /*int openNum = GetOpenNum();
        if (_OpenConsume != null && _OpenConsume.Count > openNum)
        {
            int consume = _OpenConsume[openNum];
            if (consume > 0)
            {
                //mhint1.gameObject.SetActive(false);
                //mhint2.SetActive(true);
                mlb_openvalue.text = CSBagInfo.Instance.GetMoneyByType(MoneyType.yuanbao) >= consume
                    ? consume.ToString().BBCode(ColorType.Green)
                    : consume.ToString().BBCode(ColorType.Red);
            }
            else
            {
                //mhint1.gameObject.SetActive(true);
                //mhint1.text = CSString.Format(1272, freeCount - openNum, freeCount);
                mhint2.SetActive(false);
            }
        }
        else
        {
            //mhint1.gameObject.SetActive(false);
            //mhint2.SetActive(false);
        }*/
    }

    private void RefreshUI(bool playTween, RepeatedField<ThreeTuple> Select)
    {
        //mlb_hintBeforeSelect.gameObject.SetActive(true);
        mcards.MaxCount = Select.Count;
        for (int i = 0; i < mcards.MaxCount; i++)
        {
            ThreeTuple shopCfg = Select[i];
            if (_UltimateCardList[i] == null) _UltimateCardList[i] = new UltimateCard();
            UltimateCard gainEffect = _UltimateCardList[i];
            if (!gainEffect.isInit)
                gainEffect.Init(mcards.controlList[i], OnClickSelectCard);
            gainEffect.RefreshUI(shopCfg);
            //if (playTween) gainEffect.PlayTween(true, CARDHIDETIME);
        }
        mdrawAgain.CustomActive(!isMaxRefreshCount);
        //if (playTween)
        //    Timer.Instance.Invoke(CARDHIDETIME + 0.2f, RandomAttrList);
    }

    /*private void RandomAttrList(Schedule schedule)
    {
        Random ran = new Random();
        ThreeTuple temp;
        int index = 0;
        if (_NewAdditionEffect == null) _NewAdditionEffect = new RepeatedField<ThreeTuple>();
        else _NewAdditionEffect.Clear();
        _NewAdditionEffect.AddRange(_SelectAdditionEffect);
        for (var i = 0; i < _NewAdditionEffect.Count; i++)
        {
            index = ran.Next(0, _NewAdditionEffect.Count - 1);
            if (index != i)
            {
                temp = _NewAdditionEffect[i];
                _NewAdditionEffect[i] = _NewAdditionEffect[index];
                _NewAdditionEffect[index] = temp;
            }
        }

        RefreshUI(false, _NewAdditionEffect);
        mdrawAgain.SetActive(true);
        isRefresh = false;
    }*/

    private void OnClickSelectCard(UltimateCard gainEffect)
    {
        //if (isRefresh) return;

        //if (_NewAdditionEffect == null) return;
        /*if (!gainEffect.IsOpen)
        {
            if (_CurSelectCard == null)
            {
                _CurClickCard = gainEffect;
                Net.CSOpenCardMessage();
            }
        }
        else
        {*/
            if (_CurSelectCard != null)
            {
                _CurSelectCard.SetSelect(false);

                if (_CurSelectCard == gainEffect)
                {
                    mbtn_config.SetActive(false);
                    _CurSelectCard = null;
                    mlb_hintBeforeSelect.CustomActive(true);
                }
            else
                {
                    _CurSelectCard = gainEffect;
                    _CurSelectCard.SetSelect(true);
                    mbtn_config.SetActive(true);
                    mlb_hintBeforeSelect.CustomActive(false);
                }
            }
            else
            {
                _CurSelectCard = gainEffect;
                _CurSelectCard.SetSelect(true);
                mbtn_config.SetActive(true);
                mlb_hintBeforeSelect.CustomActive(false);
             }
        //}
    }

    /*private int GetOpenNum()
    {
        int num = 0;
        if (_UltimateCardList != null)
        {
            for (var i = 0; i < _UltimateCardList.Length; i++)
            {
                if (_UltimateCardList[i] != null && _UltimateCardList[i].IsOpen)
                    num++;
            }
        }

        return num;
    }*/

    /*private void UltimateOpenCardInfoMessage(uint id, object data)
    {
        _CurClickCard.IsOpen = true;
        _CurClickCard.PlayTween(true, 0);
        RefreshConsume();
    }*/

    private void UltimateSelectCardIndexMessage(uint id, object data)
    {
        UIManager.Instance.ClosePanel<UIUltimateChallengCardPanel>();
    }

    private void UpdateMap(uint id, object data)
    {
        UltimateSelectCardIndexMessage(0, null);
    }

    /*private void OnCancelClick(GameObject go)
    {
        _CurSelectCard?.SetSelect(false);
        _CurSelectCard = null;
        mbtn_config.SetActive(false);
    }*/

    private void OnConfirmClick(GameObject go)
    {
        if (_CurSelectCard == null) return;
        for (var i = 0; i < _SelectAdditionEffect.Count; i++)
        {
            if (_SelectAdditionEffect[i] == _CurSelectCard._ThreeTuple)
            {
                Net.CSSelectCardIndexMessage(i);
                break;
            }
        }
    }

    private void OnRefreshClick(GameObject go)
    {
        if (CSItemCountManager.Instance.GetItemCount((int)MoneyType.yuanbao) >= _refreshConsume)
        {
            Net.CSRefreshCardMessage();
            //isRefresh = true;
        }
        else
        {
            UtilityTips.ShowRedTips(CSString.Format(959));
        }
    }

    private void OnGetMoneyClick(GameObject go)
    {
        Utility.ShowGetWay((int) MoneyType.yuanbao);
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mchallenge_bg);
        CSEffectPlayMgr.Instance.Recycle(mchallenge_card3);

        base.OnDestroy();
    }
}

public class UltimateCard : GridContainerBase
{
    private GameObject _challengeshow;
    private GameObject _challengehide;
    private GameObject _detail;
    private GameObject _select;
    private UISprite _spIcon;
    private UILabel _lbName;
    private UILabel _lbValue;
    
    
    
    //private TweenScale _tweenScale;

    public ThreeTuple _ThreeTuple;

    //public bool IsOpen { get; set; }
    //public bool isPlayTween;
    public bool isInit;
    //private Action<UltimateCard> _actionShow;
    private Action<UltimateCard> _actionSelect;


    public void Init(GameObject go, Action<UltimateCard> actionSelect)
    {
        gameObject = go;
        //_actionShow = actionShow;
        _actionSelect = actionSelect;
        isInit = true;
        _challengehide = Get<GameObject>("challenge_card1");
        _challengeshow = Get<GameObject>("challenge_card2");
        _detail = Get<GameObject>("detail");
        _select = Get<GameObject>("detail/sp_check");
        _spIcon = Get<UISprite>("detail/sp_icon");
        _lbName = Get<UILabel>("detail/lb_name");
        _lbValue = Get<UILabel>("detail/lb_number");
        //_tweenScale = gameObject.transform.GetComponent<TweenScale>();
        //_tweenScale.onFinished.Clear();
        //_tweenScale.AddOnFinished(TweenFinish);
        _detail.SetActive(true);
        _challengeshow.SetActive(true);
        _challengehide.SetActive(false);
        UIEventListener.Get(_challengehide).onClick = OnSelectClick;
        UIEventListener.Get(_challengeshow).onClick = OnSelectClick;
        
        CSEffectPlayMgr.Instance.ShowUITexture(_challengeshow, "challenge_card2");
        CSEffectPlayMgr.Instance.ShowUITexture(_challengehide, "challenge_card1");
    }

    public void UnInit()
    {
        //IsOpen = false;
        _select.SetActive(false);
    }

    public void RefreshUI(ThreeTuple threeTuple)
    {
        _ThreeTuple = threeTuple;
        RefreshName();
        RefreshIcon();
    }


    /*public void PlayTween(bool show, int time)
    {
        isPlayTween = true;
        if (time > 0)
        {
            _tweenScale.PlayReverse();
            Timer.Instance.Invoke(time, schedule =>
            {
                _tweenScale.Play(show);
                _detail.SetActive(!show);
                _challengeshow.SetActive(!show);
                _challengehide.SetActive(show);
            });
            RefreshItem(show);
        }
        else
        {
            _tweenScale.Play(!show);
            RefreshItem(show);
        }
    }*/

    public void SetSelect(bool isSelect)
    {
        _select.SetActive(isSelect);
        if (gameObject.transform.localScale != Vector3.one) gameObject.transform.localScale = Vector3.one;
    }

    private void RefreshName()
    {
        _lbName.text = CSUltimateInfo.Instance.GetAttrbuteName(_ThreeTuple);
        string value = CSUltimateInfo.Instance.GetAttrbuteValue(_ThreeTuple);
        _lbValue.text = string.IsNullOrEmpty(value) ? "" : $"+{value}";
    }

    private void RefreshIcon()
    {
        TABLE.MAOXIANRANDOMGAIN item = null;
        var arr = MaoXianRandomGainTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            item = arr[k].Value as TABLE.MAOXIANRANDOMGAIN;
            if (item.type == _ThreeTuple.a && item.parameter == _ThreeTuple.b)
            {
                _spIcon.spriteName = item.Icon;
                return;
            }
        }
    }


    // private void RefreshItem(bool show)
    // {
    //     Timer.Instance.Invoke(_tweenScale.duration * 0.7f, schedule2 =>
    //     {
    //         _detail.SetActive(show);
    //         _challengeshow.SetActive(show);
    //         _challengehide.SetActive(!show); 
    //     });
    // }


    /*private void TweenFinish()
    {
        isPlayTween = false;
    }
    */

    private void OnSelectClick(GameObject go)
    {
        //if (isPlayTween) return;
        if (_actionSelect != null)
            _actionSelect(this);
    }
    /*private void OnShowClick(GameObject go)
    {
        if (isPlayTween) return;
        if (_actionShow != null)
            _actionShow(this);
    }*/
    public override void Dispose()
    {
        CSEffectPlayMgr.Instance.Recycle(_challengeshow);
        CSEffectPlayMgr.Instance.Recycle(_challengehide);
    }
}