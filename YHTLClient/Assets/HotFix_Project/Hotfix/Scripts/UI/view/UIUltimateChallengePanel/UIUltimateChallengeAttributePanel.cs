using System;
using ultimate;
using UnityEngine;

public partial class UIUltimateChallengeAttributePanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    public override UILayerType PanelLayerType => UILayerType.Tips;

    private SelectAdditionEffect _SelectAdditionData;

    private int selectId;
    
    public override void Init()
    {
        base.Init();
        UIEventListener.Get(mbtn_select).onClick = OnConfirmClick;
        
        mClientEvent.AddEvent(CEvent.UltimateSelectAdditionIndexMessage, UltimateSelectAdditionIndexMessage);
    }
    
    public override void Show()
    {
        base.Show();
        _SelectAdditionData = CSUltimateInfo.Instance.SelectAdditionData;
        if(_SelectAdditionData == null ) return;
        
        mAttrbuteGrid.MaxCount = _SelectAdditionData.additionAttrs.Count;
        for (var i = 0; i < _SelectAdditionData.additionAttrs.Count; i++)
        {
            UltimateAttrbute attrbute = new UltimateAttrbute();
            attrbute.Init(mAttrbuteGrid.controlList[i].transform, _SelectAdditionData.additionAttrs[i], i);

            UIEventListener.Get(attrbute._bg, attrbute).onClick = OnSelectClick;
        }

    }

    private UltimateAttrbute lastSelect;
    private void OnSelectClick(GameObject go)
    {
        UltimateAttrbute attrbute = UIEventListener.Get(go).parameter as UltimateAttrbute;
        //if(attrbute == null) return;
        //if(lastSelect != null  && lastSelect == attrbute) return;
        
        selectId = attrbute._index;
        
        //lastSelect?.SetSelect(false);
        //attrbute.SetSelect(true);
        //lastSelect = attrbute;
        if(!mbtn_select.activeSelf)
            mbtn_select.SetActive(true);
    }

    private void OnConfirmClick(GameObject go)
    {
        Net.CSSelectAdditionIndexMessage(selectId);
    }

    private void UltimateSelectAdditionIndexMessage(uint id, object data)
    {
        int state = (int) data;
        if (state == 1)
            Close();
    }
    
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}

public class UltimateAttrbute : IDispose
{
    public GameObject _bg;
    private UISprite _spIcon;
    private UILabel _lbName;
    private UILabel _lbValue;
    public GameObject _select;

    private  ThreeTuple _ThreeTuple;

    public int _index;
    
    public void Init(Transform go, ThreeTuple tuple, int index)
    {
        _bg = go.Find("bg").gameObject;
        _spIcon = go.Find("sp_icon").GetComponent<UISprite>();
        _lbName = go.Find("lb_name").GetComponent<UILabel>();
        _lbValue = go.Find("lb_number").GetComponent<UILabel>();
        _select = go.Find("tex_select").gameObject;
        _index = index;
        _ThreeTuple = tuple;
        
        CSEffectPlayMgr.Instance.ShowUITexture(_bg, "extremity_challenge_flag");
        CSEffectPlayMgr.Instance.ShowUITexture(_select, "extremity_challenge_flag2");
        
        RefreshName();
        RefreshIcon();
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

    public void SetSelect(bool select)
    {
        _select.SetActive(select);
    }
    
    public void Dispose()
    {
        CSEffectPlayMgr.Instance.Recycle(_bg);
        CSEffectPlayMgr.Instance.Recycle(_select);
    }
}