using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class UIHandBookGroupTipsData
{
    public TABLE.HANDBOOKSUIT suit;
    public int owned;
    public bool hasNext;
}

public class UIHandBookGroupTipsBinder : UIBinder
{
    UILabel lb_name;
    UILabel lb_condition;
    UILabel lb_attr_title;
    UILabel lb_effect_title;
    UISprite sp_line;
    Transform ts_body;
    Transform ts_tail;
    UITable grids_attributes;
    UITable grid_skills;

    public override void Init(UIEventListener handle)
    {
        ts_body = Handle.transform.Find("Body");
        ts_tail = Handle.transform.Find("Tail");
        lb_name = Get<UILabel>("Head/lb_name");
        lb_condition = Get<UILabel>("Head/lb_condition");
        lb_attr_title = Get<UILabel>("Body/lb_attr");
        lb_effect_title = Get<UILabel>("Tail/lb_effect");
        grids_attributes = Get<UITable>("Body/grid_attributes");
        grid_skills = Get<UITable>("Tail/grid_skills");
        sp_line = Get<UISprite>("line");
    }

    protected void OnAttributeVisible(GameObject go,CSAttributeInfo.KeyValue kv, object data)
    {
        if (data is UIHandBookGroupTipsData tipsData)
        {
            bool reached = tipsData.owned >= tipsData.suit.requirenum;
            UILabel lb_attr = go.GetComponent<UILabel>();
            if (null != lb_attr)
            {
                if (reached)
                    lb_attr.text = CSString.Format(694, kv.Key, kv.Value);
                else
                    lb_attr.text = CSString.Format(698, kv.Key, kv.Value);
            }
        }
    }

    protected void OnSkillVisible(GameObject go, CSAttributeInfo.KeyValue kv, object data)
    {
        if (data is UIHandBookGroupTipsData tipsData)
        {
            bool reached = tipsData.owned >= tipsData.suit.requirenum;
            UILabel lb_skill_desc = go.GetComponent<UILabel>();
            if (null != lb_skill_desc)
            {
                if (reached)
                    lb_skill_desc.text = CSString.Format(696, NGUIText.StripSymbols(kv.Value));
                else
                    lb_skill_desc.text = NGUIText.StripSymbols(kv.Value).BBCode(ColorType.WeakText);
            }
        }
    }

    public override void Bind(object data)
    {
        if(data is UIHandBookGroupTipsData tipsData)
        {
            bool reached = tipsData.owned >= tipsData.suit.requirenum;
            if (null != lb_name)
                lb_name.text = CSString.Format(reached ? 690 : 691, tipsData.suit.name, tipsData.suit.Lv);

            var judgeTypeName = CSHandBookManager.Instance.GetSuitConditionName(tipsData.suit.judgeType, 0);
            var judgeValueName = CSHandBookManager.Instance.GetSuitConditionName(tipsData.suit.judgeType, tipsData.suit.judgeValue);

            if(null != lb_condition)
                lb_condition.text = CSString.Format(reached ? 692 : 697, tipsData.suit.requirenum, judgeValueName, judgeTypeName, tipsData.owned, tipsData.suit.requirenum);

            var attributes = CSAttributeInfo.Instance.GetAttributes(PoolHandle, tipsData.suit.parameter, tipsData.suit.factor);
            var skills = CSAttributeInfo.Instance.SplitAttributesToAttrAndSkill(PoolHandle, attributes);
            CSAttributeInfo.Instance.SortByOrder(attributes);

            if (null != lb_attr_title)
                lb_attr_title.text = CSString.Format(693).BBCode(reached ? ColorType.MainText : ColorType.WeakText);

            grids_attributes.Bind(attributes,OnAttributeVisible,tipsData);
            ts_body.CustomActive(attributes.Count > 0);
            PoolHandle.RecycleAttributes(attributes);

            if (null != lb_effect_title)
                lb_effect_title.text = CSString.Format(695).BBCode(reached ? ColorType.MainText : ColorType.WeakText);

            grid_skills.Bind(skills,OnSkillVisible,tipsData);
            ts_tail.CustomActive(skills.Count > 0);

            PoolHandle.RecycleAttributes(skills);

            if (null != sp_line)
                sp_line.CustomActive(tipsData.hasNext);
        }
    }

    public override void OnDestroy()
    {
        sp_line = null;
        lb_name = null;
        lb_condition = null;
        lb_attr_title = null;
        lb_effect_title = null;
        grid_skills = null;
        grids_attributes = null;
    }
}

public partial class UIHandBookGroupTipsPanel : UIBasePanel
{
    UIHandBookGroupTipsBinder current;
    UIHandBookGroupTipsBinder next;
    UIHandBookGroupTipsData currentData = new UIHandBookGroupTipsData();
    UIHandBookGroupTipsData nextData = new UIHandBookGroupTipsData();
    public override void Init()
    {
        base.Init();

        mBG.onClick = this.Close;
        current = mcurrent.gameObject.AddBinder<UIHandBookGroupTipsBinder>(mPoolHandleManager);
        next = mnext.gameObject.AddBinder<UIHandBookGroupTipsBinder>(mPoolHandleManager);
        if (null != mScrollBar)
            EventDelegate.Add(mScrollBar.onChange, InitArrow);
        Panel.alpha = 0.0f;
    }

    protected TABLE.HANDBOOKSUIT suit;
    protected TABLE.HANDBOOKSUIT nextSuit;

    public void Show(int suitId)
    {
        if(!HandBookSuitTableManager.Instance.TryGetValue(suitId, out suit) || null == suit)
        {
            return;
        }
        nextSuit = CSHandBookManager.Instance.NextSuit(suit);

        BindCoroutine(9527, PopTips());
    }

    protected IEnumerator PopTips()
    {
        Panel.alpha = 0.05f;
        InitContents();
        InitArrow();
        InitButtons();
        yield return null;
        Panel.alpha = 1.0f;
    }

    protected void InitArrow()
    {
        mDownArrow.CustomActive(mScrollBar.value < 1.0f && mScrollView.shouldMoveVertically);
        mUpArrow.CustomActive(mScrollBar.value > 0 && mScrollView.shouldMoveVertically);
    }

    protected void InitButtons()
    {
        if(null != mBtnList && null != mBtnTemp)
        {
            mBtnTemp.CustomActive(false);
            mBtnList.Reposition();
        }
    }

    float titleHeight = 124.0f;
    float endPaddingY = 18.0f;
    protected void InitContents()
    {
        float viewHeight = titleHeight;
        if (null != msp_icon)
            msp_icon.spriteName = suit.pic;

        if (null != mlb_name)
            mlb_name.text = CSString.Format(688, suit.name);

        int owned = CSHandBookManager.Instance.GetSetupedSuitCount(suit.judgeType, suit.judgeValue);

        var needShowNextSuitTips = suit.id != nextSuit.id && suit.Actived();
        if (null != mlb_collection_desc)
        {
            int need = suit.requirenum;
            //int owned = CSHandBookManager.Instance.GetSuitCount(suit.judgeType, suit.judgeValue);
            var actived = owned >= need;
            var colorString = UtilityColor.GetColorString(actived ? ColorType.Green : ColorType.Red);
            mlb_collection_desc.text = CSString.Format(689, colorString,owned,need);
        }
        currentData.owned = CSHandBookManager.Instance.GetSetupedSuitCount(suit.judgeType, suit.judgeValue);
        currentData.suit = suit;
        currentData.hasNext = needShowNextSuitTips;
        current.Bind(currentData);
        mcurrent.Reposition();
        var boundsCurrent = NGUIMath.CalculateRelativeWidgetBounds(mcurrent.transform);

        viewHeight += boundsCurrent.size.y;


        if (needShowNextSuitTips)
        {
            nextData.owned = CSHandBookManager.Instance.GetSetupedSuitCount(nextSuit.judgeType, nextSuit.judgeValue);
            nextData.suit = nextSuit;
            nextData.hasNext = false;
            next.Bind(nextData);
            mnext.CustomActive(true);
            mnext.Reposition();
        }
        else
        {
            mnext.CustomActive(false);
        }
        var boundsNext = NGUIMath.CalculateRelativeWidgetBounds(mnext.transform);
        viewHeight += boundsNext.size.y;

        viewHeight += endPaddingY;

        mWidge.height = Mathf.CeilToInt(boundsCurrent.size.y + boundsNext.size.y);
        mgroups.Reposition();

        mView.height = Mathf.CeilToInt(Mathf.Min(640.0f, viewHeight));
        mScrollBar.value = 0.0f;
    }
}