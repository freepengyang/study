using UnityEngine;

public class HandBookGroupItemData : IndexedItem
{
    public int Index { get; set; }
    public TABLE.HANDBOOKSUIT bookSuitItem;
    public int owned;
    public int max;
    public bool hasEffect;

    public string effectIcon
    {
        get
        {
            if(null != bookSuitItem)
                return bookSuitItem.effectpic;
            return "cardBg3";
        }
    }

    public void Reset()
    {
        bookSuitItem = null;
        owned = 0;
        max = 0;
        hasEffect = false;
    }
}

public class HandBookGroupItemBinder : UIBinder
{
    UISprite sp_icon;
    UILabel lb_num;
    GameObject go_effect;
    UIEventListener btnIcon;
    public override void Init(UIEventListener handle)
    {
        sp_icon = Get<UISprite>("sp_icon");
        lb_num = Get<UILabel>("lb_num");
        go_effect = handle.transform.Find("go_effect").gameObject;
        btnIcon = sp_icon.GetComponent<UIEventListener>();
        if (null != btnIcon)
            btnIcon.onClick = OnShowGroupTips;
    }

    protected void OnShowGroupTips(GameObject go)
    {
        if(null != bookGroupData)
        {
            var activedSuit = CSHandBookManager.Instance.GetActivedSuit(bookGroupData.bookSuitItem);
            if(null != activedSuit)
            {
                UIManager.Instance.CreatePanel<UIHandBookGroupTipsPanel>(f =>
                {
                    (f as UIHandBookGroupTipsPanel).Show(activedSuit.id);
                });
            }
        }
    }

    HandBookGroupItemData bookGroupData;

    public override void Bind(object data)
    {
        bookGroupData = data as HandBookGroupItemData;
        if (null != bookGroupData)
        {
            int owned = bookGroupData.owned;
            int max = bookGroupData.max;
            if (null != sp_icon && null != bookGroupData.bookSuitItem)
                sp_icon.spriteName = bookGroupData.bookSuitItem.pic;
            if (null != lb_num)
            {
                ColorType colorType = ColorType.MainText;
                if (owned <= 0)
                    colorType = ColorType.Red;
                else if (bookGroupData.hasEffect)
                    colorType = ColorType.Green;
                else
                    colorType = ColorType.MainText;
                lb_num.text = $"{owned}/{max}".BBCode(colorType);
            }
            if(null != sp_icon)
            {
                sp_icon.color = owned <= 0 ? Color.black : Color.white;
            }
            if(null != go_effect)
            {
                CSEffectPlayMgr.Instance.ShowUIEffect(go_effect, "effect_handbook_card_add", 10, true);
                go_effect.CustomActive(bookGroupData.hasEffect);
            }
        }
    }

    public override void OnDestroy()
    {
        if (null != btnIcon)
            btnIcon.onClick = null;
        btnIcon = null;
        bookGroupData = null;
        sp_icon = null;
        lb_num = null;
    }
}
