using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectItemData : IndexedItem
{
    public int Index { get; set; }
    public bool learned;
    public TABLE.SKILL skillItem;
    public void OnRecycle()
    {
        skillItem = null;
    }
}

public class SkillEffectItem : UIBinder
{
    protected UILabel lb_key;
    protected UILabel lb_value;
    protected SkillEffectItemData mData;

    public override void Init(UIEventListener handle)
    {
        lb_key = Get<UILabel>("lb_key");
        lb_value = Handle.GetComponent<UILabel>();
    }

    public override void Bind(object data)
    {
        mData = data as SkillEffectItemData;
        if(null != mData)
        {
            TABLE.SKILL currentSkillItem = mData.skillItem;
            TABLE.SKILL validSkillItem = null;
            if(this.mData.learned)
            {
                validSkillItem = CSSkillInfo.Instance.GetValidSkillItem(currentSkillItem.id, false);
            }
            bool hasEffect = null != validSkillItem && validSkillItem.level >= currentSkillItem.level;
            if (null != lb_key && null != currentSkillItem)
            {
                lb_key.text = $"lv.{currentSkillItem.level}".BBCode(hasEffect ? ColorType.MainText : ColorType.WeakText);
            }

            if (null != lb_value && null != currentSkillItem)
            {
                if(hasEffect)
                    lb_value.text = $"{currentSkillItem.Speciallv}".BBCode(ColorType.MainText);
                else
                    lb_value.text = NGUIText.StripSymbols(currentSkillItem.Speciallv).BBCode(ColorType.WeakText);
            }
        }
    }

    public override void OnDestroy()
    {
        lb_key = null;
        lb_value = null;
        mData?.OnRecycle();
        mData = null;
    }
}

public partial class UISkillUpgradeEffectPanel : UIBasePanel
{
    public override void Init()
    {
        base.Init();
        mBtnClose.onClick = Close;
    }

    SkillInfoData skillInfoData;
    public void Show(SkillInfoData skillInfoData)
    {
        this.skillInfoData = skillInfoData;
        Panel.alpha = 0.0f;
        ScriptBinder.StartCoroutine(AdjustFrame());
    }

    IEnumerator AdjustFrame()
    {
        if (null != skillInfoData)
        {
            int gid = (int)skillInfoData.item.skillGroup;
            var datas = CSSkillInfo.Instance.GetQualityFlyListByGroupId(gid);
            int count = null == datas ? 0 : datas.Count;
            mGridEffects.Bind(datas, OnItemVisible);
            var bounds = NGUIMath.CalculateRelativeWidgetBounds(mGridEffects.transform);
            mview.height = (int)bounds.size.y + mTail.height + mTitle.height;
            mview.transform.localPosition = new Vector3(mview.transform.localPosition.x, mview.height * 0.5f, mview.transform.localPosition.z);
        }
        yield return null;
        Panel.alpha = 1.0f;
    }

    protected void OnItemVisible(GameObject go, SkillEffectItemData itemData,object p)
    {
        var binder = go.GetOrAddBinder<SkillEffectItem>(mPoolHandleManager);
        binder.Bind(itemData);
    }

    protected override void OnDestroy()
    {
        mGridEffects = null;
        skillInfoData = null;
        base.OnDestroy();
    }
}