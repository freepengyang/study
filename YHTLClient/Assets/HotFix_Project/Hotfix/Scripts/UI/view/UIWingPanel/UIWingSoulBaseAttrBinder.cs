using System;
using Google.Protobuf.Collections;

public class UIWingSoulBaseAttrBinder : UIBinder
{
    private UILabel lb_name;
    private UILabel lb_nextName;
    public int maxCount;
    public RepeatedField<CSAttributeInfo.KeyValue> attrItems;

    public override void Init(UIEventListener handle)
    {
        lb_name = Get<UILabel>("lb_attr");
        lb_nextName = Get<UILabel>("lb_nextAttr");
    }

    public override void Bind(object data)
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        lb_name.text =
            $"[cbb694]{attrItems[2 * index].Key}{CSString.Format(999)}[-][00FF0C]{attrItems[2 * index].Value}[-]";
        if (index == maxCount - 1 && attrItems.Count < 2 * index + 2)
        {
            lb_nextName.gameObject.SetActive(false);
        }
        else
        {
            lb_nextName.text =
                $"[cbb694]{attrItems[2 * index + 1].Key}{CSString.Format(999)}[-][00FF0C]{attrItems[2 * index + 1].Value}[-]";
            lb_nextName.gameObject.SetActive(true);
        }
    }

    public override void OnDestroy()
    {
        lb_name = null;
        lb_nextName = null;
        attrItems = null;
    }
}