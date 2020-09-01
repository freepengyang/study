public class SingleAttrItem : UIBinder
{
    UISprite sp_bg;
    UILabel lb_name;
    UILabel lb_value;
    public override void Init(UIEventListener handle)
    {
        sp_bg = handle.GetComponent<UISprite>();
        lb_name = Get<UILabel>("lb_name");
        lb_value = Get<UILabel>("lb_value");
    }

    SingleAttrData attrData;

    public override void Bind(object data)
    {
        attrData = data as SingleAttrData;
        if (null != attrData)
        {
            if (null != sp_bg)
                sp_bg.enabled = (attrData.number & 1) == 0;

            if ((AttrType)attrData.kv.AttrType == AttrType.SkillDesc)
            {
                if (null != lb_name)
                    lb_name.text = attrData.kv.Value.BBCode(ColorType.SecondaryText);
                if (null != lb_value)
                    lb_value.text = string.Empty;
            }
            else
            {
                if (null != lb_name)
                    lb_name.text = attrData.kv.Key.BBCode(ColorType.SecondaryText);
                if (null != lb_value)
                    lb_value.text = attrData.kv.Value.BBCode(ColorType.Green);
            }
        }
    }

    public override void OnDestroy()
    {
        attrData?.OnRecycle();
        attrData = null;
        sp_bg = null;
        lb_name = null;
        lb_value = null;
    }
}
