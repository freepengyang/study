using System.Collections.Generic;
using Google.Protobuf.Collections;
using ultimate;
using UnityEngine;

public partial class UIUltimateChallengeGainEffectPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    private const int MAX_COUNT_PAGE = 12;

    private RepeatedField<ThreeTuple> _additionAttrs;
    private FixedCircleArray<UltimateGainEffect> _GainEffectList;

    private int CurShopPage = 1;
    private int MaxShopPage = 1;
    private float DragOffsetX;

    public override void Init()
    {
        base.Init();
        AddCollider();
        
        UIEventListener.Get(mscrolviewConti).onDrag = OnDrag;
        UIEventListener.Get(mscrolviewConti).onDragEnd = OnDragEnded;
        
        mbtn_close.onClick = Close;
        _additionAttrs = CSUltimateInfo.Instance.UltimateData.additionAttrs;
        _GainEffectList = new FixedCircleArray<UltimateGainEffect>(MAX_COUNT_PAGE);
    }


    public override void Show()
    {
        base.Show();
        if (_additionAttrs == null || _additionAttrs.Count == 0)
        {
            RefreshNone();
        }
        else
        {
            Refresh();
        }
    }

    private void RefreshNone()
    {
        mnoneAttrbute.SetActive(true);
        mScrollView.gameObject.SetActive(false);
        CSEffectPlayMgr.Instance.ShowUITexture(mpattern, "pattern");
    }

    private void Refresh()
    {
        MaxShopPage = Mathf.CeilToInt(_additionAttrs.Count / (float) MAX_COUNT_PAGE); //当前标签页下商品总页数
        
        RefreshCenterShopItems(1);
    }

    void RefreshCenterShopItems(int page = 1)
    {
        CurShopPage = page;
        int passCount = (page - 1) * MAX_COUNT_PAGE; //小于当前页的物品个数
        mgrid.MaxCount = MAX_COUNT_PAGE;
        for (int i = 0; i < MAX_COUNT_PAGE; i++)
        {
            ThreeTuple shopCfg = _additionAttrs.Count > i + passCount ? _additionAttrs[i + passCount] : null;
            UltimateGainEffect gainEffect = _GainEffectList[i];
            if(gainEffect == null) gainEffect = new UltimateGainEffect();
            if (!gainEffect.isInit)
                gainEffect.Init(mgrid.controlList[i]);
            gainEffect.RefreshUI(shopCfg);
        }

        if (MaxShopPage <= 1)
        {
            mpagePointGrid.gameObject.CustomActive(false);
        }
        else
        {
            mpagePointGrid.gameObject.CustomActive(true);
            mpagePointGrid.MaxCount = MaxShopPage;
            for (int i = 0; i < mpagePointGrid.MaxCount; i++)
            {
                var selected = mpagePointGrid.controlList[i].transform.GetChild(1);
                selected.gameObject.SetActive(i + 1 == CurShopPage);
            }
        }
    }
    
    void OnDrag(GameObject _go, Vector2 offSet)
    {
        DragOffsetX += offSet.x;
    }
    void OnDragEnded(GameObject _go)
    {
        int page = CurShopPage;
        if (MaxShopPage > 1)
        {
            if (DragOffsetX > 0)
            {
                page = CurShopPage == 1 ? MaxShopPage : CurShopPage - 1;
            }
            else if (DragOffsetX < 0)
            {
                page = CurShopPage == MaxShopPage ? 1 : CurShopPage + 1;
            }
            RefreshCenterShopItems(page);
        }
        
        DragOffsetX = 0f;
    }

    protected override void OnDestroy()
    {
        if (_GainEffectList != null)
        {
            for (var i = 0; i < _GainEffectList.Count; i++)
            {
                _GainEffectList[i].Dispose();
            }
            _GainEffectList.Clear();
        }
        CSEffectPlayMgr.Instance.Recycle(mpattern);
        _GainEffectList = null;
        base.OnDestroy();
    }
}

public class UltimateGainEffect : GridContainerBase
{
    public bool isInit;
    private ThreeTuple _ThreeTuple;

    private UISprite _spIcon;
    private UILabel _lbAttr;
    private UILabel _lbValue;
    private GameObject _lbNone;

    public void Init(GameObject go)
    {
        gameObject = go;
        _spIcon = Get<UISprite>("sp_buff");
        _lbAttr = Get<UILabel>("lb_buff");
        _lbValue = Get<UILabel>("lb_rate");
        _lbNone = gameObject.transform.Find("lb_none").gameObject;
        isInit = true;
    }

    public void RefreshUI(ThreeTuple shopCfg)
    {
        if (shopCfg == null)
        {
            RefreshShow(false);
            return;
        }
        RefreshShow(true);

        _ThreeTuple = shopCfg;
        RefreshName();
        RefreshIcon();
    }

    private void RefreshShow(bool show)
    {
        _lbAttr.gameObject.SetActive(show);
        _spIcon.gameObject.SetActive(show);
        _lbValue.CustomActive(show);
        _lbNone.SetActive(!show);
    }

    private void RefreshName()
    {
        _lbAttr.text = CSUltimateInfo.Instance.GetAttrbuteName(_ThreeTuple);
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

    public override void Dispose()
    {
        isInit = false;
        _ThreeTuple = null;
        _spIcon = null;
        _lbAttr = null;
        _lbValue = null;
    }
}